using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Franquicia.WebForms
{
    public partial class Pago : System.Web.UI.Page
    {
        public string URLBase { get; private set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Id"] != null)
            {
                if (Request.QueryString["Id"] != null)
                {
                    Response.Redirect(URLBase + "Views/Pagos.aspx?Id=" + Request.QueryString["Id"].ToString());
                }
            }
            else
            {
                Response.Redirect(URLBase + "Views/Pagos.aspx");
            }
        }
    }
}