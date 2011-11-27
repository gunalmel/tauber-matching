/**
 * @fileOverview This file has functions, classes and variables related to implementing user interaction on RankProjects view.
 * @author <a href="mailto:gunalmel@yahoo.com">Melih Gunal</a> 11/27/2011
 * The following varibales are dynamically added as they are fetched from the db by the controller into the view. The values displayed below are for providing an example only. The actual values on the UI might be different.
 * var scoreList = ["NoScore","1","2","3","4","5","Reject"];
 * var MaxRejectedProjects = 1;
 * var RejectedProjectThreshold = 5;
 * var EnforceContinuousProjectRanking = true;
 * var MinFirstProjects = 1;
 * var webServiceUrlToSubmit, AdminPhone, AdminEmail is set in the view.
 */
/** Error message to be displayed when the user rejects more engineering students than the number specified by MinAEngStudents */
var minFirstProjectsErrorMessage = "You have to select at least " + MinFirstProjects + " project" + (MinFirstProjects > 1 ? "s" : "")+ " as your first preference.";
/** If EnforceContinuousProjectRanking then the error message to be displayed when ranking scheme is sparse */
var sparseRankingErrorMessage = "When you are ranking projects, your ranking scheme should not be sparse, e.g.: If there are projects in First and Fourth when there are no projects in Second and Third that's an error.\n";
/** If the student tries to reject a project when in total there are less than RejectedProjectThreshold projects */
var minTotalProjectsToRejectViolationErrorMessage = "You should have interviewed at least " + RejectedProjectThreshold + " projects to be able to reject a project\n";
var maxTotalProjectsToRejectViolationErrorMessage = "Maximum # of projects you can reject is: " + MaxRejectedProjects + "\n";
var notAllProjectsRankedErrorMessage = "There are projects which are not ranked. All projects should be ranked.\n";

var studentId; 
var studentGuid;

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
var ProjectCount;

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
 * @class Data transfer object to transfer student preferences from UI to web service
 * @see StudentPreferenceDto
 */
function StudentScoreDto(projectId, score) {
    this.ProjectId = projectId;
    this.Score = score;
}
/**
 * @class Data transfer object to transfer student feedback from UI to web service.
 * @see StudentPreferenceDto
 */
function StudentFeedbackDto(projectId, type, feedbackScore) {
    this.ProjectId = projectId;
    this.Type = type;
    this.FeedbackScore = feedbackScore;
}

/**
 * @class Data transfer object to transfer all the data on the UI necessary to persist project preferences through web service call.
 * @param {Integer} studentId
 * @param {String} studentGuid Acts as a key for web service to accept the request together with studentId
 * @param studentPreferences Array of {@link StudentScoreDto} objects
 * @param studentFeedback Array of {@link StudentFeedbackDto} objects
 * @param otherComments {String} Student comments.
 */
function StudentPreferenceDto(studentId, studentGuid, studentPreferences, studentFeedback, otherComments) {
    this.ProjectId = projectId;
    this.ProjectGuid = projectGuid;
    this.ProjectPreferences = projectPreferences;
    this.StudentFeedback = studentFeedback;
    this.Feedback = feedback;
    this.OtherComments = otherComments;
}

/**
* @function Equivalent of JQuery $(document).ready() function call. Makes Ranking buckets sortable drag and drop containers using JQuery UI plug-in.
*           Initializes global variables to store ranking buckets.
*/
$(function () {
    $("ul.droptrue").sortable({
        connectWith: "ul.droptrue",
        items: "li:not(.list-heading)",
        receive: onReceived
    }).disableSelection();

    ScoreBuckets = $("ul.droptrue");
    ProjectCount = getTotalProjectCount();
    studentId = parseInt($("#hfStudentId").val());
    projectGuid = $("#hfStudentGuid").val();
    divUserErrors = $("#divUserErrors");
    $("#btnSubmit").click(onSubmit);
    $.ajaxSetup({ type: "POST", contentType: "application/json;charset=utf-8", dataType: "json", processData: false });
    RejectedProjectThreshold = -1;//Remove this line in production
    if (RejectedProjectThreshold != -1 && ProjectCount < RejectedProjectThreshold)
        $("#ul_Reject_Bucket").hide();
});

/* Helper Functions Start Here */

/**
* @function Returns the total number of projects on the UI
*/
function getTotalProjectCount() {
    return ScoreBuckets.find("li:not(.list-heading)").length;
}

/**
* @function Gets the number of projects within a given score bucket (ranking group)
* @param {String} score The ranking group (e.g. 1, 2)
*/
function getProjectCountForScore(score) {
    return ScoreBuckets.filter("." + score).find("li:not(.list-heading)").length;
}

/* Helper Functions End Here */

/* Drag and Drop Handling Starts Here */

