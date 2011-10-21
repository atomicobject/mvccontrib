<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<MvcContrib.TestHelper.Sample.Models.Star>>" %>
<%@ Import Namespace="System.Web.Mvc.Html"%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
<%@ Import Namespace="MvcContrib.TestHelper.Sample.Controllers" %>

<h2>Stars</h2>

<ul>
    <% foreach (var star in Model) { %>
        <li>
            <%= star.Name %> approx: <%= star.Distance %> AU
        </li>
    <% } %>
</ul>
<%using(Html.BeginForm(new{action= "AddFormStar"})){%>
    <%= Html.TextBox("NewStarName") %>
	<input type="submit" value="Submit" />
<%}%>

<%using (Html.BeginForm(new { action = "AddSessionStar" })) {%>
    <%= Html.TextBox("NewStarName")%>
	<input type ="submit" value="Submit" />
<%}%>

Form: <%= Html.ViewContext.TempData["NewStarName"] != null? Html.ViewContext.TempData["NewStarName"]:"" %>
Session: <%= Html.ViewContext.HttpContext.Session["NewStarName"] != null ? Html.ViewContext.HttpContext.Session["NewStarName"] : ""%>

</asp:Content>