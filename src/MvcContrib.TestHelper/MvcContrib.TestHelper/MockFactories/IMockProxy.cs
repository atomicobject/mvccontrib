using System;
using System.Linq.Expressions;

namespace MvcContrib.TestHelper.MockFactories
{
    public interface IMockProxy<T> 
    {
        T Object { get; }
        void ReturnFor<TResult>(Expression<Func<T, TResult>> expression, TResult result);
        void CallbackFor<TResult>(Expression<Func<T, TResult>> expression, Func<TResult> callback);
        void SetupProperty<TProperty>(Expression<Func<T, TProperty>> expression);
    }
}