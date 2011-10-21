<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<List<MvcContrib.TestHelper.Sample.Models.Star>>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
<%@ Import Namespace="MvcContrib.TestHelper.Sample.Controllers" %>
<h2>Stars</h2>

<ul>
    <% foreach (var star in Model) { %>
        <li>
            <%= star.Name %> approx: <%= star.Distance %> AU <%= Html.ActionLink("Nearby Stars", "ListWithLinks")%>
        </li>
    <% } %>
</ul>

</asp:Content>