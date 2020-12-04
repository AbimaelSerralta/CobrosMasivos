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
    public class PagosEfectivosRepository : SqlDataRepository
    {
        PagosEfectivos _pagosEfectivos = new PagosEfectivos();
        public PagosEfectivos PagosEfectivos
        {
            get { return _pagosEfectivos; }
            set { _pagosEfectivos = value; }
        }

        #region Metodos ReporteLigasEscuela
        public bool RegistrarPagoEfectivo(PagosEfectivos pagosEfectivos)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PagosEfectivosRegistrar";

                comando.Parameters.Add("@UidPagoColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPagoColegiatura"].Value = pagosEfectivos.UidPagoColegiatura;

                comando.Parameters.Add("@DtFHPago", SqlDbType.DateTime);
                comando.Parameters["@DtFHPago"].Value = pagosEfectivos.DtFHPago;

                comando.Parameters.Add("@DcmImporte", SqlDbType.Decimal);
                comando.Parameters["@DcmImporte"].Value = pagosEfectivos.DcmImporte;

                comando.Parameters.Add("@BitTipoTarjeta", SqlDbType.Bit);
                comando.Parameters["@BitTipoTarjeta"].Value = pagosEfectivos.BitTipoTarjeta;

                if (pagosEfectivos.UidTipoTarjeta != Guid.Empty)
                {
                    comando.Parameters.Add("@UidTipoTarjeta", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidTipoTarjeta"].Value = pagosEfectivos.UidTipoTarjeta;
                }

                comando.Parameters.Add("@BitPromocionTT", SqlDbType.Bit);
                comando.Parameters["@BitPromocionTT"].Value = pagosEfectivos.BitPromocionTT;

                if (pagosEfectivos.UidPromocionTerminal != Guid.Empty)
                {
                    comando.Parameters.Add("@UidPromocionTerminal", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidPromocionTerminal"].Value = pagosEfectivos.UidPromocionTerminal;
                }

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public bool ActualizarPagosEfectivos(Alumnos Alumnos, TelefonosAlumnos telefonosAlumnos)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_AlumnosActualizar";


                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public bool EliminarPagosEfectivos(Guid UidPagoColegiatura)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PagosColegiaturasEliminar";

                comando.Parameters.Add("@UidPagoColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPagoColegiatura"].Value = UidPagoColegiatura;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }

        public List<rdlcPagosEfectivosViewModels> ObtenerPagoEfectivoRLE(Guid UidPagoColegiatura)
        {
            List<rdlcPagosEfectivosViewModels> lsrdlcPagosEfectivosViewModels = new List<rdlcPagosEfectivosViewModels>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select pc.DcmTotal, al.VchNombres, al.VchApePaterno, al.VchApeMaterno, al.VchMatricula, PC.DtFHPago, pe.*, fop.VchDescripcion as FormaPago,  pt.VchDescripcion as PromocionTerminal, tt.VchDescripcion as TipoTarjeta from PagosEfectivos pe left join PromocionesTerminal pt on pt.UidPromocionTerminal = pe.UidPromocionTerminal left join TiposTarjetas tt on tt.UidTipoTarjeta = pe.UidTipoTarjeta left join PagosColegiaturas pc on pc.UidPagoColegiatura = pe.UidPagoColegiatura left join FechasPagos fp on fp.UidPagoColegiatura = pc.UidPagoColegiatura left join FormasPagos fop on fop.UidFormaPago = fp.UidFormaPago left join Alumnos al on al.UidAlumno = fp.UidAlumno where pe.UidPagoColegiatura = '" + UidPagoColegiatura + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsrdlcPagosEfectivosViewModels.Add(new rdlcPagosEfectivosViewModels()
                {
                    VchMatricula = item["VchMatricula"].ToString(),
                    NombreCompleto = item["VchNombres"].ToString() + " " + item["VchApePaterno"].ToString() + " " + item["VchApeMaterno"].ToString(),
                    DtFHPago = DateTime.Parse(item["DtFHPago"].ToString()),
                    DcmTotal = decimal.Parse(item["DcmTotal"].ToString()),
                    FormaPago = item["FormaPago"].ToString(),

                    UidPagosEfectivos = Guid.Parse(item["UidPagosEfectivos"].ToString()),
                    BitTipoTarjeta = bool.Parse(item["BitPromocionTT"].ToString()),
                    TipoTarjeta = item.IsNull("TipoTarjeta") ? "N/A" : item["TipoTarjeta"].ToString(),
                    BitPromocionTT = bool.Parse(item["BitPromocionTT"].ToString()),
                    PromocionTerminal = item.IsNull("PromocionTerminal") ? "N/A" : item["PromocionTerminal"].ToString()
                });
            }

            return lsrdlcPagosEfectivosViewModels;
        }

        #endregion
    }
}
