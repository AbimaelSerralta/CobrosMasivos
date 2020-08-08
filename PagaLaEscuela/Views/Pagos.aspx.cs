using Franquicia.Bussiness;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.Vista;

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

                Session["pagosPadresServices"] = pagosPadresServices;
                Session["colegiaturasServices"] = colegiaturasServices;
                Session["pagosTarjetaServices"] = pagosTarjetaServices;
                Session["ligasUrlsServices"] = ligasUrlsServices;
                Session["promocionesServices"] = promocionesServices;
                Session["parametrosEntradaServices"] = parametrosEntradaServices;
                Session["usuariosCompletosServices"] = usuariosCompletosServices;

                pagosPadresServices.CargarComercios(Guid.Parse(ViewState["UidUsuarioLocal"].ToString()));
                rpComercios.DataSource = pagosPadresServices.lsPadresComerciosViewModels;
                rpComercios.DataBind();
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

                pnlAlert.Visible = false;
                lblMensajeAlert.Text = "";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade");

                lblValidar.Text = string.Empty;
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
                }
                else
                {
                    imgLogoSelect.ImageUrl = "../Images/SinLogo2.png";
                    imgLogoSelect.DataBind();
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

        }
        protected void gvPagos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "btnPagar")
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvPagos.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKey = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());

                DateTime HoraDelServidor = DateTime.Now;
                DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

                lblTitlePagar.Text = "Seleccionar forma de pago";
                btnCerrar.Visible = true;

                ViewState["RowCommand-UidFechaColegiatura"] = colegiaturasServices.lsPagosColegiaturasViewModel[index].UidFechaColegiatura;
                ViewState["RowCommand-Identificador"] = colegiaturasServices.lsPagosColegiaturasViewModel[index].VchIdentificador;
                lblConcepto.Text = "Pago " + colegiaturasServices.lsPagosColegiaturasViewModel[index].VchNum + ", " + colegiaturasServices.lsPagosColegiaturasViewModel[index].VchMatricula + " " + colegiaturasServices.lsPagosColegiaturasViewModel[index].NombreCompleto;
                lblVencimiento.Text = hoy.ToString("dd/MM/yyyy");
                lblImporteCole.Text = colegiaturasServices.lsPagosColegiaturasViewModel[index].DcmImporte.ToString("N2");
                lblRecargo.Text = "0.00";
                if (colegiaturasServices.lsPagosColegiaturasViewModel[index].VchFHLimite != "NO TIENE")
                {
                    if (hoy > DateTime.Parse(colegiaturasServices.lsPagosColegiaturasViewModel[index].VchFHLimite))
                    {
                        lblRecargo.Text = colegiaturasServices.lsPagosColegiaturasViewModel[index].DcmRecargo.ToString("N2");
                    }
                }
                decimal ImporteTotal = decimal.Parse(lblImporteCole.Text) + decimal.Parse(lblRecargo.Text);
                lblTieneBeca.Text = "NO";
                lblTipoBeca.Text = "NINGUNO";
                lblImporteBeca.Text = "0.00";
                decimal ImporteBeca = 0;
                if (colegiaturasServices.lsPagosColegiaturasViewModel[index].BitBeca)
                {
                    lblTieneBeca.Text = "SI";
                    lblTipoBeca.Text = colegiaturasServices.lsPagosColegiaturasViewModel[index].VchTipoBeca;

                    if (colegiaturasServices.lsPagosColegiaturasViewModel[index].VchTipoBeca == "CANTIDAD")
                    {
                        ImporteBeca = colegiaturasServices.lsPagosColegiaturasViewModel[index].DcmBeca;
                        ImporteTotal = ImporteTotal - ImporteBeca;
                    }
                    else if (colegiaturasServices.lsPagosColegiaturasViewModel[index].VchTipoBeca == "PORCENTAJE")
                    {
                        decimal porcentaje = colegiaturasServices.lsPagosColegiaturasViewModel[index].DcmBeca;

                        ImporteBeca = porcentaje * ImporteTotal / 100;

                        ImporteTotal = ImporteTotal - ImporteBeca;
                    }

                    lblImporteBeca.Text = ImporteBeca.ToString("N2");
                }

                lblImporteTotal.Text = ImporteTotal.ToString("N2");

                Calcular(ImporteTotal.ToString(), colegiaturasServices.lsPagosColegiaturasViewModel[index].UidColegiatura);

                pnlPromociones.Visible = true;
                pnlIframe.Visible = false;

                btnFinalizar.Visible = false;
                btnGenerarLiga.Visible = true;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalPagar()", true);
            }
        }
        protected void gvPagos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            pnlValidar.Visible = true;
            tmValidar.Enabled = true;
            pnlIframe.Visible = false;
            ViewState["tmValidar"] = DateTime.Now.AddSeconds(5).ToString();
        }
        protected void Calcular(string Importe, Guid UidColegiatura)
        {
            if (!string.IsNullOrEmpty(Importe))
            {
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
                    decimal importe = decimal.Parse(lblImporteTotal.Text);

                    lblTotalPagar.Text = importe.ToString("N2");
                    ViewState["txtImporteTotal.Text"] = importe.ToString("N2");
                }
                else
                {
                    decimal importeTotal = decimal.Parse(lblImporteTotal.Text);

                    foreach (var itPromo in promocionesServices.lsPromocionesColegiaturaModel.Where(x => x.UidPromocion == Guid.Parse(ddlFormasPago.SelectedValue)).ToList())
                    {
                        decimal Valor = itPromo.DcmComicion * importeTotal / 100;
                        decimal Importe = Valor + importeTotal;

                        lblTotalPagar.Text = Importe.ToString("N2");
                        lblTotalPago.Text = Importe.ToString("N2");
                        ViewState["txtImporteTotal.Text"] = Importe.ToString("N2");
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
        }
        protected void btnGenerarLiga_Click(object sender, EventArgs e)
        {
            string identificador = ViewState["RowCommand-Identificador"].ToString();
            string concepto = lblConcepto.Text;

            string vencimiento = lblVencimiento.Text;

            int intCorreo = 1;
            decimal importeTotal = decimal.Parse(lblTotalPagar.Text);

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
                                if (usuariosCompletosServices.GenerarLigasPagosColegiatura(url, concepto, importeTotal, Referencia, item.UidUsuario, identificador, thisDay, DateTime.Parse(vencimiento), "PAGO COLEGIATURA", UidLigaAsociado, Guid.Parse(ddlFormasPago.SelectedValue), Guid.Parse(ViewState["RowCommand-UidFechaColegiatura"].ToString()), Guid.Parse(ViewState["ItemCommand-UidCliente"].ToString())))
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
                                if (usuariosCompletosServices.GenerarLigasPagosColegiatura(urlCobro, concepto, importeTotal, ReferenciaCobro, item.UidUsuario, identificador, thisDay, DateTime.Parse(vencimiento), "PAGO COLEGIATURA", Guid.Empty, Guid.Empty, Guid.Parse(ViewState["RowCommand-UidFechaColegiatura"].ToString()), Guid.Parse(ViewState["ItemCommand-UidCliente"].ToString())))
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
                    lblImporteTotal.BackColor = System.Drawing.Color.FromName("#f2dede");
                    lblValidar.Text = "El importe mínimo es de $50.00 y el máximo es de $15,000.00.";
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
    }
}