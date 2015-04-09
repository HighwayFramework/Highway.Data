using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.UI.WebControls;
using Rhino.Mocks.Constraints;

namespace Highway.Data.Tests.Security.ComplexDomain.Mapping
{
    public class BlogSecurityMap : SecurityConfiguration
    {
        public BlogSecurityMap()
        {
            Entity<Blog>().Secure.By(x => x.Path(c => c.Posts).AndPath(c => c.Author)).AndBy(x => x.Self());
            Entity<Post>().Secure.By(x => x.Path(c => c.Author).OrSelf());
            Entity<LiveStreamBlog>().Secure.By(x => x.Base<AudioBlog>());
            Entity<VideoBlog>().Secure.By(x => x.Self()).OrBase<Blog>();
            Entity<AudioBlog>().Secure.By(x => x.Self()).AndBase<Blog>();
        }
    }

    public class SecurityConfiguration
    {
        public EntitySecurityConfiguration<T> Entity<T>()
        {
            return new EntitySecurityConfiguration<T>();
        }
    }

    public class EntitySecurityConfiguration<T>
    {
        private readonly EntitySecurityRoot<T> _fluentApi;

        public EntitySecurityConfiguration()
        {
            _fluentApi = new EntitySecurityRoot<T>(this);
            SecuredPaths = new List<IConfiguredSecureRelationshipPart<T>>();
        }
        public IEntitySecurityRoot<T> Secure
        {
            get { return _fluentApi; }
        }

        public ICollection<IConfiguredSecureRelationshipPart<T>> SecuredPaths { get; set; }
    }

    public class EntitySecurityRoot<T> : IEntitySecurityRoot<T>, ISecureRelationshipBuilder<T>
    {
        private readonly EntitySecurityConfiguration<T> _entitySecurityConfiguration;

        public EntitySecurityRoot(EntitySecurityConfiguration<T> entitySecurityConfiguration)
        {
            _entitySecurityConfiguration = entitySecurityConfiguration;
        }

        IConfiguredSecureRelationship<T> IEntitySecurityRoot<T>.By(Func<ISecureRelationshipBuilder<T>, IConfiguredSecureRelationshipPart<T>> securingExpression)
        {
            var resultingConfiguration = securingExpression(this);
            _entitySecurityConfiguration.SecuredPaths.Add(resultingConfiguration);
            return (IConfiguredSecureRelationship<T>) resultingConfiguration;
        }

        IConfiguredSecureRelationshipPart<T> ISecureRelationshipBuilder<T>.Path<TK>(Expression<Func<T, IEnumerable<TK>>> securablePath)
        {
            return new ConfiguredSecureRelationshipPart<T>(securablePath);
        }

        IConfiguredSecureRelationshipPart<T> ISecureRelationshipBuilder<T>.Path<TK>(Expression<Func<T, TK>> securablePath)
        {
            return new ConfiguredSecureRelationshipPart<T>(securablePath);
        }

        IConfiguredSecureRelationshipPart<T> ISecureRelationshipBuilder<T>.Self()
        {
            Expression<Func<T, T>> securablePath = x => x;
            return new ConfiguredSecureRelationshipPart<T>(securablePath);
        }

        public IConfiguredSecureRelationshipPart<T> Base<TBase>()
        {
            throw new NotImplementedException();
        }
    }

    public interface IEntitySecurityRoot<T>
    {
        IConfiguredSecureRelationship<T> By(Func<ISecureRelationshipBuilder<T>, IConfiguredSecureRelationshipPart<T>> securingExpression);
    }

    public class ConfiguredSecureRelationshipPart<T> : IConfiguredSecureRelationshipPart<T>
    {
        private readonly Expression _securablePath;

        public ConfiguredSecureRelationshipPart(Expression securablePath)
        {
            _securablePath = securablePath;
        }

        public IConfiguredSecureRelationshipPart<T> OrPath<TK>(Expression<Func<T, IEnumerable<TK>>> func)
        {
            return this;
        }

        public IConfiguredSecureRelationshipPart<T> OrPath<TK>(Expression<Func<T, TK>> func)
        {
            return this;
        }

        public IConfiguredSecureRelationshipPart<T> AndPath<TK>(Expression<Func<T, IEnumerable<TK>>> func)
        {
            return this;
        }

        public IConfiguredSecureRelationshipPart<T> AndPath<TK>(Expression<Func<T, TK>> func)
        {
            return this;
        }

        public IConfiguredSecureRelationshipPart<T> OrSelf()
        {
            return this;
        }

        public IConfiguredSecureRelationshipPart<T> AndSelf()
        {
            return this;
        }
    }

    public class ConfiguredSecureRelationship<T> : IConfiguredSecureRelationship<T>
    {
        public ConfiguredSecureRelationship(ConfiguredSecureRelationshipPart<T> configuredSecureRelationshipPart)
        {
            throw new NotImplementedException();
        }

        public void OrBy(Expression<Func<ISecureRelationshipBuilder<T>, IConfiguredSecureRelationshipPart<T>>> securingExpression)
        {
            throw new NotImplementedException();
        }

        public void OrBase<TBase>()
        {
            throw new NotImplementedException();
        }

        public void AndBy(Expression<Func<ISecureRelationshipBuilder<T>, IConfiguredSecureRelationshipPart<T>>> securingExpression)
        {
            throw new NotImplementedException();
        }

        public void AndBase<TBase>()
        {
            throw new NotImplementedException();
        }
    }

    public interface IConfiguredSecureRelationship<T>
    {
        void OrBy(Expression<Func<ISecureRelationshipBuilder<T>, IConfiguredSecureRelationshipPart<T>>> securingExpression);
        void OrBase<TBase>();
        void AndBy(Expression<Func<ISecureRelationshipBuilder<T>, IConfiguredSecureRelationshipPart<T>>> securingExpression);
        void AndBase<TBase>();
    }

    public interface ISecureRelationshipBuilder<T>
    {
        IConfiguredSecureRelationshipPart<T> Path<TK>(Expression<Func<T, IEnumerable<TK>>> func);
        IConfiguredSecureRelationshipPart<T> Path<TK>(Expression<Func<T, TK>> func);
        IConfiguredSecureRelationshipPart<T> Self();
        IConfiguredSecureRelationshipPart<T> Base<TBase>();
    }

    public interface IConfiguredSecureRelationshipPart<T>
    {
        IConfiguredSecureRelationshipPart<T> OrPath<TK>(Expression<Func<T, IEnumerable<TK>>> func);
        IConfiguredSecureRelationshipPart<T> OrPath<TK>(Expression<Func<T, TK>> func);
        IConfiguredSecureRelationshipPart<T> AndPath<TK>(Expression<Func<T, IEnumerable<TK>>> func);
        IConfiguredSecureRelationshipPart<T> AndPath<TK>(Expression<Func<T, TK>> func);
        IConfiguredSecureRelationshipPart<T> OrSelf();
        IConfiguredSecureRelationshipPart<T> AndSelf();
    }
}