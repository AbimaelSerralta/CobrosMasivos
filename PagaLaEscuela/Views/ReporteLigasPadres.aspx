﻿<%@ Page Title="ReporteLigasPadres" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReporteLigasPadres.aspx.cs" Inherits="PagaLaEscuela.Views.ReporteLigasPadres" %>

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
                                <%--#00bcd4--%>
                                <div class="card-header card-header-tabs card-header-primary" style="background: #0099d4; padding-top: 0px; padding-bottom: 0px;">
                                    <div class="nav-tabs-navigation">
                                        <div class="nav-tabs-wrapper">
                                            <div class="form-group" style="margin-top: 0px; padding-bottom: 0px;">
                                                <asp:LinkButton ID="btnFiltros" OnClick="btnFiltros_Click" ToolTip="Filtros de busqueda." BackColor="#4db6ac" class="btn btn-lg btn-fab btn-fab-mini btn-round" runat="server">
                                                        <i class="material-icons">search</i>
                                                </asp:LinkButton>
                                                <asp:Label Text="Listado de pagos" runat="server" />

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
                                            <asp:GridView ID="gvPagos" OnPageIndexChanging="gvPagos_PageIndexChanging" OnSorting="gvPagos_Sorting" OnRowCommand="gvPagos_RowCommand" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidFechaColegiatura" GridLines="None" border="0" AllowPaging="true" PageSize="10" runat="server">
                                                <EmptyDataTemplate>
                                                    <div class="alert alert-info">No hay nada por pagar</div>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField SortExpression="VchIdentificador" HeaderText="COLEGIATURA">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGvIdentificador" ToolTip='<%#Eval("VchIdentificador")%>' Style="width: 100%; text-overflow: ellipsis;" Text='<%#Eval("VchIdentificador")%>' Enabled="false" BackColor="Transparent" BorderStyle="None" runat="server" />
                                                            <asp:TextBox ID="txtGvUidCliente" Text='<%#Eval("UidCliente")%>' Visible="false" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField SortExpression="VchMatricula" DataField="VchMatricula" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="MATRICULA" />
                                                    <asp:TemplateField SortExpression="NombreCompleto" HeaderStyle-CssClass="text-center" HeaderText="ALUMNO">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGvNombreCompleto" ToolTip='<%#Eval("NombreCompleto")%>' Style="width: 100%; text-overflow: ellipsis;" Text='<%#Eval("NombreCompleto")%>' Enabled="false" BackColor="Transparent" BorderStyle="None" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField SortExpression="VchNum" DataField="VchNum" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="# DE PAGOS" />
                                                    <asp:BoundField SortExpression="DcmImporte" DataField="DcmImporte" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" DataFormatString="{0:C}" HeaderText="IMPORTE" />
                                                    <asp:BoundField SortExpression="ImpPagado" DataField="ImpPagado" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" DataFormatString="{0:C}" HeaderText="ABONADO" />
                                                    <asp:BoundField SortExpression="ImpTotal" DataField="ImpTotal" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" DataFormatString="{0:C}" HeaderText="SALDO" />
                                                    <asp:BoundField SortExpression="DtFHInicio" DataField="DtFHInicio" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" DataFormatString="{0:dd/MM/yyyy}" HeaderText="INICIO" />
                                                    <asp:BoundField SortExpression="VchFHLimite" DataField="VchFHLimite" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="LIMITE" />
                                                    <asp:BoundField SortExpression="VchFHVencimiento" DataField="VchFHVencimiento" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="VENCIMIENTO" />
                                                    <asp:TemplateField SortExpression="VchEstatusFechas" HeaderText="ESTATUS">
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%#Eval("VchEstatusFechas")%>' ForeColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("VchColor").ToString()) %>' Font-Names="Comic Sans MS" Font-Bold="true" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <table>
                                                                <tbody>
                                                                    <tr style="background: transparent;">
                                                                        <asp:Panel Visible='false' runat="server">
                                                                            <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                                <asp:LinkButton ID="btnFormasPago" ToolTip="Pagar por otro medio" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="btnFormasPago" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label class="btn btn-sm btn-success btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">add</i>
                                                                                </asp:Label>
                                                                                </asp:LinkButton>
                                                                            </td>
                                                                        </asp:Panel>
                                                                        <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:LinkButton ID="btnPagos" ToolTip="Pagos realizados" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="btnPagos" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label class="btn btn-sm btn-info btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">info_outline</i>
                                                                                </asp:Label>
                                                                            </asp:LinkButton>
                                                                        </td>
                                                                        <td style="display:none; border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:LinkButton ID="btnReportePagos" ToolTip="Reporte de pagos" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="btnReportePagos" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label class="btn btn-sm btn-warning btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">description</i>
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
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvLigasGeneradas" Visible="false" OnSorting="gvLigasGeneradas_Sorting" OnRowCommand="gvLigasGeneradas_RowCommand" OnRowDataBound="gvLigasGeneradas_RowDataBound" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidLigaUrl" GridLines="None" border="0" AllowPaging="true" PageSize="10" OnPageIndexChanging="gvLigasGeneradas_PageIndexChanging" runat="server">
                                                <EmptyDataTemplate>
                                                    <div class="alert alert-info">No hay pagos registrados</div>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:BoundField SortExpression="VchNombreComercial" DataField="VchNombreComercial" HeaderStyle-CssClass="text-center" HeaderText="ESCUELA" />
                                                    <asp:BoundField SortExpression="VchConcepto" DataField="VchConcepto" HeaderStyle-CssClass="text-center" HeaderText="CONCEPTO" />
                                                    <asp:BoundField SortExpression="DtVencimiento" DataField="DtVencimiento" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" HeaderText="FECHA DE PAGO" />
                                                    <asp:BoundField SortExpression="DcmImporte" DataField="DcmImporte" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-right" DataFormatString="{0:C}" HeaderText="IMPORTE" />
                                                    <asp:BoundField SortExpression="VchPromocion" DataField="VchPromocion" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" HeaderText="PROMOCIÓN" />
                                                    <asp:BoundField SortExpression="DcmImportePromocion" DataField="DcmImportePromocion" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-right" DataFormatString="{0:C}" HeaderText="PAGADO" />
                                                    <%--<asp:BoundField SortExpression="VchEstatus" DataField="VchEstatus" HeaderText="ESTATUS" />--%>
                                                    <asp:TemplateField SortExpression="VchEstatus" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" HeaderText="ESTATUS">
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%#Eval("VchEstatus")%>' ForeColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("VchColor").ToString()) %>' Font-Size="Larger" Font-Names="Comic Sans MS" Font-Bold="true" runat="server"></asp:Label>
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
                                                                <label for="ddlImporteMayor" style="color: black;">Importe</label>
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
                                                                    <asp:ListItem Text="Pendiente" Value="Pendiente" />
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

    <div id="ModalPagos" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="Label1" Text="Pago(s) de la colegiatura" runat="server" /></h5>
                            <asp:LinkButton ID="LinkButton3" data-dismiss="modal" aria-label="Close" CssClass="btn btn-danger btn-round" Style="padding-bottom: 5px; padding-top: 5px; padding-left: 5px; padding-right: 5px;" runat="server">
                            <i class="material-icons">close</i>
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="modal-body pt-0" style="padding-bottom: 0px;">
                    <div class="tab-content">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="Panel1" Visible="false" runat="server">
                                    <div id="div1" role="alert" runat="server">
                                        <asp:Label ID="Label2" runat="server" />
                                        <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    </div>
                                </asp:Panel>
                                <div class="row">
                                    <div class="col-lg-12 col-md-12">
                                        <div class="card" style="margin-top: 15px; margin-bottom: 15px;">
                                            <div class="card-body">
                                                <div class="row">
                                                    <div class="table-responsive">
                                                        <asp:GridView ID="gvPagosColegiaturas" OnRowCommand="gvPagosColegiaturas_RowCommand" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidPagoColegiatura" GridLines="None" border="0" AllowPaging="true" PageSize="10" OnPageIndexChanging="gvPagosColegiaturas_PageIndexChanging" runat="server">
                                                            <EmptyDataTemplate>
                                                                <div class="alert alert-info">No hay pagos registrados</div>
                                                            </EmptyDataTemplate>
                                                            <Columns>
                                                                <asp:BoundField SortExpression="NombreCompleto" DataField="NombreCompleto" HeaderText="TUTOR" />
                                                                <asp:BoundField SortExpression="DtFHPago" DataField="DtFHPago" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" HeaderText="FECHA PAGO" />
                                                                <asp:BoundField SortExpression="VchFormaPago" DataField="VchFormaPago" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" HeaderText="FORMA DE PAGO" />
                                                                <asp:BoundField SortExpression="DcmImportePagado" DataField="DcmImportePagado" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-right" DataFormatString="{0:C}" HeaderText="IMPORTE PAGADO" />
                                                                <asp:TemplateField SortExpression="VchEstatus" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" HeaderText="ESTATUS">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblGvUidFormaPago" Text='<%#Eval("UidFormaPago")%>' Visible="false" runat="server" />
                                                                        <asp:Label Text='<%#Eval("VchEstatus")%>' ForeColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("VchColor").ToString()) %>' Font-Names="Comic Sans MS" Font-Bold="true" runat="server"></asp:Label>
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
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <%--<div class="modal-footer justify-content-center">
                            <asp:LinkButton ID="LinkButton2" data-dismiss="modal" aria-label="Close" CssClass="btn btn-info btn-round" runat="server">
                            <i class="material-icons">close</i> Cerrar
                            </asp:LinkButton>
                        </div>--%>
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
                                <asp:Label ID="Label3" Text="Detalle del pago" runat="server" /></h5>
                            <asp:LinkButton ID="LinkButton2" data-dismiss="modal" aria-label="Close" CssClass="close" runat="server">
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
                                                                <td align="center" valign="top" style="padding-top: 80px;"></td>
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
                                                                                    <img height="80" width="250" src="https://pagalaescuela.mx/images/logoCompetoPagaLaEscuela.png" alt="PagaLaEscuela" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td bgcolor="#ffffff" valign="top" style="border-radius: 4px 4px 0px 0px; padding-left: 10px; padding-right: 10px;">
                                                                                    <br />
                                                                                    <asp:Label ID="lblDetaAlumno" Text="Alumno: " runat="server" />
                                                                                    <br />
                                                                                    <asp:Label ID="lblDetaMatricula" Text="Matricula: " runat="server" />
                                                                                    <br />
                                                                                    <br />

                                                                                </td>
                                                                                <td bgcolor="#ffffff" valign="top" style="border-radius: 4px 4px 0px 0px; padding-left: 10px; padding-right: 10px;">
                                                                                    <br />
                                                                                    <asp:Label ID="lblDetaFHpago" Text="Fecha de pago: " runat="server" />
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
                                                                                <th style="border-collapse: collapse; background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px;" align="center">N°</th>
                                                                                <th style="border-collapse: collapse; background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px;" align="left">CONCEPTO</th>
                                                                                <th style="border-collapse: collapse; background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px;" align="right">PRECIO</th>
                                                                            </tr>
                                                                        </thead>
                                                                        <tbody>
                                                                            <asp:Repeater ID="rpDetalleLiga" runat="server">
                                                                                <ItemTemplate>
                                                                                    <tr>
                                                                                        <td style="border-bottom: 1px solid #ddd;" bgcolor="#ffffff" align="center"><%#Eval("IntNum")%></td>
                                                                                        <td style="border-bottom: 1px solid #ddd;" bgcolor="#ffffff"><%#Eval("VchDescripcion")%></td>
                                                                                        <td style="border-bottom: 1px solid #ddd; color: <%#Eval("VchColor")%>;" bgcolor="#ffffff" align="right">$<%#Eval("DcmImporte")%> </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>

                                                                            <tr id="trsubtotall" runat="server">
                                                                                <td style="font-weight: bold; padding-top: 15px; padding-bottom: 0px;" bgcolor="#ffffff" align="center"></td>
                                                                                <td style="font-weight: bold; padding-top: 15px; padding-bottom: 0px;" bgcolor="#ffffff" align="right">Subtotal:</td>
                                                                                <td style="font-weight: bold; padding-top: 15px; padding-bottom: 0px;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="DcmSubtotal" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trvalidarimportee" runat="server">
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" bgcolor="#ffffff" align="center"></td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">Importe por validar:
                                                                                </td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="DcmValidarImporte" Style="color: #f55145;" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" bgcolor="#ffffff" align="center"></td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">Importe pagado:</td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="DcmTotal" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trcomicion" runat="server">
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" bgcolor="#ffffff" align="center"></td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="VchComicionBancaria" runat="server" />
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
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">Abono pago:
                                                                                </td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="DcmImpAbono" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" bgcolor="#ffffff" align="center"></td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">Resta:
                                                                                </td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="DcmImpResta" runat="server" />
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
                                            <tr id="trdetallepromociones" runat="server">
                                                <%--Detalle de promocion--%>
                                                <td bgcolor="#b62322" align="center" style="padding: 30px 10px 0px 10px;">
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                                                        <tbody>
                                                            <tr>
                                                                <td bgcolor="#ffffff" align="left" style="padding: 10px 10px 10px 10px;">
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                                                                        <thead>
                                                                            <tr>
                                                                                <th colspan="3" style="border-collapse: collapse; background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px; padding-left: 8px;" align="left">DETALLE DE PAGOS DE LA PROMOCIÓN
                                                                                </th>
                                                                            </tr>
                                                                        </thead>
                                                                        <tbody>
                                                                            <tr>
                                                                                <td bgcolor="#ffffff" align="left" style="padding: 30px 30px 30px 30px; border-radius: 4px 4px 4px 4px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 25px;">
                                                                                    <p style="color: #111111; margin: 0; font-size: 14px;">
                                                                                        <asp:Label ID="VchDetallePromocion" runat="server" />
                                                                                    </p>
                                                                                </td>
                                                                                <td bgcolor="#ffffff" align="left" style="padding: 30px 30px 30px 30px; border-radius: 4px 4px 4px 4px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 25px;">
                                                                                    <p style="color: #111111; margin: 0; font-size: 14px;">$<asp:Label ID="DcmImpDetallePromocion" runat="server" /></p>
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
                                            <tr id="trdetalleoperacion" runat="server">
                                                <%--Detalle de la operacion--%>
                                                <td bgcolor="#b62322" align="center" style="padding: 30px 10px 0px 10px;">
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                                                        <tbody>
                                                            <tr>
                                                                <td bgcolor="#ffffff" align="left" style="padding: 10px 10px 10px 10px;">
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                                                                        <thead>
                                                                            <tr>
                                                                                <th colspan="3" style="border-collapse: collapse; background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px; padding-left: 8px;" align="left">DETALLE DE LA OPERACIÓN
                                                                                </th>
                                                                            </tr>
                                                                        </thead>
                                                                        <tbody>
                                                                            <tr>
                                                                                <td bgcolor="#ffffff" align="left" style="padding: 30px 30px 30px 30px; border-radius: 4px 4px 4px 4px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 25px;">
                                                                                    <p style="color: #111111; margin: 0; font-size: 14px;">Referencia:</p>
                                                                                    <p style="color: #111111; margin: 0; font-size: 14px;">
                                                                                        <asp:Label ID="VchIdreferencia" runat="server" />
                                                                                    </p>
                                                                                    <br />
                                                                                    <p style="color: #111111; margin: 0; font-size: 14px;">Fecha:</p>
                                                                                    <p style="color: #111111; margin: 0; font-size: 14px;">
                                                                                        <asp:Label ID="DtmFechaDeRegistro" runat="server" />
                                                                                    </p>
                                                                                    <br />
                                                                                    <p style="color: #111111; margin: 0; font-size: 14px;">Tarjeta de pago:</p>
                                                                                    <p style="color: #111111; margin: 0; font-size: 14px;">
                                                                                        <asp:Label ID="VchTarjeta" runat="server" />
                                                                                    </p>
                                                                                </td>
                                                                                <td bgcolor="#ffffff" align="left" style="padding: 30px 30px 30px 30px; border-radius: 4px 4px 4px 4px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 25px;">
                                                                                    <p style="color: #111111; margin: 0; font-size: 14px;">Folio:</p>
                                                                                    <p style="color: #111111; margin: 0; font-size: 14px;">
                                                                                        <asp:Label ID="VchFolioPago" runat="server" />
                                                                                    </p>
                                                                                    <br />
                                                                                    <p style="color: #111111; margin: 0; font-size: 14px;">Hora:</p>
                                                                                    <p style="color: #111111; margin: 0; font-size: 14px;">
                                                                                        <asp:Label ID="DtmHoraDeRegistro" runat="server" />
                                                                                    </p>
                                                                                    <br />
                                                                                    <br />
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
    <div id="ModalPagoDetalleManual" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header" style="padding-top: 0px; padding-bottom: 0px;">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="Label4" Text="Detalle del pago" runat="server" /></h5>
                            <asp:LinkButton ID="LinkButton1" data-dismiss="modal" aria-label="Close" CssClass="close" runat="server">
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
                                                                <td align="center" valign="top" style="padding-top: 80px;"></td>
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
                                                                                    <img height="80" width="250" src="https://pagalaescuela.mx/images/logoCompetoPagaLaEscuela.png" alt="PagaLaEscuela" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td bgcolor="#ffffff" valign="top" style="border-radius: 4px 4px 0px 0px; padding-left: 10px; padding-right: 10px;">
                                                                                    <br />
                                                                                    <asp:Label ID="lblDetaAlumnoManual" Text="Alumno: " runat="server" />
                                                                                    <br />
                                                                                    <asp:Label ID="lblDetaMatriculaManual" Text="Matricula: " runat="server" />
                                                                                    <br />
                                                                                    <br />

                                                                                </td>
                                                                                <td bgcolor="#ffffff" valign="top" style="border-radius: 4px 4px 0px 0px; padding-left: 10px; padding-right: 10px;">
                                                                                    <br />
                                                                                    <asp:Label ID="lblDetaFHpagoManual" Text="Fecha de pago: " runat="server" />
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
                                                                                <th style="border-collapse: collapse; background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px;" align="center">N°</th>
                                                                                <th style="border-collapse: collapse; background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px;" align="left">CONCEPTO</th>
                                                                                <th style="border-collapse: collapse; background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px;" align="right">PRECIO</th>
                                                                            </tr>
                                                                        </thead>
                                                                        <tbody>
                                                                            <asp:Repeater ID="rpDetalleLigaManual" runat="server">
                                                                                <ItemTemplate>
                                                                                    <tr>
                                                                                        <td style="border-bottom: 1px solid #ddd;" bgcolor="#ffffff" align="center"><%#Eval("IntNum")%></td>
                                                                                        <td style="border-bottom: 1px solid #ddd;" bgcolor="#ffffff"><%#Eval("VchDescripcion")%></td>
                                                                                        <td style="border-bottom: 1px solid #ddd; color: <%#Eval("VchColor")%>;" bgcolor="#ffffff" align="right">$<%#Eval("DcmImporte")%> </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>

                                                                            <tr>
                                                                                <td style="font-weight: bold; padding-top: 15px; padding-bottom: 0px;" bgcolor="#ffffff" align="center"></td>
                                                                                <td style="font-weight: bold; padding-top: 15px; padding-bottom: 0px;" bgcolor="#ffffff" align="right"></td>
                                                                                <td style="font-weight: bold; padding-top: 15px; padding-bottom: 0px;" bgcolor="#ffffff" align="right"></td>
                                                                            </tr>
                                                                            <tr id="trSubtotalManual" runat="server">
                                                                                <td style="font-weight: bold; padding-top: 0px; padding-bottom: 0px;" bgcolor="#ffffff" align="center"></td>
                                                                                <td style="font-weight: bold; padding-top: 0px; padding-bottom: 0px;" bgcolor="#ffffff" align="right">Subtotal:</td>
                                                                                <td style="font-weight: bold; padding-top: 0px; padding-bottom: 0px;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="DcmSubtotalManual" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trValidarImporteManual" runat="server">
                                                                                <td style="font-weight: bold; padding-top: 0px; padding-bottom: 0px;" bgcolor="#ffffff" align="center"></td>
                                                                                <td style="font-weight: bold; padding-top: 0px; padding-bottom: 0px;" bgcolor="#ffffff" align="right">Importe por validar:</td>
                                                                                <td style="font-weight: bold; padding-top: 0px; padding-bottom: 0px;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="DcmValidarImporteManual" Style="color: #f55145;" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="font-weight: bold; padding-top: 0px; padding-bottom: 0px;" bgcolor="#ffffff" align="center"></td>
                                                                                <td style="font-weight: bold; padding-top: 0px; padding-bottom: 0px;" bgcolor="#ffffff" align="right">Total:</td>
                                                                                <td style="font-weight: bold; padding-top: 0px; padding-bottom: 0px;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="DcmTotalManual" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" bgcolor="#ffffff" align="center"></td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">Importe pagado:</td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="DcmImportePagadoManual" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" bgcolor="#ffffff" align="center"></td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">Resta:
                                                                                </td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="DcmImpRestaManual" runat="server" />
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
                                            <tr id="trDetalleOperacionManual" runat="server">
                                                <%--Detalle de la operacion--%>
                                                <td bgcolor="#b62322" align="center" style="padding: 30px 10px 0px 10px;">
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                                                        <tbody>
                                                            <tr>
                                                                <td bgcolor="#ffffff" align="left" style="padding: 10px 10px 10px 10px;">
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                                                                        <thead>
                                                                            <tr>
                                                                                <th colspan="3" style="border-collapse: collapse; background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px; padding-left: 8px;" align="left">DETALLE DE LA OPERACIÓN
                                                                                </th>
                                                                            </tr>
                                                                        </thead>
                                                                        <tbody>
                                                                            <tr>
                                                                                <td bgcolor="#ffffff" align="left" style="padding: 30px 30px 30px 30px; border-radius: 4px 4px 4px 4px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 25px;">
                                                                                    <p style="color: #111111; margin: 0; font-size: 14px;">Banco:</p>
                                                                                    <p style="color: #111111; margin: 0; font-size: 14px;">
                                                                                        <asp:Label ID="VchBancoManual" runat="server" />
                                                                                    </p>
                                                                                    <br />
                                                                                    <p style="color: #111111; margin: 0; font-size: 14px;">Fecha:</p>
                                                                                    <p style="color: #111111; margin: 0; font-size: 14px;">
                                                                                        <asp:Label ID="DtFechaPagoManual" runat="server" />
                                                                                    </p>
                                                                                    <br />
                                                                                    <p style="color: #111111; margin: 0; font-size: 14px;">Folio:</p>
                                                                                    <p style="color: #111111; margin: 0; font-size: 14px;">
                                                                                        <asp:Label ID="VchFolioManual" runat="server" />
                                                                                    </p>
                                                                                </td>
                                                                                <td bgcolor="#ffffff" align="left" style="padding: 30px 30px 30px 30px; border-radius: 4px 4px 4px 4px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 25px;">
                                                                                    <p style="color: #111111; margin: 0; font-size: 14px;">Cuenta:</p>
                                                                                    <p style="color: #111111; margin: 0; font-size: 14px;">
                                                                                        <asp:Label ID="VchCuentaManual" runat="server" />
                                                                                    </p>
                                                                                    <br />
                                                                                    <p style="color: #111111; margin: 0; font-size: 14px;">Hora:</p>
                                                                                    <p style="color: #111111; margin: 0; font-size: 14px;">
                                                                                        <asp:Label ID="DtHoraPagoManual" runat="server" />
                                                                                    </p>
                                                                                    <br />
                                                                                    <br />
                                                                                    <br />
                                                                                </td>
                                                                            </tr>
                                                                            <%--<tr>
                                                <td bgcolor="#ffffff" align="left" style="padding: 30px 30px 30px 30px; border-radius: 4px 4px 4px 4px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 25px;">
                                                    <p style="color: #111111;margin: 0; font-size: 14px;">Saludos,</p>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <p style="color: #111111;margin: 0; font-size: 14px;">Equipo </p>
                                                            </td>
                                                            <td>
                                                                <img height="50" width="200" src="https://pagalaescuela.mx/images/logo-cobroscontarjetas.png" alt="Alternate Text" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td bgcolor="#ffffff" align="left" style="padding: 30px 30px 30px 30px; border-radius: 4px 4px 4px 4px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 25px;">
                                                </td>
                                            </tr>--%>
                                                                        </tbody>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </td>
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

    <div id="ModalTipoPago" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="TitleModal" aria-hidden="true" style="overflow-y: scroll;">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header" style="padding-top: 0px; padding-bottom: 0px;">
                            <h5 id="TitleModal" class="modal-title" runat="server">
                                <asp:Label ID="lblTitleModalTipoPago" Text="Seleccione la forma de pago" runat="server" /></h5>
                            <asp:LinkButton ID="btnCancelarModalTipoPago" data-dismiss="modal" aria-label="Close" CssClass="btn btn-danger btn-round" Style="padding-bottom: 5px; padding-top: 5px; padding-left: 5px; padding-right: 5px;" runat="server">
                            <i class="material-icons">close</i>
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="modal-body pt-0" style="padding-bottom: 0px;">
                    <div class="tab-content">
                        <div class="row">
                            <div class="card card-nav-tabs">
                                <div class="card-header card-header-primary" style="background: #0099d4;">
                                    <!-- colors: "header-primary", "header-info", "header-success", "header-warning", "header-danger" -->
                                    <div class="nav-tabs-navigation">
                                        <div class="nav-tabs-wrapper">
                                            <ul class="nav nav-tabs" id="ulTabAgregarPago" data-tabs="tabs">
                                                <li class="nav-item">
                                                    <a id="aFormaPago" class="nav-link active show" href="#formapago" data-toggle="tab">
                                                        <i class="material-icons">view_module</i>Paso 1<div class="ripple-container"></div>
                                                    </a>
                                                </li>

                                                <li class="nav-item">
                                                    <a id="aPago" class="nav-link disabled" href="#pago" data-toggle="tab">
                                                        <i class="material-icons">request_page</i>Finalizar<div class="ripple-container"></div>
                                                    </a>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>

                                <div class="card-body" style="padding-top: 0px; padding-bottom: 0px;">
                                    <div class="tab-content">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <asp:Panel ID="pnlAlertModalTipoPago" Visible="false" runat="server">
                                                    <div id="divAlertModalTipoPago" class="alert alert-danger alert-dismissible fade" role="alert" runat="server">
                                                        <asp:Label runat="server" ID="lblMnsjModalTipoPago" />
                                                        <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                    </div>
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                        <div class="tab-pane active show" id="formapago">
                                            <asp:UpdatePanel runat="server">
                                                <ContentTemplate>
                                                    <%--ESTO LO UTILIZARE--%>
                                                    <%--<asp:GridView ID="gvFormasPago" Width="100%" ShowHeader="false" GridLines="None" AutoGenerateColumns="false" runat="server">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <div class="row">
                                                                                <div class="col-sm-12 col-md-12 col-lg-6 col-xl-6" style="cursor: pointer;">
                                                                                    <div class="card cardEfe" style="margin-top: 15px; margin-bottom: 15px;">
                                                                                        <div class="card-body">
                                                                                            <img src="../Images/FormasDePago/<%#Eval("VchImagen")%>" style="background-color: #d8ecfe" height="70" width="70" class="float-left rounded-circle">
                                                                                            <div style="padding-left: 80px;">
                                                                                                <h5 class="card-title" style="font-weight: bold;"><%#Eval("VchDescripcion")%></h5>
                                                                                                <p class="card-text">Captura información adicional.</p>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>--%>
                                                    <div class="row">
                                                        <div class="col-lg-6">
                                                            <asp:Repeater ID="rpFormasPago" OnItemCommand="rpFormasPago_ItemCommand" OnItemDataBound="rpFormasPago_ItemDataBound" runat="server">
                                                                <ItemTemplate>
                                                                    <div class="col-sm-12 col-md-12 col-lg-12 col-xl-12" style="cursor: pointer;">
                                                                        <asp:UpdatePanel runat="server">
                                                                            <ContentTemplate>
                                                                                <asp:LinkButton ID="btnSeleFormaPago" CommandArgument='<%#Eval("UidFormaPago")%>' CommandName="btnSeleFormaPago" runat="server">
                                                                                    <div class="card cardEfe" style="margin-top: 15px; margin-bottom: 15px; border-left: 8px solid <%#Eval("VchColor")%>;">
                                                                                        <asp:CheckBox ID="cbSeleccionado" CssClass="form-check-input" Style="margin-left: 8px;" runat="server" />
                                                                                        <div class="card-body">
                                                                                            <%--<asp:Label ID="lblSeleccionado" Width="100%" Style="background-color: red;" runat="server" />--%>
                                                                                            <img src="../Images/FormasDePago/<%#Eval("VchImagen")%>" style="background-color: #d8ecfe" height="70" width="70" class="float-left rounded-circle">
                                                                                            <div style="padding-left: 80px;">
                                                                                                <h5 class="card-title" style="font-weight: bold;"><%#Eval("VchDescripcion")%></h5>
                                                                                                <p class="card-text">Captura información adicional.</p>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </asp:LinkButton>
                                                                            </ContentTemplate>
                                                                            <Triggers>
                                                                                <asp:AsyncPostBackTrigger ControlID="btnSeleFormaPago" EventName="Click" />
                                                                            </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                        <div class="col-lg-6">
                                                            <div class="card" style="margin-top: 15px; margin-bottom: 15px;">
                                                                <div class="card-body">
                                                                    <div class="row">
                                                                        <div class="form-group col-md-12">
                                                                            <label for="txtBanco" style="color: black;">Banco</label>
                                                                            <div class="input-group">
                                                                                <div class="input-group-prepend">
                                                                                    <span class="input-group-text" style="padding-left: 0px;">
                                                                                        <i class="material-icons">account_balance</i>
                                                                                    </span>
                                                                                </div>
                                                                                <asp:DropDownList ID="ddlBanco" CssClass="form-control" runat="server">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group col-md-12">
                                                                            => Sobre la cuenta receptora
