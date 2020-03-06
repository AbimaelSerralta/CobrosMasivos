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
    public class UsuariosCompletosRepository : SqlDataRepository
    {
        UsuariosCompletos _usuarioCompleto = new UsuariosCompletos();
        public UsuariosCompletos usuarioCompleto
        {
            get { return _usuarioCompleto; }
            set { _usuarioCompleto = value; }
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


        public bool LoginUsuario(string Usuario, string Password)
        {
            bool resultado = false;
            SqlCommand comando = new SqlCommand();

            try
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.CommandText = "sp_ManejoDeSessionLogearUsuarios";

                comando.Parameters.Add("@Usuario", SqlDbType.VarChar, 20);
                comando.Parameters["@Usuario"].Value = Usuario;

                comando.Parameters.Add("@Password", SqlDbType.VarChar, 32);
                comando.Parameters["@Password"].Value = Password;

                DataTable dt = this.Busquedas(comando);

                foreach (DataRow item in dt.Rows)
                {
                    usuarioCompleto = new UsuariosCompletos()
                    {
                        UidSegPerfil = item.IsNull("UidSegPerfil") ? Guid.Empty : new Guid(item["UidSegPerfil"].ToString()),
                        UidUsuario = item.IsNull("UidUsuario") ? Guid.Empty : new Guid(item["UidUsuario"].ToString()),
                        VchUsuario = item["VchUsuario"].ToString(),
                        VchContrasenia = item["VchContrasenia"].ToString(),
                        UidModuloInicial = item.IsNull("UidModuloInicial") ? Guid.Empty : new Guid(item["UidModuloInicial"].ToString()),
                        UidUltimoModulo = item.IsNull("UidUltimoModulo") ? Guid.Empty : new Guid(item["UidUltimoModulo"].ToString()),
                        UidAppWeb = item.IsNull("UidAppWeb") ? Guid.Empty : new Guid(item["UidAppWeb"].ToString()),
                    };
                }
                resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return resultado;
        }

        public bool ObtenerEstatusdeEmpresaUsuario(Guid UidUsuario)
        {
            bool resultado = false;
            SqlCommand comando = new SqlCommand();

            try
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.CommandText = "sp_ManejoDeSessionEmpresaUsuarioActivos";

                comando.Parameters.Add("@UidUsuario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidUsuario"].Value = UidUsuario;

                foreach (DataRow item in this.Busquedas(comando).Rows)
                {
                    usuarioCompleto = new UsuariosCompletos()
                    {
                        UidEstatusEmpresa = new Guid(item["EstatusEmpresa"].ToString()),
                        UidUsuario = new Guid(item["UidUsuario"].ToString()),
                        StrNombre = item["VchNombre"].ToString(),
                        StrApePaterno = item["VchApePaterno"].ToString(),
                        UidEstatus = new Guid(item["EstatusUsuario"].ToString()),
                        UidSegPerfil = new Guid(item["UidSegPerfil"].ToString())
                    };
                }

                if (this.Busquedas(comando).Rows.Count == 0)
                {
                    SqlCommand comandoo = new SqlCommand();

                    try
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.CommandText = "sp_ManejoDeSessionClienteUsuarioActivos";


                        foreach (DataRow item in this.Busquedas(comando).Rows)
                        {
                            usuarioCompleto = new UsuariosCompletos()
                            {
                                UidEstatusEmpresa = new Guid(item["EstatusEmpresa"].ToString()),
                                UidUsuario = new Guid(item["UidUsuario"].ToString()),
                                StrNombre = item["VchNombre"].ToString(),
                                StrApePaterno = item["VchApePaterno"].ToString(),
                                UidEstatus = new Guid(item["EstatusUsuario"].ToString()),
                                UidSegPerfil = new Guid(item["UidSegPerfil"].ToString())
                            };
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }

                resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return resultado;

        }

        public bool ObtenerDatosUsuario(Guid UidUsuario)
        {
            bool resultado = false;
            SqlCommand comando = new SqlCommand();

            try
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.CommandText = "sp_ManejoDeSessionBuscarDatos";//Nombre del stored procedure


                //Preparacion de parametros
                comando.Parameters.Add("@UidUsuario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidUsuario"].Value = UidUsuario;

                DataTable dt = this.Busquedas(comando);

                foreach (DataRow item in dt.Rows)
                {
                    usuarioCompleto = new UsuariosCompletos()
                    {
                        StrNombre = item["VchNombre"].ToString(),
                        StrApePaterno = item["VchApePaterno"].ToString(),
                        UidSegPerfil = new Guid(item["UidSegPerfil"].ToString()),
                        UidEstatus = new Guid(item["UidEstatus"].ToString())
                    };
                }
                resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return resultado;
        }

        public List<UsuariosCompletos> CargarAdministradores(Guid UidTipoPerfil)
        {
            List<UsuariosCompletos> lsUsuariosCompletos = new List<UsuariosCompletos>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select us.*, su.VchUsuario, sp.VchNombre as Perfil, es.VchDescripcion, es.VchIcono from Usuarios us, SegUsuarios su, SegPerfiles sp, Estatus es where us.UidUsuario = su.UidUsuario and su.UidSegPerfil = sp.UidSegPerfil and us.UidEstatus = es.UidEstatus and sp.UidTipoPerfil = '" + UidTipoPerfil + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsUsuariosCompletos.Add(new UsuariosCompletos()
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
                });
            }

            return lsUsuariosCompletos;
        }

        public bool RegistrarAdministradores(UsuariosCompletos usuariosCompletos, DireccionesUsuarios direccionesUsuarios, TelefonosUsuarios telefonosUsuarios, Guid UidFranquicia)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_AdministradorRegistrar";

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

                //===========================TELEFONO==================================================

                comando.Parameters.Add("@VchTelefono", SqlDbType.VarChar, 50);
                comando.Parameters["@VchTelefono"].Value = telefonosUsuarios.VchTelefono;

                comando.Parameters.Add("@UidTipoTelefono", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoTelefono"].Value = telefonosUsuarios.UidTipoTelefono;

                comando.Parameters.Add("@UidFranquicia", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidFranquicia"].Value = UidFranquicia;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public bool ActualizarAdministradores(UsuariosCompletos usuariosCompletos, DireccionesUsuarios direccionesUsuarios, TelefonosUsuarios telefonosUsuarios, Guid UidFranquicia)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_AdministradorActualizar";

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

                //===========================TELEFONO==================================================

                comando.Parameters.Add("@VchTelefono", SqlDbType.VarChar, 50);
                comando.Parameters["@VchTelefono"].Value = telefonosUsuarios.VchTelefono;

                comando.Parameters.Add("@UidTipoTelefono", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoTelefono"].Value = telefonosUsuarios.UidTipoTelefono;

                comando.Parameters.Add("@UidFranquicia", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidFranquicia"].Value = UidFranquicia;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }

        public bool RegistrarAdministradoresPrincipal(UsuariosCompletos usuariosCompletos, DireccionesUsuarios direccionesUsuarios, TelefonosUsuarios telefonosUsuarios)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_AdministradorPrincipalRegistrar";

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

                //===========================TELEFONO==================================================

                comando.Parameters.Add("@VchTelefono", SqlDbType.VarChar, 50);
                comando.Parameters["@VchTelefono"].Value = telefonosUsuarios.VchTelefono;

                comando.Parameters.Add("@UidTipoTelefono", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoTelefono"].Value = telefonosUsuarios.UidTipoTelefono;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }

        public bool ActualizarAdministradoresPrincipal(UsuariosCompletos usuariosCompletos, DireccionesUsuarios direccionesUsuarios, TelefonosUsuarios telefonosUsuarios)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_AdministradorPrincipalActualizar";

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

                //===========================TELEFONO==================================================

                comando.Parameters.Add("@VchTelefono", SqlDbType.VarChar, 50);
                comando.Parameters["@VchTelefono"].Value = telefonosUsuarios.VchTelefono;

                comando.Parameters.Add("@UidTipoTelefono", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoTelefono"].Value = telefonosUsuarios.UidTipoTelefono;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }

        #region Metodos Franquicias
        public bool ObtenerFranquiciaUsuario(Guid UidUsuario)
        {
            bool resultado = false;
            SqlCommand comando = new SqlCommand();

            try
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.CommandText = "sp_ManejoDeSessionFranquiciaUsuario";

                comando.Parameters.Add("@UidUsuario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidUsuario"].Value = UidUsuario;

                foreach (DataRow item in this.Busquedas(comando).Rows)
                {
                    usuarioCompleto.UidUsuario = new Guid(item["UidUsuario"].ToString());
                    usuarioCompleto.StrNombre = item["VchNombre"].ToString();
                    franquiciatarios.UidFranquiciatarios = new Guid(item["UidFranquiciatarios"].ToString());
                    franquiciatarios.VchNombreComercial = item["VchNombreComercial"].ToString();
                }

                resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return resultado;

        }
        public List<UsuariosCompletos> CargarAdministradoresFranquicia(Guid UidFranquiciatario, Guid UidTipoPerfilFranquicia)
        {
            List<UsuariosCompletos> lsUsuariosCompletos = new List<UsuariosCompletos>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select us.*, su.VchUsuario, sp.VchNombre as Perfil, es.VchDescripcion, es.VchIcono, sp.UidTipoPerfilFranquicia from SegUsuarios su, SegPerfiles sp, Estatus es, FranquiciasUsuarios fu, Franquiciatarios fr, Usuarios us where sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and fu.UidFranquicia = fr.UidFranquiciatarios and fu.UidUsuario = us.UidUsuario and fr.UidFranquiciatarios = '" + UidFranquiciatario + "'" + " and sp.UidTipoPerfilFranquicia = '" + UidTipoPerfilFranquicia + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsUsuariosCompletos.Add(new UsuariosCompletos()
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
                });
            }

            return lsUsuariosCompletos;
        }
        public bool RegistrarAdministradoresFranquicia(UsuariosCompletos usuariosCompletos, DireccionesUsuarios direccionesUsuarios, TelefonosUsuarios telefonosUsuarios, Guid UidFranquicia)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_AdministradorFranquiciaRegistrar";

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

                //===========================TELEFONO==================================================

                comando.Parameters.Add("@VchTelefono", SqlDbType.VarChar, 50);
                comando.Parameters["@VchTelefono"].Value = telefonosUsuarios.VchTelefono;

                comando.Parameters.Add("@UidTipoTelefono", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoTelefono"].Value = telefonosUsuarios.UidTipoTelefono;

                comando.Parameters.Add("@UidFranquicia", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidFranquicia"].Value = UidFranquicia;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public bool ActualizarAdministradoresFranquicia(UsuariosCompletos usuariosCompletos, DireccionesUsuarios direccionesUsuarios, TelefonosUsuarios telefonosUsuarios, Guid UidFranquicia)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_AdministradorFranquiciaActualizar";

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

                //===========================TELEFONO==================================================

                comando.Parameters.Add("@VchTelefono", SqlDbType.VarChar, 50);
                comando.Parameters["@VchTelefono"].Value = telefonosUsuarios.VchTelefono;

                comando.Parameters.Add("@UidTipoTelefono", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoTelefono"].Value = telefonosUsuarios.UidTipoTelefono;

                comando.Parameters.Add("@UidFranquicia", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidFranquicia"].Value = UidFranquicia;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        #endregion

        #region MetodosClientes
        public List<UsuariosCompletos> CargarAdministradoresCliente(Guid UidFranquiciatario, Guid UidTipoPerfil)
        {
            List<UsuariosCompletos> lsUsuariosCompletos = new List<UsuariosCompletos>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select us.*, su.VchUsuario, sp.VchNombre as Perfil, es.VchDescripcion, es.VchIcono, sp.UidTipoPerfilFranquicia from SegUsuarios su, SegPerfiles sp, Estatus es, ClientesUsuarios cu, Clientes cl, Usuarios us where sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and cu.UidCliente = cl.UidCliente and cu.UidUsuario = us.UidUsuario and cl.UidFranquiciatario = '" + UidFranquiciatario + "' and sp.UidTipoPerfil = '" + UidTipoPerfil + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsUsuariosCompletos.Add(new UsuariosCompletos()
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
                });
            }

            return lsUsuariosCompletos;
        }
        public bool RegistrarAdministradoresCliente(UsuariosCompletos usuariosCompletos, DireccionesUsuarios direccionesUsuarios, TelefonosUsuarios telefonosUsuarios, Guid UidCliente)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_AdministradorClienteRegistrar";

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

                //===========================TELEFONO==================================================

                comando.Parameters.Add("@VchTelefono", SqlDbType.VarChar, 50);
                comando.Parameters["@VchTelefono"].Value = telefonosUsuarios.VchTelefono;

                comando.Parameters.Add("@UidTipoTelefono", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoTelefono"].Value = telefonosUsuarios.UidTipoTelefono;

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
        public bool ActualizarAdministradoresCliente(UsuariosCompletos usuariosCompletos, DireccionesUsuarios direccionesUsuarios, TelefonosUsuarios telefonosUsuarios, Guid UidCliente)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_AdministradorClienteActualizar";

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

                //===========================TELEFONO==================================================

                comando.Parameters.Add("@VchTelefono", SqlDbType.VarChar, 50);
                comando.Parameters["@VchTelefono"].Value = telefonosUsuarios.VchTelefono;

                comando.Parameters.Add("@UidTipoTelefono", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoTelefono"].Value = telefonosUsuarios.UidTipoTelefono;

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
        public bool ObtenerFranquiciaClienteUsuario(Guid UidUsuario)
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
                    usuarioCompleto.UidUsuario = new Guid(item["UidUsuario"].ToString());
                    usuarioCompleto.StrNombre = item["VchNombre"].ToString();
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
        #endregion

        #region MetodosUsuarios
        public List<UsuariosCompletos> CargarUsuariosFinales(Guid UidCliente, Guid UidTipoPerfil)
        {
            List<UsuariosCompletos> lsUsuariosCompletos = new List<UsuariosCompletos>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select us.*, su.VchUsuario, sp.VchNombre as Perfil, es.VchDescripcion, es.VchIcono, sp.UidTipoPerfilFranquicia from SegUsuarios su, SegPerfiles sp, Estatus es, ClientesUsuarios cu, Clientes cl, Usuarios us where sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and cu.UidCliente = cl.UidCliente and cu.UidUsuario = us.UidUsuario and cl.UidCliente = '" + UidCliente + "' and sp.UidTipoPerfil = '" + UidTipoPerfil + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsUsuariosCompletos.Add(new UsuariosCompletos()
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
                });
            }

            return lsUsuariosCompletos;
        }
        public bool RegistrarUsuarios(UsuariosCompletos usuariosCompletos, DireccionesUsuarios direccionesUsuarios, TelefonosUsuarios telefonosUsuarios, Guid UidCliente)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_UsuarioRegistrar";

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

                //===========================TELEFONO==================================================

                comando.Parameters.Add("@VchTelefono", SqlDbType.VarChar, 50);
                comando.Parameters["@VchTelefono"].Value = telefonosUsuarios.VchTelefono;

                comando.Parameters.Add("@UidTipoTelefono", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoTelefono"].Value = telefonosUsuarios.UidTipoTelefono;

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
        public bool ActualizarUsuarios(UsuariosCompletos usuariosCompletos, DireccionesUsuarios direccionesUsuarios, TelefonosUsuarios telefonosUsuarios, Guid UidCliente)
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

                //===========================TELEFONO==================================================

                comando.Parameters.Add("@VchTelefono", SqlDbType.VarChar, 50);
                comando.Parameters["@VchTelefono"].Value = telefonosUsuarios.VchTelefono;

                comando.Parameters.Add("@UidTipoTelefono", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoTelefono"].Value = telefonosUsuarios.UidTipoTelefono;

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
                    usuarioCompleto.UidUsuario = new Guid(item["UidUsuario"].ToString());
                    usuarioCompleto.StrNombre = item["VchNombre"].ToString();
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

        public List<UsuariosCompletos> BuscarUsuariosFinales(Guid UidCliente, Guid UidTipoPerfil, string Nombre, string ApePaterno, string ApeMaterno, string Correo, Guid UidEstatus)
        {
            List<UsuariosCompletos> lsUsuariosCompletos = new List<UsuariosCompletos>();

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
                    usuarioCompleto = new UsuariosCompletos()
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

                    lsUsuariosCompletos.Add(usuarioCompleto);
                }

                return lsUsuariosCompletos;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion



        #region MetodosExel
        #region Simple
        public List<LigasUsuariosGridViewModel> CargarUsuariosFinales(List<LigasUsuariosGridViewModel> lsLigasUsuarios, Guid UidCliente, Guid UidTipoPerfil)
        {
            List<LigasUsuariosGridViewModel> lsLigasUsuariosGridViewModel = new List<LigasUsuariosGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select us.*, cl.IdCliente, tu.VchTelefono from TelefonosUsuarios tu, SegUsuarios su, SegPerfiles sp, Estatus es, ClientesUsuarios cu, Clientes cl, Usuarios us where tu.UidUsuario = us.UidUsuario and sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and cu.UidCliente = cl.UidCliente and cu.UidUsuario = us.UidUsuario and cl.UidCliente = '" + UidCliente + "' and sp.UidTipoPerfil = '" + UidTipoPerfil + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                bool accion = false;

                foreach (var it in lsLigasUsuarios)
                {
                    if (it.IdUsuario == int.Parse(item["IdUsuario"].ToString()))
                    {
                        accion = it.blSeleccionado;
                    }
                }

                if (accion != true)
                {
                    lsLigasUsuariosGridViewModel.Add(new LigasUsuariosGridViewModel()
                    {
                        UidUsuario = new Guid(item["UidUsuario"].ToString()),
                        IdUsuario = int.Parse(item["IdUsuario"].ToString()),
                        StrNombre = item["VchNombre"].ToString(),
                        StrApePaterno = item["VchApePaterno"].ToString(),
                        StrApeMaterno = item["VchApeMaterno"].ToString(),
                        StrCorreo = item["VchCorreo"].ToString(),
                        UidEstatus = new Guid(item["UidEstatus"].ToString()),
                        StrTelefono = item["VchTelefono"].ToString(),
                        blSeleccionado = accion,
                        IdCliente = int.Parse(item["IdCliente"].ToString())
                    });
                }
            }

            return lsLigasUsuariosGridViewModel;
        }
        public List<LigasUsuariosGridViewModel> ActualizarListaUsuarios(List<LigasUsuariosGridViewModel> lsLigasUsuarios, int IdUsuario, bool accion)
        {
            List<LigasUsuariosGridViewModel> lsLigasUsuariosGridViewModel = new List<LigasUsuariosGridViewModel>();

            foreach (var item in lsLigasUsuarios)
            {
                if (item.IdUsuario == IdUsuario)
                {
                    lsLigasUsuariosGridViewModel.Add(new LigasUsuariosGridViewModel()
                    {
                        UidUsuario = item.UidUsuario,
                        IdUsuario = item.IdUsuario,
                        StrNombre = item.StrNombre,
                        StrApePaterno = item.StrApePaterno,
                        StrApeMaterno = item.StrApeMaterno,
                        StrCorreo = item.StrCorreo,
                        UidEstatus = item.UidEstatus,
                        StrTelefono = item.StrTelefono,
                        blSeleccionado = accion,
                        IdCliente = item.IdCliente
                    });
                }
                else
                {
                    lsLigasUsuariosGridViewModel.Add(new LigasUsuariosGridViewModel()
                    {
                        UidUsuario = item.UidUsuario,
                        IdUsuario = item.IdUsuario,
                        StrNombre = item.StrNombre,
                        StrApePaterno = item.StrApePaterno,
                        StrApeMaterno = item.StrApeMaterno,
                        StrCorreo = item.StrCorreo,
                        UidEstatus = item.UidEstatus,
                        StrTelefono = item.StrTelefono,
                        blSeleccionado = item.blSeleccionado,
                        IdCliente = item.IdCliente
                    });
                }


            }


            return lsLigasUsuariosGridViewModel;
        }
        public List<LigasUsuariosGridViewModel> ExcelToList(List<LigasUsuariosGridViewModel> lsLigasUsuariosGridView, List<LigasUsuariosGridViewModel> lsLigasInsertar, Guid UidCliente)
        {
            bool accion = true;
            List<LigasUsuariosGridViewModel> lsLigasUsuariosGridViewModel = new List<LigasUsuariosGridViewModel>();

            foreach (var item in lsLigasInsertar)
            {
                string Usuario = string.Empty;
                string Password = string.Empty;

                SqlCommand query = new SqlCommand();
                query.CommandType = CommandType.Text;

                query.CommandText = "select * from usuarios where VchCorreo = '" + item.StrCorreo.ToString() + "'";

                DataTable exist = this.Busquedas(query);

                if (exist.Rows.Count >= 1)
                {

                }
                else
                {
                    if (!string.IsNullOrEmpty(item.StrNombre) && !string.IsNullOrEmpty(item.StrApePaterno))
                    {
                        string[] Descripcion = Regex.Split(item.StrNombre.Trim().ToUpper(), " ");
                        int numMax = Descripcion.Length;
                        Usuario = Descripcion[numMax - 1].Substring(0, 1).ToString() + "." + item.StrApePaterno.Trim().ToUpper();

                        SqlCommand quer = new SqlCommand();
                        quer.CommandType = CommandType.Text;
                        quer.CommandText = "select VchUsuario from SegUsuarios where VchUsuario = '" + Usuario + "'";
                        DataTable existUser = this.Busquedas(quer);

                        if (existUser.Rows.Count >= 1)
                        {
                            Usuario = Descripcion[numMax - 1].Substring(0, 1).ToString() + "." + item.StrApeMaterno.Trim().ToUpper();

                            SqlCommand que = new SqlCommand();
                            que.CommandType = CommandType.Text;
                            que.CommandText = "select VchUsuario from SegUsuarios where VchUsuario = '" + Usuario + "'";
                            DataTable existUse = this.Busquedas(que);

                            if (existUse.Rows.Count >= 1)
                            {
                                DateTime dateTime = DateTime.Now;

                                Usuario = Descripcion[numMax - 1].Substring(0, 1).ToString() + "." + item.StrApePaterno.Trim().ToUpper() + dateTime.ToString("mmssff");
                            }
                        }
                    }

                    Random obj = new Random();
                    string posibles = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                    int longitud = posibles.Length;
                    char letra;
                    int longitudnuevacadena = 8;
                    for (int i = 0; i < longitudnuevacadena; i++)
                    {
                        letra = posibles[obj.Next(longitud)];
                        Password += letra.ToString();
                    }


                    if (!string.IsNullOrEmpty(item.StrCorreo))
                    {
                        RegistrarUsuariosExcel(
                            new UsuariosCompletos
                            {
                                UidUsuario = Guid.NewGuid(),
                                StrNombre = item.StrNombre.Trim().ToUpper(),
                                StrApePaterno = item.StrApePaterno.Trim().ToUpper(),
                                StrApeMaterno = item.StrApeMaterno.Trim().ToUpper(),
                                StrCorreo = item.StrCorreo.Trim().ToUpper(),
                                VchUsuario = Usuario,
                                VchContrasenia = Password
                            },
                            new TelefonosUsuarios
                            {
                                UidTipoTelefono = new Guid("B1055882-BCBA-4AB7-94FA-90E57647E607"),
                                VchTelefono = item.StrTelefono
                            },
                            UidCliente);
                    }
                }
            }

            lsLigasUsuariosGridViewModel = RecuperarUsuariosExcel(lsLigasUsuariosGridView, lsLigasInsertar);
            return lsLigasUsuariosGridViewModel;
        }
        public bool RegistrarUsuariosExcel(UsuariosCompletos usuariosCompletos, TelefonosUsuarios telefonosUsuarios, Guid UidCliente)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_UsuarioRegistrarExcel";

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

                //===========================TELEFONO==================================================

                comando.Parameters.Add("@VchTelefono", SqlDbType.VarChar, 50);
                comando.Parameters["@VchTelefono"].Value = telefonosUsuarios.VchTelefono;

                comando.Parameters.Add("@UidTipoTelefono", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoTelefono"].Value = telefonosUsuarios.UidTipoTelefono;

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
        public List<LigasUsuariosGridViewModel> RecuperarUsuariosExcel(List<LigasUsuariosGridViewModel> lsLigasUsuariosGridView, List<LigasUsuariosGridViewModel> lsLigasInsertar)
        {
            List<LigasUsuariosGridViewModel> lsLigasUsuariosGridViewModel = new List<LigasUsuariosGridViewModel>();

            lsLigasUsuariosGridViewModel = lsLigasUsuariosGridView;

            foreach (var item in lsLigasInsertar)
            {
                SqlCommand query = new SqlCommand();
                query.CommandType = CommandType.Text;

                query.CommandText = "select us.*, cl.IdCliente, tu.VchTelefono from TelefonosUsuarios tu, SegUsuarios su, SegPerfiles sp, Estatus es, ClientesUsuarios cu, Clientes cl, Usuarios us where tu.UidUsuario = us.UidUsuario and sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and cu.UidCliente = cl.UidCliente and cu.UidUsuario = us.UidUsuario and us.VchCorreo = '" + item.StrCorreo + "'";

                DataTable dt = this.Busquedas(query);

                foreach (DataRow us in dt.Rows)
                {
                    if (!lsLigasUsuariosGridViewModel.Exists(x => x.StrCorreo == us["VchCorreo"].ToString()))
                    {
                        lsLigasUsuariosGridViewModel.Add(new LigasUsuariosGridViewModel()
                        {
                            UidUsuario = new Guid(us["UidUsuario"].ToString()),
                            IdUsuario = int.Parse(us["IdUsuario"].ToString()),
                            StrNombre = us["VchNombre"].ToString(),
                            StrApePaterno = us["VchApePaterno"].ToString(),
                            StrApeMaterno = us["VchApeMaterno"].ToString(),
                            StrCorreo = us["VchCorreo"].ToString(),
                            UidEstatus = new Guid(us["UidEstatus"].ToString()),
                            StrTelefono = us["VchTelefono"].ToString(),
                            blSeleccionado = true,
                            IdCliente = int.Parse(us["IdCliente"].ToString())
                        });
                    }
                    else
                    {

                    }
                }
            }

            return lsLigasUsuariosGridViewModel;
        }
        #endregion

        #region Multiple
        public List<LigasMultiplesUsuariosGridViewModel> CargarUsuariosFinalesMultiples(List<LigasMultiplesUsuariosGridViewModel> lsLigasUsuarios, Guid UidCliente, Guid UidTipoPerfil)
        {
            List<LigasMultiplesUsuariosGridViewModel> lsLigasMultiplesUsuariosGridViewModel = new List<LigasMultiplesUsuariosGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select us.*, cl.IdCliente, tu.VchTelefono from TelefonosUsuarios tu, SegUsuarios su, SegPerfiles sp, Estatus es, ClientesUsuarios cu, Clientes cl, Usuarios us where tu.UidUsuario = us.UidUsuario and sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and cu.UidCliente = cl.UidCliente and cu.UidUsuario = us.UidUsuario and cl.UidCliente = '" + UidCliente + "' and sp.UidTipoPerfil = '" + UidTipoPerfil + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                bool accion = false;
                string Asunto = string.Empty;
                string Concepto = string.Empty;
                Decimal Importe = 0;
                DateTime Vencimiento = DateTime.Now;

                foreach (var it in lsLigasUsuarios)
                {
                    if (it.IdUsuario == int.Parse(item["IdUsuario"].ToString()))
                    {
                        accion = it.blSeleccionado;
                        Asunto = it.StrAsunto;
                        Concepto = it.StrConcepto;
                        Importe = it.DcmImporte;
                        Vencimiento = it.DtVencimiento;
                    }
                }

                lsLigasMultiplesUsuariosGridViewModel.Add(new LigasMultiplesUsuariosGridViewModel()
                {
                    UidUsuario = new Guid(item["UidUsuario"].ToString()),
                    IdUsuario = int.Parse(item["IdUsuario"].ToString()),
                    StrNombre = item["VchNombre"].ToString(),
                    StrApePaterno = item["VchApePaterno"].ToString(),
                    StrApeMaterno = item["VchApeMaterno"].ToString(),
                    StrCorreo = item["VchCorreo"].ToString(),
                    UidEstatus = new Guid(item["UidEstatus"].ToString()),
                    StrTelefono = item["VchTelefono"].ToString(),
                    blSeleccionado = accion,
                    IdCliente = int.Parse(item["IdCliente"].ToString()),
                    StrAsunto = Asunto,
                    StrConcepto = Concepto,
                    DcmImporte = Importe,
                    DtVencimiento = Vencimiento
                });
            }

            return lsLigasMultiplesUsuariosGridViewModel;
        }
        public List<LigasMultiplesUsuariosGridViewModel> ExcelToListMultiple(DataTable dt, Guid UidCliente)
        {
            bool accion = true;
            List<LigasMultiplesUsuariosGridViewModel> lsLigasMultiplesUsuariosGridViewModel = new List<LigasMultiplesUsuariosGridViewModel>();

            foreach (DataRow item in dt.Rows)
            {
                string Usuario = string.Empty;
                string Password = string.Empty;

                SqlCommand query = new SqlCommand();
                query.CommandType = CommandType.Text;

                query.CommandText = "select * from usuarios where VchCorreo = '" + item["CORREO"].ToString() + "'";

                DataTable exist = this.Busquedas(query);

                if (exist.Rows.Count >= 1)
                {

                }
                else
                {
                    if (!string.IsNullOrEmpty(item["CORREO"].ToString()))
                    {
                        if (!string.IsNullOrEmpty(item["NOMBRE(S)"].ToString()) && !string.IsNullOrEmpty(item["APEPATERNO"].ToString()))
                        {
                            string[] Descripcion = Regex.Split(item["NOMBRE(S)"].ToString().Trim(), " ");

                            int numMax = Descripcion.Length;

                            Usuario = Descripcion[numMax - 1].Substring(0, 1).ToString() + "." + item["APEPATERNO"].ToString();
                        }

                        Random obj = new Random();
                        string posibles = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                        int longitud = posibles.Length;
                        char letra;
                        int longitudnuevacadena = 8;
                        for (int i = 0; i < longitudnuevacadena; i++)
                        {
                            letra = posibles[obj.Next(longitud)];
                            Password += letra.ToString();
                        }

                        RegistrarUsuariosExcel(
                            new UsuariosCompletos
                            {
                                UidUsuario = Guid.NewGuid(),
                                StrNombre = item["NOMBRE(S)"].ToString(),
                                StrApePaterno = item["APEPATERNO"].ToString(),
                                StrApeMaterno = item["APEMATERNO"].ToString(),
                                StrCorreo = item["CORREO"].ToString(),
                                VchUsuario = Usuario,
                                VchContrasenia = Password
                            },
                            new TelefonosUsuarios
                            {
                                UidTipoTelefono = new Guid("B1055882-BCBA-4AB7-94FA-90E57647E607"),
                                VchTelefono = item["TELEFONO"].ToString()
                            },
                            UidCliente);
                    }
                }
            }

            lsLigasMultiplesUsuariosGridViewModel = RecuperarUsuariosExcelMultiple(dt);
            return lsLigasMultiplesUsuariosGridViewModel;
        }
        public List<LigasMultiplesUsuariosGridViewModel> RecuperarUsuariosExcelMultiple(DataTable data)
        {
            List<LigasMultiplesUsuariosGridViewModel> lsLigasMultiplesUsuariosGridViewModel = new List<LigasMultiplesUsuariosGridViewModel>();

            foreach (DataRow item in data.Rows)
            {
                SqlCommand query = new SqlCommand();
                query.CommandType = CommandType.Text;

                query.CommandText = "select us.*, cl.IdCliente, tu.VchTelefono from TelefonosUsuarios tu, SegUsuarios su, SegPerfiles sp, Estatus es, ClientesUsuarios cu, Clientes cl, Usuarios us where tu.UidUsuario = us.UidUsuario and sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and cu.UidCliente = cl.UidCliente and cu.UidUsuario = us.UidUsuario and us.VchCorreo = '" + item["CORREO"].ToString() + "'";

                DataTable dt = this.Busquedas(query);

                foreach (DataRow us in dt.Rows)
                {
                    lsLigasMultiplesUsuariosGridViewModel.Add(new LigasMultiplesUsuariosGridViewModel()
                    {
                        UidUsuario = new Guid(us["UidUsuario"].ToString()),
                        IdUsuario = int.Parse(us["IdUsuario"].ToString()),
                        StrNombre = us["VchNombre"].ToString(),
                        StrApePaterno = us["VchApePaterno"].ToString(),
                        StrApeMaterno = us["VchApeMaterno"].ToString(),
                        StrCorreo = us["VchCorreo"].ToString(),
                        UidEstatus = new Guid(us["UidEstatus"].ToString()),
                        StrTelefono = us["VchTelefono"].ToString(),
                        blSeleccionado = true,
                        IdCliente = int.Parse(us["IdCliente"].ToString()),

                        StrAsunto = item["ASUNTO"].ToString(),
                        StrConcepto = item["CONCEPTO"].ToString(),
                        DcmImporte = decimal.Parse(item["IMPORTE"].ToString()),
                        DtVencimiento = DateTime.Parse(item["VENCIMIENTO"].ToString()),
                    });
                }
            }

            return lsLigasMultiplesUsuariosGridViewModel;
        }
        public List<LigasMultiplesUsuariosGridViewModel> ActualizarListaUsuariosMultiple(List<LigasMultiplesUsuariosGridViewModel> lsLigasUsuarios, int IdUsuario, bool accion)
        {
            List<LigasMultiplesUsuariosGridViewModel> lsLigasMultiplesUsuariosGridViewModel = new List<LigasMultiplesUsuariosGridViewModel>();

            foreach (var item in lsLigasUsuarios)
            {
                if (item.IdUsuario == IdUsuario)
                {
                    lsLigasMultiplesUsuariosGridViewModel.Add(new LigasMultiplesUsuariosGridViewModel()
                    {
                        UidUsuario = item.UidUsuario,
                        IdUsuario = item.IdUsuario,
                        StrNombre = item.StrNombre,
                        StrApePaterno = item.StrApePaterno,
                        StrApeMaterno = item.StrApeMaterno,
                        StrCorreo = item.StrCorreo,
                        UidEstatus = item.UidEstatus,
                        StrTelefono = item.StrTelefono,
                        blSeleccionado = accion,
                        IdCliente = item.IdCliente,
                        StrAsunto = item.StrAsunto,
                        StrConcepto = item.StrConcepto,
                        DcmImporte = item.DcmImporte,
                        DtVencimiento = item.DtVencimiento
                    });
                }
                else
                {
                    lsLigasMultiplesUsuariosGridViewModel.Add(new LigasMultiplesUsuariosGridViewModel()
                    {
                        UidUsuario = item.UidUsuario,
                        IdUsuario = item.IdUsuario,
                        StrNombre = item.StrNombre,
                        StrApePaterno = item.StrApePaterno,
                        StrApeMaterno = item.StrApeMaterno,
                        StrCorreo = item.StrCorreo,
                        UidEstatus = item.UidEstatus,
                        StrTelefono = item.StrTelefono,
                        blSeleccionado = item.blSeleccionado,
                        IdCliente = item.IdCliente,
                        StrAsunto = item.StrAsunto,
                        StrConcepto = item.StrConcepto,
                        DcmImporte = item.DcmImporte,
                        DtVencimiento = item.DtVencimiento
                    });
                }
            }
            return lsLigasMultiplesUsuariosGridViewModel;
        }
        public List<LigasMultiplesUsuariosGridViewModel> ActualizarListaGvUsuariosMultiple(List<LigasMultiplesUsuariosGridViewModel> lsLigasUsuarios, int IdUsuario, bool accion, string Asunto, string Concepto, decimal Importe, DateTime Vencimiento)
        {
            List<LigasMultiplesUsuariosGridViewModel> lsLigasMultiplesUsuariosGridViewModel = new List<LigasMultiplesUsuariosGridViewModel>();

            foreach (var item in lsLigasUsuarios)
            {
                if (item.IdUsuario == IdUsuario)
                {
                    lsLigasMultiplesUsuariosGridViewModel.Add(new LigasMultiplesUsuariosGridViewModel()
                    {
                        UidUsuario = item.UidUsuario,
                        IdUsuario = item.IdUsuario,
                        StrNombre = item.StrNombre,
                        StrApePaterno = item.StrApePaterno,
                        StrApeMaterno = item.StrApeMaterno,
                        StrCorreo = item.StrCorreo,
                        UidEstatus = item.UidEstatus,
                        StrTelefono = item.StrTelefono,
                        blSeleccionado = accion,
                        IdCliente = item.IdCliente,
                        StrAsunto = Asunto,
                        StrConcepto = Concepto,
                        DcmImporte = Importe,
                        DtVencimiento = Vencimiento
                    });
                }
                else
                {
                    lsLigasMultiplesUsuariosGridViewModel.Add(new LigasMultiplesUsuariosGridViewModel()
                    {
                        UidUsuario = item.UidUsuario,
                        IdUsuario = item.IdUsuario,
                        StrNombre = item.StrNombre,
                        StrApePaterno = item.StrApePaterno,
                        StrApeMaterno = item.StrApeMaterno,
                        StrCorreo = item.StrCorreo,
                        UidEstatus = item.UidEstatus,
                        StrTelefono = item.StrTelefono,
                        blSeleccionado = item.blSeleccionado,
                        IdCliente = item.IdCliente,
                        StrAsunto = item.StrAsunto,
                        StrConcepto = item.StrConcepto,
                        DcmImporte = item.DcmImporte,
                        DtVencimiento = item.DtVencimiento
                    });
                }
            }
            return lsLigasMultiplesUsuariosGridViewModel;
        }
        public List<LigasMultiplesUsuariosGridViewModel> BuscarUsuarios(List<LigasMultiplesUsuariosGridViewModel> lsLigasUsuarios, Guid UidCliente, Guid UidTipoPerfil, string Nombre, string ApePaterno, string ApeMaterno, string Correo, string Telefono)
        {
            List<LigasMultiplesUsuariosGridViewModel> lsLigasMultiplesUsuariosGridViewModel = new List<LigasMultiplesUsuariosGridViewModel>();

            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "sp_UsuariosBuscar";
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
                if (Telefono != string.Empty)
                {
                    comando.Parameters.Add("@Telefono", SqlDbType.VarChar, 50);
                    comando.Parameters["@Telefono"].Value = Telefono;
                }

                foreach (DataRow item in this.Busquedas(comando).Rows)
                {
                    bool accion = false;
                    string Asunto = string.Empty;
                    string Concepto = string.Empty;
                    Decimal Importe = 0;
                    DateTime Vencimiento = DateTime.Now;

                    foreach (var it in lsLigasUsuarios)
                    {
                        if (it.IdUsuario == int.Parse(item["IdUsuario"].ToString()))
                        {
                            accion = it.blSeleccionado;
                            Asunto = it.StrAsunto;
                            Concepto = it.StrConcepto;
                            Importe = it.DcmImporte;
                            Vencimiento = it.DtVencimiento;
                        }
                    }

                    lsLigasMultiplesUsuariosGridViewModel.Add(new LigasMultiplesUsuariosGridViewModel()
                    {
                        UidUsuario = new Guid(item["UidUsuario"].ToString()),
                        IdUsuario = int.Parse(item["IdUsuario"].ToString()),
                        StrNombre = item["VchNombre"].ToString(),
                        StrApePaterno = item["VchApePaterno"].ToString(),
                        StrApeMaterno = item["VchApeMaterno"].ToString(),
                        StrCorreo = item["VchCorreo"].ToString(),
                        UidEstatus = new Guid(item["UidEstatus"].ToString()),
                        StrTelefono = item["VchTelefono"].ToString(),
                        blSeleccionado = accion,
                        IdCliente = int.Parse(item["IdCliente"].ToString()),
                        StrAsunto = Asunto,
                        StrConcepto = Concepto,
                        DcmImporte = Importe,
                        DtVencimiento = Vencimiento
                    });
                }

                return lsLigasMultiplesUsuariosGridViewModel;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public bool GenerarLigasPagos(Guid UidLigaUrl, string VchUrl, string VchConcepto, decimal DcmImporte, string IdReferencia, Guid UidUsuario, string VchIdentificador, DateTime DtRegistro, DateTime DtVencimiento, string VchAsunto)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_LigasRegistrar";

                comando.Parameters.Add("@UidLigaUrl", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidLigaUrl"].Value = UidLigaUrl;

                comando.Parameters.Add("@VchUrl", SqlDbType.VarChar, 100);
                comando.Parameters["@VchUrl"].Value = VchUrl;

                comando.Parameters.Add("@VchConcepto", SqlDbType.VarChar, 100);
                comando.Parameters["@VchConcepto"].Value = VchConcepto;

                comando.Parameters.Add("@DcmImporte", SqlDbType.Decimal);
                comando.Parameters["@DcmImporte"].Value = DcmImporte;

                comando.Parameters.Add("@IdReferencia", SqlDbType.VarChar, 100);
                comando.Parameters["@IdReferencia"].Value = IdReferencia;

                comando.Parameters.Add("@UidUsuario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidUsuario"].Value = UidUsuario;

                comando.Parameters.Add("@VchIdentificador", SqlDbType.VarChar, 50);
                comando.Parameters["@VchIdentificador"].Value = VchIdentificador;

                comando.Parameters.Add("@DtRegistro", SqlDbType.DateTime);
                comando.Parameters["@DtRegistro"].Value = DtRegistro;

                comando.Parameters.Add("@DtVencimiento", SqlDbType.DateTime);
                comando.Parameters["@DtVencimiento"].Value = DtVencimiento;

                comando.Parameters.Add("@VchAsunto", SqlDbType.VarChar, 50);
                comando.Parameters["@VchAsunto"].Value = VchAsunto;

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
