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
 */
/** Error message to be displayed when the user rejects more engineering students than the number specified by MinAEngStudents */
var engStudentsErrorMessage = "You have to assign A to at least " + MinAEngStudents + " engineering student" + (MinAEngStudents > 1 ? "s.\n" : ".\n");
/** Error message to be displayed when the user rejects more business students than the number specified by MinABusStudents */
var busStudentsErrorMessage = "You have to assign A to at least " + MinABusStudents + " business student" + (MinABusStudents > 1 ? "s.\n" : ".\n");
/** Error message to be displayed when the user rejects more students than the number specified by MinAStudents */
var allStudentsErrorMessage = "You have to assign A to at least " + MinAStudents + " student" + (MinAStudents > 1 ? "s.\n" : ".\n");
/** Error message to be displayed when ranking scheme is sparse */
var sparseRankingErrorMessage = "When you are ranking students, your ranking scheme should not be sparse, e.g.: If there are students in A and C when there are no students in B that's an error.";

var minTotalStudentsToRejectViolationErrorMessage = "You should have interviewed at least " + RejectedStudentThreshold + " students to be able to reject any students";
var maxEngStudentsToRejectViolationErrorMessage = "Maximum # of Engineering students you can reject is: " + MaxRejectedEngStudents;
var maxBusStudentsToRejectViolationErrorMessage = "Maximum # of Business students you can reject is: " + MaxRejectedBusStudents;
var maxTotalStudentsToRejectViolationErrorMessage = "Maximum # of students you can reject is: " + MaxRejectedStudents;
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
 * @function Equivalent of JQuery $(document).ready() function call. Makes Ranking buckets sortable drag and drop containers using JQuery UI plug-in.
 *           Initializes global variables to store ranking buckets and student counts grouped by degree on the interface
 */
$(function () {
    $("ul.droptrue").sortable({
        connectWith: "ul",
        items: "li:not(.list-heading)",
        receive: onReceived
    });
    $(".droptrue").disableSelection(); // Do not let the draggable li items to be text selectable
    ScoreBuckets = $("ul.droptrue");
    StudentCount = getTotalStudentCountByDegree();
});

/** 
 * @function Event handler function for JQuery sortable darg and drop UI. If score names or degree names are changed this function need to be updated.
 * Whenever the droppable elemnt receives a draggable element this function is triggered before the drop action is finalized. Handles the real time validation to decide if a selected student can be rejected.
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
        // alert("Eng:"+rejectedEngStudentCount+" Bus:"+rejectedBusStudentCount);
    }
    else {
    }
    //alert(isRankingContinuous().isError+" "+isRankingContinuous().errorMessage);
    //alert(checkForAStudentError().errorMessage);
    //alert(runAllValidations().errorMessage);
}

/* UI Validation Functions Starts Here.*/

/**
 * @function Calls UI validation functions to run on the client side before calling server side web service to persist user preferences.
 * @returns {UIError}
 */
function runAllValidations() {
    $("#divUserErrors").html("");
    var AError = checkForAStudentError();
    var isContinuousError = isRankingContinuous();
    var error = new UIError(AError.isError || isContinuousError.isError, "");
    error.errorMessage = (AError.isError ? AError.errorMessage : "") + (isContinuousError.isError ? isContinuousError.errorMessage : "");
    error.errorMessage = error.errorMessage.replace(/\n/g, '<br/>').replace(/  ,/g, '<br/>');
    $("#divUserErrors").html(error.errorMessage);
    return error;
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
    if (rejectedTotalStudentCount != -1 && StudentCount.All < RejectedStudentThreshold && rejectedTotalStudentCount > 0) { //Projects can reject students only when they interviewed more than certain # of students
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
        var studentCountInTheBucket = $(this).find("li.:not(.list-heading)").length;
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
    return ScoreBuckets.filter("." + score).find("li." + degree + ":not(.list-heading)").length; //$("#ul_"+score+"_Bucket li."+degree+":not(.list-heading)").length;
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