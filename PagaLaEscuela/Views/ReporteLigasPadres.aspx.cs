using Franquicia.Bussiness;
using Franquicia.Domain.Models;
using Franquicia.Domain.ViewModels;
using Franquicia.WebForms.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Franquicia.Bussiness.DateTimeSpanServices;

namespace PagaLaEscuela.Views
{
    public partial class ReporteLigasPadres : System.Web.UI.Page
    {
        LigasUrlsServices ligasUrlsServices = new LigasUrlsServices();
        PagosTarjetaServices pagosTarjetaServices = new PagosTarjetaServices();
        ColegiaturasServices colegiaturasServices = new ColegiaturasServices();
        PagosColegiaturasServices pagosColegiaturasServices = new PagosColegiaturasServices();
        DetallesPagosColegiaturasServices detallesPagosColegiaturasServices = new DetallesPagosColegiaturasServices();
        FormasPagosServices formasPagosServices = new FormasPagosServices();
        ComisionesTarjetasClientesServices comisionesTarjetasCl = new ComisionesTarjetasClientesServices();
        BancosServices bancosServices = new BancosServices();
        PagosManualesServices pagosManualesServices = new PagosManualesServices();
        CorreosEscuelaServices correosEscuelaServices = new CorreosEscuelaServices();
        ValidacionesServices validacionesServices = new ValidacionesServices();
        PagosServices pagosServices = new PagosServices();
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
                Session["ReporteLigasUsuarioscolegiaturasServices"] = colegiaturasServices;
                Session["ReporteLigasUsuariospagosColegiaturasServices"] = pagosColegiaturasServices;
                Session["ReporteLigasUsuariosformasPagosServices"] = formasPagosServices;
                Session["ReporteLigasUsuariosbancosServices"] = bancosServices;

                ligasUrlsServices.ConsultarLigaPadres(Guid.Parse(ViewState["UidUsuarioLocal"].ToString()));
                gvLigasGeneradas.DataSource = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel;
                gvLigasGeneradas.DataBind();

                colegiaturasServices.CargarPagosColegiaturasReporte(Guid.Parse(ViewState["UidUsuarioLocal"].ToString()));
                gvPagos.DataSource = colegiaturasServices.lsPagosColegiaturasViewModel;
                gvPagos.DataBind();
            }
            else
            {
                ligasUrlsServices = (LigasUrlsServices)Session["ReporteLigasUsuariosligasUrlsServices"];
                pagosTarjetaServices = (PagosTarjetaServices)Session["ReporteLigasUsuariospagosTarjetaServices"];
                colegiaturasServices = (ColegiaturasServices)Session["ReporteLigasUsuarioscolegiaturasServices"];
                pagosColegiaturasServices = (PagosColegiaturasServices)Session["ReporteLigasUsuariospagosColegiaturasServices"];
                formasPagosServices = (FormasPagosServices)Session["ReporteLigasUsuariosformasPagosServices"];
                bancosServices = (BancosServices)Session["ReporteLigasUsuariosbancosServices"];

                pnlAlert.Visible = false;
                lblMensajeAlert.Text = "";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade");

                pnlAlertModalTipoPago.Visible = false;
                lblMnsjModalTipoPago.Text = "";
                divAlertModalTipoPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade");

                rpFormasPago.DataSource = formasPagosServices.lsFormasPagos;
                rpFormasPago.DataBind();
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
            //ligasUrlsServices.ConsultarLigaPadres(Guid.Parse(ViewState["UidUsuarioLocal"].ToString()));
            //gvLigasGeneradas.DataSource = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel;
            //gvLigasGeneradas.DataBind();

            colegiaturasServices.CargarPagosColegiaturasReporte(Guid.Parse(ViewState["UidUsuarioLocal"].ToString()));
            gvPagos.DataSource = colegiaturasServices.lsPagosColegiaturasViewModel;
            gvPagos.DataBind();

            pnlAlert.Visible = true;
            lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha sincronizado exitosamente.";
            divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
        }

