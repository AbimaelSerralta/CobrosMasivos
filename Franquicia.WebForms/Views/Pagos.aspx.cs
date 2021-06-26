using Franquicia.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Franquicia.WebForms.Views
{
    public partial class Pagos : System.Web.UI.Page
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

        LigasUrlsServices ligasUrlsService = new LigasUrlsServices();
        PromocionesServices promocionesServices = new PromocionesServices();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["Id"] != null)
                {
                    ligasUrlsService.ContruirLiga(Guid.Parse(Request.QueryString["Id"]));

                    lblNombreComp.Text = "&nbsp;" + ligasUrlsService.ligasUrlsRepository.ligasUrlsConstruirLigaModel.NombreCompleto;
                    lblNombreComercial.Text = "&nbsp;" + ligasUrlsService.ligasUrlsRepository.ligasUrlsConstruirLigaModel.VchNombreComercial;
                    lblConcepto.Text = "&nbsp;" + ligasUrlsService.ligasUrlsRepository.ligasUrlsConstruirLigaModel.VchConcepto;
                    lblImporte.Text = "&nbsp;$" + ligasUrlsService.ligasUrlsRepository.ligasUrlsConstruirLigaModel.DcmTotal.ToString("N2");
                    lblVencimiento.Text = "&nbsp;" + ligasUrlsService.ligasUrlsRepository.ligasUrlsConstruirLigaModel.DtVencimiento.ToString("dd/MM/yyyy");
                    lblPagar.Text = "Pagar $" + ligasUrlsService.ligasUrlsRepository.ligasUrlsConstruirLigaModel.DcmTotal.ToString("N2");

                    aPagar.HRef = ligasUrlsService.ligasUrlsRepository.ligasUrlsConstruirLigaModel.VchUrl;

                    if (ligasUrlsService.ligasUrlsRepository.ligasUrlsConstruirLigaModel.UidLigaAsociado != Guid.Empty)
                    {
                        promocionesServices.CargarPromocionesValidas(ligasUrlsService.ligasUrlsRepository.ligasUrlsConstruirLigaModel.UidLigaAsociado);
                        string strPromociones = string.Empty;
                        if (promocionesServices.lsLigasUrlsPromocionesModel.Count >= 1)
                        {
                            foreach (var itPromo in promocionesServices.lsLigasUrlsPromocionesModel)
                            {
                                decimal promocion = int.Parse(itPromo.VchDescripcion.Replace(" MESES", ""));
                                decimal Final = itPromo.DcmTotal / promocion;

                                strPromociones +=
                                    "\t\t\t\t\t\t\t\t<tr>\r\n" +
                                    "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 40px;text-align: right;\">\r\n" +
                                    "\t\t\t\t\t\t\t\t\t\t" + itPromo.VchDescripcion + " de $" + Final.ToString("N2") + "\r\n" +
                                    "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                                    "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 40px;text-align: center;\">\r\n" +
                                    "\t\t\t\t\t\t\t\t\t\t &nbsp;" + "<a style =\"color:#fff;font-weight:400;text-align:center;width:100px;font-size:15px;text-decoration:none;background:#28a745;margin:0 auto; padding:5px;\" href=" + URLBase + "Views/Promociones.aspx?CodigoPromocion=" + ligasUrlsService.ligasUrlsRepository.ligasUrlsConstruirLigaModel.UidLigaAsociado + "&CodigoLiga=" + itPromo.IdReferencia + "> Pagar $" + itPromo.DcmTotal.ToString("N2") + "</a>" + "\r\n" +
                                    "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                                    "\t\t\t\t\t\t\t\t</tr>\r\n";
                            }

                            ltlPromociones.Text = strPromociones;
                            pnlPromociones.Visible = true;
                        }
                    }
                }
            }
            else
            {

            }
        }
    }
}