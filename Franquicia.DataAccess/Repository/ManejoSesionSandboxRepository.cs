using Franquicia.DataAccess.Common;
using Franquicia.Domain.Models;
using Franquicia.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Franquicia.DataAccess.Repository
{
    public class ManejoSesionSandboxRepository : SqlDataRepository
    {
        IntegracionesCompleto _integracionesCompleto = new IntegracionesCompleto();
        public IntegracionesCompleto integracionesCompleto
        {
            get { return _integracionesCompleto; }
            set { _integracionesCompleto = value; }
        }


        public bool LoginUsuario(string Usuario, string Password)
        {
            bool resultado = false;
            SqlCommand comando = new SqlCommand();

            try
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.CommandText = "sp_ManejoDeSessionSandboxLoguearUsuario";

                comando.Parameters.Add("@Usuario", SqlDbType.VarChar);
                comando.Parameters["@Usuario"].Value = Usuario;

                comando.Parameters.Add("@Password", SqlDbType.VarChar);
                comando.Parameters["@Password"].Value = Password;

                DataTable dt = this.Busquedas(comando);

                foreach (DataRow item in dt.Rows)
                {
                    integracionesCompleto = new IntegracionesCompleto()
                    {
                        UidIntegracion = item.IsNull("UidIntegracion") ? Guid.Empty : Guid.Parse(item["UidIntegracion"].ToString()),
                        IdIntegracion = item.IsNull("IdIntegracion") ? 0 : int.Parse(item["IdIntegracion"].ToString()),
                        VchIdentificador = item["VchIdentificador"].ToString(),
                        UidEstatus = item.IsNull("UidEstatus") ? Guid.Empty : Guid.Parse(item["UidEstatus"].ToString()),
                        UidCredencial = item.IsNull("UidCredencial") ? Guid.Empty : Guid.Parse(item["UidCredencial"].ToString())
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

        public string ObtenerModuloInicial(Guid UidCredencial)
        {
            string Home = string.Empty;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select top 1 smi.VchUrl from AccesosIntegraciones ai, Integraciones inte, CredenSandbox cs, SegModulosIntegraciones smi where ai.UidIntegracion = inte.UidIntegracion and ai.UidSegModulo = smi.UidSegModulo and inte.UidIntegracion = cs.UidIntegracion and cs.UidCredencial = '" + UidCredencial + "' order by smi.IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                Home = item["VchUrl"].ToString();
            }
            return Home;
        }
        public List<ModulosSandbox> CargarAccesosPermitidos(Guid UidCredencial)
        {
            List<ModulosSandbox> lsModulosSandbox = new List<ModulosSandbox>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from Integraciones inte, SegModulosIntegraciones smi, AccesosIntegraciones ai, CredenSandbox cs where cs.UidIntegracion = inte.UidIntegracion and inte.UidIntegracion = ai.UidIntegracion and smi.UidSegModulo = ai.UidSegModulo and cs.UidCredencial = '" + UidCredencial + "' order by smi.IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsModulosSandbox.Add(new ModulosSandbox
                {
                    UidSegModulo = item.IsNull("UidSegModulo") ? Guid.Empty : Guid.Parse(item["UidSegModulo"].ToString()),
                    VchNombre = item["VchNombre"].ToString(),
                    VchUrl = item["VchUrl"].ToString(),
                    VchIcono = item["VchIcono"].ToString(),
                    IntGerarquia = int.Parse(item["IntGerarquia"].ToString())
                });
            }
            return lsModulosSandbox;
        }
    }
}
