<%@ Import Namespace="TauberMatching.Models" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<RankStudentsIndexModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Tauber Matching Web Application - Rank Students
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptOrCssContent" runat="server">
    <link href="../../Content/RankStudents.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Content/jquery-ui-1.8.7.custom.min.js"></script>
    <% if (!Model.IsError) %>
    <script type="text/javascript">
        <%=Model.UIJsStatements %>
        /*Class to store total student count on the UI in total and with respect to each Student Degree*/
        function TotalStudentCount(engCount,busCount){
            this.Eng=engCount;
            this.Bus=busCount;
            this.All=engCount+busCount;
        }
        /*Class to transfer error status and error message to display*/
        function Error(isThereAnyError,messageToDisplay){
            this.isError=isThereAnyError;
            this.errorMessage=messageToDisplay;
        }
        var ScoreBuckets;
        var StudentCount;
        $(function () {
            $("ul.droptrue").sortable({
                connectWith: "ul",
                items: "li:not(.list-heading)",
                receive: onReceived
            });
            
            $(".droptrue").disableSelection();
            ScoreBuckets=$("ul.droptrue");
            StudentCount=new TotalStudentCount(parseInt($("#hf_Eng_Total").val()),parseInt($("#hf_Bus_Total").val()));
        });
        /*Extracts the elemnt that triggers the event from the event object*/
        function getTargetElementFromEvent(event){
            var targ;
            if (event.target)
                targ = event.target;
            else if (event.srcElement)
                targ = event.srcElement;
            return targ;
        }
        /*If score names or degree names are changed this function need to be updated.*/
        function onReceived(event, ui) {
            
            var receiver = ui.item.parent();
            var degree = ui.item.attr("class");

            var rejectedEngStudentCount = getStudentCountForScoreForDegree("Reject","Eng");
            var rejectedBusStudentCount =  getStudentCountForScoreForDegree("Reject","Bus");
            var rejectedTotalStudentCount = rejectedEngStudentCount+rejectedBusStudentCount;
             
            if(receiver.attr("id")=="ul_Reject_Bucket"){
                var rejectError = CheckForRejectedStudentError(rejectedEngStudentCount,rejectedBusStudentCount,rejectedTotalStudentCount);
                if(rejectError.isError){
                    alert(rejectError.errorMessage);
                    $(ui.sender).sortable("cancel");
                    }
               // alert("Eng:"+rejectedEngStudentCount+" Bus:"+rejectedBusStudentCount);
            }
            else{
            }
            alert(isRankingContinuous().isError+" "+isRankingContinuous().errorMessage);
        }
        /*Checks if the student ranking scheme is sparse. e.g.: It returns an Error object whose isError is set to true with appropriate errorMessage if there are students in A and C group when there are no students in B group*/
        function isRankingContinuous(){
            var positionOfTheLastBucketWithStudents=-1;
            var positionOfTheNextBucketWithStudents=0;
            var isRankingContinuous=true;
            var error = new Error(!isRankingContinuous,"");
            if(!EnforceContinuousStudentRanking)
                return new Error(isRankingContinuous,"");
            ScoreBuckets.filter(":not(#ul_NoScore_Bucket,#ul_Reject_Bucket)").each(function(index){
                var studentCountInTheBucket = $(this).find("li.:not(.list-heading)").length;
                positionOfTheNextBucketWithStudents=studentCountInTheBucket>0?index:positionOfTheNextBucketWithStudents;
                if(studentCountInTheBucket>0){
                    if((positionOfTheNextBucketWithStudents-positionOfTheLastBucketWithStudents)>1){
                        isRankingContinuous = false;
                        error.isError=!isRankingContinuous;
                        error.errorMessage="When you are ranking students, your ranking scheme should not be sparse, e.g.: If you there are students in A and C when there are no students in B that's an error.";
                        }
                    positionOfTheLastBucketWithStudents=positionOfTheNextBucketWithStudents;
                }
            });
            return error;
        }
        /*Error class that sets the error state and error message with respect to the number of students rejected.*/
        function CheckForRejectedStudentError(rejectedEngStudentCount,rejectedBusStudentCount,rejectedTotalStudentCount){
            var error = new Error();
            if(rejectedTotalStudentCount!=-1&&StudentCount.All<RejectedStudentThreshold&&rejectedTotalStudentCount>0){ //Projects can reject students only when they interviewed more than ceratin # of students
                error.isError=true;
                error.errorMessage="You should have interviewed at least "+RejectedStudentThreshold+" students to be able to reject any students";
                }
            else if(rejectedEngStudentCount!=-1&&rejectedEngStudentCount>MaxRejectedEngStudents){
                error.isError=true;
                error.errorMessage="Maximum # of Engineering students you can reject is: "+MaxRejectedEngStudents;
                }
            else if (rejectedBusStudentCount!=-1&&rejectedBusStudentCount>MaxRejectedBusStudents){
                error.isError=true;
                error.errorMessage="Maximum # of Business students you can reject is: "+MaxRejectedBusStudents;
                }
            else if (rejectedTotalStudentCount!=-1&&rejectedTotalStudentCount>MaxRejectedStudents){
                error.isError=true;
                error.errorMessage="Maximum # of students you can reject is: "+MaxRejectedStudents;
                }
            else{
                error.isError=false;
                error.errorMessage="";
            }
            return error;
        }
        function submit(){
            
        }
        /* Returns an object whose properties are named such as [<Degree>TotalCount] to refer to the total number of students towards the specified degree type*/
        function getTotalStudentCountByDegree(){
            var TotalStudentCountByDegree={};
            var degreeListLength = degreeList.length;
            for(var degreeIndex=0; degreeIndex<degreeListLength; degreeIndex++) {
	            var degree = degreeList[degreeIndex];
                TotalStudentCountByDegree[degree+"TotalCount"]=$("#hf_"+degree+"_Total").val();
             }
             return TotalStudentCountByDegree;
        }

        /*Returns an object whose properties are named such as [<Score><Degree>Count] to refer to the count of students with a specific degree in the score bucket specified.*/
        function getStudentCountGroupedByScoreAndDegree(){
            var StudentCountGroupedByScoreAndDegree={};
            var scoreListLength = scoreList.length;
            for(var scoreIndex=0; scoreIndex<scoreListLength; scoreIndex++) {
	            var score = scoreList[scoreIndex];
                var degreeListLength = degreeList.length;
                var scoreBucketListItems = ScoreBuckets.filter("."+score).find("li:not(.list-heading)");
                StudentCountGroupedByScoreAndDegree[score+"TotalCount"]=scoreBucketListItems.length;
                for(var degreeIndex=0; degreeIndex<degreeListLength; degreeIndex++) {
	                var degree = degreeList[degreeIndex];
                    var degreeCountInBucket = scoreBucketListItems.filter("."+degree).length;
                    StudentCountGroupedByScoreAndDegree[score+degree+"Count"]=degreeCountInBucket;
                }
             }
             /* For testing
             alert("NoScore: "+StudentCountGroupedByScoreAndDegree.NoScoreTotalCount);
             alert("A: "+StudentCountGroupedByScoreAndDegree.ATotalCount);
             alert("B: "+StudentCountGroupedByScoreAndDegree.BTotalCount);
             alert("C: "+StudentCountGroupedByScoreAndDegree.CTotalCount);
             alert("Reject: "+StudentCountGroupedByScoreAndDegree.RejectTotalCount);

             alert("NoScoreEng: "+StudentCountGroupedByScoreAndDegree.NoScoreEngCount);
             alert("NoScoreBus: "+StudentCountGroupedByScoreAndDegree.NoScoreBusCount);
             alert("AEng: "+StudentCountGroupedByScoreAndDegree.AEngCount);
             alert("ABus: "+StudentCountGroupedByScoreAndDegree.ABusCount);
             alert("BEng: "+StudentCountGroupedByScoreAndDegree.BEngCount);
             alert("BBus: "+StudentCountGroupedByScoreAndDegree.BBusCount);
             alert("CEng: "+StudentCountGroupedByScoreAndDegree.CEngCount);
             alert("CBus: "+StudentCountGroupedByScoreAndDegree.CBusCount);
             alert("RejectEng: "+StudentCountGroupedByScoreAndDegree.RejectEngCount);
             alert("RejectBus: "+StudentCountGroupedByScoreAndDegree.RejectBusCount);
             */
             return StudentCountGroupedByScoreAndDegree;
        }
        function getStudentCountForScoreForDegree(score,degree){
            return ScoreBuckets.filter("."+score).find("li."+degree+":not(.list-heading)").length;//$("#ul_"+score+"_Bucket li."+degree+":not(.list-heading)").length;
        }
    </script>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% //TODO #32, #24 Interaction js %>
