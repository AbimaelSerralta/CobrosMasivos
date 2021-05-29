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
    public class PagosTarjetaPragaRepository : SqlDataRepository
    {
        public bool AgregarInformacionTarjeta(PagosTarjetaPraga pagosTarjetaPraga)
        {
            SqlCommand Comando = new SqlCommand();
            bool resultado = false;
            try
            {
                Comando.CommandType = CommandType.StoredProcedure;
                Comando.CommandText = "sp_AgregarInformacionPagoTarjetaPraga";

                Comando.Parameters.Add("@UidPagoTarjeta", SqlDbType.UniqueIdentifier);
                Comando.Parameters["@UidPagoTarjeta"].Value = Guid.NewGuid();

                Comando.Parameters.Add("@IdReferencia", SqlDbType.VarChar);
                Comando.Parameters["@IdReferencia"].Value = pagosTarjetaPraga.IdReferencia;
                
                Comando.Parameters.Add("@VchEstatus", SqlDbType.VarChar);
                Comando.Parameters["@VchEstatus"].Value = pagosTarjetaPraga.VchEstatus;
                
                Comando.Parameters.Add("@foliocpagos", SqlDbType.VarChar);
                Comando.Parameters["@foliocpagos"].Value = pagosTarjetaPraga.foliocpagos;
                
                Comando.Parameters.Add("@auth", SqlDbType.VarChar);
                Comando.Parameters["@auth"].Value = pagosTarjetaPraga.auth;
                
                Comando.Parameters.Add("@cd_response", SqlDbType.VarChar);
                Comando.Parameters["@cd_response"].Value = pagosTarjetaPraga.cd_response;
                
                Comando.Parameters.Add("@cd_error", SqlDbType.VarChar);
                Comando.Parameters["@cd_error"].Value = pagosTarjetaPraga.cd_error;
                
                Comando.Parameters.Add("@nb_error", SqlDbType.VarChar);
                Comando.Parameters["@nb_error"].Value = pagosTarjetaPraga.nb_error;
                
                Comando.Parameters.Add("@DtmFechaDeRegistro", SqlDbType.DateTime);
                Comando.Parameters["@DtmFechaDeRegistro"].Value = pagosTarjetaPraga.DtmFechaDeRegistro;
                
                Comando.Parameters.Add("@DtFechaOperacion", SqlDbType.DateTime);
                Comando.Parameters["@DtFechaOperacion"].Value = pagosTarjetaPraga.DtFechaOperacion;
                
                Comando.Parameters.Add("@nb_company", SqlDbType.VarChar);
                Comando.Parameters["@nb_company"].Value = pagosTarjetaPraga.nb_company;
                
                Comando.Parameters.Add("@nb_merchant", SqlDbType.VarChar);
                Comando.Parameters["@nb_merchant"].Value = pagosTarjetaPraga.nb_merchant;
                
                Comando.Parameters.Add("@cc_type", SqlDbType.VarChar);
                Comando.Parameters["@cc_type"].Value = pagosTarjetaPraga.cc_type;
                
                Comando.Parameters.Add("@tp_operation", SqlDbType.VarChar);
                Comando.Parameters["@tp_operation"].Value = pagosTarjetaPraga.tp_operation;
                
                Comando.Parameters.Add("@cc_name", SqlDbType.VarChar);
                Comando.Parameters["@cc_name"].Value = pagosTarjetaPraga.cc_name;
                
                Comando.Parameters.Add("@cc_number", SqlDbType.VarChar);
                Comando.Parameters["@cc_number"].Value = pagosTarjetaPraga.cc_number;
                
                Comando.Parameters.Add("@cc_expmonth", SqlDbType.VarChar);
                Comando.Parameters["@cc_expmonth"].Value = pagosTarjetaPraga.cc_expmonth;
                
                Comando.Parameters.Add("@cc_expyear", SqlDbType.VarChar);
                Comando.Parameters["@cc_expyear"].Value = pagosTarjetaPraga.cc_expyear;
                
                Comando.Parameters.Add("@amount", SqlDbType.Decimal);
                Comando.Parameters["@amount"].Value = pagosTarjetaPraga.amount;

                Comando.Parameters.Add("@emv_key_date", SqlDbType.VarChar);
                Comando.Parameters["@emv_key_date"].Value = pagosTarjetaPraga.emv_key_date;

                Comando.Parameters.Add("@id_url", SqlDbType.VarChar);
                Comando.Parameters["@id_url"].Value = pagosTarjetaPraga.id_url;
                
                Comando.Parameters.Add("@email", SqlDbType.VarChar);
                Comando.Parameters["@email"].Value = pagosTarjetaPraga.email;
                
                Comando.Parameters.Add("@payment_type", SqlDbType.VarChar);
                Comando.Parameters["@payment_type"].Value = pagosTarjetaPraga.payment_type;

                resultado = this.ManipulacionDeDatos(Comando);

            }
            catch (Exception ex)
            {
                string mnsj = ex.Message;
            }
            return resultado;
        }

        public List<LigasUrlsPayCardModel> ConsultarPromocionLiga(string IdReferencia)
        {
            List<LigasUrlsPayCardModel> lsLigasUrlsPayCardModel = new List<LigasUrlsPayCardModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from LigasUrls where IdReferencia = '" + IdReferencia + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                Guid UidLigaAsociado = item.IsNull("UidLigaAsociado") ? Guid.Empty : Guid.Parse(item["UidLigaAsociado"].ToString());

                if (UidLigaAsociado != null && UidLigaAsociado != Guid.Empty)
                {
                    SqlCommand quer = new SqlCommand();
                    quer.CommandType = CommandType.Text;

                    quer.CommandText = "select * from LigasUrls where IdReferencia != '" + IdReferencia + "' and UidLigaAsociado = '" + UidLigaAsociado + "'";

                    DataTable data = this.Busquedas(quer);

                    foreach (DataRow it in data.Rows)
                    {
                        lsLigasUrlsPayCardModel.Add(new LigasUrlsPayCardModel()
                        {
                            UidLigaUrl = Guid.Parse(it["UidLigaUrl"].ToString()),
                            VchUrl = it["VchUrl"].ToString(),
                            VchConcepto = it["VchConcepto"].ToString(),
                            IdReferencia = it["IdReferencia"].ToString()
                        });
                    }
                }
            }

            return lsLigasUrlsPayCardModel;
        }
        public List<LigasEventoPayCardModel> ConsultarPagoEventoLiga(string IdReferencia)
        {
            List<LigasEventoPayCardModel> lsLigasEventoPayCardModel = new List<LigasEventoPayCardModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select lu.IdReferencia, lu.DcmImporte, cl.VchNombreComercial from LigasUrls lu, Clientes cl where lu.UidPropietario = cl.UidCliente and lu.IdReferencia = '" + IdReferencia + "'";
            DataTable dt = this.Busquedas(query);

            string pago = string.Empty;
            bool promocion = false;

            if (dt.Rows.Count == 0)
            {
                SqlCommand quer = new SqlCommand();
                quer.CommandType = CommandType.Text;

                quer.CommandText = "select * from LigasUrls where IdReferencia != '" + IdReferencia + "' and UidLigaAsociado = '" + IdReferencia + "'";
                dt = this.Busquedas(quer);

                promocion = true;
            }

            foreach (DataRow it in dt.Rows)
            {
                if (promocion)
                {
                    pago = "Al contado";
                }
                else
                {
                    pago = it["VchDescripcion"].ToString();
                }

                lsLigasEventoPayCardModel.Add(new LigasEventoPayCardModel()
                {
                    IdReferencia = it["IdReferencia"].ToString(),
                    DcmImporte =decimal.Parse(it["DcmImporte"].ToString()),
                    VchNombreComercial = it["VchNombreComercial"].ToString(),
                    VchPago = pago                    
                });
            }

            return lsLigasEventoPayCardModel;
        }
        
        #region Metodos Escuela
        #region Pagos
        public Tuple<string, string, string> ConsultarPagoColegiatura(string IdReferencia)
        {
            string Resultado = string.Empty;
            string Resultado2 = string.Empty;
            string Resultado3 = string.Empty;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select lup.UidPagoColegiatura, us.VchCorreo, lup.UidPropietario from LigasUrlsPraga lup, PagosTarjetaPraga ptp, Usuarios us where us.UidUsuario = lup.UidUsuario and lup.IdReferencia = ptp.IdReferencia and ptp.VchEstatus = 'approved' and lup.IdReferencia = '" + IdReferencia + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                Resultado = item["UidPagoColegiatura"].ToString();
                Resultado2 = item["VchCorreo"].ToString();
                Resultado3 = item["UidPropietario"].ToString();
            }

            return Tuple.Create(Resultado, Resultado2, Resultado3);
        }
        public Tuple<string, string, string> ConsultarPagoColegiaturaDenegado(string IdReferencia)
        {
            string Resultado = string.Empty;
            string Resultado2 = string.Empty;
            string Resultado3 = string.Empty;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select distinct lup.UidPagoColegiatura, us.VchCorreo, lup.UidPropietario from LigasUrlsPraga lup, PagosTarjetaPraga ptp, Usuarios us where us.UidUsuario = lup.UidUsuario and lup.IdReferencia = ptp.IdReferencia and lup.IdReferencia = '" + IdReferencia + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                Resultado = item["UidPagoColegiatura"].ToString();
                Resultado2 = item["VchCorreo"].ToString();
                Resultado3 = item["UidPropietario"].ToString();
            }

            return Tuple.Create(Resultado, Resultado2, Resultado3);
        }
        public Tuple<List<PagosColegiaturasViewModels>, List<DetallesPagosColegiaturas>> ObtenerPagoColegiatura(Guid UidPagoColegiatura)
        {
            List<PagosColegiaturasViewModels> lsPagosColegiaturasViewModels = new List<PagosColegiaturasViewModels>();
            List<DetallesPagosColegiaturas> lsDetallesPagosColegiaturas = new List<DetallesPagosColegiaturas>();

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

            //===============================================================================================
            SqlCommand query2 = new SqlCommand();
            query2.CommandType = CommandType.Text;

            query2.CommandText = "select * from DetallesPagosColegiaturas where UidPagoColegiatura = '" + UidPagoColegiatura + "'";

            DataTable dt2 = this.Busquedas(query2);

            foreach (DataRow item in dt2.Rows)
            {
                lsDetallesPagosColegiaturas.Add(new DetallesPagosColegiaturas
                {
                    IntNum = int.Parse(item["IntNum"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    DcmImporte = decimal.Parse(item["DcmImporte"].ToString())
                });
            }

            return Tuple.Create(lsPagosColegiaturasViewModels, lsDetallesPagosColegiaturas);
        }
        public bool ActualizarPagoColegiatura(Guid UidPagoColegiatura, Guid UidEstatusFechaPago)
        {
            SqlCommand Comando = new SqlCommand();
            bool resultado = false;
            try
            {
                Comando.CommandType = CommandType.StoredProcedure;
                Comando.CommandText = "sp_PagosColegiaturasEstatusActualizar";

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

        public Tuple<string, string, string, string> ConsultarDatosValidarPago(Guid UidPagoColegiatura)
        {
            string UidCliente = string.Empty;
            string UidUsuario = string.Empty;
            string UidFechaColegiatura = string.Empty;
            string UidAlumno = string.Empty;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select al.UidCliente, pc.UidUsuario, fp.UidFechaColegiatura, al.UidAlumno from FechasPagos fp, PagosColegiaturas pc, Alumnos al where pc.UidPagoColegiatura = fp.UidPagoColegiatura and fp.UidAlumno = al.UidAlumno and pc.UidPagoColegiatura = '" + UidPagoColegiatura + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                UidCliente = item["UidCliente"].ToString();
                UidUsuario = item["UidUsuario"].ToString();
                UidFechaColegiatura = item["UidFechaColegiatura"].ToString();
                UidAlumno = item["UidAlumno"].ToString();
            }

            return Tuple.Create(UidCliente, UidUsuario, UidFechaColegiatura, UidAlumno);
        }

        public bool ValidarPagoPadre(string IdReferencia)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from PagosTarjetaPraga where VchEstatus = 'approved' and IdReferencia = '" + IdReferencia + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }
        #endregion

        #region ReporteLigasPadres
        public List<PagosTarjetaColeDetalleGridViewModel> ConsultarDetallePagoColegiatura(Guid UidPagoColegiatura)
        {
            List<PagosTarjetaColeDetalleGridViewModel> lsPagosTarjetaColeDetalleGridViewModel = new List<PagosTarjetaColeDetalleGridViewModel>(); ;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select lu.IdReferencia, pt.FolioPago, pt.DtmFechaDeRegistro, pt.cc_number from LigasUrls lu, PagosTarjeta pt where lu.UidPagoColegiatura is not null and pt.VchEstatus = 'approved'and lu.IdReferencia = pt.IdReferencia and lu.UidPagoColegiatura = '" + UidPagoColegiatura + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsPagosTarjetaColeDetalleGridViewModel.Add(new PagosTarjetaColeDetalleGridViewModel()
                {
                    IdReferencia = item["IdReferencia"].ToString(),
                    FolioPago = item["FolioPago"].ToString(),
                    DtmFechaDeRegistro = DateTime.Parse(item["DtmFechaDeRegistro"].ToString()),
                    cc_number = item["cc_number"].ToString()
                });
            }

            return lsPagosTarjetaColeDetalleGridViewModel;
        }
        #endregion
        #endregion
    }
}
