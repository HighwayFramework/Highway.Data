using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Highway.Pavement;
using Highway.Pavement.Collections;

namespace Highway.Data.Contexts.TypeRepresentations
{
    internal sealed class ObjectRepresentationRepository
    {
        internal ConcurrentList<ObjectRepresentation> ObjectRepresentations = new ConcurrentList<ObjectRepresentation>();

        public ObjectRepresentationRepository()
        {
            IdentityStrategies = new Dictionary<Type, Action<object>>();
        }

        public Dictionary<Type, Action<object>> IdentityStrategies { get; set; }

        internal void Add<T>(T item)
            where T : class
        {
            if (EntityExistsInRepository(item))
            {
                return;
            }

            var rep = new ObjectRepresentation
            {
                Entity = item
            };

            ObjectRepresentations.Add(rep);
            rep.RelatedEntities = AddRelatedObjects(item);
            UpdateExistingRepresentations(rep);
        }

        internal void Commit()
        {
            CleanGraph();
            FindChanges();
            ApplyIdentityStrategies();
        }

        internal IQueryable<T> Data<T>()
        {
            return ObjectRepresentations.Where(x => x.Entity is T).Select(x => x.Entity).Cast<T>().AsQueryable();
        }

        internal bool EntityExistsInRepository(object item)
        {
            return ObjectRepresentations.Any(x => x.Entity == item);
        }

        internal bool Remove<T>(T item)
            where T : class
        {
            var success = false;
            var representation = ObjectRepresentations.Where(x => x.Entity == item).ToList();
            foreach (var rep in representation)
            {
                success = ObjectRepresentations.Remove(rep);
                if (!success)
                {
                    throw new InvalidDataException("Object was not removed");
                }

                foreach (var parent in rep.Parents)
                {
                    parent.Value.RemoveAction();
                }

                foreach (var objectRepresentation in rep.AllRelated())
                {
                    if (objectRepresentation.Parents.Count == 1)
                    {
                        success = ObjectRepresentations.Remove(objectRepresentation);
                    }
                    else
                    {
                        objectRepresentation.Parents[item].RemoveAction();
                    }

                    if (!success)
                    {
                        throw new InvalidDataException("Dependent Object was not removed");
                    }
                }
            }

            return success;
        }

