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
    public partial class HistorialPagosComercio : System.Web.UI.Page
    {
        TarifasServices tarifasServices = new TarifasServices();
        UsuariosCompletosServices usuariosCompletosServices = new UsuariosCompletosServices();
        HistorialPagosServices historialPagosServices = new HistorialPagosServices();
        ClienteCuentaServices clienteCuentaServices = new ClienteCuentaServices();
        ValidacionesServices validacionesServices = new ValidacionesServices();
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

            txtImporte.Attributes.Add("onchange", "button_click(this,'" + btnCalcular.ClientID + "')");
            txtCantidadWA.Attributes.Add("onchange", "button_click(this,'" + btnCalcularWA.ClientID + "')");
            txtCantidadSms.Attributes.Add("onchange", "button_click(this,'" + btnCalcularSms.ClientID + "')");

            clienteCuentaServices.ObtenerDineroCuentaCliente(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
            lblGvSaldo.Text = "Saldo: $ " + clienteCuentaServices.clienteCuentaRepository.clienteCuenta.DcmDineroCuenta.ToString("N2");

            if (!IsPostBack)
            {
                ViewState["gvHistorial"] = SortDirection.Ascending;

                Session["historialPagosServices"] = historialPagosServices;
                Session["usuariosCompletosServices"] = usuariosCompletosServices;
                tmValidar.Enabled = false;

                historialPagosServices.CargarMovimientos(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                gvHistorial.DataSource = historialPagosServices.lsHistorialPagosGridViewModel;
                gvHistorial.DataBind();
            }
            else
            {
                historialPagosServices = (HistorialPagosServices)Session["historialPagosServices"];
                usuariosCompletosServices = (UsuariosCompletosServices)Session["usuariosCompletosServices"];

                pnlAlert.Visible = false;
                lblMensajeAlert.Text = "";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade");
            }
        }
        private string GenLigaPara(string id_company, string id_branch, string user, string pwd, string Referencia, decimal Importe,
            string moneda, string canal, string promocion, string Correo, string semillaAES, string data0, string urlGen)
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
                "    <st_correo>1</st_correo>\r\n" +
                "    <fh_vigencia>" + DateTime.Parse(txtVencimiento.Text).ToString("dd/MM/yyyy") + "</fh_vigencia>\r\n" +
                "    <mail_cliente>" + Correo + "</mail_cliente>\r\n" +
                "    <datos_adicionales>\r\n" +
                "      <data id=\"1\" display=\"true\">\r\n" +
                "        <label>Concepto:</label>\r\n" +
                "        <value>" + txtConcepto.Text + "</value>\r\n" +
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
        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            if (Session["UidUsuarioMaster"] != null)
            {
                ViewState["UidUsuarioLocal"] = Session["UidUsuarioMaster"];
            }
            else
            {
                ViewState["UidUsuarioLocal"] = Guid.Empty;
            }

            tarifasServices.CargarTarifas();

            foreach (var item in tarifasServices.lsTarifasGridViewModel)
            {
                lblDcmWhatsapp.Text = item.DcmWhatsapp.ToString("N2");
                lblDcmSms.Text = item.DcmSms.ToString("N2");
            }

            clienteCuentaServices.ObtenerDineroCuentaCliente(Guid.Parse(ViewState["UidClienteLocal"].ToString()));

            txtSaldo.Text = clienteCuentaServices.clienteCuentaRepository.clienteCuenta.DcmDineroCuenta.ToString("N2");

            txtVencimiento.Text = DateTime.Now.ToString("yyyy-MM-dd");

            lblTituloModal.Text = "<strong>Paso 1</strong> cantidad de recarga.";

            LimpiarTodo();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);
        }

        public void LimpiarTodo()
        {
            txtCantidadWA.Text = string.Empty;
            txtResultadoWA.Text = string.Empty;
            btnAgregarWa.Visible = false;

            txtCantidadSms.Text = string.Empty;
            txtResultadoSms.Text = string.Empty;
            btnAgregarSms.Visible = false;

            txtImporte.Text = string.Empty;
            txtNuevoSaldo.Text = string.Empty;

            pnlSeleccion.Visible = true;
            pnlIframe.Visible = false;
            pnlValidar.Visible = false;

            btnCerrar.Visible = false;
            btnGenerar.Visible = true;
            btnCancelar.Visible = true;

            btnGenerar.Enabled = false;
            lblValidar.Text = string.Empty;
        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            if (ViewState["val2"].ToString() != txtImporte.Text)
            {
                lblValidar.Text = "Parece que el monto ha cambiado, por favor calcule de nuevo para continuar";
                btnGenerar.Enabled = false;
                return;
            }

            if (txtImporte.EmptyTextBox())
            {
                lblValidar.Text = "El campo Monto es obligatorio";
                return;
            }

            if (!string.IsNullOrEmpty(lblValidar.Text))
            {
                lblValidar.Text = string.Empty;
            }

            string id_company = "Z937";
            string id_branch = "851";
            string user = "Z937SIUS21";
            string pwd = "C0BR05T4RJ3TA";
            string moneda = "MXN";
            string canal = "W";
            string semillaAES = "7AACFE849FABD796F6DCB947FD4D5268";
            string urlGen = "https://bc.mitec.com.mx/p/gen";
            string data0 = "9265655113";

            lblTituloModal.Text = "<strong>Paso 2</strong> pagar";

            usuariosCompletosServices.SeleccionarUsuariosCliente(Guid.Parse(ViewState["UidUsuarioLocal"].ToString()));

            foreach (var item in usuariosCompletosServices.lsPagoLiga)
            {
                string ReferenciaCobro = item.IdCliente.ToString() + item.IdUsuario.ToString() + DateTime.Now.ToString("ddMMyyyyHHmmssfff");

                pnlSeleccion.Visible = false;
                pnlIframe.Visible = true;
                pnlValidar.Visible = false;

                btnCerrar.Visible = true;
                btnGenerar.Visible = false;
                btnCancelar.Visible = false;

                string urlCobro = GenLigaPara(id_company, id_branch, user, pwd, ReferenciaCobro, decimal.Parse(txtImporte.Text), moneda, canal, "C", item.StrCorreo, semillaAES, data0, urlGen);

                if (urlCobro.Contains("https://"))
                {
                    if (usuariosCompletosServices.GenerarLigasPagos(urlCobro, "Recarga para Whatsapp y sms", decimal.Parse(txtImporte.Text), ReferenciaCobro, item.UidUsuario, "Recarga para Whatsapp y sms", DateTime.Now, DateTime.Parse(txtVencimiento.Text), "Recarga para Whatsapp y sms", Guid.Empty, Guid.Empty, Guid.Parse(ViewState["UidClienteLocal"].ToString())))
                    {
                        //clienteCuentaServices.RegistrarDineroCuentaCliente(decimal.Parse(txtImporte.Text), Guid.Parse(ViewState["UidClienteLocal"].ToString()), ReferenciaCobro);
                        historialPagosServices.RegistrarHistorialPago(decimal.Parse(txtSaldo.Text), decimal.Parse(txtImporte.Text), decimal.Parse(txtNuevoSaldo.Text), ReferenciaCobro);
                        ViewState["IdReferenciaCobro"] = ReferenciaCobro;
                        ifrLiga.Src = urlCobro;
                    }
                }
            }
        }

        protected void btnCalcular_Click(object sender, EventArgs e)
        {
            Calcular();
        }

        private void Calcular()
        {
            lblValidar.Text = string.Empty;

            if (txtImporte.Text != string.Empty)
            {
                decimal val = decimal.Parse(txtSaldo.Text);
                decimal val2 = decimal.Parse(txtImporte.Text);
                decimal val3 = val + val2;

                txtImporte.Text = val2.ToString("N2");
                txtNuevoSaldo.Text = val3.ToString("N2");

                ViewState["val2"] = val2.ToString("N2");

                if (val2 >= 50)
                {
                    btnGenerar.Enabled = true;
                }
                else
                {
                    lblValidar.Text = "Lo sentimos, el monto minimo es de $50";
                }
            }
            else
            {
                txtNuevoSaldo.Text = "0.00";
            }
        }

        protected void btnCalcularWA_Click(object sender, EventArgs e)
        {
            if (txtCantidadWA.Text != string.Empty)
            {
                btnAgregarWa.Visible = true;

                decimal val = decimal.Parse(txtCantidadWA.Text);
                decimal val2 = decimal.Parse(lblDcmWhatsapp.Text);
                decimal val3 = val * val2;

                txtResultadoWA.Text = val3.ToString("N2");
            }
            else
            {
                btnAgregarWa.Visible = false;
                txtResultadoWA.Text = "0.00";
            }

        }
        protected void btnCalcularSms_Click(object sender, EventArgs e)
        {
            if (txtCantidadSms.Text != string.Empty)
            {
                btnAgregarSms.Visible = true;
                decimal val = decimal.Parse(txtCantidadSms.Text);
                decimal val2 = decimal.Parse(lblDcmSms.Text);
                decimal val3 = val * val2;

                txtResultadoSms.Text = val3.ToString("N2");
            }
            else
            {
                btnAgregarSms.Visible = false;
                txtResultadoSms.Text = "0.00";
            }
        }

        protected void tmValidar_Tick(object sender, EventArgs e)
        {
            if (DateTime.Compare(DateTime.Now, DateTime.Parse(Session["tmValidar"].ToString())) <= 0)
            {
                ltMnsj.Text = "Verificando...: " + (((Int32)DateTime.Parse(Session["tmValidar"].ToString()).Subtract(DateTime.Now).TotalSeconds) % 60).ToString();

                if (!validacionesServices.ValidarPagoCliente(ViewState["IdReferenciaCobro"].ToString()))
                {
                    tmValidar.Enabled = false;

                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "<b>Felicidades,</b> su pago se proceso exitosamente.";
                    divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);

                    historialPagosServices.CargarMovimientos(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                    gvHistorial.DataSource = historialPagosServices.lsHistorialPagosGridViewModel;
                    gvHistorial.DataBind();

                    clienteCuentaServices.ObtenerDineroCuentaCliente(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                    Master.GvSaldo.Text = "Saldo: $ " + clienteCuentaServices.clienteCuentaRepository.clienteCuenta.DcmDineroCuenta.ToString("N2");
                }
            }
            else
            {
                tmValidar.Enabled = false;

                historialPagosServices.EliminarHistorialPagoLigas(ViewState["IdReferenciaCobro"].ToString());

                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "<b>Lo sentimos,</b> no se ha podido procesar su pago.";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);

                clienteCuentaServices.ObtenerDineroCuentaCliente(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                Master.GvSaldo.Text = "Saldo: $ prueba " + clienteCuentaServices.clienteCuentaRepository.clienteCuenta.DcmDineroCuenta.ToString("N2");
            }
        }

        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            lblTituloModal.Text = "<strong>Paso final</strong> validar pago";

            pnlSeleccion.Visible = false;
            pnlIframe.Visible = false;
            pnlValidar.Visible = true;

            btnCerrar.Visible = false;
            btnGenerar.Visible = false;
            btnCancelar.Visible = false;

            tmValidar.Enabled = true;
            Session["tmValidar"] = DateTime.Now.AddSeconds(5).ToString();
        }

        protected void btnAgregarWa_Click(object sender, EventArgs e)
        {
            ViewState["ModalDialog"] = "DialogWA";

            lblMnsjDialog.Text = "Esta apunto de agregar <strong>" 
                + "$" + decimal.Parse(txtResultadoWA.Text).ToString("N2") +
                "</strong> al monto final. <br/><br/> ¿Esta seguro que desea continuar?";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalDialog()", true);
        }

        protected void btnAgregarSms_Click(object sender, EventArgs e)
        {
            ViewState["ModalDialog"] = "DialogSms";

            lblMnsjDialog.Text = "Esta apunto de agregar <strong>"
                + "$" + decimal.Parse(txtResultadoSms.Text).ToString("N2") +
                "</strong> al monto final. <br/><br/> ¿Esta seguro que desea continuar?";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalDialog()", true);
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            if (ViewState["ModalDialog"].ToString() == "DialogWA")
            {
                decimal val = decimal.Parse(txtResultadoWA.Text);
                decimal val2 = 0;
                if (txtImporte.Text != string.Empty)
                {
                    val2 = decimal.Parse(txtImporte.Text);
                }

                decimal result = val + val2;

                txtImporte.Text = result.ToString("N2");

                Calcular();

                txtCantidadWA.Text = string.Empty;
                txtResultadoWA.Text = string.Empty;
                btnAgregarWa.Visible = false;
            }
            else if(ViewState["ModalDialog"].ToString() == "DialogSms")
            {
                decimal val = decimal.Parse(txtResultadoSms.Text);
                decimal val2 = 0;
                if (txtImporte.Text != string.Empty)
                {
                    val2 = decimal.Parse(txtImporte.Text);
                }

                decimal result = val + val2;

                txtImporte.Text = result.ToString("N2");

                Calcular();

                txtCantidadSms.Text = string.Empty;
                txtResultadoSms.Text = string.Empty;
                btnAgregarSms.Visible = false;

            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModalDialog()", true);
        }

        protected void gvHistorial_Sorting(object sender, GridViewSortEventArgs e)
        {
            string SortExpression = e.SortExpression;
            SortDirection direccion;
            string Orden = string.Empty;

            if (ViewState["gvHistorial"] != null)
            {
                direccion = (SortDirection)ViewState["gvHistorial"];
                if (direccion == SortDirection.Ascending)
                {
                    ViewState["gvHistorial"] = SortDirection.Descending;
                    Orden = "ASC";
                }
                else
                {
                    ViewState["gvHistorial"] = SortDirection.Ascending;
                    Orden = "DESC";
                }

                switch (SortExpression)
                {
                    case "DtRegistro":
                        if (Orden == "ASC")
                        {
                            historialPagosServices.lsHistorialPagosGridViewModel = historialPagosServices.lsHistorialPagosGridViewModel.OrderBy(x => x.DtRegistro).ToList();
                        }
                        else
                        {
                            historialPagosServices.lsHistorialPagosGridViewModel = historialPagosServices.lsHistorialPagosGridViewModel.OrderByDescending(x => x.DtRegistro).ToList();
                        }
                        break;
                    case "VchIdentificador":
                        if (Orden == "ASC")
                        {
                            historialPagosServices.lsHistorialPagosGridViewModel = historialPagosServices.lsHistorialPagosGridViewModel.OrderBy(x => x.VchIdentificador).ToList();
                        }
                        else
                        {
                            historialPagosServices.lsHistorialPagosGridViewModel = historialPagosServices.lsHistorialPagosGridViewModel.OrderByDescending(x => x.VchIdentificador).ToList();
                        }
                        break;
                    case "DcmSaldo":
                        if (Orden == "ASC")
                        {
                            historialPagosServices.lsHistorialPagosGridViewModel = historialPagosServices.lsHistorialPagosGridViewModel.OrderBy(x => x.DcmSaldo).ToList();
                        }
                        else
                        {
                            historialPagosServices.lsHistorialPagosGridViewModel = historialPagosServices.lsHistorialPagosGridViewModel.OrderByDescending(x => x.DcmSaldo).ToList();
                        }
                        break;
                    case "DcmOperacion":
                        if (Orden == "ASC")
                        {
                            historialPagosServices.lsHistorialPagosGridViewModel = historialPagosServices.lsHistorialPagosGridViewModel.OrderBy(x => x.DcmOperacion).ToList();
                        }
                        else
                        {
                            historialPagosServices.lsHistorialPagosGridViewModel = historialPagosServices.lsHistorialPagosGridViewModel.OrderByDescending(x => x.DcmOperacion).ToList();
                        }
                        break;
                    case "DcmNuevoSaldo":
                        if (Orden == "ASC")
                        {
                            historialPagosServices.lsHistorialPagosGridViewModel = historialPagosServices.lsHistorialPagosGridViewModel.OrderBy(x => x.DcmNuevoSaldo).ToList();
                        }
                        else
                        {
                            historialPagosServices.lsHistorialPagosGridViewModel = historialPagosServices.lsHistorialPagosGridViewModel.OrderByDescending(x => x.DcmNuevoSaldo).ToList();
                        }
                        break;
                }

                gvHistorial.DataSource = historialPagosServices.lsHistorialPagosGridViewModel;
                gvHistorial.DataBind();
            }
        }

        protected void gvHistorial_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvHistorial.PageIndex = e.NewPageIndex;
            gvHistorial.DataSource = historialPagosServices.lsHistorialPagosGridViewModel;
            gvHistorial.DataBind();
        }
    }
}