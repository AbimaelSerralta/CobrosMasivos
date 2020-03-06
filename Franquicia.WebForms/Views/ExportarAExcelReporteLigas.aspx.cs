using Franquicia.Domain.ViewModels;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Franquicia.WebForms.Views
{
    public partial class ExportarAExcelReporteLigas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime dateTime = DateTime.Now;

                List<LigasUrlsGridViewModel> ls = (List<LigasUrlsGridViewModel>)Session["lsLigasUrlsGridViewModel"];

                if (ls != null && ls.Count >= 1)
                {
                    gvReporteLigas.DataSource = ls;
                    gvReporteLigas.DataBind();

                    Expor(dateTime.ToString("ddMMyyyyHHmmssfff"));
                }
                else
                {
                    List<LigasUrlsGridViewModel> l = new List<LigasUrlsGridViewModel>();

                    l.Add(new LigasUrlsGridViewModel
                    {
                        DtVencimiento = dateTime,
                        DcmImporte = 0
                    });

                    gvReporteLigas.DataSource = l;
                    gvReporteLigas.DataBind();

                    Expor(dateTime.ToString("ddMMyyyyHHmmssfff"));
                }
            }
            else
            {

            }
        }

        public void Expor(string dateTime)
        {
            DataTable data = new DataTable();

            string header = string.Empty;

            for (int i = 0; i < gvReporteLigas.Columns.Count; i++)
            {
                header = HttpUtility.HtmlDecode(gvReporteLigas.HeaderRow.Cells[i].Text);

                data.Columns.Add(header);
            }
            foreach (GridViewRow row in gvReporteLigas.Rows)
            {
                DataRow dr = data.NewRow();

                for (int j = 0; j < gvReporteLigas.Columns.Count; j++)
                {
                    header = gvReporteLigas.HeaderRow.Cells[j].Text;

                    dr[header] = HttpUtility.HtmlDecode(row.Cells[j].Text);
                }

                data.Rows.Add(dr);
            }

            using (DataTable dt = data)
            {
                ExporttoExcel(dt, dateTime);
            }
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
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename= Reporte Ligas" + filename + ".xlsx");


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