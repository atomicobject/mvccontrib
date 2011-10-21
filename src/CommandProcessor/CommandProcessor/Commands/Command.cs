namespace MvcContrib.CommandProcessor.Commands
{
	public abstract class Command<T> : ICommandMessageHandler
	{
		#region ICommandMessageHandler Members

		public ReturnValue Execute(object commandMessage)
		{
			return Execute((T) commandMessage);
		}

		#endregion

		protected abstract ReturnValue Execute(T commandMessage);
	}

	public interface ICommandMessageHandler : ICommandMessageHandler<object, ReturnValue> {}

	public interface ICommandMessageHandler<TCommand, TResult>
	{
		TResult Execute(TCommand commandMessage);
	}
}