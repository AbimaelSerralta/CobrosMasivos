<%@ Page Title="Empresas" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="Empresas.aspx.cs" Inherits="Franquicia.WebForms.Views.Empresas" %>

<%@ MasterType VirtualPath="~/Views/MasterPage.Master" %>

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
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="content">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-12 col-md-12">
                            <div class="card">
                                <div class="card-header card-header-tabs card-header-primary" style="background: #b9504c; padding-top: 0px; padding-bottom: 0px;">
                                    <div class="nav-tabs-navigation">
                                        <div class="nav-tabs-wrapper">
                                            <div class="form-group" style="margin-top: 0px; padding-bottom: 0px;">
                                                <asp:LinkButton ID="btnFiltros" ToolTip="Filtros de busqueda." BackColor="#4db6ac" class="btn btn-lg btn-fab btn-fab-mini btn-round" runat="server">
                                                        <i class="material-icons">search</i>
                                                </asp:LinkButton>
                                                <asp:Label Text="Listado de comercios" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvEmpresas" OnRowCreated="gvEmpresas_RowCreated" OnSorting="gvEmpresas_Sorting" OnPageIndexChanging="gvEmpresas_PageIndexChanging" AllowPaging="true" PageSize="10" OnRowCommand="gvEmpresas_RowCommand" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidCliente" GridLines="None" border="0" runat="server">
                                                <EmptyDataTemplate>
                                                    <div class="alert alert-info">No hay clientes registrados</div>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:ButtonField CommandName="Select" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                                                    <asp:BoundField SortExpression="VchRFC" DataField="VchRFC" HeaderText="RFC" />
                                                    <asp:BoundField SortExpression="VchRazonSocial" DataField="VchRazonSocial" HeaderText="RAZÓN SOCIAL" />
                                                    <asp:BoundField SortExpression="VchNombreComercial" DataField="VchNombreComercial" HeaderText="NOMBRE COMERCIAL" />
                                                    <asp:TemplateField SortExpression="UidEstatus" HeaderText="ESTATUS">
                                                        <ItemTemplate>
                                                            <div class="col-md-6">
                                                                <asp:Label ToolTip='<%#Eval("VchEstatus")%>' runat="server">
                                                                <i class="large material-icons">
                                                                    <%#Eval("VchIcono")%>
                                                                </i>
                                                                </asp:Label>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <table>
                                                                <tbody>
                                                                    <tr style="background: transparent;">
                                                                        <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:LinkButton ID="btnCredencialesLigas" ToolTip="Credenciales para generar ligas" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="btnCredencialesLigas" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label class="btn btn-sm btn-primary btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">security</i>
                                                                                </asp:Label>
                                                                            </asp:LinkButton>
                                                                        </td>
                                                                        <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:LinkButton ID="btnPromociones" ToolTip="Configurar promociones" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="btnPromociones" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label class="btn btn-sm btn-warning btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">settings</i>
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
    <!--MODAL-->
    <div id="ModalCredenciales" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="lblTittleCredenciales" Text="Parámetros de Entrada" runat="server" /></h5>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="col-12 pt-3">
                    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="upCredenciales">
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
                                <asp:Panel ID="pnlAlertCredenciales" runat="server">
                                    <div id="divAlertCredenciales" class="alert alert-danger alert-dismissible fade" role="alert" runat="server">
                                        <asp:Label ID="lblMnsjAlertCredenciales" runat="server" />
                                        <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlCredenciales" runat="server">
                                    <div class="card" style="margin-top: 0px;">
                                        <div class="card-header card-header-tabs card-header" style="padding-top: 0px; padding-bottom: 0px;">
                                            <div class="nav-tabs-navigation">
                                                <div class="nav-tabs-wrapper">
                                                    <div class="form-group">
                                                        <div class="row">
                                                            <div class="form-group col-md-4">
                                                                <label for="txtIdCompany" style="color: black;">Id_company *</label>
                                                                <asp:TextBox ID="txtIdCompany" CssClass="form-control" required="required" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="txtIdBranch" style="color: black;">Id_branch *</label>
                                                                <asp:TextBox ID="txtIdBranch" CssClass="form-control" required="required" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="txtMoneda" style="color: black;">Moneda *</label>
                                                                <asp:TextBox ID="txtMoneda" Text="MXN" Enabled="false" CssClass="form-control" required="required" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="txtUsuario" style="color: black;">User *</label>
                                                                <asp:TextBox ID="txtUsuario" CssClass="form-control" required="required" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="txtPassword" style="color: black;">Password *</label>
                                                                <asp:TextBox ID="txtPassword" CssClass="form-control" required="required" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="txtCanal" style="color: black;">Canal *</label>
                                                                <asp:TextBox ID="txtCanal" Text="W" Enabled="false" CssClass="form-control" required="required" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-6">
                                                                <label for="txtData" style="color: black;">Data0 *</label>
                                                                <asp:TextBox ID="txtData" Text="9265655113" Enabled="false" CssClass="form-control" required="required" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-6">
                                                                <label for="txtUrl" style="color: black;">Url *</label>
                                                                <asp:TextBox ID="txtUrl" Text="https://bc.mitec.com.mx/p/gen" Enabled="false" CssClass="form-control" required="required" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-6">
                                                                <label for="txtSemillaAES" style="color: black;">Semilla AES *</label>
                                                                <asp:TextBox ID="txtSemillaAES" Text="7AACFE849FABD796F6DCB947FD4D5268" Enabled="false" CssClass="form-control" required="required" runat="server" />
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="card" style="margin-top: 15px; margin-bottom: 0px;">
                                                                <div class="card-header card-header-text card-header-primary">
                                                                    <div class="card-text" style="padding-top: 0px; padding-bottom: 0px;">
                                                                        <h4 class="card-title">Importe liga</h4>
                                                                    </div>
                                                                </div>
                                                                <div class="card-body">
                                                                    <div class="row">
                                                                        <div class="form-group col-md-4">
                                                                            <label for="cbActivarImp" style="color: black;">Activar importe *</label>
                                                                            <div class="input-group" style="padding-left: 10px;">
                                                                                <asp:CheckBox ID="cbActivarImp" Style="margin-top: 12px;" runat="server" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group col-md-4">
                                                                            <label for="txtImpMin" style="color: black;">Importe mínimo *</label>
                                                                            <div class="input-group">
                                                                                <div class="input-group-prepend">
                                                                                    <span class="input-group-text" style="padding-left: 0px; padding-right: 5px;">
                                                                                        <i class="material-icons">$</i>
                                                                                    </span>
                                                                                </div>
                                                                                <asp:TextBox ID="txtImpMin" Text="0.00" CssClass="form-control" runat="server" />
                                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtImpMin" runat="server" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group col-md-4">
                                                                            <label for="txtImpMax" style="color: black;">Importe máximo *</label>
                                                                            <div class="input-group">
                                                                                <div class="input-group-prepend">
                                                                                    <span class="input-group-text" style="padding-left: 0px; padding-right: 5px;">
                                                                                        <i class="material-icons">$</i>
                                                                                    </span>
                                                                                </div>
                                                                                <asp:TextBox ID="txtImpMax" Text="0.00" CssClass="form-control" runat="server" />
                                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtImpMax" runat="server" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
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
                <asp:UpdatePanel runat="server" ID="upCredenciales">
                    <ContentTemplate>
                        <div class="modal-footer justify-content-center">
                            <asp:LinkButton ID="btnGuardarCredenciales" OnClick="btnGuardarCredenciales_Click" CssClass="btn btn-success btn-round" runat="server">
                            <i class="material-icons">check</i> Guardar
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnCancelarCredenciales" class="close" data-dismiss="modal" aria-label="Close" CssClass="btn btn-danger btn-round" runat="server">
                            <i class="material-icons">close</i> Cancelar
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarCredenciales" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <div id="ModalPromociones" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="Label1" Text="Configuración de promociones" runat="server" /></h5>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="col-12 pt-3">
                    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="upPromociones">
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
                                <asp:Panel ID="pnlAlertPromociones" runat="server">
                                    <div id="divAlertPromociones" class="alert alert-danger alert-dismissible fade" role="alert" runat="server">
                                        <asp:Label ID="lblMnsjAlertPromociones" runat="server" />
                                        <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    </div>
                                </asp:Panel>
                                <div class="card" style="margin-top: 0px;">
                                    <div class="card-header card-header-tabs card-header" style="padding-top: 0px; padding-bottom: 0px;">
                                        <div class="nav-tabs-navigation">
                                            <div class="nav-tabs-wrapper">
                                                <div class="form-group">
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
                                                                                                <asp:CheckBox ID="cbPromocion" Text='<%#Eval("VchDescripcion")%>' runat="server" />
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
                                                                                                    <div class="col-sm-4">
                                                                                                        <div class="input-group">
                                                                                                            <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtComicion" runat="server" />
                                                                                                            <asp:TextBox ID="txtComicion" Font-Size="Large" class="form-control" placeholder="123...100" required="required" runat="server" />
                                                                                                            <div class="input-group-prepend">
                                                                                                                <span class="input-group-text">
                                                                                                                    <i class="material-icons">%</i>
                                                                                                                </span>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="col-sm-6">
                                                                                                        <div class="input-group">
                                                                                                            <div class="input-group-prepend">
                                                                                                                <span class="input-group-text">
                                                                                                                    <i class="material-icons">$</i>
                                                                                                                </span>
                                                                                                            </div>
                                                                                                            <asp:TextBox ID="txtApartirDe" ToolTip="A partir del monto ingresado se activará esta promoción." Font-Size="Large" class="form-control" placeholder="A partir de" required="required" runat="server" />
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
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <asp:UpdatePanel runat="server" ID="upPromociones">
                    <ContentTemplate>
                        <div class="modal-footer justify-content-center">
                            <asp:LinkButton ID="btnGuardarPromociones" OnClick="btnGuardarPromociones_Click" CssClass="btn btn-success btn-round" runat="server">
                            <i class="material-icons">check</i> Guardar
                            </asp:LinkButton>
                            <asp:LinkButton class="close" data-dismiss="modal" aria-label="Close" CssClass="btn btn-danger btn-round" runat="server">
                            <i class="material-icons">close</i> Cancelar
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarPromociones" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <!--END MODAL-->

    <script>
        function showModalCredenciales() {
            $('#ModalCredenciales').modal('show');
        }

        function hideModalCredenciales() {
            $('#ModalCredenciales').modal('hide');
        }

        function showModalPromociones() {
            $('#ModalPromociones').modal('show');
        }

        function hideModalPromociones() {
            $('#ModalPromociones').modal('hide');
        }
    </script>
</asp:Content>
