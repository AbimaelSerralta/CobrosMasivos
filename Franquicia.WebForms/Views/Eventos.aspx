<%@ Page Title="Eventos" Language="C#" AutoEventWireup="true" CodeBehind="Eventos.aspx.cs" Inherits="Franquicia.WebForms.Views.Eventos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <!-- Required meta tags -->
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <!--     Fonts and icons     -->
    <link rel="stylesheet" type="text/css" href="https://fonts.googleapis.com/css?family=Roboto:300,400,500,700|Roboto+Slab:400,700|Material+Icons" />
    <%--<link href="../assets/css/MaterialIcons.css" type="text/css" rel="stylesheet" />--%>
    <link href="../assets/css/font-awesome.min.css" rel="stylesheet" />
    <!-- Material Kit CSS -->
    <link href="../assets/css/material-dashboard.css?v=2.1.1" rel="stylesheet" />

    <%--CSS PROPIO--%>
    <link href="../CSSPropio/Loader.css" rel="stylesheet" />
    <link href="../CSSPropio/StyleLoadingEvento.css" rel="stylesheet" />
    <link href="../CSSPropio/BasicEventos.css" rel="stylesheet" />

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="shortcut icon" href="../Images/logoCobroscontarjetas.png" />

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" />
        <asp:UpdateProgress runat="server">
            <ProgressTemplate>
                <div class="PogressModal">
                    <div>
                        <img height="150" width="150" src="../CSSPropio/loader.gif" alt="imgCobrosMasivos" />
                        <%--<div class="loader"></div>--%>
                        <br />
                        <asp:Label Text="Espere por favor..." Style="color: white;" runat="server" />
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlForm" runat="server">
                    <div class="col-sm-12 row justify-content-center align-items-center" style="margin-right: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
                        <div class="col col-sm-12 col-md-12 col-lg-8 col-xl-8">
                            <div class="form-group col-md-12">
                                <div class="card card-nav-tabs">
                                    <%--<asp:Image ImageUrl="../Images/BannerEvent2.png" CssClass="embed-responsive" runat="server" />--%>
                                    <div class="card-header card-header-success">
                                        <asp:Label ID="lblTitle" Text="APORTACIÓN VOLUNTARIA" Style="text-transform: uppercase;" runat="server" />

                                        <div class="pull-right">
                                            <asp:LinkButton ID="btnAceptar" OnClick="btnAceptar_Click" Visible="false" ToolTip="Finalizar" runat="server">
                                            <asp:Label class="btn btn-info btn-round" runat="server">
                                                <i class="material-icons">check</i>Finalizar
                                            </asp:Label>
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="card-body">
                                        <asp:Panel ID="pnlAlert" Visible="false" runat="server">
                                            <div id="divAlert" class="alert alert-danger alert-dismissible fade" role="alert" runat="server">
                                                <asp:Label ID="lblMensajeAlert" runat="server" />
                                                <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                            </div>
                                        </asp:Panel>

                                        <asp:Panel ID="pnlDatosComercio" runat="server">
                                            <div class="card border-success" style="margin-top: 0px;">
                                                <div class="card-body text-primary">
                                                    <div class="card-header text-success"></div>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox Text="BENEFICIARIO:" Width="100px" ReadOnly="true" CssClass="form-control-plaintext text-right" runat="server" />
                                                            </td>
                                                            <td style="width: 100%">
                                                                <asp:TextBox ID="txtNombreComercial" ReadOnly="true" Text="Nombre comercial" CssClass="form-control-plaintext" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <asp:Panel ID="pnlDatosBeneficiario" Visible="true" runat="server">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox Text="TELÉFONO:" Width="100px" ReadOnly="true" CssClass="form-control-plaintext text-right" runat="server" />
                                                                </td>
                                                                <td style="width: 100%">
                                                                    <asp:TextBox ID="txtComeCelular" ReadOnly="true" Text="+521234567890" CssClass="form-control-plaintext" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox Text="CORREO:" Width="100px" ReadOnly="true" CssClass="form-control-plaintext text-right" runat="server" />
                                                                </td>
                                                                <td style="width: 100%">
                                                                    <asp:TextBox ID="txtComeCorreo" ReadOnly="true" Text="EJEMPLO@EJEMPLO.COM" CssClass="form-control-plaintext" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <div class="row" style="padding-left: 15px; padding-right: 15px;">
                                                            <%--<div class="form-group col-md-2" style="padding-left: 0px;" runat="server">
                                                            </div>
                                                            <div class="form-group col-md-4" style="padding-left: 0px;" runat="server">
                                                            </div>
                                                            <div class="form-group col-md-4" style="padding-left: 0px;" runat="server">
                                                            </div>--%>
                                                            <div class="form-group col-md-4" style="padding-left: 0px;" runat="server">
                                                                <asp:Label ID="lblNombreEvento" Style="color: black; font-weight: bold;" CssClass="form-control-plaintext" Text="Nombre del evento" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-12" style="padding-left: 0px;" runat="server">
                                                                <asp:TextBox ID="txtDescripcion" Rows="4" ReadOnly="true" class="form-control-plaintext" TextMode="MultiLine" runat="server" />
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                </div>
                                            </div>
                                        </asp:Panel>

                                        <asp:Panel ID="pnlCorreo" runat="server">
                                            <div class="card border-primary">
                                                <div class="card-body text-primary">
                                                    <div class="card-header" style="padding-left: 0px;">
                                                        <asp:Label ID="lblTituloDatosUsuario" Style="color: black;" Font-Size="Large" Font-Bold="true" runat="server" />
                                                    </div>
                                                    <div class="row" style="padding-left: 15px; padding-right: 15px;">
                                                        <div class="form-group col-md-12" style="padding-left: 0px;" runat="server">
                                                            <asp:Label ID="lblTituloCorreo" for="txtCorreo" Style="color: black;" Font-Bold="true" runat="server">Correo *:</asp:Label>
                                                            <asp:TextBox ID="txtCorreo" Style="margin-top: 7px;" CssClass="form-control" runat="server" />
                                                        </div>

                                                        <asp:Panel ID="pnlUsuario" Visible="false" runat="server">
                                                            <div class="row" style="padding-left: 15px; padding-right: 15px;">
                                                                <div class="form-group col-md-4" style="padding-left: 0px;" runat="server">
                                                                    <asp:Label for="txtNombre" Style="color: black;" Font-Bold="true" runat="server">Nombre(s) *</asp:Label>
                                                                    <%--<label for="txtNombre" style="color: black;">Nombre(s) *:</label>--%>
                                                                    <asp:TextBox ID="txtNombre" CssClass="form-control" runat="server" />
                                                                </div>
                                                                <div class="form-group col-md-4" style="padding-left: 0px;" runat="server">
                                                                    <asp:Label for="txtApePaterno" Style="color: black;" Font-Bold="true" runat="server">Apellido Paterno *</asp:Label>
                                                                    <%--<label for="txtApePaterno" style="color: black;">Apellido Paterno *</label>--%>
                                                                    <asp:TextBox ID="txtApePaterno" CssClass="form-control" runat="server" />
                                                                </div>
                                                                <div class="form-group col-md-4" style="padding-left: 0px;" runat="server">
                                                                    <asp:Label for="txtApeMaterno" Style="color: black;" Font-Bold="true" runat="server">Apellido Materno *</asp:Label>
                                                                    <%--<label for="txtApeMaterno" style="color: black;">Apellido Materno *</label>--%>
                                                                    <asp:TextBox ID="txtApeMaterno" CssClass="form-control" runat="server" />
                                                                </div>
                                                                <div class="form-group col-md-4" style="padding-left: 0px;" runat="server">
                                                                    <asp:Label for="ddlPrefijo" Style="color: black;" Font-Bold="true" runat="server">Código pais *</asp:Label>
                                                                    <asp:DropDownList ID="ddlPrefijo" CssClass="form-control" runat="server">
                                                                    </asp:DropDownList>
                                                                </div>
                                                                <div class="form-group col-md-4" style="padding-left: 0px;" runat="server">
                                                                    <asp:Label for="txtCelelular" Style="color: black;" Font-Bold="true" runat="server">Celeular *</asp:Label>
                                                                    <%--<label for="txtCelelular" style="color: black;"></label>--%>
                                                                    <asp:TextBox ID="txtCelular" CssClass="form-control" Style="margin-top: 4px;" runat="server" />
                                                                </div>
                                                            </div>
                                                        </asp:Panel>

                                                        <div class="form-group col-md-4" style="padding-left: 0px;">
                                                            <asp:Label for="txtImporte" Style="color: black;" Font-Bold="true" runat="server">Monto *</asp:Label>
                                                            <%--<label for="txtImporte" style="color: black;">Monto *</label>--%>
                                                            <div class="input-group">
                                                                <div class="input-group-prepend">
                                                                    <span class="input-group-text" style="padding-left: 0px; padding-right: 5px;">
                                                                        <i class="material-icons">$</i>
                                                                    </span>
                                                                </div>
                                                                <asp:TextBox ID="txtImporte" PlaceHolder="Capture el monto" CssClass="form-control" TextMode="Phone" Style="margin-top: 5px;" runat="server" />
                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtImporte" runat="server" />
                                                                <asp:LinkButton ID="btnCalcular" OnClick="btnCalcular_Click" runat="server" />
                                                            </div>
                                                            <asp:Label ID="lblEditable" Text="El monto es modificable" CssClass="text-info" Font-Size="Smaller" Font-Bold="true" runat="server" />
                                                        </div>
                                                        <div class="form-group col-md-4" style="padding-left: 0px;" runat="server">
                                                            <asp:Label for="ddlFormasPago" Style="color: black;" Font-Bold="true" runat="server">Formas de pago *</asp:Label>
                                                            <%--<label for="ddlFormasPago" style="color: black;">Formas de pago</label>--%>
                                                            <asp:DropDownList ID="ddlFormasPago" OnSelectedIndexChanged="ddlFormasPago_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="true" CssClass="form-control" runat="server">
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="form-group col-md-4" style="padding-left: 0px;">
                                                            <asp:Label for="txtImporte" Style="color: black;" Font-Bold="true" runat="server">Total a pagar *</asp:Label>
                                                            <%--<label for="txtImporte" style="color: black;">Total</label>--%>
                                                            <div class="input-group">
                                                                <div class="input-group-prepend">
                                                                    <span class="input-group-text" style="padding-left: 0px; padding-right: 5px;">
                                                                        <i class="material-icons">$</i>
                                                                    </span>
                                                                </div>
                                                                <asp:TextBox ID="txtImporteTotal" Enabled="false" CssClass="form-control" TextMode="Phone" Style="margin-top: 4px;" runat="server" />
                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtImporteTotal" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>

                                        <asp:Panel ID="pnlGenerarLigas" Visible="false" runat="server">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td>
                                                        <table align="center" border="0" cellpadding="0" cellspacing="0" style="border: 1px solid #cccccc; border-collapse: collapse;">
                                                            <tr>
                                                                <td align="center" bgcolor="#00bcd4" style="padding: 40px 0 30px 0; color: #153643; font-size: 28px; font-weight: bold; font-family: Arial, sans-serif;">
                                                                    <a href="http://www.cobroscontarjeta.com/">
                                                                        <img src="../Images/logo-cobroscontarjetas.png" class="img-fluid" alt="Cobroscontarjeta" style="display: block;" /></a>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td bgcolor="#ffffff" style="padding: 40px 30px 40px 30px;">
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                        <tr>
                                                                            <td colspan="2" style="text-align: center; color: #153643; font-family: Arial, sans-serif; font-size: 24px;">
                                                                                <b>Hola,
                                                                                    <asp:Label ID="lblNombreComp" runat="server" /></b>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                        <tr>
                                                                            <td colspan="2" style="padding-top: 20px; color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px; text-align: center;">
                                                                                <h4>
                                                                                    <asp:Label ID="lblNombreComercial" runat="server" />
                                                                                    le agradece su aportacion para el evento: , le mostramos las siguientes formas de pago:
                                                                                </h4>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td width="50%" style="color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px; text-align: right;">Concepto:</td>
                                                                            <td width="50%" style="color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px; text-align: left;">
                                                                                <asp:Label ID="lblConcepto" runat="server" /></td>

                                                                        </tr>
                                                                        <tr>
                                                                            <td width="50%" style="color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px; text-align: right;">Importe:
                                                                            </td>

                                                                            <td width="50%" style="color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px; text-align: left;">
                                                                                <asp:Label ID="lblImporte" runat="server" /></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td width="50%" style="color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px; text-align: right;">Vencimiento:</td>

                                                                            <td width="50%" style="color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px; text-align: left;">
                                                                                <asp:Label ID="lblVencimiento" runat="server" /></td>
                                                                        </tr>
                                                                    </table>
                                                                    <br />
                                                                    <br />
                                                                    <asp:LinkButton ID="btnPagar" class="btn btn-success btn-round" Style="display: block; text-align: center; width: 150px; font-size: 15px; text-decoration: none; margin: 0 auto; padding: 15px 0" OnClick="Unnamed_Click" runat="server">
                                                                        <asp:Label ID="lblPagar" runat="server" />
                                                                    </asp:LinkButton>
                                                                    <asp:Panel ID="pnlPromociones" Visible="false" runat="server">
                                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                            <tr>
                                                                                <td colspan="2" style="padding-top: 20px; color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px; text-align: center;">
                                                                                    <h4>Si lo desea puede pagar con las siguientes promociones:</h4>
                                                                                </td>
                                                                            </tr>
                                                                            <asp:Literal ID="ltlPromociones" runat="server" />
                                                                            <asp:Literal ID="ltlPromo" runat="server" />

                                                                            <%--<asp:Repeater ID="jhu" runat="server">
                                                                                <ItemTemplate>
                                                                                    <tr>
                                                                                        <td width="50%" style="color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px; text-align: right;"><%#Eval("DcmImporte")%>
                                                                                        </td>
                                                                                        <td width="50%" style="color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px; text-align: left;">
                                                                                            <a onclick="Unnamed_Click" style="display: block; color: #fff; font-weight: 400; text-align: center; width: 100px; font-size: 15px; text-decoration: none; background: #28a745; margin: 0 auto; padding: 5px;" href="#"> serralat $ <%#Eval("DcmImporte")%></a>

                                                                                            <asp:LinkButton OnClick="Unnamed_Click" Text='<%#Eval("DcmImporte")%>' runat="server" >
                                                                                                <asp:textbox ID="" runat="server" />
                                                                                            </asp:LinkButton>
                                                                                        </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>--%>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td bgcolor="#df5f16" style="padding: 30px 30px 30px 30px;">
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                        <tr>
                                                                            <td style="color: #ffffff; font-family: Arial, sans-serif; font-size: 14px;" width="75%">Cobroscontarjeta &reg; Todos los derechos reservados, 2020<br />
                                                                            </td>
                                                                            <td align="right" width="25%"></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>

                                        <asp:Panel ID="pnlIframe" Visible="false" runat="server">
                                            <div style="width: 100%;">
                                                <iframe id="ifrLiga" style="width: 80%; margin: 0 auto; display: block;" width="450px" height="750px" class="centrado" src="https://wppsandbox.mit.com.mx/i/SNDBX001" frameborder="0" seamless="seamless" runat="server"></iframe>
                                            </div>
                                        </asp:Panel>

                                        <div class="pull-left">
                                            <asp:LinkButton ID="btnCancelar" OnClick="btnCancelar_Click" Visible="false" ToolTip="Cancelar" runat="server">
                                                <asp:Label class="btn btn-warning btn-round" runat="server">
                                                    <i class="material-icons">arrow_back</i>Anterior
                                                </asp:Label>
                                            </asp:LinkButton>
                                        </div>

                                        <div class="pull-right">
                                            <asp:LinkButton ID="btnGenerarLigas" OnClick="btnGenerarPago_Click" ToolTip="Generar pago" runat="server">
                                                <asp:Label class="btn btn-primary btn-round" runat="server">
                                                    <asp:Label ID="lblTotalPago" runat="server" /><i class="material-icons">arrow_forward</i>
                                                </asp:Label>
                                            </asp:LinkButton>
                                        </div>

                                        <asp:Panel ID="pnlValidar" Visible="false" runat="server">
                                            <asp:Timer ID="tmValidar" OnTick="tmValidar_Tick" runat="server" Interval="1000" />
                                            <asp:UpdatePanel ID="upValidar" UpdateMode="Conditional" runat="server">
                                                <ContentTemplate>

                                                    <div style="height: 100%; width: 100%; display: flex; justify-content: center; align-items: center;">
                                                        <div>
                                                            <img height="150" width="150" src="../CSSPropio/loader.gif" alt="imgCobrosMasivos" />
                                                            <%--<div class="loader"></div>--%>
                                                            <br />
                                                            <strong>
                                                                <asp:Literal ID="ltMnsj" Text="Verificando..." runat="server" /></strong>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="tmValidar" EventName="tick" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </asp:Panel>

                                        <asp:Panel ID="pnlValidarEvento" Visible="false" runat="server">
                                            <div class="card">

                                                <div style="width: 100%; background-color: #45abad;">
                                                    <img src="../Images/EventoCerrado.png" style="width: 30%; margin: 0 auto; display: block;" height="100" width="100" class="img-fluid align-items-center" alt="Responsive image">
                                                </div>
                                                <div class="card-body">
                                                    <div class="tab-content text-center">
                                                        <h3 runat="server">
                                                            <asp:Label ID="lblTitleDialog" Text="El evento ha finalizado." runat="server" />
                                                        </h3>
                                                        <asp:Label ID="lblMnsjDialog" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>

    <script>
        function button_click(objTextBox, objBtnID) {
            if (window.event.keyCode != 13) {
                document.getElementById(objBtnID).focus();
                document.getElementById(objBtnID).click();
            }
        }
    </script>

    <!--   Core JS Files   -->
    <script src="../assets/js/core/jquery.min.js"></script>
    <script src="../assets/js/core/popper.min.js"></script>
    <script src="../assets/js/core/bootstrap-material-design.min.js"></script>
    <script src="../assets/js/plugins/perfect-scrollbar.jquery.min.js"></script>
    <!-- Plugin for the momentJs  -->
    <script src="../assets/js/plugins/moment.min.js"></script>
    <!--  Plugin for Sweet Alert -->
    <script src="../assets/js/plugins/sweetalert2.js"></script>
    <!-- Forms Validations Plugin -->
    <script src="../assets/js/plugins/jquery.validate.min.js"></script>
    <!-- Plugin for the Wizard, full documentation here: https://github.com/VinceG/twitter-bootstrap-wizard -->
    <script src="../assets/js/plugins/jquery.bootstrap-wizard.js"></script>
    <!--	Plugin for Select, full documentation here: http://silviomoreto.github.io/bootstrap-select -->
    <script src="../assets/js/plugins/bootstrap-selectpicker.js"></script>
    <!--  Plugin for the DateTimePicker, full documentation here: https://eonasdan.github.io/bootstrap-datetimepicker/ -->
    <script src="../assets/js/plugins/bootstrap-datetimepicker.min.js"></script>
    <!--  DataTables.net Plugin, full documentation here: https://datatables.net/  -->
    <script src="../assets/js/plugins/jquery.dataTables.min.js"></script>
    <!--	Plugin for Tags, full documentation here: https://github.com/bootstrap-tagsinput/bootstrap-tagsinputs  -->
    <script src="../assets/js/plugins/bootstrap-tagsinput.js"></script>
    <!-- Plugin for Fileupload, full documentation here: http://www.jasny.net/bootstrap/javascript/#fileinput -->
    <script src="../assets/js/plugins/jasny-bootstrap.min.js"></script>
    <!--  Full Calendar Plugin, full documentation here: https://github.com/fullcalendar/fullcalendar    -->
    <script src="../assets/js/plugins/fullcalendar.min.js"></script>

    <!--  Plugin for the Sliders, full documentation here: http://refreshless.com/nouislider/ -->
    <script src="../assets/js/plugins/nouislider.min.js"></script>


    <!-- Chartist JS -->
    <script src="../assets/js/plugins/chartist.min.js"></script>
    <!--  Notifications Plugin    -->
    <script src="../assets/js/plugins/bootstrap-notify.js"></script>
    <!-- Control Center for Material Dashboard: parallax effects, scripts for the example pages etc -->
    <script src="../assets/js/material-dashboard.js?v=2.1.1" type="text/javascript"></script>
    <!-- Material Dashboard DEMO methods, don't include it in your project! -->
    <script src="../assets/demo/demo.js"></script>
    <script>
        $(document).ready(function () {
            $().ready(function () {
                $sidebar = $('.sidebar');

                $sidebar_img_container = $sidebar.find('.sidebar-background');

                $full_page = $('.full-page');

                $sidebar_responsive = $('body > .navbar-collapse');

                window_width = $(window).width();

                fixed_plugin_open = $('.sidebar .sidebar-wrapper .nav li.active a p').html();

                if (window_width > 767 && fixed_plugin_open == 'Dashboard') {
                    if ($('.fixed-plugin .dropdown').hasClass('show-dropdown')) {
                        $('.fixed-plugin .dropdown').addClass('open');
                    }

                }

                $('.fixed-plugin a').click(function (event) {
                    // Alex if we click on switch, stop propagation of the event, so the dropdown will not be hide, otherwise we set the  section active
                    if ($(this).hasClass('switch-trigger')) {
                        if (event.stopPropagation) {
                            event.stopPropagation();
                        } else if (window.event) {
                            window.event.cancelBubble = true;
                        }
                    }
                });

                $('.fixed-plugin .active-color span').click(function () {
                    $full_page_background = $('.full-page-background');

                    $(this).siblings().removeClass('active');
                    $(this).addClass('active');

                    var new_color = $(this).data('color');

                    if ($sidebar.length != 0) {
                        $sidebar.attr('data-color', new_color);
                    }

                    if ($full_page.length != 0) {
                        $full_page.attr('filter-color', new_color);
                    }

                    if ($sidebar_responsive.length != 0) {
                        $sidebar_responsive.attr('data-color', new_color);
                    }
                });

                $('.fixed-plugin .background-color .badge').click(function () {
                    $(this).siblings().removeClass('active');
                    $(this).addClass('active');

                    var new_color = $(this).data('background-color');

                    if ($sidebar.length != 0) {
                        $sidebar.attr('data-background-color', new_color);
                    }
                });

                $('.fixed-plugin .img-holder').click(function () {
                    $full_page_background = $('.full-page-background');

                    $(this).parent('li').siblings().removeClass('active');
                    $(this).parent('li').addClass('active');


                    var new_image = $(this).find("img").attr('src');

                    if ($sidebar_img_container.length != 0 && $('.switch-sidebar-image input:checked').length != 0) {
                        $sidebar_img_container.fadeOut('fast', function () {
                            $sidebar_img_container.css('background-image', 'url("' + new_image + '")');
                            $sidebar_img_container.fadeIn('fast');
                        });
                    }

                    if ($full_page_background.length != 0 && $('.switch-sidebar-image input:checked').length != 0) {
                        var new_image_full_page = $('.fixed-plugin li.active .img-holder').find('img').data('src');

                        $full_page_background.fadeOut('fast', function () {
                            $full_page_background.css('background-image', 'url("' + new_image_full_page + '")');
                            $full_page_background.fadeIn('fast');
                        });
                    }

                    if ($('.switch-sidebar-image input:checked').length == 0) {
                        var new_image = $('.fixed-plugin li.active .img-holder').find("img").attr('src');
                        var new_image_full_page = $('.fixed-plugin li.active .img-holder').find('img').data('src');

                        $sidebar_img_container.css('background-image', 'url("' + new_image + '")');
                        $full_page_background.css('background-image', 'url("' + new_image_full_page + '")');
                    }

                    if ($sidebar_responsive.length != 0) {
                        $sidebar_responsive.css('background-image', 'url("' + new_image + '")');
                    }
                });

                $('.switch-sidebar-image input').change(function () {
                    $full_page_background = $('.full-page-background');

                    $input = $(this);

                    if ($input.is(':checked')) {
                        if ($sidebar_img_container.length != 0) {
                            $sidebar_img_container.fadeIn('fast');
                            $sidebar.attr('data-image', '#');
                        }

                        if ($full_page_background.length != 0) {
                            $full_page_background.fadeIn('fast');
                            $full_page.attr('data-image', '#');
                        }

                        background_image = true;
                    } else {
                        if ($sidebar_img_container.length != 0) {
                            $sidebar.removeAttr('data-image');
                            $sidebar_img_container.fadeOut('fast');
                        }

                        if ($full_page_background.length != 0) {
                            $full_page.removeAttr('data-image', '#');
                            $full_page_background.fadeOut('fast');
                        }

                        background_image = false;
                    }
                });

                $('.switch-sidebar-mini input').change(function () {
                    $body = $('body');

                    $input = $(this);

                    if (md.misc.sidebar_mini_active == true) {
                        $('body').removeClass('sidebar-mini');
                        md.misc.sidebar_mini_active = false;

                        $('.sidebar .sidebar-wrapper, .main-panel').perfectScrollbar();

                    } else {

                        $('.sidebar .sidebar-wrapper, .main-panel').perfectScrollbar('destroy');

                        setTimeout(function () {
                            $('body').addClass('sidebar-mini');

                            md.misc.sidebar_mini_active = true;
                        }, 300);
                    }

                    // we simulate the window Resize so the charts will get updated in realtime.
                    var simulateWindowResize = setInterval(function () {
                        window.dispatchEvent(new Event('resize'));
                    }, 180);

                    // we stop the simulation of Window Resize after the animations are completed
                    setTimeout(function () {
                        clearInterval(simulateWindowResize);
                    }, 1000);

                });
            });
        });
  </script>
    <script>
        $(document).ready(function () {
            // Javascript method's body can be found in assets/js/demos.js
            md.initDashboardPageCharts();

        });
  </script>
</body>
</html>
