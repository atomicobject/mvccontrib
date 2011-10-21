using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.FluentHtml.Elements
{
	public abstract class OptionsElementBase<T> : FormElement<T> where T : OptionsElementBase<T>
	{
		protected OptionsElementBase(string tag, string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors) 
			: base(tag, name, forMember, behaviors) { }

		protected OptionsElementBase(string tag, string name) : base(tag, name) { }

		protected OptionChoices _optionChoices = new OptionChoices();

		/// <summary>
		/// The selected values.
		/// </summary>
		public IEnumerable SelectedValues
		{
			get { return _optionChoices.SelectedValues; }
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

		protected override void PreRender()
		{
			if (_optionChoices.Items != null)
			{
				builder.InnerHtml = RenderOptions();
			}
			base.PreRender();
		}

		protected abstract string RenderOptions();
	}
}