/**
 * @fileOverview This file has functions, classes and variables related to implementing user interaction on RankStudents view.
 * @author <a href="mailto:gunalmel@yahoo.com">Melih Gunal</a> 11/25/2011
 * The following varibales are dynamically added as they are fetched from the db by the controller into the view. The values displayed below are for providing an example only. The actual values on the UI might be different.
 * var scoreList = ["NoScore","A","B","C","Reject"];
 * var degreeList = ["Bus","Eng"];
 * var MinABusStudents = 1; (-1 ignores associated validation rule)
 * var MinAEngStudents = 1; (-1 ignores associated validation rule)
 * var MinAStudents = 2; (-1 ignores associated validation rule)
 * var MaxRejectedBusStudents = 1; (-1 ignores associated validation rule)
 * var MaxRejectedEngStudents = 1; (-1 ignores associated validation rule)
 * var MaxRejectedStudents = 2; (-1 ignores associated validation rule)
 * var RejectedStudentThreshold = 5; (0 ignores associated validation rule)
 * var EnforceContinuousStudentRanking = true;
 * var webServiceUrlToSubmit, AdminPhone, AdminEmail is set in the view.
 */
/** Error message to be displayed when the user assigns A to a number of engineering students less  than the number specified by MinAEngStudents */
var engStudentsErrorMessage = "You have to assign A to at least " + MinAEngStudents + " engineering student" + (MinAEngStudents > 1 ? "s.\n" : ".\n");
/** Error message to be displayed when the user the user assigns A to a number of business students less than the number specified by MinABusStudents */
var busStudentsErrorMessage = "You have to assign A to at least " + MinABusStudents + " business student" + (MinABusStudents > 1 ? "s.\n" : ".\n");
/** Error message to be displayed when the the user assigns A to a number of students less than the number specified by MinAStudents */
var allStudentsErrorMessage = "You have to assign A to at least " + MinAStudents + " student" + (MinAStudents > 1 ? "s.\n" : ".\n");
/** If EnforceContinuousStudentRanking then the Error message to be displayed when ranking scheme is sparse */
var sparseRankingErrorMessage = "When you are ranking students, your ranking scheme should not be sparse, e.g.: If there are students in A and C when there are no students in B that's an error.\n";

var minTotalStudentsToRejectViolationErrorMessage = "You should have interviewed at least " + RejectedStudentThreshold + " students to be able to reject a student\n";
var maxEngStudentsToRejectViolationErrorMessage = "Maximum # of Engineering students you can reject is: " + MaxRejectedEngStudents+"\n";
var maxBusStudentsToRejectViolationErrorMessage = "Maximum # of Business students you can reject is: " + MaxRejectedBusStudents+"\n";
var maxTotalStudentsToRejectViolationErrorMessage = "Maximum # of students you can reject is: " + MaxRejectedStudents + "\n";
var notAllStudentsRankedErrorMessage = "There are students who are not ranked. All students should be ranked.\n";

var rejectReasonErrorMessage = "You have to enter reason for every student rejected.\n";
var submissionSuccessMessage = "Your changes are successfully submitted & saved.";

var projectId; 
var projectGuid;

var systemErrorMessage = "We apologize for the unexpected system failure. We appreciate it if you inform Tauber Institute about the system error referencing the message below (Phone:" + AdminPhone + ", E-mail: <a href='mailto:" + AdminEmail + "?subject=Tauber Institute Matching Web Application Unexpected system Error&body=Project guid {0}, projectId {1} experienced an error with error message {2}'>"+AdminEmail+"</a>):<br/>";

var divUserErrors;
/**
 * Global variable to store all ranking buckets on the interface using JQuery 
 * @type JQuery object array
 */
var ScoreBuckets;
/** 
 * Global varible to transfer the result of validation functions.
 * @type Error 
 * @see Error
 */
var StudentCount;

/**
 * @class Used to transfer error status and associated error message to be displayed between functions.
 * @param {Boolean} isError Represents if the object has been created due to an error. When false errorMessage will be empty string.
 * @param {String} errorMessage The message that will be displayed to user if this object has been created as a result of UI error, otherwise will be empty string.
 */
function UIError(isThereAnyError, messageToDisplay) {
    this.isError = isThereAnyError;
    this.errorMessage = messageToDisplay;
}
/**
 * @class Data transfer object to transfer project preferences from UI to web service
 * @see ProjectPreferenceDto
 */
