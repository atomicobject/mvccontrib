using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcContrib.Samples.UI.Models.DerivedTypeModelBinder
{
	public class AddressInfo : IContent
	{
		public string Address { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string PostalCode { get; set; }
	}
}