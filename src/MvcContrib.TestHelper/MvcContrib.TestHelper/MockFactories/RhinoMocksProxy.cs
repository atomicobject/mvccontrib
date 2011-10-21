using System;
using System.Linq.Expressions;
using System.Reflection;

namespace MvcContrib.TestHelper.MockFactories
{
	/// <summary>
	/// Proxy for a mock created by Rhino Mocks. 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal class RhinoMocksProxy<T> : IMockProxy<T>
	{
		private static readonly Type _setupResult;
		private static readonly MethodInfo _forOpenMethod;

		private readonly T _dynamicMock;
		private readonly object _mocksRepository;

		/// <summary>
		/// Grabs references to static types and generic methods to speed things up later. 
		/// </summary>
		static RhinoMocksProxy()
		{
			if (RhinoMocksFactory.RhinoMocks != null)
			{
				_setupResult = RhinoMocksFactory.RhinoMocks.GetType("Rhino.Mocks.SetupResult");
				_forOpenMethod = _setupResult.GetMethod("For");
			}
		}

		/// <summary>
		/// Creates a proxy for the specified dynamic mock that was created
		/// from the specified mock repository. 
		/// </summary>
		/// <param name="dynamicMock"></param>
		/// <param name="mockRepository"></param>
		public RhinoMocksProxy(T dynamicMock, object mockRepository)
		{
			_dynamicMock = dynamicMock;
			_mocksRepository = mockRepository;
		}

		/// <summary>
		/// Changes the mock mode to "Replay" and gets the underlying mock object.
		/// </summary>
		public T Object
		{
			get
			{
				_mocksRepository.GetType().GetMethod("Replay").Invoke(_mocksRepository, new object[] {_dynamicMock});
				return _dynamicMock;
			}
		}

		private void SetupMockBehavior<TResult>(Expression<Func<T, TResult>> expression, string behaviorName, object[] behaviorArguments)
		{
			var setupFor = _forOpenMethod.MakeGenericMethod(typeof(TResult));

			var methodOptions = setupFor.Invoke(_setupResult, new object[] { expression.Compile().Invoke(_dynamicMock) });

			var behavior = methodOptions.GetType().GetMethod(behaviorName);

			behavior.Invoke(methodOptions, behaviorArguments);
		}

		/// <summary>
		/// Equivalent to SetupResult.For{TResult}().Return()
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="expression"></param>
		/// <param name="result"></param>
		public void ReturnFor<TResult>(Expression<Func<T, TResult>> expression, TResult result)
		{
			SetupMockBehavior(expression, "Return", new object[] {result});
		}

		/// <summary>
		/// Equivalent to SetupResult.For{TResult}().Do().
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="expression"></param>
		/// <param name="callback"></param>
		public void CallbackFor<TResult>(Expression<Func<T, TResult>> expression, Func<TResult> callback)
		{
			SetupMockBehavior(expression, "Do", new object[] { callback });
		}

		/// <summary>
		/// Equivalent to SetupResult.For{TResult}().PropertyBehavior().
		/// </summary>
		/// <typeparam name="TProperty"></typeparam>
		/// <param name="expression"></param>
		public void SetupProperty<TProperty>(Expression<Func<T, TProperty>> expression)
		{
			SetupMockBehavior(expression, "PropertyBehavior", new object[0]);
		}
	}
}