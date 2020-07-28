<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PagaLaEscuela.Views.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.0.8/css/solid.css" />

    <!-- CssPropio -->
    <link href="../CSSPropio/StyleVentanaModal.css" rel="stylesheet" />
    <link href="../CSSPropio/Loader.css" rel="stylesheet" />
    <link href="../CSSPropio/Login.css" rel="stylesheet" />
    <!-- CssPropio -->

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Login</title>
    <link rel="shortcut icon" href="../Images/logoCobroscontarjetas.png" />

    <script>
        function button_click(objTextBox, objBtnID) {
            if (window.event.keyCode == 13) {
                document.getElementById(objBtnID).focus();
                document.getElementById(objBtnID).click();
            }
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" />

        <asp:UpdateProgress runat="server">
            <ProgressTemplate>
                <div class="PogressModal" style="top: 0px;">
                    <div>
                        <img height="150" width="150" src="../CSSPropio/loader.gif" alt="imgCobrosMasivos" />
                        <%--<div class="loader"></div>--%>
                        <br />
                        <asp:Label Text="Validando..." Style="color: white;" runat="server" />
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="container">
            <div class="modal-dialog text-center">
                <div class="col-sm-9 main-section">
                    <div class="modal-content">

                        <div class="col-12 user-img">
                            <asp:Image src="../Images/logoCobroscontarjetas.png" runat="server" />
                        </div>

                        <div class="col-12 form-input">
                            <div class="card">
                                <asp:Label ID="lblValiUser" style="color:red;" runat="server" />
                                <asp:Label ID="lblValiPassword" style="color:red;" runat="server" />
                            </div>
                            <br />
                            <div class="form-group">
                                <asp:TextBox ID="txtUsuario" class="form-control" placeholder="Usuario" runat="server" />
                            </div>
                            <div class="form-group">
                                <asp:TextBox ID="txtPassword" class="form-control" TextMode="Password" placeholder="Contraseña" runat="server" />
                            </div>

                            <asp:LinkButton ID="btnLogin" OnClick="btnLogin_Click" runat="server" CssClass="btn btn-success">
                                            Iniciar Sesion <i class="glyphicon glyphicon-log-in"></i>
                            </asp:LinkButton>
                        </div>

                        <div class="col-12 forgot">
                            <a href="#">¿Olvidó su contraseña?</a>
                        </div>

                    </div>
                </div>
            </div>
        </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
