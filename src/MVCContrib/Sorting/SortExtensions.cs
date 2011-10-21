using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MvcContrib.Sorting
{
	/// <summary>
	/// Extension methods for sorting.
	/// </summary>
	public static class SortExtensions
	{
		/// <summary>
		/// Orders a datasource by a property with the specified name in the specified direction
		/// </summary>
		/// <param name="datasource">The datasource to order</param>
		/// <param name="propertyName">The name of the property to order by</param>
		/// <param name="direction">The direction</param>
		public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> datasource, string propertyName, SortDirection direction)
		{
			return datasource.AsQueryable().OrderBy(propertyName, direction);
		}

		/// <summary>
		/// Orders a datasource by a property with the specified name in the specified direction
		/// </summary>
		/// <param name="datasource">The datasource to order</param>
		/// <param name="propertyName">The name of the property to order by</param>
		/// <param name="direction">The direction</param>
		public static IQueryable<T> OrderBy<T>(this IQueryable<T> datasource, string propertyName, SortDirection direction)
		{
			//http://msdn.microsoft.com/en-us/library/bb882637.aspx

			if(string.IsNullOrEmpty(propertyName))
			{
				return datasource;
			}

			var type = typeof(T);
			var property = type.GetProperty(propertyName);

			if(property == null)
			{
				throw new InvalidOperationException(string.Format("Could not find a property called '{0}' on type {1}", propertyName, type));
			}

			var parameter = Expression.Parameter(type, "p");
			var propertyAccess = Expression.MakeMemberAccess(parameter, property);
			var orderByExp = Expression.Lambda(propertyAccess, parameter);

			const string orderBy = "OrderBy";
			const string orderByDesc = "OrderByDescending";

			string methodToInvoke = direction == SortDirection.Ascending ? orderBy : orderByDesc;

			var orderByCall = Expression.Call(typeof(Queryable), 
				methodToInvoke, 
				new[] { type, property.PropertyType }, 
				datasource.Expression, 
				Expression.Quote(orderByExp));

			return datasource.Provider.CreateQuery<T>(orderByCall);
		}
	}
}