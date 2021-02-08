using Franquicia.DataAccess.Common;
using Franquicia.Domain.Models.ClubPago;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.DataAccess.Repository.ClubPago
{
    public class CancelacionPagoRepository : SqlDataRepository
    {
        private PagosColegiaturasRepository _pagosColegiaturasRepository = new PagosColegiaturasRepository();
        public PagosColegiaturasRepository pagosColegiaturasRepository
        {
            get { return _pagosColegiaturasRepository; }
            set { _pagosColegiaturasRepository = value; }
        }

        private PagosRepository _pagosRepository = new PagosRepository();
        public PagosRepository pagosRepository
        {
            get { return _pagosRepository; }
            set { _pagosRepository = value; }
        }

        private ColegiaturasRepository _colegiaturasRepository = new ColegiaturasRepository();
        public ColegiaturasRepository colegiaturasRepository
        {
            get { return _colegiaturasRepository; }
            set { _colegiaturasRepository = value; }
        }

        public CancelacionPagoResp ConsultarReferenciaClubPago(string Transaccion, string Fecha, decimal Monto, string Referencia, int Autorizacion)
        {
            CancelacionPagoResp cancelacionPagoResp = new CancelacionPagoResp();

            int codigo = 61;
            string mensaje = "Cancelación fallida";

            try
            {
                SqlCommand query = new SqlCommand();
                query.CommandType = CommandType.Text;

                query.CommandText = "select pcp.*, rcp.UidPagoColegiatura from PagosClubPago pcp, ReferenciasClubPago rcp where pcp.IdReferencia = rcp.IdReferencia and pcp.VchTransaccion = '" + Transaccion + "' and pcp.DcmMonto = '" + Monto + "' and pcp.IdReferencia = '" + Referencia + "' and pcp.VchAutorizacion = '" + Autorizacion + "'";

                DataTable dt = this.Busquedas(query);

                Guid UidPago = Guid.Empty;
                Guid UidPagoColegiatura = Guid.Empty;
                string DtFechaOperacion = "";

                foreach (DataRow item in dt.Rows)
                {
                    UidPago = Guid.Parse(item["UidPago"].ToString());
                    UidPagoColegiatura = Guid.Parse(item["UidPagoColegiatura"].ToString());
                    DtFechaOperacion = DateTime.Parse(item["DtFechaOperacion"].ToString()).ToString("dd/MM/yyyy");
                }

                if (DateTime.Parse(DtFechaOperacion) == DateTime.Parse(Fecha))
                {
                    if (CancelacionPagoClubPago(UidPago, UidPagoColegiatura))
                    {
                        codigo = 0;
                        mensaje = "Cancelación exitosa";
                    }
                }
                else
                {
                    codigo = 60;
                    mensaje = "Cancelación fuera de periodo";
                }
            }
            catch (Exception ex)
            {
                string mns = ex.Message;
            }

            cancelacionPagoResp.codigo = codigo;
            cancelacionPagoResp.mensaje = mensaje;

            return cancelacionPagoResp;
        }

        public bool CancelacionPagoClubPago(Guid UidPago, Guid UidPagoColegiatura)
        {
            bool result = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PagoClubPagoCancelacion";

                comando.Parameters.Add("@UidPago", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPago"].Value = UidPago;

                comando.Parameters.Add("@UidPagoEstatus", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPagoEstatus"].Value = Guid.Parse("40704C0D-515C-4985-B2A7-614917E8831A");

                if (this.ManipulacionDeDatos(comando))
                {
                    if (pagosColegiaturasRepository.ActualizarEstatusFechaPago(UidPagoColegiatura, Guid.Parse("F25E4AAB-6044-46E9-A575-98DCBCCF7604")))
                    {
                        ValidarColegiatura(UidPagoColegiatura);

                        result = true;
                    }
                }

            }
            catch (Exception ex)
            {
                result = false;
                string mnsj = ex.Message;
            }

            return result;
        }
        public void ValidarColegiatura(Guid UidPagoColegiatura)
        {
            var data = pagosRepository.ConsultarDatosValidarPago(UidPagoColegiatura);

            Guid UidClienteLocal = Guid.Parse(data.Item1);
            Guid UidUsuario = Guid.Parse(data.Item2);
            Guid UidFechaColegiatura = Guid.Parse(data.Item3);
            Guid UidAlumno = Guid.Parse(data.Item4);

            //Necesito saber el importe de la colegiatura
            decimal ImporteCole = colegiaturasRepository.ObtenerDatosFechaColegiatura(UidClienteLocal, UidUsuario, UidFechaColegiatura, UidAlumno);

            //Necesito saber el importe de todos los pagos
            decimal ImportePagado = pagosColegiaturasRepository.ObtenerPagosPadresRLE(UidFechaColegiatura, UidAlumno);
            decimal ImportePendiente = pagosColegiaturasRepository.ObtenerPendientesPadresRLE(UidFechaColegiatura, UidAlumno);

            // ==>Validar con importe<==
            if (ImporteCole == ImportePagado) //el importeColegiatura es igual al importe de todos los pagos con estatus aprobado
            {
                //Se cambia el estatus de la colegiatura a pagado.
                colegiaturasRepository.ActualizarEstatusFeColegiaturaAlumno(UidFechaColegiatura, UidAlumno, Guid.Parse("605A7881-54E0-47DF-8398-EDE080F4E0AA"), true);
            }
            else if (ImporteCole == (ImportePagado + ImportePendiente)) //el importe de los pagos aprobado y pendiente es igual al importe la colegiatura
            {
                // La colegiatura mantiene el estatus en proceso
                colegiaturasRepository.ActualizarEstatusFeColegiaturaAlumno(UidFechaColegiatura, UidAlumno, Guid.Parse("5554CE57-1288-46D5-B36A-8AC69CB94B9A"), true);
            }
            else
            {
                // La colegiatura regresa al ultimo estatus
                DateTime HoraServidor = DateTime.Now;
                DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");
                string UidEstatus = colegiaturasRepository.ObtenerEstatusColegiaturasRLE(hoy, UidFechaColegiatura, UidAlumno);
                colegiaturasRepository.ActualizarEstatusFeColegiaturaAlumno(UidFechaColegiatura, UidAlumno, Guid.Parse(UidEstatus.ToString()), false);
            }
        }
    }
}
