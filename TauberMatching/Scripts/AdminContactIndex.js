/**
* @fileOverview Js function implementations that are used to handle UI interactions on index pages for Project and Student admin pages.
* @author Melih Gunal 12/04/2011
* Global variable are set on the page explicitly as listed below:
* var ajaxUrl; The url for the server side web service method as resolved on the server side from relative path.
* var ColumnHeaderForContactName; The table column header text for displaying the contact full name. It will be used as a parameter to search within the selected row to extract contact first and last name.
* var ColumnHeaderForContactEmail; The table column header text for displaying the contact email address. It will be used as a parameter to search within the selected row to extract contact email. 
* var ContactType; Maps to the string representation of values in ContacType enum Project|Student.
*/

/**
* @class Data transfer object to transfer contacts to be emailed to the server side method.
* @param {Integer} id contact identifier for the selected contact to be emailed.
* @param {String} guid the unique access url identifier for the selected contact.
* @param {String} firstName First name of the contact to be emailed.
* @param {String} lastName Last name of the contact to be emailed.
* @param {String} contactType The ContactType enum string value assigned to contact type. Student|Project.
*/
function Contact(id, guid, email, firstName, lastName, contactType) {
    this.Id = id; this.Guid = guid; this.Email = email, this.FirstName = firstName; this.LastName = lastName; this.ContactType = contactType;
}
//Flag to denote if any of the listed items has been e-mailed before
var duplicateSendEmail = false;
var alreadyEmailedMessage = "Some of your selections has already been e-mailed before. Are you sure you'd like to e-mail the previously e-mailed contacts?";
var contactCheckBoxesForWhichThereAreNoMatchings;
$(document).ready(function () {
    $.ajaxSetup({ type: "POST", contentType: "application/json;charset=utf-8", dataType: "json", processData: false });
    contactCheckBoxesForWhichThereAreNoMatchings = $("input[type=hidden][id*=hfIsThereAnyMatchings][value='false']").parent().find("input[type=checkbox][id*=chkSelect]");
    contactCheckBoxesForWhichThereAreNoMatchings.attr('disabled', 'disabled');
    // Make the header checkbox select all checkbox
    $("#chkAll").live("click", function () { var checked = this.checked; $("input[type=checkbox][id*=chkSelect]").not(contactCheckBoxesForWhichThereAreNoMatchings).attr('checked', checked); });
});

/**
 * @function Sends the ajax request for sending access url notification emails to the selected contacts. If the contacts selected were already e-mailed befroe a confirmation message will notify user before actually sending out the -emails to the selected contacts.
 */
function sendMail() {
    var list = getSelected();
    var answer = true;
    if (duplicateSendEmail)
        answer = confirm(alreadyEmailedMessage);
    if (!answer)
        return;
    if (list == "")
        return;
    var paramString = $.toJSON({ contacts: list });
    $.ajax({
        url: ajaxUrl,
        data: paramString,
        beforeSend: beforeEmailSend,
        success: onEmailSuccess,
        error: onError,
        async: false
    });
}
function beforeEmailSend() {
    grayOut(true);
    $("#divWait").toggle();
}
function onEmailSuccess(msg, event, xhr) {
    $("#divWait").toggle();
    grayOut(false);
    $("#divMessage").css("color", "black");
    // Reloads the page by resetting the selected page in pagination so that grid will be bound to fresh data
    $("input[type=checkbox][id*=chkSelect]:checked").attr('checked', false);
    $("#chkAll").attr('checked', false);
    window.location = location.href;
}
function onError(xhr, error) {
    grayOut(false);
    $("#divMessage").css("color", "red");
    $("#divMessage").html(xhr.status + ": " + xhr.statusText + " " + xhr.responseText);
    $("#divWait").toggle();
}
/**
 * @function Finds the checked checkboxes used to select the entries in the list and returns the list of contact objects to pass to the web service method
 */
function getSelected() {
    duplicateSendEmail = $("input[type=checkbox][id*=chkSelect]:checked").parent().parent().find("input[type=checkbox][id*=chkEmailed]:checked").length > 0;
    var selectedRows = $("input[type=checkbox][id*=chkSelect]:checked").parent().parent();
    var contactArray = new Array();
    selectedRows.each(function (index) {
        contactArray[index] = extractContactFromTableRow($(this), ColumnHeaderForContactName, ColumnHeaderForContactEmail, ContactType);
    });
    return contactArray;
}
/**
 * @function Instantiates the contact object from the selected row from the table of contacts that will be put into the list of contacts to be e-mailed.
 * @param {Object} row - JQuery object representing the table row selected
 * @param {String} colHeadForContact
 * @param {String} colHeadForEmail
 * @returns {Contact}
 */
function extractContactFromTableRow(row, colHeadForContact, colHeadForEmail, contactType) {
    var id = row.find("[id*=hfId]").val();
    var guid = row.find("[id*=hfGuid]").val();
    var names = extractTextFromTableRowColumn(row, colHeadForContact).split(' ');
    var email = extractTextFromTableRowColumn(row, colHeadForEmail);
    return new Contact(id, guid, email, names[0], names[1], contactType);
}
/**
 * @function Given a table row and column header, extracts the text that is appearing in the column.
 * @param {Object} row - JQuery object
 * @param {String} colHeader
 * returns {String}
 */
function extractTextFromTableRowColumn(row, colHeader) {
    var indexOfTheTargetColumn;
    $("table tr:first-child th").each(function (index) {
        if ($(this).text() == colHeader) {
            indexOfTheTargetColumn = index;
            return false;
        }
    });
    var colText = row.find("td:eq(" + indexOfTheTargetColumn + ")").text();
    return colText;
}