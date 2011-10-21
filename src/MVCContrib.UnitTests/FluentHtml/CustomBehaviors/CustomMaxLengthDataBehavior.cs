using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;

namespace MvcContrib.UnitTests.FluentHtml.CustomBehaviors
{
    public class CustomMaxLengthDataBehavior : IBehavior<IMemberElement>
    {
        /// <summary>
        /// This behavior affects elements bound to model properties having the Range attribute.  It adds Json data 
        /// to the element's attribute incdicating the maximum and minimum values for the field.
        /// </summary>
        public void Execute(IMemberElement behavee)
        {
            var memberBehaviorHelper = new MemberBehaviorHelper<RangeAttribute>();
            var attribute = memberBehaviorHelper.GetAttribute(behavee);
            if (attribute != null)
            {
                var data = new Dictionary<string, object>
                {
                    {"maximum", attribute.Maximum},
                    {"minimum", attribute.Minimum}
                };
                BehaviorHelper.AddDataToClass(behavee, data);
            }
        }
    }
}