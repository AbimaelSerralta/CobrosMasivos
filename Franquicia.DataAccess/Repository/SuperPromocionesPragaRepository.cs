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
    public class SuperPromocionesPragaRepository : SqlDataRepository
    {
        public List<PromocionesPraga> CargarPromocionesPraga()
        {
            List<PromocionesPraga> lsPromocionesPraga = new List<PromocionesPraga>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from PromocionesPraga order by IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsPromocionesPraga.Add(new PromocionesPraga()
                {
                    UidPromocion = Guid.Parse(item["UidPromocion"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString()
                });
            }

            return lsPromocionesPraga;
        }

        #region SuperPromocionesPraga
        public List<SuperPromocionesPragaDisponiblesViewModel> CargarSuperPromocionesPragaDisponible()
        {
            List<SuperPromocionesPragaDisponiblesViewModel> lsSuperPromocionesPragaDisponiblesViewModel = new List<SuperPromocionesPragaDisponiblesViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select pr.* from SuperPromocionesPraga sp, PromocionesPraga pr where sp.UidPromocion = pr.UidPromocion order by IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsSuperPromocionesPragaDisponiblesViewModel.Add(new SuperPromocionesPragaDisponiblesViewModel()
                {
                    UidPromocion = Guid.Parse(item["UidPromocion"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString()
                });
            }

            return lsSuperPromocionesPragaDisponiblesViewModel;
        }
        public List<SuperPromocionesPraga> CargarSuperPromocionesPraga()
        {
            List<SuperPromocionesPraga> lsSuperPromocionesPraga = new List<SuperPromocionesPraga>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from SuperPromocionesPraga";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsSuperPromocionesPraga.Add(new SuperPromocionesPraga()
                {
                    UidSuperPromocion = Guid.Parse(item["UidSuperPromocion"].ToString()),
                    UidPromocion = Guid.Parse(item["UidPromocion"].ToString()),
                    DcmComicion = decimal.Parse(item["DcmComicion"].ToString()),
                    DcmApartirDe = decimal.Parse(item["DcmApartirDe"].ToString()),
                    UidTipoTarjeta = Guid.Parse(item["UidTipoTarjeta"].ToString())
                });
            }

            return lsSuperPromocionesPraga;
        }
        public bool RegistrarSuperPromocionesPraga(SuperPromocionesPraga superPromocionesPraga)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_SuperPromocionesPragaRegistrar";

                comando.Parameters.Add("@UidPromocion", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPromocion"].Value = superPromocionesPraga.UidPromocion;

                comando.Parameters.Add("@DcmComicion", SqlDbType.Decimal);
                comando.Parameters["@DcmComicion"].Value = superPromocionesPraga.DcmComicion;
                
                comando.Parameters.Add("@DcmApartirDe", SqlDbType.Decimal);
                comando.Parameters["@DcmApartirDe"].Value = superPromocionesPraga.DcmApartirDe;
                
                comando.Parameters.Add("@UidTipoTarjeta", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoTarjeta"].Value = superPromocionesPraga.UidTipoTarjeta;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public bool EliminarSuperPromocionesPraga()
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_SuperPromocionesPragaEliminar";

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        
        public bool RegistrarCodigoPromocionesPraga(string VchCodigo, Guid UidPromocion, Guid UidTipoTarjeta)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_CodigoPromocionesPragaRegistrar";

                comando.Parameters.Add("@VchCodigo", SqlDbType.VarChar);
                comando.Parameters["@VchCodigo"].Value = VchCodigo;
                
                comando.Parameters.Add("@UidPromocion", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPromocion"].Value = UidPromocion;
                
                comando.Parameters.Add("@UidTipoTarjeta", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoTarjeta"].Value = UidTipoTarjeta;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public bool EliminarCodigoPromocionesPraga()
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_CodigoPromocionesPragaEliminar";

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
