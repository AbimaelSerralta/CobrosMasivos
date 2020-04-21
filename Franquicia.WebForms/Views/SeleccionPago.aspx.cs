using Franquicia.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Franquicia.WebForms.Views
{
    public partial class SeleccionPago : System.Web.UI.Page
    {
        LigasUrlsServices ligasUrlsService = new LigasUrlsServices();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["CodigoPromocion"] != null && Request.QueryString["CodigoLiga"] != null)
                {
                    ligasUrlsService.ContruirLiga(Guid.Parse(Request.QueryString["CodigoPromocion"].ToString()), Request.QueryString["CodigoLiga"].ToString());

                    lblNombreComp.Text = ligasUrlsService.ligasUrlsRepository.ligasUrlsConstruirLigaModel.NombreCompleto;
                    lblNombreComercial.Text = ligasUrlsService.ligasUrlsRepository.ligasUrlsConstruirLigaModel.VchNombreComercial;
                    lblConcepto.Text = ligasUrlsService.ligasUrlsRepository.ligasUrlsConstruirLigaModel.VchConcepto;
                    lblImporte.Text = ligasUrlsService.ligasUrlsRepository.ligasUrlsConstruirLigaModel.DcmImporte.ToString("N2");
                    lblVencimiento.Text = ligasUrlsService.ligasUrlsRepository.ligasUrlsConstruirLigaModel.DtVencimiento.ToString("dd/MM/yyyy");

                    aPagar.HRef = ligasUrlsService.ligasUrlsRepository.ligasUrlsConstruirLigaModel.VchUrl;
                }
            }
        }
    }
}