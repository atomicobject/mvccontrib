using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
    /// <summary>
    /// Base class for HTML input element of type 'submit.'
    /// </summary>
    public abstract class SubmitButtonBase<T> : Input<T> where T : SubmitButtonBase<T>
    {
        protected SubmitButtonBase(string text) : this(text, null) { }

        protected SubmitButtonBase(string text, IEnumerable<IBehaviorMarker> behaviors)
            : base(HtmlInputType.Submit, text == null ? null : text.FormatAsHtmlName(), null, behaviors)
        {
            elementValue = text;
        }

        /// <summary>
        /// Set the 'formaction' attribute.
        /// </summary>
        /// <param name="value">The value of the attribute.</param>
        public T Action(string value)
        {
            Attr(HtmlAttribute.FormAction, value);
            return (T)this;
        }

        /// <summary>
        /// Set the 'formenctype' attribute.
        /// </summary>
        /// <param name="value">The value of the attribute.</param>
        public T EncType(string value)
        {
            Attr(HtmlAttribute.FormEncType, value);
            return (T)this;
        }

        /// <summary>
        /// Set the 'formmethod' attribute.
        /// </summary>
        /// <param name="value">The value of the attribute.</param>
        public T Method(FormMethod value)
        {
            Attr(HtmlAttribute.FormMethod, value.ToString().ToLower());
            return (T)this;
        }

        /// <summary>
        /// Set the 'formtarget' attribute.
        /// </summary>
        /// <param name="value">The value of the attribute.</param>
        public T Target(string value)
        {
            Attr(HtmlAttribute.FormTarget, value);
            return (T)this;
        }

        /// <summary>
        /// Add or remove the 'formnovalidate' attribute. 
        /// </summary>
        /// <param name="value">Whether to add or remove the attribute.</param>
        public virtual T FormNoValidate(bool value)
        {
            if (value)
            {
                Attr(HtmlAttribute.FormNoValidate, HtmlAttribute.FormNoValidate);
            }
            else
            {
                ((IElement)this).RemoveAttr(HtmlAttribute.FormNoValidate);
            }
            return (T)this;
        }

        /// <summary>
        /// Set the 'formtarget' attribute to '_blank.'
        /// </summary>
        public T TargetBlank()
        {
            return Target("_blank");
        }

        /// <summary>
        /// Set the 'formtarget' attribute to '_top.'
        /// </summary>
        public T TargetTop()
        {
            return Target("_top");
        }

        /// <summary>
        /// Set the 'formtarget' attribute to '_self.'
        /// </summary>
        public T TargetSelf()
        {
            return Target("_self");
        }
        
        /// <summary>
        /// Set the 'formtarget' attribute to '_parent.'
        /// </summary>
        public T TargetParent()
        {
            return Target("_parent");
        }
    }
}