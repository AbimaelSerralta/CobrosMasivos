<%@ Page Title="" Language="C#" MasterPageFile="~/Sandbox/Sandbox.Master" AutoEventWireup="true" CodeBehind="GeneratedReferences.aspx.cs" Inherits="PagaLaEscuela.Sandbox.GeneratedReferences" %>

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
                                <div class="card-header card-header-tabs card-header-primary" style="background: #0099d4; padding-top: 0px; padding-bottom: 0px;">
                                    <div class="nav-tabs-navigation">
                                        <div class="nav-tabs-wrapper">
                                            <div class="form-group" style="margin-top: 0px; padding-bottom: 0px;">
                                                <asp:LinkButton ID="btnFiltros" ToolTip="Filtros de busqueda." BackColor="#4db6ac" class="btn btn-lg btn-fab btn-fab-mini btn-round" runat="server">
                                                        <i class="material-icons">search</i>
                                                </asp:LinkButton>
                                                <asp:Label Text="Listado de pagos generados" runat="server" />

                                                <asp:LinkButton ID="btnActualizarLista" ToolTip="Actualizar tabla." class="btn btn-lg btn-success btn-fab btn-fab-mini btn-round pull-right" runat="server">
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
                                            <asp:GridView ID="gvLigasGeneradas" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidLigaUrl" GridLines="None" border="0" AllowPaging="true" PageSize="10" OnPageIndexChanging="gvLigasGeneradas_PageIndexChanging" runat="server">
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
</asp:Content>
