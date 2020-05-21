using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Franquicia.WebForms
{
    public partial class Evento : System.Web.UI.Page
    {
        public string URLBase { get; private set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Id"] != null || Request.QueryString["n"] != null)
            {
                if (Request.QueryString["Id"] != null)
                {
                    Response.Redirect(URLBase + "Views/Eventos.aspx?Id=" + Request.QueryString["Id"].ToString());
                }
                else if (Request.QueryString["n"] != null)
                {
                    Response.Redirect(URLBase + "Views/Eventos.aspx?n=" + Request.QueryString["n"].ToString());
                }
            }
        }
    }
}