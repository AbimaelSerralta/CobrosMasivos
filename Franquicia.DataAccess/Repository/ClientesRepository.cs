using Franquicia.DataAccess.Common;
using Franquicia.Domain.Models;
using Franquicia.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Franquicia.DataAccess.Repository
{
    public class ClientesRepository : SqlDataRepository
    {
        private ClientesGridViewModel _clientesGridViewModel = new ClientesGridViewModel();
        public ClientesGridViewModel clientesGridViewModel
        {
            get { return _clientesGridViewModel; }
            set { _clientesGridViewModel = value; }
        }
        public List<ClientesGridViewModel> CargarClientes(Guid UidFranquiciatario)
        {
            List<ClientesGridViewModel> lsClientesGridViewModel = new List<ClientesGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "Select cl.*, es.VchDescripcion, es.VchIcono from Clientes cl, Estatus es where cl.UidEstatus = es.UidEstatus and Cl.UidFranquiciatario = '" + UidFranquiciatario + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsClientesGridViewModel.Add(new ClientesGridViewModel()
                {
                    UidCliente = Guid.Parse(item["UidCliente"].ToString()),
                    VchRFC = item["VchRFC"].ToString(),
                    VchRazonSocial = item["VchRazonSocial"].ToString(),
                    VchNombreComercial = item["VchNombreComercial"].ToString(),
                    DtFechaAlta = (DateTime)item["DtFechaAlta"],
                    VchCorreoElectronico = item["VchCorreoElectronico"].ToString(),
                    UidEstatus = Guid.Parse(item["UidEstatus"].ToString()),
                    VchEstatus = item["VchDescripcion"].ToString(),
                    VchIcono = item["VchIcono"].ToString(),
                    IdCliente = int.Parse(item["IdCliente"].ToString()),
                    VchIdCliente = int.Parse(item["IdCliente"].ToString()).ToString("D6"),
                    VchIdWAySMS = item["VchIdWAySMS"].ToString(),
                    VchZonaHoraria = item["VchZonaHoraria"].ToString(),
                    BitEscuela = bool.Parse(item["BitEscuela"].ToString())
                });
            }

            return lsClientesGridViewModel;
        }

        public bool RegistrarClientes(Clientes clientes, DireccionesClientes direccionesClientes, TelefonosClientes telefonosClientes)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ClientesRegistrar";

                comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCliente"].Value = clientes.UidCliente;


                comando.Parameters.Add("@VchRFC", SqlDbType.VarChar, 50);
                comando.Parameters["@VchRFC"].Value = clientes.VchRFC;

                comando.Parameters.Add("@VchRazonSocial", SqlDbType.VarChar, 50);
                comando.Parameters["@VchRazonSocial"].Value = clientes.VchRazonSocial;

                comando.Parameters.Add("@VchNombreComercial", SqlDbType.VarChar, 50);
                comando.Parameters["@VchNombreComercial"].Value = clientes.VchNombreComercial;

                comando.Parameters.Add("@DtFechaAlta", SqlDbType.DateTime);
                comando.Parameters["@DtFechaAlta"].Value = clientes.DtFechaAlta;

                comando.Parameters.Add("@VchCorreoElectronico", SqlDbType.VarChar, 50);
                comando.Parameters["@VchCorreoElectronico"].Value = clientes.VchCorreoElectronico;

                comando.Parameters.Add("@UidFranquiciatario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidFranquiciatario"].Value = clientes.UidFranquiciatario;

                comando.Parameters.Add("@VchIdWAySMS", SqlDbType.VarChar, 50);
                comando.Parameters["@VchIdWAySMS"].Value = clientes.VchIdWAySMS;

                comando.Parameters.Add("@VchZonaHoraria", SqlDbType.VarChar, 60);
                comando.Parameters["@VchZonaHoraria"].Value = clientes.VchZonaHoraria;

                comando.Parameters.Add("@BitEscuela", SqlDbType.Bit);
                comando.Parameters["@BitEscuela"].Value = clientes.BitEscuela;

                //===========================DIRECCION==================================================

                comando.Parameters.Add("@Identificador", SqlDbType.VarChar, 50);
                comando.Parameters["@Identificador"].Value = direccionesClientes.Identificador;

                comando.Parameters.Add("@UidPais", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPais"].Value = direccionesClientes.UidPais;

                comando.Parameters.Add("@UidEstado", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEstado"].Value = direccionesClientes.UidEstado;

                comando.Parameters.Add("@UidMunicipio", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidMunicipio"].Value = direccionesClientes.UidMunicipio;

                comando.Parameters.Add("@UidCiudad", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCiudad"].Value = direccionesClientes.UidCiudad;

                comando.Parameters.Add("@UidColonia", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidColonia"].Value = direccionesClientes.UidColonia;

                comando.Parameters.Add("@Calle", SqlDbType.VarChar, 50);
                comando.Parameters["@Calle"].Value = direccionesClientes.Calle;

                comando.Parameters.Add("@EntreCalle", SqlDbType.VarChar, 50);
                comando.Parameters["@EntreCalle"].Value = direccionesClientes.EntreCalle;

                comando.Parameters.Add("@YCalle", SqlDbType.VarChar, 50);
                comando.Parameters["@YCalle"].Value = direccionesClientes.YCalle;

                comando.Parameters.Add("@NumeroExterior", SqlDbType.VarChar, 50);
                comando.Parameters["@NumeroExterior"].Value = direccionesClientes.NumeroExterior;

                comando.Parameters.Add("@NumeroInterior", SqlDbType.VarChar, 50);
                comando.Parameters["@NumeroInterior"].Value = direccionesClientes.NumeroInterior;

                comando.Parameters.Add("@CodigoPostal", SqlDbType.VarChar, 30);
                comando.Parameters["@CodigoPostal"].Value = direccionesClientes.CodigoPostal;

                comando.Parameters.Add("@Referencia", SqlDbType.VarChar, 100);
                comando.Parameters["@Referencia"].Value = direccionesClientes.Referencia;

                //===========================TELEFONO==================================================

                comando.Parameters.Add("@VchTelefono", SqlDbType.VarChar, 50);
                comando.Parameters["@VchTelefono"].Value = telefonosClientes.VchTelefono;

                comando.Parameters.Add("@UidTipoTelefono", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoTelefono"].Value = telefonosClientes.UidTipoTelefono;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public bool RegistrarLogo(Guid UidCliente, byte[] Imagen)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ClientesRegistrarLogo";

                comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCliente"].Value = UidCliente;

                comando.Parameters.Add("@Imagen", SqlDbType.VarBinary);
                comando.Parameters["@Imagen"].Value = Imagen;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public bool ActualizarLogo(Guid UidCliente, byte[] Imagen)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ClientesActualizarLogo";

                comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCliente"].Value = UidCliente;

                comando.Parameters.Add("@Imagen", SqlDbType.VarBinary);
                comando.Parameters["@Imagen"].Value = Imagen;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public byte[] CargarLogo(Guid UidCliente)
        {
            byte[] img = null;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "Select * from ImagenesClientes where  UidCliente = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                img = (byte[])item["Imagen"];
            }

            return img;
        }
        public string CargarNombre(Guid UidCliente)
        {
            string Nombre = string.Empty;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select VchNombreComercial from Clientes where UidCliente = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                Nombre = item["VchNombreComercial"].ToString();
            }

            return Nombre;
        }

        public bool ActualizarClientes(Clientes clientes, DireccionesClientes direccionesClientes, TelefonosClientes telefonosClientes)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ClientesActualizar";

                comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCliente"].Value = clientes.UidCliente;

                comando.Parameters.Add("@VchRFC", SqlDbType.VarChar, 50);
                comando.Parameters["@VchRFC"].Value = clientes.VchRFC;

                comando.Parameters.Add("@VchRazonSocial", SqlDbType.VarChar, 50);
                comando.Parameters["@VchRazonSocial"].Value = clientes.VchRazonSocial;

                comando.Parameters.Add("@VchNombreComercial", SqlDbType.VarChar, 50);
                comando.Parameters["@VchNombreComercial"].Value = clientes.VchNombreComercial;

                comando.Parameters.Add("@VchCorreoElectronico", SqlDbType.VarChar, 50);
                comando.Parameters["@VchCorreoElectronico"].Value = clientes.VchCorreoElectronico;

                comando.Parameters.Add("@UidEstatus", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEstatus"].Value = clientes.UidEstatus;

                comando.Parameters.Add("@VchIdWAySMS", SqlDbType.VarChar, 50);
                comando.Parameters["@VchIdWAySMS"].Value = clientes.VchIdWAySMS;

                comando.Parameters.Add("@VchZonaHoraria", SqlDbType.VarChar, 60);
                comando.Parameters["@VchZonaHoraria"].Value = clientes.VchZonaHoraria;

                comando.Parameters.Add("@BitEscuela", SqlDbType.Bit);
                comando.Parameters["@BitEscuela"].Value = clientes.BitEscuela;

                //===========================DIRECCION==================================================

                comando.Parameters.Add("@Identificador", SqlDbType.VarChar, 50);
                comando.Parameters["@Identificador"].Value = direccionesClientes.Identificador;

                comando.Parameters.Add("@UidPais", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPais"].Value = direccionesClientes.UidPais;

                comando.Parameters.Add("@UidEstado", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEstado"].Value = direccionesClientes.UidEstado;

                comando.Parameters.Add("@UidMunicipio", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidMunicipio"].Value = direccionesClientes.UidMunicipio;

                comando.Parameters.Add("@UidCiudad", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCiudad"].Value = direccionesClientes.UidCiudad;

                comando.Parameters.Add("@UidColonia", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidColonia"].Value = direccionesClientes.UidColonia;

                comando.Parameters.Add("@Calle", SqlDbType.VarChar, 50);
                comando.Parameters["@Calle"].Value = direccionesClientes.Calle;

                comando.Parameters.Add("@EntreCalle", SqlDbType.VarChar, 50);
                comando.Parameters["@EntreCalle"].Value = direccionesClientes.EntreCalle;

                comando.Parameters.Add("@YCalle", SqlDbType.VarChar, 50);
                comando.Parameters["@YCalle"].Value = direccionesClientes.YCalle;

                comando.Parameters.Add("@NumeroExterior", SqlDbType.VarChar, 50);
                comando.Parameters["@NumeroExterior"].Value = direccionesClientes.NumeroExterior;

                comando.Parameters.Add("@NumeroInterior", SqlDbType.VarChar, 50);
                comando.Parameters["@NumeroInterior"].Value = direccionesClientes.NumeroInterior;

                comando.Parameters.Add("@CodigoPostal", SqlDbType.VarChar, 30);
                comando.Parameters["@CodigoPostal"].Value = direccionesClientes.CodigoPostal;

                comando.Parameters.Add("@Referencia", SqlDbType.VarChar, 100);
                comando.Parameters["@Referencia"].Value = direccionesClientes.Referencia;

                //===========================TELEFONO==================================================

                comando.Parameters.Add("@VchTelefono", SqlDbType.VarChar, 50);
                comando.Parameters["@VchTelefono"].Value = telefonosClientes.VchTelefono;

                comando.Parameters.Add("@UidTipoTelefono", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoTelefono"].Value = telefonosClientes.UidTipoTelefono;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }

        #region AdminGeneral
        public List<ClientesGridViewEmpresasModel> CargarTodosClientes()
        {
            List<ClientesGridViewEmpresasModel> lsClientesGridViewEmpresasModel = new List<ClientesGridViewEmpresasModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "Select cl.*, es.VchDescripcion, es.VchIcono from Clientes cl, Estatus es where cl.UidEstatus = es.UidEstatus";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsClientesGridViewEmpresasModel.Add(new ClientesGridViewEmpresasModel()
                {
                    UidCliente = Guid.Parse(item["UidCliente"].ToString()),
                    IdCliente = int.Parse(item["IdCliente"].ToString()),
                    VchIdCliente = int.Parse(item["IdCliente"].ToString()).ToString("D6"),
                    VchRFC = item["VchRFC"].ToString(),
                    VchRazonSocial = item["VchRazonSocial"].ToString(),
                    VchNombreComercial = item["VchNombreComercial"].ToString(),
                    DtFechaAlta = (DateTime)item["DtFechaAlta"],
                    VchCorreoElectronico = item["VchCorreoElectronico"].ToString(),
                    UidEstatus = Guid.Parse(item["UidEstatus"].ToString()),
                    VchEstatus = item["VchDescripcion"].ToString(),
                    VchIcono = item["VchIcono"].ToString()
                });
            }

            return lsClientesGridViewEmpresasModel;
        }
        #endregion

        #region AdminCliente
        public void ObtenerClientes(Guid UidAdministrador)
        {
            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "Select cl.*, es.VchDescripcion, es.VchIcono from Clientes cl, Estatus es, ClientesUsuarios cu, Usuarios us where cu.UidUsuario = us.UidUsuario and cl.UidCliente = cu.UidCliente and cl.UidEstatus = es.UidEstatus and us.UidUsuario = '" + UidAdministrador + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                clientesGridViewModel = new ClientesGridViewModel()
                {
                    UidCliente = Guid.Parse(item["UidCliente"].ToString()),
                    VchRFC = item["VchRFC"].ToString(),
                    VchRazonSocial = item["VchRazonSocial"].ToString(),
                    VchNombreComercial = item["VchNombreComercial"].ToString(),
                    BitEscuela = bool.Parse(item["BitEscuela"].ToString())
                };
            }
        }
        #endregion

        #region ReportViewer
        public List<ClienteLogoViewModel> rdlcObtenerLogo(Guid UidPagoColegiatura)
        {
            List<ClienteLogoViewModel> lsClienteLogoViewModel = new List<ClienteLogoViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select ic.Imagen from ImagenesClientes ic, Clientes cl, Alumnos al, PagosColegiaturas pc, FechasPagos fp where ic.UidCliente = cl.UidCliente and cl.UidCliente = al.UidCliente and pc.UidPagoColegiatura = fp.UidPagoColegiatura and fp.UidAlumno = al.UidAlumno and pc.UidPagoColegiatura = '" + UidPagoColegiatura + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsClienteLogoViewModel.Add(new ClienteLogoViewModel()
                {
                    ByLogo = (byte[])item["Imagen"]
                });
            }

            return lsClienteLogoViewModel;
        }
        public List<ClienteLogoViewModel> rdlcObtenerCodigoBarraClubPago(string CodigoBarra)
        {
            List<ClienteLogoViewModel> lsClienteLogoViewModel = new List<ClienteLogoViewModel>();

            byte[] ByLogo;
            using (var webClient = new WebClient())
            {
                ByLogo = webClient.DownloadData(CodigoBarra);
            }

            lsClienteLogoViewModel.Add(new ClienteLogoViewModel()
            {
                ByLogo = ByLogo
            });

            return lsClienteLogoViewModel;
        }
        public List<ClienteLogoViewModel> rdlcObtenerComerciosDisponibles()
        {
            List<ClienteLogoViewModel> lsClienteLogoViewModel = new List<ClienteLogoViewModel>();

            lsClienteLogoViewModel.Add(new ClienteLogoViewModel()
            {
                ByLogo = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("/Images/ComerciosClubPago/comerciosDisponibles.png"))
            });

            return lsClienteLogoViewModel;
        }
        #endregion
    }
}
