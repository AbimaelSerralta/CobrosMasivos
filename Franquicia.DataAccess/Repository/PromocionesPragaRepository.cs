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
    public class PromocionesPragaRepository : SqlDataRepository
    {
        public List<PromocionesPraga> CargarPromociones()
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
                    UidPromocion = new Guid(item["UidPromocion"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString()
                });
            }

            return lsPromocionesPraga;
        }

        #region Metodos Panel Administrativo

        #region Escuela
        public List<CBLPromocionesPragaViewModel> CargarPromocionesCliente(Guid UidCliente, Guid UidTipoTarjeta)
        {
            List<CBLPromocionesPragaViewModel> lsCBLPromocionesPragaViewModel = new List<CBLPromocionesPragaViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select cpp.* from Clientes cl, ClientesPromocionesPraga cpp where cl.UidCliente = cpp.UidCliente and cl.UidCliente = '" + UidCliente + "' and cpp.UidTipoTarjeta = '" + UidTipoTarjeta + "'";

            DataTable dt = this.Busquedas(query);


            SqlCommand query2 = new SqlCommand();
            query2.CommandType = CommandType.Text;

            query2.CommandText = "select * from PromocionesPraga order by IntGerarquia asc";

            DataTable dtPT = this.Busquedas(query2);

            foreach (DataRow itemPT in dtPT.Rows)
            {
                decimal DcmComicion = decimal.Parse("0.00");
                decimal DcmApartirDe = decimal.Parse("0.00");
                bool blChecked = false;

                foreach (DataRow item in dt.Rows)
                {
                    if (Guid.Parse(itemPT["UidPromocion"].ToString()) == Guid.Parse(item["UidPromocion"].ToString()))
                    {
                        DcmComicion = decimal.Parse(item["DcmComicion"].ToString());
                        DcmApartirDe = decimal.Parse(item["DcmApartirDe"].ToString());
                        blChecked = true;
                    }
                }

                lsCBLPromocionesPragaViewModel.Add(new CBLPromocionesPragaViewModel()
                {
                    UidPromocion = Guid.Parse(itemPT["UidPromocion"].ToString()),
                    VchDescripcion = itemPT["VchDescripcion"].ToString(),
                    DcmComicion = DcmComicion,
                    DcmApartirDe = DcmApartirDe,
                    blChecked = blChecked
                });
            }

            return lsCBLPromocionesPragaViewModel;
        }
        public bool RegistrarPromocionesCliente(CBLPromocionesPragaViewModel cBLPromocionesPragaViewModel)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ClientesPromocionesPragaRegistrar";

                comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCliente"].Value = cBLPromocionesPragaViewModel.UidCliente;

                comando.Parameters.Add("@UidPromocion", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPromocion"].Value = cBLPromocionesPragaViewModel.UidPromocion;

                comando.Parameters.Add("@DcmComicion", SqlDbType.Decimal);
                comando.Parameters["@DcmComicion"].Value = cBLPromocionesPragaViewModel.DcmComicion;

                comando.Parameters.Add("@DcmApartirDe", SqlDbType.Decimal);
                comando.Parameters["@DcmApartirDe"].Value = cBLPromocionesPragaViewModel.DcmApartirDe;

                comando.Parameters.Add("@UidTipoTarjeta", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoTarjeta"].Value = cBLPromocionesPragaViewModel.UidTipoTarjeta;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public bool EliminarPromocionesCliente(Guid UidCliente)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ClientesPromocionesPragaEliminar";

                comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCliente"].Value = UidCliente;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        #endregion

        #region Tarifas
        public List<CBLSuperPromocionesPragaViewModel> CargarSuperPromociones(Guid UidTipoTarjeta)
        {
            List<CBLSuperPromocionesPragaViewModel> lsCBLSuperPromocionesPragaViewModel = new List<CBLSuperPromocionesPragaViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from ClientesPromocionesPraga where UidTipoTarjeta = '" + UidTipoTarjeta + "'";

            DataTable dt = this.Busquedas(query);


            SqlCommand query2 = new SqlCommand();
            query2.CommandType = CommandType.Text;

            query2.CommandText = "select pp.*, cpp.VchCodigo from TiposTarjetasPraga ttp, CodigoPromocionesPraga cpp, PromocionesPraga pp where ttp.UidTipoTarjeta = cpp.UidTipoTarjeta and cpp.UidPromocion = pp.UidPromocion and ttp.UidTipoTarjeta = '" + UidTipoTarjeta + "' order by pp.IntGerarquia asc";

            DataTable dtPT = this.Busquedas(query2);

            foreach (DataRow itemPT in dtPT.Rows)
            {
                decimal DcmComicion = decimal.Parse("0.00");
                decimal DcmApartirDe = decimal.Parse("0.00");
                bool blChecked = false;

                foreach (DataRow item in dt.Rows)
                {
                    if (Guid.Parse(itemPT["UidPromocion"].ToString()) == Guid.Parse(item["UidPromocion"].ToString()))
                    {
                        DcmComicion = decimal.Parse(item["DcmComicion"].ToString());
                        DcmApartirDe = decimal.Parse(item["DcmApartirDe"].ToString());
                        blChecked = true;
                    }
                }

                lsCBLSuperPromocionesPragaViewModel.Add(new CBLSuperPromocionesPragaViewModel()
                {
                    UidPromocion = Guid.Parse(itemPT["UidPromocion"].ToString()),
                    VchDescripcion = itemPT["VchDescripcion"].ToString(),
                    DcmComicion = DcmComicion,
                    DcmApartirDe = DcmApartirDe,
                    VchCodigo = itemPT["VchCodigo"].ToString(),
                    blChecked = blChecked
                });
            }

            return lsCBLSuperPromocionesPragaViewModel;
        }
        #endregion
        #endregion

        #region Metodos Escuela

        #region Colegiatura
        public List<PromocionesColegiaturaModel> ObtenerPromocionesColegiatura(Guid UidColegiatura)
        {
            List<PromocionesColegiaturaModel> lsPromocionesColegiaturaModel = new List<PromocionesColegiaturaModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from Colegiaturas co, ColegiaturasPromociones cp where co.UidColegiatura = cp.UidColegiatura and co.UidColegiatura = '" + UidColegiatura + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsPromocionesColegiaturaModel.Add(new PromocionesColegiaturaModel()
                {
                    UidPromocion = new Guid(item["UidPromocion"].ToString())
                });
            }
            return lsPromocionesColegiaturaModel;
        }
        #endregion

        #region Pagos
        public List<CBLPromocionesPragaViewModel> CargarPromocionesPagosImporte(Guid UidCliente, Guid UidTipoTarjeta, decimal Importe)
        {
            List<CBLPromocionesPragaViewModel> lsCBLPromocionesPragaViewModel = new List<CBLPromocionesPragaViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from PromocionesPraga pp, ClientesPromocionesPraga cpp where pp.UidPromocion = cpp.UidPromocion and UidCliente = '" + UidCliente + "' and UidTipoTarjeta = '" + UidTipoTarjeta + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {

                if (Importe >= decimal.Parse(item["DcmApartirDe"].ToString()))
                {
                    lsCBLPromocionesPragaViewModel.Add(new CBLPromocionesPragaViewModel()
                    {
                        UidPromocion = new Guid(item["UidPromocion"].ToString()),
                        VchDescripcion = item["VchDescripcion"].ToString(),
                        DcmComicion = decimal.Parse(item["DcmComicion"].ToString()),
                        IntGerarquia = int.Parse(item["IntGerarquia"].ToString())
                    });
                }
            }
            return lsCBLPromocionesPragaViewModel.OrderBy(x => x.IntGerarquia).ToList();
        }
        #endregion
        #endregion
    }
}
