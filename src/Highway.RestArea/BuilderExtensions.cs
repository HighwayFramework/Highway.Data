using Highway.Data;
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
				var root = options.UrlPrefix;
				CreateRoutes(routeBuilder, entity, root);
			}


			var routes = routeBuilder.Build();
			app.UseRouter(routes);
			return app;
		}

		private static void CreateRoutes(RouteBuilder routeBuilder, EntityOptions entity, string parentRoot)
		{
			var currentRoot = $"{parentRoot}/{entity.UrlName}";
			var currentRootId = $"{currentRoot}/{{{entity.IdentityRouteValue}}}";

			routeBuilder.MapRoute(
				name: $"{currentRoot} GetAll",
				template: currentRoot,
				defaults: new { type = entity, method = "GetAll" },
				constraints: new { httpMethod = new HttpMethodRouteConstraint("GET") }
			);
			routeBuilder.MapRoute(
				name: $"{parentRoot}{entity.UrlName} Get",
				template: currentRootId,
				defaults: new { type = entity, method = "GetOne" },
				constraints: new { httpMethod = new HttpMethodRouteConstraint("GET") }
			);
			routeBuilder.MapRoute(
				name: $"{currentRoot} Post",
				template: currentRoot,
				defaults: new { type = entity, method = "Post" },
				constraints: new { httpMethod = new HttpMethodRouteConstraint("POST") }
			);
			routeBuilder.MapRoute(
				name: $"{currentRoot} Put",
				template: currentRoot,
				defaults: new { type = entity, method = "Put" },
				constraints: new { httpMethod = new HttpMethodRouteConstraint("PUT") }
			);

			foreach (var entityOptions in entity.Children)
			{
				CreateRoutes(routeBuilder, entityOptions, currentRootId);
			}
		}
	}
}
