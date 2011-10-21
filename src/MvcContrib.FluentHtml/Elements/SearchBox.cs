using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Generate an HTML input element of type 'search.'
	/// </summary>
	public class SearchBox : SearchBoxBase<SearchBox>
	{
		/// <summary>
		/// Generate an HTML input element of type 'search.'
		/// </summary>
		/// <param name="name">Value of the 'name' attribute of the element.  Also used to derive the 'id' attribute.</param>
		public SearchBox(string name) : base(name) { }

		/// <summary>
		/// Generate an HTML input element of type 'search.'
		/// </summary>
		/// <param name="name">Value of the 'name' attribute of the element.  Also used to derive the 'id' attribute.</param>
		/// <param name="forMember">Expression indicating the view model member assocaited with the element</param>
		/// <param name="behaviors">Behaviors to apply to the element</param>
		public SearchBox(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(name, forMember, behaviors) { }
	}
}