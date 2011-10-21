using System;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;

namespace MvcContrib.Services
{
	/// <remarks>
	/// Inspired by Castle's EmailTemplateService.
	/// </remarks>
	public class EmailTemplateService : IEmailTemplateService
	{
		private readonly IViewStreamReader _viewReader;

		public EmailTemplateService()
		{
			_viewReader = new ViewStreamReader();
		}

		public EmailTemplateService(IViewStreamReader viewReader)
		{
			_viewReader = viewReader;
		}

		public MailMessage RenderMessage(string viewName, EmailMetadata metadata, ControllerContext context)
		{
			var details = GetEmailDetails(viewName, metadata, context);

			var result = new MailMessage
			{From = metadata.From, Subject = details.Subject, Body = details.Body, IsBodyHtml = metadata.IsHtmlEmail};
			metadata.To.ForEach(x => result.To.Add(x));
			metadata.Cc.ForEach(x => result.CC.Add(x));
			metadata.Bcc.ForEach(x => result.Bcc.Add(x));

			return result;
		}

		private EmailDetails GetEmailDetails(string viewName, EmailMetadata metadata, ControllerContext context)
		{
			using(var stream = _viewReader.GetViewStream(viewName, metadata, context))
			{
				string subject = "";
				string body = "";

				using(var reader = new StreamReader(stream))
				{
					bool subjectProcessed = false;
					string line;
					while((line = reader.ReadLine()) != null)
					{
						if(!subjectProcessed)
						{
							if(string.IsNullOrEmpty(line))
							{
								continue;
							}

							subject = line;
							subjectProcessed = true;
							continue;
						}
						body += line;
					}
				}

				return new EmailDetails {Body = body, Subject = subject};
			}
		}

		/// <summary>
		/// The only information that comes from the email template is subject and body. 
		/// 
		/// Everything else ("to", "from", etc) is known when we call the service. 
		/// But subject/body are localizable and can contain placeholders - so they need 
		/// to get fetched from the email template view. 
		/// </summary>
		private class EmailDetails
		{
			public string Subject { get; set; }
			public string Body { get; set; }
		}

		private class ViewStreamReader : IViewStreamReader
		{
			public Stream GetViewStream(string viewName, object model, ControllerContext context)
			{
				var view = ViewEngines.Engines.FindPartialView(context, viewName).View;
				if(view == null)
				{
					throw new InvalidOperationException(string.Format("Could not find a view named '{0}'", viewName));
				}

				var sb = new StringBuilder();
				using(var writer = new StringWriter(sb))
				{
					var viewContext = new ViewContext(context, view, new ViewDataDictionary(model), new TempDataDictionary(), writer);
					view.Render(viewContext, writer);

					writer.Flush();
				}
				return new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));
			}
		}
	}
}