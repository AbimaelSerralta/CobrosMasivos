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
        public int ObtenerUltimoFolio(Guid UidCliente)
        {
            int UltimoFolio = 0;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select MAX(pc.IntFolio) as UltimoFolio from PagosColegiaturas pc, FechasPagos fp, Alumnos al where pc.UidPagoColegiatura = fp.UidPagoColegiatura and al.UidAlumno = fp.UidAlumno and al.UidCliente = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                UltimoFolio = item.IsNull("UltimoFolio") ? 0 : int.Parse(item["UltimoFolio"].ToString());
            }

            return UltimoFolio + 1;
        }
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

                comando.Parameters.Add("@IntFolio", SqlDbType.Int);
                comando.Parameters["@IntFolio"].Value = pagosColegiaturas.IntFolio;

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

                comando.Parameters.Add("@BitComisionBancaria", SqlDbType.Bit);
                comando.Parameters["@BitComisionBancaria"].Value = pagosColegiaturas.BitComisionBancaria;

                comando.Parameters.Add("@DcmComisionBancaria", SqlDbType.Decimal);
                comando.Parameters["@DcmComisionBancaria"].Value = pagosColegiaturas.DcmComisionBancaria;

                comando.Parameters.Add("@BitPromocionDePago", SqlDbType.Bit);
                comando.Parameters["@BitPromocionDePago"].Value = pagosColegiaturas.BitPromocionDePago;

                comando.Parameters.Add("@DcmPromocionDePago", SqlDbType.Decimal);
                comando.Parameters["@DcmPromocionDePago"].Value = pagosColegiaturas.DcmPromocionDePago;

                comando.Parameters.Add("@BitValidarImporte", SqlDbType.Bit);
                comando.Parameters["@BitValidarImporte"].Value = pagosColegiaturas.BitValidarImporte;

                comando.Parameters.Add("@DcmValidarImporte", SqlDbType.Decimal);
                comando.Parameters["@DcmValidarImporte"].Value = pagosColegiaturas.DcmValidarImporte;

                comando.Parameters.Add("@DcmTotal", SqlDbType.Decimal);
                comando.Parameters["@DcmTotal"].Value = pagosColegiaturas.DcmTotal;

                comando.Parameters.Add("@UidUsuario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidUsuario"].Value = pagosColegiaturas.UidUsuario;

                comando.Parameters.Add("@UidEstatusPagoColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEstatusPagoColegiatura"].Value = pagosColegiaturas.UidEstatusPagoColegiatura;

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
        public bool RegistrarPagoColegiatura2(PagosColegiaturas pagosColegiaturas, int IdParcialidad, Guid UidFechaColegiatura, Guid UidAlumno, Guid UidFormaPago, decimal DcmImporteCole, decimal DcmImportePagado, decimal DcmImporteNuevo, Guid EstatusFechaPago)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PagosColegiaturasRegistrar2";

                comando.Parameters.Add("@UidPagoColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPagoColegiatura"].Value = pagosColegiaturas.UidPagoColegiatura;

                comando.Parameters.Add("@IntFolio", SqlDbType.Int);
                comando.Parameters["@IntFolio"].Value = pagosColegiaturas.IntFolio;

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

                comando.Parameters.Add("@BitComisionBancaria", SqlDbType.Bit);
                comando.Parameters["@BitComisionBancaria"].Value = pagosColegiaturas.BitComisionBancaria;

                comando.Parameters.Add("@DcmComisionBancaria", SqlDbType.Decimal);
                comando.Parameters["@DcmComisionBancaria"].Value = pagosColegiaturas.DcmComisionBancaria;

                comando.Parameters.Add("@BitPromocionDePago", SqlDbType.Bit);
                comando.Parameters["@BitPromocionDePago"].Value = pagosColegiaturas.BitPromocionDePago;

                comando.Parameters.Add("@DcmPromocionDePago", SqlDbType.Decimal);
                comando.Parameters["@DcmPromocionDePago"].Value = pagosColegiaturas.DcmPromocionDePago;

                comando.Parameters.Add("@BitValidarImporte", SqlDbType.Bit);
                comando.Parameters["@BitValidarImporte"].Value = pagosColegiaturas.BitValidarImporte;

                comando.Parameters.Add("@DcmValidarImporte", SqlDbType.Decimal);
                comando.Parameters["@DcmValidarImporte"].Value = pagosColegiaturas.DcmValidarImporte;

                comando.Parameters.Add("@DcmTotal", SqlDbType.Decimal);
                comando.Parameters["@DcmTotal"].Value = pagosColegiaturas.DcmTotal;

                comando.Parameters.Add("@UidUsuario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidUsuario"].Value = pagosColegiaturas.UidUsuario;

                comando.Parameters.Add("@UidEstatusPagoColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEstatusPagoColegiatura"].Value = pagosColegiaturas.UidEstatusPagoColegiatura;

                //=============================================================================================

                comando.Parameters.Add("@IdParcialidad", SqlDbType.Int);
                comando.Parameters["@IdParcialidad"].Value = IdParcialidad;
                
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
        #region Pagos
        public List<FechasPagosColegiaturasViewModel> ObtenerPagosPadres(Guid UidFechaColegiatura, Guid UidAlumno)
        {
            List<FechasPagosColegiaturasViewModel> lsFechasPagosColegiaturasViewModel = new List<FechasPagosColegiaturasViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            // ==> Revisar consulta query.CommandText = "select paco.DtFHPago, fepa.DcmImportePagado from FechasColegiaturas feco, FechasPagos fepa, PagosColegiaturas paco where feco.UidFechaColegiatura = fepa.UidFechaColegiatura and fepa.UidPagoColegiatura = paco.UidPagoColegiatura and fepa.UidEstatusFechaPago != '8720B2B9-5712-4E75-A981-932887AACDC9' and paco.UidEstatusPagoColegiatura = '51B85D66-866B-4BC2-B08F-FECE1A994053' and fepa.UidFechaColegiatura = '" + UidFechaColegiatura + "' and fepa.UidAlumno = '" + UidAlumno + "'";
            query.CommandText = "select paco.DtFHPago, fepa.DcmImportePagado, efp.UidEstatusFechaPago, efp.VchDescripcion  as EstatusPago from FechasColegiaturas feco, FechasPagos fepa, PagosColegiaturas paco, EstatusFechasPagos efp where efp.UidEstatusFechaPago = fepa.UidEstatusFechaPago and feco.UidFechaColegiatura = fepa.UidFechaColegiatura and fepa.UidPagoColegiatura = paco.UidPagoColegiatura and fepa.UidEstatusFechaPago = '8720B2B9-5712-4E75-A981-932887AACDC9' and paco.UidEstatusPagoColegiatura = '51B85D66-866B-4BC2-B08F-FECE1A994053' and fepa.UidFechaColegiatura = '" + UidFechaColegiatura + "' and fepa.UidAlumno = '" + UidAlumno + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsFechasPagosColegiaturasViewModel.Add(new FechasPagosColegiaturasViewModel()
                {
                    UidEstatusFechaPago = Guid.Parse(item["UidEstatusFechaPago"].ToString()),
                    VchEstatus = item["EstatusPago"].ToString(),
                    DtFHPago = DateTime.Parse(item["DtFHPago"].ToString()),
                    DcmImportePagado = decimal.Parse(item["DcmImportePagado"].ToString())
                });

            }
            return lsFechasPagosColegiaturasViewModel;
        }

        public List<FechasPagosColegiaturasViewModel> ObtenerPagosPendientesPadres(Guid UidFechaColegiatura, Guid UidAlumno)
        {
            List<FechasPagosColegiaturasViewModel> lsFechasPagosColegiaturasViewModel = new List<FechasPagosColegiaturasViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select paco.DtFHPago, fepa.DcmImportePagado, efp.UidEstatusFechaPago, efp.VchDescripcion  as EstatusPago from FechasColegiaturas feco, FechasPagos fepa, PagosColegiaturas paco, EstatusFechasPagos efp where efp.UidEstatusFechaPago = fepa.UidEstatusFechaPago and feco.UidFechaColegiatura = fepa.UidFechaColegiatura and fepa.UidPagoColegiatura = paco.UidPagoColegiatura and fepa.UidEstatusFechaPago = 'F25E4AAB-6044-46E9-A575-98DCBCCF7604' and paco.UidEstatusPagoColegiatura = '51B85D66-866B-4BC2-B08F-FECE1A994053' and fepa.UidFechaColegiatura = '" + UidFechaColegiatura + "' and fepa.UidAlumno = '" + UidAlumno + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsFechasPagosColegiaturasViewModel.Add(new FechasPagosColegiaturasViewModel()
                {
                    UidEstatusFechaPago = Guid.Parse(item["UidEstatusFechaPago"].ToString()),
                    VchEstatus = item["EstatusPago"].ToString(),
                    DtFHPago = DateTime.Parse(item["DtFHPago"].ToString()),
                    DcmImportePagado = decimal.Parse(item["DcmImportePagado"].ToString()),
                    Comentario = "NO SE HA APROVADO"
                });
            }
            return lsFechasPagosColegiaturasViewModel;
        }
        public List<ReportePadresFechasPagosColeViewModel> ObtenerPagosReportePadres(Guid UidFechaColegiatura, string VchMatricula)
        {
            List<ReportePadresFechasPagosColeViewModel> lsReportePadresFechasPagosColeViewModel = new List<ReportePadresFechasPagosColeViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select pc.UidPagoColegiatura, us.VchNombre, us.VchApePaterno, us.VchApeMaterno, pc.DtFHPago, fop.VchDescripcion as VchFormaPago, fp.DcmImportePagado, efp.VchDescripcion as EstatusPago, efp.VchColor from PagosColegiaturas pc, FechasPagos fp, FechasColegiaturas fc, FormasPagos fop, Usuarios us, Alumnos al, EstatusFechasPagos efp where efp.UidEstatusFechaPago = fp.UidEstatusFechaPago and al.UidAlumno = fp.UidAlumno and us.UidUsuario = pc.UidUsuario and fop.UidFormaPago = fp.UidFormaPago and fp.UidPagoColegiatura = pc.UidPagoColegiatura and fp.UidFechaColegiatura = fc.UidFechaColegiatura and pc.UidEstatusPagoColegiatura = '51B85D66-866B-4BC2-B08F-FECE1A994053' and fc.UidFechaColegiatura = '" + UidFechaColegiatura + "' and al.VchMatricula = '" + VchMatricula + "' order by DtFHPago desc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsReportePadresFechasPagosColeViewModel.Add(new ReportePadresFechasPagosColeViewModel()
                {
                    UidPagoColegiatura = Guid.Parse(item["UidPagoColegiatura"].ToString()),
                    VchNombre = item["VchNombre"].ToString(),
                    VchApePaterno = item["VchApePaterno"].ToString(),
                    VchApeMaterno = item["VchApeMaterno"].ToString(),
                    DtFHPago = DateTime.Parse(item["DtFHPago"].ToString()),
                    VchFormaPago = item["VchFormaPago"].ToString(),
                    DcmImportePagado = decimal.Parse(item["DcmImportePagado"].ToString()),
                    VchEstatus = item["EstatusPago"].ToString(),
                    VchColor = item["VchColor"].ToString()
                });

            }
            return lsReportePadresFechasPagosColeViewModel;
        }
        #endregion

        #region Metodos ReporteLigasPadres
        public List<ReportePadresFechasPagosColeViewModel> ObtenerPagosPadresReporte(Guid UidFechaColegiatura, string VchMatricula)
        {
            List<ReportePadresFechasPagosColeViewModel> lsReportePadresFechasPagosColeViewModel = new List<ReportePadresFechasPagosColeViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select pc.UidPagoColegiatura, us.VchNombre, us.VchApePaterno, us.VchApeMaterno, pc.DtFHPago, fop.UidFormaPago, fop.VchDescripcion as VchFormaPago, fp.DcmImportePagado, efp.UidEstatusFechaPago, efp.VchDescripcion as EstatusPago, efp.VchColor from PagosColegiaturas pc, FechasPagos fp, FechasColegiaturas fc, FormasPagos fop, Usuarios us, Alumnos al, EstatusFechasPagos efp where efp.UidEstatusFechaPago = fp.UidEstatusFechaPago and al.UidAlumno = fp.UidAlumno and us.UidUsuario = pc.UidUsuario and fop.UidFormaPago = fp.UidFormaPago and fp.UidPagoColegiatura = pc.UidPagoColegiatura and fp.UidFechaColegiatura = fc.UidFechaColegiatura and pc.UidEstatusPagoColegiatura = '51B85D66-866B-4BC2-B08F-FECE1A994053' and fc.UidFechaColegiatura = '" + UidFechaColegiatura + "' and al.VchMatricula = '" + VchMatricula + "' order by DtFHPago desc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {


                bool blCancelRefClub = false;
                bool blMostrarRefClub = false;

                if (Guid.Parse(item["UidFormaPago"].ToString()) == Guid.Parse("6BE13FFE-E567-4D4D-9CBC-37DA30EC23A5"))
                {
                    if (Guid.Parse(item["UidEstatusFechaPago"].ToString()) != Guid.Parse("408431CA-DB94-4BAA-AB9B-8FF468A77582"))
                    {
                        blCancelRefClub = true;
                    }
                    blMostrarRefClub = true;
                }

                lsReportePadresFechasPagosColeViewModel.Add(new ReportePadresFechasPagosColeViewModel()
                {
                    UidPagoColegiatura = Guid.Parse(item["UidPagoColegiatura"].ToString()),
                    VchNombre = item["VchNombre"].ToString(),
                    VchApePaterno = item["VchApePaterno"].ToString(),
                    VchApeMaterno = item["VchApeMaterno"].ToString(),
                    DtFHPago = DateTime.Parse(item["DtFHPago"].ToString()),
                    UidFormaPago = Guid.Parse(item["UidFormaPago"].ToString()),
                    VchFormaPago = item["VchFormaPago"].ToString(),
                    DcmImportePagado = decimal.Parse(item["DcmImportePagado"].ToString()),
                    VchEstatus = item["EstatusPago"].ToString(),
                    VchColor = item["VchColor"].ToString(),

                    blCancelRefClub = blCancelRefClub,
                    blMostrarRefClub = blMostrarRefClub
                });

            }
            return lsReportePadresFechasPagosColeViewModel;
        }
        public Tuple<List<PagosColegiaturasViewModels>, List<DetallePagosColeGridViewModel>> ObtenerPagoColegiatura(Guid UidPagoColegiatura)
        {
            List<PagosColegiaturasViewModels> lsPagosColegiaturasViewModels = new List<PagosColegiaturasViewModels>();
            List<DetallePagosColeGridViewModel> lsDetallePagosColeGridViewModel = new List<DetallePagosColeGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            //query.CommandText = "select pc.*, al.VchMatricula, al.VchNombres, al.VchApePaterno, VchApeMaterno from PagosColegiaturas pc, FechasPagos fg, Alumnos al where pc.UidPagoColegiatura = fg.UidPagoColegiatura and fg.UidAlumno = al.UidAlumno and pc.UidPagoColegiatura = '" + UidPagoColegiatura + "'";
            query.CommandText = "select pc.*, al.VchMatricula, al.VchNombres, al.VchApePaterno, VchApeMaterno, fp.DcmImporteCole from PagosColegiaturas pc, FechasPagos fg, Alumnos al, FechasPagos fp where fp.UidPagoColegiatura = pc.UidPagoColegiatura and pc.UidPagoColegiatura = fg.UidPagoColegiatura and fg.UidAlumno = al.UidAlumno and pc.UidPagoColegiatura = '" + UidPagoColegiatura + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsPagosColegiaturasViewModels.Add(new PagosColegiaturasViewModels
                {
                    VchMatricula = item["VchMatricula"].ToString(),
                    VchNombres = item["VchNombres"].ToString(),
                    VchApePaterno = item["VchApePaterno"].ToString(),
                    VchApeMaterno = item["VchApeMaterno"].ToString(),
                    DtFHPago = DateTime.Parse(item["DtFHPago"].ToString()),
                    VchPromocionDePago = item["VchPromocionDePago"].ToString(),
                    VchComisionBancaria = item["VchComisionBancaria"].ToString(),
                    BitSubtotal = bool.Parse(item["BitSubtotal"].ToString()),
                    DcmSubtotal = decimal.Parse(item["DcmSubtotal"].ToString()),
                    BitComisionBancaria = bool.Parse(item["BitComisionBancaria"].ToString()),
                    DcmComisionBancaria = decimal.Parse(item["DcmComisionBancaria"].ToString()),
                    BitPromocionDePago = bool.Parse(item["BitPromocionDePago"].ToString()),
                    DcmPromocionDePago = decimal.Parse(item["DcmPromocionDePago"].ToString()),
                    BitValidarImporte = bool.Parse(item["BitValidarImporte"].ToString()),
                    DcmValidarImporte = decimal.Parse(item["DcmValidarImporte"].ToString()),
                    DcmTotal = decimal.Parse(item["DcmTotal"].ToString()),

                    DcmImporteCole = decimal.Parse(item["DcmImporteCole"].ToString())
                });
            }

            //===============================================================================================
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

            return Tuple.Create(lsPagosColegiaturasViewModels, lsDetallePagosColeGridViewModel);
        }

        public decimal ObtenerImporteResta(Guid UidFechaColegiatura, Guid UidAlumno)
        {
            decimal ImporteResta = 0;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from FechasColegiaturasAlumnos where UidFechaColegiatura = '" + UidFechaColegiatura + "' and UidAlumno = '" + UidAlumno + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                ImporteResta = decimal.Parse(item["DcmImporteResta"].ToString());
            }
            return ImporteResta;
        }
        public bool ActualizarImporteResta(Guid UidFechaColegiatura, Guid UidAlumno, decimal DcmImporteResta)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_FechasColegiaturasAlumnosActualizarImporteResta";

                comando.Parameters.Add("@UidFechaColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidFechaColegiatura"].Value = UidFechaColegiatura;

                comando.Parameters.Add("@UidAlumno", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidAlumno"].Value = UidAlumno;

                comando.Parameters.Add("@DcmImporteResta", SqlDbType.Decimal);
                comando.Parameters["@DcmImporteResta"].Value = DcmImporteResta;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }

        #region ReportViewer
        public List<PagosColegiaturasViewModels> rdlcObtenerPagoColegiatura(Guid UidPagoColegiatura)
        {
            List<PagosColegiaturasViewModels> lsPagosColegiaturasViewModels = new List<PagosColegiaturasViewModels>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select pc.*, al.VchMatricula, al.VchNombres, al.VchApePaterno, VchApeMaterno, fp.DcmImporteCole from PagosColegiaturas pc, FechasPagos fg, Alumnos al, FechasPagos fp where fp.UidPagoColegiatura = pc.UidPagoColegiatura and pc.UidPagoColegiatura = fg.UidPagoColegiatura and fg.UidAlumno = al.UidAlumno and pc.UidPagoColegiatura = '" + UidPagoColegiatura + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsPagosColegiaturasViewModels.Add(new PagosColegiaturasViewModels
                {
                    VchMatricula = item["VchMatricula"].ToString(),
                    VchNombres = item["VchNombres"].ToString(),
                    VchApePaterno = item["VchApePaterno"].ToString(),
                    VchApeMaterno = item["VchApeMaterno"].ToString(),
                    DtFHPago = DateTime.Parse(item["DtFHPago"].ToString()),
                    VchPromocionDePago = item["VchPromocionDePago"].ToString(),
                    VchComisionBancaria = item["VchComisionBancaria"].ToString(),
                    BitSubtotal = bool.Parse(item["BitSubtotal"].ToString()),
                    DcmSubtotal = decimal.Parse(item["DcmSubtotal"].ToString()),
                    BitComisionBancaria = bool.Parse(item["BitComisionBancaria"].ToString()),
                    DcmComisionBancaria = decimal.Parse(item["DcmComisionBancaria"].ToString()),
                    BitPromocionDePago = bool.Parse(item["BitPromocionDePago"].ToString()),
                    DcmPromocionDePago = decimal.Parse(item["DcmPromocionDePago"].ToString()),
                    BitValidarImporte = bool.Parse(item["BitValidarImporte"].ToString()),
                    DcmValidarImporte = decimal.Parse(item["DcmValidarImporte"].ToString()),
                    DcmTotal = decimal.Parse(item["DcmTotal"].ToString()),

                    DcmImporteCole = decimal.Parse(item["DcmImporteCole"].ToString())
                });
            }

            return lsPagosColegiaturasViewModels;
        }

        #endregion
        #endregion
        #endregion

        #region Metodos PanelEscuela
        #region ReporteLigasEscuelas
        public bool ActualizarEstatusFechaPago(Guid UidPagoColegiatura, Guid UidEstatusFechaPago)
        {
            SqlCommand Comando = new SqlCommand();
            bool resultado = false;
            try
            {
                Comando.CommandType = CommandType.StoredProcedure;
                Comando.CommandText = "sp_FechasPagosEstatusActualizar";

                Comando.Parameters.Add("@UidPagoColegiatura", SqlDbType.UniqueIdentifier);
                Comando.Parameters["@UidPagoColegiatura"].Value = UidPagoColegiatura;

                Comando.Parameters.Add("@UidEstatusFechaPago", SqlDbType.UniqueIdentifier);
                Comando.Parameters["@UidEstatusFechaPago"].Value = UidEstatusFechaPago;

                resultado = this.ManipulacionDeDatos(Comando);

            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return resultado;
        }
        public decimal ObtenerPagosPadresRLE(Guid UidFechaColegiatura, Guid UidAlumno)
        {
            decimal DcmImportePagado = 0;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select SUM(fepa.DcmImportePagado) as DcmImportePagado from FechasColegiaturas feco, FechasPagos fepa, PagosColegiaturas paco, EstatusFechasPagos efp where efp.UidEstatusFechaPago = fepa.UidEstatusFechaPago and feco.UidFechaColegiatura = fepa.UidFechaColegiatura and fepa.UidPagoColegiatura = paco.UidPagoColegiatura and fepa.UidEstatusFechaPago = '8720B2B9-5712-4E75-A981-932887AACDC9' and paco.UidEstatusPagoColegiatura = '51B85D66-866B-4BC2-B08F-FECE1A994053' and fepa.UidFechaColegiatura = '" + UidFechaColegiatura + "' and fepa.UidAlumno = '" + UidAlumno + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                DcmImportePagado = item.IsNull("DcmImportePagado") ? 0 : decimal.Parse(item["DcmImportePagado"].ToString());
            }
            return DcmImportePagado;
        }
        public decimal ObtenerPendientesPadresRLE(Guid UidFechaColegiatura, Guid UidAlumno)
        {
            decimal DcmImportePendiente = 0;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select SUM(fepa.DcmImportePagado) as DcmImportePendiente from FechasColegiaturas feco, FechasPagos fepa, PagosColegiaturas paco, EstatusFechasPagos efp where efp.UidEstatusFechaPago = fepa.UidEstatusFechaPago and feco.UidFechaColegiatura = fepa.UidFechaColegiatura and fepa.UidPagoColegiatura = paco.UidPagoColegiatura and fepa.UidEstatusFechaPago = 'F25E4AAB-6044-46E9-A575-98DCBCCF7604' and paco.UidEstatusPagoColegiatura = '51B85D66-866B-4BC2-B08F-FECE1A994053' and fepa.UidFechaColegiatura = '" + UidFechaColegiatura + "' and fepa.UidAlumno = '" + UidAlumno + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                DcmImportePendiente = item.IsNull("DcmImportePendiente") ? 0 : decimal.Parse(item["DcmImportePendiente"].ToString());
            }
            return DcmImportePendiente;
        }
        public Tuple<string, decimal> ObtenerDatosPagoRLE(Guid UidPagoColegiatura)
        {
            string IdReferencia = "";
            decimal Monto = 0;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from ReferenciasClubPago where UidPagoColegiatura = '" + UidPagoColegiatura + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                IdReferencia = item["IdReferencia"].ToString();
                Monto = decimal.Parse(item["DcmImporte"].ToString());
            }
            return Tuple.Create(IdReferencia, Monto);
        }
        #endregion
        #endregion

        #region GenerarReferencia
        public Tuple<string, int, int, bool> GenerarReferencia(Guid UidFechaColegiatura, Guid UidAlumno)
        {
            string Referencia = "";
            int IdPago = 0;
            int IdParcialidad = 0;
            bool Error = false;

            try
            {
                SqlCommand query = new SqlCommand();
                query.CommandType = CommandType.Text;

                query.CommandText = "select cl.IdCliente, al.IdAlumno, fca.IdPago, (select CASE WHEN MAX(IdParcialidad) IS NULL THEN 0 ELSE MAX(IdParcialidad) END AS IdParcialidad from FechasPagos where UidFechaColegiatura = '" + UidFechaColegiatura + "' and UidAlumno = '" + UidAlumno + "') AS IdParcialidad from Clientes cl, Alumnos al, FechasColegiaturasAlumnos fca where cl.UidCliente = al.UidCliente and fca.UidAlumno = al.UidAlumno and fca.UidFechaColegiatura = '" + UidFechaColegiatura + "' and fca.UidAlumno = '" + UidAlumno + "'";

                DataTable dt = this.Busquedas(query);

                foreach (DataRow item in dt.Rows)
                {
                    IdPago = int.Parse(item["IdPago"].ToString());
                    IdParcialidad = int.Parse(item["IdParcialidad"].ToString()) + 1;

                    Referencia = int.Parse(item["IdAlumno"].ToString()).ToString("D6") + int.Parse(item["IdAlumno"].ToString()).ToString("D9") + IdPago.ToString("D4") + IdParcialidad.ToString("D3");

                    Error = true;
                }
            }
            catch (Exception ex)
            {
                string mnsj = ex.Message;
            }

            return Tuple.Create(Referencia, IdPago, IdParcialidad, Error);
        }
        #endregion
    }
}
