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
    public class AutorizacionPagoRepository : SqlDataRepository
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

        public AutorizacionPago AutorizacionPagoClubPago(string IdReferencia, DateTime FechaRegistro, string Fecha, decimal Monto, string Transaccion)
        {
            AutorizacionPago autorizacionPago = new AutorizacionPago();

            try
            {
                SqlCommand query = new SqlCommand();
                query.CommandType = CommandType.Text;

                query.CommandText = "select * from ReferenciasClubPago where IdReferencia = '" + IdReferencia + "'";

                DataTable dt = this.Busquedas(query);

                int codigo = 0;
                string mnsj = "Transaccion Exitosa";
                decimal monto = 0;
                string referencia = "";
                string autorizacion = "";

                if (dt.Rows.Count == 0)
                {
                    codigo = 40;
                    mnsj = "Adquiriente inválido";
                    monto = 0;
                }
                else
                {
                    string Vencimiento = "";
                    Guid UidPagoColegiatura = Guid.Empty;

                    foreach (DataRow item in dt.Rows)
                    {
                        monto = decimal.Parse(item["DcmImporte"].ToString());
                        referencia = item["IdReferencia"].ToString();
                        Vencimiento = item["DtVencimiento"].ToString();
                        UidPagoColegiatura = Guid.Parse(item["UidPagoColegiatura"].ToString());
                    }

                    if (DateTime.Parse(DateTime.Parse(Fecha).ToString("dd-MM-yyyy")) > DateTime.Parse(Vencimiento))
                    {
                        codigo = 14;
                        mnsj = "Referencia fuera de vigencia";
                        monto = 0;
                        autorizacion = "";
                    }
                    else
                    {
                        if (monto == Monto)
                        {
                            var correct = ConsultarPagoClubPago();

                            if (correct.Item2)
                            {
                                autorizacion = correct.Item1.ToString();

                                if (RegistrarPagoClubPago(Guid.NewGuid(), referencia, FechaRegistro, DateTime.Parse(Fecha), Monto, Transaccion, autorizacion, Guid.Parse("9F512165-96A6-407F-925A-A27C2149F3B9")))
                                {
                                    if (pagosColegiaturasRepository.ActualizarEstatusFechaPago(UidPagoColegiatura, Guid.Parse("8720B2B9-5712-4E75-A981-932887AACDC9")))
                                    {
                                        ValidarColegiatura(UidPagoColegiatura);
                                    }
                                }
                                else
                                {
                                    codigo = 50;
                                    mnsj = "Error de sistema.";
                                    monto = 0;
                                    autorizacion = "";
                                }
                            }
                            else
                            {
                                codigo = 50;
                                mnsj = "Error de sistema.";
                                monto = 0;
                                autorizacion = "";
                            }
                        }
                        else
                        {
                            codigo = 30;
                            mnsj = "Monto inválido";
                            monto = 0;
                            autorizacion = "";
                        }
                    }
                }

                autorizacionPago.codigo = codigo;
                autorizacionPago.autorizacion = autorizacion;
                autorizacionPago.mensaje = mnsj;
                //autorizacionPago.monto = Math.Truncate(monto * 100).ToString();
                autorizacionPago.transaccion = Transaccion;
                autorizacionPago.fecha = Fecha;
                autorizacionPago.notificacion_sms = "";
                autorizacionPago.mensaje_sms = "";
                autorizacionPago.mensaje_ticket = "Hola probando mnsj en sandbox, gracias por su pago.";

            }
            catch (Exception ex)
            {
                string mns = ex.Message;

                autorizacionPago.codigo = 50;
                autorizacionPago.autorizacion = "";
                autorizacionPago.mensaje = "Error de sistema.";
                autorizacionPago.transaccion = Transaccion;
                autorizacionPago.fecha = Fecha;
                autorizacionPago.notificacion_sms = "";
                autorizacionPago.mensaje_sms = "";
                autorizacionPago.mensaje_ticket = "";
            }

            return autorizacionPago;
        }
        public Tuple<int, bool> ConsultarPagoClubPago()
        {
            int result = 1;
            bool resbool = false;

            try
            {
                SqlCommand query = new SqlCommand();
                query.CommandType = CommandType.Text;

                query.CommandText = "select CASE WHEN MAX(VchAutorizacion) IS NOT NULL THEN MAX(VchAutorizacion) ELSE 0 END VchAutorizacion from PagosClubPago";

                DataTable dt = this.Busquedas(query);

                foreach (DataRow item in dt.Rows)
                {
                    result = int.Parse(item["VchAutorizacion"].ToString()) + 1;
                }

                resbool = true;
            }
            catch (Exception ex)
            {
                string mns = ex.Message;
            }

            return Tuple.Create(result, resbool);
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
        public bool RegistrarPagoClubPago(Guid UidPago, string IdReferencia, DateTime FechaRegistro, DateTime FechaOperacion, decimal Monto, string Transaccion, string Autorizacion, Guid UidPagoEstatus)
        {
            bool result = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PagosClubPagoRegistrar";

                comando.Parameters.Add("@UidPago", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPago"].Value = UidPago;

                comando.Parameters.Add("@IdReferencia", SqlDbType.VarChar);
                comando.Parameters["@IdReferencia"].Value = IdReferencia;

                comando.Parameters.Add("@DtFechaRegistro", SqlDbType.DateTime);
                comando.Parameters["@DtFechaRegistro"].Value = FechaRegistro;

                comando.Parameters.Add("@DtFechaOperacion", SqlDbType.DateTime);
                comando.Parameters["@DtFechaOperacion"].Value = FechaOperacion;

                comando.Parameters.Add("@DcmMonto", SqlDbType.Decimal);
                comando.Parameters["@DcmMonto"].Value = Monto;

                comando.Parameters.Add("@VchTransaccion", SqlDbType.VarChar);
                comando.Parameters["@VchTransaccion"].Value = Transaccion;

                comando.Parameters.Add("@VchAutorizacion", SqlDbType.VarChar);
                comando.Parameters["@VchAutorizacion"].Value = Autorizacion;

                comando.Parameters.Add("@UidPagoEstatus", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPagoEstatus"].Value = UidPagoEstatus;

                result = this.ManipulacionDeDatos(comando);

            }
            catch (Exception ex)
            {
                string mnsj = ex.Message;
            }

            return result;
        }
    }
}
