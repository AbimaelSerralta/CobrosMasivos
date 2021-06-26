using Franquicia.Bussiness;
using Franquicia.WebForms.Util;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.Vista;

namespace Franquicia.WebForms.Views
{
    public partial class EventosUsuario : System.Web.UI.Page
    {
        EventosServices eventosServices = new EventosServices();
        ImporteLigaMinMaxServices importeLigaMinMaxServices = new ImporteLigaMinMaxServices();
        ParametrosEntradaServices parametrosEntradaServices = new ParametrosEntradaServices();
        UsuariosCompletosServices usuariosCompletosServices = new UsuariosCompletosServices();
        PromocionesServices promocionesServices = new PromocionesServices();
        EstatusServices estatusServices = new EstatusServices();
        ComisionesTarjetasClientesServices comisionesTarjetasCl = new ComisionesTarjetasClientesServices();

        string id_company = "";
        string id_branch = "";
        string user = "";
        string pwd = "";
        string moneda = "";
        string canal = "";
        string semillaAES = "";
        string urlGen = "";
        string data0 = "";
        decimal ImporteMin = 0;
        decimal ImporteMax = 0;

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

            txtImporte.Attributes.Add("onchange", "button_click(this,'" + btnCalcular.ClientID + "')");

            if (!IsPostBack)
            {
                ViewState["gvEventos"] = SortDirection.Ascending;
                ViewState["SoExgvEventos"] = "";

                Session["EventosUsuarioseventosServices"] = eventosServices;
                Session["EventosUsuariosusuariosCompletosServices"] = usuariosCompletosServices;
                Session["importeLigaMinMaxServices"] = importeLigaMinMaxServices;
                Session["EventosUsuariosparametrosEntradaServices"] = parametrosEntradaServices;
                Session["EventosUsuariospromocionesServices"] = promocionesServices;
                Session["EventosUsuariosestatusServices"] = estatusServices;
                Session["comisionesTarjetasCl"] = comisionesTarjetasCl;

                eventosServices.CargarEventosUsuariosFinal(Guid.Parse(ViewState["UidUsuarioLocal"].ToString()));
                gvEventos.DataSource = eventosServices.lsEventosUsuarioFinalGridViewModel;
                gvEventos.DataBind();

                estatusServices.CargarEstatus();
                ddlFiltroEstatus.DataSource = estatusServices.lsEstatus;
                ddlFiltroEstatus.Items.Insert(0, new ListItem("SELECCIONE", Guid.Empty.ToString()));
                ddlFiltroEstatus.DataTextField = "VchDescripcion";
                ddlFiltroEstatus.DataValueField = "UidEstatus";
                ddlFiltroEstatus.DataBind();

            }
            else
            {
                eventosServices = (EventosServices)Session["EventosUsuarioseventosServices"];
                usuariosCompletosServices = (UsuariosCompletosServices)Session["EventosUsuariosusuariosCompletosServices"];
                importeLigaMinMaxServices = (ImporteLigaMinMaxServices)Session["importeLigaMinMaxServices"];
                parametrosEntradaServices = (ParametrosEntradaServices)Session["EventosUsuariosparametrosEntradaServices"];
                promocionesServices = (PromocionesServices)Session["EventosUsuariospromocionesServices"];
                estatusServices = (EstatusServices)Session["EventosUsuariosestatusServices"];
                comisionesTarjetasCl = (ComisionesTarjetasClientesServices)Session["comisionesTarjetasCl"];

                pnlAlert.Visible = false;
                lblMensajeAlert.Text = "";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade");

                AsignarParametrosEntradaCliente();
            }
        }

        private void AsignarParametrosEntradaCliente()
        {
            id_company = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.IdCompany;
            id_branch = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.IdBranch;
            user = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchUsuario;
            pwd = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchPassword;
            moneda = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchModena;
            canal = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchCanal;
            semillaAES = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchSemillaAES;
            urlGen = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchUrl;
            data0 = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchData0;

            if (parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.BitImporteLiga)
            {
                //Asigna los importes min y max del cliente
                ImporteMin = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.DcmImporteMin;
                ImporteMax = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.DcmImporteMax;
            }
            else
            {
                //Asigna los importes min y max del sistema
                foreach (var item in importeLigaMinMaxServices.lsImporteLigaMinMax)
                {
                    ImporteMin = item.DcmImporteMin;
                    ImporteMax = item.DcmImporteMax;
                }
            }
        }

