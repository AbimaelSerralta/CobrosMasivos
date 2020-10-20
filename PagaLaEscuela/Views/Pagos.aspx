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
    <style>
        .cardEfe {
            transition: .5s;
        }

            .cardEfe:hover {
                transform: scale(1.05);
            }

        .h-100 {
            height: 90% !important;
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
                                                <asp:LinkButton ID="btnRegresar" OnClick="btnRegresar_Click" Style="padding-left: 10px; padding-right: 10px;" CssClass="btn btn-round" runat="server">
                                                    <asp:Label ForeColor="White" runat="server">
                                                        <i class="material-icons">arrow_back</i> Regresar
                                                    </asp:Label>
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
                                                <div class="alert alert-info">No hay nada por pagar</div>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField SortExpression="VchIdentificador" HeaderText="COLEGIATURA">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtGvCorreo" ToolTip='<%#Eval("VchIdentificador")%>' Style="width: 100%; text-overflow: ellipsis;" Text='<%#Eval("VchIdentificador")%>' Enabled="false" BackColor="Transparent" BorderStyle="None" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField SortExpression="VchMatricula" DataField="VchMatricula" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="MATRICULA" />
                                                <asp:BoundField SortExpression="NombreCompleto" DataField="NombreCompleto" HeaderStyle-CssClass="text-center" HeaderText="ALUMNO" />
                                                <asp:BoundField SortExpression="VchNum" DataField="VchNum" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="# DE PAGOS" />
                                                <asp:BoundField SortExpression="DcmImporte" DataField="DcmImporte" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" DataFormatString="{0:C}" HeaderText="IMPORTE" />
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
                                                                    <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                        <asp:Panel Visible='<%#Eval("blPagar")%>' runat="server">
                                                                            <asp:LinkButton ID="btnPagar" ToolTip="Pagar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="btnPagar" Style="margin-left: 5px;" runat="server">
                                                                            <asp:Label class="btn btn-sm btn-warning btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">attach_money</i>
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
                                                            <div class="col-12 col-md-12 col-lg-8">
                                                                <div class="form-group col-md-12">
                                                                    <div class="input-group">
                                                                        <div class="input-group-prepend">
                                                                            <asp:Label Text="Alumno:&nbsp;" Font-Bold="true" runat="server" />
                                                                        </div>
                                                                        <asp:Label ID="headAlumno" runat="server" />
                                                                    </div>
                                                                </div>
                                                                <div class="form-group col-md-12">
                                                                    <div class="input-group">
                                                                        <div class="input-group-prepend">
                                                                            <asp:Label Text="Matricula:&nbsp;" Font-Bold="true" runat="server" />
                                                                        </div>
                                                                        <asp:Label ID="headMatricula" runat="server" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-12 col-md-12 col-lg-4">
                                                                <div class="form-group col-md-12">
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
                                                            </div>
                                                            <div class="form-group col-md-12">
                                                                <div class="input-group">
                                                                    <div class="input-group-prepend">
                                                                        <asp:Label Text="Fecha de pago:&nbsp;" Font-Bold="true" runat="server" />
                                                                    </div>
                                                                    <asp:Label ID="headFPago" runat="server" />
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
                                                                    <tr id="trSubtotal" runat="server">
                                                                        <td style="padding-bottom: 0px;" class="text-center"></td>
                                                                        <td style="font-weight: bold; padding-top: 0px; padding-bottom: 0px;" class="text-right">Subtotal:</td>
                                                                        <td style="font-weight: bold; padding-top: 0px; padding-bottom: 0px;" class="text-right">
                                                                            <asp:Label ID="lblSubtotaltb" runat="server" />
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
                                                                                <span class="tiptext" style="width:230px;"><asp:Label ID="lblToolApagar" Text="A pagar" runat="server" /></span>
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
    <!--END MODAL-->

    <script>
        function showModalPagar() {
            $('#ModalPagar').modal({ backdrop: 'static', keyboard: false, show: true });
        }

        function hideModalPagar() {
            $('#ModalPagar').modal('hide');
        }
    </script>
</asp:Content>
