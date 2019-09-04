using System.Linq;
using Highway.Data.Contexts.TypeRepresentations;
using System;
using System.Collections;
using System.Threading.Tasks;
using Highway.Data.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Highway.Data.Contexts
{
    public class InMemoryActiveDataContext : InMemoryDataContext, IDataContext
    {
        private int commitVersion = 0;
        private static int CommitCounter = 0;
        internal static ObjectRepresentationRepository Repo = new ObjectRepresentationRepository();
        private BiDictionary<object, object> entityToRepoEntityMap = new BiDictionary<object, object>();

        public InMemoryActiveDataContext()
            : base(Repo)
        {

        }

        public static void DropRepository()
        {
            Repo = new ObjectRepresentationRepository();
            CommitCounter = 0;
        }

        public override T Add<T>(T item)
        {
            var repoItem = item.Clone(entityToRepoEntityMap);
            base.Add(repoItem);
            return item;
        }

        public override T Remove<T>(T item)
        {
            var repoItem = entityToRepoEntityMap[item];
            base.Remove(repoItem);
            return item;
        }


        public override IQueryable<T> AsQueryable<T>()
        {
            UpdateMapFromRepo();

            return base.AsQueryable<T>().Select(t => (T)entityToRepoEntityMap.Reverse[t]);
        }

        public override T Update<T>(T item)
        {
            throw new NotSupportedException();
        }

        public override T Reload<T>(T item)
        {
            throw new NotSupportedException();
        }

        public override int Commit()
        {
            if (commitVersion != CommitCounter)
                throw new InvalidOperationException("Cannot commit on stale data. Possibly need to requery. Unexpected scenario.");

            foreach (var pair in entityToRepoEntityMap)
            {
                if (!typeof(IEnumerable).IsAssignableFrom(pair.Key.GetType()))
                    CopyPrimitives(pair.Key, pair.Value);
            }

            ProcessCommitQueues();

            UpdateForwardEntityToRepoEntityMap();

            repo.Commit();

            foreach (var pair in entityToRepoEntityMap.Reverse)
            {
                if (!typeof(IEnumerable).IsAssignableFrom(pair.Key.GetType()))
                    CopyPrimitives(pair.Key, pair.Value);
            }

            commitVersion = ++CommitCounter;

            return 0;
        }

        public override Task<int> CommitAsync()
        {
            var commitAsync = new Task<int>(Commit);
            commitAsync.Start();
            return commitAsync;
        }

        private void UpdateForwardEntityToRepoEntityMap()
        {
            var entities = new List<object>(entityToRepoEntityMap.Keys);
            foreach (var entity in entities)
            {
                CloneCollectionsUpdate(entity);
            }
        }

        private void UpdateMapFromRepo()
        {
            if (commitVersion == CommitCounter) return;

            foreach (var item in Repo._data.Select(o => o.Entity))
            {
                item.Clone(entityToRepoEntityMap.Reverse);
            }

            commitVersion = CommitCounter;
        }

        private void CopyPrimitives(object source, object destination)
        {
            var type = source.GetType();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                var fieldInfo = type.GetField(field.Name, BindingFlags.Public
                    | BindingFlags.Instance
                    | BindingFlags.NonPublic);
                var value = fieldInfo.GetValue(source);

                if (value == null) continue;

                if (fieldInfo.FieldType.IsPrimitive
                    || fieldInfo.FieldType == typeof(string)
                    || fieldInfo.FieldType == typeof(Guid))
                    fieldInfo.SetValue(destination, value);
            }
        }

        private void CloneCollectionsUpdate<T>(T entityCollection) where T : class
        {
            var type = entityCollection.GetType();
            if (!typeof(IEnumerable).IsAssignableFrom(type)) return;

            var collectionType = type.GetGenericTypeDefinition();
            Type genericType = collectionType.MakeGenericType(type.GetGenericArguments());

            if (!typeof(IList).IsAssignableFrom(collectionType))
                throw new NotSupportedException("Uncertain of what other collection types to handle.");

            var repoEntityCollection = (IList)entityToRepoEntityMap[entityCollection];

            var unremovedRepoEntities = new List<object>();
            foreach (var item in (IEnumerable)entityCollection)
            {
                if (!entityToRepoEntityMap.ContainsKey(item))
                    repoEntityCollection.Add(item.Clone(entityToRepoEntityMap));
                unremovedRepoEntities.Add(entityToRepoEntityMap[item]);
            }

            var removeRepoEntities = new List<object>(((IEnumerable<object>)repoEntityCollection).Except(unremovedRepoEntities));

            foreach (var item in removeRepoEntities)
            {
                repoEntityCollection.Remove(item);
            }
        }
    }
}