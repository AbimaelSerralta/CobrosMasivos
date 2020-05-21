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
    public class PromocionesRepository : SqlDataRepository
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

        #region Franquicias
        public List<FranquiciasCBLPromocionesModel> CargarPromocionesFranquicia(Guid UidFranquicia)
        {
            List<FranquiciasCBLPromocionesModel> lsFranquiciasCBLPromocionesModel = new List<FranquiciasCBLPromocionesModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select fp.* from Franquiciatarios fr, FranquiciasPromociones fp where fr.UidFranquiciatarios = fp.UidFranquicia and fr.UidFranquiciatarios = '" + UidFranquicia + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsFranquiciasCBLPromocionesModel.Add(new FranquiciasCBLPromocionesModel()
                {
                    UidFranquiciaPromocion = new Guid(item["UidFranquiciaPromocion"].ToString()),
                    UidFranquicia = new Guid(item["UidFranquicia"].ToString()),
                    UidPromocion = new Guid(item["UidPromocion"].ToString()),
                    DcmComicion = decimal.Parse(item["DcmComicion"].ToString())
                });
            }

            return lsFranquiciasCBLPromocionesModel;
        }
        public bool EliminarPromocionesFranquicias(FranquiciasCBLPromocionesModel cBLPromocionesModel)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PromocionesFranquiciaEliminar";

                comando.Parameters.Add("@UidFranquicia", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidFranquicia"].Value = cBLPromocionesModel.UidFranquicia;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public bool RegistrarPromocionesFranquicias(FranquiciasCBLPromocionesModel cBLPromocionesModel)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PromocionesFranquiciaRegistrar";

                comando.Parameters.Add("@UidFranquicia", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidFranquicia"].Value = cBLPromocionesModel.UidFranquicia;

                comando.Parameters.Add("@UidPromocion", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPromocion"].Value = cBLPromocionesModel.UidPromocion;

                comando.Parameters.Add("@DcmComicion", SqlDbType.Decimal);
                comando.Parameters["@DcmComicion"].Value = cBLPromocionesModel.DcmComicion;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public List<FranquiciasCBLPromocionesModel> CargarPromocionesFranquicias(Guid UidFranquicia)
        {
            List<FranquiciasCBLPromocionesModel> lsFranquiciasCBLPromocionesModel = new List<FranquiciasCBLPromocionesModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select pr.*, fp.DcmComicion from Franquiciatarios fr, FranquiciasPromociones fp, Promociones pr where pr.UidPromocion = fp.UidPromocion and fr.UidFranquiciatarios = fp.UidFranquicia and fr.UidFranquiciatarios = '" + UidFranquicia + "' order by IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsFranquiciasCBLPromocionesModel.Add(new FranquiciasCBLPromocionesModel()
                {
                    UidPromocion = new Guid(item["UidPromocion"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    DcmComicion = decimal.Parse(item["DcmComicion"].ToString())
                });
            }
            return lsFranquiciasCBLPromocionesModel;
        }
        #endregion

        #region Empresa
        public List<CBLPromocionesModel> CargarPromociones(Guid UidCliente)
        {
            List<CBLPromocionesModel> lsClientesCBLPromocionesModel = new List<CBLPromocionesModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select cp.* from Clientes cl, ClientesPromociones cp where cl.UidCliente = cp.UidCliente and cl.UidCliente = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsClientesCBLPromocionesModel.Add(new CBLPromocionesModel()
                {
                    UidClientePromocion = new Guid(item["UidClientePromocion"].ToString()),
                    UidCliente = new Guid(item["UidCliente"].ToString()),
                    UidPromocion = new Guid(item["UidPromocion"].ToString()),
                    DcmComicion = decimal.Parse(item["DcmComicion"].ToString())
                });
            }

            return lsClientesCBLPromocionesModel;
        }
        public bool RegistrarPromociones(CBLPromocionesModel cBLPromocionesModel)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PromocionesRegistrar";

                comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCliente"].Value = cBLPromocionesModel.UidCliente;

                comando.Parameters.Add("@UidPromocion", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPromocion"].Value = cBLPromocionesModel.UidPromocion;

                comando.Parameters.Add("@DcmComicion", SqlDbType.Decimal);
                comando.Parameters["@DcmComicion"].Value = cBLPromocionesModel.DcmComicion;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public bool EliminarPromociones(CBLPromocionesModel cBLPromocionesModel)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PromocionesEliminar";

                comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCliente"].Value = cBLPromocionesModel.UidCliente;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        #endregion

        #region Clientes
        public List<CBLPromocionesModel> CargarPromocionesClientes(Guid UidCliente)
        {
            List<CBLPromocionesModel> lsClientesCBLPromocionesModel = new List<CBLPromocionesModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select pr.*, cp.DcmComicion from Clientes cl, ClientesPromociones cp, Promociones pr where pr.UidPromocion = cp.UidPromocion and cl.UidCliente = cp.UidCliente and cl.UidCliente = '" + UidCliente + "' order by IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsClientesCBLPromocionesModel.Add(new CBLPromocionesModel()
                {
                    UidPromocion = new Guid(item["UidPromocion"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    DcmComicion = decimal.Parse(item["DcmComicion"].ToString())
                });
            }
            return lsClientesCBLPromocionesModel;
        }

        #region Eventos
        public List<EventosGenerarLigasModel> CargarPromocionesEvento(Guid UidCliente, Guid UidEvento)
        {
            List<EventosGenerarLigasModel> lsEventosGenerarLigasModel = new List<EventosGenerarLigasModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select pr.*, cp.* from Eventos ev, EventosPromociones ep, Promociones pr, Clientes cl, ClientesPromociones cp where ev.UidEvento = ep.UidEvento and ep.UidPromocion = pr.UidPromocion and cl.UidCliente = cp.UidCliente and cp.UidPromocion = pr.UidPromocion and cl.UidCliente = '" + UidCliente + "' and ev.UidEvento = '" + UidEvento + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsEventosGenerarLigasModel.Add(new EventosGenerarLigasModel()
                {
                    UidPromocion = new Guid(item["UidPromocion"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    DcmComicion = decimal.Parse(item["DcmComicion"].ToString()),
                    IntGerarquia = int.Parse(item["IntGerarquia"].ToString())
                });
            }
            return lsEventosGenerarLigasModel.OrderBy(x => x.IntGerarquia).ToList();
        }
        public List<EventosPromocionesModel> ObtenerPromocionesEvento(Guid UidEvento)
        {
            List<EventosPromocionesModel> lsEventosPromocionesModel = new List<EventosPromocionesModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select ep.* from Eventos ev, EventosPromociones ep where ev.UidEvento = ep.UidEvento and ev.UidEvento = '" + UidEvento + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsEventosPromocionesModel.Add(new EventosPromocionesModel()
                {
                    UidPromocion = new Guid(item["UidPromocion"].ToString())
                });
            }
            return lsEventosPromocionesModel;
        }
        #endregion
        #endregion

        #region PromocionesValidas
        public List<LigasUrlsPromocionesModel> CargarPromocionesValidas(Guid UidLigaAsociado)
        {
            List<LigasUrlsPromocionesModel> lsLigasUrlsPromocionesModel = new List<LigasUrlsPromocionesModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select lu.*, pr.VchDescripcion, pr.IntGerarquia from LigasUrls lu, Promociones pr where lu.UidPromocion = pr.UidPromocion and lu.UidLigaAsociado = '" + UidLigaAsociado  +  "' order by IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsLigasUrlsPromocionesModel.Add(new LigasUrlsPromocionesModel()
                {
                    UidLigaUrl = Guid.Parse(item["UidLigaUrl"].ToString()),
                    VchUrl = item["VchUrl"].ToString(),
                    VchConcepto = item["VchConcepto"].ToString(),
                    IdReferencia = item["IdReferencia"].ToString(),
                    DtVencimiento = DateTime.Parse(item["DtVencimiento"].ToString()),
                    DcmImporte = decimal.Parse(item["DcmImporte"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString()
                });
            }

            return lsLigasUrlsPromocionesModel;
        }

        public List<LigasUrlsPromocionesModel> ValidarPromociones(Guid UidLigaAsociado)
        {
            List<LigasUrlsPromocionesModel> lsLigasUrlsPromocionesModel = new List<LigasUrlsPromocionesModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from LigasUrls where UidLigaAsociado = '" + UidLigaAsociado + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsLigasUrlsPromocionesModel.Add(new LigasUrlsPromocionesModel()
                {
                    UidLigaUrl = Guid.Parse(item["UidLigaUrl"].ToString()),
                    VchUrl = item["VchUrl"].ToString(),
                    VchConcepto = item["VchConcepto"].ToString(),
                    IdReferencia = item["IdReferencia"].ToString(),
                    DtVencimiento = DateTime.Parse(item["DtVencimiento"].ToString()),
                    DcmImporte = decimal.Parse(item["DcmImporte"].ToString())
                });
            }

            return lsLigasUrlsPromocionesModel;
        }
        #endregion
    }
}
