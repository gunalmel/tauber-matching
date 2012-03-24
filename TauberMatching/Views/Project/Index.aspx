<%@ Import Namespace="TauberMatching.Services" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TauberMatching.Models.Project>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Tauber Matching - Projects
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptOrCssContent" runat="server">
    <script type="text/javascript" defer="defer">
        var ajaxUrl = '<%=ResolveUrl("~/Email/SendAccessUrl") %>'
        // The following variables are used to find the column that holds the variables used to build the ajax call parameter objects. If those values changes in the table that lists the projects in the body html then the variables blow should b updated accordingly for server side method to execute successfully
        var ColumnHeaderForContactName = "Contact Name";
        var ColumnHeaderForContactEmail = "Contact Email";
        var ContactType = 'Project';
    </script>
    <script src='<%=ResolveUrl("~/Scripts/json.js")%>' type="text/javascript"></script>
    <script src='<%=ResolveUrl("~/Scripts/UIFunctions.js")%>' type="text/javascript" defer="defer"></script>
    <script src='<%=ResolveUrl("~/Scripts/AdminContactIndex.js")%>' type="text/javascript" defer="defer"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divWait" style="position:fixed;top:32%;left:42%;background-color:white;z-index:100;display:none">
        Please Wait. Processing...
    </div>
    <div id="divMessage">
        <% =ViewContext.TempData["message"] %>
    </div>
    <h2>Projects</h2>
    <b>Warning: </b>If there are any projects not matching any students you will not be able to select those project contatcs to send access url link.<br />
    The e-mails are not sent in real-time, e-mailed checkboxes will be updated after a minute you sent the e-mails. Once you sent e-mails do not get surprised that Emailed checkboxes are not immediately checked. Wait and refresh page every minute to see that Emailed information gets updated.<br />
    <a href="javascript:sendMail();">Send Email to the Selected Contacts</a>
    <table>
        <tr>
            <th><%: Html.CheckBox("chkAll", new { title="Chek all"})%></th>
            <th></th>
            <th>Name</th>
            <th>Contact Name</th>
            <th>Contact Email</th>
            <th>URL Emailed</th>
            <th>Ranked</th>
            <th>Contact Phone</th>
            <th>Access Link</th>
            <th>Comments</th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%:Html.CheckBox("chkSelect_"+item.Id) %>
                <%:Html.Hidden("hfGuid_"+item.Id,item.Guid) %>
                <%:Html.Hidden("hfId_"+item.Id,item.Id) %>
                <%:Html.Hidden("hfIsThereAnyMatchings_"+item.Id,(item.Matchings.Count>0).ToString().ToLower()) %>
            </td>
            <td>
                <%: Html.ActionLink("Edit", "Edit", new { id=item.Id }) %> |
                <%: Html.ActionLink("Details", "Details", new { id=item.Id })%> |
                <%: Html.ActionLink("Delete", "Delete", new { id=item.Id })%>
            </td>
            <td><%: item.Name %></td>
            <td><%: item.ContactFullName %></td>
            <td><%: item.ContactEmail %></td>
            <td align="center"><%: Html.CheckBox("chkEmailed_"+item.Id, item.Emailed, new { disabled = "disabled" })%></td>
            <td align="center"><%: Html.CheckBox("chkRanked_"+item.Id, item.ScoreDate!=null, new { disabled = "disabled" })%></td>
            <td><%: item.ContactPhone %></td>
                        <td>
                <% if (item.Matchings != null && item.Matchings.Count != 0)
                   {%>
                <a id="hl_<%: item.Id %>" href="<%=TauberMatching.Services.UrlHelper.GetAccessUrlForTheUser(item.Guid,UrlType.Project) %>">Rank Students</a>
                <%}
                   else
                   {%>
                <b>Click on details to match with students first</b>
                <%} %>
                
            </td>
            <td><%: item.Comments %></td>
        </tr>
    <% } %>
    </table>

    <p>
        <%: Html.ActionLink("Add New Project", "Create") %>
    </p>

</asp:Content>

