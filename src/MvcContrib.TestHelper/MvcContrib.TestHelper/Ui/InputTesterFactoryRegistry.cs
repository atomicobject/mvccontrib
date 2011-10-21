using System;
using System.Collections.Generic;

namespace MvcContrib.TestHelper.Ui
{
    public class InputTesterFactoryRegistry
    {
    	public InputTesterFactoryRegistry()
    	{
    		InputTesterFactories = new List<IInputTesterFactory>();
			MultipleInputTesterFactories = new List<IMultipleInputTesterFactory>();

			InputTesterFactories.Add(new TextInputTesterFactory());
    	}

    	public IList<IInputTesterFactory> InputTesterFactories { get; private set; }
    	public IList<IMultipleInputTesterFactory> MultipleInputTesterFactories { get; private set; }
    }
}