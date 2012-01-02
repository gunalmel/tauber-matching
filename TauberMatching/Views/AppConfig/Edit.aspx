﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TauberMatching.Models.AppConfiguration>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Tauber Matching Web Application - Edit Application Configuration
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptOrCssContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#spClearDatabase").bind("click", function () { return confirm("!!!!!!!! ATTENTION !!!!!!!!!! Are you sure you'd like to reset the database? That will clear all records from the database except the app setup and email config parameters."); });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.EnableClientValidation(); %>
    <h2>Edit Application Configuration</h2>
    <p>If you'd like to <b>delete all records</b> in the database except App Setup and Email Config parameters then click on <span id="spClearDatabase"><%: Html.ActionLink("Delete All Records (Students, Projects, Rankings)", "ClearDatabase", "AppConfig")%></span></p>
    <p>You can alter the business rules that are being forced upon projects and students when submitting their prefrences for each other.</p>
    <% =ViewContext.TempData["message"] %>
    <% using (Html.BeginForm()) {%>
        <%--<%: Html.ValidationSummary(true) %>--%>
        <fieldset>
            <legend>Configuration Parameters</legend>
            <p>* Site Master and contact information will be sent in the notification e-mails to the users to let them know whom to contact when they have any questions/concerns.</p>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.SiteMasterFirstName) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.SiteMasterFirstName) %>
                <%: Html.ValidationMessageFor(model => model.SiteMasterFirstName) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.SiteMasterLastName) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.SiteMasterLastName) %>
                <%: Html.ValidationMessageFor(model => model.SiteMasterLastName) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.SiteMasterEmail) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.SiteMasterEmail) %>
                <%: Html.ValidationMessageFor(model => model.SiteMasterEmail) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.SiteMasterPhone) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.SiteMasterPhone) %>
                <%: Html.ValidationMessageFor(model => model.SiteMasterPhone) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.MinABusStudents) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.MinABusStudents) %>
                <%: Html.ValidationMessageFor(model => model.MinABusStudents) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.MinAEngStudents) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.MinAEngStudents) %>
                <%: Html.ValidationMessageFor(model => model.MinAEngStudents) %>
            </div>

            <div class="editor-label">
                <%: Html.LabelFor(model => model.MinAStudents) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.MinAStudents) %>
                <%: Html.ValidationMessageFor(model => model.MinAStudents) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.MaxRejectedBusStudents) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.MaxRejectedBusStudents) %>
                <%: Html.ValidationMessageFor(model => model.MaxRejectedBusStudents) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.MaxRejectedEngStudents) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.MaxRejectedEngStudents) %>
                <%: Html.ValidationMessageFor(model => model.MaxRejectedEngStudents) %>
            </div>

            <div class="editor-label">
                <%: Html.LabelFor(model => model.MaxRejectedStudents) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.MaxRejectedStudents) %>
                <%: Html.ValidationMessageFor(model => model.MaxRejectedStudents) %>
            </div>

            <div class="editor-label">
                <%: Html.LabelFor(model => model.RejectedStudentThreshold) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.RejectedStudentThreshold) %>
                <%: Html.ValidationMessageFor(model => model.RejectedStudentThreshold) %>
            </div>

            <div class="editor-label">
                <%: Html.LabelFor(model => model.MinFirstProjects) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.MinFirstProjects)%>
                <%: Html.ValidationMessageFor(model => model.MinFirstProjects)%>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.MaxRejectedProjects) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.MaxRejectedProjects) %>
                <%: Html.ValidationMessageFor(model => model.MaxRejectedProjects) %>
            </div>

            <div class="editor-label">
                <%: Html.LabelFor(model => model.RejectedProjectThreshold) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.RejectedProjectThreshold) %>
                <%: Html.ValidationMessageFor(model => model.RejectedProjectThreshold) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.EnforceContinuousProjectRanking) %>
            </div>
            <div class="editor-field">
                <%: Html.RadioButtonFor(model=>model.EnforceContinuousProjectRanking,"True") %>Yes
                <%: Html.RadioButtonFor(model=>model.EnforceContinuousProjectRanking,"False") %>No
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.EnforceContinuousStudentRanking) %>
            </div>
            <div class="editor-field">
                <%: Html.RadioButtonFor(model => model.EnforceContinuousStudentRanking, "True")%>Yes
                <%: Html.RadioButtonFor(model => model.EnforceContinuousStudentRanking, "False")%>No
            </div>           
            <p>
                <input type="submit" value="Save" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%: Html.ActionLink("Back to Home Page", "Index") %>
    </div>

</asp:Content>

