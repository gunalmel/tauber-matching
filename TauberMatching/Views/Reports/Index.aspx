<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Excel Reports</h2>
    <ul style="list-style:none">
        <li>
            <%: Html.ActionLink("Rankings", "GenerateRankingExcelReport", "Reports")%></li>
        <li>
            <%: Html.ActionLink("Project Comments", "GenerateProjectCommentsExcelReport", "Reports")%></li>
        <li>
            <%: Html.ActionLink("Project Rejects", "GenerateProjectRejectsExcelReport", "Reports")%></li>
        <li>
            <%: Html.ActionLink("Student Comments", "GenerateStudentCommentsExcelReport", "Reports")%></li>
        <li>
            <%: Html.ActionLink("Student Feedbacks", "GenerateStudentFeedbackExcelReport", "Reports")%></li>
    </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptOrCssContent" runat="server">
</asp:Content>
