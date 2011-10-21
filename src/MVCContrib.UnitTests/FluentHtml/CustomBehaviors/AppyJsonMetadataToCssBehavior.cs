using System.Globalization;
using System.Web.Script.Serialization;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;

namespace MvcContrib.UnitTests.FluentHtml.CustomBehaviors
{
	public class AppyJsonMetadataToCssBehavior : IOrderedBehavior<IElement>
	{
		public AppyJsonMetadataToCssBehavior(int order)
		{
			Order = order;
		}

		public int Order { get; private set; }

		public void Execute(IElement element)
		{
			if(element.Metadata == null)
			{
				return;
			}
			var jsSerializer = new JavaScriptSerializer();
			var serializedMetadata = jsSerializer.Serialize(element.Metadata);
			var classToAdd = string.Format(CultureInfo.CurrentCulture, "{0}", serializedMetadata.Replace('\"', '\''));
			element.AddClass(classToAdd);
		}
	}
}