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

namespace Highway.RestArea
{
	public class RestAreaOptions<TContext>
		where TContext : UnitOfWork
	{
		public IEnumerable<EntityOptions> Entities { get; }
		private readonly Dictionary<Type, Func<string, object>> converters;
		public RequestDelegate Handler { get; }

		public RestAreaOptions(IEnumerable<EntityOptions> entityOptions, Dictionary<Type, Func<string, object>> converters)
		{
			this.converters = converters;
			Entities = entityOptions;
			Handler = async (c) =>
			{
				var lFactory = c.RequestServices.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
				IRepository repo = c.RequestServices.GetService(typeof(IRepository)) as IRepository;
				var logger = lFactory.CreateLogger("Highway.RestArea");
				var routeData = c.GetRouteData();
				var entityOption = routeData.Values["type"] as EntityOptions;

				var handlerType = typeof(Handler<,,>).MakeGenericType(entityOption.UnitOfWorkType, entityOption.EntityType, entityOption.IdentityType);
				var handler = Activator.CreateInstance(handlerType, repo, this, entityOption, logger);

				await handlerType.GetMethod(routeData.Values["method"] as string)
					.InvokeAsync(handler, c, routeData);
			};
		}

		public TId ConvertTo<TId>(string input)
		{
			return (TId) converters[typeof(TId)].Invoke(input);
		}
	}
	public static class IRepositoryExtensions
	{
		private static MethodInfo asyncQuery = typeof(IReadOnlyRepository).GetMethods()
						 .Where(m => m.Name == "FindAsync")
						 .Select(m => new {
							 Method = m,
							 Params = m.GetParameters(),
							 Args = m.GetGenericArguments()
						 })
						 .Where(x => x.Params.Length == 1
									 && x.Args.Length == 1
									 && x.Params[0].ParameterType == typeof(IQuery<>).MakeGenericType(x.Args[0]))
						 .Select(x => x.Method)
						 .First();

		public static async Task<IEnumerable<object>> FindTypeAsync(this IRepository repo, Type entityType, object query, ILogger logger)
		{
			var methodInfo = asyncQuery.MakeGenericMethod(entityType);
			var data = await methodInfo.InvokeAsync(repo, new object[] { query });
			return data as IEnumerable<object>;
		}
	}
}
