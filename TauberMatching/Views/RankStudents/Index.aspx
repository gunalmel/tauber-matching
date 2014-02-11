<%@ Import Namespace="TauberMatching.Models" %>
<%@ Import Namespace="TauberMatching.Services" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<RankStudentsIndexModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Tauber Matching Web Application - Rank Students
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptOrCssContent" runat="server">
    <link href='<%=ResolveUrl("~/Content/RankStudents.css")%>' rel="stylesheet" type="text/css" />
    <script src='<%=ResolveUrl("~/Scripts/jquery-ui-1.8.16.custom.min.js")%>' type="text/javascript" ></script>
    <script src='<%=ResolveUrl("~/Scripts/json.js")%>' type="text/javascript"></script>
    <script src='<%=ResolveUrl("~/Scripts/UIFunctions.js")%>' type="text/javascript"></script>
    <script src='<%=ResolveUrl("~/Scripts/Helper.js")%>' type="text/javascript"></script>
    <% if (!Model.IsError)
    {%>     
    <script type="text/javascript">
        <%=UIParamsAndMessages.RANK_STUDENTS_INDEX_HEAD %>
        var webServiceUrlToSubmit = '<%=ResolveUrl("~/RankStudents/SubmitPreferences") %>';
        var AdminPhone = '<%= UIParamsAndMessages.TAUBER_PHONE %>';
        var AdminEmail = '<%= UIParamsAndMessages.TAUBER_EMAIL %>';
    </script>
    <script type="text/javascript" src="../../Scripts/RankStudents.js"></script>
    <%} %>
    <%  if(!this.Request.IsAuthenticated)
    {%>
        <script type="text/javascript">
            $(function () {
                $("#menu").remove();
                $("#logindisplay").remove();
            });
        </script>
    <%}%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% //TODO Externalize error messages in javascript %>

    <h2><%= Model.IsError?"":Model.ProjectName %></h2>
    <div id="divMessage" class="errorMessage"><%= Model.IsError?Model.ErrorMessage:"" %></div>
    <%  if (!Model.IsError)
        {%>
           <div id="divInstructions">
               Click on the yellow box with a student's name and discipline and 'drag and drop' it under the appropriate heading (Ideal,Desired,Acceptable or Reject)
               <ol>
                   <li class="inst">
                       Rank all of the candidates you interviewed using Ideal,Desired,Acceptable, or Reject selections:
                       <ul id="ulRanks">
                           <li class="disc"><b>Ideal:</b> Students who would be <b>ideal</b> members of your project team. Please rate at least <u><script type="text/javascript">document.write(MinAEngStudents);</script></u> engineering student and <u><script type="text/javascript">document.write(MinABusStudents);</script></u> business student in the 'Ideal' category. </li>
                           <li class="disc"><b>Desired:</b> Students who would be <b>desired</b> members of your project team. Please rate at least <u><script type="text/javascript">document.write(MinBEngStudents);</script></u> engineering student and <u><script type="text/javascript">document.write(MinBBusStudents);</script></u> business student in the 'Desired' category. </li>
                           <li class="disc"><b>Acceptable:</b> Students who would be <b>acceptable</b> members of your project.</li>
                           <li class="disc"><b>Reject:</b> <i>Optional.</i> If you feel strongly that a candidate would be <b>unacceptable</b> on your team, place his or her name here. A maximum of <u><script type="text/javascript">document.write(MaxRejectedEngStudents);</script></u> engineering student and <u><script type="text/javascript">document.write(MaxRejectedBusStudents);</script></u> business student can be placed in the 'Reject' category. </li>
                       </ul>
                   </li>
                   <li class="inst">Engineering (Eng) students are those pursuing Masters degrees in any engineering discipline; Business (Bus) students are those pursuing MBAs (including dual degree students) and those students in the MSCM program.</li>
                   <script type="text/javascript">
                        if(EnforceContinuousStudentRanking)
                            document.write('<li class="inst">You can assign more than one student to the same ranking category, however, your ranking list should be continuous (e.g., if you want to rank a student in the \'Acceptable\' category, you must have already ranked students in the \'Ideal\' and \'Desired\' category, otherwise you will receive an error message).</li>');
                   </script>
                   <li class="inst">
                       Students with the same ranking will be given equal weight during the matching process. There is a significant difference between rankings.
                       <ul id="ulReject">
                           <li class="disc">Note that if you reject a student (assign a rating of 'Reject'), a short note explaining the reasons would be appreciated in the box below.</li>
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
                    <ul id="ul_<%=scoreDetail.Score %>_Bucket" class="droptrue <%=scoreDetail.Score %>">
                        <li class="list-heading">
                            <b><%=scoreDetail.ScoreTypeDisplay%></b>
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
                    <li id="liRejectReason_<%=student.Id.ToString() %>" class="RejectReason">
                        <label id="lblReject_<%=student.Id.ToString() %>"  for="txtReject<%=student.Id.ToString() %>">Please enter the reason for rejecting <u><%=student.FullName %></u></label>
                        <textarea id="txtReject_<%=student.Id.ToString() %>" class="RejectReason" rows="4" cols="80"><%= Model.Rejects[student]%></textarea>
                    </li>
                <%} %>
                </ul>
            </div>
            <div id="divFeedback">
                <label id="lblFeedback" for="txtFeedback">Feedback:</label>
                <span id="spnFeedback">Students frequently ask for feedback from interviewers. Please provide feedback that might be useful to the students in future interview sessions. Recruiter names and companies will not be given to the students. This feedback is intended only as an aid to current and future Tauber students.</span>
                <textarea id="txtFeedback" cols="120" rows="5"><%=Model.Feedback %></textarea>
            </div>
            <input type="button" id="btnSubmit" value="Submit My Preferences" />
            <%} %>
            <div id="divWait"><img src="../../Content/images/wait.gif" alt="Please Wait..."></img></div>
</asp:Content>
