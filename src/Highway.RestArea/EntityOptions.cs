using System;
using System.Linq;
using Highway.Data;
using System.Reflection;

namespace Highway.RestArea
{
	public abstract class EntityOptions
	{
		public Microsoft.EntityFrameworkCore.Metadata.Internal.EntityType Model { get; }
		public string UrlName { get; }
		public Type UnitOfWorkType { get; }
		public Type EntityType { get; }
		public Type IdentityType { get; }
		public string IdentityRouteValue { get; }

		protected EntityOptions(Microsoft.EntityFrameworkCore.Metadata.Internal.EntityType model, Type unitOfWorkType, Type entityType, Type identityType, string urlEntityName = null)
		{
			Model = model;
			UrlName = urlEntityName ?? model.ClrType.Name;
			IdentityRouteValue = UrlName + "Id";
			UnitOfWorkType = unitOfWorkType;
			EntityType = entityType;
			IdentityType = identityType;
		}
	}
}
