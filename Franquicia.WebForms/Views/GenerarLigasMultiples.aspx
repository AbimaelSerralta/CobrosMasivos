﻿<%@ Page Title="GenerarLigasMultiples" Culture="es-MX" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="GenerarLigasMultiples.aspx.cs" Inherits="Franquicia.WebForms.Views.GenerarLigasMultiples" %>

<%@ MasterType VirtualPath="~/Views/MasterPage.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHCaja" runat="server">

    <%--<style>
        .table .radio, .table .checkbox {
            margin-top: 0;
            margin-bottom: 0;
            padding: 0;
            width: 30px;
        }
    </style>--%>
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
        <contenttemplate>
            <asp:Panel ID="pnlAlertImportarError" Visible="false" runat="server">
                <div id="divAlertImportarError" class="alert alert-danger alert-dismissible fade" role="alert" runat="server">
                    <div class="row">
                        <asp:Label ID="lblMnsjAlertImportarError" Style="margin-top: 5px; margin-left: 15px;" runat="server" />
                        
                        <%--<asp:LinkButton ID="LinkButton1" OnClick="btnDescargarError_Click" ToolTip="Descargar errores." class="btn btn-sm btn-success btn-fab btn-fab-mini btn-round" runat="server">
                                                        <i class="material-icons">file_download</i>
                        </asp:LinkButton>--%>
                        <asp:LinkButton ID="btnDescargarError" Visible="false" OnClick="btnDescargarError_Click" Style="padding-bottom: 5px; padding-top: 5px; padding-right: 5px; padding-left: 5px; margin-top: 0px;" class="btn btn-success" runat="server">Descargar Error</asp:LinkButton>
                        <asp:LinkButton ID="btnMasDetalle" Visible="false" OnClick="btnMasDetalle_Click" Style="padding-bottom: 5px; padding-top: 5px; padding-right: 5px; padding-left: 5px; margin-top: 0px;" class="btn btn-info" runat="server">Más detalle</asp:LinkButton>
                    </div>

                    <asp:LinkButton ID="btnCloseAlertImportarError" OnClick="btnCloseAlertImportarError_Click" class="close" aria-label="Close" runat="server"><span aria-hidden="true">&times;</span></asp:LinkButton>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlAlert" Visible="false" runat="server">
                <div id="divAlert" class="alert alert-danger alert-dismissible fade" role="alert" runat="server">
                    <asp:Label ID="lblMensajeAlert" runat="server" />
                    <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                </div>
            </asp:Panel>
        </contenttemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel runat="server">
        <contenttemplate>
            <asp:FileUpload ID="fuSelecionarExcel" Style="display: none;" runat="server" />

            <script type="text/javascript">
                function UploadFile(fileUpload) {
                    if (fileUpload.value != '') {
                        document.getElementById("<%=btnImportarExcel.ClientID %>").click();
                    }
                }
            </script>
            <asp:Button ID="btnImportarExcel" OnClick="btnImportarExcel_Click" Style="display: none;" Text="Subir" runat="server" />

        </contenttemplate>
        <triggers>

            <asp:PostBackTrigger ControlID="btnImportarExcel" />
        </triggers>
    </asp:UpdatePanel>
    <asp:UpdatePanel runat="server">
        <contenttemplate>
            <div class="content">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-12 col-md-12">
                            <div class="card">
                                <div class="card-header card-header-tabs card-header-primary" style="padding-top: 0px; padding-bottom: 0px;">
                                    <div class="nav-tabs-navigation">
                                        <div class="nav-tabs-wrapper">
                                            <div class="row">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 40%">
                                                            <asp:Label Text="Listado de Usuarios" runat="server" />
                                                        </td>
                                                        <td style="width: 20%">
                                                            <asp:Label ID="lblGvSaldo" CssClass="text-center" runat="server" /></td>
                                                        <td style="width: 40%">
                                                            <div class="pull-right">
                                                                <asp:LinkButton ID="btnCargarExcel" ToolTip="Importar usuarios a excel." class="btn btn-lg btn-ligh btn-fab btn-fab-mini btn-round" runat="server">
                                                        <i class="material-icons">file_upload</i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnExportarLista" OnClick="btnExportarLista_Click" ToolTip="Exportar usuarios de excel." class="btn btn-lg btn-warning btn-fab btn-fab-mini btn-round" runat="server">
                                                        <i class="material-icons">file_download</i>
                                                                </asp:LinkButton>
                                                                ||
                                                    <asp:LinkButton ID="btnReiniciar" ToolTip="Reiniciar todo." OnClick="btnReiniciar_Click" class="btn btn-lg btn-danger btn-fab btn-fab-mini btn-round" runat="server">
                                                        <i class="material-icons">refresh</i>
                                                    </asp:LinkButton>
                                                                <asp:LinkButton ID="btnSeleccionar" OnClick="btnSeleccionar_Click" ToolTip="Agregar usuarios." class="btn btn-lg btn-info btn-fab btn-fab-mini btn-round" runat="server">
                                                        <i class="material-icons">add</i>
                                                                </asp:LinkButton>
                                                                ||
                                                    <asp:LinkButton ID="btnGenerarLigas" OnClick="btnGenerarLigas_Click" ToolTip="Generar ligas." class="btn btn-lg btn-success btn-fab btn-fab-mini btn-round" runat="server">
                                                        <i class="material-icons">link</i>
                                                    </asp:LinkButton>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="card-body">
                                    <div id="divTipoLiga" class="card card-stats bg-light border-success" style="margin-top: 0px;" runat="server">
                                        <div class="row" style="margin-left: 5px; margin-right: 5px; margin-top: 5px;">
                                            <div class="form-group col-md-4" style="padding-left: 0px;">
                                                <label for="txtNombre" style="color: black;">Identificador *</label>
                                                <asp:TextBox ID="txtIdentificador" CssClass="form-control" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvUsuariosSeleccionados" OnRowDataBound="gvUsuariosSeleccionados_RowDataBound" OnSorting="gvUsuariosSeleccionados_Sorting" OnRowCommand="gvUsuariosSeleccionados_RowCommand" AllowSorting="true" AutoGenerateColumns="false" CssClass="table-hover" DataKeyNames="IdUsuario" GridLines="Vertical" border="0" BorderColor="#CFB1AA" AllowPaging="true" PageSize="10" OnPageIndexChanging="gvUsuariosSeleccionados_PageIndexChanging" runat="server">
                                                <EmptyDataTemplate>
                                                    <div class="alert alert-info">No hay usuarios</div>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <table>
                                                                <tbody>
                                                                    <tr style="background: transparent;">
                                                                        <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:CheckBox ID="cbSeleccionadoLote" runat="server" />
                                                                            <asp:label ID="lblGvAuxiliar" Visible="false" Text='<%#Eval("IntAuxiliar")%>' Enabled="false" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-BackColor="#aa46bb" HeaderStyle-ForeColor="White" ItemStyle-Width="200px" SortExpression="NombreCompleto" HeaderText="NOMBRE">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGvNombre" ToolTip='<%#Eval("NombreCompleto")%>' Style="width: 100%;" Text='<%#Eval("NombreCompleto")%>' Enabled="false" BackColor="Transparent" BorderStyle="None" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-BackColor="#aa46bb" HeaderStyle-ForeColor="White" SortExpression="StrCorreo" HeaderText="CORREO">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGvCorreo" ToolTip='<%#Eval("StrCorreo")%>' Style="width: 100%; text-overflow: ellipsis;" Text='<%#Eval("StrCorreo")%>' Enabled="false" BackColor="Transparent" BorderStyle="None" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderStyle-BackColor="#aa46bb" HeaderStyle-ForeColor="White" SortExpression="StrTelefono" DataField="StrTelefono" HeaderText="TELÉFONO" />
                                                    <asp:TemplateField HeaderStyle-BackColor="#aa46bb" HeaderStyle-ForeColor="White" SortExpression="StrAsunto" HeaderText="ASUNTO">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGvAsunto" ToolTip='<%#Eval("StrAsunto")%>' Style="width: 100%" Text='<%#Eval("StrAsunto")%>' Enabled="false" BackColor="Transparent" BorderStyle="None" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-BackColor="#aa46bb" HeaderStyle-ForeColor="White" SortExpression="StrConcepto" HeaderText="CONCEPTO">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGvConcepto" ToolTip='<%#Eval("StrConcepto")%>' Style="width: 100%" Text='<%#Eval("StrConcepto")%>' Enabled="false" BackColor="Transparent" BorderStyle="None" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-BackColor="#aa46bb" HeaderStyle-ForeColor="White" HeaderStyle-CssClass="text-center" SortExpression="DcmImporte" HeaderText="IMPORTE">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGvImporte" CssClass="text-right" ToolTip='<%#Eval("DcmImporte")%>' Style="width: 100%" Text='<%#Eval("DcmImporte", "{0:N2}")%>' Enabled="false" BackColor="Transparent" BorderStyle="None" runat="server" />
                                                            <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtGvImporte" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-BackColor="#aa46bb" HeaderStyle-ForeColor="White" SortExpression="DtVencimiento" HeaderText="VENCIMIENTO">
                                                        <ItemTemplate>
                                                            <%--<asp:TextBox ID="txtGvVencimiento" Style="width: 100%" Text='<%#Eval("DtVencimiento", "{0:yyyy-MM-dd}")%>' TextMode="Date" Enabled="false" BorderStyle="None" runat="server" />--%>
                                                            <asp:TextBox ID="txtGvVencimiento" CssClass="text-center" Style="width: 100%" Text='<%#Eval("DtVencimiento", "{0:dd/MM/yyyy}")%>' Enabled="false" BackColor="Transparent" BorderStyle="None" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-BackColor="#aa46bb" HeaderStyle-ForeColor="#000000" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" HeaderText="EMAIL">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cbGvCorreo" Enabled="false" Checked='<%#Eval("CBCorreo")%>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-BackColor="#aa46bb" HeaderStyle-ForeColor="#000000" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" HeaderText="WHATS">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cbGvWa" Enabled="false" Checked='<%#Eval("CBWhatsApp")%>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-BackColor="#aa46bb" HeaderStyle-ForeColor="#000000" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" HeaderText="SMS">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cbGvSms" Enabled="false" Checked='<%#Eval("CBSms")%>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-CssClass="text-right" HeaderStyle-BackColor="#aa46bb" HeaderStyle-ForeColor="#000000" HeaderStyle-CssClass="text-center" HeaderText="PROMOCIÓN">
                                                        <ItemTemplate>
                                                            <asp:Label ID="GvlblPromociones" Style="width: 100%;" runat="server" />
                                                            <asp:ListBox ID="ListBoxMultiple" Visible="false" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                                            <asp:ListBox ID="GvlbPromociones" Visible="false" style="width: 100%; background-color:transparent; border:none;" CssClass="text-right" Enabled="false" runat="server">
                                                            </asp:ListBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <table>
                                                                <tbody>
                                                                    <tr style="background: transparent;">
                                                                        <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:LinkButton ID="btnEditar" ToolTip="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="btnEditar" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label class="btn btn-sm btn-warning btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">edit</i>
                                                                                </asp:Label>
                                                                            </asp:LinkButton>
                                                                        </td>
                                                                        <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:LinkButton ID="btnEliminar" ToolTip="Eliminar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="btnEliminar" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label class="btn btn-sm btn-danger btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">remove</i>
                                                                                </asp:Label>
                                                                            </asp:LinkButton>
                                                                            <asp:CheckBox ID="cbSeleccionado" CssClass="d-none" OnCheckedChanged="cbSeleccionado_CheckedChanged" AutoPostBack="true" Checked='<%#Eval("blSeleccionado")%>' runat="server" />
                                                                        </td>
                                                                        <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:LinkButton ID="btnAceptar" Visible="false" ToolTip="Aceptar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="btnAceptar" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label class="btn btn-sm btn-success btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">check</i>
                                                                                </asp:Label>
                                                                            </asp:LinkButton>
                                                                        </td>
                                                                        <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:LinkButton ID="btnCancelar" Visible="false" ToolTip="Cancelar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="btnCancelar" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label class="btn btn-sm btn-danger btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">close</i>
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

                                            <asp:GridView ID="gridview" AutoGenerateColumns="false" CssClass="table table-hover" GridLines="None" border="0" runat="server">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <table>
                                                                <tbody>
                                                                    <tr style="background: transparent;">
                                                                        <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:CheckBox ID="cbSeleccionado" OnCheckedChanged="cbSeleccionado_CheckedChanged" AutoPostBack="true" Checked='<%#Eval("blSeleccionado")%>' runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="NombreCompleto" HeaderText="NOMBRE" />
                                                    <asp:BoundField DataField="StrCorreo" HeaderText="CORREO" />
                                                    <asp:BoundField DataField="StrTelefono" HeaderText="TELEFONO" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <div class="form-group col-md-4" visible="false" style="padding-left: 0px;" runat="server">
                                        <label for="txtCadena" style="color: black;">cade</label>
                                        <asp:TextBox ID="txtCadena" CssClass="form-control" runat="server" />
                                    </div>

                                    <asp:LinkButton ID="BTNdEs" Text="dESC" Visible="false" OnClick="BTNdEs_Click" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <div class="row" visible="false" runat="server">
                <%--considerado para borrar--%>
                <div class="row">
                    <div class="col-lg-3 col-md-6 col-sm-6">
                        <asp:LinkButton ID="btnImportarUsuarios" OnClick="btnImportarUsuarios_Click" runat="server">
                    <div class="card card-stats">
                        <div class="card-header card-header-info card-header-icon">
                            <div class="card-icon">
                                <i class="material-icons">file_upload</i>
                            </div>
                            <h3 class="card-title"><strong>Opción 1</strong>
                            </h3>
                        </div>
                        <div class="card-footer">
                            <div class="stats">
                                <h6 style="color: black;">Importe un excel que haya generado anteriormente.</h6>
                            </div>
                        </div>
                    </div>
                        </asp:LinkButton>
                    </div>

                    <div class="col-lg-3 col-md-6 col-sm-6">
                        <asp:LinkButton runat="server">
                    <div class="card card-stats">
                        <div class="card-header card-header-success card-header-icon">
                            <div class="card-icon">
                                <i class="material-icons">playlist_add_check</i>
                            </div>
                            <h3 class="card-title"><strong>Opción 2</strong>
                            </h3>
                        </div>
                        <div class="card-footer">
                            <div class="stats">
                                <h6 style="color: black;">Seleccione los usuarios para generar las ligas.</h6>
                            </div>
                        </div>
                    </div>
                        </asp:LinkButton>
                    </div>
                    <%--considerado para borrar--%>
                </div>
                <div class="form-group col col-sm-6 col-md-6 col-lg-6 col-xl-6">
                    <div class="form-group col-md-12">
                        <label for="ddlUsuario" style="color: black;">Usuarios</label>
                        <asp:DropDownList ID="ddlUsuario" AutoPostBack="true" OnSelectedIndexChanged="ddlUsuario_SelectedIndexChanged" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-md-12">
                        <label for="txtNombre" style="color: black;">Monto</label>
                        <asp:TextBox ID="txtNombre" CssClass="form-control" runat="server" />
                    </div>
                    <div class="form-group col-md-12">
                        <label for="txtCorreo" style="color: black;">Correo</label>
                        <asp:TextBox ID="txtCorreo" CssClass="form-control" runat="server" />
                    </div>
                    <div class="form-group col-md-12">
                        <label for="txtConcepto2" style="color: black;">Concepto</label>
                        <asp:TextBox ID="txtConcepto2" CssClass="form-control" runat="server" />
                    </div>
                    <div class="form-group col-md-12">
                        <label for="txtFecha" style="color: black;">Vencimiento</label>
                        <asp:TextBox ID="txtFecha" TextMode="Date" CssClass="form-control" runat="server" />
                    </div>
                    <div class="form-group col-md-12">
                        <asp:LinkButton ID="btnGenerarLiga" CssClass="btn btn-info pull-right" Text="Generar Liga" OnClick="btnGenerarLiga_Click" runat="server" />
                    </div>
                    <div class="form-group col-md-12">
                        <label for="txtNombre" style="color: black;">Url</label>
                        <asp:TextBox ID="txtUrlGene" CssClass="form-control" runat="server" />
                    </div>
                </div>
                <div class="form-group col col-sm-6 col-md-6 col-lg-6 col-xl-6">
                    <div class="col-md-12">
                        <label for="txtConsultar" style="color: black;">Consultar Estatus de Liga</label>
                        <asp:TextBox ID="txtConsultar" CssClass="form-control" runat="server" />
                        <asp:LinkButton ID="btnConsultar" Text="Consultar" OnClick="btnConsultar_Click" CssClass="btn btn-success pull-right" runat="server" />
                    </div>
                    <asp:GridView ID="gvMovimiento" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidPagoTarjeta" GridLines="None" border="0" runat="server">
                        <EmptyDataTemplate>
                            <div class="alert alert-info">No hay moviminetos</div>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:BoundField DataField="IdReferencia" HeaderText="Referencia" />
                            <asp:BoundField DataField="VchEstatus" HeaderText="Estatus" />
                            <asp:BoundField DataField="Autorizacion" HeaderText="Autorización" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </contenttemplate>
    </asp:UpdatePanel>
    <script>
        function multi() {
            $('[id*=ListBox]').multiselect({
                includeSelectAllOption: true
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cBodyBottom" runat="server">
    <!--MODAL-->
    <div id="ModalNuevo" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <contenttemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="lblTituloModal" runat="server" /></h5>
                            <button type="button" class="close d-lg-none" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                    </contenttemplate>
                </asp:UpdatePanel>
                <div class="col-12 pt-3">
                    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="upRegistro">
                        <progresstemplate>
                            <div class="progress">
                                <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" style="width: 100%"></div>
                            </div>
                        </progresstemplate>
                    </asp:UpdateProgress>
                </div>
                <div class="modal-body pt-0" style="padding-bottom: 0px;">
                    <div class="tab-content">
                        <asp:UpdatePanel runat="server">
                            <contenttemplate>
                                <asp:Panel ID="pnlAlertModal" Visible="false" runat="server">
                                    <div id="divAlertModal" role="alert" runat="server">
                                        <asp:Label ID="lblResumen" Text="idhjuihui" runat="server" />
                                        <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    </div>
                                </asp:Panel>
                                <div class="row">
                                    <div class="card" style="margin-top: 0px; margin-bottom: 0px;">
                                        <div class="card-body">
                                            <div class="row">
                                                <div class="col-12 col-sm-6 col-md-6 col-lg-6">
                                                    <div class="card card-stats">
                                                        <div class="card-header card-header-success card-header-icon">
                                                            <div class="card-icon">
                                                                <img class="card-img-top" style="height: 50px; width: 50px" src="../Images/icoWhats.png" alt="whatsapp">
                                                            </div>
                                                            <p class="card-category">Whatsapp</p>
                                                            <h3 class="card-title">
                                                                <asp:Label ID="lblDcmWhatsapp" runat="server">0 <%--<%# Eval("DcmWhatsapp", "{0:C}") %>--%></asp:Label>
                                                            </h3>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-12 col-sm-6 col-md-6 col-lg-6">
                                                    <div class="card card-stats">
                                                        <div class="card-header card-header-warning card-header-icon">
                                                            <div class="card-icon">
                                                                <img class="card-img-top" style="height: 50px; width: 50px" src="../Images/icoSms.jpg" alt="sms">
                                                            </div>
                                                            <p class="card-category">Sms</p>
                                                            <h3 class="card-title">
                                                                <asp:Label ID="lblDcmSms" runat="server">0 <%--<%# Eval("DcmSms", "{0:C}") %>--%></asp:Label></h3>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="table-responsive">
                                                    <table class="table">
                                                        <thead>
                                                            <tr>
                                                                <th class="text-center"></th>
                                                                <th class="text-center"></th>
                                                                <th class="text-center">Disponible</th>
                                                                <th class="text-center">Utilizado</th>
                                                                <th class="text-center">Saldo</th>
                                                                <th class="text-center">Pendiente(s)</th>
                                                                <th id="thError" visible="false" class="text-center" runat="server">Error(es)</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr>
                                                                <td class="text-center"><asp:CheckBox Checked="true" Visible="false" Enabled="false" runat="server" /></td>
                                                                <td class="text-center">Correo</td>
                                                                <td class="text-center">Ilimitado</td>
                                                                <td class="text-center"><asp:Label ID="lblCorreoUsado" Text="0" runat="server" /></td>
                                                                <td class="text-center">Ilimitado</td>
                                                                <td class="text-center">$0.00</td>
                                                                <td id="tdErrorCorreo" visible="false" class="text-center" runat="server"><asp:Label ID="lblErrorCorreo" Text="0" runat="server" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="text-center"><asp:CheckBox ID="cbSms" Visible="false" Checked="true" Enabled="false" runat="server" /></td>
                                                                <td class="text-center">SMS</td>
                                                                <td rowspan="2" class="text-center"><asp:Label Text="$" runat="server" /><asp:Label ID="lblDcmCuenta" runat="server" /></td>
                                                                <td class="text-center"><asp:Label ID="lblAUtilizarSms" Text="0" runat="server" /><asp:Label Text="$" runat="server" /><asp:Label ID="lblTotalUtilizarSms" runat="server" /></td>
                                                                <td rowspan="2" class="text-center"><asp:Label Text="$" runat="server" /><asp:Label ID="lblDcmSaldo" Text="0.00" runat="server" /></td>
                                                                <td class="text-center">$0.00</td>
                                                                <td id="tdErrorSms" visible="false" class="text-center" runat="server"><asp:Label ID="lblErrorSms" Text="0" runat="server" /></td>
                                                                
                                                            </tr>
                                                            <tr>
                                                                <td class="text-center"><asp:CheckBox ID="cbWhats" Visible="false" Checked="false" Enabled="false" runat="server" /></td>
                                                                <td class="text-center">WhatsApp</td>
                                                                <td class="text-center"><asp:Label ID="lblAUtilizarWA" Text="0" runat="server" /><asp:Label Text="$" runat="server" /><asp:Label ID="lblTotalUtilizarWA" runat="server" /></td>
                                                                <td class="text-center"><asp:Label ID="lblPendienteWA" Text="0" runat="server" /></td>
                                                                <td id="tdErrorWA" visible="false" class="text-center" runat="server"><asp:Label ID="lblErrorWA" Text="0" runat="server" /></td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </contenttemplate>

                        </asp:UpdatePanel>
                    </div>
                </div>
                <asp:UpdatePanel runat="server" ID="upRegistro">
                    <contenttemplate>
                        <div class="modal-footer justify-content-center">
                            <asp:LinkButton ID="btnGenerar" OnClick="btnGenerar_Click" CssClass="btn btn-success btn-round" runat="server">
                            <i class="material-icons">link</i> Generar
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnCancelar" OnClick="btnCancelar_Click" CssClass="btn btn-danger btn-round" runat="server">
                            <i class="material-icons">close</i> Cancelar
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnAceptar2" OnClick="btnAceptar2_Click" CssClass="btn btn-info btn-round" runat="server">
                            <i class="material-icons">close</i> Cerrar
                            </asp:LinkButton>
                        </div>
                    </contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGenerar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar2" EventName="Click" />
                    </triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <div id="ModalSeleccionar" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <contenttemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="lblTittleLigas" Text="Selección de usuarios" runat="server" /></h5>
                            <%--<button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>--%>
                        </div>
                    </contenttemplate>
                </asp:UpdatePanel>
                <div class="col-12 pt-3">
                    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="upSeleccionar">
                        <progresstemplate>
                            <div class="progress">
                                <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" style="width: 100%"></div>
                            </div>
                        </progresstemplate>
                    </asp:UpdateProgress>
                </div>
                <div class="modal-body pt-0" style="padding-bottom: 0px;">
                    <div class="tab-content">
                        <asp:UpdatePanel runat="server">
                            <contenttemplate>
                                <asp:Label CssClass="text-danger" runat="server" ID="Label2" />

                                <asp:Panel ID="pnlTipoLigas" Visible="false" runat="server">
                                    <div class="row">
                                        <div class="col-lg-6 col-md-6 col-sm-6">
                                            <asp:LinkButton ID="btnSimple" OnClick="btnSimple_Click" runat="server">
                                                <div class="card card-stats bg-light border-info">
                                                    <div class="card-header card-header-info card-header-icon">
                                                        <div class="card-icon">
                                <i class="material-icons">low_priority</i>
                            </div>
                                                        <h3 class="text-center card-title"><strong>Simple</strong></h3>
                                                    </div>
                                                    <div class="card-footer">
                                                        <div class="stats">
                                                            <h6 style="color: black;">Configuración para todos los usuarios seleccionados.</h6>
                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:LinkButton>
                                        </div>

                                        <div class="col-lg-6 col-md-6 col-sm-6">
                                            <asp:LinkButton ID="btnMultiple" OnClick="btnMultiple_Click" runat="server">
                                                <div class="card card-stats bg-light border-success">
                                                    <div class="card-header card-header-success card-header-icon">
                                                        <div class="card-icon">
                                                            <i class="material-icons">format_line_spacing</i>
                                                        </div>
                                                        <h3 class="text-center card-title"><strong>Multiple</strong></h3>
                                                    </div>
                                                    <div class="card-footer">
                                                        <div class="stats">
                                                            <h6 style="color: black;">Exporte la lista a excel para rellenar los campos correspondientes.</h6>
                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </asp:Panel>

                                <asp:Panel ID="pnlSeleccionUsuarios" runat="server">
                                    <div class="card" style="margin-top: 0px;">
                                        <div class="card-header card-header-tabs card-header" style="padding-top: 0px; padding-bottom: 0px;">
                                            <div class="nav-tabs-navigation">
                                                <div class="nav-tabs-wrapper">
                                                    <div class="form-group d-none">
                                                        <div class="pull-right">
                                                            <asp:LinkButton ID="btnLimpiarFiltros" OnClick="btnLimpiarFiltros_Click" CssClass="btn btn-sm btn-warning btn-round" runat="server">
                                                                    <i class="material-icons left">search</i> Limpiar
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="btnBuscarUsuarios" OnClick="btnBuscarUsuarios_Click" CssClass="btn btn-sm btn-primary btn-round" runat="server">
                                                                    <i class="material-icons left">search</i> Buscar
                                                            </asp:LinkButton>
                                                        </div>
                                                    </div>
                                                    <div class="form-group d-none">
                                                        <asp:Label Text="Busqueda" runat="server" />
                                                        <div class="row">
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroNombre" style="color: black;">Nombre</label>
                                                                <asp:TextBox ID="FiltroNombre" CssClass="form-control" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroApePaterno" style="color: black;">ApePaterno</label>
                                                                <asp:TextBox ID="FiltroApePaterno" CssClass="form-control" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroApeMaterno" style="color: black;">ApeMaterno</label>
                                                                <asp:TextBox ID="FiltroApeMaterno" CssClass="form-control" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroCorreo" style="color: black;">Correo</label>
                                                                <asp:TextBox ID="FiltroCorreo" CssClass="form-control" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroTelefono" style="color: black;">Teléfono</label>
                                                                <asp:TextBox ID="FiltroTelefono" CssClass="form-control" aria-label="Search" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-body">
                                            <div class="row">
                                                <div class="table-responsive">
                                                    <asp:GridView ID="gvUsuarios" OnSorting="gvUsuarios_Sorting" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="IdUsuario" GridLines="None" border="0" AllowPaging="true" PageSize="10" OnPageIndexChanging="gvUsuarios_PageIndexChanging" runat="server">
                                                        <EmptyDataTemplate>
                                                            <div class="alert alert-info">No hay usuarios</div>
                                                        </EmptyDataTemplate>
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <table>
                                                                        <tbody>
                                                                            <tr style="background: transparent;">
                                                                                <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                                    <asp:CheckBox ID="cbSeleccionar" OnCheckedChanged="cbSeleccionar_CheckedChanged" AutoPostBack="true" Checked='<%#Eval("blSeleccionado")%>' runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField SortExpression="NombreCompleto" DataField="NombreCompleto" HeaderText="NOMBRE" />
                                                            <asp:BoundField SortExpression="StrCorreo" DataField="StrCorreo" HeaderText="CORREO" />
                                                            <asp:BoundField SortExpression="StrTelefono" DataField="StrTelefono" HeaderText="TELEFONO" />
                                                        </Columns>
                                                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>


                            </contenttemplate>

                        </asp:UpdatePanel>
                    </div>
                </div>
                <asp:UpdatePanel runat="server" ID="upSeleccionar">
                    <contenttemplate>
                        <div class="modal-footer justify-content-center">
                            <asp:LinkButton ID="btnAceptar" OnClick="btnAceptar_Click" CssClass="btn btn-success btn-round" runat="server">
                            <i class="material-icons">check</i> Aceptar
                            </asp:LinkButton>
                        </div>
                    </contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
                    </triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <div id="ModalMasDetalle" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <contenttemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="Label1" Text="Más Detalle" runat="server" /></h5>
                            <%--<button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>--%>
                        </div>
                    </contenttemplate>
                </asp:UpdatePanel>
                <div class="modal-body pt-0" style="padding-bottom: 0px;">
                    <div class="tab-content">
                        <asp:UpdatePanel runat="server">
                            <contenttemplate>
                                <asp:Panel ID="Panel1" runat="server">
                                    <div class="row">
                                        <div class="card" style="margin-top: 0px; margin-bottom: 0px;">
                                            <img src="../Images/LigaSimple.PNG" class="card-img-top" alt="...">
                                            <div class="card-body">
                                                <h5 class="card-title"><strong>Campos obligatorios *</strong></h5>
                                                <div class="table-responsive">
                                                    <table class="table">
                                                        <tbody>
                                                            <tr>
                                                                <td class="td-name">
                                                                    <p class="card-text">Nombre(s) *.</p>
                                                                </td>
                                                                <td>
                                                                    <p class="card-text">ApePaterno *.</p>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="td-name">
                                                                    <p class="card-text">ApeMaterno *.</p>
                                                                </td>
                                                                <td>
                                                                    <p class="card-text">Correo * + Formato correcto (ejemplo@ejemplo.com).</p>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="td-name">
                                                                    <p class="card-text">Celular *.</p>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card" style="margin-top: 0px; margin-bottom: 0px;">
                                            <img src="../Images/Multiple.PNG" class="card-img-top" alt="...">
                                            <div class="card-body">
                                                <h5 class="card-title"><strong>Campos obligatorios *</strong></h5>
                                                <div class="table-responsive">
                                                    <table class="table">
                                                        <tbody>
                                                            <tr>
                                                                <td class="td-name">
                                                                    <p class="card-text">Asunto *.</p>
                                                                </td>
                                                                <td>
                                                                    <p class="card-text">Concepto *.</p>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="td-name">
                                                                    <p class="card-text">Importe *.</p>
                                                                </td>
                                                                <td>
                                                                    <p class="card-text">Vencimiento *.</p>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="td-name">
                                                                    <p class="card-text">Promocion(es) (Opcional).</p>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>

                                                <h5 class="card-title"><strong>¿Agrego promociones?</strong></h5>
                                                <p class="card-text">Por favor revice que las promociones ingresadas en el campo (PROMOCION(ES)) se encuentre en las permitidas.</p>
                                            </div>
                                        </div>
                                    </div>

                                </asp:Panel>
                            </contenttemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <asp:UpdatePanel runat="server">
                    <contenttemplate>
                        <div class="modal-footer justify-content-center">
                            <asp:LinkButton class="close" data-dismiss="modal" aria-label="Close" CssClass="btn btn-success btn-round" runat="server">
                            <i class="material-icons">check</i> Aceptar
                            </asp:LinkButton>
                        </div>
                    </contenttemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <div id="ModalDatosLiga" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <contenttemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="lblTituloDatosLiga" Text="Editar datos" runat="server" /></h5>
                            <%--<button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>--%>
                        </div>
                    </contenttemplate>
                </asp:UpdatePanel>
                <div class="col-12 pt-3">
                    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="upRegistro">
                        <progresstemplate>
                            <div class="progress">
                                <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" style="width: 100%"></div>
                            </div>
                        </progresstemplate>
                    </asp:UpdateProgress>
                </div>
                <div class="modal-body pt-0" style="padding-bottom: 0px;">
                    <div class="row">
                        <div class="card-body">
                            <div class="tab-content">
                                <asp:UpdatePanel runat="server">
                                    <contenttemplate>
                                        <asp:Label CssClass="text-danger" runat="server" ID="lblValidar" />
                                    </contenttemplate>
                                </asp:UpdatePanel>

                                <asp:UpdatePanel runat="server">
                                    <contenttemplate>
                                        <div class="row">
                                            <div class="form-group col-md-4">
                                                <label for="txtModNombre" style="color: black;padding-left: 0px;">Nombre</label>
                                                <asp:TextBox ID="txtModNombre" Enabled="false" CssClass="form-control" runat="server" />
                                            </div>
                                            <div class="form-group col-md-4">
                                                <label for="txtModCorreo" style="color: black;padding-left: 0px;">Correo</label>
                                                <asp:TextBox ID="txtModCorreo" Enabled="false" CssClass="form-control" runat="server" />
                                            </div>
                                            <div class="form-group col-md-4">
                                                <label for="txtModTelefono" style="color: black;padding-left: 0px;">Teléfono</label>
                                                <asp:TextBox ID="txtModTelefono" Enabled="false" CssClass="form-control" runat="server" />
                                            </div>
                                            <div class="form-group col-md-6">
                                                <label for="txtModAsunto" style="color: black;padding-left: 0px;">Asunto *</label>
                                                <asp:TextBox ID="txtModAsunto" CssClass="form-control" runat="server" />
                                            </div>
                                            <div class="form-group col-md-6">
                                                <label for="txtModConcepto" style="color: black;padding-left: 0px;">Concepto *</label>
                                                <asp:TextBox ID="txtModConcepto" CssClass="form-control" runat="server" />
                                            </div>
                                            <div class="form-group col-md-4">
                                                <label for="txtModImporte" style="color: black;padding-left: 0px;">Importe *</label>
                                                <asp:TextBox ID="txtModImporte" CssClass="form-control" runat="server" />
                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtModImporte" runat="server" />
                                            </div>
                                            <div class="form-group col-md-4">
                                                <label for="txtModVencimiento" style="color: black;padding-left: 0px;">Vencimiento *</label>
                                                <asp:TextBox ID="txtModVencimiento" TextMode="Date" CssClass="form-control" runat="server" />
                                            </div>
                                            <div class="form-group col-md-1">
                                                <label for="cbModEmail" style="color: black;padding-left: 0px;">Email</label>
                                                <asp:CheckBox ID="cbModEmail" runat="server" />
                                            </div>
                                            <div class="form-group col-md-1">
                                                <label for="cbModWa" style="color: black;padding-left: 0px;">Whats</label>
                                                <asp:CheckBox ID="cbModWa" Checked="false" runat="server" />
                                            </div>
                                            <div class="form-group col-md-1">
                                                <label for="cbModSms" style="color: black;padding-left: 0px;">Sms</label>
                                                <asp:CheckBox ID="cbModSms" runat="server" />
                                            </div>
                                            <div class="form-group col-md-4">
                                                <label for="ListBoxMultipleMod" style="color: black;padding-left: 0px;">Promoción(es)</label>
                                                <asp:ListBox ID="ListBoxMultipleMod" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                            </div>
                                        </div>
                                    </contenttemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                    <contenttemplate>
                        <div class="modal-footer justify-content-center">
                            <asp:LinkButton ID="btnGuardar" OnClick="btnGuardar_Click" CssClass="btn btn-success btn-round" runat="server">
                            <i class="material-icons">check</i> Guardar
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnCancelarModLiga" CssClass="btn btn-danger btn-round" data-dismiss="modal" aria-label="Close" runat="server">
                            <i class="material-icons">close</i> Cancelar
                            </asp:LinkButton>
                        </div>
                    </contenttemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <!--END MODAL-->

    <script>
        function showModal() {
            $('#ModalNuevo').modal('show');
        }

        function hideModal() {
            $('#ModalNuevo').modal('hide');
        }
    </script>
    <script>
        function showModalSeleccionar() {
            $('#ModalSeleccionar').modal('show');
        }

        function hideModalSeleccionar() {
            $('#ModalSeleccionar').modal('hide');
        }

        function showModalMasDetalle() {
            $('#ModalMasDetalle').modal('show');
        }

        function hideModalMasDetalle() {
            $('#ModalMasDetalle').modal('hide');
        }

        function showModalDatosLiga() {
            $('#ModalDatosLiga').modal('show');
        }

        function hideModalDatosLiga() {
            $('#ModalDatosLiga').modal('hide');
        }
    </script>
</asp:Content>
