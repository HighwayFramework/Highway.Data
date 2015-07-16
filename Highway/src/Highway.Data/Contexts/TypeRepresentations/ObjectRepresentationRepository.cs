
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Highway.Data.Utilities;
using Highway.Pavement.Collections;


namespace Highway.Data.Contexts.TypeRepresentations
{
    internal sealed class ObjectRepresentationRepository
    {
        internal ConcurrentList<ObjectRepresentation> _data = new ConcurrentList<ObjectRepresentation>();

        public ObjectRepresentationRepository()
        {
            IdentityStrategies = new Dictionary<Type, Action<object>>();
        }

        public Dictionary<Type, Action<object>> IdentityStrategies { get; set; }

        internal IQueryable<T> Data<T>()
        {
            return _data.Where(x => x.IsType<T>()).Select(x => x.Entity).Cast<T>().AsQueryable();
        }

        internal void Add<T>(T item) where T : class
        {
            if (EntityExistsInRepository(item)) return;

            var rep = new ObjectRepresentation
            {
                Entity = item
            };

            _data.Add(rep);
            rep.RelatedEntities = AddRelatedObjects(item);
            UpdateExistingRepresentations(rep);
        }

        private void UpdateExistingRepresentations(ObjectRepresentation rep)
        {
            var type = rep.Entity.GetType();
            var nonPrimitivePropertiesFromObject = type.GetProperties().Where(x => !x.PropertyType.IsPrimitive).ToList();
            var typesCurrentlyStored = rep.RelatedEntities.Select(x => x.Entity.GetType()).ToList();
            List<object> referencedProperties = new List<object>();
            foreach (var info in nonPrimitivePropertiesFromObject)
            {
                if (typesCurrentlyStored.Contains(info.PropertyType.ToSingleType()))
                {
                    if (info.PropertyType.IsEnumerable())
                    {
                        IEnumerable values = (IEnumerable) info.GetValue(rep.Entity, null);
                        if (values != null)
                        {
                            referencedProperties.AddRange(values.Cast<object>());
                        }
                    }
                    else
                    {
                        referencedProperties.Add(info.GetValue(rep.Entity, null));    
                    }
                }
            }

            foreach (var data in rep.RelatedEntities.Where(x => typesCurrentlyStored.Contains(x.Entity.GetType())))
            {
                if (!referencedProperties.Contains(data.Entity))
                {
                    continue;                    
                }
                var collectionType = typeof(ICollection<>).MakeGenericType(type);
                var propertiesThatReferToRepresentation =
                    data.Entity.GetType()
                        .GetProperties()
                        .Where(x => x.CanWrite && (x.PropertyType == type || x.PropertyType.IsAssignableFrom(collectionType)));
                var addMethod = collectionType.GetMethod("Add");
                var propertyInfos = propertiesThatReferToRepresentation.ToList();
                if (!propertyInfos.Any() || propertyInfos.Count() > 1)
                {
                    return;
                }
                var referencingProperty = propertyInfos.Single();
                if (referencingProperty.PropertyType.IsAssignableFrom(collectionType))
                {
                    var collection = referencingProperty.GetValue(data.Entity, null);
                    if (collection == null)
                    {
                        collectionType = typeof(Collection<>).MakeGenericType(type);
                        collection = Activator.CreateInstance(collectionType);
                        referencingProperty.SetValue(data.Entity, collection, null);
                    }
                    addMethod.Invoke(collection, new[] { rep.Entity });
                }
                else
                {
                    referencingProperty.SetValue(data.Entity, rep.Entity, null);
                }
            }
        }

        internal bool Remove<T>(T item) where T : class
        {
            var success = false;
            var representation = _data.Where(x => x.Entity == item).ToList();
            foreach (var rep in representation)
            {
                success = _data.Remove(rep);
                if (!success) throw new InvalidDataException("Object was not removed");
                foreach (var parent in rep.Parents)
                {
                    parent.Value.RemoveAction();
                }
                foreach (var objectRepresentation in rep.AllRelated())
                {
                    if (objectRepresentation.Parents.Count == 1)
                    {
                        success = _data.Remove(objectRepresentation);
                    }
                    else
                    {
                        objectRepresentation.Parents[item].RemoveAction();
                    }
                    if (!success) throw new InvalidDataException("Dependent Object was not removed");
                }
            }
            return success;
        }

        private ObjectRepresentation CreateChildObjectRepresentation(object item, object parent = null, Action removeAction = null,
            Func<object, object, object> getterFunc = null)
        {
            if (EntityExistsInRepository(item))
            {
                var objectRepresentation = _data.SingleOrDefault(x => x.Entity == item);
                if (!objectRepresentation.Parents.ContainsKey(parent))
                {
                    objectRepresentation.Parents.Add(parent, new Accessor(removeAction, getterFunc));
                }
                return objectRepresentation;
            }
            else
            {
                var objectRepresentation = new ObjectRepresentation
                {
                    Entity = item,
                    Parents = new Dictionary<object, Accessor> { { parent, new Accessor(removeAction, getterFunc) } }
                };

                _data.Add(objectRepresentation);
                objectRepresentation.RelatedEntities = AddRelatedObjects(item);

                return objectRepresentation;
            }
        }

