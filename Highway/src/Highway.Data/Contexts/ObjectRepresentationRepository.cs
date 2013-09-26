using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Highway.Data.Contexts
{
    internal sealed class ObjectRepresentationRepository
    {
        internal List<ObjectRepresentation> _data = new List<ObjectRepresentation>();

        internal IQueryable<T> Data<T>()
        {
            return _data.Where(x => x.IsType<T>()).Select(x => x.Entity).Cast<T>().AsQueryable();
        }

        internal void Add<T>(T item)
        {
            var rep = new ObjectRepresentation()
            {
                Id = Guid.NewGuid(),
                Entity = item,
                RelatedEntities = AddRelatedObjects(item)
            };
            _data.Add(rep);
        }

        internal ObjectRepresentation CreateChild<T>(T item, Action removeAction)
        {
            return new ObjectRepresentation()
            {
                Id = Guid.NewGuid(),
                Entity = item,
                RelatedEntities = AddRelatedObjects(item),
                EntityRemove = removeAction
            };
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

        private IEnumerable<ObjectRepresentation> GetMultipleRelationships<T>(T item)
        {
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
                    yield return CreateChildTypedRepresetation(childItem, removeAction);
                }
            }
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

        private IEnumerable<ObjectRepresentation> GetSingularRelationships<T>(T item)
        {
            var properties =
                item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.PropertyType.IsClass);
            foreach (var propertyInfo in properties)
            {
                var child = propertyInfo.GetValue(item, null);
                if (child == null) continue;
                PropertyInfo info = propertyInfo;
                Action removeAction = () => info.SetValue(item, null, null);
                yield return CreateChildTypedRepresetation(child, removeAction);
            }
        }

        private ObjectRepresentation CreateChildTypedRepresetation(object child, Action removeAction)
        {
            Type type = typeof(ObjectRepresentation);
            var method = type.GetMethod("CreateChild", BindingFlags.NonPublic | BindingFlags.Instance).GetGenericMethodDefinition();
            MethodInfo generic = method.MakeGenericMethod(child.GetType());
            var obj = generic.Invoke(null, new[] { child, removeAction });
            return (ObjectRepresentation)obj;
        }


        public bool Remove(ObjectRepresentation typeObjectRepresentation)
        {
            return _data.Remove(typeObjectRepresentation);
        }
    }
}