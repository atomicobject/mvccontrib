using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;

namespace MvcContrib.UnitTests.FluentHtml.CustomBehaviors
{
    /// <summary>
    /// This behavior affects elements bound to model properties having the Required attribute.  It adds Json data 
    /// to the element's attribute incdicating that the field is required.
    /// </summary>
    public class CustomRequiredDataBehavior : IBehavior<IMemberElement>
    {
        public void Execute(IMemberElement behavee)
        {
            var helper = new MemberBehaviorHelper<RequiredAttribute>();
            var attribute = helper.GetAttribute(behavee);
            if (attribute != null)
            {
                var data = new Dictionary<string, bool> { {"required", true} };
                BehaviorHelper.AddDataToClass(behavee, data);
            }
        }
    }
}