using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base class for select elements.
	/// </summary>
	/// <typeparam name="T">The derived type.</typeparam>
	public abstract class SelectBase<T> : OptionsElementBase<T> where T : SelectBase<T>
	{
		protected SelectBase(string name) : base(HtmlTag.Select, name) { }

		protected SelectBase(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(HtmlTag.Select, name, forMember, behaviors) { }

		/// <summary>
		/// Set the 'size' attribute.
		/// </summary>
		/// <param name="value">The value of the 'size' attribute.</param>
		/// <returns></returns>
		public virtual T Size(int value)
		{
			Attr(HtmlAttribute.Size, value);
			return (T)this;
		}

		/// <summary>
		/// Uses the specified open as the first option in the select.
		/// </summary>
		/// <param name="firstOption">The first option</param>
		/// <returns></returns>
		public virtual T FirstOption(Option firstOption)
		{
			_optionChoices.FirstOption(firstOption);
			return (T)this;
		}

		/// <summary>
		/// Specifies the text for the first option. The value for the first option will be an empty string.
		/// </summary>
		/// <param name="text">The text for the first option</param>
		/// <returns></returns>
		public virtual T FirstOption(string text)
		{
			FirstOption(null, text);
			return (T)this;
		}

		/// <summary>
		/// Specifies the value and text for the first option.
		/// </summary>
		/// <param name="value">The value for the first option. If ommitted, will default to an empty string.</param>
		/// <param name="text">The text for the first option. If ommitted, will the default to an empty string.</param>
		/// <returns></returns>
		public virtual T FirstOption(string value, string text)
		{
			_optionChoices.FirstOption(value, text);
			return (T)this;
		}

		[Obsolete("Use the 'FirstOption' method instead.")]
		public virtual T FirstOptionText(string firstOptionText)
		{
			FirstOption(firstOptionText);
			return (T)this;
		}

		/// <summary>
		/// Hides the first option when the value passed in is true. 
		/// </summary>
		/// <param name="hideFirstOption">True to hide the first option, otherwise false.</param>
		/// <returns></returns>
		public virtual T HideFirstOptionWhen(bool hideFirstOption)
		{
			_optionChoices.HideFirstOptionWhen(hideFirstOption);
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

		/// <summary>
		/// Set the options for the select list.
		/// </summary>
		/// <typeparam name="TSource">The type of the source item.</typeparam>
		/// <typeparam name="TKey">The key for the group selector.</typeparam>
		/// <param name="values">The items used to create the select list.</param>
		/// <param name="valueField">The name of the member to use as the value of the select list.</param>
		/// <param name="textField">The name of the member to use as the text of the select list.</param>
		/// <param name="groupSelector">Function used to group items in the select list.</param>
		public virtual T Options<TSource, TKey>(IEnumerable<TSource> values, string valueField, string textField, Func<TSource, TKey> groupSelector)
		{
			_optionChoices.Set(values, valueField, textField, groupSelector);
			return (T)this;
		}

		/// <summary>
		/// Set the options for the select list.
		/// </summary>
		/// <typeparam name="TSource">The type of the source item.</typeparam>
		/// <param name="values">The items used to create the select list.</param>
		/// <param name="valueFieldSelector">The member to use as the value of the select list.</param>
		/// <param name="textFieldSelector">The member to use as the text of the select list.</param>
		/// <param name="groupSelector">Function used to group items in the select list.</param>
		public virtual T Options<TSource>(IEnumerable<TSource> values, Func<TSource, object> valueFieldSelector, Func<TSource, object> textFieldSelector, Func<TSource, object> groupSelector)
		{
			_optionChoices.Set(values, valueFieldSelector, textFieldSelector, groupSelector);
			return (T)this;
		}

		protected override TagRenderMode TagRenderMode
		{
			get { return TagRenderMode.Normal; }
		}

		protected override string RenderOptions()
		{
			return _optionChoices.Render();
		}
	}
}