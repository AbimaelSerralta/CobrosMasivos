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


        public string ObtenerIdAlumno(Guid UidAlumno)
        {
            string IdAlumno = "";

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select IdAlumno from Alumnos where UidAlumno = '" + UidAlumno + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                IdAlumno = item["IdAlumno"].ToString();
            }

            return IdAlumno;
        }

        #region Metodos Alumnos
        public List<AlumnosGridViewModel> CargarAlumnos(Guid UidCliente)
        {
            List<AlumnosGridViewModel> lsAlumnosGridViewModel = new List<AlumnosGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            //=>Sin contabilizar Colegiatura  query.CommandText = "select al.*, es.VchDescripcion, es.VchIcono, pt.Prefijo, tu.VchTelefono, (select COUNT(*) from Usuarios usu, UsuariosAlumnos usa, Alumnos alu where usu.UidUsuario = usa.UidUsuario and usa.UidAlumno = alu.UidAlumno and alu.UidAlumno = al.UidAlumno) as CantPadres from Alumnos al, Estatus es, Clientes cl, TelefonosAlumnos tu, PrefijosTelefonicos pt where tu.UidAlumno = al.UidAlumno and pt.UidPrefijo = tu.UidPrefijo and al.UidEstatus = es.UidEstatus and cl.UidCliente = al.UidCliente and al.UidCliente = '" + UidCliente + "' order by al.VchMatricula asc";
            query.CommandText = "select al.*, es.VchDescripcion, es.VchIcono, pt.Prefijo, tu.VchTelefono, (select COUNT(*) from Usuarios usu, UsuariosAlumnos usa, Alumnos alu where usu.UidUsuario = usa.UidUsuario and usa.UidAlumno = alu.UidAlumno and alu.UidAlumno = al.UidAlumno) as CantPadres, (select COUNT(*) from clientes cli, Colegiaturas co, FechasColegiaturas fc, ColegiaturasAlumnos ca, Alumnos alu, Usuarios usua, UsuariosAlumnos ua, EstatusFechasColegiaturas efc, Periodicidades pe where pe.UidPeriodicidad = co.UidPeriodicidad and not exists (select * from LigasUrls lu, PagosTarjeta pt where pt.IdReferencia = lu.IdReferencia and pt.VchEstatus = 'approved' and lu.UidFechaColegiatura = fc.UidFechaColegiatura) and co.UidEstatus = '65E46BC9-1864-4145-AD1A-70F5B5F69739' and efc.UidEstatusFechaColegiatura = fc.UidEstatusFechaColegiatura and cli.UidCliente = co.UidCliente and co.UidColegiatura = fc.UidColegiatura and ca.UidColegiatura = co.UidColegiatura and ca.UidAlumno = alu.UidAlumno and ua.UidUsuario = usua.UidUsuario and ua.UidAlumno = alu.UidAlumno and cli.UidCliente = cl.UidCliente and alu.UidAlumno = al.UidAlumno) as CantColegiaturas from Alumnos al, Estatus es, Clientes cl, TelefonosAlumnos tu, PrefijosTelefonicos pt where tu.UidAlumno = al.UidAlumno and pt.UidPrefijo = tu.UidPrefijo and al.UidEstatus = es.UidEstatus and cl.UidCliente = al.UidCliente and al.UidCliente = '" + UidCliente + "' order by al.VchMatricula asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                string BitBeca = "NO";
                string Telefono = string.Empty;
                string DescripcionAsociado = string.Empty;
                string IconoAsociado = string.Empty;
                bool blVisibleDesasociar = false;

                SqlCommand queryAsociado = new SqlCommand();
                queryAsociado.CommandType = CommandType.Text;

                queryAsociado.CommandText = "select * from UsuariosAlumnos where UidAlumno = '" + item["UidAlumno"].ToString() + "'";

                DataTable dtAsociado = this.Busquedas(queryAsociado);

                string ColorNotification = "#f44336";
                if (int.Parse(item["CantPadres"].ToString()) >= 1)
                {
                    ColorNotification = "#4CAF50";
                }

                if (dtAsociado.Rows.Count >= 1)
                {
                    DescripcionAsociado = "ASOCIADO";
                    IconoAsociado = "check_circle";
                    blVisibleDesasociar = true;
                }
                else
                {
                    DescripcionAsociado = "NO ASOCIADO";
                    IconoAsociado = "cancel";
                }

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
                    VchIcono = item["VchIcono"].ToString(),
                    blVisibleDesasociar = blVisibleDesasociar,
                    VchDescripcionAsociado = DescripcionAsociado,
                    VchIconoAsociado = IconoAsociado,
                    IntCantPadres = int.Parse(item["CantPadres"].ToString()),
                    ColorNotification = ColorNotification
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
        public List<AlumnosGridViewModel> BuscarAlumnos(string Identificador, string Matricula, string Correo, string Nombre, string ApePaterno, string ApeMaterno, string Celular, string Asociado, string Beca, Guid UidEstatus, string Colegiatura, Guid UidCliente)
        {
            List<AlumnosGridViewModel> lsAlumnosGridViewModel = new List<AlumnosGridViewModel>();

            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "sp_AlumnosBuscar";
            try
            {
                if (Identificador != string.Empty)
                {
                    comando.Parameters.Add("@Identificador", SqlDbType.VarChar);
                    comando.Parameters["@Identificador"].Value = Identificador;
                }
                if (Matricula != string.Empty)
                {
                    comando.Parameters.Add("@Matricula", SqlDbType.VarChar, 50);
                    comando.Parameters["@Matricula"].Value = Matricula;
                }
                if (Correo != string.Empty)
                {
                    comando.Parameters.Add("@Correo", SqlDbType.VarChar, 50);
                    comando.Parameters["@Correo"].Value = Correo;
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
                if (Celular != string.Empty)
                {
                    comando.Parameters.Add("@Celular", SqlDbType.VarChar, 50);
                    comando.Parameters["@Celular"].Value = Celular;
                }
                if (Beca != string.Empty && Beca != "TODOS")
                {
                    bool bec = false;

                    if (Beca == "SI")
                    {
                        bec = true;
                    }

                    comando.Parameters.Add("@Beca", SqlDbType.Bit);
                    comando.Parameters["@Beca"].Value = bec;
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

                foreach (DataRow item in this.Busquedas(comando).Rows)
                {
                    string BitBeca = "NO";
                    string Telefono = string.Empty;
                    string DescripcionAsociado = string.Empty;
                    string IconoAsociado = string.Empty;
                    bool blVisibleDesasociar = false;

                    SqlCommand queryAsociado = new SqlCommand();
                    queryAsociado.CommandType = CommandType.Text;

                    queryAsociado.CommandText = "select * from UsuariosAlumnos where UidAlumno = '" + item["UidAlumno"].ToString() + "'";

                    DataTable dtAsociado = this.Busquedas(queryAsociado);

                    string ColorNotification = "#f44336";
                    if (int.Parse(item["CantPadres"].ToString()) >= 1)
                    {
                        ColorNotification = "#4CAF50";
                    }

                    if (dtAsociado.Rows.Count >= 1)
                    {
                        DescripcionAsociado = "ASOCIADO";
                        IconoAsociado = "check_circle";
                        blVisibleDesasociar = true;
                    }
                    else
                    {
                        DescripcionAsociado = "NO ASOCIADO";
                        IconoAsociado = "cancel";
                    }

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
                        VchIcono = item["VchIcono"].ToString(),
                        blVisibleDesasociar = blVisibleDesasociar,
                        VchDescripcionAsociado = DescripcionAsociado,
                        VchIconoAsociado = IconoAsociado,
                        IntCantPadres = int.Parse(item["CantPadres"].ToString()),
                        ColorNotification = ColorNotification
                    });
                }

                if (Asociado != string.Empty && Asociado != "TODOS")
                {
                    return lsAlumnosGridViewModel = lsAlumnosGridViewModel.Where(x => x.VchDescripcionAsociado == Asociado).OrderBy(x => x.VchMatricula).ToList();
                }
                else
                {
                    return lsAlumnosGridViewModel = lsAlumnosGridViewModel.OrderBy(x => x.VchMatricula).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DesasociarPadreAlumno(Guid UidUsuario, Guid UidAlumno)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_AlumnosDesasociarPadres";

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
        public bool EliminarClienteAlumnos(Guid UidCliente, Guid UidUsuario)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ClientesAlumnosEliminar";

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
        public List<AlumnosGridViewModel> ObtenerClienteAlumnos(Guid UidUidCliente, Guid UidUsuario)
        {
            List<AlumnosGridViewModel> lsAlumnosGridViewModel = new List<AlumnosGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            // TRAE TODOS LOS ALUMNOS SIN IMPORTAR LA ESCUELA query.CommandText = "select al.*, es.VchDescripcion, es.VchIcono from Alumnos al, Estatus es, UsuariosAlumnos ua, Usuarios us where al.UidEstatus = es.UidEstatus and ua.UidUsuario = us.UidUsuario and ua.UidAlumno = al.UidAlumno and ua.UidUsuario = '" + UidUsuario + "'";
            query.CommandText = "select al.*, es.VchDescripcion, es.VchIcono from Alumnos al, Estatus es, UsuariosAlumnos ua, Usuarios us, Clientes cl where cl.UidCliente = al.UidCliente and al.UidEstatus = es.UidEstatus and ua.UidUsuario = us.UidUsuario and ua.UidAlumno = al.UidAlumno and cl.UidCliente = '" + UidUidCliente + "' and ua.UidUsuario = '" + UidUsuario + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsAlumnosGridViewModel.Add(new AlumnosGridViewModel()
                {
                    UidAlumno = new Guid(item["UidAlumno"].ToString()),
                    VchIdentificador = item["VchIdentificador"].ToString(),
                    VchNombres = item["VchNombres"].ToString(),
                    VchApePaterno = item["VchApePaterno"].ToString(),
                    VchApeMaterno = item["VchApeMaterno"].ToString(),
                    VchMatricula = item["VchMatricula"].ToString(),
                    blSeleccionado = true
                });
            }

            return lsAlumnosGridViewModel;
        }
        public List<AlumnosGridViewModel> AsignarAlumnos(List<AlumnosGridViewModel> lsSelectAlumnosGridViewModel, Guid UidCliente, Guid UidUsuario, string IntCanAlum, string Identificador, string Nombre, string ApePaterno, string ApeMaterno, string Matricula)
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

                if (IntCanAlum != string.Empty)
                {
                    comando.Parameters.Add("@IntCanAlum", SqlDbType.Int);
                    comando.Parameters["@IntCanAlum"].Value = IntCanAlum;
                }
                if (Identificador != string.Empty)
                {
                    comando.Parameters.Add("@Identificador", SqlDbType.VarChar);
                    comando.Parameters["@Identificador"].Value = Identificador;
                }
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
        public List<AlumnosGridViewModel> AsignarColeAlumnos(List<AlumnosGridViewModel> lsExcelSelect, List<AlumnosGridViewModel> lsSelectAlumnosGridViewModel, Guid UidCliente, string Identificador, string Nombre, string ApePaterno, string ApeMaterno, string Matricula)
        {
            List<AlumnosGridViewModel> lsAlumnosGridViewModel = new List<AlumnosGridViewModel>();

            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "sp_ColegiaturaBuscarAlumnos";
            try
            {
                comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCliente"].Value = UidCliente;

                if (Identificador != string.Empty)
                {
                    comando.Parameters.Add("@Identificador", SqlDbType.VarChar);
                    comando.Parameters["@Identificador"].Value = Identificador;
                }
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

        #region Metodos de Excel

        #region Alumnos
        public bool AccionAlumnosExcelToList(List<AlumnosGridViewModel> lsAccionAlumnos, Guid UidCliente)
        {
            bool result = false;

            List<LigasUsuariosGridViewModel> lsLigasUsuariosGridViewModel = new List<LigasUsuariosGridViewModel>();

            foreach (var item in lsAccionAlumnos)
            {
                SqlCommand query = new SqlCommand();
                query.CommandType = CommandType.Text;

                query.CommandText = "select UidAlumno from Alumnos where VchMatricula = '" + item.VchMatricula + "' and UidCliente = '" + UidCliente + "'";

                DataTable exist = this.Busquedas(query);

                if (exist.Rows.Count >= 1)
                {
                    foreach (DataRow itemExist in exist.Rows)
                    {
                        result = ActualizarAlumno(
                            new Alumnos
                            {
                                UidAlumno = Guid.Parse(itemExist["UidAlumno"].ToString()),
                                VchIdentificador = item.VchIdentificador,
                                VchNombres = item.VchNombres,
                                VchApePaterno = item.VchApePaterno,
                                VchApeMaterno = item.VchApeMaterno,
                                VchMatricula = item.VchMatricula,
                                VchCorreo = item.VchCorreo,
                                BitBeca = item.BitBeca,
                                VchTipoBeca = item.VchTipoBeca,
                                DcmBeca = item.DcmBeca,
                                UidEstatus = item.UidEstatus
                            },
                            new TelefonosAlumnos
                            {
                                VchTelefono = item.VchTelefono,
                                UidPrefijo = item.UidPrefijo
                            });
                    }
                }
                else
                {
                    result = RegistrarAlumno(
                        new Alumnos
                        {
                            UidAlumno = Guid.NewGuid(),
                            VchIdentificador = item.VchIdentificador,
                            VchNombres = item.VchNombres,
                            VchApePaterno = item.VchApePaterno,
                            VchApeMaterno = item.VchApeMaterno,
                            VchMatricula = item.VchMatricula,
                            VchCorreo = item.VchCorreo,
                            BitBeca = item.BitBeca,
                            VchTipoBeca = item.VchTipoBeca,
                            DcmBeca = item.DcmBeca,
                            UidCliente = UidCliente
                        },
                        new TelefonosAlumnos
                        {
                            VchTelefono = item.VchTelefono,
                            UidPrefijo = item.UidPrefijo
                        });
                }
            }

            return result;
        }
        //public bool EliminarAlumnoCliente(Guid UidAlumno)
        //{
        //    bool Resultado = false;

        //    SqlCommand comando = new SqlCommand();
        //    try
        //    {
        //        comando.CommandType = System.Data.CommandType.StoredProcedure;
        //        comando.CommandText = "sp_ClientesAlumnosDesasociar";

        //        comando.Parameters.Add("@UidAlumno", SqlDbType.UniqueIdentifier);
        //        comando.Parameters["@UidAlumno"].Value = UidAlumno;

        //        Resultado = this.ManipulacionDeDatos(comando);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return Resultado;
        //}
        #endregion

        #endregion

        #region Metodos ReporteLigasEscuelas
        public List<AlumnosRLEGridViewModel> BuscarAlumnosRLE(Guid UidCliente, string Identificador, string Nombre, string ApePaterno, string ApeMaterno, string Matricula)
        {
            List<AlumnosRLEGridViewModel> lsAlumnosRLEGridViewModel = new List<AlumnosRLEGridViewModel>();

            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "sp_ReporteLigasEscuelasBuscarAlumnos";
            try
            {
                comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCliente"].Value = UidCliente;

                if (Identificador != string.Empty)
                {
                    comando.Parameters.Add("@Identificador", SqlDbType.VarChar);
                    comando.Parameters["@Identificador"].Value = Identificador;
                }
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
                    lsAlumnosRLEGridViewModel.Add(new AlumnosRLEGridViewModel()
                    {
                        UidAlumno = new Guid(item["UidAlumno"].ToString()),
                        VchIdentificador = item["VchIdentificador"].ToString(),
                        VchNombres = item["VchNombres"].ToString(),
                        VchApePaterno = item["VchApePaterno"].ToString(),
                        VchApeMaterno = item["VchApeMaterno"].ToString(),
                        VchMatricula = item["VchMatricula"].ToString()
                    });
                }

                return lsAlumnosRLEGridViewModel;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
