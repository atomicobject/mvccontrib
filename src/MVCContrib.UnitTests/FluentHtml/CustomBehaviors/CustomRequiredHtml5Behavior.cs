using System.ComponentModel.DataAnnotations;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.UnitTests.FluentHtml.CustomBehaviors
{
    public class CustomRequiredHtml5Behavior : IBehavior<IMemberElement>
    {
        public void Execute(IMemberElement element)
        {
            var attribute = element.GetAttribute<RequiredAttribute>();
            if (attribute != null)
            {
                element.SetAttr(HtmlAttribute.Required, HtmlAttribute.Required);
            }
        }
    }
}
