<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TauberMatching.Models.Student>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit</h2>

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>
        
        <fieldset>
            <legend>Fields</legend>
            
            <%: Html.HiddenFor(model=>model.Id) %>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.UniqueName) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.UniqueName) %>
                <%: Html.ValidationMessageFor(model => model.UniqueName) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.FirstName) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.FirstName) %>
                <%: Html.ValidationMessageFor(model => model.FirstName) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.LastName) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.LastName) %>
                <%: Html.ValidationMessageFor(model => model.LastName) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Degree) %>
            </div>
            <div class="editor-field">
                <%foreach (String item in ViewData["degrees"] as IEnumerable)
                  { %>
                    <%: Html.RadioButtonFor(model=>model.Degree,item) %><%:item %>
                <%} %>
                <%: Html.ValidationMessageFor(model => model.Degree) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Email) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Email) %>
                <%: Html.ValidationMessageFor(model => model.Email) %>
            </div>
                       
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Comments) %>
            </div>
            <div class="editor-field">
                <%: Html.TextAreaFor(model => model.Comments, new { style = "width: 250px;" })%>
                <%: Html.ValidationMessageFor(model => model.Comments) %>
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

