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
    public class SuperPromocionesRepository : SqlDataRepository
    {
        public List<Promociones> CargarPromociones()
        {
            List<Promociones> lsPromociones = new List<Promociones>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from promociones order by IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsPromociones.Add(new Promociones()
                {
                    UidPromocion = new Guid(item["UidPromocion"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString()
                });
            }

            return lsPromociones;
        }

        #region SuperPromociones
        public List<SuperPromocionesDisponiblesViewModel> CargarSuperPromocionesDisponible()
        {
            List<SuperPromocionesDisponiblesViewModel> lsSuperPromocionesDisponiblesViewModel = new List<SuperPromocionesDisponiblesViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select pr.* from SuperPromociones sp, Promociones pr where sp.UidPromocion = pr.UidPromocion order by IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsSuperPromocionesDisponiblesViewModel.Add(new SuperPromocionesDisponiblesViewModel()
                {
                    UidPromocion = new Guid(item["UidPromocion"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString()
                });
            }

            return lsSuperPromocionesDisponiblesViewModel;
        }
        public List<SuperPromociones> CargarSuperPromociones()
        {
            List<SuperPromociones> lsSuperPromociones = new List<SuperPromociones>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from SuperPromociones";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsSuperPromociones.Add(new SuperPromociones()
                {
                    UidSuperPromocion = new Guid(item["UidSuperPromocion"].ToString()),
                    UidPromocion = new Guid(item["UidPromocion"].ToString()),
                    DcmComicion = decimal.Parse(item["DcmComicion"].ToString()),
                    DcmApartirDe = decimal.Parse(item["DcmApartirDe"].ToString())
                });
            }

            return lsSuperPromociones;
        }
        public bool RegistrarSuperPromociones(SuperPromociones superPromociones)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PromocionesSuperRegistrar";

                comando.Parameters.Add("@UidPromocion", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPromocion"].Value = superPromociones.UidPromocion;

                comando.Parameters.Add("@DcmComicion", SqlDbType.Decimal);
                comando.Parameters["@DcmComicion"].Value = superPromociones.DcmComicion;
                
                comando.Parameters.Add("@DcmApartirDe", SqlDbType.Decimal);
                comando.Parameters["@DcmApartirDe"].Value = superPromociones.DcmApartirDe;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public bool EliminarSuperPromociones()
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PromocionesSuperEliminar";

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
