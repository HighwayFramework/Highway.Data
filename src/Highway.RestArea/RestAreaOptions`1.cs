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
using Microsoft.EntityFrameworkCore.Metadata;

namespace Highway.RestArea
{
	public class RestAreaOptions<TContext> : RestAreaOptions
		where TContext : UnitOfWork
	{
		public RequestDelegate Handler { get; }

		public RestAreaOptions(
			IModel model,
			string urlPrefix,
			IEnumerable<EntityOptions> entityOptions, 
			Action<IMapperConfigurationExpression> mapperConfig
		) : base(model, urlPrefix, entityOptions, mapperConfig)
		{
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
	}
}
