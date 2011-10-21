using System;
using System.Linq.Expressions;

namespace MvcContrib.CommandProcessor.Validation
{
	public class ValidationRuleInstance
	{
		public LambdaExpression ToCheckExpression { get; set; }
		public LambdaExpression ToCompareExpression { get; set; }
		public LambdaExpression UIAttributeExpression { get; set; }
		public Type ValidationRuleType { get; set; }
		public bool ArrayRule { get; set; }
		public Delegate ShouldApply { get; set; }

		public bool Equals(ValidationRuleInstance other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Equals(other.ToCheckExpression, ToCheckExpression) && Equals(other.ToCompareExpression, ToCompareExpression) &&
			       Equals(other.UIAttributeExpression, UIAttributeExpression) &&
			       Equals(other.ValidationRuleType, ValidationRuleType) && other.ArrayRule.Equals(ArrayRule) &&
			       Equals(other.ShouldApply, ShouldApply);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof (ValidationRuleInstance)) return false;
			return Equals((ValidationRuleInstance) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int result = (ToCheckExpression != null ? ToCheckExpression.GetHashCode() : 0);
				result = (result*397) ^ (ToCompareExpression != null ? ToCompareExpression.GetHashCode() : 0);
				result = (result*397) ^ (UIAttributeExpression != null ? UIAttributeExpression.GetHashCode() : 0);
				result = (result*397) ^ (ValidationRuleType != null ? ValidationRuleType.GetHashCode() : 0);
				result = (result*397) ^ ArrayRule.GetHashCode();
				result = (result*397) ^ (ShouldApply != null ? ShouldApply.GetHashCode() : 0);
				return result;
			}
		}
	}
}