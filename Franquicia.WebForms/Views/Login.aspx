<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Franquicia.WebForms.Views.Login" %>

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
    <link href="../CSSPropio/Login.css" rel="stylesheet" />
    <!-- CssPropio -->

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Login</title>
    <link rel="shortcut icon" href="../Images/logoCobroscontarjetas.png" />
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
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
                                        <asp:Label ID="lblValiUser" Style="color: red;" runat="server" />
                                        <asp:Label ID="lblValiPassword" Style="color: red;" runat="server" />
                                    </div>
                                    <br />
                                    <div class="form-group">
                                        <asp:TextBox ID="txtUsuario" class="form-control" placeholder="Usuario o correo" runat="server" />
                                    </div>
                                    <div class="form-group">
                                        <asp:TextBox ID="txtPassword" class="form-control" TextMode="Password" placeholder="Contraseña" runat="server" />
                                    </div>

                                    <asp:LinkButton ID="btnLogin" OnClick="btnLogin_Click" runat="server" CssClass="btn btn-success">
                                            Iniciar Sesion <i class="glyphicon glyphicon-log-in"></i>
                                    </asp:LinkButton>
                                </div>

                                <div class="col-12 forgot">
                                    <asp:LinkButton ID="btnRecovery" OnClick="btnRecovery_Click" Text="¿Olvidó su contraseña?" runat="server" />
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <!-- BeginModals -->
        <div class="modal fade" id="ModalRecovery" tabindex="-1" role="dialog" aria-labelledby="ModalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content" style="background-color: #fff; opacity: 1;">
                    <div class="modal-header" style="background-color: #007bff; border-bottom-width: 0px; margin-left: 1px; margin-right: 1px;padding-bottom: 0px;padding-top: 0px;">

                        <div style="width: 100%; background-color: #007bff;">
                            <h5 class="modal-title" style="width: 50%; margin: 0 auto; display: block; color: #fff;">Recuperar Contraseña</h5>
                        </div>
                    </div>
                    <div class="modal-body" style="padding-top: 0px;">
                        <div class="row">
                            <div style="width: 100%; background-color: #007bff;">
                                <img src="../Images/llave.png" style="width: 30%; margin: 0 auto; display: block;" height="100" width="100" class="img-fluid align-items-center" alt="Responsive image" />
                            </div>
                            <p>Por favor ingrese su <strong>correo</strong> o <strong>usuario</strong> asosiado a la cuenta.</p>
                        </div>
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlAlertRecovery" Visible="false" runat="server">
                                    <div id="divAlertRecovery" class="alert alert-danger alert-dismissible fade" role="alert" runat="server">
                                        <asp:Label ID="lblMensajeAlertRecovery" runat="server" />
                                        <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <div class="form-row align-items-center">
                                    <div class="col-md-12">
                                        <label class="sr-only" for="inlineFormInputGroup">Username</label>
                                        <div class="input-group mb-2">
                                            <div class="input-group-prepend">
                                                <div class="input-group-text"><i class="material-icons">admin_panel_settings</i></div>
                                            </div>
                                            <asp:TextBox ID="txtDatos" class="form-control" placeholder="Ingrese su correo o usuario" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:LinkButton ID="btnCancelar" class="btn btn-danger" data-dismiss="modal" runat="server">
                            Cancelar
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnEnviar" OnClick="btnEnviar_Click" class="btn btn-danger" Style="border: #28a745; background-color: #28a745;" runat="server">
                            Recuperar
                                </asp:LinkButton>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        
        <script>
            function showModal() {
                $('#ModalRecovery').modal('show');
            }

            function hideModal() {
                $('#ModalRecovery').modal('hide');
            }
        </script>
        <!-- EndModals -->
    </form>
</body>
</html>
