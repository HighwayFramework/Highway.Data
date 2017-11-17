using Highway.Data;
using System;
using System.Linq;

namespace Highway.RestArea
{
	public class EntityOptions<TEntity, TId> : EntityOptions
	where TId : IEquatable<TId>
	where TEntity : class, IIdentifiable<TId>
	{
		public Func<TId, IScalar<TEntity>> GetByIdFactory { get; }
		public Func<IQuery<TEntity>> GetAllFactory { get; }

		public EntityOptions(Microsoft.EntityFrameworkCore.Metadata.Internal.EntityType model, Type unitOfWorkType, string urlEntityName = null) : base(model, unitOfWorkType, typeof(TEntity), typeof(TId), urlEntityName)
		{
			GetByIdFactory = id => new Highway.Data.Queries.GetById<TId, TEntity>(id);
			GetAllFactory = () => new Highway.Data.Queries.FindAll<TEntity>();
		}
	}
}
