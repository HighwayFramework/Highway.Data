﻿using Highway.Data;
using Highway.RestArea;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
namespace Microsoft.AspNetCore.Builder
{
	public static class BuilderExtensions
	{
		public static IApplicationBuilder UseHighwayRestArea<TContext>(this IApplicationBuilder app, Action<RestAreaOptionsBuilder<TContext>> configure)
			where TContext : UnitOfWork
		{
			var contextOptions = app.ApplicationServices.GetService(typeof(DbContextOptions<TContext>)) as DbContextOptions<TContext>;
			var model = contextOptions.GetExtension<CoreOptionsExtension>().Model;
			var builder = new RestAreaOptionsBuilder<TContext>(model);
			configure(builder);
			var options = builder.Build();

			
			var routeBuilder = new RouteBuilder(app, new RouteHandler(options.Handler));

			foreach (var entity in options.Entities)
			{
				routeBuilder.MapRoute(
					name: $"{entity.UrlName} GetAll Route Name",
					template: $"apis/{entity.UrlName}",
					defaults: new {
						type = entity,
						method = "GetAll"
					},
					constraints: new { httpMethod = new HttpMethodRouteConstraint("GET") }
				);
				routeBuilder.MapRoute(
					name: $"{entity.UrlName} GET Route Name",
					template: $"apis/{entity.UrlName}/{{{entity.IdentityRouteValue}}}",
					defaults: new
					{
						type = entity,
						method = "GetOne"
					},
					constraints: new { httpMethod = new HttpMethodRouteConstraint("GET") }
				);
			}


			var routes = routeBuilder.Build();
			app.UseRouter(routes);
			return app;
		}
	}
}
