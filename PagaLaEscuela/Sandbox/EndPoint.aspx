<%@ Page Title="Paga La Escuela" Language="C#" MasterPageFile="~/Sandbox/Sandbox.Master" AutoEventWireup="true" CodeBehind="EndPoint.aspx.cs" Inherits="PagaLaEscuela.Sandbox.EndPoint" %>

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
                    <div class="card-header card-header-primary" style="background: #b9504c;">
                        <!-- colors: "header-primary", "header-info", "header-success", "header-warning", "header-danger" -->
                        <div class="nav-tabs-navigation">
                            <div class="nav-tabs-wrapper">
                                <ul class="nav nav-tabs" id="ulTabColegiatura" data-tabs="tabs">
                                    <li class="nav-item">
                                        <a class="nav-link active show" href="#comercios" data-toggle="tab">
                                            <i class="material-icons">message</i>Comercios<div class="ripple-container"></div>
                                        </a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" href="#pagosEnlinea" data-toggle="tab">
                                            <i class="material-icons">payment</i>Pago En linea<div class="ripple-container"></div>
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

                            <div class="tab-pane active show" id="comercios">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <div class="row" style="margin-bottom: 20px;">
                                            <div class="col-md-12">
                                                <label for="txtEntregarReferencia" style="color: black;">Entregar referencia</label>
                                                <div class="input-group">
                                                    <div class="input-group-prepend">
                                                        <span class="input-group-text">
                                                            <i class="material-icons">article</i>
                                                        </span>
                                                    </div>
                                                    <asp:TextBox ID="txtEntregarReferencia" CssClass="form-control" runat="server" />
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <label for="txtConsultarReferencia" style="color: black;">Consultar referencia</label>
                                                <div class="input-group">
                                                    <div class="input-group-prepend">
                                                        <span class="input-group-text">
                                                            <i class="material-icons">request_page</i>
                                                        </span>
                                                    </div>
                                                    <asp:TextBox ID="txtConsultarReferencia" CssClass="form-control" runat="server" />
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <label for="txtPagarReferencia" style="color: black;">Pagar referencia</label>
                                                <div class="input-group">
                                                    <div class="input-group-prepend">
                                                        <span class="input-group-text">
                                                            <i class="material-icons">attach_money</i>
                                                        </span>
                                                    </div>
                                                    <asp:TextBox ID="txtPagarReferencia" CssClass="form-control" runat="server" />
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <label for="txtCancelarReferencia" style="color: black;">Cancelar pago</label>
                                                <div class="input-group">
                                                    <div class="input-group-prepend">
                                                        <span class="input-group-text">
                                                            <i class="material-icons">money_off</i>
                                                        </span>
                                                    </div>
                                                    <asp:TextBox ID="txtCancelarReferencia" CssClass="form-control" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <div class="modal-footer justify-content-center">
                                                    <asp:LinkButton ID="btnGuardarComision" CssClass="btn btn-success btn-round" runat="server">
                                                                <i class="material-icons">check</i> Guardar
                                                    </asp:LinkButton>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                            <div class="tab-pane" id="pagosEnlinea">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <div class="row" style="margin-bottom: 20px;">
                                            <div class="col-md-12">
                                                <label for="txtEntregarLiga" style="color: black;">Entregar liga</label>
                                                <div class="input-group">
                                                    <div class="input-group-prepend">
                                                        <span class="input-group-text">
                                                            <i class="material-icons">article</i>
                                                        </span>
                                                    </div>
                                                    <asp:TextBox ID="txtEntregarLiga" CssClass="form-control" runat="server" />
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <label for="txtPagarLiga" style="color: black;">Pagar liga</label>
                                                <div class="input-group">
                                                    <div class="input-group-prepend">
                                                        <span class="input-group-text">
                                                            <i class="material-icons">attach_money</i>
                                                        </span>
                                                    </div>
                                                    <asp:TextBox ID="txtPagarLiga" CssClass="form-control" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <div class="modal-footer justify-content-center">
                                                    <asp:LinkButton ID="LinkButton1" CssClass="btn btn-success btn-round" runat="server">
                                                                <i class="material-icons">check</i> Guardar
                                                    </asp:LinkButton>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
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
</asp:Content>
