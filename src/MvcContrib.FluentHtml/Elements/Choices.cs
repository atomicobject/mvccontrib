using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MvcContrib.FluentHtml.Elements
{
	public class Choices
	{
		public IEnumerable Items { get; private set; }
		public Func<object, object> TextFieldSelector { get; private set; }
		public Func<object, object> ValueFieldSelector { get; private set; }

		public IEnumerable SelectedValues { get; set; }

		public void Set<TKey, TValue>(IDictionary<TKey, TValue> value)
		{
			Items = value;
			SetFieldExpressions("Key", "Value");
		}

		public void Set(MultiSelectList value)
		{
			if (value != null)
			{
				Items = value.Items;
				SetFieldExpressions(value.DataValueField, value.DataTextField);
				if (value.SelectedValues != null)
				{
					SelectedValues = value.SelectedValues;
				}
			}
		}

		public void Set(IEnumerable<SelectListItem> value)
		{
			Items = value;
			SetFieldExpressions("Value", "Text");
			if (value != null)
			{
				SelectedValues = value.Where(x => x.Selected).Select(x => x.Value).ToList();
			}
		}

		public void Set(IEnumerable values, string valueField, string textField)
		{
			Items = values;
			SetFieldExpressions(valueField, textField);
		}

		public void Set<TValue>(IEnumerable<TValue> value)
		{
			if (typeof(TValue).IsEnum)
			{
				Set(value.ToDictionary(x => Convert.ToInt32(x).ToString(), x => x.ToString()));
			}
			else
			{
				Set(value.ToDictionary(x => x, x => x));
			}
		}

		public void Set<TDataSource>(IEnumerable<TDataSource> values, Func<TDataSource, object> valueFieldSelector, Func<TDataSource, object> textFieldSelector)
		{
			if (valueFieldSelector == null) throw new ArgumentNullException("valueFieldSelector");
			if (textFieldSelector == null) throw new ArgumentNullException("textFieldSelector");

			Items = values;
			TextFieldSelector = x => textFieldSelector((TDataSource)x);
			ValueFieldSelector = x => valueFieldSelector((TDataSource)x);
		}

		public void Set<TEnum>() where TEnum : struct
		{
			if (!typeof(TEnum).IsEnum)
			{
				throw new ArgumentException("The generic parameter must be an enum", "TEnum");
			}
			var values = Enum.GetValues(typeof(TEnum));
			var dict = values.Cast<object>().ToDictionary(item => Convert.ToInt32(item).ToString(), item => item.ToString());
			Set(dict);
		}

		protected void SetFieldExpressions(string dataValueField, string dataTextField)
		{
			if (dataValueField == null) throw new ArgumentNullException("dataValueField");
			if (dataTextField == null) throw new ArgumentNullException("dataTextField");

			var enumerator = Items.GetEnumerator();
			if (!enumerator.MoveNext())
			{
				return;
			}
			var type = enumerator.Current.GetType();
			var valueProp = type.GetProperty(dataValueField);
			if (valueProp == null)
			{
				throw new ArgumentException(string.Format("The option list does not contain the specified value property: {0}", dataValueField), "dataValueField");
			}
			var textProp = type.GetProperty(dataTextField);
			if (textProp == null)
			{
				throw new ArgumentException(string.Format("The option list does not contain the specified text property: {0}", dataTextField), "dataTextField");
			}

			TextFieldSelector = x => textProp.GetValue(x, null);
			ValueFieldSelector = x => valueProp.GetValue(x, null);
		}

		public bool IsSelectedValue(object value)
		{
			var valueString = value == null ? string.Empty : value.ToString();
			if (SelectedValues != null)
			{
				var enumerator = SelectedValues.GetEnumerator();
				while (enumerator.MoveNext())
				{
					var selectedValueString = enumerator.Current == null
					                          	? string.Empty
					                          	: enumerator.Current.GetType().IsEnum
					                          	  	? ((int)enumerator.Current).ToString()
					                          	  	: enumerator.Current.ToString();
					if (valueString == selectedValueString)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}