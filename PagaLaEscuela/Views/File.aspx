<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="File.aspx.cs" Inherits="PagaLaEscuela.Views.File" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

    <style type="text/css">
        .popup {
            background-color: #ddd;
            margin: 0px auto;
            width: 450px;
            position: relative;
            border: Gray 2px inset;
        }

            .popup .content {
                padding: 20px;
                background-color: #ddd;
                float: left;
            }
    </style>
</head>
<body>
    <form id="form1" enctype="multipart/form-data" method="post" runat="server">
        <asp:ScriptManager runat="server" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">

            <ContentTemplate>

                <asp:Image ID="Image1" runat="server" src="image/defaultProfile.jpg" Width="200" Height="200" />

                <br />
                <br />

                <asp:AsyncFileUpload ID="AsyncFileUpload2" runat="server"
                    CompleteBackColor="Lime" UploaderStyle="Modern"
                    ErrorBackColor="Red" ThrobberID="Throbber" OnUploadedComplete="FileUploadComplete"
                    UploadingBackColor="#66CCFF" />

                <br />
                <br />

                <asp:Label ID="Throbber" runat="server" Style="display: none">
                <img src="image/indicator.gif" alt="loading" />
                </asp:Label>

                <br />
                <br />

                <asp:Label ID="lblUpload" runat="server" Text=""></asp:Label>

            </ContentTemplate>

        </asp:UpdatePanel>

        <div>
            <asp:AsyncFileUpload ID="AsyncFileUpload1" runat="server" UploaderStyle="Traditional" OnClientUploadStarted="Start"
                OnClientUploadComplete="Fire" />
            <asp:UpdatePanel runat="server" ID="upd" UpdateMode="Conditional">
                <ContentTemplate>
                    <input type="button" id="btnCheck" onclick="FileUpload();" value="Upload Files Client Side" />
                    <asp:Button runat="server" ID="btnClick" Text="Upload File Server Side" OnClick="btnClick_Click" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnClick" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>

        </div>
        <asp:Panel ID="Panel1" runat="server">

            <asp:Button ID="Button1" runat="server" Text="Button" />
        </asp:Panel>
        <asp:Panel ID="Panel2" runat="server" CssClass="popup">
            <p class="content">
                <asp:AjaxFileUpload ID="AjaxFileUpload1" OnUploadComplete="UploadComplete" ThrobberID="loader" Width="400px" runat="server" />
                <br />
                <br />
                <asp:Image ID="loader" runat="server" ImageUrl="ajax-loading.gif" Style="display: None" />
                <asp:Button ID="ok" runat="server" Text="ok" />
            </p>
        </asp:Panel>
        <asp:ModalPopupExtender ID="ModalPopupExtender1" OkControlID="ok" PopupControlID="Panel2" TargetControlID="Panel1" runat="server"></asp:ModalPopupExtender>

        <script>
            function FileUpload() {
                var asyncFileUpload = $find('AsyncFileUpload1');
                alert(asyncFileUpload.get_postBackUrl());
                alert($get('AsyncFileUpload1_ctl00').value);
                // do something
            }

            function Fire() {
                var asyncFileUpload = $find('AsyncFileUpload1');
                // do something
            }

            function Start() {
                var asyncFileUpload = $find('AsyncFileUpload1');
                // do something
            }
        </script>
    </form>
</body>
</html>
