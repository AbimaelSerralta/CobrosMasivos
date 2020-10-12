using ClosedXML.Excel;
using Franquicia.Bussiness;
using Franquicia.Domain.ViewModels;
using Franquicia.WebForms.Util;
using OfficeOpenXml;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using WebApplication1.Vista;

namespace Franquicia.WebForms.Views
{
    public partial class GenerarLigas : System.Web.UI.Page
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
        ParametrosEntradaServices parametrosEntradaServices = new ParametrosEntradaServices();
        PromocionesServices promocionesServices = new PromocionesServices();
        ClienteCuentaServices clienteCuentaServices = new ClienteCuentaServices();
        TarifasServices tarifasServices = new TarifasServices();
        TicketsServices ticketsServices = new TicketsServices();
        ValidacionesServices validacionesServices = new ValidacionesServices();
        WhatsAppPendientesServices whatsAppPendientesServices = new WhatsAppPendientesServices();
        TelefonosUsuariosServices telefonosUsuariosServices = new TelefonosUsuariosServices();
        ParametrosTwiServices parametrosTwiServices = new ParametrosTwiServices();

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

            txtVencimiento.Attributes.Add("min", DateTime.Now.ToString("yyyy-MM-dd"));
            txtVencimiento.Attributes.Add("max", DateTime.Now.AddDays(89).ToString("yyyy-MM-dd"));

