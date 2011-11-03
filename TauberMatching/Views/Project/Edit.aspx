<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TauberMatching.Models.Project>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit</h2>

    <% using (Html.BeginForm()) {%>
        <%--<%: Html.ValidationSummary(true) %>--%>
        
        <fieldset>
            <legend>Fields</legend>
            <%: Html.HiddenFor(model=>model.Id) %>
            
            <div class="editor-label">
                <span style="color: red;">*</span><%: Html.LabelFor(model => model.Name) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Name) %>
                <%: Html.ValidationMessageFor(model => model.Name) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Comments) %>
            </div>
            <div class="editor-field">
                <%: Html.TextAreaFor(model => model.Comments, new { style = "width: 250px;" })%>
                <%: Html.ValidationMessageFor(model => model.Comments) %>
            </div>  
                       
            <div class="editor-label">
                <%: Html.LabelFor(model => model.ContactFirst) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.ContactFirst) %>
                <%: Html.ValidationMessageFor(model => model.ContactFirst) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.ContactLast) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.ContactLast) %>
                <%: Html.ValidationMessageFor(model => model.ContactLast) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.ContactEmail) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.ContactEmail) %>
                <%: Html.ValidationMessageFor(model => model.ContactEmail) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.ContactPhone) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.ContactPhone) %>
                <%: Html.ValidationMessageFor(model => model.ContactPhone) %>
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

