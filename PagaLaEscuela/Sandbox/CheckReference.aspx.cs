using Franquicia.Bussiness;
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
        protected void Page_Load(object sender, EventArgs e)
        {
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
            ValidacionesServices validacionesServices = new ValidacionesServices();

            btnConsultar.Visible = true;
            btnPagar.Visible = false;
            btnNuevoPago.Visible = false;

            var refInt = validacionesServices.ExisteReferenciaIntegracion(txtReferencia.Text);
            if (refInt.Item1)
            {
                EndPointClubPagoServices endPointClubPagoServices = new EndPointClubPagoServices();

                endPointClubPagoServices.ObtenerEndPointClubPago(refInt.Item2, Guid.Parse("609302B6-0DEE-4680-B6BF-D547FD09ED12"));

                ConsultarReferenciasServices consultarReferenciasServices = new ConsultarReferenciasServices();
                ConsultarReferencia consultarReferecia = consultarReferenciasServices.ApiConsultarReferenciaWeb("https://localhost:44352/Pagalaescuela/Service/ConsultaReferencia", txtReferencia.Text);

                txtCodigo.Text = consultarReferecia.codigo.ToString();
                txtMnsj.Text = consultarReferecia.mensaje;
                txtMonto.Text = (decimal.Parse(consultarReferecia.monto) / 100).ToString("N2");
                txtMonto.ReadOnly = false;

                if (consultarReferecia.codigo == 0)
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

        protected void btnPagar_Click(object sender, EventArgs e)
        {
            btnNuevoPago.Visible = false;

            DateTime HoraDelServidor = DateTime.Now;
            DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

            decimal pago = decimal.Parse(txtMonto.Text) * 100;

            ConsultarReferenciasServices consultarReferenciasServices = new ConsultarReferenciasServices();
            ConsultarReferencia consultarReferecia = consultarReferenciasServices.ApiPagarReferenciaWeb("https://localhost:44352/Pagalaescuela/Service/PagoReferencia", "101", hoy.ToString("yyyy-MM-dd"), pago.ToString(), txtReferencia.Text);

            txtCodigo.Text = consultarReferecia.codigo.ToString();
            txtMnsj.Text = consultarReferecia.mensaje;
            txtMonto.Text = txtMonto.Text;

            if (consultarReferecia.codigo == 0)
            {
                btnConsultar.Visible = false;
                btnPagar.Visible = false;
                btnNuevoPago.Visible = true;
                txtMonto.ReadOnly = true;

                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "<b>Felicidades,</b> su pago pago se ha realizado exitosamente.";
                divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
            }
        }

        protected void btnNuevoPago_Click(object sender, EventArgs e)
        {
            txtCodigo.Text = string.Empty;
            txtMnsj.Text = string.Empty;
            txtMonto.Text = "0.00";
            txtMonto.ReadOnly = false;
            txtReferencia.Text = string.Empty;
        }
    }
}