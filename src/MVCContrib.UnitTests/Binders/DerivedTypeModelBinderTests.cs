using System;
using System.Web.Mvc;
using MvcContrib.Binders;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.Binders
{
    public class StubClass
    {}

    [TestFixture]
    public class DerivedTypeModelBinderTests
    {
        [Test]
        public void ValidateBehaviorWhenValidDerivedTypeIsFound()
        {
            DerivedTypeModelBinderCache.Reset();

            DerivedTypeModelBinderCache.RegisterDerivedTypes(typeof(DerivedTypeModelBinderTests),
                                                             new[] {typeof(StubClass)});

            var valueProvider = MockRepository.GenerateMock<IValueProvider>();
            var typeStampOperator = MockRepository.GenerateMock<ITypeStampOperator>();

            valueProvider.Expect(b => b.ContainsPrefix("test"))
                .Return(true);

            typeStampOperator.Expect(a => a.DetectTypeStamp(null, null)).IgnoreArguments().Return(DerivedTypeModelBinderCache.GetTypeName(typeof(StubClass)));

            var binder = new DerivedTypeModelBinder(typeStampOperator);

             var bindingContext = new ModelBindingContext
                             {
                                 ModelMetadata =
                                     new ModelMetadata(new EmptyModelMetadataProvider(), typeof(DerivedTypeModelBinder),
                                                       null, typeof(DerivedTypeModelBinderTests), "propertyName"
                                     ),
                                 ModelName = "test",
                                 ValueProvider = valueProvider
                             };


            object model = binder.BindModel(new ControllerContext(),bindingContext);

            Assert.That(model, Is.TypeOf(typeof(StubClass)));
            Assert.That(bindingContext.ModelType.FullName, Is.EqualTo(typeof(StubClass).FullName));

            DerivedTypeModelBinderCache.Reset();
        }


        [Test]
        public void VerifyThatTargetTypeIsCreatedWhenDerivedTypeIsNotFound()
        {
            var valueProvider = MockRepository.GenerateMock<IValueProvider>();
            var typeStampOperator = MockRepository.GenerateMock<ITypeStampOperator>();

            valueProvider.Expect(b => b.ContainsPrefix("test"))
                .Return(true);

            typeStampOperator.Expect(a => a.DetectTypeStamp(null, null)).IgnoreArguments().Return(null);

            var binder = new DerivedTypeModelBinder(typeStampOperator);

            object model = binder.BindModel(new ControllerContext(),
                             new ModelBindingContext
                             {
                                 ModelMetadata =
                                     new ModelMetadata(new EmptyModelMetadataProvider(), typeof(DerivedTypeModelBinder),
                                                       null, typeof(DerivedTypeModelBinderTests), "propertyName"
                                     ),
                                 ModelName = "test",
                                 ValueProvider = valueProvider
                             });

            Assert.That(model, Is.TypeOf(typeof(DerivedTypeModelBinderTests)));
        }

        [Test]
        public void ExpectExceptionOnTypeThatIsNotRegistered()
        {
            var valueProvider = MockRepository.GenerateMock<IValueProvider>();
            var typeStampOperator = MockRepository.GenerateMock<ITypeStampOperator>();

            valueProvider.Expect(b => b.ContainsPrefix("test"))
                .Return(true);

            typeStampOperator.Expect(a => a.DetectTypeStamp(null, null)).IgnoreArguments().Return("foo");

            var binder = new DerivedTypeModelBinder(typeStampOperator);

            Assert.Throws<InvalidOperationException>(() => binder.BindModel(new ControllerContext(),
                             new ModelBindingContext
                             {
                                 ModelMetadata =
                                     new ModelMetadata(new EmptyModelMetadataProvider(), typeof(DerivedTypeModelBinder),
                                                       null, typeof(DerivedTypeModelBinderTests), "propertyName"
                                     ),
                                 ModelName = "test",
                                 ValueProvider = valueProvider
                             }));
        }
    }
}
