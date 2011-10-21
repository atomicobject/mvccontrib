using System.ComponentModel.DataAnnotations;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.UnitTests.FluentHtml.CustomBehaviors
{
    public class CustomExpressionHtmlBehavior: IBehavior<IMemberElement>
    {
        public void Execute(IMemberElement element)
        {
            var attribute = element.GetAttribute<RegularExpressionAttribute>();
            if (attribute != null)
            {
                element.SetAttr(HtmlAttribute.Pattern, attribute.Pattern);
            }
        }
    }
}
