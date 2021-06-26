using Franquicia.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Franquicia.WebForms.Views
{
    public partial class LigaUsuario : System.Web.UI.Page
    {
        LigasUrlsServices ligasUrlsServices = new LigasUrlsServices();
        PagosTarjetaServices pagosTarjetaServices = new PagosTarjetaServices();
        PromocionesServices promocionesServices = new PromocionesServices();
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
                tmValidar.Enabled = false;

                ViewState["gvLigasGeneradas"] = SortDirection.Ascending;
                ViewState["SoExgvLigasGeneradas"] = "";

                Session["ReporteLigasUsuariosligasUrlsServices"] = ligasUrlsServices;
                Session["ReporteLigasUsuariospagosTarjetaServices"] = pagosTarjetaServices;
                Session["ReporteLigasUsuariospromocionesServices"] = promocionesServices;

                ligasUrlsServices.ConsultarLigaUsuarioFinal(Guid.Parse(ViewState["UidUsuarioLocal"].ToString()));
                gvLigasGeneradas.DataSource = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel;
                gvLigasGeneradas.DataBind();
            }
            else
            {
                ligasUrlsServices = (LigasUrlsServices)Session["ReporteLigasUsuariosligasUrlsServices"];
                pagosTarjetaServices = (PagosTarjetaServices)Session["ReporteLigasUsuariospagosTarjetaServices"];
                promocionesServices = (PromocionesServices)Session["ReporteLigasUsuariospromocionesServices"];

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

                ligasUrlsServices.FormarCabeceraDetalleFinal(dataKey);
                foreach (var item in ligasUrlsServices.lsLigasUrlsDetalleGridViewModel)
                {
                    lblDetaCliente.Text = "Cliente: " + item.NombreCompleto;
                    lblDetaFHpago.Text = "Fecha Pago: " + item.FechaPago;
                    lblDetaIdentificador.Text = "Identificador: " + item.VchIdentificador;
                    lblDetaAsunto.Text = "Asunto: " + item.VchAsunto;
                }

                rpDetalleLiga.DataSource = ligasUrlsServices.lsLigasUrlsDetalleGridViewModel;
                rpDetalleLiga.DataBind();

                foreach (var item in ligasUrlsServices.lsLigasUrlsDetalleGridViewModel)
                {
                    DcmSubtotal.Text = "$" + item.DcmImporte.ToString("N2");

                    VchComisionBancaria.Text = "COMISIÓN BANCARIA";
                    DcmImpComisionBancaria.Text = "$" + item.DcmComisionBancaria.ToString("N2");

                    VchPromocion.Text = "COMISIÓN " + item.VchPromocion;
                    DcmImpPromocion.Text = "$" + item.DcmPromocionDePago.ToString("N2");

                    DcmTotal.Text = "$" + item.DcmImportePromocion.ToString("n2");
                }

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

                rptMovimientosLiga.DataSource = pagosTarjetaServices.lsPagTarjDetalUsFinalGridViewModel;
                rptMovimientosLiga.DataBind();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalPagoDetalle()", true);
            }

            if (e.CommandName == "btnPagar")
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvLigasGeneradas.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKey = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());

                string UidLigaAsociado = ligasUrlsServices.ObtenerPromocionesUsuarioFinal(dataKey);

                if (!string.IsNullOrEmpty(UidLigaAsociado))
                {
                    lblTitlePagar.Text = "Seleccionar forma de pago";
                    btnCerrar.Visible = true;

                    promocionesServices.CargarPromoPagoLigaUsuarioFinal(Guid.Parse(UidLigaAsociado));
                    ddlFormasPago.DataSource = promocionesServices.lsSelectPagoLigaModel;
                    ddlFormasPago.DataTextField = "VchDescripcion";
                    ddlFormasPago.DataValueField = "UidPromocion";
                    ddlFormasPago.DataBind();

                    ddlFormasPago_SelectedIndexChanged(null, null);

                    pnlPromociones.Visible = true;
                    pnlIframe.Visible = false;

                    btnFinalizar.Visible = false;
                    btnGenerarLiga.Visible = true;
                }
                else
                {
                    lblTitlePagar.Text = "Pagar";
                    btnCerrar.Visible = false;

                    string UrlLiga = ligasUrlsServices.ObtenerUrlLigaUsuarioFinal(dataKey);

                    string[] Datos = Regex.Split(UrlLiga, ",");

                    ifrLiga.Src = Datos[0].ToString();
                    ViewState["IdReferencia"] = Datos[1].ToString();

                    pnlPromociones.Visible = false;
                    pnlIframe.Visible = true;

                    btnFinalizar.Visible = true;
                    btnGenerarLiga.Visible = false;
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalPagar()", true);
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
        protected void gvLigasGeneradas_Sorting(object sender, GridViewSortEventArgs e)
        {
            string SortExpression = e.SortExpression;
            ViewState["SoExgvLigasGeneradas"] = e.SortExpression;
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
                    case "Comisiones":
                        if (Orden == "ASC")
                        {
                            ligasUrlsServices.lsLigasUrlsGridViewModel = ligasUrlsServices.lsLigasUrlsGridViewModel.OrderBy(x => x.Comisiones).ToList();
                        }
                        else
                        {
                            ligasUrlsServices.lsLigasUrlsGridViewModel = ligasUrlsServices.lsLigasUrlsGridViewModel.OrderByDescending(x => x.Comisiones).ToList();
                        }
                        break;
                    case "DcmImportePromocion":
                        if (Orden == "ASC")
                        {
                            ligasUrlsServices.lsLigasUrlsGridViewModel = ligasUrlsServices.lsLigasUrlsGridViewModel.OrderBy(x => x.DcmImportePromocion).ToList();
                        }
                        else
                        {
                            ligasUrlsServices.lsLigasUrlsGridViewModel = ligasUrlsServices.lsLigasUrlsGridViewModel.OrderByDescending(x => x.DcmImportePromocion).ToList();
                        }
                        break;
                    case "VchPromocion":
                        if (Orden == "ASC")
                        {
                            ligasUrlsServices.lsLigasUrlsGridViewModel = ligasUrlsServices.lsLigasUrlsGridViewModel.OrderBy(x => x.VchPromocion).ToList();
                        }
                        else
                        {
                            ligasUrlsServices.lsLigasUrlsGridViewModel = ligasUrlsServices.lsLigasUrlsGridViewModel.OrderByDescending(x => x.VchPromocion).ToList();
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
        protected void gvLigasGeneradas_RowCreated(object sender, GridViewRowEventArgs e)
        {
            SortDirection direccion = (SortDirection)ViewState["gvLigasGeneradas"];
            string SortExpression = ViewState["SoExgvLigasGeneradas"].ToString();

            if (e.Row.RowType == DataControlRowType.Header)
            {
                foreach (TableCell tc in e.Row.Cells)
                {
                    if (tc.HasControls())
                    {
                        // Buscar el enlace de la cabecera
                        LinkButton lnk = tc.Controls[0] as LinkButton;
                        if (lnk != null && SortExpression == lnk.CommandArgument)
                        {
                            // Verificar que se está ordenando por el campo indicado en el comando de ordenación
                            // Crear una imagen
                            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                            img.Height = 20;
                            img.Width = 20;
                            // Ajustar dinámicamente el icono adecuado
                            img.ImageUrl = "~/Images/SortingGv/" + (direccion == SortDirection.Ascending ? "desc" : "asc") + ".png";
                            img.ImageAlign = ImageAlign.AbsMiddle;
                            // Le metemos un espacio delante de la imagen para que no se pegue al enlace
                            tc.Controls.Add(new LiteralControl(""));
                            tc.Controls.Add(img);
                        }
                    }
                }
            }
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

            ligasUrlsServices.BuscarLigasUsuarioFinal(Guid.Parse(ViewState["UidUsuarioLocal"].ToString()), txtIdentificador.Text, txtAsunto.Text, txtConcepto.Text, ImporteMayor, ImporteMenor, txtRegistroDesde.Text, txtRegistroHasta.Text, txtVencimientoDesde.Text, txtVencimientoHasta.Text, ddlEstatus.SelectedValue);
            gvLigasGeneradas.DataSource = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel;
            gvLigasGeneradas.DataBind();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModalBusqueda()", true);
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

        protected void ddlFormasPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Guid.Parse(ddlFormasPago.SelectedValue) != Guid.Empty)
            {
                foreach (var item in promocionesServices.lsSelectPagoLigaModel.Where(x => x.UidPromocion == Guid.Parse(ddlFormasPago.SelectedValue)))
                {
                    lblConcepto.Text = item.VchConcepto;
                    lblVencimiento.Text = item.DtVencimiento.ToString("dd/MM/yyyy");
                    ViewState["TotalApagar"] = item.DcmImporte.ToString("N2");
                    ViewState["UrlLigaSelect"] = item.VchUrl;
                    lblTotal.Text = item.DcmImporte.ToString("N2");
                    lblTotalPago.Text = "Pagar $" + item.DcmImporte.ToString("N2");
                    ViewState["IdReferencia"] = item.IdReferencia;
                }
            }
            else
            {
                foreach (var item in promocionesServices.lsSelectPagoLigaModel.Where(x => x.UidPromocion == Guid.Parse(ddlFormasPago.SelectedValue)))
                {
                    lblConcepto.Text = item.VchConcepto;
                    lblVencimiento.Text = item.DtVencimiento.ToString("dd/MM/yyyy");
                    ViewState["TotalApagar"] = item.DcmImporte.ToString("N2");
                    ViewState["UrlLigaSelect"] = item.VchUrl;
                    lblTotal.Text = item.DcmImporte.ToString("N2");
                    lblTotalPago.Text = "Pagar $" + item.DcmImporte.ToString("N2");
                    ViewState["IdReferencia"] = item.IdReferencia;
                }
            }
        }

        protected void btnGenerarLiga_Click(object sender, EventArgs e)
        {
            ifrLiga.Src = ViewState["UrlLigaSelect"].ToString();
            pnlPromociones.Visible = false;
            pnlIframe.Visible = true;

            btnFinalizar.Visible = true;
            btnCerrar.Visible = false;
        }

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            pnlValidar.Visible = true;
            tmValidar.Enabled = true;
            pnlIframe.Visible = false;
            ViewState["tmValidar"] = DateTime.Now.AddSeconds(5).ToString();
        }

        protected void tmValidar_Tick(object sender, EventArgs e)
        {
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

                    ligasUrlsServices.ConsultarLigaUsuarioFinal(Guid.Parse(ViewState["UidUsuarioLocal"].ToString()));
                    gvLigasGeneradas.DataSource = ligasUrlsServices.lsLigasUsuariosFinalGridViewModel;
                    gvLigasGeneradas.DataBind();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModalPagar()", true);
                }
            }
            else
            {
                pnlValidar.Visible = false;
                tmValidar.Enabled = false;

                pnlPromociones.Visible = true;
                pnlIframe.Visible = false;

                btnFinalizar.Visible = false;
                btnGenerarLiga.Visible = true;

                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "<b>Lo sentimos,</b> no se ha podido procesar su pago.";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModalPagar()", true);
            }
        }
    }
}