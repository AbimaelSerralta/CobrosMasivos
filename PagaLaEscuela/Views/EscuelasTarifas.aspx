<%@ Page Title="Tarifas" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="EscuelasTarifas.aspx.cs" Inherits="PagaLaEscuela.Views.EscuelasTarifas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHCaja" runat="server">
    <style>
        .form-check,
        label {
            font-size: 14px;
            line-height: 1.42857;
            color: #333333;
            font-weight: 400;
            padding-left: 5px;
            padding-right: 20px;
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

    <div class="content">
        <div class="container-fluid">
            <div class="row">
                <div class="card card-nav-tabs">
                    <div class="card-header card-header-primary" style="background: #326497;">
                        <!-- colors: "header-primary", "header-info", "header-success", "header-warning", "header-danger" -->
                        <div class="nav-tabs-navigation">
                            <div class="nav-tabs-wrapper">
                                <ul class="nav nav-tabs" id="ulTabColegiatura" data-tabs="tabs">
                                    <li class="nav-item">
                                        <a class="nav-link active show" href="#pagosEnlinea" data-toggle="tab">
                                            <i class="material-icons">payment</i>Pago en linea<div class="ripple-container"></div>
                                        </a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" href="#promociones" data-toggle="tab">
                                            <i class="material-icons">list</i>Promociones<div class="ripple-container"></div>
                                        </a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" href="#comision" data-toggle="tab">
                                            <i class="material-icons">credit_card</i>Comisión<div class="ripple-container"></div>
                                        </a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" href="#terminal" data-toggle="tab">
                                            <i class="material-icons">phonelink_ring</i>Terminal bancaria<div class="ripple-container"></div>
                                        </a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" href="#pagoenefectivo" data-toggle="tab">
                                            <i class="material-icons">attach_money</i>Pago en efectivo<div class="ripple-container"></div>
                                        </a>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>

                    <div class="card-body">
                        <div class="tab-content">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:Label CssClass="text-danger" runat="server" ID="Label2" />
                                    <asp:Panel ID="pnlAlertMnsjEstatus" Visible="false" runat="server">
                                        <div id="divAlertMnsjEstatus" class="alert alert-danger alert-dismissible fade" role="alert" runat="server">
                                            <asp:Label runat="server" ID="lblMnsjEstatus" />
                                            <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                        </div>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <div class="tab-pane active show" id="pagosEnlinea">
                                <asp:GridView ID="gvTipoTarjetaPraga" OnRowDataBound="gvTipoTarjetaPraga_RowDataBound" Width="100%" ShowHeader="false" GridLines="None" AutoGenerateColumns="false" runat="server">
                                    <Columns>
                                        <asp:BoundField DataField="UidTipoTarjeta" ItemStyle-CssClass="d-none" HeaderStyle-CssClass="d-none" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <div class="row">
                                                    <div class="accordionTipoTarjeta" style="margin-top: 15px; margin-bottom: 0px; border-left: 8px solid <%#Eval("VchColor")%>;">
                                                        <asp:UpdatePanel runat="server">
                                                            <ContentTemplate>
                                                                <div>
                                                                    <label style="font-size: 1.0625rem; font-weight: bold; color: black;">
                                                                        <div class="row">
                                                                            <div class="col-sm-2">
                                                                                <img src="../Images/TipoTarjeta/<%#Eval("VchImagen")%>" style="background-color: #d8ecfe" height="70" width="70" class="float-left rounded-circle">
                                                                            </div>
                                                                            <div class="col-sm-5">
                                                                                <h5 class="card-title" style="padding-top: 15px;">
                                                                                    <asp:CheckBox ID="cbComisionPraga" Font-Bold="true" AutoPostBack="true" Text='<%#Eval("VchDescripcion")%>' runat="server" />
                                                                                </h5>
                                                                            </div>
                                                                            <div class="col-sm-5">
                                                                                <div class="form-group col-md-12" style="padding-left: 0px;">
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtComisionTipoTarjetaPraga" Text="0.00" CssClass="form-control" TextMode="Phone" Font-Size="Large" runat="server" />
                                                                                        <div class="input-group-prepend">
                                                                                            <span class="input-group-text" style="padding-left: 0px; padding-right: 5px;">
                                                                                                <i class="material-icons">%</i>
                                                                                            </span>
                                                                                        </div>
                                                                                    </div>

                                                                                    <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtComisionTipoTarjetaPraga" runat="server" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </label>
                                                                </div>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="cbComisionPraga" EventName="CheckedChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="panelTipoTarjeta">
                                                        <asp:UpdatePanel runat="server">
                                                            <ContentTemplate>
                                                                <div class="row">
                                                                    <div class="card" style="margin-top: 0px; margin-bottom: 0px; border-left: 8px solid <%#Eval("VchColor")%>;">
                                                                        <div class="card-body">
                                                                            <div class="row">
                                                                                <div class="table-responsive">
                                                                                    <asp:GridView ID="gvPromocionesPraga" CssClass="table" GridLines="None" border="0" OnRowDataBound="gvPromocionesPraga_RowDataBound" AutoGenerateColumns="false" DataKeyNames="UidPromocion" runat="server">
                                                                                        <EmptyDataTemplate>
                                                                                            <div class="alert alert-info">No hay promociones disponibles</div>
                                                                                        </EmptyDataTemplate>
                                                                                        <Columns>
                                                                                            <asp:BoundField DataField="UidPromocion" ItemStyle-CssClass="d-none" HeaderStyle-CssClass="d-none" />
                                                                                            <asp:TemplateField HeaderStyle-CssClass="d-none">
                                                                                                <ItemTemplate>
                                                                                                    <asp:UpdatePanel runat="server">
                                                                                                        <ContentTemplate>
                                                                                                            <table>
                                                                                                                <tbody>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 30%;">
                                                                                                                            <asp:CheckBox ID="cbPromocionPraga" Checked='<%#Eval("blChecked")%>' OnCheckedChanged="cbPromocionPraga_CheckedChanged" AutoPostBack="true" Text='<%#Eval("VchDescripcion")%>' runat="server" />
                                                                                                                        </td>
                                                                                                                        <td style="width: 35%;">
                                                                                                                            <div class="input-group">
                                                                                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtComisionPraga" runat="server" />
                                                                                                                                <asp:TextBox ID="txtComisionPraga" Text='<%#Eval("DcmComicion")%>' Font-Size="Large" CssClass="form-control text-right" placeholder="123...100" runat="server" />
                                                                                                                                <div class="input-group-prepend">
                                                                                                                                    <span class="input-group-text">
                                                                                                                                        <i class="material-icons">%</i>
                                                                                                                                    </span>
                                                                                                                                </div>
                                                                                                                            </div>
                                                                                                                        </td>
                                                                                                                        <td style="width: 35%;">
                                                                                                                            <div class="input-group">
                                                                                                                                <div class="input-group-prepend">
                                                                                                                                    <span class="input-group-text">
                                                                                                                                        <i class="material-icons">$</i>
                                                                                                                                    </span>
                                                                                                                                </div>
                                                                                                                                <asp:TextBox ID="txtApartirDePraga" Text='<%#Eval("DcmApartirDe")%>' ToolTip="A partir del monto ingresado se activará esta promoción." Font-Size="Large" CssClass="form-control text-right" placeholder="A partir de" runat="server" />
                                                                                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtApartirDePraga" runat="server" />
                                                                                                                            </div>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </tbody>
                                                                                                            </table>
                                                                                                        </ContentTemplate>
                                                                                                        <Triggers>
                                                                                                            <asp:AsyncPostBackTrigger ControlID="cbPromocionPraga" EventName="CheckedChanged" />
                                                                                                        </Triggers>
                                                                                                    </asp:UpdatePanel>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                        </Columns>
                                                                                    </asp:GridView>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <div class="modal-footer justify-content-center">
                                            <asp:LinkButton ID="btnGuardarPromocionesPraga" CssClass="btn btn-success btn-round" runat="server">
                                                                <i class="material-icons">check</i> Guardar
                                            </asp:LinkButton>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                            <div class="tab-pane" id="promociones">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="form-group col-md-12">
                                                <div class="row">
                                                    <div class="table-responsive">
                                                        <asp:GridView ID="gvPromociones" CssClass="table table-hover" GridLines="None" border="0" OnRowDataBound="gvPromociones_RowDataBound" AutoGenerateColumns="false" DataKeyNames="UidPromocion" runat="server">
                                                            <EmptyDataTemplate>
                                                                <div class="alert alert-info">No hay promociones disponibles</div>
                                                            </EmptyDataTemplate>
                                                            <Columns>
                                                                <asp:BoundField DataField="UidPromocion" ItemStyle-CssClass="d-none" HeaderStyle-CssClass="d-none" />
                                                                <asp:TemplateField HeaderStyle-CssClass="d-none">
                                                                    <ItemTemplate>
                                                                        <table>
                                                                            <tr>
                                                                                <td style="vertical-align: bottom;">
                                                                                    <asp:CheckBox ID="cbPromocion" OnCheckedChanged="cbPromocion_CheckedChanged" AutoPostBack="true" Text='<%#Eval("VchDescripcion")%>' runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-CssClass="d-none">
                                                                    <ItemTemplate>
                                                                        <table>
                                                                            <tr style="background: transparent;">
                                                                                <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                                    <div class="form-row">
                                                                                        <div class="col-6 col-sm-3">
                                                                                            <div class="input-group">
                                                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtComision" runat="server" />
                                                                                                <asp:TextBox ID="txtComision" Font-Size="Large" CssClass="form-control text-right" placeholder="123...100" runat="server" />
                                                                                                <div class="input-group-prepend">
                                                                                                    <span class="input-group-text">
                                                                                                        <i class="material-icons">%</i>
                                                                                                    </span>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col-6 col-sm-3">
                                                                                            <div class="input-group">
                                                                                                <div class="input-group-prepend">
                                                                                                    <span class="input-group-text">
                                                                                                        <i class="material-icons">$</i>
                                                                                                    </span>
                                                                                                </div>
                                                                                                <asp:TextBox ID="txtApartirDe" ToolTip="A partir del monto ingresado se activará esta promoción." Font-Size="Large" CssClass="form-control text-right" placeholder="A partir de" runat="server" />
                                                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtApartirDe" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <div class="modal-footer justify-content-center">
                                                    <asp:LinkButton ID="btnGuardarPromociones" OnClick="btnGuardarPromociones_Click" CssClass="btn btn-success btn-round" runat="server">
                                                                <i class="material-icons">check</i> Guardar
                                                    </asp:LinkButton>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                            <div class="tab-pane" id="comision">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="form-group col-sm-12 col-md-12 col-lg-6">
                                                <div class="card">
                                                    <div class="card-body">
                                                        <div>
                                                            <h4 style="margin-top: 0px; font-weight: bold;">Cobro en linea</h4>
                                                            <div class="row">
                                                                <div class="col-4">
                                                                    <img class="card-img-top" style="height: 100px; width: 100px" src="../Images/logoPagaLaEscuela.png" alt="porcentaje">
                                                                </div>
                                                                <div class="col-8">
                                                                    <h5 class="card-title">
                                                                        <asp:CheckBox ID="cbActivarComision" OnCheckedChanged="cbActivarComision_CheckedChanged" AutoPostBack="true" Text="Agregar comisión" runat="server" /></h5>
                                                                    <div class="form-group col-md-12" style="padding-left: 0px;">
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtComisionTarjeta" Text="0.00" CssClass="form-control" TextMode="Phone" Font-Size="Large" runat="server" />
                                                                            <div class="input-group-prepend">
                                                                                <span class="input-group-text" style="padding-left: 0px; padding-right: 5px;">
                                                                                    <i class="material-icons">%</i>
                                                                                </span>
                                                                            </div>
                                                                        </div>

                                                                        <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtComisionTarjeta" runat="server" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <div class="modal-footer justify-content-center">
                                                    <asp:LinkButton ID="btnGuardarComision" OnClick="btnGuardarComision_Click" CssClass="btn btn-success btn-round" runat="server">
                                                                <i class="material-icons">check</i> Guardar
                                                    </asp:LinkButton>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                            <div class="tab-pane" id="terminal">
                                <asp:GridView ID="gvTipoTarjeta" OnRowDataBound="gvTipoTarjeta_RowDataBound" Width="100%" ShowHeader="false" GridLines="None" AutoGenerateColumns="false" runat="server">
                                    <Columns>
                                        <asp:BoundField DataField="UidTipoTarjeta" ItemStyle-CssClass="d-none" HeaderStyle-CssClass="d-none" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <div class="row">
                                                    <div class="accordionTipoTarjeta" style="margin-top: 15px; margin-bottom: 0px; border-left: 8px solid <%#Eval("VchColor")%>;">
                                                        <asp:UpdatePanel runat="server">
                                                            <ContentTemplate>
                                                                <div>
                                                                    <label style="font-size: 1.0625rem; font-weight: bold; color: black;">
                                                                        <div class="row">
                                                                            <div class="col-sm-2">
                                                                                <img src="../Images/TipoTarjeta/<%#Eval("VchImagen")%>" style="background-color: #d8ecfe" height="70" width="70" class="float-left rounded-circle">
                                                                            </div>
                                                                            <div class="col-sm-5">
                                                                                <h5 class="card-title" style="padding-top: 15px;">
                                                                                    <asp:CheckBox ID="cbComisionTerminal" Font-Bold="true" AutoPostBack="true" Text='<%#Eval("VchDescripcion")%>' runat="server" />
                                                                                </h5>
                                                                            </div>
                                                                            <div class="col-sm-5">
                                                                                <div class="form-group col-md-12" style="padding-left: 0px;">
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtComisionTipoTarjeta" Text="0.00" CssClass="form-control" TextMode="Phone" Font-Size="Large" runat="server" />
                                                                                        <div class="input-group-prepend">
                                                                                            <span class="input-group-text" style="padding-left: 0px; padding-right: 5px;">
                                                                                                <i class="material-icons">%</i>
                                                                                            </span>
                                                                                        </div>
                                                                                    </div>

                                                                                    <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtComisionTipoTarjeta" runat="server" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </label>
                                                                </div>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="cbComisionTerminal" EventName="CheckedChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="panelTipoTarjeta">
                                                        <asp:UpdatePanel runat="server">
                                                            <ContentTemplate>
                                                                <div class="row">
                                                                    <div class="card" style="margin-top: 0px; margin-bottom: 0px; border-left: 8px solid <%#Eval("VchColor")%>;">
                                                                        <div class="card-body">
                                                                            <div class="row">
                                                                                <div class="table-responsive">
                                                                                    <asp:GridView ID="gvPromocionesTerminal" CssClass="table" GridLines="None" border="0" OnRowDataBound="gvPromocionesTerminal_RowDataBound" AutoGenerateColumns="false" DataKeyNames="UidPromocionTerminal" runat="server">
                                                                                        <EmptyDataTemplate>
                                                                                            <div class="alert alert-info">No hay promociones disponibles</div>
                                                                                        </EmptyDataTemplate>
                                                                                        <Columns>
                                                                                            <asp:BoundField DataField="UidPromocionTerminal" ItemStyle-CssClass="d-none" HeaderStyle-CssClass="d-none" />
                                                                                            <asp:TemplateField HeaderStyle-CssClass="d-none">
                                                                                                <ItemTemplate>
                                                                                                    <asp:UpdatePanel runat="server">
                                                                                                        <ContentTemplate>
                                                                                                            <table>
                                                                                                                <tbody>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 30%;">
                                                                                                                            <asp:CheckBox ID="cbPromocionTerminal" Checked='<%#Eval("blChecked")%>' OnCheckedChanged="cbPromocionTerminal_CheckedChanged" AutoPostBack="true" Text='<%#Eval("VchDescripcion")%>' runat="server" />
                                                                                                                        </td>
                                                                                                                        <td style="width: 35%;">
                                                                                                                            <div class="input-group">
                                                                                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtComisionTerminal" runat="server" />
                                                                                                                                <asp:TextBox ID="txtComisionTerminal" Text='<%#Eval("DcmComicion")%>' Font-Size="Large" CssClass="form-control text-right" placeholder="123...100" runat="server" />
                                                                                                                                <div class="input-group-prepend">
                                                                                                                                    <span class="input-group-text">
                                                                                                                                        <i class="material-icons">%</i>
                                                                                                                                    </span>
                                                                                                                                </div>
                                                                                                                            </div>
                                                                                                                        </td>
                                                                                                                        <td style="width: 35%;">
                                                                                                                            <div class="input-group">
                                                                                                                                <div class="input-group-prepend">
                                                                                                                                    <span class="input-group-text">
                                                                                                                                        <i class="material-icons">$</i>
                                                                                                                                    </span>
                                                                                                                                </div>
                                                                                                                                <asp:TextBox ID="txtApartirDeTerminal" Text='<%#Eval("DcmApartirDe")%>' ToolTip="A partir del monto ingresado se activará esta promoción." Font-Size="Large" CssClass="form-control text-right" placeholder="A partir de" runat="server" />
                                                                                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtApartirDeTerminal" runat="server" />
                                                                                                                            </div>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </tbody>
                                                                                                            </table>
                                                                                                        </ContentTemplate>
                                                                                                        <Triggers>
                                                                                                            <asp:AsyncPostBackTrigger ControlID="cbPromocionTerminal" EventName="CheckedChanged" />
                                                                                                        </Triggers>
                                                                                                    </asp:UpdatePanel>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                        </Columns>
                                                                                    </asp:GridView>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <div class="modal-footer justify-content-center">
                                            <asp:LinkButton ID="btnGuardarPromocionesTerminal" OnClick="btnGuardarPromocionesTerminal_Click" CssClass="btn btn-success btn-round" runat="server">
                                                                <i class="material-icons">check</i> Guardar
                                            </asp:LinkButton>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                            <div class="tab-pane" id="pagoenefectivo">
                                <asp:GridView ID="gvTipoTarjetaClubPago" OnRowDataBound="gvTipoTarjetaClubPago_RowDataBound" Width="100%" ShowHeader="false" GridLines="None" AutoGenerateColumns="false" runat="server">
                                    <Columns>
                                        <asp:BoundField DataField="UidTipoTarjeta" ItemStyle-CssClass="d-none" HeaderStyle-CssClass="d-none" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <div class="row">
                                                    <div class="accordionTipoTarjeta" style="margin-top: 15px; margin-bottom: 0px; border-left: 8px solid <%#Eval("VchColor")%>;">
                                                        <asp:UpdatePanel runat="server">
                                                            <ContentTemplate>
                                                                <div>
                                                                    <label style="font-size: 1.0625rem; font-weight: bold; color: black;">
                                                                        <div class="row">
                                                                            <div class="col-sm-2">
                                                                                <img src="../Images/TipoTarjeta/<%#Eval("VchImagen")%>" style="background-color: #d8ecfe" height="70" width="70" class="float-left rounded-circle">
                                                                            </div>
                                                                            <div class="col-sm-5">
                                                                                <h5 class="card-title" style="padding-top: 15px;">
                                                                                    <asp:CheckBox ID="cbComisionClubPago" Font-Bold="true" AutoPostBack="true" Text='<%#Eval("VchDescripcion")%>' runat="server" />
                                                                                </h5>
                                                                            </div>
                                                                            <div class="col-sm-5">
                                                                                <div class="form-group col-md-12" style="padding-left: 0px;">
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtComisionTipoTarjetaClubPago" Text="0.00" CssClass="form-control" TextMode="Phone" Font-Size="Large" runat="server" />
                                                                                        <div class="input-group-prepend">
                                                                                            <span class="input-group-text" style="padding-left: 0px; padding-right: 5px;">
                                                                                                <i class="material-icons">%</i>
                                                                                            </span>
                                                                                        </div>
                                                                                    </div>

                                                                                    <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtComisionTipoTarjetaClubPago" runat="server" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </label>
                                                                </div>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="cbComisionClubPago" EventName="CheckedChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="panelTipoTarjeta">
                                                        <asp:UpdatePanel runat="server">
                                                            <ContentTemplate>
                                                                <div class="row">
                                                                    <div class="card" style="margin-top: 0px; margin-bottom: 0px; border-left: 8px solid <%#Eval("VchColor")%>;">
                                                                        <div class="card-body">
                                                                            <div class="row">
                                                                                <div class="table-responsive">
                                                                                    <asp:GridView ID="gvPromocionesClubPago" CssClass="table" GridLines="None" border="0" OnRowDataBound="gvPromocionesClubPago_RowDataBound" AutoGenerateColumns="false" DataKeyNames="UidPromocionTerminal" runat="server">
                                                                                        <EmptyDataTemplate>
                                                                                            <div class="alert alert-info">No hay promociones disponibles</div>
                                                                                        </EmptyDataTemplate>
                                                                                        <Columns>
                                                                                            <asp:BoundField DataField="UidPromocionTerminal" ItemStyle-CssClass="d-none" HeaderStyle-CssClass="d-none" />
                                                                                            <asp:TemplateField HeaderStyle-CssClass="d-none">
                                                                                                <ItemTemplate>
                                                                                                    <asp:UpdatePanel runat="server">
                                                                                                        <ContentTemplate>
                                                                                                            <table>
                                                                                                                <tbody>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 30%;">
                                                                                                                            <asp:CheckBox ID="cbPromocionClubPago" Checked='<%#Eval("blChecked")%>' OnCheckedChanged="cbPromocionClubPago_CheckedChanged" AutoPostBack="true" Text='<%#Eval("VchDescripcion")%>' runat="server" />
                                                                                                                        </td>
                                                                                                                        <td style="width: 35%;">
                                                                                                                            <div class="input-group">
                                                                                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtComisionTerminalPagoEnLinea" runat="server" />
                                                                                                                                <asp:TextBox ID="txtComisionClubPago" Text='<%#Eval("DcmComicion")%>' Font-Size="Large" CssClass="form-control text-right" placeholder="123...100" runat="server" />
                                                                                                                                <div class="input-group-prepend">
                                                                                                                                    <span class="input-group-text">
                                                                                                                                        <i class="material-icons">%</i>
                                                                                                                                    </span>
                                                                                                                                </div>
                                                                                                                            </div>
                                                                                                                        </td>
                                                                                                                        <td style="width: 35%;">
                                                                                                                            <div class="input-group">
                                                                                                                                <div class="input-group-prepend">
                                                                                                                                    <span class="input-group-text">
                                                                                                                                        <i class="material-icons">$</i>
                                                                                                                                    </span>
                                                                                                                                </div>
                                                                                                                                <asp:TextBox ID="txtApartirDeClubPago" Text='<%#Eval("DcmApartirDe")%>' ToolTip="A partir del monto ingresado se activará esta promoción." Font-Size="Large" CssClass="form-control text-right" placeholder="A partir de" runat="server" />
                                                                                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtApartirDeTerminalPagoEnLinea" runat="server" />
                                                                                                                            </div>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </tbody>
                                                                                                            </table>
                                                                                                        </ContentTemplate>
                                                                                                        <Triggers>
                                                                                                            <asp:AsyncPostBackTrigger ControlID="cbPromocionClubPago" EventName="CheckedChanged" />
                                                                                                        </Triggers>
                                                                                                    </asp:UpdatePanel>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                        </Columns>
                                                                                    </asp:GridView>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <div class="modal-footer justify-content-center">
                                            <asp:LinkButton ID="btnGuardarPromocionesClubPago" OnClick="btnGuardarPromocionesClubPago_Click" CssClass="btn btn-success btn-round" runat="server">
                                                                <i class="material-icons">check</i> Guardar
                                            </asp:LinkButton>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cBodyBottom" runat="server">
    <script>
        var acc = document.getElementsByClassName("accordionTipoTarjeta");
        var i;

        for (i = 0; i < acc.length; i++) {
            acc[i].addEventListener("click", function () {
                this.classList.toggle("activeAccordionTT");
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