        private IEnumerable<ObjectRepresentation> AddRelatedObjects<T>(T item)
        {
            var reps = new List<ObjectRepresentation>();
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

        private void ApplyIdentityStrategies()
        {
            foreach (var entity in ObjectRepresentations.Select(x => x.Entity))
            {
                ApplyIdentityStrategy(entity);
            }
        }

        private void ApplyIdentityStrategy<T>(T item)
            where T : class
        {
            var type = item.GetType();
            var types = new List<Type>(type.GetInterfaces())
            {
                type
            };

            var intersectingType = IdentityStrategies.Keys.Intersect(types).FirstOrDefault();
            if (intersectingType != null)
            {
                IdentityStrategies[intersectingType](item);
            }
        }

        private void CleanGraph()
        {
            var objectRepresentations = ObjectRepresentations.Where(x => x.Parents.Count == 0).ToList();
            foreach (var root in objectRepresentations)
            {
                var orphans = root.GetObjectRepresentationsToPrune();
                foreach (var objectRepresentation in orphans)
                {
                    ObjectRepresentations.Remove(objectRepresentation);
                }
            }
        }

        private ObjectRepresentation CreateChildObjectRepresentation(
            object item,
            object parent = null,
            Action removeAction = null,
            Func<object, object, object> getterFunc = null)
        {
            if (EntityExistsInRepository(item))
            {
                var objectRepresentation = ObjectRepresentations.SingleOrDefault(x => x.Entity == item);
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

                ObjectRepresentations.Add(objectRepresentation);
                objectRepresentation.RelatedEntities = AddRelatedObjects(item);

                return objectRepresentation;
            }
        }

        private Object CreateGenericList(Type type)
        {
            var listType = typeof(List<>);
            Type[] typeArgs = { type };
            var genericType = listType.MakeGenericType(typeArgs);

            return Activator.CreateInstance(genericType);
        }

        private Func<object, object, object> CreateGetterFromCollectionFunc(PropertyInfo propertyInfo, object childItem)
        {
            return (parent, child) =>
            {
                var value = propertyInfo.GetValue(parent, null);
                if (value == null)
                {
                    return null;
                }

                var collection = (IEnumerable)value;

                return collection.Cast<object>().FirstOrDefault(item => item == child);
            };
        }

        private Action CreateRemoveFromCollectionAction(PropertyInfo propertyInfo, object item, object childItem)
        {
            return () =>
            {
                var items = propertyInfo.GetValue(item, null);
                if (items == null)
                {
                    return;
                }

                var list = CreateGenericList(childItem.GetType());
                var mListAdd = list.GetType().GetMethod("Add");
                var childItems = (IEnumerable)items;
                foreach (var itemInList in childItems)
                {
                    if (itemInList != childItem)
                    {
                        mListAdd.Invoke(list, new[] { itemInList });
                    }
                }

                try
                {
                    propertyInfo.SetValue(item, list, null);
                }
                catch (ArgumentException ex)
                {
                    if (ex.Message == "Property set method not found.")
                    {
                        throw new ArgumentException($"Entity Type {propertyInfo.Name} could not be removed through {propertyInfo.ReflectedType.Name}.{propertyInfo.Name}" +
                            $" because {propertyInfo.DeclaringType.Name}.{propertyInfo.Name} has no setter.", ex);
                    }
                    throw ex;
                }
            };
        }

        private void FindChanges()
        {
            var objectRepresentations = ObjectRepresentations.Where(x => x.Parents.Count == 0).ToList();
            foreach (var root in objectRepresentations)
            {
                root.RelatedEntities = AddRelatedObjects(root.Entity);
                foreach (var objRep in root.AllRelated().Where(x => x.Parents.Count == 1 && !ObjectRepresentations.Contains(x)))
                {
                    ObjectRepresentations.Add(objRep);
                }
            }
        }

        private IEnumerable<ObjectRepresentation> GetMultipleRelationships<T>(T item)
        {
            var reps = new List<ObjectRepresentation>();
            var properties =
                item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(
                        x => x.PropertyType != typeof(string)
                            && typeof(IEnumerable).IsAssignableFrom(x.PropertyType)
                            && x.GetValue(item, null) != null
                            && x.GetCustomAttribute(typeof(InMemoryIgnoreAttribute)) == null);

            foreach (var propertyInfo in properties)
            {
                var childCollection = (IEnumerable)propertyInfo.GetValue(item, null);
                foreach (var child in childCollection)
                {
                    var removeAction = CreateRemoveFromCollectionAction(propertyInfo, item, child);
                    var getterFunc = CreateGetterFromCollectionFunc(propertyInfo, child);
                    var childTypeRepresentation = CreateChildObjectRepresentation(child, item, removeAction, getterFunc);
                    reps.Add(childTypeRepresentation);
                }
            }

            return reps;
        }

        private IEnumerable<ObjectRepresentation> GetSingularRelationships<T>(T item)
        {
            var reps = new List<ObjectRepresentation>();
            var properties =
                item.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(
                        x => x.PropertyType.IsClass
                            && !typeof(IEnumerable).IsAssignableFrom(x.PropertyType)
                            && x.GetValue(item, null) != null
                            && x.GetCustomAttribute(typeof(InMemoryIgnoreAttribute)) == null);

            foreach (var propertyInfo in properties)
            {
                object Getter(object parent, object kid)
                {
                    return propertyInfo.GetValue(parent, null);
                }

                void Remover()
                {
                    try
                    {
                        propertyInfo.SetValue(item, null, null);
                    }
                    catch (ArgumentException ex)
                    {
                        if (ex.Message == "Property set method not found.")
                        {
                            throw new ArgumentException($"Entity Type {propertyInfo.Name} could not be removed through {propertyInfo.ReflectedType.Name}.{propertyInfo.Name}" +
                            $" because {propertyInfo.DeclaringType.Name}.{propertyInfo.Name} has no setter.", ex);
                        }
                        throw ex;
                    }
                }

                var child = propertyInfo.GetValue(item, null);
                var childTypeRepresentation = CreateChildObjectRepresentation(child, item, Remover, Getter);
                reps.Add(childTypeRepresentation);
            }

            return reps;
        }

        private void UpdateExistingRepresentations(ObjectRepresentation rep)
        {
            var type = rep.Entity.GetType();
            var nonPrimitivePropertiesFromObject = type.GetProperties().Where(x => !x.PropertyType.IsPrimitive).ToList();
            var typesCurrentlyStored = rep.RelatedEntities.Select(x => x.Entity.GetType()).ToList();
            var referencedProperties = new List<object>();
            foreach (var info in nonPrimitivePropertiesFromObject)
            {
                if (typesCurrentlyStored.Contains(info.PropertyType.ToSingleType()))
                {
                    if (info.PropertyType.IsEnumerable())
                    {
                        var values = (IEnumerable)info.GetValue(rep.Entity, null);
                        referencedProperties.AddRange(values.Cast<object>());
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
                        .Where(x => x.PropertyType == type || x.PropertyType.IsAssignableFrom(collectionType));

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
                        var listType = typeof(List<>).MakeGenericType(type);
                        referencingProperty.SetValue(data.Entity, Activator.CreateInstance(listType), null);
                        collection = referencingProperty.GetValue(data.Entity, null);
                    }

                    addMethod.Invoke(collection, new[] { rep.Entity });
                }
                else
                {
                    referencingProperty.SetValue(data.Entity, rep.Entity, null);
                }
            }
        }
    }
}
