using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using MvcContrib.PortableAreas;
using MvcContrib.UI.InputBuilder.Conventions;
using MvcContrib.UI.InputBuilder.InputSpecification;
using MvcContrib.UI.InputBuilder.ViewEngine;

namespace MvcContrib.UI.InputBuilder
{
	public class InputBuilder
	{
		private static Func<IList<IPropertyViewModelFactory>> _propertyConventionProvider = () => new DefaultPropertyConventionsFactory();
		private static Func<IList<ITypeViewModelFactory>> _typeConventionProvider = () => new DefaultTypeConventionsFactory();
		public static Action<VirtualPathProvider> RegisterPathProvider = HostingEnvironment.RegisterVirtualPathProvider;

		public static IList<IPropertyViewModelFactory> Conventions
		{
			get { return _propertyConventionProvider(); }
		}

		public static IList<ITypeViewModelFactory> TypeConventions
		{
			get { return _typeConventionProvider(); }
		}

		public static void BootStrap()
		{
			if (!ViewEngines.Engines.Any(engine => engine.GetType().Equals(typeof(InputBuilderViewEngine))))
			{
				VirtualPathProvider pathProvider = new AssemblyResourceProvider();

				RegisterPathProvider(pathProvider);

				var resourceStore = new AssemblyResourceStore(typeof(PortableAreaRegistration), "/areas", typeof(PortableAreaRegistration).Namespace);
				AssemblyResourceManager.RegisterAreaResources(resourceStore);

				ViewEngines.Engines.Add(new InputBuilderViewEngine(new string[0]));
			}
		}

		public static void SetPropertyConvention(Func<IList<IPropertyViewModelFactory>> conventionProvider)
		{
			_propertyConventionProvider = conventionProvider;
		}
		public static void SetTypeConventions(Func<IList<ITypeViewModelFactory>> conventionProvider)
		{
			_typeConventionProvider = conventionProvider;
		}
	}
}