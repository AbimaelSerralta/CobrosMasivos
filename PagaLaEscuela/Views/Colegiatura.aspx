<%@ Page Title="Colegiatura" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="Colegiatura.aspx.cs" Inherits="PagaLaEscuela.Views.Colegiatura" %>

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
            <asp:Panel ID="pnlAlertImportarError" Visible="false" runat="server">
                <div id="divAlertImportarError" class="alert alert-danger alert-dismissible fade" role="alert" runat="server">
                    <div class="row">
                        <asp:Label ID="lblMnsjAlertImportarError" Style="margin-top: 5px; margin-left: 15px;" runat="server" />
                        <asp:LinkButton ID="btnDescargarError" Visible="false" OnClick="btnDescargarError_Click" Style="padding-bottom: 5px; padding-top: 5px; padding-right: 5px; padding-left: 5px; margin-top: 0px;" class="btn btn-success" runat="server">Descargar Error(es)</asp:LinkButton>
                        <asp:LinkButton ID="btnMasDetalle" Visible="false" OnClick="btnMasDetalle_Click" Style="padding-bottom: 5px; padding-top: 5px; padding-right: 5px; padding-left: 5px; margin-top: 0px;" class="btn btn-info" runat="server">Más detalle</asp:LinkButton>
                    </div>

                    <asp:LinkButton ID="btnCloseAlertImportarError" OnClick="btnCloseAlertImportarError_Click" class="close" aria-label="Close" runat="server"><span aria-hidden="true">&times;</span></asp:LinkButton>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
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
                                <div class="card-header card-header-tabs card-header-primary" style="background: #326497; padding-top: 0px; padding-bottom: 0px;">
                                    <div class="nav-tabs-navigation">
                                        <div class="nav-tabs-wrapper">
                                            <div class="form-group" style="margin-top: 0px; padding-bottom: 0px;">
                                                <asp:LinkButton ID="btnFiltros" OnClick="btnFiltros_Click" ToolTip="Filtros de busqueda." BackColor="#4db6ac" class="btn btn-lg btn-fab btn-fab-mini btn-round" runat="server">
                                                        <i class="material-icons">search</i>
                                                </asp:LinkButton>
                                                <asp:Label Text="Listado de colegiaturas" runat="server" />

                                                <asp:LinkButton ID="btnNuevo" OnClick="btnNuevo_Click" ToolTip="Agregar clientes." class="btn btn-lg btn-success btn-fab btn-fab-mini btn-round pull-right" runat="server">
                                                        <i class="material-icons">add</i>
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvColegiaturas" OnSorting="gvColegiaturas_Sorting" OnRowCommand="gvColegiaturas_RowCommand" OnRowDataBound="gvColegiaturas_RowDataBound" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidColegiatura" GridLines="None" border="0" AllowPaging="true" PageSize="2" OnPageIndexChanging="gvColegiaturas_PageIndexChanging" ShowFooter="true" runat="server">
                                                <EmptyDataTemplate>
                                                    <div class="alert alert-info">No hay colegiaturas registrados</div>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField SortExpression="VchIdentificador" HeaderText="IDENTIFICADOR">
                                                        <ItemTemplate>
                                                            <asp:TextBox ToolTip='<%#Eval("VchIdentificador")%>' Style="width: 100%; text-overflow: ellipsis;" Text='<%#Eval("VchIdentificador")%>' Enabled="false" BackColor="Transparent" BorderStyle="None" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField SortExpression="VchIdentificador" DataField="VchIdentificador" HeaderText="IDENTIFICADOR" />--%>
                                                    <asp:BoundField SortExpression="DcmImporte" DataField="DcmImporte" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" DataFormatString="{0:C}" HeaderText="IMPORTE" />
                                                    <asp:BoundField SortExpression="IntCantPagos" DataField="IntCantPagos" ItemStyle-CssClass="text-center" HeaderText="# DE PAGOS" />
                                                    <asp:BoundField SortExpression="VchDescripcion" DataField="VchDescripcion" ItemStyle-CssClass="text-left" HeaderText="PERIODICIDAD" />
                                                    <asp:BoundField SortExpression="DtFHInicio" DataField="DtFHInicio" ItemStyle-CssClass="text-center" DataFormatString="{0:dd/MM/yyyy}" HeaderText="INICIO" />
                                                    <asp:BoundField SortExpression="VchFHLimite" DataField="VchFHLimite" ItemStyle-CssClass="text-center" HeaderText="LIMITE" />
                                                    <asp:BoundField SortExpression="VchFHVencimiento" DataField="VchFHVencimiento" ItemStyle-CssClass="text-center" HeaderText="VENCIMIENTO" />
                                                    <asp:BoundField SortExpression="DcmRecargo" DataField="VchDcmRecargo" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" HeaderText="RECARGO LIMITE" />
                                                    <asp:BoundField SortExpression="DcmRecargoPeriodo" DataField="VchDcmRecargoPeriodo" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" HeaderText="RECARGO PERIODO" />
                                                    <asp:TemplateField SortExpression="UidEstatus" HeaderText="ESTATUS">
                                                        <ItemTemplate>
                                                            <div class="col-md-6">
                                                                <asp:Label ToolTip='<%#Eval("VchEstatus")%>' runat="server">
                                                                <i class="large material-icons">
                                                                    <%#Eval("VchIconoEstatus")%>
                                                                </i>
                                                                </asp:Label>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-Width="160">
                                                        <ItemTemplate>
                                                            <table>
                                                                <tbody>
                                                                    <tr style="background: transparent;">
                                                                        <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:LinkButton ID="LinkButton1" ToolTip="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="Editar" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label ID="Label1" class="btn btn-sm btn-info btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">edit</i>
                                                                                </asp:Label>
                                                                            </asp:LinkButton>

                                                                        </td>
                                                                        <td style="display: none; border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:LinkButton ID="btnCancelarCambio" ToolTip="Visualizar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="Ver" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label ID="lblCancelarCambio" class="btn btn-sm btn-warning btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">remove_red_eye</i>
                                                                                </asp:Label>
                                                                            </asp:LinkButton>
                                                                        </td>
                                                                        <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:LinkButton ToolTip="Datelle" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="Detalle" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label class="btn btn-sm btn-warning btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">date_range</i>
                                                                                </asp:Label>
                                                                            </asp:LinkButton>
                                                                        </td>
                                                                        <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:LinkButton ID="btnAbrirImpor" ToolTip="Importar Alumnos" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="Importar" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label class="btn btn-sm btn-light btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">file_upload</i>
                                                                                </asp:Label>
                                                                            </asp:LinkButton>

                                                                            <asp:LinkButton ID="btnCargarExcel" Visible="false" ToolTip="Seleccionar archivo" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label class="btn btn-sm btn-success btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">attach_file</i>
                                                                                </asp:Label>
                                                                            </asp:LinkButton>
                                                                            <asp:LinkButton ID="btnCancelarExcel" Visible="false" ToolTip="Cancelar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="CancelarImport" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label class="btn btn-sm btn-danger btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">close</i>
                                                                                </asp:Label>
                                                                            </asp:LinkButton>
                                                                        </td>
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
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnNuevo" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cBodyBottom" runat="server">
    <!--MODAL-->
    <div id="ModalNuevo" class="modal fade" tabindex="-1" role="dialog" data-backdrop="static" aria-hidden="true">
        <div class="modal-dialog modal-lg modal-dialog-scrollable" role="document">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="lblTituloModal" runat="server" /></h5>
                            <%--<button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>--%>
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
                    <div class="row">
                        <div class="card card-nav-tabs">
                            <div class="card-header card-header-primary" style="background: #326497;">
                                <!-- colors: "header-primary", "header-info", "header-success", "header-warning", "header-danger" -->
                                <div class="nav-tabs-navigation">
                                    <div class="nav-tabs-wrapper">
                                        <ul class="nav nav-tabs" id="ulTabColegiatura" data-tabs="tabs">
                                            <li class="nav-item">
                                                <a class="nav-link active show" href="#general" data-toggle="tab">
                                                    <i class="material-icons">business</i>Colegiatura<div class="ripple-container"></div>
                                                </a>
                                            </li>
                                            <li class="nav-item">
                                                <a class="nav-link" href="#alumnos" data-toggle="tab">
                                                    <i class="material-icons">wc</i>Asignar Alumnos<div class="ripple-container"></div>
                                                </a>
                                            </li>
                                            <%--<li class="nav-item">
                                                <a class="nav-link" href="#telefono" data-toggle="tab">
                                                    <i class="material-icons">phone</i>Teléfono<div class="ripple-container"></div>
                                                </a>
                                            </li>--%>
                                        </ul>
                                    </div>
                                </div>
                            </div>

                            <div class="card-body">
                                <div class="tab-content">
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:Label CssClass="text-danger" runat="server" ID="lblValidar" />
                                            <asp:Panel ID="pnlAlertMnsjEstatus" Visible="false" runat="server">
                                                <div id="divAlertMnsjEstatus" class="alert alert-danger alert-dismissible fade" role="alert" runat="server">
                                                    <asp:Label runat="server" ID="lblMnsjEstatus" />
                                                    <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                    <div class="tab-pane active show" id="general">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <div class="row">
                                                    <div class="form-group col-md-4">
                                                        <label for="txtIdentificador" style="color: black; margin-bottom: 13px;">Identificador *</label>
                                                        <asp:TextBox ID="txtIdentificador" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-2">
                                                        <label for="txtImporte" style="color: black; margin-bottom: 13px;">Importe *</label>
                                                        <asp:TextBox ID="txtImporte" CssClass="form-control" runat="server" />
                                                        <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtImporte" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-3">
                                                        <asp:CheckBox ID="cbActivarRL" Text="Recargo limite" OnCheckedChanged="cbActivarRL_CheckedChanged" Style="margin-bottom: 0px;" AutoPostBack="true" runat="server" />
                                                        <asp:Panel ID="pnlRecargo" Visible="false" CssClass="row" runat="server">
                                                            <div class="col-md-4" style="padding-right: 0px;">
                                                                <asp:DropDownList ID="ddlTipoRecargo" CssClass="form-control" runat="server">
                                                                    <asp:ListItem Text="$" title="CANTIDAD" Value="CANTIDAD" />
                                                                    <asp:ListItem Text="%" title="PORCENTAJE" Value="PORCENTAJE" />
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-6" style="padding-right: 0px;">
                                                                <asp:TextBox ID="txtRecargo" CssClass="form-control" Style="padding-bottom: 20px; padding-top: 20px;" runat="server" />
                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtRecargo" runat="server" />
                                                            </div>
                                                        </asp:Panel>
                                                    </div>
                                                    <div class="form-group col-md-3">
                                                        <asp:CheckBox ID="cbActivarRP" Text="Recargo periodo" OnCheckedChanged="cbActivarRP_CheckedChanged" Style="margin-bottom: 0px;" AutoPostBack="true" runat="server" />
                                                        <asp:Panel ID="pnlRecargoP" Visible="false" CssClass="row" runat="server">
                                                            <div class="col-md-4" style="padding-right: 0px;">
                                                                <asp:DropDownList ID="ddlTipoRecargoP" CssClass="form-control" runat="server">
                                                                    <asp:ListItem Text="$" title="CANTIDAD" Value="CANTIDAD" />
                                                                    <asp:ListItem Text="%" title="PORCENTAJE" Value="PORCENTAJE" />
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-6" style="padding-right: 0px;">
                                                                <asp:TextBox ID="txtRecargoP" CssClass="form-control" Style="padding-bottom: 20px; padding-top: 20px;" runat="server" />
                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtRecargo" runat="server" />
                                                            </div>
                                                        </asp:Panel>
                                                    </div>
                                                    <div class="form-group col-md-3">
                                                        <label for="ddlPeriodicidad" style="color: black;">Periodicidad *</label>
                                                        <asp:DropDownList ID="ddlPeriodicidad" OnSelectedIndexChanged="ddlPeriodicidad_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-md-3">
                                                        <label for="txtFHInicio" style="color: black;">F/H Inicio *</label>
                                                        <asp:TextBox ID="txtFHInicio" TextMode="Date" CssClass="form-control" runat="server" />
                                                        <asp:LinkButton ID="btnCalcular" OnClick="btnCalcular_Click" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-3">
                                                        <asp:CheckBox ID="cbActivarFHL" Text="Fecha Limite" OnCheckedChanged="cbActivarFHL_CheckedChanged" AutoPostBack="true" runat="server" />
                                                        <asp:Panel ID="pnlActivarFHL" Enabled="false" Width="100%" runat="server">
                                                            <asp:TextBox ID="txtFHLimite" TextMode="Date" CssClass="form-control" runat="server" />
                                                            <asp:LinkButton ID="btnCalcularFHV" OnClick="btnCalcularFHV_Click" runat="server" />
                                                        </asp:Panel>
                                                    </div>
                                                    <div class="form-group col-md-3">
                                                        <asp:CheckBox ID="cbActivarFHV" Text="Fecha Vencimiento" OnCheckedChanged="cbActivarFHV_CheckedChanged" AutoPostBack="true" runat="server" />
                                                        <asp:Panel ID="pnlActivarFHV" Enabled="false" Width="100%" runat="server">
                                                            <asp:TextBox ID="txtFHVencimiento" TextMode="Date" CssClass="form-control" runat="server" />
                                                        </asp:Panel>
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="txtCantPagos" style="color: black; margin-bottom: 13px;">Cant. de pagos *</label>
                                                        <asp:TextBox ID="txtCantPagos" TextMode="Number" CssClass="form-control" runat="server" />
                                                        <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtCantPagos" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="ListBoxPromociones" style="color: black; padding-left: 0px;">Promocion(es)</label>
                                                        <asp:ListBox ID="ListBoxPromociones" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="ddlEstatus" style="color: black; padding-left: 0px;">Estatus</label>
                                                        <asp:DropDownList ID="ddlEstatus" CssClass="form-control" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                    </div>

                                    <div class="tab-pane" id="alumnos">
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

                                                    <div>
                                                        <h5>Seleccionados:
                                                            <asp:Label ID="lblCantSeleccionado" CssClass="alert alert-warning" Font-Size="Larger" Text="0" Style="padding-top: 5px; padding-bottom: 5px; padding-left: 10px; padding-right: 10px;" runat="server" />
                                                        </h5>
                                                    </div>

                                                    <div class="table-responsive">
                                                        <asp:GridView ID="gvAlumnos" OnRowDataBound="gvAlumnos_RowDataBound" OnPageIndexChanging="gvAlumnos_PageIndexChanging" OnSorting="gvAlumnos_Sorting" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidAlumno" GridLines="None" border="0" AllowPaging="true" PageSize="5" runat="server">
                                                            <EmptyDataTemplate>
                                                                <div class="alert alert-info">No hay alumnos asignados</div>
                                                            </EmptyDataTemplate>
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="cbTodo" AutoPostBack="true" OnCheckedChanged="cbTodo_CheckedChanged" runat="server" />
                                                                    </HeaderTemplate>
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
                                                                <asp:BoundField SortExpression="VchIdentificador" DataField="VchIdentificador" HeaderText="IDENTIFICADOR" />
                                                                <asp:BoundField SortExpression="VchMatricula" DataField="VchMatricula" HeaderText="MATRICULA" />
                                                                <asp:BoundField SortExpression="NombreCompleto" DataField="NombreCompleto" HeaderText="NOMBRE" />
                                                            </Columns>
                                                            <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                                        </asp:GridView>
                                                    </div>

                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:UpdatePanel runat="server" ID="upRegistro">
                    <ContentTemplate>
                        <div class="modal-footer justify-content-center">
                            <asp:LinkButton ID="btnCerrar" Visible="false" OnClick="btnCerrar_Click" CssClass="btn btn-info btn-round" runat="server">
                            <i class="material-icons">close</i> Cerrar
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnGuardar" OnClick="btnGuardar_Click" CssClass="btn btn-success btn-round" runat="server">
                            <i class="material-icons">check</i> Guardar
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnCancelar" OnClick="btnCancelar_Click" CssClass="btn btn-danger btn-round" runat="server">
                            <i class="material-icons">close</i> Cancelar
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnEditar" OnClick="btnEditar_Click" CssClass="btn btn-warning btn-round" runat="server">
                            <i class="material-icons">refresh</i> Editar
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnCerrar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnEditar" EventName="Click" />
                    </Triggers>
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
                <div class="modal-body pt-0" style="padding-bottom: 0px;">
                    <div class="tab-content">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlFiltrosBusqueda" runat="server">
                                    <div class="card" style="margin-top: 0px;">
                                        <div class="card-header card-header-tabs card-header" style="padding-top: 0px; padding-bottom: 0px; margin-top: 0px;">
                                            <div class="nav-tabs-navigation">
                                                <div class="nav-tabs-wrapper">
                                                    <div class="form-group">
                                                        <div class="row">
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroIdentificador" style="color: black;">Identificador</label>
                                                                <asp:TextBox ID="FiltroIdentificador" CssClass="form-control" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroCantPagos" style="color: black;"># de pagos</label>
                                                                <asp:TextBox ID="FiltroCantPagos" TextMode="Phone" CssClass="form-control" aria-label="Search" runat="server" />
                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" TargetControlID="FiltroCantPagos" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroPeriodicidad" style="color: black;">Periodicidad</label>
                                                                <asp:DropDownList ID="FiltroPeriodicidad" AppendDataBoundItems="true" CssClass="form-control" runat="server">
                                                                </asp:DropDownList>
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
                                                                <asp:TextBox ID="FiltroImporteMayor" CssClass="form-control" placeholder="Mayor" aria-label="Search" Style="margin-top: 12px;" runat="server" />
                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="FiltroImporteMayor" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-3">
                                                                <label for="ddlImporteMenor" style="color: black;"></label>
                                                                <asp:DropDownList ID="ddlImporteMenor" CssClass="form-control" Style="margin-top: 6px;" runat="server">
                                                                    <asp:ListItem Text="(<=) Menor o igual que" Value="<=" />
                                                                    <asp:ListItem Text="(<) Menor que" Value="<" />
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="form-group col-md-3">
                                                                <label for="FiltroImporteMenor" style="color: black;"></label>
                                                                <asp:TextBox ID="FiltroImporteMenor" CssClass="form-control" placeholder="Menor" aria-label="Search" Style="margin-top: 12px;" runat="server" />
                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="FiltroImporteMenor" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroInicioDesde" style="color: black;">Fecha Inicio (Desde)</label>
                                                                <asp:TextBox ID="FiltroInicioDesde" CssClass="form-control" TextMode="Date" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroInicioHasta" style="color: black;">Fecha Inicio (Hasta)</label>
                                                                <asp:TextBox ID="FiltroInicioHasta" CssClass="form-control" TextMode="Date" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroFechaLimite" style="color: black;">Fecha limite</label>
                                                                <asp:DropDownList ID="FiltroFechaLimite" AppendDataBoundItems="true" CssClass="form-control" runat="server">
                                                                    <asp:ListItem Text="AMBOS" />
                                                                    <asp:ListItem Text="SI" />
                                                                    <asp:ListItem Text="NO" />
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="form-group col-md-3">
                                                                <label for="FiltroFechaVencimiento" style="color: black;">Fecha vencimiento</label>
                                                                <asp:DropDownList ID="FiltroFechaVencimiento" AppendDataBoundItems="true" CssClass="form-control" runat="server">
                                                                    <asp:ListItem Text="AMBOS" />
                                                                    <asp:ListItem Text="SI" />
                                                                    <asp:ListItem Text="NO" />
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="form-group col-md-3">
                                                                <label for="FiltroRecargoLimite" style="color: black;">Recargo limite</label>
                                                                <asp:DropDownList ID="FiltroRecargoLimite" AppendDataBoundItems="true" CssClass="form-control" runat="server">
                                                                    <asp:ListItem Text="AMBOS" />
                                                                    <asp:ListItem Text="SI" />
                                                                    <asp:ListItem Text="NO" />
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="form-group col-md-3">
                                                                <label for="FiltroRecargoPeriodo" style="color: black;">Recargo periodo</label>
                                                                <asp:DropDownList ID="FiltroRecargoPeriodo" AppendDataBoundItems="true" CssClass="form-control" runat="server">
                                                                    <asp:ListItem Text="AMBOS" />
                                                                    <asp:ListItem Text="SI" />
                                                                    <asp:ListItem Text="NO" />
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="form-group col-md-3">
                                                                <label for="FiltroEstatus" style="color: black;">Estatus</label>
                                                                <asp:DropDownList ID="FiltroEstatus" AppendDataBoundItems="true" CssClass="form-control" runat="server"></asp:DropDownList>
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
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-footer justify-content-center" style="padding-top: 0px; padding-bottom: 10px;">
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

    <div id="ModalDetalle" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="lblTittleDetalle" Text="Listado de Fechas" runat="server" /></h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="modal-body pt-0" style="padding-bottom: 0px;">
                    <div class="tab-content">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <div class="card">
                                    <div class="table-responsive">
                                        <asp:GridView ID="gvFechas" OnPageIndexChanging="gvFechas_PageIndexChanging" OnSorting="gvFechas_Sorting" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidFechaColegiatura" GridLines="None" border="0" AllowPaging="true" PageSize="10" runat="server">
                                            <EmptyDataTemplate>
                                                <div class="alert alert-info">No hay fechas registradas</div>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:BoundField SortExpression="IntNumero" DataField="IntNumero" HeaderText="#" />
                                                <asp:BoundField SortExpression="DtFHInicio" DataField="DtFHInicio" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" DataFormatString="{0:dd/MM/yyyy}" HeaderText="FECHA INICIO" />
                                                <asp:BoundField SortExpression="VchFHLimite" DataField="VchFHLimite" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="FECHA LIMITE" />
                                                <asp:BoundField SortExpression="VchFHVencimiento" DataField="VchFHVencimiento" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="FECHA VENCIMIENTO" />
                                            </Columns>
                                            <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
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

        function showModalMasDetalle() {
            $('#ModalMasDetalle').modal('show');
        }
        function hideModalMasDetalle() {
            $('#ModalMasDetalle').modal('hide');
        }
    </script>
    <script>
        function multi() {
            $('[id*=ListBox]').multiselect({
                includeSelectAllOption: true
            });
        }
    </script>
    <script>
        function button_click(objTextBox, objBtnID) {
            if (window.event.keyCode != 13) {
                document.getElementById(objBtnID).focus();
                document.getElementById(objBtnID).click();
            }
        }
    </script>
    <script>
        function showTab(tab) {
            $('#ulTabColegiatura a[href="#general"]').tab('show')
        }
    </script>
</asp:Content>
