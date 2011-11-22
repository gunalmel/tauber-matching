<%@ Import Namespace="TauberMatching.Models" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TauberMatching.Models.RankProjectsIndexModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Tauber Matching Web Application - Rank Projects
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptOrCssContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Model.IsError?"":"Welcome "+Model.StudentName %></h2>
    <div id="divMessage">
        <%= Model.IsError?Model.ErrorMessage:"" %>
        <%=ViewContext.TempData["message"] %>
    </div>

        <%  if(!Model.IsError)
        foreach(ScoreDetail scoreDetail in Model.ScoreGroupedProjects.Keys)
        { %>
                <ul id="<%=scoreDetail.Score %>" class="droptrue">
                    <li class="list-heading">
                        <b><%=scoreDetail.ScoreTypeDisplay %></b>
                        <input id="hdn_<%=scoreDetail.Score %>" type="hidden" value="<%= Model.ScoreGroupedProjects[scoreDetail].Count() %>" />
                    </li>
                    <% 
                       if(Model.ScoreGroupedProjects.Keys.Contains(scoreDetail))
                           foreach (Project project in Model.ScoreGroupedProjects[scoreDetail])
                           { %>
                                <li id="<%= project.Id%>">
                                    <%= project.Name %>
                                </li>
                           <%} %>
                </ul>
            <%} %>

</asp:Content>
