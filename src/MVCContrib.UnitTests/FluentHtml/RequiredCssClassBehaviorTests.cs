using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Expressions;
using MvcContrib.UnitTests.FluentHtml.Fakes;
using NUnit.Framework;

namespace MvcContrib.UnitTests.FluentHtml
{
	public class behaves_like_required_behavior_test
	{
		protected RequiredCssClassBehavior behavior;
		protected IMemberElement element;

		protected void Establish_context(Expression<Func<FakeModel, object>> member)
		{
			Establish_context(member, null);
		}

		protected void Establish_context(Expression<Func<FakeModel, object>> member, string requiredClass)
		{
			behavior = requiredClass == null ? new RequiredCssClassBehavior() : new RequiredCssClassBehavior(requiredClass);
			element = new TextBox(member.GetNameFor(), member.GetMemberExpression(), null);
		}

		protected void Becuase_of()
		{
			behavior.Execute(element);
		}
	}

	[TestFixture]
	public class when_applying_required_behavior_to_element_for_non_nullable_value_type : behaves_like_required_behavior_test
	{
		[TestFixtureSetUp]
		public void SetUp()
		{
			Establish_context(x => x.Date);
			Becuase_of();
		}

		[Test]
		public void It_should_add_required_class()
		{
			element.GetAttr("class").ShouldEqual("required");
		}
	}

	[TestFixture]
	public class when_applying_required_behavior_to_element_for_nullable_member : behaves_like_required_behavior_test
	{
		[TestFixtureSetUp]
		public void SetUp()
		{
			Establish_context(x => x.Title);
			Becuase_of();
		}

		[Test]
		public void It_should_not_add_required_class()
		{
			element.GetAttr("class").ShouldBeNullOrEmpty();
		}
	}

	[TestFixture]
	public class when_applying_required_behavior_to_element_for_required_member_using_custom_css_class : behaves_like_required_behavior_test
	{
		[TestFixtureSetUp]
		public void SetUp()
		{
			Establish_context(x => x.Date, "myRequiredClass");
			Becuase_of();
		}

		[Test]
		public void It_should_add_custom_required_class()
		{
			element.GetAttr("class").ShouldEqual("myRequiredClass");
		}
	}

	public class behaves_like_required_behavior_test_with_attribute_test
	{
		protected RequiredCssClassBehavior<RequiredAttribute> behavior;
		protected IMemberElement element;

		protected void Establish_context(Expression<Func<FakeModel, object>> member)
		{
			behavior = new RequiredCssClassBehavior<RequiredAttribute>();
			element = new TextBox(member.GetNameFor(), member.GetMemberExpression(), null);
		}

		protected void Becuase_of()
		{
			behavior.Execute(element);
		}
	}

	[TestFixture]
	public class when_applying_required_behavior_to_element_for_nullable_type_with_required_attribute : behaves_like_required_behavior_test_with_attribute_test
	{
		[TestFixtureSetUp]
		public void SetUp()
		{
			Establish_context(x => x.Price);
			Becuase_of();
		}

		[Test]
		public void It_should_add_required_class()
		{
			element.GetAttr("class").ShouldEqual("required");
		}
	}

	[TestFixture]
	public class when_applying_required_behavior_to_element_for_nullable_type_without_required_attribute : behaves_like_required_behavior_test_with_attribute_test
	{
		[TestFixtureSetUp]
		public void SetUp()
		{
			Establish_context(x => x.Title);
			Becuase_of();
		}

		[Test]
		public void It_should_not_add_required_class()
		{
			element.GetAttr("class").ShouldBeNullOrEmpty();
		}
	}
}