<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TauberMatching.Models.Student>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptOrCssContent" runat="server">
    <script type="text/javascript" defer="defer">
        //Flag to denote if any of the listed items has been e-mailed before
        var duplicateSendEmail = false;
        $(document).ready(function () {
            // Make the header checkbox select all checkbox
            $("[id=chkAll]").live("click", function () { $("input[type=checkbox][id*=chkSelect]").attr('checked', $(this).attr("checked")); });
            $.ajaxSetup({ type: "POST", contentType: "application/json;charset=utf-8", dataType: "json", processData: false });
        });
        //Finds the checked checkboxes used to select the entries in the list and returns the ids appended to the checkbox id in a csv list to be fed to the web service method.
        function getSelected() {
            duplicateSendEmail = $("input[type=checkbox][id*=chkSelect]:checked").parent().parent().find("input[type=checkbox][id*=chkEmailed]:checked").length > 0;
            var checkboxes = $("input[type=checkbox][id*=chkSelect]:checked");
            var csv = "";
            checkboxes.each(function (index) {
                var id = this.id.split('_')[1];
                csv += (index == 0) ? id : ("," + id);
            });
            return csv;
        }
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
                Student Name
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
                <%:Html.Hidden("hdnGuid_"+item.Id,item.Guid) %>
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
                <%: (item.FirstName != null || item.LastName != null) ? (item.FirstName + " " + item.LastName) : ""%>
            </td>
            <td>
                <%: item.Degree %>
            </td>
            <td>
                <%: item.Email %>
            </td>
            <td align="center">
                <%: Html.CheckBox("chkEmailed_"+item.Id, item.Emailed, new { disabled = "disabled" })%>
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

