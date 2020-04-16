using AjaxControlToolkit;
using ClosedXML.Excel;
using Franquicia.Bussiness;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
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
        ParametrosEntradaServices parametrosEntradaServices = new ParametrosEntradaServices();
        PromocionesServices promocionesServices = new PromocionesServices();
        ClienteCuentaServices clienteCuentaServices = new ClienteCuentaServices();

        List<string> listPromocionesSeleccionados = new List<string>();

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
                ViewState["gvUsuarios"] = SortDirection.Ascending;

                ViewState["listPromocionesSeleccionados"] = listPromocionesSeleccionados;

                btnCargarExcel.Attributes.Add("onclick", "document.getElementById('" + fuSelecionarExcel.ClientID + "').click(); return false;");
                fuSelecionarExcel.Attributes["onchange"] = "UploadFile(this)";

                Session["usuariosCompletosServices"] = usuariosCompletosServices;
                Session["promocionesServices"] = promocionesServices;

                promocionesServices.lsCBLPromocionesModelCliente.Clear();
                promocionesServices.CargarPromocionesClientes(Guid.Parse(ViewState["UidClienteLocal"].ToString()));

            }
            else
            {
                usuariosCompletosServices = (UsuariosCompletosServices)Session["usuariosCompletosServices"];
                promocionesServices = (PromocionesServices)Session["promocionesServices"];
                listPromocionesSeleccionados = (List<string>)ViewState["listPromocionesSeleccionados"];

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Mult", "multi()", true);

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
                        && dt.Columns.Contains("IMPORTE".Trim()) && dt.Columns.Contains("VENCIMIENTO".Trim()) && dt.Columns.Contains("PROMOCION(ES)".Trim()))
                    {

                        usuariosCompletosServices.ValidarExcelToListMultiple(dt, promocionesServices.lsCBLPromocionesModelCliente);

                        if (usuariosCompletosServices.lsLigasInsertarMultiple.Count >= 1)
                        {
                            usuariosCompletosServices.ExcelToListMultiple(usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple, usuariosCompletosServices.lsLigasInsertarMultiple, Guid.Parse(ViewState["UidClienteLocal"].ToString()));

                            foreach (var item in usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple)
                            {
                                promocionesServices.lsLigasMultiplePromocionesModel.RemoveAll(x => x.IdUsuario == item.IdUsuario);

                                foreach (var it in promocionesServices.lsCBLPromocionesModelCliente)
                                {
                                    string[] arPromo = Regex.Split(item.StrPromociones, ",");

                                    for (int i = 0; i < arPromo.Length; i++)
                                    {
                                        if (it.VchDescripcion.Trim() == arPromo[i].Trim())
                                        {
                                            promocionesServices.PromocionesMultiples(it.UidPromocion.ToString(), item.IdUsuario, it.VchDescripcion);
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
            if (!string.IsNullOrEmpty(txtIdentificador.Text.Trim()))
            {
                if (usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple.Count >= 1)
                {
                    bool Accion = true;
                    foreach (GridViewRow row in gvUsuariosSeleccionados.Rows)
                    {
                        TextBox txtGvAsunto = row.FindControl("txtGvAsunto") as TextBox;
                        TextBox txtGvConcepto = row.FindControl("txtGvConcepto") as TextBox;
                        TextBox txtGvImporte = row.FindControl("txtGvImporte") as TextBox;
                        TextBox txtGvVencimiento = row.FindControl("txtGvVencimiento") as TextBox;

                        if (txtGvAsunto.Text.Trim() == string.Empty || txtGvConcepto.Text.Trim() == string.Empty || txtGvImporte.Text.Trim() == string.Empty || txtGvVencimiento.Text.Trim() == string.Empty)
                        {
                            Accion = false;
                            break;
                        }
                    }

                    if (Accion)
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
                        lblMensajeAlert.Text = "Los campos Asunto, Concepto, Importe y Vencimiento son campos obligatorios.";
                        divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                    }
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

            gvUsuariosSeleccionados.DataSource = null;
            gvUsuariosSeleccionados.DataBind();

            usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple.Clear();
            Session["lsgvUsuariosSeleccionadosMultiple"] = null;

            promocionesServices.lsLigasMultiplePromocionesModel.Clear();
        }

        protected void gvUsuarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUsuarios.PageIndex = e.NewPageIndex;
            gvUsuarios.DataSource = usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel.Where(x => x.blSeleccionado == false).ToList();
            gvUsuarios.DataBind();
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
                GvVencimiento.Attributes.Add("min", DateTime.Now.ToString("yyyy-MM-dd"));
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

                LinkButton btnAceptar = (LinkButton)row.FindControl("btnAceptar");
                LinkButton btnCancelar = (LinkButton)row.FindControl("btnCancelar");
                LinkButton btnEditar = (LinkButton)row.FindControl("btnEditar");
                LinkButton btnEliminar = (LinkButton)row.FindControl("btnEliminar");

                TextBox txtGvAsunto = (TextBox)row.FindControl("txtGvAsunto");
                TextBox txtGvConcepto = (TextBox)row.FindControl("txtGvConcepto");
                TextBox txtGvImporte = (TextBox)row.FindControl("txtGvImporte");
                TextBox txtGvVencimiento = (TextBox)row.FindControl("txtGvVencimiento");
                
                btnAceptar.Visible = true;
                btnCancelar.Visible = true;
                btnEditar.Visible = false;
                btnEliminar.Visible = false;

                txtGvAsunto.Enabled = true;
                txtGvConcepto.Enabled = true;
                txtGvImporte.Enabled = true;
                txtGvVencimiento.Enabled = true;

                txtGvAsunto.BorderStyle = BorderStyle.Solid;
                txtGvConcepto.BorderStyle = BorderStyle.Solid;
                txtGvImporte.BorderStyle = BorderStyle.Solid;
                txtGvVencimiento.BorderStyle = BorderStyle.Solid;

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
                            promocionesServices.PromocionesMultiples(item.Value, dataKey, item.Text);
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

                usuariosCompletosServices.ActualizarListaGvUsuariosMultiple(usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple, dataKey, true, txtGvAsunto.Text, txtGvConcepto.Text, decimal.Parse(txtGvImporte.Text), DateTime.Parse(txtGvVencimiento.Text), Promociones);
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
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvUsuariosSeleccionados.Rows[index];
                GridView valor = (GridView)sender;
                int dataKey = int.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());

                //usuariosCompletosServices.ActualizarListaUsuariosMultiple(usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel, dataKey, false);
                //gvUsuariosSeleccionados.DataSource = usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel.Where(x => x.blSeleccionado == true).ToList();
                //gvUsuariosSeleccionados.DataBind();

                usuariosCompletosServices.EliminarItemgvUsuariosSeleccionadosMultiple(dataKey);
                promocionesServices.EliminarPromocionesMultiples(dataKey);
                gvUsuariosSeleccionados.DataSource = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple;
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
                foreach (var item in usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple)
                {
                    bool VariasLigas = false;
                    Guid UidLigaAsociado = Guid.NewGuid();

                    if (promocionesServices.lsLigasMultiplePromocionesModel.Where(x => x.IdUsuario == item.IdUsuario).ToList().Count >= 1)
                    {
                        VariasLigas = true;
                    }

                    if (VariasLigas)
                    {
                        DateTime thisDay = DateTime.Now;
                        string ReferenciaCobro = item.IdCliente.ToString() + item.IdUsuario.ToString() + thisDay.ToString("ddMMyyyyHHmmssfff");

                        string urlCobro = GenLigaPara(id_company, id_branch, user, pwd, ReferenciaCobro, item.DcmImporte, moneda, canal, "C", item.DtVencimiento, item.StrCorreo, item.StrConcepto, semillaAES, data0, urlGen);

                        if (urlCobro.Contains("https://"))
                        {
                            if (usuariosCompletosServices.GenerarLigasPagos(urlCobro, item.StrConcepto, item.DcmImporte, ReferenciaCobro, item.UidUsuario, txtIdentificador.Text, thisDay, item.DtVencimiento, item.StrAsunto, UidLigaAsociado, Guid.Empty, Guid.Parse(ViewState["UidClienteLocal"].ToString())))
                            {
                                resu = true;
                                foreach (var itPromo in promocionesServices.lsLigasMultiplePromocionesModel.Where(x => x.IdUsuario == item.IdUsuario).ToList())
                                {
                                    int i = promocionesServices.lsCBLPromocionesModelCliente.IndexOf(promocionesServices.lsCBLPromocionesModelCliente.First(x => x.UidPromocion == itPromo.UidPromocion));
                                    decimal cobro = promocionesServices.lsCBLPromocionesModelCliente[i].DcmComicion;

                                    decimal Valor = cobro * item.DcmImporte / 100;
                                    decimal Importe = Valor + item.DcmImporte;

                                    string promocion = itPromo.VchDescripcion.Replace(" MESES", "");

                                    DateTime thisDay2 = DateTime.Now;
                                    string Referencia = item.IdCliente.ToString() + item.IdUsuario.ToString() + thisDay2.ToString("ddMMyyyyHHmmssfff");

                                    url = GenLigaPara(id_company, id_branch, user, pwd, Referencia, Importe, moneda, canal, promocion, item.DtVencimiento, item.StrCorreo, item.StrConcepto, semillaAES, data0, urlGen);

                                    if (url.Contains("https://"))
                                    {
                                        if (usuariosCompletosServices.GenerarLigasPagos(url, item.StrConcepto, Importe, Referencia, item.UidUsuario, txtIdentificador.Text, thisDay, item.DtVencimiento, item.StrAsunto, UidLigaAsociado, itPromo.UidPromocion, Guid.Parse(ViewState["UidClienteLocal"].ToString())))
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
                            correosServices.CorreoLiga(item.NombreCompleto, item.StrAsunto, item.StrConcepto, item.DcmImporte, item.DtVencimiento, LigaUrl, item.StrCorreo, strPromociones, true, item.VchNombreComercial);
                        }
                    }
                    else
                    {
                        DateTime thisDay = DateTime.Now;
                        string ReferenciaCobro = item.IdCliente.ToString() + item.IdUsuario.ToString() + thisDay.ToString("ddMMyyyyHHmmssfff");

                        string urlCobro = GenLigaPara(id_company, id_branch, user, pwd, ReferenciaCobro, item.DcmImporte, moneda, canal, "C", item.DtVencimiento, item.StrCorreo, item.StrConcepto, semillaAES, data0, urlGen);

                        if (urlCobro.Contains("https://"))
                        {
                            if (usuariosCompletosServices.GenerarLigasPagos(urlCobro, item.StrConcepto, item.DcmImporte, ReferenciaCobro, item.UidUsuario, txtIdentificador.Text, thisDay, item.DtVencimiento, item.StrAsunto, Guid.Empty, Guid.Empty, Guid.Parse(ViewState["UidClienteLocal"].ToString())))
                            {
                                correosServices.CorreoLiga(item.NombreCompleto, item.StrAsunto, item.StrConcepto, item.DcmImporte, item.DtVencimiento, urlCobro, item.StrCorreo, "", false, item.VchNombreComercial);
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
                    lblResumen.Text = " Se han generado: " + usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple.Count.ToString() + " liga(s) exitosamente.";
                    lblCorreoUsado.Text = usuariosCompletosServices.lsgvUsuariosSeleccionadosMultiple.Count.ToString();

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

        protected void gvUsuariosSeleccionados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUsuariosSeleccionados.PageIndex = e.NewPageIndex;
            gvUsuariosSeleccionados.DataSource = usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel.Where(x => x.blSeleccionado == true).ToList();
            gvUsuariosSeleccionados.DataBind();
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
                    case "StrAsunto":
                        if (Orden == "ASC")
                        {
                            usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel = usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel.OrderBy(x => x.StrAsunto).ToList();
                        }
                        else
                        {
                            usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel = usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel.OrderByDescending(x => x.StrAsunto).ToList();
                        }
                        break;
                    case "StrConcepto":
                        if (Orden == "ASC")
                        {
                            usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel = usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel.OrderBy(x => x.StrConcepto).ToList();
                        }
                        else
                        {
                            usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel = usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel.OrderByDescending(x => x.StrConcepto).ToList();
                        }
                        break;
                    case "DcmImporte":
                        if (Orden == "ASC")
                        {
                            usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel = usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel.OrderBy(x => x.DcmImporte).ToList();
                        }
                        else
                        {
                            usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel = usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel.OrderByDescending(x => x.DcmImporte).ToList();
                        }
                        break;
                    case "DtVencimiento":
                        if (Orden == "ASC")
                        {
                            usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel = usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel.OrderBy(x => x.DtVencimiento).ToList();
                        }
                        else
                        {
                            usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel = usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel.OrderByDescending(x => x.DtVencimiento).ToList();
                        }
                        break;
                }

                gvUsuariosSeleccionados.DataSource = usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel.Where(x => x.blSeleccionado == true).ToList(); ;
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

                gvUsuarios.DataSource = usuariosCompletosServices.lsLigasMultiplesUsuariosGridViewModel.Where(x => x.blSeleccionado == false).ToList();
                gvUsuarios.DataBind();
            }
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

        protected void gvUsuariosSeleccionados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvFranquiciatarios, "Select$" + e.Row.RowIndex);


                string DataKey = gvUsuariosSeleccionados.DataKeys[e.Row.RowIndex].Values[0].ToString();

                ListBox GvListBoxMultiple = (ListBox)e.Row.FindControl("ListBoxMultiple");

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


                foreach (ListItem item in GvListBoxMultiple.Items)
                {
                    foreach (var it in promocionesServices.lsLigasMultiplePromocionesModel)
                    {
                        if (it.IdUsuario == int.Parse(DataKey))
                        {
                            if (item.Value == it.UidPromocion.ToString())
                            {
                                item.Selected = true;
                            }
                        }
                    }
                }

            }
        }
    }
}