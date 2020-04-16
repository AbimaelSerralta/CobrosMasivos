﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="HistorialPagosComercio.aspx.cs" Inherits="Franquicia.WebForms.Views.HistorialPagosComercio" %>

<%@ MasterType VirtualPath="~/Views/MasterPage.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHCaja" runat="server">

    <script>
        function button_click(objTextBox, objBtnID) {
            if (window.event.keyCode != 13) {
                document.getElementById(objBtnID).focus();
                document.getElementById(objBtnID).click();
            }
        }
    </script>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlAlert" Visible="false" runat="server">
                <div id="divAlert" class="alert alert-danger alert-dismissible fade" role="alert" runat="server">
                    <asp:Label ID="lblMensajeAlert" runat="server" />
                    <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="content">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-12 col-md-12">
                            <div class="card">
                                <div class="card-header card-header-tabs card-header-primary" style="padding-top: 0px; padding-bottom: 0px;">
                                    <div class="nav-tabs-navigation">
                                        <div class="nav-tabs-wrapper">
                                            <div class="row">
                                                <table style="width:100%">
                                                    <tr>
                                                        <td style="width:40%">
                                                            <asp:LinkButton ID="btnFiltros" ToolTip="Filtros de busqueda." BackColor="#4db6ac" class="btn btn-lg btn-fab btn-fab-mini btn-round" runat="server">
                                                                <i class="material-icons">search</i>
                                                            </asp:LinkButton>
                                                            <asp:Label Text="Historial de pagos" runat="server" />

                                                        </td>
                                                        <td style="width: 20%">
                                                            <asp:Label ID="lblGvSaldo" CssClass="text-center" runat="server" /></td>
                                                        <td style="width:40%">
                                                            <asp:LinkButton ID="btnNuevo" OnClick="btnNuevo_Click" ToolTip="Nueva recarga." class="btn btn-lg btn-success btn-fab btn-fab-mini btn-round pull-right" runat="server">
                                                        <i class="material-icons">add</i>
                                                            </asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvHistorial" DataKeyNames="UidHistorialPago" AutoGenerateColumns="false" CssClass="table table-hover" GridLines="None" border="0" runat="server">
                                                <EmptyDataTemplate>
                                                    <div class="alert alert-info">Su historial esta vacio</div>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:BoundField DataField="DtRegistro" HeaderText="FECHA" />
                                                    <asp:BoundField DataField="VchIdentificador" HeaderText="IDENTIFICADOR" />
                                                    <asp:BoundField DataField="DcmSaldo" HeaderText="SALDO" />
                                                    <asp:BoundField DataField="DcmOperacion" HeaderText="IMPORTE" />
                                                    <asp:BoundField DataField="DcmNuevoSaldo" HeaderText="NUEVO SALDO" />
                                                    <%--<asp:TemplateField>
                                                        <ItemTemplate>
                                                            <table>
                                                                <tbody>
                                                                    <tr style="background: transparent;">
                                                                        <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:LinkButton ID="LinkButton1" ToolTip="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="Editar" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label ID="Label1" class="btn btn-sm btn-info btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">edit</i>
                                                                                </asp:Label>
                                                                            </asp:LinkButton>

                                                                        </td>
                                                                        <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:LinkButton ID="btnCancelarCambio" ToolTip="Visualizar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="Ver" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label ID="lblCancelarCambio" class="btn btn-sm btn-warning btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">remove_red_eye</i>
                                                                                </asp:Label>
                                                                            </asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                </Columns>
                                                <PagerStyle CssClass="pagination-ys" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnNuevo" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cBodyBottom" runat="server">
    <!--MODAL-->
    <div class="modal fade" id="ModalNuevo" tabindex="-1" role="dialog" data-backdrop="static" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="lblTituloModal" Text="Selección" runat="server" /></h5>
                            <%--<button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>--%>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="modal-body pt-0" style="padding-bottom: 0px;">
                    <div class="tab-content">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlSeleccion" runat="server">
                                    <div class="row">
                                        <style>
                                            .card-stats .card-header.card-header-icon, .card-stats .card-header.card-header-text {
                                                text-align: left;
                                            }
                                        </style>
                                        <div class="col-12 col-sm-12 col-md-12 col-lg-12">
                                            <div class="card card-stats">
                                                <div class="card-header card-header-success card-header-icon">
                                                    <div class="card-icon">
                                                        <img class="card-img-top" style="height: 50px; width: 50px" src="../Images/icoWhats.png" alt="whatsapp">
                                                    </div>
                                                    <table>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <label for="txtCantidadWA" style="color: black;">Cantidad</label>
                                                                <asp:TextBox ID="txtCantidadWA" CssClass="form-control" runat="server" />
                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars="" TargetControlID="txtCantidadWA" runat="server" />
                                                            </td>
                                                            <td style="width: 25%">
                                                                <p class="card-category">Whatsapp</p>
                                                                <h3 class="card-title">
                                                                    <asp:Label ID="lblDcmWhatsapp" runat="server"> <%# Eval("DcmWhatsapp", "{0:C}") %></asp:Label>
                                                                </h3>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <label for="txtResultadoWA" style="color: black;">Total</label>
                                                                <asp:TextBox ID="txtResultadoWA" Enabled="false" CssClass="form-control" runat="server" /></td>
                                                            <td style="width: 25%">
                                                                <asp:LinkButton ID="btnAgregarWa" Visible="false" OnClick="btnAgregarWa_Click" CssClass="btn btn-info btn-sm btn-round" Style="padding-left: 0px; padding-right: 0px;" runat="server">
                                                                            <i class="material-icons">check</i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnCalcularWA" OnClick="btnCalcularWA_Click" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-12 col-sm-12 col-md-12 col-lg-12 ">
                                            <div class="card card-stats">
                                                <div class="card-header card-header-warning card-header-icon">
                                                    <div class="card-icon">
                                                        <img class="card-img-top" style="height: 50px; width: 50px" src="../Images/icoSms.jpg" alt="sms">
                                                    </div>
                                                    <table>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <label for="txtCantidadSms" style="color: black;">Cantidad</label>
                                                                <asp:TextBox ID="txtCantidadSms" CssClass="form-control" runat="server" />
                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars="" TargetControlID="txtCantidadSms" runat="server" />
                                                            </td>
                                                            <td style="width: 25%">
                                                                <p class="card-category">Sms</p>
                                                                <h3 class="card-title">
                                                                    <asp:Label ID="lblDcmSms" runat="server"> <%# Eval("DcmSms", "{0:C}") %></asp:Label></h3>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <label for="txtResultadoSms" style="color: black;">Total</label>
                                                                <asp:TextBox ID="txtResultadoSms" Enabled="false" CssClass="form-control" runat="server" />
                                                            </td>
                                                            <td style="width: 25%">
                                                                <asp:LinkButton ID="btnAgregarSms" Visible="false" OnClick="btnAgregarSms_Click" CssClass="btn btn-info btn-sm btn-round" Style="padding-left: 0px; padding-right: 0px;" runat="server">
                                                                            <i class="material-icons">check</i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnCalcularSms" OnClick="btnCalcularSms_Click" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <asp:Panel ID="pnlForm" runat="server">
                                        <div class="row justify-content-center align-items-center h-100">
                                            <div class="col col-sm-12 col-md-12 col-lg-8 col-xl-8">
                                                <asp:Label ID="lblValidar" ForeColor="Red" runat="server" />
                                                <div class="form-group col-md-12" visible="false" style="padding-left: 0px;" runat="server">
                                                    <label for="txtIdentificador" style="color: black;">Identificador</label>
                                                    <asp:TextBox ID="txtIdentificador" Enabled="false" Text="Recarga" CssClass="form-control" runat="server" />
                                                </div>
                                                <div class="form-group col-md-12" visible="false" style="padding-left: 0px;" runat="server">
                                                    <label for="txtConcepto" style="color: black;">Concepto</label>
                                                    <asp:TextBox ID="txtConcepto" Enabled="false" Text="Recarga para Whatsapp y sms" CssClass="form-control" runat="server" />
                                                </div>
                                                <div class="form-group col-md-12" visible="false" style="padding-left: 0px;" runat="server">
                                                    <label for="txtVencimiento" style="color: black;">Vencimiento</label>
                                                    <asp:TextBox ID="txtVencimiento" Enabled="false" TextMode="Date" CssClass="form-control" runat="server" />
                                                </div>
                                                <div class="form-group col-md-12" style="padding-left: 0px;">
                                                    <label for="txtImporte" style="color: black;">Saldo disponible</label>
                                                    <div class="input-group">
                                                        <div class="input-group-prepend">
                                                            <span class="input-group-text" style="padding-left: 0px; padding-right: 5px;">
                                                                <i class="material-icons">$</i>
                                                            </span>
                                                        </div>
                                                        <asp:TextBox ID="txtSaldo" Enabled="false" CssClass="form-control" TextMode="Phone" runat="server" />
                                                    </div>

                                                    <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtSaldo" runat="server" />
                                                </div>
                                                <div class="form-group col-md-12" style="padding-left: 0px;">
                                                    <label for="txtImporte" style="color: black;">Monto *</label>
                                                    <div class="input-group">
                                                        <div class="input-group-prepend">
                                                            <span class="input-group-text" style="padding-left: 0px; padding-right: 5px;">
                                                                <i class="material-icons">$</i>
                                                            </span>
                                                        </div>
                                                        <asp:TextBox ID="txtImporte" PlaceHolder="Monto minimo $50" CssClass="form-control" TextMode="Phone" runat="server" />
                                                        <asp:LinkButton ID="btnCalcular" OnClick="btnCalcular_Click" runat="server" />
                                                    </div>

                                                    <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtImporte" runat="server" />
                                                </div>
                                                <div class="form-group col-md-12" style="padding-left: 0px;">
                                                    <label for="txtImporte" style="color: black;">Nuevo saldo</label>
                                                    <div class="input-group">
                                                        <div class="input-group-prepend">
                                                            <span class="input-group-text" style="padding-left: 0px; padding-right: 5px;">
                                                                <i class="material-icons">$</i>
                                                            </span>
                                                        </div>
                                                        <asp:TextBox ID="txtNuevoSaldo" Enabled="false" CssClass="form-control" TextMode="Phone" runat="server" />
                                                    </div>

                                                    <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtNuevoSaldo" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </asp:Panel>

                                <asp:Panel ID="pnlIframe" Visible="false" runat="server">
                                    <div class="row">
                                        <strong>
                                            <asp:Label Text="Pague la liga con el monto correspondiente, una vez pagada, por favor haga clic en cerrar para validar el pago." runat="server" /></strong>
                                        <div style="width: 100%;">
                                            <iframe id="ifrLiga" style="width: 80%; margin: 0 auto; display: block;" width="450px" height="479px" class="centrado" src="https://u.mitec.com.mx/p/i/EHXYPEKR" frameborder="0" seamless="seamless" runat="server"></iframe>
                                        </div>
                                    </div>
                                </asp:Panel>

                                <asp:Panel ID="pnlValidar" Visible="false" runat="server">
                                    <asp:Timer ID="tmValidar" OnTick="tmValidar_Tick" runat="server" Interval="1000" />
                                    <asp:UpdatePanel ID="upValidar" UpdateMode="Conditional" runat="server">
                                        <ContentTemplate>

                                            <div style="height: 100%; width: 100%; display: flex; justify-content: center; align-items: center;">
                                                <div>
                                                    <div class="loader"></div>
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <asp:UpdatePanel runat="server" ID="upRegistro">
                    <ContentTemplate>
                        <div class="modal-footer justify-content-center">
                            <asp:LinkButton ID="btnGenerar" Enabled="false" OnClick="btnGenerar_Click" CssClass="btn btn-success btn-round" runat="server">
                            <i class="material-icons">link</i> Generar liga
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnCancelar" class="close" data-dismiss="modal" aria-label="Close" CssClass="btn btn-danger btn-round" runat="server">
                            <i class="material-icons">close</i> Cancelar
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnCerrar" Visible="false" OnClick="btnCerrar_Click" CssClass="btn btn-info btn-round" runat="server">
                            <i class="material-icons">close</i> Finalizar
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGenerar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCerrar" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <div id="ModalBusqueda" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="lblTittleLigas" Text="Filtro de busqueda" runat="server" /></h5>
                            <%--<button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>--%>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="col-12 pt-3">
                    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="upBusqueda">
                        <ProgressTemplate>
                            <div class="progress">
                                <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" style="width: 100%"></div>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                <div class="modal-body pt-0" style="padding-bottom: 0px;">
                    <div class="tab-content">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlFiltrosBusqueda" runat="server">
                                    <div class="card" style="margin-top: 0px;">
                                        <div class="card-header card-header-tabs card-header" style="padding-top: 0px; padding-bottom: 0px;">
                                            <div class="nav-tabs-navigation">
                                                <div class="nav-tabs-wrapper">
                                                    <div class="form-group">
                                                        <asp:Label Text="Busqueda" runat="server" />
                                                        <div class="row">
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroNombre" style="color: black;">Nombre</label>
                                                                <asp:TextBox ID="FiltroNombre" CssClass="form-control" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroApePaterno" style="color: black;">ApePaterno</label>
                                                                <asp:TextBox ID="FiltroApePaterno" CssClass="form-control" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroApeMaterno" style="color: black;">ApeMaterno</label>
                                                                <asp:TextBox ID="FiltroApeMaterno" CssClass="form-control" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-8">
                                                                <label for="FiltroCorreo" style="color: black;">Correo</label>
                                                                <asp:TextBox ID="FiltroCorreo" CssClass="form-control" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroEstatus" style="color: black;">Estatus</label>
                                                                <asp:DropDownList ID="FiltroEstatus" AppendDataBoundItems="true" CssClass="form-control" runat="server"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <asp:UpdatePanel runat="server" ID="upBusqueda">
                    <ContentTemplate>
                        <div class="modal-footer justify-content-center">
                            <asp:LinkButton ID="btnBuscar" CssClass="btn btn-primary btn-round" runat="server">
                            <i class="material-icons">search</i> Buscar
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnLimpiar" CssClass="btn btn-warning btn-round" runat="server">
                            <i class="material-icons">clear_all</i> Limpiar
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <!--END MODAL-->

    <script>
        function showModal() {
            $('#ModalNuevo').modal({ backdrop: 'static', keyboard: false, show: true });
        }

        function hideModal() {
            $('#ModalNuevo').modal('hide');
        }
    </script>
    <script>
        function showModalBusqueda() {
            $('#ModalBusqueda').modal('show');
        }

        function hideModalBusqueda() {
            $('#ModalBusqueda').modal('hide');
        }
    </script>
</asp:Content>
