using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PagaLaEscuela.Views
{
    public partial class Prue : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblReloj.Text = DateTime.Now.ToLongTimeString();

            if (!IsPostBack)
            {
                btnCargarExcel.Attributes.Add("onclick", "document.getElementById('" + AsyncFileUpload2.ClientID + "').click(); return false;");
                fuSelecionarExcel.Attributes["onchange"] = "UploadFile(this)";


            }
            else
            {
                    
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            lblValidar.Text = string.Empty;
            ViewState["Accion"] = "Guardar";
            btnCerrar.Visible = false;
            btnCancelar.Visible = true;
            btnGuardar.Visible = true;
            btnEditar.Visible = false;
            lblTituloModal.Text = "Registro de Usuario";
            btnGuardar.Text = "<i class=" + "material-icons>" + "check </i> Guardar";

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

                        if (dt.Columns.Contains("MATRICULA".Trim()))
                        {
                            //alumnosServices.ValidarExcelToList(dt);

                            //if (alumnosServices.lsExcelSeleccionar.Count >= 1)
                            //{
                            //    alumnosServices.AsignarColeAlumnos(alumnosServices.lsExcelSeleccionar, alumnosServices.lsSelectAlumnosGridViewModel, Guid.Parse(ViewState["UidClienteLocal"].ToString()), txtFiltroAlumNombre.Text.Trim(), txtFiltroAlumPaterno.Text.Trim(), txtFiltroAlumMaterno.Text.Trim(), txtFiltroAlumMatricula.Text.Trim());
                            //    gvAlumnos.DataSource = alumnosServices.lsAlumnosGridViewModel;
                            //    gvAlumnos.DataBind();

                            //    //gvUsuariosSeleccionados.DataSource = usuariosCompletosServices.lsgvUsuariosSeleccionados;
                            //    //gvUsuariosSeleccionados.DataBind();

                            //    //Session["PosbackExcelSimple"] = usuariosCompletosServices.lsgvUsuariosSeleccionados;
                            //}

                            //if (usuariosCompletosServices.lsLigasErrores.Count >= 1)
                            //{
                            //    btnDescargarError.Visible = true;
                            //    btnMasDetalle.Visible = false;
                            //    pnlAlertImportarError.Visible = true;
                            //    lblMnsjAlertImportarError.Text = "<strong>!Lo sentimos¡</strong> algunos usuarios no se han importado. **Recuerde que todos los campos son obligatorios**";
                            //    divAlertImportarError.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                            //}
                        }
                        else
                        {
                            //btnDescargarError.Visible = false;
                            //btnMasDetalle.Visible = true;
                            //pnlAlertImportarError.Visible = true;
                            //lblMnsjAlertImportarError.Text = "<strong>!Lo sentimos¡</strong> el archivo no tiene las columnas correctas.";
                            //divAlertImportarError.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
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

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "$('#LoginModal').modal('show')", true);
                }
            }
        }

        protected void FileUploadComplete(object sender, EventArgs e)
        {
            if (AsyncFileUpload2.HasFile)
            {
                string filePath = AsyncFileUpload2.PostedFile.FileName;
                AsyncFileUpload2.SaveAs(@"C:\Drivers\" + AsyncFileUpload2.PostedFile.FileName);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);
            }
        }

        private void UpdateTimer()
        {
            lblReloj.Text = DateTime.Now.ToLongTimeString();
        }

        protected void tmrRelojInterno_Tick1(object sender, EventArgs e)
        {
            UpdateTimer();
        }
    }
}