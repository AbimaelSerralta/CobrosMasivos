using Franquicia.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PagaLaEscuela.Views
{
    public partial class Pagos : System.Web.UI.Page
    {
        PagosPadresServices pagosPadresServices = new PagosPadresServices();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UidUsuarioMaster"] != null)
            {
                ViewState["UidUsuarioLocal"] = Session["UidUsuarioMaster"];
            }
            else
            {
                ViewState["UidUsuarioLocal"] = Guid.Empty;
            }

            if (!IsPostBack)
            {
                pagosPadresServices.CargarComercios(Guid.Parse(ViewState["UidUsuarioLocal"].ToString()));
                rpComercios.DataSource = pagosPadresServices.lsPadresComerciosViewModels;
                rpComercios.DataBind();
            }
            else
            {

            }
        }

        protected void rpComercios_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Pagos")
            {
                string Id = (string)e.CommandArgument;

                pnlComercios.Visible = false;
                pnlPagos.Visible = true;
            }
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            pnlComercios.Visible = true;
            pnlPagos.Visible = false;
        }
    }
}