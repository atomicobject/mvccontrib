using System;
using System.Globalization;
using System.Web.Mvc;
using MvcContrib.Binders;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.Binders
{
    [TestFixture]
    public class TypeStampOperatorTests
    {
        [Test]
        public void TypeStampIsFoundOnPageTest()
        {
            var valueProvider = MockRepository.GenerateMock<IValueProvider>();
            var propertyNameProvider = MockRepository.GenerateMock<IPropertyNameProvider>();

            propertyNameProvider.Expect(e => e.CreatePropertyName("test", DerivedTypeModelBinderCache.TypeStampFieldName)).Return("foo");
            valueProvider.Expect(b => b.ContainsPrefix("foo"))
                .Return(true);
            valueProvider.Expect(e => e.GetValue("foo")).Return(new ValueProviderResult(new[] { "bar" }, "foo",
                                                                                        CultureInfo.InvariantCulture));


            var typeStampOperator = new TypeStampOperator();

            Assert.That(typeStampOperator.DetectTypeStamp(
                new ModelBindingContext
                {
                    ModelMetadata =
                        new ModelMetadata(new EmptyModelMetadataProvider(), typeof(DerivedTypeModelBinder),
                                          null, typeof(DerivedTypeModelBinderTests), "propertyName"
                        ),
                    ModelName = "test",
                    ValueProvider = valueProvider
                }, propertyNameProvider), Is.EqualTo("bar"));
        }


        [Test]
        public void TypeStampIsNotFoundOnPageTest()
        {
            var valueProvider = MockRepository.GenerateMock<IValueProvider>();
            var propertyNameProvider = MockRepository.GenerateMock<IPropertyNameProvider>();

			propertyNameProvider.Expect(e => e.CreatePropertyName("test", DerivedTypeModelBinderCache.TypeStampFieldName)).Return("foo");
            valueProvider.Expect(b => b.ContainsPrefix("foo"))
                .Return(false);

            var typeStampOperator = new TypeStampOperator();

            Assert.That(typeStampOperator.DetectTypeStamp(
                new ModelBindingContext
                {
                    ModelMetadata =
                        new ModelMetadata(new EmptyModelMetadataProvider(), typeof(DerivedTypeModelBinder),
                                          null, typeof(DerivedTypeModelBinderTests), "propertyName"
                        ),
                    ModelName = "test",
                    ValueProvider = valueProvider
                }, propertyNameProvider), Is.EqualTo(string.Empty));
        }

        [Test]
        public void TypeStampFormattedReturnIsInvalid()
        {
            var valueProvider = MockRepository.GenerateMock<IValueProvider>();
            var propertyNameProvider = MockRepository.GenerateMock<IPropertyNameProvider>();

			propertyNameProvider.Expect(e => e.CreatePropertyName("test", DerivedTypeModelBinderCache.TypeStampFieldName)).Return("foo");
            valueProvider.Expect(b => b.ContainsPrefix("foo"))
                .Return(true);
            valueProvider.Expect(e => e.GetValue("foo")).Return(new ValueProviderResult(new DerivedTypeModelBinder(), "foo",
                                                                                        CultureInfo.InvariantCulture));

            var typeStampOperator = new TypeStampOperator();

            Assert.Throws<InvalidOperationException>(() => typeStampOperator.DetectTypeStamp(
                new ModelBindingContext
                {
                    ModelMetadata =
                        new ModelMetadata(new EmptyModelMetadataProvider(), typeof(DerivedTypeModelBinder),
                                          null, typeof(DerivedTypeModelBinderTests), "propertyName"
                        ),
                    ModelName = "test",
                    ValueProvider = valueProvider
                }, propertyNameProvider));
        }

    }
}
