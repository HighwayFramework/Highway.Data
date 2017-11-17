using Microsoft.AspNetCore.Routing;
using System;
using Highway.RestArea;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Highway.Data;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ConfigureExtensions
	{
		public static IServiceCollection AddHighwayRestArea(this IServiceCollection services)
		{
			services.AddRouting();
			services.AddScoped<IRepository, Repository>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			return services;
		}
	}
}
