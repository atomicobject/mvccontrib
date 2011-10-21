using MvcContrib.FluentHtml.Elements;

namespace MvcContrib.FluentHtml.Behaviors
{
	public abstract class ThreadSafeMemberBehavior : IBehavior<IMemberElement>
	{
		private static readonly object objLock = new object();

		public void Execute(IMemberElement element)
		{
			lock (objLock)
			{
				DoExecute(element);
			}
		}

		protected abstract void DoExecute(IMemberElement element);
	}
}