using Highway.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using RestAreaTest.Entities;
using RestAreaTest.Models;
using RestAreaTest.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

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
				ra.AddEntityById<Blog, Guid, BlogModel>(e =>
				{
					e.WithGetOne<GetOneBlog>();
					e.WithGetAll<GetAllBlogs>();

					e.AddEntityById<Post, Guid, PostModel>(p => p.Posts, x =>
					{
						x.WithGetOne<GetOnePost>();
						x.WithGetAll<GetAllPosts>();
					});
				});
			});
			SetupTestData(app);
		}
		private void SetupTestData(IApplicationBuilder app)
		{
			using (var scope = app.ApplicationServices.CreateScope())
			using (var c = scope.ServiceProvider.GetService<UnitOfWork>())
			{
				c.Add(new Blog
				{
					Id = new Guid("AAFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF"),
					Title = "TimRayburn.net",
					Posts = new List<Post>
					{
						new Post
						{
							Id = new Guid("AAAAFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF"),
							Title = "Ruling the World",
							Body = "#Awesome!"
						}
					}
				});
				c.Add(new Blog
				{
					Id = new Guid("BBFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF"),
					Title = "Driven To Develop",
					Posts = new List<Post>
					{
						new Post
						{
							Id = new Guid("BBBBFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF"),
							Title = "Smashing Success",
							Body = "It's what we do."
						}
					}
				});
				c.Commit();
			}
		}
	}
}
