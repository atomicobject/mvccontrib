using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Mail;

namespace MvcContrib.Services
{
    /// <summary>
    /// Base class for email models. 
    /// 
    /// Derived classes will provide specific values to substitute placeholders in concrete emails.
    /// If nothing to substitute, this class can be used directly.
    /// </summary>
    public class EmailMetadata
    {
        public MailAddress From { get; set; }
        public MailAddress ReplyTo { get; set; }
        public List<MailAddress> To { get; private set; }
        public List<MailAddress> Cc { get; private set; }
        public List<MailAddress> Bcc { get; private set; }
        public bool IsHtmlEmail { get; set; }
        public NameValueCollection Headers { get; private set; }

        /// <summary>
        /// Initializes an empty instance of EmailMetadata.
        /// </summary>
        public EmailMetadata()
        {
            To = new List<MailAddress>();
            Cc = new List<MailAddress>();
            Bcc = new List<MailAddress>();
            Headers = new NameValueCollection();
            IsHtmlEmail = true;
        }

        /// <summary>
        /// Initializes a new instance of the EmailMetadata class by using 
        /// the specified "from" and "to" email addresses.
        /// </summary>
        /// <param name="from">A mail address of the sender.</param>
        /// <param name="to">A mail address of the recipient.</param>
        public EmailMetadata(MailAddress from, MailAddress to)
            : this()
        {
            From = from;
            To.Add(to);
        }

        /// <summary>
        /// Initializes a new instance of the EmailMetadata class by using the specified 
        /// "from" and "to" email addresses.
        /// </summary>
        /// <param name="from">A string that contains email address of the sender.</param>
        /// <param name="to">A string that contains email address of the recipient.</param>
        public EmailMetadata(string from, string to)
            : this()
        {
            From = new MailAddress(from);
            To.Add(new MailAddress(to));
        }
    }
}
