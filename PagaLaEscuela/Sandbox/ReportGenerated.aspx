<%@ Page Title="" Language="C#" MasterPageFile="~/Sandbox/Sandbox.Master" AutoEventWireup="true" CodeBehind="ReportGenerated.aspx.cs" Inherits="PagaLaEscuela.Sandbox.ReportGenerated" %>

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
                                <%--#00bcd4--%>
                                <div class="card-header card-header-tabs card-header-primary" style="background: #b9504c; padding-top: 0px; padding-bottom: 0px;">
                                    <div class="nav-tabs-navigation">
                                        <div class="nav-tabs-wrapper">
                                            <div class="form-group" style="margin-top: 0px;padding-bottom: 20px;padding-top: 20px;" <%--style="margin-top: 0px; padding-bottom: 0px;"--%>>
                                                <%--<asp:LinkButton ID="btnFiltros" ToolTip="Filtros de busqueda." BackColor="#4db6ac" class="btn btn-lg btn-fab btn-fab-mini btn-round" runat="server">
                                                        <i class="material-icons">search</i>
                                                </asp:LinkButton>--%>
                                                <asp:Label Text="Reporte" runat="server" />

                                                <%--<asp:LinkButton ID="btnActualizarLista" ToolTip="Actualizar tabla." class="btn btn-lg btn-success btn-fab btn-fab-mini btn-round pull-right" runat="server">
                                                        <i class="material-icons">sync</i>
                                                </asp:LinkButton>--%>

                                                <%--<asp:LinkButton ID="btnExportarLista" OnClick="btnExportarLista_Click" ToolTip="Exportar tabla a excel." class="btn btn-lg btn-warning btn-fab btn-fab-mini btn-round pull-right" runat="server">
                                                        <i class="material-icons">file_download</i>
                                                </asp:LinkButton>--%>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="card" style="border-left: 8px solid #b9504c;padding: 10px;margin-top: 0px;margin-bottom: 0px;">
                                                <div class="row">
                                                    <div class="col-md-3">
                                                        <label for="txtFiltroDesde" style="color: black;">Desde</label>
                                                        <asp:TextBox ID="txtFiltroDesde" TextMode="Date" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label for="txtFiltroHasta" style="color: black;">Hasta</label>
                                                        <asp:TextBox ID="txtFiltroHasta" TextMode="Date" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label for="txtFiltroReferencia" style="color: black;">Referencia</label>
                                                        <asp:TextBox ID="txtFiltroReferencia" CssClass="form-control" runat="server" />
                                                    </div>

                                                    <div class="col-md-3">
                                                        <asp:LinkButton ID="btnLimpiar" OnClick="btnLimpiar_Click" BorderWidth="1" CssClass="btn btn-warning btn-sm" runat="server">
                                                            <i class="material-icons">delete</i> Limpiar
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="btnBuscar" OnClick="btnBuscar_Click" BorderWidth="1" CssClass="btn btn-info btn-sm" runat="server">
                                                            <i class="material-icons">search</i> Buscar
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-12" style="margin-top: 20px;">
                                            <div class="table-responsive">
                                                <asp:GridView ID="gvReporteGeneradas" OnSorting="gvReporteGeneradas_Sorting" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidPagoIntegracion" GridLines="None" border="0" ShowHeaderWhenEmpty="true" runat="server">
                                                    <EmptyDataTemplate>
                                                        <div style="color:black;">No hay información para mostrar</div>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:BoundField SortExpression="DtRegistro" DataField="DtRegistro" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" HeaderText="FECHA" />
                                                        <asp:BoundField SortExpression="IdReferencia" DataField="IdReferencia" HeaderText="REFERENCIA" />
                                                        <asp:BoundField SortExpression="DcmImporte" DataField="DcmImporte" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-right" DataFormatString="{0:C}" HeaderText="IMPORTE" />
                                                        <asp:BoundField SortExpression="DtVencimiento" DataField="DtVencimiento" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" HeaderText="VENCIMIENTO" />
                                                        <asp:BoundField SortExpression="VchFormaPago" DataField="VchFormaPago" HeaderText="FORMA PAGO" />
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cBodyBottom" runat="server">
</asp:Content>
