<%@ Page Title="" Language="C#" MasterPageFile="~/Sandbox/Sandbox.Master" AutoEventWireup="true" CodeBehind="CheckReference.aspx.cs" Inherits="PagaLaEscuela.Sandbox.CheckReference" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHCaja" runat="server">

    <style>
        .rowSA {
            display: -ms-flexbox;
            display: flex;
            -ms-flex-wrap: wrap;
            flex-wrap: wrap;
            margin-right: -15px;
            margin-left: -15px;
        }

        .form-controlSA {
            display: block;
            width: 100%;
            height: calc(2.25rem + 2px);
            padding: .375rem .75rem;
            font-size: 1rem;
            line-height: 1.5;
            color: #495057;
            background-color: #fff;
            background-clip: padding-box;
            border: 1px solid #ced4da;
            border-radius: .25rem;
            transition: border-color .15s ease-in-out,box-shadow .15s ease-in-out;
        }

        .reference_emu {
            text-align: center;
            padding: 5px;
            width: 120%;
            margin-left: -10%;
            font-weight: bold;
            font-stretch: expanded;
            font-size: x-large !important;
        }

        .shadow {
            box-shadow: 0 .5rem 1rem rgba(0,0,0,.15) !important;
        }
    </style>

    <div class="form-horizontal">
        <div class="rowSA">
            <div class="col-md-4">&nbsp;</div>
            <div class="col-md-4 text-center">
                <h5 style="color: black">Por favor ingrese su referencia</h5>
                <img src="../Images/terminal.png" style="width: 80%">
            </div>
            <div class="col-md-4">&nbsp;</div>
        </div>
        <div class="rowSA" style="margin-top: -120px;">
            <div class="col-md-4">&nbsp;</div>
            <div class="col-md-4 text-center">
                <h3 class="text-white">Referencia</h3>

                <asp:TextBox ID="txtReferencia" CssClass="form-controlSA reference_emu shadow text-box single-line" PlaceHolder="Ingrese su referencia" data-val="true" data-val-required="El campo Referencia es obligatorio." name="Reference" required="required" type="text" value="" runat="server" />
                <span class="field-validation-valid text-danger" data-valmsg-for="Reference" data-valmsg-replace="true"></span>
            </div>
            <div class="col-md-4">&nbsp;</div>
        </div>

        <div class="clearfix my-3 py-2 px-2 shadow" style="background-color: #FDFDFD">

            <div class="row align-items-center text-center">
                <div class="col-md-3">
                    <h6 style="color: black">Resultado de la Consulta:</h6>
                </div>
                <div class="col-md-2">
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Código:</span>
                        </div>
                        <asp:TextBox ID="txtCodigo" CssClass="form-control text-box single-line" ReadOnly="true" runat="server" />
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Mensaje:</span>
                        </div>
                        <asp:TextBox ID="txtMnsj" CssClass="form-control text-box single-line" ReadOnly="true" runat="server" />
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Monto:</span>
                        </div>
                        <asp:TextBox ID="txtMonto" CssClass="form-control text-box single-line" data-val="true" data-val-number="El campo Monto debe ser un número." data-val-range="El campo Monto debe estar entre 1 y 9999999." data-val-range-max="9999999" data-val-range-min="1" data-val-required="El campo Monto es obligatorio." runat="server" />
                    </div>
                    <div class="text-right">
                        <span class="field-validation-valid text-danger" data-valmsg-for="CheckResponse.Amount" data-valmsg-replace="true"></span>
                    </div>
                </div>
            </div>
            <div class="col-md12" style="margin-top: 10px;">
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
            </div>
        </div>

        <div class="rowSA text-center mt-5">
            <div class="col-md-12">
                <asp:LinkButton ID="btnConsultar" OnClick="btnConsultar_Click" OnClientClick="OpenProgress();" class="btn btn-info" runat="server">
                    Consultar Pago
                    <i class="material-icons">send</i>
                </asp:LinkButton>
                <asp:LinkButton ID="btnPagar" Visible="false" OnClick="btnPagar_Click" OnClientClick="OpenProgress();" class="btn btn-success" runat="server">
                    <i class="material-icons">attach_money</i>
                    Pagar referencia
                </asp:LinkButton>
                <asp:LinkButton ID="btnCancelarPago" Visible="false" OnClick="btnCancelarPago_Click" OnClientClick="OpenProgress();" class="btn btn-danger" runat="server">
                    <i class="material-icons">clear</i>
                    Cancelar pago
                </asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cBodyBottom" runat="server">
</asp:Content>
