using Franquicia.Bussiness;
using Franquicia.Domain.Models;
using Franquicia.Domain.ViewModels;
using PagaLaEscuela.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Franquicia.Bussiness.DateTimeSpanServices;

namespace PagaLaEscuela.Views
{
    public partial class ReporteLigasEscuelas : System.Web.UI.Page
    {
        PagosTarjetaServices pagosTarjetaServices = new PagosTarjetaServices();
        ColegiaturasServices colegiaturasServices = new ColegiaturasServices();
        EstatusFechasPagosServices estatusFechasPagosServices = new EstatusFechasPagosServices();
        PagosManualesServices pagosManualesServices = new PagosManualesServices();
        PagosColegiaturasServices pagosColegiaturasServices = new PagosColegiaturasServices();
        FormasPagosServices formasPagosServices = new FormasPagosServices();
        TiposTarjetasServices tiposTarjetasServices = new TiposTarjetasServices();
        PagosServices pagosServices = new PagosServices();
        AlumnosServices alumnosServices = new AlumnosServices();
        ComisionesTarjetasClientesTerminalServices comisionesTarjetasClTer = new ComisionesTarjetasClientesTerminalServices();
        PromocionesTerminalServices promocionesTerminalServices = new PromocionesTerminalServices();
        DetallesPagosColegiaturasServices detallesPagosColegiaturasServices = new DetallesPagosColegiaturasServices();
        PagosEfectivosServices pagosEfectivosServices = new PagosEfectivosServices();
        UsuariosServices usuariosServices = new UsuariosServices();
        ClientesServices clientesServices = new ClientesServices();

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
                txtTotaltb.Attributes.Add("onchange", "button_click(this,'" + btnCalcular.ClientID + "')");

                ViewState["gvDatos"] = SortDirection.Ascending;
                ViewState["gvAlumnos"] = SortDirection.Ascending;
                ViewState["gvPagos"] = SortDirection.Ascending;

                Session["pagosTarjetaServices"] = pagosTarjetaServices;
                Session["colegiaturasServices"] = colegiaturasServices;
                Session["estatusFechasPagosServices"] = estatusFechasPagosServices;
                Session["alumnosServices"] = alumnosServices;

                Session["promocionesTerminalServices"] = promocionesTerminalServices;

                ActualizarDatosPrincipal();

                formasPagosServices.CargarFormasPagos();
                ddlFormaPago.DataSource = formasPagosServices.lsFormasPagos;
                ddlFormaPago.Items.Insert(0, new ListItem("TODOS", Guid.Empty.ToString()));
                ddlFormaPago.DataTextField = "VchDescripcion";
                ddlFormaPago.DataValueField = "UidFormaPago";
                ddlFormaPago.DataBind();

                estatusFechasPagosServices.CargarEstatusFechasPagosBusquedaRLE();
                ddlEstatus.DataSource = estatusFechasPagosServices.lsEstatusFechasPagos;
                ddlEstatus.Items.Insert(0, new ListItem("TODOS", Guid.Empty.ToString()));
                ddlEstatus.DataTextField = "VchDescripcion";
                ddlEstatus.DataValueField = "UidEstatusFechaPago";
                ddlEstatus.DataBind();

                byte[] imagen = clientesServices.clientesRepository.CargarLogo(Guid.Parse(ViewState["UidClienteLocal"].ToString()));

