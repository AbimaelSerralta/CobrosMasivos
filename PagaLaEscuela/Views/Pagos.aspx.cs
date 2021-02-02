﻿using Franquicia.Bussiness;
using Franquicia.Domain.Models;
using Franquicia.Domain.Models.ClubPago;
using Franquicia.Domain.ViewModels;
using PagaLaEscuela.Util;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.Vista;
using static Franquicia.Bussiness.DateTimeSpanServices;

namespace PagaLaEscuela.Views
{
    public partial class Pagos : System.Web.UI.Page
    {
        PagosPadresServices pagosPadresServices = new PagosPadresServices();
        ColegiaturasServices colegiaturasServices = new ColegiaturasServices();
        ClientesServices clientesServices = new ClientesServices();
        PagosTarjetaServices pagosTarjetaServices = new PagosTarjetaServices();
        LigasUrlsServices ligasUrlsServices = new LigasUrlsServices();
        PromocionesServices promocionesServices = new PromocionesServices();
        ParametrosEntradaServices parametrosEntradaServices = new ParametrosEntradaServices();
        UsuariosCompletosServices usuariosCompletosServices = new UsuariosCompletosServices();
        ComisionesTarjetasClientesServices comisionesTarjetasCl = new ComisionesTarjetasClientesServices();
        SuperPromocionesServices superPromocionesServices = new SuperPromocionesServices();
        PagosColegiaturasServices pagosColegiaturasServices = new PagosColegiaturasServices();
        DetallesPagosColegiaturasServices detallesPagosColegiaturasServices = new DetallesPagosColegiaturasServices();

        FormasPagosServices formasPagosServices = new FormasPagosServices();
        BancosServices bancosServices = new BancosServices();
        PagosManualesServices pagosManualesServices = new PagosManualesServices();
        CorreosEscuelaServices correosEscuelaServices = new CorreosEscuelaServices();
        ValidacionesServices validacionesServices = new ValidacionesServices();
        PagosServices pagosServices = new PagosServices();
        ReferenciasClubPagoServices referenciasClubPagoServices = new ReferenciasClubPagoServices();
        ComisionesTarjetasClubPagoServices comisionesTarjetasClubPagoServices = new ComisionesTarjetasClubPagoServices();

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
                txtTotaltb.Attributes.Add("onchange", "button_click(this,'" + btnCalcular.ClientID + "')");

                tmValidar.Enabled = false;
                ViewState["gvPagos"] = SortDirection.Descending;

                Session["pagosPadresServices"] = pagosPadresServices;
                Session["colegiaturasServices"] = colegiaturasServices;
                Session["pagosTarjetaServices"] = pagosTarjetaServices;
                Session["ligasUrlsServices"] = ligasUrlsServices;
                Session["promocionesServices"] = promocionesServices;
                Session["parametrosEntradaServices"] = parametrosEntradaServices;
                Session["usuariosCompletosServices"] = usuariosCompletosServices;
                Session["comisionesTarjetasCl"] = comisionesTarjetasCl;

                Session["formasPagosServices"] = formasPagosServices;
                Session["bancosServices"] = bancosServices;

                pagosPadresServices.CargarComercios(Guid.Parse(ViewState["UidUsuarioLocal"].ToString()));
                rpComercios.DataSource = pagosPadresServices.lsPadresComerciosViewModels;
                rpComercios.DataBind();

