using System.ComponentModel.DataAnnotations;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;

namespace MvcContrib.UnitTests.FluentHtml.CustomBehaviors
{
	public class CustomMaxLengthInMetadataBehavior : IOrderedBehavior<IMemberElement>
	{
		public CustomMaxLengthInMetadataBehavior(int order)
		{
			Order = order;
		}

		public int Order{ get; private set; }

		public void Execute(IMemberElement behavee)
		{
			var attribute = behavee.GetAttribute<RangeAttribute>();
			if (attribute != null)
			{
				behavee.Metadata.Add("maximum", attribute.Maximum);
				behavee.Metadata.Add("minimum", attribute.Minimum);
			}
		}
	}
}