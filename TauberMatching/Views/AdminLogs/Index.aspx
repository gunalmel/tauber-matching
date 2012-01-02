<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Tauber Matcing Web Application Logs
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Admin Logs</h2>
    <ul style="list-style: none">
        <li>
            <%: Html.ActionLink("System Logs", "SystemLogs", "AdminLogs")%></li>
        <li>
            <%: Html.ActionLink("Email Logs", "EmailLogs", "AdminLogs")%></li>
        <li>
            <%: Html.ActionLink("Email Queue", "EmailQueue", "AdminLogs")%></li>
    </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptOrCssContent" runat="server">
</asp:Content>
