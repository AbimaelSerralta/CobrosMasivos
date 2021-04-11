using Franquicia.Bussiness.IntegracionesClubPago;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PagaLaEscuela.Sandbox
{
    public partial class EndPoint : System.Web.UI.Page
    {
        EndPointClubPagoServices EndPointClubPagoServices = new EndPointClubPagoServices();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                foreach (var item in EndPointClubPagoServices.ObtenerEndPointClubPagoSandboxWeb(101, Guid.Parse("DA801181-6975-41C1-A615-CF9F50580369")))
                {
                    if (Guid.Parse("5E6CCA56-B547-4F18-ABDD-C50D394923B3") == item.UidTipoEndPoint)
                    {
                        txtEntregarReferencia.Text = item.VchEndPoint;
                    }
                    if (Guid.Parse("609302B6-0DEE-4680-B6BF-D547FD09ED12") == item.UidTipoEndPoint)
                    {
                        txtConsultarReferencia.Text = item.VchEndPoint;
                    }
                    if (Guid.Parse("65341240-E22B-49B4-88B5-792B80E13F97") == item.UidTipoEndPoint)
                    {
                        txtPagarReferencia.Text = item.VchEndPoint;
                    }
                    if (Guid.Parse("D8903F55-B478-452D-ACC9-A7F0524C687B") == item.UidTipoEndPoint)
                    {
                        txtCancelarReferencia.Text = item.VchEndPoint;
                    }
                }

                foreach (var itemPraga in EndPointClubPagoServices.ObtenerEndPointPragaSandboxWeb(101, Guid.Parse("DA801181-6975-41C1-A615-CF9F50580369")))
                {
                    if (Guid.Parse("1F3A8ECB-806A-4970-958C-5360E2BB1009") == itemPraga.UidTipoEndPoint)
                    {
                        txtEntregarLiga.Text = itemPraga.VchEndPoint;
                    }
                    if (Guid.Parse("2C1854EA-00BC-474E-BF3E-F8395819DE53") == itemPraga.UidTipoEndPoint)
                    {
                        txtPagarLiga.Text = itemPraga.VchEndPoint;
                    }
                }
            }
            else
            {

            }
        }
    }
}