                if (imagen != null)
                {
                    imgLogo.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(imagen);
                    imgLogo.DataBind();
                }
                else
                {
                    imgLogo.ImageUrl = "../Images/SinLogo2.png";
                    imgLogo.DataBind();
                }
            }
            else
            {
                pagosTarjetaServices = (PagosTarjetaServices)Session["pagosTarjetaServices"];
                colegiaturasServices = (ColegiaturasServices)Session["colegiaturasServices"];
                estatusFechasPagosServices = (EstatusFechasPagosServices)Session["estatusFechasPagosServices"];
                alumnosServices = (AlumnosServices)Session["alumnosServices"];

                promocionesTerminalServices = (PromocionesTerminalServices)Session["promocionesTerminalServices"];

                pnlAlert.Visible = false;
                lblMensajeAlert.Text = "";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade");

                pnlAlertModalVerificarPago.Visible = false;
                lblMnsjModalVerificarPago.Text = "";
                divAlertModalVerificarPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade");

                pnlAlertModalTipoPago.Visible = false;
                lblMnsjModalTipoPago.Text = "";
                divAlertModalTipoPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade");

                pnlAlertPago.Visible = false;
                lblMensajeAlertPago.Text = "";
                divAlertPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade");

                rpEstatusPagos.DataSource = estatusFechasPagosServices.lsEstatusFechasPagos;
                rpEstatusPagos.DataBind();
            }
        }

        protected void btnFiltros_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalBusqueda()", true);
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

            ViewState["NewPageIndex"] = null;
            ViewState["NewPageIndex2"] = null;

            colegiaturasServices.BuscarPagosEscuelas(Guid.Parse(ViewState["UidClienteLocal"].ToString()), txtColegiatura.Text, txtNumPago.Text, txtMatricula.Text, txtAlNombre.Text, txtAlApPaterno.Text, txtAlApMaterno.Text, txtTuNombre.Text, txtTuApPaterno.Text, txtTuApMaterno.Text, txtFolio.Text, txtCuenta.Text, txtBanco.Text, ImporteMayor, ImporteMenor, txtRegistroDesde.Text, txtRegistroHasta.Text, Guid.Parse(ddlFormaPago.SelectedValue), Guid.Parse(ddlEstatus.SelectedValue));

            gvDatosAlumnos.DataSource = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels;
            gvDatosAlumnos.DataBind();

            gvDatosPagos.DataSource = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels;
            gvDatosPagos.DataBind();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModalBusqueda()", true);
        }
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtColegiatura.Text = string.Empty;
            txtNumPago.Text = string.Empty;

            txtMatricula.Text = string.Empty;
            txtAlNombre.Text = string.Empty;
            txtAlApPaterno.Text = string.Empty;
            txtAlApMaterno.Text = string.Empty;

            txtTuNombre.Text = string.Empty;
            txtTuApPaterno.Text = string.Empty;
            txtTuApMaterno.Text = string.Empty;

            txtFolio.Text = string.Empty;
            txtCuenta.Text = string.Empty;
            txtBanco.Text = string.Empty;
            txtImporteMayor.Text = string.Empty;
            txtImporteMenor.Text = string.Empty;
            ddlImporteMayor.SelectedIndex = -1;
            ddlImporteMenor.SelectedIndex = -1;
            txtRegistroDesde.Text = string.Empty;
            txtRegistroHasta.Text = string.Empty;
            ddlFormaPago.SelectedIndex = -1;
            ddlEstatus.SelectedIndex = -1;
        }

        protected void btnExportarLista_Click(object sender, EventArgs e)
        {
            Session["lsPagosReporteLigaEscuelaViewModels"] = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels;
            string _open = "window.open('Excel/ExportarAExcelReportePagosEscuela.aspx', '_blank');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
        }
        protected void btnActualizarLista_Click(object sender, EventArgs e)
        {
            ViewState["NewPageIndex"] = null;
            ViewState["NewPageIndex2"] = null;

            ActualizarDatosPrincipal();

            pnlAlert.Visible = true;
            lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha sincronizado exitosamente.";
            divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
        }

        protected void btnPago_Click(object sender, EventArgs e)
        {
            ViewState["cantTabsAddPago"] = 0;
            btnAnterior.Visible = false;
            btnSiguiente.Visible = true;

            ViewState["gvAlumnosSelect"] = false;
            gvAlumnos.SelectedIndex = -1;
            gvAlumnos.DataSource = null;
            gvAlumnos.DataBind();

            btnFiltroLimpiar_Click(null, null);

            ViewState["gvPagosSelect"] = false;
            gvPagos.SelectedIndex = -1;
            gvPagos.DataSource = null;
            gvPagos.DataBind();

            lblTitleModalTipoPago.Text = "Seleccione un alumno";

            lblHeadAlum.Text = "Alumno: ";
            lblHeadCole.Text = "";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalTipoPago()", true);
        }

        #region RepeaterFormasPago
        protected void rpEstatusPagos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "btnSeleEstatusPago")
            {
                Guid UidEstatusFechaPago = Guid.Parse((string)e.CommandArgument);
                ViewState["ItemCommand-UidEstatusFechaPago"] = UidEstatusFechaPago;

                rpEstatusPagos.DataSource = estatusFechasPagosServices.lsEstatusFechasPagos;
                rpEstatusPagos.DataBind();
            }
        }
        protected void rpEstatusPagos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (ViewState["ItemCommand-UidEstatusFechaPago"] == null)
                {
                    ViewState["ItemCommand-UidEstatusFechaPago"] = Guid.Empty;
                }

                if (((EstatusFechasPagos)e.Item.DataItem).UidEstatusFechaPago == Guid.Parse(ViewState["ItemCommand-UidEstatusFechaPago"].ToString()))
                {
                    ((CheckBox)e.Item.FindControl("cbSeleccionado")).Checked = true;
                }
            }
        }
        #endregion

        #region VisualizarDatosAlumnosyPagos
        protected void btnDatosAlumnos_Click(object sender, EventArgs e)
        {
            pnlDatosAlumnos.Visible = true;
            btnDatosAlumnos.CssClass = "btn btn-primary pull-right";

            pnlDatosPagos.Visible = false;
            btnDatosPagos.CssClass = "btn btn-secondary pull-left";
        }
        protected void btnDatosPagos_Click(object sender, EventArgs e)
        {
            pnlDatosAlumnos.Visible = false;
            btnDatosAlumnos.CssClass = "btn btn-secondary pull-right";

            pnlDatosPagos.Visible = true;
            btnDatosPagos.CssClass = "btn btn-primary pull-left";
        }

        #region GridViewDatosAlumnos
        protected void gvDatosAlumnos_Sorting(object sender, GridViewSortEventArgs e)
        {
            string SortExpression = e.SortExpression;
            SortDirection direccion;
            string Orden = string.Empty;

            if (ViewState["gvDatos"] != null)
            {
                direccion = (SortDirection)ViewState["gvDatos"];
                if (direccion == SortDirection.Ascending)
                {
                    ViewState["gvDatos"] = SortDirection.Descending;
                    Orden = "ASC";
                }
                else
                {
                    ViewState["gvDatos"] = SortDirection.Ascending;
                    Orden = "DESC";
                }

                switch (SortExpression)
                {
                    case "IntFolio":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderBy(x => x.IntFolio).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderByDescending(x => x.IntFolio).ToList();
                        }
                        break;
                    case "VchIdentificador":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderBy(x => x.VchIdentificador).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderByDescending(x => x.VchIdentificador).ToList();
                        }
                        break;
                    case "VchAlumno":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderBy(x => x.VchAlumno).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderByDescending(x => x.VchAlumno).ToList();
                        }
                        break;
                    case "VchNum":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderBy(x => x.VchNum).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderByDescending(x => x.VchNum).ToList();
                        }
                        break;
                    case "DtFHPago":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderBy(x => x.DtFHPago).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderByDescending(x => x.DtFHPago).ToList();
                        }
                        break;
                    case "DcmImporteCole":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderBy(x => x.DcmImporteCole).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderByDescending(x => x.DcmImporteCole).ToList();
                        }
                        break;
                    case "DcmImportePagado":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderBy(x => x.DcmImportePagado).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderByDescending(x => x.DcmImportePagado).ToList();
                        }
                        break;
                    case "DcmImporteNuevo":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderBy(x => x.DcmImporteNuevo).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderByDescending(x => x.DcmImporteNuevo).ToList();
                        }
                        break;
                    case "VchFormaPago":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderBy(x => x.VchFormaPago).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderByDescending(x => x.VchFormaPago).ToList();
                        }
                        break;
                    case "VchEstatus":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderBy(x => x.VchEstatus).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderByDescending(x => x.VchEstatus).ToList();
                        }
                        break;
                }

                ViewState["NewPageIndex"] = int.Parse(ViewState["NewPageIndex"].ToString()) - 1;
                gvDatosAlumnos.DataSource = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels;
                gvDatosAlumnos.DataBind();

                ViewState["NewPageIndex2"] = int.Parse(ViewState["NewPageIndex2"].ToString()) - 1;
                gvDatosPagos.DataSource = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels;
                gvDatosPagos.DataBind();
            }
        }
        protected void gvDatosAlumnos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "btnConfirmarPago")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvDatosAlumnos.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKey = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["RowCommand-UidPagoColegiatura"] = dataKey;

                Label lblGvUidAlumno = (Label)row.FindControl("lblGvUidAlumno");
                ViewState["RowCommand-UidAlumno"] = lblGvUidAlumno.Text;

                Label lblGvUidFechaColegiatura = (Label)row.FindControl("lblGvUidFechaColegiatura");
                ViewState["RowCommand-UidFechaColegiatura"] = lblGvUidFechaColegiatura.Text;

                Label lblGvUidUsuario = (Label)row.FindControl("lblGvUidUsuario");
                ViewState["RowCommand-UidUsuario"] = lblGvUidUsuario.Text;

                ViewState["ItemCommand-UidEstatusFechaPago"] = null;
                estatusFechasPagosServices.CargarEstatusFechasPagosApRe();
                rpEstatusPagos.DataSource = estatusFechasPagosServices.lsEstatusFechasPagos;
                rpEstatusPagos.DataBind();

                pagosManualesServices.ObtenerPagoManual(dataKey);
                foreach (var item in pagosManualesServices.lsPagosManualesReporteEscuelaViewModel)
                {
                    lblFormaPago.Text = item.VchFormaPago;
                    lblBanco.Text = item.VchBanco;
                    lblCuenta.Text = item.VchCuenta;
                    lblFechaPago.Text = item.DtFHPago.ToString("dd/MM/yyyy HH:mm:ss");
                    lblImportePagado.Text = item.DcmImporte.ToString("N2");
                    lblFolio.Text = item.VchFolio;
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalVerificarPago()", true);
            }

            if (e.CommandName == "btnEditarPago")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvDatosAlumnos.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKey = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["RowCommand-UidPagoColegiatura"] = dataKey;

                Label lblGvUidAlumno = (Label)row.FindControl("lblGvUidAlumno");
                ViewState["RowCommand-UidAlumno"] = lblGvUidAlumno.Text;

                Label lblGvUidFechaColegiatura = (Label)row.FindControl("lblGvUidFechaColegiatura");
                ViewState["RowCommand-UidFechaColegiatura"] = lblGvUidFechaColegiatura.Text;

                Label lblGvUidUsuario = (Label)row.FindControl("lblGvUidUsuario");
                ViewState["RowCommand-UidUsuario"] = lblGvUidUsuario.Text;

                pagosManualesServices.ObtenerPagoManual(dataKey);
                foreach (var item in pagosManualesServices.lsPagosManualesReporteEscuelaViewModel)
                {
                    ViewState["ItemCommand-UidEstatusFechaPago"] = item.UidEstatusFechaPago;
                    lblFormaPago.Text = item.VchFormaPago;
                    lblBanco.Text = item.VchBanco;
                    lblCuenta.Text = item.VchCuenta;
                    lblFechaPago.Text = item.DtFHPago.ToString("dd/MM/yyyy HH:mm:ss");
                    lblImportePagado.Text = item.DcmImporte.ToString("N2");
                    lblFolio.Text = item.VchFolio;
                }

                estatusFechasPagosServices.CargarEstatusFechasPagosApRe();
                rpEstatusPagos.DataSource = estatusFechasPagosServices.lsEstatusFechasPagos;
                rpEstatusPagos.DataBind();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalVerificarPago()", true);
            }

            if (e.CommandName == "btnInfoMovimiento")
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvDatosAlumnos.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKey = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["RowCommand-UidPagoColegiatura"] = dataKey;

                Label lblGvUidFormaPago = (Label)Seleccionado.FindControl("lblGvUidFormaPago");

                string FormaPago = string.Empty;

                btnImprimir.Visible = false;
                btnImprimirManual.Visible = false;

                if (Guid.Parse(lblGvUidFormaPago.Text) == Guid.Parse("31BE9A23-73EE-4F44-AF6C-6C0648DCEBF7"))
                {
                    FormaPago = "LIGA";
                    trdetalleoperacion.Style.Add("display", "");
                }
                else if (Guid.Parse(lblGvUidFormaPago.Text) == Guid.Parse("3359D33E-C879-4A8B-96D3-C6A211AF014F"))
                {
                    FormaPago = "TARJETA";
                    trdetalleoperacion.Style.Add("display", "none");
                    btnImprimir.Visible = true;
                }
                else if (Guid.Parse(lblGvUidFormaPago.Text) == Guid.Parse("D92A2C64-C797-4C96-AD18-C2A433081F37"))
                {
                    FormaPago = "EFECTIVO";
                    trDetalleOperacionManual.Style.Add("display", "none");
                    btnImprimirManual.Visible = true;
                }
                else
                {
                    FormaPago = "MANUAL";
                    trDetalleOperacionManual.Style.Add("display", "");
                }

                var list = pagosColegiaturasServices.ObtenerPagoColegiatura(dataKey);

                switch (FormaPago)
                {
                    case "LIGA":
                        DetallePagoColegiaturaLiga(list.Item1, list.Item2, dataKey);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalPagoDetalle()", true);
                        break;

                    case "TARJETA":
                        DetallePagoColegiaturaLiga(list.Item1, list.Item2, dataKey);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalPagoDetalle()", true);
                        break;

                    case "EFECTIVO":

                        DetallePagoColegiaturaManual(list.Item1, list.Item2, dataKey);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalPagoDetalleManual()", true);
                        break;

                    case "MANUAL":

                        DetallePagoColegiaturaManual(list.Item1, list.Item2, dataKey);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalPagoDetalleManual()", true);
                        break;
                }
            }
        }
        protected void gvDatosAlumnos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDatosAlumnos.PageIndex = e.NewPageIndex;
            gvDatosPagos.PageIndex = e.NewPageIndex;

            ViewState["NewPageIndex"] = e.NewPageIndex;
            gvDatosAlumnos.DataSource = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels;
            gvDatosAlumnos.DataBind();

            ViewState["NewPageIndex2"] = e.NewPageIndex;
            gvDatosPagos.DataSource = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels;
            gvDatosPagos.DataBind();
        }
        protected void gvDatosAlumnos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblPaginado = (Label)e.Row.FindControl("lblPaginado");

                int PageSize = gvDatosAlumnos.PageSize;
                int antNum = 0;

                int numTotal = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.Count;

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

            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    double precio;
            //    precio = (double)DataBinder.Eval(e.Row.DataItem, "precio");
            //    if (precio > 8)
            //    {
            //        e.Row.ForeColor = System.Drawing.Color.Red;
            //        e.Row.BackColor = System.Drawing.Color.Yellow;
            //        e.Row.Font.Bold = true;
            //    }

            //}
        }
        #endregion

        #region GridViewDatosPagos
        protected void gvDatosPagos_Sorting(object sender, GridViewSortEventArgs e)
        {
            string SortExpression = e.SortExpression;
            SortDirection direccion;
            string Orden = string.Empty;

            if (ViewState["gvDatos"] != null)
            {
                direccion = (SortDirection)ViewState["gvDatos"];
                if (direccion == SortDirection.Ascending)
                {
                    ViewState["gvDatos"] = SortDirection.Descending;
                    Orden = "ASC";
                }
                else
                {
                    ViewState["gvDatos"] = SortDirection.Ascending;
                    Orden = "DESC";
                }

                switch (SortExpression)
                {
                    case "DtFHPago":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderBy(x => x.DtFHPago).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderByDescending(x => x.DtFHPago).ToList();
                        }
                        break;
                    case "IntFolio":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderBy(x => x.IntFolio).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderByDescending(x => x.IntFolio).ToList();
                        }
                        break;
                    case "VchFolio":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderBy(x => x.VchFolio).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderByDescending(x => x.VchFolio).ToList();
                        }
                        break;
                    case "DcmImportePagado":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderBy(x => x.DcmImportePagado).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderByDescending(x => x.DcmImportePagado).ToList();
                        }
                        break;
                    case "VchFormaPago":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderBy(x => x.VchFormaPago).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderByDescending(x => x.VchFormaPago).ToList();
                        }
                        break;
                    case "VchBanco":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderBy(x => x.VchBanco).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderByDescending(x => x.VchBanco).ToList();
                        }
                        break;
                    case "VchCuenta":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderBy(x => x.VchCuenta).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderByDescending(x => x.VchCuenta).ToList();
                        }
                        break;
                    case "VchEstatus":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderBy(x => x.VchEstatus).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsPagosReporteLigaEscuelaViewModels = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.OrderByDescending(x => x.VchEstatus).ToList();
                        }
                        break;
                }

                ViewState["NewPageIndex"] = int.Parse(ViewState["NewPageIndex"].ToString()) - 1;
                gvDatosAlumnos.DataSource = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels;
                gvDatosAlumnos.DataBind();

                ViewState["NewPageIndex2"] = int.Parse(ViewState["NewPageIndex2"].ToString()) - 1;
                gvDatosPagos.DataSource = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels;
                gvDatosPagos.DataBind();
            }
        }
        protected void gvDatosPagos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "btnConfirmarPago")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvDatosPagos.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKey = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["RowCommand-UidPagoColegiatura"] = dataKey;

                Label lblGvUidAlumno = (Label)row.FindControl("lblGvUidAlumno");
                ViewState["RowCommand-UidAlumno"] = lblGvUidAlumno.Text;

                Label lblGvUidFechaColegiatura = (Label)row.FindControl("lblGvUidFechaColegiatura");
                ViewState["RowCommand-UidFechaColegiatura"] = lblGvUidFechaColegiatura.Text;

                Label lblGvUidUsuario = (Label)row.FindControl("lblGvUidUsuario");
                ViewState["RowCommand-UidUsuario"] = lblGvUidUsuario.Text;

                if (pagosColegiaturasServices.ActualizarEstatusFechaPago(Guid.Parse(ViewState["RowCommand-UidPagoColegiatura"].ToString()), Guid.Parse("8720B2B9-5712-4E75-A981-932887AACDC9")))
                {
                    ValidarPago(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse(ViewState["RowCommand-UidUsuario"].ToString()), Guid.Parse(ViewState["RowCommand-UidFechaColegiatura"].ToString()), Guid.Parse(ViewState["RowCommand-UidAlumno"].ToString()));

                    ActualizarDatosPrincipal();

                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "<b>¡Felicidades! </b> la confirmación del pago se ha registrado exitosamente.";
                    divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                }
                else
                {
                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "<b>¡Lo sentimos! </b> ha ocurrido un error inesperadamente por favor intentelo más tarde, si el error persiste comuníquese con los administradores.";
                    divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                }
            }

            if (e.CommandName == "btnRechazarPago")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvDatosPagos.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKey = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["RowCommand-UidPagoColegiatura"] = dataKey;

                Label lblGvUidAlumno = (Label)row.FindControl("lblGvUidAlumno");
                ViewState["RowCommand-UidAlumno"] = lblGvUidAlumno.Text;

                Label lblGvUidFechaColegiatura = (Label)row.FindControl("lblGvUidFechaColegiatura");
                ViewState["RowCommand-UidFechaColegiatura"] = lblGvUidFechaColegiatura.Text;

                Label lblGvUidUsuario = (Label)row.FindControl("lblGvUidUsuario");
                ViewState["RowCommand-UidUsuario"] = lblGvUidUsuario.Text;


                if (pagosColegiaturasServices.ActualizarEstatusFechaPago(Guid.Parse(ViewState["RowCommand-UidPagoColegiatura"].ToString()), Guid.Parse("77DB3F13-7EC8-4CE1-A3DB-E5C96D14A581")))
                {
                    ValidarPago(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse(ViewState["RowCommand-UidUsuario"].ToString()), Guid.Parse(ViewState["RowCommand-UidFechaColegiatura"].ToString()), Guid.Parse(ViewState["RowCommand-UidAlumno"].ToString()));

                    ActualizarDatosPrincipal();

                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "<b>¡Felicidades! </b> la confirmación del pago se ha registrado exitosamente.";
                    divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                }
                else
                {
                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "<b>¡Lo sentimos! </b> ha ocurrido un error inesperadamente por favor intentelo más tarde, si el error persiste comuníquese con los administradores.";
                    divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                }
            }

            if (e.CommandName == "btnInfoMovimiento")
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvDatosPagos.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKey = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["RowCommand-UidPagoColegiatura"] = dataKey;

                Label lblGvUidFormaPago = (Label)Seleccionado.FindControl("lblGvUidFormaPago");

                string FormaPago = string.Empty;

                if (Guid.Parse(lblGvUidFormaPago.Text) == Guid.Parse("31BE9A23-73EE-4F44-AF6C-6C0648DCEBF7"))
                {
                    FormaPago = "LIGA";
                    trdetalleoperacion.Style.Add("display", "");
                }
                else if (Guid.Parse(lblGvUidFormaPago.Text) == Guid.Parse("3359D33E-C879-4A8B-96D3-C6A211AF014F"))
                {
                    FormaPago = "TARJETA";
                    trdetalleoperacion.Style.Add("display", "none");
                    btnImprimir.Visible = true;
                }
                else if (Guid.Parse(lblGvUidFormaPago.Text) == Guid.Parse("D92A2C64-C797-4C96-AD18-C2A433081F37"))
                {
                    FormaPago = "EFECTIVO";
                    trDetalleOperacionManual.Style.Add("display", "none");
                    btnImprimirManual.Visible = true;
                }
                else
                {
                    FormaPago = "MANUAL";
                    trDetalleOperacionManual.Style.Add("display", "");
                }

                var list = pagosColegiaturasServices.ObtenerPagoColegiatura(dataKey);

                switch (FormaPago)
                {
                    case "LIGA":
                        DetallePagoColegiaturaLiga(list.Item1, list.Item2, dataKey);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalPagoDetalle()", true);
                        break;

                    case "TARJETA":
                        DetallePagoColegiaturaLiga(list.Item1, list.Item2, dataKey);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalPagoDetalle()", true);
                        break;

                    case "EFECTIVO":

                        DetallePagoColegiaturaManual(list.Item1, list.Item2, dataKey);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalPagoDetalleManual()", true);
                        break;

                    case "MANUAL":

                        DetallePagoColegiaturaManual(list.Item1, list.Item2, dataKey);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalPagoDetalleManual()", true);
                        break;
                }
            }
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
                    trsubtotall.Style.Add("display", "");
                }
                else
                {
                    trsubtotall.Style.Add("display", "none");
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
                    VchComicionBancaria.Text = item.VchComisionBancaria;
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

            //Desglose del pago Liga
            pagosServices.ConsultarDetallePagoColegiatura(UidPagoColegiatura);
            foreach (var item in pagosServices.lsPagosTarjetaColeDetalleGridViewModel)
            {
                //Detalle de la operacion
                VchIdreferencia.Text = item.IdReferencia;
                DtmFechaDeRegistro.Text = item.DtmFechaDeRegistro.ToLongDateString();
                DtmHoraDeRegistro.Text = item.DtmFechaDeRegistro.ToString("HH:mm:ss");
                VchTarjeta.Text = "************" + item.cc_number;
                VchFolioPago.Text = item.FolioPago;
            }
        }
        private void DetallePagoColegiaturaManual(List<PagosColegiaturasViewModels> lsPagosColegiaturas, List<DetallePagosColeGridViewModel> lsDetallePagosColeGridViewModel, Guid UidPagoColegiatura)
        {
            //Resumen del pago
            foreach (var item in lsPagosColegiaturas)
            {
                //Asigancion de parametros

                //Encabezado del pago
                lblDetaAlumnoManual.Text = "Alumno: " + item.VchAlumno;
                lblDetaMatriculaManual.Text = "Matricula: " + item.VchMatricula;
                lblDetaFHpagoManual.Text = "Fecha de pago: " + item.DtFHPago.ToString("dd/MM/yyyy");

                if (item.BitSubtotal)
                {
                    DcmSubtotalManual.Text = "$" + item.DcmSubtotal.ToString("N2");
                    trSubtotalManual.Style.Add("display", "");
                }
                else
                {
                    trSubtotalManual.Style.Add("display", "none");
                    DcmSubtotalManual.Text = "$0.00";
                }

                if (item.BitValidarImporte)
                {
                    DcmValidarImporteManual.Text = "$-" + item.DcmValidarImporte.ToString("N2");
                    trValidarImporteManual.Style.Add("display", "");
                }
                else
                {
                    trValidarImporteManual.Style.Add("display", "none");
                    DcmValidarImporteManual.Text = "$0.00";
                }

                DcmTotalManual.Text = "$" + item.DcmImporteCole.ToString("N2");
                DcmImportePagadoManual.Text = "$" + item.DcmTotal.ToString("N2");
                DcmImpRestaManual.Text = "$" + (item.DcmImporteCole - item.DcmTotal).ToString("N2");
            }

            rpDetalleLigaManual.DataSource = lsDetallePagosColeGridViewModel;
            rpDetalleLigaManual.DataBind();


            pagosManualesServices.ConsultarDetallePagoColegiatura(UidPagoColegiatura);
            foreach (var item in pagosManualesServices.lsPagosManualesReporteEscuelaViewModel)
            {
                VchBancoManual.Text = item.VchBanco;
                DtFechaPagoManual.Text = item.DtFHPago.ToLongDateString();
                VchFolioManual.Text = item.VchFolio;
                VchCuentaManual.Text = "************" + item.VchCuenta;
                DtHoraPagoManual.Text = item.DtFHPago.ToString("HH:mm:ss");
            }
        }
        protected void gvDatosPagos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDatosAlumnos.PageIndex = e.NewPageIndex;
            gvDatosPagos.PageIndex = e.NewPageIndex;

            ViewState["NewPageIndex"] = e.NewPageIndex;
            gvDatosAlumnos.DataSource = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels;
            gvDatosAlumnos.DataBind();

            ViewState["NewPageIndex2"] = e.NewPageIndex;
            gvDatosPagos.DataSource = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels;
            gvDatosPagos.DataBind();
        }
        protected void gvDatosPagos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblPaginado = (Label)e.Row.FindControl("lblPaginado");

                int PageSize = gvDatosPagos.PageSize;
                int antNum = 0;

                int numTotal = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels.Count;

                if (numTotal >= 1)
                {
                    if (ViewState["NewPageIndex2"] != null)
                    {
                        int gh = int.Parse(ViewState["NewPageIndex2"].ToString());
                        ViewState["NewPageIndex2"] = gh + 1;

                        int r1 = int.Parse(ViewState["NewPageIndex2"].ToString()) * PageSize;
                        antNum = r1 - (PageSize - 1);
                    }
                    else
                    {
                        ViewState["NewPageIndex2"] = 1;
                        antNum = 1;
                    }

                    int NewPageIndex = int.Parse(ViewState["NewPageIndex2"].ToString());

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
        #endregion

        #endregion

        protected void btnVerificar_Click(object sender, EventArgs e)
        {
            if (Guid.Parse(ViewState["ItemCommand-UidEstatusFechaPago"].ToString()) == Guid.Empty)
            {
                pnlAlertModalVerificarPago.Visible = true;
                lblMnsjModalVerificarPago.Text = "Para continuar seleccione un estatus para el pago.";
                divAlertModalVerificarPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");

                rpEstatusPagos.DataSource = estatusFechasPagosServices.lsEstatusFechasPagos;
                rpEstatusPagos.DataBind();
                return;
            }

            if (pagosColegiaturasServices.ActualizarEstatusFechaPago(Guid.Parse(ViewState["RowCommand-UidPagoColegiatura"].ToString()), Guid.Parse(ViewState["ItemCommand-UidEstatusFechaPago"].ToString())))
            {
                ValidarPago(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse(ViewState["RowCommand-UidUsuario"].ToString()), Guid.Parse(ViewState["RowCommand-UidFechaColegiatura"].ToString()), Guid.Parse(ViewState["RowCommand-UidAlumno"].ToString()));

                ActualizarDatosPrincipal();

                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "<b>¡Felicidades! </b> la confirmación del pago se ha registrado exitosamente.";
                divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
            }
            else
            {
                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "<b>¡Lo sentimos! </b> ha ocurrido un error inesperadamente por favor intentelo más tarde, si el error persiste comuníquese con los administradores.";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModalVerificarPago()", true);
        }

        private void ActualizarDatosPrincipal()
        {
            colegiaturasServices.CargarPagosColeReporteEscuela(Guid.Parse(ViewState["UidClienteLocal"].ToString()));

            gvDatosAlumnos.DataSource = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels;
            gvDatosAlumnos.DataBind();

            gvDatosPagos.DataSource = colegiaturasServices.lsPagosReporteLigaEscuelaViewModels;
            gvDatosPagos.DataBind();
        }

        private void ValidarPago(Guid UidClienteLocal, Guid UidUsuario, Guid UidFechaColegiatura, Guid UidAlumno)
        {
            //Necesito saber el importe de la colegiatura
            decimal ImporteCole = colegiaturasServices.ObtenerDatosFechaColegiatura(UidClienteLocal, UidUsuario, UidFechaColegiatura, UidAlumno);

            //Necesito saber el importe de todos los pagos
            decimal ImportePagado = pagosColegiaturasServices.ObtenerPagosPadresRLE(UidFechaColegiatura, UidAlumno);
            decimal ImportePendiente = pagosColegiaturasServices.ObtenerPendientesPadresRLE(UidFechaColegiatura, UidAlumno);

            // ==>Validar con importe<==
            if (ImporteCole == ImportePagado) //el importeColegiatura es igual al importe de todos los pagos con estatus aprobado
            {
                //Se cambia el estatus de la colegiatura a pagado.
                colegiaturasServices.ActualizarEstatusFeColegiaturaAlumno(UidFechaColegiatura, UidAlumno, Guid.Parse("605A7881-54E0-47DF-8398-EDE080F4E0AA"), true);
            }
            else if (ImporteCole == (ImportePagado + ImportePendiente)) //el importe de los pagos aprobado y pendiente es igual al importe la colegiatura
            {
                // La colegiatura mantiene el estatus en proceso
                colegiaturasServices.ActualizarEstatusFeColegiaturaAlumno(UidFechaColegiatura, UidAlumno, Guid.Parse("5554CE57-1288-46D5-B36A-8AC69CB94B9A"), true);
            }
            else
            {
                // La colegiatura regresa al ultimo estatus
                DateTime HoraDelServidor = DateTime.Now;
                DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");
                string UidEstatus = colegiaturasServices.ObtenerEstatusColegiaturasRLE(hoy, UidFechaColegiatura, UidAlumno);
                colegiaturasServices.ActualizarEstatusFeColegiaturaAlumno(UidFechaColegiatura, UidAlumno, Guid.Parse(UidEstatus.ToString()), false);
            }
        }

        #region GridViewAlumnos
        protected void btnFiltroBuscar_Click(object sender, EventArgs e)
        {
            alumnosServices.BuscarAlumnosRLE(Guid.Parse(ViewState["UidClienteLocal"].ToString()), txtFiltroAlumIdentificador.Text, txtFiltroAlumNombre.Text.Trim(), txtFiltroAlumPaterno.Text.Trim(), txtFiltroAlumMaterno.Text.Trim(), txtFiltroAlumMatricula.Text.Trim());
            gvAlumnos.DataSource = alumnosServices.lsAlumnosRLEGridViewModel;
            gvAlumnos.DataBind();
        }
        protected void btnFiltroLimpiar_Click(object sender, EventArgs e)
        {
            txtFiltroAlumIdentificador.Text = string.Empty;
            txtFiltroAlumNombre.Text = string.Empty;
            txtFiltroAlumPaterno.Text = string.Empty;
            txtFiltroAlumMaterno.Text = string.Empty;
            txtFiltroAlumMatricula.Text = string.Empty;

            ViewState["gvAlumnosSelect"] = false;
            gvAlumnos.SelectedIndex = -1;
            btnFiltroBuscar_Click(null, null);
        }
        protected void gvAlumnos_Sorting(object sender, GridViewSortEventArgs e)
        {
            string SortExpression = e.SortExpression;
            SortDirection direccion;
            string Orden = string.Empty;

            if (ViewState["gvAlumnos"] != null)
            {
                direccion = (SortDirection)ViewState["gvAlumnos"];
                if (direccion == SortDirection.Ascending)
                {
                    ViewState["gvAlumnos"] = SortDirection.Descending;
                    Orden = "ASC";
                }
                else
                {
                    ViewState["gvAlumnos"] = SortDirection.Ascending;
                    Orden = "DESC";
                }

                switch (SortExpression)
                {
                    case "VchIdentificador":
                        if (Orden == "ASC")
                        {
                            alumnosServices.lsAlumnosRLEGridViewModel = alumnosServices.lsAlumnosRLEGridViewModel.OrderBy(x => x.VchIdentificador).ToList();
                        }
                        else
                        {
                            alumnosServices.lsAlumnosRLEGridViewModel = alumnosServices.lsAlumnosRLEGridViewModel.OrderByDescending(x => x.VchIdentificador).ToList();
                        }
                        break;
                    case "VchMatricula":
                        if (Orden == "ASC")
                        {
                            alumnosServices.lsAlumnosRLEGridViewModel = alumnosServices.lsAlumnosRLEGridViewModel.OrderBy(x => x.VchMatricula).ToList();
                        }
                        else
                        {
                            alumnosServices.lsAlumnosRLEGridViewModel = alumnosServices.lsAlumnosRLEGridViewModel.OrderByDescending(x => x.VchMatricula).ToList();
                        }
                        break;
                    case "NombreCompleto":
                        if (Orden == "ASC")
                        {
                            alumnosServices.lsAlumnosRLEGridViewModel = alumnosServices.lsAlumnosRLEGridViewModel.OrderBy(x => x.NombreCompleto).ToList();
                        }
                        else
                        {
                            alumnosServices.lsAlumnosRLEGridViewModel = alumnosServices.lsAlumnosRLEGridViewModel.OrderByDescending(x => x.NombreCompleto).ToList();
                        }
                        break;
                }

                gvAlumnos.DataSource = alumnosServices.lsAlumnosRLEGridViewModel;
                gvAlumnos.DataBind();
            }
        }
        protected void gvAlumnos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAlumnos.PageIndex = e.NewPageIndex;
            gvAlumnos.DataSource = alumnosServices.lsAlumnosRLEGridViewModel;
            gvAlumnos.DataBind();
        }
        protected void gvAlumnos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvAlumnos, "Select$" + e.Row.RowIndex);
            }
        }
        protected void gvAlumnos_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gvAlumnos.SelectedRow;

            Guid UidAlumno = Guid.Parse(gvAlumnos.SelectedDataKey.Value.ToString());
            ViewState["Selected-UidAlumno"] = UidAlumno;

            gvPagos.SelectedIndex = -1;
            usuariosServices.lsUsuarioGridViewModel.Clear();
            ddlTutorAlu.Items.Clear();
            usuariosServices.CargarTutoresAlumnos(UidAlumno);
            ddlTutorAlu.Items.Insert(0, new ListItem("SELECCIONE UN TUTOR", ""));
            ddlTutorAlu.DataSource = usuariosServices.lsUsuarioGridViewModel;
            ddlTutorAlu.DataTextField = "NombreCompleto";
            ddlTutorAlu.DataValueField = "UidUsuario";
            ddlTutorAlu.DataBind();

            string Matricula = row.Cells[3].Text;
            ViewState["Selected-Matricula"] = Matricula;

            lblHeadAlum.Text = "Alumno: " + row.Cells[3].Text;
            lblHeadCole.Text = string.Empty;

            //row.Cells[4].Text = "<b>" + row.Cells[4].Text + "</b>"; -- PREUBA DE LETRAS EN NEGRITA

            ViewState["gvAlumnosSelect"] = true;

            //LLenar campos de busqueda
            txtFiltroAlumIdentificador.Text = alumnosServices.lsAlumnosRLEGridViewModel.Find(x => x.UidAlumno == UidAlumno).VchIdentificador;
            txtFiltroAlumMatricula.Text = alumnosServices.lsAlumnosRLEGridViewModel.Find(x => x.UidAlumno == UidAlumno).VchMatricula;
            txtFiltroAlumNombre.Text = alumnosServices.lsAlumnosRLEGridViewModel.Find(x => x.UidAlumno == UidAlumno).VchNombres;
            txtFiltroAlumPaterno.Text = alumnosServices.lsAlumnosRLEGridViewModel.Find(x => x.UidAlumno == UidAlumno).VchApePaterno;
            txtFiltroAlumMaterno.Text = alumnosServices.lsAlumnosRLEGridViewModel.Find(x => x.UidAlumno == UidAlumno).VchApeMaterno;

            ViewState["gvPagosSelect"] = false;
            gvPagos.SelectedIndex = -1;

            colegiaturasServices.CargarPagosColegiaturasRLE(Guid.Parse(ViewState["UidClienteLocal"].ToString()), UidAlumno);
            gvPagos.DataSource = colegiaturasServices.lsPagosColegiaturasViewModel;
            gvPagos.DataBind();
        }
        #endregion

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
                }

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
            gvPagos.DataSource = colegiaturasServices.lsPagosColegiaturasViewModel;
            gvPagos.DataBind();
        }
        protected void gvPagos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvPagos, "Select$" + e.Row.RowIndex);
            }
        }
        protected void gvPagos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["gvPagosSelect"] = true;

            GridViewRow row = gvAlumnos.SelectedRow;

            Guid UidFechaColegiatura = Guid.Parse(gvPagos.SelectedDataKey.Value.ToString());

            DateTime HoraDelServidor = DateTime.Now;
            DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

            colegiaturasServices.lsDesglosePagosGridViewModel.Clear();
            colegiaturasServices.ObtenerPagosColegiaturasRLE(Guid.Parse(ViewState["UidClienteLocal"].ToString()), UidFechaColegiatura, Guid.Parse(ViewState["Selected-UidAlumno"].ToString()));

            ViewState["Selected-UidAlumno"] = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.UidAlumno;
            ViewState["Selected-UidFechaColegiatura"] = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.UidFechaColegiatura;
            ViewState["Selected-Identificador"] = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchIdentificador;
            lblHeadCole.Text = "Colegiatura: " + ViewState["Selected-Identificador"].ToString() + " " + colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchNum;
            headAlumno.Text = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.NombreCompleto;
            headMatricula.Text = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchMatricula;
            headFPago.Text = hoy.ToString("dd/MM/yyyy");
            ViewState["headFPago"] = hoy;
            decimal Recargo = 0;

            colegiaturasServices.FormarDesgloseCole(1, colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchIdentificador + ". PAGO " + colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchNum, decimal.Parse(colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.DcmImporte.ToString("N2")));

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

                        if (colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.BitBeca)
                        {
                            colegiaturasServices.FormarDesgloseCole(3, "RECARGO POR FECHA LIMITE (" + colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchFHLimite + ")", decimal.Parse(recargoTotalLimite.ToString("N2")));
                        }
                        else
                        {
                            colegiaturasServices.FormarDesgloseCole(2, "RECARGO POR FECHA LIMITE (" + colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchFHLimite + ")", decimal.Parse(recargoTotalLimite.ToString("N2")));
                        }
                    }
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

            Recargo = recargoTotalLimite + recargoTotalPeriodo;

            decimal ImporteTotal = ImporteCole;
            decimal ImporteBeca = 0;

            string TipoBeca = string.Empty;

            if (colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.BitBeca)
            {
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
            }

            ImporteTotal = ImporteTotal + Recargo;

            if (ImporteBeca >= 1)
            {
                colegiaturasServices.FormarDesgloseCole(2, "DESCUENTO BECA (" + TipoBeca + ")", -decimal.Parse(ImporteBeca.ToString("N2")), "#f55145");
            }

            pagosColegiaturasServices.ObtenerPagosPadres(Guid.Parse(ViewState["Selected-UidFechaColegiatura"].ToString()), Guid.Parse(ViewState["Selected-UidAlumno"].ToString()));

            if (pagosColegiaturasServices.lsFechasPagosColegiaturasViewModel.Count >= 1)
            {
                foreach (var item in pagosColegiaturasServices.lsFechasPagosColegiaturasViewModel)
                {
                    int num = colegiaturasServices.lsDesglosePagosGridViewModel.Count();
                    colegiaturasServices.FormarDesgloseCole(num + 1, "Pago parcial (" + item.DtFHPago.ToString("dd/MM/yyyy") + ")", -decimal.Parse(item.DcmImportePagado.ToString("N2")), "#f55145");
                }

                ImporteTotal = ImporteTotal - pagosColegiaturasServices.lsFechasPagosColegiaturasViewModel.Sum(x => x.DcmImportePagado);
            }

            lblValidarImportetb.Text = "$0.00";
            trValidarImportetb.Attributes.Add("style", "display:none;");
            ViewState["ValidarImporte"] = 0;
            ViewState["booltrValidarImporte"] = false;

            pagosColegiaturasServices.ObtenerPagosPendientesPadres(Guid.Parse(ViewState["Selected-UidFechaColegiatura"].ToString()), Guid.Parse(ViewState["Selected-UidAlumno"].ToString()));
            if (pagosColegiaturasServices.lsPagosPendientes.Count >= 1)
            {
                decimal ValidarImporte = pagosColegiaturasServices.lsPagosPendientes.Sum(x => x.DcmImportePagado);

                ImporteTotal = ImporteTotal - ValidarImporte;

                lblValidarImportetb.Text = "$-" + ValidarImporte.ToString("N2");
                trValidarImportetb.Attributes.Add("style", "");
                ViewState["ValidarImporte"] = ValidarImporte;
                ViewState["booltrValidarImporte"] = true;
            }

            ViewState["ImporteTotal"] = ImporteTotal;
            lblSubtotaltb.Text = "$" + (ImporteTotal + decimal.Parse(ViewState["ValidarImporte"].ToString())).ToString("N2");

            formasPagosServices.lsFormasPagos.Clear();
            ddlFormasPago.Items.Clear();

            formasPagosServices.CargarFormasPagosReporteLigasEscuelas(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
            ddlFormasPago.DataSource = formasPagosServices.lsFormasPagos;
            ddlFormasPago.DataTextField = "VchDescripcion";
            ddlFormasPago.DataValueField = "UidFormaPago";
            ddlFormasPago.DataBind();

            Calcular(ViewState["ImporteTotal"].ToString(), colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.UidColegiatura);

            pnlPromociones.Visible = true;

            btnGenerarPago.Visible = true;

            rptDesglose.DataSource = colegiaturasServices.lsDesglosePagosGridViewModel.OrderBy(x => x.IntNum);
            rptDesglose.DataBind();
        }
        protected void Calcular(string Importe, Guid UidTipoTarjeta)
        {
            ddlFormasPago_SelectedIndexChanged(null, null);
        }
        protected void ddlFormasPagoACTUAL_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ViewState["ImporteTotal"].ToString()) && decimal.Parse(ViewState["ImporteTotal"].ToString()) != 0)
            {
                if (ddlFormasPago.SelectedValue == "contado")
                {
                    decimal importe = decimal.Parse(ViewState["ImporteTotal"].ToString()) + decimal.Parse(ViewState["ImporteCCT"].ToString());

                    txtTotaltb.Text = importe.ToString("N2");
                    ViewState["txtImporteTotal.Text"] = importe.ToString("N2");

                    trPromociones.Attributes.Add("style", "display:none;");

                    ViewState["booltrPromociones"] = false;

                    lblPromotb.Text = "AL CONTADO(0%):";
                    lblImpPromotb.Text = "$0.00";
                    lblTotaltb.Text = "$" + importe.ToString("N2");
                }
                else
                {
                    trPromociones.Attributes.Add("style", "");
                    ViewState["booltrPromociones"] = true;

                    decimal importeTotal = decimal.Parse(ViewState["ImporteTotal"].ToString()) + decimal.Parse(ViewState["ImporteCCT"].ToString());

                    //foreach (var itPromo in promocionesServices.lsPromocionesColegiaturaModel.Where(x => x.UidPromocion == Guid.Parse(ddlFormasPago.SelectedValue)).ToList())
                    //{
                    //    decimal Valor = itPromo.DcmComicion * importeTotal / 100;
                    //    decimal Importe = Valor + importeTotal;

                    //    lblComisionPromocion.Text = Valor.ToString("N2");
                    //    lblPromotb.Text = "COMISIÓN " + itPromo.VchDescripcion + ":";
                    //    lblImpPromotb.Text = "$" + Valor.ToString("N2");

                    //    lblTotalPagar.Text = Importe.ToString("N2");
                    //    txtTotaltb.Text = Importe.ToString("N2");
                    //    lblTotalPago.Text = Importe.ToString("N2");
                    //    lblTotaltb.Text = "$" + Importe.ToString("N2");
                    //    ViewState["txtImporteTotal.Text"] = Importe.ToString("N2");

                    //    string dPromo = itPromo.VchDescripcion.Replace(" MESES", "");

                    //    lblToolPromo.Text = dPromo + " pagos mensuales de $" + (Importe / decimal.Parse(dPromo)).ToString("N2");
                    //}
                }

                lblTotalPago.Text = "Generar pago $" + ViewState["txtImporteTotal.Text"].ToString();
                btnGenerarPago.Enabled = true;
            }
            else
            {
                lblTotalPago.Text = "Generar pago $0.00";
                btnGenerarPago.Enabled = false;
            }

            btnCalcular_Click(null, null);

            if (trComisionTarjeta.Style.Value == "display:none;" && trPromociones.Style.Value == "display:none;")
            {
                trSubtotal.Attributes.Add("style", "display:none;");
                ViewState["booltrSubtotal"] = false;
            }
            else
            {
                trSubtotal.Attributes.Add("style", "");
                ViewState["booltrSubtotal"] = true;
            }
        }
        protected void btnCalcular_Click(object sender, EventArgs e)
        {
            btnGenerarPago.Enabled = false;

            ValidacionesServices validacionesServices = new ValidacionesServices();
            if (!validacionesServices.IsNumeric(txtTotaltb.Text))
            {
                pnlAlertPago.Visible = true;
                txtTotaltb.BackColor = System.Drawing.Color.FromName("#f2dede");
                lblMensajeAlertPago.Text = "El monto ingresado no es un formato correcto.";
                divAlertPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                return;
            }

            decimal Resta = 0;

            lblTotalPago.Text = "Generar pago $0.00";

            ViewState["txtTotaltb.Text"] = 0;

            if (string.IsNullOrEmpty(txtTotaltb.Text))
            {
                txtTotaltb.Text = "0";
            }

            decimal Total = decimal.Parse(ViewState["txtImporteTotal.Text"].ToString());
            decimal Totaltb = decimal.Parse(txtTotaltb.Text);

            if (Totaltb > Total)
            {
                pnlAlertPago.Visible = true;
                txtTotaltb.BackColor = System.Drawing.Color.FromName("#f2dede");
                lblMensajeAlertPago.Text = "El monto ingresado no puede ser mayor al  Total.";
                divAlertPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
            }
            else
            {

                ViewState["txtTotaltb.Text"] = decimal.Parse(txtTotaltb.Text);

                lblTotalPago.Text = "Generar pago $" + decimal.Parse(txtTotaltb.Text).ToString("N2");
                btnGenerarPago.Enabled = true;

                decimal SubTotal = 0;
                ViewState["ImpOtraSubTotalTT"] = 0;
                decimal ImporteCTT = 0;
                ViewState["ImpOtraCantCTT"] = 0;
                string CTT = string.Empty;
                decimal ImporteCPTT = 0;
                ViewState["ImpOtraCantCPTT"] = 0;
                string PromocionTT = string.Empty;

                if (promocionesTerminalServices.lsCBLPromocionesTerminalViewModel.Count != 0)
                {
                    foreach (var itPromo in promocionesTerminalServices.lsCBLPromocionesTerminalViewModel.Where(x => x.UidPromocionTerminal == Guid.Parse(ddlPromocionesTT.SelectedValue)).ToList())
                    {
                        ImporteCPTT = Totaltb * itPromo.DcmComicion / (100 + itPromo.DcmComicion);
                        ViewState["ImpOtraCantCPTT"] = ImporteCPTT;
                        PromocionTT = "Comisión " + itPromo.VchDescripcion + ": $" + ImporteCPTT.ToString("N2") + "<br />";
                    }
                }

                SubTotal = Totaltb - ImporteCPTT;
                if (ddlTiposTarjetas.SelectedValue != string.Empty)
                {
                    comisionesTarjetasClTer.CargarComisionesTarjetaTerminalTipoTarjeta(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse(ddlTiposTarjetas.SelectedValue));
                    if (comisionesTarjetasClTer.lsComisionesTarjetasClientesTerminal.Count >= 1)
                    {
                        foreach (var itComi in comisionesTarjetasClTer.lsComisionesTarjetasClientesTerminal)
                        {
                            if (itComi.BitComision)
                            {
                                ImporteCTT = SubTotal * itComi.DcmComision / (100 + itComi.DcmComision);
                                SubTotal = SubTotal - ImporteCTT;
                                ViewState["ImpOtraCantCTT"] = ImporteCTT;
                                CTT = "Comisión Bancaria: $" + ImporteCTT.ToString("N2") + "<br />";
                            }
                        }
                    }
                }
                Resta = decimal.Parse(ViewState["ImporteTotal"].ToString()) - SubTotal;
                ViewState["ImporteResta"] = Resta;

                lblToolApagar.Text = "Subtotal: $" + SubTotal.ToString("N2") + "<br />"
                                   + CTT
                                   + PromocionTT
                                   + "Total: $" + Totaltb.ToString("N2");

                ViewState["ImpOtraSubTotalTT"] = SubTotal;
            }

            txtTotaltb.Text = decimal.Parse(txtTotaltb.Text).ToString("N2");
            lblRestaTotal.Text = "Resta: $" + Resta.ToString("N2");
        }
        #endregion

        protected void ddlFormasPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            divTiposTarjetas.Visible = false;
            divPromocionesTT.Visible = false;

            if (ddlFormasPago.SelectedItem.Text == "EFECTIVO")
            {
                decimal importe = decimal.Parse(ViewState["ImporteTotal"].ToString());

                txtTotaltb.Text = importe.ToString("N2");
                ViewState["txtImporteTotal.Text"] = importe.ToString("N2");

                trPromociones.Attributes.Add("style", "display:none;");
                ViewState["booltrPromociones"] = false;

                lblPromotb.Text = "AL CONTADO(0%):";
                lblImpPromotb.Text = "$0.00";
                lblTotaltb.Text = "$" + importe.ToString("N2");

                ReiniciarComisionPTT();

                lblTotalPago.Text = "Generar pago $" + ViewState["txtImporteTotal.Text"].ToString();
                btnGenerarPago.Enabled = true;

                tiposTarjetasServices.lsTiposTarjetas.Clear();
                ddlTiposTarjetas.Items.Clear();

                promocionesTerminalServices.lsCBLPromocionesTerminalViewModel.Clear();
                ddlPromocionesTT.Items.Clear();
            }
            else
            {
                tiposTarjetasServices.lsTiposTarjetas.Clear();
                ddlTiposTarjetas.Items.Clear();

                tiposTarjetasServices.CargarTiposTarjetasCliente(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                ddlTiposTarjetas.DataSource = tiposTarjetasServices.lsTiposTarjetas;
                ddlTiposTarjetas.DataTextField = "VchDescripcion";
                ddlTiposTarjetas.DataValueField = "UidTipoTarjeta";
                ddlTiposTarjetas.DataBind();
            }

            ddlTiposTarjetas_SelectedIndexChanged(null, null);
        }

        protected void ddlTiposTarjetas_SelectedIndexChanged(object sender, EventArgs e)
        {
            decimal ImporteTotal = decimal.Parse(ViewState["ImporteTotal"].ToString());

            decimal ImporteCTT = 0;

            ReiniciarComisionPTT();

            if (ddlTiposTarjetas.SelectedValue != string.Empty)
            {
                comisionesTarjetasClTer.CargarComisionesTarjetaTerminalTipoTarjeta(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse(ddlTiposTarjetas.SelectedValue));
                if (comisionesTarjetasClTer.lsComisionesTarjetasClientesTerminal.Count >= 1)
                {
                    foreach (var itComi in comisionesTarjetasClTer.lsComisionesTarjetasClientesTerminal)
                    {
                        if (itComi.BitComision)
                        {
                            ImporteCTT = itComi.DcmComision * ImporteTotal / 100;
                            ViewState["ImporteCTT"] = ImporteCTT;

                            trComisionTarjeta.Attributes.Add("style", "");
                            ViewState["booltrComisionTipoTarjeta"] = true;

                            lblComisionTarjetatb.Text = "COMISIÓN BANCARIA:";
                            lblImpComisionTrajetatb.Text = "$" + ImporteCTT.ToString("N2");
                        }
                    }

                    decimal newImporteTotal = ImporteTotal + decimal.Parse(ViewState["ImporteCTT"].ToString());

                    txtTotaltb.Text = newImporteTotal.ToString("N2");
                    lblTotalPago.Text = newImporteTotal.ToString("N2");
                    lblTotaltb.Text = "$" + newImporteTotal.ToString("N2");
                    ViewState["txtImporteTotal.Text"] = newImporteTotal.ToString("N2");

                    lblTotalPago.Text = "Generar pago $" + newImporteTotal.ToString("N2");
                    btnGenerarPago.Enabled = true;

                    divTiposTarjetas.Visible = true;
                }

                trPromociones.Attributes.Add("style", "");
                ViewState["booltrPromociones"] = true;

                ddlPromocionesTT.Items.Clear();
                promocionesTerminalServices.CargarPromocionesPagosImporte(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse(ddlTiposTarjetas.SelectedValue), ImporteTotal);
                ddlPromocionesTT.DataSource = promocionesTerminalServices.lsCBLPromocionesTerminalViewModel;
                ddlPromocionesTT.DataTextField = "VchDescripcion";
                ddlPromocionesTT.DataValueField = "UidPromocionTerminal";
                ddlPromocionesTT.DataBind();
            }

            ddlPromocionesTT_SelectedIndexChanged(null, null);
        }

        protected void ddlPromocionesTT_SelectedIndexChanged(object sender, EventArgs e)
        {
            decimal importeTotal = decimal.Parse(ViewState["ImporteTotal"].ToString()) + decimal.Parse(ViewState["ImporteCTT"].ToString());

            if (promocionesTerminalServices.lsCBLPromocionesTerminalViewModel.Count != 0)
            {
                foreach (var itPromo in promocionesTerminalServices.lsCBLPromocionesTerminalViewModel.Where(x => x.UidPromocionTerminal == Guid.Parse(ddlPromocionesTT.SelectedValue)).ToList())
                {
                    decimal Valor = itPromo.DcmComicion * importeTotal / 100;
                    decimal Importe = Valor + importeTotal;

                    lblPromotb.Text = "COMISIÓN " + itPromo.VchDescripcion + ":";
                    lblImpPromotb.Text = "$" + Valor.ToString("N2");

                    txtTotaltb.Text = Importe.ToString("N2");
                    lblTotalPago.Text = Importe.ToString("N2");
                    lblTotaltb.Text = "$" + Importe.ToString("N2");
                    ViewState["txtImporteTotal.Text"] = Importe.ToString("N2");

                    string dPromo = itPromo.VchDescripcion.Replace(" MESES", "");

                    lblToolPromo.Text = dPromo + " pagos mensuales de $" + (Importe / decimal.Parse(dPromo)).ToString("N2");
                }
                divPromocionesTT.Visible = true;
            }
            else
            {
                trPromociones.Attributes.Add("style", "display:none;");
                ViewState["booltrPromociones"] = false;

                lblPromotb.Text = "AL CONTADO(0%):";
                lblImpPromotb.Text = "$0.00";
            }

            lblTotalPago.Text = "Generar pago $" + ViewState["txtImporteTotal.Text"].ToString();
            btnGenerarPago.Enabled = true;

            btnCalcular_Click(null, null);

            if (trValidarImportetb.Style.Value == "display:none;" && trComisionTarjeta.Style.Value == "display:none;" && trPromociones.Style.Value == "display:none;")
            {
                trSubtotal.Attributes.Add("style", "display:none;");
                ViewState["booltrSubtotal"] = false;
            }
            else
            {
                trSubtotal.Attributes.Add("style", "");
                ViewState["booltrSubtotal"] = true;
            }
        }

        private void ReiniciarComisionPTT()
        {
            trComisionTarjeta.Attributes.Add("style", "display:none;");
            ViewState["ImporteCTT"] = 0;
            ViewState["booltrComisionTipoTarjeta"] = false;

            lblComisionTarjetatb.Text = "COMISIÓN BANCARIA:";
            lblImpComisionTrajetatb.Text = "0.00";
        }

        protected void btnAnterior_Click(object sender, EventArgs e)
        {
            ViewState["cantTabsAddPago"] = (int.Parse(ViewState["cantTabsAddPago"].ToString()) - 1);

            if (int.Parse(ViewState["cantTabsAddPago"].ToString()) == 0)
            {
                lblTitleModalTipoPago.Text = "Seleccione un alumno";

                btnAnterior.Visible = false;
                btnSiguiente.Visible = true;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Tabs", "showTabAlumno()", true);
            }

            if (int.Parse(ViewState["cantTabsAddPago"].ToString()) == 1)
            {
                btnAnterior.Visible = true;
                btnSiguiente.Visible = true;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Tabs", "showTabColegiatura()", true);
            }
        }
        protected void btnSiguiente_Click(object sender, EventArgs e)
        {
            ViewState["cantTabsAddPago"] = (int.Parse(ViewState["cantTabsAddPago"].ToString()) + 1);

            if (int.Parse(ViewState["cantTabsAddPago"].ToString()) == 1)
            {
                if (!bool.Parse(ViewState["gvAlumnosSelect"].ToString()))
                {
                    ViewState["cantTabsAddPago"] = (int.Parse(ViewState["cantTabsAddPago"].ToString()) - 1);
                    pnlAlertModalTipoPago.Visible = true;
                    lblMnsjModalTipoPago.Text = "Para continuar seleccione un alumno.";
                    divAlertModalTipoPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                    return;
                }

                lblTitleModalTipoPago.Text = "Seleccione una colegiatura";

                btnAnterior.Visible = true;
                btnSiguiente.Visible = true;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Tabs", "showTabColegiatura()", true);
            }
            else if (int.Parse(ViewState["cantTabsAddPago"].ToString()) == 2)
            {
                if (!bool.Parse(ViewState["gvPagosSelect"].ToString()))
                {
                    ViewState["cantTabsAddPago"] = (int.Parse(ViewState["cantTabsAddPago"].ToString()) - 1);
                    pnlAlertModalTipoPago.Visible = true;
                    lblMnsjModalTipoPago.Text = "Para continuar seleccione una colegiatura.";
                    divAlertModalTipoPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                    return;
                }

                btnAnterior.Visible = true;
                btnSiguiente.Visible = false;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Tabs", "showTabFinalizar()", true);
            }
        }

        protected void btnGenerarpago_Click(object sender, EventArgs e)
        {
            if (ddlTutorAlu.EmptyDropDownList())
            {
                pnlAlertPago.Visible = true;
                lblMensajeAlertPago.Text = "El campo Tutor del alumno es obligatorio.";
                divAlertPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                return;
            }

            decimal importeTotal = decimal.Parse(ViewState["txtTotaltb.Text"].ToString());

            if (importeTotal > 0)
            {
                lblMnsjDialog.Text = "<strong>Importe a pagar </strong>" +
                                 "<br />" +
                                 "<h2>$" + importeTotal + "</h2>";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalDialog", "showModalDialog()", true);
            }
            else
            {
                pnlAlertPago.Visible = true;
                txtTotaltb.BackColor = System.Drawing.Color.FromName("#f2dede");
                lblMensajeAlertPago.Text = "El importe debe ser mayor a $0.00.";
                divAlertPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
            }
        }
        protected void btnSi_Click(object sender, EventArgs e)
        {
            Guid UidPagoColegiatura = Guid.NewGuid();

            decimal importeTotal = decimal.Parse(ViewState["txtTotaltb.Text"].ToString());
            bool trSubtotal = bool.Parse(ViewState["booltrSubtotal"].ToString());
            bool trComisionTarjeta = bool.Parse(ViewState["booltrComisionTipoTarjeta"].ToString());
            bool trPromociones = bool.Parse(ViewState["booltrPromociones"].ToString());

            decimal DcmValidarImporte = decimal.Parse(ViewState["ValidarImporte"].ToString());
            bool trValidarImporte = bool.Parse(ViewState["booltrValidarImporte"].ToString());

            Guid EstatusPagoColegiatura = Guid.Parse("51B85D66-866B-4BC2-B08F-FECE1A994053");
            Guid estatusFechaPago = Guid.Parse("8720B2B9-5712-4E75-A981-932887AACDC9");

            bool BitTipoTarjeta = divTiposTarjetas.Visible;
            Guid UidTipoTarjeta = Guid.Empty;
            if (ddlTiposTarjetas.SelectedValue != string.Empty)
            {
                UidTipoTarjeta = Guid.Parse(ddlTiposTarjetas.SelectedValue);
            }
            bool BitPromocionTT = divPromocionesTT.Visible;
            Guid UidPromocionTerminal = Guid.Empty;
            if (ddlPromocionesTT.SelectedValue != string.Empty)
            {
                UidPromocionTerminal = Guid.Parse(ddlPromocionesTT.SelectedValue);
            }


            if (decimal.Parse(ViewState["ImporteResta"].ToString()) == 0)
            {
                colegiaturasServices.ActualizarEstatusColegiaturaAlumno(Guid.Parse(ViewState["Selected-UidFechaColegiatura"].ToString()), Guid.Parse(ViewState["Selected-UidAlumno"].ToString()), DateTime.Parse(headFPago.Text), Guid.Parse("5554CE57-1288-46D5-B36A-8AC69CB94B9A"));
            }


            int UltimoFolio = pagosColegiaturasServices.ObtenerUltimoFolio(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
            if (pagosColegiaturasServices.RegistrarPagoColegiatura(UidPagoColegiatura, UltimoFolio, DateTime.Parse(ViewState["headFPago"].ToString()), lblPromotb.Text, lblComisionTarjetatb.Text, trSubtotal, decimal.Parse(ViewState["ImpOtraSubTotalTT"].ToString()), trComisionTarjeta, decimal.Parse(ViewState["ImpOtraCantCTT"].ToString()), trPromociones, decimal.Parse(ViewState["ImpOtraCantCPTT"].ToString()), trValidarImporte, DcmValidarImporte, importeTotal, Guid.Parse(ddlTutorAlu.SelectedValue), EstatusPagoColegiatura,
                Guid.Parse(ViewState["Selected-UidFechaColegiatura"].ToString()), Guid.Parse(ViewState["Selected-UidAlumno"].ToString()), Guid.Parse(ddlFormasPago.SelectedValue), decimal.Parse(lblSubtotaltb.Text.Replace("$", "")), decimal.Parse(ViewState["ImpOtraSubTotalTT"].ToString()), decimal.Parse(ViewState["ImporteResta"].ToString()), estatusFechaPago))
            {
                foreach (var item in colegiaturasServices.lsDesglosePagosGridViewModel)
                {
                    detallesPagosColegiaturasServices.RegistrarDetallePagoColegiatura(item.IntNum, item.VchConcepto, item.DcmImporte, UidPagoColegiatura);
                }

                pagosColegiaturasServices.ActualizarImporteResta(Guid.Parse(ViewState["Selected-UidFechaColegiatura"].ToString()), Guid.Parse(ViewState["Selected-UidAlumno"].ToString()), decimal.Parse(ViewState["ImporteResta"].ToString()));

                pagosEfectivosServices.RegistrarPagoEfectivo(UidPagoColegiatura, DateTime.Parse(ViewState["headFPago"].ToString()), decimal.Parse(ViewState["ImpOtraSubTotalTT"].ToString()), BitTipoTarjeta, UidTipoTarjeta, BitPromocionTT, UidPromocionTerminal);

            }

            ValidarPago(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse(ddlTutorAlu.SelectedValue), Guid.Parse(ViewState["Selected-UidFechaColegiatura"].ToString()), Guid.Parse(ViewState["Selected-UidAlumno"].ToString()));

            string promocion = "0 MESES";
            string dPromo = "0 MESES";
            decimal rdlcDetaPromoImporte = 0;

            string tiposTarjetas = "Ninguna";

            if (ddlPromocionesTT.SelectedValue != string.Empty)
            {
                promocion = ddlPromocionesTT.SelectedItem.Text;

                dPromo = ddlPromocionesTT.SelectedItem.Text.Trim().Replace(" MESES", "");

                rdlcDetaPromoImporte = importeTotal / decimal.Parse(dPromo.Trim());
            }

            if (ddlTiposTarjetas.SelectedValue != string.Empty)
            {
                tiposTarjetas = ddlTiposTarjetas.SelectedItem.Text;
            }

            Session["rdlcAlumno"] = headAlumno.Text;
            Session["rdlcMatricula"] = headMatricula.Text;
            Session["rdlcFechaPago"] = headFPago.Text;
            Session["rdlcFormaPago"] = ddlFormasPago.SelectedItem.Text;
            Session["rdlcBoolTipoTarjeta"] = BitTipoTarjeta;
            Session["rdlcTipoTarjeta"] = tiposTarjetas;
            Session["rdlcBoolPromocion"] = BitPromocionTT;
            Session["rdlcPromocion"] = promocion;
            Session["rdlcDetallePromocion"] = dPromo.Trim() + " pagos mensuales de:";
            Session["rdlcDetaPromoImporte"] = rdlcDetaPromoImporte;
            Session["rdlcUidPagoColegiatura"] = UidPagoColegiatura;

            string _open = "window.open('Reports/ReciboPagoEfectivo2.aspx', '_blank');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);

            ActualizarDatosPrincipal();

            pnlAlert.Visible = true;
            lblMensajeAlert.Text = "<b>Felicidades,</b> el pago se registró exitosamente.";
            divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalDialog", "hideModalTipoPago()", true);
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            pagosEfectivosServices.ObtenerPagoEfectivoRLE(Guid.Parse(ViewState["RowCommand-UidPagoColegiatura"].ToString()));

            if (pagosEfectivosServices.lsrdlcPagosEfectivosViewModels.Count >= 1)
            {
                foreach (var item in pagosEfectivosServices.lsrdlcPagosEfectivosViewModels)
                {
                    decimal importeTotal = item.DcmTotal;
                    string promo = "0 MESES";
                    string dPromo = "0";
                    decimal rdlcDetaPromoImporte = 0;

                    if (item.PromocionTerminal != string.Empty && item.PromocionTerminal != "N/A")
                    {
                        promo = item.PromocionTerminal;
                        dPromo = promo.Trim().Replace(" MESES", "");

                        rdlcDetaPromoImporte = importeTotal / decimal.Parse(dPromo.Trim());
                    }

                    Session["rdlcAlumno"] = item.NombreCompleto;
                    Session["rdlcMatricula"] = item.VchMatricula;
                    Session["rdlcFechaPago"] = item.DtFHPago.ToString("dd/MM/yyyy");
                    Session["rdlcFormaPago"] = item.FormaPago;

                    Session["rdlcBoolTipoTarjeta"] = item.BitTipoTarjeta;
                    Session["rdlcTipoTarjeta"] = item.TipoTarjeta;
                    Session["rdlcBoolPromocion"] = item.BitPromocionTT;
                    Session["rdlcPromocion"] = promo;
                    Session["rdlcDetallePromocion"] = dPromo.Trim() + " pagos mensuales de:";
                    Session["rdlcDetaPromoImporte"] = rdlcDetaPromoImporte.ToString("N2");
                    Session["rdlcUidPagoColegiatura"] = Guid.Parse(ViewState["RowCommand-UidPagoColegiatura"].ToString());
                }

                string _open = "window.open('Reports/ReciboPagoEfectivo2.aspx', '_blank');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
            }
        }
    }
}