using System.Collections.Generic;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Generates an input element of type 'file.'
	/// </summary>
	public class FileUpload : Input<FileUpload>, ISupportsMaxLength
	{
		/// <summary>
		/// Generates an input element of type 'file.'
		/// </summary>
		/// <param name="name">Value of the 'name' attribute of the element. Also used to derive the 'id' attribute.</param>
		public FileUpload(string name) : base(HtmlInputType.File, name) { }

		/// <summary>
		/// Generates an input element of type 'file.'
		/// </summary>
		/// <param name="name">Value of the 'name' attribute of the element. Also used to derive the 'id' attribute.</param>
		/// <param name="behaviors">Behaviors to apply to the element.</param>
		public FileUpload(string name, IEnumerable<IBehaviorMarker> behaviors)
			: base(HtmlInputType.File, name, null, behaviors) { }

		/// <summary>
		/// Set the 'maxlength' attribute.
		/// </summary>
		/// <param name="value">The value of the attribute.</param>
		public virtual FileUpload MaxLength(int value)
		{
			Attr(HtmlAttribute.MaxLength, value);
			return this;
		}

		/// <summary>
		/// Add or remove the 'multiple' attribute. 
		/// </summary>
		/// <param name="value">Whether to add or remove the attribute.</param>
		public virtual FileUpload Multiple(bool value)
		{
			if (value)
			{
				Attr(HtmlAttribute.Multiple, HtmlAttribute.Multiple);
			}
			else
			{
				((IElement)this).RemoveAttr(HtmlAttribute.Multiple);
			}
			return this;
		}
	}
}
