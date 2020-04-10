using Franquicia.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Franquicia.WebForms.Views
{
    public partial class Promociones : System.Web.UI.Page
    {
        PromocionesServices promocionesServices = new PromocionesServices();
        ValidacionesServices validacionesServices = new ValidacionesServices();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["CodigoPromocion"] != null && Request.QueryString["CodigoLiga"] != null)
                {
                    promocionesServices.ValidarPromociones(Guid.Parse(Request.QueryString["CodigoPromocion"]));
                    //rpPromociones.DataSource = promocionesServices.lsLigasUrlsPromocionesModel;
                    //rpPromociones.DataBind();

                    string url = string.Empty;

                    if (promocionesServices.lsLigasUrlsPromocionesModel.Count >= 1)
                    {
                        int index = promocionesServices.lsLigasUrlsPromocionesModel.IndexOf(promocionesServices.lsLigasUrlsPromocionesModel.First(x => x.IdReferencia == Request.QueryString["CodigoLiga"].ToString()));
                        url = promocionesServices.lsLigasUrlsPromocionesModel[index].VchUrl;

                        if (validacionesServices.LigaAsociadoPagado(Guid.Parse(Request.QueryString["CodigoPromocion"].ToString())))
                        {
                            lblMnsj.Text = "Esta liga ya se ha pagado";
                        }
                        else
                        {
                            Response.Redirect(url);
                        }
                    }
                    else
                    {
                        lblMnsj.Text = "La liga no es válida";
                    }
                }
            }
        }
    }
}