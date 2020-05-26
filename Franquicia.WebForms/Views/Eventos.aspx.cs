using Franquicia.Bussiness;
using Franquicia.WebForms.Util;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.Vista;

namespace Franquicia.WebForms.Views
{
    public partial class Eventos : System.Web.UI.Page
    {
        #region Propiedades
        // Nombre del servidor
        string ServerName
        {
            get { return Request.ServerVariables["SERVER_NAME"].ToString(); }
        }
        // Puerto del servidor
        string ServerPort
        {
            get { return Request.ServerVariables["SERVER_PORT"].ToString(); }
        }

        // Obtener URL Base
        public string URLBase
        {
            get
            {
                if (ServerPort != string.Empty && ServerPort.Trim() != "")
                { return "https://" + ServerName + ":" + ServerPort + "/"; }
                else
                { return "https://" + ServerName + "/"; }
            }
        }
        #endregion

        EventosServices eventosServices = new EventosServices();
        UsuariosCompletosServices usuariosCompletosServices = new UsuariosCompletosServices();
        ParametrosEntradaServices parametrosEntradaServices = new ParametrosEntradaServices();
        PromocionesServices promocionesServices = new PromocionesServices();
        PrefijosTelefonicosServices prefijosTelefonicosServices = new PrefijosTelefonicosServices();
        ValidacionesServices validacionesServices = new ValidacionesServices();
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            txtImporte.Attributes.Add("onchange", "button_click(this,'" + btnCalcular.ClientID + "')");

