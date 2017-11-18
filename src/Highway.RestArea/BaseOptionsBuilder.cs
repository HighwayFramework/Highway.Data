using Highway.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Linq;
namespace Highway.RestArea
{
	public abstract class BaseOptionsBuilder
	{
		protected readonly IModel model;

		public BaseOptionsBuilder(IModel model)
		{
			this.model = model;
		}

		protected EntityOptions<TEntity, TId, TModel> CreateEntityOptions<TEntity, TId, TModel>(
			Type unitOfWorkType,
			Action<EntityOptionsBuilder<TEntity, TId, TModel>> configure
		)
			where TEntity : class, IIdentifiable<TId>
			where TId : IEquatable<TId>
		{
			var entityModel = model.GetEntityTypes(typeof(TEntity)).FirstOrDefault();
			if (entityModel == null)
				throw new ArgumentException($"Unable to locate {typeof(TEntity).FullName} as an Entity on {unitOfWorkType.FullName}");
			else
			{
				var eConfigBuilder = new EntityOptionsBuilder<TEntity, TId, TModel>(model, entityModel, unitOfWorkType);
				configure(eConfigBuilder);
				return eConfigBuilder.Build();
			}
		}

	}
}