            clienteCuentaServices.ObtenerDineroCuentaCliente(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
            lblGvSaldo.Text = "Saldo: $ " + clienteCuentaServices.clienteCuentaRepository.clienteCuenta.DcmDineroCuenta.ToString("N2");

            if (!IsPostBack)
            {
                ////PRUEBA
                //if (Session["PosbackExcelSimple"] != null)
                //{
                //    List<LigasUsuariosGridViewModel> lsRecovery = (List<LigasUsuariosGridViewModel>)Session["PosbackExcelSimple"];
                //    gvUsuariosSeleccionados.DataSource = lsRecovery;
                //    gvUsuariosSeleccionados.DataBind();

                //    Session["PosbackExcelSimple"] = null;
                //}


                ViewState["gvUsuariosSeleccionados"] = SortDirection.Ascending;
                ViewState["gvUsuarios"] = SortDirection.Ascending;

                btnCargarExcel.Attributes.Add("onclick", "document.getElementById('" + fuSelecionarExcel.ClientID + "').click(); return false;");
                fuSelecionarExcel.Attributes["onchange"] = "UploadFile(this)";

                Session["usuariosCompletosServices"] = usuariosCompletosServices;
                Session["parametrosEntradaServices"] = parametrosEntradaServices;
                Session["promocionesServices"] = promocionesServices;
                Session["tarifasServices"] = tarifasServices;
                Session["validacionesServices"] = validacionesServices;
                Session["telefonosUsuariosServices"] = telefonosUsuariosServices;

                promocionesServices.lsCBLPromocionesModelCliente.Clear();
                promocionesServices.CargarPromocionesClientes(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                if (promocionesServices.lsCBLPromocionesModelCliente.Count >= 1)
                {
                    cblPromociones.DataSource = promocionesServices.lsCBLPromocionesModelCliente;
                    cblPromociones.DataTextField = "VchDescripcion";
                    cblPromociones.DataValueField = "UidPromocion";
                    cblPromociones.DataBind();

                    ListBoxSimple.DataSource = promocionesServices.lsCBLPromocionesModelCliente;
                    ListBoxSimple.DataTextField = "VchDescripcion";
                    ListBoxSimple.DataValueField = "UidPromocion";
                    ListBoxSimple.DataBind();
                }
                else
                {
                    pnlPromociones.Visible = false;
                }

                //usuariosCompletosServices.CargarUsuariosFinalesPRUEBA(new Guid(ViewState["UidClienteLocal"].ToString()), new Guid("E39FF705-8A01-4302-829A-7CFB9615CC8F"));
                //gvUsuariosSeleccionados.DataSource = usuariosCompletosServices.lsLigasUsuariosGridViewModel;
                //gvUsuariosSeleccionados.DataBind();

                //usuariosCompletosServices.CargarUsuariosFinalesPRUEBA(new Guid(ViewState["UidClienteLocal"].ToString()), new Guid("E39FF705-8A01-4302-829A-7CFB9615CC8F"));
                //ddlUsuario.DataSource = usuariosCompletosServices.lsUsuariosCompletos;
                //ddlUsuario.DataTextField = "NombreCompleto";
                //ddlUsuario.DataValueField = "IdUsuario";
                //ddlUsuario.DataBind();
            }
            else
            {
                usuariosCompletosServices = (UsuariosCompletosServices)Session["usuariosCompletosServices"];
                parametrosEntradaServices = (ParametrosEntradaServices)Session["parametrosEntradaServices"];
                promocionesServices = (PromocionesServices)Session["promocionesServices"];
                tarifasServices = (TarifasServices)Session["tarifasServices"];
                validacionesServices = (ValidacionesServices)Session["validacionesServices"];
                telefonosUsuariosServices = (TelefonosUsuariosServices)Session["telefonosUsuariosServices"];

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Mult", String.Format(@"multi();"), true);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Mult", "multi()", true);
                //pnlAlertImportarError.Visible = false;
                //lblMnsjAlertImportarError.Text = "";
                //divAlertImportarError.Attributes.Add("class", "alert alert-danger alert-dismissible fade");

                pnlAlert.Visible = false;
                lblMensajeAlert.Text = "";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade");

                pnlAlertModal.Visible = false;
                lblResumen.Text = "";
                divAlertModal.Attributes.Add("class", "alert alert-danger alert-dismissible fade");
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
                "        <value>" + txtConcepto.Text + "</value>\r\n" +
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
                            && dt.Columns.Contains("CORREO".Trim()) && dt.Columns.Contains("CELULAR".Trim()))
                        {
                            usuariosCompletosServices.ValidarExcelToList(dt);

                            if (usuariosCompletosServices.lsLigasInsertar.Count >= 1)
                            {
                                usuariosCompletosServices.ExcelToList(usuariosCompletosServices.lsgvUsuariosSeleccionados, usuariosCompletosServices.lsLigasInsertar, Guid.Parse(ViewState["UidClienteLocal"].ToString()), URLBase);

                                #region EnviarWhats
                                foreach (var item in usuariosCompletosServices.lsgvUsuariosSeleccionados)
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
                                gvUsuariosSeleccionados.DataSource = usuariosCompletosServices.lsgvUsuariosSeleccionados;
                                gvUsuariosSeleccionados.DataBind();

                                //Session["PosbackExcelSimple"] = usuariosCompletosServices.lsgvUsuariosSeleccionados;
                            }

                            if (usuariosCompletosServices.lsLigasErrores.Count >= 1)
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

                        //Response.Write("");
                        //Page.Response.Redirect(Page.Request.Url.ToString(), true); RESUELVE TEMPORALMENTE EL LOADING(REVISAR)
                        //StringBuilder sbScript = new StringBuilder();
                        //sbScript.Append("<script language='JavaScript' type='text/javascript'>\n");
                        //sbScript.Append("<!--\n");
                        //sbScript.Append(this.GetPostBackEventReference(this, "PBArg") + ";\n");
                        //sbScript.Append("// -->\n");
                        //sbScript.Append("</script>\n");
                        //this.RegisterStartupScript("AutoPostBackScript", sbScript.ToString());
                        //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "DoPostBack", "__doPostBack(sender, e)", true);
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
            usuariosCompletosServices.CargarUsuariosFinales(usuariosCompletosServices.lsgvUsuariosSeleccionados, new Guid(ViewState["UidClienteLocal"].ToString()), new Guid("E39FF705-8A01-4302-829A-7CFB9615CC8F"));
            gvUsuarios.DataSource = usuariosCompletosServices.lsLigasUsuariosGridViewModel;
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
            Session["lsgvUsuariosSeleccionados"] = usuariosCompletosServices.lsgvUsuariosSeleccionados;
            Session["lsLigasUsuariosGridViewModelError"] = null;
            string _open = "window.open('ExportarAExcel.aspx', '_blank');";
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
                usuariosCompletosServices.ActualizarListaUsuarios(usuariosCompletosServices.lsLigasUsuariosGridViewModel, dataKey, true);
            }
            else
            {
                usuariosCompletosServices.ActualizarListaUsuarios(usuariosCompletosServices.lsLigasUsuariosGridViewModel, dataKey, false);
            }

            //gvUsuarios.DataSource = usuariosCompletosServices.lsLigasUsuariosGridViewModel;
            //gvUsuarios.DataBind();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            usuariosCompletosServices.gvUsuariosSeleccionados(usuariosCompletosServices.lsLigasUsuariosGridViewModel);
            gvUsuariosSeleccionados.DataSource = usuariosCompletosServices.lsgvUsuariosSeleccionados;
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
                usuariosCompletosServices.ActualizarListaUsuarios(usuariosCompletosServices.lsLigasUsuariosGridViewModel, dataKey, true);
            }
            else
            {
                usuariosCompletosServices.ActualizarListaUsuarios(usuariosCompletosServices.lsLigasUsuariosGridViewModel, dataKey, false);
            }

