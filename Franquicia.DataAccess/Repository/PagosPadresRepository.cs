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

            //query.CommandText = "select cl.UidCliente, cl.VchNombreComercial, dc.Calle, dc.EntreCalle, dc.YCalle, cc.VchColonia, dc.CodigoPostal, tc.VchTelefono, (select Imagen from ImagenesClientes ic where ic.UidCliente = cl.UidCliente) as Imagen from Clientes cl, ClientesUsuarios cu, Usuarios us, DireccionesClientes dc, CatClnas cc, TelefonosClientes tc where cc.UidColonia = dc.UidColonia and tc.UidCliente = cl.UidCliente and dc.UidCliente = cl.UidCliente and cu.UidCliente = cl.UidCliente and cu.UidUsuario = us.UidUsuario and us.UidUsuario = '" + UidUsuario + "'";
            query.CommandText = "select cl.UidCliente, cl.VchNombreComercial, pa.VchPais, esta.VchEstado, mu.VchMunicipio, ciu.VchCiudad, dc.Calle, dc.EntreCalle, dc.YCalle, cc.VchColonia, dc.CodigoPostal, tc.VchTelefono, (select Imagen from ImagenesClientes ic where ic.UidCliente = cl.UidCliente) as Imagen, (select COUNT(*) from clientes cli, Colegiaturas co, FechasColegiaturas fc, ColegiaturasAlumnos ca, Alumnos al, Usuarios usu, UsuariosAlumnos ua, EstatusFechasColegiaturas efc, Periodicidades pe where pe.UidPeriodicidad = co.UidPeriodicidad and not exists (select * from LigasUrls lu, PagosTarjeta pt where pt.IdReferencia = lu.IdReferencia and pt.VchEstatus = 'approved' and lu.UidFechaColegiatura = fc.UidFechaColegiatura) and co.UidEstatus = '65E46BC9-1864-4145-AD1A-70F5B5F69739' and efc.UidEstatusFechaColegiatura = fc.UidEstatusFechaColegiatura and cli.UidCliente = co.UidCliente and co.UidColegiatura = fc.UidColegiatura and ca.UidColegiatura = co.UidColegiatura and ca.UidAlumno = al.UidAlumno and ua.UidUsuario = usu.UidUsuario and ua.UidAlumno = al.UidAlumno and cli.UidCliente = cl.UidCliente and usu.UidUsuario = us.UidUsuario) PagosDisponibles from Clientes cl, ClientesUsuarios cu, Usuarios us, DireccionesClientes dc, CatClnas cc, TelefonosClientes tc, CatPaises pa, CatEstados esta, CatMpios mu, CatCddes ciu where ciu.UidCiudad = dc.UidCiudad and mu.UidMunicipio = dc.UidMunicipio and esta.UidEstado = dc.UidEstado and pa.UidPais = dc.UidPais and cc.UidColonia = dc.UidColonia and tc.UidCliente = cl.UidCliente and dc.UidCliente = cl.UidCliente and cu.UidCliente = cl.UidCliente and cu.UidUsuario = us.UidUsuario and us.UidUsuario = '" + UidUsuario + "'";

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

                if (int.Parse(item["PagosDisponibles"].ToString()) >= 1)
                {
                    lsPadresComerciosViewModels.Add(new PadresComerciosViewModels()
                    {
                        UidCliente = Guid.Parse(item["UidCliente"].ToString()),
                        VchNombreComercial = item["VchNombreComercial"].ToString(),
                        Imagen = Image,

                        Pais = item["VchPais"].ToString(),
                        Estado = item["VchEstado"].ToString(),
                        Municipio = item["VchMunicipio"].ToString(),
                        Ciudad = item["VchCiudad"].ToString(),

                        Calle = item["Calle"].ToString(),
                        EntreCalle = item["EntreCalle"].ToString(),
                        YCalle = item["YCalle"].ToString(),
                        VchColonia = item["VchColonia"].ToString(),
                        CodigoPostal = item["CodigoPostal"].ToString(),
                        VchTelefono = item["VchTelefono"].ToString()
                    });
                }
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
