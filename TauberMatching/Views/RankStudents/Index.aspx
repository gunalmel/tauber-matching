<%@ Import Namespace="TauberMatching.Controllers" %>
<%@ Import Namespace="TauberMatching.Models" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IDictionary<RankStudentsController.IndexModelAttributes,Object>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptOrCssContent" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Model[RankStudentsController.IndexModelAttributes.ProjectName].ToString()%></h2>
    <div id="divMessage">
        <%=ViewContext.TempData["message"] %>
    </div>
    <%  if (Model[RankStudentsController.IndexModelAttributes.GroupedStudents]!=null)
            foreach (ScoreDetail scoreDetail in (IList<ScoreDetail>)Model[RankStudentsController.IndexModelAttributes.ScoreDetails])
            { 
                var rankStudentDict = ((IDictionary<string, List<Student>>)Model[RankStudentsController.IndexModelAttributes.GroupedStudents]);
                var engDegree = Degree.Eng.ToString();
                var busDegree = Degree.Bus.ToString();
                var engCount = rankStudentDict.Keys.Contains(scoreDetail.Score)?rankStudentDict[scoreDetail.Score].Where(s=>s.Degree==engDegree).Count():0;
                var busCount = rankStudentDict.Keys.Contains(scoreDetail.Score)?rankStudentDict[scoreDetail.Score].Where(s=>s.Degree==busDegree).Count():0;
             %>
                <ul id="<%=scoreDetail.Score %>" class="droptrue">
                    <li class="list-heading">
                        <b><%=scoreDetail.ScoreTypeDisplay %></b>
                        <input id="hdn<%=scoreDetail.Score %>_Eng" type="hidden" value="<%= engCount %>" />
                        <input id="hdn<%=scoreDetail.Score %>_Bus" type="hidden" value="<%= busCount %>"/>
                    </li>
                    <% 
                       if(rankStudentDict.Keys.Contains(scoreDetail.Score))
                           foreach (Student student in rankStudentDict[scoreDetail.Score])
                           { %>
                                <li id="<%= student.Id%>" class="<%= student.Degree%>">
                                    <%= student.FullName %><span class="degree">(<%= student.Degree%>)</span>
                                </li>
                           <%} %>
                </ul>
            <%} %>
</asp:Content>
