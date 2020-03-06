using Franquicia.Domain.ViewModels;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Franquicia.WebForms.Views
{
    public partial class ExportarAExcelMultiple : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime dateTime = DateTime.Now;

                List<LigasMultiplesUsuariosGridViewModel> ls = (List<LigasMultiplesUsuariosGridViewModel>)Session["lsLigasMultiplesUsuariosGridViewModel"];

                if (ls != null && ls.Where(x => x.blSeleccionado == true).ToList().Count >= 1)
                {
                    gvLigasMultiples.DataSource = ls.Where(x => x.blSeleccionado == true).ToList();
                    gvLigasMultiples.DataBind();

                    Expor(dateTime.ToString("ddMMyyyyHHmmssfff"));
                }
                else
                {
                    List<LigasMultiplesUsuariosGridViewModel> l = new List<LigasMultiplesUsuariosGridViewModel>();

                    l.Add(new LigasMultiplesUsuariosGridViewModel
                    {
                        StrNombre = "",
                        DtVencimiento = dateTime,
                        DcmImporte = 0
                    });

                    gvLigasMultiples.DataSource = l;
                    gvLigasMultiples.DataBind();

                    Expor(dateTime.ToString("ddMMyyyyHHmmssfff"));
                }
            }
            else
            {

            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        public void Expor(string dateTime)
        {
            DataTable data = new DataTable();

            string header = string.Empty;
            List<string> lsHeader = new List<string>();

            for (int i = 0; i < gvLigasMultiples.Columns.Count; i++)
            {
                lsHeader.Add(gvLigasMultiples.HeaderRow.Cells[i].Text);
                header = HttpUtility.HtmlDecode(gvLigasMultiples.HeaderRow.Cells[i].Text);

                data.Columns.Add(header);

                //data.Columns.Add("column" + i.ToString());
            }
            foreach (GridViewRow row in gvLigasMultiples.Rows)
            {
                DataRow dr = data.NewRow();

                for (int j = 0; j < gvLigasMultiples.Columns.Count; j++)
                {
                    header = gvLigasMultiples.HeaderRow.Cells[j].Text;

                    dr[header] = HttpUtility.HtmlDecode(row.Cells[j].Text.Replace("&nbsp;", ""));
                }

                data.Rows.Add(dr);
            }

            using (DataTable dt = data)
            {
                ExporttoExcel(dt, dateTime);
            }
        }

        public DataTable ConvertirListaToDataTable(List<LigasMultiplesUsuariosGridViewModel> data)
        {

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(LigasMultiplesUsuariosGridViewModel));

            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties) table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (LigasMultiplesUsuariosGridViewModel item in data)
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
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename= Pagos multiples" + filename + ".xlsx");


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