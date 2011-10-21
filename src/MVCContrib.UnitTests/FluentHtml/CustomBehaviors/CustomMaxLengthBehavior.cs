using System.ComponentModel.DataAnnotations;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.UnitTests.FluentHtml.CustomBehaviors
{
	public class CustomMaxLengthBehavior : IBehavior<IMemberElement>
	{
		public void Execute(IMemberElement element)
		{
			var attribute = element.GetAttribute<RangeAttribute>();
			if (attribute == null) 
			{
				return;
			}
			if (element is ISupportsMaxLength) 
			{
				element.SetAttr(HtmlAttribute.MaxLength, attribute.Maximum);
			}
		}
	}
}