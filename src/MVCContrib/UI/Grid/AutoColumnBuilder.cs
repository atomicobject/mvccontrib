using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Can be used to build column using metadata
	/// </summary>
	public class AutoColumnBuilder<T> : ColumnBuilder<T> where T : class
	{
		private readonly ModelMetadataProvider _metadataProvider;

		public AutoColumnBuilder(ModelMetadataProvider metadata)
		{
			_metadataProvider = metadata;
			BuildColumns();
		}

		private void BuildColumns()
		{
			var modelMetadata = _metadataProvider.GetMetadataForType(() => null, typeof(T));

			foreach(var property in modelMetadata.Properties)
			{
				if(!property.ShowForDisplay)
				{
					continue;
				}

				var column = For(PropertyToExpression(property));

				if(!string.IsNullOrEmpty(property.DisplayName))
				{
					column.Named(property.DisplayName);
				}

				if(!string.IsNullOrEmpty(property.DisplayFormatString))
				{
					column.Format(property.DisplayFormatString);
				}
			}
		}

		private Expression<Func<T, object>> PropertyToExpression(ModelMetadata property)
		{
			var parameterExpression = Expression.Parameter(typeof(T), "x");
			Expression propertyExpression = Expression.Property(parameterExpression, property.PropertyName);

			if(property.ModelType.IsValueType)
			{
				propertyExpression = Expression.Convert(propertyExpression, typeof(object));
			}

			var expression = Expression.Lambda(
				typeof(Func<T, object>),
				propertyExpression,
				parameterExpression
				);

			return (Expression<Func<T, object>>)expression;
		}
	}
}