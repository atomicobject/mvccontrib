using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MvcContrib.TestHelper.Ui
{
	public class InputForm<TFormType>
	{
		private readonly IBrowserDriver _browserDriver;
		private readonly InputTesterFactoryRegistry _factories;
		private readonly LinkedList<IInputTester> _inputTesters = new LinkedList<IInputTester>();

		public InputForm(IBrowserDriver browserDriver, InputTesterFactoryRegistry factories)
		{
			_browserDriver = browserDriver;
			_factories = factories;
			SubmitName = "Submit";
		}

		public string SubmitName { get; set; }

		public InputForm<TFormType> Input(Expression<Func<TFormType, object>> expression, string text)
		{
			PropertyInfo propertyInfo = ReflectionHelper.FindPropertyFromExpression(expression);

			IInputTesterFactory factory = GetInputForProperty(propertyInfo);

			return Input(factory.Create(expression, text));
		}

		public InputForm<TFormType> Input(Expression<Func<TFormType, object>> expression, params string[] text)
		{
			PropertyInfo propertyInfo = ReflectionHelper.FindPropertyFromExpression(expression);

			IMultipleInputTesterFactory factory = GetInputForMultipleProperty(propertyInfo);

			return Input(factory.Create(expression, text));
		}

		private IInputTesterFactory GetInputForProperty(PropertyInfo propertyInfo)
		{
			return _factories.InputTesterFactories.First(factory => factory.CanHandle(propertyInfo));
		}

		private IMultipleInputTesterFactory GetInputForMultipleProperty(PropertyInfo propertyInfo)
		{
			return _factories.MultipleInputTesterFactories.First(factory => factory.CanHandle(propertyInfo));
		}

		public InputForm<TFormType> Input(IInputTester tester)
		{
			_inputTesters.AddLast(tester);
			return this;
		}

		public IBrowserDriver Submit()
		{
			SetInputs();

			_browserDriver.ClickButton(SubmitName);
			return _browserDriver;
		}

		private void SetInputs()
		{
			_inputTesters.ForEach(x => x.SetInput(_browserDriver));
		}

		public IBrowserDriver BrowserDriver
		{
			get
			{
				return _browserDriver;
			}
		}

	}
}