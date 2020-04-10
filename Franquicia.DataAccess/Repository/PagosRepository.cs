using Franquicia.DataAccess.Common;
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

                    quer.CommandText = "select * from LigasUrls where IdReferencia != '"+ IdReferencia + "' and UidLigaAsociado = '" + UidLigaAsociado + "'";

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
    }
}
