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

        ValidacionesRepository _validacionesRepository = new ValidacionesRepository();
        public ValidacionesRepository validacionesRepository
        {
            get { return _validacionesRepository; }
            set { _validacionesRepository = value; }
        }

        CorreosRepository _correosRepository = new CorreosRepository();
        public CorreosRepository correosRepository
        {
            get { return _correosRepository; }
            set { _correosRepository = value; }
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

                if (Usuario.Contains("@"))
                {
                    comando.Parameters.Add("@Correo", SqlDbType.VarChar, 50);
                    comando.Parameters["@Correo"].Value = Usuario;
                }
                else
                {
                    comando.Parameters.Add("@Usuario", SqlDbType.VarChar, 30);
                    comando.Parameters["@Usuario"].Value = Usuario;
                }

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

            //query.CommandText = "select us.*, su.VchUsuario, sp.VchNombre as Perfil, es.VchDescripcion, es.VchIcono from Usuarios us, SegUsuarios su, SegPerfiles sp, Estatus es where sp.UidSegPerfil != '17BB8F08-9D5F-425C-9B9B-1CA230C07C7F' and us.UidUsuario = su.UidUsuario and su.UidSegPerfil = sp.UidSegPerfil and us.UidEstatus = es.UidEstatus and sp.UidTipoPerfil = '" + UidTipoPerfil + "'";
            query.CommandText = "select us.*, su.VchUsuario, su.VchContrasenia, sp.VchNombre as Perfil, es.VchDescripcion, es.VchIcono, fr.VchNombreComercial from Usuarios us, SegUsuarios su, SegPerfiles sp, Estatus es, Franquiciatarios fr, FranquiciasUsuarios fu where fr.UidFranquiciatarios = fu.UidFranquicia and fu.UidUsuario = us.UidUsuario and sp.UidSegPerfil != '17BB8F08-9D5F-425C-9B9B-1CA230C07C7F' and us.UidUsuario = su.UidUsuario and su.UidSegPerfil = sp.UidSegPerfil and us.UidEstatus = es.UidEstatus and sp.UidTipoPerfil = '" + UidTipoPerfil + "'";

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
                    VchContrasenia = item["VchContrasenia"].ToString(),
                    VchNombrePerfil = item["Perfil"].ToString(),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    VchIcono = item["VchIcono"].ToString(),
                    VchNombreComercial = item["VchNombreComercial"].ToString()
                });
            }

            return lsUsuariosCompletos;
        }
        public List<UsuariosCompletos> BuscarAdministradores(Guid UidTipoPerfil, string Nombre, string ApePaterno, string ApeMaterno, string Correo, string Franquicia, Guid UidEstatus)
        {
            List<UsuariosCompletos> lsUsuariosCompletos = new List<UsuariosCompletos>();

            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "sp_AdministradoresBuscar";
            try
            {
                comando.Parameters.Add("@UidTipoPerfil", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoPerfil"].Value = UidTipoPerfil;

                if (Nombre != string.Empty)
                {
                    comando.Parameters.Add("@Nombre", SqlDbType.VarChar);
                    comando.Parameters["@Nombre"].Value = Nombre;
                }
                if (ApePaterno != string.Empty)
                {
                    comando.Parameters.Add("@ApePaterno", SqlDbType.VarChar);
                    comando.Parameters["@ApePaterno"].Value = ApePaterno;
                }
                if (ApeMaterno != string.Empty)
                {
                    comando.Parameters.Add("@ApeMaterno", SqlDbType.VarChar);
                    comando.Parameters["@ApeMaterno"].Value = ApeMaterno;
                }
                if (Correo != string.Empty)
                {
                    comando.Parameters.Add("@Correo", SqlDbType.VarChar);
                    comando.Parameters["@Correo"].Value = Correo;
                }
                if (Franquicia != string.Empty)
                {
                    comando.Parameters.Add("@Franquicia", SqlDbType.VarChar);
                    comando.Parameters["@Franquicia"].Value = Franquicia;
                }
                if (UidEstatus != Guid.Empty)
                {
                    comando.Parameters.Add("@UidEstatus", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidEstatus"].Value = UidEstatus;
                }

                foreach (DataRow item in this.Busquedas(comando).Rows)
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
                        VchContrasenia = item["VchContrasenia"].ToString(),
                        VchNombrePerfil = item["Perfil"].ToString(),
                        VchDescripcion = item["VchDescripcion"].ToString(),
                        VchIcono = item["VchIcono"].ToString(),
                        VchNombreComercial = item["VchNombreComercial"].ToString()
                    });
                }

                return lsUsuariosCompletos;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<UsuariosCompletos> CargarUsuariosPrincipal(Guid UidTipoPerfil)
        {
            List<UsuariosCompletos> lsUsuariosCompletos = new List<UsuariosCompletos>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            //query.CommandText = "select us.*, su.VchUsuario, sp.VchNombre as Perfil, es.VchDescripcion, es.VchIcono from Usuarios us, SegUsuarios su, SegPerfiles sp, Estatus es where sp.UidSegPerfil != '17BB8F08-9D5F-425C-9B9B-1CA230C07C7F' and us.UidUsuario = su.UidUsuario and su.UidSegPerfil = sp.UidSegPerfil and us.UidEstatus = es.UidEstatus and sp.UidTipoPerfil = '" + UidTipoPerfil + "'";
            //query.CommandText = "select us.*, su.VchUsuario, sp.VchNombre as Perfil, es.VchDescripcion, es.VchIcono, fr.VchNombreComercial from Usuarios us, SegUsuarios su, SegPerfiles sp, Estatus es, Franquiciatarios fr, FranquiciasUsuarios fu where fr.UidFranquiciatarios = fu.UidFranquicia and fu.UidUsuario = us.UidUsuario and sp.UidSegPerfil != '17BB8F08-9D5F-425C-9B9B-1CA230C07C7F' and us.UidUsuario = su.UidUsuario and su.UidSegPerfil = sp.UidSegPerfil and us.UidEstatus = es.UidEstatus and sp.UidTipoPerfil = '" + UidTipoPerfil + "'";
            query.CommandText = "select us.*, su.VchUsuario, su.VchContrasenia, sp.UidSegPerfil, sp.VchNombre as Perfil, es.VchDescripcion, es.VchIcono, sp.UidTipoPerfil from Usuarios us, SegUsuarios su, SegPerfiles sp, Estatus es where sp.UidSegPerfil != '17BB8F08-9D5F-425C-9B9B-1CA230C07C7F' and us.UidUsuario = su.UidUsuario and su.UidSegPerfil = sp.UidSegPerfil and us.UidEstatus = es.UidEstatus and sp.UidTipoPerfil = '" + UidTipoPerfil + "'";

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
                    VchContrasenia = item["VchContrasenia"].ToString(),
                    UidSegPerfil = Guid.Parse(item["UidSegPerfil"].ToString()),
                    VchNombrePerfil = item["Perfil"].ToString(),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    VchIcono = item["VchIcono"].ToString()
                });
            }

            return lsUsuariosCompletos;
        }
        public List<UsuariosCompletos> BuscarUsuariosPrincipal(Guid UidTipoPerfil, string Nombre, string ApePaterno, string ApeMaterno, string Correo, string Perfil, Guid UidEstatus)
        {
            List<UsuariosCompletos> lsUsuariosCompletos = new List<UsuariosCompletos>();

            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "sp_UsuariosPrincipalBuscar";
            try
            {
                comando.Parameters.Add("@UidTipoPerfil", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoPerfil"].Value = UidTipoPerfil;

                if (Nombre != string.Empty)
                {                    
                    comando.Parameters.Add("@Nombre", SqlDbType.VarChar);
                    comando.Parameters["@Nombre"].Value = Nombre;
                }
                if (ApePaterno != string.Empty)
                {
                    comando.Parameters.Add("@ApePaterno", SqlDbType.VarChar);
                    comando.Parameters["@ApePaterno"].Value = ApePaterno;
                }
                if (ApeMaterno != string.Empty)
                {
                    comando.Parameters.Add("@ApeMaterno", SqlDbType.VarChar);
                    comando.Parameters["@ApeMaterno"].Value = ApeMaterno;
                }
                if (Correo != string.Empty)
                {
                    comando.Parameters.Add("@Correo", SqlDbType.VarChar);
                    comando.Parameters["@Correo"].Value = Correo;
                }
                if (Perfil != string.Empty)
                {
                    comando.Parameters.Add("@Perfil", SqlDbType.VarChar);
                    comando.Parameters["@Perfil"].Value = Perfil;
                }
                if (UidEstatus != Guid.Empty)
                {
                    comando.Parameters.Add("@UidEstatus", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidEstatus"].Value = UidEstatus;
                }

                foreach (DataRow item in this.Busquedas(comando).Rows)
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
                        VchContrasenia = item["VchContrasenia"].ToString(),
                        UidSegPerfil = Guid.Parse(item["UidSegPerfil"].ToString()),
                        VchNombrePerfil = item["Perfil"].ToString(),
                        VchDescripcion = item["VchDescripcion"].ToString(),
                        VchIcono = item["VchIcono"].ToString()
                    });
                }

                return lsUsuariosCompletos;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool RegistrarAdministradores(UsuariosCompletos usuariosCompletos, Guid UidSegPerfilEscuela, DireccionesUsuarios direccionesUsuarios, TelefonosUsuarios telefonosUsuarios, Guid UidFranquicia)
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

                comando.Parameters.Add("@UidSegPerfilEscuela", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidSegPerfilEscuela"].Value = UidSegPerfilEscuela;

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

        public bool RegistrarAdministradoresPrincipal(UsuariosCompletos usuariosCompletos, Guid UidSegPerfilEscuela, DireccionesUsuarios direccionesUsuarios, TelefonosUsuarios telefonosUsuarios)
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

                comando.Parameters.Add("@UidSegPerfilEscuela", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidSegPerfilEscuela"].Value = UidSegPerfilEscuela;

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

        public bool ActualizarAdministradoresPrincipal(UsuariosCompletos usuariosCompletos, Guid UidSegPerfilEscuela, DireccionesUsuarios direccionesUsuarios, TelefonosUsuarios telefonosUsuarios)
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

                comando.Parameters.Add("@UidSegPerfilEscuela", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidSegPerfilEscuela"].Value = UidSegPerfilEscuela;

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

        public List<UsuariosCompletos> RecoveryPassword(string Parametro, string Dato)
        {
            List<UsuariosCompletos> lsRecoveryPassword = new List<UsuariosCompletos>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select us.VchNombre, us.VchApePaterno, us.VchApeMaterno, us.VchCorreo, su.VchContrasenia from Usuarios us, SegUsuarios su where us.UidUsuario = su.UidUsuario and " + Parametro + "= '" + Dato + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsRecoveryPassword.Add(new UsuariosCompletos()
                {
                    StrNombre = item["VchNombre"].ToString(),
                    StrApePaterno = item["VchApePaterno"].ToString(),
                    StrApeMaterno = item["VchApeMaterno"].ToString(),
                    VchContrasenia = item["VchContrasenia"].ToString(),
                    StrCorreo = item["VchCorreo"].ToString()
                });
            }

            return lsRecoveryPassword;
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
        public string ObtenerIDFranquiciaUsuario(Guid UidUsuario)
        {
            string IdFranquicia = "";

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select fr.IdFranquicia from Usuarios us, Franquiciatarios fr, FranquiciasUsuarios fu where fu.UidUsuario = us.UidUsuario and fu.UidFranquicia = fr.UidFranquiciatarios and us.UidUsuario = '" + UidUsuario + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                IdFranquicia = item["IdFranquicia"].ToString();
            }

            return IdFranquicia;
        }
        public List<UsuariosCompletos> CargarAdministradoresFranquicia(Guid UidFranquiciatario, Guid UidTipoPerfilFranquicia)
        {
            List<UsuariosCompletos> lsUsuariosCompletos = new List<UsuariosCompletos>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select us.*, su.VchUsuario, su.VchContrasenia, sp.VchNombre as Perfil, es.VchDescripcion, es.VchIcono, sp.UidTipoPerfil from SegUsuarios su, SegPerfiles sp, Estatus es, FranquiciasUsuarios fu, Franquiciatarios fr, Usuarios us where sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and fu.UidFranquicia = fr.UidFranquiciatarios and fu.UidUsuario = us.UidUsuario and fr.UidFranquiciatarios = '" + UidFranquiciatario + "'" + " and sp.UidTipoPerfil = '" + UidTipoPerfilFranquicia + "'";

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
                    VchContrasenia = item["VchContrasenia"].ToString(),
                    VchNombrePerfil = item["Perfil"].ToString(),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    VchIcono = item["VchIcono"].ToString()
                });
            }

            return lsUsuariosCompletos;
        }
        public List<UsuariosCompletos> BuscarAdministradoresFranquicia(Guid UidFranquiciatario, Guid UidTipoPerfilFranquicia, string Nombre, string ApePaterno, string ApeMaterno, string Correo, string Perfil, Guid UidEstatus)
        {
            List<UsuariosCompletos> lsUsuariosCompletos = new List<UsuariosCompletos>();

            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "sp_AdministradoresFranquiciaBuscar";
            try
            {
                comando.Parameters.Add("@UidFranquiciatario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidFranquiciatario"].Value = UidFranquiciatario;
                
                comando.Parameters.Add("@UidTipoPerfilFranquicia", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoPerfilFranquicia"].Value = UidTipoPerfilFranquicia;

                if (Nombre != string.Empty)
                {
                    comando.Parameters.Add("@Nombre", SqlDbType.VarChar);
                    comando.Parameters["@Nombre"].Value = Nombre;
                }
                if (ApePaterno != string.Empty)
                {
                    comando.Parameters.Add("@ApePaterno", SqlDbType.VarChar);
                    comando.Parameters["@ApePaterno"].Value = ApePaterno;
                }
                if (ApeMaterno != string.Empty)
                {
                    comando.Parameters.Add("@ApeMaterno", SqlDbType.VarChar);
                    comando.Parameters["@ApeMaterno"].Value = ApeMaterno;
                }
                if (Correo != string.Empty)
                {
                    comando.Parameters.Add("@Correo", SqlDbType.VarChar);
                    comando.Parameters["@Correo"].Value = Correo;
                }
                if (Perfil != string.Empty)
                {
                    comando.Parameters.Add("@Perfil", SqlDbType.VarChar);
                    comando.Parameters["@Perfil"].Value = Perfil;
                }
                if (UidEstatus != Guid.Empty)
                {
                    comando.Parameters.Add("@UidEstatus", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidEstatus"].Value = UidEstatus;
                }

                foreach (DataRow item in this.Busquedas(comando).Rows)
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
                        VchContrasenia = item["VchContrasenia"].ToString(),
                        VchNombrePerfil = item["Perfil"].ToString(),
                        VchDescripcion = item["VchDescripcion"].ToString(),
                        VchIcono = item["VchIcono"].ToString()
                    });
                }

                return lsUsuariosCompletos;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool RegistrarAdministradoresFranquicia(UsuariosCompletos usuariosCompletos, Guid UidSegPerfilEscuela, DireccionesUsuarios direccionesUsuarios, TelefonosUsuarios telefonosUsuarios, Guid UidFranquicia)
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

                comando.Parameters.Add("@UidSegPerfilEscuela", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidSegPerfilEscuela"].Value = UidSegPerfilEscuela;

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
        public bool ActualizarAdministradoresFranquicia(UsuariosCompletos usuariosCompletos, Guid UidSegPerfilEscuela, DireccionesUsuarios direccionesUsuarios, TelefonosUsuarios telefonosUsuarios, Guid UidFranquicia)
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

                comando.Parameters.Add("@UidSegPerfilEscuela", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidSegPerfilEscuela"].Value = UidSegPerfilEscuela;

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

        #region MetodosFranquiciaUsuarios
        public List<UsuariosCompletos> CargarFranquiciasUsuariosFinales(Guid UidFranquicia, Guid UidTipoPerfil)
        {
            List<UsuariosCompletos> lsUsuariosCompletos = new List<UsuariosCompletos>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select us.*, su.VchUsuario, su.VchContrasenia, sp.VchNombre as Perfil, es.VchDescripcion, es.VchIcono, sp.UidTipoPerfilFranquicia from SegUsuarios su, SegPerfiles sp, Estatus es, FranquiciasUsuarios fu, Franquiciatarios fr, Usuarios us where sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and fu.UidFranquicia = fr.UidFranquiciatarios and fu.UidUsuario = us.UidUsuario and fr.UidFranquiciatarios = '" + UidFranquicia + "' and sp.UidTipoPerfil = '" + UidTipoPerfil + "'";

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
                    VchContrasenia = item["VchContrasenia"].ToString(),
                    VchNombrePerfil = item["Perfil"].ToString(),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    VchIcono = item["VchIcono"].ToString()
                });
            }

            return lsUsuariosCompletos;
        }
        public bool RegistrarFranquiciasUsuarios(UsuariosCompletos usuariosCompletos, TelefonosUsuarios telefonosUsuarios, Guid UidFranquicia)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_UsuarioFranquiciaRegistrar";

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
        public bool RegistrarFranquiciasDireccionUsuarios(Guid UidUsuario, DireccionesUsuarios direccionesUsuarios)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_UsuarioFranquiciaDireccionRegistrar";

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
        public bool ActualizarFranquiciasDireccionUsuarios(Guid UidUsuario, DireccionesUsuarios direccionesUsuarios)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_UsuarioFranquiciaDireccionActualizar";

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
        public bool ActualizarFranquiciasUsuarios(UsuariosCompletos usuariosCompletos, TelefonosUsuarios telefonosUsuarios, Guid UidFranquicia)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_UsuarioFranquiciaActualizar";

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
        public bool ObtenerFranquiciasUsuarios(Guid UidUsuario)
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

        public List<UsuariosCompletos> BuscarFranquiciaUsuariosFinales(Guid UidCliente, Guid UidTipoPerfil, string Nombre, string ApePaterno, string ApeMaterno, string Correo, Guid UidEstatus)
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
        public void AsociarFranquiciaUsuariosFinales(string Correo)
        {
            usuarioCompleto = new UsuariosCompletos();

            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "sp_UsuariosFinalesAsociarFranquicia";
            try
            {
                if (Correo != string.Empty)
                {
                    comando.Parameters.Add("@Correo", SqlDbType.VarChar, 50);
                    comando.Parameters["@Correo"].Value = Correo;
                }

                foreach (DataRow item in this.Busquedas(comando).Rows)
                {
                    usuarioCompleto.UidUsuario = new Guid(item["UidUsuario"].ToString());
                    usuarioCompleto.StrNombre = item["VchNombre"].ToString();
                    usuarioCompleto.StrApePaterno = item["VchApePaterno"].ToString();
                    usuarioCompleto.StrApeMaterno = item["VchApeMaterno"].ToString();
                    usuarioCompleto.StrCorreo = item["VchCorreo"].ToString();
                    usuarioCompleto.UidEstatus = new Guid(item["UidEstatus"].ToString());
                    usuarioCompleto.VchUsuario = item["VchUsuario"].ToString();
                    usuarioCompleto.VchContrasenia = item["VchContrasenia"].ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region MetodosClientes
        public List<UsuariosCompletos> CargarAdministradoresCliente(Guid UidFranquiciatario, Guid UidTipoPerfil)
        {
            List<UsuariosCompletos> lsUsuariosCompletos = new List<UsuariosCompletos>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select us.*, su.VchUsuario, su.VchContrasenia, sp.VchNombre as Perfil, es.VchDescripcion, es.VchIcono, sp.UidTipoPerfilFranquicia, cl.VchNombreComercial from SegUsuarios su, SegPerfiles sp, Estatus es, ClientesUsuarios cu, Clientes cl, Usuarios us where sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and cu.UidCliente = cl.UidCliente and cu.UidUsuario = us.UidUsuario and cl.UidFranquiciatario = '" + UidFranquiciatario + "' and sp.UidTipoPerfil = '" + UidTipoPerfil + "'";

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
                    VchContrasenia = item["VchContrasenia"].ToString(),
                    VchNombrePerfil = item["Perfil"].ToString(),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    VchIcono = item["VchIcono"].ToString(),
                    VchNombreComercial = item["VchNombreComercial"].ToString()
                });
            }

            return lsUsuariosCompletos;
        }
        public List<UsuariosCompletos> BuscarAdministradoresCliente(Guid UidFranquiciatario, Guid UidTipoPerfil, string Nombre, string ApePaterno, string ApeMaterno, string Correo, string Cliente, Guid UidEstatus)
        {
            List<UsuariosCompletos> lsUsuariosCompletos = new List<UsuariosCompletos>();

            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "sp_AdministradoresClientesBuscar";
            try
            {
                comando.Parameters.Add("@UidFranquiciatario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidFranquiciatario"].Value = UidFranquiciatario;
                
                comando.Parameters.Add("@UidTipoPerfil", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoPerfil"].Value = UidTipoPerfil;

                if (Nombre != string.Empty)
                {
                    comando.Parameters.Add("@Nombre", SqlDbType.VarChar);
                    comando.Parameters["@Nombre"].Value = Nombre;
                }
                if (ApePaterno != string.Empty)
                {
                    comando.Parameters.Add("@ApePaterno", SqlDbType.VarChar);
                    comando.Parameters["@ApePaterno"].Value = ApePaterno;
                }
                if (ApeMaterno != string.Empty)
                {
                    comando.Parameters.Add("@ApeMaterno", SqlDbType.VarChar);
                    comando.Parameters["@ApeMaterno"].Value = ApeMaterno;
                }
                if (Correo != string.Empty)
                {
                    comando.Parameters.Add("@Correo", SqlDbType.VarChar);
                    comando.Parameters["@Correo"].Value = Correo;
                }
                if (Cliente != string.Empty)
                {
                    comando.Parameters.Add("@Cliente", SqlDbType.VarChar);
                    comando.Parameters["@Cliente"].Value = Cliente;
                }
                if (UidEstatus != Guid.Empty)
                {
                    comando.Parameters.Add("@UidEstatus", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidEstatus"].Value = UidEstatus;
                }

                foreach (DataRow item in this.Busquedas(comando).Rows)
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
                        VchContrasenia = item["VchContrasenia"].ToString(),
                        VchNombrePerfil = item["Perfil"].ToString(),
                        VchDescripcion = item["VchDescripcion"].ToString(),
                        VchIcono = item["VchIcono"].ToString(),
                        VchNombreComercial = item["VchNombreComercial"].ToString()
                    });
                }

                return lsUsuariosCompletos;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool RegistrarAdministradoresCliente(UsuariosCompletos usuariosCompletos, bool BitEscuela, Guid UidSegPerfilEscuela, DireccionesUsuarios direccionesUsuarios, TelefonosUsuarios telefonosUsuarios, Guid UidCliente)
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

                if (BitEscuela)
                {
                    comando.Parameters.Add("@UidSegPerfilEscuela", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidSegPerfilEscuela"].Value = UidSegPerfilEscuela;
                }

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
        public bool ActualizarAdministradoresCliente(UsuariosCompletos usuariosCompletos, bool BitEscuela, Guid UidSegPerfilEscuela, DireccionesUsuarios direccionesUsuarios, TelefonosUsuarios telefonosUsuarios, Guid UidCliente)
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

                if (BitEscuela)
                {
                    comando.Parameters.Add("@UidSegPerfilEscuela", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidSegPerfilEscuela"].Value = UidSegPerfilEscuela;
                }

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

        public List<UsuariosCompletos> CargarAdminCliente(Guid UidFranquiciatario, Guid UidCliente, Guid UidTipoPerfil)
        {
            List<UsuariosCompletos> lsActualizarUsuarios = new List<UsuariosCompletos>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select su.UidSegUsuario, us.VchNombre, sp.VchNombre as Perfil, es.VchDescripcion from SegUsuarios su, SegPerfiles sp, Estatus es, ClientesUsuarios cu, Clientes cl, Usuarios us where sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and cu.UidCliente = cl.UidCliente and cu.UidUsuario = us.UidUsuario and cl.UidFranquiciatario = '" + UidFranquiciatario + "' and cl.UidCliente = '" + UidCliente + "' and sp.UidTipoPerfil = '" + UidTipoPerfil + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsActualizarUsuarios.Add(new UsuariosCompletos()
                {
                    UidSegUsuario = new Guid(item["UidSegUsuario"].ToString()),
                    StrNombre = item["VchNombre"].ToString(),
                    VchNombrePerfil = item["Perfil"].ToString(),
                    VchDescripcion = item["VchDescripcion"].ToString()
                });
            }

            return lsActualizarUsuarios;
        }
        public bool ActualizarAdminClientePerfilEscu(Guid UidSegUsuario, bool BitEscuela, Guid UidSegPerfilEscuela)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_AdministradorCliActuPerfilEscuela";

                comando.Parameters.Add("@UidSegUsuario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidSegUsuario"].Value = UidSegUsuario;

                if (BitEscuela)
                {
                    comando.Parameters.Add("@UidSegPerfilEscuela", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidSegPerfilEscuela"].Value = UidSegPerfilEscuela;
                }

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

        #region Pagos
        public List<LigasUsuariosGridViewModel> SeleccionarUsuariosCliente(Guid UidUsuario)
        {
            List<LigasUsuariosGridViewModel> lsLigasUsuariosGridViewModel = new List<LigasUsuariosGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select us.*, cl.IdCliente, cl.VchNombreComercial, tu.VchTelefono from TelefonosUsuarios tu, SegUsuarios su, SegPerfiles sp, Estatus es, ClientesUsuarios cu, Clientes cl, Usuarios us where tu.UidUsuario = us.UidUsuario and sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and cu.UidCliente = cl.UidCliente and cu.UidUsuario = us.UidUsuario and us.UidUsuario = '" + UidUsuario + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
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
                    IdCliente = int.Parse(item["IdCliente"].ToString()),
                    VchNombreComercial = item["VchNombreComercial"].ToString()
                });
            }

            return lsLigasUsuariosGridViewModel;
        }
        #endregion

        #region Eventos
        public List<LigasUsuariosGridViewModel> SelectUsClienteEvento(Guid UidUsuario)
        {
            List<LigasUsuariosGridViewModel> lsLigasUsuariosGridViewModel = new List<LigasUsuariosGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select us.*, cl.IdCliente, cl.VchNombreComercial, tu.VchTelefono from TelefonosUsuarios tu, SegUsuarios su, SegPerfiles sp, Estatus es, ClientesUsuarios cu, Clientes cl, Usuarios us where tu.UidUsuario = us.UidUsuario and sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and cu.UidCliente = cl.UidCliente and cu.UidUsuario = us.UidUsuario and us.UidUsuario = '" + UidUsuario + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
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
                    IdCliente = int.Parse(item["IdCliente"].ToString()),
                    VchNombreComercial = item["VchNombreComercial"].ToString()
                });
            }

            return lsLigasUsuariosGridViewModel;
        }

        public List<EventoUsuarioGridViewModel> AsociarUsuariosEvento(Guid UidCliente, Guid UidTipoPerfil)
        {
            List<EventoUsuarioGridViewModel> lsEventoUsuarioGridViewModel = new List<EventoUsuarioGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select us.*, sp.VchNombre as Perfil, es.VchDescripcion, es.VchIcono, pt.Prefijo, tu.VchTelefono from SegUsuarios su, SegPerfiles sp, Estatus es, ClientesUsuarios cu, Clientes cl, Usuarios us, TelefonosUsuarios tu, PrefijosTelefonicos pt where us.UidUsuario = tu.UidUsuario and tu.UidPrefijo = pt.UidPrefijo and sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and cu.UidCliente = cl.UidCliente and cu.UidUsuario = us.UidUsuario and cl.UidCliente = '" + UidCliente + "' and sp.UidTipoPerfil = '" + UidTipoPerfil + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsEventoUsuarioGridViewModel.Add(new EventoUsuarioGridViewModel()
                {
                    UidUsuario = new Guid(item["UidUsuario"].ToString()),
                    StrNombre = item["VchNombre"].ToString(),
                    StrApePaterno = item["VchApePaterno"].ToString(),
                    StrApeMaterno = item["VchApeMaterno"].ToString(),
                    StrCorreo = item["VchCorreo"].ToString(),
                    UidEstatus = new Guid(item["UidEstatus"].ToString()),
                    StrTelefono = item["Prefijo"].ToString() + item["VchTelefono"].ToString(),
                    blSeleccionado = false
                });
            }

            return lsEventoUsuarioGridViewModel;
        }
        public List<EventoUsuarioGridViewModel> BuscarUsuariosEvento(List<EventoUsuarioGridViewModel> lsEventoUsuarios, Guid UidCliente, string Nombre, string ApePaterno, string ApeMaterno, string Correo)
        {
            List<EventoUsuarioGridViewModel> lsEventoUsuarioGridViewModel = new List<EventoUsuarioGridViewModel>();

            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "sp_EventosBuscarUsuarios";
            try
            {
                comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCliente"].Value = UidCliente;

                if (Nombre != string.Empty)
                {
                    comando.Parameters.Add("@Nombre", SqlDbType.VarChar);
                    comando.Parameters["@Nombre"].Value = Nombre;
                }
                if (ApePaterno != string.Empty)
                {
                    comando.Parameters.Add("@ApePaterno", SqlDbType.VarChar);
                    comando.Parameters["@ApePaterno"].Value = ApePaterno;
                }
                if (ApeMaterno != string.Empty)
                {
                    comando.Parameters.Add("@ApeMaterno", SqlDbType.VarChar);
                    comando.Parameters["@ApeMaterno"].Value = ApeMaterno;
                }
                if (Correo != string.Empty)
                {
                    comando.Parameters.Add("@Correo", SqlDbType.VarChar);
                    comando.Parameters["@Correo"].Value = Correo;
                }

                foreach (DataRow item in this.Busquedas(comando).Rows)
                {
                    bool blSeleccionado = false;

                    foreach (var item2 in lsEventoUsuarios)
                    {
                        if (Guid.Parse(item["UidUsuario"].ToString()) == item2.UidUsuario)
                        {
                            blSeleccionado = true;
                            break;
                        }
                    }

                    lsEventoUsuarioGridViewModel.Add(new EventoUsuarioGridViewModel()
                    {
                        UidUsuario = new Guid(item["UidUsuario"].ToString()),
                        StrNombre = item["VchNombre"].ToString(),
                        StrApePaterno = item["VchApePaterno"].ToString(),
                        StrApeMaterno = item["VchApeMaterno"].ToString(),
                        StrCorreo = item["VchCorreo"].ToString(),
                        UidEstatus = new Guid(item["UidEstatus"].ToString()),
                        StrTelefono = item["Prefijo"].ToString() + item["VchTelefono"].ToString(),
                        blSeleccionado = blSeleccionado
                    });
                }

                return lsEventoUsuarioGridViewModel;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<EventoUsuarioGridViewModel> ObtenerUsuariosEvento(Guid UidEvento)
        {
            List<EventoUsuarioGridViewModel> lsEventoUsuarioGridViewModel = new List<EventoUsuarioGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select us.*, pt.Prefijo, tu.VchTelefono from Eventos ev, EventosUsuarios eu, Usuarios us, TelefonosUsuarios tu, PrefijosTelefonicos pt where tu.UidUsuario = us.UidUsuario and pt.UidPrefijo = tu.UidPrefijo and eu.UidEvento = ev.UidEvento and eu.UidUsuario = us.UidUsuario and ev.UidEvento = '" + UidEvento + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsEventoUsuarioGridViewModel.Add(new EventoUsuarioGridViewModel()
                {
                    UidUsuario = new Guid(item["UidUsuario"].ToString()),
                    StrNombre = item["VchNombre"].ToString(),
                    StrApePaterno = item["VchApePaterno"].ToString(),
                    StrApeMaterno = item["VchApeMaterno"].ToString(),
                    StrCorreo = item["VchCorreo"].ToString(),
                    UidEstatus = new Guid(item["UidEstatus"].ToString()),
                    StrTelefono = item["Prefijo"].ToString() + item["VchTelefono"].ToString(),
                    blSeleccionado = true
                });
            }

            return lsEventoUsuarioGridViewModel;
        }
        #endregion
        #endregion

        #region MetodosUsuarios
        public List<UsuariosCompletos> CargarUsuariosFinales(Guid UidCliente, Guid UidTipoPerfil)
        {
            List<UsuariosCompletos> lsUsuariosCompletos = new List<UsuariosCompletos>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select us.*, su.UidEstatusCuenta, su.VchUsuario, su.VchContrasenia, sp.VchNombre as Perfil, es.VchDescripcion, es.VchIcono, sp.UidTipoPerfilFranquicia, cl.VchIdWAySMS, cu.VchAccion from SegUsuarios su, SegPerfiles sp, Estatus es, ClientesUsuarios cu, Clientes cl, Usuarios us where sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and cu.UidCliente = cl.UidCliente and cu.UidUsuario = us.UidUsuario and cl.UidCliente = '" + UidCliente + "' and sp.UidTipoPerfil = '" + UidTipoPerfil + "'";

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
                    VchContrasenia = item["VchContrasenia"].ToString(),
                    VchNombrePerfil = item["Perfil"].ToString(),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    VchIcono = item["VchIcono"].ToString(),
                    VchNombreComercial = item["VchIdWAySMS"].ToString(),
                    UidEstatusCuenta = Guid.Parse(item["UidEstatusCuenta"].ToString()),
                    VchAccion = item["VchAccion"].ToString()
                });
            }

            return lsUsuariosCompletos;
        }
        public bool RegistrarUsuarios(UsuariosCompletos usuariosCompletos, TelefonosUsuarios telefonosUsuarios, Guid UidCliente)
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

                //===========================TELEFONO==================================================

                comando.Parameters.Add("@VchTelefono", SqlDbType.VarChar, 50);
                comando.Parameters["@VchTelefono"].Value = telefonosUsuarios.VchTelefono;

                comando.Parameters.Add("@UidTipoTelefono", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoTelefono"].Value = telefonosUsuarios.UidTipoTelefono;

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
        public bool ActualizarUsuarios(UsuariosCompletos usuariosCompletos, TelefonosUsuarios telefonosUsuarios, Guid UidCliente)
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
                comando.Parameters["@UidTipoTelefono"].Value = telefonosUsuarios.UidTipoTelefono;

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
        public void AsociarUsuariosFinales(string Correo)
        {
            usuarioCompleto = new UsuariosCompletos();

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
                    usuarioCompleto.UidUsuario = new Guid(item["UidUsuario"].ToString());
                    usuarioCompleto.StrNombre = item["VchNombre"].ToString();
                    usuarioCompleto.StrApePaterno = item["VchApePaterno"].ToString();
                    usuarioCompleto.StrApeMaterno = item["VchApeMaterno"].ToString();
                    usuarioCompleto.StrCorreo = item["VchCorreo"].ToString();
                    usuarioCompleto.UidEstatus = new Guid(item["UidEstatus"].ToString());
                    usuarioCompleto.VchUsuario = item["VchUsuario"].ToString();
                    usuarioCompleto.VchContrasenia = item["VchContrasenia"].ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Metodos Usuarios final
        public List<LigasUsuariosGridViewModel> SelectUsClienteEventoUsuarioFinal(Guid UidUsuario)
        {
            List<LigasUsuariosGridViewModel> lsLigasUsuariosGridViewModel = new List<LigasUsuariosGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select us.*, cl.IdCliente, cl.VchNombreComercial, tu.VchTelefono from TelefonosUsuarios tu, SegUsuarios su, SegPerfiles sp, Estatus es, ClientesUsuarios cu, Clientes cl, Usuarios us where tu.UidUsuario = us.UidUsuario and sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and cu.UidCliente = cl.UidCliente and cu.UidUsuario = us.UidUsuario and us.UidUsuario = '" + UidUsuario + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
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
                    IdCliente = int.Parse(item["IdCliente"].ToString()),
                    VchNombreComercial = item["VchNombreComercial"].ToString()
                });
            }

            return lsLigasUsuariosGridViewModel;
        }
        #endregion  

        #region MetodosExel

        #region Clientes
        #region Simple
        public List<LigasUsuariosGridViewModel> CargarUsuariosFinales(List<LigasUsuariosGridViewModel> lsLigasUsuarios, Guid UidCliente, Guid UidTipoPerfil)
        {
            List<LigasUsuariosGridViewModel> lsLigasUsuariosGridViewModel = new List<LigasUsuariosGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            //query.CommandText = "select us.*, cl.IdCliente, cl.VchNombreComercial, tu.VchTelefono from TelefonosUsuarios tu, SegUsuarios su, SegPerfiles sp, Estatus es, ClientesUsuarios cu, Clientes cl, Usuarios us where tu.UidUsuario = us.UidUsuario and sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and cu.UidCliente = cl.UidCliente and cu.UidUsuario = us.UidUsuario and cl.UidCliente = '" + UidCliente + "' and sp.UidTipoPerfil = '" + UidTipoPerfil + "'";
            query.CommandText = "select us.*, cl.IdCliente, cl.VchIdWAySMS, pt.Prefijo, tu.VchTelefono from TelefonosUsuarios tu, SegUsuarios su, SegPerfiles sp, Estatus es, ClientesUsuarios cu, Clientes cl, Usuarios us, PrefijosTelefonicos pt where pt.UidPrefijo = tu.UidPrefijo and tu.UidUsuario = us.UidUsuario and sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and cu.UidCliente = cl.UidCliente and cu.UidUsuario = us.UidUsuario and cl.UidCliente = '" + UidCliente + "' and sp.UidTipoPerfil = '" + UidTipoPerfil + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                bool accion = false;

                //foreach (var it in lsLigasUsuarios)
                //{
                //    if (it.IdUsuario == int.Parse(item["IdUsuario"].ToString()))
                //    {
                //        accion = it.blSeleccionado;
                //    }
                //}

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
                        StrTelefono = "(" + item["Prefijo"].ToString() + ")" + item["VchTelefono"].ToString(),
                        blSeleccionado = accion,
                        IdCliente = int.Parse(item["IdCliente"].ToString()),
                        VchNombreComercial = item["VchIdWAySMS"].ToString()
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
                        IdCliente = item.IdCliente,
                        VchNombreComercial = item.VchNombreComercial
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
                        IdCliente = item.IdCliente,
                        VchNombreComercial = item.VchNombreComercial
                    });
                }


            }


            return lsLigasUsuariosGridViewModel;
        }
        public List<LigasUsuariosGridViewModel> ExcelToList(List<LigasUsuariosGridViewModel> lsLigasUsuariosGridView, List<LigasUsuariosGridViewModel> lsLigasInsertar, Guid UidCliente, string URLBase)
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
                    foreach (DataRow itemExist in exist.Rows)
                    {
                        SqlCommand queryCliente = new SqlCommand();
                        queryCliente.CommandType = CommandType.Text;

                        queryCliente.CommandText = "select * from ClientesUsuarios where UidCliente = '" + UidCliente + "' and  UidUsuario = '" + itemExist["UidUsuario"].ToString() + "'";

                        DataTable existClienteUsuarios = this.Busquedas(queryCliente);

                        if (existClienteUsuarios.Rows.Count >= 1)
                        {

                        }
                        else
                        {
                            AsociarUsuarioCliente(UidCliente, Guid.Parse(itemExist["UidUsuario"].ToString()));
                        }
                    }
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
                        bool enviarCorreo = false;
                        Guid UidUsuario = Guid.NewGuid();

                        enviarCorreo = RegistrarUsuariosExcel(
                            new UsuariosCompletos
                            {
                                UidUsuario = UidUsuario,
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
                                VchTelefono = item.StrTelefono,
                                UidPrefijo = item.UidPrefijo
                            },
                            UidCliente);

                        if (enviarCorreo)
                        {
                            string Asunto = "Acceso al sistema Cobros Masivos";
                            string LigaUrl = URLBase + "Views/Login.aspx";

                            var Crden = validacionesRepository.Creden(UidUsuario, UidCliente);
                            string mnsj = correosRepository.CorreoEnvioCredenciales(item.NombreCompleto, Asunto, Crden.Item1, Crden.Item2, LigaUrl, item.StrCorreo, Crden.Item3);
                        }
                    }
                }
            }

            lsLigasUsuariosGridViewModel = RecuperarUsuariosExcel(lsLigasUsuariosGridView, lsLigasInsertar, UidCliente);
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
        public List<LigasUsuariosGridViewModel> RecuperarUsuariosExcel(List<LigasUsuariosGridViewModel> lsLigasUsuariosGridView, List<LigasUsuariosGridViewModel> lsLigasInsertar, Guid UidCliente)
        {
            List<LigasUsuariosGridViewModel> lsLigasUsuariosGridViewModel = new List<LigasUsuariosGridViewModel>();

            lsLigasUsuariosGridViewModel = lsLigasUsuariosGridView;

            foreach (var item in lsLigasInsertar)
            {
                SqlCommand query = new SqlCommand();
                query.CommandType = CommandType.Text;

                //query.CommandText = "select us.*, cl.IdCliente, cl.VchNombreComercial, tu.VchTelefono from TelefonosUsuarios tu, SegUsuarios su, SegPerfiles sp, Estatus es, ClientesUsuarios cu, Clientes cl, Usuarios us where tu.UidUsuario = us.UidUsuario and sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and cu.UidCliente = cl.UidCliente and cu.UidUsuario = us.UidUsuario and us.VchCorreo = '" + item.StrCorreo + "'";
                query.CommandText = "select us.*, cl.IdCliente, cl.VchIdWAySMS, pt.Prefijo, tu.VchTelefono from TelefonosUsuarios tu, SegUsuarios su, SegPerfiles sp, Estatus es, ClientesUsuarios cu, Clientes cl, Usuarios us, PrefijosTelefonicos pt where pt.UidPrefijo = tu.UidPrefijo and tu.UidUsuario = us.UidUsuario and sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and cu.UidCliente = cl.UidCliente and cu.UidUsuario = us.UidUsuario and cl.UidCliente = '" + UidCliente + "' and us.VchCorreo = '" + item.StrCorreo + "'";

                DataTable dt = this.Busquedas(query);

                foreach (DataRow us in dt.Rows)
                {
                    //if (!lsLigasUsuariosGridViewModel.Exists(x => x.StrCorreo == us["VchCorreo"].ToString()))
                    //{
                    lsLigasUsuariosGridViewModel.Add(new LigasUsuariosGridViewModel()
                    {
                        UidUsuario = new Guid(us["UidUsuario"].ToString()),
                        IdUsuario = int.Parse(us["IdUsuario"].ToString()),
                        StrNombre = us["VchNombre"].ToString(),
                        StrApePaterno = us["VchApePaterno"].ToString(),
                        StrApeMaterno = us["VchApeMaterno"].ToString(),
                        StrCorreo = us["VchCorreo"].ToString(),
                        UidEstatus = new Guid(us["UidEstatus"].ToString()),
                        StrTelefono = "(" + us["Prefijo"].ToString() + ")" + us["VchTelefono"].ToString(),
                        blSeleccionado = true,
                        IdCliente = int.Parse(us["IdCliente"].ToString()),
                        VchNombreComercial = us["VchIdWAySMS"].ToString()
                    });
                    //}
                    //else
                    //{

                    //}
                }
            }

            return lsLigasUsuariosGridViewModel;
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

        #region Multiple
        public List<LigasMultiplesUsuariosGridViewModel> CargarUsuariosFinalesMultiples(List<LigasMultiplesUsuariosGridViewModel> lsLigasUsuarios, Guid UidCliente, Guid UidTipoPerfil)
        {
            List<LigasMultiplesUsuariosGridViewModel> lsLigasMultiplesUsuariosGridViewModel = new List<LigasMultiplesUsuariosGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            //query.CommandText = "select us.*, cl.IdCliente, cl.VchNombreComercial, tu.VchTelefono from TelefonosUsuarios tu, SegUsuarios su, SegPerfiles sp, Estatus es, ClientesUsuarios cu, Clientes cl, Usuarios us where tu.UidUsuario = us.UidUsuario and sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and cu.UidCliente = cl.UidCliente and cu.UidUsuario = us.UidUsuario and cl.UidCliente = '" + UidCliente + "' and sp.UidTipoPerfil = '" + UidTipoPerfil + "'";
            query.CommandText = "select us.*, cl.IdCliente, cl.VchIdWAySMS, pt.Prefijo, tu.VchTelefono from TelefonosUsuarios tu, SegUsuarios su, SegPerfiles sp, Estatus es, ClientesUsuarios cu, Clientes cl, Usuarios us, PrefijosTelefonicos pt where pt.UidPrefijo = tu.UidPrefijo and tu.UidUsuario = us.UidUsuario and sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and cu.UidCliente = cl.UidCliente and cu.UidUsuario = us.UidUsuario and cl.UidCliente = '" + UidCliente + "' and sp.UidTipoPerfil = '" + UidTipoPerfil + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                bool accion = false;
                string Asunto = string.Empty;
                string Concepto = string.Empty;
                Decimal Importe = 0;
                DateTime Vencimiento = DateTime.Now;

                //foreach (var it in lsLigasUsuarios)
                //{
                //    if (it.IdUsuario == int.Parse(item["IdUsuario"].ToString()))
                //    {
                //        accion = it.blSeleccionado;
                //        Asunto = it.StrAsunto;
                //        Concepto = it.StrConcepto;
                //        Importe = it.DcmImporte;
                //        Vencimiento = it.DtVencimiento;
                //    }
                //}
                if (accion != true)
                {
                    lsLigasMultiplesUsuariosGridViewModel.Add(new LigasMultiplesUsuariosGridViewModel()
                    {
                        UidUsuario = new Guid(item["UidUsuario"].ToString()),
                        IdUsuario = int.Parse(item["IdUsuario"].ToString()),
                        StrNombre = item["VchNombre"].ToString(),
                        StrApePaterno = item["VchApePaterno"].ToString(),
                        StrApeMaterno = item["VchApeMaterno"].ToString(),
                        StrCorreo = item["VchCorreo"].ToString(),
                        UidEstatus = new Guid(item["UidEstatus"].ToString()),
                        StrTelefono = "(" + item["Prefijo"].ToString() + ")" + item["VchTelefono"].ToString(),
                        blSeleccionado = accion,
                        IdCliente = int.Parse(item["IdCliente"].ToString()),
                        VchNombreComercial = item["VchIdWAySMS"].ToString(),
                        StrAsunto = Asunto,
                        StrConcepto = Concepto,
                        DcmImporte = Importe,
                        DtVencimiento = Vencimiento
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
                        VchNombreComercial = item.VchNombreComercial,
                        StrAsunto = item.StrAsunto,
                        StrConcepto = item.StrConcepto,
                        DcmImporte = item.DcmImporte,
                        DtVencimiento = item.DtVencimiento,
                        CBCorreo = accion,
                        CBWhatsApp = accion,
                        CBSms = accion
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
                        VchNombreComercial = item.VchNombreComercial,
                        StrAsunto = item.StrAsunto,
                        StrConcepto = item.StrConcepto,
                        DcmImporte = item.DcmImporte,
                        DtVencimiento = item.DtVencimiento,
                        CBCorreo = accion,
                        CBWhatsApp = accion,
                        CBSms = accion
                    });
                }
            }
            return lsLigasMultiplesUsuariosGridViewModel;
        }
        public List<LigasMultiplesUsuariosGridViewModel> ActualizarListaGvUsuariosMultiple(List<LigasMultiplesUsuariosGridViewModel> lsLigasUsuarios, int IdUsuario, bool accion, string Asunto, string Concepto, decimal Importe, DateTime Vencimiento, string Promociones, bool CBCorreo, bool CBSms, bool CBWhatsApp, int Index)
        {
            List<LigasMultiplesUsuariosGridViewModel> lsLigasMultiplesUsuariosGridViewModel = new List<LigasMultiplesUsuariosGridViewModel>();

            for (int i = 0; i < lsLigasUsuarios.Count; i++)
            {
                if (i == Index)
                {
                    lsLigasMultiplesUsuariosGridViewModel.Add(new LigasMultiplesUsuariosGridViewModel()
                    {
                        UidUsuario = lsLigasUsuarios[Index].UidUsuario,
                        IdUsuario = lsLigasUsuarios[Index].IdUsuario,
                        StrNombre = lsLigasUsuarios[Index].StrNombre,
                        StrApePaterno = lsLigasUsuarios[Index].StrApePaterno,
                        StrApeMaterno = lsLigasUsuarios[Index].StrApeMaterno,
                        StrCorreo = lsLigasUsuarios[Index].StrCorreo,
                        UidEstatus = lsLigasUsuarios[Index].UidEstatus,
                        StrTelefono = lsLigasUsuarios[Index].StrTelefono,
                        blSeleccionado = accion,
                        IdCliente = lsLigasUsuarios[Index].IdCliente,
                        VchNombreComercial = lsLigasUsuarios[Index].VchNombreComercial,
                        StrAsunto = Asunto,
                        StrConcepto = Concepto,
                        DcmImporte = Importe,
                        DtVencimiento = Vencimiento,
                        StrPromociones = Promociones,
                        CBCorreo = CBCorreo,
                        CBSms = CBSms,
                        CBWhatsApp = CBWhatsApp,
                        IntAuxiliar = lsLigasUsuarios[Index].IntAuxiliar
                    });
                }
                else
                {
                    lsLigasMultiplesUsuariosGridViewModel.Add(new LigasMultiplesUsuariosGridViewModel()
                    {
                        UidUsuario = lsLigasUsuarios[i].UidUsuario,
                        IdUsuario = lsLigasUsuarios[i].IdUsuario,
                        StrNombre = lsLigasUsuarios[i].StrNombre,
                        StrApePaterno = lsLigasUsuarios[i].StrApePaterno,
                        StrApeMaterno = lsLigasUsuarios[i].StrApeMaterno,
                        StrCorreo = lsLigasUsuarios[i].StrCorreo,
                        UidEstatus = lsLigasUsuarios[i].UidEstatus,
                        StrTelefono = lsLigasUsuarios[i].StrTelefono,
                        blSeleccionado = lsLigasUsuarios[i].blSeleccionado,
                        IdCliente = lsLigasUsuarios[i].IdCliente,
                        VchNombreComercial = lsLigasUsuarios[i].VchNombreComercial,
                        StrAsunto = lsLigasUsuarios[i].StrAsunto,
                        StrConcepto = lsLigasUsuarios[i].StrConcepto,
                        DcmImporte = lsLigasUsuarios[i].DcmImporte,
                        DtVencimiento = lsLigasUsuarios[i].DtVencimiento,
                        StrPromociones = lsLigasUsuarios[i].StrPromociones,
                        CBCorreo = lsLigasUsuarios[i].CBCorreo,
                        CBSms = lsLigasUsuarios[i].CBSms,
                        CBWhatsApp = lsLigasUsuarios[i].CBWhatsApp,
                        IntAuxiliar = lsLigasUsuarios[i].IntAuxiliar
                    });
                }
            }


            //foreach (var item in lsLigasUsuarios)
            //{
            //    int Inde = lsLigasUsuarios.IndexOf(lsLigasUsuarios.Find(x => x.IdUsuario == item.IdUsuario));

            //    if (Inde == Index)
            //    {
            //        lsLigasMultiplesUsuariosGridViewModel.Add(new LigasMultiplesUsuariosGridViewModel()
            //        {
            //            UidUsuario = lsLigasUsuarios[Index].UidUsuario,
            //            IdUsuario = lsLigasUsuarios[Index].IdUsuario,
            //            StrNombre = lsLigasUsuarios[Index].StrNombre,
            //            StrApePaterno = lsLigasUsuarios[Index].StrApePaterno,
            //            StrApeMaterno = lsLigasUsuarios[Index].StrApeMaterno,
            //            StrCorreo = lsLigasUsuarios[Index].StrCorreo,
            //            UidEstatus = lsLigasUsuarios[Index].UidEstatus,
            //            StrTelefono = lsLigasUsuarios[Index].StrTelefono,
            //            blSeleccionado = accion,
            //            IdCliente = lsLigasUsuarios[Index].IdCliente,
            //            VchNombreComercial = lsLigasUsuarios[Index].VchNombreComercial,
            //            StrAsunto = Asunto,
            //            StrConcepto = Concepto,
            //            DcmImporte = Importe,
            //            DtVencimiento = Vencimiento,
            //            StrPromociones = Promociones,
            //            CBCorreo = CBCorreo,
            //            CBSms = CBSms,
            //            CBWhatsApp = CBWhatsApp
            //        });
            //    }
            //    else
            //    {
            //        lsLigasMultiplesUsuariosGridViewModel.Add(new LigasMultiplesUsuariosGridViewModel()
            //        {
            //            UidUsuario = item.UidUsuario,
            //            IdUsuario = item.IdUsuario,
            //            StrNombre = item.StrNombre,
            //            StrApePaterno = item.StrApePaterno,
            //            StrApeMaterno = item.StrApeMaterno,
            //            StrCorreo = item.StrCorreo,
            //            UidEstatus = item.UidEstatus,
            //            StrTelefono = item.StrTelefono,
            //            blSeleccionado = item.blSeleccionado,
            //            IdCliente = item.IdCliente,
            //            VchNombreComercial = item.VchNombreComercial,
            //            StrAsunto = item.StrAsunto,
            //            StrConcepto = item.StrConcepto,
            //            DcmImporte = item.DcmImporte,
            //            DtVencimiento = item.DtVencimiento,
            //            StrPromociones = item.StrPromociones,
            //            CBCorreo = item.CBCorreo,
            //            CBSms = item.CBSms,
            //            CBWhatsApp = item.CBWhatsApp
            //        });
            //    }
            //}
            return lsLigasMultiplesUsuariosGridViewModel;
        }
        public List<LigasMultiplesUsuariosGridViewModel> ExcelToListMultiple(List<LigasMultiplesUsuariosGridViewModel> lsLigasMultiplesUsuariosGridView, List<LigasMultiplesUsuariosGridViewModel> lsLigasInsertarMultiple, Guid UidCliente, string URLBase)
        {
            bool accion = true;
            List<LigasMultiplesUsuariosGridViewModel> LsLigasMultiplesUsuariosGridViewModel = new List<LigasMultiplesUsuariosGridViewModel>();

            foreach (var item in lsLigasInsertarMultiple)
            {
                string Usuario = string.Empty;
                string Password = string.Empty;

                SqlCommand query = new SqlCommand();
                query.CommandType = CommandType.Text;

                query.CommandText = "select * from usuarios where VchCorreo = '" + item.StrCorreo.ToString() + "'";

                DataTable exist = this.Busquedas(query);

                if (exist.Rows.Count >= 1)
                {
                    foreach (DataRow itemExist in exist.Rows)
                    {
                        SqlCommand queryCliente = new SqlCommand();
                        queryCliente.CommandType = CommandType.Text;

                        queryCliente.CommandText = "select * from ClientesUsuarios where UidCliente = '" + UidCliente + "' and  UidUsuario = '" + itemExist["UidUsuario"].ToString() + "'";

                        DataTable existClienteUsuarios = this.Busquedas(queryCliente);

                        if (existClienteUsuarios.Rows.Count >= 1)
                        {

                        }
                        else
                        {
                            AsociarUsuarioCliente(UidCliente, Guid.Parse(itemExist["UidUsuario"].ToString()));
                        }
                    }
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
                        bool enviarCorreo = false;
                        Guid UidUsuario = Guid.NewGuid();

                        enviarCorreo = RegistrarUsuariosExcel(
                            new UsuariosCompletos
                            {
                                UidUsuario = UidUsuario,
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
                                VchTelefono = item.StrTelefono,
                                UidPrefijo = item.UidPrefijo
                            },
                            UidCliente);

                        if (enviarCorreo)
                        {
                            string Asunto = "Acceso al sistema Cobros Masivos";
                            string LigaUrl = URLBase + "Views/Login.aspx";

                            var Crden = validacionesRepository.Creden(UidUsuario, UidCliente);
                            string mnsj = correosRepository.CorreoEnvioCredenciales(item.NombreCompleto, Asunto, Crden.Item1, Crden.Item2, LigaUrl, item.StrCorreo, Crden.Item3);
                        }
                    }
                }
            }

            LsLigasMultiplesUsuariosGridViewModel = RecuperarUsuariosExcelMultiple(lsLigasMultiplesUsuariosGridView, lsLigasInsertarMultiple, UidCliente);
            return LsLigasMultiplesUsuariosGridViewModel;
        }
        public List<LigasMultiplesUsuariosGridViewModel> RecuperarUsuariosExcelMultiple(List<LigasMultiplesUsuariosGridViewModel> lsMultipleLigasUsuariosGridView, List<LigasMultiplesUsuariosGridViewModel> lsLigasInsertarMultiple, Guid UidCliente)
        {
            Random random = new Random();
            List<LigasMultiplesUsuariosGridViewModel> lsLigasMultiplesUsuariosGridViewModel = new List<LigasMultiplesUsuariosGridViewModel>();

            lsLigasMultiplesUsuariosGridViewModel = lsMultipleLigasUsuariosGridView;

            foreach (var item in lsLigasInsertarMultiple)
            {
                DateTime dtAuxiliar = DateTime.Now.AddSeconds(3).AddMilliseconds(5);
                SqlCommand query = new SqlCommand();
                query.CommandType = CommandType.Text;

                //query.CommandText = "select us.*, cl.IdCliente, cl.VchNombreComercial, tu.VchTelefono from TelefonosUsuarios tu, SegUsuarios su, SegPerfiles sp, Estatus es, ClientesUsuarios cu, Clientes cl, Usuarios us where tu.UidUsuario = us.UidUsuario and sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and cu.UidCliente = cl.UidCliente and cu.UidUsuario = us.UidUsuario and us.VchCorreo = '" + item.StrCorreo + "'";
                query.CommandText = "select us.*, cl.IdCliente, cl.VchIdWAySMS, pt.Prefijo, tu.VchTelefono from TelefonosUsuarios tu, SegUsuarios su, SegPerfiles sp, Estatus es, ClientesUsuarios cu, Clientes cl, Usuarios us, PrefijosTelefonicos pt where pt.UidPrefijo = tu.UidPrefijo and tu.UidUsuario = us.UidUsuario and sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and cu.UidCliente = cl.UidCliente and cu.UidUsuario = us.UidUsuario and cl.UidCliente = '" + UidCliente + "' and us.VchCorreo = '" + item.StrCorreo + "'";

                DataTable dt = this.Busquedas(query);

                foreach (DataRow us in dt.Rows)
                {
                    //if (!lsLigasMultiplesUsuariosGridViewModel.Exists(x => x.StrCorreo == us["VchCorreo"].ToString()))
                    //{
                    lsLigasMultiplesUsuariosGridViewModel.Add(new LigasMultiplesUsuariosGridViewModel()
                    {
                        UidUsuario = Guid.Parse(us["UidUsuario"].ToString()),
                        IdUsuario = int.Parse(us["IdUsuario"].ToString()),
                        StrNombre = us["VchNombre"].ToString(),
                        StrApePaterno = us["VchApePaterno"].ToString(),
                        StrApeMaterno = us["VchApeMaterno"].ToString(),
                        StrCorreo = us["VchCorreo"].ToString(),
                        UidEstatus = Guid.Parse(us["UidEstatus"].ToString()),
                        StrTelefono = "(" + us["Prefijo"].ToString() + ")" + us["VchTelefono"].ToString(),
                        blSeleccionado = true,
                        IdCliente = int.Parse(us["IdCliente"].ToString()),
                        VchNombreComercial = us["VchIdWAySMS"].ToString(),
                        StrAsunto = item.StrAsunto,
                        StrConcepto = item.StrConcepto,
                        DcmImporte = item.DcmImporte,
                        DtVencimiento = item.DtVencimiento,
                        CBCorreo = item.CBCorreo,
                        CBWhatsApp = item.CBWhatsApp,
                        CBSms = item.CBSms,
                        StrPromociones = item.StrPromociones,
                        IntAuxiliar = dtAuxiliar.ToString("ssfff") + random.Next(10000000, 100000001).ToString()
                    });
                    //}
                    //else
                    //{
                    //    lsLigasMultiplesUsuariosGridViewModel.RemoveAt(lsLigasMultiplesUsuariosGridViewModel.FindIndex(x => x.StrCorreo == us["VchCorreo"].ToString()));

                    //    lsLigasMultiplesUsuariosGridViewModel.Add(new LigasMultiplesUsuariosGridViewModel()
                    //    {
                    //        UidUsuario = Guid.Parse(us["UidUsuario"].ToString()),
                    //        IdUsuario = int.Parse(us["IdUsuario"].ToString()),
                    //        StrNombre = us["VchNombre"].ToString(),
                    //        StrApePaterno = us["VchApePaterno"].ToString(),
                    //        StrApeMaterno = us["VchApeMaterno"].ToString(),
                    //        StrCorreo = us["VchCorreo"].ToString(),
                    //        UidEstatus = Guid.Parse(us["UidEstatus"].ToString()),
                    //        StrTelefono = "(" + us["Prefijo"].ToString() + ")" + us["VchTelefono"].ToString(),
                    //        blSeleccionado = true,
                    //        IdCliente = int.Parse(us["IdCliente"].ToString()),
                    //        VchNombreComercial = us["VchIdWAySMS"].ToString(),
                    //        StrAsunto = item.StrAsunto,
                    //        StrConcepto = item.StrConcepto,
                    //        DcmImporte = item.DcmImporte,
                    //        DtVencimiento = item.DtVencimiento,
                    //        CBCorreo = item.CBCorreo,
                    //        CBWhatsApp = item.CBWhatsApp,
                    //        CBSms = item.CBSms,
                    //        StrPromociones = item.StrPromociones
                    //    });
                    //}
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
        #endregion

        #region Franquicias
        #region Simple
        public List<LigasUsuariosGridViewModel> CargarUsuariosFinalesFranquicias(List<LigasUsuariosGridViewModel> lsLigasUsuarios, Guid UidFranquiciatario, Guid UidTipoPerfil)
        {
            List<LigasUsuariosGridViewModel> lsLigasUsuariosGridViewModel = new List<LigasUsuariosGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select us.*, fr.IdFranquicia, fr.VchNombreComercial, tu.VchTelefono from TelefonosUsuarios tu, SegUsuarios su, SegPerfiles sp, Estatus es, FranquiciasUsuarios fu, Franquiciatarios fr, Usuarios us where tu.UidUsuario = us.UidUsuario and sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and fu.UidFranquicia = fr.UidFranquiciatarios and fu.UidUsuario = us.UidUsuario and fr.UidFranquiciatarios = '" + UidFranquiciatario + "' and sp.UidTipoPerfil = '" + UidTipoPerfil + "'";

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
                        IdFranquicia = int.Parse(item["IdFranquicia"].ToString()),
                        VchNombreComercial = item["VchNombreComercial"].ToString()
                    });
                }
            }

            return lsLigasUsuariosGridViewModel;
        }
        public List<LigasUsuariosGridViewModel> ActualizarListaUsuariosFranquicias(List<LigasUsuariosGridViewModel> lsLigasUsuarios, int IdUsuario, bool accion)
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
                        IdFranquicia = item.IdFranquicia,
                        VchNombreComercial = item.VchNombreComercial
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
                        IdFranquicia = item.IdFranquicia,
                        VchNombreComercial = item.VchNombreComercial
                    });
                }
            }
            return lsLigasUsuariosGridViewModel;
        }
        public List<LigasUsuariosGridViewModel> ExcelToListFranquicias(List<LigasUsuariosGridViewModel> lsLigasUsuariosGridView, List<LigasUsuariosGridViewModel> lsLigasInsertar, Guid UidFranquicia)
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
                    foreach (DataRow itemExist in exist.Rows)
                    {
                        SqlCommand queryFranquicia = new SqlCommand();
                        queryFranquicia.CommandType = CommandType.Text;

                        queryFranquicia.CommandText = "select * from FranquiciasUsuarios where UidFranquicia = '" + UidFranquicia + "' and  UidUsuario = '" + itemExist["UidUsuario"].ToString() + "'";

                        DataTable existFranquiciasUsuarios = this.Busquedas(queryFranquicia);

                        if (existFranquiciasUsuarios.Rows.Count >= 1)
                        {

                        }
                        else
                        {
                            AsociarUsuarioFranquicia(UidFranquicia, Guid.Parse(itemExist["UidUsuario"].ToString()));
                        }
                    }
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
                        RegistrarUsuariosFranquiciaExcel(
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
                            UidFranquicia);
                    }
                }
            }

            lsLigasUsuariosGridViewModel = RecuperarUsuariosExcelFranquicias(lsLigasUsuariosGridView, lsLigasInsertar);
            return lsLigasUsuariosGridViewModel;
        }
        public List<LigasUsuariosGridViewModel> RecuperarUsuariosExcelFranquicias(List<LigasUsuariosGridViewModel> lsLigasUsuariosGridView, List<LigasUsuariosGridViewModel> lsLigasInsertar)
        {
            List<LigasUsuariosGridViewModel> lsLigasUsuariosGridViewModel = new List<LigasUsuariosGridViewModel>();

            lsLigasUsuariosGridViewModel = lsLigasUsuariosGridView;

            foreach (var item in lsLigasInsertar)
            {
                SqlCommand query = new SqlCommand();
                query.CommandType = CommandType.Text;

                query.CommandText = "select us.*, fr.IdFranquicia, fr.VchNombreComercial, tu.VchTelefono from TelefonosUsuarios tu, SegUsuarios su, SegPerfiles sp, Estatus es, FranquiciasUsuarios fu , Franquiciatarios fr, Usuarios us where tu.UidUsuario = us.UidUsuario and sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and fu.UidFranquicia = fr.UidFranquiciatarios and fu.UidUsuario = us.UidUsuario and us.VchCorreo = '" + item.StrCorreo + "'";

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
                            IdFranquicia = int.Parse(us["IdFranquicia"].ToString()),
                            VchNombreComercial = us["VchNombreComercial"].ToString()
                        });
                    }
                    else
                    {

                    }
                }
            }

            return lsLigasUsuariosGridViewModel;
        }
        public bool AsociarUsuarioFranquicia(Guid UidFranquicia, Guid UidUsuario)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_UsuarioAsociarFranquiciaExcel";

                comando.Parameters.Add("@UidFranquicia", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidFranquicia"].Value = UidFranquicia;

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
        public bool RegistrarUsuariosFranquiciaExcel(UsuariosCompletos usuariosCompletos, TelefonosUsuarios telefonosUsuarios, Guid UidFranquicia)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_UsuarioRegistrarFranquiciaExcel";

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
        #region Multiple
        public List<LigasMultiplesUsuariosGridViewModel> CargarUsuariosFinalesMultiplesFranquicias(List<LigasMultiplesUsuariosGridViewModel> lsLigasUsuarios, Guid UidFranquiciatario, Guid UidTipoPerfil)
        {
            List<LigasMultiplesUsuariosGridViewModel> lsLigasMultiplesUsuariosGridViewModel = new List<LigasMultiplesUsuariosGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select us.*, fr.IdFranquicia, fr.VchNombreComercial, tu.VchTelefono from TelefonosUsuarios tu, SegUsuarios su, SegPerfiles sp, Estatus es, FranquiciasUsuarios fu, Franquiciatarios fr, Usuarios us where tu.UidUsuario = us.UidUsuario and sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and fu.UidFranquicia = fr.UidFranquiciatarios and fu.UidUsuario = us.UidUsuario and fr.UidFranquiciatarios = '" + UidFranquiciatario + "' and sp.UidTipoPerfil = '" + UidTipoPerfil + "'";

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
                if (accion != true)
                {
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
                        IdFranquicia = int.Parse(item["IdFranquicia"].ToString()),
                        VchNombreComercial = item["VchNombreComercial"].ToString(),
                        blSeleccionado = accion,
                        StrAsunto = Asunto,
                        StrConcepto = Concepto,
                        DcmImporte = Importe,
                        DtVencimiento = Vencimiento
                    });
                }
            }

            return lsLigasMultiplesUsuariosGridViewModel;
        }
        public List<LigasMultiplesUsuariosGridViewModel> ActualizarListaUsuariosMultipleFranquicias(List<LigasMultiplesUsuariosGridViewModel> lsLigasUsuarios, int IdUsuario, bool accion)
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
                        IdFranquicia = item.IdFranquicia,
                        VchNombreComercial = item.VchNombreComercial,
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
                        IdFranquicia = item.IdFranquicia,
                        VchNombreComercial = item.VchNombreComercial,
                        StrAsunto = item.StrAsunto,
                        StrConcepto = item.StrConcepto,
                        DcmImporte = item.DcmImporte,
                        DtVencimiento = item.DtVencimiento
                    });
                }
            }
            return lsLigasMultiplesUsuariosGridViewModel;
        }
        public List<LigasMultiplesUsuariosGridViewModel> ActualizarListaGvUsuariosMultipleFranquicias(List<LigasMultiplesUsuariosGridViewModel> lsLigasUsuarios, int IdUsuario, bool accion, string Asunto, string Concepto, decimal Importe, DateTime Vencimiento, string Promociones)
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
                        IdFranquicia = item.IdFranquicia,
                        VchNombreComercial = item.VchNombreComercial,
                        StrAsunto = Asunto,
                        StrConcepto = Concepto,
                        DcmImporte = Importe,
                        DtVencimiento = Vencimiento,
                        StrPromociones = Promociones
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
                        IdFranquicia = item.IdFranquicia,
                        VchNombreComercial = item.VchNombreComercial,
                        StrAsunto = item.StrAsunto,
                        StrConcepto = item.StrConcepto,
                        DcmImporte = item.DcmImporte,
                        DtVencimiento = item.DtVencimiento,
                        StrPromociones = item.StrPromociones
                    });
                }
            }
            return lsLigasMultiplesUsuariosGridViewModel;
        }
        public List<LigasMultiplesUsuariosGridViewModel> ExcelToListMultipleFranquicias(List<LigasMultiplesUsuariosGridViewModel> lsLigasMultiplesUsuariosGridView, List<LigasMultiplesUsuariosGridViewModel> lsLigasInsertarMultiple, Guid UidFranquicia)
        {
            bool accion = true;
            List<LigasMultiplesUsuariosGridViewModel> LsLigasMultiplesUsuariosGridViewModel = new List<LigasMultiplesUsuariosGridViewModel>();

            foreach (var item in lsLigasInsertarMultiple)
            {
                string Usuario = string.Empty;
                string Password = string.Empty;

                SqlCommand query = new SqlCommand();
                query.CommandType = CommandType.Text;

                query.CommandText = "select * from usuarios where VchCorreo = '" + item.StrCorreo.ToString() + "'";

                DataTable exist = this.Busquedas(query);

                if (exist.Rows.Count >= 1)
                {
                    foreach (DataRow itemExist in exist.Rows)
                    {
                        SqlCommand queryFranquicia = new SqlCommand();
                        queryFranquicia.CommandType = CommandType.Text;

                        queryFranquicia.CommandText = "select * from FranquiciasUsuarios where UidFranquicia = '" + UidFranquicia + "' and  UidUsuario = '" + itemExist["UidUsuario"].ToString() + "'";

                        DataTable existFranquiciasUsuarios = this.Busquedas(queryFranquicia);

                        if (existFranquiciasUsuarios.Rows.Count >= 1)
                        {

                        }
                        else
                        {
                            AsociarUsuarioFranquicia(UidFranquicia, Guid.Parse(itemExist["UidUsuario"].ToString()));
                        }
                    }
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
                        RegistrarUsuariosFranquiciaExcel(
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
                            UidFranquicia);
                    }
                }
            }

            LsLigasMultiplesUsuariosGridViewModel = RecuperarUsuariosExcelMultipleFranquicias(lsLigasMultiplesUsuariosGridView, lsLigasInsertarMultiple);
            return LsLigasMultiplesUsuariosGridViewModel;
        }
        public List<LigasMultiplesUsuariosGridViewModel> RecuperarUsuariosExcelMultipleFranquicias(List<LigasMultiplesUsuariosGridViewModel> lsMultipleLigasUsuariosGridView, List<LigasMultiplesUsuariosGridViewModel> lsLigasInsertarMultiple)
        {
            List<LigasMultiplesUsuariosGridViewModel> lsLigasMultiplesUsuariosGridViewModel = new List<LigasMultiplesUsuariosGridViewModel>();

            lsLigasMultiplesUsuariosGridViewModel = lsMultipleLigasUsuariosGridView;

            foreach (var item in lsLigasInsertarMultiple)
            {
                SqlCommand query = new SqlCommand();
                query.CommandType = CommandType.Text;

                query.CommandText = "select us.*, fr.IdFranquicia, fr.VchNombreComercial, tu.VchTelefono from TelefonosUsuarios tu, SegUsuarios su, SegPerfiles sp, Estatus es, FranquiciasUsuarios fu, Franquiciatarios fr, Usuarios us where tu.UidUsuario = us.UidUsuario and sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and fu.UidFranquicia = fr.UidFranquiciatarios and fu.UidUsuario = us.UidUsuario and us.VchCorreo = '" + item.StrCorreo + "'";

                DataTable dt = this.Busquedas(query);

                foreach (DataRow us in dt.Rows)
                {
                    if (!lsLigasMultiplesUsuariosGridViewModel.Exists(x => x.StrCorreo == us["VchCorreo"].ToString()))
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
                            IdFranquicia = int.Parse(us["IdFranquicia"].ToString()),
                            VchNombreComercial = us["VchNombreComercial"].ToString(),
                            StrAsunto = item.StrAsunto,
                            StrConcepto = item.StrConcepto,
                            DcmImporte = item.DcmImporte,
                            DtVencimiento = item.DtVencimiento,
                            StrPromociones = item.StrPromociones
                        });
                    }
                    else
                    {
                        lsLigasMultiplesUsuariosGridViewModel.RemoveAt(lsLigasMultiplesUsuariosGridViewModel.FindIndex(x => x.StrCorreo == us["VchCorreo"].ToString()));

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
                            IdFranquicia = int.Parse(us["IdFranquicia"].ToString()),
                            VchNombreComercial = us["VchNombreComercial"].ToString(),
                            StrAsunto = item.StrAsunto,
                            StrConcepto = item.StrConcepto,
                            DcmImporte = item.DcmImporte,
                            DtVencimiento = item.DtVencimiento,
                            StrPromociones = item.StrPromociones
                        });
                    }
                }
            }

            return lsLigasMultiplesUsuariosGridViewModel;
        }
        #endregion
        #endregion

        public bool GenerarLigasPagos(Guid UidLigaUrl, string VchUrl, string VchConcepto, decimal DcmImporte, string IdReferencia, Guid UidUsuario, string VchIdentificador, DateTime DtRegistro, DateTime DtVencimiento, string VchAsunto, Guid UidLigaAsociado, Guid UidPromocion, Guid UidPropietario)
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

                if (UidLigaAsociado != Guid.Empty)
                {
                    comando.Parameters.Add("@UidLigaAsociado", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidLigaAsociado"].Value = UidLigaAsociado;
                }

                if (UidPromocion != Guid.Empty)
                {
                    comando.Parameters.Add("@UidPromocion", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidPromocion"].Value = UidPromocion;
                }

                if (UidPropietario != Guid.Empty)
                {
                    comando.Parameters.Add("@UidPropietario", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidPropietario"].Value = UidPropietario;
                }

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        public bool GenerarLigasPagosEvento(Guid UidLigaUrl, string VchUrl, string VchConcepto, decimal DcmImporte, string IdReferencia, Guid UidUsuario, string VchIdentificador, DateTime DtRegistro, DateTime DtVencimiento, string VchAsunto, Guid UidLigaAsociado, Guid UidPromocion, Guid UidEvento, Guid UidPropietario)
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

                if (UidLigaAsociado != Guid.Empty)
                {
                    comando.Parameters.Add("@UidLigaAsociado", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidLigaAsociado"].Value = UidLigaAsociado;
                }

                if (UidPromocion != Guid.Empty)
                {
                    comando.Parameters.Add("@UidPromocion", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidPromocion"].Value = UidPromocion;
                }

                comando.Parameters.Add("@UidEvento", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEvento"].Value = UidEvento;

                comando.Parameters.Add("@UidPropietario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPropietario"].Value = UidPropietario;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        #endregion

        #region Metodos Escuela
        public bool LoginUsuarioEscuela(string Usuario, string Password)
        {
            bool resultado = false;
            SqlCommand comando = new SqlCommand();

            try
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.CommandText = "sp_ManejoDeSessionLogearUsuariosEscuela";

                if (Usuario.Contains("@"))
                {
                    comando.Parameters.Add("@Correo", SqlDbType.VarChar, 50);
                    comando.Parameters["@Correo"].Value = Usuario;
                }
                else
                {
                    comando.Parameters.Add("@Usuario", SqlDbType.VarChar, 30);
                    comando.Parameters["@Usuario"].Value = Usuario;
                }

                comando.Parameters.Add("@Password", SqlDbType.VarChar, 32);
                comando.Parameters["@Password"].Value = Password;

                DataTable dt = this.Busquedas(comando);

                foreach (DataRow item in dt.Rows)
                {
                    usuarioCompleto = new UsuariosCompletos()
                    {
                        UidSegPerfil = item.IsNull("UidSegPerfilEscuela") ? Guid.Empty : new Guid(item["UidSegPerfilEscuela"].ToString()),
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
        public bool ObtenerDatosUsuarioEscuela(Guid UidUsuario)
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
                        UidSegPerfil = new Guid(item["UidSegPerfilEscuela"].ToString()),
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
        public bool ObtenerEstatusdeEmpresaUsuarioEscuela(Guid UidUsuario)
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
                        UidSegPerfil = new Guid(item["UidSegPerfilEscuela"].ToString())
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
                                UidSegPerfil = new Guid(item["UidSegPerfilEscuela"].ToString())
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

        public List<UsuariosCompletos> RecoveryPasswordEscuela(string Parametro, string Dato)
        {
            List<UsuariosCompletos> lsRecoveryPassword = new List<UsuariosCompletos>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select us.VchNombre, us.VchApePaterno, us.VchApeMaterno, us.VchCorreo, su.VchContrasenia from Usuarios us, SegUsuarios su where us.UidUsuario = su.UidUsuario and " + Parametro + "= '" + Dato + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsRecoveryPassword.Add(new UsuariosCompletos()
                {
                    StrNombre = item["VchNombre"].ToString(),
                    StrApePaterno = item["VchApePaterno"].ToString(),
                    StrApeMaterno = item["VchApeMaterno"].ToString(),
                    VchContrasenia = item["VchContrasenia"].ToString(),
                    StrCorreo = item["VchCorreo"].ToString()
                });
            }

            return lsRecoveryPassword;
        }

        #region Pagos padres
        public List<LigasUsuariosGridViewModel> SelectUsuCliColegiatura(Guid UidCliente, Guid UidUsuario)
        {
            List<LigasUsuariosGridViewModel> lsLigasUsuariosGridViewModel = new List<LigasUsuariosGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select us.*, cl.IdCliente, cl.VchNombreComercial, tu.VchTelefono from TelefonosUsuarios tu, SegUsuarios su, SegPerfiles sp, Estatus es, ClientesUsuarios cu, Clientes cl, Usuarios us where tu.UidUsuario = us.UidUsuario and sp.UidSegPerfil = su.UidSegPerfilEscuela and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and cu.UidCliente = cl.UidCliente and cu.UidUsuario = us.UidUsuario and cl.UidCliente = '" + UidCliente + "' and us.UidUsuario = '" + UidUsuario + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
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
                    IdCliente = int.Parse(item["IdCliente"].ToString()),
                    VchNombreComercial = item["VchNombreComercial"].ToString()
                });
            }

            return lsLigasUsuariosGridViewModel;
        }
        public bool GenerarLigasPagosColegiatura(Guid UidLigaUrl, string VchUrl, string VchConcepto, decimal DcmImporte, string IdReferencia, Guid UidUsuario, string VchIdentificador, DateTime DtRegistro, DateTime DtVencimiento, string VchAsunto, Guid UidLigaAsociado, Guid UidPromocion, Guid UidFechaColegiatura, Guid UidPagoColegiatura, Guid UidPropietario)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_LigasRegistrarPagosPadres";

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

                if (UidLigaAsociado != Guid.Empty)
                {
                    comando.Parameters.Add("@UidLigaAsociado", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidLigaAsociado"].Value = UidLigaAsociado;
                }

                if (UidPromocion != Guid.Empty)
                {
                    comando.Parameters.Add("@UidPromocion", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidPromocion"].Value = UidPromocion;
                }

                comando.Parameters.Add("@UidFechaColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidFechaColegiatura"].Value = UidFechaColegiatura;

                comando.Parameters.Add("@UidPagoColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPagoColegiatura"].Value = UidPagoColegiatura;

                comando.Parameters.Add("@UidPropietario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPropietario"].Value = UidPropietario;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }

        public bool GenerarLigasPagosColegiaturaPraga(Guid UidLigaUrl, string VchUrl, string VchConcepto, string IdReferencia, Guid UidUsuario, string VchIdentificador, DateTime DtRegistro, DateTime DtVencimiento, decimal DcmImporte, string VchAsunto, Guid UidLigaAsociado, Guid UidTipoTarjeta, Guid UidPromocion, Guid UidFechaColegiatura, Guid UidPagoColegiatura, Guid UidPropietario)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_LigasPragaRegistrarPagosPadres";

                comando.Parameters.Add("@UidLigaUrl", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidLigaUrl"].Value = UidLigaUrl;

                comando.Parameters.Add("@VchUrl", SqlDbType.VarChar, 100);
                comando.Parameters["@VchUrl"].Value = VchUrl;

                comando.Parameters.Add("@VchConcepto", SqlDbType.VarChar, 100);
                comando.Parameters["@VchConcepto"].Value = VchConcepto;

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

                comando.Parameters.Add("@DcmImporte", SqlDbType.Decimal);
                comando.Parameters["@DcmImporte"].Value = DcmImporte;

                comando.Parameters.Add("@VchAsunto", SqlDbType.VarChar, 50);
                comando.Parameters["@VchAsunto"].Value = VchAsunto;

                if (UidLigaAsociado != Guid.Empty)
                {
                    comando.Parameters.Add("@UidLigaAsociado", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidLigaAsociado"].Value = UidLigaAsociado;
                }

                comando.Parameters.Add("@UidTipoTarjeta", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoTarjeta"].Value = UidTipoTarjeta;

                comando.Parameters.Add("@UidPromocion", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPromocion"].Value = UidPromocion;

                comando.Parameters.Add("@UidFechaColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidFechaColegiatura"].Value = UidFechaColegiatura;

                comando.Parameters.Add("@UidPagoColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPagoColegiatura"].Value = UidPagoColegiatura;

                comando.Parameters.Add("@UidPropietario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPropietario"].Value = UidPropietario;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception ex)
            {
                string mnsj = ex.Message;
            }
            return Resultado;
        }
        #endregion
        #endregion

        #region Metodos MasterPage
        public List<UsuarioActivarCuentaViewModel> ObtenerDatossUsuario(Guid UidUsuario)
        {
            List<UsuarioActivarCuentaViewModel> lsUsuarioActivarCuentaViewModel = new List<UsuarioActivarCuentaViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select us.*, UidEstatusCuenta, pt.UidPrefijo, pt.Prefijo, tu.VchTelefono from SegUsuarios su, Usuarios us, TelefonosUsuarios tu, PrefijosTelefonicos pt where tu.UidUsuario = us.UidUsuario and tu.UidPrefijo = pt.UidPrefijo and su.UidUsuario = us.UidUsuario and su.UidSegPerfil = '18E9669B-C238-4BCC-9213-AF995644A5A4' and us.UidUsuario = '" + UidUsuario + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsUsuarioActivarCuentaViewModel.Add(new UsuarioActivarCuentaViewModel()
                {
                    UidUsuario = Guid.Parse(item["UidUsuario"].ToString()),
                    StrNombre = item["VchNombre"].ToString(),
                    StrApePaterno = item["VchApePaterno"].ToString(),
                    StrApeMaterno = item["VchApeMaterno"].ToString(),
                    StrCorreo = item["VchCorreo"].ToString(),
                    StrTelefono = item["VchTelefono"].ToString(),
                    UidPrefijo = Guid.Parse(item["UidPrefijo"].ToString()),
                    UidEstatusCuenta = Guid.Parse(item["UidEstatusCuenta"].ToString())
                });
            }
            return lsUsuarioActivarCuentaViewModel;
        }
        public bool ActivarCuentaUsuario(UsuariosCompletos usuariosCompletos, TelefonosUsuarios telefonosUsuarios)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_UsuarioActivarCuenta";

                comando.Parameters.Add("@UidUsuario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidUsuario"].Value = usuariosCompletos.UidUsuario;

                comando.Parameters.Add("@StrNombre", SqlDbType.VarChar, 50);
                comando.Parameters["@StrNombre"].Value = usuariosCompletos.StrNombre;

                comando.Parameters.Add("@StrApePaterno", SqlDbType.VarChar, 50);
                comando.Parameters["@StrApePaterno"].Value = usuariosCompletos.StrApePaterno;

                comando.Parameters.Add("@StrApeMaterno", SqlDbType.VarChar, 50);
                comando.Parameters["@StrApeMaterno"].Value = usuariosCompletos.StrApeMaterno;

                comando.Parameters.Add("@VchContrasenia", SqlDbType.VarChar, 50);
                comando.Parameters["@VchContrasenia"].Value = usuariosCompletos.VchContrasenia;

                //===========================TELEFONO==================================================

                comando.Parameters.Add("@VchTelefono", SqlDbType.VarChar, 50);
                comando.Parameters["@VchTelefono"].Value = telefonosUsuarios.VchTelefono;

                comando.Parameters.Add("@UidTipoTelefono", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoTelefono"].Value = Guid.Parse("B1055882-BCBA-4AB7-94FA-90E57647E607");

                comando.Parameters.Add("@UidPrefijo", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPrefijo"].Value = telefonosUsuarios.UidPrefijo;

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
