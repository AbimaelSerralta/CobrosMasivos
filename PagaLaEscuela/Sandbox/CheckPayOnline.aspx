<%@ Page Title="" Language="C#" MasterPageFile="~/Sandbox/Sandbox.Master" AutoEventWireup="true" CodeBehind="CheckPayOnline.aspx.cs" Inherits="PagaLaEscuela.Sandbox.CheckPayOnline" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHCaja" runat="server">
    <style>
        .UrlPrag::placeholder {
            color: white;
        }

        .UrlPrag::-webkit-input-placeholder {
            color: white;
        }

        .UrlPrag::-moz-placeholder {
            color: white;
        }

        .UrlPrag:-ms-input-placeholder {
            color: white;
        }

        .UrlPrag:-moz-placeholder {
            color: white;
        }

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
            color: white;
            background-color: #fff;
            background-clip: padding-box;
            border: 1px solid #ced4da;
            border-radius: .25rem;
            transition: border-color .15s ease-in-out,box-shadow .15s ease-in-out;
        }
    </style>

    <div class="clearfix my-3 py-2 px-2" style="background-color: #FDFDFD">
        <div class="row align-items-center text-center">
            <div class="col-3 col-md-1">
                <img src="../Images/logoPagaLaEscuela.png" height="50" width="50" class="float-left rounded-circle">
            </div>
            <div class="col-9 col-md-10">
                <div class="row">
                    <div class="btn btn-round" style="width: 100%; background-color: #b9504c; padding-top: 0px; padding-bottom: 0px; padding-left: 20px; padding-right: 2px;">
                        <div class="row">
                            <div class="col-9 col-md-11">
                                <asp:TextBox ID="txtUrlPrag" Style="border: 0; background-color: #b9504c; margin-top: 6px;" CssClass=" text-center form-controlSA UrlPrag" Placeholder="Ingrese la url" runat="server" />
                            </div>
                            <div class="col-3 col-md-1">
                                <asp:LinkButton ID="btnBuscar" OnClick="btnBuscar_Click" OnClientClick="OpenProgress();" CssClass="btn btn-primary btn-fab btn-round" runat="server">
                                    <i class="material-icons">search</i>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="clearfix my-3 py-2 px-2">
        <div class="row">
            <iframe id="ifmLiga" visible="false" style="display: block; background: #000; border: none; height: 100vh; width: 100vw;" runat="server"></iframe>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cBodyBottom" runat="server">
</asp:Content>
