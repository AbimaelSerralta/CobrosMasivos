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
    public class PagosColegiaturasRepository : SqlDataRepository
    {
        PagosColegiaturas _pagosColegiaturas = new PagosColegiaturas();
        public PagosColegiaturas pagosColegiaturas
        {
            get { return _pagosColegiaturas; }
            set { _pagosColegiaturas = value; }
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
        public bool RegistrarPagoColegiatura(PagosColegiaturas pagosColegiaturas, Guid UidFechaColegiatura, Guid UidAlumno, Guid UidFormaPago, decimal DcmImporteCole, decimal DcmImportePagado, decimal DcmImporteNuevo, Guid EstatusFechaPago)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PagosColegiaturasRegistrar";

                comando.Parameters.Add("@UidPagoColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPagoColegiatura"].Value = pagosColegiaturas.UidPagoColegiatura;

                comando.Parameters.Add("@DtFHPago", SqlDbType.DateTime);
                comando.Parameters["@DtFHPago"].Value = pagosColegiaturas.DtFHPago;

                comando.Parameters.Add("@VchPromocionDePago", SqlDbType.VarChar);
                comando.Parameters["@VchPromocionDePago"].Value = pagosColegiaturas.VchPromocionDePago;

                comando.Parameters.Add("@VchComisionBancaria", SqlDbType.VarChar);
                comando.Parameters["@VchComisionBancaria"].Value = pagosColegiaturas.VchComisionBancaria;

                comando.Parameters.Add("@BitSubtotal", SqlDbType.Bit);
                comando.Parameters["@BitSubtotal"].Value = pagosColegiaturas.BitSubtotal;

                comando.Parameters.Add("@DcmSubtotal", SqlDbType.Decimal);
                comando.Parameters["@DcmSubtotal"].Value = pagosColegiaturas.DcmSubtotal;

                comando.Parameters.Add("@BitComisionBancaria", SqlDbType.Decimal);
                comando.Parameters["@BitComisionBancaria"].Value = pagosColegiaturas.BitComisionBancaria;

                comando.Parameters.Add("@DcmComisionBancaria", SqlDbType.Decimal);
                comando.Parameters["@DcmComisionBancaria"].Value = pagosColegiaturas.DcmComisionBancaria;

                comando.Parameters.Add("@BitPromocionDePago", SqlDbType.Decimal);
                comando.Parameters["@BitPromocionDePago"].Value = pagosColegiaturas.BitPromocionDePago;

                comando.Parameters.Add("@DcmPromocionDePago", SqlDbType.Decimal);
                comando.Parameters["@DcmPromocionDePago"].Value = pagosColegiaturas.DcmPromocionDePago;

                comando.Parameters.Add("@DcmTotal", SqlDbType.Decimal);
                comando.Parameters["@DcmTotal"].Value = pagosColegiaturas.DcmTotal;
                
                comando.Parameters.Add("@UidUsuario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidUsuario"].Value = pagosColegiaturas.UidUsuario;

                //=============================================================================================

                comando.Parameters.Add("@UidFechaColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidFechaColegiatura"].Value = UidFechaColegiatura;

                comando.Parameters.Add("@UidAlumno", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidAlumno"].Value = UidAlumno;

                comando.Parameters.Add("@UidFormaPago", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidFormaPago"].Value = UidFormaPago;

                comando.Parameters.Add("@DcmImporteCole", SqlDbType.Decimal);
                comando.Parameters["@DcmImporteCole"].Value = DcmImporteCole;

                comando.Parameters.Add("@DcmImportePagado", SqlDbType.Decimal);
                comando.Parameters["@DcmImportePagado"].Value = DcmImportePagado;

                comando.Parameters.Add("@DcmImporteNuevo", SqlDbType.Decimal);
                comando.Parameters["@DcmImporteNuevo"].Value = DcmImporteNuevo;
                
                comando.Parameters.Add("@EstatusFechaPago", SqlDbType.UniqueIdentifier);
                comando.Parameters["@EstatusFechaPago"].Value = EstatusFechaPago;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public bool ActualizarPagoColegiatura(Alumnos Alumnos, TelefonosAlumnos telefonosAlumnos)
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
        public bool EliminarPagoColegiatura(Guid UidPagoColegiatura)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PagosColegiaturasEliminar";

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

        #region Metodos Clientes

        #endregion

        #region Metodos Colegiaturas

        #endregion

    }
}
