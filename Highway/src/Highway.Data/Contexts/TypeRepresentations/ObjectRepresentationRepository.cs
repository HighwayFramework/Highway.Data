using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Highway.Data.Contexts.TypeRepresentations
{
    internal sealed class ObjectRepresentationRepository
    {
        internal List<ObjectRepresentation> _data = new List<ObjectRepresentation>();

        internal IQueryable<T> Data<T>()
        {
            return _data.Where(x => x.IsType<T>()).Select(x => x.Entity).Cast<T>().AsQueryable();
        }

        internal void Add<T>(T item) where T : class
        {
            var existing = _data.SingleOrDefault(x => x.Entity == item);
            if (existing == null)
            {
                var rep = new ObjectRepresentation()
                {
                    Id = Guid.NewGuid(),
                    Entity = item,
                    RelatedEntities = AddRelatedObjects(item)
                };
                _data.Add(rep);
                _data.AddRange(rep.AllRelated().Where(x=>x.Parents.Count == 1));
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

        private ObjectRepresentation AddChild(object item, object parent = null, Action removeAction = null, Func<object,object,object> getterFunc = null)
        {
            var existing = _data.SingleOrDefault(x => x.Entity == item);
            if(existing == null)
            {
                return new ObjectRepresentation()
                {
                    Id = Guid.NewGuid(),
                    Entity = item,
                    RelatedEntities = AddRelatedObjects(item),
                    Parents = new Dictionary<object, Accessor> { { parent, new Accessor(removeAction, getterFunc) } }
                };    
            }
            existing.Parents.Add(parent, new Accessor(removeAction,getterFunc));
            return existing;
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
                item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.PropertyType.IsClass);
            foreach (var propertyInfo in properties)
            {
                var child = propertyInfo.GetValue(item, null);
                if (child == null) continue;
                PropertyInfo info = propertyInfo;
                Func<object, object, object> getterFunc = (parent,kid) => propertyInfo.GetValue(parent, null);
                Action removeAction = () => info.SetValue(item, null, null);
                ObjectRepresentation childTypedRepresetation = AddChild(child, item, removeAction, getterFunc);
                if(childTypedRepresetation != null) reps.Add(childTypedRepresetation);
            }
            return reps;
        }

        private IEnumerable<ObjectRepresentation> GetMultipleRelationships<T>(T item)
        {
            List<ObjectRepresentation> reps = new List<ObjectRepresentation>();
            var properties =
                item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(x => typeof(IEnumerable).IsAssignableFrom(x.PropertyType));
            foreach (var propertyInfo in properties)
            {
                var child = propertyInfo.GetValue(item, null);
                if (child == null) continue;
                var childItems = (IEnumerable)child;
                foreach (var childItem in childItems)
                {
                    var removeAction = CreateRemoveFromCollectionAction(propertyInfo, item, childItem);
                    var getterFunc = CreateGetterFromCollectionFunc(propertyInfo, childItem);
                    ObjectRepresentation childTypedRepresetation = AddChild(childItem, item, removeAction, getterFunc);
                    if(childTypedRepresetation != null) reps.Add(childTypedRepresetation);
                }
            }
            return reps;
        }

        private Func<object,object,object> CreateGetterFromCollectionFunc(PropertyInfo propertyInfo, object childItem)
        {
            return (parent, child) =>
            {
                var value = propertyInfo.GetValue(parent, null);
                if (value == null) return null;
                var collection = (IEnumerable) value;
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

        public void CleanGraph()
        {
            var objectRepresentations = _data.Where(x=>x.Parents.Count == 0).ToList();
            foreach (var root in objectRepresentations)
            {
                var orphans = root.GetObjectRepresentationsToPrune();
                foreach (var objectRepresentation in orphans)
                {
                    _data.Remove(objectRepresentation);
                }

            }
        }
    }
}