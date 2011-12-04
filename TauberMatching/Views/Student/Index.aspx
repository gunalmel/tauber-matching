<%@ Import Namespace="TauberMatching.Services" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TauberMatching.Models.Student>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptOrCssContent" runat="server">
    <script src='<%=ResolveUrl("~/Scripts/json.js")%>' type="text/javascript"></script>
    <script src='<%=ResolveUrl("~/Scripts/UIFunctions.js")%>' type="text/javascript" defer="defer"></script>
    <script type="text/javascript" defer="defer">
        function Contact(id, guid, email, firstName, lastName) {
            this.Id = id; this.Guid = guid; this.Email = email, this.FirstName = firstName; this.LastName = lastName; this.ContactType = "10";
        }
        //Flag to denote if any of the listed items has been e-mailed before
        var duplicateSendEmail = false;
        $(document).ready(function () {
            // Make the header checkbox select all checkbox
            $("#chkAll").live("click", function () { var checked = this.checked; $("input[type=checkbox][id*=chkSelect]").attr('checked', checked); });
            $.ajaxSetup({ type: "POST", contentType: "application/json;charset=utf-8", dataType: "json", processData: false });
        });
        function sendMail() {
            var list = getSelected();
            var answer = true;
            if (duplicateSendEmail)
                answer = confirm("Some of your selections has already been e-mailed before. Are you sure you'd like to e-mail the previously e-mailed contacts?");
            if (!answer)
                return;
            if (list == "")
                return;
            var paramString = $.toJSON({ contacts: list });
            $.ajax({
                url: '<%=ResolveUrl("~/Email/SendAccessUrl") %>',
                data: paramString,
                beforeSend: beforeEmailSend,
                success: onEmailSuccess,
                error: onError,
                async: false
            });
        }
        function beforeEmailSend() {
            grayOut(true);
            $("#divWait").toggle();
        }
        function onEmailSuccess(msg, event, xhr) {
            $("#divWait").toggle();
            grayOut(false);
            $("#divMessage").css("color", "black");
            // Reloads the page by resetting the selected page in pagination so that grid will be bound to fresh data
            $("input[type=checkbox][id*=chkSelect]:checked").attr('checked', false);
            $("#chkAll").attr('checked', false);
            window.location = location.href;
        }
        function onError(xhr, error) {
            grayOut(false);
            $("#divMessage").css("color", "red");
            $("#divMessage").html(xhr.status + ": " + xhr.statusText + " " + xhr.responseText);
            $("#divWait").toggle();
        }

        //Finds the checked checkboxes used to select the entries in the list and returns the list of contact objects to pass to the web service method
        function getSelected() {
            duplicateSendEmail = $("input[type=checkbox][id*=chkSelect]:checked").parent().parent().find("input[type=checkbox][id*=chkEmailed]:checked").length > 0;
            var selectedRows = $("input[type=checkbox][id*=chkSelect]:checked").parent().parent();
            var contactArray = new Array();
            selectedRows.each(function (index) {
                contactArray[index] = extractContactFromTableRow($(this));
            });
            return contactArray;
        }
        function extractContactFromTableRow(row) {
            var id = row.find("[id*=hdnId]").val();
            var guid = row.find("[id*=hdnGuid]").val();
            var names = extractTextFromTableRowColumn(row, 'Student Name').split(' ');
            var email = extractTextFromTableRowColumn(row, 'Email');
            return new Contact(id, guid, email, names[0], names[1]);
        }
        function extractTextFromTableRowColumn(row, colHeader) {
            var index = $("table tr:first-child th").filter(function (index) { return $(this).text()==colHeader}).index();
            var colText = row.find("td:eq(" + index + ")").text();
            return colText;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divWait" style="position:fixed;top:32%;left:42%;background-color:white;z-index:100;display:none">
        Please Wait. Processing...
    </div>
    <div id="divMessage">
        <% =ViewContext.TempData["message"] %>
    </div>
    <h2>Students</h2>
    <a href="javascript:sendMail();">Send Email to the Selected Students</a>
    <table>
        <tr>
            <th><%: Html.CheckBox("chkAll", new { title="Chek all"})%></th>
            <th></th>
            <th>Unique Name</th>
            <th>Student Name</th>
            <th>Degree</th>
            <th>Email</th>
            <th>URL Emailed?</th>
            <th>Access Url</th>
            <th>Comments</th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%:Html.CheckBox("chkSelect_"+item.Id) %>
                <%:Html.Hidden("hdnGuid_"+item.Id,item.Guid) %>
                <%:Html.Hidden("hdnId_"+item.Id,item.Id) %>
            </td>
            <td>
                <%: Html.ActionLink("Edit", "Edit", new { id=item.Id }) %> |
                <%: Html.ActionLink("Details", "Details", new { id = item.Id })%> |
                <%: Html.ActionLink("Delete", "Delete", new { id = item.Id })%>
            </td>
            <td><%: item.UniqueName %></td>
            <td><%: item.FullName %></td>
            <td><%: item.Degree %></td>
            <td><%: item.Email %></td>
            <td align="center"><%: Html.CheckBox("chkEmailed_"+item.Id, item.Emailed, new { disabled = "disabled" })%></td>
            <td>
                <% if (item.Matchings != null && item.Matchings.Count != 0)
                   {%>
                <a id="hl_<%: item.Id %>" href="<%=TauberMatching.Services.UrlHelper.GetAccessUrlForTheUser(item.Guid,UrlType.Student) %>">Rank Projects</a>
                <%}
                   else
                   {%>
                <b>Click on details to match with projects first</b>
                <%} %>
                
            </td>
            <td><%: item.Comments %></td>
        </tr>
    <% } %>
    </table>

    <p>
        <%: Html.ActionLink("Add New Student", "Create") %>
    </p>

</asp:Content>

