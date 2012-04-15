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

 /** 0th argument is the minumum number constraint for the students with a degree specified by 1st argument (engineering, business) for the rank category specified by the 3rd argument. 2nd argument is used to pluralize student word if the number specified by oth argument is greater than 1*/
var rankingMinCountErrorMsg = 'You have to pick at least {0} {1}student{2} as "{3}"{4}';
var rankingMinCountErrorMsgSuffix = '.\n';
var rankingMinTotalCountErrorMsgSuffix = ' in total.\n'

function getRankingValidationErrorMsgTextForMinNumberConstraints(minNumber, degree, ranking, total) {
    var rankingName = ranking=="A"?"Ideal":(ranking=="B"?"Desired":(ranking=="C"?"Acceptable":"Reject"));
    var msg="";
    if (total)
        msg = rankingMinCountErrorMsg.format(minNumber, "", (minNumber > 1 ? "s" : ""), rankingName, rankingMinTotalCountErrorMsgSuffix);
    else
        msg = rankingMinCountErrorMsg.format(minNumber, degree+" ", (minNumber > 1 ? "s" : ""), rankingName, rankingMinCountErrorMsgSuffix);
    return msg;
}
/** If EnforceContinuousStudentRanking then the Error message to be displayed when ranking scheme is sparse */
var sparseRankingErrorMessage = 'When you are ranking students, your ranking scheme should not be sparse, e.g.: If there are students in the "Ideal" and "Aceptable" boxes when there are no students in the "Desired" box that is an error.\n';

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
    }
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
    var studentCountMap = getStudentCountGroupedByScoreAndDegree();
    divUserErrors.html("");
    var AError = checkForAStudentError(studentCountMap);
    var BError = checkForBStudentError(studentCountMap);
    var isContinuousError = isRankingContinuous();
    var isRejectReasonEmptyError = validateRejectReason();
    var areAllStudentsRanked = checkIfAllStudentsRanked();
    var error = new UIError(AError.isError || BError.isError || isContinuousError.isError || isRejectReasonEmptyError.isError || areAllStudentsRanked.isError, "");
    error.errorMessage = AError.errorMessage + BError.errorMessage + isContinuousError.errorMessage + isRejectReasonEmptyError.errorMessage + areAllStudentsRanked.errorMessage;
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
function checkForAStudentError(stCountMap) {
    var aEngStudentCount = stCountMap.AEngCount;
    var aBusStudentCount = stCountMap.ABusCount;
    var aStudentCount = aEngStudentCount + aBusStudentCount;

    var error = new UIError(false, "");

    MinAEngStudents = (MinAEngStudents > StudentCount.Eng) ? StudentCount.Eng : MinAEngStudents;
    MinABusStudents = (MinABusStudents > StudentCount.Bus) ? StudentCount.Bus : MinABusStudents;
    MinAStudents = (MinAStudents > StudentCount.All) ? StudentCount.All : (MinAStudents > (MinAEngStudents + MinABusStudents) ? MinAStudents : (MinAEngStudents + MinABusStudents));

    var isAEngRequired = (MinAEngStudents > 0 && StudentCount.Eng > 0 && aEngStudentCount < StudentCount.Eng );
    var isABusRequired = (MinABusStudents > 0 && StudentCount.Bus > 0 && aBusStudentCount < StudentCount.Bus);
    var isARequired = (MinAStudents > 0 && StudentCount.Bus > 0 && aStudentCount < StudentCount.All);

    var noValidationRequired = (!isAEngRequired && !isABusRequired && !isARequired);

    if (noValidationRequired) {
        return error;
    } else {
        if (isAEngRequired && aEngStudentCount < MinAEngStudents) {
            error.isError = true;
            error.errorMessage += getRankingValidationErrorMsgTextForMinNumberConstraints(MinAEngStudents,"engineering","A",false);
        }
        if (isABusRequired && aBusStudentCount < MinABusStudents) {
            error.isError = true;
            error.errorMessage += getRankingValidationErrorMsgTextForMinNumberConstraints(MinABusStudents, "business", "A", false);
        }
        if (isARequired > 0 && aStudentCount < MinAStudents) {
            error.isError = true;
            error.errorMessage += getRankingValidationErrorMsgTextForMinNumberConstraints(MinAStudents, "", "A", true);
        }
    }
    return error;
}

/**
* @function Checks if the required min # of eng, bus and total students are assigned B
* @param isAError If there are enough eng/bus and in total students in A then when there's only one eng/bus student validation should automatically pass because it can not be forced that there should be at least 1 eng and 1 us student in B
* @returns {UIError} Returns an UIError object to indicate whether the validation result is an error, if it is then errorMessage property is set to the error message to be displayed.
*/
function checkForBStudentError(stCountMap) {
    var bEngStudentCount = stCountMap.BEngCount;
    var bBusStudentCount = stCountMap.BBusCount;
    var bStudentCount = bEngStudentCount + bBusStudentCount;
    var engStudents = StudentCount.Eng;
    var busStudents = StudentCount.Bus;

    var error = new UIError(false,"");

    var engStudentsLeftAfterA = (engStudents - MinAEngStudents)>0?(engStudents - MinAEngStudents):0;
    var busStudentsLeftAfterA = (busStudents - MinABusStudents)>0?(busStudents - MinAEngStudents):0;
    var totalStudentsLeftAfterA = engStudentsLeftAfterA+busStudentsLeftAfterA;

    var isBEngRequired = (MinBEngStudents > 0 && engStudentsLeftAfterA > 0 && engStudentsLeftAfterA > bEngStudentCount);
    var isBBusRequired = (MinBBusStudents > 0 && busStudentsLeftAfterA > 0 && busStudentsLeftAfterA > bBusStudentCount);
    var isBRequired = (MinBStudents > 0 && totalStudentsLeftAfterA > 0 && totalStudentsLeftAfterA > bStudentCount);

    MinBEngStudents = (MinBEngStudents < engStudentsLeftAfterA) ? MinBEngStudents : engStudentsLeftAfterA;
    MinBBusStudents = (MinBBusStudents < busStudentsLeftAfterA)?MinBBusStudents:busStudentsLeftAfterA;
    MinBStudents = (MinBStudents < totalStudentsLeftAfterA) ? MinBStudents : totalStudentsLeftAfterA;

    var noValidationRequired = (!isBEngRequired && !isBBusRequired && !isBRequired);

    if (noValidationRequired) {
        return error;
    } else {
        if (isBEngRequired && MinBEngStudents > bEngStudentCount) {
            error.isError = true;
            error.errorMessage += getRankingValidationErrorMsgTextForMinNumberConstraints(MinBEngStudents, "engineering", "B", false);
        }
        if (isBBusRequired && MinBBusStudents > bBusStudentCount) {
            error.isError = true;
            error.errorMessage += getRankingValidationErrorMsgTextForMinNumberConstraints(MinBBusStudents, "business", "B", false);
        }
        if (isBRequired && MinBStudents > bStudentCount) {
            error.isError = true;
            error.errorMessage += getRankingValidationErrorMsgTextForMinNumberConstraints(MinBStudents, "", "B", true);
        }
    }
    return error;
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
       return error;
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