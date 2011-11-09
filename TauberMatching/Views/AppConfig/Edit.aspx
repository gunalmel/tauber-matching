<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TauberMatching.Models.AppConfiguration>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.EnableClientValidation(); %>
    <h2>Edit Application Configuration</h2>

    <p>You can lter the business rules that are being nforced upon projects and students when submitting their prefrences for each other.</p>
    <% =ViewContext.TempData["message"] %>
    <% using (Html.BeginForm()) {%>
        <%--<%: Html.ValidationSummary(true) %>--%>
        
        <fieldset>
            <legend>Configuration Parameters</legend>
            
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
        <%: Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>

