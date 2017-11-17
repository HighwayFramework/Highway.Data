using AutoMapper;
using Highway.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Linq;

namespace Highway.RestArea
{
	public class EntityOptionsBuilder<TEntity, TId, TModel>
		where TId : IEquatable<TId>
		where TEntity : class, IIdentifiable<TId>
	{
		private readonly EntityType model;
		private readonly Type unitOfWorkType;
		private Action<IMappingExpression<TEntity, TModel>> modelTransform;
		private string name;

		public EntityOptionsBuilder(EntityType model, Type unitOfWorkType)
		{
			this.model = model;
			this.unitOfWorkType = unitOfWorkType;
		}

		public EntityOptionsBuilder<TEntity, TId, TModel> WithUrlName(string entityName)
		{
			this.name = entityName;
			return this;
		}

		public EntityOptionsBuilder<TEntity, TId, TModel> WithModelTransform(Action<IMappingExpression<TEntity, TModel>> modelTransform)
		{
			this.modelTransform = modelTransform;
			return this;
		}
		public EntityOptions<TEntity, TId, TModel> Build()
		{
			return new EntityOptions<TEntity, TId, TModel>(model, modelTransform, unitOfWorkType, name);
		}
	}
}