<% //TODO Validation for MinABusStudents, MinAEngStudents, MinAStudents %>
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
                           <li class="disc"><b>X:</b> <i>Optional.</i> If you feel strongly that a candidate would be <b>unacceptable</b> on your team, place his or her name here. A maximum of one engineering student and one business student can be placed in the 'X' category. </li>
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
                <% foreach (StudentDegree degree in Enum.GetValues(typeof(StudentDegree)))
                {%>
                    <li class="RejectReason">
                        <label id="lblReject_<%=degree.ToString() %>"  for="txtReject<%=degree.ToString() %>">Reason if any <u><i><%=degree==StudentDegree.Bus?"business":"engineering" %></i></u> is rejected</label>
                        <textarea id="txtReject_<%=degree.ToString() %>" class="RejectReason"><%= Model.Rejects.Keys.Contains(degree)?Model.Rejects[degree]:""%></textarea>
                    </li>
                <%} %>
                </ul>
            </div>
            <div id="divFeedback">
                <label id="lblFeedback" for="txtFeedback">Feedback:</label>
                <span id="spnFeedback">Students frequently ask for feedback from interviewers. Please provide feedback that might be useful to the students in future interview sessions. Recruiter names and companies will not be given to the students. This feedback is intended only as an aid to current and future Tauber students.</span>
                <textarea id="txtFeedbaack" cols="120" rows="5"></textarea>
            </div>
            <%} %>

            <input type="button" id="btnSubmit" value="Submit My Preferences" onclick="getStudentCountGroupedByScoreAndDegree();" />
            <span id="spnWait">Please Wait...<img src="../../Content/images/wait.gif"></img></span>
</asp:Content>
