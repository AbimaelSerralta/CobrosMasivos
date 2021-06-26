using AjaxControlToolkit;
using ClosedXML.Excel;
using Franquicia.Bussiness;
using Franquicia.WebForms.Util;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using WebApplication1.Vista;

namespace Franquicia.WebForms.Views
{
    public partial class GenerarLigasMultiples : System.Web.UI.Page
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

        UsuariosCompletosServices usuariosCompletosServices = new UsuariosCompletosServices();
        PagosTarjetaServices pagosServices = new PagosTarjetaServices();
        CorreosServices correosServices = new CorreosServices();
        ImporteLigaMinMaxServices importeLigaMinMaxServices = new ImporteLigaMinMaxServices();
        ParametrosEntradaServices parametrosEntradaServices = new ParametrosEntradaServices();
        PromocionesServices promocionesServices = new PromocionesServices();
        ClienteCuentaServices clienteCuentaServices = new ClienteCuentaServices();
        TarifasServices tarifasServices = new TarifasServices();
        TicketsServices ticketsServices = new TicketsServices();
        ValidacionesServices validacionesServices = new ValidacionesServices();
        WhatsAppPendientesServices whatsAppPendientesServices = new WhatsAppPendientesServices();
        TelefonosUsuariosServices telefonosUsuariosServices = new TelefonosUsuariosServices();
        ParametrosTwiServices parametrosTwiServices = new ParametrosTwiServices();
        ComisionesTarjetasClientesServices comisionesTarjetasCl = new ComisionesTarjetasClientesServices();

        List<string> listPromocionesSeleccionados = new List<string>();

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
            if (Session["UidClienteMaster"] != null)
            {
                ViewState["UidClienteLocal"] = Session["UidClienteMaster"];
            }
            else
            {
                ViewState["UidClienteLocal"] = Guid.Empty;
            }

