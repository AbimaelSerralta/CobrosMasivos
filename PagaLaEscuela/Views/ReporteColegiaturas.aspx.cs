using Franquicia.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PagaLaEscuela.Views
{
    public partial class ReporteColegiaturas : System.Web.UI.Page
    {
        ColegiaturasServices colegiaturasServices = new ColegiaturasServices();
        AlumnosServices alumnosServices = new AlumnosServices();
        EstatusFechasColegiaturasServices estatusFechasColegiaturasServices = new EstatusFechasColegiaturasServices();
        EstatusColegiaturasAlumnosServices estatusColegiaturasAlumnosServices = new EstatusColegiaturasAlumnosServices();

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
                ViewState["gvPagos"] = SortDirection.Descending;

                Session["colegiaturasServices"] = colegiaturasServices;

                colegiaturasServices.CargarReporteColegiaturas(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                gvPagos.DataSource = colegiaturasServices.lsPagosColegiaturasViewModel;
                gvPagos.DataBind();

                estatusFechasColegiaturasServices.CargarEstatusFechasColegiaturas();
                LBFiltroEstatusCole.DataSource = estatusFechasColegiaturasServices.lsEstatusFechasColegiaturas;
                LBFiltroEstatusCole.DataTextField = "VchDescripcion";
                LBFiltroEstatusCole.DataValueField = "UidEstatusFechaColegiatura";
                LBFiltroEstatusCole.DataBind();
                foreach (ListItem item in LBFiltroEstatusCole.Items) { if (item.Value == "76c8793b-4493-44c8-b274-696a61358bdf") { item.Selected = true; } }

                estatusColegiaturasAlumnosServices.CargarEstatusColegiaturasAlumnos();
                LBFiltroEstatusPago.DataSource = estatusColegiaturasAlumnosServices.lsEstatusColegiaturasAlumnos;
                LBFiltroEstatusPago.DataTextField = "VchDescripcion";
                LBFiltroEstatusPago.DataValueField = "UidEstatusColeAlumnos";
                LBFiltroEstatusPago.DataBind();
            }
            else
            {
                colegiaturasServices = (ColegiaturasServices)Session["colegiaturasServices"];

                pnlAlert.Visible = false;
                lblMensajeAlert.Text = "";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade");
            }
        }

        protected void btnFiltros_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalBusqueda()", true);
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            DateTime HoraDelServidor = DateTime.Now;
            DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

            decimal ImporteMayor = 0;
            decimal ImporteMenor = 0;

            //if (txtImporteMayor.Text != string.Empty)
            //{
            //    switch (ddlImporteMayor.SelectedValue)
            //    {
            //        case ">":
            //            ImporteMayor = Convert.ToDecimal(txtImporteMayor.Text) + 1;
            //            break;
            //        case ">=":
            //            ImporteMayor = Convert.ToDecimal(txtImporteMayor.Text);
            //            break;
            //    }
            //}
            //if (txtImporteMenor.Text != string.Empty)
            //{
            //    switch (ddlImporteMenor.SelectedValue)
            //    {
            //        case "<":
            //            ImporteMenor = Convert.ToDecimal(txtImporteMenor.Text) - 1;
            //            break;
            //        case "<=":
            //            ImporteMenor = Convert.ToDecimal(txtImporteMenor.Text);
            //            break;
            //    }
            //}

            ViewState["NewPageIndex"] = null;

            colegiaturasServices.BuscarColegiaturaPadre(Guid.Parse(ViewState["ItemCommand-UidCliente"].ToString()), Guid.Parse(ViewState["UidUsuarioLocal"].ToString()), hoy, GetItemListBox(LBFiltroAlumnos), txtColegiatura.Text, txtNumPago.Text, GetItemListBox(LBFiltroEstatusCole), GetItemListBox(LBFiltroEstatusPago));
            gvPagos.DataSource = colegiaturasServices.lsPagosColegiaturasViewModel;
            gvPagos.DataBind();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModalBusqueda()", true);
        }
        private string GetItemListBox(ListBox listBox)
        {
            string Values = string.Empty;
            foreach (ListItem item in listBox.Items)
            {
                if (item.Selected)
                {
                    if (Values == string.Empty)
                        Values = item.Value;
                    else
                        Values += "," + item.Value;
                }
            }
            return Values;
        }
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtColegiatura.Text = string.Empty;
            txtNumPago.Text = string.Empty;
            foreach (ListItem item in LBFiltroEstatusCole.Items) { if (item.Value == "76c8793b-4493-44c8-b274-696a61358bdf") { item.Selected = true; } else { item.Selected = false; } }
            LBFiltroEstatusPago.SelectedIndex = -1;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "MultFiltro", "multiFiltro()", true);
        }

        protected void btnExportarLista_Click(object sender, EventArgs e)
        {
            Session["lsPagosColegiaturasViewModel"] = colegiaturasServices.lsPagosColegiaturasViewModel;
            string _open = "window.open('Excel/ExportarAExcelReporteColegiaturas.aspx', '_blank');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
        }
        protected void btnActualizarLista_Click(object sender, EventArgs e)
        {
            colegiaturasServices.CargarReporteColegiaturas(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
            gvPagos.DataSource = colegiaturasServices.lsPagosColegiaturasViewModel;
            gvPagos.DataBind();

            pnlAlert.Visible = true;
            lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha sincronizado exitosamente.";
            divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
        }

        protected void gvPagos_Sorting(object sender, GridViewSortEventArgs e)
        {
            string SortExpression = e.SortExpression;
            SortDirection direccion;
            string Orden = string.Empty;

            if (ViewState["gvPagos"] != null)
            {
                direccion = (SortDirection)ViewState["gvPagos"];
                if (direccion == SortDirection.Ascending)
                {
                    ViewState["gvPagos"] = SortDirection.Descending;
                    Orden = "ASC";
                }
                else
                {
                    ViewState["gvPagos"] = SortDirection.Ascending;
                    Orden = "DESC";
                }

                switch (SortExpression)
                {
                    case "VchIdentificador":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosColegiaturasViewModel = colegiaturasServices.lsPagosColegiaturasViewModel.OrderBy(x => x.VchIdentificador).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosColegiaturasViewModel = colegiaturasServices.lsPagosColegiaturasViewModel.OrderByDescending(x => x.VchIdentificador).ToList();
                        }
                        break;
                    case "VchNum":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosColegiaturasViewModel = colegiaturasServices.lsPagosColegiaturasViewModel.OrderBy(x => x.VchNum).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosColegiaturasViewModel = colegiaturasServices.lsPagosColegiaturasViewModel.OrderByDescending(x => x.VchNum).ToList();
                        }
                        break;
                    case "DcmImporte":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosColegiaturasViewModel = colegiaturasServices.lsPagosColegiaturasViewModel.OrderBy(x => x.DcmImporte).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosColegiaturasViewModel = colegiaturasServices.lsPagosColegiaturasViewModel.OrderByDescending(x => x.DcmImporte).ToList();
                        }
                        break;
                    case "ImpPagado":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosColegiaturasViewModel = colegiaturasServices.lsPagosColegiaturasViewModel.OrderBy(x => x.ImpPagado).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosColegiaturasViewModel = colegiaturasServices.lsPagosColegiaturasViewModel.OrderByDescending(x => x.ImpPagado).ToList();
                        }
                        break;
                    case "ImpTotal":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosColegiaturasViewModel = colegiaturasServices.lsPagosColegiaturasViewModel.OrderBy(x => x.ImpTotal).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosColegiaturasViewModel = colegiaturasServices.lsPagosColegiaturasViewModel.OrderByDescending(x => x.ImpTotal).ToList();
                        }
                        break;
                    case "DtFHInicio":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosColegiaturasViewModel = colegiaturasServices.lsPagosColegiaturasViewModel.OrderBy(x => x.DtFHInicio).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosColegiaturasViewModel = colegiaturasServices.lsPagosColegiaturasViewModel.OrderByDescending(x => x.DtFHInicio).ToList();
                        }
                        break;
                    case "VchEstatusFechas":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosColegiaturasViewModel = colegiaturasServices.lsPagosColegiaturasViewModel.OrderBy(x => x.VchEstatusFechas).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosColegiaturasViewModel = colegiaturasServices.lsPagosColegiaturasViewModel.OrderByDescending(x => x.VchEstatusFechas).ToList();
                        }
                        break;
                    case "EstatusPago":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosColegiaturasViewModel = colegiaturasServices.lsPagosColegiaturasViewModel.OrderBy(x => x.EstatusPago).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosColegiaturasViewModel = colegiaturasServices.lsPagosColegiaturasViewModel.OrderByDescending(x => x.EstatusPago).ToList();
                        }
                        break;
                }

                ViewState["NewPageIndex"] = int.Parse(ViewState["NewPageIndex"].ToString()) - 1;
                gvPagos.DataSource = colegiaturasServices.lsPagosColegiaturasViewModel;
                gvPagos.DataBind();
            }
        }
        protected void gvPagos_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void gvPagos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPagos.PageIndex = e.NewPageIndex;
            ViewState["NewPageIndex"] = e.NewPageIndex;
            gvPagos.DataSource = colegiaturasServices.lsPagosColegiaturasViewModel;
            gvPagos.DataBind();
        }
        protected void gvPagos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblPaginado = (Label)e.Row.FindControl("lblPaginado");

                int PageSize = gvPagos.PageSize;
                int antNum = 0;

                int numTotal = colegiaturasServices.lsPagosColegiaturasViewModel.Count;

                if (numTotal >= 1)
                {
                    if (ViewState["NewPageIndex"] != null)
                    {
                        int gh = int.Parse(ViewState["NewPageIndex"].ToString());
                        ViewState["NewPageIndex"] = gh + 1;

                        int r1 = int.Parse(ViewState["NewPageIndex"].ToString()) * PageSize;
                        antNum = r1 - (PageSize - 1);
                    }
                    else
                    {
                        ViewState["NewPageIndex"] = 1;
                        antNum = 1;
                    }

                    int NewPageIndex = int.Parse(ViewState["NewPageIndex"].ToString());

                    int newNum = NewPageIndex * PageSize;

                    if (numTotal >= newNum)
                    {
                        lblPaginado.Text = "Del " + antNum + " al " + newNum + " de " + numTotal;
                    }
                    else
                    {
                        lblPaginado.Text = "Del " + antNum + " al " + numTotal + " de " + numTotal;
                    }

                    ViewState["lblPaginado"] = lblPaginado.Text;
                }
                else
                {
                    lblPaginado.Text = ViewState["lblPaginado"].ToString();
                }
            }
        }
    }
}