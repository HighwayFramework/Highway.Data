using AutoMapper;
using Highway.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Highway.RestArea
{
	public class RestAreaOptionsBuilder<TContext>
		where TContext : UnitOfWork
	{
		private readonly IModel model;
		private readonly List<EntityOptions> entityOptions = new List<EntityOptions>();
		private readonly Dictionary<Type, Func<string, object>> converters = new Dictionary<Type, Func<string, object>>();
		private Action<IMapperConfigurationExpression> mapperConfig = null;

		public RestAreaOptionsBuilder(IModel model)
		{
			this.model = model;
		}

		public RestAreaOptionsBuilder<TContext> AddFromContext<TEntity, TId, TModel>(Action<EntityOptionsBuilder<TEntity, TId, TModel>> configure)
			where TId : IEquatable<TId>
			where TEntity : class, IIdentifiable<TId>
		{
			var entityModel = model.GetEntityTypes(typeof(TEntity)).FirstOrDefault();
			if (entityModel == null) throw new ArgumentException($"Unable to locate {typeof(TEntity).FullName} as an Entity on {typeof(TContext).FullName}");
			else
			{
				var eConfigBuilder = new EntityOptionsBuilder<TEntity, TId, TModel>(entityModel, typeof(TContext));
				configure(eConfigBuilder);
				entityOptions.Add(eConfigBuilder.Build());
			}
			return this;
		}
		public RestAreaOptionsBuilder<TContext> AddTransform(Action<IMapperConfigurationExpression> cfgBuilder)
		{
			mapperConfig = cfgBuilder;
			return this;
		}
		public RestAreaOptions<TContext> Build()
		{
			return new RestAreaOptions<TContext>(entityOptions, converters, mapperConfig);
		}

		public void ConvertTo<TId>(Func<string, TId> converter)
		{
			converters.Add(typeof(TId), s => converter(s));
		}
	}
}
