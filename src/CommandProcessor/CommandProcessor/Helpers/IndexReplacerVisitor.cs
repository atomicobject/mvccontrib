using System.Linq.Expressions;

namespace MvcContrib.CommandProcessor.Helpers
{
	public class IndexReplacerVisitor : ExpressionVisitor
	{
		private readonly int _index;

		public IndexReplacerVisitor(int index)
		{
			_index = index;
		}

		protected override Expression VisitConstant(ConstantExpression c)
		{
			if(c.Value is int && (((int)c.Value) == int.MaxValue))
			{
				return Expression.Constant(_index);
			}

			return Expression.Constant(c.Value);
		}
	}
}