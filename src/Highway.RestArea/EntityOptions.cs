using AutoMapper;
using System;
using System.Linq;
using Highway.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;
using System.Collections.Generic;

namespace Highway.RestArea
{
	public abstract class EntityOptions
	{
		public EntityType Model { get; }

		public Type UnitOfWorkType { get; }
		public Type EntityType { get; }
		public Type IdentityType { get; }
		public Type ModelType { get; }
		public Type GetOneType { get; }
		public Type GetAllType { get; }

		public string UrlName { get; }
		public string IdentityRouteValue { get; }

		public List<EntityOptions> Children { get; }

		public EntityOptions Parent { get; internal set; }
		public RestAreaOptions RestArea { get; internal set; }

		protected EntityOptions(
			EntityType model,
			List<EntityOptions> children,
			Type unitOfWorkType, 
			Type entityType, 
			Type identityType, 
			Type modelType,
			Type getOneType,
			Type getAllType,
			string urlEntityName)
		{
			Model = model;
			Children = children;
			UrlName = urlEntityName ?? model.ClrType.Name;
			IdentityRouteValue = UrlName + "Id";
			UnitOfWorkType = unitOfWorkType;
			EntityType = entityType;
			IdentityType = identityType;
			ModelType = modelType;
			GetOneType = getOneType;
			GetAllType = getAllType;
		}

		public abstract void ConfigureMap(IMapperConfigurationExpression cfg);

		internal Type FindIdType(string name)
		{
			if (name == this.IdentityRouteValue) return this.IdentityType;
			if (this.Parent == null) throw new KeyNotFoundException($"Could not find an IdentityRouteValue of {name}");
			else return this.Parent.FindIdType(name);
		}

	}
}
