<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TauberMatching.Models.Student>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% =ViewContext.TempData["message"] %>
    <h2>Index</h2>

    <table>
        <tr>
            <th></th>
            <th>
                Unique Name
            </th>
            <th>
                Last Name
            </th>
            <th>
                First Name
            </th>
            <th>
                Degree
            </th>
            <th>
                Email
            </th>
            <th>
               URL Emailed?
            </th>
            <th>
                Comments
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: Html.ActionLink("Edit", "Edit", new { id=item.Id }) %> |
                <%: Html.ActionLink("Details", "Details", new { id = item.Id })%> |
                <%: Html.ActionLink("Delete", "Delete", new { id = item.Id })%>
            </td>
            <td>
                <%: item.UniqueName %>
            </td>
            <td>
                <%: item.Last %>
            </td>
            <td>
                <%: item.First %>
            </td>
            <td>
                <%: item.Degree %>
            </td>
            <td>
                <%: item.Email %>
            </td>
            <td>
                <%: item.Emailed?"Yes":"No" %>
            </td>
            <td>
                <%: item.Comments %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%: Html.ActionLink("Add New Student", "Create") %>
    </p>

</asp:Content>

