<%@ Page Title="" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReporteColegiaturas.aspx.cs" Inherits="PagaLaEscuela.Views.ReporteColegiaturas" %>

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
            /*width: 100%;*/
        }
    </style>

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
                                                <asp:Label Text="Listado de colegiaturas" runat="server" />

                                                <div class="pull-right">
                                                    <asp:LinkButton ID="btnExportarLista" OnClick="btnExportarLista_Click" ToolTip="Exportar lista a excel." class="btn btn-lg btn-warning btn-fab btn-fab-mini btn-round" runat="server">
                                                        <i class="material-icons">file_download</i>
                                                    </asp:LinkButton>
                                                    ||
                                                    <asp:LinkButton ID="btnActualizarLista" OnClick="btnActualizarLista_Click" ToolTip="Actualizar tabla." class="btn btn-lg btn-info btn-fab btn-fab-mini btn-round pull-right" runat="server">
                                                        <i class="material-icons">sync</i>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvPagos" OnRowCreated="gvPagos_RowCreated" OnPageIndexChanging="gvPagos_PageIndexChanging" OnSorting="gvPagos_Sorting" OnRowCommand="gvPagos_RowCommand" OnRowDataBound="gvPagos_RowDataBound" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidFechaColegiatura" GridLines="None" border="0" AllowPaging="true" PageSize="10" ShowFooter="true" runat="server">
                                                <EmptyDataTemplate>
                                                    <div class="alert alert-info">No hay colegiaturas disponibles.</div>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-Width="160" SortExpression="VchIdentificador" HeaderText="COLEGIATURA">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGvIdentificador" ToolTip='<%#Eval("VchIdentificador")%>' Style="width: 100%; text-overflow: ellipsis;" Text='<%#Eval("VchIdentificador")%>' Enabled="false" BackColor="Transparent" BorderStyle="None" runat="server" />
                                                            <asp:TextBox ID="txtGvUidCliente" Text='<%#Eval("UidCliente")%>' Visible="false" runat="server" />
                                                            <asp:TextBox ID="txtGvUidAlumno" Text='<%#Eval("UidAlumno")%>' Visible="false" runat="server" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblPaginado" Font-Bold="true" runat="server" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="VchMatricula" HeaderStyle-CssClass="hiddenHeaderGrid" ItemStyle-CssClass="hiddenHeaderGrid" />
                                                    <asp:TemplateField SortExpression="NombreCompleto" HeaderStyle-CssClass="text-center" HeaderText="ALUMNO">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGvAlumno" ToolTip='<%#Eval("NombreCompleto")%>' Style="width: 100%; text-overflow: ellipsis;" Text='<%#Eval("NombreCompleto")%>' Enabled="false" BackColor="Transparent" BorderStyle="None" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="VchNum" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="# DE PAGOS">
                                                        <ItemTemplate>
                                                            <table style="width: 100%;">
                                                                <tbody>
                                                                    <tr style="background: transparent;">
                                                                        <td style="width: 70%; vertical-align: middle; border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:Label Text='<%#Eval("VchNum")%>' runat="server" />
                                                                        </td>
                                                                        <td style="width: 30%; border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:LinkButton ID="btnInfoCole" ToolTip="Detalle de la colegiatura" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="btnInfoCole" Style="margin-left: 5px;" runat="server">
                                                                                        <i class="material-icons" style="color:black;">info_outline</i>
                                                                            </asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:BoundField SortExpression="DcmImporte" DataField="DcmImporte" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" DataFormatString="{0:C}" HeaderText="IMPORTE" />
                                                    <asp:BoundField SortExpression="ImpPagado" DataField="ImpPagado" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" DataFormatString="{0:C}" HeaderText="ABONADO" />
                                                    <asp:BoundField SortExpression="ImpTotal" DataField="ImpTotal" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" DataFormatString="{0:C}" HeaderText="SALDO" />
                                                    <asp:BoundField SortExpression="DtFHInicio" DataField="DtFHInicio" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" DataFormatString="{0:dd/MM/yyyy}" HeaderText="INICIO" />
                                                    <asp:TemplateField SortExpression="VchEstatusFechas" HeaderText="ESTATUS">
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%#Eval("VchEstatusFechas")%>' ForeColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("VchColor").ToString()) %>' Font-Names="Comic Sans MS" Font-Bold="true" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="EstatusPago" HeaderText="PAGO">
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%#Eval("EstatusPago")%>' ForeColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("ColorEstatusPago").ToString()) %>' Font-Names="Comic Sans MS" Font-Bold="true" runat="server"></asp:Label>
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
                    <asp:Panel ID="pnlFiltrosBusqueda" runat="server">
                        <div class="row" style="margin-left: 0px; margin-right: 0px;">
                            <div class="card" style="margin-top: 0px; margin-bottom: 0px; border-left: 8px solid black;">
                                <div class="card-body" style="padding-top: 0px;">
                                    <div style="margin-top: 7px; margin-bottom: 0px;">
                                        <label style="font-size: 1.0625rem; font-weight: bold; color: black; padding-left: 0px;">Datos colegiatura</label>
                                    </div>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="form-group col-md-3">
                                                    <label for="txtColegiatura" style="color: black;">Colegiatura</label>
                                                    <asp:TextBox ID="txtColegiatura" Style="margin-top: 8px;" CssClass="form-control" aria-label="Search" runat="server" />
                                                </div>
                                                <div class="form-group col-md-2">
                                                    <label for="txtNumPago" style="color: black;"># de pago</label>
                                                    <asp:TextBox ID="txtNumPago" Style="margin-top: 8px;" CssClass="form-control" TextMode="Number" aria-label="Search" runat="server" />
                                                    <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" TargetControlID="txtNumPago" runat="server" />
                                                </div>
                                                <div class="form-group col-md-4">
                                                    <label for="LBFiltroEstatusCole" style="color: black; padding-left: 0px;">Estatus</label>
                                                    <asp:ListBox ID="LBFiltroEstatusCole" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                                </div>
                                                <div class="form-group col-md-3">
                                                    <label for="LBFiltroEstatusPago" style="color: black; padding-left: 0px;">Pago</label>
                                                    <asp:ListBox ID="LBFiltroEstatusPago" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>

                            <div class="card" style="margin-top: 0px; margin-bottom: 0px; border-left: 8px solid #326497;">
                                <div class="card-body" style="padding-top: 0px;">
                                    <div style="margin-top: 7px; margin-bottom: 0px;">
                                        <label style="font-size: 1.0625rem; font-weight: bold; color: black; padding-left: 0px;">Datos alumno</label>
                                    </div>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="form-group col-md-3">
                                                    <label for="txtAlMatricula" style="margin-left: 15px; color: black;">Matricula</label>
                                                    <asp:TextBox ID="txtAlMatricula" CssClass="form-control" aria-label="Search" runat="server" />
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

                            <div class="card" style="margin-top: 0px; margin-bottom: 0px; border-left: 8px solid #ff9800;">
                                <div class="card-body" style="padding-top: 0px;">
                                    <div style="margin-top: 7px; margin-bottom: 0px;">
                                        <label style="font-size: 1.0625rem; font-weight: bold; color: black; padding-left: 0px;">Datos tutor</label>
                                    </div>
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
    <div id="ModalDetalleCole" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="Label5" Text="Detalle de la colegiatura" runat="server" />
                            </h5>
                            <asp:LinkButton ID="LinkButton5" OnClientClick="hideModalDetalleCole();" aria-label="Close" CssClass="close" runat="server">
                            <span aria-hidden="true">&times;</span>
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="modal-body pt-0" style="padding-bottom: 0px;">
                    <div class="tab-content">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="Panel2" Visible="false" runat="server">
                                    <div id="div1" class="alert alert-danger alert-dismissible fade" role="alert" runat="server">
                                        <asp:Label ID="Label6" runat="server" />
                                        <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    </div>
                                </asp:Panel>
                                <div class="row">
                                    <div class="col-lg-12 col-md-12">
                                        <div class="card" style="margin-top: 15px; margin-bottom: 15px;">
                                            <div class="card-body">
                                                <div class="row">
                                                    <div class="table-responsive">
                                                        <table class="table table-hover">
                                                            <tr>
                                                                <td style="padding-bottom: 0px;" class="text-center"></td>
                                                                <td style="font-weight: bold; padding-top: 0px; padding-bottom: 0px;" class="text-right"></td>
                                                                <td style="font-weight: bold; padding-top: 0px; padding-bottom: 0px;" class="text-right"></td>
                                                            </tr>
                                                            <tr>
                                                                <th style="padding-left: 5px;" class="text-right"># DE PAGO:</th>
                                                                <td class="text-left">
                                                                    <asp:Label ID="lblNoPagoDetalleCole" runat="server" /></td>
                                                            </tr>
                                                            <tr>
                                                                <th style="padding-left: 5px;" class="text-right">MATRICULA:</th>
                                                                <td class="text-left">
                                                                    <asp:Label ID="lblMatriculaDetalleCole" runat="server" /></td>
                                                            </tr>
                                                            <tr>
                                                                <th style="padding-left: 5px;" class="text-right">ALUMNO:</th>
                                                                <td class="text-left">
                                                                    <asp:Label ID="lblAlumnoDetalleCole" runat="server" /></td>
                                                            </tr>
                                                            <tr>
                                                                <th style="padding-left: 5px;" class="text-right">FECHA LIMITE:</th>
                                                                <td class="text-left">
                                                                    <asp:Label ID="lblFLDetalleCole" runat="server" /></td>
                                                            </tr>

                                                            <tr>
                                                                <th style="width: 35%; padding-left: 5px;" class="text-right">FECHA VENCIMIENTO:</th>
                                                                <td class="text-left">
                                                                    <asp:Label ID="lblFVDetalleCole" runat="server" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-bottom: 0px;" class="text-center"></td>
                                                                <td style="font-weight: bold; padding-top: 0px; padding-bottom: 0px;" class="text-right"></td>
                                                                <td style="font-weight: bold; padding-top: 0px; padding-bottom: 0px;" class="text-right"></td>
                                                            </tr>
                                                        </table>
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
    <!--END MODAL-->

    <script>
        function showModalBusqueda() {
            $('#ModalBusqueda').modal('show');
            multiFiltro();
        }
        function hideModalBusqueda() {
            $('#ModalBusqueda').modal('hide');
        }
    </script>
    <script>
        function showModalDetalleCole() {
            $('#ModalDetalleCole').modal('show');
        }
        function hideModalDetalleCole() {
            $('#ModalDetalleCole').modal('hide');
        }
    </script>
    <script>
        function multiFiltro() {
            $('[id*=LBFiltro]').multiselect({
                includeSelectAllOption: false,
                nonSelectedText: "TODOS"
            });
        }
    </script>
</asp:Content>
