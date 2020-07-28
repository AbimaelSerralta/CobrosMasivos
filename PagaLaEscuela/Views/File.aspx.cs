using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PagaLaEscuela.Views
{
    public partial class File : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnClick_Click(object sender, EventArgs e)
        {
            if (AsyncFileUpload1.HasFile)
            {
                AsyncFileUpload1.SaveAs(@"C:\Drivers\" + AsyncFileUpload1.PostedFile.FileName);
            }
        }

        protected void UploadComplete(object sender, AjaxControlToolkit.AjaxFileUploadEventArgs e)
        {
            string path = Server.MapPath("~/Uploads/") + e.FileName;
            AjaxFileUpload1.SaveAs(path);
        }

        protected void FileUploadComplete(object sender, EventArgs e)
        {
            if (AsyncFileUpload2.HasFile)
            {
                string filePath = AsyncFileUpload2.PostedFile.FileName;
                AsyncFileUpload2.SaveAs(@"C:\Drivers\" + AsyncFileUpload2.PostedFile.FileName);
            }
        }
    }
}