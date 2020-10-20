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

            //query.CommandText = "select us.*, su.VchUsuario, su.VchContrasenia, sp.VchNombre as Perfil, es.VchDescripcion, es.VchIcono, sp.UidTipoPerfilFranquicia, cl.VchNombreComercial from SegUsuarios su, SegPerfiles sp, Estatus es, ClientesUsuarios cu, Clientes cl, Usuarios us where sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and cu.UidCliente = cl.UidCliente and cu.UidUsuario = us.UidUsuario and cl.UidCliente = '" + UidCliente + "' and sp.UidTipoPerfil = '" + UidTipoPerfil + "'";
            query.CommandText = "select (select COUNT(*) from Alumnos al, UsuariosAlumnos ua, Usuarios usu where ua.UidAlumno = al.UidAlumno and ua.UidUsuario = usu.UidUsuario and al.UidCliente = cl.UidCliente and usu.UidUsuario = us.UidUsuario) as CantAlumnos, (select COUNT(*) from clientes cli, Colegiaturas co, FechasColegiaturas fc, ColegiaturasAlumnos ca, Alumnos alu, Usuarios usua, UsuariosAlumnos ua, EstatusFechasColegiaturas efc, Periodicidades pe where pe.UidPeriodicidad = co.UidPeriodicidad and not exists (select * from LigasUrls lu, PagosTarjeta pt where pt.IdReferencia = lu.IdReferencia and pt.VchEstatus = 'approved' and lu.UidFechaColegiatura = fc.UidFechaColegiatura) and co.UidEstatus = '65E46BC9-1864-4145-AD1A-70F5B5F69739' and efc.UidEstatusFechaColegiatura = fc.UidEstatusFechaColegiatura and cli.UidCliente = co.UidCliente and co.UidColegiatura = fc.UidColegiatura and ca.UidColegiatura = co.UidColegiatura and ca.UidAlumno = alu.UidAlumno and ua.UidUsuario = usua.UidUsuario and ua.UidAlumno = alu.UidAlumno and cli.UidCliente = cl.UidCliente and usua.UidUsuario = us.UidUsuario) as CantColegiaturas, us.*, su.UidEstatusCuenta, su.VchUsuario, su.VchContrasenia, sp.VchNombre as Perfil, es.VchDescripcion, es.VchIcono, sp.UidTipoPerfilFranquicia, cl.VchNombreComercial, pt.Prefijo, tu.VchTelefono, cu.VchAccion from SegUsuarios su, SegPerfiles sp, Estatus es, ClientesUsuarios cu, Clientes cl, Usuarios us, TelefonosUsuarios tu, PrefijosTelefonicos pt where tu.UidUsuario = us.UidUsuario and tu.UidPrefijo = pt.UidPrefijo and sp.UidSegPerfil = su.UidSegPerfil and su.UidUsuario = us.UidUsuario and es.UidEstatus = us.UidEstatus and cu.UidCliente = cl.UidCliente and cu.UidUsuario = us.UidUsuario and su.UidSegPerfilEscuela is not null and cl.UidCliente = '" + UidCliente + "' and sp.UidTipoPerfil = '" + UidTipoPerfil + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                List<string> lsMatriculas = new List<string>();

                SqlCommand queMatri = new SqlCommand();
                queMatri.CommandType = CommandType.Text;

                queMatri.CommandText = "select al.VchMatricula, ua.* from Alumnos al, UsuariosAlumnos ua, Usuarios us, Clientes cl where cl.UidCliente = al.UidCliente and al.UidAlumno = ua.UidAlumno and us.UidUsuario = ua.UidUsuario and cl.UidCliente = '"+ UidCliente + "' and us.UidUsuario = '" + item["UidUsuario"].ToString() + "'";

                DataTable dtMatri = this.Busquedas(queMatri);

                foreach (DataRow dtMat in dtMatri.Rows)
                {
                    lsMatriculas.Add(dtMat["VchMatricula"].ToString());
                }

                string ColorNotification = "#f44336";
                if (int.Parse(item["CantAlumnos"].ToString()) >= 1)
                {
                    ColorNotification = "#4CAF50";
                }

                lsPadres.Add(new Padres()
                {
                    UidUsuario = Guid.Parse(item["UidUsuario"].ToString()),
                    IntCantAlumnos = int.Parse(item["CantAlumnos"].ToString()),
                    ColorNotification = ColorNotification,
                    StrNombre = item["VchNombre"].ToString(),
                    StrApePaterno = item["VchApePaterno"].ToString(),
                    StrApeMaterno = item["VchApeMaterno"].ToString(),
                    StrCorreo = item["VchCorreo"].ToString(),
                    UidEstatus = Guid.Parse(item["UidEstatus"].ToString()),
                    VchNombreComercial = item["VchNombreComercial"].ToString(),
                    VchUsuario = item["VchUsuario"].ToString(),
                    VchContrasenia = item["VchContrasenia"].ToString(),
                    VchNombrePerfil = item["Perfil"].ToString(),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    VchIcono = item["VchIcono"].ToString(),
                    StrTelefono = "(" + item["Prefijo"].ToString() + ")" + item["VchTelefono"].ToString(),
                    VchMatricula = string.Join(", ", lsMatriculas.ToArray()),
                    UidEstatusCuenta = Guid.Parse(item["UidEstatusCuenta"].ToString()),
                    VchAccion = item["VchAccion"].ToString()
                });
            }

            return lsPadres;
        }
        public bool RegistrarPadres(Padres usuariosCompletos, Guid UidSegPerfilEscuela, TelefonosUsuarios telefonosUsuarios, Guid UidCliente)
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
                    padres.UidUsuario = Guid.Parse(item["UidUsuario"].ToString());
                    padres.StrNombre = item["VchNombre"].ToString();
                    franquiciatarios.UidFranquiciatarios = Guid.Parse(item["UidFranquiciatarios"].ToString());
                    franquiciatarios.VchNombreComercial = item["Franquicia"].ToString();
                    clientes.UidCliente = Guid.Parse(item["UidCliente"].ToString());
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

        public List<Padres> BuscarPadres(string Nombre, string ApePaterno, string ApeMaterno, string Correo, string Celular, string CantAlumnos, Guid UidEstatus, string Colegiatura, Guid UidCliente, Guid UidTipoPerfil)
        {
            List<Padres> lsPadres = new List<Padres>();

            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "sp_PadresBuscar";
            try
            {
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
                if (Celular != string.Empty)
                {
                    comando.Parameters.Add("@Celular", SqlDbType.VarChar, 30);
                    comando.Parameters["@Celular"].Value = Celular;
                }
                if (CantAlumnos != "todos")
                {
                    comando.Parameters.Add("@CantAlumnos", SqlDbType.Int);
                    comando.Parameters["@CantAlumnos"].Value = CantAlumnos;
                }
                if (UidEstatus != Guid.Empty)
                {
                    comando.Parameters.Add("@UidEstatus", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidEstatus"].Value = UidEstatus;
                }
                if (Colegiatura != string.Empty && Colegiatura != "NO IMPORTA")
                {
                    if (Colegiatura == "SI")
                    {
                        comando.Parameters.Add("@Colegiatura", SqlDbType.Int);
                        comando.Parameters["@Colegiatura"].Value = 1;
                    }
                    else
                    {
                        comando.Parameters.Add("@SinColegiatura", SqlDbType.Int);
                        comando.Parameters["@SinColegiatura"].Value = 0;
                    }
                }
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

                foreach (DataRow item in this.Busquedas(comando).Rows)
                {
                    List<string> lsMatriculas = new List<string>();

                    SqlCommand queMatri = new SqlCommand();
                    queMatri.CommandType = CommandType.Text;

                    queMatri.CommandText = "select al.VchMatricula, ua.* from Alumnos al, UsuariosAlumnos ua, Usuarios us, Clientes cl where cl.UidCliente = al.UidCliente and al.UidAlumno = ua.UidAlumno and us.UidUsuario = ua.UidUsuario and cl.UidCliente = '" + UidCliente + "' and us.UidUsuario = '" + item["UidUsuario"].ToString() + "'";

                    DataTable dtMatri = this.Busquedas(queMatri);

                    foreach (DataRow dtMat in dtMatri.Rows)
                    {
                        lsMatriculas.Add(dtMat["VchMatricula"].ToString());
                    }

                    string ColorNotification = "#f44336";
                    if (int.Parse(item["CantAlumnos"].ToString()) >= 1)
                    {
                        ColorNotification = "#4CAF50";
                    }

                    lsPadres.Add(new Padres()
                    {
                        UidUsuario = Guid.Parse(item["UidUsuario"].ToString()),
                        IntCantAlumnos = int.Parse(item["CantAlumnos"].ToString()),
                        ColorNotification = ColorNotification,
                        StrNombre = item["VchNombre"].ToString(),
                        StrApePaterno = item["VchApePaterno"].ToString(),
                        StrApeMaterno = item["VchApeMaterno"].ToString(),
                        StrCorreo = item["VchCorreo"].ToString(),
                        UidEstatus = Guid.Parse(item["UidEstatus"].ToString()),
                        VchNombreComercial = item["VchNombreComercial"].ToString(),
                        VchUsuario = item["VchUsuario"].ToString(),
                        VchContrasenia = item["VchContrasenia"].ToString(),
                        VchNombrePerfil = item["Perfil"].ToString(),
                        VchDescripcion = item["VchDescripcion"].ToString(),
                        VchIcono = item["VchIcono"].ToString(),
                        StrTelefono = "(" + item["Prefijo"].ToString() + ")" + item["VchTelefono"].ToString(),
                        VchMatricula = string.Join(", ", lsMatriculas.ToArray()),
                        UidEstatusCuenta = Guid.Parse(item["UidEstatusCuenta"].ToString()),
                        VchAccion = item["VchAccion"].ToString()
                    });
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
                    padres.UidUsuario = Guid.Parse(item["UidUsuario"].ToString());
                    padres.StrNombre = item["VchNombre"].ToString();
                    padres.StrApePaterno = item["VchApePaterno"].ToString();
                    padres.StrApeMaterno = item["VchApeMaterno"].ToString();
                    padres.StrCorreo = item["VchCorreo"].ToString();
                    padres.UidEstatus = Guid.Parse(item["UidEstatus"].ToString());
                    padres.UidSegUsuario = Guid.Parse(item["UidSegUsuario"].ToString());
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
        public bool ActualizarAsociarClienteUsuario(Guid UidSegUsuario)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PadresActualizarAsociarCliente";

                comando.Parameters.Add("@UidSegUsuario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidSegUsuario"].Value = UidSegUsuario;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        #endregion

        #region Metodos MasterPage Escuela
        public List<PadresActivarCuentaViewModel> ObtenerDatosPadre(Guid UidUsuario)
        {
            List<PadresActivarCuentaViewModel> lsPadresActivarCuentaViewModel = new List<PadresActivarCuentaViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select us.*, UidEstatusCuenta, pt.UidPrefijo, pt.Prefijo, tu.VchTelefono from SegUsuarios su, Usuarios us, TelefonosUsuarios tu, PrefijosTelefonicos pt where tu.UidUsuario = us.UidUsuario and tu.UidPrefijo = pt.UidPrefijo and su.UidUsuario = us.UidUsuario and su.UidSegPerfilEscuela is not null and us.UidUsuario = '" + UidUsuario + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsPadresActivarCuentaViewModel.Add(new PadresActivarCuentaViewModel()
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
            return lsPadresActivarCuentaViewModel;
        }
        public bool ActivarCuentaPadre(Padres usuariosCompletos, TelefonosUsuarios telefonosUsuarios)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PadreActivarCuenta";

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

        #region METODOS EXCEL

        #region Padres
        public Tuple<bool, Guid> AccionPadresExcelToList(List<PadresGridViewModel> lsAccionPadres, Guid UidSegPerfil, Guid UidSegPerfilEscuela, Guid UidCliente)
        {
            bool result = false;
            Guid UidUsuario = Guid.Empty;

            foreach (var item in lsAccionPadres)
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
                        UidUsuario = Guid.Parse(itemExist["UidUsuario"].ToString());

                        SqlCommand comando = new SqlCommand();

                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.CommandText = "sp_PadresExcelActualizar";

                        comando.Parameters.Add("@UidUsuario", SqlDbType.UniqueIdentifier);
                        comando.Parameters["@UidUsuario"].Value = UidUsuario;

                        comando.Parameters.Add("@StrNombre", SqlDbType.VarChar, 50);
                        comando.Parameters["@StrNombre"].Value = item.StrNombre;

                        comando.Parameters.Add("@StrApePaterno", SqlDbType.VarChar, 50);
                        comando.Parameters["@StrApePaterno"].Value = item.StrApePaterno;

                        comando.Parameters.Add("@StrApeMaterno", SqlDbType.VarChar, 50);
                        comando.Parameters["@StrApeMaterno"].Value = item.StrApeMaterno;

                        //===========================TELEFONO==================================================

                        comando.Parameters.Add("@VchTelefono", SqlDbType.VarChar, 50);
                        comando.Parameters["@VchTelefono"].Value = item.StrTelefono;

                        comando.Parameters.Add("@UidPrefijo", SqlDbType.UniqueIdentifier);
                        comando.Parameters["@UidPrefijo"].Value = item.UidPrefijo;

                        result = this.ManipulacionDeDatos(comando);
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
                        UidUsuario = Guid.NewGuid();

                        result = RegistrarPadres(
                            new Padres
                            {
                                UidUsuario = UidUsuario,
                                StrNombre = item.StrNombre.Trim().ToUpper(),
                                StrApePaterno = item.StrApePaterno.Trim().ToUpper(),
                                StrApeMaterno = item.StrApeMaterno.Trim().ToUpper(),
                                StrCorreo = item.StrCorreo.Trim().ToUpper(),
                                VchUsuario = Usuario,
                                VchContrasenia = Password,
                                UidSegPerfil = UidSegPerfil
                            },
                            UidSegPerfilEscuela,
                            new TelefonosUsuarios
                            {
                                UidTipoTelefono = Guid.Parse("B1055882-BCBA-4AB7-94FA-90E57647E607"),
                                VchTelefono = item.StrTelefono,
                                UidPrefijo = item.UidPrefijo
                            },
                            UidCliente);
                    }
                }
            }

            return Tuple.Create(result, UidUsuario);
        }
        #endregion

        #endregion

        public List<PadresSelectAlumnosViewModel> ObtenerAlumnoPadres(Guid UidAlumno)
        {
            List<PadresSelectAlumnosViewModel> lsPadresSelectAlumnosViewModel = new List<PadresSelectAlumnosViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select us.*, pt.Prefijo, tu.VchTelefono from Usuarios us, UsuariosAlumnos ua, Alumnos al, TelefonosUsuarios tu, PrefijosTelefonicos pt where pt.UidPrefijo = tu.UidPrefijo and tu.UidUsuario = us.UidUsuario and us.UidUsuario = ua.UidUsuario and ua.UidAlumno = al.UidAlumno and al.UidAlumno = '" + UidAlumno + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsPadresSelectAlumnosViewModel.Add(new PadresSelectAlumnosViewModel()
                {
                    UidUsuario = new Guid(item["UidUsuario"].ToString()),
                    StrNombre = item["VchNombre"].ToString(),
                    StrApePaterno = item["VchApePaterno"].ToString(),
                    StrApeMaterno = item["VchApeMaterno"].ToString(),
                    StrCorreo = item["VchCorreo"].ToString(),
                    StrTelefono = "(" + item["Prefijo"].ToString() + ")" + item["VchTelefono"].ToString(),
                    blSeleccionadoTodo = false,
                    blSeleccionado = false
                });
            }

            return lsPadresSelectAlumnosViewModel;
        }
    }
}
