using AutoMapper;
using Highway.Data;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Highway.RestArea
{
	public class EntityOptionsBuilder<TEntity, TId, TModel> : BaseOptionsBuilder
		where TId : IEquatable<TId>
		where TEntity : class, IIdentifiable<TId>
	{
		private readonly EntityType entityModel;
		private readonly Type unitOfWorkType;

		private Action<IMappingExpression<TEntity, TModel>> modelTransform;
		private string name;
		private List<EntityOptions> children;
		private Type getOneType;
		private Type getAllType;

		public EntityOptionsBuilder(IModel model, EntityType entityModel, Type unitOfWorkType) : base(model)
		{
			this.entityModel = entityModel;
			this.unitOfWorkType = unitOfWorkType;
			children = new List<EntityOptions>();
			name = null;
		}

		public EntityOptionsBuilder<TEntity, TId, TModel> WithUrlName(string entityName)
		{
			this.name = entityName;
			return this;
		}

		public EntityOptionsBuilder<TEntity, TId, TModel> AddEntityById<TPropEntity, TPropId, TPropModel>(Func<TEntity, IEnumerable<TPropEntity>> propExpression, Action<EntityOptionsBuilder<TPropEntity, TPropId, TPropModel>> builder)
			where TPropId : IEquatable<TPropId>
			where TPropEntity : class, IIdentifiable<TPropId>
		{
			children.Add(CreateEntityOptions(unitOfWorkType, builder));
			return this;
		}

		public EntityOptionsBuilder<TEntity, TId, TModel> WithGetOne<TScalar>()
			where TScalar : IScalar<TEntity> 
		{
			getOneType = typeof(TScalar);
			return this;
		}

		public EntityOptionsBuilder<TEntity, TId, TModel> WithGetAll<TQuery>()
			where TQuery : IQuery<TEntity>
		{
			getAllType = typeof(TQuery);
			return this;
		}

		public EntityOptionsBuilder<TEntity, TId, TModel> WithModelTransform(Action<IMappingExpression<TEntity, TModel>> modelTransform)
		{
			this.modelTransform = modelTransform;
			return this;
		}

		public EntityOptions<TEntity, TId, TModel> Build()
		{
			var opt = new EntityOptions<TEntity, TId, TModel>(entityModel, children, modelTransform, unitOfWorkType, getOneType, getAllType, name);
			opt.Children.ForEach(e => e.Parent = opt as EntityOptions);
			return opt;
		}
	}
}
