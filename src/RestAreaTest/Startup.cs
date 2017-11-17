using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Highway.Data;
using RestAreaTest.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using RestAreaTest.Models;

namespace RestAreaTest
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<UnitOfWork>(opt =>
			{
				opt.UseInMemoryDatabase("Test", im =>
				 {
				 });
				var csBuilder = new CoreConventionSetBuilder(
					new CoreConventionSetBuilderDependencies(
						new CoreTypeMapper(
							new CoreTypeMapperDependencies())));
				var builder = new ModelBuilder(csBuilder.CreateConventionSet());

				builder.Entity<Blog>();
				builder.Entity<Post>();

				opt.UseModel(builder.Model);
			},
			contextLifetime: ServiceLifetime.Scoped,
			optionsLifetime: ServiceLifetime.Singleton);
			services.AddHighwayRestArea();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHighwayRestArea<UnitOfWork>(ra =>
			{
				ra.ConvertTo(s => new Guid(s));
				//ra.AddTransform(cfg =>
				//{
				//	cfg.CreateMap<Post, PostModel>();
				//});
				ra.AddFromContext<Post, Guid, PostModel>(e => e.WithUrlName("posts"));
				ra.AddFromContext<Blog, Guid, BlogModel>(e =>
				{
					e.WithUrlName("blogs");
				});
			});

			using (var scope = app.ApplicationServices.CreateScope())
			using (var c = scope.ServiceProvider.GetService<UnitOfWork>())
			{
				c.Add(new Blog
				{
					Id = new Guid("510B2A5E-0CDD-4590-95E3-05E93DFA247E"),
					Title = "TimRayburn.net",
					Posts = new List<Post>
					{
						new Post
						{
							Id = Guid.NewGuid(),
							Title = "Ruling the World",
							Body = "#Awesome!"
						}
					}
				});
				c.Commit();
			}
		}
	}
}
