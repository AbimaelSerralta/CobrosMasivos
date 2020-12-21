using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace PagaLaEscuela.Util
{
    public static class WebControlUtils
    {
        /// <summary>
        /// validate if a textbox its empty
        /// </summary>
        /// <param name="textBox"></param>
        /// <returns></returns>
        public static bool EmptyTextBox(this TextBox textBox)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                // set border to red
                textBox.BackColor = System.Drawing.Color.FromName("#f2dede");
                return true;
            }
            else
            {
                // set border to default border color
                textBox.BackColor = System.Drawing.Color.Transparent;
                return false;
            }
        }

        public static bool IsNumericTextBox(this TextBox textBox)
        {
            double number = 0;
            if (double.TryParse(textBox.Text.Trim(), out number))
            {
                // set border to default border color
                textBox.BorderColor = System.Drawing.Color.FromArgb(206, 212, 218);
                return false;
            }
            else
            {
                // set border to red
                textBox.BorderColor = System.Drawing.Color.Red;
                return true;
            }
        }

        public static bool EmptyDropDownList(this DropDownList dropDownList)
        {
            if (dropDownList.SelectedValue == "Seleccione" || dropDownList.SelectedValue == string.Empty)
            {
                // set border to red
                dropDownList.BackColor = System.Drawing.Color.FromName("#f2dede");
                return true;
            }
            else
            {
                // set border to default border color
                dropDownList.BackColor = System.Drawing.Color.Transparent;
                return false;
            }
        }
    }
}