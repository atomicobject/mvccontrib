using MvcContrib.FluentHtml;

namespace MvcContrib.Samples.UI.Views
{
	/// <summary>
	/// This is a sample ModelViewPage implementation that illustrates how to add Behaviors.
	/// </summary>
	public class SampleFluentHtml5ViewPage<T> : ModelViewPage<T> where T : class
	{
        public SampleFluentHtml5ViewPage()
            : base(new RegularExpressionBehavior(), new RangeBehavior(), new RequiredAttributeBehavior())
		{
			
		}
	}
}