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
    public class TarifasRepository : SqlDataRepository
    {
        private TarifasGridViewModel _tarifasGridViewModel = new TarifasGridViewModel();
        public TarifasGridViewModel tarifasGridViewModel
        {
            get { return _tarifasGridViewModel; }
            set { _tarifasGridViewModel = value; }
        }

        public List<TarifasGridViewModel> CargarTarifas()
        {
            List<TarifasGridViewModel> lsTarifasGridViewModel = new List<TarifasGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from Tarifas";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsTarifasGridViewModel.Add(new TarifasGridViewModel()
                {
                    UidTarifa = new Guid(item["UidTarifa"].ToString()),
                    DcmWhatsapp = decimal.Parse(item["DcmWhatsapp"].ToString()),
                    DcmSms = decimal.Parse(item["DcmSms"].ToString()),
                    UidEstatus = new Guid(item["UidEstatus"].ToString())
                });
            }

            return lsTarifasGridViewModel;
        }
        public bool RegistrarTarifas(TarifasGridViewModel tarifasGridViewModel)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_TarifasRegistrar";

                comando.Parameters.Add("@DcmWhatsapp", SqlDbType.Decimal);
                comando.Parameters["@DcmWhatsapp"].Value = tarifasGridViewModel.DcmWhatsapp;
                
                comando.Parameters.Add("@DcmSms", SqlDbType.Decimal);
                comando.Parameters["@DcmSms"].Value = tarifasGridViewModel.DcmSms;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public bool ActualizarTarifas(TarifasGridViewModel tarifasGridViewModel)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_TarifasActualizar";

                comando.Parameters.Add("@UidTarifa", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTarifa"].Value = tarifasGridViewModel.UidTarifa;
                
                comando.Parameters.Add("@DcmWhatsapp", SqlDbType.Decimal);
                comando.Parameters["@DcmWhatsapp"].Value = tarifasGridViewModel.DcmWhatsapp;

                comando.Parameters.Add("@DcmSms", SqlDbType.Decimal);
                comando.Parameters["@DcmSms"].Value = tarifasGridViewModel.DcmSms;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
    }
}
