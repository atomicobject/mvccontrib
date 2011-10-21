using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.TestHelper.MockFactories;

namespace MvcContrib.TestHelper
{
	/// <summary>
	/// This is primary class used to create controllers.
	/// After initializing, call InitializeController to create a controller with proper environment elements.
	/// Exposed properties such as Form, QueryString, and HttpContext allow access to text environment.
	/// RenderViewData and RedirectToActionData record those methods
	/// </summary>
	public class TestControllerBuilder
	{
		protected TempDataDictionary _tempData;

		/// <summary>
		/// Initializes a new instance of the <see cref="TestControllerBuilder"/> class
		/// using the first available mocking framework available.
		/// </summary>
		/// <remarks>
		/// See FirstAvailableMockFactory for a description of the mocking framework
		/// selection strategy. 
		/// </remarks>
		public TestControllerBuilder()
			: this(new FirstAvailableMockFactory())
		{
			
		}

	    /// <summary>
		/// Initializes a new instance of the <see cref="TestControllerBuilder"/> class using
		/// the specified mock factory to create any mock objects. 
		/// </summary>
		public TestControllerBuilder(IMockFactory mockFactory)
		{
			AppRelativeCurrentExecutionFilePath = "~/";
			ApplicationPath = "/";
			PathInfo = "";

			RouteData = new RouteData();
	    	Session = new MockSession();
			TempDataDictionary = new TempDataDictionary();
			QueryString = new NameValueCollection();
			Form = new NameValueCollection();
			Files = new WriteableHttpFileCollection();

	    	Setup(mockFactory);
		}

		private void Setup(IMockFactory factory)
		{
			var httpContext = factory.DynamicMock<HttpContextBase>();

			var request = factory.DynamicMock<HttpRequestBase>();
			var response = factory.DynamicMock<HttpResponseBase>();
			var server = factory.DynamicMock<HttpServerUtilityBase>();
			var cache = HttpRuntime.Cache;

			httpContext.ReturnFor(c => c.Session, Session);
			httpContext.ReturnFor(c => c.Cache, cache);
			httpContext.SetupProperty(c => c.User);

			request.ReturnFor(r => r.QueryString, QueryString);
			request.ReturnFor(r => r.Form, Form);
			request.ReturnFor(r => r.Files, (HttpFileCollectionBase)Files);
			request.ReturnFor(r => r.ServerVariables, new NameValueCollection());
			request.CallbackFor(r => r.AcceptTypes, () => AcceptTypes);
			request.CallbackFor(r => r.Params, () => new NameValueCollection { QueryString, Form });
			request.CallbackFor(r => r.AppRelativeCurrentExecutionFilePath, () => AppRelativeCurrentExecutionFilePath);
			request.CallbackFor(r => r.ApplicationPath, () => ApplicationPath);
			request.CallbackFor(r => r.PathInfo, () => PathInfo);
			request.CallbackFor(r => r.RawUrl, () => RawUrl);
			response.SetupProperty(r => r.Status);

			httpContext.ReturnFor(c => c.Request, request.Object);
			httpContext.ReturnFor(c => c.Response, response.Object);
			httpContext.ReturnFor(c => c.Server, server.Object);
	
			HttpContext = httpContext.Object;
		}

		/// <summary>
		/// Gets the HttpContext that built controllers will have set internally when created with InitializeController
		/// </summary>
		/// <value>The HTTPContext</value>
		public HttpContextBase HttpContext { get; protected internal set; }

		/// <summary>
		/// Gets the HttpPostedFiles that controllers will have set internally when created with InitializeController
		/// </summary>
		/// <value>The HttpFileCollection Files</value>
		public IWriteableHttpFileCollection Files { get; protected internal set; }

		/// <summary>
		/// Gets the Form data that built controllers will have set internally when created with InitializeController
		/// </summary>
		/// <value>The NameValueCollection Form</value>
		public NameValueCollection Form { get; protected internal set; }

		/// <summary>
		/// Gets the QueryString that built controllers will have set internally when created with InitializeController
		/// </summary>
		/// <value>The NameValueCollection QueryString</value>
		public NameValueCollection QueryString { get; protected internal set; }

        /// <summary>
        /// Gets or sets the AcceptTypes property of Request that built controllers will have set internally when created with InitializeController
        /// </summary>
        public string[] AcceptTypes { get; set; }

		/// <summary>
		/// Gets the Session that built controllers will have set internally when created with InitializeController
		/// </summary>
		/// <value>The IHttpSessionState Session</value>
		public HttpSessionStateBase Session { get; protected internal set; }

		/// <summary>
		/// Gets the TempDataDictionary that built controllers will have set internally when created with InitializeController
		/// </summary>
		/// <value>The TempDataDictionary</value>
		public TempDataDictionary TempDataDictionary { get; protected internal set; }

		/// <summary>
		/// Gets or sets the RouteData that built controllers will have set internally when created with InitializeController
		/// </summary>
		/// <value>The RouteData</value>
		public RouteData RouteData { get; set; }

		/// <summary>
		/// Gets or sets the AppRelativeCurrentExecutionFilePath that built controllers will have set internally when created with InitializeController
		/// </summary>
		/// <value>The RouteData</value>
		public string AppRelativeCurrentExecutionFilePath { get; set; }

		/// <summary>
		/// Gets or sets the AppRelativeCurrentExecutionFilePath string that built controllers will have set internally when created with InitializeController
		/// </summary>
		/// <value>The ApplicationPath string</value>
		public string ApplicationPath { get; set; }

		/// <summary>
		/// Gets or sets the PathInfo string that built controllers will have set internally when created with InitializeController
		/// </summary>
		/// <value>The PathInfo string</value>
		public string PathInfo { get; set; }

		/// <summary>
		/// Gets or sets the RawUrl string that built controllers will have set internally when created with InitializeController
		/// </summary>
		/// <value>The RawUrl string</value>
		public string RawUrl { get; set; }

		/// <summary>
		/// Creates the controller with proper environment variables setup. 
		/// </summary>
		/// <param name="controller">The controller to initialize</param>
		public void InitializeController(Controller controller)
		{
			var controllerContext = new ControllerContext(HttpContext, RouteData, controller);
			controller.ControllerContext = controllerContext;
			controller.TempData = TempDataDictionary;
			controller.Url = new UrlHelper(controllerContext.RequestContext);
		}


		/// <summary>
		/// Creates the controller with proper environment variables setup. 
		/// </summary>
		/// <typeparam name="T">The type of controller to create</typeparam>
		/// <param name="constructorArgs">Arguments to pass to the constructor for the controller</param>
		/// <returns>A new controller of the specified type</returns>
		public T CreateController<T>(params object[] constructorArgs) where T : Controller
		{
			var controller = (Controller)Activator.CreateInstance(typeof(T), constructorArgs);
			InitializeController(controller);
			return controller as T;
		}

		/// <summary>
		/// Creates the controller with proper environment variables setup, using IoC for arguments
		/// </summary>
		/// <typeparam name="T">The type of controller to create</typeparam>
		/// <returns>A new controller of the specified type</returns>
		public T CreateIoCController<T>() where T : Controller
		{
			var controller = DependencyResolver.Current.GetService<T>();
			InitializeController(controller);
			return controller;
		}
	}
}
