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
    public class AlumnosRepository : SqlDataRepository
    {
        AlumnosGridViewModel _alumnosGridViewModel = new AlumnosGridViewModel();
        public AlumnosGridViewModel alumnosGridViewModel
        {
            get { return _alumnosGridViewModel; }
            set { _alumnosGridViewModel = value; }
        }

        #region Metodos Alumnos
        public List<AlumnosGridViewModel> CargarAlumnos(Guid UidCliente)
        {
            List<AlumnosGridViewModel> lsAlumnosGridViewModel = new List<AlumnosGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select al.*, es.VchDescripcion, es.VchIcono, pt.Prefijo, tu.VchTelefono from Alumnos al, Estatus es, Clientes cl, TelefonosAlumnos tu, PrefijosTelefonicos pt where tu.UidAlumno = al.UidAlumno and pt.UidPrefijo = tu.UidPrefijo and al.UidEstatus = es.UidEstatus and cl.UidCliente = al.UidCliente and al.UidCliente = '" + UidCliente + "' order by al.VchMatricula asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                string BitBeca = "NO";
                string Telefono = string.Empty;

                if (bool.Parse(item["BitBeca"].ToString()))
                {
                    BitBeca = "SI";
                }

                if (!string.IsNullOrEmpty(item["VchTelefono"].ToString()))
                {
                    Telefono = "(" + item["Prefijo"].ToString() + ")" + item["VchTelefono"].ToString();
                }

                lsAlumnosGridViewModel.Add(new AlumnosGridViewModel()
                {
                    UidAlumno = new Guid(item["UidAlumno"].ToString()),
                    VchIdentificador = item["VchIdentificador"].ToString(),
                    VchNombres = item["VchNombres"].ToString(),
                    VchApePaterno = item["VchApePaterno"].ToString(),
                    VchApeMaterno = item["VchApeMaterno"].ToString(),
                    VchMatricula = item["VchMatricula"].ToString(),
                    VchCorreo = item["VchCorreo"].ToString(),
                    VchTelefono = Telefono,
                    VchBeca = BitBeca,
                    BitBeca = bool.Parse(item["BitBeca"].ToString()),
                    VchTipoBeca = item["VchTipoBeca"].ToString(),
                    DcmBeca = item.IsNull("DcmBeca") ? 0 : decimal.Parse(item["DcmBeca"].ToString()),
                    UidEstatus = new Guid(item["UidEstatus"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    VchIcono = item["VchIcono"].ToString()
                });
            }

            return lsAlumnosGridViewModel;
        }
        public bool RegistrarAlumno(Alumnos Alumnos, TelefonosAlumnos telefonosAlumnos)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_AlumnosRegistrar";

                comando.Parameters.Add("@UidAlumno", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidAlumno"].Value = Alumnos.UidAlumno;

                comando.Parameters.Add("@VchIdentificador", SqlDbType.VarChar);
                comando.Parameters["@VchIdentificador"].Value = Alumnos.VchIdentificador;
                
                comando.Parameters.Add("@VchNombres", SqlDbType.VarChar, 50);
                comando.Parameters["@VchNombres"].Value = Alumnos.VchNombres;

                comando.Parameters.Add("@VchApePaterno", SqlDbType.VarChar, 50);
                comando.Parameters["@VchApePaterno"].Value = Alumnos.VchApePaterno;

                comando.Parameters.Add("@VchApeMaterno", SqlDbType.VarChar, 50);
                comando.Parameters["@VchApeMaterno"].Value = Alumnos.VchApeMaterno;

                comando.Parameters.Add("@VchMatricula", SqlDbType.VarChar, 50);
                comando.Parameters["@VchMatricula"].Value = Alumnos.VchMatricula;

                comando.Parameters.Add("@VchCorreo", SqlDbType.VarChar, 50);
                comando.Parameters["@VchCorreo"].Value = Alumnos.VchCorreo;

                comando.Parameters.Add("@BitBeca", SqlDbType.Bit);
                comando.Parameters["@BitBeca"].Value = Alumnos.BitBeca;

                if (Alumnos.BitBeca)
                {
                    comando.Parameters.Add("@VchTipoBeca", SqlDbType.VarChar, 30);
                    comando.Parameters["@VchTipoBeca"].Value = Alumnos.VchTipoBeca;
                }
                else
                {
                    comando.Parameters.Add("@VchTipoBeca", SqlDbType.VarChar, 30);
                    comando.Parameters["@VchTipoBeca"].Value = "";
                }

                comando.Parameters.Add("@DcmBeca", SqlDbType.Decimal);
                comando.Parameters["@DcmBeca"].Value = Alumnos.DcmBeca;

                comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCliente"].Value = Alumnos.UidCliente;

                //===========================TELEFONO==================================================

                comando.Parameters.Add("@VchTelefono", SqlDbType.VarChar, 50);
                comando.Parameters["@VchTelefono"].Value = telefonosAlumnos.VchTelefono;

                comando.Parameters.Add("@UidPrefijo", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPrefijo"].Value = telefonosAlumnos.UidPrefijo;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public bool ActualizarAlumno(Alumnos Alumnos, TelefonosAlumnos telefonosAlumnos)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_AlumnosActualizar";

                comando.Parameters.Add("@UidAlumno", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidAlumno"].Value = Alumnos.UidAlumno;

                comando.Parameters.Add("@VchIdentificador", SqlDbType.VarChar);
                comando.Parameters["@VchIdentificador"].Value = Alumnos.VchIdentificador;

                comando.Parameters.Add("@VchNombres", SqlDbType.VarChar, 50);
                comando.Parameters["@VchNombres"].Value = Alumnos.VchNombres;

                comando.Parameters.Add("@VchApePaterno", SqlDbType.VarChar, 50);
                comando.Parameters["@VchApePaterno"].Value = Alumnos.VchApePaterno;

                comando.Parameters.Add("@VchApeMaterno", SqlDbType.VarChar, 50);
                comando.Parameters["@VchApeMaterno"].Value = Alumnos.VchApeMaterno;

                comando.Parameters.Add("@VchMatricula", SqlDbType.VarChar, 50);
                comando.Parameters["@VchMatricula"].Value = Alumnos.VchMatricula;

                comando.Parameters.Add("@VchCorreo", SqlDbType.VarChar, 50);
                comando.Parameters["@VchCorreo"].Value = Alumnos.VchCorreo;

                comando.Parameters.Add("@BitBeca", SqlDbType.Bit);
                comando.Parameters["@BitBeca"].Value = Alumnos.BitBeca;

                if (Alumnos.BitBeca)
                {
                    comando.Parameters.Add("@VchTipoBeca", SqlDbType.VarChar, 30);
                    comando.Parameters["@VchTipoBeca"].Value = Alumnos.VchTipoBeca;
                }
                else
                {
                    comando.Parameters.Add("@VchTipoBeca", SqlDbType.VarChar, 30);
                    comando.Parameters["@VchTipoBeca"].Value = "";
                }

                comando.Parameters.Add("@DcmBeca", SqlDbType.Decimal);
                comando.Parameters["@DcmBeca"].Value = Alumnos.DcmBeca;

                comando.Parameters.Add("@UidEstatus", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEstatus"].Value = Alumnos.UidEstatus;

                //===========================TELEFONO==================================================

                comando.Parameters.Add("@VchTelefono", SqlDbType.VarChar, 50);
                comando.Parameters["@VchTelefono"].Value = telefonosAlumnos.VchTelefono;

                comando.Parameters.Add("@UidPrefijo", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPrefijo"].Value = telefonosAlumnos.UidPrefijo;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        //public List<Alumnos> BuscarAlumnos(Guid UidCliente, Guid UidTipoPerfil, string Nombre, string ApePaterno, string ApeMaterno, string Correo, Guid UidEstatus)
        //{
        //    List<Alumnos> lsAlumnos = new List<Alumnos>();

        //    SqlCommand comando = new SqlCommand();
        //    comando.CommandType = CommandType.StoredProcedure;
        //    comando.CommandText = "sp_UsuariosFinalesBuscar";
        //    try
        //    {
        //        if (UidCliente != Guid.Empty)
        //        {
        //            comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
        //            comando.Parameters["@UidCliente"].Value = UidCliente;
        //        }
        //        if (UidTipoPerfil != Guid.Empty)
        //        {
        //            comando.Parameters.Add("@UidTipoPerfil", SqlDbType.UniqueIdentifier);
        //            comando.Parameters["@UidTipoPerfil"].Value = UidTipoPerfil;
        //        }
        //        if (Nombre != string.Empty)
        //        {
        //            comando.Parameters.Add("@Nombre", SqlDbType.VarChar, 50);
        //            comando.Parameters["@Nombre"].Value = Nombre;
        //        }
        //        if (ApePaterno != string.Empty)
        //        {
        //            comando.Parameters.Add("@ApePaterno", SqlDbType.VarChar, 50);
        //            comando.Parameters["@ApePaterno"].Value = ApePaterno;
        //        }
        //        if (ApeMaterno != string.Empty)
        //        {
        //            comando.Parameters.Add("@ApeMaterno", SqlDbType.VarChar, 50);
        //            comando.Parameters["@ApeMaterno"].Value = ApeMaterno;
        //        }
        //        if (Correo != string.Empty)
        //        {
        //            comando.Parameters.Add("@Correo", SqlDbType.VarChar, 50);
        //            comando.Parameters["@Correo"].Value = Correo;
        //        }
        //        if (UidEstatus != Guid.Empty)
        //        {
        //            comando.Parameters.Add("@UidEstatus", SqlDbType.UniqueIdentifier);
        //            comando.Parameters["@UidEstatus"].Value = UidEstatus;
        //        }

        //        foreach (DataRow item in this.Busquedas(comando).Rows)
        //        {
        //            alumnos = new Alumnos()
        //            {
        //                UidUsuario = new Guid(item["UidUsuario"].ToString()),
        //                StrNombre = item["VchNombre"].ToString(),
        //                StrApePaterno = item["VchApePaterno"].ToString(),
        //                StrApeMaterno = item["VchApeMaterno"].ToString(),
        //                StrCorreo = item["VchCorreo"].ToString(),
        //                UidEstatus = new Guid(item["UidEstatus"].ToString()),
        //                VchUsuario = item["VchUsuario"].ToString(),
        //                VchNombrePerfil = item["Perfil"].ToString(),
        //                VchDescripcion = item["VchDescripcion"].ToString(),
        //                VchIcono = item["VchIcono"].ToString()
        //            };

        //            lsAlumnos.Add(alumnos);
        //        }

        //        return lsAlumnos;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        #endregion

        #region Metodos Clientes
        public bool RegistrarClienteAlumnos(Guid UidUsuario, Guid UidAlumno)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ClientesAlumnosRegistrar";

                comando.Parameters.Add("@UidUsuario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidUsuario"].Value = UidUsuario;

                comando.Parameters.Add("@UidAlumno", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidAlumno"].Value = UidAlumno;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        public bool EliminarClienteAlumnos(Guid UidUsuario)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ClientesAlumnosEliminar";

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
        public List<AlumnosGridViewModel> ObtenerClienteAlumnos(Guid UidUsuario)
        {
            List<AlumnosGridViewModel> lsAlumnosGridViewModel = new List<AlumnosGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select al.*, es.VchDescripcion, es.VchIcono from Alumnos al, Estatus es, UsuariosAlumnos ua, Usuarios us where al.UidEstatus = es.UidEstatus and ua.UidUsuario = us.UidUsuario and ua.UidAlumno = al.UidAlumno and ua.UidUsuario = '" + UidUsuario + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsAlumnosGridViewModel.Add(new AlumnosGridViewModel()
                {
                    UidAlumno = new Guid(item["UidAlumno"].ToString()),
                    VchNombres = item["VchNombres"].ToString(),
                    VchApePaterno = item["VchApePaterno"].ToString(),
                    VchApeMaterno = item["VchApeMaterno"].ToString(),
                    VchMatricula = item["VchMatricula"].ToString(),
                    blSeleccionado = true
                });
            }

            return lsAlumnosGridViewModel;
        }
        public List<AlumnosGridViewModel> AsignarAlumnos(List<AlumnosGridViewModel> lsSelectAlumnosGridViewModel, Guid UidCliente, Guid UidUsuario, string Nombre, string ApePaterno, string ApeMaterno, string Matricula)
        {
            List<AlumnosGridViewModel> lsAlumnosGridViewModel = new List<AlumnosGridViewModel>();

            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "sp_ClienteBuscarAlumnos";
            try
            {
                comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCliente"].Value = UidCliente;
                
                comando.Parameters.Add("@UidUsuario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidUsuario"].Value = UidUsuario;

                if (Nombre != string.Empty)
                {
                    comando.Parameters.Add("@Nombres", SqlDbType.VarChar);
                    comando.Parameters["@Nombres"].Value = Nombre;
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
                if (Matricula != string.Empty)
                {
                    comando.Parameters.Add("@Matricula", SqlDbType.VarChar);
                    comando.Parameters["@Matricula"].Value = Matricula;
                }

                foreach (DataRow item in this.Busquedas(comando).Rows)
                {

                    bool blSeleccionado = false;

                    foreach (var item2 in lsSelectAlumnosGridViewModel)
                    {
                        if (Guid.Parse(item["UidAlumno"].ToString()) == item2.UidAlumno)
                        {
                            blSeleccionado = true;
                            break;
                        }
                    }

                    lsAlumnosGridViewModel.Add(new AlumnosGridViewModel()
                    {
                        UidAlumno = new Guid(item["UidAlumno"].ToString()),
                        VchNombres = item["VchNombres"].ToString(),
                        VchApePaterno = item["VchApePaterno"].ToString(),
                        VchApeMaterno = item["VchApeMaterno"].ToString(),
                        VchMatricula = item["VchMatricula"].ToString(),
                        blSeleccionado = blSeleccionado
                    });
                }

                return lsAlumnosGridViewModel;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Metodos Colegiaturas
        public bool RegistrarColeAlumnos(Guid UidColegiatura, Guid UidAlumno)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ColegiaturasAlumnosRegistrar";

                comando.Parameters.Add("@UidColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidColegiatura"].Value = UidColegiatura;

                comando.Parameters.Add("@UidAlumno", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidAlumno"].Value = UidAlumno;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        public bool EliminarColeAlumnos(Guid UidColegiatura)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ColegiaturasAlumnosEliminar";

                comando.Parameters.Add("@UidColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidColegiatura"].Value = UidColegiatura;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }

        public List<AlumnosGridViewModel> ObtenerColeAlumnos(Guid UidColegiatura)
        {
            List<AlumnosGridViewModel> lsAlumnosGridViewModel = new List<AlumnosGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select al.*, es.VchDescripcion, es.VchIcono from Alumnos al, Estatus es, ColegiaturasAlumnos ca, Colegiaturas co where al.UidEstatus = es.UidEstatus and ca.UidColegiatura = co.UidColegiatura and ca.UidAlumno = al.UidAlumno and ca.UidColegiatura = '" + UidColegiatura + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsAlumnosGridViewModel.Add(new AlumnosGridViewModel()
                {
                    UidAlumno = new Guid(item["UidAlumno"].ToString()),
                    VchNombres = item["VchNombres"].ToString(),
                    VchApePaterno = item["VchApePaterno"].ToString(),
                    VchApeMaterno = item["VchApeMaterno"].ToString(),
                    VchIdentificador = item["VchIdentificador"].ToString(),
                    VchMatricula = item["VchMatricula"].ToString(),
                    blSeleccionado = true
                });
            }

            return lsAlumnosGridViewModel;
        }
        public List<AlumnosGridViewModel> AsignarColeAlumnos(List<AlumnosGridViewModel> lsExcelSelect, List<AlumnosGridViewModel> lsSelectAlumnosGridViewModel, Guid UidCliente, string Nombre, string ApePaterno, string ApeMaterno, string Matricula)
        {
            List<AlumnosGridViewModel> lsAlumnosGridViewModel = new List<AlumnosGridViewModel>();

            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "sp_ColegiaturaBuscarAlumnos";
            try
            {
                comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCliente"].Value = UidCliente;

                if (Nombre != string.Empty)
                {
                    comando.Parameters.Add("@Nombres", SqlDbType.VarChar);
                    comando.Parameters["@Nombres"].Value = Nombre;
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
                if (Matricula != string.Empty)
                {
                    comando.Parameters.Add("@Matricula", SqlDbType.VarChar);
                    comando.Parameters["@Matricula"].Value = Matricula;
                }

                foreach (DataRow item in this.Busquedas(comando).Rows)
                {

                    bool blSeleccionado = false;

                    foreach (var item1 in lsExcelSelect)
                    {
                        if (item["VchMatricula"].ToString() == item1.VchMatricula)
                        {
                            blSeleccionado = true;
                            break;
                        }
                    }
                    
                    foreach (var item2 in lsSelectAlumnosGridViewModel)
                    {
                        if (Guid.Parse(item["UidAlumno"].ToString()) == item2.UidAlumno)
                        {
                            blSeleccionado = true;
                            break;
                        }
                    }

                    lsAlumnosGridViewModel.Add(new AlumnosGridViewModel()
                    {
                        UidAlumno = new Guid(item["UidAlumno"].ToString()),
                        VchIdentificador = item["VchIdentificador"].ToString(),
                        VchNombres = item["VchNombres"].ToString(),
                        VchApePaterno = item["VchApePaterno"].ToString(),
                        VchApeMaterno = item["VchApeMaterno"].ToString(),
                        VchMatricula = item["VchMatricula"].ToString(),
                        blSeleccionado = blSeleccionado
                    });
                }

                return lsAlumnosGridViewModel;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        #endregion
    }
}
