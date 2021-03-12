<%@ Page Title="ReporteLigasEscuelas" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReporteLigasEscuelas.aspx.cs" Inherits="PagaLaEscuela.Views.ReporteLigasEscuelas" %>

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
                                <div class="card-header card-header-tabs card-header-primary" style="background: #326497; padding-top: 0px; padding-bottom: 0px;">
                                    <div class="nav-tabs-navigation">
                                        <div class="nav-tabs-wrapper">
                                            <div class="form-group" style="margin-top: 0px; padding-bottom: 0px;">
                                                <asp:LinkButton ID="btnFiltros" OnClick="btnFiltros_Click" ToolTip="Filtros de busqueda." BackColor="#4db6ac" class="btn btn-lg btn-fab btn-fab-mini btn-round" runat="server">
                                                        <i class="material-icons">search</i>
                                                </asp:LinkButton>
                                                <asp:Label Text="Listado de pagos" runat="server" />

                                                <asp:LinkButton ID="btnPago" OnClick="btnPago_Click" ToolTip="Realizar pago" class="btn btn-lg btn-success btn-fab btn-fab-mini btn-round pull-right" runat="server">
                                                        <i class="material-icons">add</i>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnActualizarLista" OnClick="btnActualizarLista_Click" ToolTip="Actualizar tabla." class="btn btn-lg btn-info btn-fab btn-fab-mini btn-round pull-right" runat="server">
                                                        <i class="material-icons">sync</i>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnExportarLista" OnClick="btnExportarLista_Click" ToolTip="Exportar lista a excel." class="btn btn-lg btn-warning btn-fab btn-fab-mini btn-round pull-right" runat="server">
                                                        <i class="material-icons">file_download</i>
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-6" style="padding-right: 2px;">
                                            <asp:LinkButton ID="btnDatosAlumnos" OnClick="btnDatosAlumnos_Click" CssClass="btn btn-primary pull-right" runat="server">
                                            <i class="material-icons">wc</i> Alumnos
                                            </asp:LinkButton>
                                        </div>
                                        <div class="col-6" style="padding-left: 2px;">
                                            <asp:LinkButton ID="btnDatosPagos" OnClick="btnDatosPagos_Click" CssClass="btn btn-secondary pull-left" runat="server">
                                            <i class="material-icons">attach_money</i> PAGOS
                                            </asp:LinkButton>
                                        </div>
                                        <div class="table-responsive">
                                            <asp:Panel ID="pnlDatosAlumnos" runat="server">
                                                <asp:GridView ID="gvDatosAlumnos" OnPageIndexChanging="gvDatosAlumnos_PageIndexChanging" OnSorting="gvDatosAlumnos_Sorting" OnRowCommand="gvDatosAlumnos_RowCommand" OnRowDataBound="gvDatosAlumnos_RowDataBound" DataKeyNames="UidPagoColegiatura" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" GridLines="None" border="0" AllowPaging="true" PageSize="10" ShowFooter="true" runat="server">
                                                    <EmptyDataTemplate>
                                                        <div class="alert alert-info">No hay pagos realizados</div>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:BoundField SortExpression="IntFolio" DataField="IntFolio" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="FOLIO" />
                                                        <asp:TemplateField SortExpression="VchIdentificador" HeaderText="COLEGIATURA">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtGvIdentificador" ToolTip='<%#Eval("VchIdentificador")%>' Style="width: 100%; text-overflow: ellipsis;" Text='<%#Eval("VchIdentificador")%>' Enabled="false" BackColor="Transparent" BorderStyle="None" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="VchAlumno" HeaderStyle-CssClass="text-center" HeaderText="ALUMNO">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtGvAlumno" ToolTip='<%#Eval("VchAlumno")%>' Style="width: 100%; text-overflow: ellipsis;" Text='<%#Eval("VchAlumno")%>' Enabled="false" BackColor="Transparent" BorderStyle="None" runat="server" />
                                                                <asp:Label ID="lblGvUidAlumno" Visible="false" Text='<%#Eval("UidAlumno")%>' runat="server" />
                                                                <asp:Label ID="lblGvUidFechaColegiatura" Visible="false" Text='<%#Eval("UidFechaColegiatura")%>' runat="server" />
                                                                <asp:Label ID="lblGvUidUsuario" Visible="false" Text='<%#Eval("UidUsuario")%>' runat="server" />
                                                                <asp:Label ID="lblGvUidFormaPago" Text='<%#Eval("UidFormaPago")%>' Visible="false" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField SortExpression="VchNum" DataField="VchNum" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="# DE PAGOS" />
                                                        <asp:BoundField SortExpression="DtFHPago" DataField="DtFHPago" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" DataFormatString="{0:dd/MM/yyyy}" HeaderText="FECHA PAGO" />
                                                        <asp:BoundField SortExpression="DcmImporteCole" DataField="DcmImporteCole" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" DataFormatString="{0:C}" HeaderText="IMPORTE COLEGIATURA" />
                                                        <asp:BoundField SortExpression="DcmImporteSaldado" DataField="DcmImporteSaldado" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" DataFormatString="{0:C}" HeaderText="IMPORTE SALDADO" />
                                                        <asp:BoundField SortExpression="DcmImportePagado" DataField="DcmImportePagado" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" DataFormatString="{0:C}" HeaderText="IMPORTE PAGADO" />
                                                        <asp:BoundField SortExpression="DcmImporteNuevo" DataField="DcmImporteNuevo" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" DataFormatString="{0:C}" HeaderText="RESTA" />
                                                        <asp:BoundField SortExpression="VchFormaPago" DataField="VchFormaPago" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="FORMA PAGO" />
                                                        <asp:TemplateField SortExpression="VchEstatus" HeaderText="ESTATUS">
                                                            <ItemTemplate>
                                                                <asp:Label Text='<%#Eval("VchEstatus")%>' ForeColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("VchColor").ToString()) %>' Font-Names="Comic Sans MS" Font-Bold="true" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-Width="160">
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
                                                                            <asp:Panel Visible='<%#Eval("blConfirmarPago")%>' runat="server">
                                                                                <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                                    <asp:LinkButton ID="btnConfirmarPago" ToolTip="Confirmar pago" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="btnConfirmarPago" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label class="btn btn-sm btn-success btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">attach_money</i>
                                                                                </asp:Label>
                                                                                    </asp:LinkButton>
                                                                                </td>
                                                                            </asp:Panel>
                                                                            <asp:Panel ID="pnlEditarPago" Visible='<%#Eval("blEditarPago")%>' runat="server">
                                                                                <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                                    <asp:LinkButton ID="btnEditarPago" ToolTip="Editar pago" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="btnEditarPago" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label class="btn btn-sm btn-warning btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">edit</i>
                                                                                </asp:Label>
                                                                                    </asp:LinkButton>
                                                                                </td>
                                                                            </asp:Panel>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblPaginado" Font-Bold="true" runat="server" />
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                                </asp:GridView>
                                            </asp:Panel>

                                            <asp:Panel ID="pnlDatosPagos" Visible="false" runat="server">
                                                <asp:GridView ID="gvDatosPagos" OnPageIndexChanging="gvDatosPagos_PageIndexChanging" OnSorting="gvDatosPagos_Sorting" OnRowCommand="gvDatosPagos_RowCommand" OnRowDataBound="gvDatosPagos_RowDataBound" DataKeyNames="UidPagoColegiatura" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" GridLines="None" border="0" AllowPaging="true" PageSize="10" ShowFooter="true" runat="server">
                                                    <EmptyDataTemplate>
                                                        <div class="alert alert-info">No hay pagos realizados</div>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:BoundField SortExpression="DtFHPago" DataField="DtFHPago" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" DataFormatString="{0:dd/MM/yyyy}" HeaderText="FECHA PAGO" />
                                                        <asp:BoundField SortExpression="IntFolio" DataField="IntFolio" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="FOLIO" />
                                                        <asp:BoundField SortExpression="VchFolio" DataField="VchFolio" HeaderText="FOLIO" />
                                                        <asp:BoundField SortExpression="DcmImportePagado" DataField="DcmImportePagado" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" DataFormatString="{0:C}" HeaderText="IMPORTE PAGADO" />
                                                        <asp:BoundField SortExpression="VchFormaPago" DataField="VchFormaPago" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="FORMA PAGO" />
                                                        <asp:BoundField SortExpression="VchBanco" DataField="VchBanco" HeaderText="BANCO" />
                                                        <asp:BoundField SortExpression="VchCuenta" DataField="VchCuenta" HeaderText="CUENTA" />
                                                        <asp:TemplateField SortExpression="VchEstatus" HeaderText="ESTATUS">
                                                            <ItemTemplate>
                                                                <asp:Label Text='<%#Eval("VchEstatus")%>' ForeColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("VchColor").ToString()) %>' Font-Names="Comic Sans MS" Font-Bold="true" runat="server"></asp:Label>
                                                                <asp:Label ID="lblGvUidAlumno" Visible="false" Text='<%#Eval("UidAlumno")%>' runat="server" />
                                                                <asp:Label ID="lblGvUidFechaColegiatura" Visible="false" Text='<%#Eval("UidFechaColegiatura")%>' runat="server" />
                                                                <asp:Label ID="lblGvUidUsuario" Visible="false" Text='<%#Eval("UidUsuario")%>' runat="server" />
                                                                <asp:Label ID="lblGvUidFormaPago" Text='<%#Eval("UidFormaPago")%>' Visible="false" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-Width="160">
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
                                                                            <asp:Panel ID="pnlConfirmarPago" Visible='<%#Eval("blAprobarPago")%>' runat="server">
                                                                                <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                                    <asp:LinkButton ID="btnConfirmarPago" ToolTip="Confirmar pago" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="btnConfirmarPago" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label class="btn btn-sm btn-success btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">check</i>
                                                                                </asp:Label>
                                                                                    </asp:LinkButton>
                                                                                </td>
                                                                            </asp:Panel>
                                                                            <asp:Panel ID="pnlRechazarPago" Visible='<%#Eval("blRechazarPago")%>' runat="server">
                                                                                <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                                    <asp:LinkButton ID="btnRechazarPago" ToolTip="Rechazar pago" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="btnRechazarPago" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label class="btn btn-sm btn-danger btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">close</i>
                                                                                </asp:Label>
                                                                                    </asp:LinkButton>
                                                                                </td>
                                                                            </asp:Panel>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblPaginado" Font-Bold="true" runat="server" />
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                                </asp:GridView>
                                            </asp:Panel>
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
    <%--MODAL--%>
    <div id="ModalTipoPago" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="myLargeModalLabel" aria-hidden="true" style="overflow-y: scroll;">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header" style="padding-top: 5px; padding-bottom: 5px;">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="lblTitleModalTipoPago" Text="Seleccione un alumno" runat="server" /></h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="modal-body pt-0" style="padding-bottom: 0px;">
                    <div class="tab-content">
                        <div class="row">
                            <div class="card card-nav-tabs" style="margin-bottom: 0px;">
                                <div class="card-header card-header-primary" style="background: #326497;">
                                    <div class="nav-tabs-navigation">
                                        <div class="nav-tabs-wrapper">
                                            <ul class="nav nav-tabs" id="ulTabAgregarPago" data-tabs="tabs">
                                                <li class="nav-item">
                                                    <a id="aAlumno" class="nav-link active show" href="#alumno" data-toggle="tab">
                                                        <i class="material-icons">wc</i>Paso 1<div class="ripple-container"></div>
                                                    </a>
                                                </li>
                                                <li class="nav-item">
                                                    <a id="aColegiatura" class="nav-link disabled" href="#colegiatura" data-toggle="tab">
                                                        <i class="material-icons">account_balance</i>Paso 2<div class="ripple-container"></div>
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

                                <div class="card-body" style="padding-top: 0px;">
                                    <div class="tab-content">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <div class="row">
                                                    <div class="col-sm-12" style="padding-right: 10px; padding-left: 10px;">
                                                        <div class="card" style="background-color: #ededed; margin-top: 0px; margin-bottom: 20px;">
                                                            <div class="card-body" style="padding-bottom: 5px; padding-top: 5px;">
                                                                <div class="input-group">
                                                                    <asp:Label ID="lblHeadAlum" Text="Alumno:" Font-Bold="true" runat="server" />
                                                                    <div class="input-group-prepend">
                                                                        <span class="input-group-text" style="padding-left: 0px; padding-right: 0px;">
                                                                            <i class="material-icons">chevron_right</i>
                                                                        </span>
                                                                    </div>
                                                                    <asp:Label ID="lblHeadCole" Font-Bold="true" runat="server" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <asp:Label CssClass="text-danger" runat="server" ID="lblValidar" />
                                                <asp:Panel ID="pnlAlertModalTipoPago" Visible="false" runat="server">
                                                    <div id="divAlertModalTipoPago" class="alert alert-danger alert-dismissible fade" role="alert" runat="server">
                                                        <asp:Label runat="server" ID="lblMnsjModalTipoPago" />
                                                        <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                    </div>
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                        <div class="tab-pane active show" id="alumno">
                                            <asp:UpdatePanel runat="server">
                                                <ContentTemplate>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="card" style="margin-top: 0px;">
                                                                <div class="card-header">
                                                                    <div class="pull-right">
                                                                        <asp:LinkButton ID="btnFiltroLimpiar" OnClick="btnFiltroLimpiar_Click" BorderWidth="1" CssClass="btn btn-warning btn-sm" runat="server">
                                                                                <i class="material-icons">delete</i> Limpiar
                                                                        </asp:LinkButton>
                                                                        <asp:LinkButton ID="btnFiltroBuscar" OnClick="btnFiltroBuscar_Click" BorderWidth="1" CssClass="btn btn-info btn-sm" runat="server">
                                                                                <i class="material-icons">search</i> Buscar
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                </div>
                                                                <div class="card-body" style="padding-top: 0px; padding-bottom: 0px; padding-right: 10px; padding-left: 10px;">
                                                                    <div class="form-group" style="margin-top: 0px;">
                                                                        <div class="row">
                                                                            <div class="form-group col-sm-6 col-md-6 col-lg-3">
                                                                                <label for="txtFiltroAlumIdentificador" style="color: black; padding-left: 0px;">Identificador</label>
                                                                                <asp:TextBox ID="txtFiltroAlumIdentificador" CssClass="form-control" aria-label="Search" runat="server" />
                                                                            </div>
                                                                            <div class="form-group col-sm-6 col-md-6 col-lg-3">
                                                                                <label for="txtFiltroAlumMatricula" style="color: black; padding-left: 0px;">Matricula</label>
                                                                                <asp:TextBox ID="txtFiltroAlumMatricula" CssClass="form-control" aria-label="Search" runat="server" />
                                                                            </div>
                                                                            <div class="form-group col-sm-6 col-md-6 col-lg-2">
                                                                                <label for="txtFiltroAlumNombre" style="color: black; padding-left: 0px;">Nombre(s)</label>
                                                                                <asp:TextBox ID="txtFiltroAlumNombre" CssClass="form-control" aria-label="Search" runat="server" />
                                                                            </div>
                                                                            <div class="form-group col-sm-6 col-md-6 col-lg-2">
                                                                                <label for="txtFiltroAlumPaterno" style="color: black; padding-left: 0px;">ApePaterno</label>
                                                                                <asp:TextBox ID="txtFiltroAlumPaterno" CssClass="form-control" aria-label="Search" runat="server" />
                                                                            </div>
                                                                            <div class="form-group col-sm-6 col-md-6 col-lg-2">
                                                                                <label for="txtFiltroAlumMaterno" style="color: black; padding-left: 0px;">ApeMaterno</label>
                                                                                <asp:TextBox ID="txtFiltroAlumMaterno" CssClass="form-control" aria-label="Search" runat="server" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="table-responsive">
                                                            <asp:GridView ID="gvAlumnos" OnSelectedIndexChanged="gvAlumnos_SelectedIndexChanged" OnRowDataBound="gvAlumnos_RowDataBound" OnPageIndexChanging="gvAlumnos_PageIndexChanging" OnSorting="gvAlumnos_Sorting" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidAlumno" GridLines="None" border="0" EnablePersistedSelection="true" AllowPaging="true" PageSize="5" runat="server">
                                                                <EmptyDataTemplate>
                                                                    <div class="alert alert-info">No se encontro alumnos</div>
                                                                </EmptyDataTemplate>
                                                                <Columns>
                                                                    <asp:ButtonField CommandName="Select" HeaderStyle-CssClass="d-none" ItemStyle-CssClass="d-none" />
                                                                    <asp:BoundField SortExpression="VchIdentificador" DataField="VchIdentificador" HeaderText="IDENTIFICADOR" />
                                                                    <asp:BoundField SortExpression="VchMatricula" DataField="VchMatricula" HeaderText="MATRICULA" />
                                                                    <asp:BoundField SortExpression="NombreCompleto" DataField="NombreCompleto" HeaderText="NOMBRE" />
                                                                </Columns>
                                                                <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                                                <SelectedRowStyle BackColor="#dff0d8" />
                                                            </asp:GridView>
                                                        </div>

                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="tab-pane" id="colegiatura">
                                            <asp:UpdatePanel runat="server">
                                                <ContentTemplate>
                                                    <div class="table-responsive">
                                                        <asp:GridView ID="gvPagos" OnSelectedIndexChanged="gvPagos_SelectedIndexChanged" OnPageIndexChanging="gvPagos_PageIndexChanging" OnRowDataBound="gvPagos_RowDataBound" OnSorting="gvPagos_Sorting" OnRowCommand="gvPagos_RowCommand" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidFechaColegiatura" GridLines="None" border="0" EnablePersistedSelection="true" AllowPaging="true" PageSize="5" runat="server">
                                                            <EmptyDataTemplate>
                                                                <div class="alert alert-info"><strong>Felicidades</strong> no tiene pagos disponibles.</div>
                                                            </EmptyDataTemplate>
                                                            <Columns>
                                                                <asp:ButtonField CommandName="Select" HeaderStyle-CssClass="d-none" ItemStyle-CssClass="d-none" />
                                                                <asp:TemplateField SortExpression="VchIdentificador" HeaderText="COLEGIATURA">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtGvIdentificador" ToolTip='<%#Eval("VchIdentificador")%>' Style="width: 100%; text-overflow: ellipsis;" Text='<%#Eval("VchIdentificador")%>' Enabled="false" BackColor="Transparent" BorderStyle="None" runat="server" />
                                                                        <asp:TextBox ID="txtGvUidCliente" Text='<%#Eval("UidCliente")%>' Visible="false" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField SortExpression="VchNum" DataField="VchNum" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="# DE PAGOS" />
                                                                <asp:BoundField SortExpression="DcmImporte" DataField="DcmImporte" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" DataFormatString="{0:C}" HeaderText="IMPORTE" />
                                                                <asp:BoundField SortExpression="ImpPagado" DataField="ImpPagado" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" DataFormatString="{0:C}" HeaderText="ABONADO" />
                                                                <asp:BoundField SortExpression="ImpTotal" DataField="ImpTotal" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" DataFormatString="{0:C}" HeaderText="SALDO" />
                                                                <asp:BoundField SortExpression="VchFHLimite" DataField="VchFHLimite" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="LIMITE" />
                                                                <asp:BoundField SortExpression="VchFHVencimiento" DataField="VchFHVencimiento" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="VENCIMIENTO" />
                                                            </Columns>
                                                            <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                                            <SelectedRowStyle BackColor="#dff0d8" />
                                                        </asp:GridView>
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
                                                                                    <asp:Image ID="imgLogo" Height="100" Width="150" class="img-fluid" alt="logoEscuela" runat="server" />
                                                                                </div>
                                                                                <div class="col-12 col-md-12 col-lg-6">
                                                                                    <asp:Image Height="80" Width="250" class="img-fluid pull-right" ImageUrl="../Images/logoCompetoPagaLaEscuela.png" runat="server" />
                                                                                </div>
                                                                            </div>
                                                                            <div class="row" style="padding-top: 10px;">
                                                                                <div class="col-12 col-md-12 col-lg-7" style="padding-left: 0px;">
                                                                                    <div class="form-group col-md-12" style="padding-bottom: 0px;">
                                                                                        <div class="input-group">
                                                                                            <div class="input-group-prepend">
                                                                                                <asp:Label Text="Alumno:&nbsp;" Font-Bold="true" runat="server" />
                                                                                            </div>
                                                                                            <asp:Label ID="headAlumno" runat="server" />
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-group col-md-12" style="padding-bottom: 0px;">
                                                                                        <div class="input-group">
                                                                                            <div class="input-group-prepend">
                                                                                                <asp:Label Text="Matricula:&nbsp;" Font-Bold="true" runat="server" />
                                                                                            </div>
                                                                                            <asp:Label ID="headMatricula" runat="server" />
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-12 col-md-12 col-lg-5" style="padding-left: 0px;">
                                                                                    <div class="form-group col-md-12">
                                                                                        <div class="input-group">
                                                                                            <div class="input-group-prepend">
                                                                                                <asp:Label Text="Fecha de pago:&nbsp;" Font-Bold="true" runat="server" />
                                                                                            </div>
                                                                                            <asp:Label ID="headFPago" runat="server" />
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="form-group col-md-12" style="padding-bottom: 0px;">
                                                                                    <label for="ddlTutorAlu" style="color: #ff9800;">Tutor del alumno *</label>
                                                                                    <div class="input-group">
                                                                                        <div class="input-group-prepend">
                                                                                            <span class="input-group-text" style="padding-left: 0px;">
                                                                                                <i class="material-icons">person</i>
                                                                                            </span>
                                                                                        </div>
                                                                                        <asp:DropDownList ID="ddlTutorAlu" AppendDataBoundItems="true" CssClass="form-control" runat="server">
                                                                                        </asp:DropDownList>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-sm-12">
                                                                                    <div class="row">
                                                                                        <div class="form-group col-md-4">
                                                                                            <label for="ddlFormasPago" style="color: #ff9800;">Forma de pago</label>
                                                                                            <div class="input-group">
                                                                                                <div class="input-group-prepend">
                                                                                                    <span class="input-group-text" style="padding-left: 0px;">
                                                                                                        <i class="material-icons">format_list_numbered</i>
                                                                                                    </span>
                                                                                                </div>
                                                                                                <asp:DropDownList ID="ddlFormasPago" OnSelectedIndexChanged="ddlFormasPago_SelectedIndexChanged" AppendDataBoundItems="true" AutoPostBack="true" CssClass="form-control" runat="server">
                                                                                                </asp:DropDownList>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div id="divTiposTarjetas" class="form-group col-md-4" runat="server">
                                                                                            <label for="ddlTiposTarjetas" style="color: #ff9800;">Tipo de tarjeta</label>
                                                                                            <div class="input-group">
                                                                                                <div class="input-group-prepend">
                                                                                                    <span class="input-group-text" style="padding-left: 0px;">
                                                                                                        <i class="material-icons">format_list_numbered</i>
                                                                                                    </span>
                                                                                                </div>
                                                                                                <asp:DropDownList ID="ddlTiposTarjetas" OnSelectedIndexChanged="ddlTiposTarjetas_SelectedIndexChanged" AppendDataBoundItems="true" AutoPostBack="true" CssClass="form-control" runat="server">
                                                                                                </asp:DropDownList>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div id="divPromocionesTT" class="form-group col-md-4" runat="server">
                                                                                            <label for="ddlPromocionesTT" style="color: #ff9800;">Promociones</label>
                                                                                            <div class="input-group">
                                                                                                <div class="input-group-prepend">
                                                                                                    <span class="input-group-text" style="padding-left: 0px;">
                                                                                                        <i class="material-icons">format_list_numbered</i>
                                                                                                    </span>
                                                                                                </div>
                                                                                                <asp:DropDownList ID="ddlPromocionesTT" OnSelectedIndexChanged="ddlPromocionesTT_SelectedIndexChanged" AppendDataBoundItems="true" AutoPostBack="true" CssClass="form-control" runat="server">
                                                                                                </asp:DropDownList>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div class="row">
                                                                                <table class="table" style="margin-top: 16px;">
                                                                                    <thead>
                                                                                        <tr>
                                                                                            <th style="background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px;" class="text-center">N°</th>
                                                                                            <th style="background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px;">CONCEPTO</th>
                                                                                            <th style="background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px;" class="text-right">PRECIO</th>
                                                                                        </tr>
                                                                                    </thead>
                                                                                    <tbody>
                                                                                        <asp:Repeater ID="rptDesglose" runat="server">
                                                                                            <ItemTemplate>
                                                                                                <tr>
                                                                                                    <td class="text-center"><%#Eval("IntNum")%></td>
                                                                                                    <td><%#Eval("VchConcepto")%></td>
                                                                                                    <td class="text-right" style="color: <%#Eval("VchCoResta")%>;">$<%#Eval("DcmImporte")%></td>
                                                                                                </tr>
                                                                                            </ItemTemplate>
                                                                                        </asp:Repeater>
                                                                                        <tr>
                                                                                            <td style="padding-bottom: 0px;" class="text-center"></td>
                                                                                            <td style="font-weight: bold; padding-top: 0px; padding-bottom: 0px;" class="text-right"></td>
                                                                                            <td style="font-weight: bold; padding-top: 0px; padding-bottom: 0px;" class="text-right"></td>
                                                                                        </tr>
                                                                                        <tr id="trSubtotal" runat="server">
                                                                                            <td style="border-color: white; padding-bottom: 0px;" class="text-center"></td>
                                                                                            <td style="border-color: white; font-weight: bold; padding-top: 0px; padding-bottom: 0px;" class="text-right">Subtotal:</td>
                                                                                            <td style="border-color: white; font-weight: bold; padding-top: 0px; padding-bottom: 0px;" class="text-right">
                                                                                                <asp:Label ID="lblSubtotaltb" runat="server" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trValidarImportetb" runat="server">
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" class="text-center"></td>
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" class="text-right">Importe por validar:
                                                                                            </td>
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" class="text-right">
                                                                                                <asp:Label ID="lblValidarImportetb" Style="color: #f55145;" runat="server" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trComisionTarjeta" runat="server">
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" class="text-center"></td>
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" class="text-right">
                                                                                                <asp:Label ID="lblComisionTarjetatb" runat="server" />
                                                                                            </td>
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" class="text-right">
                                                                                                <asp:Label ID="lblImpComisionTrajetatb" runat="server" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trPromociones" runat="server">
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" class="text-center"></td>
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" class="text-right">
                                                                                                <div class="tooltipse bottom">
                                                                                                    <i class="material-icons">info</i><span class="tiptext"><asp:Label ID="lblToolPromo" Text="Promociones" runat="server" /></span>
                                                                                                </div>
                                                                                                <asp:Label ID="lblPromotb" runat="server" />
                                                                                            </td>
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" class="text-right">
                                                                                                <asp:Label ID="lblImpPromotb" runat="server" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" class="text-center"></td>
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" class="text-right">Total:</td>
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" class="text-right">
                                                                                                <asp:Label ID="lblTotaltb" runat="server" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" class="text-center"></td>
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" class="text-right">
                                                                                                <div class="tooltipse bottom">
                                                                                                    <i class="material-icons">info</i>
                                                                                                    <span class="tiptext" style="width: 230px;">
                                                                                                        <asp:Label ID="lblToolApagar" Text="A pagar" runat="server" /></span>
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
                                                                                                        <asp:TextBox ID="txtTotaltb" Width="80" CssClass="form-control text-right" Font-Bold="true" Font-Size="Medium" runat="server" />
                                                                                                        <asp:LinkButton ID="btnCalcular" OnClick="btnCalcular_Click" runat="server" />
                                                                                                        <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtTotaltb" runat="server" />
                                                                                                    </div>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" class="text-center"></td>
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" class="text-right"></td>
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" class="text-right">
                                                                                                <asp:Label ID="lblRestaTotal" runat="server" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </div>
                                                                            <div class="pull-right" style="padding-top: 10px;">
                                                                                <asp:LinkButton ID="btnGenerarPago" OnClick="btnGenerarpago_Click" ToolTip="Generar pago" runat="server">
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
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
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

    <div id="ModalVerificarPago" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="TitleModal" aria-hidden="true" style="overflow-y: scroll;">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header" style="padding-top: 0px; padding-bottom: 0px;">
                            <h5 id="TitleModal" class="modal-title" runat="server">
                                <asp:Label ID="lblModalVerificarPago" Text="Verificación del pago" runat="server" /></h5>
                            <asp:LinkButton ID="btnCancelarModalTipoPago" OnClientClick="hideModalVerificarPago();" CssClass="close" runat="server">
                            <span aria-hidden="true">&times;</span>
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="modal-body pt-0" style="padding-bottom: 0px;">
                    <div class="tab-content">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlAlertModalVerificarPago" Visible="false" runat="server">
                                    <div id="divAlertModalVerificarPago" class="alert alert-danger alert-dismissible fade" role="alert" runat="server">
                                        <asp:Label runat="server" ID="lblMnsjModalVerificarPago" />
                                        <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
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
                                        <asp:Repeater ID="rpEstatusPagos" OnItemCommand="rpEstatusPagos_ItemCommand" OnItemDataBound="rpEstatusPagos_ItemDataBound" runat="server">
                                            <ItemTemplate>
                                                <div class="col-sm-12 col-md-12 col-lg-12 col-xl-12" style="cursor: pointer;">
                                                    <asp:UpdatePanel runat="server">
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="btnSeleEstatusPago" CommandArgument='<%#Eval("UidEstatusFechaPago")%>' CommandName="btnSeleEstatusPago" runat="server">
                                                                <div class="card cardEfe" style="margin-top: 15px; margin-bottom: 15px; border-left: 8px solid <%#Eval("VchColor")%>;">
                                                                    <asp:CheckBox ID="cbSeleccionado" CssClass="form-check-input" Style="margin-left: 8px;" runat="server" />
                                                                    <div class="card-body">
                                                                        <%--<asp:Label ID="lblSeleccionado" Width="100%" Style="background-color: red;" runat="server" />--%>
                                                                        <%--<img src="../Images/FormasDePago/<%#Eval("VchImagen")%>" style="background-color: #d8ecfe" height="70" width="70" class="float-left rounded-circle">--%>
                                                                        <div style="padding-left: 80px;">
                                                                            <h5 class="card-title" style="font-weight: bold;"><%#Eval("VchDescripcion")%></h5>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </asp:LinkButton>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnSeleEstatusPago" EventName="Click" />
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
                                                        <label for="lblFormaPago" style="color: black;">Forma de pago</label>
                                                        <div class="input-group">
                                                            <div class="input-group-prepend">
                                                                <span class="input-group-text" style="padding-left: 0px;">
                                                                    <i class="material-icons">payments</i>
                                                                </span>
                                                            </div>
                                                            <asp:Label ID="lblFormaPago" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-md-12">
                                                        <label for="lblBanco" style="color: black;">Banco receptor</label>
                                                        <div class="input-group">
                                                            <div class="input-group-prepend">
                                                                <span class="input-group-text" style="padding-left: 0px;">
                                                                    <i class="material-icons">account_balance</i>
                                                                </span>
                                                            </div>
                                                            <asp:Label ID="lblBanco" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-md-12">
                                                        <label for="lblCuenta" style="color: black;">Ultimos 4 digitos de la cuenta receptora</label>
                                                        <div class="input-group">
                                                            <div class="input-group-prepend">
                                                                <span class="input-group-text" style="padding-left: 0px;">
                                                                    <i class="material-icons">account_box</i>
                                                                </span>
                                                            </div>
                                                            <asp:Label ID="lblCuenta" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-md-12">
                                                        <label for="lblFechaPago" style="color: black;">Fecha y Hora</label>
                                                        <div class="input-group">
                                                            <div class="input-group-prepend">
                                                                <span class="input-group-text" style="padding-left: 0px;">
                                                                    <i class="material-icons">date_range</i>
                                                                </span>
                                                            </div>
                                                            <asp:Label ID="lblFechaPago" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-md-12">
                                                        <label for="lblImportePagado" style="color: black;">Monto</label>
                                                        <div class="input-group">
                                                            <div class="input-group-prepend">
                                                                <span class="input-group-text" style="padding-left: 0px;">
                                                                    <i class="material-icons">attach_money</i>
                                                                </span>
                                                            </div>
                                                            <asp:Label ID="lblImportePagado" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-md-12">
                                                        <label for="lblFolio" style="color: black;">Folio</label>
                                                        <div class="input-group" style="padding-top: 7px;">
                                                            <div class="input-group-prepend">
                                                                <span class="input-group-text" style="padding-left: 0px;">
                                                                    <i class="material-icons">article</i>
                                                                </span>
                                                            </div>
                                                            <asp:Label ID="lblFolio" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <div class="row justify-content-between">
                                    <div class="col-12">
                                        <asp:LinkButton ID="btnVerificar" OnClick="btnVerificar_Click" CssClass="btn btn-success btn-round pull-right" runat="server">
                                            <i class="material-icons">check</i> CONFIRMAR
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

    <div id="ModalBusqueda" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header" style="padding-bottom: 0px; padding-top: 5px; margin-bottom: 5px;">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="lblTittleLigas" Text="Filtro de busqueda" runat="server" /></h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <div class="modal-body pt-0" style="padding-bottom: 0px;">
                    <asp:Label Text="Despliegue una sección para mostrar los campos de busqueda." runat="server" />
                    <asp:Panel ID="pnlFiltrosBusqueda" runat="server">
                        <div class="accordionCard" style="margin-top: 15px; margin-bottom: 0px; border-left: 8px solid black;">
                            <label style="font-size: 1.0625rem; font-weight: bold; color: black;">Datos colegiatura</label>
                        </div>
                        <div class="panelFiltro">
                            <div class="row">
                                <div class="card" style="margin-top: 0px; margin-bottom: 0px; border-left: 8px solid black;">
                                    <div class="card-body">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <div class="row">
                                                    <div class="form-group col-md-6">
                                                        <label for="txtColegiatura" style="margin-left: 15px; color: black;">Colegiatura</label>
                                                        <asp:TextBox ID="txtColegiatura" CssClass="form-control" aria-label="Search" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-2">
                                                        <label for="txtNumPago" style="margin-left: 15px; color: black;"># de pago</label>
                                                        <asp:TextBox ID="txtNumPago" CssClass="form-control" TextMode="Number" aria-label="Search" runat="server" />
                                                        <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" TargetControlID="txtNumPago" runat="server" />
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="accordionCard" style="margin-top: 15px; margin-bottom: 0px; border-left: 8px solid #326497;">
                            <label style="font-size: 1.0625rem; font-weight: bold; color: black;">Datos alumno</label>
                        </div>
                        <div class="panelFiltro">
                            <div class="row">
                                <div class="card" style="margin-top: 0px; margin-bottom: 0px; border-left: 8px solid #326497;">
                                    <div class="card-body">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <div class="row">
                                                    <div class="form-group col-md-3">
                                                        <label for="txtMatricula" style="margin-left: 15px; color: black;">Matricula</label>
                                                        <asp:TextBox ID="txtMatricula" CssClass="form-control" aria-label="Search" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-3">
                                                        <label for="txtAlNombre" style="margin-left: 15px; color: black;">Nombre</label>
                                                        <asp:TextBox ID="txtAlNombre" CssClass="form-control" aria-label="Search" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-3">
                                                        <label for="txtAlApPaterno" style="margin-left: 15px; color: black;">Apellido Paterno</label>
                                                        <asp:TextBox ID="txtAlApPaterno" CssClass="form-control" aria-label="Search" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-3">
                                                        <label for="txtAlApMaterno" style="margin-left: 15px; color: black;">Apellido Materno</label>
                                                        <asp:TextBox ID="txtAlApMaterno" CssClass="form-control" aria-label="Search" runat="server" />
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="accordionCard" style="margin-top: 15px; margin-bottom: 0px; border-left: 8px solid #ff9800;">
                            <label style="font-size: 1.0625rem; font-weight: bold; color: black;">Datos tutor</label>
                        </div>
                        <div class="panelFiltro">
                            <div class="row">
                                <div class="card" style="margin-top: 0px; margin-bottom: 0px; border-left: 8px solid #ff9800;">
                                    <div class="card-body">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <div class="row">
                                                    <div class="form-group col-md-4">
                                                        <label for="txtTuNombre" style="margin-left: 15px; color: black;">Nombre</label>
                                                        <asp:TextBox ID="txtTuNombre" CssClass="form-control" aria-label="Search" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="txtTuApPaterno" style="margin-left: 15px; color: black;">Apellido Paterno</label>
                                                        <asp:TextBox ID="txtTuApPaterno" CssClass="form-control" aria-label="Search" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="txtTuApMaterno" style="margin-left: 15px; color: black;">Apellido Materno</label>
                                                        <asp:TextBox ID="txtTuApMaterno" CssClass="form-control" aria-label="Search" runat="server" />
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="accordionCard" style="margin-top: 15px; margin-bottom: 0px; border-left: 8px solid #0094ff;">
                            <label style="font-size: 1.0625rem; font-weight: bold; color: black;">Datos pago</label>
                        </div>
                        <div class="panelFiltro">
                            <div class="row">
                                <div class="card" style="margin-top: 0px; margin-bottom: 0px; border-left: 8px solid #0094ff;">
                                    <div class="card-body">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <div class="row">
                                                    <div class="form-group col-md-4">
                                                        <label for="txtFolio" style="margin-left: 15px; color: black;">Folio</label>
                                                        <asp:TextBox ID="txtFolio" CssClass="form-control" aria-label="Search" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="txtCuenta" style="margin-left: 15px; color: black;">Cuenta</label>
                                                        <asp:TextBox ID="txtCuenta" CssClass="form-control" MaxLength="4" aria-label="Search" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="txtBanco" style="margin-left: 15px; color: black;">Banco</label>
                                                        <asp:TextBox ID="txtBanco" CssClass="form-control" aria-label="Search" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-6">
                                                        <label for="txtRegistroDesde" style="margin-left: 15px; color: black;">Fecha Pago(desde)</label>
                                                        <asp:TextBox ID="txtRegistroDesde" CssClass="form-control" TextMode="Date" aria-label="Search" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-6">
                                                        <label for="txtRegistroHasta" style="color: black;">Fecha Pago(hasta)</label>
                                                        <asp:TextBox ID="txtRegistroHasta" CssClass="form-control" TextMode="Date" aria-label="Search" runat="server" />
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
                                                    <div class="form-group col-md-3">
                                                        <label for="ddlFormaPago" style="color: black;">Forma pago</label>
                                                        <asp:DropDownList ID="ddlFormaPago" AppendDataBoundItems="true" CssClass="form-control" Style="margin-top: 6px;" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-md-3">
                                                        <label for="ddlEstatus" style="color: black;">Estatus</label>
                                                        <asp:DropDownList ID="ddlEstatus" AppendDataBoundItems="true" CssClass="form-control" Style="margin-top: 6px;" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <div class="modal-footer justify-content-center" style="padding-top: 5px; padding-bottom: 5px;">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:LinkButton ID="btnBuscar" OnClick="btnBuscar_Click" CssClass="btn btn-primary btn-round" runat="server">
                            <i class="material-icons">search</i> Buscar
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnLimpiar" OnClick="btnLimpiar_Click" CssClass="btn btn-warning btn-round" runat="server">
                            <i class="material-icons">clear_all</i> Limpiar
                            </asp:LinkButton>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
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
                                                                <td align="center" valign="top" style="padding-top: 20px;">
                                                                    <asp:LinkButton ID="btnImprimir" OnClick="btnImprimir_Click" CssClass="btn btn-success btn-round" runat="server">
                                                                        <i class="material-icons">print</i>Imprimir
                                                                    </asp:LinkButton>
                                                                </td>
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
                                                                            <tr id="trvalidarimporte" runat="server">
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
                                <asp:Label ID="Label2" Text="Detalle del pago" runat="server" /></h5>
                            <asp:LinkButton ID="LinkButton3" data-dismiss="modal" aria-label="Close" CssClass="close" runat="server">
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
                                                                <td align="center" valign="top" style="padding-top: 20px;">
                                                                    <asp:LinkButton ID="btnImprimirManual" OnClick="btnImprimir_Click" CssClass="btn btn-success btn-round" runat="server">
                                                                        <i class="material-icons">print</i>Imprimir
                                                                    </asp:LinkButton>
                                                                </td>
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
    <div id="ModalPagoDetalleClubPago" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header" style="padding-top: 0px; padding-bottom: 0px;">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="Label1" Text="Detalle del pago" runat="server" /></h5>
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
                                                                <td align="center" valign="top" style="padding-top: 20px;">
                                                                    
                                                                </td>
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
                                                                                    <asp:Label ID="lblDetaAlumnoClubPago" Text="Alumno: " runat="server" />
                                                                                    <br />
                                                                                    <asp:Label ID="lblDetaMatriculaClubPago" Text="Matricula: " runat="server" />
                                                                                    <br />
                                                                                    <br />

                                                                                </td>
                                                                                <td bgcolor="#ffffff" valign="top" style="border-radius: 4px 4px 0px 0px; padding-left: 10px; padding-right: 10px;">
                                                                                    <br />
                                                                                    <asp:Label ID="lblDetaFHpagoClubPago" Text="Fecha de pago: " runat="server" />
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
                                                                                <th style="border-collapse: collapse; background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px;" align="right">IMPORTE</th>
                                                                            </tr>
                                                                        </thead>
                                                                        <tbody>
                                                                            <asp:Repeater ID="rpDetalleClubPago" runat="server">
                                                                                <ItemTemplate>
                                                                                    <tr>
                                                                                        <td style="border-bottom: 1px solid #ddd;" bgcolor="#ffffff" align="center"><%#Eval("IntNum")%></td>
                                                                                        <td style="border-bottom: 1px solid #ddd;" bgcolor="#ffffff"><%#Eval("VchDescripcion")%></td>
                                                                                        <td style="border-bottom: 1px solid #ddd; color: <%#Eval("VchColor")%>;" bgcolor="#ffffff" align="right">$<%#Eval("DcmImporte")%> </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>

                                                                            <tr id="trsubtotalClubPago" runat="server">
                                                                                <td style="font-weight: bold; padding-top: 15px; padding-bottom: 0px;" bgcolor="#ffffff" align="center"></td>
                                                                                <td style="font-weight: bold; padding-top: 15px; padding-bottom: 0px;" bgcolor="#ffffff" align="right">Subtotal:</td>
                                                                                <td style="font-weight: bold; padding-top: 15px; padding-bottom: 0px;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="DcmSubtotalClubPago" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trvalidarimporteClubPago" runat="server">
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" bgcolor="#ffffff" align="center"></td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">Importe por validar:
                                                                                </td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="DcmValidarImporteClubPago" Style="color: #f55145;" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" bgcolor="#ffffff" align="center"></td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">Importe pagado:</td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="DcmTotalClubPago" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trcomicionClubPago" runat="server">
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" bgcolor="#ffffff" align="center"></td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="VchComicionClubPago" runat="server" />
                                                                                </td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="DcmImpComisionClubPago" Style="color: #f55145;" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" bgcolor="#ffffff" align="center"></td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">Abono pago:
                                                                                </td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="DcmImpAbonoClubPago" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" bgcolor="#ffffff" align="center"></td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">Resta:
                                                                                </td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="DcmImpRestaClubPago" runat="server" />
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
                                            <tr id="trdetalleoperacionClubPago" runat="server">
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
                                                                                <td bgcolor="#ffffff" align="left" style="padding: 10px 0px 30px 0px; border-radius: 4px 4px 4px 4px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 25px;">
                                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                        <thead>
                                                                                            <tr>
                                                                                                <th style="border-collapse: collapse; background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px;" align="left">REFERENCIA</th>
                                                                                                <th style="border-collapse: collapse; background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px;" align="left">FECHA</th>
                                                                                                <th style="border-collapse: collapse; background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px;" align="left">TRANSACCIÓN</th>
                                                                                                <th style="border-collapse: collapse; background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px;" align="right">MONTO</th>
                                                                                            </tr>
                                                                                        </thead>
                                                                                        <tbody>
                                                                                            <asp:Repeater ID="rptPagosRefClubPago" runat="server">
                                                                                                <ItemTemplate>
                                                                                                    <tr>
                                                                                                        <td style="border-bottom: 1px solid #ddd;" bgcolor="#ffffff" align="left"><%#Eval("IdReferencia")%></td>
                                                                                                        <td style="border-bottom: 1px solid #ddd;" bgcolor="#ffffff" align="left"><%#Eval("DtFechaRegistro", "{0:dd/MM/yyyy}")%></td>
                                                                                                        <td style="border-bottom: 1px solid #ddd;" bgcolor="#ffffff" align="center"><%#Eval("VchTransaccion")%></td>
                                                                                                        <td style="border-bottom: 1px solid #ddd;" bgcolor="#ffffff" align="right">$<%#Eval("DcmMonto")%></td>
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
    <%--END MODAL--%>

    <script>
        function showModalTipoPago() {
            $('#ModalTipoPago').modal('show');
            showTabAlumno();
        }

        function hideModalTipoPago() {
            $('#ModalTipoPago').modal('hide');
            hideModalDialog();
        }
    </script>
    <script>
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
        function showModalBusqueda() {
            $('#ModalBusqueda').modal('show');
        }

        function hideModalBusqueda() {
            $('#ModalBusqueda').modal('hide');
        }
    </script>
    <script>
        function showModalVerificarPago() {
            $('#ModalVerificarPago').modal('show');
        }

        function hideModalVerificarPago() {
            $('#ModalVerificarPago').modal('hide');
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
        function showModalPagoDetalleClubPago() {
            $('#ModalPagoDetalleClubPago').modal('show');
        }
        function hideModalPagoDetalleClubPago() {
            $('#ModalPagoDetalleClubPago').modal('hide');
        }
    </script>
    <script>
        var acc = document.getElementsByClassName("accordionCard");
        var i;

        for (i = 0; i < acc.length; i++) {
            acc[i].addEventListener("click", function () {
                this.classList.toggle("activeAccordion");
                var panel = this.nextElementSibling;
                if (panel.style.maxHeight) {
                    panel.style.maxHeight = null;
                } else {
                    panel.style.maxHeight = panel.scrollHeight + "px";
                }
            });
        }
    </script>

    <script>
        function showTabAlumno() {
            let elm = document.getElementById('aAlumno');
            elm.className = 'nav-link';

            let elm2 = document.getElementById('aColegiatura');
            elm2.className = 'nav-link disabled';

            let elm3 = document.getElementById('aPago');
            elm3.className = 'nav-link disabled';

            $('#ulTabAgregarPago a[href="#alumno"]').tab('show')
        }
        function showTabColegiatura() {
            let elm = document.getElementById('aAlumno');
            elm.className = 'nav-link disabled';

            let elm2 = document.getElementById('aColegiatura');
            elm2.className = 'nav-link';

            let elm3 = document.getElementById('aPago');
            elm3.className = 'nav-link disabled';

            $('#ulTabAgregarPago a[href="#colegiatura"]').tab('show')
        }
        function showTabFinalizar() {
            let elm = document.getElementById('aAlumno');
            elm.className = 'nav-link disabled';

            let elm2 = document.getElementById('aColegiatura');
            elm2.className = 'nav-link disabled';

            let elm3 = document.getElementById('aPago');
            elm3.className = 'nav-link';

            $('#ulTabAgregarPago a[href="#pago"]').tab('show')
        }
    </script>
</asp:Content>