function ProjectScoreDto(studentId, score) {
    this.StudentId = studentId;
    this.Score = score;
}
/**
 * @class Data transfer object to transfer project rejects from UI to web service.
 * @see ProjectPreferenceDto
 */
function ProjectRejectDto(studentId, reason) {
    this.StudentId = studentId;
    this.Reason = reason;
}

/**
 * @class Data transfer object to transfer all the data on the UI necessary to persist project preferences through web service call.
 * @param {Integer} projectId
 * @param {String} projectGuid Acts as a key for web service to accept the request together with projectId
 * @param projectPreferences Array of {@link ProjectScoreDto} objects
 * @param projectRejects Array of {@link ProjectRejectDto} objects
 */
function ProjectPreferenceDto(projectId, projectGuid, projectPreferences, projectRejects, feedback) {
    this.ProjectId = projectId;
    this.ProjectGuid = projectGuid;
    this.ProjectPreferences = projectPreferences;
    this.ProjectRejects = projectRejects;
    this.Feedback = feedback;
}

/**
* @function Equivalent of JQuery $(document).ready() function call. Makes Ranking buckets sortable drag and drop containers using JQuery UI plug-in.
*           Initializes global variables to store ranking buckets and student counts grouped by degree on the interface
*/
$(function () {
    $("ul.droptrue").sortable({
        connectWith: "ul.droptrue",
        items: "li:not(.list-heading)",
        receive: onReceived,
        remove: onRemoved
    }).disableSelection();

    ScoreBuckets = $("ul.droptrue");
    StudentCount = getTotalStudentCountByDegree();
    projectId = parseInt($("#hfProjectId").val());
    projectGuid = $("#hfProjectGuid").val();
    divUserErrors = $("#divUserErrors");
    $("#btnSubmit").click(onSubmit);
    $.ajaxSetup({ type: "POST", contentType: "application/json;charset=utf-8", dataType: "json", processData: false });
    if (RejectedStudentThreshold != -1 && StudentCount.All < RejectedStudentThreshold)
        $("#ul_Reject_Bucket").hide();
});

/**
 * @function Ajax submit call to submit project preferences to the web service controller
 * @param {ProjectPreferenceDto} The data transfer object that will be submitted to the web service to persist rankings.
 * @see <a href="http://api.jquery.com/jQuery.ajax/">JQuery Ajax</a>
 */
function submitPreferences(dataToBeSubmitted) {
    var paramString = $.toJSON({ preferencesDto: dataToBeSubmitted });
    $.ajax({
        url: webServiceUrlToSubmit,
        data: paramString,
        beforeSend: beforeSubmit,
        success: onSubmitSuccess,
        error: onError,
        async: false
    });
}

/**
* @function JQuery AJAX event handler that is triggered before AJAX call is made. 
* @see <a href="http://api.jquery.com/jQuery.ajax/">JQuery Ajax</a>
*/
function beforeSubmit() {
    divUserErrors.html("");
    grayOut(true);
    $("#divWait").toggle();
}

/**
* @function JQuery AJAX event handler that is triggered after Ajax call is successfully completed. 
* @see <a href="http://api.jquery.com/jQuery.ajax/">JQuery Ajax</a>
*/
function onSubmitSuccess(msg, event, xhr) {
    $("#divWait").toggle();
    grayOut(false);
    if (msg == "Success") {
        alert(submissionSuccessMessage);
        // Reloads the page.
        window.location = location.href;
    } else {
        divUserErrors.html(systemErrorMessage.format(projectId, projectGuid, msg)+msg);
        divUserErrors.attr("tabindex", -1).focus();
    }
}

/**
 * @function JQuery AJAX event handler that is triggered whenever there is an AJAX call error. 
 * @see <a href="http://api.jquery.com/jQuery.ajax/">JQuery Ajax</a>
 */
function onError(xhr, error) {
    grayOut(false);
    // use xhr.responseText whenever the response text from the server is needed.
    var ajaxErrorMessage = xhr.status + ": " + xhr.statusText;
    var serverResponsePlainTextUrlEncoded = encodeURIComponent(xhr.responseText);
    var ajaxErrorMessageURLEncoded = encodeURIComponent(ajaxErrorMessage);
    var errorMessage = systemErrorMessage.format(projectId, projectGuid, ajaxErrorMessage);
    divUserErrors.html(errorMessage + ajaxErrorMessage);
    divUserErrors.attr("tabindex", -1).focus();
    $("#divWait").toggle();
}

