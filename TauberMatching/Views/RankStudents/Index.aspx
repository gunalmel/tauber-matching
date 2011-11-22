<%@ Import Namespace="TauberMatching.Models" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<RankStudentsIndexModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Tauber Matching Web Application - Rank Students
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptOrCssContent" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Model.IsError?"":Model.ProjectName %></h2>
    <div id="divMessage">
        <%= Model.IsError?Model.ErrorMessage:"" %>
        <%=ViewContext.TempData["message"] %>
    </div>
    <%  string hiddenStudentCountIdTemplate = "hdn_{0}_{1}";
        if(!Model.IsError)
        foreach(ScoreDetail scoreDetail in Model.ScoreGroupedStudents.Keys)
        { %>
                <ul id="<%=scoreDetail.Score %>" class="droptrue">
                    <li class="list-heading">
                        <b><%=scoreDetail.ScoreTypeDisplay %></b>
                        <% foreach(StudentDegree degree in Enum.GetValues(typeof(StudentDegree)))
                           {
                               string hiddenStudentCountId = String.Format(hiddenStudentCountIdTemplate, scoreDetail.Score, degree.ToString());
                               %>
                                <input id="<%=hiddenStudentCountId %>" type="hidden" value="<%= Model.ProjectScoreStudentCountMatrix[scoreDetail.Score,degree] %>" />
                        <% } %>
                    </li>
                    <% 
                       if(Model.ScoreGroupedStudents.Keys.Contains(scoreDetail))
                           foreach (Student student in Model.ScoreGroupedStudents[scoreDetail])
                           { %>
                                <li id="<%= student.Id%>" class="<%= student.Degree%>">
                                    <%= student.FullName %><span class="degree">(<%= student.Degree%>)</span>
                                </li>
                           <%} %>
                </ul>
            <%} %>
</asp:Content>
