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
    public class PagosRepository : SqlDataRepository
    {
        public bool AgregarInformacionTarjeta(string Autorizacion, string reference, DateTime HoraTransaccion, string response, string cc_type, string tp_operation, string nb_company, string nb_merchant, string id_url, string cd_error, string nb_error, string cc_number, string cc_mask, string FolioPago, decimal Monto, DateTime DtFechaOperacion)
        {
            SqlCommand Comando = new SqlCommand();
            bool resultado = false;
            try
            {
                Comando.CommandType = CommandType.StoredProcedure;
                Comando.CommandText = "sp_AgregaInformacionPagoConTarjeta";

                Comando.Parameters.Add("@FolioPago", SqlDbType.VarChar, 50);
                Comando.Parameters["@FolioPago"].Value = FolioPago;

                Comando.Parameters.Add("@cc_number", SqlDbType.VarChar, 50);
                Comando.Parameters["@cc_number"].Value = cc_number;

                Comando.Parameters.Add("@cc_mask", SqlDbType.VarChar, 50);
                Comando.Parameters["@cc_mask"].Value = cc_mask;

                Comando.Parameters.Add("@nb_company", SqlDbType.VarChar, 50);
                Comando.Parameters["@nb_company"].Value = nb_company;

                Comando.Parameters.Add("@nb_merchant", SqlDbType.VarChar, 50);
                Comando.Parameters["@nb_merchant"].Value = nb_merchant;

                Comando.Parameters.Add("@id_url", SqlDbType.VarChar, 50);
                Comando.Parameters["@id_url"].Value = id_url;

                Comando.Parameters.Add("@cd_error", SqlDbType.VarChar, 200);
                Comando.Parameters["@cd_error"].Value = cd_error;

                Comando.Parameters.Add("@nb_error", SqlDbType.VarChar, 200);
                Comando.Parameters["@nb_error"].Value = nb_error;

                Comando.Parameters.Add("@IdReferencia", SqlDbType.VarChar, 100);
                Comando.Parameters["@IdReferencia"].Value = reference;

                Comando.Parameters.Add("@UidPago", SqlDbType.UniqueIdentifier);
                Comando.Parameters["@UidPago"].Value = Guid.NewGuid();

                Comando.Parameters.Add("@FechaRegistro", SqlDbType.DateTime);
                Comando.Parameters["@FechaRegistro"].Value = HoraTransaccion;

                Comando.Parameters.Add("@VchReferencia", SqlDbType.Text);
                Comando.Parameters["@VchReferencia"].Value = reference;

                Comando.Parameters.Add("@VchEstatusPago", SqlDbType.VarChar, 50);
                Comando.Parameters["@VchEstatusPago"].Value = response;

                Comando.Parameters.Add("@VchTipoDeTarjeta", SqlDbType.VarChar, 200);
                Comando.Parameters["@VchTipoDeTarjeta"].Value = cc_type;

                Comando.Parameters.Add("@VchTipoDeOperacion", SqlDbType.VarChar, 100);
                Comando.Parameters["@VchTipoDeOperacion"].Value = tp_operation;

                Comando.Parameters.Add("@MMonto", SqlDbType.Money);
                Comando.Parameters["@MMonto"].Value = Monto;

                Comando.Parameters.Add("@Autorizacion", SqlDbType.VarChar, 10);
                Comando.Parameters["@Autorizacion"].Value = Autorizacion;

                Comando.Parameters.Add("@DtFechaOperacion", SqlDbType.DateTime);
                Comando.Parameters["@DtFechaOperacion"].Value = DtFechaOperacion;

                resultado = this.ManipulacionDeDatos(Comando);

            }
            catch (Exception)
            {
                throw;
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
        
        public string ObtenerCorreoAuxiliar(string IdReferencia)
        {
            string Resultado = string.Empty;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from AuxiliarCorreos where IdReferencia = '" + IdReferencia + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                Resultado = item["VchCorreo"].ToString();
            }

            return Resultado;
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

            query.CommandText = "select lu.UidPropietario, lu.UidPagoColegiatura, us.VchCorreo from LigasUrls lu, PagosTarjeta pt, Usuarios us where us.UidUsuario = lu.UidUsuario and lu.IdReferencia = pt.IdReferencia and lu.IdReferencia = '" + IdReferencia + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                Resultado = item["UidPagoColegiatura"].ToString();
                Resultado2 = item["VchCorreo"].ToString();
                Resultado3 = item["UidPropietario"].ToString();
            }

            return Tuple.Create(Resultado, Resultado2, Resultado3);
        }
        public Tuple<List<PagosColegiaturas>, List<DetallesPagosColegiaturas>> ObtenerPagoColegiatura(Guid UidPagoColegiatura)
        {
            List<PagosColegiaturas> lsPagosColegiaturas = new List<PagosColegiaturas>();
            List<DetallesPagosColegiaturas> lsDetallesPagosColegiaturas = new List<DetallesPagosColegiaturas>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from PagosColegiaturas where UidPagoColegiatura = '" + UidPagoColegiatura + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsPagosColegiaturas.Add(new PagosColegiaturas
                {
                    VchAlumno = item["VchAlumno"].ToString(),
                    VchMatricula = item["VchMatricula"].ToString(),
                    DtFHPago = DateTime.Parse(item["DtFHPago"].ToString()),
                    VchPromocionDePago = item["VchPromocionDePago"].ToString(),
                    VchComisionBancaria = item["VchComisionBancaria"].ToString(),
                    BitSubtotal = bool.Parse(item["BitSubtotal"].ToString()),
                    DcmSubtotal = decimal.Parse(item["DcmSubtotal"].ToString()),
                    BitComisionBancaria = bool.Parse(item["BitComisionBancaria"].ToString()),
                    DcmComisionBancaria = decimal.Parse(item["DcmComisionBancaria"].ToString()),
                    BitPromocionDePago = bool.Parse(item["BitPromocionDePago"].ToString()),
                    DcmPromocionDePago = decimal.Parse(item["DcmPromocionDePago"].ToString()),
                    DcmTotal = decimal.Parse(item["DcmTotal"].ToString())
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

            return Tuple.Create(lsPagosColegiaturas, lsDetallesPagosColegiaturas);
        }
        #endregion
        #endregion
    }
}