/** 
* @function Event handler function for JQuery sortable drag and drop UI. Whenever the droppable element receives a draggable element this function is triggered before the drop action is finalized. Handles the real time validation to decide if a selected student can be rejected also adds reject reason text areas whenever a student is rejected.
* @see checkForRejectedStudentError
*/
function onReceived(event, ui) {
    var receiver = ui.item.parent();
    if (receiver.attr("id") == "ul_Reject_Bucket") {
        var rejectError = checkForRejectedStudentError();

        if (rejectError.isError) {
            alert(rejectError.errorMessage);
            $(ui.sender).sortable("cancel");
        }
        else {
            var studentId = ui.item.attr("id");
            var fullName = ui.item.text();
            addRejectReasonForStudent(studentId, fullName);
        }
        // alert("Eng:"+rejectedEngStudentCount+" Bus:"+rejectedBusStudentCount);
       // alert(validateRejectReason().errorMessage);
    }
    //alert(isRankingContinuous().isError+" "+isRankingContinuous().errorMessage);
    //alert(checkForAStudentError().errorMessage);
    //alert(runAllValidations().errorMessage);
}
/**
 * @function Event handler function for JQuery sortable drag and drop UI. Whenever a draggable elemnt is removed from a sortable droppable element this function is triggered to discard reject reason textarea if rejected student is removed from rejected bucket.
 */
function onRemoved(event, ui) {
    if (this.id == "ul_Reject_Bucket") {
        var studentId = ui.item.attr("id");
        removeRejectReasonForStudent(studentId);
    }
}

/**
* @function Triggered when submit button is clicked. Runs validations and transfers the user preferences to the web service.
* @returns {Boolean} Returns true if all UI validations pass.
*/
function onSubmit() {
    if (runAllValidations().isError) {
        divUserErrors.attr("tabindex", -1).focus(); // to be able to focus on div set tabindex to -1
        return false;
    }
    submitPreferences(buildProjectRankingDto());
    return true;
}
/**
* @function Builds the ProjectPreferenceDto object from the dat on the UI.
* @see ProjectPreferenceDto
*/
function buildProjectRankingDto() {
    var projectFeedback = $("#txtFeedback").val();
    var projectScoreDtoArray = new Array();
    var projectRejectDtoArray = new Array();

    // Get all ProjectScoreDto objects
    var listIndexOffset = 0;
    ScoreBuckets.filter(":not(#ul_NoScore_Bucket,#ulRejectReasons)").each(function (bucketIndex) {
        var bucket = $(this);
        var score = bucket.attr("id").split("_")[1];
        bucket.find("li:not(.list-heading)").each(function (listIndex) {
            projectScoreDtoArray[listIndexOffset+listIndex] = new ProjectScoreDto(this.id, score);
        });
        listIndexOffset = projectScoreDtoArray.length;
    });

    //Get all ProjectReject objects
    $("#ulRejectReasons li textarea").each(function (index) {
        projectRejectDtoArray[index] = new ProjectRejectDto(parseInt(this.id.split('_')[1]), this.value);
    });

    return new ProjectPreferenceDto(projectId, projectGuid, projectScoreDtoArray, projectRejectDtoArray, projectFeedback);
}
/* UI Validation Functions Starts Here.*/

/**
 * @function Calls UI validation functions to run on the client side before calling server side web service to persist user preferences.
 * @returns {UIError}
 */
function runAllValidations() {
    divUserErrors.html("");
    var AError = checkForAStudentError();
    var isContinuousError = isRankingContinuous();
    var isRejectReasonEmptyError = validateRejectReason();
    var areAllStudentsRanked = checkIfAllStudentsRanked();
    var error = new UIError(AError.isError || isContinuousError.isError || isRejectReasonEmptyError.isError || areAllStudentsRanked.isError, "");
    error.errorMessage = AError.errorMessage + isContinuousError.errorMessage + isRejectReasonEmptyError.errorMessage + areAllStudentsRanked.errorMessage;
    error.errorMessage = error.errorMessage.replace(/\n/g, '<br/>').replace(/  ,/g, '<br/>');
    divUserErrors.html(error.errorMessage);
    return error;
}

/**
 * @function Checks if the user has scored all the students
 * @returns {UIError}
 */
