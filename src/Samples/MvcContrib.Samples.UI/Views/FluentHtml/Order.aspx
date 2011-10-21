<%@Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="SampleFluentHtml5ViewPage<MvcContrib.Samples.UI.Models.Order>" %>
<%@Import Namespace="MvcContrib.FluentHtml" %>
<%@Import Namespace="MvcContrib.Samples.UI.Views" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>Fluent HTML</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="fluentHtmlContainer">
        <fieldset>
          <legend>Place Your Order</legend>
          <p><%= this.TextBox(m => m.Name).Autofocus(true).Placeholder("ex. Hugo Reyes").Label("Name:") %></p><br />
          <p><%= this.EmailBox(m => m.Email).Placeholder("ex. name@domain.com").Label("Website:") %></p>
          <p><%= this.UrlBox(m => m.Website).Placeholder("ex. http://www.domain.com").Label("Phone:") %></p>
          <p><%= this.TelephoneBox(m => m.Phone).Title("(xxx) xxx-xxxx").Label("Phone:") %></p>
          <p><%= this.DatePicker(m => m.DeliveryDate).Label("Delivery:") %></p>
          <p><%= this.TextArea(m => m.Address).Label("Address:") %></p>
          <p><%= this.NumberBox(m => m.Quantity).Label("Quantity:") %></p>
          <p><%= this.SubmitButton("Place Order") %>
          <%= this.SubmitButton("Save For Later").FormNoValidate(true).Id("saveForLater") %></p>
        </fieldset>
    </div>
</asp:Content>