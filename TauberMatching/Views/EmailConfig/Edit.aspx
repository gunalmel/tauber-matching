<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TauberMatching.Models.EmailConfiguration>" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Tauber Matching Web Application - Edit Email Configuration
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptOrCssContent" runat="server">
    <script src="<%=Url.Content("~/Scripts/ckeditor/ckeditor.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/ckeditor/adapters/jquery.js") %>" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //$('#ProjectAccessUrlEmailBody').ckeditor(); // Instantiating CKEditor using jquery plugin
            CKEDITOR.replace('ProjectAccessUrlEmailBody', { width: '75%', height: '100' });
            CKEDITOR.replace('StudentAccessUrlEmailBody', { width: '75%', height: '100' });
            CKEDITOR.replace('EmailHeader', { width: '75%', height: '100' });
            CKEDITOR.replace('EmailFooter', { width: '75%', height: '100' });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.EnableClientValidation(); %>
    <h2>Edit Email Configuration</h2>
    <% =ViewContext.TempData["message"] %>
    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>
        
        <fieldset>
            <legend>Configuration Parameters</legend>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.MailServer) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.MailServer) %>
                <%: Html.ValidationMessageFor(model => model.MailServer) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.MailServerPort) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.MailServerPort) %>
                <%: Html.ValidationMessageFor(model => model.MailServerPort) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.IsSSLEnabled) %>
            </div>
            <div class="editor-field">
                <%: Html.RadioButtonFor(model=>model.IsSSLEnabled,"True") %>Yes
                <%: Html.RadioButtonFor(model=>model.IsSSLEnabled,"False") %>No
                <%: Html.ValidationMessageFor(model => model.IsSSLEnabled) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.IsMailBodyHtml) %>
            </div>
            <div class="editor-field">
                <%: Html.RadioButtonFor(model=>model.IsMailBodyHtml,"True") %>Yes
                <%: Html.RadioButtonFor(model=>model.IsMailBodyHtml,"False") %>No
                <%: Html.ValidationMessageFor(model => model.IsMailBodyHtml) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.MailAccount) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.MailAccount) %>
                <%: Html.ValidationMessageFor(model => model.MailAccount) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.MailPassword) %>
            </div>
            <div class="editor-field">
                <%: Html.PasswordFor(model => model.MailPassword,new { @value = Model.MailPassword })%>
                <%: Html.ValidationMessageFor(model => model.MailPassword) %>
            </div>
            
            <div class="editor-label">
               <br /><%: Html.LabelFor(model => model.IsTesting) %><br />
            </div>
            <div class="editor-field">
                <%: Html.RadioButtonFor(model => model.IsTesting, "True")%>Yes
                <%: Html.RadioButtonFor(model => model.IsTesting, "False")%>No
                <%: Html.ValidationMessageFor(model => model.IsTesting) %>
            </div>
            
            <div class="editor-label">
                <br /><%: Html.LabelFor(model => model.EmailHeader) %><br />
                * {0} is the place holder for the contact full name. e.g.: If user full name is John Doe then "Dear {0}," will be displayed as "Dear John Doe,"
            </div>
            <div class="editor-field">
                <%: Html.TextAreaFor(model => model.EmailHeader, new { @id = "EmailHeader", @style = "width: 800px;height:50px" })%>
                <%: Html.ValidationMessageFor(model => model.EmailHeader) %>
            </div>
            
            <div class="editor-label">
                <br /><%: Html.LabelFor(model => model.EmailFooter) %><br />
                * If you click on "Source" in the toolbar you will see that the link for email address is built using the following parameters: {0}: Full name of the site admin, {1}: E-mail address of the site admin, {2}: Full name of the user who is contacting the site admin by following the link included in the notification e-mail, {3}: Phone# of the site admin
            </div>
            <div class="editor-field">
                <%: Html.TextAreaFor(model => model.EmailFooter, new { @id = "EmailFooter",@style = "width: 800px;height:50px" }) %>
                <%: Html.ValidationMessageFor(model => model.EmailFooter) %>
            </div>

            <div class="editor-label">
                <%: Html.LabelFor(model => model.ProjectAccessUrlEmailSubject) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.ProjectAccessUrlEmailSubject, new { @id = "ProjectAccessUrlEmailSubject", @style = "width: 800px;" })%>
                <%: Html.ValidationMessageFor(model => model.ProjectAccessUrlEmailSubject) %>
            </div>
            
            <div class="editor-label">
                <br /><%: Html.LabelFor(model => model.ProjectAccessUrlEmailBody) %><br />
                * If you click on "Source" in the toolbar you will see that the link is built using the following parameters: You should include &lt;a href="{0}"&gt;{0}&lt;/a&gt; within the body of the e-mail to display the access url link in the e-mail to the project contacts.
            </div>
            <div class="editor-field">
                <%: Html.TextAreaFor(model => model.ProjectAccessUrlEmailBody, new { @id="ProjectAccessUrlEmailBody"})%>
                <%: Html.ValidationMessageFor(model => model.ProjectAccessUrlEmailBody) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.StudentAccessUrlEmailSubject) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.StudentAccessUrlEmailSubject, new { @id = "StudentAccessUrlEmailSubject", @style = "width: 800px;" })%>
                <%: Html.ValidationMessageFor(model => model.StudentAccessUrlEmailSubject) %>
            </div>
            
            <div class="editor-label">
                <br /><%: Html.LabelFor(model => model.StudentAccessUrlEmailBody) %><br />
                * If you click on "Source" in the toolbar you will see that the link is built using the following parameters: You should include &lt;a href="{0}"&gt;{0}&lt;/a&gt; within the body of the e-mail to display the access url link in the e-mail to the students.
            </div>
            <div class="editor-field">
                <%: Html.TextAreaFor(model => model.StudentAccessUrlEmailBody, new { @id = "StudentAccessUrlEmailBody", @style = "width: 800px;height:50px" })%>
                <%: Html.ValidationMessageFor(model => model.StudentAccessUrlEmailBody) %>
            </div>
            <div id="divHiddenFields">
                <%:Html.HiddenFor(model=>model.InvalidAccessUrlMessage) %>
                <%:Html.HiddenFor(model=>model.MailPickupDirectory) %>
                <%:Html.HiddenFor(model=>model.RankProjectsController) %>
                <%:Html.HiddenFor(model => model.RankStudentsController)%>
            </div>
<%--            <div class="editor-label">
                <%: Html.LabelFor(model => model.InvalidAccessUrlMessage) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.InvalidAccessUrlMessage) %>
                <%: Html.ValidationMessageFor(model => model.InvalidAccessUrlMessage) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.MailPickupDirectory) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.MailPickupDirectory) %>
                <%: Html.ValidationMessageFor(model => model.MailPickupDirectory) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.RankProjectsController) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.RankProjectsController) %>
                <%: Html.ValidationMessageFor(model => model.RankProjectsController) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.RankStudentsController) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.RankStudentsController) %>
                <%: Html.ValidationMessageFor(model => model.RankStudentsController) %>
            </div>--%>
            
            <div class="editor-label">          
                <br /><%: Html.LabelFor(model => model.ConfirmationEmailReceivers) %><br />
                * Enter the list of e-mail addresses that will receive ranking confirmation e-mails that will also be sent to users to confirm their submissions.<br />
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.ConfirmationEmailReceivers, new { @id = "ConfirmationEmailReceivers", @style = "width: 600px;" })%>
                <%: Html.ValidationMessageFor(model => model.ConfirmationEmailReceivers) %>
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