        private IEnumerable<ObjectRepresentation> AddRelatedObjects<T>(T item)
        {
            List<ObjectRepresentation> reps = new List<ObjectRepresentation>();
            foreach (var objectRepresentationBase in GetSingularRelationships(item))
            {
                reps.Add(objectRepresentationBase);
            }
            foreach (var objectRepresentationBase in GetMultipleRelationships(item))
            {
                reps.Add(objectRepresentationBase);
            }
            return reps;
        }

        private IEnumerable<ObjectRepresentation> GetSingularRelationships<T>(T item)
        {
            List<ObjectRepresentation> reps = new List<ObjectRepresentation>();
            var properties =
                item.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(x => x.PropertyType.IsClass
                        && !typeof(IEnumerable).IsAssignableFrom(x.PropertyType)
                        && x.GetValue(item, null) != null);
            foreach (var propertyInfo in properties)
            {
                var child = propertyInfo.GetValue(item, null);
                Func<object, object, object> getterFunc = (parent, kid) => propertyInfo.GetValue(parent, null);
                Action removeAction = () => propertyInfo.SetValue(item, null, null);
                ObjectRepresentation childTypeRepresentation = CreateChildObjectRepresentation(child, item, removeAction, getterFunc);
                reps.Add(childTypeRepresentation);
            }
            return reps;
        }

        private IEnumerable<ObjectRepresentation> GetMultipleRelationships<T>(T item)
        {
            List<ObjectRepresentation> reps = new List<ObjectRepresentation>();
            var properties =
                item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(x => x.PropertyType != typeof(string)
                        && typeof(IEnumerable).IsAssignableFrom(x.PropertyType)
                        && x.GetValue(item, null) != null);
            foreach (var propertyInfo in properties)
            {
                var childCollection = (IEnumerable)propertyInfo.GetValue(item, null);
                foreach (var child in childCollection)
                {
                    var removeAction = CreateRemoveFromCollectionAction(propertyInfo, item, child);
                    var getterFunc = CreateGetterFromCollectionFunc(propertyInfo, child);
                    ObjectRepresentation childTypeRepresentation = CreateChildObjectRepresentation(child, item, removeAction, getterFunc);
                    reps.Add(childTypeRepresentation);
                }
            }
            return reps;
        }

        private Func<object, object, object> CreateGetterFromCollectionFunc(PropertyInfo propertyInfo, object childItem)
        {
            return (parent, child) =>
            {
                var value = propertyInfo.GetValue(parent, null);
                if (value == null) return null;
                var collection = (IEnumerable)value;
                return collection.Cast<object>().FirstOrDefault(item => item == child);
            };
        }

        private Action CreateRemoveFromCollectionAction(PropertyInfo propertyInfo, object item, object childItem)
        {
            return () =>
            {
                var items = propertyInfo.GetValue(item, null);
                if (items == null) return;
                var list = CreateGenericList(childItem.GetType());
                MethodInfo mListAdd = list.GetType().GetMethod("Add");
                var childItems = (IEnumerable)items;
                foreach (var itemInList in childItems)
                {
                    if (itemInList != childItem)
                    {
                        mListAdd.Invoke(list, new[] { itemInList });
                    }
                }

                propertyInfo.SetValue(item, list, null);
            };
        }

        private Object CreateGenericList(Type type)
        {
            Type listType = typeof(List<>);
            Type[] typeArgs = { type };
            Type genericType = listType.MakeGenericType(typeArgs);
            object o = Activator.CreateInstance(genericType);
            return o;
        }

        private void CleanGraph()
        {
            var objectRepresentations = _data.Where(x => x.Parents.Count == 0).ToList();
            foreach (var root in objectRepresentations)
            {
                var orphans = root.GetObjectRepresentationsToPrune();
                foreach (var objectRepresentation in orphans)
                {
                    _data.Remove(objectRepresentation);
                }
            }
        }

        private void FindChanges()
        {
            var objectRepresentations = _data.Where(x => x.Parents.Count == 0).ToList();
            foreach (var root in objectRepresentations)
            {
                root.RelatedEntities = AddRelatedObjects(root.Entity);
                foreach (var objRep in root.AllRelated().Where(x => x.Parents.Count == 1 && !_data.Contains(x)))
                {
                    _data.Add(objRep);
                }
            }
        }

        private void ApplyIdentityStrategy<T>(T item) where T : class
        {
            var type = item.GetType();
            var types = new List<Type>(type.GetInterfaces());
            types.Add(type);
            var intersectingType = IdentityStrategies.Keys.Intersect(types).FirstOrDefault();
            if (intersectingType != null)
            {
                IdentityStrategies[intersectingType](item);
            }
        }

        private void ApplyIdentityStrategies()
        {
            foreach (var entity in _data.Select(x => x.Entity))
            {
                ApplyIdentityStrategy(entity);
            }
        }

        internal bool EntityExistsInRepository(object item)
        {
            return _data.Any(x => x.Entity == item);
        }

        internal void Commit()
        {
            CleanGraph();
            FindChanges();
            ApplyIdentityStrategies();
        }
    }
}