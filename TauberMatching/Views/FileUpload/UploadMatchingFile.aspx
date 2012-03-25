<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    UploadMatchingFile
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptOrCssContent" runat="server">
    <script src='<%=ResolveUrl("~/Scripts/UIFunctions.js")%>' type="text/javascript"></script>
    <script type="text/javascript">
        var divMessage;
        var btnSubmit;
        var divWait;
        $(function () {
            divMessage = $("#divMessage");
            btnSubmit = $("#btnSubmit");
            divWait = $("#divWait");
            btnSubmit.live('click', beforeSubmit);
        });
        function beforeSubmit() {
            divMessage.html("");
            grayOut(true);
            divWait.toggle();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Upload Matching File</h2>
    <div id="divMessage">
        <%= TempData["message"] %>
    </div>
    <form method="post" enctype="multipart/form-data">
    <label for="file">Filename:</label>
    <input type="file" name="file" id="file" />
    <input id="btnSubmit" type="submit" value="Upload File" />
    </form>
    <div id="divWait">Please Wait while upload is in progress</div>
</asp:Content>
