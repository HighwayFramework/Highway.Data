using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Highway.Data.Contexts
{
    internal class ObjectRepresentation
    {
        internal object Entity { get; set; }

        public bool IsType<T1>()
        {
            return typeof(T1) == Entity.GetType();
        }

        protected virtual Type Type { get { return typeof(object); } }
        internal IEnumerable<ObjectRepresentation> RelatedEntities { get; set; }
        public Action EntityRemove { get; set; }
        public object Parent { get; set; }

        internal IEnumerable<ObjectRepresentation> AllRelated()
        {
            var items = RelatedEntities.ToList();
            foreach (var objectRepresentationBase in RelatedEntities)
            {
                items.AddRange(objectRepresentationBase.AllRelated());
            }
            return items;
        }

        internal static ObjectRepresentation Create<T>(T item)
        {
            return new ObjectRepresentation()
            {
                Id = Guid.NewGuid(),
                Entity = item,
                RelatedEntities = AddRelatedObjects(item),
            };
        }

        public Guid Id { get; set; }

        internal static ObjectRepresentation CreateChild<T>(T item, Action removeAction)
        {
            return new ObjectRepresentation()
            {
                Id = Guid.NewGuid(),
                Entity = item,
                RelatedEntities = AddRelatedObjects(item),
                EntityRemove = removeAction
            };
        }

        private static IEnumerable<ObjectRepresentation> AddRelatedObjects<T>(T item)
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

        private static IEnumerable<ObjectRepresentation> GetMultipleRelationships<T>(T item)
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
                if(child == null) continue;
                var childItems = (IEnumerable)child;
                foreach (var childItem in childItems)
                {
                    var removeAction = CreateRemoveFromCollectionAction(propertyInfo, item, childItem);
                    yield return CreateChildTypedRepresetation(childItem,removeAction);
                }
            }
        }

        private static Action CreateRemoveFromCollectionAction(PropertyInfo propertyInfo, object item, object childItem)
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
                        mListAdd.Invoke(list, new[] {itemInList});
                    }
                }

                propertyInfo.SetValue(item,list,null);

            };
        }

        private static Object CreateGenericList(Type type)
        {
            Type listType = typeof(List<>);
            Type[] typeArgs = {type}; 
            Type genericType = listType.MakeGenericType(typeArgs); 
            object o = Activator.CreateInstance(genericType);
            return o;
        }

        private static IEnumerable<ObjectRepresentation> GetSingularRelationships<T>(T item)
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

        private static ObjectRepresentation CreateChildTypedRepresetation(object child, Action removeAction)
        {
            Type type = typeof(ObjectRepresentation);
            var method = type.GetMethod("CreateChild", BindingFlags.NonPublic | BindingFlags.Static).GetGenericMethodDefinition();
            MethodInfo generic = method.MakeGenericMethod(child.GetType());
            var obj = generic.Invoke(null, new[] { child, removeAction});
            return (ObjectRepresentation)obj;
        }
    }
}