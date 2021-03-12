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

            //No tiene cantPagos query.CommandText = "select co.*, pe.VchDescripcion, es.VchDescripcion as Estatus, es.VchIcono from Colegiaturas co, Periodicidades pe, Estatus es where es.UidEstatus = co.UidEstatus and co.UidPeriodicidad = pe.UidPeriodicidad and co.UidCliente = '" + UidCliente + "'";
            query.CommandText = "select co.*, pe.VchDescripcion, es.VchDescripcion as Estatus, es.VchIcono, (select COUNT(*) from Colegiaturas cole, FechasColegiaturas feco, FechasPagos fepa, PagosColegiaturas paco where cole.UidColegiatura = feco.UidColegiatura and feco.UidFechaColegiatura = fepa.UidFechaColegiatura and fepa.UidPagoColegiatura = paco.UidPagoColegiatura and paco.UidEstatusPagoColegiatura != '3B1517E9-6E32-43E8-9D9C-A11CD08F6F55' and cole.UidColegiatura = co.UidColegiatura) CantPagos from Colegiaturas co, Periodicidades pe, Estatus es where es.UidEstatus = co.UidEstatus and co.UidPeriodicidad = pe.UidPeriodicidad and co.UidCliente = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                bool editar = true;

                if (int.Parse(item["CantPagos"].ToString()) > 0)
                {
                    editar = false;
                }

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

        public List<ColegiaturasFechasGridViewModel> ObtenerFechasColegiaturasVicular(Guid UidColegiatura)
        {
            List<ColegiaturasFechasGridViewModel> lsColegiaturasFechasGridViewModel = new List<ColegiaturasFechasGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from FechasColegiaturas where UidColegiatura = '" + UidColegiatura + "' order by DtFHInicio asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsColegiaturasFechasGridViewModel.Add(new ColegiaturasFechasGridViewModel()
                {
                    UidFechaColegiatura = new Guid(item["UidFechaColegiatura"].ToString()),
                    IntNumero = int.Parse(item["IntNum"].ToString()),
                    DtFHInicio = DateTime.Parse(item["DtFHInicio"].ToString())
                }); ;
            }

            return lsColegiaturasFechasGridViewModel;
        }
        public bool RegistrarFechasColegiaturasAlumnos(Guid UidFechaColegiatura, Guid UidAlumno, decimal DcmImporteResta, Guid UidEstatusFechaColegiatura)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ColegiaturasFechasAlumnosRegistrar";

                comando.Parameters.Add("@UidFechaColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidFechaColegiatura"].Value = UidFechaColegiatura;

                comando.Parameters.Add("@UidAlumno", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidAlumno"].Value = UidAlumno;

                comando.Parameters.Add("@DcmImporteResta", SqlDbType.Decimal);
                comando.Parameters["@DcmImporteResta"].Value = DcmImporteResta;

                comando.Parameters.Add("@UidEstatusFechaColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEstatusFechaColegiatura"].Value = UidEstatusFechaColegiatura;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        public bool EliminarFechasColegiaturasAlumnos(Guid UidFechaColegiatura)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ColegiaturasFechasAlumnosEliminar";

                comando.Parameters.Add("@UidFechaColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidFechaColegiatura"].Value = UidFechaColegiatura;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
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
        public struct DateTimeSpan
        {
            private readonly int years;
            private readonly int months;
            private readonly int days;
            private readonly int hours;
            private readonly int minutes;
            private readonly int seconds;
            private readonly int milliseconds;

            public DateTimeSpan(int years, int months, int days, int hours, int minutes, int seconds, int milliseconds)
            {
                this.years = years;
                this.months = months;
                this.days = days;
                this.hours = hours;
                this.minutes = minutes;
                this.seconds = seconds;
                this.milliseconds = milliseconds;
            }

            public int Years { get { return years; } }
            public int Months { get { return months; } }
            public int Days { get { return days; } }
            public int Hours { get { return hours; } }
            public int Minutes { get { return minutes; } }
            public int Seconds { get { return seconds; } }
            public int Milliseconds { get { return milliseconds; } }

            enum Phase { Years, Months, Days, Done }

            public static DateTimeSpan CompareDates(DateTime date1, DateTime date2)
            {
                if (date2 < date1)
                {
                    var sub = date1;
                    date1 = date2;
                    date2 = sub;
                }

                DateTime current = date1;
                int years = 0;
                int months = 0;
                int days = 0;

                Phase phase = Phase.Years;
                DateTimeSpan span = new DateTimeSpan();

                while (phase != Phase.Done)
                {
                    switch (phase)
                    {
                        case Phase.Years:
                            if (current.AddYears(years + 1) > date2)
                            {
                                phase = Phase.Months;
                                //current = current.AddYears(years);
                            }
                            else
                            {
                                years++;
                            }
                            break;
                        case Phase.Months:
                            if (current.AddMonths(months + 1) > date2)
                            {
                                phase = Phase.Days;
                                //current = current.AddMonths(months);
                            }
                            else
                            {
                                months++;
                            }
                            break;
                        case Phase.Days:
                            if (current.AddDays(days + 1) > date2)
                            {
                                //current = current.AddDays(days);
                                var timespan = date2 - current;
                                span = new DateTimeSpan(years, months, days, timespan.Hours, timespan.Minutes, timespan.Seconds, timespan.Milliseconds);
                                phase = Phase.Done;
                            }
                            else
                            {
                                days++;
                            }
                            break;
                    }
                }

                return span;
            }
        }
        private int CantDias(DateTime FPago)
        {
            DateTime HoraDelServidor = DateTime.Now;
            DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

            var dateTime = DateTimeSpan.CompareDates(FPago, hoy);

            return dateTime.Days;
        }
        private decimal Calculo(decimal DcmImporte, bool BitRecargo, string VchTipoRecargo, decimal DcmRecargo, string VchFHLimite,
            bool BitRecargoPeriodo, string VchTipoRecargoPeriodo, decimal DcmRecargoPeriodo, string VchPeriodicidad,
            bool BitBeca, string VchTipoBeca, decimal DcmBeca, DateTime DtFHFinPeriodo, bool blDatoFecha = false, string dtDatoFecha = "")
        {
            DateTime hoy;

            if (blDatoFecha)
            {
                hoy = DateTime.Parse(dtDatoFecha);
            }
            else
            {
                DateTime HoraDelServidor = DateTime.Now;
                hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");
            }

            hoy = DateTime.Parse(hoy.ToString("dd/MM/yyyy"));

            decimal ImporteCole = DcmImporte;
            decimal recargoTotalLimite = 0;
            decimal recargoTotalPeriodo = 0;
            decimal recargoTotal = 0;

            if (BitRecargo)
            {
                if (VchFHLimite != "NO TIENE")
                {
                    if (hoy > DateTime.Parse(VchFHLimite) && DateTime.Parse(VchFHLimite) < DtFHFinPeriodo)
                    {
                        if (VchTipoRecargo == "CANTIDAD")
                        {
                            recargoTotalLimite = DcmRecargo;
                        }
                        else if (VchTipoRecargo == "PORCENTAJE")
                        {
                            recargoTotalLimite = DcmRecargo * ImporteCole / 100;
                        }
                    }
                }
            }

            if (BitRecargoPeriodo)
            {
                decimal recargo = decimal.Parse(DcmRecargoPeriodo.ToString("N2"));
                decimal recargoTemp = 0;

                if (VchTipoRecargoPeriodo == "CANTIDAD")
                {
                    if (VchPeriodicidad == "MENSUAL")
                    {
                        var dateTime = DateTimeSpan.CompareDates(DtFHFinPeriodo, hoy);
                        recargoTemp = recargo * dateTime.Months;
                    }
                    else if (VchPeriodicidad == "SEMANAL")
                    {
                        int canSem = 0;

                        var dateTime = DateTimeSpan.CompareDates(DtFHFinPeriodo, hoy);
                        canSem = dateTime.Days / 7;
                        recargoTemp = recargo * canSem;
                    }
                }
                else if (VchTipoRecargoPeriodo == "PORCENTAJE")
                {
                    recargo = recargo * ImporteCole / 100;

                    if (VchPeriodicidad == "MENSUAL")
                    {
                        var dateTime = DateTimeSpan.CompareDates(DtFHFinPeriodo, hoy);
                        recargoTemp = recargo * dateTime.Months;
                    }
                    else if (VchPeriodicidad == "SEMANAL")
                    {
                        int canSem = 0;

                        var dateTime = DateTimeSpan.CompareDates(DtFHFinPeriodo, hoy);
                        canSem = dateTime.Days / 7;
                        recargoTemp = recargo * canSem;
                    }
                }

                recargoTotalPeriodo = recargoTemp;
            }

            recargoTotal = recargoTotalLimite + recargoTotalPeriodo;

            decimal ImporteTotal = ImporteCole;
            decimal ImporteBeca = 0;

            if (BitBeca)
            {
                if (VchTipoBeca == "CANTIDAD")
                {
                    ImporteBeca = DcmBeca;
                    ImporteTotal = ImporteTotal - ImporteBeca;
                }
                else if (VchTipoBeca == "PORCENTAJE")
                {
                    decimal porcentaje = DcmBeca;

                    ImporteBeca = porcentaje * ImporteTotal / 100;
                    ImporteTotal = ImporteTotal - ImporteBeca;
                }
            }

            ImporteTotal = ImporteTotal + recargoTotal;

            //ImporteTotal = ImporteTotal - pagosColegiaturasServices.lsFechasPagosColegiaturasViewModel.Sum(x => x.DcmImportePagado);

            return ImporteTotal;
        }
        public List<PagosColegiaturasViewModel> CargarPagosColegiaturas(Guid UidCliente, Guid UidUsuario, DateTime FechaInicio)
        {
            List<PagosColegiaturasViewModel> lsPagosColegiaturasViewModel = new List<PagosColegiaturasViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            // ==>SIN ESTATUS<== query.CommandText = "select cl.VchNombreComercial, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno,  co.*, fc.UidFechaColegiatura, fc.IntNum, fc.DtFHInicio as fcInicio, fc.DtFHLimite as fcLimite, fc.DtFHVencimiento as fcVencimiento from clientes cl, Colegiaturas co, FechasColegiaturas fc, ColegiaturasAlumnos ca, Alumnos al, Usuarios us, UsuariosAlumnos ua where not exists (select * from LigasUrls lu, PagosTarjeta pt where pt.IdReferencia = lu.IdReferencia and pt.VchEstatus = 'approved' and lu.UidFechaColegiatura = fc.UidFechaColegiatura) and cl.UidCliente = co.UidCliente and co.UidColegiatura = fc.UidColegiatura and ca.UidColegiatura = co.UidColegiatura and ca.UidAlumno = al.UidAlumno and ua.UidUsuario = us.UidUsuario and ua.UidAlumno = al.UidAlumno and cl.UidCliente = '" + UidCliente + "' and us.UidUsuario = '" + UidUsuario + "' order by al.VchMatricula";
            //query.CommandText = "select cl.VchNombreComercial, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, al.BitBeca, al.VchTipoBeca, al.DcmBeca, co.*, fc.UidFechaColegiatura, fc.IntNum, fc.DtFHInicio as fcInicio, fc.DtFHLimite as fcLimite, fc.DtFHVencimiento as fcVencimiento, fc.DtFHFinPeriodo as fcFinPeriodo, efc.VchDescripcion as EstatusFechas, pe.VchDescripcion as Periodicidad from clientes cl, Colegiaturas co, FechasColegiaturas fc, ColegiaturasAlumnos ca, Alumnos al, Usuarios us, UsuariosAlumnos ua, EstatusFechasColegiaturas efc, Periodicidades pe where pe.UidPeriodicidad = co.UidPeriodicidad and not exists (select * from LigasUrls lu, PagosTarjeta pt where lu.UidUsuario = us.UidUsuario and pt.IdReferencia = lu.IdReferencia and pt.VchEstatus = 'approved' and lu.UidFechaColegiatura = fc.UidFechaColegiatura) and co.UidEstatus = '65E46BC9-1864-4145-AD1A-70F5B5F69739' and efc.UidEstatusFechaColegiatura = fc.UidEstatusFechaColegiatura and cl.UidCliente = co.UidCliente and co.UidColegiatura = fc.UidColegiatura and ca.UidColegiatura = co.UidColegiatura and ca.UidAlumno = al.UidAlumno and ua.UidUsuario = us.UidUsuario and ua.UidAlumno = al.UidAlumno and cl.UidCliente = '" + UidCliente + "' and us.UidUsuario = '" + UidUsuario + "' order by al.VchMatricula";
            // ==> ANTES DE LOS 3 IMPORTES query.CommandText = "select cl.VchNombreComercial, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, al.BitBeca, al.VchTipoBeca, al.DcmBeca, co.*, fc.UidFechaColegiatura, fc.IntNum, fc.DtFHInicio as fcInicio, fc.DtFHLimite as fcLimite, fc.DtFHVencimiento as fcVencimiento, fc.DtFHFinPeriodo as fcFinPeriodo, efc.VchDescripcion as EstatusFechas, pe.VchDescripcion as Periodicidad from clientes cl, Colegiaturas co, FechasColegiaturas fc, ColegiaturasAlumnos ca, Alumnos al, Usuarios us, UsuariosAlumnos ua, EstatusFechasColegiaturas efc, Periodicidades pe where pe.UidPeriodicidad = co.UidPeriodicidad and not exists (select * from FechasColegiaturas feco, FechasPagos fepa, PagosColegiaturas paco, Alumnos alu where alu.UidAlumno = fepa.UidAlumno and feco.UidFechaColegiatura = fepa.UidFechaColegiatura and fepa.UidPagoColegiatura = paco.UidPagoColegiatura and alu.UidAlumno = al.UidAlumno and fepa.UidEstatusFechaPago = '8720B2B9-5712-4E75-A981-932887AACDC9' and paco.UidEstatusPagoColegiatura = '51B85D66-866B-4BC2-B08F-FECE1A994053' and fepa.UidFechaColegiatura = fc.UidFechaColegiatura) and co.UidEstatus = '65E46BC9-1864-4145-AD1A-70F5B5F69739' and efc.UidEstatusFechaColegiatura = fc.UidEstatusFechaColegiatura and cl.UidCliente = co.UidCliente and co.UidColegiatura = fc.UidColegiatura and ca.UidColegiatura = co.UidColegiatura and ca.UidAlumno = al.UidAlumno and ua.UidUsuario = us.UidUsuario and ua.UidAlumno = al.UidAlumno and cl.UidCliente = '" + UidCliente + "' and us.UidUsuario = '" + UidUsuario + "' order by al.VchMatricula";
            // ==> Amarre sin la tabla nueva FecPagAlu query.CommandText = "select cl.VchNombreComercial, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, al.BitBeca, al.VchTipoBeca, al.DcmBeca, co.*, fc.UidFechaColegiatura, fc.IntNum, fc.DtFHInicio as fcInicio, fc.DtFHLimite as fcLimite, fc.DtFHVencimiento as fcVencimiento, fc.DtFHFinPeriodo as fcFinPeriodo, efc.VchDescripcion as EstatusFechas, pe.VchDescripcion as Periodicidad, (select SUM(fepa.DcmImportePagado) from FechasColegiaturas feco, FechasPagos fepa, PagosColegiaturas paco where feco.UidFechaColegiatura = fepa.UidFechaColegiatura and fepa.UidPagoColegiatura = paco.UidPagoColegiatura and fepa.UidEstatusFechaPago != '8720B2B9-5712-4E75-A981-932887AACDC9' and paco.UidEstatusPagoColegiatura = '51B85D66-866B-4BC2-B08F-FECE1A994053' and fepa.UidFechaColegiatura = fc.UidFechaColegiatura and fepa.UidAlumno = al.UidAlumno) ImpPagado from clientes cl, Colegiaturas co, FechasColegiaturas fc, ColegiaturasAlumnos ca, Alumnos al, Usuarios us, UsuariosAlumnos ua, EstatusFechasColegiaturas efc, Periodicidades pe where pe.UidPeriodicidad = co.UidPeriodicidad and not exists (select * from FechasColegiaturas feco, FechasPagos fepa, PagosColegiaturas paco, Alumnos alu where alu.UidAlumno = fepa.UidAlumno and feco.UidFechaColegiatura = fepa.UidFechaColegiatura and fepa.UidPagoColegiatura = paco.UidPagoColegiatura and alu.UidAlumno = al.UidAlumno and fepa.UidEstatusFechaPago = '8720B2B9-5712-4E75-A981-932887AACDC9' and paco.UidEstatusPagoColegiatura = '51B85D66-866B-4BC2-B08F-FECE1A994053' and fepa.UidFechaColegiatura = fc.UidFechaColegiatura) and co.UidEstatus = '65E46BC9-1864-4145-AD1A-70F5B5F69739' and efc.UidEstatusFechaColegiatura = fc.UidEstatusFechaColegiatura and cl.UidCliente = co.UidCliente and co.UidColegiatura = fc.UidColegiatura and ca.UidColegiatura = co.UidColegiatura and ca.UidAlumno = al.UidAlumno and ua.UidUsuario = us.UidUsuario and ua.UidAlumno = al.UidAlumno and cl.UidCliente = '" + UidCliente + "' and us.UidUsuario = '" + UidUsuario + "' order by al.VchMatricula";
            //==> Amarre sin la tabla nueva EstatusColegiaturasAlumnos query.CommandText = "select fca.BitUsarFecha, fca.DtFechaPago, fc.UidFechaColegiatura, cl.VchNombreComercial, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, al.BitBeca, al.VchTipoBeca, al.DcmBeca, co.*, fc.UidFechaColegiatura, fc.IntNum, fc.DtFHInicio as fcInicio, fc.DtFHLimite as fcLimite, fc.DtFHVencimiento as fcVencimiento, fc.DtFHFinPeriodo as fcFinPeriodo, efc.UidEstatusFechaColegiatura, efc.VchDescripcion as EstatusFechas, efc.VchColor, pe.VchDescripcion as Periodicidad, (select SUM(fepa.DcmImportePagado) from FechasColegiaturas feco, FechasPagos fepa, PagosColegiaturas paco where feco.UidFechaColegiatura = fepa.UidFechaColegiatura and fepa.UidPagoColegiatura = paco.UidPagoColegiatura and fepa.UidFechaColegiatura = fc.UidFechaColegiatura and fepa.UidAlumno = al.UidAlumno and (fepa.UidEstatusFechaPago = '8720B2B9-5712-4E75-A981-932887AACDC9' or fepa.UidEstatusFechaPago = 'F25E4AAB-6044-46E9-A575-98DCBCCF7604')) ImpPagado from clientes cl, Colegiaturas co, FechasColegiaturas fc, Alumnos al, Usuarios us, UsuariosAlumnos ua, EstatusFechasColegiaturas efc, Periodicidades pe, FechasColegiaturasAlumnos fca where fca.UidEstatusFechaColegiatura = efc.UidEstatusFechaColegiatura and fca.UidAlumno = al.UidAlumno and fca.UidFechaColegiatura = fc.UidFechaColegiatura and pe.UidPeriodicidad = co.UidPeriodicidad and co.UidEstatus = '65E46BC9-1864-4145-AD1A-70F5B5F69739' and (fca.UidEstatusFechaColegiatura != '5554CE57-1288-46D5-B36A-8AC69CB94B9A' and fca.UidEstatusFechaColegiatura != '605A7881-54E0-47DF-8398-EDE080F4E0AA') and cl.UidCliente = co.UidCliente and co.UidColegiatura = fc.UidColegiatura and ua.UidUsuario = us.UidUsuario and ua.UidAlumno = al.UidAlumno and cl.UidCliente = '" + UidCliente + "' and us.UidUsuario = '" + UidUsuario + "' order by al.VchMatricula";
            query.CommandText = "select eca.UidEstatusColeAlumnos, eca.VchDescripcion as EstatusPago, eca.VchColor as ColorEstatusPago, fca.BitUsarFecha, fca.DtFechaPago, fc.UidFechaColegiatura, cl.VchNombreComercial, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, al.BitBeca, al.VchTipoBeca, al.DcmBeca, co.*, fc.UidFechaColegiatura, fc.IntNum, fc.DtFHInicio as fcInicio, fc.DtFHLimite as fcLimite, fc.DtFHVencimiento as fcVencimiento, fc.DtFHFinPeriodo as fcFinPeriodo, efc.UidEstatusFechaColegiatura, efc.VchDescripcion as EstatusFechas, efc.VchColor, pe.VchDescripcion as Periodicidad, (select SUM(fepa.DcmImportePagado) from FechasColegiaturas feco, FechasPagos fepa, PagosColegiaturas paco where feco.UidFechaColegiatura = fepa.UidFechaColegiatura and fepa.UidPagoColegiatura = paco.UidPagoColegiatura and fepa.UidFechaColegiatura = fc.UidFechaColegiatura and fepa.UidAlumno = al.UidAlumno and (fepa.UidEstatusFechaPago = '8720B2B9-5712-4E75-A981-932887AACDC9' or fepa.UidEstatusFechaPago = 'F25E4AAB-6044-46E9-A575-98DCBCCF7604')) ImpPagado from clientes cl, Colegiaturas co, FechasColegiaturas fc, Alumnos al, Usuarios us, UsuariosAlumnos ua, EstatusFechasColegiaturas efc, Periodicidades pe, FechasColegiaturasAlumnos fca, EstatusColegiaturasAlumnos eca where eca.UidEstatusColeAlumnos = fca.UidEstatusColeAlumnos and  fca.UidEstatusFechaColegiatura = efc.UidEstatusFechaColegiatura and fca.UidAlumno = al.UidAlumno and fca.UidFechaColegiatura = fc.UidFechaColegiatura and pe.UidPeriodicidad = co.UidPeriodicidad and co.UidEstatus = '65E46BC9-1864-4145-AD1A-70F5B5F69739' and (fca.UidEstatusFechaColegiatura != '5554CE57-1288-46D5-B36A-8AC69CB94B9A' and fca.UidEstatusFechaColegiatura != '605A7881-54E0-47DF-8398-EDE080F4E0AA') and cl.UidCliente = co.UidCliente and co.UidColegiatura = fc.UidColegiatura and ua.UidUsuario = us.UidUsuario and ua.UidAlumno = al.UidAlumno and cl.UidCliente = '" + UidCliente + "' and us.UidUsuario = '" + UidUsuario + "' order by al.VchMatricula";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                string FHLimite = "NO TIENE";
                string FHVencimiento = "NO TIENE";
                bool FHVence = false;
                bool blDatoFecha = false;

                bool blpagar = true;

                if (Guid.Parse(item["UidEstatusFechaColegiatura"].ToString()) == Guid.Parse("1331D93D-EA53-487F-BF28-E72F5E7D19BF"))
                {
                    blpagar = false;
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

                if (bool.Parse(item["BitUsarFecha"].ToString()))
                {
                    if (!string.IsNullOrEmpty(item["DtFechaPago"].ToString()))
                    {
                        blDatoFecha = true;
                    }
                }

                lsPagosColegiaturasViewModel.Add(new PagosColegiaturasViewModel()
                {
                    UidCliente = Guid.Parse(item["UidCliente"].ToString()),
                    UidFechaColegiatura = Guid.Parse(item["UidFechaColegiatura"].ToString()),
                    VchIdentificador = item["VchIdentificador"].ToString(),
                    DcmImporte = Calculo(decimal.Parse(item["DcmImporte"].ToString()), bool.Parse(item["BitRecargo"].ToString()), item["VchTipoRecargo"].ToString(), decimal.Parse(item["DcmRecargo"].ToString()), FHLimite, bool.Parse(item["BitRecargoPeriodo"].ToString()), item["VchTipoRecargoPeriodo"].ToString(), decimal.Parse(item["DcmRecargoPeriodo"].ToString()), item["Periodicidad"].ToString(), bool.Parse(item["BitBeca"].ToString()), item["VchTipoBeca"].ToString(), decimal.Parse(item["DcmBeca"].ToString()), DateTime.Parse(item["fcFinPeriodo"].ToString()), blDatoFecha, item["DtFechaPago"].ToString()),
                    ImpPagado = item.IsNull("ImpPagado") ? 0 : decimal.Parse(item["ImpPagado"].ToString()),
                    UidColegiatura = Guid.Parse(item["UidColegiatura"].ToString()),
                    VchNum = int.Parse(item["IntNum"].ToString()) + " de " + int.Parse(item["IntCantPagos"].ToString()),
                    DtFHInicio = DateTime.Parse(item["fcInicio"].ToString()),
                    VchFHLimite = FHLimite,
                    VchFHVencimiento = FHVencimiento,
                    DtFHFinPeriodo = DateTime.Parse(item["fcFinPeriodo"].ToString()),
                    VchEstatusFechas = item["EstatusFechas"].ToString(),
                    VchColor = item["VchColor"].ToString(),
                    EstatusPago = item["EstatusPago"].ToString(),
                    ColorEstatusPago = item["ColorEstatusPago"].ToString(),
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

            //query.CommandText = "select cl.VchNombreComercial, al.UidAlumno, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, al.BitBeca, al.VchTipoBeca, al.DcmBeca, co.*, fc.UidFechaColegiatura, fc.IntNum, fc.DtFHInicio as fcInicio, fc.DtFHLimite as fcLimite, fc.DtFHVencimiento as fcVencimiento, fc.DtFHFinPeriodo as fcFinPeriodo, efc.VchDescripcion as EstatusFechas, pe.VchDescripcion as Periodicidad from clientes cl, Colegiaturas co, FechasColegiaturas fc, ColegiaturasAlumnos ca, Alumnos al, Usuarios us, UsuariosAlumnos ua, EstatusFechasColegiaturas efc, Periodicidades pe where pe.UidPeriodicidad = co.UidPeriodicidad and not exists (select * from LigasUrls lu, PagosTarjeta pt where lu.UidUsuario = us.UidUsuario and pt.IdReferencia = lu.IdReferencia and pt.VchEstatus = 'approved' and lu.UidFechaColegiatura = fc.UidFechaColegiatura) and co.UidEstatus = '65E46BC9-1864-4145-AD1A-70F5B5F69739' and efc.UidEstatusFechaColegiatura = fc.UidEstatusFechaColegiatura and cl.UidCliente = co.UidCliente and co.UidColegiatura = fc.UidColegiatura and ca.UidColegiatura = co.UidColegiatura and ca.UidAlumno = al.UidAlumno and ua.UidUsuario = us.UidUsuario and ua.UidAlumno = al.UidAlumno and cl.UidCliente = '" + UidCliente + "' and us.UidUsuario = '" + UidUsuario + "' and fc.UidFechaColegiatura = '" + UidFechaColegiatura + "' and al.VchMatricula = '" + VchMatricula + "'";
            query.CommandText = "select cl.VchNombreComercial, al.UidAlumno, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, al.BitBeca, al.VchTipoBeca, al.DcmBeca, co.*, fc.UidFechaColegiatura, fc.IntNum, fc.DtFHInicio as fcInicio, fc.DtFHLimite as fcLimite, fc.DtFHVencimiento as fcVencimiento, fc.DtFHFinPeriodo as fcFinPeriodo, efc.UidEstatusFechaColegiatura, efc.VchDescripcion as EstatusFechas, efc.VchColor, pe.VchDescripcion as Periodicidad from clientes cl, Colegiaturas co, FechasColegiaturas fc, ColegiaturasAlumnos ca, Alumnos al, Usuarios us, UsuariosAlumnos ua, EstatusFechasColegiaturas efc, Periodicidades pe where pe.UidPeriodicidad = co.UidPeriodicidad and co.UidEstatus = '65E46BC9-1864-4145-AD1A-70F5B5F69739' and efc.UidEstatusFechaColegiatura = fc.UidEstatusFechaColegiatura and cl.UidCliente = co.UidCliente and co.UidColegiatura = fc.UidColegiatura and ca.UidColegiatura = co.UidColegiatura and ca.UidAlumno = al.UidAlumno and ua.UidUsuario = us.UidUsuario and ua.UidAlumno = al.UidAlumno and cl.UidCliente = '" + UidCliente + "' and us.UidUsuario = '" + UidUsuario + "' and fc.UidFechaColegiatura = '" + UidFechaColegiatura + "' and al.VchMatricula = '" + VchMatricula + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                string FHLimite = "NO TIENE";
                string FHVencimiento = "NO TIENE";
                bool FHVence = false;

                bool blpagar = false;

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
                pagosColegiaturasViewModel.VchColor = item["VchColor"].ToString();
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
        public bool EliminarLigaPragaColegiatura(string IdReferencia)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ColegiaturasLigaPragaEliminar";

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

        public bool ActualizarEstatusColegiaturaAlumno(Guid UidFechaColegiatura, Guid UidAlumno, DateTime DtFechaPago, Guid UidEstatus)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ColegiaturasAlumnosEstatusActualizar";

                comando.Parameters.Add("@UidFechaColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidFechaColegiatura"].Value = UidFechaColegiatura;

                comando.Parameters.Add("@UidAlumno", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidAlumno"].Value = UidAlumno;

                comando.Parameters.Add("@DtFechaPago", SqlDbType.DateTime);
                comando.Parameters["@DtFechaPago"].Value = DtFechaPago;

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

        public List<PagosColegiaturasViewModel> BuscarColegiaturaPadre(Guid UidCliente, Guid UidUsuario, DateTime FechaInicio, string Colegiatura, string NumPago, Guid EstatusCole, Guid EstatusPago, string Matricula, string AlNombre, string AlApePaterno, string AlApeMaterno)
        {
            List<PagosColegiaturasViewModel> lsPagosColegiaturasViewModel = new List<PagosColegiaturasViewModel>();

            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "sp_PagosColegiaturasPadresBuscar";
            try
            {
                if (UidCliente != Guid.Empty)
                {
                    comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidCliente"].Value = UidCliente;
                }

                if (UidCliente != Guid.Empty)
                {
                    comando.Parameters.Add("@UidUsuario", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidUsuario"].Value = UidUsuario;
                }


                if (Colegiatura != string.Empty)
                {
                    comando.Parameters.Add("@Colegiatura", SqlDbType.VarChar, 50);
                    comando.Parameters["@Colegiatura"].Value = Colegiatura;
                }
                if (NumPago != string.Empty)
                {
                    comando.Parameters.Add("@NumPago", SqlDbType.Int);
                    comando.Parameters["@NumPago"].Value = int.Parse(NumPago);
                }
                if (EstatusCole != Guid.Empty)
                {
                    comando.Parameters.Add("@EstatusCole", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@EstatusCole"].Value = EstatusCole;
                }
                if (EstatusPago != Guid.Empty)
                {
                    comando.Parameters.Add("@EstatusPago", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@EstatusPago"].Value = EstatusPago;
                }


                if (Matricula != string.Empty)
                {
                    comando.Parameters.Add("@Matricula", SqlDbType.VarChar, 50);
                    comando.Parameters["@Matricula"].Value = Matricula;
                }
                if (AlNombre != string.Empty)
                {
                    comando.Parameters.Add("@AlNombre", SqlDbType.VarChar, 50);
                    comando.Parameters["@AlNombre"].Value = AlNombre;
                }
                if (AlApePaterno != string.Empty)
                {
                    comando.Parameters.Add("@AlApePaterno", SqlDbType.VarChar, 50);
                    comando.Parameters["@AlApePaterno"].Value = AlApePaterno;
                }
                if (AlApeMaterno != string.Empty)
                {
                    comando.Parameters.Add("@AlApeMaterno", SqlDbType.VarChar, 50);
                    comando.Parameters["@AlApeMaterno"].Value = AlApeMaterno;
                }

                foreach (DataRow item in this.Busquedas(comando).Rows)
                {
                    string FHLimite = "NO TIENE";
                    string FHVencimiento = "NO TIENE";
                    bool FHVence = false;
                    bool blDatoFecha = false;

                    bool blpagar = true;

                    if (Guid.Parse(item["UidEstatusFechaColegiatura"].ToString()) == Guid.Parse("1331D93D-EA53-487F-BF28-E72F5E7D19BF"))
                    {
                        blpagar = false;
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

                    if (bool.Parse(item["BitUsarFecha"].ToString()))
                    {
                        if (!string.IsNullOrEmpty(item["DtFechaPago"].ToString()))
                        {
                            blDatoFecha = true;
                        }
                    }

                    lsPagosColegiaturasViewModel.Add(new PagosColegiaturasViewModel()
                    {
                        UidCliente = Guid.Parse(item["UidCliente"].ToString()),
                        UidFechaColegiatura = Guid.Parse(item["UidFechaColegiatura"].ToString()),
                        VchIdentificador = item["VchIdentificador"].ToString(),
                        DcmImporte = Calculo(decimal.Parse(item["DcmImporte"].ToString()), bool.Parse(item["BitRecargo"].ToString()), item["VchTipoRecargo"].ToString(), decimal.Parse(item["DcmRecargo"].ToString()), FHLimite, bool.Parse(item["BitRecargoPeriodo"].ToString()), item["VchTipoRecargoPeriodo"].ToString(), decimal.Parse(item["DcmRecargoPeriodo"].ToString()), item["Periodicidad"].ToString(), bool.Parse(item["BitBeca"].ToString()), item["VchTipoBeca"].ToString(), decimal.Parse(item["DcmBeca"].ToString()), DateTime.Parse(item["fcFinPeriodo"].ToString()), blDatoFecha, item["DtFechaPago"].ToString()),
                        ImpPagado = item.IsNull("ImpPagado") ? 0 : decimal.Parse(item["ImpPagado"].ToString()),
                        UidColegiatura = Guid.Parse(item["UidColegiatura"].ToString()),
                        VchNum = int.Parse(item["IntNum"].ToString()) + " de " + int.Parse(item["IntCantPagos"].ToString()),
                        DtFHInicio = DateTime.Parse(item["fcInicio"].ToString()),
                        VchFHLimite = FHLimite,
                        VchFHVencimiento = FHVencimiento,
                        DtFHFinPeriodo = DateTime.Parse(item["fcFinPeriodo"].ToString()),
                        VchEstatusFechas = item["EstatusFechas"].ToString(),
                        VchColor = item["VchColor"].ToString(),
                        EstatusPago = item["EstatusPago"].ToString(),
                        ColorEstatusPago = item["ColorEstatusPago"].ToString(),
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
                }

                return lsPagosColegiaturasViewModel.OrderBy(x => x.DtFHInicio).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Metodos ReporteLigasPadres
        public List<PagosColegiaturasViewModel> CargarPagosColegiaturasReporte(Guid UidUsuario)
        {
            List<PagosColegiaturasViewModel> lsPagosColegiaturasViewModel = new List<PagosColegiaturasViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            // ==>SIN ESTATUS<== query.CommandText = "select cl.VchNombreComercial, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno,  co.*, fc.UidFechaColegiatura, fc.IntNum, fc.DtFHInicio as fcInicio, fc.DtFHLimite as fcLimite, fc.DtFHVencimiento as fcVencimiento from clientes cl, Colegiaturas co, FechasColegiaturas fc, ColegiaturasAlumnos ca, Alumnos al, Usuarios us, UsuariosAlumnos ua where not exists (select * from LigasUrls lu, PagosTarjeta pt where pt.IdReferencia = lu.IdReferencia and pt.VchEstatus = 'approved' and lu.UidFechaColegiatura = fc.UidFechaColegiatura) and cl.UidCliente = co.UidCliente and co.UidColegiatura = fc.UidColegiatura and ca.UidColegiatura = co.UidColegiatura and ca.UidAlumno = al.UidAlumno and ua.UidUsuario = us.UidUsuario and ua.UidAlumno = al.UidAlumno and cl.UidCliente = '" + UidCliente + "' and us.UidUsuario = '" + UidUsuario + "' order by al.VchMatricula";
            //query.CommandText = "select cl.VchNombreComercial, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, al.BitBeca, al.VchTipoBeca, al.DcmBeca, co.*, fc.UidFechaColegiatura, fc.IntNum, fc.DtFHInicio as fcInicio, fc.DtFHLimite as fcLimite, fc.DtFHVencimiento as fcVencimiento, fc.DtFHFinPeriodo as fcFinPeriodo, efc.VchDescripcion as EstatusFechas, pe.VchDescripcion as Periodicidad from clientes cl, Colegiaturas co, FechasColegiaturas fc, ColegiaturasAlumnos ca, Alumnos al, Usuarios us, UsuariosAlumnos ua, EstatusFechasColegiaturas efc, Periodicidades pe where pe.UidPeriodicidad = co.UidPeriodicidad and not exists (select * from LigasUrls lu, PagosTarjeta pt where lu.UidUsuario = us.UidUsuario and pt.IdReferencia = lu.IdReferencia and pt.VchEstatus = 'approved' and lu.UidFechaColegiatura = fc.UidFechaColegiatura) and co.UidEstatus = '65E46BC9-1864-4145-AD1A-70F5B5F69739' and efc.UidEstatusFechaColegiatura = fc.UidEstatusFechaColegiatura and cl.UidCliente = co.UidCliente and co.UidColegiatura = fc.UidColegiatura and ca.UidColegiatura = co.UidColegiatura and ca.UidAlumno = al.UidAlumno and ua.UidUsuario = us.UidUsuario and ua.UidAlumno = al.UidAlumno and cl.UidCliente = '" + UidCliente + "' and us.UidUsuario = '" + UidUsuario + "' order by al.VchMatricula";
            // ==>ANTES DE LOS 3 IMPORTES query.CommandText = "select cl.VchNombreComercial, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, al.BitBeca, al.VchTipoBeca, al.DcmBeca, co.*, fc.UidFechaColegiatura, fc.IntNum, fc.DtFHInicio as fcInicio, fc.DtFHLimite as fcLimite, fc.DtFHVencimiento as fcVencimiento, fc.DtFHFinPeriodo as fcFinPeriodo, efc.VchDescripcion as EstatusFechas, pe.VchDescripcion as Periodicidad from clientes cl, Colegiaturas co, FechasColegiaturas fc, ColegiaturasAlumnos ca, Alumnos al, Usuarios us, UsuariosAlumnos ua, EstatusFechasColegiaturas efc, Periodicidades pe where pe.UidPeriodicidad = co.UidPeriodicidad and not exists (select * from FechasColegiaturas feco, FechasPagos fepa, PagosColegiaturas paco, Alumnos alu where alu.UidAlumno = fepa.UidAlumno and feco.UidFechaColegiatura = fepa.UidFechaColegiatura and fepa.UidPagoColegiatura = paco.UidPagoColegiatura and alu.UidAlumno = al.UidAlumno and fepa.UidEstatusFechaPago = '8720B2B9-5712-4E75-A981-932887AACDC9' and paco.UidEstatusPagoColegiatura = '51B85D66-866B-4BC2-B08F-FECE1A994053' and fepa.UidFechaColegiatura = fc.UidFechaColegiatura) and co.UidEstatus = '65E46BC9-1864-4145-AD1A-70F5B5F69739' and efc.UidEstatusFechaColegiatura = fc.UidEstatusFechaColegiatura and cl.UidCliente = co.UidCliente and co.UidColegiatura = fc.UidColegiatura and ca.UidColegiatura = co.UidColegiatura and ca.UidAlumno = al.UidAlumno and ua.UidUsuario = us.UidUsuario and ua.UidAlumno = al.UidAlumno and cl.UidCliente = '" + UidCliente + "' and us.UidUsuario = '" + UidUsuario + "' order by al.VchMatricula";
            //==> Amarre sin la tabla nueva Amarre sin la tabla nueva FecPagAlu query.CommandText = "select cl.VchNombreComercial, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, al.BitBeca, al.VchTipoBeca, al.DcmBeca, co.*, fc.UidFechaColegiatura, fc.IntNum, fc.DtFHInicio as fcInicio, fc.DtFHLimite as fcLimite, fc.DtFHVencimiento as fcVencimiento, fc.DtFHFinPeriodo as fcFinPeriodo, efc.VchDescripcion as EstatusFechas, pe.VchDescripcion as Periodicidad, (select SUM(fepa.DcmImportePagado) from FechasColegiaturas feco, FechasPagos fepa, PagosColegiaturas paco where feco.UidFechaColegiatura = fepa.UidFechaColegiatura and fepa.UidPagoColegiatura = paco.UidPagoColegiatura and fepa.UidEstatusFechaPago != '8720B2B9-5712-4E75-A981-932887AACDC9' and paco.UidEstatusPagoColegiatura = '51B85D66-866B-4BC2-B08F-FECE1A994053' and fepa.UidFechaColegiatura = fc.UidFechaColegiatura and fepa.UidAlumno = al.UidAlumno) ImpPagado from clientes cl, Colegiaturas co, FechasColegiaturas fc, ColegiaturasAlumnos ca, Alumnos al, Usuarios us, UsuariosAlumnos ua, EstatusFechasColegiaturas efc, Periodicidades pe where pe.UidPeriodicidad = co.UidPeriodicidad and co.UidEstatus = '65E46BC9-1864-4145-AD1A-70F5B5F69739' and efc.UidEstatusFechaColegiatura = fc.UidEstatusFechaColegiatura and cl.UidCliente = co.UidCliente and co.UidColegiatura = fc.UidColegiatura and ca.UidColegiatura = co.UidColegiatura and ca.UidAlumno = al.UidAlumno and ua.UidUsuario = us.UidUsuario and ua.UidAlumno = al.UidAlumno and us.UidUsuario = '" + UidUsuario + "' order by al.VchMatricula";
            query.CommandText = "select fca.BitUsarFecha, fca.DtFechaPago, cl.VchNombreComercial, al.UidAlumno, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, al.BitBeca, al.VchTipoBeca, al.DcmBeca, co.*, fc.UidFechaColegiatura, fc.IntNum, fc.DtFHInicio as fcInicio, fc.DtFHLimite as fcLimite, fc.DtFHVencimiento as fcVencimiento, fc.DtFHFinPeriodo as fcFinPeriodo, efc.UidEstatusFechaColegiatura, efc.VchDescripcion as EstatusFechas, efc.VchColor, pe.VchDescripcion as Periodicidad, (select SUM(fepa.DcmImportePagado) from FechasColegiaturas feco, FechasPagos fepa, PagosColegiaturas paco where feco.UidFechaColegiatura = fepa.UidFechaColegiatura and fepa.UidPagoColegiatura = paco.UidPagoColegiatura and fepa.UidFechaColegiatura = fc.UidFechaColegiatura and fepa.UidAlumno = al.UidAlumno and (fepa.UidEstatusFechaPago = '8720B2B9-5712-4E75-A981-932887AACDC9' or fepa.UidEstatusFechaPago = 'F25E4AAB-6044-46E9-A575-98DCBCCF7604')) ImpPagado from clientes cl, Colegiaturas co, FechasColegiaturas fc, Alumnos al, Usuarios us, UsuariosAlumnos ua, EstatusFechasColegiaturas efc, Periodicidades pe, FechasColegiaturasAlumnos fca where fca.UidEstatusFechaColegiatura = efc.UidEstatusFechaColegiatura and fca.UidAlumno = al.UidAlumno and fca.UidFechaColegiatura = fc.UidFechaColegiatura and  pe.UidPeriodicidad = co.UidPeriodicidad and co.UidEstatus = '65E46BC9-1864-4145-AD1A-70F5B5F69739' and cl.UidCliente = co.UidCliente and co.UidColegiatura = fc.UidColegiatura and ua.UidUsuario = us.UidUsuario and ua.UidAlumno = al.UidAlumno and us.UidUsuario = '" + UidUsuario + "' order by al.VchMatricula";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                string FHLimite = "NO TIENE";
                string FHVencimiento = "NO TIENE";
                bool FHVence = false;
                bool blDatoFecha = false;

                bool blpagar = false;

                if (!string.IsNullOrEmpty(item["fcLimite"].ToString()))
                {
                    FHLimite = DateTime.Parse(item["fcLimite"].ToString()).ToString("dd/MM/yyyy");
                }
                if (!string.IsNullOrEmpty(item["fcVencimiento"].ToString()))
                {
                    FHVencimiento = DateTime.Parse(item["fcVencimiento"].ToString()).ToString("dd/MM/yyyy");
                    FHVence = true;
                }

                if (bool.Parse(item["BitUsarFecha"].ToString()))
                {
                    if (!string.IsNullOrEmpty(item["DtFechaPago"].ToString()))
                    {
                        blDatoFecha = true;
                    }
                }

                lsPagosColegiaturasViewModel.Add(new PagosColegiaturasViewModel()
                {
                    UidCliente = Guid.Parse(item["UidCliente"].ToString()),
                    UidFechaColegiatura = Guid.Parse(item["UidFechaColegiatura"].ToString()),
                    VchIdentificador = item["VchIdentificador"].ToString(),
                    DcmImporte = Calculo(decimal.Parse(item["DcmImporte"].ToString()), bool.Parse(item["BitRecargo"].ToString()), item["VchTipoRecargo"].ToString(), decimal.Parse(item["DcmRecargo"].ToString()), FHLimite, bool.Parse(item["BitRecargoPeriodo"].ToString()), item["VchTipoRecargoPeriodo"].ToString(), decimal.Parse(item["DcmRecargoPeriodo"].ToString()), item["Periodicidad"].ToString(), bool.Parse(item["BitBeca"].ToString()), item["VchTipoBeca"].ToString(), decimal.Parse(item["DcmBeca"].ToString()), DateTime.Parse(item["fcFinPeriodo"].ToString()), blDatoFecha, item["DtFechaPago"].ToString()),
                    ImpPagado = item.IsNull("ImpPagado") ? 0 : decimal.Parse(item["ImpPagado"].ToString()),
                    UidColegiatura = Guid.Parse(item["UidColegiatura"].ToString()),
                    VchNum = int.Parse(item["IntNum"].ToString()) + " de " + int.Parse(item["IntCantPagos"].ToString()),
                    DtFHInicio = DateTime.Parse(item["fcInicio"].ToString()),
                    VchFHLimite = FHLimite,
                    VchFHVencimiento = FHVencimiento,
                    DtFHFinPeriodo = DateTime.Parse(item["fcFinPeriodo"].ToString()),
                    VchEstatusFechas = item["EstatusFechas"].ToString(),
                    VchColor = item["VchColor"].ToString(),
                    BitRecargo = bool.Parse(item["BitRecargo"].ToString()),
                    VchTipoRecargo = item["VchTipoRecargo"].ToString(),
                    DcmRecargo = decimal.Parse(item["DcmRecargo"].ToString()),
                    BitRecargoPeriodo = bool.Parse(item["BitRecargoPeriodo"].ToString()),
                    VchTipoRecargoPeriodo = item["VchTipoRecargoPeriodo"].ToString(),
                    DcmRecargoPeriodo = decimal.Parse(item["DcmRecargoPeriodo"].ToString()),
                    VchPeriodicidad = item["Periodicidad"].ToString(),
                    UidAlumno = Guid.Parse(item["UidAlumno"].ToString()),
                    VchMatricula = item["VchMatricula"].ToString(),
                    VchNombres = item["VchNombres"].ToString(),
                    VchApePaterno = item["VchApePaterno"].ToString(),
                    VchApeMaterno = item["VchApeMaterno"].ToString(),
                    BitBeca = bool.Parse(item["BitBeca"].ToString()),
                    VchTipoBeca = item["VchTipoBeca"].ToString(),
                    DcmBeca = decimal.Parse(item["DcmBeca"].ToString()),

                    blPagar = blpagar
                });
            }

            return lsPagosColegiaturasViewModel.OrderBy(x => x.DtFHInicio).ToList();
        }
        public List<PagosReporteLigaPadreViewModels> CargarPagosColeReportePadre(Guid UidUsuario)
        {
            List<PagosReporteLigaPadreViewModels> lsPagosReporteLigaPadreViewModels = new List<PagosReporteLigaPadreViewModels>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select fc.UidFechaColegiatura, pc.UidPagoColegiatura, pc.IntFolio, co.VchIdentificador, al.UidAlumno, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, fc.IntNum, co.IntCantPagos, pc.DtFHPago, fp.DcmImporteSaldado, fp.DcmImporteCole, fp.DcmImportePagado, fp.DcmImporteNuevo, us.UidUsuario, us.VchNombre as UsNombre, us.VchApePaterno as UsPaterno, Us.VchApeMaterno as UsMaterno, fop.UidFormaPago, fop.VchDescripcion as VchFormaPago, efp.UidEstatusFechaPago, efp.VchDescripcion as VchEstatus,efp.VchColor, pe.VchBanco as Banco, pe.VchCuenta as Cuenta, pe.VchFolio as Folio from FechasPagos fp, PagosColegiaturas pc, FechasColegiaturas fc, Colegiaturas co, Alumnos al, Usuarios us, FormasPagos fop, EstatusFechasPagos efp, PagosEfectivos pe where pe.UidPagoColegiatura = pc.UidPagoColegiatura and fp.UidPagoColegiatura = pc.UidPagoColegiatura and fc.UidFechaColegiatura = fp.UidFechaColegiatura and fc.UidColegiatura = co.UidColegiatura and fp.UidAlumno = al.UidAlumno and pc.UidUsuario = us.UidUsuario and fop.UidFormaPago = fp.UidFormaPago and pc.UidEstatusPagoColegiatura = '51B85D66-866B-4BC2-B08F-FECE1A994053' and efp.UidEstatusFechaPago = fp.UidEstatusFechaPago and us.UidUsuario = '" + UidUsuario + "' union select fc.UidFechaColegiatura, pc.UidPagoColegiatura, pc.IntFolio, co.VchIdentificador, al.UidAlumno, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, fc.IntNum, co.IntCantPagos, pc.DtFHPago, fp.DcmImporteSaldado, fp.DcmImporteCole, fp.DcmImportePagado, fp.DcmImporteNuevo, us.UidUsuario, us.VchNombre as UsNombre, us.VchApePaterno as UsPaterno, Us.VchApeMaterno as UsMaterno, fop.UidFormaPago, fop.VchDescripcion as VchFormaPago, efp.UidEstatusFechaPago, efp.VchDescripcion as VchEstatus,efp.VchColor, ba.VchDescripcion as Banco, pm.VchCuenta as Cuenta, pm.VchFolio as Folio from FechasPagos fp, PagosColegiaturas pc, FechasColegiaturas fc, Colegiaturas co, Alumnos al, Usuarios us, FormasPagos fop, EstatusFechasPagos efp, PagosManuales pm, Bancos ba where ba.UidBanco = pm.UidBanco and pm.UidPagoColegiatura = pc.UidPagoColegiatura and fp.UidPagoColegiatura = pc.UidPagoColegiatura and fc.UidFechaColegiatura = fp.UidFechaColegiatura and fc.UidColegiatura = co.UidColegiatura and fp.UidAlumno = al.UidAlumno and pc.UidUsuario = us.UidUsuario and fop.UidFormaPago = fp.UidFormaPago and pc.UidEstatusPagoColegiatura = '51B85D66-866B-4BC2-B08F-FECE1A994053' and efp.UidEstatusFechaPago = fp.UidEstatusFechaPago and us.UidUsuario = '" + UidUsuario + "' union select fc.UidFechaColegiatura, pc.UidPagoColegiatura, pc.IntFolio, co.VchIdentificador, al.UidAlumno, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, fc.IntNum, co.IntCantPagos, pc.DtFHPago, fp.DcmImporteSaldado, fp.DcmImporteCole, fp.DcmImportePagado, fp.DcmImporteNuevo, us.UidUsuario, us.VchNombre as UsNombre, us.VchApePaterno as UsPaterno, Us.VchApeMaterno as UsMaterno, fop.UidFormaPago, fop.VchDescripcion as VchFormaPago, efp.UidEstatusFechaPago, efp.VchDescripcion as VchEstatus,efp.VchColor, pt.VchTipoDeTarjeta as Banco, pt.cc_number as Cuenta, pt.FolioPago as Folio from FechasPagos fp, PagosColegiaturas pc, FechasColegiaturas fc, Colegiaturas co, Alumnos al, Usuarios us, FormasPagos fop, EstatusFechasPagos efp, LigasUrls lu, PagosTarjeta pt where pt.IdReferencia = lu.IdReferencia and lu.UidPagoColegiatura = pc.UidPagoColegiatura and fp.UidPagoColegiatura = pc.UidPagoColegiatura and fc.UidFechaColegiatura = fp.UidFechaColegiatura and fc.UidColegiatura = co.UidColegiatura and fp.UidAlumno = al.UidAlumno and pc.UidUsuario = us.UidUsuario and fop.UidFormaPago = fp.UidFormaPago and pc.UidEstatusPagoColegiatura = '51B85D66-866B-4BC2-B08F-FECE1A994053' and efp.UidEstatusFechaPago = fp.UidEstatusFechaPago and us.UidUsuario = '" + UidUsuario + "' union select fc.UidFechaColegiatura, pc.UidPagoColegiatura, pc.IntFolio, co.VchIdentificador, al.UidAlumno, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, fc.IntNum, co.IntCantPagos, pc.DtFHPago, fp.DcmImporteSaldado, fp.DcmImporteCole, fp.DcmImportePagado, fp.DcmImporteNuevo, us.UidUsuario, us.VchNombre as UsNombre, us.VchApePaterno as UsPaterno, Us.VchApeMaterno as UsMaterno, fop.UidFormaPago, fop.VchDescripcion as VchFormaPago, efp.UidEstatusFechaPago, efp.VchDescripcion as VchEstatus,efp.VchColor, pcp.VchBanco as Banco, pcp.VchCuenta as Cuenta, pcp.VchTransaccion as Folio from FechasPagos fp, PagosColegiaturas pc, FechasColegiaturas fc, Colegiaturas co, Alumnos al, Usuarios us, FormasPagos fop, EstatusFechasPagos efp, ReferenciasClubPago rcp, PagosClubPago pcp where (pcp.UidPagoEstatus = '9F512165-96A6-407F-925A-A27C2149F3B9' or pcp.UidPagoEstatus = 'A90B996E-A78E-44B2-AB9A-37B961D4FB27') and pcp.IdReferencia = rcp.IdReferencia and rcp.UidPagoColegiatura = pc.UidPagoColegiatura and fp.UidPagoColegiatura = pc.UidPagoColegiatura and fc.UidFechaColegiatura = fp.UidFechaColegiatura and fc.UidColegiatura = co.UidColegiatura and fp.UidAlumno = al.UidAlumno and pc.UidUsuario = us.UidUsuario and fop.UidFormaPago = fp.UidFormaPago and pc.UidEstatusPagoColegiatura = '51B85D66-866B-4BC2-B08F-FECE1A994053' and efp.UidEstatusFechaPago = fp.UidEstatusFechaPago and us.UidUsuario = '" + UidUsuario + "' union select fc.UidFechaColegiatura, pc.UidPagoColegiatura, pc.IntFolio, co.VchIdentificador, al.UidAlumno, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, fc.IntNum, co.IntCantPagos, pc.DtFHPago, fp.DcmImporteSaldado, fp.DcmImporteCole, fp.DcmImportePagado, fp.DcmImporteNuevo, us.UidUsuario, us.VchNombre as UsNombre, us.VchApePaterno as UsPaterno, Us.VchApeMaterno as UsMaterno, fop.UidFormaPago, fop.VchDescripcion as VchFormaPago, efp.UidEstatusFechaPago, efp.VchDescripcion as VchEstatus,efp.VchColor, ptp.cc_type as Banco, ptp.cc_number as Cuenta, ptp.foliocpagos as Folio from FechasPagos fp, PagosColegiaturas pc, FechasColegiaturas fc, Colegiaturas co, Alumnos al, Usuarios us, FormasPagos fop, EstatusFechasPagos efp, LigasUrlsPraga lup, PagosTarjetaPraga ptp where ptp.IdReferencia = lup.IdReferencia and lup.UidPagoColegiatura = pc.UidPagoColegiatura and fp.UidPagoColegiatura = pc.UidPagoColegiatura and fc.UidFechaColegiatura = fp.UidFechaColegiatura and fc.UidColegiatura = co.UidColegiatura and fp.UidAlumno = al.UidAlumno and pc.UidUsuario = us.UidUsuario and fop.UidFormaPago = fp.UidFormaPago and ptp.VchEstatus = 'approved' and pc.UidEstatusPagoColegiatura = '51B85D66-866B-4BC2-B08F-FECE1A994053' and efp.UidEstatusFechaPago = fp.UidEstatusFechaPago and us.UidUsuario = '" + UidUsuario + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                decimal DcmImporteCole = DatosParaImpColeActualizado(Guid.Parse(item["UidAlumno"].ToString()), Guid.Parse(item["UidFechaColegiatura"].ToString()));
                decimal SaldadoSumPagado = decimal.Parse(item["DcmImporteSaldado"].ToString()) + decimal.Parse(item["DcmImportePagado"].ToString());
                decimal DcmImporteNuevo = DcmImporteCole - SaldadoSumPagado;

                lsPagosReporteLigaPadreViewModels.Add(new PagosReporteLigaPadreViewModels()
                {
                    UidPagoColegiatura = Guid.Parse(item["UidPagoColegiatura"].ToString()),
                    IntFolio = int.Parse(item["IntFolio"].ToString()),
                    VchIdentificador = item["VchIdentificador"].ToString(),
                    UidAlumno = Guid.Parse(item["UidAlumno"].ToString()),
                    VchMatricula = item["VchMatricula"].ToString(),
                    VchNombres = item["VchNombres"].ToString(),
                    VchApePaterno = item["VchApePaterno"].ToString(),
                    VchApeMaterno = item["VchApeMaterno"].ToString(),
                    UidFechaColegiatura = Guid.Parse(item["UidFechaColegiatura"].ToString()),
                    VchNum = int.Parse(item["IntNum"].ToString()) + " de " + int.Parse(item["IntCantPagos"].ToString()),
                    DtFHPago = DateTime.Parse(item["DtFHPago"].ToString()),
                    DcmImporteCole = DcmImporteCole,
                    DcmImporteSaldado = decimal.Parse(item["DcmImporteSaldado"].ToString()),
                    DcmImportePagado = decimal.Parse(item["DcmImportePagado"].ToString()),
                    DcmImporteNuevo = DcmImporteNuevo,
                    UidUsuario = Guid.Parse(item["UidUsuario"].ToString()),
                    UsNombre = item["UsNombre"].ToString(),
                    UsPaterno = item["UsPaterno"].ToString(),
                    UsMaterno = item["UsMaterno"].ToString(),
                    UidFormaPago = Guid.Parse(item["UidFormaPago"].ToString()),
                    VchFormaPago = item["VchFormaPago"].ToString(),
                    VchEstatus = item["VchEstatus"].ToString(),
                    VchColor = item["VchColor"].ToString(),

                    VchBanco = item["Banco"].ToString(),
                    VchCuenta = item["Cuenta"].ToString(),
                    VchFolio = item["Folio"].ToString(),

                });
            }

            return lsPagosReporteLigaPadreViewModels.OrderByDescending(x => x.DtFHPago).ToList();
        }
        public List<PagosReporteLigaPadreViewModels> BuscarPagosColeReportePadre(Guid UidUsuario, string Colegiatura, string NumPago, string Matricula, string AlNombre, string AlApePaterno, string AlApeMaterno, string TuNombre, string TuApePaterno, string TuApeMaterno, string Folio, string Cuenta, string Banco, decimal ImporteMayor, decimal ImporteMenor, string RegistroDesde, string RegistroHasta, Guid FormaPago, Guid Estatus)
        {
            List<PagosReporteLigaPadreViewModels> lsPagosReporteLigaPadreViewModels = new List<PagosReporteLigaPadreViewModels>();

            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "sp_PagosPadresBuscar";
            try
            {
                if (UidUsuario != Guid.Empty)
                {
                    comando.Parameters.Add("@UidUsuario", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidUsuario"].Value = UidUsuario;
                }

                if (Colegiatura != string.Empty)
                {
                    comando.Parameters.Add("@Colegiatura", SqlDbType.VarChar, 50);
                    comando.Parameters["@Colegiatura"].Value = Colegiatura;
                }
                if (NumPago != string.Empty)
                {
                    comando.Parameters.Add("@NumPago", SqlDbType.Int);
                    comando.Parameters["@NumPago"].Value = int.Parse(NumPago);
                }

                if (Matricula != string.Empty)
                {
                    comando.Parameters.Add("@Matricula", SqlDbType.VarChar, 50);
                    comando.Parameters["@Matricula"].Value = Matricula;
                }
                if (AlNombre != string.Empty)
                {
                    comando.Parameters.Add("@AlNombre", SqlDbType.VarChar, 50);
                    comando.Parameters["@AlNombre"].Value = AlNombre;
                }
                if (AlApePaterno != string.Empty)
                {
                    comando.Parameters.Add("@AlApePaterno", SqlDbType.VarChar, 50);
                    comando.Parameters["@AlApePaterno"].Value = AlApePaterno;
                }
                if (AlApeMaterno != string.Empty)
                {
                    comando.Parameters.Add("@AlApeMaterno", SqlDbType.VarChar, 50);
                    comando.Parameters["@AlApeMaterno"].Value = AlApeMaterno;
                }

                if (TuNombre != string.Empty)
                {
                    comando.Parameters.Add("@TuNombre", SqlDbType.VarChar, 50);
                    comando.Parameters["@TuNombre"].Value = TuNombre;
                }
                if (TuApePaterno != string.Empty)
                {
                    comando.Parameters.Add("@TuApePaterno", SqlDbType.VarChar, 50);
                    comando.Parameters["@TuApePaterno"].Value = TuApePaterno;
                }
                if (TuApeMaterno != string.Empty)
                {
                    comando.Parameters.Add("@TuApeMaterno", SqlDbType.VarChar, 50);
                    comando.Parameters["@TuApeMaterno"].Value = TuApeMaterno;
                }

                if (Folio != string.Empty)
                {
                    comando.Parameters.Add("@Folio", SqlDbType.VarChar, 100);
                    comando.Parameters["@Folio"].Value = Folio;
                }
                if (Cuenta != string.Empty)
                {
                    comando.Parameters.Add("@Cuenta", SqlDbType.VarChar, 5);
                    comando.Parameters["@Cuenta"].Value = Cuenta;
                }
                if (Banco != string.Empty)
                {
                    comando.Parameters.Add("@Banco", SqlDbType.VarChar, 50);
                    comando.Parameters["@Banco"].Value = Banco;
                }
                if (RegistroDesde != string.Empty)
                {
                    comando.Parameters.Add("@RegistroDesde", SqlDbType.DateTime);
                    comando.Parameters["@RegistroDesde"].Value = RegistroDesde;
                }
                if (RegistroHasta != string.Empty)
                {
                    comando.Parameters.Add("@RegistroHasta", SqlDbType.Date);
                    comando.Parameters["@RegistroHasta"].Value = RegistroHasta;
                }
                if (ImporteMayor != 0)
                {
                    comando.Parameters.Add("@ImporteMayor", SqlDbType.Decimal);
                    comando.Parameters["@ImporteMayor"].Value = ImporteMayor;
                }
                if (ImporteMenor != 0)
                {
                    comando.Parameters.Add("@ImporteMenor", SqlDbType.Decimal);
                    comando.Parameters["@ImporteMenor"].Value = ImporteMenor;
                }
                if (FormaPago != Guid.Empty)
                {
                    comando.Parameters.Add("@FormaPago", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@FormaPago"].Value = FormaPago;
                }
                if (Estatus != Guid.Empty)
                {
                    comando.Parameters.Add("@Estatus", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@Estatus"].Value = Estatus;
                }

                foreach (DataRow item in this.Busquedas(comando).Rows)
                {
                    decimal DcmImporteCole = DatosParaImpColeActualizado(Guid.Parse(item["UidAlumno"].ToString()), Guid.Parse(item["UidFechaColegiatura"].ToString()));
                    decimal SaldadoSumPagado = decimal.Parse(item["DcmImporteSaldado"].ToString()) + decimal.Parse(item["DcmImportePagado"].ToString());
                    decimal DcmImporteNuevo = DcmImporteCole - SaldadoSumPagado;

                    lsPagosReporteLigaPadreViewModels.Add(new PagosReporteLigaPadreViewModels()
                    {
                        UidPagoColegiatura = Guid.Parse(item["UidPagoColegiatura"].ToString()),
                        IntFolio = int.Parse(item["IntFolio"].ToString()),
                        VchIdentificador = item["VchIdentificador"].ToString(),
                        UidAlumno = Guid.Parse(item["UidAlumno"].ToString()),
                        VchMatricula = item["VchMatricula"].ToString(),
                        VchNombres = item["VchNombres"].ToString(),
                        VchApePaterno = item["VchApePaterno"].ToString(),
                        VchApeMaterno = item["VchApeMaterno"].ToString(),
                        UidFechaColegiatura = Guid.Parse(item["UidFechaColegiatura"].ToString()),
                        VchNum = int.Parse(item["IntNum"].ToString()) + " de " + int.Parse(item["IntCantPagos"].ToString()),
                        DtFHPago = DateTime.Parse(item["DtFHPago"].ToString()),
                        DcmImporteCole = DcmImporteCole,
                        DcmImporteSaldado = decimal.Parse(item["DcmImporteSaldado"].ToString()),
                        DcmImportePagado = decimal.Parse(item["DcmImportePagado"].ToString()),
                        DcmImporteNuevo = DcmImporteNuevo,
                        UidUsuario = Guid.Parse(item["UidUsuario"].ToString()),
                        UsNombre = item["UsNombre"].ToString(),
                        UsPaterno = item["UsPaterno"].ToString(),
                        UsMaterno = item["UsMaterno"].ToString(),
                        UidFormaPago = Guid.Parse(item["UidFormaPago"].ToString()),
                        VchFormaPago = item["VchFormaPago"].ToString(),
                        VchEstatus = item["VchEstatus"].ToString(),
                        VchColor = item["VchColor"].ToString(),

                        VchBanco = item["Banco"].ToString(),
                        VchCuenta = item["Cuenta"].ToString(),
                        VchFolio = item["Folio"].ToString(),

                    });
                }

                return lsPagosReporteLigaPadreViewModels.OrderByDescending(x => x.DtFHPago).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Metodos ReporteLigasEscuelas
        public decimal DatosParaImpColeActualizado(Guid UidAlumno, Guid UidFechaColegiatura)
        {
            decimal ImporteCole = 0;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select fca.BitUsarFecha, fca.DtFechaPago, al.BitBeca, al.VchTipoBeca, al.DcmBeca, co.*, fc.UidFechaColegiatura, fc.IntNum, fc.DtFHInicio as fcInicio, fc.DtFHLimite as fcLimite, fc.DtFHVencimiento as fcVencimiento, fc.DtFHFinPeriodo as fcFinPeriodo, efc.UidEstatusFechaColegiatura, efc.VchDescripcion as EstatusFechas, efc.VchColor, pe.VchDescripcion as Periodicidad, (select SUM(fepa.DcmImportePagado) from FechasColegiaturas feco, FechasPagos fepa, PagosColegiaturas paco where feco.UidFechaColegiatura = fepa.UidFechaColegiatura and fepa.UidPagoColegiatura = paco.UidPagoColegiatura and fepa.UidFechaColegiatura = fc.UidFechaColegiatura and fepa.UidAlumno = al.UidAlumno and (fepa.UidEstatusFechaPago = '8720B2B9-5712-4E75-A981-932887AACDC9' or fepa.UidEstatusFechaPago = 'F25E4AAB-6044-46E9-A575-98DCBCCF7604')) ImpPagado from clientes cl, Colegiaturas co, FechasColegiaturas fc, Alumnos al, Usuarios us, UsuariosAlumnos ua, EstatusFechasColegiaturas efc, Periodicidades pe, FechasColegiaturasAlumnos fca where fca.UidEstatusFechaColegiatura = efc.UidEstatusFechaColegiatura and fca.UidAlumno = al.UidAlumno and fca.UidFechaColegiatura = fc.UidFechaColegiatura and  pe.UidPeriodicidad = co.UidPeriodicidad and co.UidEstatus = '65E46BC9-1864-4145-AD1A-70F5B5F69739' and cl.UidCliente = co.UidCliente and co.UidColegiatura = fc.UidColegiatura and ua.UidUsuario = us.UidUsuario and ua.UidAlumno = al.UidAlumno and al.UidAlumno = '" + UidAlumno + "' and fc.UidFechaColegiatura = '" + UidFechaColegiatura + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                bool blDatoFecha = false;
                string FHLimite = "NO TIENE";

                if (!string.IsNullOrEmpty(item["fcLimite"].ToString()))
                {
                    FHLimite = DateTime.Parse(item["fcLimite"].ToString()).ToString("dd/MM/yyyy");
                }
                if (bool.Parse(item["BitUsarFecha"].ToString()))
                {
                    if (!string.IsNullOrEmpty(item["DtFechaPago"].ToString()))
                    {
                        blDatoFecha = true;
                    }
                }

                ImporteCole = Calculo(decimal.Parse(item["DcmImporte"].ToString()), bool.Parse(item["BitRecargo"].ToString()), item["VchTipoRecargo"].ToString(), decimal.Parse(item["DcmRecargo"].ToString()), FHLimite, bool.Parse(item["BitRecargoPeriodo"].ToString()), item["VchTipoRecargoPeriodo"].ToString(), decimal.Parse(item["DcmRecargoPeriodo"].ToString()), item["Periodicidad"].ToString(), bool.Parse(item["BitBeca"].ToString()), item["VchTipoBeca"].ToString(), decimal.Parse(item["DcmBeca"].ToString()), DateTime.Parse(item["fcFinPeriodo"].ToString()), blDatoFecha, item["DtFechaPago"].ToString());

            }

            return ImporteCole;
        }
        public List<PagosReporteLigaEscuelaViewModels> CargarPagosColeReporteEscuela(Guid UidCliente)
        {
            List<PagosReporteLigaEscuelaViewModels> lsPagosReporteLigaEscuelaViewModels = new List<PagosReporteLigaEscuelaViewModels>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            //No tiene ClubPago query.CommandText = "select fc.UidFechaColegiatura, pc.UidPagoColegiatura, pc.IntFolio, co.VchIdentificador, al.UidAlumno, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, fc.IntNum, co.IntCantPagos, pc.DtFHPago, fp.DcmImporteCole, fp.DcmImportePagado, fp.DcmImporteNuevo, us.UidUsuario, us.VchNombre as UsNombre, us.VchApePaterno as UsPaterno, Us.VchApeMaterno as UsMaterno, fop.UidFormaPago, fop.VchDescripcion as VchFormaPago, efp.UidEstatusFechaPago, efp.VchDescripcion as VchEstatus,efp.VchColor, pe.VchBanco as Banco, pe.VchCuenta as Cuenta, pe.VchFolio as Folio from FechasPagos fp, PagosColegiaturas pc, FechasColegiaturas fc, Colegiaturas co, Alumnos al, Usuarios us, FormasPagos fop, EstatusFechasPagos efp, PagosEfectivos pe where pe.UidPagoColegiatura = pc.UidPagoColegiatura and fp.UidPagoColegiatura = pc.UidPagoColegiatura and fc.UidFechaColegiatura = fp.UidFechaColegiatura and fc.UidColegiatura = co.UidColegiatura and fp.UidAlumno = al.UidAlumno and pc.UidUsuario = us.UidUsuario and fop.UidFormaPago = fp.UidFormaPago and pc.UidEstatusPagoColegiatura = '51B85D66-866B-4BC2-B08F-FECE1A994053' and efp.UidEstatusFechaPago = fp.UidEstatusFechaPago and co.UidCliente = '" + UidCliente + "' union select fc.UidFechaColegiatura, pc.UidPagoColegiatura, pc.IntFolio, co.VchIdentificador, al.UidAlumno, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, fc.IntNum, co.IntCantPagos, pc.DtFHPago, fp.DcmImporteCole, fp.DcmImportePagado, fp.DcmImporteNuevo, us.UidUsuario, us.VchNombre as UsNombre, us.VchApePaterno as UsPaterno, Us.VchApeMaterno as UsMaterno, fop.UidFormaPago, fop.VchDescripcion as VchFormaPago, efp.UidEstatusFechaPago, efp.VchDescripcion as VchEstatus,efp.VchColor, ba.VchDescripcion as Banco, pm.VchCuenta as Cuenta, pm.VchFolio as Folio from FechasPagos fp, PagosColegiaturas pc, FechasColegiaturas fc, Colegiaturas co, Alumnos al, Usuarios us, FormasPagos fop, EstatusFechasPagos efp, PagosManuales pm, Bancos ba where ba.UidBanco = pm.UidBanco and pm.UidPagoColegiatura = pc.UidPagoColegiatura and fp.UidPagoColegiatura = pc.UidPagoColegiatura and fc.UidFechaColegiatura = fp.UidFechaColegiatura and fc.UidColegiatura = co.UidColegiatura and fp.UidAlumno = al.UidAlumno and pc.UidUsuario = us.UidUsuario and fop.UidFormaPago = fp.UidFormaPago and pc.UidEstatusPagoColegiatura = '51B85D66-866B-4BC2-B08F-FECE1A994053' and efp.UidEstatusFechaPago = fp.UidEstatusFechaPago and co.UidCliente = '" + UidCliente + "' union select fc.UidFechaColegiatura, pc.UidPagoColegiatura, pc.IntFolio, co.VchIdentificador, al.UidAlumno, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, fc.IntNum, co.IntCantPagos, pc.DtFHPago, fp.DcmImporteCole, fp.DcmImportePagado, fp.DcmImporteNuevo, us.UidUsuario, us.VchNombre as UsNombre, us.VchApePaterno as UsPaterno, Us.VchApeMaterno as UsMaterno, fop.UidFormaPago, fop.VchDescripcion as VchFormaPago, efp.UidEstatusFechaPago, efp.VchDescripcion as VchEstatus,efp.VchColor, pt.VchTipoDeTarjeta as Banco, pt.cc_number as Cuenta, pt.FolioPago as Folio from FechasPagos fp, PagosColegiaturas pc, FechasColegiaturas fc, Colegiaturas co, Alumnos al, Usuarios us, FormasPagos fop, EstatusFechasPagos efp, LigasUrls lu, PagosTarjeta pt where pt.IdReferencia = lu.IdReferencia and lu.UidPagoColegiatura = pc.UidPagoColegiatura and fp.UidPagoColegiatura = pc.UidPagoColegiatura and fc.UidFechaColegiatura = fp.UidFechaColegiatura and fc.UidColegiatura = co.UidColegiatura and fp.UidAlumno = al.UidAlumno and pc.UidUsuario = us.UidUsuario and fop.UidFormaPago = fp.UidFormaPago and pc.UidEstatusPagoColegiatura = '51B85D66-866B-4BC2-B08F-FECE1A994053' and efp.UidEstatusFechaPago = fp.UidEstatusFechaPago and co.UidCliente = '" + UidCliente + "'";
            //No tiene Praga query.CommandText = "select fc.UidFechaColegiatura, pc.UidPagoColegiatura, pc.IntFolio, co.VchIdentificador, al.UidAlumno, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, fc.IntNum, co.IntCantPagos, pc.DtFHPago, fp.DcmImporteSaldado, fp.DcmImporteCole, fp.DcmImportePagado, fp.DcmImporteNuevo, us.UidUsuario, us.VchNombre as UsNombre, us.VchApePaterno as UsPaterno, Us.VchApeMaterno as UsMaterno, fop.UidFormaPago, fop.VchDescripcion as VchFormaPago, efp.UidEstatusFechaPago, efp.VchDescripcion as VchEstatus,efp.VchColor, pe.VchBanco as Banco, pe.VchCuenta as Cuenta, pe.VchFolio as Folio from FechasPagos fp, PagosColegiaturas pc, FechasColegiaturas fc, Colegiaturas co, Alumnos al, Usuarios us, FormasPagos fop, EstatusFechasPagos efp, PagosEfectivos pe where pe.UidPagoColegiatura = pc.UidPagoColegiatura and fp.UidPagoColegiatura = pc.UidPagoColegiatura and fc.UidFechaColegiatura = fp.UidFechaColegiatura and fc.UidColegiatura = co.UidColegiatura and fp.UidAlumno = al.UidAlumno and pc.UidUsuario = us.UidUsuario and fop.UidFormaPago = fp.UidFormaPago and pc.UidEstatusPagoColegiatura = '51B85D66-866B-4BC2-B08F-FECE1A994053' and efp.UidEstatusFechaPago = fp.UidEstatusFechaPago and co.UidCliente = '" + UidCliente + "' union select fc.UidFechaColegiatura, pc.UidPagoColegiatura, pc.IntFolio, co.VchIdentificador, al.UidAlumno, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, fc.IntNum, co.IntCantPagos, pc.DtFHPago, fp.DcmImporteSaldado, fp.DcmImporteCole, fp.DcmImportePagado, fp.DcmImporteNuevo, us.UidUsuario, us.VchNombre as UsNombre, us.VchApePaterno as UsPaterno, Us.VchApeMaterno as UsMaterno, fop.UidFormaPago, fop.VchDescripcion as VchFormaPago, efp.UidEstatusFechaPago, efp.VchDescripcion as VchEstatus,efp.VchColor, ba.VchDescripcion as Banco, pm.VchCuenta as Cuenta, pm.VchFolio as Folio from FechasPagos fp, PagosColegiaturas pc, FechasColegiaturas fc, Colegiaturas co, Alumnos al, Usuarios us, FormasPagos fop, EstatusFechasPagos efp, PagosManuales pm, Bancos ba where ba.UidBanco = pm.UidBanco and pm.UidPagoColegiatura = pc.UidPagoColegiatura and fp.UidPagoColegiatura = pc.UidPagoColegiatura and fc.UidFechaColegiatura = fp.UidFechaColegiatura and fc.UidColegiatura = co.UidColegiatura and fp.UidAlumno = al.UidAlumno and pc.UidUsuario = us.UidUsuario and fop.UidFormaPago = fp.UidFormaPago and pc.UidEstatusPagoColegiatura = '51B85D66-866B-4BC2-B08F-FECE1A994053' and efp.UidEstatusFechaPago = fp.UidEstatusFechaPago and co.UidCliente = '" + UidCliente + "' union select fc.UidFechaColegiatura, pc.UidPagoColegiatura, pc.IntFolio, co.VchIdentificador, al.UidAlumno, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, fc.IntNum, co.IntCantPagos, pc.DtFHPago, fp.DcmImporteSaldado, fp.DcmImporteCole, fp.DcmImportePagado, fp.DcmImporteNuevo, us.UidUsuario, us.VchNombre as UsNombre, us.VchApePaterno as UsPaterno, Us.VchApeMaterno as UsMaterno, fop.UidFormaPago, fop.VchDescripcion as VchFormaPago, efp.UidEstatusFechaPago, efp.VchDescripcion as VchEstatus,efp.VchColor, pt.VchTipoDeTarjeta as Banco, pt.cc_number as Cuenta, pt.FolioPago as Folio from FechasPagos fp, PagosColegiaturas pc, FechasColegiaturas fc, Colegiaturas co, Alumnos al, Usuarios us, FormasPagos fop, EstatusFechasPagos efp, LigasUrls lu, PagosTarjeta pt where pt.IdReferencia = lu.IdReferencia and lu.UidPagoColegiatura = pc.UidPagoColegiatura and fp.UidPagoColegiatura = pc.UidPagoColegiatura and fc.UidFechaColegiatura = fp.UidFechaColegiatura and fc.UidColegiatura = co.UidColegiatura and fp.UidAlumno = al.UidAlumno and pc.UidUsuario = us.UidUsuario and fop.UidFormaPago = fp.UidFormaPago and pc.UidEstatusPagoColegiatura = '51B85D66-866B-4BC2-B08F-FECE1A994053' and efp.UidEstatusFechaPago = fp.UidEstatusFechaPago and co.UidCliente = '" + UidCliente + "' union select fc.UidFechaColegiatura, pc.UidPagoColegiatura, pc.IntFolio, co.VchIdentificador, al.UidAlumno, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, fc.IntNum, co.IntCantPagos, pc.DtFHPago, fp.DcmImporteSaldado, fp.DcmImporteCole, fp.DcmImportePagado, fp.DcmImporteNuevo, us.UidUsuario, us.VchNombre as UsNombre, us.VchApePaterno as UsPaterno, Us.VchApeMaterno as UsMaterno, fop.UidFormaPago, fop.VchDescripcion as VchFormaPago, efp.UidEstatusFechaPago, efp.VchDescripcion as VchEstatus,efp.VchColor, pcp.VchBanco as Banco, pcp.VchCuenta as Cuenta, pcp.VchTransaccion as Folio from FechasPagos fp, PagosColegiaturas pc, FechasColegiaturas fc, Colegiaturas co, Alumnos al, Usuarios us, FormasPagos fop, EstatusFechasPagos efp, ReferenciasClubPago rcp, PagosClubPago pcp where (pcp.UidPagoEstatus = '9F512165-96A6-407F-925A-A27C2149F3B9' or pcp.UidPagoEstatus = 'A90B996E-A78E-44B2-AB9A-37B961D4FB27') and pcp.IdReferencia = rcp.IdReferencia and rcp.UidPagoColegiatura = pc.UidPagoColegiatura and fp.UidPagoColegiatura = pc.UidPagoColegiatura and fc.UidFechaColegiatura = fp.UidFechaColegiatura and fc.UidColegiatura = co.UidColegiatura and fp.UidAlumno = al.UidAlumno and pc.UidUsuario = us.UidUsuario and fop.UidFormaPago = fp.UidFormaPago and pc.UidEstatusPagoColegiatura = '51B85D66-866B-4BC2-B08F-FECE1A994053' and efp.UidEstatusFechaPago = fp.UidEstatusFechaPago and co.UidCliente = '" + UidCliente + "'";
            query.CommandText = "select fc.UidFechaColegiatura, pc.UidPagoColegiatura, pc.IntFolio, co.VchIdentificador, al.UidAlumno, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, fc.IntNum, co.IntCantPagos, pc.DtFHPago, fp.DcmImporteSaldado, fp.DcmImporteCole, fp.DcmImportePagado, fp.DcmImporteNuevo, us.UidUsuario, us.VchNombre as UsNombre, us.VchApePaterno as UsPaterno, Us.VchApeMaterno as UsMaterno, fop.UidFormaPago, fop.VchDescripcion as VchFormaPago, efp.UidEstatusFechaPago, efp.VchDescripcion as VchEstatus,efp.VchColor, pe.VchBanco as Banco, pe.VchCuenta as Cuenta, pe.VchFolio as Folio from FechasPagos fp, PagosColegiaturas pc, FechasColegiaturas fc, Colegiaturas co, Alumnos al, Usuarios us, FormasPagos fop, EstatusFechasPagos efp, PagosEfectivos pe where pe.UidPagoColegiatura = pc.UidPagoColegiatura and fp.UidPagoColegiatura = pc.UidPagoColegiatura and fc.UidFechaColegiatura = fp.UidFechaColegiatura and fc.UidColegiatura = co.UidColegiatura and fp.UidAlumno = al.UidAlumno and pc.UidUsuario = us.UidUsuario and fop.UidFormaPago = fp.UidFormaPago and pc.UidEstatusPagoColegiatura = '51B85D66-866B-4BC2-B08F-FECE1A994053' and efp.UidEstatusFechaPago = fp.UidEstatusFechaPago and co.UidCliente = '" + UidCliente + "' union select fc.UidFechaColegiatura, pc.UidPagoColegiatura, pc.IntFolio, co.VchIdentificador, al.UidAlumno, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, fc.IntNum, co.IntCantPagos, pc.DtFHPago, fp.DcmImporteSaldado, fp.DcmImporteCole, fp.DcmImportePagado, fp.DcmImporteNuevo, us.UidUsuario, us.VchNombre as UsNombre, us.VchApePaterno as UsPaterno, Us.VchApeMaterno as UsMaterno, fop.UidFormaPago, fop.VchDescripcion as VchFormaPago, efp.UidEstatusFechaPago, efp.VchDescripcion as VchEstatus,efp.VchColor, ba.VchDescripcion as Banco, pm.VchCuenta as Cuenta, pm.VchFolio as Folio from FechasPagos fp, PagosColegiaturas pc, FechasColegiaturas fc, Colegiaturas co, Alumnos al, Usuarios us, FormasPagos fop, EstatusFechasPagos efp, PagosManuales pm, Bancos ba where ba.UidBanco = pm.UidBanco and pm.UidPagoColegiatura = pc.UidPagoColegiatura and fp.UidPagoColegiatura = pc.UidPagoColegiatura and fc.UidFechaColegiatura = fp.UidFechaColegiatura and fc.UidColegiatura = co.UidColegiatura and fp.UidAlumno = al.UidAlumno and pc.UidUsuario = us.UidUsuario and fop.UidFormaPago = fp.UidFormaPago and pc.UidEstatusPagoColegiatura = '51B85D66-866B-4BC2-B08F-FECE1A994053' and efp.UidEstatusFechaPago = fp.UidEstatusFechaPago and co.UidCliente = '" + UidCliente + "' union select fc.UidFechaColegiatura, pc.UidPagoColegiatura, pc.IntFolio, co.VchIdentificador, al.UidAlumno, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, fc.IntNum, co.IntCantPagos, pc.DtFHPago, fp.DcmImporteSaldado, fp.DcmImporteCole, fp.DcmImportePagado, fp.DcmImporteNuevo, us.UidUsuario, us.VchNombre as UsNombre, us.VchApePaterno as UsPaterno, Us.VchApeMaterno as UsMaterno, fop.UidFormaPago, fop.VchDescripcion as VchFormaPago, efp.UidEstatusFechaPago, efp.VchDescripcion as VchEstatus,efp.VchColor, pt.VchTipoDeTarjeta as Banco, pt.cc_number as Cuenta, pt.FolioPago as Folio from FechasPagos fp, PagosColegiaturas pc, FechasColegiaturas fc, Colegiaturas co, Alumnos al, Usuarios us, FormasPagos fop, EstatusFechasPagos efp, LigasUrls lu, PagosTarjeta pt where pt.IdReferencia = lu.IdReferencia and lu.UidPagoColegiatura = pc.UidPagoColegiatura and fp.UidPagoColegiatura = pc.UidPagoColegiatura and fc.UidFechaColegiatura = fp.UidFechaColegiatura and fc.UidColegiatura = co.UidColegiatura and fp.UidAlumno = al.UidAlumno and pc.UidUsuario = us.UidUsuario and fop.UidFormaPago = fp.UidFormaPago and pc.UidEstatusPagoColegiatura = '51B85D66-866B-4BC2-B08F-FECE1A994053' and efp.UidEstatusFechaPago = fp.UidEstatusFechaPago and co.UidCliente = '" + UidCliente + "' union select fc.UidFechaColegiatura, pc.UidPagoColegiatura, pc.IntFolio, co.VchIdentificador, al.UidAlumno, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, fc.IntNum, co.IntCantPagos, pc.DtFHPago, fp.DcmImporteSaldado, fp.DcmImporteCole, fp.DcmImportePagado, fp.DcmImporteNuevo, us.UidUsuario, us.VchNombre as UsNombre, us.VchApePaterno as UsPaterno, Us.VchApeMaterno as UsMaterno, fop.UidFormaPago, fop.VchDescripcion as VchFormaPago, efp.UidEstatusFechaPago, efp.VchDescripcion as VchEstatus,efp.VchColor, pcp.VchBanco as Banco, pcp.VchCuenta as Cuenta, pcp.VchTransaccion as Folio from FechasPagos fp, PagosColegiaturas pc, FechasColegiaturas fc, Colegiaturas co, Alumnos al, Usuarios us, FormasPagos fop, EstatusFechasPagos efp, ReferenciasClubPago rcp, PagosClubPago pcp where (pcp.UidPagoEstatus = '9F512165-96A6-407F-925A-A27C2149F3B9' or pcp.UidPagoEstatus = 'A90B996E-A78E-44B2-AB9A-37B961D4FB27') and pcp.IdReferencia = rcp.IdReferencia and rcp.UidPagoColegiatura = pc.UidPagoColegiatura and fp.UidPagoColegiatura = pc.UidPagoColegiatura and fc.UidFechaColegiatura = fp.UidFechaColegiatura and fc.UidColegiatura = co.UidColegiatura and fp.UidAlumno = al.UidAlumno and pc.UidUsuario = us.UidUsuario and fop.UidFormaPago = fp.UidFormaPago and pc.UidEstatusPagoColegiatura = '51B85D66-866B-4BC2-B08F-FECE1A994053' and efp.UidEstatusFechaPago = fp.UidEstatusFechaPago and co.UidCliente = '" + UidCliente + "' union select fc.UidFechaColegiatura, pc.UidPagoColegiatura, pc.IntFolio, co.VchIdentificador, al.UidAlumno, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, fc.IntNum, co.IntCantPagos, pc.DtFHPago, fp.DcmImporteSaldado, fp.DcmImporteCole, fp.DcmImportePagado, fp.DcmImporteNuevo, us.UidUsuario, us.VchNombre as UsNombre, us.VchApePaterno as UsPaterno, Us.VchApeMaterno as UsMaterno, fop.UidFormaPago, fop.VchDescripcion as VchFormaPago, efp.UidEstatusFechaPago, efp.VchDescripcion as VchEstatus,efp.VchColor, ptp.cc_type as Banco, ptp.cc_number as Cuenta, ptp.foliocpagos as Folio from FechasPagos fp, PagosColegiaturas pc, FechasColegiaturas fc, Colegiaturas co, Alumnos al, Usuarios us, FormasPagos fop, EstatusFechasPagos efp, LigasUrlsPraga lup, PagosTarjetaPraga ptp where ptp.IdReferencia = lup.IdReferencia and lup.UidPagoColegiatura = pc.UidPagoColegiatura and fp.UidPagoColegiatura = pc.UidPagoColegiatura and fc.UidFechaColegiatura = fp.UidFechaColegiatura and fc.UidColegiatura = co.UidColegiatura and fp.UidAlumno = al.UidAlumno and pc.UidUsuario = us.UidUsuario and fop.UidFormaPago = fp.UidFormaPago and ptp.VchEstatus = 'approved' and pc.UidEstatusPagoColegiatura = '51B85D66-866B-4BC2-B08F-FECE1A994053' and efp.UidEstatusFechaPago = fp.UidEstatusFechaPago and co.UidCliente = '" + UidCliente + "' ";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                bool blAprobarPago = true;
                bool blRechazarPago = true;

                bool blConfirmarPago = false;
                bool blEditarPago = true;

                if (Guid.Parse(item["UidEstatusFechaPago"].ToString()) == Guid.Parse("8720B2B9-5712-4E75-A981-932887AACDC9"))
                {
                    blAprobarPago = false;
                    blRechazarPago = true;
                }

                if (Guid.Parse(item["UidEstatusFechaPago"].ToString()) == Guid.Parse("77DB3F13-7EC8-4CE1-A3DB-E5C96D14A581"))
                {
                    blAprobarPago = true;
                    blRechazarPago = false;
                }

                if (Guid.Parse(item["UidEstatusFechaPago"].ToString()) == Guid.Parse("F25E4AAB-6044-46E9-A575-98DCBCCF7604"))
                {
                    blConfirmarPago = true;
                    blEditarPago = false;
                }

                if (Guid.Parse(item["UidFormaPago"].ToString()) == Guid.Parse("31BE9A23-73EE-4F44-AF6C-6C0648DCEBF7"))
                {
                    blAprobarPago = false;
                    blRechazarPago = false;

                    blConfirmarPago = false;
                    blEditarPago = false;
                }

                if (Guid.Parse(item["UidFormaPago"].ToString()) == Guid.Parse("6BE13FFE-E567-4D4D-9CBC-37DA30EC23A5"))
                {
                    blAprobarPago = false;
                    blRechazarPago = false;

                    blConfirmarPago = false;
                    blEditarPago = false;
                }

                if (Guid.Parse(item["UidFormaPago"].ToString()) == Guid.Parse("310F3557-682A-4144-9433-E47E48805D28"))
                {
                    blAprobarPago = false;
                    blRechazarPago = false;

                    blConfirmarPago = false;
                    blEditarPago = false;
                }

                if (Guid.Parse(item["UidFormaPago"].ToString()) == Guid.Parse("D92A2C64-C797-4C96-AD18-C2A433081F37") || Guid.Parse(item["UidFormaPago"].ToString()) == Guid.Parse("3359D33E-C879-4A8B-96D3-C6A211AF014F"))
                {
                    blAprobarPago = false;
                    blRechazarPago = false;

                    blConfirmarPago = false;
                    blEditarPago = false;
                }

                int CatDias = CantDias(DateTime.Parse(item["DtFHPago"].ToString()));

                if (CatDias > 30)
                {
                    if (Guid.Parse("F25E4AAB-6044-46E9-A575-98DCBCCF7604") != Guid.Parse(item["UidEstatusFechaPago"].ToString()))
                    {
                        blAprobarPago = false;
                        blRechazarPago = false;

                        blConfirmarPago = false;
                        blEditarPago = false;
                    }
                }

                decimal DcmImporteCole = DatosParaImpColeActualizado(Guid.Parse(item["UidAlumno"].ToString()), Guid.Parse(item["UidFechaColegiatura"].ToString()));
                decimal SaldadoSumPagado = decimal.Parse(item["DcmImporteSaldado"].ToString()) + decimal.Parse(item["DcmImportePagado"].ToString());
                decimal DcmImporteNuevo = DcmImporteCole - SaldadoSumPagado;

                lsPagosReporteLigaEscuelaViewModels.Add(new PagosReporteLigaEscuelaViewModels()
                {
                    UidPagoColegiatura = Guid.Parse(item["UidPagoColegiatura"].ToString()),
                    IntFolio = int.Parse(item["IntFolio"].ToString()),
                    VchIdentificador = item["VchIdentificador"].ToString(),
                    UidAlumno = Guid.Parse(item["UidAlumno"].ToString()),
                    VchMatricula = item["VchMatricula"].ToString(),
                    VchNombres = item["VchNombres"].ToString(),
                    VchApePaterno = item["VchApePaterno"].ToString(),
                    VchApeMaterno = item["VchApeMaterno"].ToString(),
                    UidFechaColegiatura = Guid.Parse(item["UidFechaColegiatura"].ToString()),
                    VchNum = int.Parse(item["IntNum"].ToString()) + " de " + int.Parse(item["IntCantPagos"].ToString()),
                    DtFHPago = DateTime.Parse(item["DtFHPago"].ToString()),
                    DcmImporteCole = DcmImporteCole,
                    DcmImporteSaldado = decimal.Parse(item["DcmImporteSaldado"].ToString()),
                    DcmImportePagado = decimal.Parse(item["DcmImportePagado"].ToString()),
                    DcmImporteNuevo = DcmImporteNuevo,
                    UidUsuario = Guid.Parse(item["UidUsuario"].ToString()),
                    UsNombre = item["UsNombre"].ToString(),
                    UsPaterno = item["UsPaterno"].ToString(),
                    UsMaterno = item["UsMaterno"].ToString(),
                    UidFormaPago = Guid.Parse(item["UidFormaPago"].ToString()),
                    VchFormaPago = item["VchFormaPago"].ToString(),
                    blAprobarPago = blAprobarPago,
                    blRechazarPago = blRechazarPago,
                    blConfirmarPago = blConfirmarPago,
                    blEditarPago = blEditarPago,
                    VchEstatus = item["VchEstatus"].ToString(),
                    VchColor = item["VchColor"].ToString(),

                    VchBanco = item["Banco"].ToString(),
                    VchCuenta = item["Cuenta"].ToString(),
                    VchFolio = item["Folio"].ToString(),

                });
            }

            return lsPagosReporteLigaEscuelaViewModels.OrderByDescending(x => x.DtFHPago).ToList();
        }
        public decimal ObtenerDatosFechaColegiatura(Guid UidCliente, Guid UidUsuario, Guid UidFechaColegiatura, Guid UidAlumno)
        {
            decimal ImporteColegiatura = 0;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select al.BitBeca, al.VchTipoBeca, al.DcmBeca, co.*, fc.DtFHInicio as fcInicio, fc.DtFHLimite as fcLimite, fc.DtFHVencimiento as fcVencimiento, fc.DtFHFinPeriodo as fcFinPeriodo, efc.VchDescripcion as EstatusFechas, pe.VchDescripcion as Periodicidad, fca.BitUsarFecha, fca.DtFechaPago from Usuarios us, UsuariosAlumnos ua, Alumnos al, FechasColegiaturasAlumnos fca, FechasColegiaturas fc, Colegiaturas co, clientes cl, EstatusFechasColegiaturas efc, Periodicidades pe where us.UidUsuario = ua.UidUsuario and al.UidAlumno = ua.UidAlumno and fca.UidAlumno = al.UidAlumno and fca.UidFechaColegiatura = fc.UidFechaColegiatura and fc.UidColegiatura = co.UidColegiatura and cl.UidCliente = co.UidCliente and efc.UidEstatusFechaColegiatura = fca.UidEstatusFechaColegiatura and pe.UidPeriodicidad = co.UidPeriodicidad and co.UidEstatus = '65E46BC9-1864-4145-AD1A-70F5B5F69739' and cl.UidCliente = '" + UidCliente + "' and us.UidUsuario = '" + UidUsuario + "' and fc.UidFechaColegiatura = '" + UidFechaColegiatura + "' and al.UidAlumno = '" + UidAlumno + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                string FHLimite = "NO TIENE";
                string FHVencimiento = "NO TIENE";
                bool blDatoFecha = false;

                if (!string.IsNullOrEmpty(item["fcLimite"].ToString()))
                {
                    FHLimite = DateTime.Parse(item["fcLimite"].ToString()).ToString("dd/MM/yyyy");
                }
                if (!string.IsNullOrEmpty(item["fcVencimiento"].ToString()))
                {
                    FHVencimiento = DateTime.Parse(item["fcVencimiento"].ToString()).ToString("dd/MM/yyyy");
                }

                if (bool.Parse(item["BitUsarFecha"].ToString()))
                {
                    if (!string.IsNullOrEmpty(item["DtFechaPago"].ToString()))
                    {
                        blDatoFecha = true;
                    }
                }

                ImporteColegiatura = Calculo(decimal.Parse(
                    item["DcmImporte"].ToString()),
                    bool.Parse(item["BitRecargo"].ToString()),
                    item["VchTipoRecargo"].ToString(),
                    decimal.Parse(item["DcmRecargo"].ToString()),
                    FHLimite,
                    bool.Parse(item["BitRecargoPeriodo"].ToString()),
                    item["VchTipoRecargoPeriodo"].ToString(),
                    decimal.Parse(item["DcmRecargoPeriodo"].ToString()),
                    item["Periodicidad"].ToString(),
                    bool.Parse(item["BitBeca"].ToString()),
                    item["VchTipoBeca"].ToString(),
                    decimal.Parse(item["DcmBeca"].ToString()),
                    DateTime.Parse(item["fcFinPeriodo"].ToString()),
                    blDatoFecha,
                    item["DtFechaPago"].ToString()
                    );

            }
            return decimal.Parse(ImporteColegiatura.ToString("N2"));
        }
        public bool ActualizarEstatusFeColegiaturaAlumno(Guid UidFechaColegiatura, Guid UidAlumno, Guid UidEstatus, bool BitUsarFecha)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_FechasColegiaturasAlumnosEstatusActualizar";

                comando.Parameters.Add("@UidFechaColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidFechaColegiatura"].Value = UidFechaColegiatura;

                comando.Parameters.Add("@UidAlumno", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidAlumno"].Value = UidAlumno;

                comando.Parameters.Add("@UidEstatus", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEstatus"].Value = UidEstatus;

                comando.Parameters.Add("@BitUsarFecha", SqlDbType.Bit);
                comando.Parameters["@BitUsarFecha"].Value = BitUsarFecha;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }

        public string ObtenerEstatusColegiaturasRLE(DateTime hoy, Guid UidFechaColegiatura, Guid UidAlumno)
        {
            string valor = string.Empty;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select fc.*, efc.UidEstatusFechaColegiatura, efc.VchDescripcion as EstatusFechaColegiatura from FechasColegiaturasAlumnos fca, FechasColegiaturas fc, EstatusFechasColegiaturas efc where efc.UidEstatusFechaColegiatura = fca.UidEstatusFechaColegiatura and fca.UidFechaColegiatura = fc.UidFechaColegiatura and fca.UidFechaColegiatura = '" + UidFechaColegiatura + "' and fca.UidAlumno = '" + UidAlumno + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                hoy = DateTime.Parse(hoy.ToString("dd/MM/yyyy"));

                Guid EstatusFechaColegiatura = Guid.Parse("76C8793B-4493-44C8-B274-696A61358BDF");
                string strEstatusFechaColegiatura = "VIGENTE";

                //DtFHInicio DtFHLimite  DtFHVencimiento DtFHFinPeriodo

                if (!string.IsNullOrEmpty(item["DtFHLimite"].ToString()))
                {

                    if (hoy > DateTime.Parse(item["DtFHLimite"].ToString()))
                    {
                        strEstatusFechaColegiatura = "RETRASADO";
                        EstatusFechaColegiatura = Guid.Parse("FD1E57E9-1476-482A-A850-501E55298500");
                    }

                    if (!string.IsNullOrEmpty(item["DtFHVencimiento"].ToString()))
                    {
                        if (hoy > DateTime.Parse(item["DtFHVencimiento"].ToString()))
                        {
                            strEstatusFechaColegiatura = "BLOQUEADO";
                            EstatusFechaColegiatura = Guid.Parse("1331D93D-EA53-487F-BF28-E72F5E7D19BF");
                        }
                    }

                    if (!string.IsNullOrEmpty(item["DtFHFinPeriodo"].ToString()))
                    {
                        if (hoy > DateTime.Parse(item["DtFHFinPeriodo"].ToString()))
                        {
                            strEstatusFechaColegiatura = "VENCIDO";
                            EstatusFechaColegiatura = Guid.Parse("DB36D040-9E05-4E7B-83B4-DD4FF0D5AC3C");
                        }
                    }

                }
                else if (!string.IsNullOrEmpty(item["DtFHVencimiento"].ToString()))
                {

                    if (hoy > DateTime.Parse(item["DtFHVencimiento"].ToString()))
                    {
                        strEstatusFechaColegiatura = "BLOQUEADO";
                        EstatusFechaColegiatura = Guid.Parse("1331D93D-EA53-487F-BF28-E72F5E7D19BF");
                    }
                    if (!string.IsNullOrEmpty(item["DtFHFinPeriodo"].ToString()))
                    {
                        if (hoy > DateTime.Parse(item["DtFHFinPeriodo"].ToString()))
                        {
                            strEstatusFechaColegiatura = "VENCIDO";
                            EstatusFechaColegiatura = Guid.Parse("DB36D040-9E05-4E7B-83B4-DD4FF0D5AC3C");
                        }
                    }

                }
                else if (!string.IsNullOrEmpty(item["DtFHFinPeriodo"].ToString()))
                {
                    if (hoy > DateTime.Parse(item["DtFHFinPeriodo"].ToString()))
                    {
                        strEstatusFechaColegiatura = "VENCIDO";
                        EstatusFechaColegiatura = Guid.Parse("DB36D040-9E05-4E7B-83B4-DD4FF0D5AC3C");
                    }
                }

                valor = EstatusFechaColegiatura.ToString();
            }
            return valor;
        }

        public List<PagosReporteLigaEscuelaViewModels> BuscarPagosEscuelas(Guid UidCliente, string Colegiatura, string NumPago, string Matricula, string AlNombre, string AlApePaterno, string AlApeMaterno, string TuNombre, string TuApePaterno, string TuApeMaterno, string Folio, string Cuenta, string Banco, decimal ImporteMayor, decimal ImporteMenor, string RegistroDesde, string RegistroHasta, Guid FormaPago, Guid Estatus)
        {
            List<PagosReporteLigaEscuelaViewModels> lsPagosReporteLigaEscuelaViewModels = new List<PagosReporteLigaEscuelaViewModels>();

            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "sp_PagosEscuelasBuscar";
            try
            {
                if (UidCliente != Guid.Empty)
                {
                    comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidCliente"].Value = UidCliente;
                }

                if (Colegiatura != string.Empty)
                {
                    comando.Parameters.Add("@Colegiatura", SqlDbType.VarChar, 50);
                    comando.Parameters["@Colegiatura"].Value = Colegiatura;
                }
                if (NumPago != string.Empty)
                {
                    comando.Parameters.Add("@NumPago", SqlDbType.Int);
                    comando.Parameters["@NumPago"].Value = int.Parse(NumPago);
                }

                if (Matricula != string.Empty)
                {
                    comando.Parameters.Add("@Matricula", SqlDbType.VarChar, 50);
                    comando.Parameters["@Matricula"].Value = Matricula;
                }
                if (AlNombre != string.Empty)
                {
                    comando.Parameters.Add("@AlNombre", SqlDbType.VarChar, 50);
                    comando.Parameters["@AlNombre"].Value = AlNombre;
                }
                if (AlApePaterno != string.Empty)
                {
                    comando.Parameters.Add("@AlApePaterno", SqlDbType.VarChar, 50);
                    comando.Parameters["@AlApePaterno"].Value = AlApePaterno;
                }
                if (AlApeMaterno != string.Empty)
                {
                    comando.Parameters.Add("@AlApeMaterno", SqlDbType.VarChar, 50);
                    comando.Parameters["@AlApeMaterno"].Value = AlApeMaterno;
                }

                if (TuNombre != string.Empty)
                {
                    comando.Parameters.Add("@TuNombre", SqlDbType.VarChar, 50);
                    comando.Parameters["@TuNombre"].Value = TuNombre;
                }
                if (TuApePaterno != string.Empty)
                {
                    comando.Parameters.Add("@TuApePaterno", SqlDbType.VarChar, 50);
                    comando.Parameters["@TuApePaterno"].Value = TuApePaterno;
                }
                if (TuApeMaterno != string.Empty)
                {
                    comando.Parameters.Add("@TuApeMaterno", SqlDbType.VarChar, 50);
                    comando.Parameters["@TuApeMaterno"].Value = TuApeMaterno;
                }

                if (Folio != string.Empty)
                {
                    comando.Parameters.Add("@Folio", SqlDbType.VarChar, 100);
                    comando.Parameters["@Folio"].Value = Folio;
                }
                if (Cuenta != string.Empty)
                {
                    comando.Parameters.Add("@Cuenta", SqlDbType.VarChar, 5);
                    comando.Parameters["@Cuenta"].Value = Cuenta;
                }
                if (Banco != string.Empty)
                {
                    comando.Parameters.Add("@Banco", SqlDbType.VarChar, 50);
                    comando.Parameters["@Banco"].Value = Banco;
                }
                if (RegistroDesde != string.Empty)
                {
                    comando.Parameters.Add("@RegistroDesde", SqlDbType.DateTime);
                    comando.Parameters["@RegistroDesde"].Value = RegistroDesde;
                }
                if (RegistroHasta != string.Empty)
                {
                    comando.Parameters.Add("@RegistroHasta", SqlDbType.Date);
                    comando.Parameters["@RegistroHasta"].Value = RegistroHasta;
                }
                if (ImporteMayor != 0)
                {
                    comando.Parameters.Add("@ImporteMayor", SqlDbType.Decimal);
                    comando.Parameters["@ImporteMayor"].Value = ImporteMayor;
                }
                if (ImporteMenor != 0)
                {
                    comando.Parameters.Add("@ImporteMenor", SqlDbType.Decimal);
                    comando.Parameters["@ImporteMenor"].Value = ImporteMenor;
                }
                if (FormaPago != Guid.Empty)
                {
                    comando.Parameters.Add("@FormaPago", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@FormaPago"].Value = FormaPago;
                }
                if (Estatus != Guid.Empty)
                {
                    comando.Parameters.Add("@Estatus", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@Estatus"].Value = Estatus;
                }

                foreach (DataRow item in this.Busquedas(comando).Rows)
                {
                    bool blAprobarPago = true;
                    bool blRechazarPago = true;

                    bool blConfirmarPago = false;
                    bool blEditarPago = true;

                    if (Guid.Parse(item["UidEstatusFechaPago"].ToString()) == Guid.Parse("8720B2B9-5712-4E75-A981-932887AACDC9"))
                    {
                        blAprobarPago = false;
                        blRechazarPago = true;
                    }

                    if (Guid.Parse(item["UidEstatusFechaPago"].ToString()) == Guid.Parse("77DB3F13-7EC8-4CE1-A3DB-E5C96D14A581"))
                    {
                        blAprobarPago = true;
                        blRechazarPago = false;
                    }

                    if (Guid.Parse(item["UidEstatusFechaPago"].ToString()) == Guid.Parse("F25E4AAB-6044-46E9-A575-98DCBCCF7604"))
                    {
                        blConfirmarPago = true;
                        blEditarPago = false;
                    }

                    if (Guid.Parse(item["UidFormaPago"].ToString()) == Guid.Parse("31BE9A23-73EE-4F44-AF6C-6C0648DCEBF7"))
                    {
                        blAprobarPago = false;
                        blRechazarPago = false;

                        blConfirmarPago = false;
                        blEditarPago = false;
                    }

                    if (Guid.Parse(item["UidFormaPago"].ToString()) == Guid.Parse("6BE13FFE-E567-4D4D-9CBC-37DA30EC23A5"))
                    {
                        blAprobarPago = false;
                        blRechazarPago = false;

                        blConfirmarPago = false;
                        blEditarPago = false;
                    }

                    if (Guid.Parse(item["UidFormaPago"].ToString()) == Guid.Parse("310F3557-682A-4144-9433-E47E48805D28"))
                    {
                        blAprobarPago = false;
                        blRechazarPago = false;

                        blConfirmarPago = false;
                        blEditarPago = false;
                    }

                    if (Guid.Parse(item["UidFormaPago"].ToString()) == Guid.Parse("D92A2C64-C797-4C96-AD18-C2A433081F37") || Guid.Parse(item["UidFormaPago"].ToString()) == Guid.Parse("3359D33E-C879-4A8B-96D3-C6A211AF014F"))
                    {
                        blAprobarPago = false;
                        blRechazarPago = false;

                        blConfirmarPago = false;
                        blEditarPago = false;
                    }

                    int CatDias = CantDias(DateTime.Parse(item["DtFHPago"].ToString()));

                    if (CatDias > 30)
                    {
                        if (Guid.Parse("F25E4AAB-6044-46E9-A575-98DCBCCF7604") != Guid.Parse(item["UidEstatusFechaPago"].ToString()))
                        {
                            blAprobarPago = false;
                            blRechazarPago = false;

                            blConfirmarPago = false;
                            blEditarPago = false;
                        }
                    }

                    decimal DcmImporteCole = DatosParaImpColeActualizado(Guid.Parse(item["UidAlumno"].ToString()), Guid.Parse(item["UidFechaColegiatura"].ToString()));
                    decimal SaldadoSumPagado = decimal.Parse(item["DcmImporteSaldado"].ToString()) + decimal.Parse(item["DcmImportePagado"].ToString());
                    decimal DcmImporteNuevo = DcmImporteCole - SaldadoSumPagado;

                    lsPagosReporteLigaEscuelaViewModels.Add(new PagosReporteLigaEscuelaViewModels()
                    {
                        UidPagoColegiatura = Guid.Parse(item["UidPagoColegiatura"].ToString()),
                        IntFolio = int.Parse(item["IntFolio"].ToString()),
                        VchIdentificador = item["VchIdentificador"].ToString(),
                        UidAlumno = Guid.Parse(item["UidAlumno"].ToString()),
                        VchMatricula = item["VchMatricula"].ToString(),
                        VchNombres = item["VchNombres"].ToString(),
                        VchApePaterno = item["VchApePaterno"].ToString(),
                        VchApeMaterno = item["VchApeMaterno"].ToString(),
                        UidFechaColegiatura = Guid.Parse(item["UidFechaColegiatura"].ToString()),
                        VchNum = int.Parse(item["IntNum"].ToString()) + " de " + int.Parse(item["IntCantPagos"].ToString()),
                        DtFHPago = DateTime.Parse(item["DtFHPago"].ToString()),
                        DcmImporteCole = DcmImporteCole,
                        DcmImporteSaldado = decimal.Parse(item["DcmImporteSaldado"].ToString()),
                        DcmImportePagado = decimal.Parse(item["DcmImportePagado"].ToString()),
                        DcmImporteNuevo = DcmImporteNuevo,
                        UidUsuario = Guid.Parse(item["UidUsuario"].ToString()),
                        UsNombre = item["UsNombre"].ToString(),
                        UsPaterno = item["UsPaterno"].ToString(),
                        UsMaterno = item["UsMaterno"].ToString(),
                        UidFormaPago = Guid.Parse(item["UidFormaPago"].ToString()),
                        VchFormaPago = item["VchFormaPago"].ToString(),
                        blAprobarPago = blAprobarPago,
                        blRechazarPago = blRechazarPago,
                        blConfirmarPago = blConfirmarPago,
                        blEditarPago = blEditarPago,
                        VchEstatus = item["VchEstatus"].ToString(),
                        VchColor = item["VchColor"].ToString(),

                        VchBanco = item["Banco"].ToString(),
                        VchCuenta = item["Cuenta"].ToString(),
                        VchFolio = item["Folio"].ToString(),

                    });
                }

                return lsPagosReporteLigaEscuelaViewModels.OrderByDescending(x => x.DtFHPago).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<PagosColegiaturasViewModel> CargarPagosColegiaturasRLE(Guid UidCliente, Guid UidAlumno)
        {
            List<PagosColegiaturasViewModel> lsPagosColegiaturasViewModel = new List<PagosColegiaturasViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select fca.BitUsarFecha, fca.DtFechaPago, fc.UidFechaColegiatura, cl.VchNombreComercial, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, al.BitBeca, al.VchTipoBeca, al.DcmBeca, co.*, fc.UidFechaColegiatura, fc.IntNum, fc.DtFHInicio as fcInicio, fc.DtFHLimite as fcLimite, fc.DtFHVencimiento as fcVencimiento, fc.DtFHFinPeriodo as fcFinPeriodo, efc.UidEstatusFechaColegiatura, efc.VchDescripcion as EstatusFechas, efc.VchColor, pe.VchDescripcion as Periodicidad, (select SUM(fepa.DcmImportePagado) from FechasColegiaturas feco, FechasPagos fepa, PagosColegiaturas paco where feco.UidFechaColegiatura = fepa.UidFechaColegiatura and fepa.UidPagoColegiatura = paco.UidPagoColegiatura and fepa.UidFechaColegiatura = fc.UidFechaColegiatura and fepa.UidAlumno = al.UidAlumno and (fepa.UidEstatusFechaPago = '8720B2B9-5712-4E75-A981-932887AACDC9' or fepa.UidEstatusFechaPago = 'F25E4AAB-6044-46E9-A575-98DCBCCF7604')) ImpPagado from clientes cl, Colegiaturas co, FechasColegiaturas fc, Alumnos al, EstatusFechasColegiaturas efc, Periodicidades pe, FechasColegiaturasAlumnos fca where fca.UidEstatusFechaColegiatura = efc.UidEstatusFechaColegiatura and fca.UidAlumno = al.UidAlumno and fca.UidFechaColegiatura = fc.UidFechaColegiatura and pe.UidPeriodicidad = co.UidPeriodicidad and co.UidEstatus = '65E46BC9-1864-4145-AD1A-70F5B5F69739' and (fca.UidEstatusFechaColegiatura != '5554CE57-1288-46D5-B36A-8AC69CB94B9A' and fca.UidEstatusFechaColegiatura != '605A7881-54E0-47DF-8398-EDE080F4E0AA') and cl.UidCliente = co.UidCliente and co.UidColegiatura = fc.UidColegiatura and fca.UidEstatusFechaColegiatura != '1331D93D-EA53-487F-BF28-E72F5E7D19BF' and cl.UidCliente = '" + UidCliente + "' and al.UidAlumno = '" + UidAlumno + "' order by al.VchMatricula";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                string FHLimite = "NO TIENE";
                string FHVencimiento = "NO TIENE";
                bool FHVence = false;
                bool blDatoFecha = false;

                bool blpagar = false;

                if (!string.IsNullOrEmpty(item["fcLimite"].ToString()))
                {
                    FHLimite = DateTime.Parse(item["fcLimite"].ToString()).ToString("dd/MM/yyyy");
                }
                if (!string.IsNullOrEmpty(item["fcVencimiento"].ToString()))
                {
                    FHVencimiento = DateTime.Parse(item["fcVencimiento"].ToString()).ToString("dd/MM/yyyy");
                    FHVence = true;
                }

                if (bool.Parse(item["BitUsarFecha"].ToString()))
                {
                    if (!string.IsNullOrEmpty(item["DtFechaPago"].ToString()))
                    {
                        blDatoFecha = true;
                    }
                }

                lsPagosColegiaturasViewModel.Add(new PagosColegiaturasViewModel()
                {
                    UidCliente = Guid.Parse(item["UidCliente"].ToString()),
                    UidFechaColegiatura = Guid.Parse(item["UidFechaColegiatura"].ToString()),
                    VchIdentificador = item["VchIdentificador"].ToString(),
                    DcmImporte = Calculo(decimal.Parse(item["DcmImporte"].ToString()), bool.Parse(item["BitRecargo"].ToString()), item["VchTipoRecargo"].ToString(), decimal.Parse(item["DcmRecargo"].ToString()), FHLimite, bool.Parse(item["BitRecargoPeriodo"].ToString()), item["VchTipoRecargoPeriodo"].ToString(), decimal.Parse(item["DcmRecargoPeriodo"].ToString()), item["Periodicidad"].ToString(), bool.Parse(item["BitBeca"].ToString()), item["VchTipoBeca"].ToString(), decimal.Parse(item["DcmBeca"].ToString()), DateTime.Parse(item["fcFinPeriodo"].ToString()), blDatoFecha, item["DtFechaPago"].ToString()),
                    ImpPagado = item.IsNull("ImpPagado") ? 0 : decimal.Parse(item["ImpPagado"].ToString()),
                    UidColegiatura = Guid.Parse(item["UidColegiatura"].ToString()),
                    VchNum = int.Parse(item["IntNum"].ToString()) + " de " + int.Parse(item["IntCantPagos"].ToString()),
                    DtFHInicio = DateTime.Parse(item["fcInicio"].ToString()),
                    VchFHLimite = FHLimite,
                    VchFHVencimiento = FHVencimiento,
                    DtFHFinPeriodo = DateTime.Parse(item["fcFinPeriodo"].ToString()),
                    VchEstatusFechas = item["EstatusFechas"].ToString(),
                    VchColor = item["VchColor"].ToString(),
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
            }

            return lsPagosColegiaturasViewModel.OrderBy(x => x.DtFHInicio).ToList();
        }
        public PagosColegiaturasViewModel ObtenerPagosColegiaturasRLE(Guid UidCliente, Guid UidFechaColegiatura, Guid UidAlumno)
        {
            pagosColegiaturasViewModel = new PagosColegiaturasViewModel();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            //con tutor query.CommandText = "select cl.VchNombreComercial, al.UidAlumno, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, al.BitBeca, al.VchTipoBeca, al.DcmBeca, co.*, fc.UidFechaColegiatura, fc.IntNum, fc.DtFHInicio as fcInicio, fc.DtFHLimite as fcLimite, fc.DtFHVencimiento as fcVencimiento, fc.DtFHFinPeriodo as fcFinPeriodo, efc.VchDescripcion as EstatusFechas, pe.VchDescripcion as Periodicidad from clientes cl, Colegiaturas co, FechasColegiaturas fc, ColegiaturasAlumnos ca, Alumnos al, Usuarios us, UsuariosAlumnos ua, EstatusFechasColegiaturas efc, Periodicidades pe where pe.UidPeriodicidad = co.UidPeriodicidad and co.UidEstatus = '65E46BC9-1864-4145-AD1A-70F5B5F69739' and efc.UidEstatusFechaColegiatura = fc.UidEstatusFechaColegiatura and cl.UidCliente = co.UidCliente and co.UidColegiatura = fc.UidColegiatura and ca.UidColegiatura = co.UidColegiatura and ca.UidAlumno = al.UidAlumno and ua.UidUsuario = us.UidUsuario and ua.UidAlumno = al.UidAlumno and cl.UidCliente = '" + UidCliente + "' and us.UidUsuario = '" + UidUsuario + "' and fc.UidFechaColegiatura = '" + UidFechaColegiatura + "' and al.VchMatricula = '" + VchMatricula + "'";
            query.CommandText = "select cl.VchNombreComercial, al.UidAlumno, al.VchMatricula, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, al.BitBeca, al.VchTipoBeca, al.DcmBeca, co.*, fc.UidFechaColegiatura, fc.IntNum, fc.DtFHInicio as fcInicio, fc.DtFHLimite as fcLimite, fc.DtFHVencimiento as fcVencimiento, fc.DtFHFinPeriodo as fcFinPeriodo, efc.UidEstatusFechaColegiatura, efc.VchDescripcion as EstatusFechas, efc.VchColor, pe.VchDescripcion as Periodicidad from clientes cl, Colegiaturas co, FechasColegiaturas fc, ColegiaturasAlumnos ca, Alumnos al, EstatusFechasColegiaturas efc, Periodicidades pe where pe.UidPeriodicidad = co.UidPeriodicidad and co.UidEstatus = '65E46BC9-1864-4145-AD1A-70F5B5F69739' and efc.UidEstatusFechaColegiatura = fc.UidEstatusFechaColegiatura and cl.UidCliente = co.UidCliente and co.UidColegiatura = fc.UidColegiatura and ca.UidColegiatura = co.UidColegiatura and ca.UidAlumno = al.UidAlumno and cl.UidCliente = '" + UidCliente + "' and fc.UidFechaColegiatura = '" + UidFechaColegiatura + "' and al.UidAlumno = '" + UidAlumno + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                string FHLimite = "NO TIENE";
                string FHVencimiento = "NO TIENE";
                bool FHVence = false;

                bool blpagar = false;

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
                pagosColegiaturasViewModel.VchColor = item["VchColor"].ToString();
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
        #endregion


        #region Procesos Automaticos
        public List<FCAATMViewModel> ObtenerFechaColegiaturasATM(DateTime hoy)
        {
            List<FCAATMViewModel> lsFCAATMViewModel = new List<FCAATMViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select *, efc.VchDescripcion as EstatusFechaColegiatura from FechasColegiaturasAlumnos fca, FechasColegiaturas fc, EstatusFechasColegiaturas efc where efc.UidEstatusFechaColegiatura = fca.UidEstatusFechaColegiatura and fca.UidEstatusFechaColegiatura != '5554CE57-1288-46D5-B36A-8AC69CB94B9A' and fca.UidEstatusFechaColegiatura != '605A7881-54E0-47DF-8398-EDE080F4E0AA' and fca.UidFechaColegiatura = fc.UidFechaColegiatura";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                hoy = DateTime.Parse(hoy.ToString("dd/MM/yyyy"));

                Guid EstatusFechaColegiatura = Guid.Parse("76C8793B-4493-44C8-B274-696A61358BDF");
                string strEstatusFechaColegiatura = "VIGENTE";

                //DtFHInicio DtFHLimite  DtFHVencimiento DtFHFinPeriodo

                if (!string.IsNullOrEmpty(item["DtFHLimite"].ToString()))
                {

                    if (hoy > DateTime.Parse(item["DtFHLimite"].ToString()))
                    {
                        strEstatusFechaColegiatura = "RETRASADO";
                        EstatusFechaColegiatura = Guid.Parse("FD1E57E9-1476-482A-A850-501E55298500");
                    }

                    if (!string.IsNullOrEmpty(item["DtFHVencimiento"].ToString()))
                    {
                        if (hoy > DateTime.Parse(item["DtFHVencimiento"].ToString()))
                        {
                            strEstatusFechaColegiatura = "BLOQUEADO";
                            EstatusFechaColegiatura = Guid.Parse("1331D93D-EA53-487F-BF28-E72F5E7D19BF");
                        }
                    }

                    if (!string.IsNullOrEmpty(item["DtFHFinPeriodo"].ToString()))
                    {
                        if (hoy > DateTime.Parse(item["DtFHFinPeriodo"].ToString()))
                        {
                            strEstatusFechaColegiatura = "VENCIDO";
                            EstatusFechaColegiatura = Guid.Parse("DB36D040-9E05-4E7B-83B4-DD4FF0D5AC3C");
                        }
                    }

                }
                else if (!string.IsNullOrEmpty(item["DtFHVencimiento"].ToString()))
                {

                    if (hoy > DateTime.Parse(item["DtFHVencimiento"].ToString()))
                    {
                        strEstatusFechaColegiatura = "BLOQUEADO";
                        EstatusFechaColegiatura = Guid.Parse("1331D93D-EA53-487F-BF28-E72F5E7D19BF");
                    }
                    if (!string.IsNullOrEmpty(item["DtFHFinPeriodo"].ToString()))
                    {
                        if (hoy > DateTime.Parse(item["DtFHFinPeriodo"].ToString()))
                        {
                            strEstatusFechaColegiatura = "VENCIDO";
                            EstatusFechaColegiatura = Guid.Parse("DB36D040-9E05-4E7B-83B4-DD4FF0D5AC3C");
                        }
                    }

                }
                else if (!string.IsNullOrEmpty(item["DtFHFinPeriodo"].ToString()))
                {
                    if (hoy > DateTime.Parse(item["DtFHFinPeriodo"].ToString()))
                    {
                        strEstatusFechaColegiatura = "VENCIDO";
                        EstatusFechaColegiatura = Guid.Parse("DB36D040-9E05-4E7B-83B4-DD4FF0D5AC3C");
                    }
                }

                lsFCAATMViewModel.Add(new FCAATMViewModel()
                {
                    UidFechaColegiaturaAlumno = Guid.Parse(item["UidFechaColegiaturaAlumno"].ToString()),
                    UidFechaColegiatura = Guid.Parse(item["UidFechaColegiatura"].ToString()),
                    UidAlumno = Guid.Parse(item["UidAlumno"].ToString()),
                    EstatusFechaColegiatura = strEstatusFechaColegiatura,
                    FHInicio = DateTime.Parse(item["DtFHInicio"].ToString()).ToString("dd/MM/yyyy"),
                    FHLimite = item["DtFHLimite"].ToString(),
                    FHVencimiento = item["DtFHVencimiento"].ToString(),
                    UidEstatusFechaColegiatura = EstatusFechaColegiatura
                });
            }
            return lsFCAATMViewModel;
        }
        public bool ActualizarEstatusFechasPagosATM(Guid UidFechaColegiaturaAlumno, Guid UidEstatusFechaColegiatura)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ColegiaturasFechasActualizarAutomatico";

                comando.Parameters.Add("@UidFechaColegiaturaAlumno", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidFechaColegiaturaAlumno"].Value = UidFechaColegiaturaAlumno;

                comando.Parameters.Add("@UidEstatusFechaColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEstatusFechaColegiatura"].Value = UidEstatusFechaColegiatura;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        #endregion

        #region ValidarEstatusColegiatura
        public bool ObtenerEstatusColegiatura(string IdReferencia)
        {
            pagosColegiaturasViewModel = new PagosColegiaturasViewModel();

            bool Activo = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select co.UidEstatus from FechasPagos fp, FechasColegiaturas fc, Colegiaturas co, PagosColegiaturas pc, ReferenciasClubPago rcp where fp.UidFechaColegiatura = fc.UidFechaColegiatura and co.UidColegiatura = fc.UidColegiatura and pc.UidPagoColegiatura = fp.UidPagoColegiatura and rcp.UidPagoColegiatura = pc.UidPagoColegiatura and rcp.IdReferencia = '" + IdReferencia + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                if (Guid.Parse(item["UidEstatus"].ToString()) == Guid.Parse("65E46BC9-1864-4145-AD1A-70F5B5F69739"))
                {
                    Activo = true;
                }
            }
            return Activo;
        }
        public bool ObtenerEstatusColegiatura2(Guid UidFechaColegiatura)
        {
            pagosColegiaturasViewModel = new PagosColegiaturasViewModel();

            bool Activo = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select co.UidEstatus from Colegiaturas co, FechasColegiaturas fc where co.UidColegiatura = fc.UidColegiatura and fc.UidFechaColegiatura = '" + UidFechaColegiatura + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                if (Guid.Parse(item["UidEstatus"].ToString()) == Guid.Parse("65E46BC9-1864-4145-AD1A-70F5B5F69739"))
                {
                    Activo = true;
                }
            }
            return Activo;
        }
        #endregion
    }
}
