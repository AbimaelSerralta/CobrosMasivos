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

namespace PagaLaEscuela.Views.Excel
{
    public partial class ExportarAExcelReportePagosPadre : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime dateTime = DateTime.Now;

                List<PagosReporteLigaPadreViewModels> ls = (List<PagosReporteLigaPadreViewModels>)Session["lsPagosReporteLigaPadreViewModels"];

                if (ls != null && ls.Count >= 1)
                {
                    gvReportePagosPadre.DataSource = ls;
                    gvReportePagosPadre.DataBind();

                    Expor(dateTime.ToString("HHmmssfff"), gvReportePagosPadre);
                }
            }
            else
            {

            }
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
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename= ReportePagos" + filename + ".xlsx");


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