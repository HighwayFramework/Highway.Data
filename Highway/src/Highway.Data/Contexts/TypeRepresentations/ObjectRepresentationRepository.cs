using System;
using System.Collections;
using System.Collections.Generic;
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
                    parent.Value();
                }
                foreach (var objectRepresentation in rep.AllRelated())
                {
                    if (objectRepresentation.Parents.Count == 1)
                    {
                        success = _data.Remove(objectRepresentation);
                    }
                    else
                    {
                        objectRepresentation.Parents[item]();
                    }
                    if (!success) throw new InvalidDataException("Dependent Object was not removed");
                }
            }
            return success;
        }

        private ObjectRepresentation AddChild(object parent, object item, Action removeAction)
        {
            var existing = _data.SingleOrDefault(x => x.Entity == item);
            if(existing == null)
            {
                return new ObjectRepresentation()
                {
                    Id = Guid.NewGuid(),
                    Entity = item,
                    RelatedEntities = AddRelatedObjects(item),
                    Parents = new Dictionary<object,Action> { {parent,removeAction}}
                };    
            }
            existing.Parents.Add(parent,removeAction);
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
                Action removeAction = () => info.SetValue(item, null, null);
                ObjectRepresentation childTypedRepresetation = AddChild(item, child, removeAction);
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
                var genericArguments = propertyInfo.PropertyType.GetGenericArguments();
                if (genericArguments.Count() != 1) continue;
                var listType = genericArguments[0];
                var child = propertyInfo.GetValue(item, null);
                if (child == null) continue;
                var childItems = (IEnumerable)child;
                foreach (var childItem in childItems)
                {
                    var removeAction = CreateRemoveFromCollectionAction(propertyInfo, item, childItem);
                    ObjectRepresentation childTypedRepresetation = AddChild(item, childItem,removeAction);
                    if(childTypedRepresetation != null) reps.Add(childTypedRepresetation);
                }
            }
            return reps;
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

    }
}