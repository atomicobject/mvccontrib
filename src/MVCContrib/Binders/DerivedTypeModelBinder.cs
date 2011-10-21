using System;
using System.Web.Mvc;

namespace MvcContrib.Binders
{
	/// <summary>
	/// This model binder extends the default model binder to detect alternate runtime types
	/// on a page and allow the binder adapt to derived types.
	/// </summary>
	public class DerivedTypeModelBinder : DefaultModelBinder, IPropertyNameProvider
	{
	    private readonly ITypeStampOperator _typeStampOperator;

        public DerivedTypeModelBinder() : this(null){}

        public DerivedTypeModelBinder(ITypeStampOperator typeStampOperator)
        {
            _typeStampOperator = typeStampOperator ?? new TypeStampOperator();
        }

		/// <summary>
		/// An override of CreateModel that focuses on detecting alternate types at runtime
		/// </summary>
		/// <param name="controllerContext">the controller context</param>
		/// <param name="bindingContext">the binding context</param>
		/// <param name="modelType">the target type to be instantiated by this method and rehydrated by 
		/// the default model binder</param>
		/// <returns>instance of the target model type</returns>
		protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
		{
			var instantiationType = DetectInstantiationType(controllerContext, bindingContext, modelType);

			if (instantiationType == modelType)
				return base.CreateModel(controllerContext, bindingContext, modelType);

			bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, instantiationType);
			// set up the binding context to acknowledge our derived concrete instance
			//bindingContext.ModelMetadata = new CustomModelMetadata(instantiationType, bindingContext.ModelMetadata);

			return Activator.CreateInstance(instantiationType);
		}

		protected Type DetectInstantiationType(ControllerContext controllerContext, ModelBindingContext bindingContext, Type typeToCreate)
		{
		    var typeValue = _typeStampOperator.DetectTypeStamp(bindingContext, this);

			if (String.IsNullOrEmpty(typeValue))
				return typeToCreate;

			var derivedType = DerivedTypeModelBinderCache.GetDerivedType(typeValue);

			if (derivedType != null)
				return derivedType;

			throw new InvalidOperationException(string.Format("unable to located identified type '{0}' as a variant of '{1}'", typeValue, typeToCreate.FullName));
		}


        #region IPropertyNameProvider Members

        public string CreatePropertyName(string prefix, string propertyName)
        {
            return CreateSubPropertyName(prefix, propertyName);
        }

        #endregion
    }

}


