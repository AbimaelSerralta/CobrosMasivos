<%@ Page Title="Reporte Ligas" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReporteLigaUsuario.aspx.cs" Inherits="Franquicia.WebForms.Views.ReporteLigaUsuario" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHCaja" runat="server">
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
                                <div class="card-header card-header-tabs card-header-primary" style="background: #5b3c64; padding-top: 0px; padding-bottom: 0px;">
                                    <div class="nav-tabs-navigation">
                                        <div class="nav-tabs-wrapper">
                                            <div class="form-group" style="margin-top: 0px; padding-bottom: 0px;">
                                                <asp:LinkButton ID="btnFiltros" OnClick="btnFiltros_Click" ToolTip="Filtros de busqueda." BackColor="#4db6ac" class="btn btn-lg btn-fab btn-fab-mini btn-round" runat="server">
                                                        <i class="material-icons">search</i>
                                                </asp:LinkButton>
                                                <asp:Label Text="Listado de ligas" runat="server" />

                                                <asp:LinkButton ID="btnActualizarLista" OnClick="btnActualizarLista_Click" ToolTip="Actualizar tabla." class="btn btn-lg btn-success btn-fab btn-fab-mini btn-round pull-right" runat="server">
                                                        <i class="material-icons">sync</i>
                                                </asp:LinkButton>
                                                <%--<asp:LinkButton ID="btnExportarLista" OnClick="btnExportarLista_Click" ToolTip="Exportar tabla a excel." class="btn btn-lg btn-warning btn-fab btn-fab-mini btn-round pull-right" runat="server">
                                                        <i class="material-icons">file_download</i>
                                                </asp:LinkButton>--%>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvLigasGeneradas" OnSorting="gvLigasGeneradas_Sorting" OnRowCommand="gvLigasGeneradas_RowCommand" OnRowDataBound="gvLigasGeneradas_RowDataBound" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidLigaUrl" GridLines="None" border="0" AllowPaging="true" PageSize="10" OnPageIndexChanging="gvLigasGeneradas_PageIndexChanging" runat="server">
                                                <EmptyDataTemplate>
                                                    <div class="alert alert-info">No hay ligas registradas</div>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:BoundField SortExpression="VchIdentificador" DataField="VchIdentificador" HeaderText="IDENTI." />
                                                    <asp:BoundField SortExpression="VchNombreComercial" DataField="VchNombreComercial" HeaderText="COMERCIO" />
                                                    <asp:BoundField SortExpression="VchAsunto" DataField="VchAsunto" HeaderText="ASUNTO" />
                                                    <asp:BoundField SortExpression="VchConcepto" DataField="VchConcepto" HeaderText="CONCEPTO" />
                                                    <asp:BoundField SortExpression="DtVencimiento" DataField="DtVencimiento" DataFormatString="{0:d}" HeaderText="VENCIMIENTO" />
                                                    <asp:BoundField SortExpression="DcmImporte" DataField="DcmImporte" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C}" HeaderText="SUBTOTAL" />
                                                    <asp:BoundField SortExpression="Comisiones" DataField="Comisiones" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C}" HeaderText="COMI." />
                                                    <asp:BoundField SortExpression="DcmImportePromocion" DataField="DcmImportePromocion" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C}" HeaderText="PAGADO" />
                                                    <asp:TemplateField SortExpression="VchPromocion" ItemStyle-HorizontalAlign="Center" HeaderText="PROMO.">
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%#Eval("VchPromocionIcono")%>' ToolTip='<%#Eval("VchPromocion")%>' Font-Size="Larger" Font-Bold="true" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField SortExpression="VchEstatus" DataField="VchEstatus" HeaderText="ESTATUS" />--%>
                                                    <asp:TemplateField SortExpression="VchEstatus" HeaderText="ESTATUS">
                                                        <ItemTemplate>
                                                            <div class="col-md-6">
                                                                <asp:Label ToolTip='<%#Eval("VchEstatus")%>' runat="server">
                                                                <i class="large material-icons" style="color:<%#Eval("VchColor")%>;">
                                                                    <%#Eval("VchEstatusIcono")%>
                                                                </i>
                                                                </asp:Label>
                                                            </div>
                                                            <%--<asp:Label Text='<%#Eval("VchEstatus")%>' ForeColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("VchColor").ToString()) %>' Font-Size="Larger" Font-Names="Comic Sans MS" Font-Bold="true" runat="server"></asp:Label>--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <table>
                                                                <tbody>
                                                                    <tr style="background: transparent;">
                                                                        <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:LinkButton ID="btnInfoMovimiento" ToolTip="Detalle" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="btnInfoMovimiento" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label class="btn btn-sm btn-info btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">info_outline</i>
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
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cBodyBottom" runat="server">
    <!--MODAL-->
    <div id="ModalDetalle" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="lblTituloModal" Text="Movimientos de la liga" runat="server" /></h5>
                            <button type="button" class="close d-lg-none" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="col-12 pt-3">
                    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="upDetalleLiga">
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
                                <asp:Panel ID="pnlAlertModal" Visible="false" runat="server">
                                    <div id="divAlertModal" role="alert" runat="server">
                                        <asp:Label ID="lblResumen" runat="server" />
                                        <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    </div>
                                </asp:Panel>
                                <div class="row">
                                    <div class="col-lg-12 col-md-12">
                                        <div class="card">
                                            <div class="card-body">
                                                <div class="row">
                                                    <div class="table-responsive">
                                                        <asp:GridView ID="gvDetalleLiga" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="IdReferencia" GridLines="None" border="0" AllowPaging="true" PageSize="10" OnPageIndexChanging="gvDetalleLiga_PageIndexChanging" runat="server">
                                                            <EmptyDataTemplate>
                                                                <div class="alert alert-info">No hay movimientos</div>
                                                            </EmptyDataTemplate>
                                                            <Columns>
                                                                <asp:BoundField SortExpression="DtmFechaDeRegistro" DataField="DtmFechaDeRegistro" HeaderText="FECHA" />
                                                                <%--<asp:BoundField SortExpression="VchEstatus" DataField="VchEstatus" HeaderText="ESTATUS" />--%>
                                                                <asp:TemplateField SortExpression="VchEstatus" HeaderText="ESTATUS">
                                                                    <ItemTemplate>
                                                                        <asp:Label Text='<%#Eval("VchEstatus")%>' ForeColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("VchColor").ToString()) %>' Font-Size="Larger" Font-Names="Comic Sans MS" Font-Bold="true" runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <PagerStyle CssClass="pagination-ys" />
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>

                        </asp:UpdatePanel>
                    </div>
                </div>
                <asp:UpdatePanel runat="server" ID="upDetalleLiga">
                    <ContentTemplate>
                        <div class="modal-footer justify-content-center">
                            <asp:LinkButton ID="btnAceptar" data-dismiss="modal" aria-label="Close" CssClass="btn btn-info btn-round" runat="server">
                            <i class="material-icons">close</i> Cerrar
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <div id="ModalPagoDetalle" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header" style="padding-top: 0px; padding-bottom: 0px;">
                            <h5 class="modal-title" runat="server">
                                <asp:Label Text="Detalle de la liga" runat="server" /></h5>
                            <asp:LinkButton data-dismiss="modal" aria-label="Close" CssClass="close" runat="server">
                            <i class="material-icons">close</i>
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="modal-body pt-0" style="padding-bottom: 0px; padding-left: 15px; padding-right: 15px;">
                    <div class="tab-content">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tbody>
                                            <tr>
                                                <td bgcolor="#b62322" align="center">
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                                                        <tbody>
                                                            <tr>
                                                                <td align="center" valign="top" style="padding-top: 40px;"></td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#b62322" align="center" style="padding: 0px 10px 0px 10px;">
                                                    <table bgcolor="#ffffff" border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                                                        <tbody>
                                                            <tr>
                                                                <td>
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td bgcolor="#ffffff" align="left" valign="top" style="border-radius: 4px 4px 0px 0px; padding-left: 10px; padding-right: 10px;"></td>
                                                                                <td bgcolor="#ffffff" align="right" valign="top" style="border-radius: 4px 4px 0px 0px; padding-left: 10px; padding-right: 10px;">
                                                                                    <img height="80" width="250" src="https://pagalaescuela.mx/Images/logo-cobroscontarjetas.png" alt="PagaLaEscuela" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td bgcolor="#ffffff" valign="top" style="border-radius: 4px 4px 0px 0px; padding-left: 10px; padding-right: 10px;">
                                                                                    <br />
                                                                                    <asp:Label ID="lblDetaCliente" Text="Cliente: " runat="server" />
                                                                                    <br />
                                                                                    <asp:Label ID="lblDetaIdentificador" Text="Identificador: " runat="server" />
                                                                                    <br />
                                                                                    <br />

                                                                                </td>
                                                                                <td bgcolor="#ffffff" valign="top" style="border-radius: 4px 4px 0px 0px; padding-left: 10px; padding-right: 10px;">
                                                                                    <br />
                                                                                    <asp:Label ID="lblDetaFHpago" Text="Fecha de pago: " runat="server" />
                                                                                    <br />
                                                                                    <asp:Label ID="lblDetaAsunto" Text="Asunto: " runat="server" />
                                                                                    <br />
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#b62322" align="center" style="padding: 0px 10px 0px 10px;">
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                                                        <tbody>
                                                            <tr>
                                                                <td bgcolor="#ffffff" align="left" style="padding: 10px 10px 10px 10px;">
                                                                    <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                                                        <thead>
                                                                            <tr>
                                                                                <th style="text-align: center; border-collapse: collapse; background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px;">N°</th>
                                                                                <th style="border-collapse: collapse; background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px;">CONCEPTO</th>
                                                                                <th style="text-align: right; border-collapse: collapse; background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px;">IMPORTE</th>
                                                                            </tr>
                                                                        </thead>
                                                                        <tbody>
                                                                            <asp:Repeater ID="rpDetalleLiga" runat="server">
                                                                                <ItemTemplate>
                                                                                    <tr>
                                                                                        <td style="border-bottom: 1px solid #ddd;" bgcolor="#ffffff" align="center"><%#Eval("IntNum")%></td>
                                                                                        <td style="border-bottom: 1px solid #ddd;" bgcolor="#ffffff"><%#Eval("VchConcepto")%></td>
                                                                                        <td style="border-bottom: 1px solid #ddd;" bgcolor="#ffffff" align="right">$<%#Eval("DcmImporte")%> </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>

                                                                            <tr id="trsubtotal" runat="server">
                                                                                <td style="font-weight: bold; padding-top: 15px; padding-bottom: 0px;" bgcolor="#ffffff" align="center"></td>
                                                                                <td style="font-weight: bold; padding-top: 15px; padding-bottom: 0px;" bgcolor="#ffffff" align="right">Subtotal:</td>
                                                                                <td style="font-weight: bold; padding-top: 15px; padding-bottom: 0px;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="DcmSubtotal" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trcomicion" runat="server">
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" bgcolor="#ffffff" align="center"></td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="VchComisionBancaria" runat="server" />
                                                                                </td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="DcmImpComisionBancaria" Style="color: #f55145;" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trpromocion" runat="server">
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" bgcolor="#ffffff" align="center"></td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="VchPromocion" runat="server" />
                                                                                </td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="DcmImpPromocion" Style="color: #f55145;" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" bgcolor="#ffffff" align="center"></td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">Importe pagado:</td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="DcmTotal" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <%--Movimientos de la liga--%>
                                                <tr>
                                                    <%--Movimientos de la liga--%>
                                                    <td bgcolor="#b62322" align="center" style="padding: 30px 10px 0px 10px;">
                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                                                            <tbody>
                                                                <tr>
                                                                    <td bgcolor="#ffffff" align="left" style="padding: 10px 10px 10px 10px;">
                                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                                                                            <thead>
                                                                                <tr>
                                                                                    <th colspan="3" style="border-collapse: collapse; background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px; padding-left: 8px;" align="left">MOVIMIENTOS DE LA LIGA
                                                                                    </th>
                                                                                </tr>
                                                                            </thead>
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td bgcolor="#ffffff" align="left" style="padding: 10px 0px 30px 0px; border-radius: 4px 4px 4px 4px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 25px;">
                                                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td bgcolor="#ffffff" align="left" style="padding: 10px 0px 10px 0px;">
                                                                                                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                                                                                            <thead>
                                                                                                                <tr>
                                                                                                                    <th style="text-align: center; border-collapse: collapse; background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px;">N°</th>
                                                                                                                    <th style="border-collapse: collapse; background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px;">FECHA</th>
                                                                                                                    <th style="text-align: right; border-collapse: collapse; background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px;">ESTATUS</th>
                                                                                                                </tr>
                                                                                                            </thead>
                                                                                                            <tbody>
                                                                                                                <asp:Repeater ID="rptMovimientosLiga" runat="server">
                                                                                                                    <ItemTemplate>
                                                                                                                        <tr>
                                                                                                                            <td style="border-bottom: 1px solid #ddd;" bgcolor="#ffffff" align="center"><%#Eval("IntNum")%></td>
                                                                                                                            <td style="border-bottom: 1px solid #ddd;" bgcolor="#ffffff"><%#Eval("DtmFechaDeRegistro")%></td>
                                                                                                                            <td style="border-bottom: 1px solid #ddd; color: <%#Eval("VchColor")%>; font-family: Comic Sans MS; font-weight: bold;" bgcolor="#ffffff" align="right"><%#Eval("VchEstatus")%> </td>
                                                                                                                        </tr>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:Repeater>
                                                                                                            </tbody>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#b62322" align="center" style="padding: 30px 10px 0px 10px;"></td>
                                                <%--#f4f4f4--%>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
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
                                                            <div class="form-group col-md-4">
                                                                <label for="txtIdentificador" style="color: black;">Identificador</label>
                                                                <asp:TextBox ID="txtIdentificador" CssClass="form-control" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="txtAsunto" style="color: black;">Asunto</label>
                                                                <asp:TextBox ID="txtAsunto" CssClass="form-control" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="txtConcepto" style="color: black;">Concepto</label>
                                                                <asp:TextBox ID="txtConcepto" CssClass="form-control" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-3">
                                                                <label for="ddlImporteMayor" style="color: black;">Subtotal</label>
                                                                <asp:DropDownList ID="ddlImporteMayor" CssClass="form-control" runat="server">
                                                                    <asp:ListItem Text="(>=) Mayor o igual que" Value=">=" />
                                                                    <asp:ListItem Text="(>) Mayor que" Value=">" />
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="form-group col-md-3">
                                                                <label for="txtImporte" style="color: black;"></label>
                                                                <asp:TextBox ID="txtImporteMayor" CssClass="form-control" placeholder="Mayor" aria-label="Search" Style="margin-top: 12px;" runat="server" />
                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtImporteMayor" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-3">
                                                                <label for="ddlImporteMenor" style="color: black;"></label>
                                                                <asp:DropDownList ID="ddlImporteMenor" CssClass="form-control" Style="margin-top: 6px;" runat="server">
                                                                    <asp:ListItem Text="(<=) Menor o igual que" Value="<=" />
                                                                    <asp:ListItem Text="(<) Menor que" Value="<" />
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="form-group col-md-3">
                                                                <label for="txtImporte" style="color: black;"></label>
                                                                <asp:TextBox ID="txtImporteMenor" CssClass="form-control" placeholder="Menor" aria-label="Search" Style="margin-top: 12px;" runat="server" />
                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtImporteMenor" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-6">
                                                                <label for="txtRegistroDesde" style="color: black;">Fecha Registro</label>
                                                                <asp:TextBox ID="txtRegistroDesde" CssClass="form-control" TextMode="Date" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-6">
                                                                <label for="txtRegistroHasta" style="color: black;"></label>
                                                                <asp:TextBox ID="txtRegistroHasta" CssClass="form-control" TextMode="Date" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-6">
                                                                <label for="txtVencimientoDesde" style="color: black;">Fecha Vencimiento</label>
                                                                <asp:TextBox ID="txtVencimientoDesde" CssClass="form-control" TextMode="Date" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-6">
                                                                <label for="txtVencimientoHasta" style="color: black;"></label>
                                                                <asp:TextBox ID="txtVencimientoHasta" CssClass="form-control" TextMode="Date" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-3">
                                                                <label for="ddlEstatus" style="color: black;">Estatus</label>
                                                                <asp:DropDownList ID="ddlEstatus" CssClass="form-control" Style="margin-top: 6px;" runat="server">
                                                                    <asp:ListItem Text="Todos" Value="" />
                                                                    <asp:ListItem Text="approved" Value="approved" />
                                                                    <asp:ListItem Text="denied" Value="denied" />
                                                                    <asp:ListItem Text="error" Value="error" />
                                                                    <%--<asp:ListItem Text="Pendiente" Value="Pendiente" />--%>
                                                                </asp:DropDownList>
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
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <div id="ModalPagar" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header" style="padding-bottom: 0px; padding-top: 0px;">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="lblTitlePagar" Text="Pagar" runat="server" /></h5>

                            <asp:LinkButton ID="btnCerrar" Visible="false" class="close" data-dismiss="modal" aria-label="Close" runat="server">
                                <span aria-hidden="true">&times;</span>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnFinalizar" OnClick="btnFinalizar_Click" Visible="false" CssClass="btn btn-info btn-round" Style="padding-bottom: 5px; padding-top: 5px; padding-left: 5px; padding-right: 5px;" runat="server">
                            <i class="material-icons">close</i> Finalizar
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="modal-body pt-0" style="padding-bottom: 0px;">
                    <div class="tab-content">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlPromociones" runat="server">
                                    <div style="position: absolute; width: 100%; height: 50%; background-color: #b62322; left: 0px;"></div>
                                    <div class="row">
                                        <div style="width: 100%;">
                                            <div style="width: 80%; margin: 0 auto; display: block;">
                                                <div class="card">
                                                    <div class="card-body">
                                                        <div class="form-group col-md-12">
                                                            <label for="lblConcepto" style="color: black;">Concepto</label>
                                                            <div class="input-group">
                                                                <div class="input-group-prepend">
                                                                    <span class="input-group-text" style="padding-left: 0px;">
                                                                        <i class="material-icons">assignment</i>
                                                                    </span>
                                                                </div>
                                                                <asp:Label ID="lblConcepto" Text="Concepto" CssClass="form-control" runat="server" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group col-md-12">
                                                            <label for="lblVencimiento" style="color: black;">F/H Vencimiento</label>
                                                            <div class="input-group">
                                                                <div class="input-group-prepend">
                                                                    <span class="input-group-text" style="padding-left: 0px;">
                                                                        <i class="material-icons">date_range</i>
                                                                    </span>
                                                                </div>
                                                                <asp:Label ID="lblVencimiento" Text="12/09/2020" CssClass="form-control" runat="server" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group col-md-12">
                                                            <label for="ddlFormasPago" style="color: #ff9800;">Forma de pago</label>
                                                            <div class="input-group">
                                                                <div class="input-group-prepend">
                                                                    <span class="input-group-text" style="padding-left: 0px;">
                                                                        <i class="material-icons">format_list_numbered</i>
                                                                    </span>
                                                                </div>
                                                                <asp:DropDownList ID="ddlFormasPago" OnSelectedIndexChanged="ddlFormasPago_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control" runat="server">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="form-group col-md-12">
                                                            <label for="lblTotal" style="color: black;">Total a pagar</label>
                                                            <div class="input-group">
                                                                <div class="input-group-prepend">
                                                                    <span class="input-group-text" style="padding-left: 0px;">
                                                                        <i class="material-icons">payment</i>
                                                                    </span>
                                                                </div>
                                                                <asp:Label ID="lblTotal" Text="$150.00" CssClass="form-control" runat="server" />
                                                            </div>
                                                        </div>

                                                        <div class="pull-right">
                                                            <asp:LinkButton ID="btnGenerarLiga" OnClick="btnGenerarLiga_Click" ToolTip="Generar pago" runat="server">
                                                                <asp:Label class="btn btn-success btn-round" runat="server">
                                                                    <asp:Label ID="lblTotalPago" Text="Generar pago $0.00" runat="server" /><i class="material-icons">arrow_forward</i>
                                                                </asp:Label>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlIframe" Visible="false" runat="server">
                                    <div class="row">
                                        <div style="width: 100%;">
                                            <iframe id="ifrLiga" style="width: 80%; margin: 0 auto; display: block;" width="450px" height="650px" class="centrado" src="https://u.mitec.com.mx/p/i/S3SEJ1IO" frameborder="0" seamless="seamless" runat="server"></iframe>
                                        </div>
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="modal-footer justify-content-center">
                </div>
            </div>
        </div>
    </div>

    <!--END MODAL-->

    <script>
        function showModalBusqueda() {
            $('#ModalBusqueda').modal('show');
        }

        function hideModalBusqueda() {
            $('#ModalBusqueda').modal('hide');
        }

        function showModalDetalle() {
            $('#ModalDetalle').modal('show');
        }

        function hideModalDetalle() {
            $('#ModalDetalle').modal('hide');
        }

        function showModalPagar() {
            $('#ModalPagar').modal({ backdrop: 'static', keyboard: false, show: true });
        }

        function hideModalPagar() {
            $('#ModalPagar').modal('hide');
        }
    </script>
    <script>
        function showModalPagoDetalle() {
            $('#ModalPagoDetalle').modal('show');
        }

        function hideModalPagoDetalle() {
            $('#ModalPagoDetalle').modal('hide');
        }
    </script>
</asp:Content>
