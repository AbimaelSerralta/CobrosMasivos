<%@ Page Title="Configuración" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="Configuracion.aspx.cs" Inherits="PagaLaEscuela.Views.Configuracion" %>

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

    <div class="row" style="padding-left: 10px; padding-right: 10px;">
        <div class="card card-nav-tabs">
            <div class="card-header card-header-primary" style="background:#326497;">
                <!-- colors: "header-primary", "header-info", "header-success", "header-warning", "header-danger" -->
                <div class="nav-tabs-navigation">
                    <div class="nav-tabs-wrapper">
                        <ul class="nav nav-tabs" data-tabs="tabs">
                            <li class="nav-item">
                                <a class="nav-link active show" href="#general" data-toggle="tab">
                                    <i class="material-icons">business</i>Estructura<div class="ripple-container"></div>
                                </a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" href="#direccion" data-toggle="tab">
                                    <i class="material-icons">money_off</i>Comiciones<div class="ripple-container"></div>
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
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <div class="tab-pane active show" id="general">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <div class="card" style="margin-top: 0px;">
                                    <div class="card-header">
                                        <div class="d-flex flex-row-reverse">
                                            <div>
                                                <asp:LinkButton ID="btnGuardar" BorderWidth="1" CssClass="btn btn-sm btn-success btn-round" runat="server">
                                                                                <i class="material-icons">check</i> Guardar
                                                </asp:LinkButton>
                                            </div>
                                            <div>
                                                <asp:LinkButton ID="btnLimpiar" BorderWidth="1" CssClass="btn btn-sm btn-warning btn-round" runat="server">
                                                                                <i class="material-icons">delete</i> Limpiar
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="card-body">
                                        <form>
                                            <div class="row">
                                                <div class="col-sm-6 col-md-4 col-lg-3">
                                                    <label for="ddlProceso" style="color: black; padding-left: 0px;">Proceso de enseñanza</label>
                                                    <asp:DropDownList ID="ddlProceso" OnSelectedIndexChanged="ddlProceso_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-sm-6 col-md-4 col-lg-3">
                                                    <label for="ddlTipo" style="color: black; padding-left: 0px;">Tipo de enseñanza</label>
                                                    <asp:DropDownList ID="ddlTipo" OnSelectedIndexChanged="ddlTipo_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                                <div id="pnlNivel" class="col-sm-6 col-md-4 col-lg-3" runat="server">
                                                    <label for="ListBoxNivel" style="color: black; padding-left: 0px;">Nivel de enseñanza</label>
                                                    <br />
                                                    <asp:ListBox ID="ListBoxNivel" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                                </div>
                                                <div id="pnlNombreCarrera" class="col-sm-6 col-md-4 col-lg-3" runat="server">
                                                    <label for="txtNombreCarrera" style="color: black; padding-left: 0px;"><asp:Label ID="lblTitleNombreCarrera" Text="Nombre carrera" runat="server" /></label>
                                                    <asp:TextBox ID="txtNombreCarrera" Style="text-transform: uppercase;" class="form-control" placeholder="LIC. EN TECNOLOGÍAS DE LA INFORMACIÓN" runat="server" />
                                                </div>
                                                <div id="pnlPeriodo" class="col-sm-6 col-md-4 col-lg-3" runat="server">
                                                    <label for="txtPeriodo" style="color: black; padding-left: 0px;"><asp:Label ID="lblTitlePeriodo" Text="Periodo" runat="server" /></label>
                                                    <asp:TextBox ID="txtPeriodo" Style="text-transform: uppercase;" class="form-control" placeholder="PRIMER CUATRIMESTRE" runat="server" />
                                                </div>
                                            </div>
                                        </form>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="table-responsive">
                                        <asp:GridView ID="gvNivelEnsenianza" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidNivelEnsenianza" GridLines="None" border="0" AllowPaging="true" PageSize="10" runat="server">
                                            <EmptyDataTemplate>
                                                <div class="alert alert-info">No hay estructura registrado</div>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:BoundField SortExpression="VchNivelEnsenianza" DataField="VchNivelEnsenianza" HeaderText="NIVEL" />
                                            </Columns>
                                            <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                        </asp:GridView>
                                    </div>
                                </div>
                                
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>

                    <div class="tab-pane" id="direccion">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="tab-pane" id="telefono">
                        <div class="container-fluid">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="form-group col-md-6">
                                            <label for="ddlTipoTelefono" style="color: black;">Tipo Teléfono</label>
                                            <asp:DropDownList ID="ddlTipoTelefono" CssClass="form-control" runat="server">
                                            </asp:DropDownList>
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cBodyBottom" runat="server">

    <script>
        function multi() {
            $('[id*=ListBox]').multiselect({
                includeSelectAllOption: true
            });
        }
    </script>
</asp:Content>
