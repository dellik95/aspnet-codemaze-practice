using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Entities.Models;

namespace Repository.Extensions
{
	public static class RepositoryExtensions
	{
		public static IQueryable<Employee> FilterEmployee(this IQueryable<Employee> query, uint minAge, uint maxAge)
		{
			return query.Where(e => e.Age >= minAge && e.Age <= maxAge);
		}


		public static IOrderedQueryable<TSource> ApplyOrder<TSource>(this IQueryable<TSource> source, string columns)
		{
			if (string.IsNullOrEmpty(columns)) throw new ArgumentException("Invalid argument", nameof(columns));
			var orderColumns = columns.Split(",");
			var query = source.OrderBy(orderColumns.First().Trim());
			foreach (var orderBy in orderColumns.Skip(1))
			{
				query = query.ThenBy(orderBy.Trim());
			}

			return query;
		}

		public static IQueryable<Employee> Search(this IQueryable<Employee> query, string? searchTerm)
		{
			if (!string.IsNullOrEmpty(searchTerm))
			{
				var term = searchTerm.Trim().ToLower();
				query.Where(e => e.Name.ToLower().Contains(term));
			}

			return query;
		}


		public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string field,
			string dir = "asc")
		{
			var parameter = Expression.Parameter(typeof(TSource), "r");
			var expression = Expression.Property(parameter, field);
			var lambda = Expression.Lambda(expression, parameter);
			var type = typeof(TSource)
				.GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).PropertyType;

			var name = "OrderBy";
			if (string.Equals(dir, "desc", StringComparison.InvariantCultureIgnoreCase))
			{
				name = "OrderByDescending";
			}

			var method = typeof(Queryable).GetMethods().First(m => m.Name == name && m.GetParameters().Length == 2);
			var genericMethod = method.MakeGenericMethod(new[] {typeof(TSource), type});
			return genericMethod.Invoke(source, new object[] {source, lambda}) as IOrderedQueryable<TSource>;
		}

		public static IOrderedQueryable<TSource> ThenBy<TSource>(this IOrderedQueryable<TSource> source, string field,
			string dir = "asc")
		{
			var parameter = Expression.Parameter(typeof(TSource), "r");
			var expression = Expression.Property(parameter, field);
			var lambda = Expression.Lambda(expression, parameter); // r => r.AlgumaCoisa
			var type = typeof(TSource)
				.GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).PropertyType;

			var name = "ThenBy";
			if (string.Equals(dir, "desc", StringComparison.InvariantCultureIgnoreCase))
			{
				name = "ThenByDescending";
			}

			var methodInfo = typeof(Queryable).GetMethods().First(m => m.Name == name && m.GetParameters().Length == 2);
			var genericMethod = methodInfo.MakeGenericMethod(new[] {typeof(TSource), type});
			return genericMethod.Invoke(source, new object[] {source, lambda}) as IOrderedQueryable<TSource>;
		}
	}
}