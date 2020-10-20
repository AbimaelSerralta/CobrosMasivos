using Franquicia.DataAccess.Common;
using Franquicia.Domain.Models;
using Franquicia.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.DataAccess.Repository
{
    public class ColegiaturasRepository : SqlDataRepository
    {

        private ColegiaturasGridViewModel _colegiaturasGridViewModel = new ColegiaturasGridViewModel();
        public ColegiaturasGridViewModel colegiaturasGridViewModel
        {
            get { return _colegiaturasGridViewModel; }
            set { _colegiaturasGridViewModel = value; }
        }

        private PagosColegiaturasViewModel _pagosColegiaturasViewModel = new PagosColegiaturasViewModel();
        public PagosColegiaturasViewModel pagosColegiaturasViewModel
        {
            get { return _pagosColegiaturasViewModel; }
            set { _pagosColegiaturasViewModel = value; }
        }



        #region MetodosFranquicias

        #endregion

        #region MetodosClientes
        public List<ColegiaturasGridViewModel> CargarColegiaturas(Guid UidCliente)
        {
            List<ColegiaturasGridViewModel> lsColegiaturasGridViewModel = new List<ColegiaturasGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select co.*, pe.VchDescripcion, es.VchDescripcion as Estatus, es.VchIcono from Colegiaturas co, Periodicidades pe, Estatus es where es.UidEstatus = co.UidEstatus and co.UidPeriodicidad = pe.UidPeriodicidad and co.UidCliente = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                bool editar = true;

                #region ConsultarFechasPagos
                SqlCommand queryFP = new SqlCommand();
                queryFP.CommandType = CommandType.Text;

                queryFP.CommandText = "select * from FechasColegiaturas where UidColegiatura = '" + item["UidColegiatura"].ToString() + "'";

                DataTable dtFP = this.Busquedas(queryFP);

                foreach (DataRow itemFP in dtFP.Rows)
                {
                    #region Validar si hay un pago de la fecha
                    SqlCommand queryFCP = new SqlCommand();
                    queryFCP.CommandType = CommandType.Text;

                    queryFCP.CommandText = "select * from LigasUrls lu, PagosTarjeta pt where lu.IdReferencia = pt.IdReferencia and pt.VchEstatus = 'approved' and lu.UidFechaColegiatura = '" + itemFP["UidFechaColegiatura"].ToString() + "'";

                    DataTable dtFCP = this.Busquedas(queryFCP);

                    foreach (DataRow itemFCP in dtFCP.Rows)
                    {
                        editar = false;
                    }
                    #endregion
                }
                #endregion

                string FHLimite = "NO TIENE";
                string FHVencimiento = "NO TIENE";

                string VchDcmRecargo = "";
                string VchDcmRecargoPeriodo = "";

                if (!string.IsNullOrEmpty(item["DtFHLimite"].ToString()))
                {
                    FHLimite = DateTime.Parse(item["DtFHLimite"].ToString()).ToString("dd/MM/yyyy");
                }
                if (!string.IsNullOrEmpty(item["DtFHVencimiento"].ToString()))
                {
                    FHVencimiento = DateTime.Parse(item["DtFHVencimiento"].ToString()).ToString("dd/MM/yyyy");
                }
                if (bool.Parse(item["BitRecargo"].ToString()))
                {
                    if (item["VchTipoRecargo"].ToString() == "CANTIDAD")
                    {
                        VchDcmRecargo = "$" + decimal.Parse(item["DcmRecargo"].ToString());
                    }
                    else if (item["VchTipoRecargo"].ToString() == "PORCENTAJE")
                    {
                        VchDcmRecargo = decimal.Parse(item["DcmRecargo"].ToString()) + "%";
                    }
                }
                else
                {
                    VchDcmRecargo = "NO TIENE";
                }

                if (bool.Parse(item["BitRecargoPeriodo"].ToString()))
                {
                    if (item["VchTipoRecargoPeriodo"].ToString() == "CANTIDAD")
                    {
                        VchDcmRecargoPeriodo = "$" + decimal.Parse(item["DcmRecargoPeriodo"].ToString());
                    }
                    else if (item["VchTipoRecargoPeriodo"].ToString() == "PORCENTAJE")
                    {
                        VchDcmRecargoPeriodo = decimal.Parse(item["DcmRecargoPeriodo"].ToString()) + "%";
                    }
                }
                else
                {
                    VchDcmRecargoPeriodo = "NO TIENE";
                }

                lsColegiaturasGridViewModel.Add(new ColegiaturasGridViewModel()
                {
                    UidColegiatura = Guid.Parse(item["UidColegiatura"].ToString()),
                    VchIdentificador = item["VchIdentificador"].ToString(),
                    DcmImporte = decimal.Parse(item["DcmImporte"].ToString()),
                    IntCantPagos = int.Parse(item["IntCantPagos"].ToString()),
                    UidPeriodicidad = Guid.Parse(item["UidPeriodicidad"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    DtFHInicio = DateTime.Parse(item["DtFHInicio"].ToString()),
                    BitFHLimite = bool.Parse(item["BitFHLimite"].ToString()),
                    VchFHLimite = FHLimite,
                    BitFHVencimiento = bool.Parse(item["BitFHVencimiento"].ToString()),
                    VchFHVencimiento = FHVencimiento,
                    BitRecargo = bool.Parse(item["BitRecargo"].ToString()),
                    VchTipoRecargo = item["VchTipoRecargo"].ToString(),
                    DcmRecargo = decimal.Parse(item["DcmRecargo"].ToString()),
                    VchDcmRecargo = VchDcmRecargo,
                    BitRecargoPeriodo = bool.Parse(item["BitRecargoPeriodo"].ToString()),
                    VchTipoRecargoPeriodo = item["VchTipoRecargoPeriodo"].ToString(),
                    DcmRecargoPeriodo = decimal.Parse(item["DcmRecargoPeriodo"].ToString()),
                    VchDcmRecargoPeriodo = VchDcmRecargoPeriodo,
                    UidEstatus = Guid.Parse(item["UidEstatus"].ToString()),
                    VchEstatus = item["Estatus"].ToString(),
                    VchIconoEstatus = item["VchIcono"].ToString(),
                    blEditar = editar
                });
            }

            return lsColegiaturasGridViewModel;
        }
        public bool RegistrarColegiatura(ColegiaturasGridViewModel colegiaturas)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ColegiaturasRegistrar";

                comando.Parameters.Add("@UidColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidColegiatura"].Value = colegiaturas.UidColegiatura;

                comando.Parameters.Add("@VchIdentificador", SqlDbType.VarChar);
                comando.Parameters["@VchIdentificador"].Value = colegiaturas.VchIdentificador;

                comando.Parameters.Add("@DcmImporte", SqlDbType.Decimal);
                comando.Parameters["@DcmImporte"].Value = colegiaturas.DcmImporte;

                comando.Parameters.Add("@IntCantPagos", SqlDbType.Int);
                comando.Parameters["@IntCantPagos"].Value = colegiaturas.IntCantPagos;

                comando.Parameters.Add("@UidPeriodicidad", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPeriodicidad"].Value = colegiaturas.UidPeriodicidad;

                comando.Parameters.Add("@DtFHInicio", SqlDbType.DateTime);
                comando.Parameters["@DtFHInicio"].Value = colegiaturas.DtFHInicio;

                comando.Parameters.Add("@BitFHLimite", SqlDbType.Bit);
                comando.Parameters["@BitFHLimite"].Value = colegiaturas.BitFHLimite;

                if (colegiaturas.DtFHLimite.ToString() != "01/01/0001 12:00:00 p. m.")
                {
                    comando.Parameters.Add("@DtFHLimite", SqlDbType.DateTime);
                    comando.Parameters["@DtFHLimite"].Value = colegiaturas.DtFHLimite;
                }

                comando.Parameters.Add("@BitFHVencimiento", SqlDbType.Bit);
                comando.Parameters["@BitFHVencimiento"].Value = colegiaturas.BitFHVencimiento;

                if (colegiaturas.DtFHVencimiento.ToString() != "01/01/0001 12:00:00 p. m.")
                {
                    comando.Parameters.Add("@DtFHVencimiento", SqlDbType.DateTime);
                    comando.Parameters["@DtFHVencimiento"].Value = colegiaturas.DtFHVencimiento;
                }

                comando.Parameters.Add("@BitRecargo", SqlDbType.Bit);
                comando.Parameters["@BitRecargo"].Value = colegiaturas.BitRecargo;

                comando.Parameters.Add("@VchTipoRecargo", SqlDbType.VarChar);
                comando.Parameters["@VchTipoRecargo"].Value = colegiaturas.VchTipoRecargo;

                comando.Parameters.Add("@DcmRecargo", SqlDbType.Decimal);
                comando.Parameters["@DcmRecargo"].Value = colegiaturas.DcmRecargo;

                comando.Parameters.Add("@BitRecargoPeriodo", SqlDbType.Bit);
                comando.Parameters["@BitRecargoPeriodo"].Value = colegiaturas.BitRecargoPeriodo;

                comando.Parameters.Add("@VchTipoRecargoPeriodo", SqlDbType.VarChar);
                comando.Parameters["@VchTipoRecargoPeriodo"].Value = colegiaturas.VchTipoRecargoPeriodo;

                comando.Parameters.Add("@DcmRecargoPeriodo", SqlDbType.Decimal);
                comando.Parameters["@DcmRecargoPeriodo"].Value = colegiaturas.DcmRecargoPeriodo;

                comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCliente"].Value = colegiaturas.UidCliente;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        public bool ActualizarColegiatura(ColegiaturasGridViewModel colegiaturas)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ColegiaturasActualizar";

                comando.Parameters.Add("@UidColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidColegiatura"].Value = colegiaturas.UidColegiatura;

                comando.Parameters.Add("@VchIdentificador", SqlDbType.VarChar);
                comando.Parameters["@VchIdentificador"].Value = colegiaturas.VchIdentificador;

                comando.Parameters.Add("@DcmImporte", SqlDbType.Decimal);
                comando.Parameters["@DcmImporte"].Value = colegiaturas.DcmImporte;

                comando.Parameters.Add("@IntCantPagos", SqlDbType.Int);
                comando.Parameters["@IntCantPagos"].Value = colegiaturas.IntCantPagos;

                comando.Parameters.Add("@UidPeriodicidad", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPeriodicidad"].Value = colegiaturas.UidPeriodicidad;

                comando.Parameters.Add("@DtFHInicio", SqlDbType.DateTime);
                comando.Parameters["@DtFHInicio"].Value = colegiaturas.DtFHInicio;

                comando.Parameters.Add("@BitFHLimite", SqlDbType.Bit);
                comando.Parameters["@BitFHLimite"].Value = colegiaturas.BitFHLimite;

                if (colegiaturas.DtFHLimite.ToString() != "01/01/0001 12:00:00 p. m.")
                {
                    comando.Parameters.Add("@DtFHLimite", SqlDbType.DateTime);
                    comando.Parameters["@DtFHLimite"].Value = colegiaturas.DtFHLimite;
                }

                comando.Parameters.Add("@BitFHVencimiento", SqlDbType.Bit);
                comando.Parameters["@BitFHVencimiento"].Value = colegiaturas.BitFHVencimiento;

                if (colegiaturas.DtFHVencimiento.ToString() != "01/01/0001 12:00:00 p. m.")
                {
                    comando.Parameters.Add("@DtFHVencimiento", SqlDbType.DateTime);
                    comando.Parameters["@DtFHVencimiento"].Value = colegiaturas.DtFHVencimiento;
                }

                comando.Parameters.Add("@BitRecargo", SqlDbType.Bit);
                comando.Parameters["@BitRecargo"].Value = colegiaturas.BitRecargo;

                comando.Parameters.Add("@VchTipoRecargo", SqlDbType.VarChar);
                comando.Parameters["@VchTipoRecargo"].Value = colegiaturas.VchTipoRecargo;

                comando.Parameters.Add("@DcmRecargo", SqlDbType.Decimal);
                comando.Parameters["@DcmRecargo"].Value = colegiaturas.DcmRecargo;

                comando.Parameters.Add("@BitRecargoPeriodo", SqlDbType.Bit);
                comando.Parameters["@BitRecargoPeriodo"].Value = colegiaturas.BitRecargoPeriodo;

                comando.Parameters.Add("@VchTipoRecargoPeriodo", SqlDbType.VarChar);
                comando.Parameters["@VchTipoRecargoPeriodo"].Value = colegiaturas.VchTipoRecargoPeriodo;

                comando.Parameters.Add("@DcmRecargoPeriodo", SqlDbType.Decimal);
                comando.Parameters["@DcmRecargoPeriodo"].Value = colegiaturas.DcmRecargoPeriodo;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        public bool ActualizarEstatusColegiatura(Guid UidColegiatura, Guid UidEstatus)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ColegiaturasEstatusActualizar";

                comando.Parameters.Add("@UidColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidColegiatura"].Value = UidColegiatura;

                comando.Parameters.Add("@UidEstatus", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEstatus"].Value = UidEstatus;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        public List<ColegiaturasGridViewModel> BuscarColegiatura(string Identificador, decimal ImporteMayor, decimal ImporteMenor, string CantPagos, Guid UidPeriodicidad, string FHInicioDesde, string FHInicioHasta, string FechaLimite, string FechaVencimineto, string RecargoLimite, string RecargoPeriodo, Guid UidEstatus, Guid UidCliente)
        {
            List<ColegiaturasGridViewModel> lsColegiaturasGridViewModel = new List<ColegiaturasGridViewModel>();

            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "sp_ColegiaturasBuscar";
            try
            {
                if (Identificador != string.Empty)
                {
                    comando.Parameters.Add("@Identificador", SqlDbType.VarChar);
                    comando.Parameters["@Identificador"].Value = Identificador;
                }
                if (ImporteMayor != 0)
                {
                    comando.Parameters.Add("@DcmImporteMayor", SqlDbType.Decimal);
                    comando.Parameters["@DcmImporteMayor"].Value = ImporteMayor;
                }
                if (ImporteMenor != 0)
                {
                    comando.Parameters.Add("@DcmImporteMenor", SqlDbType.Decimal);
                    comando.Parameters["@DcmImporteMenor"].Value = ImporteMenor;
                }
                if (CantPagos != string.Empty)
                {
                    comando.Parameters.Add("@CantPagos", SqlDbType.Int);
                    comando.Parameters["@CantPagos"].Value = int.Parse(CantPagos);
                }
                if (UidPeriodicidad != Guid.Empty)
                {
                    comando.Parameters.Add("@UidPeriodicidad", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidPeriodicidad"].Value = UidPeriodicidad;
                }

                if (FHInicioDesde != string.Empty)
                {
                    comando.Parameters.Add("@FHInicioDesde", SqlDbType.DateTime);
                    comando.Parameters["@FHInicioDesde"].Value = FHInicioDesde;
                }
                if (FHInicioHasta != string.Empty)
                {
                    comando.Parameters.Add("@FHInicioHasta", SqlDbType.Date);
                    comando.Parameters["@FHInicioHasta"].Value = FHInicioHasta;
                }
                if (FechaLimite != "AMBOS")
                {
                    bool fecha = false;
                    if (FechaLimite == "SI")
                    {
                        fecha = true;
                    }
                    comando.Parameters.Add("@FechaLimite", SqlDbType.Bit);
                    comando.Parameters["@FechaLimite"].Value = fecha;
                }
                if (FechaVencimineto != "AMBOS")
                {
                    bool fecha = false;
                    if (FechaVencimineto == "SI")
                    {
                        fecha = true;
                    }
                    comando.Parameters.Add("@FechaVencimineto", SqlDbType.Bit);
                    comando.Parameters["@FechaVencimineto"].Value = fecha;
                }
                if (RecargoLimite != "AMBOS")
                {
                    bool recargo = false;
                    if (RecargoLimite == "SI")
                    {
                        recargo = true;
                    }
                    comando.Parameters.Add("@RecargoLimite", SqlDbType.Bit);
                    comando.Parameters["@RecargoLimite"].Value = recargo;
                }
                if (RecargoPeriodo != "AMBOS")
                {
                    bool recargo = false;
                    if (RecargoPeriodo == "SI")
                    {
                        recargo = true;
                    }
                    comando.Parameters.Add("@RecargoPeriodo", SqlDbType.Bit);
                    comando.Parameters["@RecargoPeriodo"].Value = recargo;
                }
                if (UidEstatus != Guid.Empty)
                {
                    comando.Parameters.Add("@UidEstatus", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidEstatus"].Value = UidEstatus;
                }

                comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCliente"].Value = UidCliente;

                foreach (DataRow item in this.Busquedas(comando).Rows)
                {
                    bool editar = true;

                    #region ConsultarFechasPagos
                    SqlCommand queryFP = new SqlCommand();
                    queryFP.CommandType = CommandType.Text;

                    queryFP.CommandText = "select * from FechasColegiaturas where UidColegiatura = '" + item["UidColegiatura"].ToString() + "'";

                    DataTable dtFP = this.Busquedas(queryFP);

                    foreach (DataRow itemFP in dtFP.Rows)
                    {
                        #region Validar si hay un pago de la fecha
                        SqlCommand queryFCP = new SqlCommand();
                        queryFCP.CommandType = CommandType.Text;

                        queryFCP.CommandText = "select * from LigasUrls lu, PagosTarjeta pt where lu.IdReferencia = pt.IdReferencia and pt.VchEstatus = 'approved' and lu.UidFechaColegiatura = '" + itemFP["UidFechaColegiatura"].ToString() + "'";

                        DataTable dtFCP = this.Busquedas(queryFCP);

                        foreach (DataRow itemFCP in dtFCP.Rows)
                        {
                            editar = false;
                        }
                        #endregion
                    }
                    #endregion

                    string FHLimite = "NO TIENE";
                    string FHVencimiento = "NO TIENE";

                    string VchDcmRecargo = "";
                    string VchDcmRecargoPeriodo = "";

                    if (!string.IsNullOrEmpty(item["DtFHLimite"].ToString()))
                    {
                        FHLimite = DateTime.Parse(item["DtFHLimite"].ToString()).ToString("dd/MM/yyyy");
                    }
                    if (!string.IsNullOrEmpty(item["DtFHVencimiento"].ToString()))
                    {
                        FHVencimiento = DateTime.Parse(item["DtFHVencimiento"].ToString()).ToString("dd/MM/yyyy");
                    }
                    if (bool.Parse(item["BitRecargo"].ToString()))
                    {
                        if (item["VchTipoRecargo"].ToString() == "CANTIDAD")
                        {
                            VchDcmRecargo = "$" + decimal.Parse(item["DcmRecargo"].ToString());
                        }
                        else if (item["VchTipoRecargo"].ToString() == "PORCENTAJE")
                        {
                            VchDcmRecargo = decimal.Parse(item["DcmRecargo"].ToString()) + "%";
                        }
                    }
                    else
                    {
                        VchDcmRecargo = "NO TIENE";
                    }

                    if (bool.Parse(item["BitRecargoPeriodo"].ToString()))
                    {
                        if (item["VchTipoRecargoPeriodo"].ToString() == "CANTIDAD")
                        {
                            VchDcmRecargoPeriodo = "$" + decimal.Parse(item["DcmRecargoPeriodo"].ToString());
                        }
                        else if (item["VchTipoRecargoPeriodo"].ToString() == "PORCENTAJE")
                        {
                            VchDcmRecargoPeriodo = decimal.Parse(item["DcmRecargoPeriodo"].ToString()) + "%";
                        }
                    }
                    else
                    {
                        VchDcmRecargoPeriodo = "NO TIENE";
                    }

                    lsColegiaturasGridViewModel.Add(new ColegiaturasGridViewModel()
                    {
                        UidColegiatura = Guid.Parse(item["UidColegiatura"].ToString()),
                        VchIdentificador = item["VchIdentificador"].ToString(),
                        DcmImporte = decimal.Parse(item["DcmImporte"].ToString()),
                        IntCantPagos = int.Parse(item["IntCantPagos"].ToString()),
                        UidPeriodicidad = Guid.Parse(item["UidPeriodicidad"].ToString()),
                        VchDescripcion = item["VchDescripcion"].ToString(),
                        DtFHInicio = DateTime.Parse(item["DtFHInicio"].ToString()),
                        BitFHLimite = bool.Parse(item["BitFHLimite"].ToString()),
                        VchFHLimite = FHLimite,
                        BitFHVencimiento = bool.Parse(item["BitFHVencimiento"].ToString()),
                        VchFHVencimiento = FHVencimiento,
                        BitRecargo = bool.Parse(item["BitRecargo"].ToString()),
                        VchTipoRecargo = item["VchTipoRecargo"].ToString(),
                        DcmRecargo = decimal.Parse(item["DcmRecargo"].ToString()),
                        VchDcmRecargo = VchDcmRecargo,
                        BitRecargoPeriodo = bool.Parse(item["BitRecargoPeriodo"].ToString()),
                        VchTipoRecargoPeriodo = item["VchTipoRecargoPeriodo"].ToString(),
                        DcmRecargoPeriodo = decimal.Parse(item["DcmRecargoPeriodo"].ToString()),
                        VchDcmRecargoPeriodo = VchDcmRecargoPeriodo,
                        UidEstatus = Guid.Parse(item["UidEstatus"].ToString()),
                        VchEstatus = item["Estatus"].ToString(),
                        VchIconoEstatus = item["VchIcono"].ToString(),
                        blEditar = editar
                    });
                }

                return lsColegiaturasGridViewModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool RegistrarColegiaturaFechas(Guid UidColegiatura, int IntNum, DateTime DtFHInicio, DateTime DtFHLimite, DateTime DtFHVencimiento, DateTime DtFHFinPeriodo)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ColegiaturasFechasRegistrar";

                comando.Parameters.Add("@UidColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidColegiatura"].Value = UidColegiatura;

                comando.Parameters.Add("@IntNum", SqlDbType.Int);
                comando.Parameters["@IntNum"].Value = IntNum;

                comando.Parameters.Add("@DtFHInicio", SqlDbType.DateTime);
                comando.Parameters["@DtFHInicio"].Value = DtFHInicio;

                if (DtFHLimite.ToString() != "01/01/0001 12:00:00 p. m.")
                {
                    comando.Parameters.Add("@DtFHLimite", SqlDbType.DateTime);
                    comando.Parameters["@DtFHLimite"].Value = DtFHLimite;
                }
                if (DtFHVencimiento.ToString() != "01/01/0001 12:00:00 p. m.")
                {
                    comando.Parameters.Add("@DtFHVencimiento", SqlDbType.DateTime);
                    comando.Parameters["@DtFHVencimiento"].Value = DtFHVencimiento;
                }

                comando.Parameters.Add("@DtFHFinPeriodo", SqlDbType.DateTime);
                comando.Parameters["@DtFHFinPeriodo"].Value = DtFHFinPeriodo;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        public bool EliminarColegiaturaFechas(Guid UidColegiatura)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ColegiaturasFechasEliminar";

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

        public List<ColegiaturasFechasGridViewModel> ObtenerFechaColegiatura(Guid UidColegiatura)
        {
            List<ColegiaturasFechasGridViewModel> lsColegiaturasFechasGridViewModel = new List<ColegiaturasFechasGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from FechasColegiaturas where UidColegiatura = '" + UidColegiatura + "' order by DtFHInicio asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                string FHLimite = "NO TIENE";
                string FHVencimiento = "NO TIENE";

                if (!string.IsNullOrEmpty(item["DtFHLimite"].ToString()))
                {
                    FHLimite = DateTime.Parse(item["DtFHLimite"].ToString()).ToString("dd/MM/yyyy");
                }
                if (!string.IsNullOrEmpty(item["DtFHVencimiento"].ToString()))
                {
                    FHVencimiento = DateTime.Parse(item["DtFHVencimiento"].ToString()).ToString("dd/MM/yyyy");
                }

                lsColegiaturasFechasGridViewModel.Add(new ColegiaturasFechasGridViewModel()
                {
                    UidFechaColegiatura = new Guid(item["UidFechaColegiatura"].ToString()),
                    IntNumero = int.Parse(item["IntNum"].ToString()),
                    DtFHInicio = DateTime.Parse(item["DtFHInicio"].ToString()),
                    VchFHLimite = FHLimite,
                    VchFHVencimiento = FHVencimiento
                }); ;
            }

            return lsColegiaturasFechasGridViewModel;
        }

        public bool RegistrarPromocionesColegiatura(Guid UidColegiatura, Guid UidPromocion)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ColegiaturasPromocionesRegistrar";

                comando.Parameters.Add("@UidColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidColegiatura"].Value = UidColegiatura;

                comando.Parameters.Add("@UidPromocion", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPromocion"].Value = UidPromocion;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        public bool EliminarPromocionesColegiatura(Guid UidColegiatura)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ColegiaturasPromocionesEliminar";

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

        //public bool EliminarEvento(Guid UidCliente)
        //{
        //    bool Resultado = false;

        //    SqlCommand comando = new SqlCommand();
        //    try
        //    {
        //        comando.CommandType = System.Data.CommandType.StoredProcedure;
        //        comando.CommandText = "sp_EventosEliminar";

        //        comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
        //        comando.Parameters["@UidCliente"].Value = UidCliente;

        //        Resultado = this.ManipulacionDeDatos(comando);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return Resultado;
        //}
        //public void ObtenerDatosEvento(Guid UidEvento)
        //{
        //    SqlCommand query = new SqlCommand();
        //    query.CommandType = CommandType.Text;

        //    query.CommandText = "select ev.*, es.VchDescripcion as Estatus, es.VchIcono, cl.VchNombreComercial, cl.VchCorreoElectronico, tc.VchTelefono from Eventos ev, Estatus es, Clientes cl, TelefonosClientes tc where ev.UidEstatus = es.UidEstatus and cl.UidCliente = ev.UidPropietario and tc.UidCliente = cl.UidCliente and UidEvento = '" + UidEvento + "'";

        //    DataTable dt = this.Busquedas(query);

        //    foreach (DataRow item in dt.Rows)
        //    {
        //        string FechaFin = "ABIERTO";
        //        DateTime fechaFin = DateTime.Parse("1/1/0001 12:00:00");

        //        if (!string.IsNullOrEmpty(item["DtFHFin"].ToString()))
        //        {
        //            FechaFin = DateTime.Parse(item["DtFHFin"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
        //            fechaFin = DateTime.Parse(item["DtFHFin"].ToString());
        //        }

        //        eventosGridViewModel = new EventosGridViewModel()
        //        {
        //            UidEvento = Guid.Parse(item["UidEvento"].ToString()),
        //            VchNombreEvento = item["VchNombreEvento"].ToString(),
        //            VchDescripcion = item["VchDescripcion"].ToString(),
        //            DtRegistro = DateTime.Parse(item["DtRegistro"].ToString()),
        //            DtFHInicio = DateTime.Parse(item["DtFHInicio"].ToString()),
        //            VchFHFin = FechaFin,
        //            DtFHFin = fechaFin,
        //            BitTipoImporte = bool.Parse(item["BitTipoImporte"].ToString()),
        //            DcmImporte = decimal.Parse(item["DcmImporte"].ToString()),
        //            VchConcepto = item["VchConcepto"].ToString(),
        //            BitDatosUsuario = bool.Parse(item["BitDatosUsuario"].ToString()),
        //            VchUrlEvento = item["VchUrlEvento"].ToString(),
        //            UidEstatus = Guid.Parse(item["UidEstatus"].ToString()),
        //            VchEstatus = item["Estatus"].ToString(),
        //            VchIcono = item["VchIcono"].ToString(),
        //            UidPropietario = Guid.Parse(item["UidPropietario"].ToString()),
        //            VchNombreComercial = item["VchNombreComercial"].ToString(),
        //            VchTelefono = item["VchTelefono"].ToString(),
        //            VchCorreo = item["VchCorreoElectronico"].ToString(),
        //            BitFHFin = bool.Parse(item["BitFHFin"].ToString()),
        //            UidTipoEvento = Guid.Parse(item["UidTipoEvento"].ToString()),
        //        };
        //    }
        //}
        //public string ObtenerUrlLiga(string IdReferencia)
        //{
        //    string Resultado = string.Empty;

        //    SqlCommand query = new SqlCommand();
        //    query.CommandType = CommandType.Text;

        //    query.CommandText = "select VchUrl from LigasUrls where IdReferencia = '" + IdReferencia + "'";

        //    DataTable dt = this.Busquedas(query);

        //    foreach (DataRow item in dt.Rows)
        //    {
        //        Resultado = item["VchUrl"].ToString();
        //    }

        //    return Resultado;
        //}
        //public string ObtenerUidAdminCliente(Guid UidCliente)
        //{
        //    string Resultado = string.Empty;

        //    SqlCommand query = new SqlCommand();
        //    query.CommandType = CommandType.Text;

        //    query.CommandText = "select top 1 us.UidUsuario from Usuarios us, ClientesUsuarios cu, SegUsuarios su where su.UidUsuario = us.UidUsuario and us.UidUsuario = cu.UidUsuario and su.UidSegPerfil = 'D2C80D47-C14C-4677-A63D-C46BCB50FE17' and cu.UidCliente = '" + UidCliente + "'";

        //    DataTable dt = this.Busquedas(query);

        //    foreach (DataRow item in dt.Rows)
        //    {
        //        Resultado = item["UidUsuario"].ToString();
        //    }

        //    return Resultado;
        //}

        //public bool ValidarPagoEvento(string IdReferencia)
        //{
        //    bool result = false;

        //    SqlCommand query = new SqlCommand();
        //    query.CommandType = CommandType.Text;

        //    query.CommandText = "select * from PagosTarjeta where VchEstatus = 'approved' and IdReferencia = '" + IdReferencia + "'";

        //    DataTable dt = this.Busquedas(query);

        //    if (dt.Rows.Count >= 1)
        //    {
        //        result = true;
        //    }

        //    return result;
        //}
        //public bool EliminarLigaEvento(string IdReferencia)
        //{
        //    bool Resultado = false;

        //    SqlCommand comando = new SqlCommand();
        //    try
        //    {
        //        comando.CommandType = System.Data.CommandType.StoredProcedure;
        //        comando.CommandText = "sp_EventosLigaEliminar";

        //        comando.Parameters.Add("@IdReferencia", SqlDbType.VarChar);
        //        comando.Parameters["@IdReferencia"].Value = IdReferencia;

        //        Resultado = this.ManipulacionDeDatos(comando);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return Resultado;
        //}
        //public bool InsertCorreoLigaEvento(string Correo, string IdReferencia)
        //{
        //    bool Resultado = false;

        //    SqlCommand comando = new SqlCommand();
        //    try
        //    {
        //        comando.CommandType = System.Data.CommandType.StoredProcedure;
        //        comando.CommandText = "sp_EventosCorreoLigaInsertar";

        //        comando.Parameters.Add("@VchCorreo", SqlDbType.VarChar);
        //        comando.Parameters["@VchCorreo"].Value = Correo;

        //        comando.Parameters.Add("@IdReferencia", SqlDbType.VarChar);
        //        comando.Parameters["@IdReferencia"].Value = IdReferencia;

        //        Resultado = this.ManipulacionDeDatos(comando);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return Resultado;
        //}

        //public List<EventosGridViewModel> BuscarEventos(Guid UidPropietario, string VchNombreEvento, string DtFHInicioDesde, string DtFHInicioHasta, string DtFHFinDesde, string DtFHFinHasta, decimal DcmImporteMayor, decimal DcmImporteMenor, Guid UidEstatus)
        //{
        //    List<EventosGridViewModel> lsEventosGridViewModel = new List<EventosGridViewModel>();

        //    SqlCommand comando = new SqlCommand();
        //    comando.CommandType = CommandType.StoredProcedure;
        //    comando.CommandText = "sp_EventosBuscar";
        //    try
        //    {
        //        comando.Parameters.Add("@UidPropietario", SqlDbType.UniqueIdentifier);
        //        comando.Parameters["@UidPropietario"].Value = UidPropietario;

        //        if (VchNombreEvento != string.Empty)
        //        {
        //            comando.Parameters.Add("@VchNombreEvento", SqlDbType.VarChar);
        //            comando.Parameters["@VchNombreEvento"].Value = VchNombreEvento;
        //        }

        //        if (DtFHInicioDesde != string.Empty)
        //        {
        //            comando.Parameters.Add("@DtFHInicioDesde", SqlDbType.DateTime);
        //            comando.Parameters["@DtFHInicioDesde"].Value = DtFHInicioDesde;
        //        }
        //        if (DtFHInicioHasta != string.Empty)
        //        {
        //            comando.Parameters.Add("@DtFHInicioHasta", SqlDbType.Date);
        //            comando.Parameters["@DtFHInicioHasta"].Value = DtFHInicioHasta;
        //        }
        //        if (DtFHFinDesde != string.Empty)
        //        {
        //            comando.Parameters.Add("@DtFHFinDesde", SqlDbType.DateTime);
        //            comando.Parameters["@DtFHFinDesde"].Value = DtFHFinDesde;
        //        }
        //        if (DtFHFinHasta != string.Empty)
        //        {
        //            comando.Parameters.Add("@DtFHFinHasta", SqlDbType.Date);
        //            comando.Parameters["@DtFHFinHasta"].Value = DtFHFinHasta;
        //        }
        //        if (DcmImporteMayor != 0)
        //        {
        //            comando.Parameters.Add("@DcmImporteMayor", SqlDbType.Decimal);
        //            comando.Parameters["@DcmImporteMayor"].Value = DcmImporteMayor;
        //        }
        //        if (DcmImporteMenor != 0)
        //        {
        //            comando.Parameters.Add("@DcmImporteMenor", SqlDbType.Decimal);
        //            comando.Parameters["@DcmImporteMenor"].Value = DcmImporteMenor;
        //        }

        //        if (UidEstatus != Guid.Empty)
        //        {
        //            comando.Parameters.Add("@UidEstatus", SqlDbType.UniqueIdentifier);
        //            comando.Parameters["@UidEstatus"].Value = UidEstatus;
        //        }

        //        foreach (DataRow item in this.Busquedas(comando).Rows)
        //        {
        //            string FechaFin = "ABIERTO";

        //            if (!string.IsNullOrEmpty(item["DtFHFin"].ToString()))
        //            {
        //                FechaFin = DateTime.Parse(item["DtFHFin"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
        //            }

        //            eventosGridViewModel = new EventosGridViewModel()
        //            {
        //                UidEvento = Guid.Parse(item["UidEvento"].ToString()),
        //                VchNombreEvento = item["VchNombreEvento"].ToString(),
        //                VchDescripcion = item["VchDescripcion"].ToString(),
        //                DtRegistro = DateTime.Parse(item["DtRegistro"].ToString()),
        //                DtFHInicio = DateTime.Parse(item["DtFHInicio"].ToString()),
        //                VchFHFin = FechaFin,
        //                BitTipoImporte = bool.Parse(item["BitTipoImporte"].ToString()),
        //                DcmImporte = decimal.Parse(item["DcmImporte"].ToString()),
        //                VchConcepto = item["VchConcepto"].ToString(),
        //                BitDatosUsuario = bool.Parse(item["BitDatosUsuario"].ToString()),
        //                VchUrlEvento = item["VchUrlEvento"].ToString(),
        //                UidEstatus = Guid.Parse(item["UidEstatus"].ToString()),
        //                VchEstatus = item["Estatus"].ToString(),
        //                VchIcono = item["VchIcono"].ToString(),
        //                BitFHFin = bool.Parse(item["BitFHFin"].ToString()),
        //                UidTipoEvento = Guid.Parse(item["UidTipoEvento"].ToString())
        //            };

        //            lsEventosGridViewModel.Add(eventosGridViewModel);
        //        }

        //        return lsEventosGridViewModel.OrderByDescending(x => x.DtRegistro).ToList();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public bool RegistrarUsuariosEvento(Guid UidEvento, Guid UidUsuario)
        //{
        //    bool Resultado = false;

        //    SqlCommand comando = new SqlCommand();
        //    try
        //    {
        //        comando.CommandType = System.Data.CommandType.StoredProcedure;
        //        comando.CommandText = "sp_EventosUsuariosRegistrar";

        //        comando.Parameters.Add("@UidEvento", SqlDbType.UniqueIdentifier);
        //        comando.Parameters["@UidEvento"].Value = UidEvento;

        //        comando.Parameters.Add("@UidUsuario", SqlDbType.UniqueIdentifier);
        //        comando.Parameters["@UidUsuario"].Value = UidUsuario;

        //        Resultado = this.ManipulacionDeDatos(comando);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return Resultado;
        //}
        //public bool EliminarUsuariosEvento(Guid UidEvento)
        //{
        //    bool Resultado = false;

        //    SqlCommand comando = new SqlCommand();
        //    try
        //    {
        //        comando.CommandType = System.Data.CommandType.StoredProcedure;
        //        comando.CommandText = "sp_EventosUsuariosEliminar";

        //        comando.Parameters.Add("@UidEvento", SqlDbType.UniqueIdentifier);
        //        comando.Parameters["@UidEvento"].Value = UidEvento;

        //        Resultado = this.ManipulacionDeDatos(comando);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return Resultado;
        //}
        //#endregion

        //#region Metodos Usuarios final
        //public List<EventosUsuarioFinalGridViewModel> CargarEventosUsuariosFinal(Guid UidUsuario)
        //{
        //    List<EventosUsuarioFinalGridViewModel> lsEventosUsuarioFinalGridViewModel = new List<EventosUsuarioFinalGridViewModel>();

        //    SqlCommand query = new SqlCommand();
        //    query.CommandType = CommandType.Text;

        //    query.CommandText = "select ev.*, es.VchDescripcion as Estatus, es.VchIcono from Eventos ev, EventosUsuarios eu, Usuarios us, Estatus es where es.UidEstatus = ev.UidEstatus and eu.UidEvento = ev.UidEvento and eu.UidUsuario = us.UidUsuario and us.UidUsuario = '" + UidUsuario + "'";

        //    DataTable dt = this.Busquedas(query);

        //    foreach (DataRow item in dt.Rows)
        //    {
        //        string FechaFin = "ABIERTO";

        //        if (!string.IsNullOrEmpty(item["DtFHFin"].ToString()))
        //        {
        //            FechaFin = DateTime.Parse(item["DtFHFin"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
        //        }

        //        lsEventosUsuarioFinalGridViewModel.Add(new EventosUsuarioFinalGridViewModel()
        //        {
        //            UidEvento = Guid.Parse(item["UidEvento"].ToString()),
        //            VchNombreEvento = item["VchNombreEvento"].ToString(),
        //            VchDescripcion = item["VchDescripcion"].ToString(),
        //            DtRegistro = DateTime.Parse(item["DtRegistro"].ToString()),
        //            DtFHInicio = DateTime.Parse(item["DtFHInicio"].ToString()),
        //            VchFHFin = FechaFin,
        //            BitTipoImporte = bool.Parse(item["BitTipoImporte"].ToString()),
        //            DcmImporte = decimal.Parse(item["DcmImporte"].ToString()),
        //            VchConcepto = item["VchConcepto"].ToString(),
        //            BitDatosUsuario = bool.Parse(item["BitDatosUsuario"].ToString()),
        //            VchUrlEvento = item["VchUrlEvento"].ToString(),
        //            UidEstatus = Guid.Parse(item["UidEstatus"].ToString()),
        //            VchEstatus = item["Estatus"].ToString(),
        //            VchIcono = item["VchIcono"].ToString(),
        //            BitFHFin = bool.Parse(item["BitFHFin"].ToString()),
        //            UidTipoEvento = Guid.Parse(item["UidTipoEvento"].ToString())
        //        });
        //    }

        //    return lsEventosUsuarioFinalGridViewModel.OrderByDescending(x => x.DtRegistro).ToList();
        //}
        //public List<EventosUsuarioFinalGridViewModel> BuscarEventosUsuarioFinal(Guid UidUsuario, string VchNombreEvento, string DtFHInicioDesde, string DtFHInicioHasta, string DtFHFinDesde, string DtFHFinHasta, decimal DcmImporteMayor, decimal DcmImporteMenor, Guid UidEstatus)
        //{
        //    List<EventosUsuarioFinalGridViewModel> lsEventosUsuarioFinalGridViewModel = new List<EventosUsuarioFinalGridViewModel>();

        //    SqlCommand comando = new SqlCommand();
        //    comando.CommandType = CommandType.StoredProcedure;
        //    comando.CommandText = "sp_EventosBuscarUsuarioFinal";
        //    try
        //    {
        //        comando.Parameters.Add("@UidUsuario", SqlDbType.UniqueIdentifier);
        //        comando.Parameters["@UidUsuario"].Value = UidUsuario;

        //        if (VchNombreEvento != string.Empty)
        //        {
        //            comando.Parameters.Add("@VchNombreEvento", SqlDbType.VarChar);
        //            comando.Parameters["@VchNombreEvento"].Value = VchNombreEvento;
        //        }

        //        if (DtFHInicioDesde != string.Empty)
        //        {
        //            comando.Parameters.Add("@DtFHInicioDesde", SqlDbType.DateTime);
        //            comando.Parameters["@DtFHInicioDesde"].Value = DtFHInicioDesde;
        //        }
        //        if (DtFHInicioHasta != string.Empty)
        //        {
        //            comando.Parameters.Add("@DtFHInicioHasta", SqlDbType.Date);
        //            comando.Parameters["@DtFHInicioHasta"].Value = DtFHInicioHasta;
        //        }
        //        if (DtFHFinDesde != string.Empty)
        //        {
        //            comando.Parameters.Add("@DtFHFinDesde", SqlDbType.DateTime);
        //            comando.Parameters["@DtFHFinDesde"].Value = DtFHFinDesde;
        //        }
        //        if (DtFHFinHasta != string.Empty)
        //        {
        //            comando.Parameters.Add("@DtFHFinHasta", SqlDbType.Date);
        //            comando.Parameters["@DtFHFinHasta"].Value = DtFHFinHasta;
        //        }
        //        if (DcmImporteMayor != 0)
        //        {
        //            comando.Parameters.Add("@DcmImporteMayor", SqlDbType.Decimal);
        //            comando.Parameters["@DcmImporteMayor"].Value = DcmImporteMayor;
        //        }
        //        if (DcmImporteMenor != 0)
        //        {
        //            comando.Parameters.Add("@DcmImporteMenor", SqlDbType.Decimal);
        //            comando.Parameters["@DcmImporteMenor"].Value = DcmImporteMenor;
        //        }

        //        if (UidEstatus != Guid.Empty)
        //        {
        //            comando.Parameters.Add("@UidEstatus", SqlDbType.UniqueIdentifier);
        //            comando.Parameters["@UidEstatus"].Value = UidEstatus;
        //        }

        //        foreach (DataRow item in this.Busquedas(comando).Rows)
        //        {
        //            string FechaFin = "ABIERTO";

        //            if (!string.IsNullOrEmpty(item["DtFHFin"].ToString()))
        //            {
        //                FechaFin = DateTime.Parse(item["DtFHFin"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
        //            }


        //            eventosUsuarioFinalGridViewModel = new EventosUsuarioFinalGridViewModel()
        //            {
        //                UidEvento = Guid.Parse(item["UidEvento"].ToString()),
        //                VchNombreEvento = item["VchNombreEvento"].ToString(),
        //                VchDescripcion = item["VchDescripcion"].ToString(),
        //                DtRegistro = DateTime.Parse(item["DtRegistro"].ToString()),
        //                DtFHInicio = DateTime.Parse(item["DtFHInicio"].ToString()),
        //                VchFHFin = FechaFin,
        //                BitTipoImporte = bool.Parse(item["BitTipoImporte"].ToString()),
        //                DcmImporte = decimal.Parse(item["DcmImporte"].ToString()),
        //                VchConcepto = item["VchConcepto"].ToString(),
        //                BitDatosUsuario = bool.Parse(item["BitDatosUsuario"].ToString()),
        //                VchUrlEvento = item["VchUrlEvento"].ToString(),
        //                UidEstatus = Guid.Parse(item["UidEstatus"].ToString()),
        //                VchEstatus = item["Estatus"].ToString(),
        //                VchIcono = item["VchIcono"].ToString(),
        //                BitFHFin = bool.Parse(item["BitFHFin"].ToString()),
        //                UidTipoEvento = Guid.Parse(item["UidTipoEvento"].ToString())
        //            };

        //            lsEventosUsuarioFinalGridViewModel.Add(eventosUsuarioFinalGridViewModel);
        //        }

        //        return lsEventosUsuarioFinalGridViewModel.OrderByDescending(x => x.DtRegistro).ToList();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public void ObtenerDatosEventoUsuariosFinal(Guid UidEvento)
        //{
        //    SqlCommand query = new SqlCommand();
        //    query.CommandType = CommandType.Text;

        //    query.CommandText = "select ev.*, es.VchDescripcion as Estatus, es.VchIcono, cl.VchNombreComercial, cl.VchCorreoElectronico, tc.VchTelefono from Eventos ev, Estatus es, Clientes cl, TelefonosClientes tc where ev.UidEstatus = es.UidEstatus and cl.UidCliente = ev.UidPropietario and tc.UidCliente = cl.UidCliente and UidEvento = '" + UidEvento + "'";

        //    DataTable dt = this.Busquedas(query);

        //    foreach (DataRow item in dt.Rows)
        //    {
        //        string FechaFin = "ABIERTO";
        //        DateTime fechaFin = DateTime.Parse("1/1/0001 12:00:00");

        //        if (!string.IsNullOrEmpty(item["DtFHFin"].ToString()))
        //        {
        //            FechaFin = DateTime.Parse(item["DtFHFin"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
        //            fechaFin = DateTime.Parse(item["DtFHFin"].ToString());
        //        }

        //        eventosGridViewModel = new EventosGridViewModel()
        //        {
        //            UidEvento = Guid.Parse(item["UidEvento"].ToString()),
        //            VchNombreEvento = item["VchNombreEvento"].ToString(),
        //            VchDescripcion = item["VchDescripcion"].ToString(),
        //            DtRegistro = DateTime.Parse(item["DtRegistro"].ToString()),
        //            DtFHInicio = DateTime.Parse(item["DtFHInicio"].ToString()),
        //            VchFHFin = FechaFin,
        //            DtFHFin = fechaFin,
        //            BitTipoImporte = bool.Parse(item["BitTipoImporte"].ToString()),
        //            DcmImporte = decimal.Parse(item["DcmImporte"].ToString()),
        //            VchConcepto = item["VchConcepto"].ToString(),
        //            BitDatosUsuario = bool.Parse(item["BitDatosUsuario"].ToString()),
        //            VchUrlEvento = item["VchUrlEvento"].ToString(),
        //            UidEstatus = Guid.Parse(item["UidEstatus"].ToString()),
        //            VchEstatus = item["Estatus"].ToString(),
        //            VchIcono = item["VchIcono"].ToString(),
        //            UidPropietario = Guid.Parse(item["UidPropietario"].ToString()),
        //            VchNombreComercial = item["VchNombreComercial"].ToString(),
        //            VchTelefono = item["VchTelefono"].ToString(),
        //            VchCorreo = item["VchCorreoElectronico"].ToString(),
        //            BitFHFin = bool.Parse(item["BitFHFin"].ToString()),
        //            UidTipoEvento = Guid.Parse(item["UidTipoEvento"].ToString()),
        //        };
        //    }
        //}
        #endregion

        #region Metodos Padres
        public List<PagosColegiaturasViewModel> CargarPagosColegiaturas(Guid UidCliente, Guid UidUsuario, DateTime FechaInicio)
        {
            List<PagosColegiaturasViewModel> lsPagosColegiaturasViewModel = new List<PagosColegiaturasViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            // ==>SIN ESTATUS<== query.CommandText = "select cl.VchNombreComercial, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno,  co.*, fc.UidFechaColegiatura, fc.IntNum, fc.DtFHInicio as fcInicio, fc.DtFHLimite as fcLimite, fc.DtFHVencimiento as fcVencimiento from clientes cl, Colegiaturas co, FechasColegiaturas fc, ColegiaturasAlumnos ca, Alumnos al, Usuarios us, UsuariosAlumnos ua where not exists (select * from LigasUrls lu, PagosTarjeta pt where pt.IdReferencia = lu.IdReferencia and pt.VchEstatus = 'approved' and lu.UidFechaColegiatura = fc.UidFechaColegiatura) and cl.UidCliente = co.UidCliente and co.UidColegiatura = fc.UidColegiatura and ca.UidColegiatura = co.UidColegiatura and ca.UidAlumno = al.UidAlumno and ua.UidUsuario = us.UidUsuario and ua.UidAlumno = al.UidAlumno and cl.UidCliente = '" + UidCliente + "' and us.UidUsuario = '" + UidUsuario + "' order by al.VchMatricula";
            query.CommandText = "select cl.VchNombreComercial, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, al.BitBeca, al.VchTipoBeca, al.DcmBeca, co.*, fc.UidFechaColegiatura, fc.IntNum, fc.DtFHInicio as fcInicio, fc.DtFHLimite as fcLimite, fc.DtFHVencimiento as fcVencimiento, fc.DtFHFinPeriodo as fcFinPeriodo, efc.VchDescripcion as EstatusFechas, pe.VchDescripcion as Periodicidad from clientes cl, Colegiaturas co, FechasColegiaturas fc, ColegiaturasAlumnos ca, Alumnos al, Usuarios us, UsuariosAlumnos ua, EstatusFechasColegiaturas efc, Periodicidades pe where pe.UidPeriodicidad = co.UidPeriodicidad and not exists (select * from LigasUrls lu, PagosTarjeta pt where lu.UidUsuario = us.UidUsuario and pt.IdReferencia = lu.IdReferencia and pt.VchEstatus = 'approved' and lu.UidFechaColegiatura = fc.UidFechaColegiatura) and co.UidEstatus = '65E46BC9-1864-4145-AD1A-70F5B5F69739' and efc.UidEstatusFechaColegiatura = fc.UidEstatusFechaColegiatura and cl.UidCliente = co.UidCliente and co.UidColegiatura = fc.UidColegiatura and ca.UidColegiatura = co.UidColegiatura and ca.UidAlumno = al.UidAlumno and ua.UidUsuario = us.UidUsuario and ua.UidAlumno = al.UidAlumno and cl.UidCliente = '" + UidCliente + "' and us.UidUsuario = '" + UidUsuario + "' order by al.VchMatricula";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                string FHLimite = "NO TIENE";
                string FHVencimiento = "NO TIENE";
                bool FHVence = false;

                bool blpagar = false;

                string VchColor = "#007bff";

                if (!string.IsNullOrEmpty(item["EstatusFechas"].ToString()))
                {
                    switch (item["EstatusFechas"].ToString())
                    {
                        case "VIGENTE":
                            blpagar = true;
                            VchColor = "#4caf50 ";
                            break;
                        case "VENCIDO":
                            VchColor = "#f55145 ";
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(item["fcLimite"].ToString()))
                {
                    FHLimite = DateTime.Parse(item["fcLimite"].ToString()).ToString("dd/MM/yyyy");
                }
                if (!string.IsNullOrEmpty(item["fcVencimiento"].ToString()))
                {
                    FHVencimiento = DateTime.Parse(item["fcVencimiento"].ToString()).ToString("dd/MM/yyyy");
                    FHVence = true;
                }

                lsPagosColegiaturasViewModel.Add(new PagosColegiaturasViewModel()
                {
                    UidFechaColegiatura = Guid.Parse(item["UidFechaColegiatura"].ToString()),
                    VchIdentificador = item["VchIdentificador"].ToString(),
                    DcmImporte = decimal.Parse(item["DcmImporte"].ToString()),
                    UidColegiatura = Guid.Parse(item["UidColegiatura"].ToString()),
                    VchNum = int.Parse(item["IntNum"].ToString()) + " de " + int.Parse(item["IntCantPagos"].ToString()),
                    DtFHInicio = DateTime.Parse(item["fcInicio"].ToString()),
                    VchFHLimite = FHLimite,
                    VchFHVencimiento = FHVencimiento,
                    DtFHFinPeriodo = DateTime.Parse(item["fcFinPeriodo"].ToString()),
                    VchEstatusFechas = item["EstatusFechas"].ToString(),
                    VchColor = VchColor,
                    BitRecargo = bool.Parse(item["BitRecargo"].ToString()),
                    VchTipoRecargo = item["VchTipoRecargo"].ToString(),
                    DcmRecargo = decimal.Parse(item["DcmRecargo"].ToString()),
                    BitRecargoPeriodo = bool.Parse(item["BitRecargoPeriodo"].ToString()),
                    VchTipoRecargoPeriodo = item["VchTipoRecargoPeriodo"].ToString(),
                    DcmRecargoPeriodo = decimal.Parse(item["DcmRecargoPeriodo"].ToString()),
                    VchPeriodicidad = item["Periodicidad"].ToString(),
                    VchMatricula = item["VchMatricula"].ToString(),
                    VchNombres = item["VchNombres"].ToString(),
                    VchApePaterno = item["VchApePaterno"].ToString(),
                    VchApeMaterno = item["VchApeMaterno"].ToString(),
                    BitBeca = bool.Parse(item["BitBeca"].ToString()),
                    VchTipoBeca = item["VchTipoBeca"].ToString(),
                    DcmBeca = decimal.Parse(item["DcmBeca"].ToString()),

                    blPagar = blpagar
                });

                //if (FHVence)
                //{
                //    if (FechaInicio >= DateTime.Parse(item["fcInicio"].ToString()) && FechaInicio <= DateTime.Parse(item["fcVencimiento"].ToString()))
                //    {
                //        lsPagosColegiaturasViewModel.Add(new PagosColegiaturasViewModel()
                //        {
                //            UidFechaColegiatura = Guid.Parse(item["UidFechaColegiatura"].ToString()),
                //            VchIdentificador = item["VchIdentificador"].ToString(),
                //            DcmImporte = decimal.Parse(item["DcmImporte"].ToString()),
                //            UidColegiatura = Guid.Parse(item["UidColegiatura"].ToString()),
                //            VchNum = int.Parse(item["IntNum"].ToString()) + " de " + int.Parse(item["IntCantPagos"].ToString()),
                //            DtFHInicio = DateTime.Parse(item["fcInicio"].ToString()),
                //            VchFHLimite = FHLimite,
                //            VchFHVencimiento = FHVencimiento,
                //            VchEstatusFechas = item["EstatusFechas"].ToString(),
                //            VchColor = VchColor,
                //            BitRecargo = bool.Parse(item["BitRecargo"].ToString()),
                //            VchTipoRecargo = item["VchTipoRecargo"].ToString(),
                //            DcmRecargo = decimal.Parse(item["DcmRecargo"].ToString()),

                //            VchMatricula = item["VchMatricula"].ToString(),
                //            VchNombres = item["VchNombres"].ToString(),
                //            VchApePaterno = item["VchApePaterno"].ToString(),
                //            VchApeMaterno = item["VchApeMaterno"].ToString(),
                //            BitBeca = bool.Parse(item["BitBeca"].ToString()),
                //            VchTipoBeca = item["VchTipoBeca"].ToString(),
                //            DcmBeca = decimal.Parse(item["DcmBeca"].ToString()),

                //            blPagar = blpagar
                //        });
                //    }
                //}
                //else
                //{
                //    if (FechaInicio >= DateTime.Parse(item["fcInicio"].ToString()))
                //    {
                //        lsPagosColegiaturasViewModel.Add(new PagosColegiaturasViewModel()
                //        {
                //            UidFechaColegiatura = Guid.Parse(item["UidFechaColegiatura"].ToString()),
                //            VchIdentificador = item["VchIdentificador"].ToString(),
                //            DcmImporte = decimal.Parse(item["DcmImporte"].ToString()),
                //            UidColegiatura = Guid.Parse(item["UidColegiatura"].ToString()),
                //            VchNum = int.Parse(item["IntNum"].ToString()) + " de " + int.Parse(item["IntCantPagos"].ToString()),
                //            DtFHInicio = DateTime.Parse(item["fcInicio"].ToString()),
                //            VchFHLimite = FHLimite,
                //            VchFHVencimiento = FHVencimiento,
                //            VchEstatusFechas = item["EstatusFechas"].ToString(),
                //            VchColor = VchColor,
                //            BitRecargo = bool.Parse(item["BitRecargo"].ToString()),
                //            VchTipoRecargo = item["VchTipoRecargo"].ToString(),
                //            DcmRecargo = decimal.Parse(item["DcmRecargo"].ToString()),

                //            VchMatricula = item["VchMatricula"].ToString(),
                //            VchNombres = item["VchNombres"].ToString(),
                //            VchApePaterno = item["VchApePaterno"].ToString(),
                //            VchApeMaterno = item["VchApeMaterno"].ToString(),
                //            BitBeca = bool.Parse(item["BitBeca"].ToString()),
                //            VchTipoBeca = item["VchTipoBeca"].ToString(),
                //            DcmBeca = decimal.Parse(item["DcmBeca"].ToString()),

                //            blPagar = blpagar
                //        });
                //    }
                //}
            }

            return lsPagosColegiaturasViewModel.OrderBy(x => x.DtFHInicio).ToList();
        }
        public PagosColegiaturasViewModel ObtenerPagoColegiatura(Guid UidCliente, Guid UidUsuario, Guid UidFechaColegiatura, string VchMatricula)
        {
            pagosColegiaturasViewModel = new PagosColegiaturasViewModel();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select cl.VchNombreComercial, al.UidAlumno, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, al.BitBeca, al.VchTipoBeca, al.DcmBeca, co.*, fc.UidFechaColegiatura, fc.IntNum, fc.DtFHInicio as fcInicio, fc.DtFHLimite as fcLimite, fc.DtFHVencimiento as fcVencimiento, fc.DtFHFinPeriodo as fcFinPeriodo, efc.VchDescripcion as EstatusFechas, pe.VchDescripcion as Periodicidad from clientes cl, Colegiaturas co, FechasColegiaturas fc, ColegiaturasAlumnos ca, Alumnos al, Usuarios us, UsuariosAlumnos ua, EstatusFechasColegiaturas efc, Periodicidades pe where pe.UidPeriodicidad = co.UidPeriodicidad and not exists (select * from LigasUrls lu, PagosTarjeta pt where lu.UidUsuario = us.UidUsuario and pt.IdReferencia = lu.IdReferencia and pt.VchEstatus = 'approved' and lu.UidFechaColegiatura = fc.UidFechaColegiatura) and co.UidEstatus = '65E46BC9-1864-4145-AD1A-70F5B5F69739' and efc.UidEstatusFechaColegiatura = fc.UidEstatusFechaColegiatura and cl.UidCliente = co.UidCliente and co.UidColegiatura = fc.UidColegiatura and ca.UidColegiatura = co.UidColegiatura and ca.UidAlumno = al.UidAlumno and ua.UidUsuario = us.UidUsuario and ua.UidAlumno = al.UidAlumno and cl.UidCliente = '" + UidCliente + "' and us.UidUsuario = '" + UidUsuario + "' and fc.UidFechaColegiatura = '" + UidFechaColegiatura + "' and al.VchMatricula = '" + VchMatricula + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                string FHLimite = "NO TIENE";
                string FHVencimiento = "NO TIENE";
                bool FHVence = false;

                bool blpagar = false;

                string VchColor = "#007bff";

                if (!string.IsNullOrEmpty(item["EstatusFechas"].ToString()))
                {
                    switch (item["EstatusFechas"].ToString())
                    {
                        case "VIGENTE":
                            blpagar = true;
                            VchColor = "#4caf50";
                            break;
                        case "VENCIDO":
                            VchColor = "#f55145";
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(item["fcLimite"].ToString()))
                {
                    FHLimite = DateTime.Parse(item["fcLimite"].ToString()).ToString("dd/MM/yyyy");
                }
                if (!string.IsNullOrEmpty(item["fcVencimiento"].ToString()))
                {
                    FHVencimiento = DateTime.Parse(item["fcVencimiento"].ToString()).ToString("dd/MM/yyyy");
                    FHVence = true;
                }

                pagosColegiaturasViewModel.UidFechaColegiatura = Guid.Parse(item["UidFechaColegiatura"].ToString());
                pagosColegiaturasViewModel.VchIdentificador = item["VchIdentificador"].ToString();
                pagosColegiaturasViewModel.DcmImporte = decimal.Parse(item["DcmImporte"].ToString());
                pagosColegiaturasViewModel.UidColegiatura = Guid.Parse(item["UidColegiatura"].ToString());
                pagosColegiaturasViewModel.VchNum = int.Parse(item["IntNum"].ToString()) + " de " + int.Parse(item["IntCantPagos"].ToString());
                pagosColegiaturasViewModel.DtFHInicio = DateTime.Parse(item["fcInicio"].ToString());
                pagosColegiaturasViewModel.VchFHLimite = FHLimite;
                pagosColegiaturasViewModel.VchFHVencimiento = FHVencimiento;
                pagosColegiaturasViewModel.DtFHFinPeriodo = DateTime.Parse(item["fcFinPeriodo"].ToString());
                pagosColegiaturasViewModel.VchEstatusFechas = item["EstatusFechas"].ToString();
                pagosColegiaturasViewModel.VchColor = VchColor;
                pagosColegiaturasViewModel.BitRecargo = bool.Parse(item["BitRecargo"].ToString());
                pagosColegiaturasViewModel.VchTipoRecargo = item["VchTipoRecargo"].ToString();
                pagosColegiaturasViewModel.DcmRecargo = decimal.Parse(item["DcmRecargo"].ToString());
                pagosColegiaturasViewModel.BitRecargoPeriodo = bool.Parse(item["BitRecargoPeriodo"].ToString());
                pagosColegiaturasViewModel.VchTipoRecargoPeriodo = item["VchTipoRecargoPeriodo"].ToString();
                pagosColegiaturasViewModel.DcmRecargoPeriodo = decimal.Parse(item["DcmRecargoPeriodo"].ToString());
                pagosColegiaturasViewModel.VchPeriodicidad = item["Periodicidad"].ToString();
                pagosColegiaturasViewModel.UidAlumno = Guid.Parse(item["UidAlumno"].ToString());
                pagosColegiaturasViewModel.VchMatricula = item["VchMatricula"].ToString();
                pagosColegiaturasViewModel.VchNombres = item["VchNombres"].ToString();
                pagosColegiaturasViewModel.VchApePaterno = item["VchApePaterno"].ToString();
                pagosColegiaturasViewModel.VchApeMaterno = item["VchApeMaterno"].ToString();
                pagosColegiaturasViewModel.BitBeca = bool.Parse(item["BitBeca"].ToString());
                pagosColegiaturasViewModel.VchTipoBeca = item["VchTipoBeca"].ToString();
                pagosColegiaturasViewModel.DcmBeca = decimal.Parse(item["DcmBeca"].ToString());

                pagosColegiaturasViewModel.blPagar = blpagar;

            }
            return pagosColegiaturasViewModel;
        }

        public bool EliminarLigaColegiatura(string IdReferencia)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ColegiaturasLigaEliminar";

                comando.Parameters.Add("@IdReferencia", SqlDbType.VarChar);
                comando.Parameters["@IdReferencia"].Value = IdReferencia;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        #endregion


        #region Procesos Automaticos
        public bool ActualizarEstatusFechasPagos(DateTime Fecha)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ColegiaturasFechasActualizarAutomatico";

                comando.Parameters.Add("@Fecha", SqlDbType.Date);
                comando.Parameters["@Fecha"].Value = Fecha;

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