        #region GridViewPagos
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
                    case "VchMatricula":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosColegiaturasViewModel = colegiaturasServices.lsPagosColegiaturasViewModel.OrderBy(x => x.VchMatricula).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosColegiaturasViewModel = colegiaturasServices.lsPagosColegiaturasViewModel.OrderByDescending(x => x.VchMatricula).ToList();
                        }
                        break;
                    case "NombreCompleto":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosColegiaturasViewModel = colegiaturasServices.lsPagosColegiaturasViewModel.OrderBy(x => x.NombreCompleto).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosColegiaturasViewModel = colegiaturasServices.lsPagosColegiaturasViewModel.OrderByDescending(x => x.NombreCompleto).ToList();
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
                    case "VchFHLimite":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosColegiaturasViewModel = colegiaturasServices.lsPagosColegiaturasViewModel.OrderBy(x => x.VchFHLimite).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosColegiaturasViewModel = colegiaturasServices.lsPagosColegiaturasViewModel.OrderByDescending(x => x.VchFHLimite).ToList();
                        }
                        break;
                    case "VchFHVencimiento":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosColegiaturasViewModel = colegiaturasServices.lsPagosColegiaturasViewModel.OrderBy(x => x.VchFHVencimiento).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosColegiaturasViewModel = colegiaturasServices.lsPagosColegiaturasViewModel.OrderByDescending(x => x.VchFHVencimiento).ToList();
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
                }

                gvPagos.DataSource = colegiaturasServices.lsPagosColegiaturasViewModel;
                gvPagos.DataBind();
            }
        }
        protected void gvPagos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "btnPagos")
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvPagos.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKey = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());

                TextBox txtGvUidCliente = (TextBox)Seleccionado.FindControl("txtGvUidCliente");
                ViewState["RowCommand-UidCliente"] = txtGvUidCliente.Text;

                string Matri = gvPagos.Rows[index].Cells[1].Text;
                ViewState["RowCommand-Matricula"] = Matri;

                pagosColegiaturasServices.ObtenerPagosPadresReporte(dataKey, Matri);
                gvPagosColegiaturas.DataSource = pagosColegiaturasServices.lsReportePadresFechasPagosColeViewModel;
                gvPagosColegiaturas.DataBind();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalPagos()", true);
            }

            if (e.CommandName == "btnFormasPago")
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvPagos.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKey = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["RowCommand-UidFechaColegiatura"] = dataKey;

                TextBox txtGvUidCliente = (TextBox)Seleccionado.FindControl("txtGvUidCliente");
                ViewState["RowCommand-UidCliente"] = txtGvUidCliente.Text;

                string Matri = gvPagos.Rows[index].Cells[1].Text;
                ViewState["RowCommand-Matricula"] = Matri;

                ViewState["RowCommand-UidFormaPago"] = null;
                formasPagosServices.CargarFormasPagosReporteLigasPadres();
                rpFormasPago.DataSource = formasPagosServices.lsFormasPagos;
                rpFormasPago.DataBind();

                bancosServices.CargarBancos();
                ddlBanco.DataSource = bancosServices.lsBancos;
                ddlBanco.DataTextField = "VchDescripcion";
                ddlBanco.DataValueField = "UidBanco";
                ddlBanco.DataBind();

                LimpiarDatosPago();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalTipoPago()", true);
            }
        }
        protected void gvPagos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPagos.PageIndex = e.NewPageIndex;
            gvPagos.DataSource = colegiaturasServices.lsPagosColegiaturasViewModel;
            gvPagos.DataBind();
        }
        #endregion

        #region GridViewPagosColegiatura
        protected void gvPagosColegiaturas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "btnInfoMovimiento")
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvPagosColegiaturas.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKey = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());

                Label lblGvUidFormaPago = (Label)Seleccionado.FindControl("lblGvUidFormaPago");

                string FormaPago = string.Empty;

                if (Guid.Parse(lblGvUidFormaPago.Text) == Guid.Parse("31BE9A23-73EE-4F44-AF6C-6C0648DCEBF7"))
                {
                    FormaPago = "LIGA";
                }
                else
                {
                    FormaPago = "MANUAL";
                }

                switch (FormaPago)
                {
                    case "LIGA":
                        var list = pagosColegiaturasServices.ObtenerPagoColegiatura(dataKey);
                        DetallePagoColegiaturaLiga(list.Item1, list.Item2, dataKey);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalPagoDetalle()", true);
                        break;

                    case "MANUAL":

                        break;
                }
            }
        }
        protected void gvPagosColegiaturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        private void DetallePagoColegiaturaLiga(List<PagosColegiaturasViewModels> lsPagosColegiaturas, List<DetallePagosColeGridViewModel> lsDetallePagosColeGridViewModel, Guid UidPagoColegiatura)
        {
            //Resumen del pago
            foreach (var item in lsPagosColegiaturas)
            {
                //Asigancion de parametros

                //Encabezado del pago
                lblDetaAlumno.Text = "Alumno: " + item.VchAlumno;
                lblDetaMatricula.Text = "Matricula: " + item.VchMatricula;
                lblDetaFHpago.Text = "Fecha de pago: " + item.DtFHPago.ToString("dd/MM/yyyy");

                if (item.BitSubtotal)
                {
                    DcmSubtotal.Text = "$" + item.DcmImporteCole.ToString("N2");
                    trsubtotal.Style.Add("display", "");
                }
                else
                {
                    trsubtotal.Style.Add("display", "none");
                    DcmSubtotal.Text = "$0.00";
                }

                if (item.BitValidarImporte)
                {
                    DcmValidarImporte.Text = "$-" + item.DcmValidarImporte.ToString("N2");
                    trvalidarimporte.Style.Add("display", "");
                }
                else
                {
                    trvalidarimporte.Style.Add("display", "none");
                    DcmValidarImporte.Text = "$0.00";
                }

                DcmTotal.Text = item.DcmTotal.ToString("N2");

                if (item.BitComisionBancaria)
                {
                    VchComicionBancaria.Text =  item.VchComisionBancaria;
                    DcmImpComisionBancaria.Text = "$-" + item.DcmComisionBancaria.ToString("N2");
                    trcomicion.Style.Add("display", "");
                }
                else
                {
                    trcomicion.Style.Add("style", "display:none");
                    DcmImpComisionBancaria.Text = "$0.00";
                }

                if (item.BitPromocionDePago)
                {
                    VchPromocion.Text = item.VchPromocionDePago;
                    DcmImpPromocion.Text = "$-" + item.DcmPromocionDePago.ToString("N2");
                    trpromocion.Style.Add("display", "");

                    string dPromo = item.VchPromocionDePago.Replace("COMISIÓN ", "").Replace(" MESES:", "");

                    VchDetallePromocion.Text = dPromo.Trim() + " pagos mensuales de:";
                    DcmImpDetallePromocion.Text = (item.DcmTotal / decimal.Parse(dPromo.Trim())).ToString("N2");
                    trdetallepromociones.Style.Add("display", "");
                }
                else
                {
                    trpromocion.Style.Add("display", "none");
                    trdetallepromociones.Style.Add("display", "none");
                    DcmImpPromocion.Text = "$0.00";
                }

                DcmImpAbono.Text = "$" + item.DcmSubtotal.ToString("N2");
                DcmImpResta.Text = "$" + (item.DcmImporteCole - item.DcmSubtotal).ToString("N2");

            }

            rpDetalleLiga.DataSource = lsDetallePagosColeGridViewModel;
            rpDetalleLiga.DataBind();

            ////Desglose del pago
            pagosServices.ConsultarDetallePagoColegiatura(UidPagoColegiatura);
            foreach (var item in pagosServices.lsPagosTarjetaColeDetalleGridViewModel)
            {
                //Detalle de la operacion
                VchIdreferencia.Text = item.IdReferencia;
                DtmFechaDeRegistro.Text = item.DtmFechaDeRegistro.ToLongDateString();
                DtmHoraDeRegistro.Text = item.DtmFechaDeRegistro.ToString("HH:mm:ss");
                VchTarjeta.Text = item.cc_number;
                VchFolioPago.Text = item.FolioPago;
            }
        }

        private void DetallePagoColegiaturaManual() { }
        #endregion

        #region RepeaterFormasPago
        protected void rpFormasPago_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "btnSeleFormaPago")
            {
                Guid UidFormaPago = Guid.Parse((string)e.CommandArgument);
                ViewState["RowCommand-UidFormaPago"] = UidFormaPago;

                rpFormasPago.DataSource = formasPagosServices.lsFormasPagos;
                rpFormasPago.DataBind();
            }
        }
        protected void rpFormasPago_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (ViewState["RowCommand-UidFormaPago"] == null)
                {
                    ViewState["RowCommand-UidFormaPago"] = Guid.Empty;
                }


                if (((FormasPagos)e.Item.DataItem).UidFormaPago == Guid.Parse(ViewState["RowCommand-UidFormaPago"].ToString()))
                {

                    ((CheckBox)e.Item.FindControl("cbSeleccionado")).Checked = true;

                }
            }
        }
        #endregion

        #region FormaPago y Captura de datos
        protected void btnAnterior_Click(object sender, EventArgs e)
        {
            lblTitleModalTipoPago.Text = "Seleccione la forma de pago";

            btnAnterior.Visible = false;
            btnSiguiente.Visible = true;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Tabs", "showTabFormaPago()", true);
        }
        protected void btnSiguiente_Click(object sender, EventArgs e)
        {


            if (Guid.Parse(ViewState["RowCommand-UidFormaPago"].ToString()) == Guid.Empty)
            {
                pnlAlertModalTipoPago.Visible = true;
                lblMnsjModalTipoPago.Text = "Para continuar seleccione una forma de pago";
                divAlertModalTipoPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");

                rpFormasPago.DataSource = formasPagosServices.lsFormasPagos;
                rpFormasPago.DataBind();
                return;
            }

            #region Validacion de datos
            if (txtCuenta.EmptyTextBox())
            {
                pnlAlertModalTipoPago.Visible = true;
                lblMnsjModalTipoPago.Text = "Los ultimos 4 digitos de la cuenta es obligatorio.";
                divAlertModalTipoPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                return;
            }
            if (txtFHPago.EmptyTextBox())
            {
                pnlAlertModalTipoPago.Visible = true;
                lblMnsjModalTipoPago.Text = "El campo Fecha y Hora es obligatorio.";
                divAlertModalTipoPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                return;
            }
            if (txtMontoPagado.EmptyTextBox())
            {
                pnlAlertModalTipoPago.Visible = true;
                lblMnsjModalTipoPago.Text = "El campo Monto es obligatorio.";
                divAlertModalTipoPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                return;
            }
            if (txtFolioPago.EmptyTextBox())
            {
                pnlAlertModalTipoPago.Visible = true;
                lblMnsjModalTipoPago.Text = "El campo Folio es obligatorio.";
                divAlertModalTipoPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                return;
            }
            if (ddlBanco.EmptyDropDownList())
            {
                pnlAlertModalTipoPago.Visible = true;
                lblMnsjModalTipoPago.Text = "El campo Banco es obligatorio.";
                divAlertModalTipoPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                return;
            }
            #endregion
            lblTitleModalTipoPago.Text = "Detalle del pago";

            btnAnterior.Visible = true;
            btnSiguiente.Visible = false;

            CalcularPagoCole();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Tabs", "showTabPago()", true);
            txtCuenta.Focus();
        }
        private void CalcularPagoCole()
        {
            string Matri = ViewState["RowCommand-Matricula"].ToString();

            DateTime HoraDelServidor = DateTime.Now;
            DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

            colegiaturasServices.lsDesglosePagosGridViewModel.Clear();
            colegiaturasServices.ObtenerPagosColegiaturas(Guid.Parse(ViewState["RowCommand-UidCliente"].ToString()), Guid.Parse(ViewState["UidUsuarioLocal"].ToString()), Guid.Parse(ViewState["RowCommand-UidFechaColegiatura"].ToString()), Matri);

            ViewState["RowCommand-UidAlumno"] = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.UidAlumno;
            ViewState["RowCommand-UidFechaColegiatura"] = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.UidFechaColegiatura;
            ViewState["RowCommand-Identificador"] = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchIdentificador;
            lblConcepto2.Text = "Pago " + colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchNum + ", " + colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchMatricula + " " + colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.NombreCompleto;
            headAlumno2.Text = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.NombreCompleto;
            headMatricula2.Text = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchMatricula;
            lblVencimiento2.Text = hoy.ToString("dd/MM/yyyy");
            headFPago2.Text = hoy.ToString("dd/MM/yyyy");
            lblImporteCole2.Text = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.DcmImporte.ToString("N2");
            lblRecargo2.Text = "0.00";

            colegiaturasServices.FormarDesgloseCole(1, colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchIdentificador + ". PAGO " + colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchNum, colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.DcmImporte);

            decimal recargoTotalLimite = 0;
            decimal recargoTotalPeriodo = 0;
            decimal ImporteCole = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.DcmImporte;

            if (colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.BitRecargo)
            {
                if (colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchFHLimite != "NO TIENE")
                {
                    if (hoy > DateTime.Parse(colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchFHLimite) && DateTime.Parse(colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchFHLimite) < colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.DtFHFinPeriodo)
                    {
                        if (colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchTipoRecargo == "CANTIDAD")
                        {
                            recargoTotalLimite = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.DcmRecargo;
                        }
                        else if (colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchTipoRecargo == "PORCENTAJE")
                        {
                            recargoTotalLimite = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.DcmRecargo * ImporteCole / 100;
                        }
                    }

                    colegiaturasServices.FormarDesgloseCole(3, "RECARGO POR FECHA LIMITE (" + colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchFHLimite + ")", decimal.Parse(recargoTotalLimite.ToString("N2")));
                }
            }

            if (colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.BitRecargoPeriodo)
            {
                decimal recargo = decimal.Parse(colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.DcmRecargoPeriodo.ToString("N2"));
                decimal recargoTemp = 0;

                int num = colegiaturasServices.lsDesglosePagosGridViewModel.Count() + 2;

                if (colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchTipoRecargoPeriodo == "CANTIDAD")
                {
                    if (colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchPeriodicidad == "MENSUAL")
                    {
                        var dateTime = DateTimeSpan.CompareDates(colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.DtFHFinPeriodo, hoy);
                        recargoTemp = recargo * dateTime.Months;

                        colegiaturasServices.FormarDesgloseCole(num, "RECARGO POR FECHA PERIODO (" + colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.DtFHFinPeriodo.ToString("dd/MM/yyyy") + ")", decimal.Parse(recargo.ToString("N2")));
                        for (int i = 1; i < dateTime.Months; i++)
                        {
                            DateTime FHFinPeriodo = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.DtFHFinPeriodo.AddMonths(1 * i);

                            colegiaturasServices.FormarDesgloseCole(num + i, "RECARGO POR FECHA PERIODO (" + FHFinPeriodo.ToString("dd/MM/yyyy") + ")", decimal.Parse(recargo.ToString("N2")));
                        }
                    }
                    else if (colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchPeriodicidad == "SEMANAL")
                    {
                        int canSem = 0;

                        var dateTime = DateTimeSpan.CompareDates(colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.DtFHFinPeriodo, hoy);
                        canSem = dateTime.Days / 7;
                        recargoTemp = recargo * canSem;

                        colegiaturasServices.FormarDesgloseCole(num, "RECARGO POR FECHA PERIODO (" + colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.DtFHFinPeriodo.ToString("dd/MM/yyyy") + ")", decimal.Parse(recargo.ToString("N2")));

                        for (int i = 1; i < canSem; i++)
                        {
                            DateTime FHFinPeriodo = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.DtFHFinPeriodo.AddDays(7 * i);

                            colegiaturasServices.FormarDesgloseCole(num + i, "RECARGO POR FECHA PERIODO (" + FHFinPeriodo.ToString("dd/MM/yyyy") + ")", decimal.Parse(recargo.ToString("N2")));
                        }
                    }
                }
                else if (colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchTipoRecargoPeriodo == "PORCENTAJE")
                {
                    recargo = recargo * ImporteCole / 100;

                    if (colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchPeriodicidad == "MENSUAL")
                    {
                        var dateTime = DateTimeSpan.CompareDates(colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.DtFHFinPeriodo, hoy);
                        recargoTemp = recargo * dateTime.Months;

                        colegiaturasServices.FormarDesgloseCole(num, "RECARGO POR FECHA PERIODO (" + colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.DtFHFinPeriodo.ToString("dd/MM/yyyy") + ")", decimal.Parse(recargo.ToString("N2")));
                        for (int i = 1; i < dateTime.Months; i++)
                        {
                            DateTime FHFinPeriodo = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.DtFHFinPeriodo.AddMonths(1 * i);

                            colegiaturasServices.FormarDesgloseCole(num + i, "RECARGO POR FECHA PERIODO (" + FHFinPeriodo.ToString("dd/MM/yyyy") + ")", decimal.Parse(recargo.ToString("N2")));
                        }
                    }
                    else if (colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchPeriodicidad == "SEMANAL")
                    {
                        int canSem = 0;

                        var dateTime = DateTimeSpan.CompareDates(colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.DtFHFinPeriodo, hoy);
                        canSem = dateTime.Days / 7;
                        recargoTemp = recargo * canSem;

                        colegiaturasServices.FormarDesgloseCole(num, "RECARGO POR FECHA PERIODO (" + colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.DtFHFinPeriodo.ToString("dd/MM/yyyy") + ")", decimal.Parse(recargo.ToString("N2")));

                        for (int i = 1; i < canSem; i++)
                        {
                            DateTime FHFinPeriodo = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.DtFHFinPeriodo.AddDays(7 * i);

                            colegiaturasServices.FormarDesgloseCole(num + i, "RECARGO POR FECHA PERIODO (" + FHFinPeriodo.ToString("dd/MM/yyyy") + ")", decimal.Parse(recargo.ToString("N2")));
                        }
                    }
                }

                recargoTotalPeriodo = recargoTemp;

                //colegiaturasServices.FormarDesgloseCole(4, "RECARGO FECHA ", recargoTotalPeriodo);
            }

            //decimal recargoTotal = recargoTotalLimite + recargoTotalPeriodo;

            lblRecargo2.Text = (recargoTotalLimite + recargoTotalPeriodo).ToString("N2");

            decimal ImporteTotal = decimal.Parse(lblImporteCole2.Text) /*+ decimal.Parse(lblRecargo.Text)*/;
            decimal ImporteCCT = 0;
            lblTieneBeca2.Text = "NO";
            lblTipoBeca2.Text = "NINGUNO";
            lblImporteBeca2.Text = "0.00";
            decimal ImporteBeca = 0;

            string TipoBeca = string.Empty;

            if (colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.BitBeca)
            {
                lblTieneBeca2.Text = "SI";
                lblTipoBeca2.Text = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchTipoBeca;

                if (colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchTipoBeca == "CANTIDAD")
                {
                    ImporteBeca = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.DcmBeca;
                    ImporteTotal = ImporteTotal - ImporteBeca;

                    TipoBeca = "$" + ImporteBeca.ToString("N2");
                }
                else if (colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchTipoBeca == "PORCENTAJE")
                {
                    decimal porcentaje = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.DcmBeca;

                    ImporteBeca = porcentaje * ImporteTotal / 100;
                    ImporteTotal = ImporteTotal - ImporteBeca;

                    TipoBeca = porcentaje.ToString("N2") + "%";
                }

                lblImporteBeca2.Text = ImporteBeca.ToString("N2");
            }

            ImporteTotal = ImporteTotal + decimal.Parse(lblRecargo2.Text);

            if (ImporteBeca >= 1)
            {
                colegiaturasServices.FormarDesgloseCole(2, "DESCUENTO BECA (" + TipoBeca + ")", -decimal.Parse(ImporteBeca.ToString("N2")), "#f55145");
            }
            else
            {
                colegiaturasServices.FormarDesgloseCole(2, "DESCUENTO BECA (" + TipoBeca + ")", decimal.Parse(ImporteBeca.ToString("N2")));
            }

            pagosColegiaturasServices.ObtenerPagosPadres(Guid.Parse(ViewState["RowCommand-UidFechaColegiatura"].ToString()), Guid.Parse(ViewState["RowCommand-UidAlumno"].ToString()));

            if (pagosColegiaturasServices.lsFechasPagosColegiaturasViewModel.Count >= 1)
            {
                foreach (var item in pagosColegiaturasServices.lsFechasPagosColegiaturasViewModel)
                {
                    int num = colegiaturasServices.lsDesglosePagosGridViewModel.Count();
                    colegiaturasServices.FormarDesgloseCole(num + 1, "Pago parcial (" + item.DtFHPago.ToString("dd/MM/yyyy") + " " + item.Comentario + ")", -decimal.Parse(item.DcmImportePagado.ToString("N2")), "#f55145");
                }

                ImporteTotal = ImporteTotal - pagosColegiaturasServices.lsFechasPagosColegiaturasViewModel.Sum(x => x.DcmImportePagado);
            }

            //comisionesTarjetasCl.CargarComisionesTarjeta(Guid.Parse(ViewState["RowCommand-UidCliente"].ToString()));

            trComisionTarjeta2.Attributes.Add("style", "display:none;");
            ViewState["ImporteCCT"] = 0;
            ViewState["booltrComisionTarjeta"] = false;

            //if (comisionesTarjetasCl.lsComisionesTarjetasClientes.Count >= 1)
            //{
            //    foreach (var itComi in comisionesTarjetasCl.lsComisionesTarjetasClientes)
            //    {
            //        if (itComi.BitComision)
            //        {
            //            ImporteCCT = itComi.DcmComision * ImporteTotal / 100;
            //            ViewState["ImporteCCT"] = ImporteCCT;

            //            trComisionTarjeta.Attributes.Add("style", "");
            //            ViewState["booltrComisionTarjeta"] = true;

            //            lblComisionTarjetatb.Text = "COMISIÓN BANCARIA:";
            //            lblImpComisionTrajetatb.Text = "$" + ImporteCCT.ToString("N2");

            //            lblComisionTarjeta.Text = ImporteCCT.ToString("N2");
            //        }
            //    }
            //}

            lblImporteTotal2.Text = ImporteTotal.ToString("N2");
            lblSubtotaltb2.Text = "$" + ImporteTotal.ToString("N2");
            Calcular(ImporteTotal.ToString());

            btnGenerarPago2.Visible = true;

            rptDesglose2.DataSource = colegiaturasServices.lsDesglosePagosGridViewModel.OrderBy(x => x.IntNum);
            rptDesglose2.DataBind();

        }

        protected void Calcular(string Importe)
        {
            if (!string.IsNullOrEmpty(Importe))
            {
                ddlFormasPago2.Items.Clear();

                ddlFormasPago2.Items.Insert(0, new ListItem("Al contado", "contado"));
                ddlFormasPago2.DataBind();
            }

            if (!string.IsNullOrEmpty(lblImporteTotal2.Text) && decimal.Parse(lblImporteTotal2.Text) != 0)
            {
                if (ddlFormasPago2.SelectedValue == "contado")
                {
                    decimal importe = decimal.Parse(lblImporteTotal2.Text) + decimal.Parse(ViewState["ImporteCCT"].ToString());

                    lblTotalPagar2.Text = importe.ToString("N2");
                    txtTotaltb2.Text = txtMontoPagado.Text /*importe.ToString("N2")*/;
                    ViewState["txtImporteTotal.Text"] = importe.ToString("N2");

                    trPromociones2.Attributes.Add("style", "display:none;");

                    ViewState["booltrPromociones"] = false;

                    lblComisionPromocion2.Text = "0.00";
                    lblPromotb2.Text = "AL CONTADO(0%):";
                    lblImpPromotb2.Text = "$0.00";
                    lblTotaltb2.Text = "$" + importe.ToString("N2");
                }

                lblTotalPago2.Text = "Generar pago $" + ViewState["txtImporteTotal.Text"].ToString();
                btnGenerarPago2.Enabled = true;
            }
            else
            {
                lblTotalPagar2.Text = string.Empty;
                lblTotalPago2.Text = "Generar pago $0.00";
                btnGenerarPago2.Enabled = false;
            }

            CalcularManual();

            if (trComisionTarjeta2.Style.Value == "display:none;" && trPromociones2.Style.Value == "display:none;")
            {
                trSubtotal2.Attributes.Add("style", "display:none;");
                ViewState["booltrSubtotal"] = false;
            }
            else
            {
                trSubtotal2.Attributes.Add("style", "");
                ViewState["booltrSubtotal"] = true;
            }
        }
        protected void CalcularManual()
        {
            string MontoMin = "50.00";
            string MontoMax = "15000.00";
            decimal Resta = 0;

            lblTotalPagar2.Text = string.Empty;
            lblTotalPago2.Text = "Generar pago $0.00";
            btnGenerarPago2.Enabled = false;

            ViewState["txtTotaltb.Text"] = 0;

            if (string.IsNullOrEmpty(txtTotaltb2.Text))
            {
                txtTotaltb2.Text = "0";
            }

            decimal Total = decimal.Parse(ViewState["txtImporteTotal.Text"].ToString());
            decimal Totaltb = decimal.Parse(txtTotaltb2.Text);

            if (Totaltb >= decimal.Parse(MontoMin) && Totaltb <= decimal.Parse(MontoMax))
            {
                if (Totaltb > Total)
                {
                    pnlAlertPago.Visible = true;
                    txtTotaltb2.BackColor = System.Drawing.Color.FromName("#f2dede");
                    lblMensajeAlertPago.Text = "El monto ingresado no puede ser mayor al  Total.";
                    divAlertPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                }
                else
                {

                    ViewState["txtTotaltb.Text"] = decimal.Parse(txtTotaltb2.Text);

                    lblTotalPago2.Text = "Generar pago $" + decimal.Parse(txtTotaltb2.Text).ToString("N2");
                    btnGenerarPago2.Enabled = true;

                    decimal SubTotal = 0;
                    ViewState["ImpOtraSubTotal"] = 0;
                    decimal ImporteCCT = 0;
                    ViewState["ImpOtraCantCCT"] = 0;
                    string CCT = string.Empty;
                    decimal ImporteCP = 0;
                    ViewState["ImpOtraCantCP"] = 0;
                    string Promocion = string.Empty;

                    SubTotal = Totaltb;

                    Resta = decimal.Parse(lblImporteTotal2.Text) - SubTotal;
                    ViewState["ImporteResta"] = Resta;

                    lblToolApagar2.Text = "Subtotal: $" + SubTotal.ToString("N2") + "<br />"
                                       + CCT
                                       + Promocion
                                       + "Total: $" + Totaltb.ToString("N2");

                    ViewState["ImpOtraSubTotal"] = SubTotal;
                }
            }
            else
            {
                pnlAlertPago.Visible = true;
                txtTotaltb2.BackColor = System.Drawing.Color.FromName("#f2dede");
                lblMensajeAlertPago.Text = "El importe mínimo es de $50.00 y el máximo es de $15,000.00.";
                divAlertPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
            }

            txtTotaltb2.Text = decimal.Parse(txtTotaltb2.Text).ToString("N2");
            lblRestaTotal2.Text = "Resta: $" + Resta.ToString("N2");
        }
        protected void btnGenerarPago_Click(object sender, EventArgs e)
        {
            lblMnsjDialog.Text = "<strong>Importe a pagar </strong>" +
                                 "<br />" +
                                 "<h2>$" + txtTotaltb2.Text + "</h2>";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalDialog", "showModalDialog()", true);
        }
        private void LimpiarDatosPago()
        {
            lblTitleModalTipoPago.Text = "Seleccione la forma de pago";

            ddlBanco.SelectedIndex = -1;
            txtCuenta.Text = string.Empty;
            txtFHPago.Text = string.Empty;
            txtMontoPagado.Text = string.Empty;
            txtFolioPago.Text = string.Empty;

            btnAnterior.Visible = false;
            btnSiguiente.Visible = true;
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            decimal importeTotal = decimal.Parse(ViewState["txtTotaltb.Text"].ToString());
            Guid UidPagoColegiatura = Guid.NewGuid();

            bool trSubtotal = bool.Parse(ViewState["booltrSubtotal"].ToString());
            bool trComisionTarjeta = bool.Parse(ViewState["booltrComisionTarjeta"].ToString());
            bool trPromociones = bool.Parse(ViewState["booltrPromociones"].ToString());
            Guid EstatusPagoColegiatura = Guid.Parse("51B85D66-866B-4BC2-B08F-FECE1A994053");
            Guid estatusFechaPago = Guid.Parse("F25E4AAB-6044-46E9-A575-98DCBCCF7604");

            string Correo = validacionesServices.ObtenerCorreoUsuario(Guid.Parse(ViewState["UidUsuarioLocal"].ToString()));

            if (importeTotal < decimal.Parse(ViewState["txtImporteTotal.Text"].ToString()))
            {
                estatusFechaPago = Guid.Parse("F25E4AAB-6044-46E9-A575-98DCBCCF7604");
            }

            int UltimoFolio = 0;
            if (pagosColegiaturasServices.RegistrarPagoColegiatura(UidPagoColegiatura, UltimoFolio, DateTime.Parse(headFPago2.Text), lblPromotb2.Text, lblComisionTarjetatb2.Text, trSubtotal, decimal.Parse(ViewState["ImpOtraSubTotal"].ToString()), trComisionTarjeta, decimal.Parse(ViewState["ImpOtraCantCCT"].ToString()), trPromociones, decimal.Parse(ViewState["ImpOtraCantCP"].ToString()), false, 0, importeTotal, Guid.Parse(ViewState["UidUsuarioLocal"].ToString()), EstatusPagoColegiatura,
                Guid.Parse(ViewState["RowCommand-UidFechaColegiatura"].ToString()), Guid.Parse(ViewState["RowCommand-UidAlumno"].ToString()), Guid.Parse(ViewState["RowCommand-UidFormaPago"].ToString()), decimal.Parse(lblSubtotaltb2.Text.Replace("$", "")), decimal.Parse(ViewState["ImpOtraSubTotal"].ToString()), decimal.Parse(ViewState["ImporteResta"].ToString()), estatusFechaPago))
            {
                pagosManualesServices.RegistrarPagoManual(Guid.Parse(ddlBanco.SelectedValue), txtCuenta.Text, DateTime.Parse(txtFHPago.Text), decimal.Parse(txtMontoPagado.Text), txtFolioPago.Text, UidPagoColegiatura);

                foreach (var item in colegiaturasServices.lsDesglosePagosGridViewModel)
                {
                    detallesPagosColegiaturasServices.RegistrarDetallePagoColegiatura(item.IntNum, item.VchConcepto, item.DcmImporte, UidPagoColegiatura);
                }

                //correosEscuelaServices.CorreoEnvioPagoColegiaturaManual(headAlumno2.Text, headMatricula2.Text, DateTime.Parse(headFPago2.Text), decimal.Parse(ViewState["txtImporteTotal.Text"].ToString()), decimal.Parse(txtTotaltb2.Text), colegiaturasServices.lsDesglosePagosGridViewModel, "Comprobante de pago de colegiatura", ddlBanco.SelectedItem.Text, "************" + txtCuenta.Text, DateTime.Parse(txtFHPago.Text), txtFolioPago.Text, Correo, "PROCESANDO");

                colegiaturasServices.CargarPagosColegiaturasReporte(Guid.Parse(ViewState["UidUsuarioLocal"].ToString()));
                gvPagos.DataSource = colegiaturasServices.lsPagosColegiaturasViewModel;
                gvPagos.DataBind();

                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "<b>Felicidades,</b> su pago se registró exitosamente. Ahora solo falta que la escuela lo verifique.";
                divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalDialog", "hideModalTipoPago()", true);
            }
        }
        #endregion
    }
}