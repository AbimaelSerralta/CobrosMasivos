using Franquicia.Bussiness.IntegracionesPraga;
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
        PagosIntegracionServices pagosIntegracionServices = new PagosIntegracionServices();

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
                ViewState["gvReporteGeneradas"] = SortDirection.Descending;

                Session["pagosIntegracionServices"] = pagosIntegracionServices;

                DateTime HoraDelServidor = DateTime.Now;
                DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

                txtFiltroDesde.Text = hoy.ToString("yyyy-MM-dd");
                txtFiltroHasta.Text = hoy.ToString("yyyy-MM-dd");

                btnBuscar_Click(null, null);

            }
            else
            {
                pagosIntegracionServices = (PagosIntegracionServices)Session["pagosIntegracionServices"];
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            pagosIntegracionServices.BuscarReporteGenerado(txtFiltroDesde.Text, txtFiltroHasta.Text, txtFiltroReferencia.Text, Guid.Parse(ViewState["UidCredencialLocal"].ToString()));
            gvReporteGeneradas.DataSource = pagosIntegracionServices.lsReporteGeneradosGridViewModel;
            gvReporteGeneradas.DataBind();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            DateTime HoraDelServidor = DateTime.Now;
            DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

            txtFiltroDesde.Text = hoy.ToString("yyyy-MM-dd");
            txtFiltroHasta.Text = hoy.ToString("yyyy-MM-dd");
            txtFiltroReferencia.Text = string.Empty;
        }

        protected void gvReporteGeneradas_Sorting(object sender, GridViewSortEventArgs e)
        {
            string SortExpression = e.SortExpression;
            SortDirection direccion;
            string Orden = string.Empty;

            if (ViewState["gvReporteGeneradas"] != null)
            {
                direccion = (SortDirection)ViewState["gvReporteGeneradas"];
                if (direccion == SortDirection.Ascending)
                {
                    ViewState["gvReporteGeneradas"] = SortDirection.Descending;
                    Orden = "ASC";
                }
                else
                {
                    ViewState["gvReporteGeneradas"] = SortDirection.Ascending;
                    Orden = "DESC";
                }

                switch (SortExpression)
                {
                    case "DtRegistro":
                        if (Orden == "ASC")
                        {
                            pagosIntegracionServices.lsReporteGeneradosGridViewModel = pagosIntegracionServices.lsReporteGeneradosGridViewModel.OrderBy(x => x.DtRegistro).ToList();
                        }
                        else
                        {
                            pagosIntegracionServices.lsReporteGeneradosGridViewModel = pagosIntegracionServices.lsReporteGeneradosGridViewModel.OrderByDescending(x => x.DtRegistro).ToList();
                        }
                        break;
                    case "IdReferencia":
                        if (Orden == "ASC")
                        {
                            pagosIntegracionServices.lsReporteGeneradosGridViewModel = pagosIntegracionServices.lsReporteGeneradosGridViewModel.OrderBy(x => x.IdReferencia).ToList();
                        }
                        else
                        {
                            pagosIntegracionServices.lsReporteGeneradosGridViewModel = pagosIntegracionServices.lsReporteGeneradosGridViewModel.OrderByDescending(x => x.IdReferencia).ToList();
                        }
                        break;
                    case "DcmImporte":
                        if (Orden == "ASC")
                        {
                            pagosIntegracionServices.lsReporteGeneradosGridViewModel = pagosIntegracionServices.lsReporteGeneradosGridViewModel.OrderBy(x => x.DcmImporte).ToList();
                        }
                        else
                        {
                            pagosIntegracionServices.lsReporteGeneradosGridViewModel = pagosIntegracionServices.lsReporteGeneradosGridViewModel.OrderByDescending(x => x.DcmImporte).ToList();
                        }
                        break;
                    case "DtVencimiento":
                        if (Orden == "ASC")
                        {
                            pagosIntegracionServices.lsReporteGeneradosGridViewModel = pagosIntegracionServices.lsReporteGeneradosGridViewModel.OrderBy(x => x.DtVencimiento).ToList();
                        }
                        else
                        {
                            pagosIntegracionServices.lsReporteGeneradosGridViewModel = pagosIntegracionServices.lsReporteGeneradosGridViewModel.OrderByDescending(x => x.DtVencimiento).ToList();
                        }
                        break;
                    case "VchFormaPago":
                        if (Orden == "ASC")
                        {
                            pagosIntegracionServices.lsReporteGeneradosGridViewModel = pagosIntegracionServices.lsReporteGeneradosGridViewModel.OrderBy(x => x.VchFormaPago).ToList();
                        }
                        else
                        {
                            pagosIntegracionServices.lsReporteGeneradosGridViewModel = pagosIntegracionServices.lsReporteGeneradosGridViewModel.OrderByDescending(x => x.VchFormaPago).ToList();
                        }
                        break;
                }

                gvReporteGeneradas.DataSource = pagosIntegracionServices.lsReporteGeneradosGridViewModel;
                gvReporteGeneradas.DataBind();
            }
        }
    }
}