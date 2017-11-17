using AutoMapper;
using System;
using System.Linq;
using Highway.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;

namespace Highway.RestArea
{
	public abstract class EntityOptions
	{
		public EntityType Model { get; }
		public string UrlName { get; }
		public Type UnitOfWorkType { get; }
		public Type EntityType { get; }
		public Type IdentityType { get; }
		public Type ModelType { get; }
		public string IdentityRouteValue { get; }

		protected EntityOptions(
			EntityType model, 
			Type unitOfWorkType, 
			Type entityType, 
			Type identityType, 
			Type modelType,
			string urlEntityName = null)
		{
			Model = model;
			UrlName = urlEntityName ?? model.ClrType.Name;
			IdentityRouteValue = UrlName + "Id";
			UnitOfWorkType = unitOfWorkType;
			EntityType = entityType;
			IdentityType = identityType;
			ModelType = modelType;
		}
		public abstract void ConfigureMap(IMapperConfigurationExpression cfg);
	}
}
