<%@ Import Namespace="TauberMatching.Models" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<RankStudentsIndexModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Tauber Matching Web Application - Rank Students
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptOrCssContent" runat="server">
    <link href='<%=ResolveUrl("~/Content/RankStudents.css")%>' rel="stylesheet" type="text/css" />
    <script src='<%=ResolveUrl("~/Scripts/jquery-ui-1.8.16.custom.min.js")%>' type="text/javascript" ></script>
    <script src='<%=ResolveUrl("~/Scripts/json.js")%>' type="text/javascript"></script>
    <script src='<%=ResolveUrl("~/Scripts/UIFunctions.js")%>' type="text/javascript" defer="defer"></script>
    <% if (!Model.IsError)
    {%>     
        <script type="text/javascript">
            <%=Model.UIJsStatements %>
        </script>
        <script type="text/javascript" src="../../Scripts/RankStudents.js"></script>
    <%} %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% //TODO #32 Interaction js %>
<% //TODO Externalize error messages in javascript %>

    <h2><%= Model.IsError?"":Model.ProjectName %></h2>
    <div id="divMessage" class="errorMessage"><%= Model.IsError?Model.ErrorMessage:"" %></div>
    <%  if (!Model.IsError)
        {
           string hiddenStudentCountIdTemplate = "hf_{0}_{1}_Count";
    %>
           <div id="divInstructions">
               Click on the yellow box with a student's name and discipline and 'drag and drop' it under the appropriate heading (A,B,C or Reject)
               <ol>
                   <li class="inst">
                       Rank all of the candidates you interviewed using A,B,C, or X selections:
                       <ul id="ulRanks">
                           <li class="disc"><b>A:</b> Students who would be <b>ideal</b> members of your project team. Please rate at least 1 engineering student and 1 business student in the 'A' category. </li>
                           <li class="disc"><b>B:</b> Students who would be <b>desired</b> members of your project team.</li>
                           <li class="disc"><b>C:</b> Students who would be <b>acceptable</b> members of your project.</li>
                           <li class="disc"><b>Reject:</b> <i>Optional.</i> If you feel strongly that a candidate would be <b>unacceptable</b> on your team, place his or her name here. A maximum of one engineering student and one business student can be placed in the 'X' category. </li>
                       </ul>
                   </li>
                   <li class="inst">Engineering (Eng) students are those pursuing Masters degrees in any engineering discipline; Business (Bus) students are those pursuing MBAs (including dual degree students) and those students in the MSCM program.</li>
                   <li class="inst">You can assign more than one student to the same ranking category, however, your ranking list should be continuous (e.g., if you want to rank a student in the 'C' category, you must have already ranked students in the 'A' and 'B' category, otherwise you will receive an error message).</li>
                   <li class="inst">
                       Students with the same ranking will be given equal weight during the matching process. There is a significant difference between rankings.
                       <ul id="ulReject">
                           <li class="disc">Note that if you reject a student (assign a rating of 'X'), a short note explaining the reasons would be appreciated in the box below.</li>
                       </ul>
                   </li>
               </ol>
           </div>
           <div id="divUserErrors" class="errorMessage"></div>
           <input id="hfProjectId" type="hidden" value="<%= Model.ProjectId %>"/>
           <input id="hfProjectGuid" type="hidden" value="<%= Model.Guid %>"/>
           <% foreach (StudentDegree degree in Enum.GetValues(typeof(StudentDegree)))
           {%>
                <!--Total Bus and Total Eng student count-->
                <input id="hf_<%=degree.ToString() %>_Total" type="hidden" value="<%= Model.StudentCountDict[degree] %>"/> 
           <%} %>
           <div id="divRanking">
           <% foreach (ScoreDetail scoreDetail in Model.ScoreGroupedStudents.Keys)
            { %>
                    <!--Student Ids in csv format in hidden fields for each score group-->
                    <input id="hf_<%=scoreDetail.Score %>_Ids" type="hidden" value="<%=String.Join(",",Model.ScoreGroupedStudents[scoreDetail].Select(s=>s.Id.ToString()).ToArray()) %>"/>
                    <ul id="ul_<%=scoreDetail.Score %>_Bucket" class="droptrue <%=scoreDetail.Score %>">
                        <li class="list-heading">
                            <b><%=scoreDetail.ScoreTypeDisplay%></b>
                            <% foreach (StudentDegree degree in Enum.GetValues(typeof(StudentDegree)))
                               {
                                   string hiddenStudentCountId = String.Format(hiddenStudentCountIdTemplate, scoreDetail.Score, degree.ToString());
                                   %><!--Eng and Bus student count in each score bucket-->
                                    <input id="<%=hiddenStudentCountId %>" type="hidden" value="<%= Model.ProjectScoreStudentCountMatrix[scoreDetail.Score,degree] %>" />
                            <% } %>
                        </li>
                        <% 
                               if (Model.ScoreGroupedStudents.Keys.Contains(scoreDetail))
                                   foreach (Student student in Model.ScoreGroupedStudents[scoreDetail])
                                   { %>
                                    <li id="<%= student.Id%>" class="<%= student.Degree%>">
                                        <%= student.FullName%><span class="degree">(<%= student.Degree%>)</span>
                                    </li>
                               <%} %>
                    </ul>
                <%} %>
            </div>
            <div id="divRejectReasons">
                <ul id="ulRejectReasons">
                <% foreach (Student student in Model.Rejects.Keys)
                {%>
                    <li class="RejectReason">
                        <label id="lblReject_<%=student.Id.ToString() %>"  for="txtReject<%=student.Id.ToString() %>">Please enter the reason for rejecting <u><%=student.FullName %></u></label>
                        <textarea id="txtReject_<%=student.Id.ToString() %>" class="RejectReason" rows="4" cols="80"><%= Model.Rejects[student]%></textarea>
                    </li>
                <%} %>
                </ul>
            </div>
            <div id="divFeedback">
                <label id="lblFeedback" for="txtFeedback">Feedback:</label>
                <span id="spnFeedback">Students frequently ask for feedback from interviewers. Please provide feedback that might be useful to the students in future interview sessions. Recruiter names and companies will not be given to the students. This feedback is intended only as an aid to current and future Tauber students.</span>
                <textarea id="txtFeedback" cols="120" rows="5"></textarea>
            </div>
            <%} %>

            <input type="button" id="btnSubmit" value="Submit My Preferences" />
            <div id="divWait">Please Wait...<img src="../../Content/images/wait.gif" alt="Please Wait..."></img></div>
</asp:Content>
