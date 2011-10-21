using System.ComponentModel.DataAnnotations;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;

namespace MvcContrib.UnitTests.FluentHtml.CustomBehaviors
{
	public class CustomRequiredInMetadataBehavior : IOrderedBehavior<IMemberElement>
	{
		public CustomRequiredInMetadataBehavior(int order)
		{
			Order = order;
		}

		public int Order { get; private set; }

		public void Execute(IMemberElement behavee)
		{
			var attribute = behavee.GetAttribute<RequiredAttribute>();
			if (attribute != null)
			{
				behavee.Metadata.Add("required", true);
			}
		}
	}
}