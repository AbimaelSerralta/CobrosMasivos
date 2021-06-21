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
    public class ImporteLigaMinMaxRepository : SqlDataRepository
    {
        private ImporteLigaMinMax _importeLigaMinMax = new ImporteLigaMinMax();
        public ImporteLigaMinMax importeLigaMinMax
        {
            get { return _importeLigaMinMax; }
            set { _importeLigaMinMax = value; }
        }

        public List<ImporteLigaMinMax> CargarImporteLigaMinMax()
        {
            List<ImporteLigaMinMax> lsImporteLigaMinMax = new List<ImporteLigaMinMax>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from ImporteLigaMinMax";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsImporteLigaMinMax.Add(new ImporteLigaMinMax()
                {
                    UidImporteLiga = new Guid(item["UidImporteLiga"].ToString()),
                    DcmImporteMin = decimal.Parse(item["DcmImporteMin"].ToString()),
                    DcmImporteMax = decimal.Parse(item["DcmImporteMax"].ToString())
                });
            }

            return lsImporteLigaMinMax;
        }
        public bool RegistrarImporteLigaMinMax(ImporteLigaMinMax importeLigaMinMax)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ImporteLigaMinMaxSupRegistrar";

                comando.Parameters.Add("@DcmImporteMin", SqlDbType.Decimal);
                comando.Parameters["@DcmImporteMin"].Value = importeLigaMinMax.DcmImporteMin;

                comando.Parameters.Add("@DcmImporteMax", SqlDbType.Decimal);
                comando.Parameters["@DcmImporteMax"].Value = importeLigaMinMax.DcmImporteMax;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public bool ActualizarImporteLigaMinMax(ImporteLigaMinMax importeLigaMinMax)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ImporteLigaMinMaxSupActualizar";

                comando.Parameters.Add("@DcmImporteMin", SqlDbType.Decimal);
                comando.Parameters["@DcmImporteMin"].Value = importeLigaMinMax.DcmImporteMin;

                comando.Parameters.Add("@DcmImporteMax", SqlDbType.Decimal);
                comando.Parameters["@DcmImporteMax"].Value = importeLigaMinMax.DcmImporteMax;

                comando.Parameters.Add("@UidImporteLiga", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidImporteLiga"].Value = importeLigaMinMax.UidImporteLiga;

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
