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
    public class EventosRepository : SqlDataRepository
    {
        private Eventos _eventos = new Eventos();
        public Eventos eventos
        {
            get { return _eventos; }
            set { _eventos = value; }
        }

        private EventosGridViewModel _eventosGridViewModel = new EventosGridViewModel();
        public EventosGridViewModel eventosGridViewModel
        {
            get { return _eventosGridViewModel; }
            set { _eventosGridViewModel = value; }
        }

        #region MetodosFranquicias

        #endregion

        #region MetodosClientes

        public List<EventosGridViewModel> CargarEventos(Guid UidCliente)
        {
            List<EventosGridViewModel> lsEventosGridViewModel = new List<EventosGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select ev.*, es.VchDescripcion as Estatus, es.VchIcono from Eventos ev, Estatus es where ev.UidEstatus = es.UidEstatus and UidPropietario = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsEventosGridViewModel.Add(new EventosGridViewModel()
                {
                    UidEvento = Guid.Parse(item["UidEvento"].ToString()),
                    VchNombreEvento = item["VchNombreEvento"].ToString(),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    DtRegistro = DateTime.Parse(item["DtRegistro"].ToString()),
                    DtFHInicio = DateTime.Parse(item["DtFHInicio"].ToString()),
                    DtFHFin = DateTime.Parse(item["DtFHFin"].ToString()),
                    BitTipoImporte = bool.Parse(item["BitTipoImporte"].ToString()),
                    DcmImporte = decimal.Parse(item["DcmImporte"].ToString()),
                    VchConcepto = item["VchConcepto"].ToString(),
                    BitDatosUsuario = bool.Parse(item["BitDatosUsuario"].ToString()),
                    VchUrlEvento = item["VchUrlEvento"].ToString(),
                    UidEstatus = Guid.Parse(item["UidEstatus"].ToString()),
                    VchEstatus = item["Estatus"].ToString(),
                    VchIcono = item["VchIcono"].ToString()
                });
            }

            return lsEventosGridViewModel.OrderByDescending(x => x.DtRegistro).ToList();
        }
        public bool RegistrarEvento(EventosGridViewModel eventos)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_EventosRegistrar";

                comando.Parameters.Add("@UidEvento", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEvento"].Value = eventos.UidEvento;

                comando.Parameters.Add("@VchNombreEvento", SqlDbType.VarChar);
                comando.Parameters["@VchNombreEvento"].Value = eventos.VchNombreEvento;

                comando.Parameters.Add("@VchDescripcion", SqlDbType.VarChar);
                comando.Parameters["@VchDescripcion"].Value = eventos.VchDescripcion;

                comando.Parameters.Add("@DtRegistro", SqlDbType.DateTime);
                comando.Parameters["@DtRegistro"].Value = eventos.DtRegistro;

                comando.Parameters.Add("@DtFHInicio", SqlDbType.DateTime);
                comando.Parameters["@DtFHInicio"].Value = eventos.DtFHInicio;

                comando.Parameters.Add("@DtFHFin", SqlDbType.DateTime);
                comando.Parameters["@DtFHFin"].Value = eventos.DtFHFin;

                comando.Parameters.Add("@BitTipoImporte", SqlDbType.Bit);
                comando.Parameters["@BitTipoImporte"].Value = eventos.BitTipoImporte;

                comando.Parameters.Add("@DcmImporte", SqlDbType.Decimal);
                comando.Parameters["@DcmImporte"].Value = eventos.DcmImporte;

                comando.Parameters.Add("@VchConcepto", SqlDbType.VarChar);
                comando.Parameters["@VchConcepto"].Value = eventos.VchConcepto;

                comando.Parameters.Add("@BitDatosUsuario", SqlDbType.Bit);
                comando.Parameters["@BitDatosUsuario"].Value = eventos.BitDatosUsuario;

                comando.Parameters.Add("@VchUrlEvento", SqlDbType.VarChar);
                comando.Parameters["@VchUrlEvento"].Value = eventos.VchUrlEvento;

                comando.Parameters.Add("@UidPropietario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPropietario"].Value = eventos.UidPropietario;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        
        public bool ActualizarEvento(EventosGridViewModel eventos)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_EventosActualizar";

                comando.Parameters.Add("@UidEvento", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEvento"].Value = eventos.UidEvento;

                comando.Parameters.Add("@VchNombreEvento", SqlDbType.VarChar);
                comando.Parameters["@VchNombreEvento"].Value = eventos.VchNombreEvento;

                comando.Parameters.Add("@VchDescripcion", SqlDbType.VarChar);
                comando.Parameters["@VchDescripcion"].Value = eventos.VchDescripcion;

                comando.Parameters.Add("@DtFHInicio", SqlDbType.DateTime);
                comando.Parameters["@DtFHInicio"].Value = eventos.DtFHInicio;

                comando.Parameters.Add("@DtFHFin", SqlDbType.DateTime);
                comando.Parameters["@DtFHFin"].Value = eventos.DtFHFin;

                comando.Parameters.Add("@BitTipoImporte", SqlDbType.Bit);
                comando.Parameters["@BitTipoImporte"].Value = eventos.BitTipoImporte;

                comando.Parameters.Add("@DcmImporte", SqlDbType.Decimal);
                comando.Parameters["@DcmImporte"].Value = eventos.DcmImporte;

                comando.Parameters.Add("@VchConcepto", SqlDbType.VarChar);
                comando.Parameters["@VchConcepto"].Value = eventos.VchConcepto;

                comando.Parameters.Add("@BitDatosUsuario", SqlDbType.Bit);
                comando.Parameters["@BitDatosUsuario"].Value = eventos.BitDatosUsuario;

                comando.Parameters.Add("@UidEstatus", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEstatus"].Value = eventos.UidEstatus;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        public bool EliminarEvento(Guid UidCliente)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_EventosEliminar";

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
        public void ObtenerDatosEvento(Guid UidEvento)
        {
            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select ev.*, es.VchDescripcion as Estatus, es.VchIcono, cl.VchNombreComercial, cl.VchCorreoElectronico, tc.VchTelefono from Eventos ev, Estatus es, Clientes cl, TelefonosClientes tc where ev.UidEstatus = es.UidEstatus and cl.UidCliente = ev.UidPropietario and tc.UidCliente = cl.UidCliente and UidEvento = '" + UidEvento + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                eventosGridViewModel = new EventosGridViewModel()
                {
                    UidEvento = Guid.Parse(item["UidEvento"].ToString()),
                    VchNombreEvento = item["VchNombreEvento"].ToString(),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    DtRegistro = DateTime.Parse(item["DtRegistro"].ToString()),
                    DtFHInicio = DateTime.Parse(item["DtFHInicio"].ToString()),
                    DtFHFin = DateTime.Parse(item["DtFHFin"].ToString()),
                    BitTipoImporte = bool.Parse(item["BitTipoImporte"].ToString()),
                    DcmImporte = decimal.Parse(item["DcmImporte"].ToString()),
                    VchConcepto = item["VchConcepto"].ToString(),
                    BitDatosUsuario = bool.Parse(item["BitDatosUsuario"].ToString()),
                    VchUrlEvento = item["VchUrlEvento"].ToString(),
                    UidEstatus = Guid.Parse(item["UidEstatus"].ToString()),
                    VchEstatus = item["Estatus"].ToString(),
                    VchIcono = item["VchIcono"].ToString(),
                    UidPropietario = Guid.Parse(item["UidPropietario"].ToString()),
                    VchNombreComercial = item["VchNombreComercial"].ToString(),
                    VchTelefono = item["VchTelefono"].ToString(),
                    VchCorreo = item["VchCorreoElectronico"].ToString()
                };
            }
        }
        public bool RegistrarPromocionesEvento(Guid UidEvento, Guid UidPromociones)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_EventosPromocionesRegistrar";

                comando.Parameters.Add("@UidEvento", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEvento"].Value = UidEvento;

                comando.Parameters.Add("@UidPromocion", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPromocion"].Value = UidPromociones;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        public bool EliminarPromocionesEvento(Guid UidEvento)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_EventosPromocionesEliminar";

                comando.Parameters.Add("@UidEvento", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEvento"].Value = UidEvento;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        public string ObtenerUrlLiga(string IdReferencia)
        {
            string Resultado = string.Empty;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select VchUrl from LigasUrls where IdReferencia = '" + IdReferencia + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                Resultado = item["VchUrl"].ToString();
            }

            return Resultado;
        }
        public string ObtenerUidAdminCliente(Guid UidCliente)
        {
            string Resultado = string.Empty;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select top 1 us.UidUsuario from Usuarios us, ClientesUsuarios cu, SegUsuarios su where su.UidUsuario = us.UidUsuario and us.UidUsuario = cu.UidUsuario and su.UidSegPerfil = 'D2C80D47-C14C-4677-A63D-C46BCB50FE17' and cu.UidCliente = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                Resultado = item["UidUsuario"].ToString();
            }

            return Resultado;
        }

        public bool ValidarPagoEvento(string IdReferencia)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from PagosTarjeta where VchEstatus = 'approved' and IdReferencia = '" + IdReferencia + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }
        public bool EliminarLigaEvento(string IdReferencia)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_EventosLigaEliminar";

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
        public bool InsertCorreoLigaEvento(string Correo, string IdReferencia)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_EventosCorreoLigaInsertar";

                comando.Parameters.Add("@VchCorreo", SqlDbType.VarChar);
                comando.Parameters["@VchCorreo"].Value = Correo;
                
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

        public List<EventosGridViewModel> BuscarEventos(Guid UidPropietario, string VchNombreEvento, string DtFHInicioDesde, string DtFHInicioHasta, string DtFHFinDesde, string DtFHFinHasta, decimal DcmImporteMayor, decimal DcmImporteMenor, Guid UidEstatus)
        {
            List<EventosGridViewModel> lsEventosGridViewModel = new List<EventosGridViewModel>();

            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "sp_EventosBuscar";
            try
            {
                comando.Parameters.Add("@UidPropietario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPropietario"].Value = UidPropietario;

                if (VchNombreEvento != string.Empty)
                {
                    comando.Parameters.Add("@VchNombreEvento", SqlDbType.VarChar);
                    comando.Parameters["@VchNombreEvento"].Value = VchNombreEvento;
                }

                if (DtFHInicioDesde != string.Empty)
                {
                    comando.Parameters.Add("@DtFHInicioDesde", SqlDbType.DateTime);
                    comando.Parameters["@DtFHInicioDesde"].Value = DtFHInicioDesde;
                }
                if (DtFHInicioHasta != string.Empty)
                {
                    comando.Parameters.Add("@DtFHInicioHasta", SqlDbType.Date);
                    comando.Parameters["@DtFHInicioHasta"].Value = DtFHInicioHasta;
                }
                if (DtFHFinDesde != string.Empty)
                {
                    comando.Parameters.Add("@DtFHFinDesde", SqlDbType.DateTime);
                    comando.Parameters["@DtFHFinDesde"].Value = DtFHFinDesde;
                }
                if (DtFHFinHasta != string.Empty)
                {
                    comando.Parameters.Add("@DtFHFinHasta", SqlDbType.Date);
                    comando.Parameters["@DtFHFinHasta"].Value = DtFHFinHasta;
                }
                if (DcmImporteMayor != 0)
                {
                    comando.Parameters.Add("@DcmImporteMayor", SqlDbType.Decimal);
                    comando.Parameters["@DcmImporteMayor"].Value = DcmImporteMayor;
                }
                if (DcmImporteMenor != 0)
                {
                    comando.Parameters.Add("@DcmImporteMenor", SqlDbType.Decimal);
                    comando.Parameters["@DcmImporteMenor"].Value = DcmImporteMenor;
                }

                if (UidEstatus != Guid.Empty)
                {
                    comando.Parameters.Add("@UidEstatus", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidEstatus"].Value = UidEstatus;
                }

                foreach (DataRow item in this.Busquedas(comando).Rows)
                {
                    eventosGridViewModel = new EventosGridViewModel()
                    {
                        UidEvento = Guid.Parse(item["UidEvento"].ToString()),
                        VchNombreEvento = item["VchNombreEvento"].ToString(),
                        VchDescripcion = item["VchDescripcion"].ToString(),
                        DtRegistro = DateTime.Parse(item["DtRegistro"].ToString()),
                        DtFHInicio = DateTime.Parse(item["DtFHInicio"].ToString()),
                        DtFHFin = DateTime.Parse(item["DtFHFin"].ToString()),
                        BitTipoImporte = bool.Parse(item["BitTipoImporte"].ToString()),
                        DcmImporte = decimal.Parse(item["DcmImporte"].ToString()),
                        VchConcepto = item["VchConcepto"].ToString(),
                        BitDatosUsuario = bool.Parse(item["BitDatosUsuario"].ToString()),
                        VchUrlEvento = item["VchUrlEvento"].ToString(),
                        UidEstatus = Guid.Parse(item["UidEstatus"].ToString()),
                        VchEstatus = item["Estatus"].ToString(),
                        VchIcono = item["VchIcono"].ToString()
                    };

                    lsEventosGridViewModel.Add(eventosGridViewModel);
                }

                return lsEventosGridViewModel.OrderByDescending(x => x.DtRegistro).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
