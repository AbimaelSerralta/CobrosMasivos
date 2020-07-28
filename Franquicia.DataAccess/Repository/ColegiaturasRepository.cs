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

        #region MetodosFranquicias

        #endregion

        #region MetodosClientes
        public List<ColegiaturasGridViewModel> CargarColegiaturas(Guid UidCliente)
        {
            List<ColegiaturasGridViewModel> lsColegiaturasGridViewModel = new List<ColegiaturasGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select co.*, pe.VchDescripcion from Colegiaturas co, Periodicidades pe where co.UidPeriodicidad = pe.UidPeriodicidad and co.UidCliente = '" + UidCliente + "'";

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
                    DcmRecargo = decimal.Parse(item["DcmRecargo"].ToString())
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

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }

        public bool RegistrarColegiaturaFechas(Guid UidColegiatura, DateTime DtFHInicio, DateTime DtFHLimite, DateTime DtFHVencimiento)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ColegiaturasFechasRegistrar";

                comando.Parameters.Add("@UidColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidColegiatura"].Value = UidColegiatura;

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

            int num = 1;
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
                    IntNumero = num++,
                    DtFHInicio = DateTime.Parse(item["DtFHInicio"].ToString()),
                    VchFHLimite = FHLimite,
                    VchFHVencimiento = FHVencimiento
                });;
            }

            return lsColegiaturasFechasGridViewModel;
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
        //public bool RegistrarPromocionesEvento(Guid UidEvento, Guid UidPromociones)
        //{
        //    bool Resultado = false;

        //    SqlCommand comando = new SqlCommand();
        //    try
        //    {
        //        comando.CommandType = System.Data.CommandType.StoredProcedure;
        //        comando.CommandText = "sp_EventosPromocionesRegistrar";

        //        comando.Parameters.Add("@UidEvento", SqlDbType.UniqueIdentifier);
        //        comando.Parameters["@UidEvento"].Value = UidEvento;

        //        comando.Parameters.Add("@UidPromocion", SqlDbType.UniqueIdentifier);
        //        comando.Parameters["@UidPromocion"].Value = UidPromociones;

        //        Resultado = this.ManipulacionDeDatos(comando);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return Resultado;
        //}
        //public bool EliminarPromocionesEvento(Guid UidEvento)
        //{
        //    bool Resultado = false;

        //    SqlCommand comando = new SqlCommand();
        //    try
        //    {
        //        comando.CommandType = System.Data.CommandType.StoredProcedure;
        //        comando.CommandText = "sp_EventosPromocionesEliminar";

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
    }
}