        protected void gvEventos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AbrirEvento")
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvEventos.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKeys = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["UidEvento"] = dataKeys;


                if (ViewState["UidEvento"] != null)
                {
                    eventosServices.ObtenerDatosEventoUsuariosFinal(Guid.Parse(ViewState["UidEvento"].ToString()));

                    if (eventosServices.eventosRepository.eventosGridViewModel.UidPropietario != null && eventosServices.eventosRepository.eventosGridViewModel.UidPropietario != Guid.Empty)
                    {
                        ViewState["UidClienteLocal"] = eventosServices.eventosRepository.eventosGridViewModel.UidPropietario;
                    }
                    else
                    {
                        ViewState["UidClienteLocal"] = Guid.Empty;
                    }

                    parametrosEntradaServices.ObtenerParametrosEntradaClienteCM(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                    importeLigaMinMaxServices.CargarImporteLigaMinMax();
                    AsignarParametrosEntradaCliente();

                    if (eventosServices.eventosRepository.eventosGridViewModel.UidEstatus == Guid.Parse("65E46BC9-1864-4145-AD1A-70F5B5F69739"))
                    {
                        //DateTime hoy = DateTime.Now;

                        DateTime HoraDelServidor = DateTime.Now;
                        DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

                        if (eventosServices.eventosRepository.eventosGridViewModel.UidTipoEvento == Guid.Parse("883830E3-DE92-46E7-86C9-B0218FA42C58"))
                        {
                            DateTime fhInicio = eventosServices.eventosRepository.eventosGridViewModel.DtFHInicio;
                            DateTime fhFin = eventosServices.eventosRepository.eventosGridViewModel.DtFHFin;

                            if (eventosServices.eventosRepository.eventosGridViewModel.BitFHFin)
                            {
                                DateTime date1 = hoy;
                                DateTime date2 = fhInicio;
                                int result = DateTime.Compare(date1, date2);

                                DateTime date3 = hoy;
                                DateTime date4 = fhFin;
                                int result2 = DateTime.Compare(date3, date4);

                                if (result >= 0 && result2 <= 0)
                                {
                                    CargarConfiguracion();
                                }
                                else
                                {

                                    if (result <= 0 && result2 <= 0)
                                    {
                                        pnlValidar.Visible = false;
                                        tmValidar.Enabled = false;

                                        pnlDatosComercio.Visible = false;
                                        pnlCorreo.Visible = false;
                                        pnlIframe.Visible = false;

                                        btnAceptar.Visible = false;
                                        btnGenerarLigas.Visible = false;

                                        pnlValidarEvento.Visible = true;

                                        lblTitleDialog.Text = "El evento Inicia el " + fhInicio.ToString("dd/MM/yyyy HH:mm:ss") + " (Ciudad de México, CDMX)";
                                    }
                                    else if (result == 1 && result2 == 1)
                                    {
                                        pnlValidar.Visible = false;
                                        tmValidar.Enabled = false;

                                        pnlDatosComercio.Visible = false;
                                        pnlCorreo.Visible = false;
                                        pnlIframe.Visible = false;

                                        btnAceptar.Visible = false;
                                        btnGenerarLigas.Visible = false;

                                        pnlValidarEvento.Visible = true;

                                        lblTitleDialog.Text = "El evento ha terminado.";
                                    }
                                }
                            }
                            else
                            {
                                DateTime date1 = hoy;
                                DateTime date2 = fhInicio;
                                int result = DateTime.Compare(date1, date2);

                                if (result >= 0)
                                {
                                    CargarConfiguracion();
                                }
                                else
                                {
                                    pnlValidar.Visible = false;
                                    tmValidar.Enabled = false;

                                    pnlDatosComercio.Visible = false;
                                    pnlCorreo.Visible = false;
                                    pnlIframe.Visible = false;

                                    btnAceptar.Visible = false;
                                    btnGenerarLigas.Visible = false;

                                    pnlValidarEvento.Visible = true;

                                    lblTitleDialog.Text = "El evento Inicia el " + fhInicio.ToString("dd/MM/yyyy HH:mm:ss") + " (Ciudad de México, CDMX)";
                                }
                            }
                        }
                    }
                    else
                    {
                        pnlValidar.Visible = false;
                        tmValidar.Enabled = false;

                        pnlDatosComercio.Visible = false;
                        pnlCorreo.Visible = false;
                        pnlIframe.Visible = false;

                        btnAceptar.Visible = false;
                        btnGenerarLigas.Visible = false;

                        pnlValidarEvento.Visible = true;

                        lblTitleDialog.Text = "El evento ha terminado.";
                    }
                }
                else
                {
                    btnGenerarLigas.Visible = false;
                }


                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);
            }
        }
        protected void gvEventos_Sorting(object sender, GridViewSortEventArgs e)
        {
            string SortExpression = e.SortExpression;
            ViewState["SoExgvEventos"] = e.SortExpression;
            SortDirection direccion;
            string Orden = string.Empty;

            if (ViewState["gvEventos"] != null)
            {
                direccion = (SortDirection)ViewState["gvEventos"];
                if (direccion == SortDirection.Ascending)
                {
                    ViewState["gvEventos"] = SortDirection.Descending;
                    Orden = "ASC";
                }
                else
                {
                    ViewState["gvEventos"] = SortDirection.Ascending;
                    Orden = "DESC";
                }

                switch (SortExpression)
                {
                    case "VchNombreEvento":
                        if (Orden == "ASC")
                        {
                            eventosServices.lsEventosUsuarioFinalGridViewModel = eventosServices.lsEventosUsuarioFinalGridViewModel.OrderBy(x => x.VchNombreEvento).ToList();
                        }
                        else
                        {
                            eventosServices.lsEventosUsuarioFinalGridViewModel = eventosServices.lsEventosUsuarioFinalGridViewModel.OrderByDescending(x => x.VchNombreEvento).ToList();
                        }
                        break;
                    case "DtFHInicio":
                        if (Orden == "ASC")
                        {
                            eventosServices.lsEventosUsuarioFinalGridViewModel = eventosServices.lsEventosUsuarioFinalGridViewModel.OrderBy(x => x.DtFHInicio).ToList();
                        }
                        else
                        {
                            eventosServices.lsEventosUsuarioFinalGridViewModel = eventosServices.lsEventosUsuarioFinalGridViewModel.OrderByDescending(x => x.DtFHInicio).ToList();
                        }
                        break;
                    case "VchFHFin":
                        if (Orden == "ASC")
                        {
                            eventosServices.lsEventosUsuarioFinalGridViewModel = eventosServices.lsEventosUsuarioFinalGridViewModel.OrderBy(x => x.VchFHFin).ToList();
                        }
                        else
                        {
                            eventosServices.lsEventosUsuarioFinalGridViewModel = eventosServices.lsEventosUsuarioFinalGridViewModel.OrderByDescending(x => x.VchFHFin).ToList();
                        }
                        break;
                    case "UidEstatus":
                        if (Orden == "ASC")
                        {
                            eventosServices.lsEventosUsuarioFinalGridViewModel = eventosServices.lsEventosUsuarioFinalGridViewModel.OrderBy(x => x.UidEstatus).ToList();
                        }
                        else
                        {
                            eventosServices.lsEventosUsuarioFinalGridViewModel = eventosServices.lsEventosUsuarioFinalGridViewModel.OrderByDescending(x => x.UidEstatus).ToList();
                        }
                        break;
                }

                gvEventos.DataSource = eventosServices.lsEventosUsuarioFinalGridViewModel;
                gvEventos.DataBind();
            }
        }
        protected void gvEventos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvEventos.PageIndex = e.NewPageIndex;
            gvEventos.DataSource = eventosServices.lsEventosUsuarioFinalGridViewModel;
            gvEventos.DataBind();
        }
        protected void gvEventos_RowCreated(object sender, GridViewRowEventArgs e)
        {
            SortDirection direccion = (SortDirection)ViewState["gvEventos"];
            string SortExpression = ViewState["SoExgvEventos"].ToString();

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

        #region Evento
        private void CargarConfiguracion()
        {
            //Datos para acordeon
            btnAcordion1.CssClass = "accordion";
            pnlAcordion1.Attributes.Add("style", "display: none;");
            btnAcordion1.Text = "BENEFINICIARIO: " + eventosServices.eventosRepository.eventosGridViewModel.VchNombreComercial + "<i class=\u0022" + "pull-right material-icons" + "\u0022>expand_more</i>";

            btnAcordion2.CssClass = "accordion";
            pnlAcordion2.Attributes.Add("style", "display: none;");
            btnAcordion2.Text = "EVENTO: " + eventosServices.eventosRepository.eventosGridViewModel.VchNombreEvento + "<i class=\u0022" + "pull-right material-icons" + "\u0022>expand_more</i>";

            btnAcordion3.CssClass = "accordion activeA";
            pnlAcordion3.Attributes.Add("style", "display: block;");
            btnAcordion3.Text = "DATOS DEL PAGO" + "<i class=\u0022" + "pull-right material-icons" + "\u0022>expand_less</i>";


            lblTitle.Text = eventosServices.eventosRepository.eventosGridViewModel.VchConcepto;
            lblNombreEvento.Text = eventosServices.eventosRepository.eventosGridViewModel.VchNombreEvento;
            txtDescripcion.Text = eventosServices.eventosRepository.eventosGridViewModel.VchDescripcion;
            txtNombreComercial.Text = eventosServices.eventosRepository.eventosGridViewModel.VchNombreComercial;
            txtComeCelular.Text = eventosServices.eventosRepository.eventosGridViewModel.VchTelefono;
            txtComeCorreo.Text = eventosServices.eventosRepository.eventosGridViewModel.VchCorreo;

            if (eventosServices.eventosRepository.eventosGridViewModel.BitTipoImporte)
            {
                txtImporte.Enabled = true;
            }
            else
            {
                txtImporte.Enabled = false;

            }
            txtImporte.Text = eventosServices.eventosRepository.eventosGridViewModel.DcmImporte.ToString("N2");

            if (!eventosServices.eventosRepository.eventosGridViewModel.BitDatosUsuario)
            {
                ViewState["AccionEvento"] = "RegistroCorreo";
            }

            btnCalcular_Click(null, null);
        }

        private void LimpiarCampos()
        {
            txtImporte.Text = string.Empty;
            ddlFormasPago.SelectedIndex = 0;
            txtImporteTotal.Text = string.Empty;
        }
        protected void btnGenerarPago_Click(object sender, EventArgs e)
        {
            if (txtImporte.EmptyTextBox())
            {
                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "El campo <b>Importe</b> es obligatorio.";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                return;
            }
            if (txtImporteTotal.EmptyTextBox())
            {
                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "El campo <b>Total a pagar</b> es obligatorio.";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                return;
            }

            if (decimal.Parse(txtImporte.Text) >= ImporteMin && decimal.Parse(txtImporte.Text) <= ImporteMax)
            {

            }
            else
            {
                txtImporte.BackColor = System.Drawing.Color.FromName("#f2dede");
                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "El importe mínimo es de <b>$" + ImporteMin.ToString("N2") + "</b> y el máximo es de <b>$" + ImporteMax.ToString("N2") + "</b>";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                return;
            }

            pnlDatosComercio.Visible = false;
            pnlCorreo.Visible = false;
            pnlIframe.Visible = false;

            btnGenerarLigas.Visible = false;

            string identificador = eventosServices.eventosRepository.eventosGridViewModel.VchNombreEvento;
            string concepto = eventosServices.eventosRepository.eventosGridViewModel.VchConcepto;

            DateTime HoraDelServidor = DateTime.Now;
            DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

            string vencimiento = hoy.ToString("dd/MM/yyyy");

            if (eventosServices.eventosRepository.eventosGridViewModel.BitFHFin)
            {
                vencimiento = eventosServices.eventosRepository.eventosGridViewModel.DtFHFin.ToString("dd/MM/yyyy");
            }

            int intCorreo = 1;
            decimal subtotal = decimal.Parse(txtImporte.Text);
            decimal comisionTC = decimal.Parse(ViewState["ComisionTC"].ToString());
            decimal comisionP = decimal.Parse(ViewState["ComisionP"].ToString());
            decimal importeTotal = decimal.Parse(txtImporteTotal.Text);

            string url = string.Empty;
            bool resu = false;

            if (!string.IsNullOrEmpty(id_company) && !string.IsNullOrEmpty(id_branch) && !string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(pwd) && !string.IsNullOrEmpty(moneda) && !string.IsNullOrEmpty(canal) && !string.IsNullOrEmpty(semillaAES) && !string.IsNullOrEmpty(urlGen) && !string.IsNullOrEmpty(data0))
            {
                Guid UidLigaAsociado = Guid.NewGuid();

                usuariosCompletosServices.SelectUsClienteEventoUsuarioFinal(Guid.Parse(ViewState["UidUsuarioLocal"].ToString()));
                promocionesServices.CargarPromocionesEventoUsuarioFinal(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse(ViewState["UidEvento"].ToString()));

                foreach (var item in usuariosCompletosServices.lsEventoLiga)
                {
                    if (ddlFormasPago.SelectedValue != "contado")
                    {
                        DateTime thisDay = DateTime.Now;
                        string ReferenciaCobro = item.IdCliente.ToString() + item.IdUsuario.ToString() + thisDay.ToString("ddMMyyyyHHmmssfff");

                        foreach (var itPromo in promocionesServices.lsEventosGenerarLigasModel.Where(x => x.UidPromocion == Guid.Parse(ddlFormasPago.SelectedValue)).ToList())
                        {
                            //decimal Valor = itPromo.DcmComicion * importeTotal / 100;
                            //decimal Importe = Valor + importeTotal;

                            string promocion = itPromo.VchDescripcion.Replace(" MESES", "");

                            DateTime thisDay2 = DateTime.Now;
                            string Referencia = item.IdCliente.ToString() + item.IdUsuario.ToString() + thisDay2.ToString("ddMMyyyyHHmmssfff");

                            url = GenLigaPara(id_company, id_branch, user, pwd, Referencia, importeTotal, moneda, canal, promocion, intCorreo, vencimiento, item.StrCorreo, concepto, semillaAES, data0, urlGen);

                            if (url.Contains("https://"))
                            {
                                if (usuariosCompletosServices.GenerarLigasPagosEvento(url, concepto, subtotal, Referencia, item.UidUsuario, identificador, thisDay, DateTime.Parse(vencimiento), "EVENTO", UidLigaAsociado, itPromo.UidPromocion, Guid.Parse(ViewState["UidEvento"].ToString()), Guid.Parse(ViewState["UidClienteLocal"].ToString()), comisionTC, comisionP, importeTotal))
                                {
                                    ViewState["IdReferenciaCobroEvento"] = Referencia;
                                    pnlAcordiones.Visible = false;
                                    pnlDatosComercio.Visible = false;
                                    pnlCorreo.Visible = false;
                                    pnlIframe.Visible = true;

                                    btnCerrar.Visible = false;
                                    btnAceptar.Visible = true;

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
                    }
                    else if (ddlFormasPago.SelectedValue == "contado")
                    {
                        DateTime thisDay = DateTime.Now;
                        string ReferenciaCobro = item.IdCliente.ToString() + item.IdUsuario.ToString() + thisDay.ToString("ddMMyyyyHHmmssfff");

                        string urlCobro = GenLigaPara(id_company, id_branch, user, pwd, ReferenciaCobro, importeTotal, moneda, canal, "C", intCorreo, vencimiento, item.StrCorreo, concepto, semillaAES, data0, urlGen);

                        if (urlCobro.Contains("https://"))
                        {
                            if (usuariosCompletosServices.GenerarLigasPagosEvento(urlCobro, concepto, subtotal, ReferenciaCobro, item.UidUsuario, identificador, thisDay, DateTime.Parse(vencimiento), "EVENTO", Guid.Empty, Guid.Empty, Guid.Parse(ViewState["UidEvento"].ToString()), Guid.Parse(ViewState["UidClienteLocal"].ToString()), comisionTC, comisionP, importeTotal))
                            {
                                ViewState["IdReferenciaCobroEvento"] = ReferenciaCobro;
                                pnlAcordiones.Visible = false;
                                pnlDatosComercio.Visible = false;
                                pnlCorreo.Visible = false;
                                pnlIframe.Visible = true;

                                btnCerrar.Visible = false;
                                btnAceptar.Visible = true;

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

                }
                else
                {
                    if (!string.IsNullOrEmpty(url))
                    {
                        pnlAlert.Visible = true;
                        lblMensajeAlert.Text = "<b>¡Lo sentimos! </b> " + url + "." + "<br /> Las credenciales proporcionadas no son correctos, por favor contacte a los administradores.";
                        divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                    }
                    else
                    {
                        pnlAlert.Visible = true;
                        lblMensajeAlert.Text = "<b>¡Lo sentimos! </b> Las credenciales proporcionadas no son correctos, por favor contacte a los administradores.";
                        divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                    }
                }
            }
            else
            {
                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "<b>¡Lo sentimos! </b> Esta empresa no cuenta con credenciales para generar ligas, por favor contacte a los administradores.";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
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

        protected void btnCalcular_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtImporte.Text))
            {
                ddlFormasPago.Items.Clear();
                promocionesServices.CargarPromocionesEventoImporte(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse(ViewState["UidEvento"].ToString()), txtImporte.Text);
                ddlFormasPago.Items.Insert(0, new ListItem("Al contado", "contado"));
                ddlFormasPago.DataSource = promocionesServices.lsEventosGenerarLigasModel;
                ddlFormasPago.DataTextField = "VchDescripcion";
                ddlFormasPago.DataValueField = "UidPromocion";
                ddlFormasPago.DataBind();
            }

            ddlFormasPago_SelectedIndexChanged(null, null);
        }
        protected void ddlFormasPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            decimal CTC = 0;
            ViewState["ComisionTC"] = 0;
            ViewState["ComisionP"] = 0;

            if (!string.IsNullOrEmpty(txtImporte.Text) && decimal.Parse(txtImporte.Text) != 0)
            {
                //Calcula la comicion
                comisionesTarjetasCl.CargarComisionesTarjetaCM(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                if (comisionesTarjetasCl.lsComisionesTarjetasClientes.Count >= 1)
                {
                    foreach (var itComi in comisionesTarjetasCl.lsComisionesTarjetasClientes)
                    {
                        if (itComi.BitComision)
                        {
                            CTC = itComi.DcmComision * decimal.Parse(txtImporte.Text) / (100 - itComi.DcmComision);
                            ViewState["ComisionTC"] = CTC;
                        }
                    }
                }

                if (ddlFormasPago.SelectedValue == "contado")
                {
                    decimal importe = decimal.Parse(txtImporte.Text);

                    txtImporte.Text = importe.ToString("N2");
                    txtImporteTotal.Text = (importe + CTC).ToString("N2");
                    ViewState["txtImporteTotal.Text"] = (importe + CTC).ToString("N2");
                }
                else
                {
                    decimal importeTotal = decimal.Parse(txtImporte.Text) + CTC;

                    //promocionesServices.CargarPromocionesEvento(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse(ViewState["UidEvento"].ToString()));

                    foreach (var itPromo in promocionesServices.lsEventosGenerarLigasModel.Where(x => x.UidPromocion == Guid.Parse(ddlFormasPago.SelectedValue)).ToList())
                    {
                        decimal Valor = itPromo.DcmComicion * importeTotal / (100 - itPromo.DcmComicion);
                        decimal Importe = Valor + importeTotal;
                        ViewState["ComisionP"] = Valor;

                        txtImporteTotal.Text = Importe.ToString("N2");
                        ViewState["txtImporteTotal.Text"] = Importe.ToString("N2");
                    }
                }

                lblTotalPago.Text = "Generar pago $" + ViewState["txtImporteTotal.Text"].ToString();
                btnGenerarLigas.Enabled = true;
            }
            else
            {
                txtImporte.Text = string.Empty;
                lblTotalPago.Text = "Generar pago $0.00";
                btnGenerarLigas.Enabled = false;
                txtImporteTotal.Text = "0.00";
            }
        }

        protected void tmValidar_Tick(object sender, EventArgs e)
        {
            if (DateTime.Compare(DateTime.Now, DateTime.Parse(ViewState["tmValidar"].ToString())) <= 0)
            {
                ltMnsj.Text = "Verificando...: " + (((Int32)DateTime.Parse(ViewState["tmValidar"].ToString()).Subtract(DateTime.Now).TotalSeconds) % 60).ToString();

                if (eventosServices.ValidarPagoEvento(ViewState["IdReferenciaCobroEvento"].ToString()))
                {
                    pnlValidar.Visible = false;
                    tmValidar.Enabled = false;

                    pnlAcordiones.Visible = true;
                    pnlDatosComercio.Visible = true;
                    pnlCorreo.Visible = true;
                    pnlIframe.Visible = false;

                    btnCerrar.Visible = true;
                    btnAceptar.Visible = false;
                    btnGenerarLigas.Visible = true;

                    //LimpiarCampos();
                    CargarConfiguracion();

                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "<b>Felicidades,</b> su pago se proceso exitosamente.";
                    divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                }
            }
            else
            {
                pnlValidar.Visible = false;
                tmValidar.Enabled = false;

                eventosServices.EliminarLigaEvento(ViewState["IdReferenciaCobroEvento"].ToString());

                pnlAcordiones.Visible = true;
                pnlDatosComercio.Visible = true;
                pnlCorreo.Visible = true;
                pnlIframe.Visible = false;

                btnCerrar.Visible = true;
                btnAceptar.Visible = false;
                btnGenerarLigas.Visible = true;

                //LimpiarCampos();
                CargarConfiguracion();

                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "<b>Lo sentimos,</b> no se ha podido procesar su pago.";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            pnlValidar.Visible = true;
            tmValidar.Enabled = true;
            pnlIframe.Visible = false;

            ViewState["tmValidar"] = DateTime.Now.AddSeconds(5).ToString();
        }
        #endregion

        protected void btnAcordion1_Click(object sender, EventArgs e)
        {
            if (btnAcordion1.CssClass == "accordion activeA")
            {
                btnAcordion1.CssClass = "accordion";
                pnlAcordion1.Attributes.Add("style", "display: none;");

                btnAcordion1.Text = "BENEFINICIARIO: " + eventosServices.eventosRepository.eventosGridViewModel.VchNombreComercial + "<i class=\u0022" + "pull-right material-icons" + "\u0022>expand_more</i>";
            }
            else
            {
                btnAcordion1.CssClass = "accordion activeA";
                pnlAcordion1.Attributes.Add("style", "display: block;");
                btnAcordion1.Text = "BENEFINICIARIO: " + eventosServices.eventosRepository.eventosGridViewModel.VchNombreComercial + "<i class=\u0022" + "pull-right material-icons" + "\u0022>expand_less</i>";

                btnAcordion2.CssClass = "accordion";
                pnlAcordion2.Attributes.Add("style", "display: none;");
                btnAcordion2.Text = "EVENTO: " + eventosServices.eventosRepository.eventosGridViewModel.VchNombreEvento + "<i class=\u0022" + "pull-right material-icons" + "\u0022>expand_more</i>";

                btnAcordion3.CssClass = "accordion";
                pnlAcordion3.Attributes.Add("style", "display: none;");
                btnAcordion3.Text = "DATOS DEL PAGO" + "<i class=\u0022" + "pull-right material-icons" + "\u0022>expand_more</i>";
            }
        }

        protected void btnAcordion2_Click(object sender, EventArgs e)
        {
            if (btnAcordion2.CssClass == "accordion activeA")
            {
                btnAcordion2.CssClass = "accordion";
                pnlAcordion2.Attributes.Add("style", "display: none;");
                btnAcordion2.Text = "EVENTO: " + eventosServices.eventosRepository.eventosGridViewModel.VchNombreEvento + "<i class=\u0022" + "pull-right material-icons" + "\u0022>expand_more</i>";
            }
            else
            {
                btnAcordion1.CssClass = "accordion";
                pnlAcordion1.Attributes.Add("style", "display: none;");
                btnAcordion1.Text = "BENEFINICIARIO: " + eventosServices.eventosRepository.eventosGridViewModel.VchNombreComercial + "<i class=\u0022" + "pull-right material-icons" + "\u0022>expand_more</i>";

                btnAcordion2.CssClass = "accordion activeA";
                pnlAcordion2.Attributes.Add("style", "display: block;");
                btnAcordion2.Text = "EVENTO: " + eventosServices.eventosRepository.eventosGridViewModel.VchNombreEvento + "<i class=\u0022" + "pull-right material-icons" + "\u0022>expand_less</i>";

                btnAcordion3.CssClass = "accordion";
                pnlAcordion3.Attributes.Add("style", "display: none;");
                btnAcordion3.Text = "DATOS DEL PAGO" + "<i class=\u0022" + "pull-right material-icons" + "\u0022>expand_more</i>";
            }
        }

        protected void btnAcordion3_Click(object sender, EventArgs e)
        {
            if (btnAcordion3.CssClass == "accordion activeA")
            {
                btnAcordion3.CssClass = "accordion";
                pnlAcordion3.Attributes.Add("style", "display: none;");
                btnAcordion3.Text = "DATOS DEL PAGO" + "<i class=\u0022" + "pull-right material-icons" + "\u0022>expand_more</i>";
            }
            else
            {
                btnAcordion1.CssClass = "accordion";
                pnlAcordion1.Attributes.Add("style", "display: none;");
                btnAcordion1.Text = "BENEFINICIARIO: " + eventosServices.eventosRepository.eventosGridViewModel.VchNombreComercial + "<i class=\u0022" + "pull-right material-icons" + "\u0022>expand_more</i>";

                btnAcordion2.CssClass = "accordion";
                pnlAcordion2.Attributes.Add("style", "display: none;");
                btnAcordion2.Text = "EVENTO: " + eventosServices.eventosRepository.eventosGridViewModel.VchNombreEvento + "<i class=\u0022" + "pull-right material-icons" + "\u0022>expand_more</i>";

                btnAcordion3.CssClass = "accordion activeA";
                pnlAcordion3.Attributes.Add("style", "display: block;");
                btnAcordion3.Text = "DATOS DEL PAGO" + "<i class=\u0022" + "pull-right material-icons" + "\u0022>expand_less</i>";
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

            if (txtFiltroDcmImporteMayor.Text != string.Empty)
            {
                switch (ddlImporteMayor.SelectedValue)
                {
                    case ">":
                        ImporteMayor = Convert.ToDecimal(txtFiltroDcmImporteMayor.Text) + 1;
                        break;
                    case ">=":
                        ImporteMayor = Convert.ToDecimal(txtFiltroDcmImporteMayor.Text);
                        break;
                }
            }
            if (txtFiltroDcmImporteMenor.Text != string.Empty)
            {
                switch (ddlImporteMenor.SelectedValue)
                {
                    case "<":
                        ImporteMenor = Convert.ToDecimal(txtFiltroDcmImporteMenor.Text) - 1;
                        break;
                    case "<=":
                        ImporteMenor = Convert.ToDecimal(txtFiltroDcmImporteMenor.Text);
                        break;
                }
            }

            eventosServices.BuscarEventosUsuarioFinal(new Guid(ViewState["UidUsuarioLocal"].ToString()), txtFiltroNombre.Text, txtFiltroDtInicioDesde.Text, txtFiltroDtInicioHasta.Text, txtFiltroDtFinDesde.Text, txtFiltroDtFinHasta.Text, ImporteMayor, ImporteMenor, Guid.Parse(ddlFiltroEstatus.SelectedValue));
            gvEventos.DataSource = eventosServices.lsEventosUsuarioFinalGridViewModel;
            gvEventos.DataBind();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModalBusqueda()", true);
        }
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtFiltroNombre.Text = string.Empty;
            ddlFiltroEstatus.SelectedIndex = 0;
            ddlImporteMayor.SelectedIndex = 0;
            txtFiltroDcmImporteMayor.Text = string.Empty;
            ddlImporteMenor.SelectedIndex = 0;
            txtFiltroDcmImporteMenor.Text = string.Empty;
            txtFiltroDtInicioDesde.Text = string.Empty;
            txtFiltroDtInicioHasta.Text = string.Empty;
            txtFiltroDtFinDesde.Text = string.Empty;
            txtFiltroDtFinHasta.Text = string.Empty;
        }
    }
}