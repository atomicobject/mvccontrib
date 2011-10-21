using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base class for a datalist element.
	/// </summary>
	public abstract class DataListBase<T> : Element<T> where T : DataListBase<T>
	{
		protected OptionChoices _optionChoices = new OptionChoices();

		protected DataListBase(IEnumerable<IBehaviorMarker> behaviors) 
			: base(HtmlTag.DataList, null, behaviors) {}

		protected DataListBase() : base(HtmlTag.DataList) { }

		/// <summary>
		/// Sets the 'data' attribute.
		/// </summary>
		/// <param name="uri">A location that provides data to the element.</param>
		public T Data(string uri)
		{
			Attr(HtmlAttribute.Data, uri);
			return (T)this;
		}

		/// <summary>
		/// Set the options.
		/// </summary>
		/// <param name="value">List of options.</param>
		public virtual T Options(MultiSelectList value)
		{
			_optionChoices.Set(value);
			return (T)this;
		}

		/// <summary>
		/// Set the options.
		/// </summary>
		/// <param name="value">List of options.</param>
		public virtual T Options<TKey, TValue>(IDictionary<TKey, TValue> value)
		{
			_optionChoices.Set(value);
			return (T)this;
		}

		/// <summary>
		/// Set the options.
		/// </summary>
		/// <param name="value">List of options.</param>
		public virtual T Options(IEnumerable<SelectListItem> value)
		{
			_optionChoices.Set(value);
			return (T)this;
		}

		/// <summary>
		/// Set the options.
		/// </summary>
		/// <param name="values">List of options.</param>
		/// <param name="valueField">The name of the member to use as the value of the select list.</param>
		/// <param name="textField">The name of the member to use as the text of the select list.</param>
		public virtual T Options(IEnumerable values, string valueField, string textField)
		{
			_optionChoices.Set(values, valueField, textField);
			return (T)this;
		}

		/// <summary>
		/// Set the options.
		/// </summary>
		/// <param name="value">List of options.</param>
		public virtual T Options<TValue>(IEnumerable<TValue> value)
		{
			_optionChoices.Set(value);
			return (T)this;
		}

		/// <summary>
		/// Set the options.
		/// </summary>
		/// <param name="values">List of options.</param>
		/// <param name="valueFieldSelector">The member to use as the value of the select list.</param>
		/// <param name="textFieldSelector">The member to use as the text of the select list.</param>
		public virtual T Options<TDataSource>(IEnumerable<TDataSource> values, Func<TDataSource, object> valueFieldSelector, Func<TDataSource, object> textFieldSelector)
		{
			_optionChoices.Set(values, valueFieldSelector, textFieldSelector);
			return (T)this;
		}

		/// <summary>
		/// Set the options using the specified enum.
		/// </summary>
		/// <typeparam name="TEnum">The type of enum to use.</typeparam>
		public virtual T Options<TEnum>() where TEnum : struct
		{
			_optionChoices.Set<TEnum>();
			return (T)this;
		}

		/// <summary>
		/// An action performed after each Option element has been created.  This is useful to
		/// modify the element before is rendered.
		/// </summary>
		/// <param name="action">The action to perform. The parameters to the action are the Option element, 
		/// the option item, and the position of the option.</param>
		public virtual T EachOption(Action<Option, object, int> action)
		{
			_optionChoices.EachOption(action);
			return (T)this;
		}

		protected override void PreRender()
		{
			if (_optionChoices.Items != null)
			{
				builder.InnerHtml = _optionChoices.Render();
			}
			base.PreRender();
		}
	}
}