function checkIfAllStudentsRanked() {
    if ($("#ul_NoScore_Bucket li:not(.list-heading)").length > 0)
        return new UIError(true, notAllStudentsRankedErrorMessage);
    return new UIError(false, "");
}
/** 
* @function Checks if more students than specified by configuration parameters have been rejected and returns UIError object accordingly
* @returns {UIError} UIError object indicating if there's an error rejecting a student and  including error message accordingly.
*/
function checkForRejectedStudentError() {

    var rejectedEngStudentCount = getStudentCountForScoreForDegree("Reject", "Eng");
    var rejectedBusStudentCount = getStudentCountForScoreForDegree("Reject", "Bus");
    var rejectedTotalStudentCount = rejectedEngStudentCount + rejectedBusStudentCount;

    var error = new UIError();
    if (RejectedStudentThreshold != -1 && StudentCount.All < RejectedStudentThreshold && rejectedTotalStudentCount > 0) { //Projects can reject students only when they interviewed more than certain # of students
        error.isError = true;
        error.errorMessage = minTotalStudentsToRejectViolationErrorMessage;
    }
    else if (MaxRejectedEngStudents != -1 && rejectedEngStudentCount > MaxRejectedEngStudents) {
        error.isError = true;
        error.errorMessage = maxEngStudentsToRejectViolationErrorMessage;
    }
    else if (MaxRejectedBusStudents != -1 && rejectedBusStudentCount > MaxRejectedBusStudents) {
        error.isError = true;
        error.errorMessage = maxBusStudentsToRejectViolationErrorMessage;
    }
    else if (MaxRejectedStudents != -1 && rejectedTotalStudentCount > MaxRejectedStudents) {
        error.isError = true;
        error.errorMessage = maxTotalStudentsToRejectViolationErrorMessage;
    }
    else {
        error.isError = false;
        error.errorMessage = "";
    }
    return error;
}
/**
 * @function Checks if the reject reason is entered when there are students who are rejected.
 * @returns {UIError}
 */
function validateRejectReason() {
    var rejectBoxes = $("#ulRejectReasons li textarea")
    var error = new UIError(false, "");
    rejectBoxes.each(function () {
        if(this.value==""||$(this).val().trim()==""){
            error.isError = true;
            error.errorMessage = rejectReasonErrorMessage;
        }
    });
    return error;
}
/**
* @function Checks if the required min # of eng, bus and total students are assigned A
* @returns {UIError} Returns an UIError object to indicate whether the validation result is an error, if it is then errorMessage property is set to the error message to be displayed.
*/
function checkForAStudentError() {
    var aEngStudentCount = getStudentCountForScoreForDegree("A", "Eng");
    var aBusStudentCount = getStudentCountForScoreForDegree("A", "Bus");
    var aStudentCount = aEngStudentCount + aBusStudentCount;
    var isError = false;
    var errorMessage = "";
    if (MinAEngStudents != -1 && aEngStudentCount < MinAEngStudents && StudentCount.Eng > 0) {
        isError = true;
        errorMessage += engStudentsErrorMessage;
    }
    if (MinABusStudents != -1 && aBusStudentCount < MinABusStudents && StudentCount.Bus > 0) {
        isError = true;
        errorMessage += busStudentsErrorMessage;
    }
    if (MinAStudents != -1 && aStudentCount < MinAStudents) {
        isError = true;
        errorMessage += allStudentsErrorMessage;
    }
    return new UIError(isError, errorMessage);
}

/**
 * @function Checks if the student ranking scheme is sparse. e.g.: It returns an UIError object whose isError is set to true with appropriate errorMessage if there are students in A and C group when there are no students in B group
 * @returns {UIError} Returns UIError object indicating if there's an error message and associated error message.
 */
function isRankingContinuous() {
    var positionOfTheLastBucketWithStudents = -1;
    var positionOfTheNextBucketWithStudents = 0;
    var isContinuous = true;
    var error = new UIError(!isContinuous, "");
    if (!EnforceContinuousStudentRanking)
        return new UIError(isContinuous, "");
    ScoreBuckets.filter(":not(#ul_NoScore_Bucket,#ul_Reject_Bucket)").each(function (index) {
        var studentCountInTheBucket = $(this).find("li:not(.list-heading)").length;
        positionOfTheNextBucketWithStudents = studentCountInTheBucket > 0 ? index : positionOfTheNextBucketWithStudents;
        if (studentCountInTheBucket > 0) {
            if ((positionOfTheNextBucketWithStudents - positionOfTheLastBucketWithStudents) > 1) {
                isContinuous = false;
                error.isError = !isContinuous;
                error.errorMessage = sparseRankingErrorMessage;
            }
            positionOfTheLastBucketWithStudents = positionOfTheNextBucketWithStudents;
        }
    });
    return error;
}
/* UI Validation Functions Ends Here.*/

