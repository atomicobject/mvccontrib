using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcContrib.FluentHtml.Elements
{
	public class OptionChoices : Choices
	{
		private Action<Option, object, int> optionModifier;
		private Option firstOption;
		private bool hideFirstOption;
		protected Func<object, object> groupSelectory;

		public void Set<TSource, TKey>(IEnumerable<TSource> values, string valueField, string textField, Func<TSource, TKey> groupSelector)
		{
			groupSelectory = x => groupSelector((TSource)x);
			Set(values, valueField, textField);
		}

		public void Set<TSource>(IEnumerable<TSource> values, Func<TSource, object> valueFieldSelector, Func<TSource, object> textFieldSelector, Func<TSource, object> groupSelector)
		{
			groupSelectory = x => groupSelector((TSource)x);
			Set(values, valueFieldSelector, textFieldSelector);
		}

		public void EachOption(Action<Option, object, int> action)
		{
			optionModifier = action;
		}

		public void FirstOption(Option value)
		{
			firstOption = value;
		}

		public void FirstOption(string text)
		{
			FirstOption(null, text);
		}

		public void FirstOption(string value, string text)
		{
			firstOption = new Option().Text(text ?? string.Empty).Value(value ?? string.Empty).Selected(false);
		}

		public void HideFirstOptionWhen(bool value)
		{
			hideFirstOption = value;
		}

		public string Render()
		{
			var sb = new StringBuilder();

			var i = 0;

			if (firstOption != null && hideFirstOption == false)
			{
				sb.Append(firstOption);
				i++;
			}

			RenderOptions(sb, i);

			return sb.ToString();
		}

		protected void RenderOptions(StringBuilder sb, int i)
		{
			if (groupSelectory != null)
			{
				var options = Items.Cast<object>();
				foreach (var group in options.GroupBy(groupSelectory))
				{
					sb.AppendFormat("<optgroup label=\"{0}\">", group.Key);
					sb.Append(RenderOptions(group.AsEnumerable(), i));
					sb.Append("</optgroup>");
				}
			}
			else
			{
				sb.Append(RenderOptions(Items, i));
			}
		}

		private StringBuilder RenderOptions(IEnumerable options, int i)
		{
			var sb = new StringBuilder();
			foreach (var opt in options)
			{
				RenderOption(i, sb, opt);
				i++;
			}
			return sb;
		}

		private void RenderOption(int i, StringBuilder sb, object opt)
		{
			var option = GetOption(opt);
			if (optionModifier != null)
			{
				optionModifier(option, opt, i);
			}
			sb.Append(option);
		}

		private Option GetOption(object option)
		{
			var value = ValueFieldSelector(option);
			var text = TextFieldSelector(option);

			return new Option()
				.Value(value == null ? string.Empty : value.ToString())
				.Text(text == null ? string.Empty : text.ToString())
				.Selected(IsSelectedValue(value));
		}
	}
}