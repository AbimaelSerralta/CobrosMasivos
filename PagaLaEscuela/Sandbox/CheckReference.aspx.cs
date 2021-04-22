﻿using Franquicia.Bussiness;
using Franquicia.Bussiness.ClubPago;
using Franquicia.Bussiness.IntegracionesClubPago;
using Franquicia.Domain.Models.ClubPago;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PagaLaEscuela.Sandbox
{
    public partial class CheckReference : System.Web.UI.Page
    {
        HeaderClubPagoServices headerClubPagoServices = new HeaderClubPagoServices();
        DecodificarService decodificarService = new DecodificarService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UidIntegracionMaster"] != null)
            {
                ViewState["UidIntegracionLocal"] = Session["UidIntegracionMaster"];
            }
            else
            {
                ViewState["UidIntegracionLocal"] = Guid.Empty;
            }

            if (!IsPostBack)
            {

            }
            else
            {
                pnlAlert.Visible = false;
                lblMensajeAlert.Text = "";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade");
            }
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacionesServices validacionesServices = new ValidacionesServices();

                btnConsultar.Visible = true;
                btnPagar.Visible = false;

                HeaderClubPago headerClubPago = headerClubPagoServices.ObtenerHeaderClubPago();

                var refInt = validacionesServices.ExisteReferenciaIntegracionCF(txtReferencia.Text, Guid.Parse(ViewState["UidIntegracionLocal"].ToString()));
                if (refInt.Item1)
                {
                    ConsultarReferenciasServices consultarReferenciasServices = new ConsultarReferenciasServices();
                    ConsultarReferencia consultarReferecia = consultarReferenciasServices.ApiConsultarReferenciaWeb("https://pagalaescuela.mx/Pagalaescuela/Service/ConsultaReferencia", txtReferencia.Text, headerClubPago.UserAgent, decodificarService.Base64Encode(headerClubPago.XOrigin));

                    txtCodigo.Text = consultarReferecia.codigo.ToString();
                    txtMnsj.Text = consultarReferecia.mensaje;
                    txtMonto.Text = (decimal.Parse(consultarReferecia.monto) / 100).ToString("N2");
                    txtMonto.ReadOnly = false;

                    if (consultarReferecia.codigo == 0 && !string.IsNullOrEmpty(consultarReferecia.mensaje))
                    {
                        btnConsultar.Visible = false;
                        btnPagar.Visible = true;
                    }
                }
                else
                {
                    txtCodigo.Text = "40";
                    txtMnsj.Text = "Adquiriente inválido";
                    txtMonto.Text = "0.00";

                    txtMonto.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "<b>Lo sentimos</b> ha ocurrido un error inesperado, por favor inténtelo mas tarde si el error persiste, comuníquese con los administradores.";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
            }
        }

        protected void btnPagar_Click(object sender, EventArgs e)
        {
            try
            {
                HeaderClubPago headerClubPago = headerClubPagoServices.ObtenerHeaderClubPago();

                DateTime HoraDelServidor = DateTime.Now;
                DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

                decimal pago = decimal.Parse(txtMonto.Text) * 100;

                ConsultarReferenciasServices consultarReferenciasServices = new ConsultarReferenciasServices();
                ConsultarReferencia consultarReferecia = consultarReferenciasServices.ApiPagarReferenciaWeb("https://pagalaescuela.mx/Pagalaescuela/Service/PagoReferencia", "101", hoy.ToString("yyyy-MM-dd"), pago.ToString(), txtReferencia.Text, headerClubPago.UserAgent, decodificarService.Base64Encode(headerClubPago.XOrigin));

                txtCodigo.Text = consultarReferecia.codigo.ToString();
                txtMnsj.Text = consultarReferecia.mensaje;
                txtMonto.Text = txtMonto.Text;

                if (consultarReferecia.codigo == 0 && !string.IsNullOrEmpty(consultarReferecia.mensaje))
                {
                    Session["UltimaReferencia"] = txtReferencia.Text;

                    btnConsultar.Visible = true;
                    btnPagar.Visible = false;
                    btnCancelarPago.Visible = true;
                    txtMonto.ReadOnly = true;

                    txtCodigo.Text = string.Empty;
                    txtMnsj.Text = string.Empty;
                    txtMonto.Text = "0.00";
                    txtMonto.ReadOnly = false;
                    txtReferencia.Text = string.Empty;

                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "<b>Transacción exitosa.</b>";
                    divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                }
            }
            catch (Exception ex)
            {
                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "<b>Lo sentimos</b> ha ocurrido un error inesperado, por favor inténtelo mas tarde si el error persiste, comuníquese con los administradores.";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
            }
        }

        protected void btnCancelarPago_Click(object sender, EventArgs e)
        {
            try
            {
                HeaderClubPago headerClubPago = headerClubPagoServices.ObtenerHeaderClubPago();

                RefPagosClubPagoServices refPagosClubPagoServices = new RefPagosClubPagoServices();

                refPagosClubPagoServices.ObtenerPagoReferencia(Guid.Parse(ViewState["UidIntegracionLocal"].ToString()), Session["UltimaReferencia"].ToString());

                DateTime HoraDelServidor = DateTime.Now;
                DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

                string monto = string.Empty;
                string transaccion = string.Empty;
                string autorizacion = string.Empty;

                foreach (var item in refPagosClubPagoServices.lsRefPagosClubPago)
                {
                    monto = (item.DcmMonto * 100).ToString();
                    transaccion = item.VchTransaccion;
                    autorizacion = item.VchAutorizacion;
                }

                if (!string.IsNullOrEmpty(monto) && !string.IsNullOrEmpty(transaccion) && !string.IsNullOrEmpty(autorizacion))
                {

                    ConsultarReferenciasServices consultarReferenciasServices = new ConsultarReferenciasServices();
                    ConsultarReferencia consultarReferecia = consultarReferenciasServices.ApiCancelarPagarReferenciaWeb("https://pagalaescuela.mx/Pagalaescuela/Service/CancelaPago", hoy.ToString("yyyy-MM-dd"), monto, transaccion, Session["UltimaReferencia"].ToString(), autorizacion, headerClubPago.UserAgent, decodificarService.Base64Encode(headerClubPago.XOrigin));

                    txtCodigo.Text = consultarReferecia.codigo.ToString();
                    txtMnsj.Text = consultarReferecia.mensaje;
                    txtMonto.Text = txtMonto.Text;

                    if (consultarReferecia.codigo == 0 && !string.IsNullOrEmpty(consultarReferecia.mensaje))
                    {
                        btnConsultar.Visible = true;
                        btnPagar.Visible = false;
                        btnCancelarPago.Visible = false;
                        txtMonto.ReadOnly = true;

                        txtCodigo.Text = string.Empty;
                        txtMnsj.Text = string.Empty;
                        txtMonto.Text = "0.00";
                        txtMonto.ReadOnly = false;
                        txtReferencia.Text = string.Empty;

                        pnlAlert.Visible = true;
                        lblMensajeAlert.Text = "<b>Transacción exitosa.</b>";
                        divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                    }
                }
                else
                {
                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "<b>Lo sentimos</b> la referencia ya ha sido cancelada.";
                    divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                }
            }
            catch (Exception ex)
            {
                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "<b>Lo sentimos</b> ha ocurrido un error inesperado, por favor inténtelo mas tarde si el error persiste, comuníquese con los administradores.";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
            }
        }
    }
}