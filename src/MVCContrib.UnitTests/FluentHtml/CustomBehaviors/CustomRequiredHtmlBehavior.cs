using System.ComponentModel.DataAnnotations;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;

namespace MvcContrib.UnitTests.FluentHtml.CustomBehaviors
{
	public class CustomRequiredHtmlBehavior : IBehavior<IMemberElement>
	{
		public void Execute(IMemberElement element)
		{
			var attribute = element.GetAttribute<RequiredAttribute>();
			if (attribute != null)
			{
				element.SetAttr("class", "req");
			}
		}
	}
}
