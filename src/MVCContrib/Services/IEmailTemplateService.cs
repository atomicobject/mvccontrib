using System.Net.Mail;
using System.Web.Mvc;

namespace MvcContrib.Services
{
    /// <summary>
    /// Service to render MailMessages from MVC views.
    /// </summary>
    public interface IEmailTemplateService
    {
        /// <summary>
        /// Generates an instance of a MailMessage from the given view and metadata. 
        /// </summary>
        /// <param name="viewName">
        /// Name of the view that contain email subject and email body.
        /// The first line of the view is considered subject, everything else is body.
        /// </param>
        /// <param name="metadata">
        /// Mail addresses "to", "from", "cc" and other metadata about the email.
        /// </param>
        /// <param name="context">
        /// The context for view rendering.
        /// </param>
        /// <returns>Returns a MailMessage ready for sending out.</returns>
        MailMessage RenderMessage(string viewName, EmailMetadata metadata, ControllerContext context);
    }
}
