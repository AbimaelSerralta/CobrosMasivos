﻿using Franquicia.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PagaLaEscuela.Views
{
    public partial class ReporteLigasPadres : System.Web.UI.Page
    {
        LigasUrlsServices ligasUrlsServices = new LigasUrlsServices();
        PagosTarjetaServices pagosTarjetaServices = new PagosTarjetaServices();
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
                ViewState["gvLigasGeneradas"] = SortDirection.Ascending;

                Session["ReporteLigasUsuariosligasUrlsServices"] = ligasUrlsServices;
                Session["ReporteLigasUsuariospagosTarjetaServices"] = pagosTarjetaServices;

                ligasUrlsServices.ConsultarLigaPadres(Guid.Parse(ViewState["UidUsuarioLocal"].ToString()));
                gvLigasGeneradas.DataSource = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel;
                gvLigasGeneradas.DataBind();
            }
            else
            {
                ligasUrlsServices = (LigasUrlsServices)Session["ReporteLigasUsuariosligasUrlsServices"];
                pagosTarjetaServices = (PagosTarjetaServices)Session["ReporteLigasUsuariospagosTarjetaServices"];

                pnlAlert.Visible = false;
                lblMensajeAlert.Text = "";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade");
            }
        }

        protected void gvLigasGeneradas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "btnInfoMovimiento")
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvLigasGeneradas.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKey = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());

                int i = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel.IndexOf(ligasUrlsServices.lsLigasUsuariosFinalGridViewModel.First(x => x.UidLigaUrl == dataKey));
                Guid UidLigaAsociado = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel[i].UidLigaAsociado;

                if (UidLigaAsociado != Guid.Empty)
                {
                    pagosTarjetaServices.DetalleLigaPromocionUsuarioFinal(UidLigaAsociado);
                }
                else
                {
                    pagosTarjetaServices.DetalleLigaUsuarioFinal(dataKey);
                }

                gvDetalleLiga.DataSource = pagosTarjetaServices.lsPagTarjDetalUsFinalGridViewModel;
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
            gvLigasGeneradas.DataSource = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel;
            gvLigasGeneradas.DataBind();
        }

        protected void btnFiltros_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalBusqueda()", true);
        }

        protected void gvDetalleLiga_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDetalleLiga.PageIndex = e.NewPageIndex;
            gvDetalleLiga.DataSource = pagosTarjetaServices.lsPagTarjDetalUsFinalGridViewModel;
            gvDetalleLiga.DataBind();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtIdentificador.Text = string.Empty;
            txtAsunto.Text = string.Empty;
            txtConcepto.Text = string.Empty;
            txtImporteMayor.Text = string.Empty;
            txtImporteMenor.Text = string.Empty;
            ddlImporteMayor.SelectedIndex = -1;
            ddlImporteMenor.SelectedIndex = -1;
            txtRegistroDesde.Text = string.Empty;
            txtRegistroHasta.Text = string.Empty;
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

            ligasUrlsServices.BuscarLigasPadres(Guid.Parse(ViewState["UidUsuarioLocal"].ToString()), txtIdentificador.Text, txtAsunto.Text, txtConcepto.Text, ImporteMayor, ImporteMenor, txtRegistroDesde.Text, txtRegistroHasta.Text, txtVencimientoDesde.Text, txtVencimientoHasta.Text, ddlEstatus.SelectedValue);
            gvLigasGeneradas.DataSource = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel;
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
                            ligasUrlsServices.lsLigasUsuariosFinalGridViewModel = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel.OrderBy(x => x.VchIdentificador).ToList();
                        }
                        else
                        {
                            ligasUrlsServices.lsLigasUsuariosFinalGridViewModel = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel.OrderByDescending(x => x.VchIdentificador).ToList();
                        }
                        break;
                    case "VchNombreComercial":
                        if (Orden == "ASC")
                        {
                            ligasUrlsServices.lsLigasUsuariosFinalGridViewModel = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel.OrderBy(x => x.VchNombreComercial).ToList();
                        }
                        else
                        {
                            ligasUrlsServices.lsLigasUsuariosFinalGridViewModel = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel.OrderByDescending(x => x.VchNombreComercial).ToList();
                        }
                        break;
                    case "VchAsunto":
                        if (Orden == "ASC")
                        {
                            ligasUrlsServices.lsLigasUsuariosFinalGridViewModel = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel.OrderBy(x => x.VchAsunto).ToList();
                        }
                        else
                        {
                            ligasUrlsServices.lsLigasUsuariosFinalGridViewModel = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel.OrderByDescending(x => x.VchAsunto).ToList();
                        }
                        break;
                    case "VchConcepto":
                        if (Orden == "ASC")
                        {
                            ligasUrlsServices.lsLigasUsuariosFinalGridViewModel = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel.OrderBy(x => x.VchConcepto).ToList();
                        }
                        else
                        {
                            ligasUrlsServices.lsLigasUsuariosFinalGridViewModel = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel.OrderByDescending(x => x.VchConcepto).ToList();
                        }
                        break;
                    case "DcmImporte":
                        if (Orden == "ASC")
                        {
                            ligasUrlsServices.lsLigasUsuariosFinalGridViewModel = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel.OrderBy(x => x.DcmImporte).ToList();
                        }
                        else
                        {
                            ligasUrlsServices.lsLigasUsuariosFinalGridViewModel = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel.OrderByDescending(x => x.DcmImporte).ToList();
                        }
                        break;
                    case "DtVencimiento":
                        if (Orden == "ASC")
                        {
                            ligasUrlsServices.lsLigasUsuariosFinalGridViewModel = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel.OrderBy(x => x.DtVencimiento).ToList();
                        }
                        else
                        {
                            ligasUrlsServices.lsLigasUsuariosFinalGridViewModel = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel.OrderByDescending(x => x.DtVencimiento).ToList();
                        }
                        break;
                    case "VchPromocion":
                        if (Orden == "ASC")
                        {
                            ligasUrlsServices.lsLigasUsuariosFinalGridViewModel = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel.OrderBy(x => x.VchPromocion).ToList();
                        }
                        else
                        {
                            ligasUrlsServices.lsLigasUsuariosFinalGridViewModel = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel.OrderByDescending(x => x.VchPromocion).ToList();
                        }
                        break;
                    case "DcmImportePromocion":
                        if (Orden == "ASC")
                        {
                            ligasUrlsServices.lsLigasUsuariosFinalGridViewModel = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel.OrderBy(x => x.DcmImportePromocion).ToList();
                        }
                        else
                        {
                            ligasUrlsServices.lsLigasUsuariosFinalGridViewModel = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel.OrderByDescending(x => x.DcmImportePromocion).ToList();
                        }
                        break;
                    case "VchEstatus":
                        if (Orden == "ASC")
                        {
                            ligasUrlsServices.lsLigasUsuariosFinalGridViewModel = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel.OrderBy(x => x.VchEstatus).ToList();
                        }
                        else
                        {
                            ligasUrlsServices.lsLigasUsuariosFinalGridViewModel = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel.OrderByDescending(x => x.VchEstatus).ToList();
                        }
                        break;

                }

                gvLigasGeneradas.DataSource = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel;
                gvLigasGeneradas.DataBind();
            }
        }

        protected void btnExportarLista_Click(object sender, EventArgs e)
        {
            Session["lsLigasUrlsGridViewModel"] = ligasUrlsServices.lsLigasUrlsGridViewModel;
            string _open = "window.open('ExportarAExcelReporteLigas.aspx', '_blank');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
        }

        protected void btnActualizarLista_Click(object sender, EventArgs e)
        {
            ligasUrlsServices.ConsultarLigaUsuarioFinal(Guid.Parse(ViewState["UidUsuarioLocal"].ToString()));
            gvLigasGeneradas.DataSource = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel;
            gvLigasGeneradas.DataBind();

            pnlAlert.Visible = true;
            lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha sincronizado exitosamente.";
            divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
        }

    }
}