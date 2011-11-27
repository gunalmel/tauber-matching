<%@ Import Namespace="TauberMatching.Models" %>
<%@ Import Namespace="TauberMatching.Services" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TauberMatching.Models.RankProjectsIndexModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Tauber Matching Web Application - Rank Projects
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptOrCssContent" runat="server">
    <link href='<%=ResolveUrl("~/Content/RankProjects.css")%>' rel="stylesheet" type="text/css" />
    <script src='<%=ResolveUrl("~/Scripts/jquery-ui-1.8.16.custom.min.js")%>' type="text/javascript" ></script>
    <script src='<%=ResolveUrl("~/Scripts/json.js")%>' type="text/javascript"></script>
    <script src='<%=ResolveUrl("~/Scripts/UIFunctions.js")%>' type="text/javascript"></script>
    <script src='<%=ResolveUrl("~/Scripts/Helper.js")%>' type="text/javascript"></script>
    <% if (!Model.IsError)
    {%>     
        <script type="text/javascript">
            <%=UIParamsAndMessages.RANK_PROJECTS_INDEX_HEAD %>
            var webServiceUrlToSubmit = '<%=ResolveUrl("~/RankProjects/SubmitPreferences") %>';
            var AdminPhone = '<%= UIParamsAndMessages.TAUBER_PHONE %>';
            var AdminEmail = '<%= UIParamsAndMessages.TAUBER_EMAIL %>';
        </script>
        <script type="text/javascript" src="../../Scripts/RankProjects.js"></script>
    <%} %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Model.IsError?"":"Welcome "+Model.StudentName %></h2>
    <div id="divMessage">
        <%= Model.IsError?Model.ErrorMessage:"" %>
    </div>
    <%if(!Model.IsError)
      {%>
    <div id="divInstructions">
        <span id="sSection1Start"><b>Ranking Process Notes:</b></span>
        <br />Click on the yellow box with a project's name and 'drag and drop' it under the appropriate heading (FIRST, SECOND, THIRD, etc.)
        <ol>
        <li>You must rank all the projects with which you interviewed.</li>
        <li>You must rank at least one project as your #1 choice.</li>
        <li>You can give more than one project the same ranking; however, your ranking list should be continuous (e.g., if you have used #1 and #4 to rank projects, you should have used #2 & #3 to rank other projects as well, otherwise you will receive an error message).</li>
    </ol>
        <hr />
        <br />
    </div>

    <div id="divUserErrors" class="errorMessage"></div>
    <div id="divRanking">
        <input type="hidden" id="hfStudentId" value="<%=Model.StudentId %>" />
        <input type="hidden" id="hfStduentGuid" value="<%=Model.Guid %>" />
        <%foreach(ScoreDetail scoreDetail in Model.ScoreGroupedProjects.Keys)
        { %>
        <ul id="ul_<%=scoreDetail.Score %>_Bucket" class="droptrue">
            <li class="list-heading">
                <%=scoreDetail.ScoreTypeDisplay %>
            </li>
            <%if(Model.ScoreGroupedProjects.Keys.Contains(scoreDetail))
            foreach (Project project in Model.ScoreGroupedProjects[scoreDetail])
            { %>
            <li id="<%= project.Id%>" class="project">
                <%= project.Name %>
            </li>
            <%} %>
        </ul>
            <%} %>
    <%} %>
    </div>
    <div id="divFeedback">
        <ul id="ulPositiveFeedback" class="feedback">
            <li id="liPositiveFeedback_Header" class="feedback list-heading">
                Section Two: Positive Feedback
            </li>
            <li id="liPositiveFeedback_Instructions" class="feedback">
                Of the projects that you DID NOT interview, which five or six were <u>most attractive</u> to you?
            </li>
            <%for (int i = 1; i < 7; i++)
            {%>
            <li id="liPositiveFeedback_<%=i %>" class="feedback">
                  <%=i %>.<select id="ddlPositiveFeedback_<%=i %>" class="feedback">
                    <option value="0">--Select a project--</option>
                  <%foreach (Project p in Model.ProjectsNotInterviewed)
                    {%>
                    <option value="<%=p.Id %>"><%=p.Name %></option>
                    <%} %>
                  </select>
               
            </li>
            <%} %>
        </ul>
        <ul id="ulConstructiveFeedback" class="feedback">
            <li id="liConstructiveFeedback_Header" class="feedback list-heading">
                Section Three: Constructive Feedback
            </li>
            <li id="liConstructiveFeedback_Instructions" class="feedback">
                Of the projects that you DID NOT interview, which five or six were <u>least attractive</u> to you?
            </li>
            <%for (int i = 1; i < 7; i++)
            {%>
            <li id="liConstructiveFeedback_<%=i %>" class="feedback">
                  <%=i %>.<select id="ddlConstructiveFeedback_<%=i %>" class="feedback">
                    <option value="0">--Select a project--</option>
                  <%foreach (Project p in Model.ProjectsNotInterviewed)
                    {%>
                    <option value="<%=p.Id %>"><%=p.Name %></option>
                    <%} %>
                  </select>
               
            </li>
            <%} %>
        </ul>
    </div>
    <div id="divComments">
        <label id="lblComments" for="txtComments">Other Comments:</label>
        <textarea id="txtComments" cols="120" rows="5"><%=Model.OtherComments %></textarea>
    </div>
    <input type="button" id="btnSubmit" value="Submit My Preferences" />
    <div id="divWait"><img src="../../Content/images/wait.gif" alt="Please Wait..."></img></div>
</asp:Content>
