using System;
using System.Collections.Generic;
using MvcContrib.Attributes;
using MvcContrib.Binders;
using MvcContrib.UnitTests.UI.DerivedTypeModelBinder;
using NUnit.Framework;

namespace MvcContrib.UnitTests.Binders
{
    [TestFixture]
    [DerivedTypeBinderAware(typeof(int))]
    public class DerivedTypeModelBinderCacheTests
    {
		[Test]
		public void validate_declarative_registration_of_derived_types()
		{
			DerivedTypeModelBinderCache.RegisterDerivedTypes(typeof(DerivedTypeModelBinderCacheTests),
															 new[] { typeof(string) });

			Assert.That(DerivedTypeModelBinderCache.GetTypeName(typeof(string)), Is.Not.Empty);

			DerivedTypeModelBinderCache.Reset();

			// next, let's validate that the cache was cleared by reset
			Assert.Throws<KeyNotFoundException>(() => DerivedTypeModelBinderCache.GetTypeName(typeof(string)));
		}

		[Test]
		public void verify_default_type_stamp_field_name()
		{
			Assert.That(DerivedTypeModelBinderCache.TypeStampFieldName, Is.EqualTo("_xTypeStampx_"));
		}

		[Test]
		public void GetTypeName_verify_encryption()
		{
			DerivedTypeModelBinderCache.Reset();
			DerivedTypeModelBinderCache.RegisterDerivedTypes(typeof(ITestClass), new[] {typeof(TestClass)});

			var typeName = DerivedTypeModelBinderCache.GetTypeName(typeof(TestClass));

			Assert.That(typeName, Is.StringStarting("VFxCac+"));
		}

		[Test]
		public void GetTypeName_verify_error_on_unrecognized_type()
		{
			DerivedTypeModelBinderCache.Reset();
			DerivedTypeModelBinderCache.RegisterDerivedTypes(typeof(ITestClass), new[] { typeof(TestClass) });

			Assert.Throws<KeyNotFoundException>(() => DerivedTypeModelBinderCache.GetTypeName(typeof(DerivedTypeModelBinderCacheTests)));
		}

		[Test]
		public void TypeStamp_verify_encryption_set_and_reset_behaviors()
		{
			DerivedTypeModelBinderCache.Reset();
			DerivedTypeModelBinderCache.RegisterDerivedTypes(typeof(ITestClass), new[] { typeof(TestClass) });

			var standardTypeStamp = DerivedTypeModelBinderCache.GetTypeName(typeof(TestClass));


			DerivedTypeModelBinderCache.Reset();
			DerivedTypeModelBinderCache.SetTypeStampSaltValue(Guid.NewGuid());
			DerivedTypeModelBinderCache.RegisterDerivedTypes(typeof(ITestClass), new[] { typeof(TestClass) });

			var alteredTypeStamp = DerivedTypeModelBinderCache.GetTypeName(typeof(TestClass));


			DerivedTypeModelBinderCache.Reset();
			DerivedTypeModelBinderCache.RegisterDerivedTypes(typeof(ITestClass), new[] { typeof(TestClass) });

			var resetTypeStamp = DerivedTypeModelBinderCache.GetTypeName(typeof(TestClass));


			Assert.That(standardTypeStamp, Is.Not.EqualTo(alteredTypeStamp));
			Assert.That(standardTypeStamp, Is.EqualTo(resetTypeStamp));
		}
    }
}
