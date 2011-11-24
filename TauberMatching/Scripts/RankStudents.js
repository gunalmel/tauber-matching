
/*
Score containers'(<ul> elements) hiddenfields as JQuery objects which store the Student ids in a csv format are named after hidden field element ids and are injected into the page using MVC scriplets in the view (hf<ScoreContainerId>)
NoScore, hfNoScore, hfA, hfB, hfC, hfX
*/

var mainList;
var hfA;
var hfB;
var hfC;
var hfX;
var maxEngReject = 1; // Maximum number of engineering students that can be rejected.
var maxBusReject = 1;
$("document").ready(function () {
mainList = $("#mainList");
hfA = $("#hfA");
hfB = $("#hfB");
hfC = $("#hfC");
hfX = $("#hfX");
})
$(function () {
$("ul.droptrue").sortable({
connectWith: "ul",
items: "li:not(.list-heading)",
remove: onRemoved,
receive: onReceived
});

$(".droptrue").disableSelection();
});

function onRemoved(event, ui) {
var degree = ui.item.attr("class");

var targ;
if (event.target)
targ = event.target;
else if (event.srcElement)
targ = event.srcElement;
var engCount = $(targ).attr("eng");
var busCount = $(targ).attr("bus");
if (degree == "Bus") {
if (busCount > 0)
$(targ).attr("bus", (busCount - 1));
}
if (degree == "Eng") {
if (engCount > 0)
$(targ).attr("eng", (engCount - 1));
}
}

function onReceived(event, ui) {
var mainEngCount = parseInt(mainList.attr("eng"));
var mainBusCount = parseInt(mainList.attr("bus"));

var degree = ui.item.attr("class");
    
var targ;
if (event.target)
targ = event.target;
else if (event.srcElement)
targ = event.srcElement;

var engCount = parseInt($(targ).attr("eng"));
var busCount = parseInt($(targ).attr("bus"));
if (degree == "Bus") {
// If the receiver is reject box check that it has at most 1 eng and 1 bus student.
if (targ.id == "X" && busCount >= maxBusReject) {
var item = $(targ).find(".Bus").not(ui.item);
mainList.find("li.list-heading").after(item);
mainList.attr("bus", mainBusCount + 1);
//$(ui.sender).sortable("cancel");
}
else {
$(targ).attr("bus", (busCount + 1));
}
}
if (degree == "Eng") {
// If the receiver is reject box check that it has at most 1 eng and 1 bus student.
if (targ.id == "X" && engCount >= maxEngReject) {
var item = $(targ).find(".Eng").not(ui.item);
mainList.find("li.list-heading").after(item);
mainList.attr("eng", mainEngCount + 1);
//$(ui.sender).sortable("cancel");
}
else {
$(targ).attr("eng", (engCount + 1));
}
}
}
/* function to dynamically display and hide reject reason text boxes.
function displayReject(listItem, degree,isVisible) {
var id = listItem.attr("id");
var text = listItem.text();
var student = text.substring(0, text.indexOf("(")).trim();
if (isVisible) {
if (degree == "Eng") {
$("#lblRejectEng").html("Reason for rejecting " + student);
$("#hfRejectEng").val(id);
$("#rejectRowEng").show();
}
else {
$("#lblRejectBus").html("Reason for rejecting " + student);
$("#hfRejectBus").val(id);
$("#rejectRowBus").show();
}
}
else {
if (degree == "Eng") {
$("#lblRejectEng").html("");
$("#hfRejectEng").val("");
$("#rejectRowEng").hide();
}
else {
$("#lblRejectBus").html("");
$("#hfRejectBus").val("");
$("#rejectRowBus").hide();
}
}
}
*/
function validate() {
    var msg = "";
    var engTotal = parseInt($("#hfEngTotal").val());
    var busTotal = parseInt($("#hfBusTotal").val());
    var engUnranked = parseInt(mainList.attr("eng"));
    var busUnranked = parseInt(mainList.attr("bus"));
    var aEng = parseInt($("#A").attr("eng"));
    var aBus = parseInt($("#A").attr("bus"));
    
    var bEng = parseInt($("#B").attr("eng"));
    var bBus = parseInt($("#B").attr("bus"));
    
    var cEng = parseInt($("#C").attr("eng"));
    var cBus = parseInt($("#C").attr("bus"));

    var isValid = true;
    // What happens if there is only one business/engineering student and that student is to be rejected. Would still #1 rule apply (There should be at least one eng one bus student in A)
    if (engTotal>0 && aEng < 1) {
        isValid = false;
        msg = "*You must rank at least one Engineer in the A category.<br/>";
    }
    if (busTotal>0 && aBus < 1) {
        isValid = false;
        msg += "*You must rank at least one Business student in the A category.<br/>";
    }
    if (engUnranked > 0 || busUnranked > 0) {
        isValid = false;
        msg += "*You must rank all the students you interviewed.<br/>";
    }
    var invalidBus1 = (aBus > 0 && bBus < 1 && cBus > 0); //101
    var invalidBus2 = (aBus < 1 && bBus > 0 && cBus < 1); //010
    var invalidBus3 = (aBus < 1 && bBus < 1 && cBus > 0); //001
    var invalidBus4 = (aBus < 1 && bBus > 0 && cBus > 0); //011
    var invalidBus = invalidBus1 || invalidBus2 || invalidBus3 || invalidBus4;
    if (invalidBus) {
        isValid = false;
        msg += "*Ranking of Business students should be continuous (e.g., if you want to rank a student in the 'C' category, you must have already ranked students in the 'A' and 'B' category).<br/>";
    }
    var invalidEng1 = (aEng > 0 && bEng < 1 && cEng > 0); //101
    var invalidEng2 = (aEng < 1 && bEng > 0 && cEng < 1); //010
    var invalidEng3 = (aEng < 1 && bEng < 1 && cEng > 0); //001
    var invalidEng4 = (aEng < 1 && bEng > 0 && cEng > 0); //011
    var invalidEng = invalidEng1 || invalidEng2 || invalidEng3 || invalidEng4;
    if (invalidEng) {
        isValid = false;
        msg += "*Ranking of Engineering students should be continuous (e.g., if you want to rank a student in the 'C' category, you must have already ranked students in the 'A' and 'B' category).<br/>";
    }

    if (!isValid) {
        $("#error").html(msg);
    }
    var AIds = "";
    var BIds = "";
    var CIds = "";
    var XIds = "";
    $("#hfRejectEng").val('');
    $("#hfRejectBus").val('');
    $("#A li:not(.list-heading)").each(function () {
        AIds += AIds == "" ? this.id : ("," + this.id);
    });
    $("#B li:not(.list-heading)").each(function () {
        BIds += BIds == "" ? this.id : ("," + this.id);
    });
    $("#C li:not(.list-heading)").each(function () {
        CIds += CIds == "" ? this.id : ("," + this.id);
    });
    $("#X li:not(.list-heading)").each(function () {
        XIds += XIds == "" ? this.id : ("," + this.id);
        if ($(this).attr("class") == "Eng")
            $("#hfRejectEng").val(this.id);
        else if ($(this).attr("class") == "Bus")
            $("#hfRejectBus").val(this.id);
    });

    hfA.val(AIds);
    hfB.val(BIds);
    hfC.val(CIds);
    hfX.val(XIds);

    return isValid;
}
function displayWait() {
    $('#btnSubmit').hide();
    $('#sWait').show();
}