            gvUsuariosSeleccionados.DataSource = usuariosCompletosServices.lsLigasUsuariosGridViewModel.Where(x => x.blSeleccionado == true).ToList();
            gvUsuariosSeleccionados.DataBind();
        }

        protected void btnGenerarLigas_Click(object sender, EventArgs e)
        {
            DateTime fecha;

            if (DateTime.TryParse(txtVencimiento.Text, out fecha))
            {
                txtVencimiento.Text = fecha.ToString("yyyy-MM-dd");
            }
            else
            {
                txtVencimiento.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }

            DateTime date = DateTime.Parse(txtVencimiento.Text);
            DateTime hoy = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"));
            DateTime hoyMas = DateTime.Parse(DateTime.Now.AddDays(89).ToString("dd/MM/yyyy"));
            DateTime date2 = DateTime.Parse(txtVencimiento.Text);

            if (txtIdentificador.EmptyTextBox())
            {
                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "Para generar liga(s) simple(s) es necesario corregir los siguientes errores: <br /> => El Identificador es obligatorio.";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                return;
            }
            if (txtAsunto.EmptyTextBox())
            {
                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "Para generar liga(s) simple(s) es necesario corregir los siguientes errores: <br /> => El campo Asunto es obligatorio.";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                return;
            }
            if (txtConcepto.EmptyTextBox())
            {
                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "Para generar liga(s) simple(s) es necesario corregir los siguientes errores: <br /> => El campo Concepto es obligatorio.";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                return;
            }
            if (txtImporte.EmptyTextBox())
            {
                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "Para generar liga(s) simple(s) es necesario corregir los siguientes errores: <br /> => El campo Importe es obligatorio.";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                return;
            }
            if (decimal.Parse(txtImporte.Text) >= 50 && decimal.Parse(txtImporte.Text) <= 15000)
            {
            }
            else
            {
                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "Para generar liga(s) simple(s) es necesario corregir los siguientes errores: <br /> => El importe mínimo es de $50.00 y el máximo es de $15,000.00.";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                return;
            }
            if (txtVencimiento.EmptyTextBox())
            {
                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "Para generar liga(s) simple(s) el campo Vencimiento es obligatorio.";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                return;
            }

            if (date >= hoy && date2 <= hoyMas)
            {
            }
            else
            {
                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "Para generar liga(s) simple(s) es necesario corregir los siguientes errores: <br /> => La fecha mínima es el día de hoy y la máxima son 90 días.";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                return;
            }

            if (usuariosCompletosServices.lsgvUsuariosSeleccionados.Count >= 1)
            {
                ViewState["nuevoSaldo"] = 0;

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

                lblTituloModal.Text = "Selección de envio de  ligas";
                pnlAlertModal.Visible = false;
                lblResumen.Text = "";
                divAlertModal.Attributes.Add("class", "alert alert-success alert-dismissible fade");
                btnGenerar.Visible = true;
                btnGenerar.Visible = true;
                btnCancelar.Visible = true;
                btnAceptar2.Visible = false;

                cbSms.Checked = false;
                cbWhats.Checked = false;
                lblAUtilizarSms.Text = "0";
                lblAUtilizarWA.Text = "0";
                lblTotalUtilizarSms.Text = "0.00";
                lblTotalUtilizarWA.Text = "0.00";

                lblPendienteWA.Text = "$0.00";

                string Mnsj = string.Empty;

                if (Cuenta >= DcmWhatsapp && Cuenta != 0)
                {
                    cbWhats.Enabled = true;
                    ViewState["MontoCuentaWA"] = true;
                }
                else
                {
                    cbWhats.Enabled = false;
                    ViewState["MontoCuentaWA"] = false;
                }

                if (Cuenta >= DcmSms && Cuenta != 0)
                {
                    cbSms.Enabled = true;
                    ViewState["MontoCuentaSMS"] = true;
                }
                else
                {
                    cbSms.Enabled = false;
                    ViewState["MontoCuentaSMS"] = false;
                }

                if (Cuenta == 0)
                {
                    pnlAlertModal.Visible = true;
                    lblResumen.Text = "Actualmente no cuenta con saldo para enviar la(s) liga(s) por WhatsApp y/o SMS.";
                    divAlertModal.Attributes.Add("class", "alert alert-info alert-dismissible fade show");
                }
                else
                {
                    string mnsj = string.Empty;
                    bool error = false;

                    if (DcmSms > Cuenta)
                    {
                        mnsj = "El saldo no es suficiente para enviar la(s) liga(s) por SMS.";
                        error = true;
                    }

                    if (DcmWhatsapp > Cuenta)
                    {
                        if (mnsj != string.Empty)
                        {
                            mnsj += "<br />El saldo no es suficiente para enviar la(s) liga(s) por WhatsApp.";
                        }
                        else
                        {
                            mnsj = "El saldo no es suficiente para enviar la(s) liga(s) por WhatsApp.";
                        }
                        error = true;
                    }

                    if (error)
                    {
                        pnlAlertModal.Visible = true;
                        lblResumen.Text = mnsj;
                        divAlertModal.Attributes.Add("class", "alert alert-info alert-dismissible fade show");
                    }
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);
            }
            else
            {
                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "Para generar liga(s) simple(s) es necesario corregir los siguientes errores: <br /> => Agregue almenos un usuario.";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
            }

        }

        public void LimpiarDatos()
        {
            txtIdentificador.Text = string.Empty;
            txtAsunto.Text = string.Empty;
            txtConcepto.Text = string.Empty;
            txtImporte.Text = string.Empty;
            txtVencimiento.Text = string.Empty;

            gvUsuariosSeleccionados.DataSource = null;
            gvUsuariosSeleccionados.DataBind();

            usuariosCompletosServices.lsgvUsuariosSeleccionados.Clear();
            Session["lsgvUsuariosSeleccionados"] = null;
        }
        protected void btn_ImportCSV_Click(object sender, EventArgs e)
        {
            //string filePath = string.Empty;
            //if (fu_ImportCSV.HasFile && fu_ImportCSV.PostedFile.ContentType.Equals("application/vnd.ms-excel"))
            //{
            //    gv_GridView.DataSource = (DataTable)ReadToEnd(fu_ImportCSV.PostedFile.FileName);
            //    gv_GridView.DataBind();
            //    lbl_ErrorMsg.Visible = false;
            //}
            //else
            //{
            //    lbl_ErrorMsg.Text = "Please check the selected file type";
            //    lbl_ErrorMsg.Visible = true;
            //}

        }

        private object ReadToEnd(string filePath)
        {
            DataTable dtDataSource = new DataTable();
            //string[] fileContent = File.ReadAllLines("C:/Proyectos/Franquicia/Franquicia.WebForms/SqlExport (4).csv");
            string[] fileContent = { filePath };
            if (fileContent.Count() > 0)
            {
                //Create data table columns
                string[] columns = fileContent[0].Split(',');
                for (int i = 0; i < columns.Count(); i++)
                {
                    dtDataSource.Columns.Add(columns[i]);
                }

                //Add row data
                for (int i = 1; i < fileContent.Count(); i++)
                {
                    string[] rowData = fileContent[i].Split(',');
                    dtDataSource.Rows.Add(rowData);
                }
            }
            return dtDataSource;
        }

        protected void gvUsuarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUsuarios.PageIndex = e.NewPageIndex;
            gvUsuarios.DataSource = usuariosCompletosServices.lsLigasUsuariosGridViewModel;
            gvUsuarios.DataBind();
        }

        protected void gvUsuariosSeleccionados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "btnEliminar")
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvUsuariosSeleccionados.Rows[index];
                GridView valor = (GridView)sender;
                int dataKey = int.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());

                usuariosCompletosServices.EliminarItemgvUsuariosSeleccionados(dataKey, index);
                gvUsuariosSeleccionados.DataSource = usuariosCompletosServices.lsgvUsuariosSeleccionados;
                gvUsuariosSeleccionados.DataBind();
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
            ////string authToken = "0f47ce2d28c9211ac6a9ae42f630d1d6";
            //string authToken = "3f914e588826df9a93ed849cee73eae2";
            ////string NumberFrom = "+14158739087";
            //string NumberFrom = "+14155238886";

            if (cbCorreo.Checked == false && cbSms.Checked == false && cbWhats.Checked == false)
            {
                pnlAlertModal.Visible = true;
                lblResumen.Text = "Para generar liga(s) simples(s) es necesario corregir los siguientes errores: <br /> => Seleccione almenos uno de los tres tipos de envios.";
                divAlertModal.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
            }
            else
            {
                if (decimal.Parse(ViewState["nuevoSaldo"].ToString()) >= 0)
                {
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

                        foreach (var item in usuariosCompletosServices.lsgvUsuariosSeleccionados)
                        {
                            bool VariasLigas = false;
                            Guid UidLigaAsociado = Guid.NewGuid();

                            foreach (ListItem itPromo in cblPromociones.Items)
                            {
                                if (itPromo.Selected)
                                {
                                    VariasLigas = true;
                                }
                            }

                            if (VariasLigas)
                            {
                                DateTime thisDay = DateTime.Now;
                                string ReferenciaCobro = item.IdCliente.ToString() + item.IdUsuario.ToString() + thisDay.ToString("ddMMyyyyHHmmssfff");
                                Guid UidLigaUrl = Guid.NewGuid();
                                string urlCobro = GenLigaPara(id_company, id_branch, user, pwd, ReferenciaCobro, decimal.Parse(txtImporte.Text), moneda, canal, "C", item.StrCorreo, semillaAES, data0, urlGen);

                                if (urlCobro.Contains("https://"))
                                {
                                    if (usuariosCompletosServices.GenerarLigasPagosTemp(UidLigaUrl, urlCobro, txtConcepto.Text, decimal.Parse(txtImporte.Text), ReferenciaCobro, item.UidUsuario, txtIdentificador.Text, thisDay, DateTime.Parse(txtVencimiento.Text), txtAsunto.Text, UidLigaAsociado, Guid.Empty, Guid.Parse(ViewState["UidClienteLocal"].ToString())))
                                    {
                                        resu = true;
                                        foreach (ListItem itPromo in cblPromociones.Items)
                                        {
                                            if (itPromo.Selected)
                                            {
                                                SuperPromocionesServices superPromocionesServices = new SuperPromocionesServices();
                                                superPromocionesServices.CargarSuperPromociones();

                                                foreach (var itSP in superPromocionesServices.lsCBLSuperPromociones)
                                                {
                                                    if (itSP.UidPromocion == Guid.Parse(itPromo.Value))
                                                    {
                                                        int i = promocionesServices.lsCBLPromocionesModelCliente.IndexOf(promocionesServices.lsCBLPromocionesModelCliente.First(x => x.UidPromocion == Guid.Parse(itPromo.Value)));

                                                        if (decimal.Parse(txtImporte.Text) >= promocionesServices.lsCBLPromocionesModelCliente[i].DcmApartirDe)
                                                        {

                                                            decimal cobro = promocionesServices.lsCBLPromocionesModelCliente[i].DcmComicion;

                                                            decimal Valor = cobro * decimal.Parse(txtImporte.Text) / 100;
                                                            decimal Importe = Valor + decimal.Parse(txtImporte.Text);

                                                            string promocion = itPromo.Text.Replace(" MESES", "");

                                                            DateTime thisDay2 = DateTime.Now;
                                                            string Referencia = item.IdCliente.ToString() + item.IdUsuario.ToString() + thisDay2.ToString("ddMMyyyyHHmmssfff");

                                                            url = GenLigaPara(id_company, id_branch, user, pwd, Referencia, Importe, moneda, canal, promocion, item.StrCorreo, semillaAES, data0, urlGen);

                                                            if (url.Contains("https://"))
                                                            {
                                                                if (usuariosCompletosServices.GenerarLigasPagos(url, txtConcepto.Text, Importe, Referencia, item.UidUsuario, txtIdentificador.Text, thisDay, DateTime.Parse(txtVencimiento.Text), txtAsunto.Text, UidLigaAsociado, Guid.Parse(itPromo.Value), Guid.Parse(ViewState["UidClienteLocal"].ToString())))
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
                                            decimal Final = itPromo.DcmImporte / promocion;

                                            strPromociones +=
                                                                                "\t\t\t\t\t\t\t\t<tr>\r\n" +
                                                                                "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: right;\">\r\n" +
                                                                                "\t\t\t\t\t\t\t\t\t\t" + itPromo.VchDescripcion + " de $" + Final.ToString("N2") + "\r\n" +
                                                                                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                                                                                "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: left;\">\r\n" +
                                                                                "\t\t\t\t\t\t\t\t\t\t &nbsp;" + "<a style =\"display:block;color:#fff;font-weight:400;text-align:center;width:100px;font-size:15px;text-decoration:none;background:#28a745;margin:0 auto; padding:5px;\" href=" + URLBase + "Views/Promociones.aspx?CodigoPromocion=" + UidLigaAsociado + "&CodigoLiga=" + itPromo.IdReferencia + "> Pagar $" + itPromo.DcmImporte.ToString("N2") + "</a>" + "\r\n" +
                                                                                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                                                                                "\t\t\t\t\t\t\t\t</tr>\r\n";
                                        }
                                        boolPromociones = true;
                                    }

                                    string LigaUrl = URLBase + "Views/Promociones.aspx?CodigoPromocion=" + UidLigaAsociado + "&CodigoLiga=" + ReferenciaCobro;

                                    if (cbCorreo.Checked)
                                    {
                                        try
                                        {
                                            correosServices.CorreoLiga(item.NombreCompleto, txtAsunto.Text, txtConcepto.Text, decimal.Parse(txtImporte.Text), DateTime.Parse(txtVencimiento.Text), LigaUrl, item.StrCorreo, strPromociones, boolPromociones, item.VchNombreComercial);
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
                                        "\r\n" + "$" + decimal.Parse(txtImporte.Text).ToString("N2") + " https://cobrosmasivos.com/" + "Pago.aspx?Id=" + UidLigaUrl;

                                    if (cbSms.Checked)
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
                                        }
                                        catch (Exception ex)
                                        {
                                            //lblResumen.Text += "SMS: " + ex.Message;
                                            cbSms.Checked = false;
                                            int error = ErrorSms + 1;
                                            lblErrorSms.Text = error.ToString();
                                        }
                                    }

                                    if (cbWhats.Checked)
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
                                            }
                                            catch (Exception ex)
                                            {
                                                //lblResumen.Text += "WhatsApp: " + ex.Message;
                                                cbWhats.Checked = false;
                                                int error = ErrorWA + 1;
                                                lblErrorWA.Text = error.ToString();
                                            }
                                        }
                                        else
                                        {
                                            whatsAppPendientesServices.RegistrarWhatsPendiente(body.Replace("\r\n", "[n]"), DateTime.Parse(txtVencimiento.Text), NumberVali, Guid.Parse(ViewState["UidClienteLocal"].ToString()), item.UidUsuario, UidHistorialPago, VchDescripcion);

                                            cbWhats.Checked = false;
                                            //int pendiente = PendienteWA + 1;
                                            //lblPendienteWA.Text = pendiente.ToString(); //Se quito por que se valida desde el principio
                                        }
                                    }

                                    if (cbSms.Checked || cbWhats.Checked)
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

                                string urlCobro = GenLigaPara(id_company, id_branch, user, pwd, ReferenciaCobro, decimal.Parse(txtImporte.Text), moneda, canal, "C", item.StrCorreo, semillaAES, data0, urlGen);

                                if (urlCobro.Contains("https://"))
                                {
                                    Guid UidLigaUrl = Guid.NewGuid();
                                    if (usuariosCompletosServices.GenerarLigasPagosTemp(UidLigaUrl, urlCobro, txtConcepto.Text, decimal.Parse(txtImporte.Text), ReferenciaCobro, item.UidUsuario, txtIdentificador.Text, thisDay, DateTime.Parse(txtVencimiento.Text), txtAsunto.Text, Guid.Empty, Guid.Empty, Guid.Parse(ViewState["UidClienteLocal"].ToString())))
                                    {
                                        if (cbCorreo.Checked)
                                        {
                                            try
                                            {
                                                correosServices.CorreoLiga(item.NombreCompleto, txtAsunto.Text, txtConcepto.Text, decimal.Parse(txtImporte.Text), DateTime.Parse(txtVencimiento.Text), urlCobro, item.StrCorreo, "", false, item.VchNombreComercial);
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
                                            "\r\n" + "$" + decimal.Parse(txtImporte.Text).ToString("N2") + " https://cobrosmasivos.com/" + "Pago.aspx?Id=" + UidLigaUrl;

                                        if (cbSms.Checked)
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
                                            }
                                            catch (Exception ex)
                                            {
                                                //lblResumen.Text += "SMS: " + ex.Message;
                                                cbSms.Checked = false;
                                                int error = ErrorSms + 1;
                                                lblErrorSms.Text = error.ToString();
                                            }
                                        }

                                        if (cbWhats.Checked)
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

                                                    var nu = message.Status;

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
                                                }
                                                catch (Exception ex)
                                                {
                                                    //lblResumen.Text += "WhatsApp: " + ex.Message;
                                                    cbWhats.Checked = false;
                                                    int error = ErrorWA + 1;
                                                    lblErrorWA.Text = error.ToString();
                                                }
                                            }
                                            else
                                            {
                                                whatsAppPendientesServices.RegistrarWhatsPendiente(body.Replace("\r\n", "[n]"), DateTime.Parse(txtVencimiento.Text), NumberVali, Guid.Parse(ViewState["UidClienteLocal"].ToString()), item.UidUsuario, UidHistorialPago, VchDescripcion);

                                                cbWhats.Checked = false;
                                                //int pendiente = PendienteWA + 1;
                                                //lblPendienteWA.Text = pendiente.ToString(); //Se quito por que se valida desde el principio
                                            }
                                        }

                                        if (cbSms.Checked || cbWhats.Checked)
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

                            lblResumen.Text = " Se han generado: " + usuariosCompletosServices.lsgvUsuariosSeleccionados.Count.ToString() + " liga(s) exitosamente.";
                            lblCorreoUsado.Text = usuariosCompletosServices.lsgvUsuariosSeleccionados.Count.ToString();

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

        protected void gvUsuariosSeleccionados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUsuariosSeleccionados.PageIndex = e.NewPageIndex;
            gvUsuariosSeleccionados.DataSource = usuariosCompletosServices.lsgvUsuariosSeleccionados;
            gvUsuariosSeleccionados.DataBind();
        }

        protected void gvUsuariosSeleccionados_Sorting(object sender, GridViewSortEventArgs e)
        {
            string SortExpression = e.SortExpression;
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
                            usuariosCompletosServices.lsgvUsuariosSeleccionados = usuariosCompletosServices.lsgvUsuariosSeleccionados.OrderBy(x => x.NombreCompleto).ToList();
                        }
                        else
                        {
                            usuariosCompletosServices.lsgvUsuariosSeleccionados = usuariosCompletosServices.lsgvUsuariosSeleccionados.OrderByDescending(x => x.NombreCompleto).ToList();
                        }
                        break;
                    case "StrCorreo":
                        if (Orden == "ASC")
                        {
                            usuariosCompletosServices.lsgvUsuariosSeleccionados = usuariosCompletosServices.lsgvUsuariosSeleccionados.OrderBy(x => x.StrCorreo).ToList();
                        }
                        else
                        {
                            usuariosCompletosServices.lsgvUsuariosSeleccionados = usuariosCompletosServices.lsgvUsuariosSeleccionados.OrderByDescending(x => x.StrCorreo).ToList();
                        }
                        break;
                    case "StrTelefono":
                        if (Orden == "ASC")
                        {
                            usuariosCompletosServices.lsgvUsuariosSeleccionados = usuariosCompletosServices.lsgvUsuariosSeleccionados.OrderBy(x => x.StrTelefono).ToList();
                        }
                        else
                        {
                            usuariosCompletosServices.lsgvUsuariosSeleccionados = usuariosCompletosServices.lsgvUsuariosSeleccionados.OrderByDescending(x => x.StrTelefono).ToList();
                        }
                        break;
                }

                gvUsuariosSeleccionados.DataSource = usuariosCompletosServices.lsgvUsuariosSeleccionados;
                gvUsuariosSeleccionados.DataBind();
            }
        }

        protected void gvUsuarios_Sorting(object sender, GridViewSortEventArgs e)
        {
            string SortExpression = e.SortExpression;
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
                            usuariosCompletosServices.lsLigasUsuariosGridViewModel = usuariosCompletosServices.lsLigasUsuariosGridViewModel.OrderBy(x => x.NombreCompleto).ToList();
                        }
                        else
                        {
                            usuariosCompletosServices.lsLigasUsuariosGridViewModel = usuariosCompletosServices.lsLigasUsuariosGridViewModel.OrderByDescending(x => x.NombreCompleto).ToList();
                        }
                        break;
                    case "StrCorreo":
                        if (Orden == "ASC")
                        {
                            usuariosCompletosServices.lsLigasUsuariosGridViewModel = usuariosCompletosServices.lsLigasUsuariosGridViewModel.OrderBy(x => x.StrCorreo).ToList();
                        }
                        else
                        {
                            usuariosCompletosServices.lsLigasUsuariosGridViewModel = usuariosCompletosServices.lsLigasUsuariosGridViewModel.OrderByDescending(x => x.StrCorreo).ToList();
                        }
                        break;
                    case "StrTelefono":
                        if (Orden == "ASC")
                        {
                            usuariosCompletosServices.lsLigasUsuariosGridViewModel = usuariosCompletosServices.lsLigasUsuariosGridViewModel.OrderBy(x => x.StrTelefono).ToList();
                        }
                        else
                        {
                            usuariosCompletosServices.lsLigasUsuariosGridViewModel = usuariosCompletosServices.lsLigasUsuariosGridViewModel.OrderByDescending(x => x.StrTelefono).ToList();
                        }
                        break;
                }

                gvUsuarios.DataSource = usuariosCompletosServices.lsLigasUsuariosGridViewModel;
                gvUsuarios.DataBind();
            }
        }

        protected void btnReiniciar_Click(object sender, EventArgs e)
        {
            LimpiarDatos();
        }

        protected void btnDescargarError_Click(object sender, EventArgs e)
        {
            Session["lsgvUsuariosSeleccionados"] = null;
            Session["lsLigasUsuariosGridViewModelError"] = usuariosCompletosServices.lsLigasErrores;
            string _open = "window.open('ExportarAExcel.aspx', '_blank');";
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
            Session["lsLigasUsuariosGridViewModelError"] = null;
        }

        protected void cbWhats_CheckedChanged(object sender, EventArgs e)
        {
            if ((bool)ViewState["MontoCuentaWA"])
            {
                if (cbWhats.Checked)
                {
                    int permitido = 0;
                    int pendiente = 0;

                    foreach (var item in usuariosCompletosServices.lsgvUsuariosSeleccionados)
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


                    //int aUtilizar = usuariosCompletosServices.lsgvUsuariosSeleccionados.Count;
                    int aUtilizar = permitido;
                    decimal tarifaWA = decimal.Parse(lblDcmWhatsapp.Text);

                    decimal tariUtizaWA = aUtilizar * tarifaWA;
                    decimal cuenta = decimal.Parse(lblDcmCuenta.Text);

                    decimal tarifaSms = decimal.Parse(lblDcmSms.Text);
                    int aUtilizarSms = int.Parse(lblAUtilizarSms.Text);
                    decimal tariUtizaSms = aUtilizarSms * tarifaSms;

                    decimal sumWASm = tariUtizaWA + tariUtizaSms;

                    decimal nuevoSaldo = cuenta - sumWASm;

                    lblAUtilizarWA.Text = aUtilizar.ToString();
                    lblTotalUtilizarWA.Text = tariUtizaWA.ToString("N2");
                    lblDcmSaldo.Text = nuevoSaldo.ToString("N2");
                    ViewState["nuevoSaldo"] = nuevoSaldo.ToString("N2");

                    lblAUtilizarSms.Text = aUtilizarSms.ToString();

                    decimal pendient = pendiente * tarifaWA;
                    lblPendienteWA.Text = "$" + pendient.ToString("N2");
                }
                else
                {
                    lblDcmSaldo.Text = "0";

                    int aUtilizar = 0;
                    decimal tarifaWA = decimal.Parse(lblDcmWhatsapp.Text);

                    decimal tariUtizaWA = aUtilizar * tarifaWA;
                    decimal cuenta = decimal.Parse(lblDcmCuenta.Text);

                    decimal tarifaSms = decimal.Parse(lblDcmSms.Text);
                    int aUtilizarSms = int.Parse(lblAUtilizarSms.Text);
                    decimal tariUtizaSms = aUtilizarSms * tarifaSms;

                    decimal sumWASm = tariUtizaWA + tariUtizaSms;

                    decimal nuevoSaldo = cuenta - sumWASm;

                    lblAUtilizarWA.Text = "0";
                    lblTotalUtilizarWA.Text = "0.00";
                    lblDcmSaldo.Text = nuevoSaldo.ToString("N2");
                    ViewState["nuevoSaldo"] = nuevoSaldo.ToString("N2");

                    lblAUtilizarSms.Text = aUtilizarSms.ToString();

                    lblPendienteWA.Text = "$0.00";
                }
            }
            else
            {
                cbWhats.Checked = false;
            }
        }

        protected void cbSms_CheckedChanged(object sender, EventArgs e)
        {
            if ((bool)ViewState["MontoCuentaSMS"])
            {
                if (cbSms.Checked)
                {
                    int aUtilizar = int.Parse(lblAUtilizarWA.Text);
                    decimal tarifaWA = decimal.Parse(lblDcmWhatsapp.Text);

                    decimal tariUtizaWA = aUtilizar * tarifaWA;
                    decimal cuenta = decimal.Parse(lblDcmCuenta.Text);

                    decimal tarifaSms = decimal.Parse(lblDcmSms.Text);
                    int aUtilizarSms = usuariosCompletosServices.lsgvUsuariosSeleccionados.Count;
                    decimal tariUtizaSms = aUtilizarSms * tarifaSms;

                    decimal sumWASm = tariUtizaWA + tariUtizaSms;

                    decimal nuevoSaldo = cuenta - sumWASm;

                    lblAUtilizarSms.Text = aUtilizarSms.ToString();
                    lblTotalUtilizarSms.Text = tariUtizaSms.ToString("N2");
                    lblDcmSaldo.Text = nuevoSaldo.ToString("N2");
                    ViewState["nuevoSaldo"] = nuevoSaldo.ToString("N2");

                    lblAUtilizarWA.Text = aUtilizar.ToString();
                }
                else
                {
                    lblDcmSaldo.Text = "0";

                    int aUtilizar = int.Parse(lblAUtilizarWA.Text);
                    decimal tarifaWA = decimal.Parse(lblDcmWhatsapp.Text);

                    decimal tariUtizaWA = aUtilizar * tarifaWA;
                    decimal cuenta = decimal.Parse(lblDcmCuenta.Text);

                    decimal tarifaSms = decimal.Parse(lblDcmSms.Text);
                    int aUtilizarSms = 0;
                    decimal tariUtizaSms = aUtilizarSms * tarifaSms;

                    decimal sumWASm = tariUtizaWA + tariUtizaSms;

                    decimal nuevoSaldo = cuenta - sumWASm;

                    lblAUtilizarSms.Text = "0";
                    lblTotalUtilizarSms.Text = "0.00";
                    lblDcmSaldo.Text = nuevoSaldo.ToString("N2");
                    ViewState["nuevoSaldo"] = nuevoSaldo.ToString("N2");

                    lblAUtilizarWA.Text = aUtilizar.ToString();
                }
            }
            else
            {
                cbSms.Checked = false;
            }
        }

    }
}