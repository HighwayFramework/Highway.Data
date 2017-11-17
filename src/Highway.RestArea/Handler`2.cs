using Highway.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Highway.RestArea
{
	public class Handler<TContext, TEntity, TId>
		where TContext : UnitOfWork
		where TId : IEquatable<TId>
		where TEntity : class, IIdentifiable<TId>

	{
		private readonly RestAreaOptions<TContext> restAreaOptions;
		private readonly EntityOptions<TEntity, TId> options;
		private readonly IRepository repo;
		private readonly ILogger logger;
		public Handler(IRepository repo, RestAreaOptions<TContext> restAreaOptions, EntityOptions<TEntity, TId> options, ILogger logger)
		{
			this.restAreaOptions = restAreaOptions;
			this.options = options;
			this.logger = logger;
			this.repo = repo;
		}

		public async Task GetAll(HttpContext context, RouteData routeData)
		{
			var data = await repo.FindAsync(options.GetAllFactory());
			Json(data, context);
		}

		public async Task GetOne(HttpContext context, RouteData routeData)
		{
			TId id = restAreaOptions.ConvertTo<TId>(routeData.Values[options.IdentityRouteValue].ToString());
			var data = await repo.FindAsync(options.GetByIdFactory(id));
			Json(data, context);
		}

		private void Json<T>(T obj, HttpContext context)
		{
			context.Response.ContentType = "application/json";
			using (var writer = new StreamWriter(context.Response.Body))
			{
				new JsonSerializer().Serialize(writer, obj);
			}
		}
	}
}
