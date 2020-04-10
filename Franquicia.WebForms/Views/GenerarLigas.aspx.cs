using ClosedXML.Excel;
using Franquicia.Bussiness;
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
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
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

            if (!IsPostBack)
            {
                ViewState["gvUsuariosSeleccionados"] = SortDirection.Ascending;
                ViewState["gvUsuarios"] = SortDirection.Ascending;

                btnCargarExcel.Attributes.Add("onclick", "document.getElementById('" + fuSelecionarExcel.ClientID + "').click(); return false;");
                fuSelecionarExcel.Attributes["onchange"] = "UploadFile(this)";

                Session["usuariosCompletosServices"] = usuariosCompletosServices;
                Session["parametrosEntradaServices"] = parametrosEntradaServices;
                Session["promocionesServices"] = promocionesServices;

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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Mult", "multi()", true);
                //pnlAlertImportarError.Visible = false;
                //lblMnsjAlertImportarError.Text = "";
                //divAlertImportarError.Attributes.Add("class", "alert alert-danger alert-dismissible fade");

                pnlAlert.Visible = false;
                lblMensajeAlert.Text = "";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade");
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
                    byte[] buffer = new byte[fuSelecionarExcel.FileBytes.Length];
                    fuSelecionarExcel.FileContent.Seek(0, SeekOrigin.Begin);
                    fuSelecionarExcel.FileContent.Read(buffer, 0, Convert.ToInt32(fuSelecionarExcel.FileContent.Length));

                    Stream stream2 = new MemoryStream(buffer);

                    DataTable dt = new DataTable();
                    using (XLWorkbook workbook = new XLWorkbook(stream2))
                    {
                        IXLWorksheet sheet = workbook.Worksheet(1);
                        bool firRow = true;
                        foreach (IXLRow row in sheet.Rows())
                        {
                            if (firRow)
                            {
                                foreach (IXLCell cell in row.Cells())
                                {
                                    dt.Columns.Add(cell.Value.ToString());
                                }
                                firRow = false;
                            }
                            else
                            {
                                dt.Rows.Add();
                                int i = 0;
                                foreach (IXLCell cell in row.Cells())
                                {
                                    dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                                    i++;
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
                            usuariosCompletosServices.ExcelToList(usuariosCompletosServices.lsgvUsuariosSeleccionados, usuariosCompletosServices.lsLigasInsertar, Guid.Parse(ViewState["UidClienteLocal"].ToString()));

                            gvUsuariosSeleccionados.DataSource = usuariosCompletosServices.lsgvUsuariosSeleccionados;
                            gvUsuariosSeleccionados.DataBind();
                        }

                        if (usuariosCompletosServices.lsLigasErrores.Count >= 1)
                        {
                            btnDescargarError.Visible = true;
                            btnMasDetalle.Visible = true;
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
                else
                {
                    //    byte[] buffer = new byte[fuSelecionarExcel.FileBytes.Length];
                    //    fuSelecionarExcel.FileContent.Seek(0, SeekOrigin.Begin);
                    //    fuSelecionarExcel.FileContent.Read(buffer, 0, Convert.ToInt32(fuSelecionarExcel.FileContent.Length));
                    //    Stream stream2 = new MemoryStream(buffer);
                    //    int f = 0;
                    //    using (var reader = new StreamReader(stream2))

                    //        f = reader.ReadAllLines();

                    //    int f2 = '"' + ',' + '"';

                    //    //txtAsunto.Text = f.Replace("\r\n", txtAsunto.Text);

                    //    string ll = txtAsunto.Text.Replace(@"\\", "-");

                    //    string fio = "";

                    //    string lol = txtAsunto.Text.Replace("\\", "-");

                    //    string fi00o = "";

                    //    gridview.DataSource = (DataTable)ReadToEnd(txtAsunto.Text.Replace(@"\", ""));
                    //    gridview.DataBind();


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
            if (!string.IsNullOrEmpty(txtIdentificador.Text.Trim()) && !string.IsNullOrEmpty(txtAsunto.Text.Trim()) &&
                !string.IsNullOrEmpty(txtConcepto.Text.Trim()) && !string.IsNullOrEmpty(txtImporte.Text.Trim())
                && !string.IsNullOrEmpty(txtVencimiento.Text.Trim()))
            {
                if (usuariosCompletosServices.lsgvUsuariosSeleccionados.Count >= 1)
                {
                    lblTituloModal.Text = "Selección de envio de  ligas";
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
                    lblMensajeAlert.Text = "Para enviar las ligas, debe de haber almenos un usuario agregado.";
                    divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                }
            }
            else
            {
                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "Los campos con * son obligatorios.";
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
            gvUsuarios.DataSource = usuariosCompletosServices.lsLigasUsuariosGridViewModel.Where(x => x.blSeleccionado == false).ToList();
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

                usuariosCompletosServices.EliminarItemgvUsuariosSeleccionados(dataKey);
                gvUsuariosSeleccionados.DataSource = usuariosCompletosServices.lsgvUsuariosSeleccionados;
                gvUsuariosSeleccionados.DataBind();
            }
        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
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

                        string urlCobro = GenLigaPara(id_company, id_branch, user, pwd, ReferenciaCobro, decimal.Parse(txtImporte.Text), moneda, canal, "C", item.StrCorreo, semillaAES, data0, urlGen);

                        if (urlCobro.Contains("https://"))
                        {
                            if (usuariosCompletosServices.GenerarLigasPagos(urlCobro, txtConcepto.Text, decimal.Parse(txtImporte.Text), ReferenciaCobro, item.UidUsuario, txtIdentificador.Text, thisDay, DateTime.Parse(txtVencimiento.Text), txtAsunto.Text, UidLigaAsociado, Guid.Empty, Guid.Parse(ViewState["UidClienteLocal"].ToString())))
                            {
                                resu = true;
                                foreach (ListItem itPromo in cblPromociones.Items)
                                {
                                    if (itPromo.Selected)
                                    {
                                        int i = promocionesServices.lsCBLPromocionesModelCliente.IndexOf(promocionesServices.lsCBLPromocionesModelCliente.First(x => x.UidPromocion == Guid.Parse(itPromo.Value)));
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
                        else
                        {
                            resu = false;
                            break;
                        }

                        if (resu)
                        {
                            promocionesServices.CargarPromocionesValidas(UidLigaAsociado);

                            string strPromociones = string.Empty;

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
                            }

                            string LigaUrl = URLBase + "Views/Promociones.aspx?CodigoPromocion=" + UidLigaAsociado + "&CodigoLiga=" + ReferenciaCobro;
                            correosServices.CorreoLiga(item.NombreCompleto, txtAsunto.Text, txtConcepto.Text, decimal.Parse(txtImporte.Text), DateTime.Parse(txtVencimiento.Text), LigaUrl, item.StrCorreo, strPromociones, true, item.VchNombreComercial);
                        }
                    }
                    else
                    {
                        DateTime thisDay = DateTime.Now;
                        string ReferenciaCobro = item.IdCliente.ToString() + item.IdUsuario.ToString() + thisDay.ToString("ddMMyyyyHHmmssfff");

                        string urlCobro = GenLigaPara(id_company, id_branch, user, pwd, ReferenciaCobro, decimal.Parse(txtImporte.Text), moneda, canal, "C", item.StrCorreo, semillaAES, data0, urlGen);

                        if (urlCobro.Contains("https://"))
                        {
                            if (usuariosCompletosServices.GenerarLigasPagos(urlCobro, txtConcepto.Text, decimal.Parse(txtImporte.Text), ReferenciaCobro, item.UidUsuario, txtIdentificador.Text, thisDay, DateTime.Parse(txtVencimiento.Text), txtAsunto.Text, Guid.Empty, Guid.Empty, Guid.Parse(ViewState["UidClienteLocal"].ToString())))
                            {
                            correosServices.CorreoLiga(item.NombreCompleto, txtAsunto.Text, txtConcepto.Text, decimal.Parse(txtImporte.Text), DateTime.Parse(txtVencimiento.Text), urlCobro, item.StrCorreo, "", false, item.VchNombreComercial);
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
                    lblResumen.Text = " Se han generado: " + usuariosCompletosServices.lsgvUsuariosSeleccionados.Count.ToString() + " liga(s) exitosamente.";
                    lblCorreoUsado.Text = usuariosCompletosServices.lsgvUsuariosSeleccionados.Count.ToString();

                    LimpiarDatos();

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
            gvUsuariosSeleccionados.DataSource = usuariosCompletosServices.lsLigasUsuariosGridViewModel.Where(x => x.blSeleccionado == true).ToList();
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

                gvUsuariosSeleccionados.DataSource = usuariosCompletosServices.lsLigasUsuariosGridViewModel.Where(x => x.blSeleccionado == true).ToList(); ;
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

                gvUsuarios.DataSource = usuariosCompletosServices.lsLigasUsuariosGridViewModel.Where(x => x.blSeleccionado == false).ToList();
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
    }
}