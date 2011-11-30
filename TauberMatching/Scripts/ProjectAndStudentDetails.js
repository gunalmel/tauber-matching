/**
* @fileOverview Js file to help user interaction on Project and Student Details page to let user match students with a project.
* @author Melih Gunal 11/28/2011
*/
var selInterviewed;
var selNotInterviewed;
//Before submission select all options in the secodn select box to submit matchings
$(function () {
    selInterviewed = $('#selectInterviewed');
    selNotInterviewed = $('#selectNotInterviewed');
    $('#btnSubmit').click(function () {
        var selObjOptions = $('#selectInterviewed option');
        for (i = 0; i < selObjOptions.length; i++) {
            selObjOptions[i].selected = true;
        }
    });
    $('#btnMoveFromNotInterviewed').click(function () {
        selNotInterviewed.find('option:selected').remove().appendTo(selInterviewed);
        sortSelect(selInterviewed[0]);
    });
    $('#btnMoveFromInterviewed').click(function () {
        selInterviewed.find('option:selected').remove().appendTo(selNotInterviewed);
        sortSelect(selNotInterviewed[0]);
    });
    $('#btnClearInterviewed').click(function () {
        selInterviewed.find('option').remove().appendTo(selNotInterviewed);
        sortSelect(selNotInterviewed[0]);
    });
});

function sortSelect(selElem) {
    var tmpAry = new Array();
    for (var i = 0; i < selElem.options.length; i++) {
        tmpAry[i] = new Array();
        tmpAry[i][0] = selElem.options[i].text;
        tmpAry[i][1] = selElem.options[i].value;
    }
    tmpAry.sort();
    while (selElem.options.length > 0) {
        selElem.options[0] = null;
    }
    for (var i = 0; i < tmpAry.length; i++) {
        var op = new Option(tmpAry[i][0], tmpAry[i][1]);
        selElem.options[i] = op;
    }
    return;
}