=> Correo de envio de verificacion de pago

Panel tuotr
=> += Ultimos 4 digitos de la cuenta receptor

                                                                            <label for="txtCuenta" style="color: black;">Ultimos 4 digitos del banco receptor</label>
                                                                            <div class="input-group">
                                                                                <div class="input-group-prepend">
                                                                                    <span class="input-group-text" style="padding-left: 0px;">
                                                                                        <i class="material-icons">account_box</i>
                                                                                    </span>
                                                                                </div>
                                                                                <asp:TextBox ID="txtCuenta" MaxLength="4" CssClass="form-control" runat="server" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group col-md-12">
                                                                            <label for="txtFechaPago" style="color: black;">Fecha y Hora</label>
                                                                            <div class="input-group">
                                                                                <div class="input-group-prepend">
                                                                                    <span class="input-group-text" style="padding-left: 0px;">
                                                                                        <i class="material-icons">date_range</i>
                                                                                    </span>
                                                                                </div>
                                                                                <asp:TextBox ID="txtFHPago" TextMode="DateTimeLocal" CssClass="form-control" runat="server" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group col-md-12">
                                                                            <label for="lblImporteCo" style="color: black;">Monto</label>
                                                                            <div class="input-group">
                                                                                <div class="input-group-prepend">
                                                                                    <span class="input-group-text" style="padding-left: 0px;">
                                                                                        <i class="material-icons">attach_money</i>
                                                                                    </span>
                                                                                </div>
                                                                                <asp:TextBox ID="txtMontoPagado" Text="0.00" CssClass="form-control" runat="server" />
                                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtMontoPagado" runat="server" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group col-md-12">
                                                                            <label for="lblImporteBe" style="color: black;">Folio</label>
                                                                            <div class="input-group" style="padding-top: 7px;">
                                                                                <div class="input-group-prepend">
                                                                                    <span class="input-group-text" style="padding-left: 0px;">
                                                                                        <i class="material-icons">article</i>
                                                                                    </span>
                                                                                </div>
                                                                                <asp:TextBox ID="txtFolioPago" CssClass="form-control" runat="server" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </div>

                                        <div class="tab-pane" id="pago">
                                            <asp:UpdatePanel runat="server">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnlPromociones" runat="server">
                                                        <div style="position: absolute; width: 100%; height: 50%; background-color: #b62322; left: 0px;"></div>
                                                        <div class="row">
                                                            <div style="width: 100%;">
                                                                <div style="width: 80%; margin: 0 auto; display: block;">
                                                                    <div class="card">
                                                                        <div class="card-body">
                                                                            <asp:UpdatePanel runat="server">
                                                                                <ContentTemplate>
                                                                                    <asp:Panel ID="pnlAlertPago" Visible="false" runat="server">
                                                                                        <div id="divAlertPago" class="alert alert-danger alert-dismissible fade" role="alert" runat="server">
                                                                                            <asp:Label ID="lblMensajeAlertPago" runat="server" />
                                                                                            <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                                        </div>
                                                                                    </asp:Panel>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>


                                                                            <div class="row" style="padding-top: 10px;">
                                                                                <div class="col-12 col-md-12 col-lg-6">
                                                                                    <asp:Image ID="imgLogoSelect2" Height="100" Width="150" class="img-fluid" alt="logoEscuela" runat="server" />
                                                                                </div>
                                                                                <div class="col-12 col-md-12 col-lg-6">
                                                                                    <asp:Image Height="80" Width="250" class="img-fluid pull-right" ImageUrl="../Images/logoCompetoPagaLaEscuela.png" runat="server" />
                                                                                </div>
                                                                            </div>

                                                                            <div class="row" style="padding-top: 10px;">
                                                                                <div class="col-12 col-md-12 col-lg-8">
                                                                                    <div class="form-group col-md-12">
                                                                                        <div class="input-group">
                                                                                            <div class="input-group-prepend">
                                                                                                <asp:Label Text="Alumno:&nbsp;" Font-Bold="true" runat="server" />
                                                                                            </div>
                                                                                            <asp:Label ID="headAlumno2" runat="server" />
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-group col-md-12">
                                                                                        <div class="input-group">
                                                                                            <div class="input-group-prepend">
                                                                                                <asp:Label Text="Matricula:&nbsp;" Font-Bold="true" runat="server" />
                                                                                            </div>
                                                                                            <asp:Label ID="headMatricula2" runat="server" />
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-12 col-md-12 col-lg-4">
                                                                                    <div class="form-group col-md-12">
                                                                                        <label for="ddlFormasPago2" style="color: #ff9800;">Promoción de pago</label>
                                                                                        <div class="input-group">
                                                                                            <div class="input-group-prepend">
                                                                                                <span class="input-group-text" style="padding-left: 0px;">
                                                                                                    <i class="material-icons">format_list_numbered</i>
                                                                                                </span>
                                                                                            </div>
                                                                                            <asp:DropDownList ID="ddlFormasPago2" AppendDataBoundItems="true" AutoPostBack="true" CssClass="form-control" runat="server">
                                                                                            </asp:DropDownList>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="form-group col-md-12">
                                                                                    <div class="input-group">
                                                                                        <div class="input-group-prepend">
                                                                                            <asp:Label Text="Fecha de pago:&nbsp;" Font-Bold="true" runat="server" />
                                                                                        </div>
                                                                                        <asp:Label ID="headFPago2" runat="server" />
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div class="row">
                                                                                <div style="display: none;" class="form-group col-md-3">
                                                                                    <label for="lblVencimiento2" style="color: black;">Fecha de Pago</label>
                                                                                    <div class="input-group">
                                                                                        <div class="input-group-prepend">
                                                                                            <span class="input-group-text" style="padding-left: 0px;">
                                                                                                <i class="material-icons">date_range</i>
                                                                                            </span>
                                                                                        </div>
                                                                                        <asp:Label ID="lblVencimiento2" Text="12/09/2020" CssClass="form-control" runat="server" />
                                                                                    </div>
                                                                                </div>
                                                                                <div style="display: none;" class="form-group col-md-9">
                                                                                    <label for="lblConcepto2" style="color: black;">Concepto</label>
                                                                                    <div class="input-group">
                                                                                        <div class="input-group-prepend">
                                                                                            <span class="input-group-text" style="padding-left: 0px;">
                                                                                                <i class="material-icons">assignment</i>
                                                                                            </span>
                                                                                        </div>
                                                                                        <asp:Label ID="lblConcepto2" Text="Concepto" CssClass="form-control" runat="server" />
                                                                                    </div>
                                                                                </div>
                                                                                <div style="display: none;" class="form-group col-md-4">
                                                                                    <label for="lblImporteCole2" style="color: black;">Importe Colegiatura</label>
                                                                                    <div class="input-group">
                                                                                        <div class="input-group-prepend">
                                                                                            <span class="input-group-text" style="padding-left: 0px;">
                                                                                                <i class="material-icons">attach_money</i>
                                                                                            </span>
                                                                                        </div>
                                                                                        <asp:Label ID="lblImporteCole2" Text="0.00" CssClass="form-control" runat="server" />
                                                                                    </div>
                                                                                </div>
                                                                                <div style="display: none;" class="form-group col-md-4">
                                                                                    <label for="lblImporteBeca2" style="color: black;">Importe Beca</label>
                                                                                    <div class="input-group" style="padding-top: 7px;">
                                                                                        <div class="input-group-prepend">
                                                                                            <span class="input-group-text" style="padding-left: 0px;">
                                                                                                <i class="material-icons">attach_money</i>
                                                                                            </span>
                                                                                        </div>
                                                                                        <asp:Label ID="lblImporteBeca2" CssClass="form-control" Text="0.00" runat="server" />
                                                                                    </div>
                                                                                </div>
                                                                                <div style="display: none;" class="form-group col-md-4">
                                                                                    <label for="lblRecargo2" style="color: black;">Importe Recargo</label>
                                                                                    <div class="input-group">
                                                                                        <div class="input-group-prepend">
                                                                                            <span class="input-group-text" style="padding-left: 0px;">
                                                                                                <i class="material-icons">attach_money</i>
                                                                                            </span>
                                                                                        </div>
                                                                                        <asp:Label ID="lblRecargo2" Text="0.00" CssClass="form-control" runat="server" />
                                                                                    </div>
                                                                                </div>
                                                                                <div style="display: none;" class="form-group col-md-4" style="display: none;">
                                                                                    <label for="lblTieneBeca2" style="color: black;">¿Tiene beca?</label>
                                                                                    <asp:Label ID="lblTieneBeca2" CssClass="form-control" Text="NO" runat="server" />
                                                                                </div>
                                                                                <div style="display: none;" class="form-group col-md-4" style="display: none;">
                                                                                    <label for="lblTipoBeca2" style="color: black;">Tipo beca</label>
                                                                                    <asp:Label ID="lblTipoBeca2" CssClass="form-control" Text="CANTIDAD" runat="server" />
                                                                                </div>
                                                                                <div style="display: none;" class="form-group col-md-4">
                                                                                    <label for="lblComisionTarjeta2" style="color: black;">Comisión pago en linea</label>
                                                                                    <div class="input-group" style="padding-top: 7px;">
                                                                                        <div class="input-group-prepend">
                                                                                            <span class="input-group-text" style="padding-left: 0px;">
                                                                                                <i class="material-icons">attach_money</i>
                                                                                            </span>
                                                                                        </div>
                                                                                        <asp:Label ID="lblComisionTarjeta2" Text="0.00" CssClass="form-control" runat="server" />
                                                                                    </div>
                                                                                </div>
                                                                                <div style="display: none;" class="form-group col-md-4">
                                                                                    <label for="lblImporteTotal2" style="color: black;">Importe Total</label>
                                                                                    <div class="input-group" style="padding-top: 7px;">
                                                                                        <div class="input-group-prepend">
                                                                                            <span class="input-group-text" style="padding-left: 0px;">
                                                                                                <i class="material-icons">attach_money</i>
                                                                                            </span>
                                                                                        </div>
                                                                                        <asp:Label ID="lblImporteTotal2" Text="150.00" CssClass="form-control" runat="server" />
                                                                                    </div>
                                                                                </div>

                                                                                <div style="display: none;" class="form-group col-md-4">
                                                                                    <label for="lblComisionPromocion2" style="color: black;">Comisión promoción</label>
                                                                                    <div class="input-group" style="padding-top: 7px;">
                                                                                        <div class="input-group-prepend">
                                                                                            <span class="input-group-text" style="padding-left: 0px;">
                                                                                                <i class="material-icons">attach_money</i>
                                                                                            </span>
                                                                                        </div>
                                                                                        <asp:Label ID="lblComisionPromocion2" Text="150.00" CssClass="form-control" runat="server" />
                                                                                    </div>
                                                                                </div>
                                                                                <div style="display: none;" class="form-group col-md-4">
                                                                                    <label for="lblTotalPagar2" style="color: black;">Total a pagar</label>
                                                                                    <div class="input-group" style="padding-top: 7px;">
                                                                                        <div class="input-group-prepend">
                                                                                            <span class="input-group-text" style="padding-left: 0px;">
                                                                                                <i class="material-icons">attach_money</i>
                                                                                            </span>
                                                                                        </div>
                                                                                        <asp:Label ID="lblTotalPagar2" Text="150.00" CssClass="form-control" runat="server" />
                                                                                    </div>
                                                                                </div>

                                                                                <table class="table" style="margin-top: 16px;">
                                                                                    <thead>
                                                                                        <tr>
                                                                                            <th style="background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px;" class="text-center">N°</th>
                                                                                            <th style="background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px;">CONCEPTO</th>
                                                                                            <th style="background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px;" class="text-right">PRECIO</th>
                                                                                        </tr>
                                                                                    </thead>
                                                                                    <tbody>
                                                                                        <asp:Repeater ID="rptDesglose2" runat="server">
                                                                                            <ItemTemplate>
                                                                                                <tr>
                                                                                                    <td class="text-center"><%#Eval("IntNum")%></td>
                                                                                                    <td><%#Eval("VchConcepto")%></td>
                                                                                                    <td class="text-right" style="color: <%#Eval("VchCoResta")%>;">$<%#Eval("DcmImporte")%></td>
                                                                                                </tr>
                                                                                            </ItemTemplate>
                                                                                        </asp:Repeater>
                                                                                        <tr id="trSubtotal2" runat="server">
                                                                                            <td style="padding-bottom: 0px;" class="text-center"></td>
                                                                                            <td style="font-weight: bold; padding-top: 0px; padding-bottom: 0px;" class="text-right">Subtotal:</td>
                                                                                            <td style="font-weight: bold; padding-top: 0px; padding-bottom: 0px;" class="text-right">
                                                                                                <asp:Label ID="lblSubtotaltb2" runat="server" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trComisionTarjeta2" runat="server">
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" class="text-center"></td>
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" class="text-right">
                                                                                                <asp:Label ID="lblComisionTarjetatb2" runat="server" />
                                                                                            </td>
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" class="text-right">
                                                                                                <asp:Label ID="lblImpComisionTrajetatb2" runat="server" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trPromociones2" runat="server">
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" class="text-center"></td>
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" class="text-right">
                                                                                                <div class="tooltipse bottom">
                                                                                                    <i class="material-icons">info</i><span class="tiptext"><asp:Label ID="lblToolPromo2" Text="Promociones" runat="server" /></span>
                                                                                                </div>
                                                                                                <asp:Label ID="lblPromotb2" runat="server" />
                                                                                            </td>
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" class="text-right">
                                                                                                <asp:Label ID="lblImpPromotb2" runat="server" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" class="text-center"></td>
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" class="text-right">Total:</td>
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" class="text-right">
                                                                                                <asp:Label ID="lblTotaltb2" runat="server" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" class="text-center"></td>
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" class="text-right">
                                                                                                <div class="tooltipse bottom">
                                                                                                    <i class="material-icons">info</i>
                                                                                                    <span class="tiptext" style="width: 230px;">
                                                                                                        <asp:Label ID="lblToolApagar2" Text="A pagar" runat="server" /></span>
                                                                                                </div>
                                                                                                Importe a pagar:
                                                                                            </td>
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" class="text-right">
                                                                                                <div style="float: right;">
                                                                                                    <div class="input-group" style="padding-top: 7px;">
                                                                                                        <div class="input-group-prepend">
                                                                                                            <span class="input-group-text" style="padding-left: 0px; padding-right: 0px;">
                                                                                                                <i class="material-icons">attach_money</i>
                                                                                                            </span>
                                                                                                        </div>
                                                                                                        <asp:TextBox ID="txtTotaltb2" Width="80" CssClass="form-control text-right" ReadOnly="true" Font-Bold="true" Font-Size="Medium" runat="server" />
                                                                                                        <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtTotaltb2" runat="server" />
                                                                                                    </div>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" class="text-center"></td>
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" class="text-right"></td>
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" class="text-right">
                                                                                                <asp:Label ID="lblRestaTotal2" runat="server" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </div>
                                                                            <div class="pull-right" style="padding-top: 10px;">
                                                                                <asp:LinkButton ID="btnGenerarPago2" OnClick="btnGenerarPago_Click" ToolTip="Generar pago" runat="server">
                                                                                    <asp:Label class="btn btn-success btn-round" runat="server">
                                                                                        <asp:Label ID="lblTotalPago2" Text="Generar pago $0.00" runat="server" /><i class="material-icons">arrow_forward</i>
                                                                                    </asp:Label>
                                                                                </asp:LinkButton>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <div>
                                                <asp:Panel ID="pnlEfecTarjeta" runat="server"></asp:Panel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <div class="row justify-content-between">
                                    <div class="col-6">
                                        <asp:LinkButton ID="btnAnterior" Visible="false" OnClick="btnAnterior_Click" CssClass="btn btn-warning btn-round pull-left" runat="server">
                                            <i class="material-icons">arrow_back</i> Anterior
                                        </asp:LinkButton>
                                    </div>
                                    <div class="col-6">
                                        <asp:LinkButton ID="btnSiguiente" OnClick="btnSiguiente_Click" CssClass="btn btn-success btn-round pull-right" runat="server">
                                            Siguiente <i class="material-icons">arrow_forward</i> 
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="ModalDialog" class="modal fade" tabindex="-1" role="dialog" data-backdrop="static" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header" style="padding-bottom: 0px; padding-top: 3px;">

                            <div class="row">
                                <div style="width: 100%;">
                                    <img src="../Images/MnsjDialog.jpeg" style="width: 100%; margin: 0 auto; display: block;" height="100" width="100" class="img-fluid align-items-center" alt="Responsive image">
                                </div>
                            </div>
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
                                <div class="text-center">
                                    <h5 class="modal-title" runat="server">
                                        <asp:Label ID="lblTitleDialog" runat="server" />
                                    </h5>
                                    <asp:Label ID="lblMnsjDialog" runat="server" />
                                </div>
                                <br />
                                <h4 style="font-weight: bold;" class="text-center">¿Esta seguro que desea continuar?</h4>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-footer justify-content-center">
                            <asp:LinkButton ID="btnSi" OnClick="btnSi_Click" CssClass="btn btn-success btn-round" runat="server">
                            <i class="material-icons">check</i> SI
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnNo" OnClientClick="hideModalDialog();" CssClass="btn btn-danger btn-round" runat="server">
                            <i class="material-icons">close</i> NO
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
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

        function showModalPagos() {
            $('#ModalPagos').modal('show');
        }
        function hideModalPagos() {
            $('#ModalPagos').modal('hide');
        }

        function showModalTipoPago() {
            let elm = document.getElementById('aFormaPago');
            elm.className = 'nav-link';

            let elm2 = document.getElementById('aPago');
            elm2.className = 'nav-link disabled';

            $('#ulTabAgregarPago a[href="#formapago"]').tab('show')

            $('#ModalTipoPago').modal('show');
        }
        function hideModalTipoPago() {
            $('#ModalTipoPago').modal('hide');
            $('#ModalDialog').modal('hide');
        }

        function showModalDialog() {
            let elm = document.getElementById('ModalTipoPago');
            elm.style = 'overflow-y: scroll; padding-right: 17px; display: block;filter: blur(4px);';

            $('#ModalDialog').modal('show');
        }
        function hideModalDialog() {
            $('#ModalDialog').modal('hide');
            let elm = document.getElementById('ModalTipoPago');
            elm.style = 'overflow-y: scroll; padding-right: 17px; display: block;';
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
    <script>
        function showModalPagoDetalleManual() {
            $('#ModalPagoDetalleManual').modal('show');
        }
        function hideModalPagoDetalleManual() {
            $('#ModalPagoDetalleManual').modal('hide');
        }
    </script>

    <script>
        function showTabFormaPago() {
            let elm = document.getElementById('aFormaPago');
            elm.className = 'nav-link';

            let elm2 = document.getElementById('aPago');
            elm2.className = 'nav-link disabled';

            $('#ulTabAgregarPago a[href="#formapago"]').tab('show')
        }
        function showTabPago() {
            let elm = document.getElementById('aFormaPago');
            elm.className = 'nav-link disabled';

            let elm2 = document.getElementById('aPago');
            elm2.className = 'nav-link';

            $('#ulTabAgregarPago a[href="#pago"]').tab('show')
        }
    </script>
</asp:Content>
