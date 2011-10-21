using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MvcContrib.TestHelper.MockFactories
{
	/// <summary>
	/// Runtime proxy for a Moq proxy. 
	/// </summary>
	/// <typeparam name="T"></typeparam>
    internal class MoqProxy<T> : IMockProxy<T>
    {
        private readonly Type _mockType;
        private readonly PropertyInfo _objectProperty;
        private readonly object _instance;

		/// <summary>
		/// Gets the object. 
		/// </summary>
        public T Object 
        { 
            get
            {
                return (T)_objectProperty.GetValue(_instance, null);

            }
        }

		/// <summary>
		/// Creates a new proxy. 
		/// </summary>
		/// <param name="mockType"></param>
        public MoqProxy(Type mockType)
        {
            _mockType = mockType;
            _instance = Activator.CreateInstance(_mockType);
            _objectProperty = mockType.GetProperty("Object", _mockType);
        }

        private MethodInfo GetSetupMethod<TResult>() 
        {
            var openSetupMethod = _mockType.GetMethods().First(m => m.IsGenericMethod && m.Name == "Setup");
            return openSetupMethod.MakeGenericMethod(typeof(TResult));
        }

		/// <summary>
		/// Sets up the specified return value for the specified method call or property. 
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="expression"></param>
		/// <param name="result"></param>
        public void ReturnFor<TResult>(Expression<Func<T, TResult>> expression, TResult result)
        {
            var setupMethod = GetSetupMethod<TResult>();
            var setup = setupMethod.Invoke(_instance, new object[] { expression });
            var returnsMethod = setup.GetType().GetMethod("Returns", new [] {typeof(TResult)});
            returnsMethod.Invoke(setup, new object[] { result});
        }

		/// <summary>
		/// Sets up a callback function for the specified method call or property.
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="expression"></param>
		/// <param name="callback"></param>
        public void CallbackFor<TResult>(Expression<Func<T, TResult>> expression, Func<TResult> callback)
        {
            var setupMethod = GetSetupMethod<TResult>();
            var setup = setupMethod.Invoke(_instance, new object[] { expression });
            var returnsMethod = setup.GetType().GetMethod("Returns", new[] { typeof(Func<TResult>) });
            returnsMethod.Invoke(setup, new object[] {callback});
        }

		/// <summary>
		/// Sets up normal get/set property behavior. 
		/// </summary>
		/// <typeparam name="TProperty"></typeparam>
		/// <param name="expression"></param>
        public void SetupProperty<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            var openSetupMethod = _mockType.GetMethods().First(m => m.Name == "SetupProperty" && m.GetParameters().Length == 1);
            var setupMethod = openSetupMethod.MakeGenericMethod(typeof(TProperty));
            setupMethod.Invoke(_instance, new object[] {expression});
        }
    }
}