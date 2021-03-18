<%@ Page Title="Pagos" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="Pagos.aspx.cs" Inherits="PagaLaEscuela.Views.Pagos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHCaja" runat="server">
    <style type="text/css">
        .divpersonal {
            background-color: transparent;
            position: absolute;
            height: 150px;
            width: 200px;
            left: 50%;
            top: 50%;
            margin-top: -100px;
            margin-left: -150px;
        }
    </style>
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
            <%--<div class="row">
                <div class="card" style="background-color:transparent;border:0; margin-top: 20px;">
                    <div class="card-header card-header-primary" style="background: #0099d4; padding-bottom: 0px; padding-top: 0px;">
                        <div class="row">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 20%; padding-left: 5px;">--%>
            <%--Datos--%>
            <%--</td>
                                    <td style="width: 15%; padding-right: 5px;">--%>
            <%--Datos--%>
            <%--          </td>
                                    <td style="width: 45%;">
                                        <h3 class="card-title">
                                            <asp:Label ID="Label2" Text="Bienvenido" runat="server" />
                                        </h3>
                                    </td>

                                    <td style="width: 20%; padding-right: 5;">
                                        <asp:LinkButton ID="LinkButton2" ToolTip="Actualizar escuelas." class="btn btn-lg btn-success btn-fab btn-fab-mini btn-round pull-right" runat="server">
                                                        <i class="material-icons">sync</i>
                                        </asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>--%>
            <asp:Panel ID="pnlSinEscuelas" Visible="false" runat="server">
                <div class="divpersonal">
                    <div style="text-align: center;">
                        <table style="margin: 0 auto;">
                            <tbody>
                                <tr>
                                    <td>
                                        <img src="../Images/SinPagos.png" height="150" width="150" class="img-fluid" role="presentation">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <h3 style="font-weight: bold; margin-bottom: 0px;">Felicidades</h3>
                                        <p>No tiene pagos disponibles</p>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlComercios" Visible="true" runat="server">
                <div class="row">
                    <asp:Repeater ID="rpComercios" OnItemCommand="rpComercios_ItemCommand" runat="server">
                        <ItemTemplate>
                            <div class="col-sm-4 col-md-3 col-lg-3 col-xl-3">
                                <div class="card cardEfe h-100">
                                    <div class="card-header">
                                        <div class="fileinput-new thumbnail img-raised">
                                            <asp:Image ID="imgLogoCliente" ImageUrl='<%#"data:Image/png;base64," + Convert.ToBase64String((byte[])Eval("Imagen")) %>' CssClass="card-img-top" runat="server" />
                                        </div>
                                    </div>
                                    <div class="card-body">
                                        <h4 class="card-title text-center" style="font-weight: bold;">
                                            <%#Eval("VchNombreComercial")%>
                                        </h4>
                                        <p class="card-text">
                                            <%#Eval("Direccion")%>
                                            <br />
                                            C.P: <%#Eval("CodigoPostal")%>
                                            <br />
                                            Cel: <%#Eval("VchTelefono")%>
                                        </p>
                                    </div>
                                    <div class="card-footer">
                                        <asp:LinkButton ID="btnIrPagos" CssClass="pull-center" ToolTip="Editar" CommandArgument='<%#Eval("UidCliente")%>' CommandName="Pagos" runat="server">
                                            <asp:Label ID="Label1" class="btn btn-success btn-round" runat="server">
                                        <i class="material-icons">list</i> Seleccionar
                                            </asp:Label>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </asp:Panel>
            <%--</div>
            </div>--%>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlPagos" Visible="false" runat="server">
                <div class="row">
                    <div class="col-md-12">
                        <div class="card">
                            <div class="card-header card-header-primary" style="background: #0099d4; padding-bottom: 0px; padding-top: 0px;">
                                <div class="row">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="width: 20%; padding-left: 5px;">
                                                <asp:LinkButton ID="btnRegresar" OnClick="btnRegresar_Click" ToolTip="Regresar" Style="padding-left: 10px; padding-right: 10px;" CssClass="btn btn-round" runat="server">
                                                    <asp:Label ForeColor="White" runat="server">
                                                        <i class="material-icons">arrow_back</i>
                                                    </asp:Label>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnFiltros" OnClick="btnFiltros_Click" ToolTip="Filtros de busqueda." BackColor="#4db6ac" class="btn btn-lg btn-fab btn-fab-mini btn-round" runat="server">
                                                        <i class="material-icons">search</i>
                                                </asp:LinkButton>
                                            </td>
                                            <td style="width: 15%; padding-right: 5px;">
                                                <asp:Image ID="imgLogoSelect" Width="110" Height="60" class="img-fluid pull-right" alt="logoEscuela" runat="server" />
                                                <%--<div style="width: 100%;">
                                                    <img src="https://paraisoslatinos.files.wordpress.com/2017/06/fotos-filas-de-moais-chile-isla-pascua-500x3251.jpg" style="width: 30%; margin: 0 auto; display: block;" height="50" width="50" class="img-fluid align-items-center" alt="Responsive image">
                                                </div>--%>
                                            </td>
                                            <td style="width: 45%;">
                                                <h3 class="card-title">
                                                    <asp:Label ID="lblNombreComercial" runat="server" /></h3>
                                            </td>

                                            <td style="width: 20%; padding-right: 5;">
                                                <asp:LinkButton ID="btnActualizarLista" OnClick="btnActualizarLista_Click" ToolTip="Actualizar tabla." class="btn btn-lg btn-success btn-fab btn-fab-mini btn-round pull-right" runat="server">
                                                        <i class="material-icons">sync</i>
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="table-responsive">
                                        <asp:GridView ID="gvPagos" OnPageIndexChanging="gvPagos_PageIndexChanging" OnSorting="gvPagos_Sorting" OnRowCommand="gvPagos_RowCommand" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidFechaColegiatura" GridLines="None" border="0" AllowPaging="true" PageSize="10" runat="server">
                                            <EmptyDataTemplate>
                                                <div class="alert alert-info"><strong>Felicidades</strong> no tiene pagos disponibles.</div>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField SortExpression="VchIdentificador" HeaderText="COLEGIATURA">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtGvIdentificador" ToolTip='<%#Eval("VchIdentificador")%>' Style="width: 100%; text-overflow: ellipsis;" Text='<%#Eval("VchIdentificador")%>' Enabled="false" BackColor="Transparent" BorderStyle="None" runat="server" />
                                                        <asp:TextBox ID="txtGvUidCliente" Text='<%#Eval("UidCliente")%>' Visible="false" runat="server" />
                                                    </ItemTemplate>
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
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <table>
                                                            <tbody>
                                                                <tr style="background: transparent;">
                                                                    <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                        <asp:Panel Visible='<%#Eval("blPagar")%>' runat="server">
                                                                            <asp:LinkButton ID="btnPagar" ToolTip="Pagar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="btnPagar" Style="margin-left: 5px;" runat="server">
                                                                            <asp:Label class="btn btn-sm btn-success btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">attach_money</i>
                                                                            </asp:Label>
                                                                            </asp:LinkButton>
                                                                        </asp:Panel>
                                                                    </td>
                                                                    <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                        <asp:Panel Visible='<%#Eval("blPagar")%>' runat="server">
                                                                            <asp:LinkButton ID="btnFormasPago" ToolTip="Pagar por otro medio" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="btnFormasPago" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label class="btn btn-sm btn-warning btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">add</i>
                                                                                </asp:Label>
                                                                            </asp:LinkButton>
                                                                        </asp:Panel>
                                                                    </td>
                                                                    <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                        <asp:LinkButton ID="btnPagos" ToolTip="Pagos realizados" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="btnPagos" Style="margin-left: 5px;" runat="server">
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
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cBodyBottom" runat="server">
    <!--MODAL-->
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
                                                            <div class="col-12 col-md-12 col-lg-7" style="padding-left: 0px; padding-right: 0px;">
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
                                                            <div class="col-12 col-md-12 col-lg-5" style="padding-left: 0px; padding-right: 0px;">
                                                                <div class="form-group col-md-12" style="padding-bottom: 0px;">
                                                                    <div class="input-group">
                                                                        <div class="input-group-prepend">
                                                                            <asp:Label Text="Fecha de pago:&nbsp;" Font-Bold="true" runat="server" />
                                                                        </div>
                                                                        <asp:Label ID="headFPago" runat="server" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="form-group col-md-4" style="padding-bottom: 0px;">
                                                                <label for="ddlTipoPago" style="color: #ff9800;">Forma de pago</label>
                                                                <div class="input-group">
                                                                    <div class="input-group-prepend">
                                                                        <span class="input-group-text" style="padding-left: 0px;">
                                                                            <i class="material-icons">format_list_numbered</i>
                                                                        </span>
                                                                    </div>
                                                                    <asp:DropDownList ID="ddlTipoPago" OnSelectedIndexChanged="ddlTipoPago_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control" runat="server">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div id="divFormasPago" class="form-group col-md-4" style="padding-bottom: 0px;" runat="server">
                                                                <label for="ddlFormasPago" style="color: #ff9800;">Promoción de pago</label>
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

                                                        <div class="row">
                                                            <div style="display: none;" class="form-group col-md-3">
                                                                <label for="lblVencimiento" style="color: black;">Fecha de Pago</label>
                                                                <div class="input-group">
                                                                    <div class="input-group-prepend">
                                                                        <span class="input-group-text" style="padding-left: 0px;">
                                                                            <i class="material-icons">date_range</i>
                                                                        </span>
                                                                    </div>
                                                                    <asp:Label ID="lblVencimiento" Text="12/09/2020" CssClass="form-control" runat="server" />
                                                                </div>
                                                            </div>
                                                            <div style="display: none;" class="form-group col-md-9">
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
                                                            <div style="display: none;" class="form-group col-md-4">
                                                                <label for="lblImporteCole" style="color: black;">Importe Colegiatura</label>
                                                                <div class="input-group">
                                                                    <div class="input-group-prepend">
                                                                        <span class="input-group-text" style="padding-left: 0px;">
                                                                            <i class="material-icons">attach_money</i>
                                                                        </span>
                                                                    </div>
                                                                    <asp:Label ID="lblImporteCole" Text="0.00" CssClass="form-control" runat="server" />
                                                                </div>
                                                            </div>
                                                            <div style="display: none;" class="form-group col-md-4">
                                                                <label for="lblImporteBeca" style="color: black;">Importe Beca</label>
                                                                <div class="input-group" style="padding-top: 7px;">
                                                                    <div class="input-group-prepend">
                                                                        <span class="input-group-text" style="padding-left: 0px;">
                                                                            <i class="material-icons">attach_money</i>
                                                                        </span>
                                                                    </div>
                                                                    <asp:Label ID="lblImporteBeca" CssClass="form-control" Text="0.00" runat="server" />
                                                                </div>
                                                            </div>
                                                            <div style="display: none;" class="form-group col-md-4">
                                                                <label for="lblTotal" style="color: black;">Importe Recargo</label>
                                                                <div class="input-group">
                                                                    <div class="input-group-prepend">
                                                                        <span class="input-group-text" style="padding-left: 0px;">
                                                                            <i class="material-icons">attach_money</i>
                                                                        </span>
                                                                    </div>
                                                                    <asp:Label ID="lblRecargo" Text="0.00" CssClass="form-control" runat="server" />
                                                                </div>
                                                            </div>
                                                            <div style="display: none;" class="form-group col-md-4" style="display: none;">
                                                                <label for="lblTieneBeca" style="color: black;">¿Tiene beca?</label>
                                                                <asp:Label ID="lblTieneBeca" CssClass="form-control" Text="NO" runat="server" />
                                                            </div>
                                                            <div style="display: none;" class="form-group col-md-4" style="display: none;">
                                                                <label for="lblTipoBeca" style="color: black;">Tipo beca</label>
                                                                <asp:Label ID="lblTipoBeca" CssClass="form-control" Text="CANTIDAD" runat="server" />
                                                            </div>
                                                            <div style="display: none;" class="form-group col-md-4">
                                                                <label for="lblTotal" style="color: black;">Comisión pago en linea</label>
                                                                <div class="input-group" style="padding-top: 7px;">
                                                                    <div class="input-group-prepend">
                                                                        <span class="input-group-text" style="padding-left: 0px;">
                                                                            <i class="material-icons">attach_money</i>
                                                                        </span>
                                                                    </div>
                                                                    <asp:Label ID="lblComisionTarjeta" Text="0.00" CssClass="form-control" runat="server" />
                                                                </div>
                                                            </div>
                                                            <div style="display: none;" class="form-group col-md-4">
                                                                <label for="lblTotal" style="color: black;">Importe Total</label>
                                                                <div class="input-group" style="padding-top: 7px;">
                                                                    <div class="input-group-prepend">
                                                                        <span class="input-group-text" style="padding-left: 0px;">
                                                                            <i class="material-icons">attach_money</i>
                                                                        </span>
                                                                    </div>
                                                                    <asp:Label ID="lblImporteTotal" Text="150.00" CssClass="form-control" runat="server" />
                                                                </div>
                                                            </div>

                                                            <div style="display: none;" class="form-group col-md-4">
                                                                <label for="lblTotal" style="color: black;">Comisión promoción</label>
                                                                <div class="input-group" style="padding-top: 7px;">
                                                                    <div class="input-group-prepend">
                                                                        <span class="input-group-text" style="padding-left: 0px;">
                                                                            <i class="material-icons">attach_money</i>
                                                                        </span>
                                                                    </div>
                                                                    <asp:Label ID="lblComisionPromocion" Text="150.00" CssClass="form-control" runat="server" />
                                                                </div>
                                                            </div>
                                                            <div style="display: none;" class="form-group col-md-4">
                                                                <label for="lblTotal" style="color: black;">Total a pagar</label>
                                                                <div class="input-group" style="padding-top: 7px;">
                                                                    <div class="input-group-prepend">
                                                                        <span class="input-group-text" style="padding-left: 0px;">
                                                                            <i class="material-icons">attach_money</i>
                                                                        </span>
                                                                    </div>
                                                                    <asp:Label ID="lblTotalPagar" Text="150.00" CssClass="form-control" runat="server" />
                                                                </div>
                                                            </div>

                                                            <table class="table" style="margin-top: 16px;">
                                                                <thead>
                                                                    <tr>
                                                                        <th style="background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px;" class="text-center">N°</th>
                                                                        <th style="background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px;">CONCEPTO</th>
                                                                        <th style="background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px;" class="text-right">IMPORTE</th>
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
                                                                    <tr id="trValidarImporte" runat="server">
                                                                        <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" class="text-center"></td>
                                                                        <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" class="text-right">Importe por validar:</td>
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
                                            <iframe id="ifrLiga" style="width: 80%; margin: 0 auto; display: block;" width="450px" height="650px" class="centrado" src="https://wppsandbox.mit.com.mx/i/SNDBX001" frameborder="0" seamless="seamless" runat="server"></iframe>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlValidar" Visible="false" runat="server">
                                    <asp:Timer ID="tmValidar" OnTick="tmValidar_Tick" runat="server" Interval="1000" />
                                    <asp:UpdatePanel ID="upValidar" UpdateMode="Conditional" runat="server">
                                        <ContentTemplate>
                                            <div style="height: 100%; width: 100%; display: flex; justify-content: center; align-items: center;">
                                                <div>
                                                    <img height="150" width="150" src="../Images/loaderEscuela.gif" alt="imgPagaLaEscuela" />
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
                <%--<div class="modal-footer justify-content-center">
                </div>--%>
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
                            <asp:LinkButton ID="btnCancelarModalTipoPago" OnClientClick="hideModalTipoPago();" CssClass="close" runat="server">
                            <span aria-hidden="true">&times;</span>
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
                                                                            <label for="txtCuenta" style="color: black;">Ultimos 4 digitos de la cuenta</label>
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
                                                    <asp:Panel ID="Panel1" runat="server">
                                                        <div style="position: absolute; width: 100%; height: 50%; background-color: #b62322; left: 0px;"></div>
                                                        <div class="row">
                                                            <div style="width: 100%;">
                                                                <div style="width: 80%; margin: 0 auto; display: block;">
                                                                    <div class="card">
                                                                        <div class="card-body">
                                                                            <asp:UpdatePanel runat="server">
                                                                                <ContentTemplate>
                                                                                    <asp:Panel ID="pnlAlertPago2" Visible="false" runat="server">
                                                                                        <div id="divAlertPago2" class="alert alert-danger alert-dismissible fade" role="alert" runat="server">
                                                                                            <asp:Label ID="lblMensajeAlertPago2" runat="server" />
                                                                                            <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                                        </div>
                                                                                    </asp:Panel>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>


                                                                            <div class="row" style="padding-top: 10px;">
                                                                                <div class="col-12 col-md-12 col-lg-6">
                                                                                    <asp:Image ID="imgLogoSelect3" Height="100" Width="150" class="img-fluid" alt="logoEscuela" runat="server" />
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
                                                                                        <tr id="trValidarImporte2" runat="server">
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" class="text-center"></td>
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" class="text-right">Importe por validar:
                                                                                            </td>
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" class="text-right">
                                                                                                <asp:Label ID="lblValidarImportetb2" Style="color: #f55145;" runat="server" />
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
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" class="text-right">Importe a pagar:
                                                                                            </td>
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" class="text-right">
                                                                                                <asp:Label Text="$" Font-Bold="true" Font-Size="Medium" runat="server" />
                                                                                                <asp:Label ID="lblImportePagartb" Text="0.00" Font-Bold="true" Font-Size="Medium" runat="server" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" class="text-center"></td>
                                                                                            <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" class="text-right">Resta:</td>
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

    <div id="ModalPagos" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="Label1" Text="Pago(s) de la colegiatura" runat="server" /></h5>
                            <asp:LinkButton ID="LinkButton3" OnClientClick="hideModalPagos();" aria-label="Close" CssClass="close" runat="server">
                            <span aria-hidden="true">&times;</span>
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="modal-body pt-0" style="padding-bottom: 0px;">
                    <div class="tab-content">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlAlertModalPagos" Visible="false" runat="server">
                                    <div id="divAlertModalPagos" class="alert alert-danger alert-dismissible fade" role="alert" runat="server">
                                        <asp:Label ID="lblMensajeAlertModalPagos" runat="server" />
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
                                                                                    <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                                        <asp:Panel Visible='<%#Eval("blMostrarRefClub")%>' runat="server">
                                                                                            <asp:LinkButton ID="btnReferenciaCP" ToolTip="Imprimir referencia" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="btnReferenciaCP" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label class="btn btn-sm btn-fab btn-fab-mini btn-round" style="background-color:black;" runat="server">
                                                                                        <i class="material-icons">picture_as_pdf</i>
                                                                                </asp:Label>
                                                                                            </asp:LinkButton>
                                                                                        </asp:Panel>
                                                                                    </td>
                                                                                    <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                                        <asp:Panel Visible='<%#Eval("blCancelRefClub")%>' runat="server">
                                                                                            <asp:LinkButton ID="btnCancelarRef" ToolTip="Cancelar Referencia" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="btnCancelarRef" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label class="btn btn-sm btn-danger btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">cancel_presentation</i>
                                                                                </asp:Label>
                                                                                            </asp:LinkButton>
                                                                                        </asp:Panel>
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
                                <asp:Label ID="Label2" Text="Detalle del pago" runat="server" /></h5>
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
    <div id="ModalPagoDetalleClubPago" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header" style="padding-top: 0px; padding-bottom: 0px;">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="Label3" Text="Detalle del pago" runat="server" /></h5>
                            <asp:LinkButton ID="LinkButton4" data-dismiss="modal" aria-label="Close" CssClass="close" runat="server">
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
                                                                <td align="center" valign="top" style="padding-top: 20px;"></td>
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
                                                    <div class="form-group col-md-9">
                                                        <label for="txtColegiatura" style="margin-left: 15px; color: black;">Colegiatura</label>
                                                        <asp:TextBox ID="txtColegiatura" CssClass="form-control" aria-label="Search" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-3">
                                                        <label for="txtNumPago" style="margin-left: 15px; color: black;"># de pago</label>
                                                        <asp:TextBox ID="txtNumPago" CssClass="form-control" TextMode="Number" aria-label="Search" runat="server" />
                                                        <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" TargetControlID="txtNumPago" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-3">
                                                        <label for="ddlEstatusCole" style="color: black;">Estatus</label>
                                                        <asp:DropDownList ID="ddlEstatusCole" AppendDataBoundItems="true" CssClass="form-control" Style="margin-top: 6px;" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-md-3">
                                                        <label for="ddlEstatusPago" style="color: black;">Pago</label>
                                                        <asp:DropDownList ID="ddlEstatusPago" AppendDataBoundItems="true" CssClass="form-control" Style="margin-top: 6px;" runat="server">
                                                        </asp:DropDownList>
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
    <!--END MODAL-->

    <script>
        function showModalPagar() {
            $('#ModalPagar').modal({ backdrop: 'static', keyboard: false, show: true });
        }
        function hideModalPagar() {
            $('#ModalPagar').modal('hide');
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
            hideModalDialog();
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

        function showModalPagos() {
            $('#ModalPagos').modal('show');
        }
        function hideModalPagos() {
            $('#ModalPagos').modal('hide');
        }

        function showModalDetalleCole() {
            $('#ModalDetalleCole').modal('show');
        }
        function hideModalDetalleCole() {
            $('#ModalDetalleCole').modal('hide');
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
</asp:Content>
