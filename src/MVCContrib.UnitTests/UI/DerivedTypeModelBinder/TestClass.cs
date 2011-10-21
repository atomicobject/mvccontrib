namespace MvcContrib.UnitTests.UI.DerivedTypeModelBinder
{
	public interface ITestClass {
		string Name { get; set; }
	}

	public class TestClass : ITestClass
	{
		public string Name { get; set; }
	}

}
