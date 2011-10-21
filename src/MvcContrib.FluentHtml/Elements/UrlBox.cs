using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Generate an HTML input element of type 'url.'
	/// </summary>
	public class UrlBox : UrlBoxBase<UrlBox>
	{
		/// <summary>
		/// Generate an HTML input element of type 'url.'
		/// </summary>
		/// <param name="name">Value of the 'name' attribute of the element.  Also used to derive the 'id' attribute.</param>
		public UrlBox(string name) : base(name) { }

		/// <summary>
		/// Generate an HTML input element of type 'url.'
		/// </summary>
		/// <param name="name">Value of the 'name' attribute of the element.  Also used to derive the 'id' attribute.</param>
		/// <param name="forMember">Expression indicating the view model member assocaited with the element</param>
		/// <param name="behaviors">Behaviors to apply to the element</param>
		public UrlBox(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(name, forMember, behaviors) { }
	}
}