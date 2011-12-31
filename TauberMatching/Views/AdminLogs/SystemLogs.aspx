<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TauberMatching.Models.SystemLogDto>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Tauber Matching Web Application - System Logs
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>System Log Entries</h2>
    <p>* You have to hit your browser's refresh button whenever you'd like to get the most up to date log information from the database. The page will not automatically update the records shown on this page.</p>
     <%: Html.ActionLink("Clear All Logs", "Clear", new { id="systemLogs" }) %>
    <table>
        <tr>
            <th>
                Date
            </th>
            <th>
                Thread
            </th>
            <th>
                Level
            </th>
            <th>
                Logger
            </th>
            <th>
                Message
            </th>
            <th>
                Exception
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: String.Format("{0:g}", item.Date) %>
            </td>
            <td>
                <%: item.Thread %>
            </td>
            <td>
                <%: item.Level %>
            </td>
            <td>
                <%: item.Logger %>
            </td>
            <td>
                <%: item.Message %>
            </td>
            <td>
                <%: item.Exception %>
            </td>
        </tr>
    
    <% } %>
    </table>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptOrCssContent" runat="server">
</asp:Content>

