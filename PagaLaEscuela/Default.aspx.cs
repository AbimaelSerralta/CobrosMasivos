using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PagaLaEscuela
{
    public partial class Default : System.Web.UI.Page
    {
        public string URLBase { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect(URLBase + "Views/Login.aspx");
        }
    }
}