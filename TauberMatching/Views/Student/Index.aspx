<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TauberMatching.Models.Student>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptOrCssContent" runat="server">
    <script type="text/javascript" defer="defer">
        $(document).ready(function () {
            // Make the header checkbox select all checkbox
            $("[id=chkAll]").live("click", function () { $("input[type=checkbox][id*=chkSelect]").attr('checked', $(this).attr("checked")); });
            $.ajaxSetup({ type: "POST", contentType: "application/json;charset=utf-8", dataType: "json", processData: false });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% =ViewContext.TempData["message"] %>
    <h2>Students</h2>

    <table>
        <tr>
            <th><%: Html.CheckBox("chkAll", new { title="Chek all"})%></th>
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
                <%:Html.CheckBox("chkSelect"+item.Id) %>
            </td>
            <td>
                <%: Html.ActionLink("Edit", "Edit", new { id=item.Id }) %> |
                <%: Html.ActionLink("Details", "Details", new { id = item.Id })%> |
                <%: Html.ActionLink("Delete", "Delete", new { id = item.Id })%>
            </td>
            <td>
                <%: item.UniqueName %>
            </td>
            <td>
                <%: item.FirstName %>
            </td>
            <td>
                <%: item.LastName %>
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

