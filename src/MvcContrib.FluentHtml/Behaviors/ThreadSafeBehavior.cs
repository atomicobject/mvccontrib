using MvcContrib.FluentHtml.Elements;

namespace MvcContrib.FluentHtml.Behaviors
{
	public abstract class ThreadSafeBehavior : IBehavior<IElement>
	{
		private static readonly object objLock = new object();

		public void Execute(IElement element)
		{
			lock (objLock)
			{
				DoExecute(element);
			}
		}

		protected abstract void DoExecute(IElement element);
	}
}