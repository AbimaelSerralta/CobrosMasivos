using Franquicia.DataAccess.Common;
using Franquicia.Domain.Models;
using Franquicia.Domain.Models.IntegracionesPraga;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.DataAccess.Repository.IntegracionesPraga
{
    public class PagosTarjetaPragaIntegracionRepository : SqlDataRepository
    {
        public bool AgregarInformacionTarjeta(PagosTarjetaPraga pagosTarjetaPraga)
        {
            SqlCommand Comando = new SqlCommand();
            bool resultado = false;
            try
            {
                Comando.CommandType = CommandType.StoredProcedure;
                Comando.CommandText = "sp_AgregarInformacionPagoTarjetaPragaIntegracion";

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
    }
}
