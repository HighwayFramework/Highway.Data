using Highway.Data;
using System;
using System.Linq;
namespace Highway.RestArea
{
	public class EntityOptionsBuilder<TEntity, TId>
		where TId : IEquatable<TId>
		where TEntity : class, IIdentifiable<TId>
	{
		private readonly Microsoft.EntityFrameworkCore.Metadata.Internal.EntityType model;
		private readonly Type unitOfWorkType;
		private string name;

		public EntityOptionsBuilder(Microsoft.EntityFrameworkCore.Metadata.Internal.EntityType model, Type unitOfWorkType)
		{
			this.model = model;
			this.unitOfWorkType = unitOfWorkType;
		}

		public EntityOptionsBuilder<TEntity, TId> WithUrlName(string entityName)
		{
			this.name = entityName;
			return this;
		}

		public EntityOptions<TEntity, TId> Build()
		{
			return new EntityOptions<TEntity, TId>(model, unitOfWorkType, name);
		}
	}
}
