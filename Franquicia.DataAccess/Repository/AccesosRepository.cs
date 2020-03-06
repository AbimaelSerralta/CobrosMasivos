using Franquicia.DataAccess.Common;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.DataAccess.Repository
{
    public class AccesosRepository : SqlDataRepository
    {
        private Accesos _accesos = new Accesos();

        public Accesos accesos
        {
            get { return _accesos; }
            set { _accesos = value; }
        }


        public bool validarModuloPerfil(Guid parametro, string VchUrl)
        {
            bool resultado = false;
            SqlCommand comando = new SqlCommand();

            try
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.CommandText = "sp_ManejoDeSessionAccesoModulos";

                comando.Parameters.Add("@UIDPerfil", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UIDPerfil"].Value = parametro;

                comando.Parameters.Add("@VchUrl", SqlDbType.VarChar, 30);
                comando.Parameters["@VchUrl"].Value = VchUrl;

                foreach (DataRow item in this.Busquedas(comando).Rows)
                {
                    accesos = new Accesos()
                    {
                        UidPerfil = new Guid(item["UidPerfil"].ToString()),
                        UidModulo = new Guid(item["UidModulo"].ToString()),
                        VchURL = item["VchURL"].ToString(),
                    };
            }

                resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return resultado;

        }
    }
}
