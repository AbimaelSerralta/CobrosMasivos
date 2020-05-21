using Franquicia.Domain.ViewModels;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Franquicia.WebForms.Views
{
    public partial class ExportarAExcelFranquicia : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime dateTime = DateTime.Now;

                List<LigasUsuariosGridViewModel> ls = (List<LigasUsuariosGridViewModel>)Session["lsgvUsuariosSeleccionados"];
                List<LigasUsuariosGridViewModel> lsError = (List<LigasUsuariosGridViewModel>)Session["lsLigasUsuariosGridViewModelError"];

                if (lsError != null && lsError.Count >= 1)
                {
                    gvUsuariosSelecciona.DataSource = lsError;
                    gvUsuariosSelecciona.DataBind();

                    Expor("Error " + dateTime.ToString("ddMMyyyyHHmmssfff"), gvUsuariosSelecciona);
                }
                else
                {
                    if (ls != null && ls.Where(x => x.blSeleccionado == true).ToList().Count >= 1)
                    {
                        gvUsuariosSelecciona.DataSource = ls.Where(x => x.blSeleccionado == true).ToList();
                        gvUsuariosSelecciona.DataBind();

                        Expor(dateTime.ToString("ddMMyyyyHHmmssfff"), gvUsuariosSelecciona);
                    }
                    else
                    {
                        List<LigasUsuariosGridViewModel> l = new List<LigasUsuariosGridViewModel>();

                        l.Add(new LigasUsuariosGridViewModel
                        {
                            StrNombre = ""
                        });

                        gvUsuariosSelecciona.DataSource = l;
                        gvUsuariosSelecciona.DataBind();

                        Expor(dateTime.ToString("ddMMyyyyHHmmssfff"), gvUsuariosSelecciona);
                    }
                }
            }
            else
            {

            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        public void Expor(string dateTime, GridView gridView)
        {
            DataTable data = new DataTable();

            string header = string.Empty;
            List<string> lsHeader = new List<string>();

            for (int i = 0; i < gridView.Columns.Count; i++)
            {
                lsHeader.Add(gridView.HeaderRow.Cells[i].Text);
                header = HttpUtility.HtmlDecode(gridView.HeaderRow.Cells[i].Text);

                data.Columns.Add(header);

                //data.Columns.Add("column" + i.ToString());
            }
            foreach (GridViewRow row in gridView.Rows)
            {
                DataRow dr = data.NewRow();

                for (int j = 0; j < gridView.Columns.Count; j++)
                {
                    header = gridView.HeaderRow.Cells[j].Text;

                    dr[header] = HttpUtility.HtmlDecode(row.Cells[j].Text.Replace("&nbsp;", ""));
                }

                data.Rows.Add(dr);
            }

            using (DataTable dt = data)
            {

                ExporttoExcel(dt, dateTime);

                ////Build the CSV file data as a Comma separated string.
                //string csv = string.Empty;

                //foreach (DataColumn column in dt.Columns)
                //{
                //    //Add the Header row for CSV file.
                //    csv += column.ColumnName + ',';
                //}

                ////Add new line.
                //csv += "\r\n";

                //foreach (DataRow row in dt.Rows)
                //{
                //    foreach (DataColumn column in dt.Columns)
                //    {
                //        //Add the Data rows.
                //        csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
                //    }

                //    //Add new line.
                //    csv += "\r\n";
                //}

                ////Download the CSV file.
                //Response.Clear();
                //Response.Buffer = true;
                //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //Response.AddHeader("content-disposition", "attachment;filename=SqlExport.csv");
                //Response.Charset = "";
                //Response.Output.Write(csv);
                //Response.Flush();
                //Response.End();
            }


            //using (SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog = cobrosmasivos; Integrated Security = True"))
            //{
            //    using (SqlCommand cmd = new SqlCommand("select * from usuarios"))
            //    {
            //        using (SqlDataAdapter sda = new SqlDataAdapter())
            //        {
            //            cmd.Connection = con;
            //            sda.SelectCommand = cmd;
            //            using (DataTable dt = new DataTable())
            //            {
            //                sda.Fill(dt);

            //                //Build the CSV file data as a Comma separated string.
            //                string csv = string.Empty;

            //                foreach (DataColumn column in dt.Columns)
            //                {
            //                    //Add the Header row for CSV file.
            //                    csv += column.ColumnName + ',';
            //                }

            //                //Add new line.
            //                csv += "\r\n";

            //                foreach (DataRow row in dt.Rows)
            //                {
            //                    foreach (DataColumn column in dt.Columns)
            //                    {
            //                        //Add the Data rows.
            //                        csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
            //                    }

            //                    //Add new line.
            //                    csv += "\r\n";
            //                }

            //                //Download the CSV file.
            //                Response.Clear();
            //                Response.Buffer = true;
            //                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //                Response.AddHeader("content-disposition", "attachment;filename=SqlExport.csv");
            //                Response.Charset = "";
            //                Response.Output.Write(csv);
            //                Response.Flush();
            //                Response.End();
            //            }
            //        }
            //    }
            //}
        }

        public DataTable ConvertirListaToDataTable(List<LigasUsuariosGridViewModel> data)
        {

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(LigasUsuariosGridViewModel));

            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties) table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (LigasUsuariosGridViewModel item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties) row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public void ExporttoExcel(DataTable table, string filename)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename= Pagos simples" + filename + ".xlsx");


            using (ExcelPackage pack = new ExcelPackage())
            {
                ExcelWorksheet ws = pack.Workbook.Worksheets.Add(filename);
                ws.Cells["A1"].LoadFromDataTable(table, true);
                var ms = new System.IO.MemoryStream();
                pack.SaveAs(ms);
                ms.WriteTo(HttpContext.Current.Response.OutputStream);
            }

            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();

        }
    }
}