                if (pagosPadresServices.lsPadresComerciosViewModels.Count >= 1)
                {
                    pnlComercios.Visible = true;
                    pnlSinEscuelas.Visible = false;
                }
                else
                {
                    pnlComercios.Visible = false;
                    pnlSinEscuelas.Visible = true;
                }
            }
            else
            {
                pagosPadresServices = (PagosPadresServices)Session["pagosPadresServices"];
                colegiaturasServices = (ColegiaturasServices)Session["colegiaturasServices"];
                pagosTarjetaServices = (PagosTarjetaServices)Session["pagosTarjetaServices"];
                ligasUrlsServices = (LigasUrlsServices)Session["ligasUrlsServices"];
                promocionesServices = (PromocionesServices)Session["promocionesServices"];
                parametrosEntradaServices = (ParametrosEntradaServices)Session["parametrosEntradaServices"];
                usuariosCompletosServices = (UsuariosCompletosServices)Session["usuariosCompletosServices"];
                comisionesTarjetasCl = (ComisionesTarjetasClientesServices)Session["comisionesTarjetasCl"];

                formasPagosServices = (FormasPagosServices)Session["formasPagosServices"];
                bancosServices = (BancosServices)Session["bancosServices"];

                pnlAlert.Visible = false;
                lblMensajeAlert.Text = "";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade");

                pnlAlertPago.Visible = false;
                lblMensajeAlertPago.Text = "";
                divAlertPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade");

                pnlAlertPago2.Visible = false;
                lblMensajeAlertPago2.Text = "";
                divAlertPago2.Attributes.Add("class", "alert alert-danger alert-dismissible fade");

                pnlAlertModalTipoPago.Visible = false;
                lblMnsjModalTipoPago.Text = "";
                divAlertModalTipoPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade");

                rpFormasPago.DataSource = formasPagosServices.lsFormasPagos;
                rpFormasPago.DataBind();
            }
        }

        protected void rpComercios_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Pagos")
            {
                Guid UidCliente = Guid.Parse((string)e.CommandArgument);
                ViewState["ItemCommand-UidCliente"] = UidCliente;

                DateTime HoraDelServidor = DateTime.Now;
                DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

                pnlComercios.Visible = false;
                pnlPagos.Visible = true;

                lblNombreComercial.Text = clientesServices.clientesRepository.CargarNombre(UidCliente);

                byte[] imagen = clientesServices.clientesRepository.CargarLogo(UidCliente);

                if (imagen != null)
                {
                    imgLogoSelect.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(imagen);
                    imgLogoSelect.DataBind();

                    imgLogoSelect2.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(imagen);
                    imgLogoSelect2.DataBind();

                    imgLogoSelect3.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(imagen);
                    imgLogoSelect3.DataBind();
                }
                else
                {
                    imgLogoSelect.ImageUrl = "../Images/SinLogo2.png";
                    imgLogoSelect.DataBind();

                    imgLogoSelect2.ImageUrl = "../Images/SinLogo2.png";
                    imgLogoSelect2.DataBind();

                    imgLogoSelect3.ImageUrl = "../Images/SinLogo2.png";
                    imgLogoSelect3.DataBind();
                }

                colegiaturasServices.CargarPagosColegiaturas(UidCliente, Guid.Parse(ViewState["UidUsuarioLocal"].ToString()), hoy);
                gvPagos.DataSource = colegiaturasServices.lsPagosColegiaturasViewModel;
                gvPagos.DataBind();
            }
        }
        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            pnlComercios.Visible = true;
            pnlPagos.Visible = false;
        }
        protected void btnActualizarLista_Click(object sender, EventArgs e)
        {
            DateTime HoraDelServidor = DateTime.Now;
            DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

            colegiaturasServices.CargarPagosColegiaturas(Guid.Parse(ViewState["ItemCommand-UidCliente"].ToString()), Guid.Parse(ViewState["UidUsuarioLocal"].ToString()), hoy);
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
            if (e.CommandName == "btnPagar")
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvPagos.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKey = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());

                ViewState["gvPagos_Index"] = index;
                ViewState["gvPagos_dataKey"] = dataKey;

                formasPagosServices.CargarFormasPagosPadres2();
                ddlTipoPago.DataSource = formasPagosServices.lsFormasPagos;
                ddlTipoPago.DataTextField = "VchDescripcion";
                ddlTipoPago.DataValueField = "UidFormaPago";
                ddlTipoPago.DataBind();

                ddlTipoPago_SelectedIndexChanged(null, null);

                gvPagos_RowCommandManual();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalPagar()", true);
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
                formasPagosServices.CargarFormasPagosPadres();
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

            if (e.CommandName == "btnPagos")
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvPagos.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKey = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["RowCommand-UidFechaColegiatura"] = dataKey;

                string Matri = gvPagos.Rows[index].Cells[1].Text;
                ViewState["RowCommand-Matricula"] = Matri;

                pagosColegiaturasServices.ObtenerPagosReportePadres(dataKey, Matri);
                gvPagosColegiaturas.DataSource = pagosColegiaturasServices.lsReportePadresFechasPagosColeViewModel;
                gvPagosColegiaturas.DataBind();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalPagos()", true);
            }
        }
        protected void gvPagos_RowCommandManual()
        {
            string Matri = gvPagos.Rows[int.Parse(ViewState["gvPagos_Index"].ToString())].Cells[1].Text;

            DateTime HoraDelServidor = DateTime.Now;
            DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

            lblTitlePagar.Text = "Pago colegiatura";
            btnCerrar.Visible = true;
            colegiaturasServices.lsDesglosePagosGridViewModel.Clear();
            colegiaturasServices.ObtenerPagosColegiaturas(Guid.Parse(ViewState["ItemCommand-UidCliente"].ToString()), Guid.Parse(ViewState["UidUsuarioLocal"].ToString()), Guid.Parse(ViewState["gvPagos_dataKey"].ToString()), Matri);

            ViewState["RowCommand-UidAlumno"] = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.UidAlumno;
            ViewState["RowCommand-UidFechaColegiatura"] = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.UidFechaColegiatura;
            ViewState["RowCommand-Identificador"] = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchIdentificador;
            ViewState["RowCommand-FHLimite"] = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchFHLimite;
            ViewState["RowCommand-FHVencimiento"] = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchFHVencimiento;
            ViewState["RowCommand-FHFinPeriodo"] = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.DtFHFinPeriodo;

            lblConcepto.Text = "Pago " + colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchNum + ", " + colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchMatricula + " " + colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.NombreCompleto;
            headAlumno.Text = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.NombreCompleto;
            headMatricula.Text = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchMatricula;
            lblVencimiento.Text = hoy.ToString("dd/MM/yyyy");
            headFPago.Text = hoy.ToString("dd/MM/yyyy");
            ViewState["headFPago"] = hoy;
            lblImporteCole.Text = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.DcmImporte.ToString("N2");
            lblRecargo.Text = "0.00";

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

            lblRecargo.Text = (recargoTotalLimite + recargoTotalPeriodo).ToString("N2");

            decimal ImporteTotal = decimal.Parse(lblImporteCole.Text) /*+ decimal.Parse(lblRecargo.Text)*/;

            lblTieneBeca.Text = "NO";
            lblTipoBeca.Text = "NINGUNO";
            lblImporteBeca.Text = "0.00";
            decimal ImporteBeca = 0;

            string TipoBeca = string.Empty;

            if (colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.BitBeca)
            {
                lblTieneBeca.Text = "SI";
                lblTipoBeca.Text = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchTipoBeca;

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

                lblImporteBeca.Text = ImporteBeca.ToString("N2");
            }

            ImporteTotal = ImporteTotal + decimal.Parse(lblRecargo.Text);

            if (ImporteBeca >= 1)
            {
                colegiaturasServices.FormarDesgloseCole(2, "DESCUENTO BECA (" + TipoBeca + ")", -decimal.Parse(ImporteBeca.ToString("N2")), "#f55145");
            }

            pagosColegiaturasServices.ObtenerPagosPadres(Guid.Parse(ViewState["RowCommand-UidFechaColegiatura"].ToString()), Guid.Parse(ViewState["RowCommand-UidAlumno"].ToString()));

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
            trValidarImporte.Attributes.Add("style", "display:none;");
            ViewState["ValidarImporte"] = 0;
            ViewState["booltrValidarImporte"] = false;

            pagosColegiaturasServices.ObtenerPagosPendientesPadres(Guid.Parse(ViewState["RowCommand-UidFechaColegiatura"].ToString()), Guid.Parse(ViewState["RowCommand-UidAlumno"].ToString()));
            if (pagosColegiaturasServices.lsPagosPendientes.Count >= 1)
            {
                decimal ValidarImporte = pagosColegiaturasServices.lsPagosPendientes.Sum(x => x.DcmImportePagado);

                ImporteTotal = ImporteTotal - ValidarImporte;

                lblValidarImportetb.Text = "$" + ValidarImporte.ToString("N2");
                trValidarImporte.Attributes.Add("style", "");
                ViewState["ValidarImporte"] = ValidarImporte;
                ViewState["booltrValidarImporte"] = true;
            }



            lblImporteTotal.Text = ImporteTotal.ToString("N2");
            lblSubtotaltb.Text = "$" + (ImporteTotal + decimal.Parse(ViewState["ValidarImporte"].ToString())).ToString("N2");

            ViewState["ImporteTotal"] = ImporteTotal;

            rptDesglose.DataSource = colegiaturasServices.lsDesglosePagosGridViewModel.OrderBy(x => x.IntNum);
            rptDesglose.DataBind();
        }
        protected void gvPagos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPagos.PageIndex = e.NewPageIndex;
            gvPagos.DataSource = colegiaturasServices.lsPagosColegiaturasViewModel;
            gvPagos.DataBind();
        }

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            pnlValidar.Visible = true;
            tmValidar.Enabled = true;
            pnlIframe.Visible = false;
            ViewState["tmValidar"] = DateTime.Now.AddSeconds(5).ToString();
        }
        protected void ddlTipoPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvPagos_RowCommandManual();

            if (Guid.Parse(ddlTipoPago.SelectedValue.ToString()) == Guid.Parse("31BE9A23-73EE-4F44-AF6C-6C0648DCEBF7"))
            {
                ViewState["ddlTipoPago_Selected"] = "Liga";

                //Calcula la comicion
                comisionesTarjetasCl.CargarComisionesTarjeta(Guid.Parse(ViewState["ItemCommand-UidCliente"].ToString()));

                decimal ImporteCCT = 0;

                trComisionTarjeta.Attributes.Add("style", "display:none;");
                ViewState["ImporteCCT"] = 0;
                ViewState["booltrComisionTarjeta"] = false;

                if (comisionesTarjetasCl.lsComisionesTarjetasClientes.Count >= 1)
                {
                    foreach (var itComi in comisionesTarjetasCl.lsComisionesTarjetasClientes)
                    {
                        if (itComi.BitComision)
                        {
                            ImporteCCT = itComi.DcmComision * decimal.Parse(ViewState["ImporteTotal"].ToString()) / 100;
                            ViewState["ImporteCCT"] = ImporteCCT;

                            trComisionTarjeta.Attributes.Add("style", "");
                            ViewState["booltrComisionTarjeta"] = true;

                            lblComisionTarjetatb.Text = "COMISIÓN BANCARIA:";
                            lblImpComisionTrajetatb.Text = "$" + ImporteCCT.ToString("N2");

                            lblComisionTarjeta.Text = ImporteCCT.ToString("N2");
                        }
                    }
                }

                Calcular(ViewState["ImporteTotal"].ToString(), colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.UidColegiatura);

                pnlPromociones.Visible = true;
                pnlIframe.Visible = false;

                btnFinalizar.Visible = false;

                btnGenerarLiga.Visible = true;
                divFormasPago.Visible = true;
            }
            else if (Guid.Parse(ddlTipoPago.SelectedValue.ToString()) == Guid.Parse("6BE13FFE-E567-4D4D-9CBC-37DA30EC23A5"))
            {
                ViewState["ddlTipoPago_Selected"] = "Comercios";

                //Calcula la comicion
                comisionesTarjetasClubPagoServices.CargarComisionesTarjeta(Guid.Parse(ViewState["ItemCommand-UidCliente"].ToString()));
                
                decimal ImporteComercio = 0;
                trComisionTarjeta.Attributes.Add("style", "display:none;");
                ViewState["ImporteComercio"] = 0;
                ViewState["booltrComisionComercio"] = false;

                if (comisionesTarjetasClubPagoServices.lsComisionesTarjetasClubPago.Count >= 1)
                {
                    foreach (var itComi in comisionesTarjetasClubPagoServices.lsComisionesTarjetasClubPago)
                    {
                        if (itComi.BitComision)
                        {
                            ImporteComercio = itComi.DcmComision * decimal.Parse(ViewState["ImporteTotal"].ToString()) / 100;
                            ViewState["ImporteComercio"] = ImporteComercio;

                            trComisionTarjeta.Attributes.Add("style", "");
                            ViewState["booltrComisionComercio"] = true;

                            lblComisionTarjetatb.Text = "COMISIÓN:";
                            lblImpComisionTrajetatb.Text = "$" + ImporteComercio.ToString("N2");

                            lblComisionTarjeta.Text = ImporteComercio.ToString("N2");
                        }
                    }
                }

                decimal importe = decimal.Parse(lblImporteTotal.Text) + decimal.Parse(ViewState["ImporteComercio"].ToString());

                lblTotalPagar.Text = importe.ToString("N2");
                txtTotaltb.Text = importe.ToString("N2");
                ViewState["txtImporteTotal.Text"] = importe.ToString("N2");

                trPromociones.Attributes.Add("style", "display:none;");

                ViewState["booltrPromociones"] = false;

                lblComisionPromocion.Text = "0.00";
                lblPromotb.Text = "AL CONTADO(0%):";
                lblImpPromotb.Text = "$0.00";
                lblTotaltb.Text = "$" + importe.ToString("N2");



                btnCalcular_Click(null, null);

                if (trValidarImporte.Style.Value == "display:none;" && trComisionTarjeta.Style.Value == "display:none;" && trPromociones.Style.Value == "display:none;")
                {
                    trSubtotal.Attributes.Add("style", "display:none;");
                    ViewState["booltrSubtotal"] = false;
                }
                else
                {
                    trSubtotal.Attributes.Add("style", "");
                    ViewState["booltrSubtotal"] = true;
                }

                btnGenerarLiga.Visible = true;
                divFormasPago.Visible = false;
            }
        }
        protected void Calcular(string Importe, Guid UidColegiatura)
        {
            if (!string.IsNullOrEmpty(Importe))
            {
                superPromocionesServices.CargarSuperPromociones();

                ddlFormasPago.Items.Clear();
                promocionesServices.CargarPromocionesPagosImporte(Guid.Parse(ViewState["ItemCommand-UidCliente"].ToString()), UidColegiatura, Importe);

                ddlFormasPago.Items.Insert(0, new ListItem("Al contado", "contado"));
                ddlFormasPago.DataSource = promocionesServices.lsPromocionesColegiaturaModel;
                ddlFormasPago.DataTextField = "VchDescripcion";
                ddlFormasPago.DataValueField = "UidPromocion";
                ddlFormasPago.DataBind();
            }

            ddlFormasPago_SelectedIndexChanged(null, null);
        }
        protected void ddlFormasPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblImporteTotal.Text) && decimal.Parse(lblImporteTotal.Text) != 0)
            {
                if (ddlFormasPago.SelectedValue == "contado")
                {
                    decimal importe = decimal.Parse(lblImporteTotal.Text) + decimal.Parse(ViewState["ImporteCCT"].ToString());

                    lblTotalPagar.Text = importe.ToString("N2");
                    txtTotaltb.Text = importe.ToString("N2");
                    ViewState["txtImporteTotal.Text"] = importe.ToString("N2");

                    trPromociones.Attributes.Add("style", "display:none;");

                    ViewState["booltrPromociones"] = false;

                    lblComisionPromocion.Text = "0.00";
                    lblPromotb.Text = "AL CONTADO(0%):";
                    lblImpPromotb.Text = "$0.00";
                    lblTotaltb.Text = "$" + importe.ToString("N2");
                }
                else
                {
                    trPromociones.Attributes.Add("style", "");
                    ViewState["booltrPromociones"] = true;

                    decimal importeTotal = decimal.Parse(lblImporteTotal.Text) + decimal.Parse(ViewState["ImporteCCT"].ToString());

                    foreach (var itPromo in promocionesServices.lsPromocionesColegiaturaModel.Where(x => x.UidPromocion == Guid.Parse(ddlFormasPago.SelectedValue)).ToList())
                    {
                        decimal Valor = itPromo.DcmComicion * importeTotal / 100;
                        decimal Importe = Valor + importeTotal;

                        lblComisionPromocion.Text = Valor.ToString("N2");
                        lblPromotb.Text = "COMISIÓN " + itPromo.VchDescripcion + ":";
                        lblImpPromotb.Text = "$" + Valor.ToString("N2");

                        lblTotalPagar.Text = Importe.ToString("N2");
                        txtTotaltb.Text = Importe.ToString("N2");
                        lblTotalPago.Text = Importe.ToString("N2");
                        lblTotaltb.Text = "$" + Importe.ToString("N2");
                        ViewState["txtImporteTotal.Text"] = Importe.ToString("N2");

                        string dPromo = itPromo.VchDescripcion.Replace(" MESES", "");

                        lblToolPromo.Text = dPromo + " pagos mensuales de $" + (Importe / decimal.Parse(dPromo)).ToString("N2");
                    }
                }

                lblTotalPago.Text = "Generar pago $" + ViewState["txtImporteTotal.Text"].ToString();
                btnGenerarLiga.Enabled = true;
            }
            else
            {
                lblTotalPagar.Text = string.Empty;
                lblTotalPago.Text = "Generar pago $0.00";
                btnGenerarLiga.Enabled = false;
            }

            btnCalcular_Click(null, null);

            if (trValidarImporte.Style.Value == "display:none;" && trComisionTarjeta.Style.Value == "display:none;" && trPromociones.Style.Value == "display:none;")
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
        protected void btnGenerarLiga_Click(object sender, EventArgs e)
        {
            if (ViewState["ddlTipoPago_Selected"].ToString() == "Liga")
            {
                string identificador = ViewState["RowCommand-Identificador"].ToString();
                string concepto = lblConcepto.Text;

                string vencimiento = lblVencimiento.Text;

                int intCorreo = 1;
                decimal importeTotal = decimal.Parse(ViewState["txtTotaltb.Text"].ToString());

                string url = string.Empty;
                bool resu = false;

                string MontoMin = "50.00";
                string MontoMax = "15000.00";

                parametrosEntradaServices.ObtenerParametrosEntradaCliente(Guid.Parse(ViewState["ItemCommand-UidCliente"].ToString()));

                string id_company = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.IdCompany;
                string id_branch = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.IdBranch;
                string user = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchUsuario;
                string pwd = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchPassword;
                string moneda = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchModena;
                string canal = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchCanal;
                string semillaAES = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchSemillaAES;
                string urlGen = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchUrl;
                string data0 = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchData0;

                if (!string.IsNullOrEmpty(id_company) && !string.IsNullOrEmpty(id_branch) && !string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(pwd) && !string.IsNullOrEmpty(moneda) && !string.IsNullOrEmpty(canal) && !string.IsNullOrEmpty(semillaAES) && !string.IsNullOrEmpty(urlGen) && !string.IsNullOrEmpty(data0))
                {
                    if (importeTotal >= decimal.Parse(MontoMin) && importeTotal <= decimal.Parse(MontoMax))
                    {
                        Guid UidLigaAsociado = Guid.NewGuid();

                        Guid UidPagoColegiatura = Guid.NewGuid();
                        Session["UidPagoColegiatura"] = UidPagoColegiatura;

                        usuariosCompletosServices.SelectUsuCliColegiatura(Guid.Parse(ViewState["ItemCommand-UidCliente"].ToString()), Guid.Parse(ViewState["UidUsuarioLocal"].ToString()));

                        foreach (var item in usuariosCompletosServices.lsPagoColegiaturaLiga)
                        {
                            if (ddlFormasPago.SelectedValue != "contado")
                            {
                                DateTime HoraDelServidor = DateTime.Now;
                                DateTime thisDay = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

                                string ReferenciaCobro = item.IdCliente.ToString() + item.IdUsuario.ToString() + thisDay.ToString("ddMMyyyyHHmmssfff");

                                string promocion = ddlFormasPago.SelectedValue.Replace(" MESES", "");

                                DateTime thisDay2 = DateTime.Now;
                                string Referencia = item.IdCliente.ToString() + item.IdUsuario.ToString() + thisDay2.ToString("ddMMyyyyHHmmssfff");

                                url = GenLigaPara(id_company, id_branch, user, pwd, Referencia, importeTotal, moneda, canal, promocion, intCorreo, vencimiento, item.StrCorreo, concepto, semillaAES, data0, urlGen);

                                if (url.Contains("https://"))
                                {
                                    if (usuariosCompletosServices.GenerarLigasPagosColegiatura(url, concepto, importeTotal, Referencia, item.UidUsuario, identificador, thisDay, DateTime.Parse(vencimiento), "PAGO COLEGIATURA", UidLigaAsociado, Guid.Parse(ddlFormasPago.SelectedValue), Guid.Parse(ViewState["RowCommand-UidFechaColegiatura"].ToString()), UidPagoColegiatura, Guid.Parse(ViewState["ItemCommand-UidCliente"].ToString())))
                                    {
                                        ViewState["IdReferencia"] = Referencia;

                                        pnlPromociones.Visible = false;
                                        pnlIframe.Visible = true;

                                        btnFinalizar.Visible = true;
                                        btnCerrar.Visible = false;

                                        ifrLiga.Src = url;

                                        resu = true;
                                    }
                                }
                                else
                                {
                                    resu = false;
                                    break;
                                }
                            }
                            else if (ddlFormasPago.SelectedValue == "contado")
                            {
                                DateTime thisDay = DateTime.Now;
                                string ReferenciaCobro = item.IdCliente.ToString() + item.IdUsuario.ToString() + thisDay.ToString("ddMMyyyyHHmmssfff");

                                string urlCobro = GenLigaPara(id_company, id_branch, user, pwd, ReferenciaCobro, importeTotal, moneda, canal, "C", intCorreo, vencimiento, item.StrCorreo, concepto, semillaAES, data0, urlGen);

                                if (urlCobro.Contains("https://"))
                                {
                                    if (usuariosCompletosServices.GenerarLigasPagosColegiatura(urlCobro, concepto, importeTotal, ReferenciaCobro, item.UidUsuario, identificador, thisDay, DateTime.Parse(vencimiento), "PAGO COLEGIATURA", Guid.Empty, Guid.Empty, Guid.Parse(ViewState["RowCommand-UidFechaColegiatura"].ToString()), UidPagoColegiatura, Guid.Parse(ViewState["ItemCommand-UidCliente"].ToString())))
                                    {
                                        ViewState["IdReferencia"] = ReferenciaCobro;

                                        pnlPromociones.Visible = false;
                                        pnlIframe.Visible = true;

                                        btnFinalizar.Visible = true;
                                        btnCerrar.Visible = false;

                                        ifrLiga.Src = urlCobro;

                                        resu = true;
                                    }
                                }
                                else
                                {
                                    resu = false;
                                }
                            }
                        }

                        if (resu)
                        {
                            bool trSubtotal = bool.Parse(ViewState["booltrSubtotal"].ToString());
                            bool trComisionTarjeta = bool.Parse(ViewState["booltrComisionTarjeta"].ToString());
                            bool trPromociones = bool.Parse(ViewState["booltrPromociones"].ToString());

                            decimal DcmValidarImporte = decimal.Parse(ViewState["ValidarImporte"].ToString());
                            bool trValidarImporte = bool.Parse(ViewState["booltrValidarImporte"].ToString());

                            Guid EstatusPagoColegiatura = Guid.Parse("3B1517E9-6E32-43E8-9D9C-A11CD08F6F55");
                            Guid estatusFechaPago = Guid.Parse("F25E4AAB-6044-46E9-A575-98DCBCCF7604");

                            if (decimal.Parse(ViewState["ImporteResta"].ToString()) == 0)
                            {
                                colegiaturasServices.ActualizarEstatusColegiaturaAlumno(Guid.Parse(ViewState["RowCommand-UidFechaColegiatura"].ToString()), Guid.Parse(ViewState["RowCommand-UidAlumno"].ToString()), DateTime.Parse(headFPago.Text), Guid.Parse("5554CE57-1288-46D5-B36A-8AC69CB94B9A"));
                            }

                            int UltimoFolio = pagosColegiaturasServices.ObtenerUltimoFolio(Guid.Parse(ViewState["ItemCommand-UidCliente"].ToString()));
                            if (pagosColegiaturasServices.RegistrarPagoColegiatura(UidPagoColegiatura, UltimoFolio, DateTime.Parse(ViewState["headFPago"].ToString()), lblPromotb.Text, lblComisionTarjetatb.Text, trSubtotal, decimal.Parse(ViewState["ImpOtraSubTotal"].ToString()), trComisionTarjeta, decimal.Parse(ViewState["ImpOtraCantCCT"].ToString()), trPromociones, decimal.Parse(ViewState["ImpOtraCantCP"].ToString()), trValidarImporte, DcmValidarImporte, importeTotal, Guid.Parse(ViewState["UidUsuarioLocal"].ToString()), EstatusPagoColegiatura,
                                Guid.Parse(ViewState["RowCommand-UidFechaColegiatura"].ToString()), Guid.Parse(ViewState["RowCommand-UidAlumno"].ToString()), Guid.Parse(ddlTipoPago.SelectedValue), decimal.Parse(lblSubtotaltb.Text.Replace("$", "")), decimal.Parse(ViewState["ImpOtraSubTotal"].ToString()), decimal.Parse(ViewState["ImporteResta"].ToString()), estatusFechaPago))
                            {
                                foreach (var item in colegiaturasServices.lsDesglosePagosGridViewModel)
                                {
                                    detallesPagosColegiaturasServices.RegistrarDetallePagoColegiatura(item.IntNum, item.VchConcepto, item.DcmImporte, UidPagoColegiatura);
                                }

                                pagosColegiaturasServices.ActualizarImporteResta(Guid.Parse(ViewState["RowCommand-UidFechaColegiatura"].ToString()), Guid.Parse(ViewState["RowCommand-UidAlumno"].ToString()), decimal.Parse(ViewState["ImporteResta"].ToString()));

                                ValidarPago(Guid.Parse(ViewState["ItemCommand-UidCliente"].ToString()), Guid.Parse(ViewState["UidUsuarioLocal"].ToString()), Guid.Parse(ViewState["RowCommand-UidFechaColegiatura"].ToString()), Guid.Parse(ViewState["RowCommand-UidAlumno"].ToString()));
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(url))
                            {
                                pnlAlertPago.Visible = true;
                                lblMensajeAlertPago.Text = "<b>¡Lo sentimos! </b> " + url + "." + "<br /> Las credenciales proporcionadas no son correctos, por favor contacte a los administradores.";
                                divAlertPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                            }
                            else
                            {
                                pnlAlertPago.Visible = true;
                                lblMensajeAlertPago.Text = "<b>¡Lo sentimos! </b> Las credenciales proporcionadas no son correctos, por favor contacte a los administradores.";
                                divAlertPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                            }
                        }
                    }
                    else
                    {
                        pnlAlertPago.Visible = true;
                        lblImporteTotal.BackColor = System.Drawing.Color.FromName("#f2dede");
                        lblMensajeAlertPago.Text = "El importe mínimo es de $50.00 y el máximo es de $15,000.00.";
                        divAlertPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                    }
                }
                else
                {
                    pnlAlertPago.Visible = true;
                    lblMensajeAlertPago.Text = "<b>¡Lo sentimos! </b> Esta empresa no cuenta con credenciales para generar ligas, por favor contacte a los administradores.";
                    divAlertPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                }

            }
            else if (ViewState["ddlTipoPago_Selected"].ToString() == "Comercios")
            {
                DateTime HoraDelServidor = DateTime.Now;
                DateTime thisDay = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

                DateTime DtVencimiento;

                if (ViewState["RowCommand-FHLimite"].ToString() != string.Empty)
                {
                    DtVencimiento = DateTime.Parse(ViewState["RowCommand-FHLimite"].ToString());
                }
                else if (ViewState["RowCommand-FHVencimiento"].ToString() != string.Empty)
                {
                    DtVencimiento = DateTime.Parse(ViewState["RowCommand-FHVencimiento"].ToString());
                }
                else
                {
                    DtVencimiento = DateTime.Parse(ViewState["RowCommand-FHFinPeriodo"].ToString());
                }

                //Referencia, IdPago, IdParcialidad
                var data = pagosColegiaturasServices.GenerarReferencia(Guid.Parse(ViewState["RowCommand-UidFechaColegiatura"].ToString()), Guid.Parse(ViewState["RowCommand-UidAlumno"].ToString()));

                if (data.Item4)
                {
                    string GenerarAccount = data.Item1;
                    int IdPago = data.Item2;
                    int IdParcialidad = data.Item3;

                    bool procesarPago = false;
                    string VchUrl = string.Empty;

                    decimal importeTotal = decimal.Parse(ViewState["txtTotaltb.Text"].ToString());
                    Guid UidPagoColegiatura = Guid.NewGuid();

                    //string g = "https://qa.clubpago.site/ReferenceGenerator/PayFormat/12345610_970609332000.pdf";

                    GenerarRefereciaClubPago generarRefereciaClubPago = new GenerarRefereciaClubPago();

                    List<ObtenerRefereciaPago> obtenerRefereciaPago = generarRefereciaClubPago.GenerarReferencia(lblConcepto.Text, txtTotaltb.Text.Replace(".", "").Replace(",", ""), GenerarAccount, "serralta@compuandsoft.com", headAlumno.Text, DtVencimiento.ToString("yyyy-MM-dd"));

                    if (obtenerRefereciaPago.Count != 0 && obtenerRefereciaPago != null)
                    {
                        string IdReferencia = string.Empty;
                        string VchFolio = string.Empty;

                        foreach (var item in obtenerRefereciaPago)
                        {
                            VchUrl = item.PayFormat;
                            IdReferencia = item.Reference;
                            VchFolio = item.Folio;
                        }

                        bool trSubtotal = bool.Parse(ViewState["booltrSubtotal"].ToString());
                        bool trComisionTarjeta = bool.Parse(ViewState["booltrComisionTarjeta"].ToString());
                        bool trPromociones = bool.Parse(ViewState["booltrPromociones"].ToString());

                        decimal DcmValidarImporte = decimal.Parse(ViewState["ValidarImporte"].ToString());
                        bool trValidarImporte = bool.Parse(ViewState["booltrValidarImporte"].ToString());

                        Guid EstatusPagoColegiatura = Guid.Parse("51B85D66-866B-4BC2-B08F-FECE1A994053");
                        Guid estatusFechaPago = Guid.Parse("F25E4AAB-6044-46E9-A575-98DCBCCF7604");

                        if (decimal.Parse(ViewState["ImporteResta"].ToString()) == 0)
                        {
                            colegiaturasServices.ActualizarEstatusColegiaturaAlumno(Guid.Parse(ViewState["RowCommand-UidFechaColegiatura"].ToString()), Guid.Parse(ViewState["RowCommand-UidAlumno"].ToString()), DateTime.Parse(headFPago.Text), Guid.Parse("5554CE57-1288-46D5-B36A-8AC69CB94B9A"));
                        }

                        int UltimoFolio = pagosColegiaturasServices.ObtenerUltimoFolio(Guid.Parse(ViewState["ItemCommand-UidCliente"].ToString()));
                        if (pagosColegiaturasServices.RegistrarPagoColegiatura2(UidPagoColegiatura, UltimoFolio, DateTime.Parse(ViewState["headFPago"].ToString()), lblPromotb.Text, lblComisionTarjetatb.Text, trSubtotal, decimal.Parse(ViewState["ImpOtraSubTotal"].ToString()), trComisionTarjeta, decimal.Parse(ViewState["ImpOtraCantCCT"].ToString()), trPromociones, decimal.Parse(ViewState["ImpOtraCantCP"].ToString()), trValidarImporte, DcmValidarImporte, importeTotal, Guid.Parse(ViewState["UidUsuarioLocal"].ToString()), EstatusPagoColegiatura,
                            IdParcialidad, Guid.Parse(ViewState["RowCommand-UidFechaColegiatura"].ToString()), Guid.Parse(ViewState["RowCommand-UidAlumno"].ToString()), Guid.Parse(ddlTipoPago.SelectedValue), decimal.Parse(lblSubtotaltb.Text.Replace("$", "")), decimal.Parse(ViewState["ImpOtraSubTotal"].ToString()), decimal.Parse(ViewState["ImporteResta"].ToString()), estatusFechaPago))
                        {
                            foreach (var item in colegiaturasServices.lsDesglosePagosGridViewModel)
                            {
                                detallesPagosColegiaturasServices.RegistrarDetallePagoColegiatura(item.IntNum, item.VchConcepto, item.DcmImporte, UidPagoColegiatura);
                            }

                            pagosColegiaturasServices.ActualizarImporteResta(Guid.Parse(ViewState["RowCommand-UidFechaColegiatura"].ToString()), Guid.Parse(ViewState["RowCommand-UidAlumno"].ToString()), decimal.Parse(ViewState["ImporteResta"].ToString()));

                            ValidarPago(Guid.Parse(ViewState["ItemCommand-UidCliente"].ToString()), Guid.Parse(ViewState["UidUsuarioLocal"].ToString()), Guid.Parse(ViewState["RowCommand-UidFechaColegiatura"].ToString()), Guid.Parse(ViewState["RowCommand-UidAlumno"].ToString()));
                        }

                        if (referenciasClubPagoServices.GenerarReferenciaPagosColegiatura(VchFolio, VchUrl, lblConcepto.Text, IdReferencia, Guid.Parse(ViewState["UidUsuarioLocal"].ToString()), ViewState["RowCommand-Identificador"].ToString(), thisDay, DateTime.Parse(lblVencimiento.Text), decimal.Parse(txtTotaltb.Text), "PAGO COLEGIATURA", UidPagoColegiatura, Guid.Parse(ViewState["ItemCommand-UidCliente"].ToString())))
                        {
                            procesarPago = true;
                        }

                        if (procesarPago)
                        {
                            pnlAlert.Visible = true;
                            lblMensajeAlert.Text = "<b>Felicidades,</b> la referencia se ha creado exitosamente, pase al establecimiento más cercano para pagar.";
                            divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                            colegiaturasServices.CargarPagosColegiaturas(Guid.Parse(ViewState["ItemCommand-UidCliente"].ToString()), Guid.Parse(ViewState["UidUsuarioLocal"].ToString()), thisDay);
                            gvPagos.DataSource = colegiaturasServices.lsPagosColegiaturasViewModel;
                            gvPagos.DataBind();

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModalPagar()", true);

                            string _open = "window.open('" + VchUrl + "', '_blank');";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
                        }
                    }
                    else
                    {
                        pnlAlertPago.Visible = true;
                        lblMensajeAlertPago.Text = "<b>¡Lo sentimos! </b> No se ha podido generar la referencia, por favor intentelo más tarde. Si el problema persiste por favor contacte a los administradores.";
                        divAlertPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                    }
                }
                else
                {
                    pnlAlertPago.Visible = true;
                    lblMensajeAlertPago.Text = "<b>¡Lo sentimos! </b> No se ha podido generar la referencia, por favor intentelo más tarde. Si el problema persiste por favor contacte a los administradores.";
                    divAlertPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                }
            }
        }
        private string GenLigaPara(string id_company, string id_branch, string user, string pwd, string Referencia, decimal Importe,
            string moneda, string canal, string promocion, int intCorreo, string Vencimiento, string Correo, string Concepto, string semillaAES, string data0, string urlGen)
        {
            string url = string.Empty;

            string ArchivoXml = "" +
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
                "<P>\r\n  " +
                "  <business>\r\n" +
                "    <id_company>" + id_company + "</id_company>\r\n" +
                "    <id_branch>" + id_branch + "</id_branch>\r\n" +
                "    <user>" + user + "</user>\r\n" +
                "    <pwd>" + pwd + "</pwd>\r\n" +
                "  </business>\r\n" +
                "  <url>\r\n" +
                "    <reference>" + Referencia + "</reference>\r\n" +
                "    <amount>" + Importe + "</amount>\r\n" +
                "    <moneda>" + moneda + "</moneda>\r\n" +
                "    <canal>" + canal + "</canal>\r\n" +
                "    <omitir_notif_default>1</omitir_notif_default>\r\n" +
                "    <promociones>" + promocion + "</promociones>\r\n" +
                "    <st_correo>" + intCorreo + "</st_correo>\r\n" +
                "    <fh_vigencia>" + Vencimiento + "</fh_vigencia>\r\n" +
                "    <mail_cliente>" + Correo + "</mail_cliente>\r\n" +
                "    <datos_adicionales>\r\n" +
                "      <data id=\"1\" display=\"true\">\r\n" +
                "        <label>Concepto:</label>\r\n" +
                "        <value>" + Concepto + "</value>\r\n" +
                "      </data>\r\n" +
                "      <data id=\"2\" display=\"false\">\r\n" +
                "        <label>Color</label>\r\n" +
                "        <value>Azul</value>\r\n" +
                "      </data>\r\n" +
                "    </datos_adicionales>\r\n" +
                "  </url>\r\n" +
                "</P>\r\n";
            string originalString = ArchivoXml;
            string key = semillaAES;
            AESCrypto aesCrypto = new AESCrypto();
            string encryptedString = aesCrypto.encrypt(originalString, key);
            string finalString = encryptedString.Replace("%", "%25").Replace(" ", "%20").Replace("+", "%2B").Replace("=", "%3D").Replace("/", "%2F");

            string encodedString = HttpUtility.UrlEncode("<pgs><data0>" + data0 + "</data0><data>" + encryptedString + "</data></pgs>");
            string postParam = "xml=" + encodedString;

            var client = new RestClient(urlGen);
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", postParam, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            var content = response.Content;

            string decryptedString = aesCrypto.decrypt(key, content);
            string str1 = decryptedString.Replace("<P_RESPONSE><cd_response>success</cd_response><nb_response></nb_response><nb_url>", "");
            url = str1.Replace("</nb_url></P_RESPONSE>", "");

            return url;
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
        protected void tmValidar_Tick(object sender, EventArgs e)
        {
            DateTime HoraDelServidor = DateTime.Now;
            DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

            if (DateTime.Compare(DateTime.Now, DateTime.Parse(ViewState["tmValidar"].ToString())) <= 0)
            {
                ltMnsj.Text = "Verificando...: " + (((Int32)DateTime.Parse(ViewState["tmValidar"].ToString()).Subtract(DateTime.Now).TotalSeconds) % 60).ToString();

                if (pagosTarjetaServices.ValidarPagoUsuarioFinal(ViewState["IdReferencia"].ToString()))
                {
                    pnlValidar.Visible = false;
                    tmValidar.Enabled = false;

                    pnlPromociones.Visible = true;
                    pnlIframe.Visible = false;

                    btnFinalizar.Visible = false;
                    btnGenerarLiga.Visible = true;

                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "<b>Felicidades,</b> su pago se proceso exitosamente.";
                    divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                    colegiaturasServices.CargarPagosColegiaturas(Guid.Parse(ViewState["ItemCommand-UidCliente"].ToString()), Guid.Parse(ViewState["UidUsuarioLocal"].ToString()), hoy);
                    gvPagos.DataSource = colegiaturasServices.lsPagosColegiaturasViewModel;
                    gvPagos.DataBind();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModalPagar()", true);
                }
            }
            else
            {
                pnlValidar.Visible = false;
                tmValidar.Enabled = false;

                colegiaturasServices.EliminarLigaColegiatura(ViewState["IdReferencia"].ToString());
                pagosColegiaturasServices.EliminarPagoColegiatura(Guid.Parse(Session["UidPagoColegiatura"].ToString()));

                pnlPromociones.Visible = true;
                pnlIframe.Visible = false;

                btnFinalizar.Visible = false;
                btnGenerarLiga.Visible = true;

                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "<b>Lo sentimos,</b> no se ha podido procesar su pago.";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");

                colegiaturasServices.CargarPagosColegiaturas(Guid.Parse(ViewState["ItemCommand-UidCliente"].ToString()), Guid.Parse(ViewState["UidUsuarioLocal"].ToString()), hoy);
                gvPagos.DataSource = colegiaturasServices.lsPagosColegiaturasViewModel;
                gvPagos.DataBind();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModalPagar()", true);
            }
        }

        protected void btnCalcular_Click(object sender, EventArgs e)
        {
            if (ViewState["ddlTipoPago_Selected"].ToString() == "Liga")
            {
                string MontoMin = "50.00";
                string MontoMax = "15000.00";
                decimal Resta = 0;

                lblTotalPagar.Text = string.Empty;
                lblTotalPago.Text = "Generar pago $0.00";
                btnGenerarLiga.Enabled = false;

                ViewState["txtTotaltb.Text"] = 0;

                if (string.IsNullOrEmpty(txtTotaltb.Text))
                {
                    txtTotaltb.Text = "0";
                }

                decimal Total = decimal.Parse(ViewState["txtImporteTotal.Text"].ToString());
                decimal Totaltb = decimal.Parse(txtTotaltb.Text);

                if (Totaltb >= decimal.Parse(MontoMin) && Totaltb <= decimal.Parse(MontoMax))
                {
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
                        btnGenerarLiga.Enabled = true;

                        decimal SubTotal = 0;
                        ViewState["ImpOtraSubTotal"] = 0;
                        decimal ImporteCCT = 0;
                        ViewState["ImpOtraCantCCT"] = 0;
                        string CCT = string.Empty;
                        decimal ImporteCP = 0;
                        ViewState["ImpOtraCantCP"] = 0;
                        string Promocion = string.Empty;

                        if (ddlFormasPago.SelectedValue != "contado")
                        {
                            foreach (var itPromo in promocionesServices.lsPromocionesColegiaturaModel.Where(x => x.UidPromocion == Guid.Parse(ddlFormasPago.SelectedValue)).ToList())
                            {
                                ImporteCP = Totaltb * itPromo.DcmComicion / (100 + itPromo.DcmComicion);
                                ViewState["ImpOtraCantCP"] = ImporteCP;
                                Promocion = "Comisión " + itPromo.VchDescripcion + ": $" + ImporteCP.ToString("N2") + "<br />";
                            }
                        }

                        SubTotal = Totaltb - ImporteCP;

                        comisionesTarjetasCl.CargarComisionesTarjeta(Guid.Parse(ViewState["ItemCommand-UidCliente"].ToString()));
                        if (comisionesTarjetasCl.lsComisionesTarjetasClientes.Count >= 1)
                        {
                            foreach (var itComi in comisionesTarjetasCl.lsComisionesTarjetasClientes)
                            {
                                if (itComi.BitComision)
                                {
                                    ImporteCCT = SubTotal * itComi.DcmComision / (100 + itComi.DcmComision);
                                    SubTotal = SubTotal - ImporteCCT;
                                    ViewState["ImpOtraCantCCT"] = ImporteCCT;
                                    CCT = "Comisión Bancaria: $" + ImporteCCT.ToString("N2") + "<br />";
                                }
                            }
                        }

                        Resta = decimal.Parse(lblImporteTotal.Text) - decimal.Parse(SubTotal.ToString("N2"));
                        ViewState["ImporteResta"] = Resta;

                        lblToolApagar.Text = "Subtotal: $" + SubTotal.ToString("N2") + "<br />"
                                           + CCT
                                           + Promocion
                                           + "Total: $" + Totaltb.ToString("N2");

                        ViewState["ImpOtraSubTotal"] = SubTotal;
                    }
                }
                else
                {
                    pnlAlertPago.Visible = true;
                    txtTotaltb.BackColor = System.Drawing.Color.FromName("#f2dede");
                    lblMensajeAlertPago.Text = "El importe mínimo es de $50.00 y el máximo es de $15,000.00.";
                    divAlertPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                }

                txtTotaltb.Text = decimal.Parse(txtTotaltb.Text).ToString("N2");
                lblRestaTotal.Text = "Resta: $" + Resta.ToString("N2");

            }
            else if (ViewState["ddlTipoPago_Selected"].ToString() == "Comercios")
            {
                lblTotalPagar.Text = string.Empty;
                lblTotalPago.Text = "Generar referencia $0.00";
                decimal Resta = 0;

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

                    lblTotalPago.Text = "Generar referencia $" + decimal.Parse(txtTotaltb.Text).ToString("N2");

                    decimal SubTotal = 0;
                    ViewState["ImpOtraSubTotal"] = 0;
                    decimal ImporteCCT = 0;
                    ViewState["ImpOtraCantCCT"] = 0;
                    string CCT = string.Empty;
                    decimal ImporteCP = 0;
                    ViewState["ImpOtraCantCP"] = 0;
                    string Promocion = string.Empty;

                    SubTotal = Totaltb - ImporteCP;

                    //Aqui va las comiciones del comercio
                    comisionesTarjetasClubPagoServices.CargarComisionesTarjeta(Guid.Parse(ViewState["ItemCommand-UidCliente"].ToString()));
                    if (comisionesTarjetasClubPagoServices.lsComisionesTarjetasClubPago.Count >= 1)
                    {
                        foreach (var itComi in comisionesTarjetasClubPagoServices.lsComisionesTarjetasClubPago)
                        {
                            if (itComi.BitComision)
                            {
                                ImporteCCT = SubTotal * itComi.DcmComision / (100 + itComi.DcmComision);
                                SubTotal = SubTotal - ImporteCCT;
                                ViewState["ImpOtraCantCCT"] = ImporteCCT;
                                CCT = "Comisión: $" + ImporteCCT.ToString("N2") + "<br />";
                            }
                        }
                    }


                    Resta = decimal.Parse(lblImporteTotal.Text) - decimal.Parse(SubTotal.ToString("N2"));
                    ViewState["ImporteResta"] = Resta;

                    lblToolApagar.Text = "Subtotal: $" + SubTotal.ToString("N2") + "<br />"
                                       + CCT
                                       + Promocion
                                       + "Total: $" + Totaltb.ToString("N2");

                    ViewState["ImpOtraSubTotal"] = SubTotal;
                }

                txtTotaltb.Text = decimal.Parse(txtTotaltb.Text).ToString("N2");
                lblRestaTotal.Text = "Resta: $" + Resta.ToString("N2");

                btnGenerarLiga.Visible = true;
                divFormasPago.Visible = false;
            }
        }

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
                    trdetalleoperacion.Style.Add("display", "");
                }
                else if (Guid.Parse(lblGvUidFormaPago.Text) == Guid.Parse("3359D33E-C879-4A8B-96D3-C6A211AF014F"))
                {
                    FormaPago = "TARJETA";
                    trdetalleoperacion.Style.Add("display", "none");
                }
                else if (Guid.Parse(lblGvUidFormaPago.Text) == Guid.Parse("D92A2C64-C797-4C96-AD18-C2A433081F37"))
                {
                    FormaPago = "EFECTIVO";
                    trDetalleOperacionManual.Style.Add("display", "none");
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

            if (e.CommandName == "btnCancelarRef")
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvPagosColegiaturas.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKey = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());

                //UidPagoColegiatura

                if (pagosColegiaturasServices.ActualizarEstatusFechaPago(dataKey, Guid.Parse("408431CA-DB94-4BAA-AB9B-8FF468A77582")))
                {
                    pagosColegiaturasServices.ObtenerPagosReportePadres(Guid.Parse(ViewState["RowCommand-UidFechaColegiatura"].ToString()), ViewState["RowCommand-Matricula"].ToString());
                    gvPagosColegiaturas.DataSource = pagosColegiaturasServices.lsReportePadresFechasPagosColeViewModel;
                    gvPagosColegiaturas.DataBind();
                }

            }

            if (e.CommandName == "btnReferenciaCP")
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvPagosColegiaturas.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKey = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());

                string _open = "window.open('" + referenciasClubPagoServices.ReimprimirReferenciaPagoColegiatura(dataKey) + "', '_blank');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
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
                    trvalidarimportee.Style.Add("display", "");
                }
                else
                {
                    trvalidarimportee.Style.Add("display", "none");
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

            //DateTime HoraDelServidor = DateTime.Now;
            //DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");
            DateTime hoy = DateTime.Parse(txtFHPago.Text);

            colegiaturasServices.lsDesglosePagosGridViewModel.Clear();
            colegiaturasServices.ObtenerPagosColegiaturas(Guid.Parse(ViewState["RowCommand-UidCliente"].ToString()), Guid.Parse(ViewState["UidUsuarioLocal"].ToString()), Guid.Parse(ViewState["RowCommand-UidFechaColegiatura"].ToString()), Matri);

            ViewState["RowCommand-UidAlumno"] = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.UidAlumno;
            ViewState["RowCommand-UidFechaColegiatura"] = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.UidFechaColegiatura;
            ViewState["RowCommand-Identificador"] = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchIdentificador;
            headAlumno2.Text = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.NombreCompleto;
            headMatricula2.Text = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.VchMatricula;
            headFPago2.Text = hoy.ToString("dd/MM/yyyy");
            ViewState["headFPago2"] = hoy;
            ViewState["ImporteCole2"] = colegiaturasServices.colegiaturasRepository.pagosColegiaturasViewModel.DcmImporte.ToString("N2");
            ViewState["Recargo2"] = "0.00";

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

            ViewState["Recargo2"] = (recargoTotalLimite + recargoTotalPeriodo).ToString("N2");

            decimal ImporteTotal = decimal.Parse(ViewState["ImporteCole2"].ToString()) /*+ decimal.Parse(lblRecargo.Text)*/;
            decimal ImporteCCT = 0;
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

            ImporteTotal = ImporteTotal + decimal.Parse(ViewState["Recargo2"].ToString());

            if (ImporteBeca >= 1)
            {
                colegiaturasServices.FormarDesgloseCole(2, "DESCUENTO BECA (" + TipoBeca + ")", -decimal.Parse(ImporteBeca.ToString("N2")), "#f55145");
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

            lblValidarImportetb2.Text = "$0.00";
            trValidarImporte2.Attributes.Add("style", "display:none;");
            ViewState["ValidarImporte2"] = 0;
            ViewState["booltrValidarImporte2"] = false;

            pagosColegiaturasServices.ObtenerPagosPendientesPadres(Guid.Parse(ViewState["RowCommand-UidFechaColegiatura"].ToString()), Guid.Parse(ViewState["RowCommand-UidAlumno"].ToString()));
            if (pagosColegiaturasServices.lsPagosPendientes.Count >= 1)
            {
                decimal ValidarImporte = pagosColegiaturasServices.lsPagosPendientes.Sum(x => x.DcmImportePagado);

                lblValidarImportetb2.Text = "$" + ValidarImporte.ToString("N2");
                trValidarImporte2.Attributes.Add("style", "");
                ViewState["ValidarImporte2"] = ValidarImporte;
                ViewState["booltrValidarImporte2"] = true;
            }

            lblSubtotaltb2.Text = "$" + ImporteTotal.ToString("N2");
            ViewState["lblSubtotaltb2"] = ImporteTotal.ToString("N2");

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

            if (!string.IsNullOrEmpty(ViewState["lblSubtotaltb2"].ToString()) && decimal.Parse(ViewState["lblSubtotaltb2"].ToString()) != 0)
            {
                if (ddlFormasPago2.SelectedValue == "contado")
                {
                    decimal Totaltb2 = decimal.Parse(ViewState["lblSubtotaltb2"].ToString()) - decimal.Parse(ViewState["ValidarImporte2"].ToString());
                    decimal ImportePagartb = decimal.Parse(txtMontoPagado.Text);

                    lblTotaltb2.Text = "$" + Totaltb2.ToString("N2");
                    ViewState["lblTotaltb2"] = Totaltb2.ToString("N2");

                    lblImportePagartb.Text = ImportePagartb.ToString("N2");
                    ViewState["lblImportePagartb"] = ImportePagartb;

                    lblRestaTotal2.Text = "$" + (Totaltb2 - ImportePagartb).ToString("N2");
                    ViewState["lblRestaTotal2"] = Totaltb2 - ImportePagartb;
                }

                if (decimal.Parse(ViewState["lblImportePagartb"].ToString()) > decimal.Parse(ViewState["lblTotaltb2"].ToString()))
                {
                    pnlAlertPago2.Visible = true;
                    lblImportePagartb.BackColor = System.Drawing.Color.FromName("#f2dede");
                    lblMensajeAlertPago2.Text = "El monto ingresado no puede ser mayor al Total.";
                    divAlertPago2.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");

                    lblTotalPago2.Text = "Generar pago $0.00";
                    btnGenerarPago2.Enabled = false;

                    lblRestaTotal2.Text = "$0.00";
                }
                else
                {
                    lblTotalPago2.Text = "Generar pago $" + ViewState["lblImportePagartb"].ToString();
                    btnGenerarPago2.Enabled = true;
                }
            }
            else
            {
                lblTotalPago2.Text = "Generar pago $0.00";
                btnGenerarPago2.Enabled = false;

                lblRestaTotal2.Text = "$0.00";
            }

            //CalcularManual();

            if (trValidarImporte2.Style.Value == "display:none;")
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

            lblTotalPago2.Text = "Generar pago $0.00";
            btnGenerarPago2.Enabled = false;

            ViewState["txtTotaltb.Text"] = 0;

            if (string.IsNullOrEmpty(lblImportePagartb.Text))
            {
                lblImportePagartb.Text = "0";
            }

            decimal Total = decimal.Parse(ViewState["lblTotaltb2"].ToString());
            decimal ImportePagartb = decimal.Parse(lblImportePagartb.Text);

            if (ImportePagartb >= decimal.Parse(MontoMin) && ImportePagartb <= decimal.Parse(MontoMax))
            {
                if (ImportePagartb > Total)
                {
                    pnlAlertPago.Visible = true;
                    lblImportePagartb.BackColor = System.Drawing.Color.FromName("#f2dede");
                    lblMensajeAlertPago.Text = "El monto ingresado no puede ser mayor al  Total.";
                    divAlertPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                }
                else
                {

                    ViewState["txtTotaltb.Text"] = decimal.Parse(lblImportePagartb.Text);

                    lblTotalPago2.Text = "Generar pago $" + decimal.Parse(lblImportePagartb.Text).ToString("N2");
                    btnGenerarPago2.Enabled = true;

                    decimal SubTotal = 0;
                    ViewState["ImpOtraSubTotal"] = 0;
                    decimal ImporteCCT = 0;
                    ViewState["ImpOtraCantCCT"] = 0;
                    string CCT = string.Empty;
                    decimal ImporteCP = 0;
                    ViewState["ImpOtraCantCP"] = 0;
                    string Promocion = string.Empty;

                    SubTotal = ImportePagartb;

                    Resta = decimal.Parse(ViewState["lblSubtotaltb2"].ToString()) - SubTotal;
                    ViewState["ImporteResta"] = Resta;

                    ViewState["ImpOtraSubTotal"] = SubTotal;
                }
            }
            else
            {
                pnlAlertPago.Visible = true;
                lblImportePagartb.BackColor = System.Drawing.Color.FromName("#f2dede");
                lblMensajeAlertPago.Text = "El importe mínimo es de $50.00 y el máximo es de $15,000.00.";
                divAlertPago.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
            }

            lblImportePagartb.Text = decimal.Parse(lblImportePagartb.Text).ToString("N2");
            lblRestaTotal2.Text = "$" + Resta.ToString("N2");
        }
        protected void btnGenerarPago_Click(object sender, EventArgs e)
        {
            lblMnsjDialog.Text = "<strong>Importe a pagar </strong>" +
                                 "<br />" +
                                 "<h2>$" + lblImportePagartb.Text + "</h2>";

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
            Guid UidPagoColegiatura = Guid.NewGuid();

            decimal DcmImportePagar = decimal.Parse(ViewState["lblImportePagartb"].ToString());
            decimal DcmSubtotal = decimal.Parse(ViewState["lblSubtotaltb2"].ToString());
            bool trSubtotal = bool.Parse(ViewState["booltrSubtotal"].ToString());

            decimal DcmComisionTarjeta = 0;
            bool trComisionTarjeta = false;
            decimal DcmComisionPromocion = 0;
            bool trPromociones = false;
            decimal DcmValidarImporte = decimal.Parse(ViewState["ValidarImporte2"].ToString());
            bool trValidarImporte = bool.Parse(ViewState["booltrValidarImporte2"].ToString());

            Guid EstatusPagoColegiatura = Guid.Parse("51B85D66-866B-4BC2-B08F-FECE1A994053");
            Guid estatusFechaPago = Guid.Parse("F25E4AAB-6044-46E9-A575-98DCBCCF7604");

            string Correo = validacionesServices.ObtenerCorreoUsuario(Guid.Parse(ViewState["UidUsuarioLocal"].ToString()));

            if (decimal.Parse(ViewState["lblRestaTotal2"].ToString()) == 0)
            {
                colegiaturasServices.ActualizarEstatusColegiaturaAlumno(Guid.Parse(ViewState["RowCommand-UidFechaColegiatura"].ToString()), Guid.Parse(ViewState["RowCommand-UidAlumno"].ToString()), DateTime.Parse(headFPago2.Text), Guid.Parse("5554CE57-1288-46D5-B36A-8AC69CB94B9A"));
            }

            int UltimoFolio = pagosColegiaturasServices.ObtenerUltimoFolio(Guid.Parse(ViewState["ItemCommand-UidCliente"].ToString()));
            if (pagosColegiaturasServices.RegistrarPagoColegiatura(UidPagoColegiatura, UltimoFolio, DateTime.Parse(ViewState["headFPago2"].ToString()), "AL CONTADO(0%):", "", trSubtotal, DcmSubtotal, trComisionTarjeta, DcmComisionTarjeta, trPromociones, DcmComisionPromocion, trValidarImporte, DcmValidarImporte, DcmImportePagar, Guid.Parse(ViewState["UidUsuarioLocal"].ToString()), EstatusPagoColegiatura,
                Guid.Parse(ViewState["RowCommand-UidFechaColegiatura"].ToString()), Guid.Parse(ViewState["RowCommand-UidAlumno"].ToString()), Guid.Parse(ViewState["RowCommand-UidFormaPago"].ToString()), decimal.Parse(ViewState["lblTotaltb2"].ToString()), DcmImportePagar, decimal.Parse(ViewState["lblRestaTotal2"].ToString()), estatusFechaPago))
            {
                pagosManualesServices.RegistrarPagoManual(Guid.Parse(ddlBanco.SelectedValue), txtCuenta.Text, DateTime.Parse(ViewState["headFPago2"].ToString()), decimal.Parse(txtMontoPagado.Text), txtFolioPago.Text, UidPagoColegiatura);

                foreach (var item in colegiaturasServices.lsDesglosePagosGridViewModel)
                {
                    detallesPagosColegiaturasServices.RegistrarDetallePagoColegiatura(item.IntNum, item.VchConcepto, item.DcmImporte, UidPagoColegiatura);
                }

                pagosColegiaturasServices.ActualizarImporteResta(Guid.Parse(ViewState["RowCommand-UidFechaColegiatura"].ToString()), Guid.Parse(ViewState["RowCommand-UidAlumno"].ToString()), decimal.Parse(ViewState["lblRestaTotal2"].ToString()));

                correosEscuelaServices.CorreoEnvioPagoColegiaturaManual(headAlumno2.Text, headMatricula2.Text, DateTime.Parse(headFPago2.Text), trSubtotal, DcmSubtotal, trValidarImporte, DcmValidarImporte, decimal.Parse(ViewState["lblTotaltb2"].ToString()), DcmImportePagar, colegiaturasServices.lsDesglosePagosGridViewModel, "Comprobante de pago de colegiatura", ddlBanco.SelectedItem.Text, "************" + txtCuenta.Text, DateTime.Parse(txtFHPago.Text), txtFolioPago.Text, Correo, "PROCESANDO");

                DateTime HoraDelServidor = DateTime.Now;
                DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

                colegiaturasServices.CargarPagosColegiaturas(Guid.Parse(ViewState["ItemCommand-UidCliente"].ToString()), Guid.Parse(ViewState["UidUsuarioLocal"].ToString()), hoy);
                gvPagos.DataSource = colegiaturasServices.lsPagosColegiaturasViewModel;
                gvPagos.DataBind();

                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "<b>Felicidades,</b> su pago se registró exitosamente. Ahora solo falta que la escuela lo verifique.";
                divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalDialog", "hideModalTipoPago()", true);
            }
        }
        #endregion

        protected void btnPraga_Click(object sender, EventArgs e)
        {
            GenerarLigaPraga generarLigaPraga = new GenerarLigaPraga();

            generarLigaPraga.ApiGenerarURL(decimal.Parse("1500"), "MXN", "19/01/2021", "0123456789", "401", "0000000000000000000000", "PagaLaEscuela");
        }
    }
}