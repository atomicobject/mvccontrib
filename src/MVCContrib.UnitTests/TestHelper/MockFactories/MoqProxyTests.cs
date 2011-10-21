using System.Collections.Specialized;
using System.Text;
using System.Web;
using MvcContrib.TestHelper.MockFactories;
using NUnit.Framework;

namespace MvcContrib.UnitTests.TestHelper.MockFactories
{
	[TestFixture]
	public class MoqProxyTests
	{
		private MoqProxy<HttpRequestBase> _proxy;

		[SetUp]
		public void Prepare()
		{
			var factory = new MoqFactory();
			_proxy = (MoqProxy<HttpRequestBase>)factory.DynamicMock<HttpRequestBase>();
		}

		[Test]
		public void ReturnFor_Sets_Up_Return()
		{
			var queryString = new NameValueCollection();
			_proxy.ReturnFor(r => r.QueryString, queryString);

			_proxy.Object.QueryString.ShouldBeTheSameAs(queryString);
		}

		[Test]
		public void Callback_For_Is_Invoked()
		{
			var expected = new[] {"test"};

			_proxy.CallbackFor(r => r.AcceptTypes, () => expected);

			_proxy.Object.AcceptTypes.ShouldBeTheSameAs(expected);
		}

		[Test]
		public void SetupProperty_Sets_Up_Property_Behavior()
		{
			_proxy.SetupProperty(r => r.ContentEncoding);

			_proxy.Object.ContentEncoding = Encoding.ASCII;

			_proxy.Object.ContentEncoding.ShouldEqual(Encoding.ASCII);
		}
	}
}