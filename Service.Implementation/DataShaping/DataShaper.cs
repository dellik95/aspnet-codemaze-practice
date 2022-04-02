using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Contracts;
using Entities.Models;

namespace Service.Implementations.DataShaping
{
	public class DataShaper<T> : IDataShaper<T>
	{
		public PropertyInfo[] Properies { get; set; }

		public DataShaper()
		{
			Properies = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
		}

		public IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string fields)
		{
			var requiredProperties = GetRequiresProperties(fields);
			return FetchData(entities, requiredProperties);
		}


		public ShapedEntity ShapeData(T entity, string fields)
		{
			var requiredProperties = GetRequiresProperties(fields);
			return FetchDataForEntity(entity, requiredProperties);
		}

		private IEnumerable<PropertyInfo> GetRequiresProperties(string fieldsString)
		{
			var requiredProperties = new List<PropertyInfo>();
			if (string.IsNullOrWhiteSpace(fieldsString))
				return Properies;
			var fields = fieldsString.Split(",", StringSplitOptions.RemoveEmptyEntries);
			foreach (var field in fields)
			{
				var property =
					Properies.FirstOrDefault(p =>
						p.Name.Equals(field.Trim(), StringComparison.InvariantCultureIgnoreCase));
				if (property == null) continue;
				requiredProperties.Add(property);
			}

			return requiredProperties;
		}

		private IEnumerable<ShapedEntity> FetchData(IEnumerable<T> entities,
			IEnumerable<PropertyInfo> requiredProperties)
		{
			var shapeData = new List<ShapedEntity>();
			foreach (var entity in entities)
			{
				var shapeObject = FetchDataForEntity(entity, requiredProperties);
				shapeData.Add(shapeObject);
			}

			return shapeData;
		}

		private ShapedEntity FetchDataForEntity(T entity, IEnumerable<PropertyInfo> requiredProperties)
		{
			var shapeObject = new ShapedEntity();
			foreach (var property in requiredProperties)
			{
				var value = property.GetValue(entity);
				shapeObject.Entity.TryAdd(property.Name, value);
			}

			var objectProperty = entity.GetType().GetProperty("Id");
			shapeObject.Id = (int) objectProperty.GetValue(entity);
			return shapeObject;
		}
	}
}