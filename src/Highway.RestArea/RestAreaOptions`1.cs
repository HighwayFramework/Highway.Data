using Highway.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Highway.Pavement;
using Newtonsoft.Json;
using AutoMapper;

namespace Highway.RestArea
{
	public class RestAreaOptions<TContext>
		where TContext : UnitOfWork
	{
		public IEnumerable<EntityOptions> Entities { get; }
		public JsonSerializer Serializer { get; }
		private readonly Dictionary<Type, Func<string, object>> converters;
		private readonly Action<IMapperConfigurationExpression> mapperConfig;
		public RequestDelegate Handler { get; }

		public RestAreaOptions(IEnumerable<EntityOptions> entityOptions, Dictionary<Type, Func<string, object>> converters, Action<IMapperConfigurationExpression> mapperConfig)
		{
			Action<IMapperConfigurationExpression> defaultMapperConfig = cfg => { };
			this.mapperConfig = mapperConfig ?? defaultMapperConfig;
			this.converters = converters;
			Serializer = new JsonSerializer();
			Entities = entityOptions;
			Handler = CreateHandler();
		}

		private RequestDelegate CreateHandler()
		{
			return async (c) =>
			{
				var lFactory = c.RequestServices.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
				IRepository repo = c.RequestServices.GetService(typeof(IRepository)) as IRepository;
				var logger = lFactory.CreateLogger("Highway.RestArea");
				var routeData = c.GetRouteData();
				var entityOption = routeData.Values["type"] as EntityOptions;

				var handlerType = typeof(Handler<,,,>).MakeGenericType(entityOption.UnitOfWorkType, entityOption.EntityType, entityOption.IdentityType, entityOption.ModelType);
				var handler = Activator.CreateInstance(handlerType, repo, this, entityOption, logger);

				await handlerType.GetMethod(routeData.Values["method"] as string)
					.InvokeAsync(handler, c, routeData);
			};
		}

		private IMapper mapper = null;
		public IMapper GetMapper()
		{
			if (mapper != null) return mapper;
			var config = new MapperConfiguration(cfg =>
			{
				mapperConfig(cfg);
				foreach (var e in Entities)
				{
					e.ConfigureMap(cfg);
				}
			});
			config.AssertConfigurationIsValid();
			mapper = config.CreateMapper();
			return mapper;
		}

		public TId ConvertTo<TId>(string input)
		{
			return (TId) converters[typeof(TId)].Invoke(input);
		}
	}
}
