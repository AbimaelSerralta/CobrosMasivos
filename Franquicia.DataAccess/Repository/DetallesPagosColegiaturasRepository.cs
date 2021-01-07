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
    public class DetallesPagosColegiaturasRepository : SqlDataRepository
    {
        DetallesPagosColegiaturas _detallesPagosColegiaturas = new DetallesPagosColegiaturas();
        public DetallesPagosColegiaturas detallesPagosColegiaturas
        {
            get { return _detallesPagosColegiaturas; }
            set { _detallesPagosColegiaturas = value; }
        }

        #region Metodos PagosColegiatura
        //public List<AlumnosGridViewModel> CargarAlumnos(Guid UidCliente)
        //{
        //    List<AlumnosGridViewModel> lsAlumnosGridViewModel = new List<AlumnosGridViewModel>();

        //    SqlCommand query = new SqlCommand();
        //    query.CommandType = CommandType.Text;

        //    //=>Sin contabilizar Colegiatura  query.CommandText = "select al.*, es.VchDescripcion, es.VchIcono, pt.Prefijo, tu.VchTelefono, (select COUNT(*) from Usuarios usu, UsuariosAlumnos usa, Alumnos alu where usu.UidUsuario = usa.UidUsuario and usa.UidAlumno = alu.UidAlumno and alu.UidAlumno = al.UidAlumno) as CantPadres from Alumnos al, Estatus es, Clientes cl, TelefonosAlumnos tu, PrefijosTelefonicos pt where tu.UidAlumno = al.UidAlumno and pt.UidPrefijo = tu.UidPrefijo and al.UidEstatus = es.UidEstatus and cl.UidCliente = al.UidCliente and al.UidCliente = '" + UidCliente + "' order by al.VchMatricula asc";
        //    query.CommandText = "select al.*, es.VchDescripcion, es.VchIcono, pt.Prefijo, tu.VchTelefono, (select COUNT(*) from Usuarios usu, UsuariosAlumnos usa, Alumnos alu where usu.UidUsuario = usa.UidUsuario and usa.UidAlumno = alu.UidAlumno and alu.UidAlumno = al.UidAlumno) as CantPadres, (select COUNT(*) from clientes cli, Colegiaturas co, FechasColegiaturas fc, ColegiaturasAlumnos ca, Alumnos alu, Usuarios usua, UsuariosAlumnos ua, EstatusFechasColegiaturas efc, Periodicidades pe where pe.UidPeriodicidad = co.UidPeriodicidad and not exists (select * from LigasUrls lu, PagosTarjeta pt where pt.IdReferencia = lu.IdReferencia and pt.VchEstatus = 'approved' and lu.UidFechaColegiatura = fc.UidFechaColegiatura) and co.UidEstatus = '65E46BC9-1864-4145-AD1A-70F5B5F69739' and efc.UidEstatusFechaColegiatura = fc.UidEstatusFechaColegiatura and cli.UidCliente = co.UidCliente and co.UidColegiatura = fc.UidColegiatura and ca.UidColegiatura = co.UidColegiatura and ca.UidAlumno = alu.UidAlumno and ua.UidUsuario = usua.UidUsuario and ua.UidAlumno = alu.UidAlumno and cli.UidCliente = cl.UidCliente and alu.UidAlumno = al.UidAlumno) as CantColegiaturas from Alumnos al, Estatus es, Clientes cl, TelefonosAlumnos tu, PrefijosTelefonicos pt where tu.UidAlumno = al.UidAlumno and pt.UidPrefijo = tu.UidPrefijo and al.UidEstatus = es.UidEstatus and cl.UidCliente = al.UidCliente and al.UidCliente = '" + UidCliente + "' order by al.VchMatricula asc";

        //    DataTable dt = this.Busquedas(query);

        //    foreach (DataRow item in dt.Rows)
        //    {
        //        string BitBeca = "NO";
        //        string Telefono = string.Empty;
        //        string DescripcionAsociado = string.Empty;
        //        string IconoAsociado = string.Empty;
        //        bool blVisibleDesasociar = false;

        //        SqlCommand queryAsociado = new SqlCommand();
        //        queryAsociado.CommandType = CommandType.Text;

        //        queryAsociado.CommandText = "select * from UsuariosAlumnos where UidAlumno = '" + item["UidAlumno"].ToString() + "'";

        //        DataTable dtAsociado = this.Busquedas(queryAsociado);

        //        string ColorNotification = "#f44336";
        //        if (int.Parse(item["CantPadres"].ToString()) >= 1)
        //        {
        //            ColorNotification = "#4CAF50";
        //        }

        //        if (dtAsociado.Rows.Count >= 1)
        //        {
        //            DescripcionAsociado = "ASOCIADO";
        //            IconoAsociado = "check_circle";
        //            blVisibleDesasociar = true;
        //        }
        //        else
        //        {
        //            DescripcionAsociado = "NO ASOCIADO";
        //            IconoAsociado = "cancel";
        //        }

        //        if (bool.Parse(item["BitBeca"].ToString()))
        //        {
        //            BitBeca = "SI";
        //        }

        //        if (!string.IsNullOrEmpty(item["VchTelefono"].ToString()))
        //        {
        //            Telefono = "(" + item["Prefijo"].ToString() + ")" + item["VchTelefono"].ToString();
        //        }

        //        lsAlumnosGridViewModel.Add(new AlumnosGridViewModel()
        //        {
        //            UidAlumno = new Guid(item["UidAlumno"].ToString()),
        //            VchIdentificador = item["VchIdentificador"].ToString(),
        //            VchNombres = item["VchNombres"].ToString(),
        //            VchApePaterno = item["VchApePaterno"].ToString(),
        //            VchApeMaterno = item["VchApeMaterno"].ToString(),
        //            VchMatricula = item["VchMatricula"].ToString(),
        //            VchCorreo = item["VchCorreo"].ToString(),
        //            VchTelefono = Telefono,
        //            VchBeca = BitBeca,
        //            BitBeca = bool.Parse(item["BitBeca"].ToString()),
        //            VchTipoBeca = item["VchTipoBeca"].ToString(),
        //            DcmBeca = item.IsNull("DcmBeca") ? 0 : decimal.Parse(item["DcmBeca"].ToString()),
        //            UidEstatus = new Guid(item["UidEstatus"].ToString()),
        //            VchDescripcion = item["VchDescripcion"].ToString(),
        //            VchIcono = item["VchIcono"].ToString(),
        //            blVisibleDesasociar = blVisibleDesasociar,
        //            VchDescripcionAsociado = DescripcionAsociado,
        //            VchIconoAsociado = IconoAsociado,
        //            IntCantPadres = int.Parse(item["CantPadres"].ToString()),
        //            ColorNotification = ColorNotification
        //        });
        //    }

        //    return lsAlumnosGridViewModel;
        //}
        public bool RegistrarDetallePagoColegiatura(DetallesPagosColegiaturas detallesPagosColegiaturas)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_DetallesPagosColegiaturasRegistrar";

                comando.Parameters.Add("@IntNum", SqlDbType.Int);
                comando.Parameters["@IntNum"].Value = detallesPagosColegiaturas.IntNum;

                comando.Parameters.Add("@VchDescripcion", SqlDbType.VarChar);
                comando.Parameters["@VchDescripcion"].Value = detallesPagosColegiaturas.VchDescripcion;

                comando.Parameters.Add("@DcmImporte", SqlDbType.Decimal);
                comando.Parameters["@DcmImporte"].Value = detallesPagosColegiaturas.DcmImporte;

                comando.Parameters.Add("@UidPagoColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPagoColegiatura"].Value = detallesPagosColegiaturas.UidPagoColegiatura;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public bool EliminarDetallePagoColegiatura(Guid UidPagoColegiatura)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_DetallesPagosColegiaturasEliminar";

                comando.Parameters.Add("@UidPagoColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPagoColegiatura"].Value = UidPagoColegiatura;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }

        //public List<AlumnosGridViewModel> BuscarAlumnos(string Identificador, string Matricula, string Correo, string Nombre, string ApePaterno, string ApeMaterno, string Celular, string Asociado, string Beca, Guid UidEstatus, string Colegiatura, Guid UidCliente)
        //{
        //    List<AlumnosGridViewModel> lsAlumnosGridViewModel = new List<AlumnosGridViewModel>();

        //    SqlCommand comando = new SqlCommand();
        //    comando.CommandType = CommandType.StoredProcedure;
        //    comando.CommandText = "sp_AlumnosBuscar";
        //    try
        //    {
        //        if (Identificador != string.Empty)
        //        {
        //            comando.Parameters.Add("@Identificador", SqlDbType.VarChar);
        //            comando.Parameters["@Identificador"].Value = Identificador;
        //        }
        //        if (Matricula != string.Empty)
        //        {
        //            comando.Parameters.Add("@Matricula", SqlDbType.VarChar, 50);
        //            comando.Parameters["@Matricula"].Value = Matricula;
        //        }
        //        if (Correo != string.Empty)
        //        {
        //            comando.Parameters.Add("@Correo", SqlDbType.VarChar, 50);
        //            comando.Parameters["@Correo"].Value = Correo;
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
        //        if (Celular != string.Empty)
        //        {
        //            comando.Parameters.Add("@Celular", SqlDbType.VarChar, 50);
        //            comando.Parameters["@Celular"].Value = Celular;
        //        }
        //        if (Beca != string.Empty && Beca != "TODOS")
        //        {
        //            bool bec = false;

        //            if (Beca == "SI")
        //            {
        //                bec = true;
        //            }

        //            comando.Parameters.Add("@Beca", SqlDbType.Bit);
        //            comando.Parameters["@Beca"].Value = bec;
        //        }
        //        if (UidEstatus != Guid.Empty)
        //        {
        //            comando.Parameters.Add("@UidEstatus", SqlDbType.UniqueIdentifier);
        //            comando.Parameters["@UidEstatus"].Value = UidEstatus;
        //        }
        //        if (Colegiatura != string.Empty && Colegiatura != "NO IMPORTA")
        //        {
        //            if (Colegiatura == "SI")
        //            {
        //                comando.Parameters.Add("@Colegiatura", SqlDbType.Int);
        //                comando.Parameters["@Colegiatura"].Value = 1;
        //            }
        //            else
        //            {
        //                comando.Parameters.Add("@SinColegiatura", SqlDbType.Int);
        //                comando.Parameters["@SinColegiatura"].Value = 0;
        //            }
        //        }
        //        if (UidCliente != Guid.Empty)
        //        {
        //            comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
        //            comando.Parameters["@UidCliente"].Value = UidCliente;
        //        }

        //        foreach (DataRow item in this.Busquedas(comando).Rows)
        //        {
        //            string BitBeca = "NO";
        //            string Telefono = string.Empty;
        //            string DescripcionAsociado = string.Empty;
        //            string IconoAsociado = string.Empty;
        //            bool blVisibleDesasociar = false;

        //            SqlCommand queryAsociado = new SqlCommand();
        //            queryAsociado.CommandType = CommandType.Text;

        //            queryAsociado.CommandText = "select * from UsuariosAlumnos where UidAlumno = '" + item["UidAlumno"].ToString() + "'";

        //            DataTable dtAsociado = this.Busquedas(queryAsociado);

        //            string ColorNotification = "#f44336";
        //            if (int.Parse(item["CantPadres"].ToString()) >= 1)
        //            {
        //                ColorNotification = "#4CAF50";
        //            }

        //            if (dtAsociado.Rows.Count >= 1)
        //            {
        //                DescripcionAsociado = "ASOCIADO";
        //                IconoAsociado = "check_circle";
        //                blVisibleDesasociar = true;
        //            }
        //            else
        //            {
        //                DescripcionAsociado = "NO ASOCIADO";
        //                IconoAsociado = "cancel";
        //            }

        //            if (bool.Parse(item["BitBeca"].ToString()))
        //            {
        //                BitBeca = "SI";
        //            }

        //            if (!string.IsNullOrEmpty(item["VchTelefono"].ToString()))
        //            {
        //                Telefono = "(" + item["Prefijo"].ToString() + ")" + item["VchTelefono"].ToString();
        //            }

        //            lsAlumnosGridViewModel.Add(new AlumnosGridViewModel()
        //            {
        //                UidAlumno = new Guid(item["UidAlumno"].ToString()),
        //                VchIdentificador = item["VchIdentificador"].ToString(),
        //                VchNombres = item["VchNombres"].ToString(),
        //                VchApePaterno = item["VchApePaterno"].ToString(),
        //                VchApeMaterno = item["VchApeMaterno"].ToString(),
        //                VchMatricula = item["VchMatricula"].ToString(),
        //                VchCorreo = item["VchCorreo"].ToString(),
        //                VchTelefono = Telefono,
        //                VchBeca = BitBeca,
        //                BitBeca = bool.Parse(item["BitBeca"].ToString()),
        //                VchTipoBeca = item["VchTipoBeca"].ToString(),
        //                DcmBeca = item.IsNull("DcmBeca") ? 0 : decimal.Parse(item["DcmBeca"].ToString()),
        //                UidEstatus = new Guid(item["UidEstatus"].ToString()),
        //                VchDescripcion = item["VchDescripcion"].ToString(),
        //                VchIcono = item["VchIcono"].ToString(),
        //                blVisibleDesasociar = blVisibleDesasociar,
        //                VchDescripcionAsociado = DescripcionAsociado,
        //                VchIconoAsociado = IconoAsociado,
        //                IntCantPadres = int.Parse(item["CantPadres"].ToString()),
        //                ColorNotification = ColorNotification
        //            });
        //        }

        //        if (Asociado != string.Empty && Asociado != "TODOS")
        //        {
        //            return lsAlumnosGridViewModel = lsAlumnosGridViewModel.Where(x => x.VchDescripcionAsociado == Asociado).OrderBy(x => x.VchMatricula).ToList();
        //        }
        //        else
        //        {
        //            return lsAlumnosGridViewModel = lsAlumnosGridViewModel.OrderBy(x => x.VchMatricula).ToList();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        #endregion

        #region Metodos ReporteLigasPadre

        #region ReportViewer
        public List<DetallePagosColeGridViewModel> rdlcObtenerDetallePagoColegiatura(Guid UidPagoColegiatura)
        {
            List<DetallePagosColeGridViewModel> lsDetallePagosColeGridViewModel = new List<DetallePagosColeGridViewModel>();

            SqlCommand query2 = new SqlCommand();
            query2.CommandType = CommandType.Text;

            query2.CommandText = "select * from DetallesPagosColegiaturas where UidPagoColegiatura = '" + UidPagoColegiatura + "' order by IntNum asc";

            DataTable dt2 = this.Busquedas(query2);

            foreach (DataRow item in dt2.Rows)
            {
                string VchColor = "#222";
                if (decimal.Parse(item["DcmImporte"].ToString()) < 0)
                {
                    VchColor = "#f55145";
                }

                lsDetallePagosColeGridViewModel.Add(new DetallePagosColeGridViewModel
                {
                    IntNum = int.Parse(item["IntNum"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    DcmImporte = decimal.Parse(item["DcmImporte"].ToString()),
                    VchColor = VchColor
                });
            }

            return lsDetallePagosColeGridViewModel;
        }
        #endregion
        #endregion
    }
}
