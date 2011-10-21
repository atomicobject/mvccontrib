using System;
using System.Web.Mvc;

namespace MvcContrib.Binders
{
    public class TypeStampOperator : ITypeStampOperator
    {
        public string DetectTypeStamp(ModelBindingContext bindingContext, IPropertyNameProvider propertyNameProvider)
        {

			var propertyName = propertyNameProvider.CreatePropertyName(bindingContext.ModelName, DerivedTypeModelBinderCache.TypeStampFieldName);

            if (bindingContext.ValueProvider.ContainsPrefix(propertyName))
            {
                var value = bindingContext.ValueProvider.GetValue(propertyName);
                if (value.RawValue is String[])
                    return (value.RawValue as String[])[0];

                throw new InvalidOperationException(
                    string.Format("TypeStamp found for type {0} on path {1}, but format is invalid.",
                                  bindingContext.ModelType.Name, propertyName));
            }

            return string.Empty;
        }
    }
}
