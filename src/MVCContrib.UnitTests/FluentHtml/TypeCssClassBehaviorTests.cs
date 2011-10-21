using System;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Expressions;
using MvcContrib.UnitTests.FluentHtml.Fakes;
using NUnit.Framework;

namespace MvcContrib.UnitTests.FluentHtml
{
	public class behaves_like_type_css_class_behavior_test
	{
		protected TypeCssClassBehavior behavior;
		protected IMemberElement element;

		protected void Establish_context(Expression<Func<FakeModel, object>> member)
		{
			behavior = new TypeCssClassBehavior();
			element = new TextBox(member.GetNameFor(), member.GetMemberExpression(), null);
		}

		protected void Establish_context(Expression<Func<FakeModel, object>> member, string dateClass, string numberClass)
		{
			behavior = new TypeCssClassBehavior(numberClass, dateClass);
			element = new TextBox(member.GetNameFor(), member.GetMemberExpression(), null);
		}

		protected void Becuase_of()
		{
			behavior.Execute(element);
		}
	}

	[TestFixture]
	public class when_applying_type_css_class_behavior_to_element_for_member_type_that_is_number : behaves_like_type_css_class_behavior_test
	{
		[TestFixtureSetUp]
		public void SetUp()
		{
			Establish_context(x => x.Price);
			Becuase_of();
		}

		[Test]
		public void It_should_add_number_class()
		{
			element.GetAttr("class").ShouldEqual("number");
		}
	}

	[TestFixture]
	public class when_applying_type_css_class_behavior_to_element_for_member_type_that_is_date : behaves_like_type_css_class_behavior_test
	{
		[TestFixtureSetUp]
		public void SetUp()
		{
			Establish_context(x => x.Date);
			Becuase_of();
		}

		[Test]
		public void It_should_add_date_class()
		{
			element.GetAttr("class").ShouldEqual("date");
		}
	}

	[TestFixture]
	public class when_applying_type_css_class_behavior_to_element_using_a_custom_number_class : behaves_like_type_css_class_behavior_test
	{
		[TestFixtureSetUp]
		public void SetUp()
		{
			Establish_context(x => x.Price, "foobar", "myNumberClass");
			Becuase_of();
		}

		[Test]
		public void It_should_add_custom_number_class()
		{
			element.GetAttr("class").ShouldEqual("myNumberClass");
		}
	}

	[TestFixture]
	public class when_applying_type_css_class_behavior_to_element_using_a_custom_date_class : behaves_like_type_css_class_behavior_test
	{
		[TestFixtureSetUp]
		public void SetUp()
		{
			Establish_context(x => x.Date, "myDateClass", "foobar");
			Becuase_of();
		}

		[Test]
		public void It_should_add_custom_date_class()
		{
			element.GetAttr("class").ShouldEqual("myDateClass");
		}
	}
}