using Franquicia.DataAccess.Common;
using Franquicia.Domain.Models;
using Franquicia.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Franquicia.DataAccess.Repository
{
    public class PadresRepository : SqlDataRepository
    {
        Padres _padres = new Padres();
        public Padres padres
        {
            get { return _padres; }
            set { _padres = value; }
        }

        private Franquiciatarios _franquiciatarios = new Franquiciatarios();
        public Franquiciatarios franquiciatarios
        {
            get { return _franquiciatarios; }
            set { _franquiciatarios = value; }
        }

        private Clientes _clientes = new Clientes();
        public Clientes clientes
        {
            get { return _clientes; }
            set { _clientes = value; }
        }

        private LigasUsuariosGridViewModel _ligasUsuariosGridViewModel = new LigasUsuariosGridViewModel();
        public LigasUsuariosGridViewModel ligasUsuariosGridViewModel
        {
            get { return _ligasUsuariosGridViewModel; }
            set { _ligasUsuariosGridViewModel = value; }
        }

        #region MetodosUsuarios
        public List<Padres> CargarPadres(Guid UidCliente, Guid UidTipoPerfil)
        {
            List<Padres> lsPadres = new List<Padres>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select us.*, su.VchUsuario, su.VchContrasenia, sp.VchNombre as Perfil, es.VchDescripcion, es.VchIcono, sp.UidTipoPerfilFranquicia, cl.VchNombreComercial from SegUsuarios su, SegPerfiles sp, Estatus es, ClientesUsuarios cu, Clientes cl, Usuarios us where sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and cu.UidCliente = cl.UidCliente and cu.UidUsuario = us.UidUsuario and cl.UidCliente = '" + UidCliente + "' and sp.UidTipoPerfil = '" + UidTipoPerfil + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsPadres.Add(new Padres()
                {
                    UidUsuario = new Guid(item["UidUsuario"].ToString()),
                    StrNombre = item["VchNombre"].ToString(),
                    StrApePaterno = item["VchApePaterno"].ToString(),
                    StrApeMaterno = item["VchApeMaterno"].ToString(),
                    StrCorreo = item["VchCorreo"].ToString(),
                    UidEstatus = new Guid(item["UidEstatus"].ToString()),
                    VchNombreComercial = item["VchNombreComercial"].ToString(),
                    VchUsuario = item["VchUsuario"].ToString(),
                    VchContrasenia = item["VchContrasenia"].ToString(),
                    VchNombrePerfil = item["Perfil"].ToString(),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    VchIcono = item["VchIcono"].ToString()
                });
            }

            return lsPadres;
        }
        public bool RegistrarUsuarios(Padres usuariosCompletos, Guid UidSegPerfilEscuela, TelefonosUsuarios telefonosUsuarios, Guid UidCliente)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PadresRegistrar";

                comando.Parameters.Add("@UidUsuario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidUsuario"].Value = usuariosCompletos.UidUsuario;

                comando.Parameters.Add("@StrNombre", SqlDbType.VarChar, 50);
                comando.Parameters["@StrNombre"].Value = usuariosCompletos.StrNombre;

                comando.Parameters.Add("@StrApePaterno", SqlDbType.VarChar, 50);
                comando.Parameters["@StrApePaterno"].Value = usuariosCompletos.StrApePaterno;

                comando.Parameters.Add("@StrApeMaterno", SqlDbType.VarChar, 50);
                comando.Parameters["@StrApeMaterno"].Value = usuariosCompletos.StrApeMaterno;

                comando.Parameters.Add("@StrCorreo", SqlDbType.VarChar, 50);
                comando.Parameters["@StrCorreo"].Value = usuariosCompletos.StrCorreo;

                comando.Parameters.Add("@VchUsuario", SqlDbType.VarChar, 50);
                comando.Parameters["@VchUsuario"].Value = usuariosCompletos.VchUsuario;

                comando.Parameters.Add("@VchContrasenia", SqlDbType.VarChar, 50);
                comando.Parameters["@VchContrasenia"].Value = usuariosCompletos.VchContrasenia;

                comando.Parameters.Add("@UidSegPerfil", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidSegPerfil"].Value = usuariosCompletos.UidSegPerfil;

                comando.Parameters.Add("@UidSegPerfilEscuela", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidSegPerfilEscuela"].Value = UidSegPerfilEscuela;

                //===========================TELEFONO==================================================

                comando.Parameters.Add("@VchTelefono", SqlDbType.VarChar, 50);
                comando.Parameters["@VchTelefono"].Value = telefonosUsuarios.VchTelefono;
                                
                comando.Parameters.Add("@UidPrefijo", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPrefijo"].Value = telefonosUsuarios.UidPrefijo;

                comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCliente"].Value = UidCliente;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public bool ActualizarPadres(Padres usuariosCompletos, TelefonosUsuarios telefonosUsuarios, Guid UidCliente)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_UsuarioActualizar";

                comando.Parameters.Add("@UidUsuario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidUsuario"].Value = usuariosCompletos.UidUsuario;

                comando.Parameters.Add("@StrNombre", SqlDbType.VarChar, 50);
                comando.Parameters["@StrNombre"].Value = usuariosCompletos.StrNombre;

                comando.Parameters.Add("@StrApePaterno", SqlDbType.VarChar, 50);
                comando.Parameters["@StrApePaterno"].Value = usuariosCompletos.StrApePaterno;

                comando.Parameters.Add("@StrApeMaterno", SqlDbType.VarChar, 50);
                comando.Parameters["@StrApeMaterno"].Value = usuariosCompletos.StrApeMaterno;

                comando.Parameters.Add("@StrCorreo", SqlDbType.VarChar, 50);
                comando.Parameters["@StrCorreo"].Value = usuariosCompletos.StrCorreo;

                comando.Parameters.Add("@UidEstatus", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEstatus"].Value = usuariosCompletos.UidEstatus;

                comando.Parameters.Add("@VchUsuario", SqlDbType.VarChar, 50);
                comando.Parameters["@VchUsuario"].Value = usuariosCompletos.VchUsuario;

                comando.Parameters.Add("@VchContrasenia", SqlDbType.VarChar, 50);
                comando.Parameters["@VchContrasenia"].Value = usuariosCompletos.VchContrasenia;

                comando.Parameters.Add("@UidSegPerfil", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidSegPerfil"].Value = usuariosCompletos.UidSegPerfil;

                //===========================TELEFONO==================================================

                comando.Parameters.Add("@VchTelefono", SqlDbType.VarChar, 50);
                comando.Parameters["@VchTelefono"].Value = telefonosUsuarios.VchTelefono;

                comando.Parameters.Add("@UidTipoTelefono", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoTelefono"].Value = Guid.Parse("B1055882-BCBA-4AB7-94FA-90E57647E607");

                comando.Parameters.Add("@UidPrefijo", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPrefijo"].Value = telefonosUsuarios.UidPrefijo;

                comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCliente"].Value = UidCliente;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public bool RegistrarDireccionUsuarios(Guid UidUsuario, DireccionesUsuarios direccionesUsuarios)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_UsuarioDireccionRegistrar";

                comando.Parameters.Add("@UidUsuario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidUsuario"].Value = UidUsuario;

                //===========================DIRECCION==================================================

                comando.Parameters.Add("@Identificador", SqlDbType.VarChar, 50);
                comando.Parameters["@Identificador"].Value = direccionesUsuarios.Identificador;

                comando.Parameters.Add("@UidPais", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPais"].Value = direccionesUsuarios.UidPais;

                comando.Parameters.Add("@UidEstado", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEstado"].Value = direccionesUsuarios.UidEstado;

                comando.Parameters.Add("@UidMunicipio", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidMunicipio"].Value = direccionesUsuarios.UidMunicipio;

                comando.Parameters.Add("@UidCiudad", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCiudad"].Value = direccionesUsuarios.UidCiudad;

                comando.Parameters.Add("@UidColonia", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidColonia"].Value = direccionesUsuarios.UidColonia;

                comando.Parameters.Add("@Calle", SqlDbType.VarChar, 50);
                comando.Parameters["@Calle"].Value = direccionesUsuarios.Calle;

                comando.Parameters.Add("@EntreCalle", SqlDbType.VarChar, 50);
                comando.Parameters["@EntreCalle"].Value = direccionesUsuarios.EntreCalle;

                comando.Parameters.Add("@YCalle", SqlDbType.VarChar, 50);
                comando.Parameters["@YCalle"].Value = direccionesUsuarios.YCalle;

                comando.Parameters.Add("@NumeroExterior", SqlDbType.VarChar, 50);
                comando.Parameters["@NumeroExterior"].Value = direccionesUsuarios.NumeroExterior;

                comando.Parameters.Add("@NumeroInterior", SqlDbType.VarChar, 50);
                comando.Parameters["@NumeroInterior"].Value = direccionesUsuarios.NumeroInterior;

                comando.Parameters.Add("@CodigoPostal", SqlDbType.VarChar, 30);
                comando.Parameters["@CodigoPostal"].Value = direccionesUsuarios.CodigoPostal;

                comando.Parameters.Add("@Referencia", SqlDbType.VarChar, 100);
                comando.Parameters["@Referencia"].Value = direccionesUsuarios.Referencia;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public bool ActualizarDireccionUsuarios(Guid UidUsuario, DireccionesUsuarios direccionesUsuarios)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_UsuarioDireccionActualizar";

                comando.Parameters.Add("@UidUsuario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidUsuario"].Value = UidUsuario;

                //===========================DIRECCION==================================================

                comando.Parameters.Add("@Identificador", SqlDbType.VarChar, 50);
                comando.Parameters["@Identificador"].Value = direccionesUsuarios.Identificador;

                comando.Parameters.Add("@UidPais", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPais"].Value = direccionesUsuarios.UidPais;

                comando.Parameters.Add("@UidEstado", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEstado"].Value = direccionesUsuarios.UidEstado;

                comando.Parameters.Add("@UidMunicipio", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidMunicipio"].Value = direccionesUsuarios.UidMunicipio;

                comando.Parameters.Add("@UidCiudad", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCiudad"].Value = direccionesUsuarios.UidCiudad;

                comando.Parameters.Add("@UidColonia", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidColonia"].Value = direccionesUsuarios.UidColonia;

                comando.Parameters.Add("@Calle", SqlDbType.VarChar, 50);
                comando.Parameters["@Calle"].Value = direccionesUsuarios.Calle;

                comando.Parameters.Add("@EntreCalle", SqlDbType.VarChar, 50);
                comando.Parameters["@EntreCalle"].Value = direccionesUsuarios.EntreCalle;

                comando.Parameters.Add("@YCalle", SqlDbType.VarChar, 50);
                comando.Parameters["@YCalle"].Value = direccionesUsuarios.YCalle;

                comando.Parameters.Add("@NumeroExterior", SqlDbType.VarChar, 50);
                comando.Parameters["@NumeroExterior"].Value = direccionesUsuarios.NumeroExterior;

                comando.Parameters.Add("@NumeroInterior", SqlDbType.VarChar, 50);
                comando.Parameters["@NumeroInterior"].Value = direccionesUsuarios.NumeroInterior;

                comando.Parameters.Add("@CodigoPostal", SqlDbType.VarChar, 30);
                comando.Parameters["@CodigoPostal"].Value = direccionesUsuarios.CodigoPostal;

                comando.Parameters.Add("@Referencia", SqlDbType.VarChar, 100);
                comando.Parameters["@Referencia"].Value = direccionesUsuarios.Referencia;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public bool ObtenerUsuarios(Guid UidUsuario)
        {
            bool resultado = false;
            SqlCommand comando = new SqlCommand();

            try
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.CommandText = "sp_ManejoDeSessionFranquiciaClienteUsuario";

                comando.Parameters.Add("@UidUsuario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidUsuario"].Value = UidUsuario;

                foreach (DataRow item in this.Busquedas(comando).Rows)
                {
                    padres.UidUsuario = new Guid(item["UidUsuario"].ToString());
                    padres.StrNombre = item["VchNombre"].ToString();
                    franquiciatarios.UidFranquiciatarios = new Guid(item["UidFranquiciatarios"].ToString());
                    franquiciatarios.VchNombreComercial = item["Franquicia"].ToString();
                    clientes.UidCliente = new Guid(item["UidCliente"].ToString());
                    clientes.VchNombreComercial = item["Cliente"].ToString();
                }

                resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return resultado;

        }

        public List<Padres> BuscarUsuariosFinales(Guid UidCliente, Guid UidTipoPerfil, string Nombre, string ApePaterno, string ApeMaterno, string Correo, Guid UidEstatus)
        {
            List<Padres> lsPadres = new List<Padres>();

            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "sp_UsuariosFinalesBuscar";
            try
            {
                if (UidCliente != Guid.Empty)
                {
                    comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidCliente"].Value = UidCliente;
                }
                if (UidTipoPerfil != Guid.Empty)
                {
                    comando.Parameters.Add("@UidTipoPerfil", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidTipoPerfil"].Value = UidTipoPerfil;
                }
                if (Nombre != string.Empty)
                {
                    comando.Parameters.Add("@Nombre", SqlDbType.VarChar, 50);
                    comando.Parameters["@Nombre"].Value = Nombre;
                }
                if (ApePaterno != string.Empty)
                {
                    comando.Parameters.Add("@ApePaterno", SqlDbType.VarChar, 50);
                    comando.Parameters["@ApePaterno"].Value = ApePaterno;
                }
                if (ApeMaterno != string.Empty)
                {
                    comando.Parameters.Add("@ApeMaterno", SqlDbType.VarChar, 50);
                    comando.Parameters["@ApeMaterno"].Value = ApeMaterno;
                }
                if (Correo != string.Empty)
                {
                    comando.Parameters.Add("@Correo", SqlDbType.VarChar, 50);
                    comando.Parameters["@Correo"].Value = Correo;
                }
                if (UidEstatus != Guid.Empty)
                {
                    comando.Parameters.Add("@UidEstatus", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidEstatus"].Value = UidEstatus;
                }

                foreach (DataRow item in this.Busquedas(comando).Rows)
                {
                    padres = new Padres()
                    {
                        UidUsuario = new Guid(item["UidUsuario"].ToString()),
                        StrNombre = item["VchNombre"].ToString(),
                        StrApePaterno = item["VchApePaterno"].ToString(),
                        StrApeMaterno = item["VchApeMaterno"].ToString(),
                        StrCorreo = item["VchCorreo"].ToString(),
                        UidEstatus = new Guid(item["UidEstatus"].ToString()),
                        VchUsuario = item["VchUsuario"].ToString(),
                        VchNombrePerfil = item["Perfil"].ToString(),
                        VchDescripcion = item["VchDescripcion"].ToString(),
                        VchIcono = item["VchIcono"].ToString()
                    };

                    lsPadres.Add(padres);
                }

                return lsPadres;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void AsociarUsuariosFinales(string Correo)
        {
            padres = new Padres();

            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "sp_UsuariosFinalesAsociar";
            try
            {
                if (Correo != string.Empty)
                {
                    comando.Parameters.Add("@Correo", SqlDbType.VarChar, 50);
                    comando.Parameters["@Correo"].Value = Correo;
                }

                foreach (DataRow item in this.Busquedas(comando).Rows)
                {
                    padres.UidUsuario = new Guid(item["UidUsuario"].ToString());
                    padres.StrNombre = item["VchNombre"].ToString();
                    padres.StrApePaterno = item["VchApePaterno"].ToString();
                    padres.StrApeMaterno = item["VchApeMaterno"].ToString();
                    padres.StrCorreo = item["VchCorreo"].ToString();
                    padres.UidEstatus = new Guid(item["UidEstatus"].ToString());
                    padres.VchUsuario = item["VchUsuario"].ToString();
                    padres.VchContrasenia = item["VchContrasenia"].ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool AsociarUsuarioCliente(Guid UidCliente, Guid UidUsuario)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_UsuarioAsociarClienteExcel";

                comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCliente"].Value = UidCliente;

                comando.Parameters.Add("@UidUsuario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidUsuario"].Value = UidUsuario;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        #endregion

    }
}
