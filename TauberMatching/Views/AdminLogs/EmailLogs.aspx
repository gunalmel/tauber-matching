<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TauberMatching.Models.EmailLogDto>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Tauber Matching Web Application - Email Logs
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Email Log Entries</h2>
    <p>* You have to hit your browser's refresh button whenever you'd like to get the most up to date log information from the database. The page will not automatically update the records shown on this page.</p>
     <%: Html.ActionLink("Clear All Logs", "Clear", new { id = "emailLogs" })%>
    <table>
        <tr>
            <th>
                Date
            </th>
            <th>
                Guid
            </th>
            <th>
                Status
            </th>
            <th>
                Subject
            </th>
            <th>
                Message
            </th>
            <th>
                Name
            </th>
            <th>
                FirstName
            </th>
            <th>
                LastName
            </th>
            <th>
                Email
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: String.Format("{0:g}", item.Date) %>
            </td>
            <td>
                <%: item.Guid %>
            </td>
            <td>
                <%: item.Status %>
            </td>
            <td>
                <%: item.Subject %>
            </td>
            <td>
                <%: item.Message %>
            </td>
            <td>
                <%: item.Name %>
            </td>
            <td>
                <%: item.FirstName %>
            </td>
            <td>
                <%: item.LastName %>
            </td>
            <td>
                <%: item.Email %>
            </td>
        </tr>
    
    <% } %>

    </table>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptOrCssContent" runat="server">
</asp:Content>

