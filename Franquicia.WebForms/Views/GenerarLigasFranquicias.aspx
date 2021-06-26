<%@ Page Culture="es-MX" Title="GenerarLigasFranquicias" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="GenerarLigasFranquicias.aspx.cs" Inherits="Franquicia.WebForms.Views.GenerarLigasFranquicias" %>

<%@ MasterType VirtualPath="~/Views/MasterPage.Master" %>
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
        }
    </style>

    <style>
        .flotante {
            display: scroll;
            position: fixed;
            bottom: 320px;
            right: 0px;
        }
    </style>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlAlertImportarError" Visible="false" runat="server">
                <div id="divAlertImportarError" class="alert alert-danger alert-dismissible fade" role="alert" runat="server">
                    <div class="row">
                        <asp:Label ID="lblMnsjAlertImportarError" Style="margin-top: 5px; margin-left: 15px;" runat="server" />
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
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:FileUpload ID="fuSelecionarExcel" Style="display: none;" runat="server" />

            <script type="text/javascript">
                function UploadFile(fileUpload) {
                    if (fileUpload.value != '') {
                        document.getElementById("<%=btnImportarExcel.ClientID %>").click();
                    }
                }
            </script>
            <asp:Button ID="btnImportarExcel" OnClick="btnImportarExcel_Click" Style="display: none;" Text="Subir" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnImportarExcel" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="content">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-12 col-md-12">
                            <div class="card">
                                <div class="card-header card-header-tabs card-header-primary" style="background:#024693;padding-top: 0px; padding-bottom: 0px;">
                                    <div class="nav-tabs-navigation">
                                        <div class="nav-tabs-wrapper">
                                            <div class="form-group" style="margin-top: 0px; padding-bottom: 0px;">
                                                <asp:LinkButton ID="btnFiltros" Visible="false" ToolTip="Filtros de busqueda." BackColor="#4db6ac" class="btn btn-lg btn-fab btn-fab-mini btn-round" runat="server">
                                                        <i class="material-icons">search</i>
                                                </asp:LinkButton>
                                                <asp:Label Text="Listado de Usuarios" runat="server" />

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
                                            <div class="form-group col-md-4" style="padding-left: 0px;">
                                                <label for="txtAsunto" style="color: black;">Asunto *</label>
                                                <asp:TextBox ID="txtAsunto" CssClass="form-control" runat="server" />
                                            </div>
                                            <div class="form-group col-md-4" style="padding-left: 0px;">
                                                <label for="txtConcepto" style="color: black;">Concepto *</label>
                                                <asp:TextBox ID="txtConcepto" CssClass="form-control" runat="server" />
                                            </div>
                                            <div class="form-group col-md-2" style="padding-left: 0px;">
                                                <label for="txtImporte" style="color: black;">Importe *</label>
                                                <div class="input-group">
                                                    <div class="input-group-prepend">
                                                        <span class="input-group-text">
                                                            <i class="material-icons">$</i>
                                                        </span>
                                                    </div>
                                                    <asp:TextBox ID="txtImporte" CssClass="form-control" TextMode="Phone" runat="server" />
                                                </div>

                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtImporte" runat="server" />
                                            </div>
                                            <div class="form-group col-md-2" style="padding-left: 0px;">
                                                <label for="txtVencimiento" style="color: black;">Vencimiento *</label>
                                                <asp:TextBox ID="txtVencimiento" TextMode="Date" CssClass="form-control" runat="server" />
                                            </div>
                                            <asp:Panel ID="pnlPromociones" runat="server">
                                                <div class="form-group col-md-4" visible="false" style="padding-left: 0px;" runat="server">
                                                    <asp:ListBox ID="ListBoxSimple" runat="server" SelectionMode="Multiple">
                                                    </asp:ListBox>
                                                </div>
                                                <div class="form-group col-md-12" runat="server" style="padding-bottom: 0px;">
                                                    <div class="card card-stats bg-light border-info" style="margin-top: 0px; margin-bottom: 0px;" runat="server">
                                                        <label for="cblPromociones" style="color: black;">Promociones</label>
                                                        <asp:CheckBoxList ID="cblPromociones" RepeatDirection="Horizontal" CssClass="form-check" runat="server">
                                                        </asp:CheckBoxList>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvUsuariosSeleccionados" OnRowCreated="gvUsuariosSeleccionados_RowCreated" OnSorting="gvUsuariosSeleccionados_Sorting" OnRowCommand="gvUsuariosSeleccionados_RowCommand" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="IdUsuario" GridLines="None" border="0" AllowPaging="true" PageSize="10" OnPageIndexChanging="gvUsuariosSeleccionados_PageIndexChanging" runat="server">
                                                <EmptyDataTemplate>
                                                    <div class="alert alert-info">No hay usuarios</div>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:BoundField SortExpression="NombreCompleto" DataField="NombreCompleto" HeaderText="NOMBRE" />
                                                    <asp:BoundField SortExpression="StrCorreo" DataField="StrCorreo" HeaderText="CORREO" />
                                                    <asp:BoundField SortExpression="StrTelefono" DataField="StrTelefono" HeaderText="CELULAR" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <table>
                                                                <tbody>
                                                                    <tr style="background: transparent;">
                                                                        <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:LinkButton ID="btnEliminar" ToolTip="Eliminar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="btnEliminar" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label class="btn btn-sm btn-danger btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">close</i>
                                                                                </asp:Label>
                                                                            </asp:LinkButton>
                                                                            <asp:CheckBox ID="cbSeleccionado" Visible="false" OnCheckedChanged="cbSeleccionado_CheckedChanged" AutoPostBack="true" Checked='<%#Eval("blSeleccionado")%>' runat="server" />
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
                                                    <asp:BoundField DataField="StrTelefono" HeaderText="CELULAR" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
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

            <%--<a class="flotante" href='#'>
                <asp:LinkButton ToolTip="Agregar usuarios." class="btn" runat="server">
                                            <i class="material-icons">add</i>
            </asp:LinkButton></a>--%>
        </ContentTemplate>
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
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="lblTituloModal" runat="server" /></h5>
                            <button type="button" class="close d-lg-none" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="col-12 pt-3">
                    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="upRegistro">
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
                                        <asp:Label ID="lblResumen" Text="idhjuihui" runat="server" />
                                        <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    </div>
                                </asp:Panel>
                                <div class="row">
                                    <div class="col-lg-12 col-md-12">
                                        <div class="card">
                                            <div class="card-body">
                                                <div class="row">
                                                    <div class="table-responsive">
                                                        <table class="table">
                                                            <thead>
                                                                <tr>
                                                                    <th class="text-center"></th>
                                                                    <th class="text-center"></th>
                                                                    <th class="text-center">Disponible</th>
                                                                    <th class="text-center">Usado</th>
                                                                    <th class="text-center">Saldo</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <tr>
                                                                    <td class="text-center">
                                                                        <asp:CheckBox Checked="true" runat="server" /></td>
                                                                    <td class="text-center">Correo</td>
                                                                    <td class="text-center">Ilimitado</td>
                                                                    <td class="text-center">
                                                                        <asp:Label ID="lblCorreoUsado" Text="0" runat="server" /></td>
                                                                    <td class="text-center">Ilimitado</td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="text-center">
                                                                        <asp:CheckBox Enabled="false" runat="server" /></td>
                                                                    <td class="text-center">SMS</td>
                                                                    <td class="text-center">0</td>
                                                                    <td class="text-center">0</td>
                                                                    <td class="text-center">0</td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="text-center">
                                                                        <asp:CheckBox Enabled="false" runat="server" /></td>
                                                                    <td class="text-center">WhatsApp</td>
                                                                    <td class="text-center">0</td>
                                                                    <td class="text-center">0</td>
                                                                    <td class="text-center">0</td>
                                                                </tr>
                                                            </tbody>
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
                <asp:UpdatePanel runat="server" ID="upRegistro">
                    <ContentTemplate>
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
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGenerar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar2" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <div id="ModalSeleccionar" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="lblTittleLigas" Text="Selección de usuarios" runat="server" /></h5>
                            <%--<button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>--%>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="col-12 pt-3">
                    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="upSeleccionar">
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
                                                    <div class="form-group" style="display: none;">
                                                        <asp:Label Text="Busqueda" runat="server" />
                                                        <div class="row">
                                                            <div class="form-group col-md-4">
                                                                <label for="txtNombre" style="color: black;">Nombre</label>
                                                                <asp:TextBox ID="TextBox6" CssClass="form-control" placeholder="Buscar" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="txtNombre" style="color: black;">Correo</label>
                                                                <asp:TextBox ID="TextBox1" CssClass="form-control" placeholder="Buscar" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="txtNombre" style="color: black;">Teléfono</label>
                                                                <asp:TextBox ID="TextBox2" CssClass="form-control" placeholder="Buscar" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4" style="margin-top: 25px;">
                                                                <asp:LinkButton ID="btnBuscarUsuarios" CssClass="btn btn-primary btn-round" runat="server">
                                                                    <i class="material-icons left">search</i> Buscar
                                                                </asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-body">
                                            <div class="row">
                                                <div class="table-responsive">
                                                    <asp:GridView ID="gvUsuarios" OnRowCreated="gvUsuarios_RowCreated" OnSorting="gvUsuarios_Sorting" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="IdUsuario" GridLines="None" border="0" AllowPaging="true" PageSize="10" OnPageIndexChanging="gvUsuarios_PageIndexChanging" runat="server">
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
                                                            <asp:BoundField SortExpression="StrTelefono" DataField="StrTelefono" HeaderText="CELULAR" />
                                                        </Columns>
                                                        <PagerStyle CssClass="pagination-ys" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>


                            </ContentTemplate>

                        </asp:UpdatePanel>
                    </div>
                </div>
                <asp:UpdatePanel runat="server" ID="upSeleccionar">
                    <ContentTemplate>
                        <div class="modal-footer justify-content-center">
                            <asp:LinkButton ID="btnAceptar" OnClick="btnAceptar_Click" CssClass="btn btn-success btn-round" runat="server">
                            <i class="material-icons">check</i> Aceptar
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <div id="ModalMasDetalle" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="Label1" Text="Más Detalle" runat="server" /></h5>
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
                                <asp:Panel ID="Panel1" runat="server">
                                    <div class="row">
                                        <div class="card">
                                            <img src="../Images/LigaSimple.PNG" class="card-img-top" alt="...">
                                            <div class="card-body">
                                                <h5 class="card-title"><strong>Campos obligatorios *</strong></h5>
                                                <p class="card-text">Nombre(s) *.</p>
                                                <p class="card-text">ApePaterno *.</p>
                                                <p class="card-text">ApeMaterno *.</p>
                                                <p class="card-text">Correo * + Formato correcto (ejemplo@ejemplo.com).</p>
                                                <p class="card-text">Celular *.</p>
                                            </div>
                                        </div>
                                    </div>

                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-footer justify-content-center">
                            <asp:LinkButton class="close" data-dismiss="modal" aria-label="Close" CssClass="btn btn-success btn-round" runat="server">
                            <i class="material-icons">check</i> Aceptar
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
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
    </script>
    <script>
        function showModalMasDetalle() {
            $('#ModalMasDetalle').modal('show');
        }

        function hideModalMasDetalle() {
            $('#ModalMasDetalle').modal('hide');
        }
    </script>
</asp:Content>
