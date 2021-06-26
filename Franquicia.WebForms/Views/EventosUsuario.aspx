<%@ Page Title="" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="EventosUsuario.aspx.cs" Inherits="Franquicia.WebForms.Views.EventosUsuario" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHCaja" runat="server">
    <style>
        .form-check, label {
            font-size: 14px;
            line-height: 1.42857;
            color: #333333;
            font-weight: 400;
            padding-left: 5px;
            padding-right: 20px;
            width: 100%;
        }
    </style>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlAlertPrincipal" Visible="false" runat="server">
                <div id="divAlertPrincipal" class="alert alert-danger alert-dismissible fade" role="alert" runat="server">
                    <asp:Label ID="lblMensajeAlertPrincipal" runat="server" />
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
                                <div class="card-header card-header-tabs card-header-primary" style="background: #5b3c64; padding-top: 0px; padding-bottom: 0px;">
                                    <div class="nav-tabs-navigation">
                                        <div class="nav-tabs-wrapper">
                                            <div class="form-group" style="margin-top: 0px; padding-bottom: 0px;">
                                                <asp:LinkButton ID="btnFiltros" OnClick="btnFiltros_Click" ToolTip="Filtros de busqueda." BackColor="#4db6ac" class="btn btn-lg btn-fab btn-fab-mini btn-round" runat="server">
                                                                <i class="material-icons">search</i>
                                                </asp:LinkButton>

                                                <asp:Label Text="Listado de Eventos" runat="server" />

                                                <%--<div class="pull-right">
                                                    <asp:LinkButton ID="btnNuevo" OnClick="btnNuevo_Click" class="btn btn-lg btn-success btn-fab btn-fab-mini btn-round" runat="server">
                                            <i class="material-icons">add</i>
                                                    </asp:LinkButton>
                                                </div>--%>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvEventos" OnRowCreated="gvEventos_RowCreated" OnRowCommand="gvEventos_RowCommand" OnSorting="gvEventos_Sorting" AllowSorting="true" AllowPaging="true" PageSize="10" OnPageIndexChanging="gvEventos_PageIndexChanging" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidEvento" GridLines="None" border="0" runat="server">
                                                <EmptyDataTemplate>
                                                    <div class="alert alert-info">No hay eventos registrados</div>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:ButtonField CommandName="Select" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                                                    <asp:BoundField SortExpression="VchNombreEvento" DataField="VchNombreEvento" HeaderText="EVENTO" />
                                                    <asp:BoundField SortExpression="DtFHInicio" DataField="DtFHInicio" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" HeaderText="F/H INICIO" />
                                                    <asp:BoundField SortExpression="VchFHFin" DataField="VchFHFin" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" HeaderText="F/H FIN" />
                                                    <asp:TemplateField SortExpression="UidEstatus" HeaderText="ESTATUS">
                                                        <ItemTemplate>
                                                            <div class="col-md-6">
                                                                <asp:Label ToolTip='<%#Eval("VchEstatus")%>' runat="server">
                                                                <i class="large material-icons">
                                                                    <%#Eval("VchIcono")%>
                                                                </i>
                                                                </asp:Label>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <table>
                                                                <tbody>
                                                                    <tr style="background: transparent;">
                                                                        <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:LinkButton ID="btnAbrirEvento" ToolTip="Abrir evento" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="AbrirEvento" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label ID="lblAbrirEvento" class="btn btn-sm btn-warning btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">remove_red_eye</i>
                                                                                </asp:Label>
                                                                            </asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
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
            <%--<asp:AsyncPostBackTrigger ControlID="btnNuevo" EventName="Click" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cBodyBottom" runat="server">
    <!--MODAL-->
    <div class="modal fade" id="ModalNuevo" tabindex="-1" role="dialog" data-backdrop="static" aria-hidden="true">
        <div class="modal-dialog modal-lg modal-dialog-scrollable" role="document">
            <div class="modal-content">
                <%--<asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="lblTituloModal" runat="server" /></h5>
                            <%--<button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>--%>
                <div class="col-12 pt-3">
                    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="upAbrirEvento">
                        <ProgressTemplate>
                            <div class="progress">
                                <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" style="width: 100%"></div>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                <div class="modal-body pt-0" style="padding-bottom: 0px;">
                    <style>
                        .accordion {
                            background-color: white;
                            color: #444;
                            cursor: pointer;
                            padding: 18px;
                            width: 100%;
                            border: none;
                            text-align: left;
                            outline: none;
                            font-size: 15px;
                            transition: 0.4s;
                            border: 1px solid #dfdfdf;
                        }

                            .activeA, .accordion:hover {
                                background-color: #ccc;
                                background-color: #4981a0;
                                color: white;
                            }

                        .panel {
                            display: none;
                            background-color: white;
                            overflow: hidden;
                            width: 100%;
                        }
                    </style>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
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

                                            <asp:Panel ID="pnlAcordiones" class="row" runat="server">
                                                <asp:LinkButton ID="btnAcordion1" OnClick="btnAcordion1_Click" CssClass="accordion" runat="server">BENEFICIARIO: CLIENTE001 <i class="pull-right material-icons">expand_more</i></asp:LinkButton>
                                                <asp:Panel ID="pnlAcordion1" class="panel" runat="server">
                                                    <asp:Panel ID="pnlDatosComercio" runat="server">
                                                        <div class="card" style="border: 1px solid #dfdfdf; margin-top: 0px; margin-bottom: 0px;">
                                                            <div class="card-body text-primary">
                                                                <div class="card-header text-success"></div>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox Visible="false" Text="BENEFICIARIO:" Width="100px" ReadOnly="true" CssClass="form-control-plaintext text-right" runat="server" />
                                                                        </td>
                                                                        <td style="width: 100%">
                                                                            <asp:TextBox Visible="false" ID="txtNombreComercial" ReadOnly="true" Text="Nombre comercial" CssClass="form-control-plaintext" runat="server" />
                                                                        </td>
                                                                    </tr>
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

                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                </asp:Panel>

                                                <asp:LinkButton ID="btnAcordion2" OnClick="btnAcordion2_Click" CssClass="accordion" runat="server">EVENTO: PROBANDO CAMBIOS <i class="pull-right material-icons">expand_more</i></asp:LinkButton>
                                                <asp:Panel ID="pnlAcordion2" class="panel" runat="server">
                                                    <div class="card" style="border: 1px solid #dfdfdf; margin-top: 0px; margin-bottom: 0px;">
                                                        <div class="card-body text-primary">
                                                            <div class="card-header text-success">
                                                                <div class="row">
                                                                    <div visible="false" class="form-group col-md-4" style="padding-left: 0px;" runat="server">
                                                                        <asp:Label ID="lblNombreEvento" Style="color: black; font-weight: bold;" CssClass="form-control-plaintext" Text="Nombre del evento" runat="server" />
                                                                    </div>
                                                                    <div class="form-group col-md-12" style="padding-left: 0px;" runat="server">
                                                                        <asp:TextBox ID="txtDescripcion" Rows="7" ReadOnly="true" class="form-control-plaintext" TextMode="MultiLine" runat="server" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </asp:Panel>

                                                <asp:LinkButton ID="btnAcordion3" OnClick="btnAcordion3_Click" CssClass="accordion activeA" runat="server">DATOS DEL PAGO <i class="pull-right material-icons">expand_more</i></asp:LinkButton>
                                                <asp:Panel ID="pnlAcordion3" class="panel active" Style="display: block;" runat="server">
                                                    <asp:Panel ID="pnlCorreo" runat="server">
                                                        <div class="card" style="border: 1px solid #dfdfdf; margin-top: 0px; margin-bottom: 0px;">
                                                            <div class="card-body text-primary">
                                                                <div class="card-header" style="padding-left: 0px;">
                                                                </div>
                                                                <div class="row" style="padding-left: 15px; padding-right: 15px;">
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
                                                </asp:Panel>
                                            </asp:Panel>

                                            <asp:Panel ID="pnlIframe" Visible="false" runat="server">
                                                <div style="width: 100%;">
                                                    <iframe id="ifrLiga" style="width: 80%; margin: 0 auto; display: block;" width="450px" height="750px" class="centrado" src="https://wppsandbox.mit.com.mx/i/SNDBX001" frameborder="0" seamless="seamless" runat="server"></iframe>
                                                </div>
                                            </asp:Panel>

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
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <asp:UpdatePanel runat="server" ID="upAbrirEvento">
                    <ContentTemplate>
                        <div class="modal-footer justify-content-center">

                            <asp:LinkButton ID="btnCerrar" data-dismiss="modal" aria-label="Close" CssClass="btn btn-info btn-round" runat="server">
                            <i class="material-icons">close</i> Cerrar
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnGenerarLigas" OnClick="btnGenerarPago_Click" ToolTip="Generar pago" runat="server">
                                <asp:Label class="btn btn-primary btn-round" runat="server">
                                    <asp:Label ID="lblTotalPago" runat="server" /><i class="material-icons">arrow_forward</i>
                                </asp:Label>
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
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
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
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
                                                            <div class="form-group col-md-6">
                                                                <label for="txtFiltroNombre" style="color: black;">Nombre evento</label>
                                                                <asp:TextBox ID="txtFiltroNombre" Style="margin-top: 12px;" CssClass="form-control" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-6">
                                                                <label for="ddlFiltroEstatus" style="color: black;">Estatus</label>
                                                                <asp:DropDownList ID="ddlFiltroEstatus" AppendDataBoundItems="true" CssClass="form-control" Style="margin-top: 6px;" runat="server">
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="form-group col-md-3">
                                                                <label for="ddlImporteMayor" style="color: black;">Importe</label>
                                                                <asp:DropDownList ID="ddlImporteMayor" CssClass="form-control" runat="server">
                                                                    <asp:ListItem Text="(>=) Mayor o igual que" Value=">=" />
                                                                    <asp:ListItem Text="(>) Mayor que" Value=">" />
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="form-group col-md-3">
                                                                <label for="txtFiltroDcmImporteMayor" style="color: black;"></label>
                                                                <asp:TextBox ID="txtFiltroDcmImporteMayor" CssClass="form-control" placeholder="Mayor" aria-label="Search" Style="margin-top: 11px;" runat="server" />
                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtFiltroDcmImporteMayor" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-3">
                                                                <label for="ddlImporteMenor" style="color: black;"></label>
                                                                <asp:DropDownList ID="ddlImporteMenor" CssClass="form-control" Style="margin-top: 6px;" runat="server">
                                                                    <asp:ListItem Text="(<=) Menor o igual que" Value="<=" />
                                                                    <asp:ListItem Text="(<) Menor que" Value="<" />
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="form-group col-md-3">
                                                                <label for="txtFiltroDcmImporteMenor" style="color: black;"></label>
                                                                <asp:TextBox ID="txtFiltroDcmImporteMenor" CssClass="form-control" placeholder="Menor" aria-label="Search" Style="margin-top: 11px;" runat="server" />
                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtFiltroDcmImporteMenor" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-6">
                                                                <label for="txtFiltroDtInicioDesde" style="color: black;">Fecha Inicio</label>
                                                                <asp:TextBox ID="txtFiltroDtInicioDesde" CssClass="form-control" TextMode="Date" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-6">
                                                                <label for="txtFiltroDtInicioHasta" style="color: black;"></label>
                                                                <asp:TextBox ID="txtFiltroDtInicioHasta" Style="margin-top: 5px;" CssClass="form-control" TextMode="Date" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-6">
                                                                <label for="txtFiltroDtFinDesde" style="color: black;">Fecha Fin</label>
                                                                <asp:TextBox ID="txtFiltroDtFinDesde" CssClass="form-control" TextMode="Date" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-6">
                                                                <label for="txtFiltroDtFinHasta" style="color: black;"></label>
                                                                <asp:TextBox ID="txtFiltroDtFinHasta" Style="margin-top: 5px;" CssClass="form-control" TextMode="Date" aria-label="Search" runat="server" />
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
                            <asp:LinkButton ID="btnBuscar" OnClick="btnBuscar_Click" CssClass="btn btn-primary btn-round" runat="server">
                            <i class="material-icons">search</i> Buscar
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnLimpiar" OnClick="btnLimpiar_Click" CssClass="btn btn-warning btn-round" runat="server">
                            <i class="material-icons">clear_all</i> Limpiar
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
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

        function showModalBusqueda() {
            $('#ModalBusqueda').modal('show');
        }

        function hideModalBusqueda() {
            $('#ModalBusqueda').modal('hide');
        }
    </script>

    <script>
        function button_click(objTextBox, objBtnID) {
            if (window.event.keyCode != 13) {
                document.getElementById(objBtnID).focus();
                document.getElementById(objBtnID).click();
            }
        }
    </script>
</asp:Content>
