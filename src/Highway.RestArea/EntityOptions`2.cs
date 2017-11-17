using AutoMapper;
using Highway.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Highway.RestArea
{
	public class EntityOptions<TEntity, TId, TModel> : EntityOptions
	where TId : IEquatable<TId>
	where TEntity : class, IIdentifiable<TId>
	{
		public Func<TId, IScalar<TEntity>> GetByIdFactory { get; }
		public Func<IQuery<TEntity>> GetAllFactory { get; }
		public Action<IMappingExpression<TEntity, TModel>> ModelTransform { get; }

		public EntityOptions(
			EntityType model, 
			Action<IMappingExpression<TEntity, TModel>> modelTransform, 
			Type unitOfWorkType, 
			string urlEntityName = null
		) : base(
				model, unitOfWorkType, 
				typeof(TEntity), typeof(TId), typeof(TModel), urlEntityName
			)
		{
			Action<IMappingExpression<TEntity, TModel>> defaultTransform = map => { };
			ModelTransform = modelTransform ?? defaultTransform;
			GetByIdFactory = id => new Highway.Data.Queries.GetById<TId, TEntity>(id);
			GetAllFactory = () => new Highway.Data.Queries.FindAll<TEntity>();
		}

		public override void ConfigureMap(IMapperConfigurationExpression cfg)
		{
			ModelTransform(cfg.CreateMap<TEntity, TModel>());
		}
	}
}
