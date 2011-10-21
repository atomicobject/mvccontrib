using System.Web.Mvc;

namespace MvcContrib.Binders
{
    public interface ITypeStampOperator
    {
        string DetectTypeStamp(ModelBindingContext bindingContext, IPropertyNameProvider propertyNameProvider);
    }
}
