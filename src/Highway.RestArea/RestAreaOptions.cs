using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Highway.RestArea
{
	public class RestAreaOptions
	{

		private readonly Dictionary<Type, Func<string, object>> converters;
		private readonly Action<IMapperConfigurationExpression> mapperConfig;
		public IModel Model { get; }
		public IEnumerable<EntityOptions> Entities { get; }
		public JsonSerializer Serializer { get; }
		public string UrlPrefix { get; }


		public RestAreaOptions(
			IModel model,
			IEnumerable<EntityOptions> entityOptions,
			Dictionary<Type, Func<string, object>> converters,
			Action<IMapperConfigurationExpression> mapperConfig
		)
		{
			Model = model;
			Action<IMapperConfigurationExpression> defaultMapperConfig = cfg => { };
			this.mapperConfig = mapperConfig ?? defaultMapperConfig;
			this.converters = converters;
			Entities = entityOptions;
			Serializer = new JsonSerializer();
			UrlPrefix = "apis";
		}

		public TId ConvertTo<TId>(string input)
		{
			return (TId)ConvertTo(typeof(TId), input);
		}

		public object ConvertTo(Type idType, string input)
		{
			return converters[idType].Invoke(input);
		}

		private IMapper mapper = null;
		public IMapper GetMapper()
		{
			if (mapper != null) return mapper;
			var config = new MapperConfiguration(cfg =>
			{
				mapperConfig(cfg);
				ProcessEntities(cfg, Entities);
			});
			config.AssertConfigurationIsValid();
			mapper = config.CreateMapper();
			return mapper;
		}

		private void ProcessEntities(IMapperConfigurationExpression cfg, IEnumerable<EntityOptions> entities)
		{
			foreach (var e in entities)
			{
				e.ConfigureMap(cfg);
				ProcessEntities(cfg, e.Children);
			}
		}
	}
}
