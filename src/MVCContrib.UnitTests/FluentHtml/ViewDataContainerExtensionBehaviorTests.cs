using MvcContrib.FluentHtml;
using MvcContrib.UnitTests.FluentHtml.Fakes;
using NUnit.Framework;

namespace MvcContrib.UnitTests.FluentHtml
{
	public class ViewDataContainerExtensionBehaviorTests
	{
		private FakeBehavior behavior;
		private FakeBehavioralViewDataContainer target;

		[SetUp]
		public void SetUp()
		{
			behavior = new FakeBehavior();
			target = new FakeBehavioralViewDataContainer(new[] {behavior});
		}

		[Test]
		public void can_get_button_with_behaviors()
		{
			target.Button("test").ToString();
			verify_behavior_executed();
		}

		[Test]
		public void can_get_checkbox_with_behaviors()
		{
			target.CheckBox("test").ToString();
			verify_behavior_executed();
		}

		[Test]
		public void can_get_checkboxlist_with_behaviors()
		{
			target.CheckBoxList("test").ToString();
			verify_behavior_executed();
		}

		[Test]
		public void can_get_fileupload_with_behaviors()
		{
			target.FileUpload("test").ToString();
			verify_behavior_executed();
		}

		[Test]
		public void can_get_formliteral_with_behaviors()
		{
			target.FormLiteral("test").ToString();
			verify_behavior_executed();
		}

		[Test]
		public void can_get_hidden_with_behaviors()
		{
			target.Hidden("test").ToString();
			verify_behavior_executed();
		}

		[Test]
		public void can_get_label_with_behaviors()
		{
			target.Label("test").ToString();
			verify_behavior_executed();
		}

		[Test]
		public void can_get_literal_with_behaviors()
		{
			target.Literal("test").ToString();
			verify_behavior_executed();
		}

		[Test]
		public void can_get_multiselect_with_behaviors()
		{
			target.MultiSelect("test").ToString();
			verify_behavior_executed();
		}

		[Test]
		public void can_get_password_with_behaviors()
		{
			target.Password("test").ToString();
			verify_behavior_executed();
		}

		[Test]
		public void can_get_radiobutton_with_behaviors()
		{
			target.RadioButton("test").ToString();
			verify_behavior_executed();
		}

		[Test]
		public void can_get_radioset_with_behaviors()
		{
			target.RadioSet("test").ToString();
			verify_behavior_executed();
		}

		[Test]
		public void can_get_resetbutton_with_behaviors()
		{
			target.ResetButton("test").ToString();
			verify_behavior_executed();
		}

		[Test]
		public void can_get_select_with_behaviors()
		{
			target.Select("test").ToString();
			verify_behavior_executed();
		}

		[Test]
		public void can_get_submitbutton_with_behaviors()
		{
			target.SubmitButton("test").ToString();
			verify_behavior_executed();
		}

		[Test]
		public void can_get_textarea_with_behaviors()
		{
			target.TextArea("test").ToString();
			verify_behavior_executed();
		}

		[Test]
		public void can_get_textbox_with_behaviors()
		{
			target.TextBox("test").ToString();
			verify_behavior_executed();
		}

		private void verify_behavior_executed()
		{
			behavior.Executed.ShouldBeTrue();
		}
	}
}