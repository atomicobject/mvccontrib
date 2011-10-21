using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MvcContrib.CommandProcessor
{
	public class ExecutionResult
	{
		private readonly List<ErrorMessage> _messages = new List<ErrorMessage>();
		private readonly GenericItemDictionary _returnItems = new GenericItemDictionary();

		public ExecutionResult() {}

		public ExecutionResult(GenericItemDictionary returnItems)
		{
			_returnItems = returnItems;
		}

		public bool Successful
		{
			get { return _messages.Count == 0; }
		}

		public IEnumerable<ErrorMessage> Messages
		{
			get { return _messages; }
		}

		public GenericItemDictionary ReturnItems
		{
			get { return _returnItems; }
		}

		public ExecutionResult AddMessage(string message, LambdaExpression attributeExpression, LambdaExpression uiExpression)
		{
			_messages.Add(new ErrorMessage(message, uiExpression, attributeExpression));
			return this;
		}

		public ExecutionResult AddMessage(string message, LambdaExpression attributeExpression, LambdaExpression uiExpression, LambdaExpression compareToExpression)
		{
			_messages.Add(new ErrorMessage(message, uiExpression, attributeExpression, compareToExpression));
			return this;
		}

		public void MergeWith(params ExecutionResult[] otherResults)
		{
			if (otherResults == null || otherResults.Length == 0) return;

			foreach (ExecutionResult otherResult in otherResults)
			{
				foreach (ErrorMessage message in otherResult.Messages)
				{
					_messages.Add(message);
				}

				foreach (KeyValuePair<Type, object> returnItem in otherResult.ReturnItems)
				{
					_returnItems.Add(returnItem.Key, returnItem.Value);
				}
			}
		}

		public void Merge(ReturnValue returnObject)
		{
			if (returnObject != null && returnObject.Type != null)
			{
				ReturnItems.Add(returnObject.Type, returnObject.Value);
			}
		}
	}
}