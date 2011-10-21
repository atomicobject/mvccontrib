using System.Linq.Expressions;

namespace MvcContrib.CommandProcessor
{
	public class ErrorMessage
	{
		public ErrorMessage(string messageText, LambdaExpression uiAttribute, LambdaExpression incorrectAttribute,
		                    LambdaExpression compareToAttribute)
		{
			UIAttribute = uiAttribute;
			IncorrectAttribute = incorrectAttribute;
			ComparedAttribute = compareToAttribute;
			MessageText = messageText;
		}

		public ErrorMessage(string messageText, LambdaExpression uiAttribute, LambdaExpression incorrectAttribute)
			: this(messageText, uiAttribute, incorrectAttribute, null) {}

		public LambdaExpression IncorrectAttribute { get; private set; }
		public LambdaExpression ComparedAttribute { get; private set; }
		public LambdaExpression UIAttribute { get; private set; }
		public string MessageText { get; private set; }
	}
}