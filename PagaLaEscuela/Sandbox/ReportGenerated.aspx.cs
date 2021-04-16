using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PagaLaEscuela.Sandbox
{
    public partial class ReportGenerated : System.Web.UI.Page
    {
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

            if (Session["UidCredencialMaster"] != null)
            {
                ViewState["UidCredencialLocal"] = Session["UidCredencialMaster"];
            }
            else
            {
                ViewState["UidCredencialLocal"] = Guid.Empty;
            }



            if (!IsPostBack)
            {
                
            }
            else
            {

            }

            

        }
    }
}