/**
 * @function Gets the number of students within a given score bucket (ranking group) with the specified degree
 * @param {String} score The ranking group (e.g. A, B)
 * @param {String} degree The degree towards the student is studying. Eng(Engineering)|Bus(Business)
 */
function getStudentCountForScoreForDegree(score, degree) {
    return ScoreBuckets.filter("." + score).find("li." + degree + ":not(.list-heading)").length;
}

/**
 * @function Returns an object whose properties are named such as [<Degree>TotalCount] to refer to the total number of students towards the specified degree type
 * @returns {TotalStudentCountByDegree} A dynamic object whose properties consists of dynamically generated properties named after student degrees and an additional property called All stroing the total number of students on the interface.
 */
function getTotalStudentCountByDegree() {
    var TotalStudentCountByDegree = {};
    var degreeListLength = degreeList.length;
    var totalStudentCount = 0;
    for (var degreeIndex = 0; degreeIndex < degreeListLength; degreeIndex++) {
        var degree = degreeList[degreeIndex];
        TotalStudentCountByDegree[degree] = $("#hf_" + degree + "_Total").val();
        totalStudentCount += parseInt(TotalStudentCountByDegree[degree]);
    }
    TotalStudentCountByDegree["All"] = totalStudentCount;
    return TotalStudentCountByDegree;
}

/**
 * @function Returns an object whose properties are named such as [<Score><Degree>Count] to refer to the count of students with a specific degree in the score bucket specified.
 * @returns {StudentCountGroupedByScoreAndDegree} A dynamically created object that will store the count of students on the UI grouped by score and degree. The properties will be like: StudentCountGroupedByScoreAndDegree.AEngCount
 */
function getStudentCountGroupedByScoreAndDegree() {
    var StudentCountGroupedByScoreAndDegree = {};
    var scoreListLength = scoreList.length;
    for (var scoreIndex = 0; scoreIndex < scoreListLength; scoreIndex++) {
        var score = scoreList[scoreIndex];
        var degreeListLength = degreeList.length;
        var scoreBucketListItems = ScoreBuckets.filter("." + score).find("li:not(.list-heading)");
        StudentCountGroupedByScoreAndDegree[score + "TotalCount"] = scoreBucketListItems.length;
        for (var degreeIndex = 0; degreeIndex < degreeListLength; degreeIndex++) {
            var degree = degreeList[degreeIndex];
            var degreeCountInBucket = scoreBucketListItems.filter("." + degree).length;
            StudentCountGroupedByScoreAndDegree[score + degree + "Count"] = degreeCountInBucket;
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

/**
 * @function Adds a reject reason text area list item to the unordered list for displaying reject reasons input.
 * @param {Integer} studentId The id of the student who is rejected.
 * @param {String} fullName The full name of the student who is rejected.
 */
function addRejectReasonForStudent(studentId, fullName) {
    $(getListElementForRejectReasonForStudent(studentId, fullName)).appendTo("#ulRejectReasons");
}

/**
 * @function Removes the reject reason for the student who has been removed from the reject bucket.
 * @param {Integer} stduentId The id of the student for whom the reject reason textarea is to be removed.
 */
function removeRejectReasonForStudent(studentId) {
    $("#liRejectReason_" + studentId).remove();
}
/**
* @function Builds the list item that will have the text area to enter the reject reason for the selected student.
* @param {Integer} studentId The id of the student who is rejected.
* @param {String} fullName The full name of the student who is rejected.
*/
function getListElementForRejectReasonForStudent(studentId, fullName){
    var listItemHtmlString = '<li id="liRejectReason_' + studentId + '" class="RejectReason">\n';
    listItemHtmlString += '\t<label id="lblReject_' + studentId + '"  for="txtReject' + studentId + '">*Please enter the reason for rejecting' + fullName + '</label>\n';
    listItemHtmlString += '\t<textarea id="txtReject_' + studentId + '" class="RejectReason" rows="4" cols="80"></textarea>\n';
    listItemHtmlString += '</li>';
    return listItemHtmlString;
}