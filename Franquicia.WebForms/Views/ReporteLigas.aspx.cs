using Franquicia.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Franquicia.WebForms.Views
{
    public partial class ReporteLigas : System.Web.UI.Page
    {
        LigasUrlsServices ligasUrlsServices = new LigasUrlsServices();
        PagosTarjetaServices pagosTarjetaServices = new PagosTarjetaServices();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UidClienteMaster"] != null)
            {
                ViewState["UidClienteLocal"] = Session["UidClienteMaster"];
            }
            else
            {
                ViewState["UidClienteLocal"] = Guid.Empty;
            }

            if (!IsPostBack)
            {
                ViewState["gvLigasGeneradas"] = SortDirection.Ascending;
                
                Session["ligasUrlsServices"] = ligasUrlsServices;
                Session["pagosTarjetaServices"] = pagosTarjetaServices;

                ligasUrlsServices.ConsultarEstatusLiga(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                gvLigasGeneradas.DataSource = ligasUrlsServices.lsLigasUrlsGridViewModel;
                gvLigasGeneradas.DataBind();
            }
            else
            {
                ligasUrlsServices = (LigasUrlsServices)Session["ligasUrlsServices"];
                pagosTarjetaServices = (PagosTarjetaServices)Session["pagosTarjetaServices"];
            }
        }

        protected void gvLigasGeneradas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "btnInfoMovimiento")
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvLigasGeneradas.Rows[index];
                GridView valor = (GridView)sender;
                string dataKey = valor.DataKeys[Seleccionado.RowIndex].Value.ToString();

                int i = ligasUrlsServices.lsLigasUrlsGridViewModel.IndexOf(ligasUrlsServices.lsLigasUrlsGridViewModel.First(x => x.IdReferencia == dataKey));
                Guid UidLigaAsociado = ligasUrlsServices.lsLigasUrlsGridViewModel[i].UidLigaAsociado;

                if (UidLigaAsociado != Guid.Empty)
                {
                    pagosTarjetaServices.DetalleLigaPromocion(UidLigaAsociado);
                }
                else
                {
                    pagosTarjetaServices.DetalleLiga(dataKey);
                }

                gvDetalleLiga.DataSource = pagosTarjetaServices.lsPagosTarjetaDetalleGridViewModel;
                gvDetalleLiga.DataBind();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalDetalle()", true);
            }
        }

        protected void gvLigasGeneradas_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvLigasGeneradas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLigasGeneradas.PageIndex = e.NewPageIndex;
            gvLigasGeneradas.DataSource = ligasUrlsServices.lsLigasUrlsGridViewModel;
            gvLigasGeneradas.DataBind();
        }

        protected void btnFiltros_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalBusqueda()", true);
        }

        protected void gvDetalleLiga_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDetalleLiga.PageIndex = e.NewPageIndex;
            gvDetalleLiga.DataSource = pagosTarjetaServices.lsPagosTarjetaDetalleGridViewModel;
            gvDetalleLiga.DataBind();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtIdentificador.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtApePaterno.Text = string.Empty;
            txtApeMaterno.Text = string.Empty;
            txtAsunto.Text = string.Empty;
            txtConcepto.Text = string.Empty;
            txtImporteMayor.Text = string.Empty;
            txtImporteMenor.Text = string.Empty;
            ddlImporteMayor.SelectedIndex = -1;
            ddlImporteMenor.SelectedIndex = -1;
            txtVencimientoDesde.Text = string.Empty;
            txtVencimientoHasta.Text = string.Empty;
            ddlEstatus.SelectedIndex = -1;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            decimal ImporteMayor = 0;
            decimal ImporteMenor = 0;

            if (txtImporteMayor.Text != string.Empty)
            {
                switch (ddlImporteMayor.SelectedValue)
                {
                    case ">":
                        ImporteMayor = Convert.ToDecimal(txtImporteMayor.Text) + 1;
                        break;
                    case ">=":
                        ImporteMayor = Convert.ToDecimal(txtImporteMayor.Text);
                        break;
                }
            }
            if (txtImporteMenor.Text != string.Empty)
            {
                switch (ddlImporteMenor.SelectedValue)
                {
                    case "<":
                        ImporteMenor = Convert.ToDecimal(txtImporteMenor.Text) - 1;
                        break;
                    case "<=":
                        ImporteMenor = Convert.ToDecimal(txtImporteMenor.Text);
                        break;
                }
            }

            ligasUrlsServices.BuscarLigas(Guid.Parse(ViewState["UidClienteLocal"].ToString()), txtIdentificador.Text, txtNombre.Text, txtApePaterno.Text, txtApeMaterno.Text, txtAsunto.Text, txtConcepto.Text, ImporteMayor, ImporteMenor, txtRegistroDesde.Text, txtRegistroHasta.Text, txtVencimientoDesde.Text, txtVencimientoHasta.Text, ddlEstatus.SelectedValue);
            gvLigasGeneradas.DataSource = ligasUrlsServices.lsLigasUrlsGridViewModel;
            gvLigasGeneradas.DataBind();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModalBusqueda()", true);
        }

        protected void gvLigasGeneradas_Sorting(object sender, GridViewSortEventArgs e)
        {
            string SortExpression = e.SortExpression;
            SortDirection direccion;
            string Orden = string.Empty;

            if (ViewState["gvLigasGeneradas"] != null)
            {
                direccion = (SortDirection)ViewState["gvLigasGeneradas"];
                if (direccion == SortDirection.Ascending)
                {
                    ViewState["gvLigasGeneradas"] = SortDirection.Descending;
                    Orden = "ASC";
                }
                else
                {
                    ViewState["gvLigasGeneradas"] = SortDirection.Ascending;
                    Orden = "DESC";
                }

                switch (SortExpression)
                {
                    case "VchIdentificador":
                        if (Orden == "ASC")
                        {
                            ligasUrlsServices.lsLigasUrlsGridViewModel = ligasUrlsServices.lsLigasUrlsGridViewModel.OrderBy(x => x.VchIdentificador).ToList();
                        }
                        else
                        {
                            ligasUrlsServices.lsLigasUrlsGridViewModel = ligasUrlsServices.lsLigasUrlsGridViewModel.OrderByDescending(x => x.VchIdentificador).ToList();
                        }
                        break;
                    case "VchUrl":
                        if (Orden == "ASC")
                        {
                            ligasUrlsServices.lsLigasUrlsGridViewModel = ligasUrlsServices.lsLigasUrlsGridViewModel.OrderBy(x => x.VchUrl).ToList();
                        }
                        else
                        {
                            ligasUrlsServices.lsLigasUrlsGridViewModel = ligasUrlsServices.lsLigasUrlsGridViewModel.OrderByDescending(x => x.VchUrl).ToList();
                        }
                        break;
                    case "VchAsunto":
                        if (Orden == "ASC")
                        {
                            ligasUrlsServices.lsLigasUrlsGridViewModel = ligasUrlsServices.lsLigasUrlsGridViewModel.OrderBy(x => x.VchAsunto).ToList();
                        }
                        else
                        {
                            ligasUrlsServices.lsLigasUrlsGridViewModel = ligasUrlsServices.lsLigasUrlsGridViewModel.OrderByDescending(x => x.VchAsunto).ToList();
                        }
                        break;
                    case "VchConcepto":
                        if (Orden == "ASC")
                        {
                            ligasUrlsServices.lsLigasUrlsGridViewModel = ligasUrlsServices.lsLigasUrlsGridViewModel.OrderBy(x => x.VchConcepto).ToList();
                        }
                        else
                        {
                            ligasUrlsServices.lsLigasUrlsGridViewModel = ligasUrlsServices.lsLigasUrlsGridViewModel.OrderByDescending(x => x.VchConcepto).ToList();
                        }
                        break;
                    case "DcmImporte":
                        if (Orden == "ASC")
                        {
                            ligasUrlsServices.lsLigasUrlsGridViewModel = ligasUrlsServices.lsLigasUrlsGridViewModel.OrderBy(x => x.DcmImporte).ToList();
                        }
                        else
                        {
                            ligasUrlsServices.lsLigasUrlsGridViewModel = ligasUrlsServices.lsLigasUrlsGridViewModel.OrderByDescending(x => x.DcmImporte).ToList();
                        }
                        break;
                    case "DtVencimiento":
                        if (Orden == "ASC")
                        {
                            ligasUrlsServices.lsLigasUrlsGridViewModel = ligasUrlsServices.lsLigasUrlsGridViewModel.OrderBy(x => x.DtVencimiento).ToList();
                        }
                        else
                        {
                            ligasUrlsServices.lsLigasUrlsGridViewModel = ligasUrlsServices.lsLigasUrlsGridViewModel.OrderByDescending(x => x.DtVencimiento).ToList();
                        }
                        break;
                    case "VchEstatus":
                        if (Orden == "ASC")
                        {
                            ligasUrlsServices.lsLigasUrlsGridViewModel = ligasUrlsServices.lsLigasUrlsGridViewModel.OrderBy(x => x.VchEstatus).ToList();
                        }
                        else
                        {
                            ligasUrlsServices.lsLigasUrlsGridViewModel = ligasUrlsServices.lsLigasUrlsGridViewModel.OrderByDescending(x => x.VchEstatus).ToList();
                        }
                        break;
                }

                gvLigasGeneradas.DataSource = ligasUrlsServices.lsLigasUrlsGridViewModel;
                gvLigasGeneradas.DataBind();
            }
        }

        protected void btnExportarLista_Click(object sender, EventArgs e)
        {
            Session["lsLigasUrlsGridViewModel"] = ligasUrlsServices.lsLigasUrlsGridViewModel;
            string _open = "window.open('ExportarAExcelReporteLigas.aspx', '_blank');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
        }
    }
}