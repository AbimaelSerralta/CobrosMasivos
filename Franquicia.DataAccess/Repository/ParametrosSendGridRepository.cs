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
    public class ParametrosSendGridRepository : SqlDataRepository
    {
        private ParametrosSendGrid _parametrosSendGrid = new ParametrosSendGrid();
        public ParametrosSendGrid parametrosSendGrid
        {
            get { return _parametrosSendGrid; }
            set { _parametrosSendGrid = value; }
        }


        #region MetodosFranquicias
        public void CargarParametrosSendGrid()
        {
            parametrosSendGrid = new ParametrosSendGrid();
        }

        public void ObtenerParametrosSendGrid()
        {
            parametrosSendGrid = new ParametrosSendGrid();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from ParametrosSendGrid";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                parametrosSendGrid.UidParametroSendGrid = Guid.Parse(item["UidParametroSendGrid"].ToString());
                parametrosSendGrid.VchApiKey = item["VchApiKey"].ToString();
            }
        }
        public bool RegistrarParametrosSendGrid(ParametrosSendGrid parametrosSendGrid)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ParametrosSendGridRegistrar"; //NoCreado

                comando.Parameters.Add("@VchApiKey", SqlDbType.VarChar);
                comando.Parameters["@VchApiKey"].Value = parametrosSendGrid.VchApiKey;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        public bool ActualizarParametrosSendGrid(ParametrosSendGrid parametrosSendGrid)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ParametrosSendGridActualizar"; //NoCreado

                comando.Parameters.Add("@VchApiKey", SqlDbType.VarChar);
                comando.Parameters["@VchApiKey"].Value = parametrosSendGrid.VchApiKey;

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
