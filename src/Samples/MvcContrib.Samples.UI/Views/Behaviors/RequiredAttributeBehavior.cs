using System.ComponentModel.DataAnnotations;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.Samples.UI.Views
{
    public class RequiredAttributeBehavior: IBehavior<IMemberElement>
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