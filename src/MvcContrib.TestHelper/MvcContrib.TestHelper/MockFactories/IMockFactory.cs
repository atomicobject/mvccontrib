namespace MvcContrib.TestHelper.MockFactories
{
	public interface IMockFactory 
	{
		IMockProxy<T> DynamicMock<T>() where T : class;
	}
}