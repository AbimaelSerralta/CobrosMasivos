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
                Session["usuariosCompletosServices"] = usuariosCompletosServices;
            }
            else
            {
                usuariosCompletosServices = (UsuariosCompletosServices)Session["usuariosCompletosServices"];
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
            tarifasServices.CargarTarifas();
            lvTarifa.DataSource = tarifasServices.lsTarifasGridViewModel;
            lvTarifa.DataBind();

            txtVencimiento.Text = DateTime.Now.ToString("yyyy-MM-dd");

            lblTituloModal.Text = "<strong>Paso 1</strong> cantidad a comprar.";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);
        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            if (txtImporte.EmptyTextBox())
            {
                lblValidar.Text = "El campo Importe es obligatorio";
                return;
            }

            string id_company = "";
            string id_branch = "";
            string user = "";
            string pwd = "";
            string moneda = "";
            string canal = "";
            string semillaAES = "";
            string urlGen = "";
            string data0 = "";

            foreach (var item in usuariosCompletosServices.lsgvUsuariosSeleccionados)
            {
                string ReferenciaCobro = item.IdCliente.ToString() + item.IdUsuario.ToString() + DateTime.Now.ToString("ddMMyyyyHHmmssfff");
            }

            pnlSeleccion.Visible = false;
            pnlIframe.Visible = true;

            if (true)
            {
                ifrLiga.Src = GenLigaPara(id_company, id_branch, user, pwd, ReferenciaCobro, decimal.Parse(txtImporte.Text), moneda, canal, "C", item.StrCorreo, semillaAES, data0, urlGen);
            }
        }
    }
}