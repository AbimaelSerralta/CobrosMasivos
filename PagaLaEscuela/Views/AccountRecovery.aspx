<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountRecovery.aspx.cs" Inherits="PagaLaEscuela.Views.AccountRecovery" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" href="https://fonts.googleapis.com/css?family=Roboto:300,400,500,700|Roboto+Slab:400,700|Material+Icons" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.0.8/css/solid.css" />

    <!-- CssPropio -->
    <link href="../CSSPropio/StyleVentanaModal.css" rel="stylesheet" />
    <link href="../CSSPropio/Loader.css" rel="stylesheet" />
    <link href="../CSSPropio/AccountRecovery.css" rel="stylesheet" />
    <!-- CssPropio -->

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Recovery</title>
    <link rel="shortcut icon" href="../Images/logoPagaLaEscuela.png" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
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
                        <img height="150" width="150" src="../Images/loaderEscuela.gif" alt="imgPagaLaEscuela" />
                        <%--<div class="loader"></div>--%>
                        <br />
                        <asp:Label Text="Validando..." Style="color: white;" runat="server" />
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="container-fluid px-1 px-md-5 px-lg-1 px-xl-5 py-5 mx-auto">
                    <div style="display: flex; justify-content: center; align-items: center;">
                        <div class="card card0 border-0">
                            <div class="row d-flex">
                                <div class="card2 card border-0 px-4 py-5" style="margin-left: 0px; margin-right: 0px;">
                                    <div class="text-center">
                                        <img src="../Images/logoCompetoPagaLaEscuela.png" class="img-fluid">
                                    </div>
                                    <%--<div id="divAlert" visible="false" class="col-md-12" style="padding-left: 0px; padding-right: 0px;" runat="server">
                                        
                                    </div>--%>
                                    <asp:Panel ID="pnlAlertRecovery" Visible="false" runat="server">
                                        <div id="divAlertRecovery" class="alert alert-danger alert-dismissible fade" role="alert" runat="server">
                                            <asp:Label ID="lblMensajeAlertRecovery" runat="server" />
                                            <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                        </div>
                                    </asp:Panel>
                                    <div class="row px-3">
                                        <h3>Recuperación de Cuenta</h3>
                                    </div>
                                    <div class="row px-3">
                                        <h4>¡Busquemos tu cuenta!</h4>
                                        <p>Por favor ingrese su <strong>correo electrónico</strong> o <strong>nombre de usuario</strong> asosiado a la cuenta.</p>
                                    </div>
                                    <div class="col-md-12" style="padding-top: 15px; padding-left: 0px; padding-right: 0px;">
                                        <label class="sr-only" for="inlineFormInputGroup">Username</label>
                                        <div class="input-group mb-2">
                                            <div class="input-group-prepend">
                                                <div class="input-group-text"><i class="material-icons">admin_panel_settings</i></div>
                                            </div>
                                            <asp:TextBox ID="txtDatos" class="form-control" placeholder="Ingrese su correo o usuario" runat="server" />
                                        </div>
                                    </div>
                                    <div class="row px-3 mb-4" style="padding-top: 10px;">
                                        <div class="custom-control custom-checkbox custom-control-inline" style="padding-left: 0px;">
                                            <asp:LinkButton ID="btnLogin" OnClick="btnLogin_Click" Text="Iniciar Sesión" runat="server" />
                                        </div>

                                        <asp:LinkButton ID="btnEnviar" OnClick="btnEnviar_Click" runat="server" CssClass="btn btn-success ml-auto mb-0 text-sm">
                                            Recuperar
                                        </asp:LinkButton>
                                    </div>
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
