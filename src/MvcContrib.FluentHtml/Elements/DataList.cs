using System.Collections.Generic;
using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// A datalist element.
	/// </summary>
	public class DataList : DataListBase<DataList>
	{
		public DataList(string id)
		{
			Id(id);
		}

		public DataList(string id, IEnumerable<IBehaviorMarker> behaviors)
			: base(behaviors)
		{
			Id(id);
		}
	}
}