            clienteCuentaServices.ObtenerDineroCuentaCliente(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
            lblGvSaldo.Text = "Saldo: $ " + clienteCuentaServices.clienteCuentaRepository.clienteCuenta.DcmDineroCuenta.ToString("N2");

            if (!IsPostBack)
            {
                ViewState["gvUsuariosSeleccionados"] = SortDirection.Ascending;
                ViewState["SoExgvUsuariosSeleccionados"] = "";

                ViewState["gvUsuarios"] = SortDirection.Ascending;
                ViewState["SoExgvUsuarios"] = "";

                ViewState["listPromocionesSeleccionados"] = listPromocionesSeleccionados;

                btnCargarExcel.Attributes.Add("onclick", "document.getElementById('" + fuSelecionarExcel.ClientID + "').click(); return false;");
                fuSelecionarExcel.Attributes["onchange"] = "UploadFile(this)";

                Session["usuariosCompletosServices"] = usuariosCompletosServices;
                Session["importeLigaMinMaxServices"] = importeLigaMinMaxServices;
                Session["parametrosEntradaServices"] = parametrosEntradaServices;
                Session["promocionesServices"] = promocionesServices;
                Session["tarifasServices"] = tarifasServices;
                Session["comisionesTarjetasCl"] = comisionesTarjetasCl;

                promocionesServices.lsCBLPromocionesModelCliente.Clear();
                promocionesServices.CargarPromocionesClientes(Guid.Parse(ViewState["UidClienteLocal"].ToString()));

                parametrosEntradaServices.ObtenerParametrosEntradaClienteCM(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                importeLigaMinMaxServices.CargarImporteLigaMinMax();
                AsignarParametrosEntradaCliente();
            }
            else
            {
                usuariosCompletosServices = (UsuariosCompletosServices)Session["usuariosCompletosServices"];
                importeLigaMinMaxServices = (ImporteLigaMinMaxServices)Session["importeLigaMinMaxServices"];
                parametrosEntradaServices = (ParametrosEntradaServices)Session["parametrosEntradaServices"];
                promocionesServices = (PromocionesServices)Session["promocionesServices"];
                listPromocionesSeleccionados = (List<string>)ViewState["listPromocionesSeleccionados"];
                tarifasServices = (TarifasServices)Session["tarifasServices"];
                comisionesTarjetasCl = (ComisionesTarjetasClientesServices)Session["comisionesTarjetasCl"];

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Mult", String.Format(@"multi();"), true);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Mult", "multi()", true);

                pnlAlert.Visible = false;
                lblMensajeAlert.Text = "";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade");

                if (usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple.Count >= 1)
                {
                    gvUsuariosSeleccionados.DataSource = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple;
                    gvUsuariosSeleccionados.DataBind();
                }

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
        public void GenerarLiga()
        {
            int idUsuario = int.Parse(ViewState["IdUsuario"].ToString());
            DateTime thisDay = DateTime.Now;

            string j = thisDay.ToString("dd/MM/yyyy HH:mm:ss.fff");
            string o = j.Replace("/", "");
            string s = o.Replace(":", "");
            string u = s.Replace(".", "");
            string e = u.Replace(".", "");
            string Referencia = e.Replace(" ", "");

            Guid UidOrden = Guid.NewGuid();
            Guid UidOrdenPago = Guid.NewGuid();

            string ArchivoXml = "" +
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
                "<P>\r\n  " +
                "  <business>\r\n" +
                "    <id_company>Z937</id_company>\r\n" +
                "    <id_branch>851</id_branch>\r\n" +
                "    <user>Z937SIUS21</user>\r\n" +
                "    <pwd>7TYGNDWO48</pwd>\r\n" +
                "  </business>\r\n" +
                "  <url>\r\n" +
                "    <reference>" + idUsuario + Referencia + "</reference>\r\n" +
                "    <amount>" + txtNombre.Text + "</amount>\r\n" +
                "    <moneda>MXN</moneda>\r\n" +
                "    <canal>W</canal>\r\n" +
                "    <omitir_notif_default>1</omitir_notif_default>\r\n" +
                "    <st_correo>1</st_correo>\r\n" +
                "    <fh_vigencia>" + txtFecha.Text + "</fh_vigencia>\r\n" +
                "    <mail_cliente>" + txtCorreo.Text + "</mail_cliente>\r\n" +
                "    <datos_adicionales>\r\n" +
                "      <data id=\"1\" display=\"false\">\r\n" +
                "        <label>PRINCIPAL</label>\r\n" +
                "        <value>" + idUsuario + "</value>\r\n" +
                "      </data>\r\n" +
                "      <data id=\"2\" display=\"true\">\r\n" +
                "        <label>Concepto:</label>\r\n" +
                "        <value>" + "" + "</value>\r\n" +
                "      </data>\r\n" +
                "      <data id=\"3\" display=\"false\">\r\n" +
                "        <label>Color</label>\r\n" +
                "        <value>Azul</value>\r\n" +
                "      </data>\r\n" +
                "    </datos_adicionales>\r\n" +
                "  </url>\r\n" +
                "</P>\r\n";
            string originalString = ArchivoXml;
            //string key = "5DCC67393750523CD165F17E1EFADD21";
            string key = "7AACFE849FABD796F6DCB947FD4D5268";
            AESCrypto aesCrypto = new AESCrypto();
            string encryptedString = aesCrypto.encrypt(originalString, key);
            string finalString = encryptedString.Replace("%", "%25").Replace(" ", "%20").Replace("+", "%2B").Replace("=", "%3D").Replace("/", "%2F");

            string encodedString = HttpUtility.UrlEncode("<pgs><data0>9265655113</data0><data>" + encryptedString + "</data></pgs>");
            string postParam = "xml=" + encodedString;

            var client = new RestClient("(https://bc.mitec.com.mx/p/gen");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", postParam, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            var content = response.Content;

            //lblRespuesta.Text = o.decrypt(key, content);
            string decryptedString = aesCrypto.decrypt(key, content);
            string str1 = decryptedString.Replace("<P_RESPONSE><cd_response>success</cd_response><nb_response></nb_response><nb_url>", "");
            string url = str1.Replace("</nb_url></P_RESPONSE>", "");

            txtUrlGene.Text = url;
        }

        protected void btnGenerarLiga_Click(object sender, EventArgs e)
        {
            GenerarLiga();
        }

        protected void ddlUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            int f = usuariosCompletosServices.lsUsuariosCompletos.IndexOf(usuariosCompletosServices.lsUsuariosCompletos.Find(x => x.IdUsuario == int.Parse(ddlUsuario.SelectedValue.ToString())));

            txtCorreo.Text = usuariosCompletosServices.lsUsuariosCompletos[f].StrCorreo;
            ViewState["IdUsuario"] = usuariosCompletosServices.lsUsuariosCompletos[f].IdUsuario;
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            pagosServices.ObtenerEstatusLiga(txtConsultar.Text);

            gvMovimiento.DataSource = pagosServices.lsPagosTarjeta;
            gvMovimiento.DataBind();
        }

        protected void btnImportarUsuarios_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);
        }

        protected void btnImportarExcel_Click(object sender, EventArgs e)
        {
            if (fuSelecionarExcel.HasFile)
            {
                if (".xlsx" == Path.GetExtension(fuSelecionarExcel.FileName))
                {
                    try
                    {
                        byte[] buffer = new byte[fuSelecionarExcel.FileBytes.Length];
                        fuSelecionarExcel.FileContent.Seek(0, SeekOrigin.Begin);
                        fuSelecionarExcel.FileContent.Read(buffer, 0, Convert.ToInt32(fuSelecionarExcel.FileContent.Length));

                        Stream stream2 = new MemoryStream(buffer);

                        DataTable dt = new DataTable();
                        using (XLWorkbook workbook = new XLWorkbook(stream2))
                        {
                            IXLWorksheet sheet = workbook.Worksheet(1);
                            bool FirstRow = true;
                            string readRange = "1:1";
                            foreach (IXLRow row in sheet.RowsUsed())
                            {
                                //If Reading the First Row (used) then add them as column name  
                                if (FirstRow)
                                {
                                    //Checking the Last cellused for column generation in datatable  
                                    readRange = string.Format("{0}:{1}", 1, row.LastCellUsed().Address.ColumnNumber);
                                    foreach (IXLCell cell in row.Cells(readRange))
                                    {
                                        dt.Columns.Add(cell.Value.ToString());
                                    }
                                    FirstRow = false;
                                }
                                else
                                {
                                    //Adding a Row in datatable  
                                    dt.Rows.Add();
                                    int cellIndex = 0;
                                    //Updating the values of datatable  
                                    foreach (IXLCell cell in row.Cells(readRange))
                                    {
                                        dt.Rows[dt.Rows.Count - 1][cellIndex] = cell.Value.ToString();
                                        cellIndex++;
                                    }
                                }
                            }
                        }

                        if (dt.Columns.Contains("NOMBRE(S)".Trim()) && dt.Columns.Contains("APEPATERNO".Trim()) && dt.Columns.Contains("APEMATERNO".Trim())
                            && dt.Columns.Contains("CORREO".Trim()) && dt.Columns.Contains("CELULAR".Trim()) && dt.Columns.Contains("ASUNTO".Trim()) && dt.Columns.Contains("CONCEPTO".Trim())
                            && dt.Columns.Contains("IMPORTE".Trim()) && dt.Columns.Contains("VENCIMIENTO".Trim()) && dt.Columns.Contains("EMAIL".Trim()) && dt.Columns.Contains("WHATS".Trim())
                            && dt.Columns.Contains("SMS".Trim()) && dt.Columns.Contains("PROMOCION(ES)".Trim()))
                        {

                            usuariosCompletosServices.ValidarExcelToListMultiple(dt, promocionesServices.lsCBLPromocionesModelCliente, Guid.Parse(ViewState["UidClienteLocal"].ToString()));

                            if (usuariosCompletosServices.lsLigasInsertarMultiple.Count >= 1)
                            {
                                usuariosCompletosServices.ExcelToListMultiple(usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple, usuariosCompletosServices.lsLigasInsertarMultiple, Guid.Parse(ViewState["UidClienteLocal"].ToString()), URLBase);

                                #region EnviarWhats
                                foreach (var item in usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple)
                                {
                                    string NumberTo = item.StrTelefono.Split('(', ')')[2];
                                    string nombreTrun = string.Empty;
                                    string prefijo = item.StrTelefono.Split('(', ')')[1];

                                    if (prefijo == "+52")
                                    {
                                        prefijo = prefijo + "1";
                                    }

                                    string[] Descripcion = Regex.Split(item.StrNombre, " ");

                                    if (Descripcion.Length >= 2)
                                    {
                                        nombreTrun = Descripcion[0];
                                    }
                                    else
                                    {
                                        nombreTrun = item.StrNombre;
                                    }

                                    string body = "Hola " + nombreTrun + "," +
                                        "\r\n" + item.VchNombreComercial + " le agradece su registro." +
                                        "\r\n" + "Responda SI para activar las notificaciones";

                                    string EstaWhats = validacionesServices.EstatusWhatsApp(NumberTo);
                                    if (EstaWhats == "PENDIENTE")
                                    {
                                        //******Configuracion de Twilio******
                                        parametrosTwiServices.ObtenerParametrosTwi();
                                        string accountSid = parametrosTwiServices.parametrosTwiRepository.parametrosTwi.AccountSid;
                                        string authToken = parametrosTwiServices.parametrosTwiRepository.parametrosTwi.AuthToken;
                                        string NumberFrom = parametrosTwiServices.parametrosTwiRepository.parametrosTwi.NumberFrom;

                                        //string accountSid = "ACc7561cb09df3180ee1368e40055eedf5";
                                        //string authToken = "3f914e588826df9a93ed849cee73eae2";
                                        ////string NumberFrom = "+14158739087";
                                        //string NumberFrom = "+14155238886";

                                        try
                                        {
                                            tarifasServices.CargarTarifas();
                                            clienteCuentaServices.ObtenerDineroCuentaCliente(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                                            decimal DcmCuenta = clienteCuentaServices.clienteCuentaRepository.clienteCuenta.DcmDineroCuenta;

                                            decimal DcmWhatsapp = 0;
                                            foreach (var tariWhats in tarifasServices.lsTarifasGridViewModel)
                                            {
                                                DcmWhatsapp = tariWhats.DcmWhatsapp;
                                            }

                                            if (DcmCuenta >= DcmWhatsapp)
                                            {
                                                TwilioClient.Init(accountSid, authToken);

                                                var message = MessageResource.Create(
                                                body: body.Replace("\r", ""),
                                                from: new Twilio.Types.PhoneNumber("whatsapp:" + NumberFrom),
                                                to: new Twilio.Types.PhoneNumber("whatsapp:" + prefijo + NumberTo)
                                                //to: new Twilio.Types.PhoneNumber("whatsapp:+5219841651607")
                                                );

                                                string Id = telefonosUsuariosServices.ObtenerIdTelefono(NumberTo);
                                                telefonosUsuariosServices.ActualizarEstatusWhats(Guid.Parse(Id), Guid.Parse("C8A2C506-7655-4102-B987-D13AE3E25A66"));

                                                decimal NuevoSaldo = DcmCuenta - DcmWhatsapp;

                                                string Folio = item.IdCliente.ToString() + item.IdUsuario.ToString() + DateTime.Now.ToString("ddMMyyyyHHmmssfff");

                                                if (ticketsServices.RegistrarTicketPago(Folio, DcmWhatsapp, 0, DcmWhatsapp, "Pago de WhatsApp", Guid.Parse(ViewState["UidClienteLocal"].ToString()), DateTime.Now, 1, 0, 0, DcmCuenta, DcmWhatsapp, NuevoSaldo))
                                                {
                                                    clienteCuentaServices.ActualizarDineroCuentaCliente(NuevoSaldo, Guid.Parse(ViewState["UidClienteLocal"].ToString()), "");

                                                    clienteCuentaServices.ObtenerDineroCuentaCliente(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                                                    Master.GvSaldo.Text = "Saldo: $ " + clienteCuentaServices.clienteCuentaRepository.clienteCuenta.DcmDineroCuenta.ToString("N2");
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            //lblResumen.Text += "WhatsApp: " + ex.Message;
                                        }
                                    }
                                }
                                #endregion

                                foreach (var item in usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple)
                                {
                                    promocionesServices.lsLigasMultiplePromocionesModel.RemoveAll(x => x.IdUsuario == item.IdUsuario && x.IntAuxiliar == item.IntAuxiliar);

                                    foreach (var it in promocionesServices.lsCBLPromocionesModelCliente)
                                    {
                                        string[] arPromo = Regex.Split(item.StrPromociones, ",");

                                        for (int i = 0; i < arPromo.Length; i++)
                                        {
                                            if (it.VchDescripcion.Trim() == arPromo[i].Trim())
                                            {
                                                promocionesServices.PromocionesMultiplesTemporal(it.UidPromocion.ToString(), item.IdUsuario, it.VchDescripcion, item.IntAuxiliar);
                                            }
                                        }
                                    }
                                }

                                gvUsuariosSeleccionados.DataSource = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple;
                                gvUsuariosSeleccionados.DataBind();
                            }

                            if (usuariosCompletosServices.lsLigasErroresMultiple.Count >= 1)
                            {
                                btnDescargarError.Visible = true;
                                btnMasDetalle.Visible = false;
                                pnlAlertImportarError.Visible = true;
                                lblMnsjAlertImportarError.Text = "<strong>!Lo sentimos¡</strong> algunos usuarios no se han importado. **Recuerde que todos los campos son obligatorios**";
                                divAlertImportarError.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                            }
                        }
                        else
                        {
                            btnDescargarError.Visible = false;
                            btnMasDetalle.Visible = true;
                            pnlAlertImportarError.Visible = true;
                            lblMnsjAlertImportarError.Text = "<strong>!Lo sentimos¡</strong> el archivo no tiene las columnas correctas.";
                            divAlertImportarError.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                        }
                    }
                    catch (Exception ex)
                    {
                        pnlAlert.Visible = true;
                        lblMensajeAlert.Text = ex.Message;
                        divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                    }
                }
                else
                {
                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "Solo se admite los formatos xlsx";
                    divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                }
            }
        }
        protected void btnSeleccionar_Click(object sender, EventArgs e)
        {
            usuariosCompletosServices.CargarUsuariosFinalesMultiples(usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple, new Guid(ViewState["UidClienteLocal"].ToString()), new Guid("E39FF705-8A01-4302-829A-7CFB9615CC8F"));
            gvUsuarios.DataSource = usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel;
            gvUsuarios.DataBind();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalSeleccionar()", true);
        }

        protected void btnSimple_Click(object sender, EventArgs e)
        {
            lblTittleLigas.Text = "Ligas Simples";
            pnlTipoLigas.Visible = false;
            pnlSeleccionUsuarios.Visible = true;
        }

        protected void btnMultiple_Click(object sender, EventArgs e)
        {
            lblTittleLigas.Text = "Ligas Multiples";
            pnlTipoLigas.Visible = false;
            pnlSeleccionUsuarios.Visible = true;
        }

        protected void btnExportarLista_Click(object sender, EventArgs e)
        {
            Session["lsgvUsuariosSeleccionadosMultiple"] = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple;
            Session["lsLigasUsuariosGridViewModelErrorMultiple"] = null;

            string _open = "window.open('ExportarAExcelMultiple.aspx', '_blank');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
        }

        protected void cbSeleccionar_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            GridViewRow gr = (GridViewRow)checkBox.Parent.Parent;
            int dataKey = int.Parse(gvUsuarios.DataKeys[gr.RowIndex].Value.ToString());

            CheckBox cbSeleccionar = (CheckBox)gr.FindControl("cbSeleccionar");

            if (cbSeleccionar.Checked)
            {
                usuariosCompletosServices.ActualizarListaUsuariosMultiple(usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel, dataKey, true);
            }
            else
            {
                usuariosCompletosServices.ActualizarListaUsuariosMultiple(usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel, dataKey, false);
            }

            //gvUsuarios.DataSource = usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel;
            //gvUsuarios.DataBind();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //gvUsuariosSeleccionados.DataSource = usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel;
            //gvUsuariosSeleccionados.DataSource = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple;
            //gvUsuariosSeleccionados.DataBind();

            usuariosCompletosServices.gvUsuariosSeleccionadosMultiple(usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel);
            gvUsuariosSeleccionados.DataSource = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple;
            gvUsuariosSeleccionados.DataBind();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModalSeleccionar()", true);
        }

        protected void cbSeleccionado_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            GridViewRow gr = (GridViewRow)checkBox.Parent.Parent;
            int dataKey = int.Parse(gvUsuariosSeleccionados.DataKeys[gr.RowIndex].Value.ToString());

            CheckBox cbSeleccionado = (CheckBox)gr.FindControl("cbSeleccionado");

            if (cbSeleccionado.Checked)
            {
                usuariosCompletosServices.ActualizarListaUsuariosMultiple(usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel, dataKey, true);
            }
            else
            {
                usuariosCompletosServices.ActualizarListaUsuariosMultiple(usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel, dataKey, false);
            }

            gvUsuariosSeleccionados.DataSource = usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel.Where(x => x.blSeleccionado == true).ToList();
            gvUsuariosSeleccionados.DataBind();
        }

        protected void btnGenerarLigas_Click(object sender, EventArgs e)
        {
            ViewState["nuevoSaldo"] = 0;

            if (txtIdentificador.EmptyTextBox())
            {
                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "Para generar liga(s) multiple(s) el campo identificador es obligatorio.";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                return;
            }

            if (usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple.Count >= 1)
            {
                DateTime hoy = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"));
                DateTime hoyMas = DateTime.Parse(DateTime.Now.AddDays(89).ToString("dd/MM/yyyy"));

                bool Accion = true;
                bool Accion2 = false;
                bool Accion3 = false;
                bool Accion4 = false;

                foreach (GridViewRow row in gvUsuariosSeleccionados.Rows)
                {
                    TextBox txtGvAsunto = row.FindControl("txtGvAsunto") as TextBox;
                    TextBox txtGvConcepto = row.FindControl("txtGvConcepto") as TextBox;
                    TextBox txtGvImporte = row.FindControl("txtGvImporte") as TextBox;
                    TextBox txtGvVencimiento = row.FindControl("txtGvVencimiento") as TextBox;
                    CheckBox cbGvCorreo = row.FindControl("cbGvCorreo") as CheckBox;
                    CheckBox cbGvWa = row.FindControl("cbGvWa") as CheckBox;
                    CheckBox cbGvSms = row.FindControl("cbGvSms") as CheckBox;

                    DateTime date = DateTime.Parse(txtGvVencimiento.Text);
                    DateTime date2 = DateTime.Parse(txtGvVencimiento.Text);


                    if (txtGvAsunto.EmptyTextBox())
                    {
                        Accion = false;
                        txtGvAsunto.BackColor = Color.FromName("#f55145");
                    }

                    if (txtGvConcepto.EmptyTextBox())
                    {
                        Accion = false;
                        txtGvConcepto.BackColor = Color.FromName("#f55145");
                    }
                    if (txtGvImporte.EmptyTextBox())
                    {
                        Accion = false;
                        txtGvImporte.BackColor = Color.FromName("#f55145");
                    }

                    if (txtGvVencimiento.EmptyTextBox())
                    {
                        Accion = false;
                        txtGvVencimiento.BackColor = Color.FromName("#f55145");
                    }

                    if (decimal.Parse(txtGvImporte.Text) >= ImporteMin && decimal.Parse(txtGvImporte.Text) <= ImporteMax)
                    {

                    }
                    else
                    {
                        Accion = false;
                        Accion2 = true;
                        txtGvImporte.BackColor = Color.FromName("#f55145");
                    }

                    if (date >= hoy && date2 <= hoyMas)
                    {

                    }
                    else
                    {
                        Accion = false;
                        Accion2 = false;
                        Accion3 = true;
                        Accion4 = false;
                        txtGvVencimiento.BackColor = Color.FromName("#f55145");
                    }

                    if (cbGvCorreo.Checked == false && cbGvWa.Checked == false && cbGvSms.Checked == false)
                    {
                        Accion = false;
                        Accion2 = false;
                        Accion3 = false;
                        Accion4 = true;
                        break;
                    }
                }

                if (Accion)
                {
                    lblTituloModal.Text = "Selección de envio de  ligas";

                    tarifasServices.CargarTarifas();

                    decimal DcmWhatsapp = 0;
                    decimal DcmSms = 0;
                    decimal Cuenta = 0;
                    foreach (var item in tarifasServices.lsTarifasGridViewModel)
                    {
                        lblDcmWhatsapp.Text = item.DcmWhatsapp.ToString("N2");
                        DcmWhatsapp = item.DcmWhatsapp;
                        ViewState["item.DcmWhatsapp"] = item.DcmWhatsapp.ToString("N2");
                        lblDcmSms.Text = item.DcmSms.ToString("N2");
                        DcmSms = item.DcmSms;
                        ViewState["item.DcmSms"] = item.DcmSms.ToString("N2");
                    }

                    lblDcmCuenta.Text = clienteCuentaServices.clienteCuentaRepository.clienteCuenta.DcmDineroCuenta.ToString("N2");
                    lblDcmSaldo.Text = clienteCuentaServices.clienteCuentaRepository.clienteCuenta.DcmDineroCuenta.ToString("N2");
                    Cuenta = clienteCuentaServices.clienteCuentaRepository.clienteCuenta.DcmDineroCuenta;

                    //Cantidad de correos
                    lblCorreoUsado.Text = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple.Where(x => x.CBCorreo == true).Count().ToString();

                    CalcularSMS();
                    CalcularWhatsApp();

                    pnlAlertModal.Visible = false;
                    lblResumen.Text = "";
                    divAlertModal.Attributes.Add("class", "alert alert-success alert-dismissible fade");
                    btnGenerar.Visible = true;
                    btnGenerar.Visible = true;
                    btnCancelar.Visible = true;
                    btnAceptar2.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);
                }
                else
                {
                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "Para generar liga(s) multiple(s) es necesario corregir los siguientes errores: <br /> => Los campos en rojo son obligatorios.";
                    divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                }

                if (Accion2)
                {
                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "Para generar liga(s) multiple(s) es necesario corregir los siguientes errores: <br /> => El importe mínimo es de $" + ImporteMin.ToString("N2") + " y el máximo es de $" + ImporteMax.ToString("N2");
                    divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                }
                if (Accion3)
                {
                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "Para generar liga(s) multiple(s) es necesario corregir los siguientes errores: <br /> => La fecha mínima es el día de hoy y la máxima son 90 días.";
                    divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                }
                if (Accion4)
                {
                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "Para generar liga(s) multiple(s) es necesario corregir los siguientes errores: <br /> => Seleccione almenos uno de los tres tipos de envios.";
                    divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                }
            }
            else
            {
                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "Para generar liga(s) multiple(s) es necesario corregir los siguientes errores: <br /> => Agregue almenos un usuario.";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
            }

        }

        private void CalcularSMS()
        {
            int aUtilizar = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple.Where(x => x.CBWhatsApp == true).Count();
            decimal tarifaWA = decimal.Parse(lblDcmWhatsapp.Text);

            decimal tariUtizaWA = aUtilizar * tarifaWA;
            decimal cuenta = decimal.Parse(lblDcmCuenta.Text);

            decimal tarifaSms = decimal.Parse(lblDcmSms.Text);
            int aUtilizarSms = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple.Where(x => x.CBSms == true).Count();
            decimal tariUtizaSms = aUtilizarSms * tarifaSms;

            decimal sumWASm = tariUtizaWA + tariUtizaSms;

            decimal nuevoSaldo = cuenta - sumWASm;

            lblAUtilizarSms.Text = "(" + aUtilizarSms.ToString() + ") ";
            lblTotalUtilizarSms.Text = tariUtizaSms.ToString("N2");
            lblDcmSaldo.Text = nuevoSaldo.ToString("N2");
            ViewState["nuevoSaldo"] = nuevoSaldo.ToString("N2");

            lblAUtilizarWA.Text = "(" + aUtilizar.ToString() + ") ";
        }
        private void CalcularWhatsApp()
        {
            int permitido = 0;
            int pendiente = 0;

            foreach (var item in usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple.Where(x => x.CBWhatsApp == true))
            {
                string NumberVali = item.StrTelefono.Split('(', ')')[2];
                string EstaWhats = validacionesServices.EstatusWhatsApp(NumberVali);
                if (EstaWhats == "PERMITIDO")
                {
                    permitido++;
                }
                else
                {
                    pendiente++;
                }
            }


            //int aUtilizar = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple.Where(x => x.CBWhatsApp == true).Count();
            int aUtilizar = permitido;
            decimal tarifaWA = decimal.Parse(lblDcmWhatsapp.Text);

            decimal tariUtizaWA = aUtilizar * tarifaWA;
            decimal cuenta = decimal.Parse(lblDcmCuenta.Text);

            decimal tarifaSms = decimal.Parse(lblDcmSms.Text);
            int aUtilizarSms = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple.Where(x => x.CBSms == true).Count();
            decimal tariUtizaSms = aUtilizarSms * tarifaSms;

            decimal sumWASm = tariUtizaWA + tariUtizaSms;

            decimal nuevoSaldo = cuenta - sumWASm;

            lblAUtilizarWA.Text = "(" + aUtilizar.ToString() + ") ";
            lblTotalUtilizarWA.Text = tariUtizaWA.ToString("N2");
            lblDcmSaldo.Text = nuevoSaldo.ToString("N2");
            ViewState["nuevoSaldo"] = nuevoSaldo.ToString("N2");

            lblAUtilizarSms.Text = "(" + aUtilizarSms.ToString() + ") ";

            decimal pendient = pendiente * tarifaWA;
            lblPendienteWA.Text = "$" + pendient.ToString("N2");
        }

        public void LimpiarDatos()
        {
            txtIdentificador.Text = string.Empty;

            gvUsuariosSeleccionados.DataSource = null;
            gvUsuariosSeleccionados.DataBind();

            usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple.Clear();
            Session["lsgvUsuariosSeleccionadosMultiple"] = null;

            promocionesServices.lsLigasMultiplePromocionesModel.Clear();
        }

        protected void gvUsuarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUsuarios.PageIndex = e.NewPageIndex;
            gvUsuarios.DataSource = usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel;
            gvUsuarios.DataBind();
        }
        protected void gvUsuarios_Sorting(object sender, GridViewSortEventArgs e)
        {
            string SortExpression = e.SortExpression;
            ViewState["SoExgvUsuarios"] = e.SortExpression;
            SortDirection direccion;
            string Orden = string.Empty;

            if (ViewState["gvUsuarios"] != null)
            {
                direccion = (SortDirection)ViewState["gvUsuarios"];
                if (direccion == SortDirection.Ascending)
                {
                    ViewState["gvUsuarios"] = SortDirection.Descending;
                    Orden = "ASC";
                }
                else
                {
                    ViewState["gvUsuarios"] = SortDirection.Ascending;
                    Orden = "DESC";
                }

                switch (SortExpression)
                {
                    case "NombreCompleto":
                        if (Orden == "ASC")
                        {
                            usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel = usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel.OrderBy(x => x.NombreCompleto).ToList();
                        }
                        else
                        {
                            usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel = usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel.OrderByDescending(x => x.NombreCompleto).ToList();
                        }
                        break;
                    case "StrCorreo":
                        if (Orden == "ASC")
                        {
                            usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel = usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel.OrderBy(x => x.StrCorreo).ToList();
                        }
                        else
                        {
                            usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel = usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel.OrderByDescending(x => x.StrCorreo).ToList();
                        }
                        break;
                    case "StrTelefono":
                        if (Orden == "ASC")
                        {
                            usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel = usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel.OrderBy(x => x.StrTelefono).ToList();
                        }
                        else
                        {
                            usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel = usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel.OrderByDescending(x => x.StrTelefono).ToList();
                        }
                        break;
                }

                gvUsuarios.DataSource = usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel;
                gvUsuarios.DataBind();
            }
        }
        protected void gvUsuarios_RowCreated(object sender, GridViewRowEventArgs e)
        {
            SortDirection direccion = (SortDirection)ViewState["gvUsuarios"];
            string SortExpression = ViewState["SoExgvUsuarios"].ToString();

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

        protected void gvUsuariosSeleccionados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "btnEditar")
            {
                GridViewRow wor = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                TextBox GvAsunto = (TextBox)wor.FindControl("txtGvAsunto");
                ViewState["GvAsunto"] = GvAsunto.Text;
                TextBox GvConcepto = (TextBox)wor.FindControl("txtGvConcepto");
                ViewState["GvConcepto"] = GvConcepto.Text;
                TextBox GvImporte = (TextBox)wor.FindControl("txtGvImporte");
                ViewState["GvImporte"] = GvImporte.Text;
                TextBox GvVencimiento = (TextBox)wor.FindControl("txtGvVencimiento");
                //GvVencimiento.Attributes.Add("min", DateTime.Now.ToString("yyyy-MM-dd"));

                txtModVencimiento.Attributes.Add("min", DateTime.Now.ToString("yyyy-MM-dd"));
                txtModVencimiento.Attributes.Add("max", DateTime.Now.AddDays(89).ToString("yyyy-MM-dd"));
                ViewState["GvVencimiento"] = GvVencimiento.Text;

                //ListBox GvListBoxMultiple = (ListBox)wor.FindControl("ListBoxMultiple");
                //foreach (ListItem item in GvListBoxMultiple.Items)
                //{
                //    if (item.Selected)
                //    {
                //        listPromocionesSeleccionados.Add(item.Value);
                //    }
                //}
            }

            if (e.CommandName == "btnEditar")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvUsuariosSeleccionados.Rows[index];
                GridView valor = (GridView)sender;
                int dataKey = int.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());

                ViewState["IdUsuariosSeleccionadoEditar"] = dataKey;
                ViewState["indexSeleccionadoEditar"] = index;

                LinkButton btnAceptar = (LinkButton)row.FindControl("btnAceptar");
                LinkButton btnCancelar = (LinkButton)row.FindControl("btnCancelar");
                LinkButton btnEditar = (LinkButton)row.FindControl("btnEditar");
                LinkButton btnEliminar = (LinkButton)row.FindControl("btnEliminar");

                TextBox txtGvNombre = (TextBox)row.FindControl("txtGvNombre");
                TextBox txtGvCorreo = (TextBox)row.FindControl("txtGvCorreo");
                TextBox txtGvAsunto = (TextBox)row.FindControl("txtGvAsunto");
                TextBox txtGvConcepto = (TextBox)row.FindControl("txtGvConcepto");
                TextBox txtGvImporte = (TextBox)row.FindControl("txtGvImporte");
                TextBox txtGvVencimiento = (TextBox)row.FindControl("txtGvVencimiento");

                CheckBox cbGvCorreo = (CheckBox)row.FindControl("cbGvCorreo");
                CheckBox cbGvWa = (CheckBox)row.FindControl("cbGvWa");
                CheckBox cbGvSms = (CheckBox)row.FindControl("cbGvSms");

                Label lblGvAuxiliar = (Label)row.FindControl("lblGvAuxiliar");
                ViewState["lblGvAuxiliar"] = lblGvAuxiliar.Text;

                //cbGvCorreo.Enabled = true;
                //cbGvWa.Enabled = true;
                //cbGvSms.Enabled = true;

                //btnAceptar.Visible = true;
                //btnCancelar.Visible = true;
                //btnEditar.Visible = false;
                //btnEliminar.Visible = false;

                //txtGvAsunto.Enabled = true;
                //txtGvConcepto.Enabled = true;
                //txtGvImporte.Enabled = true;
                //txtGvVencimiento.Enabled = true;

                //txtGvAsunto.BorderStyle = BorderStyle.Solid;
                //txtGvConcepto.BorderStyle = BorderStyle.Solid;
                //txtGvImporte.BorderStyle = BorderStyle.Solid;
                //txtGvVencimiento.BorderStyle = BorderStyle.Solid;
                lblValidar.Text = string.Empty;
                txtModNombre.Text = txtGvNombre.Text;
                txtModCorreo.Text = txtGvCorreo.Text;
                txtModTelefono.Text = row.Cells[3].Text;
                txtModAsunto.Text = txtGvAsunto.Text;
                txtModConcepto.Text = txtGvConcepto.Text;
                txtModImporte.Text = txtGvImporte.Text;
                txtModVencimiento.Text = DateTime.Parse(txtGvVencimiento.Text).ToString("yyyy-MM-dd");
                cbModEmail.Checked = cbGvCorreo.Checked;
                cbModWa.Checked = cbGvWa.Checked;
                cbModSms.Checked = cbGvSms.Checked;

                if (promocionesServices.lsCBLPromocionesModelCliente.Count >= 1)
                {
                    ListBoxMultipleMod.DataSource = promocionesServices.lsCBLPromocionesModelCliente;
                    ListBoxMultipleMod.DataTextField = "VchDescripcion";
                    ListBoxMultipleMod.DataValueField = "UidPromocion";
                    ListBoxMultipleMod.DataBind();
                }
                else
                {
                    //pnlPromociones.Visible = false;
                }

                foreach (ListItem item in ListBoxMultipleMod.Items)
                {
                    foreach (var it in promocionesServices.lsLigasMultiplePromocionesModel)
                    {
                        if (it.IdUsuario == dataKey && it.IntAuxiliar == lblGvAuxiliar.Text)
                        {
                            if (item.Value == it.UidPromocion.ToString())
                            {
                                item.Selected = true;
                            }
                        }
                    }
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalDatosLiga()", true);
            }

            if (e.CommandName == "btnAceptar")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvUsuariosSeleccionados.Rows[index];
                GridView valor = (GridView)sender;
                int dataKey = int.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["UidUsuariosSeleccionadoEditar"] = dataKey;
                ListBox GvListBoxMultiple = (ListBox)row.FindControl("ListBoxMultiple");

                LinkButton btnAceptar = (LinkButton)row.FindControl("btnAceptar");
                LinkButton btnCancelar = (LinkButton)row.FindControl("btnCancelar");
                LinkButton btnEditar = (LinkButton)row.FindControl("btnEditar");
                LinkButton btnEliminar = (LinkButton)row.FindControl("btnEliminar");

                TextBox txtGvAsunto = (TextBox)row.FindControl("txtGvAsunto");
                TextBox txtGvConcepto = (TextBox)row.FindControl("txtGvConcepto");
                TextBox txtGvImporte = (TextBox)row.FindControl("txtGvImporte");
                TextBox txtGvVencimiento = (TextBox)row.FindControl("txtGvVencimiento");

                CheckBox cbGvCorreo = (CheckBox)row.FindControl("cbGvCorreo");
                CheckBox cbGvWa = (CheckBox)row.FindControl("cbGvWa");
                CheckBox cbGvSms = (CheckBox)row.FindControl("cbGvSms");

                cbGvCorreo.Enabled = false;
                cbGvWa.Enabled = false;
                cbGvSms.Enabled = false;

                btnAceptar.Visible = false;
                btnCancelar.Visible = false;
                btnEditar.Visible = true;
                btnEliminar.Visible = true;

                txtGvAsunto.Enabled = false;
                txtGvConcepto.Enabled = false;
                txtGvImporte.Enabled = false;
                txtGvVencimiento.Enabled = false;

                txtGvAsunto.BorderStyle = BorderStyle.None;
                txtGvConcepto.BorderStyle = BorderStyle.None;
                txtGvImporte.BorderStyle = BorderStyle.None;
                txtGvVencimiento.BorderStyle = BorderStyle.None;

                string Promociones = "";

                foreach (ListItem item in GvListBoxMultiple.Items)
                {
                    if (promocionesServices.lsLigasMultiplePromocionesModel.Exists(x => x.UidPromocion == Guid.Parse(item.Value) && x.IdUsuario == dataKey))
                    {
                        if (item.Selected == false)
                        {
                            promocionesServices.lsLigasMultiplePromocionesModel.RemoveAt(promocionesServices.lsLigasMultiplePromocionesModel.FindIndex(x => x.UidPromocion == Guid.Parse(item.Value) && x.IdUsuario == dataKey));
                        }
                    }
                    else
                    {
                        if (item.Selected == true)
                        {
                            listPromocionesSeleccionados.Add(item.Value);
                            promocionesServices.PromocionesMultiplesTemporal(item.Value, dataKey, item.Text, ViewState["lblGvAuxiliar"].ToString());
                        }
                    }
                }

                foreach (var item in promocionesServices.lsLigasMultiplePromocionesModel.Where(x => x.IdUsuario == dataKey).ToList())
                {
                    if (string.IsNullOrEmpty(Promociones))
                    {
                        Promociones = item.VchDescripcion;
                    }
                    else
                    {
                        Promociones += ", " + item.VchDescripcion;
                    }
                }

                usuariosCompletosServices.ActualizarListaGvUsuariosMultiple(usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple, dataKey, true, txtGvAsunto.Text, txtGvConcepto.Text, decimal.Parse(txtGvImporte.Text), DateTime.Parse(txtGvVencimiento.Text), Promociones, cbGvCorreo.Checked, cbGvSms.Checked, cbGvWa.Checked, index);
                gvUsuariosSeleccionados.DataSource = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple;
                gvUsuariosSeleccionados.DataBind();
            }

            if (e.CommandName == "btnCancelar")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvUsuariosSeleccionados.Rows[index];
                GridView valor = (GridView)sender;
                int dataKey = int.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());

                LinkButton btnAceptar = (LinkButton)row.FindControl("btnAceptar");
                LinkButton btnCancelar = (LinkButton)row.FindControl("btnCancelar");
                LinkButton btnEditar = (LinkButton)row.FindControl("btnEditar");
                LinkButton btnEliminar = (LinkButton)row.FindControl("btnEliminar");

                TextBox txtGvAsunto = (TextBox)row.FindControl("txtGvAsunto");
                TextBox txtGvConcepto = (TextBox)row.FindControl("txtGvConcepto");
                TextBox txtGvImporte = (TextBox)row.FindControl("txtGvImporte");
                TextBox txtGvVencimiento = (TextBox)row.FindControl("txtGvVencimiento");

                CheckBox cbGvCorreo = (CheckBox)row.FindControl("cbGvCorreo");
                CheckBox cbGvWa = (CheckBox)row.FindControl("cbGvWa");
                CheckBox cbGvSms = (CheckBox)row.FindControl("cbGvSms");

                cbGvCorreo.Enabled = false;
                cbGvWa.Enabled = false;
                cbGvSms.Enabled = false;

                txtGvAsunto.Text = ViewState["GvAsunto"].ToString();
                txtGvConcepto.Text = ViewState["GvConcepto"].ToString();
                txtGvImporte.Text = ViewState["GvImporte"].ToString();
                txtGvVencimiento.Text = ViewState["GvVencimiento"].ToString();

                btnAceptar.Visible = false;
                btnCancelar.Visible = false;
                btnEditar.Visible = true;
                btnEliminar.Visible = true;

                txtGvAsunto.Enabled = false;
                txtGvConcepto.Enabled = false;
                txtGvImporte.Enabled = false;
                txtGvVencimiento.Enabled = false;

                txtGvAsunto.BorderStyle = BorderStyle.None;
                txtGvConcepto.BorderStyle = BorderStyle.None;
                txtGvImporte.BorderStyle = BorderStyle.None;
                txtGvVencimiento.BorderStyle = BorderStyle.None;
            }

            if (e.CommandName == "btnEliminar")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvUsuariosSeleccionados.Rows[index];
                GridView valor = (GridView)sender;
                int dataKey = int.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());

                Label lblGvAuxiliar = (Label)row.FindControl("lblGvAuxiliar");

                //usuariosCompletosServices.ActualizarListaUsuariosMultiple(usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel, dataKey, false);
                //gvUsuariosSeleccionados.DataSource = usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel.Where(x => x.blSeleccionado == true).ToList();
                //gvUsuariosSeleccionados.DataBind();

                usuariosCompletosServices.EliminarItemgvUsuariosSeleccionadosMultiple(dataKey, index);
                promocionesServices.EliminarPromocionesMultiplesTemporal(dataKey, lblGvAuxiliar.Text);
                gvUsuariosSeleccionados.DataSource = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple;
                gvUsuariosSeleccionados.DataBind();
            }
        }
        protected void gvUsuariosSeleccionados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUsuariosSeleccionados.PageIndex = e.NewPageIndex;
            gvUsuariosSeleccionados.DataSource = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple;
            gvUsuariosSeleccionados.DataBind();
        }
        protected void gvUsuariosSeleccionados_Sorting(object sender, GridViewSortEventArgs e)
        {
            string SortExpression = e.SortExpression;
            ViewState["SoExgvUsuariosSeleccionados"] = e.SortExpression;
            SortDirection direccion;
            string Orden = string.Empty;

            if (ViewState["gvUsuariosSeleccionados"] != null)
            {
                direccion = (SortDirection)ViewState["gvUsuariosSeleccionados"];
                if (direccion == SortDirection.Ascending)
                {
                    ViewState["gvUsuariosSeleccionados"] = SortDirection.Descending;
                    Orden = "ASC";
                }
                else
                {
                    ViewState["gvUsuariosSeleccionados"] = SortDirection.Ascending;
                    Orden = "DESC";
                }

                switch (SortExpression)
                {
                    case "NombreCompleto":
                        if (Orden == "ASC")
                        {
                            usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple.OrderBy(x => x.NombreCompleto).ToList();
                        }
                        else
                        {
                            usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple.OrderByDescending(x => x.NombreCompleto).ToList();
                        }
                        break;
                    case "StrCorreo":
                        if (Orden == "ASC")
                        {
                            usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple.OrderBy(x => x.StrCorreo).ToList();
                        }
                        else
                        {
                            usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple.OrderByDescending(x => x.StrCorreo).ToList();
                        }
                        break;
                    case "StrTelefono":
                        if (Orden == "ASC")
                        {
                            usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple.OrderBy(x => x.StrTelefono).ToList();
                        }
                        else
                        {
                            usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple.OrderByDescending(x => x.StrTelefono).ToList();
                        }
                        break;
                    case "StrAsunto":
                        if (Orden == "ASC")
                        {
                            usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple.OrderBy(x => x.StrAsunto).ToList();
                        }
                        else
                        {
                            usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple.OrderByDescending(x => x.StrAsunto).ToList();
                        }
                        break;
                    case "StrConcepto":
                        if (Orden == "ASC")
                        {
                            usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple.OrderBy(x => x.StrConcepto).ToList();
                        }
                        else
                        {
                            usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple.OrderByDescending(x => x.StrConcepto).ToList();
                        }
                        break;
                    case "DcmImporte":
                        if (Orden == "ASC")
                        {
                            usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple.OrderBy(x => x.DcmImporte).ToList();
                        }
                        else
                        {
                            usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple.OrderByDescending(x => x.DcmImporte).ToList();
                        }
                        break;
                    case "DtVencimiento":
                        if (Orden == "ASC")
                        {
                            usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple.OrderBy(x => x.DtVencimiento).ToList();
                        }
                        else
                        {
                            usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple.OrderByDescending(x => x.DtVencimiento).ToList();
                        }
                        break;
                }

                gvUsuariosSeleccionados.DataSource = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple;
                gvUsuariosSeleccionados.DataBind();
            }
        }
        protected void gvUsuariosSeleccionados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvFranquiciatarios, "Select$" + e.Row.RowIndex);


                string DataKey = gvUsuariosSeleccionados.DataKeys[e.Row.RowIndex].Values[0].ToString();

                ListBox GvListBoxMultiple = (ListBox)e.Row.FindControl("ListBoxMultiple");
                Label GvlblPromociones = (Label)e.Row.FindControl("GvlblPromociones");
                Label lblGvAuxiliar = (Label)e.Row.FindControl("lblGvAuxiliar");
                ListBox GvlbPromociones = (ListBox)e.Row.FindControl("GvlbPromociones");

                if (promocionesServices.lsCBLPromocionesModelCliente.Count >= 1)
                {
                    GvListBoxMultiple.DataSource = promocionesServices.lsCBLPromocionesModelCliente;
                    GvListBoxMultiple.DataTextField = "VchDescripcion";
                    GvListBoxMultiple.DataValueField = "UidPromocion";
                    GvListBoxMultiple.DataBind();
                }
                else
                {
                    //pnlPromociones.Visible = false;
                }

                int Numero = e.Row.RowIndex;

                foreach (ListItem item in GvListBoxMultiple.Items)
                {
                    foreach (var it in promocionesServices.lsLigasMultiplePromocionesModel)
                    {
                        if (it.IdUsuario == int.Parse(DataKey))
                        {
                            if (item.Value == it.UidPromocion.ToString() && lblGvAuxiliar.Text == it.IntAuxiliar)
                            {
                                GvlblPromociones.Text += item.Text + ",";
                                //GvlbPromociones.Items.Add(item.Text);
                                item.Selected = true;
                            }
                        }
                    }
                }

                GvlblPromociones.Text = GvlblPromociones.Text.Replace(",", "</br>");

                if (Convert.ToBoolean((Numero % 2 == 0 ? true : false)))
                {
                    e.Row.BackColor = Color.FromName("#fceeff");
                }

            }
        }
        protected void gvUsuariosSeleccionados_RowCreated(object sender, GridViewRowEventArgs e)
        {
            SortDirection direccion = (SortDirection)ViewState["gvUsuariosSeleccionados"];
            string SortExpression = ViewState["SoExgvUsuariosSeleccionados"].ToString();

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
                            img.ImageUrl = "~/Images/SortingGv/" + (direccion == SortDirection.Ascending ? "descWhite" : "ascWhite") + ".png";
                            img.ImageAlign = ImageAlign.AbsMiddle;
                            // Le metemos un espacio delante de la imagen para que no se pegue al enlace
                            tc.Controls.Add(new LiteralControl(""));
                            tc.Controls.Add(img);
                        }
                    }
                }
            }
        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            string url = string.Empty;
            bool resu = false;

            //******Configuracion de Twilio******
            parametrosTwiServices.ObtenerParametrosTwi();
            string accountSid = parametrosTwiServices.parametrosTwiRepository.parametrosTwi.AccountSid;
            string authToken = parametrosTwiServices.parametrosTwiRepository.parametrosTwi.AuthToken;
            string NumberFrom = parametrosTwiServices.parametrosTwiRepository.parametrosTwi.NumberFrom;

            //string accountSid = "ACc7561cb09df3180ee1368e40055eedf5";
            //////string authToken = "0f47ce2d28c9211ac6a9ae42f630d1d6";
            //string authToken = "3f914e588826df9a93ed849cee73eae2";
            //////string NumberFrom = "+14158739087";
            //string NumberFrom = "+14155238886";

            if (decimal.Parse(ViewState["nuevoSaldo"].ToString()) >= 0)
            {
                if (!string.IsNullOrEmpty(id_company) && !string.IsNullOrEmpty(id_branch) && !string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(pwd) && !string.IsNullOrEmpty(moneda) && !string.IsNullOrEmpty(canal) && !string.IsNullOrEmpty(semillaAES) && !string.IsNullOrEmpty(urlGen) && !string.IsNullOrEmpty(data0))
                {
                    decimal DcmOperacionGeneral = 0;
                    string VchDescripcion = txtIdentificador.Text;

                    int intCorreo = 0;
                    int intWA = 0;
                    int intSMS = 0;

                    int ErrorCorreo = 0;
                    int ErrorSms = 0;
                    int ErrorWA = 0;
                    int PendienteWA = 0;

                    Guid UidHistorialPago = Guid.NewGuid();

                    foreach (var item in usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple)
                    {
                        decimal SubTotal = item.DcmImporte;
                        decimal ComisionTC = 0;
                        decimal ComisionP = 0;
                        decimal Total = 0;

                        //Calcula la comicion
                        comisionesTarjetasCl.CargarComisionesTarjetaCM(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                        if (comisionesTarjetasCl.lsComisionesTarjetasClientes.Count >= 1)
                        {
                            foreach (var itComi in comisionesTarjetasCl.lsComisionesTarjetasClientes)
                            {
                                if (itComi.BitComision)
                                {
                                    ComisionTC = itComi.DcmComision * SubTotal / (100 - itComi.DcmComision);
                                }
                            }
                        }

                        Total = SubTotal + ComisionTC;

                        bool VariasLigas = false;
                        Guid UidLigaAsociado = Guid.NewGuid();

                        if (promocionesServices.lsLigasMultiplePromocionesModel.Where(x => x.IdUsuario == item.IdUsuario && x.IntAuxiliar == item.IntAuxiliar).ToList().Count >= 1)
                        {
                            VariasLigas = true;
                        }

                        if (VariasLigas)
                        {
                            DateTime thisDay = DateTime.Now;
                            string ReferenciaCobro = item.IdCliente.ToString() + item.IdUsuario.ToString() + thisDay.ToString("ddMMyyyyHHmmssfff");
                            Guid UidLigaUrl = Guid.NewGuid();
                            string urlCobro = GenLigaPara(id_company, id_branch, user, pwd, ReferenciaCobro, Total, moneda, canal, "C", item.DtVencimiento, item.StrCorreo, item.StrConcepto, semillaAES, data0, urlGen);

                            if (urlCobro.Contains("https://"))
                            {
                                if (usuariosCompletosServices.GenerarLigasPagosTemp(UidLigaUrl, urlCobro, item.StrConcepto, SubTotal, ReferenciaCobro, item.UidUsuario, txtIdentificador.Text, thisDay, item.DtVencimiento, item.StrAsunto, UidLigaAsociado, Guid.Empty, Guid.Parse(ViewState["UidClienteLocal"].ToString()), ComisionTC, ComisionP, Total))
                                {
                                    resu = true;
                                    foreach (var itPromo in promocionesServices.lsLigasMultiplePromocionesModel.Where(x => x.IdUsuario == item.IdUsuario && x.IntAuxiliar == item.IntAuxiliar).ToList())
                                    {
                                        SuperPromocionesServices superPromocionesServices = new SuperPromocionesServices();
                                        superPromocionesServices.CargarSuperPromociones();

                                        foreach (var itSP in superPromocionesServices.lsCBLSuperPromociones)
                                        {
                                            if (itSP.UidPromocion == itPromo.UidPromocion)
                                            {

                                                int i = promocionesServices.lsCBLPromocionesModelCliente.IndexOf(promocionesServices.lsCBLPromocionesModelCliente.First(x => x.UidPromocion == itPromo.UidPromocion));

                                                if (SubTotal >= promocionesServices.lsCBLPromocionesModelCliente[i].DcmApartirDe)
                                                {
                                                    decimal cobro = promocionesServices.lsCBLPromocionesModelCliente[i].DcmComicion;

                                                    decimal Valor = cobro * Total / (100 - cobro);
                                                    decimal Importe = Valor + Total;

                                                    string promocion = itPromo.VchDescripcion.Replace(" MESES", "");

                                                    DateTime thisDay2 = DateTime.Now;
                                                    string Referencia = item.IdCliente.ToString() + item.IdUsuario.ToString() + thisDay2.ToString("ddMMyyyyHHmmssfff");

                                                    url = GenLigaPara(id_company, id_branch, user, pwd, Referencia, Importe, moneda, canal, promocion, item.DtVencimiento, item.StrCorreo, item.StrConcepto, semillaAES, data0, urlGen);

                                                    if (url.Contains("https://"))
                                                    {
                                                        if (usuariosCompletosServices.GenerarLigasPagos(url, item.StrConcepto, SubTotal, Referencia, item.UidUsuario, txtIdentificador.Text, thisDay, item.DtVencimiento, item.StrAsunto, UidLigaAsociado, itPromo.UidPromocion, Guid.Parse(ViewState["UidClienteLocal"].ToString()), ComisionTC, Valor, Importe))
                                                        {
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
                                        }
                                    }
                                }
                            }
                            else
                            {
                                resu = false;
                                break;
                            }

                            if (resu)
                            {
                                promocionesServices.CargarPromocionesValidas(UidLigaAsociado);

                                string strPromociones = string.Empty;
                                bool boolPromociones = false;

                                if (promocionesServices.lsLigasUrlsPromocionesModel.Count >= 1)
                                {
                                    foreach (var itPromo in promocionesServices.lsLigasUrlsPromocionesModel)
                                    {
                                        decimal promocion = int.Parse(itPromo.VchDescripcion.Replace(" MESES", ""));
                                        decimal Final = itPromo.DcmTotal / promocion;

                                        strPromociones +=
                                        "\t\t\t\t\t\t\t\t<tr>\r\n" +
                                        "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: right;\">\r\n" +
                                        "\t\t\t\t\t\t\t\t\t\t" + itPromo.VchDescripcion + " de $" + Final.ToString("N2") + "\r\n" +
                                        "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                                        "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: left;\">\r\n" +
                                        "\t\t\t\t\t\t\t\t\t\t &nbsp;" + "<a style =\"display:block;color:#fff;font-weight:400;text-align:center;width:100px;font-size:15px;text-decoration:none;background:#28a745;margin:0 auto; padding:5px;\" href=" + URLBase + "Views/Promociones.aspx?CodigoPromocion=" + UidLigaAsociado + "&CodigoLiga=" + itPromo.IdReferencia + "> Pagar $" + itPromo.DcmTotal.ToString("N2") + "</a>" + "\r\n" +
                                        "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                                        "\t\t\t\t\t\t\t\t</tr>\r\n";
                                    }
                                    boolPromociones = true;
                                }

                                string LigaUrl = URLBase + "Views/Promociones.aspx?CodigoPromocion=" + UidLigaAsociado + "&CodigoLiga=" + ReferenciaCobro;

                                if (item.CBCorreo)
                                {
                                    try
                                    {
                                        correosServices.CorreoLiga(item.NombreCompleto, item.StrAsunto, item.StrConcepto, Total, item.DtVencimiento, LigaUrl, item.StrCorreo, strPromociones, boolPromociones, item.VchNombreComercial);
                                        intCorreo = 1;
                                    }
                                    catch (Exception ex)
                                    {
                                        //lblResumen.Text += "Correo: " + ex.Message;
                                        int error = ErrorCorreo + 1;
                                        lblErrorCorreo.Text = error.ToString();
                                    }
                                }

                                #region ===>Generar WA y SMS<===
                                decimal DcmOperacion = 0;

                                string nombreTrun = string.Empty;

                                bool cbSms = false;
                                bool cbWhats = false;

                                string prefijo = item.StrTelefono.Split('(', ')')[1];
                                string NumberTo = item.StrTelefono.Split('(', ')')[2];

                                if (prefijo == "+52")
                                {
                                    prefijo = prefijo + "1";
                                }

                                string[] Descripcion = Regex.Split(item.StrNombre, " ");

                                if (Descripcion.Length >= 2)
                                {
                                    nombreTrun = Descripcion[0];
                                }
                                else
                                {
                                    nombreTrun = item.StrNombre;
                                }

                                if (nombreTrun.Count() >= 11)
                                {
                                    nombreTrun = nombreTrun.Remove(7) + "...";
                                }

                                string body = "Hola " + nombreTrun + "," +
                                    "\r\n" + item.VchNombreComercial + " le ha enviado su liga de pago:" +
                                    "\r\n" + "$" + Total.ToString("N2") + " https://cobrosmasivos.com/" + "Pago.aspx?Id=" + UidLigaUrl;

                                if (item.CBSms)
                                {
                                    try
                                    {
                                        TwilioClient.Init(accountSid, authToken);

                                        var message = MessageResource.Create(
                                    body: body,
                                    from: new Twilio.Types.PhoneNumber(NumberFrom),
                                    to: new Twilio.Types.PhoneNumber(prefijo + NumberTo)
                                    );

                                        string mnsj = message.Sid;

                                        DcmOperacion = DcmOperacion + decimal.Parse(ViewState["item.DcmSms"].ToString());
                                        DcmOperacionGeneral = DcmOperacionGeneral + decimal.Parse(ViewState["item.DcmSms"].ToString());
                                        //VchDescripcion = "Pago de SMS";
                                        intSMS = 1;

                                        cbSms = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        //lblResumen.Text += "SMS: " + ex.Message;
                                        cbSms = false;
                                        int error = ErrorSms + 1;
                                        lblErrorSms.Text = error.ToString();
                                    }
                                }

                                if (item.CBWhatsApp)
                                {
                                    string NumberVali = item.StrTelefono.Split('(', ')')[2];
                                    string EstaWhats = validacionesServices.EstatusWhatsApp(NumberVali);
                                    if (EstaWhats == "PERMITIDO")
                                    {
                                        try
                                        {
                                            TwilioClient.Init(accountSid, authToken);

                                            var message = MessageResource.Create(
                                            body: body.Replace("\r", ""),
                                            from: new Twilio.Types.PhoneNumber("whatsapp:" + NumberFrom),
                                            to: new Twilio.Types.PhoneNumber("whatsapp:" + prefijo + NumberTo)
                                        );

                                            string mnsj = message.Sid;

                                            DcmOperacion = DcmOperacion + decimal.Parse(ViewState["item.DcmWhatsapp"].ToString());
                                            DcmOperacionGeneral = DcmOperacionGeneral + decimal.Parse(ViewState["item.DcmWhatsapp"].ToString());

                                            //if (!string.IsNullOrEmpty(VchDescripcion))
                                            //{
                                            //    VchDescripcion += " y WhatsApp";
                                            //}
                                            //else
                                            //{
                                            //    VchDescripcion += "Pago de WhatsApp";
                                            //}

                                            intWA = 1;

                                            cbWhats = true;
                                        }
                                        catch (Exception ex)
                                        {
                                            //lblResumen.Text += "WhatsApp: " + ex.Message;
                                            cbWhats = false;
                                            int error = ErrorWA + 1;
                                            lblErrorWA.Text = error.ToString();
                                        }
                                    }
                                    else
                                    {
                                        whatsAppPendientesServices.RegistrarWhatsPendiente(body.Replace("\r\n", "[n]"), item.DtVencimiento, NumberVali, Guid.Parse(ViewState["UidClienteLocal"].ToString()), item.UidUsuario, UidHistorialPago, VchDescripcion);

                                        cbWhats = false;
                                        //int pendiente = PendienteWA + 1;
                                        //lblPendienteWA.Text = pendiente.ToString(); //Se quito por que se valida desde el principio
                                    }
                                }

                                if (cbSms || cbWhats)
                                {
                                    string Folio = item.IdCliente.ToString() + item.IdUsuario.ToString() + DateTime.Now.ToString("ddMMyyyyHHmmssfff");

                                    ticketsServices.RegistrarTicketPagoGeneral(UidHistorialPago, Folio, DcmOperacion, 0, DcmOperacion, VchDescripcion, Guid.Parse(ViewState["UidClienteLocal"].ToString()), DateTime.Now, intWA, intSMS, intCorreo, 0, 0, 0);
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            DateTime thisDay = DateTime.Now;
                            string ReferenciaCobro = item.IdCliente.ToString() + item.IdUsuario.ToString() + thisDay.ToString("ddMMyyyyHHmmssfff");

                            string urlCobro = GenLigaPara(id_company, id_branch, user, pwd, ReferenciaCobro, Total, moneda, canal, "C", item.DtVencimiento, item.StrCorreo, item.StrConcepto, semillaAES, data0, urlGen);

                            if (urlCobro.Contains("https://"))
                            {
                                Guid UidLigaUrl = Guid.NewGuid();

                                if (usuariosCompletosServices.GenerarLigasPagosTemp(UidLigaUrl, urlCobro, item.StrConcepto, SubTotal, ReferenciaCobro, item.UidUsuario, txtIdentificador.Text, thisDay, item.DtVencimiento, item.StrAsunto, Guid.Empty, Guid.Empty, Guid.Parse(ViewState["UidClienteLocal"].ToString()), ComisionTC, ComisionP, Total))
                                {
                                    if (item.CBCorreo)
                                    {
                                        try
                                        {
                                            correosServices.CorreoLiga(item.NombreCompleto, item.StrAsunto, item.StrConcepto, Total, item.DtVencimiento, urlCobro, item.StrCorreo, "", false, item.VchNombreComercial);
                                            intCorreo = 1;
                                        }
                                        catch (Exception ex)
                                        {
                                            //lblResumen.Text += "Correo: " + ex.Message;
                                            int error = ErrorCorreo + 1;
                                            lblErrorCorreo.Text = error.ToString();
                                        }
                                    }

                                    #region ===>Generar WA y SMS<===
                                    decimal DcmOperacion = 0;
                                    string nombreTrun = string.Empty;

                                    bool cbSms = false;
                                    bool cbWhats = false;

                                    string prefijo = item.StrTelefono.Split('(', ')')[1];
                                    string NumberTo = item.StrTelefono.Split('(', ')')[2];

                                    if (prefijo == "+52")
                                    {
                                        prefijo = prefijo + "1";
                                    }

                                    string[] Descripcion = Regex.Split(item.StrNombre, " ");

                                    if (Descripcion.Length >= 2)
                                    {
                                        nombreTrun = Descripcion[0];
                                    }
                                    else
                                    {
                                        nombreTrun = item.StrNombre;
                                    }

                                    if (nombreTrun.Count() >= 11)
                                    {
                                        nombreTrun = nombreTrun.Remove(7) + "...";
                                    }

                                    string body = "Hola " + nombreTrun + "," +
                                        "\r\n" + item.VchNombreComercial + " le ha enviado su liga de pago:" +
                                        "\r\n" + "$" + Total.ToString("N2") + " https://cobrosmasivos.com/" + "Pago.aspx?Id=" + UidLigaUrl;

                                    if (item.CBSms)
                                    {
                                        try
                                        {
                                            TwilioClient.Init(accountSid, authToken);

                                            var message = MessageResource.Create(
                                        body: body,
                                        from: new Twilio.Types.PhoneNumber(NumberFrom),
                                        to: new Twilio.Types.PhoneNumber(prefijo + NumberTo)
                                        );

                                            string mnsj = message.Sid;

                                            DcmOperacion = DcmOperacion + decimal.Parse(ViewState["item.DcmSms"].ToString());
                                            DcmOperacionGeneral = DcmOperacionGeneral + decimal.Parse(ViewState["item.DcmSms"].ToString());
                                            //VchDescripcion = "Pago de SMS";
                                            intSMS = 1;

                                            cbSms = true;
                                        }
                                        catch (Exception ex)
                                        {
                                            //lblResumen.Text += "SMS: " + ex.Message;
                                            cbSms = false;
                                            int error = ErrorSms + 1;
                                            lblErrorSms.Text = error.ToString();
                                        }
                                    }

                                    if (item.CBWhatsApp)
                                    {
                                        string NumberVali = item.StrTelefono.Split('(', ')')[2];
                                        string EstaWhats = validacionesServices.EstatusWhatsApp(NumberVali);
                                        if (EstaWhats == "PERMITIDO")
                                        {
                                            try
                                            {
                                                TwilioClient.Init(accountSid, authToken);

                                                var message = MessageResource.Create(
                                                body: body.Replace("\r", ""),
                                                from: new Twilio.Types.PhoneNumber("whatsapp:" + NumberFrom),
                                                to: new Twilio.Types.PhoneNumber("whatsapp:" + prefijo + NumberTo)
                                            );

                                                string mnsj = message.Sid;

                                                DcmOperacion = DcmOperacion + decimal.Parse(ViewState["item.DcmWhatsapp"].ToString());
                                                DcmOperacionGeneral = DcmOperacionGeneral + decimal.Parse(ViewState["item.DcmWhatsapp"].ToString());

                                                //if (!string.IsNullOrEmpty(VchDescripcion))
                                                //{
                                                //    VchDescripcion += " y WhatsApp";
                                                //}
                                                //else
                                                //{
                                                //    VchDescripcion += "Pago de WhatsApp";
                                                //}
                                                intWA = 1;

                                                cbWhats = true;
                                            }
                                            catch (Exception ex)
                                            {
                                                //lblResumen.Text += "WhatsApp: " + ex.Message;
                                                cbWhats = false;
                                                int error = ErrorWA + 1;
                                                lblErrorWA.Text = error.ToString();
                                            }
                                        }
                                        else
                                        {
                                            whatsAppPendientesServices.RegistrarWhatsPendiente(body.Replace("\r\n", "[n]"), item.DtVencimiento, NumberVali, Guid.Parse(ViewState["UidClienteLocal"].ToString()), item.UidUsuario, UidHistorialPago, VchDescripcion);

                                            cbWhats = false;
                                            //int pendiente = PendienteWA + 1;
                                            //lblPendienteWA.Text = pendiente.ToString(); //Se quito por que se valida desde el principio
                                        }
                                    }

                                    if (cbSms || cbWhats)
                                    {
                                        string Folio = item.IdCliente.ToString() + item.IdUsuario.ToString() + DateTime.Now.ToString("ddMMyyyyHHmmssfff");

                                        ticketsServices.RegistrarTicketPagoGeneral(UidHistorialPago, Folio, DcmOperacion, 0, DcmOperacion, VchDescripcion, Guid.Parse(ViewState["UidClienteLocal"].ToString()), DateTime.Now, intWA, intSMS, intCorreo, 0, 0, 0);
                                    }

                                    #endregion

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

                    if (resu)
                    {
                        if (cbSms.Checked || cbWhats.Checked)
                        {
                            clienteCuentaServices.ObtenerDineroCuentaCliente(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                            decimal DcmCuenta = clienteCuentaServices.clienteCuentaRepository.clienteCuenta.DcmDineroCuenta;

                            decimal NuevoSaldo = DcmCuenta - DcmOperacionGeneral;

                            if (ticketsServices.ActualizarTicketPagoGeneral(UidHistorialPago, DcmCuenta, DcmOperacionGeneral, NuevoSaldo))
                            {
                                clienteCuentaServices.ActualizarDineroCuentaCliente(NuevoSaldo, Guid.Parse(ViewState["UidClienteLocal"].ToString()), "");
                            }
                        }

                        lblResumen.Text = " Se han generado: " + usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple.Count.ToString() + " liga(s) exitosamente.";
                        lblCorreoUsado.Text = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple.Count.ToString();

                        //Habilitar Columna de error
                        thError.Visible = true;
                        tdErrorCorreo.Visible = true;
                        tdErrorSms.Visible = true;
                        tdErrorWA.Visible = true;

                        LimpiarDatos();

                        clienteCuentaServices.ObtenerDineroCuentaCliente(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                        Master.GvSaldo.Text = "Saldo: $ " + clienteCuentaServices.clienteCuentaRepository.clienteCuenta.DcmDineroCuenta.ToString("N2");

                        pnlAlertModal.Visible = true;
                        divAlertModal.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                        lblTituloModal.Text = "Resumen de envio de ligas";
                        btnGenerar.Visible = false;
                        btnCancelar.Visible = false;
                        btnAceptar2.Visible = true;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(url))
                        {
                            pnlAlertModal.Visible = true;
                            lblResumen.Text = "<b>¡Lo sentimos! </b> " + url + "." + "<br /> Las credenciales proporcionadas no son correctos, por favor contacte a los administradores.";
                            divAlertModal.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                        }
                        else
                        {
                            pnlAlertModal.Visible = true;
                            lblResumen.Text = "<b>¡Lo sentimos! </b> Las credenciales proporcionadas no son correctos, por favor contacte a los administradores.";
                            divAlertModal.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                        }
                    }
                }
                else
                {
                    pnlAlertModal.Visible = true;
                    lblResumen.Text = "<b>¡Lo sentimos! </b> Esta empresa no cuenta con credenciales para generar ligas, por favor contacte a los administradores.";
                    divAlertModal.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                }
            }
            else
            {
                pnlAlertModal.Visible = true;
                lblResumen.Text = "El saldo no es suficiente para enviar la(s) liga(s) por WhatsApp y/o SMS.";
                divAlertModal.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
            }
        }
        private string GenLigaPara(string id_company, string id_branch, string user, string pwd, string Referencia, decimal Importe,
            string moneda, string canal, string promocion, DateTime Vencimiento, string Correo, string Concepto, string semillaAES, string data0, string urlGen)
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
                "    <fh_vigencia>" + Vencimiento.ToString("dd/MM/yyyy") + "</fh_vigencia>\r\n" +
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

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
        }
        protected void btnAceptar2_Click(object sender, EventArgs e)
        {
            pnlAlertModal.Visible = false;
            lblResumen.Text = "";
            divAlertModal.Attributes.Add("class", "alert alert-success alert-dismissible fade");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
        }

        protected void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            FiltroNombre.Text = string.Empty;
            FiltroApePaterno.Text = string.Empty;
            FiltroApeMaterno.Text = string.Empty;
            FiltroCorreo.Text = string.Empty;
            FiltroTelefono.Text = string.Empty;
        }

        protected void btnBuscarUsuarios_Click(object sender, EventArgs e)
        {
            usuariosCompletosServices.BuscarUsuarios(usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel, new Guid(ViewState["UidClienteLocal"].ToString()), new Guid("E39FF705-8A01-4302-829A-7CFB9615CC8F"), FiltroNombre.Text, FiltroApePaterno.Text, FiltroApeMaterno.Text, FiltroCorreo.Text, FiltroTelefono.Text);
            gvUsuarios.DataSource = usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel.Where(x => x.blSeleccionado == false).ToList();
            gvUsuarios.DataBind();
        }

        protected void btnReiniciar_Click(object sender, EventArgs e)
        {
            LimpiarDatos();
        }

        protected void btnDescargarError_Click(object sender, EventArgs e)
        {
            Session["lsgvUsuariosSeleccionadosMultiple"] = null;
            Session["lsLigasUsuariosGridViewModelErrorMultiple"] = usuariosCompletosServices.lsLigasErroresMultiple;

            string _open = "window.open('ExportarAExcelMultiple.aspx', '_blank');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
        }

        protected void btnMasDetalle_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalMasDetalle()", true);
        }

        protected void btnCloseAlertImportarError_Click(object sender, EventArgs e)
        {
            pnlAlertImportarError.Visible = false;
            lblMnsjAlertImportarError.Text = "";
            divAlertImportarError.Attributes.Add("class", "alert alert-danger alert-dismissible fade");
            Session["lsLigasUsuariosGridViewModelErrorMultiple"] = null;
        }

        protected void BTNdEs_Click(object sender, EventArgs e)
        {
            //correosServices.CorreoCadena("finalString " + txtCadena.Text, "serralta2008@gmail.com");

            //string key = "5DCC67393750523CD165F17E1EFADD21";
            string key = "7AACFE849FABD796F6DCB947FD4D5268";
            AESCrypto o = new AESCrypto();
            string decryptedString = o.decrypt(key, txtCadena.Text);
            if (!string.IsNullOrEmpty(decryptedString))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(decryptedString);

                XmlNodeList RespuestaWebPayPlus = doc.DocumentElement.SelectNodes("//CENTEROFPAYMENTS");
                string reference = string.Empty;
                string response = string.Empty;
                string foliocpagos = string.Empty;
                string auth = string.Empty;
                string cc_type = string.Empty;
                string tp_operation = string.Empty;
                string cc_number = string.Empty;
                string amount = string.Empty;
                string fecha = string.Empty;
                string Hora = string.Empty;
                string nb_company = string.Empty;
                string bn_merchant = string.Empty;
                string id_url = string.Empty;
                string cd_error = string.Empty;
                string nb_error = string.Empty;
                string cc_mask = string.Empty;
                DateTime DtFechaOperacion = DateTime.Now;

                for (int i = 0; i < RespuestaWebPayPlus[0].ChildNodes.Count; i++)
                {
                    switch (RespuestaWebPayPlus[0].ChildNodes[i].Name)
                    {
                        case "nb_company":
                            nb_company = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        case "bn_merchant":
                            bn_merchant = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        case "id_url":
                            id_url = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        case "nb_error":
                            nb_error = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        case "cd_error":
                            cd_error = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        case "reference":
                            reference = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        case "response":
                            response = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        case "foliocpagos":
                            foliocpagos = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        case "auth":
                            auth = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        case "date":
                            fecha = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        case "time":
                            Hora = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        case "cc_type":
                            cc_type = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        case "tp_operation":
                            tp_operation = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;

                        case "cc_number":
                            cc_number = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        case "cc_mask":
                            cc_mask = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        case "amount":
                            amount = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        default:
                            break;
                    }
                }
                PagosServices pagosServices = new PagosServices();

                DateTime fechaRegistro = DateTime.MinValue;
                switch (response)
                {
                    case "denied":
                        fechaRegistro = DateTime.Now;

                        cc_type = "denied";
                        auth = "denied";
                        tp_operation = "denied";
                        amount = "0";
                        break;
                    case "approved":
                        fechaRegistro = DateTime.Now;
                        string fecha1 = DateTime.Parse(fecha + " " + Hora).ToString("dd/MM/yyyy HH:mm:ss");
                        DtFechaOperacion = DateTime.Parse(fecha1);
                        //correosServices.CorreoCadena("FechaEnviar " + DtFechaOperacion, "serralta2008@gmail.com");
                        break;
                    case "error":
                        fechaRegistro = DateTime.Now;
                        cc_type = "error";
                        auth = "error";
                        tp_operation = "error";
                        amount = "0";
                        break;
                }
                pagosServices.AgregarInformacionTarjeta(auth, reference, fechaRegistro, response, cc_type, tp_operation, nb_company, bn_merchant, id_url, cd_error, nb_error, cc_number, cc_mask, foliocpagos, decimal.Parse(amount), DtFechaOperacion);
            }
            else
            {
                //respuesta.Data = "Lo sentimos, no hemos podido desifrar la cadena. " + cadena;
            }
        }

        

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string Promociones = "";
            DateTime date = DateTime.Parse(txtModVencimiento.Text);
            DateTime hoy = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"));
            DateTime hoyMas = DateTime.Parse(DateTime.Now.AddDays(89).ToString("dd/MM/yyyy"));
            DateTime date2 = DateTime.Parse(txtModVencimiento.Text);
            int dataKey = int.Parse(ViewState["IdUsuariosSeleccionadoEditar"].ToString());

            if (txtModAsunto.EmptyTextBox())
            {
                lblValidar.Text = "El campo Asunto es obligatorio";
                return;
            }
            if (txtModConcepto.EmptyTextBox())
            {
                lblValidar.Text = "El campo Concepto es obligatorio";
                return;
            }
            if (txtModImporte.EmptyTextBox())
            {
                lblValidar.Text = "El campo Importe es obligatorio";
                return;
            }

            if (decimal.Parse(txtModImporte.Text) >= ImporteMin && decimal.Parse(txtModImporte.Text) <= ImporteMax)
            {

            }
            else
            {
                txtModImporte.BackColor = System.Drawing.Color.FromName("#f2dede");
                lblValidar.Text = "El importe mínimo es de $" + ImporteMin.ToString("N2") + " y el máximo es de $" + ImporteMax.ToString("N2");
                return;
            }

            if (txtModVencimiento.EmptyTextBox())
            {
                lblValidar.Text = "El campo Vencimiento es obligatorio";
                return;
            }

            if (date >= hoy && date2 <= hoyMas)
            {

            }
            else
            {
                txtModVencimiento.BackColor = System.Drawing.Color.FromName("#f2dede");
                lblValidar.Text = "La fecha mínima es el día de hoy y la máxima son 90 días.";
                return;
            }

            if (cbModEmail.Checked == false && cbModWa.Checked == false && cbModSms.Checked == false)
            {
                lblValidar.Text = "Por favor, seleccione almenos uno de los tres tipos de envios.";
                return;
            }

            foreach (ListItem item in ListBoxMultipleMod.Items)
            {
                if (promocionesServices.lsLigasMultiplePromocionesModel.Exists(x => x.UidPromocion == Guid.Parse(item.Value) && x.IdUsuario == dataKey && x.IntAuxiliar == ViewState["lblGvAuxiliar"].ToString()))
                {
                    if (item.Selected == false)
                    {
                        promocionesServices.lsLigasMultiplePromocionesModel.RemoveAt(promocionesServices.lsLigasMultiplePromocionesModel.FindIndex(x => x.UidPromocion == Guid.Parse(item.Value) && x.IdUsuario == dataKey && x.IntAuxiliar == ViewState["lblGvAuxiliar"].ToString()));
                    }
                }
                else
                {
                    if (item.Selected == true)
                    {
                        listPromocionesSeleccionados.Add(item.Value);
                        promocionesServices.PromocionesMultiplesTemporal(item.Value, dataKey, item.Text, ViewState["lblGvAuxiliar"].ToString());
                    }
                }
            }

            foreach (var item in promocionesServices.lsLigasMultiplePromocionesModel.Where(x => x.IdUsuario == dataKey && x.IntAuxiliar == ViewState["lblGvAuxiliar"].ToString()).ToList())
            {
                if (string.IsNullOrEmpty(Promociones))
                {
                    Promociones = item.VchDescripcion;
                }
                else
                {
                    Promociones += ", " + item.VchDescripcion;
                }
            }

            usuariosCompletosServices.ActualizarListaGvUsuariosMultiple(usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple, dataKey, true, txtModAsunto.Text, txtModConcepto.Text, decimal.Parse(txtModImporte.Text), DateTime.Parse(txtModVencimiento.Text), Promociones, cbModEmail.Checked, cbModSms.Checked, cbModWa.Checked, int.Parse(ViewState["indexSeleccionadoEditar"].ToString()));
            gvUsuariosSeleccionados.DataSource = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple;
            gvUsuariosSeleccionados.DataBind();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModalDatosLiga()", true);
        }
    }
}