            if (!IsPostBack)
            {
                tmValidar.Enabled = false;

                if (Request.QueryString["Id"] != null)
                {
                    Session["eventosServices"] = eventosServices;
                    Session["EvenUsuariosCompletosServices"] = usuariosCompletosServices;
                    Session["EvenParametrosEntradaServices"] = parametrosEntradaServices;
                    Session["EvenpromocionesServices"] = promocionesServices;
                    Session["EvenPrefijosTelefonicosServices"] = prefijosTelefonicosServices;

                    eventosServices.ObtenerDatosEvento(Guid.Parse(Request.QueryString["Id"]));

                    if (eventosServices.eventosRepository.eventosGridViewModel.UidPropietario != null && eventosServices.eventosRepository.eventosGridViewModel.UidPropietario != Guid.Empty)
                    {
                        ViewState["UidClienteLocal"] = eventosServices.eventosRepository.eventosGridViewModel.UidPropietario;
                    }
                    else
                    {
                        ViewState["UidClienteLocal"] = Guid.Empty;
                    }

                    promocionesServices.CargarPromocionesEvento(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse(Request.QueryString["Id"].ToString()));

                    ddlFormasPago.Items.Insert(0, new ListItem("Al contado", "contado"));
                    ddlFormasPago.DataSource = promocionesServices.lsEventosGenerarLigasModel;
                    ddlFormasPago.DataTextField = "VchDescripcion";
                    ddlFormasPago.DataValueField = "UidPromocion";
                    ddlFormasPago.DataBind();

                    if (eventosServices.eventosRepository.eventosGridViewModel.UidEstatus == Guid.Parse("65E46BC9-1864-4145-AD1A-70F5B5F69739"))
                    {
                        //DateTime hoy = DateTime.Now;

                        DateTime HoraDelServidor = DateTime.Now;
                        DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");


                        DateTime fhInicio = eventosServices.eventosRepository.eventosGridViewModel.DtFHInicio;
                        DateTime fhFin = eventosServices.eventosRepository.eventosGridViewModel.DtFHFin;


                        DateTime date1 = hoy;
                        DateTime date2 = fhInicio;
                        int result = DateTime.Compare(date1, date2);

                        DateTime date3 = hoy;
                        DateTime date4 = fhFin;
                        int result2 = DateTime.Compare(date3, date4);

                        if (result >= 0 && result2 <= 0)
                        {
                            CargarConfiguracion();

                            txtCorreo.Focus();
                        }
                        else
                        {

                            if (result <= 0 && result2 <= 0)
                            {
                                pnlValidar.Visible = false;
                                tmValidar.Enabled = false;

                                pnlDatosComercio.Visible = false;
                                pnlUsuario.Visible = false;
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
                                pnlUsuario.Visible = false;
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
                        pnlValidar.Visible = false;
                        tmValidar.Enabled = false;

                        pnlDatosComercio.Visible = false;
                        pnlUsuario.Visible = false;
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
                //else if (Request.QueryString["CodigoPromocion"] != null)
                //{
                //    pnlDatosComercio.Visible = false;
                //    pnlUsuario.Visible = false;
                //    pnlCorreo.Visible = false;
                //    pnlIframe.Visible = true;

                //    btnGenerarLigas.Visible = false;
                //    btnCancelar.Visible = false;
                //    btnAceptar.Visible = true;

                //    ifrLiga.Src = eventosServices.ObtenerUrlLiga(Request.QueryString["CodigoPromocion"].ToString());
                //}
            }
            else
            {
                eventosServices = (EventosServices)Session["eventosServices"];
                usuariosCompletosServices = (UsuariosCompletosServices)Session["EvenUsuariosCompletosServices"];
                parametrosEntradaServices = (ParametrosEntradaServices)Session["EvenParametrosEntradaServices"];
                promocionesServices = (PromocionesServices)Session["EvenPromocionesServices"];
                prefijosTelefonicosServices = (PrefijosTelefonicosServices)Session["EvenPrefijosTelefonicosServices"];

                pnlAlert.Visible = false;
                lblMensajeAlert.Text = "";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade");
            }
        }
        private void CargarConfiguracion()
        {
            lblTitle.Text = eventosServices.eventosRepository.eventosGridViewModel.VchConcepto;
            lblNombreEvento.Text = eventosServices.eventosRepository.eventosGridViewModel.VchNombreEvento;
            txtDescripcion.Text = eventosServices.eventosRepository.eventosGridViewModel.VchDescripcion;
            txtNombreComercial.Text = eventosServices.eventosRepository.eventosGridViewModel.VchNombreComercial;
            txtComeCelular.Text = eventosServices.eventosRepository.eventosGridViewModel.VchTelefono;
            txtComeCorreo.Text = eventosServices.eventosRepository.eventosGridViewModel.VchCorreo;
            //lblConcepto.Text = "&nbsp;" + ligasUrlsService.ligasUrlsRepository.ligasUrlsConstruirLigaModel.VchConcepto;
            if (eventosServices.eventosRepository.eventosGridViewModel.BitTipoImporte)
            {
                txtImporte.Enabled = true;
            }
            else
            {
                txtImporte.Enabled = false;

                lblPagar.Text = "Pagar $" + eventosServices.eventosRepository.eventosGridViewModel.DcmImporte.ToString("N2");
            }
            txtImporte.Text = eventosServices.eventosRepository.eventosGridViewModel.DcmImporte.ToString("N2");

            if (eventosServices.eventosRepository.eventosGridViewModel.BitDatosUsuario)
            {
                prefijosTelefonicosServices.CargarPrefijosTelefonicos();
                ddlPrefijo.DataSource = prefijosTelefonicosServices.lsPrefijosTelefonicos;
                ddlPrefijo.DataTextField = "VchCompleto";
                ddlPrefijo.DataValueField = "UidPrefijo";
                ddlPrefijo.DataBind();

                ViewState["AccionEvento"] = "RegistroCompleto";
                lblTituloDatosUsuario.Text = "Datos de registro";
                lblTituloCorreo.Text = "Correo *";
                ddlPrefijo.SelectedIndex = ddlPrefijo.Items.IndexOf(ddlPrefijo.Items.FindByValue("abb854c4-e7ed-420f-8561-aa4b61bf5b0f"));
                pnlUsuario.Visible = true;
            }
            else
            {
                ViewState["AccionEvento"] = "RegistroCorreo";
                lblTituloDatosUsuario.Text = "Correo para envio de comprobante de pago";
                lblTituloCorreo.Text = "Correo (Opcional)";
                pnlUsuario.Visible = false;
            }

            //lblVencimiento.Text = "&nbsp;" + ligasUrlsService.ligasUrlsRepository.ligasUrlsConstruirLigaModel.DtVencimiento.ToString("dd/MM/yyyy");
            //lblPagar.Text = "Pagar $" + ligasUrlsService.ligasUrlsRepository.ligasUrlsConstruirLigaModel.DcmImporte.ToString("N2");

            Guid UidAdminCliente = Guid.Parse(eventosServices.ObtenerUidAdminCliente(eventosServices.eventosRepository.eventosGridViewModel.UidPropietario));

            if (UidAdminCliente != null && UidAdminCliente != Guid.Empty)
            {
                ViewState["UidUsuarioLocal"] = UidAdminCliente;
            }
            else
            {
                ViewState["UidUsuarioLocal"] = Guid.Empty;
            }

            ddlFormasPago_SelectedIndexChanged(null, null);
        }

        private void LimpiarCampos()
        {
            txtCorreo.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtApePaterno.Text = string.Empty;
            txtApeMaterno.Text = string.Empty;

            txtCelular.Text = string.Empty;
            txtImporte.Text = string.Empty;
            ddlFormasPago.SelectedIndex = 0;
            txtImporteTotal.Text = string.Empty;
        }
        protected void btnGenerarPago_Click(object sender, EventArgs e)
        {
            string MontoMin = "50.00";
            string MontoMax = "15000.00";

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

            if (decimal.Parse(txtImporte.Text) >= decimal.Parse(MontoMin) && decimal.Parse(txtImporte.Text) <= decimal.Parse(MontoMax))
            {

            }
            else
            {
                txtImporte.BackColor = System.Drawing.Color.FromName("#f2dede");
                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "El importe mínimo es de <b>$50.00</b> y el máximo es de <b>$15,000.00.</b>";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                return;
            }

            ViewState["UidUsuarioRegistro"] = ViewState["UidUsuarioLocal"];

            if (ViewState["AccionEvento"].ToString() == "RegistroCompleto")
            {
                usuariosCompletosServices.AsociarUsuariosFinales(txtCorreo.Text);

                if (usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.UidUsuario != null && usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.UidUsuario != Guid.Empty)
                {
                    if (validacionesServices.ExisteUsuarioCliente(Guid.Parse(ViewState["UidClienteLocal"].ToString()), usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.UidUsuario))
                    {
                        ViewState["UidUsuarioRegistro"] = usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.UidUsuario;
                    }
                    else
                    {
                        if (usuariosCompletosServices.AsociarClienteUsuario(Guid.Parse(ViewState["UidClienteLocal"].ToString()), usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.UidUsuario))
                        {
                            ViewState["UidUsuarioRegistro"] = usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.UidUsuario;
                        }
                    }
                }
                else
                {
                    if (txtCorreo.EmptyTextBox())
                    {
                        pnlAlert.Visible = true;
                        lblMensajeAlert.Text = "El campo <b>Correo</b> es obligatorio.";
                        divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                        return;
                    }
                    if (txtNombre.EmptyTextBox())
                    {
                        pnlAlert.Visible = true;
                        lblMensajeAlert.Text = "El campo <b>Nombre(s)</b> es obligatorio.";
                        divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                        return;
                    }
                    if (txtApePaterno.EmptyTextBox())
                    {
                        pnlAlert.Visible = true;
                        lblMensajeAlert.Text = "El campo <b>Ape Paterno</b> es obligatorio.";
                        divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                        return;
                    }
                    if (txtApeMaterno.EmptyTextBox())
                    {
                        pnlAlert.Visible = true;
                        lblMensajeAlert.Text = "El campo <b>Ape Materno</b> es obligatorio.";
                        divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                        return;
                    }
                    if (txtCelular.EmptyTextBox())
                    {
                        pnlAlert.Visible = true;
                        lblMensajeAlert.Text = "El campo <b>Celular</b> es obligatorio.";
                        divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                        return;
                    }

                    #region Generar Usuario y Contraseña
                    string usuario = string.Empty;
                    string password = string.Empty;

                    string[] Descripcion = Regex.Split(txtNombre.Text.Trim().ToUpper(), " ");
                    int numMax = Descripcion.Length;
                    usuario = Descripcion[numMax - 1].Substring(0, 1).ToString() + "." + txtApePaterno.Text.Trim().ToUpper();

                    if (validacionesServices.ExisteUsuario(usuario.ToString()))
                    {
                        DateTime dateTime = DateTime.Now;

                        usuario = Descripcion[numMax - 1].Substring(0, 1).ToString() + "." + txtApeMaterno.Text.Trim().ToUpper();

                        if (validacionesServices.ExisteUsuario(usuario.ToString()))
                        {
                            usuario = Descripcion[numMax - 1].Substring(0, 1).ToString() + "." + txtApePaterno.Text.Trim().ToUpper() + dateTime.ToString("mmssff");
                        }
                    }

                    Random obj = new Random();
                    string posibles = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                    int longitud = posibles.Length;
                    char letra;
                    int longitudnuevacadena = 8;
                    for (int i = 0; i < longitudnuevacadena; i++)
                    {
                        letra = posibles[obj.Next(longitud)];
                        password += letra.ToString();
                    }
                    #endregion

                    Guid UidUsuario = Guid.NewGuid();

                    if (usuariosCompletosServices.RegistrarUsuarios(UidUsuario,
                    txtNombre.Text.Trim().ToUpper(), txtApePaterno.Text.Trim().ToUpper(), txtApeMaterno.Text.Trim().ToUpper(), txtCorreo.Text.Trim().ToUpper(), usuario.ToString().Trim().ToUpper(), password.ToString().Trim(), new Guid("18E9669B-C238-4BCC-9213-AF995644A5A4"),
                    txtCelular.Text.Trim(), Guid.Parse("B1055882-BCBA-4AB7-94FA-90E57647E607"), new Guid(ddlPrefijo.SelectedValue), new Guid(ViewState["UidClienteLocal"].ToString())))
                    {
                        ViewState["UidUsuarioRegistro"] = UidUsuario;
                    }
                }
            }

            pnlDatosComercio.Visible = false;
            pnlUsuario.Visible = false;
            pnlCorreo.Visible = false;
            pnlGenerarLigas.Visible = true;
            pnlIframe.Visible = false;

            btnGenerarLigas.Visible = false;
            btnCancelar.Visible = true;

            string identificador = eventosServices.eventosRepository.eventosGridViewModel.VchNombreEvento;
            string concepto = eventosServices.eventosRepository.eventosGridViewModel.VchConcepto;
            string vencimiento = eventosServices.eventosRepository.eventosGridViewModel.DtFHFin.ToString("dd/MM/yyyy");
            string correo = txtCorreo.Text;
            int intCorreo = 0;
            decimal importeTotal = decimal.Parse(txtImporte.Text);

            if (!string.IsNullOrEmpty(correo))
            {
                intCorreo = 1;
            }

            string url = string.Empty;
            bool resu = false;

            parametrosEntradaServices.ObtenerParametrosEntradaCliente(Guid.Parse(ViewState["UidClienteLocal"].ToString()));

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
                Guid UidLigaAsociado = Guid.NewGuid();

                usuariosCompletosServices.SelectUsClienteEvento(Guid.Parse(ViewState["UidUsuarioRegistro"].ToString()));
                promocionesServices.CargarPromocionesEvento(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse(Request.QueryString["Id"].ToString()));

                foreach (var item in usuariosCompletosServices.lsEventoLiga)
                {
                    if (ddlFormasPago.SelectedValue != "contado")
                    {
                        DateTime thisDay = DateTime.Now;
                        string ReferenciaCobro = item.IdCliente.ToString() + item.IdUsuario.ToString() + thisDay.ToString("ddMMyyyyHHmmssfff");

                        foreach (var itPromo in promocionesServices.lsEventosGenerarLigasModel.Where(x => x.UidPromocion == Guid.Parse(ddlFormasPago.SelectedValue)).ToList())
                        {
                            decimal Valor = itPromo.DcmComicion * importeTotal / 100;
                            decimal Importe = Valor + importeTotal;

                            string promocion = itPromo.VchDescripcion.Replace(" MESES", "");

                            DateTime thisDay2 = DateTime.Now;
                            string Referencia = item.IdCliente.ToString() + item.IdUsuario.ToString() + thisDay2.ToString("ddMMyyyyHHmmssfff");

                            url = GenLigaPara(id_company, id_branch, user, pwd, Referencia, Importe, moneda, canal, promocion, intCorreo, vencimiento, correo, concepto, semillaAES, data0, urlGen);

                            if (url.Contains("https://"))
                            {
                                if (usuariosCompletosServices.GenerarLigasPagosEvento(url, concepto, Importe, Referencia, item.UidUsuario, identificador, thisDay, DateTime.Parse(vencimiento), "EVENTO", UidLigaAsociado, itPromo.UidPromocion, Guid.Parse(Request.QueryString["Id"].ToString()), Guid.Parse(ViewState["UidClienteLocal"].ToString())))
                                {
                                    if (ViewState["AccionEvento"].ToString() == "RegistroCorreo")
                                    {
                                        if (!string.IsNullOrEmpty(txtCorreo.Text))
                                        {
                                            eventosServices.InsertCorreoLigaEvento(txtCorreo.Text, Referencia);
                                        }
                                    }

                                    ViewState["IdReferenciaCobroEvento"] = Referencia;
                                    btnPagar.Text = "Pagar $" + Importe;
                                    pnlGenerarLigas.Visible = false;
                                    pnlDatosComercio.Visible = false;
                                    pnlUsuario.Visible = false;
                                    pnlCorreo.Visible = false;
                                    pnlIframe.Visible = true;

                                    btnCancelar.Visible = false;
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

                        string urlCobro = GenLigaPara(id_company, id_branch, user, pwd, ReferenciaCobro, importeTotal, moneda, canal, "C", intCorreo, vencimiento, correo, concepto, semillaAES, data0, urlGen);

                        if (urlCobro.Contains("https://"))
                        {
                            if (usuariosCompletosServices.GenerarLigasPagosEvento(urlCobro, concepto, importeTotal, ReferenciaCobro, item.UidUsuario, identificador, thisDay, DateTime.Parse(vencimiento), "EVENTO", Guid.Empty, Guid.Empty, Guid.Parse(Request.QueryString["Id"].ToString()), Guid.Parse(ViewState["UidClienteLocal"].ToString())))
                            {
                                if (ViewState["AccionEvento"].ToString() == "RegistroCorreo")
                                {
                                    if (!string.IsNullOrEmpty(txtCorreo.Text))
                                    {
                                        eventosServices.InsertCorreoLigaEvento(txtCorreo.Text, ReferenciaCobro);
                                    }
                                }

                                ViewState["IdReferenciaCobroEvento"] = ReferenciaCobro;
                                btnPagar.Text = "Pagar $" + ViewState["txtImporteTotal.Text"].ToString();
                                pnlGenerarLigas.Visible = false;
                                pnlDatosComercio.Visible = false;
                                pnlUsuario.Visible = false;
                                pnlCorreo.Visible = false;
                                pnlIframe.Visible = true;

                                btnCancelar.Visible = false;
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
        protected void btnGenerarLigas_Click(object sender, EventArgs e)
        {
            //pnlDatosComercio.Visible = false;
            //pnlUsuario.Visible = false;
            //pnlCorreo.Visible = false;
            //pnlGenerarLigas.Visible = true;
            //pnlIframe.Visible = false;

            //btnGenerarLigas.Visible = false;
            //btnCancelar.Visible = true;

            //string identificador = eventosServices.eventosRepository.eventosGridViewModel.VchNombreEvento;
            //string concepto = eventosServices.eventosRepository.eventosGridViewModel.VchConcepto;
            //string vencimiento = eventosServices.eventosRepository.eventosGridViewModel.DtFHFin.ToString("dd/MM/yyyy");
            //string correo = txtCorreo.Text;
            //int intCorreo = 0;

            //if (!string.IsNullOrEmpty(correo))
            //{
            //    intCorreo = 1;
            //}

            //string url = string.Empty;
            //bool resu = false;

            //parametrosEntradaServices.ObtenerParametrosEntradaCliente(Guid.Parse(ViewState["UidClienteLocal"].ToString()));

            //string id_company = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.IdCompany;
            //string id_branch = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.IdBranch;
            //string user = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchUsuario;
            //string pwd = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchPassword;
            //string moneda = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchModena;
            //string canal = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchCanal;
            //string semillaAES = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchSemillaAES;
            //string urlGen = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchUrl;
            //string data0 = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchData0;

            //if (!string.IsNullOrEmpty(id_company) && !string.IsNullOrEmpty(id_branch) && !string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(pwd) && !string.IsNullOrEmpty(moneda) && !string.IsNullOrEmpty(canal) && !string.IsNullOrEmpty(semillaAES) && !string.IsNullOrEmpty(urlGen) && !string.IsNullOrEmpty(data0))
            //{
            //    bool VariasLigas = false;
            //    Guid UidLigaAsociado = Guid.NewGuid();

            //    usuariosCompletosServices.SelectUsClienteEvento(Guid.Parse(ViewState["UidUsuarioLocal"].ToString()));
            //    promocionesServices.CargarPromocionesEvento(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse(Request.QueryString["Id"].ToString()));

            //    foreach (var item in usuariosCompletosServices.lsEventoLiga)
            //    {
            //        if (promocionesServices.lsEventosGenerarLigasModel.Count >= 1)
            //        {
            //            VariasLigas = true;
            //        }

            //        if (VariasLigas)
            //        {
            //            DateTime thisDay = DateTime.Now;
            //            string ReferenciaCobro = item.IdCliente.ToString() + item.IdUsuario.ToString() + thisDay.ToString("ddMMyyyyHHmmssfff");
            //            Guid UidLigaUrl = Guid.NewGuid();
            //            string urlCobro = GenLigaPara(id_company, id_branch, user, pwd, ReferenciaCobro, decimal.Parse(txtImporte.Text), moneda, canal, "C", intCorreo, vencimiento, correo, concepto, semillaAES, data0, urlGen);

            //            if (urlCobro.Contains("https://"))
            //            {
            //                if (usuariosCompletosServices.GenerarLigasPagosTemp(UidLigaUrl, urlCobro, concepto, decimal.Parse(txtImporte.Text), ReferenciaCobro, item.UidUsuario, identificador, thisDay, DateTime.Parse(vencimiento), "EVENTO", UidLigaAsociado, Guid.Empty, Guid.Parse(ViewState["UidClienteLocal"].ToString())))
            //                {
            //                    resu = true;

            //                    btnPagar.Text = "Pagar $" + txtImporte.Text;
            //                    ViewState["urlCobro"] = urlCobro;

            //                    foreach (var itPromo in promocionesServices.lsEventosGenerarLigasModel)
            //                    {
            //                        //int i = promocionesServices.lsCBLPromocionesModelCliente.IndexOf(promocionesServices.lsCBLPromocionesModelCliente.First(x => x.UidPromocion == itPromo.UidPromocion));
            //                        //decimal cobro = promocionesServices.lsCBLPromocionesModelCliente[i].DcmComicion;

            //                        decimal Valor = itPromo.DcmComicion * decimal.Parse(txtImporte.Text) / 100;
            //                        decimal Importe = Valor + decimal.Parse(txtImporte.Text);

            //                        string promocion = itPromo.VchDescripcion.Replace(" MESES", "");

            //                        DateTime thisDay2 = DateTime.Now;
            //                        string Referencia = item.IdCliente.ToString() + item.IdUsuario.ToString() + thisDay2.ToString("ddMMyyyyHHmmssfff");

            //                        url = GenLigaPara(id_company, id_branch, user, pwd, Referencia, Importe, moneda, canal, promocion, intCorreo, vencimiento, correo, concepto, semillaAES, data0, urlGen);

            //                        if (url.Contains("https://"))
            //                        {
            //                            if (usuariosCompletosServices.GenerarLigasPagos(url, concepto, Importe, Referencia, item.UidUsuario, identificador, thisDay, DateTime.Parse(vencimiento), "EVENTO", UidLigaAsociado, itPromo.UidPromocion, Guid.Parse(ViewState["UidClienteLocal"].ToString())))
            //                            {
            //                                resu = true;
            //                            }
            //                        }
            //                        else
            //                        {
            //                            resu = false;
            //                            break;
            //                        }
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                resu = false;
            //                break;
            //            }

            //            if (resu)
            //            {
            //                promocionesServices.CargarPromocionesValidas(UidLigaAsociado);

            //                string strPromociones = string.Empty;

            //                if (promocionesServices.lsLigasUrlsPromocionesModel.Count >= 1)
            //                {
            //                    foreach (var itPromo in promocionesServices.lsLigasUrlsPromocionesModel)
            //                    {
            //                        decimal promocion = int.Parse(itPromo.VchDescripcion.Replace(" MESES", ""));
            //                        decimal Final = itPromo.DcmImporte / promocion;

            //                        strPromociones +=
            //                        "\t\t\t\t\t\t\t\t<tr>\r\n" +
            //                        "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: right;\">\r\n" +
            //                        "\t\t\t\t\t\t\t\t\t\t" + itPromo.VchDescripcion + " de $" + Final.ToString("N2") + "\r\n" +
            //                        "\t\t\t\t\t\t\t\t\t</td>\r\n" +
            //                        "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: left;\">\r\n" +
            //                        "\t\t\t\t\t\t\t\t\t\t &nbsp;" + "<a style =\"display:block;color:#fff;font-weight:400;text-align:center;width:100px;font-size:15px;text-decoration:none;background:#28a745;margin:0 auto; padding:5px;\" href=" + URLBase + "Views/Eventos.aspx?CodigoPromocion=" + itPromo.IdReferencia + "> Pagar $" + itPromo.DcmImporte.ToString("N2") + "</a>" + "\r\n" +
            //                        "\t\t\t\t\t\t\t\t\t</td>\r\n" +
            //                        "\t\t\t\t\t\t\t\t</tr>\r\n";
            //                    }
            //                }

            //                pnlPromociones.Visible = true;
            //                ltlPromociones.Text = strPromociones;

            //                //string LigaUrl = URLBase + "Views/Promociones.aspx?CodigoPromocion=" + UidLigaAsociado + "&CodigoLiga=" + ReferenciaCobro;
            //            }
            //        }
            //        else
            //        {
            //            DateTime thisDay = DateTime.Now;
            //            string ReferenciaCobro = item.IdCliente.ToString() + item.IdUsuario.ToString() + thisDay.ToString("ddMMyyyyHHmmssfff");

            //            string urlCobro = GenLigaPara(id_company, id_branch, user, pwd, ReferenciaCobro, decimal.Parse(txtImporte.Text), moneda, canal, "C", intCorreo, vencimiento, correo, concepto, semillaAES, data0, urlGen);

            //            if (urlCobro.Contains("https://"))
            //            {
            //                Guid UidLigaUrl = Guid.NewGuid();
            //                if (usuariosCompletosServices.GenerarLigasPagosTemp(UidLigaUrl, urlCobro, concepto, decimal.Parse(txtImporte.Text), ReferenciaCobro, item.UidUsuario, identificador, thisDay, DateTime.Parse(vencimiento), "EVENTO", Guid.Empty, Guid.Empty, Guid.Parse(ViewState["UidClienteLocal"].ToString())))
            //                {
            //                    btnPagar.Text = "Pagar $" + txtImporte.Text;
            //                    pnlGenerarLigas.Visible = false;
            //                    pnlDatosComercio.Visible = false;
            //                    pnlUsuario.Visible = false;
            //                    pnlCorreo.Visible = false;
            //                    pnlIframe.Visible = true;

            //                    btnCancelar.Visible = false;
            //                    btnAceptar.Visible = true;

            //                    ifrLiga.Src = urlCobro;

            //                    resu = true;
            //                }
            //            }
            //            else
            //            {
            //                resu = false;
            //            }
            //        }
            //    }

            //    if (resu)
            //    {

            //    }
            //    else
            //    {
            //        if (!string.IsNullOrEmpty(url))
            //        {
            //            //pnlAlertModal.Visible = true;
            //            //lblResumen.Text = "<b>¡Lo sentimos! </b> " + url + "." + "<br /> Las credenciales proporcionadas no son correctos, por favor contacte a los administradores.";
            //            //divAlertModal.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
            //        }
            //        else
            //        {
            //            //pnlAlertModal.Visible = true;
            //            //lblResumen.Text = "<b>¡Lo sentimos! </b> Las credenciales proporcionadas no son correctos, por favor contacte a los administradores.";
            //            //divAlertModal.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
            //        }
            //    }
            //}
            //else
            //{
            //    //pnlAlertModal.Visible = true;
            //    //lblResumen.Text = "<b>¡Lo sentimos! </b> Esta empresa no cuenta con credenciales para generar ligas, por favor contacte a los administradores.";
            //    //divAlertModal.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
            //}
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

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            pnlDatosComercio.Visible = false;
            pnlUsuario.Visible = false;
            pnlCorreo.Visible = false;
            pnlIframe.Visible = true;
            ifrLiga.Src = ViewState["urlCobro"].ToString();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            pnlDatosComercio.Visible = true;
            pnlUsuario.Visible = eventosServices.eventosRepository.eventosGridViewModel.BitDatosUsuario;
            pnlCorreo.Visible = true;
            pnlGenerarLigas.Visible = false;
            pnlIframe.Visible = false;

            btnGenerarLigas.Visible = true;
            btnCancelar.Visible = false;
        }
        protected void btnCalcular_Click(object sender, EventArgs e)
        {
            ddlFormasPago_SelectedIndexChanged(null, null);
        }
        protected void ddlFormasPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtImporte.Text))
            {
                if (ddlFormasPago.SelectedValue == "contado")
                {
                    decimal importe = decimal.Parse(txtImporte.Text);

                    txtImporte.Text = importe.ToString("N2");
                    txtImporteTotal.Text = importe.ToString("N2");
                    ViewState["txtImporteTotal.Text"] = importe.ToString("N2");
                }
                else
                {
                    decimal importeTotal = decimal.Parse(txtImporte.Text);

                    promocionesServices.CargarPromocionesEvento(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse(Request.QueryString["Id"].ToString()));

                    foreach (var itPromo in promocionesServices.lsEventosGenerarLigasModel.Where(x => x.UidPromocion == Guid.Parse(ddlFormasPago.SelectedValue)).ToList())
                    {
                        decimal Valor = itPromo.DcmComicion * importeTotal / 100;
                        decimal Importe = Valor + importeTotal;

                        txtImporteTotal.Text = Importe.ToString("N2");
                        ViewState["txtImporteTotal.Text"] = Importe.ToString("N2");
                    }
                }

                lblTotalPago.Text = "Generar pago $" + ViewState["txtImporteTotal.Text"].ToString();
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

                    pnlDatosComercio.Visible = true;
                    pnlUsuario.Visible = true;
                    pnlCorreo.Visible = true;
                    pnlIframe.Visible = false;

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

                pnlDatosComercio.Visible = true;
                pnlUsuario.Visible = true;
                pnlCorreo.Visible = true;
                pnlIframe.Visible = false;

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
    }
}