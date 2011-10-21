using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base class for a set of radio buttons.
	/// </summary>
	public abstract class RadioSetBase<T> : OptionsElementBase<T> where T : RadioSetBase<T>
	{
		protected string _format;
		protected string _itemClass;
		protected Action<RadioButton, object, int> _optionModifier;

		protected RadioSetBase(string tag, string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(tag, name, forMember, behaviors) { }

		protected RadioSetBase(string tag, string name) : base(tag, name) { }

		/// <summary>
		/// Set the selected option.
		/// </summary>
		/// <param name="selectedValue">A value matching the option to be selected.</param>
		/// <returns></returns>
		public virtual T Selected(object selectedValue)
		{
			_optionChoices.SelectedValues = new List<object> { selectedValue };
			return (T)this;
		}

		/// <summary>
		/// Specify a format string for the HTML of each radio button and label.
		/// </summary>
		/// <param name="format">A format string.</param>
		public virtual T ItemFormat(string format)
		{
			_format = format;
			return (T)this;
		}

		/// <summary>
		/// Specify the class for the input and label elements of each item.
		/// </summary>
		/// <param name="value">A format string.</param>
		public virtual T ItemClass(string value)
		{
			_itemClass = value;
			return (T)this;
		}

		/// <summary>
		/// An action performed after each RadioButton element has been created.  This is useful to
		/// modify the element before is rendered.
		/// </summary>
		/// <param name="action">The action to perform. The parameters to the action are the RadioButton element, 
		/// the option, and the position of the option.</param>
		public virtual T EachOption(Action<RadioButton, object, int> action)
		{
			_optionModifier = action;
			return (T)this;
		}

		protected override TagRenderMode TagRenderMode
		{
			get { return TagRenderMode.Normal; }
		}

		protected override string RenderOptions()
		{
			var name = builder.Attributes[HtmlAttribute.Name];
			builder.Attributes.Remove(HtmlAttribute.Name);
			var sb = new StringBuilder();
			var i = 0;
			foreach (var option in _optionChoices.Items)
			{
				var value = _optionChoices.ValueFieldSelector(option);
				var behaviorsToPassDown = behaviors == null
					? null :
					behaviors.Where(x => (x is ValidationBehavior) == false);
				var radioButton = (new RadioButton(name, forMember, behaviorsToPassDown)
					.Value(value)
					.Format(_format))
					.LabelAfter(_optionChoices.TextFieldSelector(option).ToString(), _itemClass)
					.Checked(_optionChoices.IsSelectedValue(value));
				if (_itemClass != null)
				{
					radioButton.Class(_itemClass);
				}
				if (_optionModifier != null)
				{
					_optionModifier(radioButton, option, i);
				}
				sb.Append(radioButton);
				i++;
			}
			return sb.ToString();
		}

		protected override void ApplyModelState(ModelState state)
		{
			Selected(state.Value.ConvertTo(typeof(string)));
		}
	}
}