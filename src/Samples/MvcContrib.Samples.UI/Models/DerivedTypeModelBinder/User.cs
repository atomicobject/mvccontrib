using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcContrib.Samples.UI.Models.DerivedTypeModelBinder
{
	public class User : IContent
	{
		public int UserId { get; set; }
		public string EmailId { get; set; }
		public string UserName { get; set; }
	}
}