using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;

namespace MvcContrib.UnitTests.FluentHtml.Fakes
{
	public class FakeViewDataContainer : IViewDataContainer
	{
		private ViewDataDictionary viewData = new ViewDataDictionary();

		public ViewDataDictionary ViewData
		{
			get { return viewData; }
			set { viewData = value; }
		}
	}

    public class FakeBehavioralViewDataContainer : FakeViewDataContainer, IBehaviorsContainer
    {
        public FakeBehavioralViewDataContainer(IEnumerable<IBehaviorMarker> behaviors)
        {
            Behaviors = behaviors; 
        }

        public IEnumerable<IBehaviorMarker> Behaviors { get; private set; }
    }

    public class FakeBehavior : IBehavior<IElement> 
    {
        public bool Executed { get; set; }

        public void Execute(IElement element)
        {
            Executed = true;
        }
    }
}
