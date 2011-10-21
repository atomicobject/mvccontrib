using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;
using MvcContrib.UnitTests.FluentHtml.Fakes;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;

namespace MvcContrib.UnitTests.FluentHtml
{
	[TestFixture]
	public class DataListTests
	{
		[Test]
		public void basic_datalist_render()
		{
			var html = new DataList("test").ToString();
			html.ShouldHaveHtmlNode("test")
				.ShouldBeNamed(HtmlTag.DataList);
		}

		[Test]
		public void datalist_set_data_should_render_data()
		{
			var html = new DataList("test").Data("http://www.test.com/data").ToString();
			html.ShouldHaveHtmlNode("test")
				.ShouldHaveAttribute(HtmlAttribute.Data).WithValue("http://www.test.com/data");
		}

		[Test]
		public void basic_datalist_renders_with_options_from_dictionary()
		{
			var options = new Dictionary<int, string> { { 1, "One" }, { 2, "Two" } };
			var html = new DataList("test").Options(options).ToString();
			var element = html.ShouldHaveHtmlNode("test");
			var optionNodes = element.ShouldHaveChildNodesCount(2);
			optionNodes[0].ShouldBeUnSelectedOption(1, "One");
			optionNodes[1].ShouldBeUnSelectedOption(2, "Two");
		}

		[Test]
		public void can_render_options_from_enumerable_of_simple_objects()
		{
			var optionNodes = new DataList("test").Options(new[] { 1, 2 }).ToString()
				.ShouldHaveHtmlNode("test")
				.ShouldHaveChildNodesCount(2);
			optionNodes[0].ShouldBeUnSelectedOption("1", "1");
			optionNodes[1].ShouldBeUnSelectedOption("2", "2");
		}

		[Test]
		public void basic_datalist_renders_select_with_options_from_select_list()
		{
			var items = new List<FakeModel>
			{
				new FakeModel {Id = 1, Title = "One"},
				new FakeModel {Id = 2, Title = "Two"}
			};
			var selectList = new SelectList(items, "Id", "Title", items[0].Id);
			var html = new DataList("test").Options(selectList).ToString();
			var element = html.ShouldHaveHtmlNode("test");
			var optionNodes = element.ShouldHaveChildNodesCount(2);
			optionNodes[0].ShouldBeSelectedOption(items[0].Id, items[0].Title);
			optionNodes[1].ShouldBeUnSelectedOption(items[1].Id, items[1].Title);
		}

		[Test]
		public void datalist_with_options_for_enum_renders_enum_values_as_options()
		{
			var html = new DataList("test").Options<FakeEnum>().ToString();
			var element = html.ShouldHaveHtmlNode("test");
			var optionNodes = element.ShouldHaveChildNodesCount(4);
			optionNodes[0].ShouldBeUnSelectedOption((int)FakeEnum.Zero, FakeEnum.Zero);
			optionNodes[1].ShouldBeUnSelectedOption((int)FakeEnum.One, FakeEnum.One);
			optionNodes[2].ShouldBeUnSelectedOption((int)FakeEnum.Two, FakeEnum.Two);
			optionNodes[3].ShouldBeUnSelectedOption((int)FakeEnum.Three, FakeEnum.Three);
		}

		[Test]
		public void datalist_with_options_for_subset_enum_renders_enum_values_as_options()
		{
			var html = new DataList("test").Options(new[] { FakeEnum.One, FakeEnum.Two, FakeEnum.Three }).ToString();
			var element = html.ShouldHaveHtmlNode("test");
			var optionNodes = element.ShouldHaveChildNodesCount(3);
			optionNodes[0].ShouldBeUnSelectedOption((int)FakeEnum.One, FakeEnum.One);
			optionNodes[1].ShouldBeUnSelectedOption((int)FakeEnum.Two, FakeEnum.Two);
			optionNodes[2].ShouldBeUnSelectedOption((int)FakeEnum.Three, FakeEnum.Three);
		}

		[Test]
		public void can_modify_each_option_element_using_the_option_data_item()
		{
			var items = new List<FakeModel>
			{
				new FakeModel {Price = 1, Title = "One"},
				new FakeModel {Price = 2, Title = "Two", Done = true},
			};
			var optionNodes = new DataList("test").Options(items, x => x.Price, x => x.Title)
				.EachOption((cb, opt, i) => cb.Disabled(((FakeModel)opt).Done)).ToString()
				.ShouldHaveHtmlNode("test")
				.ShouldHaveChildNodesCount(2);
			optionNodes[0].ShouldNotHaveAttribute("disabled");
			optionNodes[1].ShouldHaveAttribute("disabled");
		}

		[Test]
		public void datalist_with_lambda_selector_for_options_should_render()
		{
			var items = new List<FakeModel> { new FakeModel { Price = 1, Title = "One" } };
			var options = new DataList("x").Options(items, x => x.Price, x => x.Title).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldHaveChildNodesCount(1);
			options[0].ShouldBeUnSelectedOption("1", "One");
		}

		[Test]
		public void datalist_can_set_options_using_text_and_value_proprty_names()
		{
			var items = new List<FakeModel>
			{
				new FakeModel {Price = 1, Title = "One"},
				new FakeModel {Price = 2, Title = "Two"}
			};
			var optionNodes = new DataList("test").Options(items, "Price", "Title").ToString()
				.ShouldHaveHtmlNode("test")
				.ShouldHaveChildNodesCount(2);
			optionNodes[0].ShouldBeUnSelectedOption(items[0].Price, items[0].Title);
			optionNodes[1].ShouldBeUnSelectedOption(items[1].Price, items[1].Title);
		}

		[Test]
		public void datalist_options_of_enumerable_select_list_item_renders_options()
		{
			var items = new List<SelectListItem>
			{
				new SelectListItem {Value = "1", Text = "One", Selected = false},
				new SelectListItem {Value = "2", Text = "Two", Selected = true},
				new SelectListItem {Value = "3", Text = "Three", Selected = true}
			};
			var html = new DataList("test").Options(items).ToString();
			var element = html.ShouldHaveHtmlNode("test");
			var optionNodes = element.ShouldHaveChildNodesCount(3);
			optionNodes[0].ShouldBeUnSelectedOption(items[0].Value, items[0].Text);
			optionNodes[1].ShouldBeSelectedOption(items[1].Value, items[1].Text);
			optionNodes[2].ShouldBeSelectedOption(items[2].Value, items[2].Text);
		}

		[Test, ExpectedException(typeof(ArgumentException))]
		public void datalist_options_with_wrong_data_value_field_throws_on_tostring()
		{
			var items = new List<FakeModel> { new FakeModel { Price = null, Title = "One" } };
			new Select("x").Options(items, "Wrong", "Title").ToString();
		}

		[Test, ExpectedException(typeof(ArgumentException))]
		public void datalist_options_with_wrong_data_text_field_throws_on_tostring()
		{
			var items = new List<FakeModel> { new FakeModel { Price = null, Title = "One" } };
			new Select("x").Options(items, "Price", "Wrong").ToString();
		}
	}
}