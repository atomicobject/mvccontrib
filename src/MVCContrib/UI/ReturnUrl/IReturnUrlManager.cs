namespace MvcContrib.UI.ReturnUrl
{
	public interface IReturnUrlManager
	{
		string GetReturnUrl();
		bool HasReturnUrl();
	}
}