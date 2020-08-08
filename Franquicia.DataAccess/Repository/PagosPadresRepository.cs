using Franquicia.DataAccess.Common;
using Franquicia.Domain.Models;
using Franquicia.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Franquicia.DataAccess.Repository
{
    public class PagosPadresRepository : SqlDataRepository
    {
        private PadresComerciosViewModels _padresComerciosViewModels = new PadresComerciosViewModels();
        public PadresComerciosViewModels padresComerciosViewModels
        {
            get { return _padresComerciosViewModels; }
            set { _padresComerciosViewModels = value; }
        }

        #region MetodosFranquicias

        #endregion

        #region MetodosClientes
        #endregion

        #region Metodos Padres
        public List<PadresComerciosViewModels> CargarComercios(Guid UidUsuario)
        {
            List<PadresComerciosViewModels> lsPadresComerciosViewModels = new List<PadresComerciosViewModels>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select cl.UidCliente, cl.VchNombreComercial, dc.Calle, dc.EntreCalle, dc.YCalle, cc.VchColonia, dc.CodigoPostal, tc.VchTelefono, (select Imagen from ImagenesClientes ic where ic.UidCliente = cl.UidCliente) as Imagen from Clientes cl, ClientesUsuarios cu, Usuarios us, DireccionesClientes dc, CatClnas cc, TelefonosClientes tc where cc.UidColonia = dc.UidColonia and tc.UidCliente = cl.UidCliente and dc.UidCliente = cl.UidCliente and cu.UidCliente = cl.UidCliente and cu.UidUsuario = us.UidUsuario and us.UidUsuario = '" + UidUsuario + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                byte[] Image = null;

                if (!string.IsNullOrEmpty(item["Imagen"].ToString()))
                {
                    Image = (byte[])item["Imagen"];
                }
                else
                {
                    byte[] imageArray = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("/Images/SinLogo2.png"));
                    Image = imageArray;
                }

                lsPadresComerciosViewModels.Add(new PadresComerciosViewModels()
                {
                    UidCliente = Guid.Parse(item["UidCliente"].ToString()),
                    VchNombreComercial = item["VchNombreComercial"].ToString(),
                    Imagen = Image,
                    Calle = item["Calle"].ToString(),
                    EntreCalle = item["EntreCalle"].ToString(),
                    YCalle = item["YCalle"].ToString(),
                    VchColonia = item["VchColonia"].ToString(),
                    CodigoPostal = item["CodigoPostal"].ToString(),
                    VchTelefono = item["VchTelefono"].ToString()
                });
            }

            return lsPadresComerciosViewModels;
        }
        public Byte[] ImagenABytes(String ruta)
        {
            FileStream foto = new FileStream(ruta, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            Byte[] arreglo = new Byte[foto.Length];

            BinaryReader reader = new BinaryReader(foto);

            arreglo = reader.ReadBytes(Convert.ToInt32(foto.Length));

            return arreglo;

        }
        #endregion
    }
}
