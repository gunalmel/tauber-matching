<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TauberMatching.Models.EmailQueueMessage>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Tauber Matching Web Application - Email Queue
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Email Queue Entries</h2>
    <p>* You have to hit your browser's refresh button whenever you'd like to get the most up to date email queue status information from the database. The page will not automatically update the records shown on this page.</p>
     <%: Html.ActionLink("Clear The Queue", "Clear", new { id = "emailQueue" })%>
    <table>
        <tr>
            <th>
                Id
            </th>
            <th>
                ContactId
            </th>
            <th>
                ContactType
            </th>
            <th>
                FirstName
            </th>
            <th>
                LastName
            </th>
            <th>
                Guid
            </th>
            <th>
                To
            </th>
            <th>
                Subject
            </th>
            <th>
                Body
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: item.Id %>
            </td>
            <td>
                <%: item.ContactId %>
            </td>
            <td>
                <%: item.ContactType %>
            </td>
            <td>
                <%: item.FirstName %>
            </td>
            <td>
                <%: item.LastName %>
            </td>
            <td>
                <%: item.Guid %>
            </td>
            <td>
                <%: item.To %>
            </td>
            <td>
                <%: item.Subject %>
            </td>
            <td>
                <%: item.Body %>
            </td>
        </tr>
    
    <% } %>

    </table>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptOrCssContent" runat="server">
</asp:Content>

