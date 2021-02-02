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
    public class PromocionesClubPagoRepository : SqlDataRepository
    {
        public List<PromocionesClubPago> CargarPromociones()
        {
            List<PromocionesClubPago> lsPromocionesClubPago = new List<PromocionesClubPago>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from PromocionesClubPago order by IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsPromocionesClubPago.Add(new PromocionesClubPago()
                {
                    UidPromocion = new Guid(item["UidPromocion"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString()
                });
            }

            return lsPromocionesClubPago;
        }

        #region Empresa
        public List<CBLPromocionesClubPagoViewModel> CargarPromocionesCliente(Guid UidCliente, Guid UidTipoTarjeta)
        {
            List<CBLPromocionesClubPagoViewModel> lsCBLPromocionesClubPagoViewModel = new List<CBLPromocionesClubPagoViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select cpt.* from Clientes cl, ClientesPromocionesClubPago cpcp where cl.UidCliente = cpcp.UidCliente and cl.UidCliente = '" + UidCliente + "' and cpcp.UidTipoTarjeta = '" + UidTipoTarjeta + "'";

            DataTable dt = this.Busquedas(query);


            SqlCommand query2 = new SqlCommand();
            query2.CommandType = CommandType.Text;

            query2.CommandText = "select * from PromocionesClubPago order by IntGerarquia asc";

            DataTable dtPT = this.Busquedas(query2);

            foreach (DataRow itemPT in dtPT.Rows)
            {
                decimal DcmComicion = decimal.Parse("0.00");
                decimal DcmApartirDe = decimal.Parse("0.00");
                bool blChecked = false;

                foreach (DataRow item in dt.Rows)
                {
                    if (Guid.Parse(itemPT["UidPromocionTerminal"].ToString()) == Guid.Parse(item["UidPromocionTerminal"].ToString()))
                    {
                        DcmComicion = decimal.Parse(item["DcmComicion"].ToString());
                        DcmApartirDe = decimal.Parse(item["DcmApartirDe"].ToString());
                        blChecked = true;
                    }
                }

                lsCBLPromocionesClubPagoViewModel.Add(new CBLPromocionesClubPagoViewModel()
                {
                    UidPromocion = Guid.Parse(itemPT["UidPromocion"].ToString()),
                    VchDescripcion = itemPT["VchDescripcion"].ToString(),
                    DcmComicion = DcmComicion,
                    DcmApartirDe = DcmApartirDe,
                    blChecked = blChecked
                });
            }

            return lsCBLPromocionesClubPagoViewModel;
        }
        public bool RegistrarPromocionesCliente(CBLPromocionesClubPagoViewModel cBLPromocionesClubPagoViewModel)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ClientesPromocionesClubPagoRegistrar";

                comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCliente"].Value = cBLPromocionesClubPagoViewModel.UidCliente;

                comando.Parameters.Add("@UidPromocion", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPromocion"].Value = cBLPromocionesClubPagoViewModel.UidPromocion;

                comando.Parameters.Add("@DcmComicion", SqlDbType.Decimal);
                comando.Parameters["@DcmComicion"].Value = cBLPromocionesClubPagoViewModel.DcmComicion;

                comando.Parameters.Add("@DcmApartirDe", SqlDbType.Decimal);
                comando.Parameters["@DcmApartirDe"].Value = cBLPromocionesClubPagoViewModel.DcmApartirDe;

                comando.Parameters.Add("@UidTipoTarjeta", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoTarjeta"].Value = cBLPromocionesClubPagoViewModel.UidTipoTarjeta;

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
                comando.CommandText = "sp_ClientesPromocionesClubPagoEliminar";

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
        public List<CBLPromocionesClubPagoViewModel> CargarPromocionesPagosImporte(Guid UidCliente, Guid UidTipoTarjeta, decimal Importe)
        {
            List<CBLPromocionesClubPagoViewModel> lsCBLPromocionesClubPagoViewModel = new List<CBLPromocionesClubPagoViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from PromocionesTerminal pt, ClientesPromocionesTerminal cpt where pt.UidPromocionTerminal = cpt.UidPromocionTerminal and UidCliente = '" + UidCliente + "' and UidTipoTarjeta = '" + UidTipoTarjeta + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {

                if (Importe >= decimal.Parse(item["DcmApartirDe"].ToString()))
                {
                    lsCBLPromocionesClubPagoViewModel.Add(new CBLPromocionesClubPagoViewModel()
                    {
                        UidPromocion = new Guid(item["UidPromocion"].ToString()),
                        VchDescripcion = item["VchDescripcion"].ToString(),
                        DcmComicion = decimal.Parse(item["DcmComicion"].ToString()),
                        IntGerarquia = int.Parse(item["IntGerarquia"].ToString())
                    });
                }
            }
            return lsCBLPromocionesClubPagoViewModel.OrderBy(x => x.IntGerarquia).ToList();
        }
        #endregion
        #endregion
    }
}
