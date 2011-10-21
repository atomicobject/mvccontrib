using System.ComponentModel.DataAnnotations;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.Samples.UI.Views
{
    public class RegularExpressionBehavior: IBehavior<IMemberElement>
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