/** 
* @function Event handler function for JQuery sortable drag and drop UI. Whenever the droppable element receives a draggable element this function is triggered before the drop action is finalized. Handles the real time validation to decide if a selected project can be rejected.
* @see checkForRejectedStudentError
*/
function onReceived(event, ui) {
    var receiver = ui.item.parent();
    if (receiver.attr("id") == "ul_Reject_Bucket") {
        var rejectError = checkForRejectedProjectError();
        if (rejectError.isError) {
            alert(rejectError.errorMessage);
            $(ui.sender).sortable("cancel");
        }
    }
}
/* Drag and Drop Handling Ends Here */

/********* VALIDATIONS START ***************/
/**
* @function Calls UI validation functions to run on the client side before calling server side web service to persist user preferences.
* @returns {UIError}
*/
function runAllValidations() {
    divUserErrors.html("");
    var FirstProjectError = checkForAStudentError();
    var isContinuousError = isRankingContinuous();
    var areAllProjectsRanked = checkIfAllProjectsRanked();
    var error = new UIError(FirstProjectError.isError || isContinuousError.isError || areAllProjectsRanked.isError, "");
    error.errorMessage = FirstProjectError.errorMessage + isContinuousError.errorMessage + areAllProjectsRanked.errorMessage;
    error.errorMessage = error.errorMessage.replace(/\n/g, '<br/>').replace(/  ,/g, '<br/>');
    divUserErrors.html(error.errorMessage);
    return error;
}

/**
* @function Checks if the project ranking scheme is sparse. e.g.: It returns an UIError object whose isError is set to true with appropriate errorMessage if there are projects in FIRST and THIRD group when there are no students in  SECOND group
* @returns {UIError} Returns UIError object indicating if there's an error message and associated error message.
*/
function isRankingContinuous() {
    var positionOfTheLastBucketWithProjects = -1;
    var positionOfTheNextBucketWithProjects = 0;
    var isContinuous = true;
    var error = new UIError(!isContinuous, "");
    if (!EnforceContinuousProjectRanking)
        return new UIError(isContinuous, "");
    ScoreBuckets.filter(":not(#ul_NoScore_Bucket,#ul_Reject_Bucket)").each(function (index) {
        var projectCountInTheBucket = $(this).find("li:not(.list-heading)").length;
        positionOfTheNextBucketWithProjects = projectCountInTheBucket > 0 ? index : positionOfTheNextBucketWithProjects;
        if (projectCountInTheBucket > 0) {
            if ((positionOfTheNextBucketWithProjects - positionOfTheLastBucketWithProjects) > 1) {
                isContinuous = false;
                error.isError = !isContinuous;
                error.errorMessage = sparseRankingErrorMessage;
            }
            positionOfTheLastBucketWithProjects = positionOfTheNextBucketWithProjects;
        }
    });
    return error;
}

/** 
* @function Checks if more projects than specified by configuration parameters have been rejected and returns UIError object accordingly
* @returns {UIError} UIError object indicating if there's an error rejecting a project and  including error message accordingly.
*/
function checkForRejectedProjectError() {

    var rejectedProjectCount = getProjectCountForScore("Reject");

    var error = new UIError();
    if (RejectedProjectThreshold != -1 && ProjectCount < RejectedProjectThreshold && rejectedProjectCount > 0) { //Students can reject projects only when they interviewed more than certain # of projects
        error.isError = true;
        error.errorMessage = minTotalProjectstsToRejectViolationErrorMessage;
    }
    else if (MaxRejectedProjects != -1 && rejectedProjectCount > MaxRejectedProjects) {
        error.isError = true;
        error.errorMessage = maxTotalProjectsToRejectViolationErrorMessage;
    }
    else {
        error.isError = false;
        error.errorMessage = "";
    }
    return error;
}

/**
* @function Checks if the user has scored all the projects
* @returns {UIError}
*/
function checkIfAllProjectsRanked() {
    if ($("#ul_NoScore_Bucket li:not(.list-heading)").length > 0)
        return new UIError(true, notAllProjectsRankedErrorMessage);
    return new UIError(false, "");
}

/**
* @function Checks if the required min # of projects are picked as first choice
* @returns {UIError} Returns an UIError object to indicate whether the validation result is an error, if it is then errorMessage property is set to the error message to be displayed.
*/
function checkForFirstChoiceProjectError() {

    var firstChoiceProjectCount = getProjectCountForScore("1");
    var isError = false;
    var errorMessage = "";
    if (MinFirstProjects != -1 && firstChoiceProjectCount < MinFirstProjects && ProjectCount > 0) {
        isError = true;
        errorMessage += minFirstProjectsErrorMessage;
    }
    return new UIError(isError, errorMessage);
}



/*****VALIDATIONS END*********/

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
    alert("Your changes are successfully submitted & saved.");
    // Reloads the page.
    window.location = location.href;
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
    divUserErrors.focus();
    $("#divWait").toggle();
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
* @function Builds the ProjectRankingDto object from the dat on the UI.
* @see ProjectRankingDto
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

