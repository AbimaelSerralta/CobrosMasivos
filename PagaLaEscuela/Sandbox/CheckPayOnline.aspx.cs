using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PagaLaEscuela.Sandbox
{
    public partial class CheckPayOnline : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
            else
            {

            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUrlPrag.Text))
            {
                ifmLiga.Src = txtUrlPrag.Text;
                ifmLiga.Visible = true;
            }
            else
            {
                ifmLiga.Visible = false;
            }
        }
    }
}