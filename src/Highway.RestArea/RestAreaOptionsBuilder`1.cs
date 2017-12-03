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
	public class RestAreaOptionsBuilder<TUnitOfWork> : BaseOptionsBuilder
		where TUnitOfWork : UnitOfWork
	{
		private readonly List<EntityOptions> entityOptions = new List<EntityOptions>();
		private Action<IMapperConfigurationExpression> mapperConfig = null;
		private string urlPrefix;

		public RestAreaOptionsBuilder(IModel model) : base(model)
		{
		}

		public RestAreaOptionsBuilder<TUnitOfWork> SetUrlPrefix(string urlPrefix)
		{
			this.urlPrefix = urlPrefix;
			return this;
		}

		public RestAreaOptionsBuilder<TUnitOfWork> AddEntityById<TEntity, TId, TModel>(Action<EntityOptionsBuilder<TEntity, TId, TModel>> configure)
			where TId : IEquatable<TId>
			where TEntity : class, IIdentifiable<TId>
		{
			entityOptions.Add(CreateEntityOptions(typeof(TUnitOfWork), configure));
			return this;
		}

		public RestAreaOptionsBuilder<TUnitOfWork> AddTransform(Action<IMapperConfigurationExpression> cfgBuilder)
		{
			mapperConfig = cfgBuilder;
			return this;
		}
		public RestAreaOptions<TUnitOfWork> Build()
		{
			var opts = new RestAreaOptions<TUnitOfWork>(model, urlPrefix, entityOptions, mapperConfig);
			SetRestArea(opts.Entities, opts);
			return opts;
		}
		private void SetRestArea(IEnumerable<EntityOptions> entities, RestAreaOptions opts)
		{
			foreach (var entity in entities)
			{
				entity.RestArea = opts;
				SetRestArea(entity.Children, opts);
			}
		}
	}
}
