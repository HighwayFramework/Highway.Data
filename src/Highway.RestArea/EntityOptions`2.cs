using AutoMapper;
using Highway.Data;
using Microsoft.AspNetCore.Routing;
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
		public Func<RouteData, IScalar<TEntity>> GetByIdFactory { get; }
		public Func<RouteData, IQuery<TEntity>> GetAllFactory { get; }
		public Action<IMappingExpression<TEntity, TModel>> ModelTransform { get; }

		public EntityOptions(
			EntityType model, 
			List<EntityOptions> children,
			Action<IMappingExpression<TEntity, TModel>> modelTransform, 
			Type unitOfWorkType, 
			Type getOneType,
			Type getAllType,
			string urlEntityName
		) : base(
				model, children, unitOfWorkType, 
				typeof(TEntity), typeof(TId), typeof(TModel), getOneType, getAllType, urlEntityName
			)
		{
			Action<IMappingExpression<TEntity, TModel>> defaultTransform = map => { };
			ModelTransform = modelTransform ?? defaultTransform;
			GetByIdFactory = rd => CreateScalarFromRouteData(rd, getOneType);
			GetAllFactory = rd => CreateQueryFromRouteData(rd, getAllType);
		}

		private IQuery<TEntity> CreateQueryFromRouteData(RouteData rd, Type type)
		{
			return CreateFromRouteData<IQuery<TEntity>>(rd, type);
		}
		private IScalar<TEntity> CreateScalarFromRouteData(RouteData rd, Type type)
		{
			return CreateFromRouteData<IScalar<TEntity>>(rd, type);
		}
		private TClass CreateFromRouteData<TClass>(RouteData rd, Type type)
			where TClass : class
		{
			var exclude = new string[] { "type", "method" };
			var keys = rd.Values.Keys.Where(k => exclude.Contains(k) == false);
			var ctorInfo = type.GetConstructors()
				.Select(c => new
				{
					ctor = c,
					parms = c.GetParameters()
				})
				.Where(c => c.parms.Count() == keys.Count())
				.Where(c => c.parms.All(p => keys.Contains(p.Name)))
				.SingleOrDefault();
			var parms = new List<object>();
			foreach (var parameterInfo in ctorInfo.parms)
			{
				Type idType = FindIdType(parameterInfo.Name);
				parms.Add(RestArea.ConvertTo(idType, rd.Values[parameterInfo.Name].ToString()));
			}
			return ctorInfo.ctor.Invoke(parms.ToArray()) as TClass;
		}

		public override void ConfigureMap(IMapperConfigurationExpression cfg)
		{
			ModelTransform(cfg.CreateMap<TEntity, TModel>());
		}
	}
}
