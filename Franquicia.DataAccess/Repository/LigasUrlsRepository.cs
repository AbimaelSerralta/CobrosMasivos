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
    public class LigasUrlsRepository : SqlDataRepository
    {
        LigasUrlsConstruirLigaModel _ligasUrlsConstruirLigaModel = new LigasUrlsConstruirLigaModel();
        public LigasUrlsConstruirLigaModel ligasUrlsConstruirLigaModel
        {
            get { return _ligasUrlsConstruirLigaModel; }
            set { _ligasUrlsConstruirLigaModel = value; }
        }

        LigasUrlsListViewModel _ligasUrlsListViewModel = new LigasUrlsListViewModel();
        public LigasUrlsListViewModel ligasUrlsListViewModel
        {
            get { return _ligasUrlsListViewModel; }
            set { _ligasUrlsListViewModel = value; }
        }

        LigasUrlsGridViewModel _ligasUrlsGridViewModel = new LigasUrlsGridViewModel();
        public LigasUrlsGridViewModel ligasUrlsGridViewModel
        {
            get { return _ligasUrlsGridViewModel; }
            set { _ligasUrlsGridViewModel = value; }
        }

        LigasUsuariosFinalGridViewModel _ligasUsuariosFinalGridViewModel = new LigasUsuariosFinalGridViewModel();
        public LigasUsuariosFinalGridViewModel ligasUsuariosFinalGridViewModel
        {
            get { return _ligasUsuariosFinalGridViewModel; }
            set { _ligasUsuariosFinalGridViewModel = value; }
        }

        public List<LigasUrlsListViewModel> TotalEnvioLigas(Guid UidCliente)
        {
            List<LigasUrlsListViewModel> lsLigasUrlsListViewModel = new List<LigasUrlsListViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select count(VchUrl) as CorreoUsado from LigasUrls lu, Usuarios us, ClientesUsuarios cu, Clientes cl where lu.UidUsuario = us.UidUsuario and us.UidUsuario = cu.UidUsuario and cu.UidCliente = cl.UidCliente and cu.UidCliente ='" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsLigasUrlsListViewModel.Add(new LigasUrlsListViewModel()
                {
                    IntUsados = int.Parse(item["CorreoUsado"].ToString())
                });
            }

            return lsLigasUrlsListViewModel;
        }

        #region Clientes
        public List<LigasUrlsGridViewModel> ConsultarEstatusLiga(Guid UidCliente)
        {
            List<LigasUrlsGridViewModel> lsLigasUrlsGridViewModel = new List<LigasUrlsGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;
            //query.CommandText = "select lu.*, us.UidUsuario, pt.VchEstatus from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente where pt.VchEstatus != 'error' and pt.VchEstatus != 'denied' and cl.UidCliente = '" + UidCliente + "' UNION select lu.*, us.UidUsuario, pt.VchEstatus from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente where pt.VchEstatus = 'error' or pt.VchEstatus = 'denied' or pt.VchEstatus IS NULL and cl.UidCliente = '" + UidCliente + "'";
            //query.CommandText = "select lu.*, us.UidUsuario, pt.VchEstatus from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente where pt.VchEstatus != 'error' and pt.VchEstatus != 'denied' and cl.UidCliente = '" + UidCliente + "' UNION select lu.*, us.UidUsuario, pt.VchEstatus from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente where pt.VchEstatus IS NULL and cl.UidCliente = '" + UidCliente + "'";
            //query.CommandText = "select lu.*, us.UidUsuario, pt.VchEstatus from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente where pt.VchEstatus IS NULL and cl.UidCliente = '" + UidCliente + "' UNION select lu.*, us.UidUsuario, pt.VchEstatus from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente where pt.DtmFechaDeRegistro = (select MAX(DtmFechaDeRegistro) from PagosTarjeta where IdReferencia = lu.IdReferencia) and cl.UidCliente = '" + UidCliente + "'";
            //query.CommandText = "select lu.*, us.UidUsuario, pt.VchEstatus, (select liur.DcmImporte from LigasUrls liur, Promociones pr, PagosTarjeta pt where liur.UidPromocion = pr.UidPromocion and pt.IdReferencia = liur.IdReferencia and liur.UidLigaAsociado = lu.UidLigaAsociado) as DcmImportePromocion,(select pr.VchDescripcion from LigasUrls liur, Promociones pr, PagosTarjeta pt where liur.UidPromocion = pr.UidPromocion and pt.IdReferencia = liur.IdReferencia and liur.UidLigaAsociado = lu.UidLigaAsociado) as VchPromocion from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente where pt.VchEstatus IS NULL and lu.UidPromocion IS NULL and cl.UidCliente = '" + UidCliente + "' UNION select lu.*, us.UidUsuario, pt.VchEstatus, (select liur.DcmImporte from LigasUrls liur, Promociones pr, PagosTarjeta pt where liur.UidPromocion = pr.UidPromocion and pt.IdReferencia = liur.IdReferencia and liur.UidLigaAsociado = lu.UidLigaAsociado) as Importe,(select pr.VchDescripcion from LigasUrls liur, Promociones pr, PagosTarjeta pt where liur.UidPromocion = pr.UidPromocion and pt.IdReferencia = liur.IdReferencia and liur.UidLigaAsociado = lu.UidLigaAsociado) as Promocion from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente where pt.DtmFechaDeRegistro = (select MAX(DtmFechaDeRegistro) from PagosTarjeta where IdReferencia = lu.IdReferencia) and cl.UidCliente = '" + UidCliente + "'";
            //query.CommandText = "select lu.*, us.UidUsuario, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion where pt.VchEstatus IS NULL and lu.UidPromocion IS NULL and cl.UidCliente = '" + UidCliente + "' UNION select lu.*, us.UidUsuario, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion where lu.UidLigaAsociado IS NULL AND pt.DtmFechaDeRegistro = (select MAX(DtmFechaDeRegistro) from PagosTarjeta where IdReferencia = lu.IdReferencia) and cl.UidCliente = '" + UidCliente + "' UNION select lu.*, us.UidUsuario, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion where lu.UidLigaAsociado IS NOT NULL AND pt.DtmFechaDeRegistro = (select MAX(pata.DtmFechaDeRegistro) from PagosTarjeta pata, LigasUrls ls where pata.IdReferencia = ls.IdReferencia and ls.UidLigaAsociado = lu.UidLigaAsociado) and cl.UidCliente = '" + UidCliente + "'";
            //query.CommandText = "select lu.*, us.UidUsuario, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion, (select DcmImporte from LigasUrls where UidLigaAsociado = lu.UidLigaAsociado and UidPromocion IS NULL) as ImporteReal from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion where pt.VchEstatus IS NULL and lu.UidPromocion IS NULL and cl.UidCliente = '" + UidCliente + "' UNION select lu.*, us.UidUsuario, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion, (select DcmImporte from LigasUrls where UidLigaAsociado = lu.UidLigaAsociado and UidPromocion IS NULL) as ImporteReal from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion where lu.UidLigaAsociado IS NULL AND pt.DtmFechaDeRegistro = (select MAX(DtmFechaDeRegistro) from PagosTarjeta where IdReferencia = lu.IdReferencia) and cl.UidCliente = '" + UidCliente + "' UNION select lu.*, us.UidUsuario, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion, (select DcmImporte from LigasUrls where UidLigaAsociado = lu.UidLigaAsociado and UidPromocion IS NULL) as ImporteReal from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion where lu.UidLigaAsociado IS NOT NULL AND pt.DtmFechaDeRegistro = (select MAX(pata.DtmFechaDeRegistro) from PagosTarjeta pata, LigasUrls ls where pata.IdReferencia = ls.IdReferencia and ls.UidLigaAsociado = lu.UidLigaAsociado) and cl.UidCliente = '" + UidCliente + "'";
            //query.CommandText = "select lu.*, us.UidUsuario, us.VchNombre, us.VchApePaterno, us.VchApeMaterno, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion, (select DcmImporte from LigasUrls where UidLigaAsociado = lu.UidLigaAsociado and UidPromocion IS NULL) as ImporteReal from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion where pt.VchEstatus IS NULL and lu.UidPromocion IS NULL and lu.UidPropietario = cl.UidCliente and cl.UidCliente = '" + UidCliente + "' UNION select lu.*, us.UidUsuario, us.VchNombre, us.VchApePaterno, us.VchApeMaterno, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion, (select DcmImporte from LigasUrls where UidLigaAsociado = lu.UidLigaAsociado and UidPromocion IS NULL) as ImporteReal from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion where lu.UidPropietario = cl.UidCliente and lu.UidLigaAsociado IS NULL AND pt.DtmFechaDeRegistro = (select MAX(DtmFechaDeRegistro) from PagosTarjeta where IdReferencia = lu.IdReferencia) and cl.UidCliente = '" + UidCliente + "' UNION select lu.*, us.UidUsuario, us.VchNombre, us.VchApePaterno, us.VchApeMaterno, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion, (select DcmImporte from LigasUrls where UidLigaAsociado = lu.UidLigaAsociado and UidPromocion IS NULL) as ImporteReal from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion where lu.UidPropietario = cl.UidCliente and lu.UidLigaAsociado IS NOT NULL AND pt.DtmFechaDeRegistro = (select MAX(pata.DtmFechaDeRegistro) from PagosTarjeta pata, LigasUrls ls where pata.IdReferencia = ls.IdReferencia and ls.UidLigaAsociado = lu.UidLigaAsociado) and cl.UidCliente = '" + UidCliente + "'";
            query.CommandText = "select lu.*, us.UidUsuario, us.VchNombre, us.VchApePaterno, us.VchApeMaterno, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion, (select DcmImporte from LigasUrls where UidLigaAsociado = lu.UidLigaAsociado and UidPromocion IS NULL) as ImporteReal from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion left join AuxiliarPago ap on ap.IdReferencia = lu.IdReferencia where ap.IdReferencia IS NULL and pt.VchEstatus IS NULL and lu.UidPromocion IS NULL and lu.UidEvento IS NULL and lu.UidPropietario = cl.UidCliente and cl.UidCliente = '" + UidCliente + "'UNION select lu.*, us.UidUsuario, us.VchNombre, us.VchApePaterno, us.VchApeMaterno, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion, (select DcmImporte from LigasUrls where UidLigaAsociado = lu.UidLigaAsociado and UidPromocion IS NULL) as ImporteReal from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion left join AuxiliarPago ap on ap.IdReferencia = lu.IdReferencia where ap.IdReferencia IS NULL and lu.UidPropietario = cl.UidCliente and lu.UidLigaAsociado IS NULL AND pt.DtmFechaDeRegistro = (select MAX(DtmFechaDeRegistro) from PagosTarjeta where IdReferencia = lu.IdReferencia) and cl.UidCliente = '" + UidCliente + "' UNION select lu.*, us.UidUsuario, us.VchNombre, us.VchApePaterno, us.VchApeMaterno, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion, (select DcmImporte from LigasUrls where UidLigaAsociado = lu.UidLigaAsociado and UidPromocion IS NULL) as ImporteReal from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion where lu.UidPropietario = cl.UidCliente and lu.UidLigaAsociado IS NOT NULL AND pt.DtmFechaDeRegistro = (select MAX(pata.DtmFechaDeRegistro) from PagosTarjeta pata, LigasUrls ls where pata.IdReferencia = ls.IdReferencia and ls.UidLigaAsociado = lu.UidLigaAsociado) and cl.UidCliente = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                string VchColor = "#007bff";
                decimal ImporteReal = 0;

                if (!string.IsNullOrEmpty(item["VchEstatus"].ToString()))
                {
                    switch (item["VchEstatus"].ToString())
                    {
                        case "approved":
                            VchColor = "#4caf50";
                            break;
                        case "denied":
                            VchColor = "#ff9800";
                            break;
                        case "error":
                            VchColor = "#f55145";
                            break;
                    }
                }

                if (item["VchPromocion"].ToString() != "")
                {
                    ImporteReal = decimal.Parse(item.IsNull("ImporteReal") ? "0.00" : item["ImporteReal"].ToString());
                }
                else
                {
                    ImporteReal = decimal.Parse(item.IsNull("DcmImporte") ? "0.00" : item["DcmImporte"].ToString());
                }

                ligasUrlsGridViewModel = new LigasUrlsGridViewModel()
                {
                    IdReferencia = item["IdReferencia"].ToString(),
                    VchUrl = item["VchUrl"].ToString(),
                    VchIdentificador = item["VchIdentificador"].ToString(),
                    VchConcepto = item["VchConcepto"].ToString(),
                    DcmImporte = ImporteReal,
                    DtVencimiento = DateTime.Parse(item.IsNull("DtVencimiento") ? "2020-02-18 16:57:39.113" : item["DtVencimiento"].ToString()),
                    VchEstatus = item.IsNull("VchEstatus") ? "Pendiente" : item["VchEstatus"].ToString(),
                    VchAsunto = item["VchAsunto"].ToString(),
                    VchColor = VchColor,
                    DtRegistro = DateTime.Parse(item["DtRegistro"].ToString()),
                    DcmImportePromocion = decimal.Parse(item.IsNull("DcmImportePromocion") ? "0.00" : item["DcmImportePromocion"].ToString()),
                    VchPromocion = item.IsNull("VchPromocion") ? "CONTADO" : item["VchPromocion"].ToString(),
                    UidLigaAsociado = item.IsNull("UidLigaAsociado") ? Guid.Empty : Guid.Parse(item["UidLigaAsociado"].ToString()),
                    VchNombre = item["VchNombre"].ToString(),
                    VchApePaterno = item["VchApePaterno"].ToString(),
                    VchApeMaterno = item["VchApeMaterno"].ToString()
                };

                lsLigasUrlsGridViewModel.Add(ligasUrlsGridViewModel);
            }

            return lsLigasUrlsGridViewModel.OrderByDescending(x => x.DtRegistro).ToList();
        }
        public List<LigasUrlsGridViewModel> BuscarLigas(Guid UidCliente, string Identificador, string Nombre, string ApePaterno, string ApeMaterno, string Asunto, string Concepto, decimal ImporteMayor, decimal ImporteMenor, string RegistroDesde, string RegistroHasta, string VencimientoDesde, string VencimientoHasta, string Estatus)
        {
            List<LigasUrlsGridViewModel> lsLigasUrlsGridViewModel = new List<LigasUrlsGridViewModel>();

            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "sp_LigasBuscar";
            try
            {
                if (UidCliente != Guid.Empty)
                {
                    comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidCliente"].Value = UidCliente;
                }
                if (Identificador != string.Empty)
                {
                    comando.Parameters.Add("@Identificador", SqlDbType.VarChar, 50);
                    comando.Parameters["@Identificador"].Value = Identificador;
                }
                if (Nombre != string.Empty)
                {
                    comando.Parameters.Add("@Nombre", SqlDbType.VarChar, 50);
                    comando.Parameters["@Nombre"].Value = Nombre;
                }
                if (ApePaterno != string.Empty)
                {
                    comando.Parameters.Add("@ApePaterno", SqlDbType.VarChar, 50);
                    comando.Parameters["@ApePaterno"].Value = ApePaterno;
                }
                if (ApeMaterno != string.Empty)
                {
                    comando.Parameters.Add("@ApeMaterno", SqlDbType.VarChar, 50);
                    comando.Parameters["@ApeMaterno"].Value = ApeMaterno;
                }
                if (Asunto != string.Empty)
                {
                    comando.Parameters.Add("@Asunto", SqlDbType.VarChar, 50);
                    comando.Parameters["@Asunto"].Value = Asunto;
                }
                if (Concepto != string.Empty)
                {
                    comando.Parameters.Add("@Concepto", SqlDbType.VarChar, 50);
                    comando.Parameters["@Concepto"].Value = Concepto;
                }
                if (RegistroDesde != string.Empty)
                {
                    comando.Parameters.Add("@RegistroDesde", SqlDbType.DateTime);
                    comando.Parameters["@RegistroDesde"].Value = RegistroDesde;
                }
                if (RegistroHasta != string.Empty)
                {
                    comando.Parameters.Add("@RegistroHasta", SqlDbType.Date);
                    comando.Parameters["@RegistroHasta"].Value = RegistroHasta;
                }
                if (VencimientoDesde != string.Empty)
                {
                    comando.Parameters.Add("@VencimientoDesde", SqlDbType.DateTime);
                    comando.Parameters["@VencimientoDesde"].Value = VencimientoDesde;
                }
                if (VencimientoHasta != string.Empty)
                {
                    comando.Parameters.Add("@VencimientoHasta", SqlDbType.Date);
                    comando.Parameters["@VencimientoHasta"].Value = VencimientoHasta;
                }
                if (ImporteMayor != 0)
                {
                    comando.Parameters.Add("@ImporteMayor", SqlDbType.Decimal);
                    comando.Parameters["@ImporteMayor"].Value = ImporteMayor;
                }
                if (ImporteMenor != 0)
                {
                    comando.Parameters.Add("@ImporteMenor", SqlDbType.Decimal);
                    comando.Parameters["@ImporteMenor"].Value = ImporteMenor;
                }
                if (Estatus != string.Empty)
                {
                    comando.Parameters.Add("@Estatus", SqlDbType.VarChar, 50);
                    comando.Parameters["@Estatus"].Value = Estatus;
                }

                foreach (DataRow item in this.Busquedas(comando).Rows)
                {
                    string VchColor = "#007bff";
                    decimal ImporteReal = 0;

                    if (!string.IsNullOrEmpty(item["VchEstatus"].ToString()))
                    {
                        switch (item["VchEstatus"].ToString())
                        {
                            case "approved":
                                VchColor = "#4caf50 ";
                                break;
                            case "denied":
                                VchColor = "#ff9800 ";
                                break;
                            case "error":
                                VchColor = "#f55145 ";
                                break;
                        }
                    }

                    if (item["VchPromocion"].ToString() != "")
                    {
                        ImporteReal = decimal.Parse(item.IsNull("ImporteReal") ? "0.00" : item["ImporteReal"].ToString());
                    }
                    else
                    {
                        ImporteReal = decimal.Parse(item.IsNull("DcmImporte") ? "0.00" : item["DcmImporte"].ToString());
                    }

                    ligasUrlsGridViewModel = new LigasUrlsGridViewModel()
                    {
                        IdReferencia = item["IdReferencia"].ToString(),
                        VchUrl = item["VchUrl"].ToString(),
                        VchIdentificador = item["VchIdentificador"].ToString(),
                        VchConcepto = item["VchConcepto"].ToString(),
                        DcmImporte = ImporteReal,
                        DtVencimiento = DateTime.Parse(item.IsNull("DtVencimiento") ? "2020-02-18 16:57:39.113" : item["DtVencimiento"].ToString()),
                        VchEstatus = item.IsNull("VchEstatus") ? "Pendiente" : item["VchEstatus"].ToString(),
                        VchAsunto = item["VchAsunto"].ToString(),
                        VchColor = VchColor,
                        DtRegistro = DateTime.Parse(item["DtRegistro"].ToString()),
                        DcmImportePromocion = decimal.Parse(item.IsNull("DcmImportePromocion") ? "0.00" : item["DcmImportePromocion"].ToString()),
                        VchPromocion = item.IsNull("VchPromocion") ? "CONTADO" : item["VchPromocion"].ToString(),
                        UidLigaAsociado = item.IsNull("UidLigaAsociado") ? Guid.Empty : Guid.Parse(item["UidLigaAsociado"].ToString()),
                        VchNombre = item["VchNombre"].ToString(),
                        VchApePaterno = item["VchApePaterno"].ToString(),
                        VchApeMaterno = item["VchApeMaterno"].ToString()
                    };

                    lsLigasUrlsGridViewModel.Add(ligasUrlsGridViewModel);
                }

                return lsLigasUrlsGridViewModel.OrderByDescending(x => x.DtRegistro).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void ContruirLiga(Guid UidLigaUrl)
        {
            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;
            query.CommandText = "select lu.VchUrl, lu.VchConcepto, lu.DcmImporte, lu.DtVencimiento, us.VchNombre, us.VchApePaterno, us.VchApeMaterno, cl.VchIdWAySMS, lu.UidPromocion, lu.UidLigaAsociado from LigasUrls lu, Usuarios us, Clientes cl where lu.UidUsuario = us.UidUsuario and cl.UidCliente = lu.UidPropietario and lu.UidLigaUrl = '" + UidLigaUrl + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                ligasUrlsConstruirLigaModel.VchUrl = item["VchUrl"].ToString();
                ligasUrlsConstruirLigaModel.VchConcepto = item["VchConcepto"].ToString();
                ligasUrlsConstruirLigaModel.DcmImporte = decimal.Parse(item["DcmImporte"].ToString());
                ligasUrlsConstruirLigaModel.DtVencimiento = DateTime.Parse(item["DtVencimiento"].ToString());
                ligasUrlsConstruirLigaModel.VchNombre = item["VchNombre"].ToString();
                ligasUrlsConstruirLigaModel.VchApePaterno = item["VchApePaterno"].ToString();
                ligasUrlsConstruirLigaModel.VchApeMaterno = item["VchApeMaterno"].ToString();
                ligasUrlsConstruirLigaModel.VchNombreComercial = item["VchIdWAySMS"].ToString();
                ligasUrlsConstruirLigaModel.UidPromocion = item.IsNull("UidPromocion") ? Guid.Empty : Guid.Parse(item["UidPromocion"].ToString());
                ligasUrlsConstruirLigaModel.UidLigaAsociado = item.IsNull("UidLigaAsociado") ? Guid.Empty : Guid.Parse(item["UidLigaAsociado"].ToString());
            }
        }
        #endregion


        #region ClientePayCard
        public void ObtenerDatosUrl(string IdReferencia)
        {
            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;
            query.CommandText = "select lu.DcmImporte, lu.UidPropietario from LigasUrls lu where IdReferencia = '" + IdReferencia + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                ligasUrlsGridViewModel.UidPropietario = Guid.Parse(item["UidPropietario"].ToString());
                ligasUrlsGridViewModel.DcmImporte = decimal.Parse(item["DcmImporte"].ToString());
            }
        }
        #endregion

        #region UsuariosFinal
        public List<LigasUsuariosFinalGridViewModel> ConsultarLigaUsuarioFinal(Guid UidUsuario)
        {
            List<LigasUsuariosFinalGridViewModel> lsLigasUsuariosFinalGridViewModel = new List<LigasUsuariosFinalGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;
            query.CommandText = "select lu.*, cl.VchNombreComercial, us.UidUsuario, us.VchNombre, us.VchApePaterno, us.VchApeMaterno, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion, (select DcmImporte from LigasUrls where UidLigaAsociado = lu.UidLigaAsociado and UidPromocion IS NULL) as ImporteReal from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion left join AuxiliarPago ap on ap.IdReferencia = lu.IdReferencia where ap.IdReferencia IS NULL and pt.VchEstatus IS NULL and lu.UidPromocion IS NULL and lu.UidEvento IS NULL and lu.UidPropietario = cl.UidCliente and us.UidUsuario = '" + UidUsuario + "' UNION select lu.*, cl.VchNombreComercial, us.UidUsuario, us.VchNombre, us.VchApePaterno, us.VchApeMaterno, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion, (select DcmImporte from LigasUrls where UidLigaAsociado = lu.UidLigaAsociado and UidPromocion IS NULL) as ImporteReal from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion left join AuxiliarPago ap on ap.IdReferencia = lu.IdReferencia where ap.IdReferencia IS NULL and lu.UidPropietario = cl.UidCliente and lu.UidLigaAsociado IS NULL AND pt.DtmFechaDeRegistro = (select MAX(DtmFechaDeRegistro) from PagosTarjeta where IdReferencia = lu.IdReferencia) and us.UidUsuario = '" + UidUsuario + "' UNION select lu.*, cl.VchNombreComercial, us.UidUsuario, us.VchNombre, us.VchApePaterno, us.VchApeMaterno, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion, (select DcmImporte from LigasUrls where UidLigaAsociado = lu.UidLigaAsociado and UidPromocion IS NULL) as ImporteReal from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion where lu.UidPropietario = cl.UidCliente and lu.UidLigaAsociado IS NOT NULL AND pt.DtmFechaDeRegistro = (select MAX(pata.DtmFechaDeRegistro) from PagosTarjeta pata, LigasUrls ls where pata.IdReferencia = ls.IdReferencia and ls.UidLigaAsociado = lu.UidLigaAsociado) and us.UidUsuario = '" + UidUsuario + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                string VchColor = "#007bff";
                decimal ImporteReal = 0;
                bool blPagar = true;

                if (!string.IsNullOrEmpty(item["VchEstatus"].ToString()))
                {
                    switch (item["VchEstatus"].ToString())
                    {
                        case "approved":
                            VchColor = "#4caf50 ";
                            blPagar = false;
                            break;
                        case "denied":
                            VchColor = "#ff9800 ";
                            blPagar = true;
                            break;
                        case "error":
                            VchColor = "#f55145 ";
                            blPagar = true;
                            break;
                    }
                }

                if (item["VchPromocion"].ToString() != "")
                {
                    ImporteReal = decimal.Parse(item.IsNull("ImporteReal") ? "0.00" : item["ImporteReal"].ToString());
                }
                else
                {
                    ImporteReal = decimal.Parse(item.IsNull("DcmImporte") ? "0.00" : item["DcmImporte"].ToString());
                }

                ligasUsuariosFinalGridViewModel = new LigasUsuariosFinalGridViewModel()
                {
                    UidLigaUrl = Guid.Parse(item["UidLigaUrl"].ToString()),
                    IdReferencia = item["IdReferencia"].ToString(),
                    VchUrl = item["VchUrl"].ToString(),
                    VchIdentificador = item["VchIdentificador"].ToString(),
                    VchConcepto = item["VchConcepto"].ToString(),
                    DcmImporte = ImporteReal,
                    DtVencimiento = DateTime.Parse(item.IsNull("DtVencimiento") ? "2020-02-18 16:57:39.113" : item["DtVencimiento"].ToString()),
                    VchEstatus = item.IsNull("VchEstatus") ? "Pendiente" : item["VchEstatus"].ToString(),
                    VchAsunto = item["VchAsunto"].ToString(),
                    VchColor = VchColor,
                    DtRegistro = DateTime.Parse(item["DtRegistro"].ToString()),
                    DcmImportePromocion = decimal.Parse(item.IsNull("DcmImportePromocion") ? "0.00" : item["DcmImportePromocion"].ToString()),
                    VchPromocion = item.IsNull("VchPromocion") ? "CONTADO" : item["VchPromocion"].ToString(),
                    UidLigaAsociado = item.IsNull("UidLigaAsociado") ? Guid.Empty : Guid.Parse(item["UidLigaAsociado"].ToString()),
                    VchNombreComercial = item["VchNombreComercial"].ToString(),
                    blPagar = blPagar
                };

                lsLigasUsuariosFinalGridViewModel.Add(ligasUsuariosFinalGridViewModel);
            }

            return lsLigasUsuariosFinalGridViewModel.OrderByDescending(x => x.DtRegistro).ToList();
        }
        public List<LigasUsuariosFinalGridViewModel> BuscarLigasUsuarioFinal(Guid UidUsuario, string Identificador, string Asunto, string Concepto, decimal ImporteMayor, decimal ImporteMenor, string RegistroDesde, string RegistroHasta, string VencimientoDesde, string VencimientoHasta, string Estatus)
        {
            List<LigasUsuariosFinalGridViewModel> lsLigasUsuariosFinalGridViewModel = new List<LigasUsuariosFinalGridViewModel>();

            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "sp_LigasBuscarUsuarioFinal";
            try
            {
                comando.Parameters.Add("@UidUsuario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidUsuario"].Value = UidUsuario;

                if (Identificador != string.Empty)
                {
                    comando.Parameters.Add("@Identificador", SqlDbType.VarChar, 50);
                    comando.Parameters["@Identificador"].Value = Identificador;
                }
                if (Asunto != string.Empty)
                {
                    comando.Parameters.Add("@Asunto", SqlDbType.VarChar, 50);
                    comando.Parameters["@Asunto"].Value = Asunto;
                }
                if (Concepto != string.Empty)
                {
                    comando.Parameters.Add("@Concepto", SqlDbType.VarChar, 50);
                    comando.Parameters["@Concepto"].Value = Concepto;
                }
                if (RegistroDesde != string.Empty)
                {
                    comando.Parameters.Add("@RegistroDesde", SqlDbType.DateTime);
                    comando.Parameters["@RegistroDesde"].Value = RegistroDesde;
                }
                if (RegistroHasta != string.Empty)
                {
                    comando.Parameters.Add("@RegistroHasta", SqlDbType.Date);
                    comando.Parameters["@RegistroHasta"].Value = RegistroHasta;
                }
                if (VencimientoDesde != string.Empty)
                {
                    comando.Parameters.Add("@VencimientoDesde", SqlDbType.DateTime);
                    comando.Parameters["@VencimientoDesde"].Value = VencimientoDesde;
                }
                if (VencimientoHasta != string.Empty)
                {
                    comando.Parameters.Add("@VencimientoHasta", SqlDbType.Date);
                    comando.Parameters["@VencimientoHasta"].Value = VencimientoHasta;
                }
                if (ImporteMayor != 0)
                {
                    comando.Parameters.Add("@ImporteMayor", SqlDbType.Decimal);
                    comando.Parameters["@ImporteMayor"].Value = ImporteMayor;
                }
                if (ImporteMenor != 0)
                {
                    comando.Parameters.Add("@ImporteMenor", SqlDbType.Decimal);
                    comando.Parameters["@ImporteMenor"].Value = ImporteMenor;
                }
                if (Estatus != string.Empty)
                {
                    comando.Parameters.Add("@Estatus", SqlDbType.VarChar, 50);
                    comando.Parameters["@Estatus"].Value = Estatus;
                }

                foreach (DataRow item in this.Busquedas(comando).Rows)
                {
                    string VchColor = "#007bff";
                    decimal ImporteReal = 0;
                    bool blPagar = true;

                    if (!string.IsNullOrEmpty(item["VchEstatus"].ToString()))
                    {
                        switch (item["VchEstatus"].ToString())
                        {
                            case "approved":
                                VchColor = "#4caf50 ";
                                blPagar = false;
                                break;
                            case "denied":
                                VchColor = "#ff9800 ";
                                blPagar = true;
                                break;
                            case "error":
                                VchColor = "#f55145 ";
                                blPagar = true;
                                break;
                        }
                    }

                    if (item["VchPromocion"].ToString() != "")
                    {
                        ImporteReal = decimal.Parse(item.IsNull("ImporteReal") ? "0.00" : item["ImporteReal"].ToString());
                    }
                    else
                    {
                        ImporteReal = decimal.Parse(item.IsNull("DcmImporte") ? "0.00" : item["DcmImporte"].ToString());
                    }

                    ligasUsuariosFinalGridViewModel = new LigasUsuariosFinalGridViewModel()
                    {
                        UidLigaUrl = Guid.Parse(item["UidLigaUrl"].ToString()),
                        IdReferencia = item["IdReferencia"].ToString(),
                        VchUrl = item["VchUrl"].ToString(),
                        VchIdentificador = item["VchIdentificador"].ToString(),
                        VchConcepto = item["VchConcepto"].ToString(),
                        DcmImporte = ImporteReal,
                        DtVencimiento = DateTime.Parse(item.IsNull("DtVencimiento") ? "2020-02-18 16:57:39.113" : item["DtVencimiento"].ToString()),
                        VchEstatus = item.IsNull("VchEstatus") ? "Pendiente" : item["VchEstatus"].ToString(),
                        VchAsunto = item["VchAsunto"].ToString(),
                        VchColor = VchColor,
                        DtRegistro = DateTime.Parse(item["DtRegistro"].ToString()),
                        DcmImportePromocion = decimal.Parse(item.IsNull("DcmImportePromocion") ? "0.00" : item["DcmImportePromocion"].ToString()),
                        VchPromocion = item.IsNull("VchPromocion") ? "CONTADO" : item["VchPromocion"].ToString(),
                        UidLigaAsociado = item.IsNull("UidLigaAsociado") ? Guid.Empty : Guid.Parse(item["UidLigaAsociado"].ToString()),
                        VchNombreComercial = item["VchNombreComercial"].ToString(),
                        blPagar = blPagar
                    };

                    lsLigasUsuariosFinalGridViewModel.Add(ligasUsuariosFinalGridViewModel);
                }

                return lsLigasUsuariosFinalGridViewModel.OrderByDescending(x => x.DtRegistro).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string ObtenerPromocionesUsuarioFinal(Guid UidLigaUrl)
        {
            string result = string.Empty;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;
            query.CommandText = "select UidLigaAsociado from LigasUrls where UidLigaUrl = '" + UidLigaUrl + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                result = item["UidLigaAsociado"].ToString();
            }

            return result;
        }
        public string ObtenerUrlLigaUsuarioFinal(Guid UidLigaUrl)
        {
            string result = string.Empty;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;
            query.CommandText = "select IdReferencia, VchUrl from LigasUrls where UidLigaUrl = '" + UidLigaUrl + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                result = item["VchUrl"].ToString() + "," + item["IdReferencia"].ToString();
            }

            return result;
        }
        #endregion

        #region LigasUrlFranquicias
        public List<LigasUrlsGridViewModel> ConsultarEstatusLigaFranquicia(Guid UidFranquicia)
        {
            List<LigasUrlsGridViewModel> lsLigasUrlsGridViewModel = new List<LigasUrlsGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;
            //query.CommandText = "select lu.*, us.UidUsuario, pt.VchEstatus from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente where pt.VchEstatus != 'error' and pt.VchEstatus != 'denied' and cl.UidCliente = '" + UidCliente + "' UNION select lu.*, us.UidUsuario, pt.VchEstatus from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente where pt.VchEstatus = 'error' or pt.VchEstatus = 'denied' or pt.VchEstatus IS NULL and cl.UidCliente = '" + UidCliente + "'";
            //query.CommandText = "select lu.*, us.UidUsuario, pt.VchEstatus from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente where pt.VchEstatus != 'error' and pt.VchEstatus != 'denied' and cl.UidCliente = '" + UidCliente + "' UNION select lu.*, us.UidUsuario, pt.VchEstatus from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente where pt.VchEstatus IS NULL and cl.UidCliente = '" + UidCliente + "'";
            //query.CommandText = "select lu.*, us.UidUsuario, pt.VchEstatus from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente where pt.VchEstatus IS NULL and cl.UidCliente = '" + UidCliente + "' UNION select lu.*, us.UidUsuario, pt.VchEstatus from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente where pt.DtmFechaDeRegistro = (select MAX(DtmFechaDeRegistro) from PagosTarjeta where IdReferencia = lu.IdReferencia) and cl.UidCliente = '" + UidCliente + "'";
            //query.CommandText = "select lu.*, us.UidUsuario, pt.VchEstatus, (select liur.DcmImporte from LigasUrls liur, Promociones pr, PagosTarjeta pt where liur.UidPromocion = pr.UidPromocion and pt.IdReferencia = liur.IdReferencia and liur.UidLigaAsociado = lu.UidLigaAsociado) as DcmImportePromocion,(select pr.VchDescripcion from LigasUrls liur, Promociones pr, PagosTarjeta pt where liur.UidPromocion = pr.UidPromocion and pt.IdReferencia = liur.IdReferencia and liur.UidLigaAsociado = lu.UidLigaAsociado) as VchPromocion from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente where pt.VchEstatus IS NULL and lu.UidPromocion IS NULL and cl.UidCliente = '" + UidCliente + "' UNION select lu.*, us.UidUsuario, pt.VchEstatus, (select liur.DcmImporte from LigasUrls liur, Promociones pr, PagosTarjeta pt where liur.UidPromocion = pr.UidPromocion and pt.IdReferencia = liur.IdReferencia and liur.UidLigaAsociado = lu.UidLigaAsociado) as Importe,(select pr.VchDescripcion from LigasUrls liur, Promociones pr, PagosTarjeta pt where liur.UidPromocion = pr.UidPromocion and pt.IdReferencia = liur.IdReferencia and liur.UidLigaAsociado = lu.UidLigaAsociado) as Promocion from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente where pt.DtmFechaDeRegistro = (select MAX(DtmFechaDeRegistro) from PagosTarjeta where IdReferencia = lu.IdReferencia) and cl.UidCliente = '" + UidCliente + "'";
            //query.CommandText = "select lu.*, us.UidUsuario, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion where pt.VchEstatus IS NULL and lu.UidPromocion IS NULL and cl.UidCliente = '" + UidCliente + "' UNION select lu.*, us.UidUsuario, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion where lu.UidLigaAsociado IS NULL AND pt.DtmFechaDeRegistro = (select MAX(DtmFechaDeRegistro) from PagosTarjeta where IdReferencia = lu.IdReferencia) and cl.UidCliente = '" + UidCliente + "' UNION select lu.*, us.UidUsuario, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion where lu.UidLigaAsociado IS NOT NULL AND pt.DtmFechaDeRegistro = (select MAX(pata.DtmFechaDeRegistro) from PagosTarjeta pata, LigasUrls ls where pata.IdReferencia = ls.IdReferencia and ls.UidLigaAsociado = lu.UidLigaAsociado) and cl.UidCliente = '" + UidCliente + "'";
            query.CommandText = "select lu.*, us.UidUsuario, us.VchNombre, us.VchApePaterno, us.VchApeMaterno, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion, (select DcmImporte from LigasUrls where UidLigaAsociado = lu.UidLigaAsociado and UidPromocion IS NULL) as ImporteReal from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join FranquiciasUsuarios fu on fu.UidUsuario = us.UidUsuario left join Franquiciatarios fr on fr.UidFranquiciatarios = fu.UidFranquicia left join Promociones pr on pr.UidPromocion = lu.UidPromocion where pt.VchEstatus IS NULL and lu.UidPromocion IS NULL and lu.UidPropietario = fr.UidFranquiciatarios and fr.UidFranquiciatarios = '" + UidFranquicia + "' UNION select lu.*, us.UidUsuario, us.VchNombre, us.VchApePaterno, us.VchApeMaterno, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion, (select DcmImporte from LigasUrls where UidLigaAsociado = lu.UidLigaAsociado and UidPromocion IS NULL) as ImporteReal from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join FranquiciasUsuarios fu on fu.UidUsuario = us.UidUsuario left join Franquiciatarios fr on fr.UidFranquiciatarios = fu.UidFranquicia left join Promociones pr on pr.UidPromocion = lu.UidPromocion where lu.UidPropietario = fr.UidFranquiciatarios and lu.UidLigaAsociado IS NULL AND pt.DtmFechaDeRegistro = (select MAX(DtmFechaDeRegistro) from PagosTarjeta where IdReferencia = lu.IdReferencia) and fr.UidFranquiciatarios = '" + UidFranquicia + "' UNION select lu.*, us.UidUsuario, us.VchNombre, us.VchApePaterno, us.VchApeMaterno, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion, (select DcmImporte from LigasUrls where UidLigaAsociado = lu.UidLigaAsociado and UidPromocion IS NULL) as ImporteReal from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join FranquiciasUsuarios fu on fu.UidUsuario = us.UidUsuario left join Franquiciatarios fr on fr.UidFranquiciatarios = fu.UidFranquicia left join Promociones pr on pr.UidPromocion = lu.UidPromocion where lu.UidPropietario = fr.UidFranquiciatarios and lu.UidLigaAsociado IS NOT NULL AND pt.DtmFechaDeRegistro = (select MAX(pata.DtmFechaDeRegistro) from PagosTarjeta pata, LigasUrls ls where pata.IdReferencia = ls.IdReferencia and ls.UidLigaAsociado = lu.UidLigaAsociado) and fr.UidFranquiciatarios = '" + UidFranquicia + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                string VchColor = "#007bff";
                decimal ImporteReal = 0;

                if (!string.IsNullOrEmpty(item["VchEstatus"].ToString()))
                {
                    switch (item["VchEstatus"].ToString())
                    {
                        case "approved":
                            VchColor = "#4caf50 ";
                            break;
                        case "denied":
                            VchColor = "#ff9800 ";
                            break;
                        case "error":
                            VchColor = "#f55145 ";
                            break;
                    }
                }

                if (item["VchPromocion"].ToString() != "")
                {
                    ImporteReal = decimal.Parse(item.IsNull("ImporteReal") ? "0.00" : item["ImporteReal"].ToString());
                }
                else
                {
                    ImporteReal = decimal.Parse(item.IsNull("DcmImporte") ? "0.00" : item["DcmImporte"].ToString());
                }

                ligasUrlsGridViewModel = new LigasUrlsGridViewModel()
                {
                    IdReferencia = item["IdReferencia"].ToString(),
                    VchUrl = item["VchUrl"].ToString(),
                    VchIdentificador = item["VchIdentificador"].ToString(),
                    VchConcepto = item["VchConcepto"].ToString(),
                    DcmImporte = ImporteReal,
                    DtVencimiento = DateTime.Parse(item.IsNull("DtVencimiento") ? "2020-02-18 16:57:39.113" : item["DtVencimiento"].ToString()),
                    VchEstatus = item.IsNull("VchEstatus") ? "Pendiente" : item["VchEstatus"].ToString(),
                    VchAsunto = item["VchAsunto"].ToString(),
                    VchColor = VchColor,
                    DtRegistro = DateTime.Parse(item["DtRegistro"].ToString()),
                    DcmImportePromocion = decimal.Parse(item.IsNull("DcmImportePromocion") ? "0.00" : item["DcmImportePromocion"].ToString()),
                    VchPromocion = item.IsNull("VchPromocion") ? "CONTADO" : item["VchPromocion"].ToString(),
                    UidLigaAsociado = item.IsNull("UidLigaAsociado") ? Guid.Empty : Guid.Parse(item["UidLigaAsociado"].ToString()),
                    VchNombre = item["VchNombre"].ToString(),
                    VchApePaterno = item["VchApePaterno"].ToString(),
                    VchApeMaterno = item["VchApeMaterno"].ToString()
                };

                lsLigasUrlsGridViewModel.Add(ligasUrlsGridViewModel);
            }

            return lsLigasUrlsGridViewModel.OrderByDescending(x => x.DtRegistro).ToList();
        }
        public List<LigasUrlsGridViewModel> BuscarLigasFranquicia(Guid UidFranquicia, string Identificador, string Nombre, string ApePaterno, string ApeMaterno, string Asunto, string Concepto, decimal ImporteMayor, decimal ImporteMenor, string RegistroDesde, string RegistroHasta, string VencimientoDesde, string VencimientoHasta, string Estatus)
        {
            List<LigasUrlsGridViewModel> lsLigasUrlsGridViewModel = new List<LigasUrlsGridViewModel>();

            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "sp_LigasBuscarFranquicia";
            try
            {
                if (UidFranquicia != Guid.Empty)
                {
                    comando.Parameters.Add("@UidFranquicia", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidFranquicia"].Value = UidFranquicia;
                }
                if (Identificador != string.Empty)
                {
                    comando.Parameters.Add("@Identificador", SqlDbType.VarChar, 50);
                    comando.Parameters["@Identificador"].Value = Identificador;
                }
                if (Nombre != string.Empty)
                {
                    comando.Parameters.Add("@Nombre", SqlDbType.VarChar, 50);
                    comando.Parameters["@Nombre"].Value = Nombre;
                }
                if (ApePaterno != string.Empty)
                {
                    comando.Parameters.Add("@ApePaterno", SqlDbType.VarChar, 50);
                    comando.Parameters["@ApePaterno"].Value = ApePaterno;
                }
                if (ApeMaterno != string.Empty)
                {
                    comando.Parameters.Add("@ApeMaterno", SqlDbType.VarChar, 50);
                    comando.Parameters["@ApeMaterno"].Value = ApeMaterno;
                }
                if (Asunto != string.Empty)
                {
                    comando.Parameters.Add("@Asunto", SqlDbType.VarChar, 50);
                    comando.Parameters["@Asunto"].Value = Asunto;
                }
                if (Concepto != string.Empty)
                {
                    comando.Parameters.Add("@Concepto", SqlDbType.VarChar, 50);
                    comando.Parameters["@Concepto"].Value = Concepto;
                }
                if (RegistroDesde != string.Empty)
                {
                    comando.Parameters.Add("@RegistroDesde", SqlDbType.DateTime);
                    comando.Parameters["@RegistroDesde"].Value = RegistroDesde;
                }
                if (RegistroHasta != string.Empty)
                {
                    comando.Parameters.Add("@RegistroHasta", SqlDbType.Date);
                    comando.Parameters["@RegistroHasta"].Value = RegistroHasta;
                }
                if (VencimientoDesde != string.Empty)
                {
                    comando.Parameters.Add("@VencimientoDesde", SqlDbType.DateTime);
                    comando.Parameters["@VencimientoDesde"].Value = VencimientoDesde;
                }
                if (VencimientoHasta != string.Empty)
                {
                    comando.Parameters.Add("@VencimientoHasta", SqlDbType.Date);
                    comando.Parameters["@VencimientoHasta"].Value = VencimientoHasta;
                }
                if (ImporteMayor != 0)
                {
                    comando.Parameters.Add("@ImporteMayor", SqlDbType.Decimal);
                    comando.Parameters["@ImporteMayor"].Value = ImporteMayor;
                }
                if (ImporteMenor != 0)
                {
                    comando.Parameters.Add("@ImporteMenor", SqlDbType.Decimal);
                    comando.Parameters["@ImporteMenor"].Value = ImporteMenor;
                }
                if (Estatus != string.Empty)
                {
                    comando.Parameters.Add("@Estatus", SqlDbType.VarChar, 50);
                    comando.Parameters["@Estatus"].Value = Estatus;
                }

                foreach (DataRow item in this.Busquedas(comando).Rows)
                {
                    string VchColor = "#007bff";
                    decimal ImporteReal = 0;

                    if (!string.IsNullOrEmpty(item["VchEstatus"].ToString()))
                    {
                        switch (item["VchEstatus"].ToString())
                        {
                            case "approved":
                                VchColor = "#4caf50 ";
                                break;
                            case "denied":
                                VchColor = "#ff9800 ";
                                break;
                            case "error":
                                VchColor = "#f55145 ";
                                break;
                        }
                    }

                    if (item["VchPromocion"].ToString() != "")
                    {
                        ImporteReal = decimal.Parse(item.IsNull("ImporteReal") ? "0.00" : item["ImporteReal"].ToString());
                    }
                    else
                    {
                        ImporteReal = decimal.Parse(item.IsNull("DcmImporte") ? "0.00" : item["DcmImporte"].ToString());
                    }

                    ligasUrlsGridViewModel = new LigasUrlsGridViewModel()
                    {
                        IdReferencia = item["IdReferencia"].ToString(),
                        VchUrl = item["VchUrl"].ToString(),
                        VchIdentificador = item["VchIdentificador"].ToString(),
                        VchConcepto = item["VchConcepto"].ToString(),
                        DcmImporte = ImporteReal,
                        DtVencimiento = DateTime.Parse(item.IsNull("DtVencimiento") ? "2020-02-18 16:57:39.113" : item["DtVencimiento"].ToString()),
                        VchEstatus = item.IsNull("VchEstatus") ? "Pendiente" : item["VchEstatus"].ToString(),
                        VchAsunto = item["VchAsunto"].ToString(),
                        VchColor = VchColor,
                        DtRegistro = DateTime.Parse(item["DtRegistro"].ToString()),
                        DcmImportePromocion = decimal.Parse(item.IsNull("DcmImportePromocion") ? "0.00" : item["DcmImportePromocion"].ToString()),
                        VchPromocion = item.IsNull("VchPromocion") ? "CONTADO" : item["VchPromocion"].ToString(),
                        UidLigaAsociado = item.IsNull("UidLigaAsociado") ? Guid.Empty : Guid.Parse(item["UidLigaAsociado"].ToString()),
                        VchNombre = item["VchNombre"].ToString(),
                        VchApePaterno = item["VchApePaterno"].ToString(),
                        VchApeMaterno = item["VchApeMaterno"].ToString()
                    };

                    lsLigasUrlsGridViewModel.Add(ligasUrlsGridViewModel);
                }

                return lsLigasUrlsGridViewModel.OrderByDescending(x => x.DtRegistro).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Metodos Escuela

        #region ReporteLigasPadres
        public List<LigasUsuariosFinalGridViewModel> ConsultarLigaPadres(Guid UidUsuario)
        {
            List<LigasUsuariosFinalGridViewModel> lsLigasUsuariosFinalGridViewModel = new List<LigasUsuariosFinalGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;
            // ==>MUESTRA LOS PENDIENTES<==  query.CommandText = "select lu.*, cl.VchNombreComercial, us.UidUsuario, us.VchNombre, us.VchApePaterno, us.VchApeMaterno, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion, (select DcmImporte from LigasUrls where UidLigaAsociado = lu.UidLigaAsociado and UidPromocion IS NULL) as ImporteReal from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion left join AuxiliarPago ap on ap.IdReferencia = lu.IdReferencia where ap.IdReferencia IS NULL and pt.VchEstatus IS NULL and lu.UidPromocion IS NULL and lu.UidEvento IS NULL and lu.UidPropietario = cl.UidCliente and us.UidUsuario = '" + UidUsuario + "' UNION select lu.*, cl.VchNombreComercial, us.UidUsuario, us.VchNombre, us.VchApePaterno, us.VchApeMaterno, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion, (select DcmImporte from LigasUrls where UidLigaAsociado = lu.UidLigaAsociado and UidPromocion IS NULL) as ImporteReal from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion left join AuxiliarPago ap on ap.IdReferencia = lu.IdReferencia where ap.IdReferencia IS NULL and lu.UidPropietario = cl.UidCliente and lu.UidLigaAsociado IS NULL AND pt.DtmFechaDeRegistro = (select MAX(DtmFechaDeRegistro) from PagosTarjeta where IdReferencia = lu.IdReferencia) and us.UidUsuario = '" + UidUsuario + "' UNION select lu.*, cl.VchNombreComercial, us.UidUsuario, us.VchNombre, us.VchApePaterno, us.VchApeMaterno, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion, (select DcmImporte from LigasUrls where UidLigaAsociado = lu.UidLigaAsociado and UidPromocion IS NULL) as ImporteReal from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion where lu.UidPropietario = cl.UidCliente and lu.UidLigaAsociado IS NOT NULL AND pt.DtmFechaDeRegistro = (select MAX(pata.DtmFechaDeRegistro) from PagosTarjeta pata, LigasUrls ls where pata.IdReferencia = ls.IdReferencia and ls.UidLigaAsociado = lu.UidLigaAsociado) and us.UidUsuario = '" + UidUsuario + "'";
            // ==>MUESTRA LIGAS DE COBROSMASIVOS<== query.CommandText = "select lu.*, cl.VchNombreComercial, us.UidUsuario, us.VchNombre, us.VchApePaterno, us.VchApeMaterno, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion, (select DcmImporte from LigasUrls where UidLigaAsociado = lu.UidLigaAsociado and UidPromocion IS NULL) as ImporteReal from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion left join AuxiliarPago ap on ap.IdReferencia = lu.IdReferencia where ap.IdReferencia IS NULL and lu.UidPropietario = cl.UidCliente and lu.UidLigaAsociado IS NULL AND pt.DtmFechaDeRegistro = (select MAX(DtmFechaDeRegistro) from PagosTarjeta where IdReferencia = lu.IdReferencia) and us.UidUsuario = '" + UidUsuario + "' UNION select lu.*, cl.VchNombreComercial, us.UidUsuario, us.VchNombre, us.VchApePaterno, us.VchApeMaterno, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion, (select DcmImporte from LigasUrls where UidLigaAsociado = lu.UidLigaAsociado and UidPromocion IS NULL) as ImporteReal from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion where lu.UidPropietario = cl.UidCliente and lu.UidLigaAsociado IS NOT NULL AND pt.DtmFechaDeRegistro = (select MAX(pata.DtmFechaDeRegistro) from PagosTarjeta pata, LigasUrls ls where pata.IdReferencia = ls.IdReferencia and ls.UidLigaAsociado = lu.UidLigaAsociado) and us.UidUsuario = '" + UidUsuario + "'";
            query.CommandText = "select lu.*, cl.VchNombreComercial, us.UidUsuario, us.VchNombre, us.VchApePaterno, us.VchApeMaterno, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion, (select DcmImporte from LigasUrls where UidLigaAsociado = lu.UidLigaAsociado and UidPromocion IS NULL) as ImporteReal from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion left join AuxiliarPago ap on ap.IdReferencia = lu.IdReferencia left join UsuariosAlumnos ua on ua.UidUsuario = us.UidUsuario left join Alumnos al on al.UidAlumno = ua.UidAlumno left join ColegiaturasAlumnos ca on ca.UidAlumno = al.UidAlumno left join Colegiaturas co on co.UidColegiatura = ca.UidColegiatura left join FechasColegiaturas fc on fc.UidColegiatura = co.UidColegiatura where ap.IdReferencia IS NULL and lu.UidPropietario = cl.UidCliente and lu.UidLigaAsociado IS NULL AND pt.DtmFechaDeRegistro = (select MAX(DtmFechaDeRegistro) from PagosTarjeta where IdReferencia = lu.IdReferencia) and fc.UidFechaColegiatura = lu.UidFechaColegiatura and lu.UidFechaColegiatura IS NOT NULL and us.UidUsuario = '" + UidUsuario + "' UNION select lu.*, cl.VchNombreComercial, us.UidUsuario, us.VchNombre, us.VchApePaterno, us.VchApeMaterno, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion, (select DcmImporte from LigasUrls where UidLigaAsociado = lu.UidLigaAsociado and UidPromocion IS NULL) as ImporteReal from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion left join UsuariosAlumnos ua on ua.UidUsuario = us.UidUsuario left join Alumnos al on al.UidAlumno = ua.UidAlumno left join ColegiaturasAlumnos ca on ca.UidAlumno = al.UidAlumno left join Colegiaturas co on co.UidColegiatura = ca.UidColegiatura left join FechasColegiaturas fc on fc.UidColegiatura = co.UidColegiatura where lu.UidPropietario = cl.UidCliente and lu.UidLigaAsociado IS NOT NULL AND pt.DtmFechaDeRegistro = (select MAX(pata.DtmFechaDeRegistro) from PagosTarjeta pata, LigasUrls ls where pata.IdReferencia = ls.IdReferencia and ls.UidLigaAsociado = lu.UidLigaAsociado) and fc.UidFechaColegiatura = lu.UidFechaColegiatura and lu.UidFechaColegiatura IS NOT NULL and us.UidUsuario = '" + UidUsuario + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                string VchColor = "#007bff";
                decimal ImporteReal = 0;
                bool blPagar = true;

                if (!string.IsNullOrEmpty(item["VchEstatus"].ToString()))
                {
                    switch (item["VchEstatus"].ToString())
                    {
                        case "approved":
                            VchColor = "#4caf50 ";
                            blPagar = false;
                            break;
                        case "denied":
                            VchColor = "#ff9800 ";
                            blPagar = true;
                            break;
                        case "error":
                            VchColor = "#f55145 ";
                            blPagar = true;
                            break;
                    }
                }


                ImporteReal = decimal.Parse(item.IsNull("DcmImporte") ? "0.00" : item["DcmImporte"].ToString());


                ligasUsuariosFinalGridViewModel = new LigasUsuariosFinalGridViewModel()
                {
                    UidLigaUrl = Guid.Parse(item["UidLigaUrl"].ToString()),
                    IdReferencia = item["IdReferencia"].ToString(),
                    VchUrl = item["VchUrl"].ToString(),
                    VchIdentificador = item["VchIdentificador"].ToString(),
                    VchConcepto = item["VchConcepto"].ToString(),
                    DcmImporte = ImporteReal,
                    DtVencimiento = DateTime.Parse(item.IsNull("DtVencimiento") ? "2020-02-18 16:57:39.113" : item["DtVencimiento"].ToString()),
                    VchEstatus = item.IsNull("VchEstatus") ? "Pendiente" : item["VchEstatus"].ToString(),
                    VchAsunto = item["VchAsunto"].ToString(),
                    VchColor = VchColor,
                    DtRegistro = DateTime.Parse(item["DtRegistro"].ToString()),
                    DcmImportePromocion = decimal.Parse(item.IsNull("DcmImportePromocion") ? "0.00" : item["DcmImportePromocion"].ToString()),
                    VchPromocion = item.IsNull("VchPromocion") ? "CONTADO" : item["VchPromocion"].ToString(),
                    UidLigaAsociado = item.IsNull("UidLigaAsociado") ? Guid.Empty : Guid.Parse(item["UidLigaAsociado"].ToString()),
                    VchNombreComercial = item["VchNombreComercial"].ToString(),
                    blPagar = blPagar
                };

                lsLigasUsuariosFinalGridViewModel.Add(ligasUsuariosFinalGridViewModel);
            }

            return lsLigasUsuariosFinalGridViewModel.OrderByDescending(x => x.DtRegistro).ToList();
        }
        public List<LigasUsuariosFinalGridViewModel> BuscarLigasPadres(Guid UidUsuario, string Identificador, string Asunto, string Concepto, decimal ImporteMayor, decimal ImporteMenor, string RegistroDesde, string RegistroHasta, string VencimientoDesde, string VencimientoHasta, string Estatus)
        {
            List<LigasUsuariosFinalGridViewModel> lsLigasUsuariosFinalGridViewModel = new List<LigasUsuariosFinalGridViewModel>();

            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "sp_LigasBuscarUsuarioFinal";
            try
            {
                comando.Parameters.Add("@UidUsuario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidUsuario"].Value = UidUsuario;

                if (Identificador != string.Empty)
                {
                    comando.Parameters.Add("@Identificador", SqlDbType.VarChar, 50);
                    comando.Parameters["@Identificador"].Value = Identificador;
                }
                if (Asunto != string.Empty)
                {
                    comando.Parameters.Add("@Asunto", SqlDbType.VarChar, 50);
                    comando.Parameters["@Asunto"].Value = Asunto;
                }
                if (Concepto != string.Empty)
                {
                    comando.Parameters.Add("@Concepto", SqlDbType.VarChar, 50);
                    comando.Parameters["@Concepto"].Value = Concepto;
                }
                if (RegistroDesde != string.Empty)
                {
                    comando.Parameters.Add("@RegistroDesde", SqlDbType.DateTime);
                    comando.Parameters["@RegistroDesde"].Value = RegistroDesde;
                }
                if (RegistroHasta != string.Empty)
                {
                    comando.Parameters.Add("@RegistroHasta", SqlDbType.Date);
                    comando.Parameters["@RegistroHasta"].Value = RegistroHasta;
                }
                if (VencimientoDesde != string.Empty)
                {
                    comando.Parameters.Add("@VencimientoDesde", SqlDbType.DateTime);
                    comando.Parameters["@VencimientoDesde"].Value = VencimientoDesde;
                }
                if (VencimientoHasta != string.Empty)
                {
                    comando.Parameters.Add("@VencimientoHasta", SqlDbType.Date);
                    comando.Parameters["@VencimientoHasta"].Value = VencimientoHasta;
                }
                if (ImporteMayor != 0)
                {
                    comando.Parameters.Add("@ImporteMayor", SqlDbType.Decimal);
                    comando.Parameters["@ImporteMayor"].Value = ImporteMayor;
                }
                if (ImporteMenor != 0)
                {
                    comando.Parameters.Add("@ImporteMenor", SqlDbType.Decimal);
                    comando.Parameters["@ImporteMenor"].Value = ImporteMenor;
                }
                if (Estatus != string.Empty)
                {
                    comando.Parameters.Add("@Estatus", SqlDbType.VarChar, 50);
                    comando.Parameters["@Estatus"].Value = Estatus;
                }

                foreach (DataRow item in this.Busquedas(comando).Rows)
                {
                    string VchColor = "#007bff";
                    decimal ImporteReal = 0;
                    bool blPagar = true;

                    if (!string.IsNullOrEmpty(item["VchEstatus"].ToString()))
                    {
                        switch (item["VchEstatus"].ToString())
                        {
                            case "approved":
                                VchColor = "#4caf50 ";
                                blPagar = false;
                                break;
                            case "denied":
                                VchColor = "#ff9800 ";
                                blPagar = true;
                                break;
                            case "error":
                                VchColor = "#f55145 ";
                                blPagar = true;
                                break;
                        }
                    }

                    ImporteReal = decimal.Parse(item.IsNull("DcmImporte") ? "0.00" : item["DcmImporte"].ToString());

                    ligasUsuariosFinalGridViewModel = new LigasUsuariosFinalGridViewModel()
                    {
                        UidLigaUrl = Guid.Parse(item["UidLigaUrl"].ToString()),
                        IdReferencia = item["IdReferencia"].ToString(),
                        VchUrl = item["VchUrl"].ToString(),
                        VchIdentificador = item["VchIdentificador"].ToString(),
                        VchConcepto = item["VchConcepto"].ToString(),
                        DcmImporte = ImporteReal,
                        DtVencimiento = DateTime.Parse(item.IsNull("DtVencimiento") ? "2020-02-18 16:57:39.113" : item["DtVencimiento"].ToString()),
                        VchEstatus = item.IsNull("VchEstatus") ? "Pendiente" : item["VchEstatus"].ToString(),
                        VchAsunto = item["VchAsunto"].ToString(),
                        VchColor = VchColor,
                        DtRegistro = DateTime.Parse(item["DtRegistro"].ToString()),
                        DcmImportePromocion = decimal.Parse(item.IsNull("DcmImportePromocion") ? "0.00" : item["DcmImportePromocion"].ToString()),
                        VchPromocion = item.IsNull("VchPromocion") ? "CONTADO" : item["VchPromocion"].ToString(),
                        UidLigaAsociado = item.IsNull("UidLigaAsociado") ? Guid.Empty : Guid.Parse(item["UidLigaAsociado"].ToString()),
                        VchNombreComercial = item["VchNombreComercial"].ToString(),
                        blPagar = blPagar
                    };

                    lsLigasUsuariosFinalGridViewModel.Add(ligasUsuariosFinalGridViewModel);
                }

                return lsLigasUsuariosFinalGridViewModel.OrderByDescending(x => x.DtRegistro).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region ReporteLigasEscuela
        public List<LigasUrlsGridViewModel> ConsultarEstatusLigaEscuela(Guid UidCliente)
        {
            List<LigasUrlsGridViewModel> lsLigasUrlsGridViewModel = new List<LigasUrlsGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;
            // ==>MUESTRA LOS PENDIENTES<==  query.CommandText = "select lu.*, al.VchNombres as NombresAlumno, al.VchApePaterno as ApePaternoAlumno, al.VchApeMaterno as ApeMaternoAlumno, us.UidUsuario, us.VchNombre, us.VchApePaterno, us.VchApeMaterno, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion, (select DcmImporte from LigasUrls where UidLigaAsociado = lu.UidLigaAsociado and UidPromocion IS NULL) as ImporteReal from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion left join AuxiliarPago ap on ap.IdReferencia = lu.IdReferencia left join UsuariosAlumnos ua on ua.UidUsuario = us.UidUsuario left join Alumnos al on al.UidAlumno = ua.UidAlumno where ap.IdReferencia IS NULL and pt.VchEstatus IS NULL and lu.UidPromocion IS NULL and lu.UidEvento IS NULL and lu.UidFechaColegiatura IS NOT NULL and lu.UidPropietario = cl.UidCliente and cl.UidCliente = '" + UidCliente + "' UNION select lu.*, al.VchNombres as NombresAlumno, al.VchApePaterno as ApePaternoAlumno, al.VchApeMaterno as ApeMaternoAlumno, us.UidUsuario, us.VchNombre, us.VchApePaterno, us.VchApeMaterno, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion, (select DcmImporte from LigasUrls where UidLigaAsociado = lu.UidLigaAsociado and UidPromocion IS NULL) as ImporteReal from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion left join AuxiliarPago ap on ap.IdReferencia = lu.IdReferencia left join UsuariosAlumnos ua on ua.UidUsuario = us.UidUsuario left join Alumnos al on al.UidAlumno = ua.UidAlumno where ap.IdReferencia IS NULL and lu.UidPropietario = cl.UidCliente and lu.UidLigaAsociado IS NULL AND pt.DtmFechaDeRegistro = (select MAX(DtmFechaDeRegistro) from PagosTarjeta where IdReferencia = lu.IdReferencia) AND lu.UidFechaColegiatura IS NOT NULL and cl.UidCliente = '" + UidCliente + "' UNION select lu.*, al.VchNombres as NombresAlumno, al.VchApePaterno as ApePaternoAlumno, al.VchApeMaterno as ApeMaternoAlumno, us.UidUsuario, us.VchNombre, us.VchApePaterno, us.VchApeMaterno, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion, (select DcmImporte from LigasUrls where UidLigaAsociado = lu.UidLigaAsociado and UidPromocion IS NULL) as ImporteReal from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion left join UsuariosAlumnos ua on ua.UidUsuario = us.UidUsuario left join Alumnos al on al.UidAlumno = ua.UidAlumno where lu.UidPropietario = cl.UidCliente and lu.UidLigaAsociado IS NOT NULL AND pt.DtmFechaDeRegistro = (select MAX(pata.DtmFechaDeRegistro) from PagosTarjeta pata, LigasUrls ls where pata.IdReferencia = ls.IdReferencia and ls.UidLigaAsociado = lu.UidLigaAsociado) and lu.UidFechaColegiatura IS NOT NULL and cl.UidCliente = '" + UidCliente + "'";
            query.CommandText = "select lu.*, al.VchNombres as NombresAlumno, al.VchApePaterno as ApePaternoAlumno, al.VchApeMaterno as ApeMaternoAlumno, us.UidUsuario, us.VchNombre, us.VchApePaterno, us.VchApeMaterno, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion, (select DcmImporte from LigasUrls where UidLigaAsociado = lu.UidLigaAsociado and UidPromocion IS NULL) as ImporteReal from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion left join AuxiliarPago ap on ap.IdReferencia = lu.IdReferencia left join UsuariosAlumnos ua on ua.UidUsuario = us.UidUsuario left join Alumnos al on al.UidAlumno = ua.UidAlumno left join ColegiaturasAlumnos ca on ca.UidAlumno = al.UidAlumno left join Colegiaturas co on co.UidColegiatura = ca.UidColegiatura left join FechasColegiaturas fc on fc.UidColegiatura = co.UidColegiatura where ap.IdReferencia IS NULL and lu.UidPropietario = cl.UidCliente and lu.UidLigaAsociado IS NULL AND pt.DtmFechaDeRegistro = (select MAX(DtmFechaDeRegistro) from PagosTarjeta where IdReferencia = lu.IdReferencia) AND fc.UidFechaColegiatura = lu.UidFechaColegiatura and lu.UidFechaColegiatura IS NOT NULL and cl.UidCliente = '" + UidCliente + "' UNION select lu.*, al.VchNombres as NombresAlumno, al.VchApePaterno as ApePaternoAlumno, al.VchApeMaterno as ApeMaternoAlumno, us.UidUsuario, us.VchNombre, us.VchApePaterno, us.VchApeMaterno, pt.VchEstatus, lu.DcmImporte as DcmImportePromocion, pr.VchDescripcion as VchPromocion, (select DcmImporte from LigasUrls where UidLigaAsociado = lu.UidLigaAsociado and UidPromocion IS NULL) as ImporteReal from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente left join Promociones pr on pr.UidPromocion = lu.UidPromocion left join UsuariosAlumnos ua on ua.UidUsuario = us.UidUsuario left join Alumnos al on al.UidAlumno = ua.UidAlumno left join ColegiaturasAlumnos ca on ca.UidAlumno = al.UidAlumno left join Colegiaturas co on co.UidColegiatura = ca.UidColegiatura left join FechasColegiaturas fc on fc.UidColegiatura = co.UidColegiatura where lu.UidPropietario = cl.UidCliente and lu.UidLigaAsociado IS NOT NULL AND pt.DtmFechaDeRegistro = (select MAX(pata.DtmFechaDeRegistro) from PagosTarjeta pata, LigasUrls ls where pata.IdReferencia = ls.IdReferencia and ls.UidLigaAsociado = lu.UidLigaAsociado) and fc.UidFechaColegiatura = lu.UidFechaColegiatura and lu.UidFechaColegiatura IS NOT NULL and cl.UidCliente = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                string VchColor = "#007bff";
                decimal ImporteReal = 0;

                if (!string.IsNullOrEmpty(item["VchEstatus"].ToString()))
                {
                    switch (item["VchEstatus"].ToString())
                    {
                        case "approved":
                            VchColor = "#4caf50 ";
                            break;
                        case "denied":
                            VchColor = "#ff9800 ";
                            break;
                        case "error":
                            VchColor = "#f55145 ";
                            break;
                    }
                }

                ImporteReal = decimal.Parse(item.IsNull("DcmImporte") ? "0.00" : item["DcmImporte"].ToString());

                ligasUrlsGridViewModel = new LigasUrlsGridViewModel()
                {
                    IdReferencia = item["IdReferencia"].ToString(),
                    VchUrl = item["VchUrl"].ToString(),
                    VchIdentificador = item["VchIdentificador"].ToString(),
                    VchConcepto = item["VchConcepto"].ToString(),
                    DcmImporte = ImporteReal,
                    DtVencimiento = DateTime.Parse(item.IsNull("DtVencimiento") ? "2020-02-18 16:57:39.113" : item["DtVencimiento"].ToString()),
                    VchEstatus = item.IsNull("VchEstatus") ? "Pendiente" : item["VchEstatus"].ToString(),
                    VchAsunto = item["VchAsunto"].ToString(),
                    VchColor = VchColor,
                    DtRegistro = DateTime.Parse(item["DtRegistro"].ToString()),
                    DcmImportePromocion = decimal.Parse(item.IsNull("DcmImportePromocion") ? "0.00" : item["DcmImportePromocion"].ToString()),
                    VchPromocion = item.IsNull("VchPromocion") ? "CONTADO" : item["VchPromocion"].ToString(),
                    UidLigaAsociado = item.IsNull("UidLigaAsociado") ? Guid.Empty : Guid.Parse(item["UidLigaAsociado"].ToString()),
                    VchNombre = item["VchNombre"].ToString(),
                    VchApePaterno = item["VchApePaterno"].ToString(),
                    VchApeMaterno = item["VchApeMaterno"].ToString(),
                    
                    VchNombreAlumno = item["NombresAlumno"].ToString(),
                    VchApePaternoAlumno = item["ApePaternoAlumno"].ToString(),
                    VchApeMaternoAlumno = item["ApeMaternoAlumno"].ToString()
                };

                lsLigasUrlsGridViewModel.Add(ligasUrlsGridViewModel);
            }

            return lsLigasUrlsGridViewModel.OrderByDescending(x => x.DtRegistro).ToList();
        }
        public List<LigasUrlsGridViewModel> BuscarLigaEscuela(Guid UidCliente, string Identificador, string Nombre, string ApePaterno, string ApeMaterno, string Concepto, decimal ImporteMayor, decimal ImporteMenor, string RegistroDesde, string RegistroHasta)
        {
            List<LigasUrlsGridViewModel> lsLigasUrlsGridViewModel = new List<LigasUrlsGridViewModel>();

            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "sp_LigasEscuelaBuscar";
            try
            {
                if (UidCliente != Guid.Empty)
                {
                    comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                    comando.Parameters["@UidCliente"].Value = UidCliente;
                }
                if (Identificador != string.Empty)
                {
                    comando.Parameters.Add("@Identificador", SqlDbType.VarChar, 50);
                    comando.Parameters["@Identificador"].Value = Identificador;
                }
                if (Nombre != string.Empty)
                {
                    comando.Parameters.Add("@Nombre", SqlDbType.VarChar, 50);
                    comando.Parameters["@Nombre"].Value = Nombre;
                }
                if (ApePaterno != string.Empty)
                {
                    comando.Parameters.Add("@ApePaterno", SqlDbType.VarChar, 50);
                    comando.Parameters["@ApePaterno"].Value = ApePaterno;
                }
                if (ApeMaterno != string.Empty)
                {
                    comando.Parameters.Add("@ApeMaterno", SqlDbType.VarChar, 50);
                    comando.Parameters["@ApeMaterno"].Value = ApeMaterno;
                }
                if (Concepto != string.Empty)
                {
                    comando.Parameters.Add("@Concepto", SqlDbType.VarChar, 50);
                    comando.Parameters["@Concepto"].Value = Concepto;
                }
                if (RegistroDesde != string.Empty)
                {
                    comando.Parameters.Add("@RegistroDesde", SqlDbType.DateTime);
                    comando.Parameters["@RegistroDesde"].Value = RegistroDesde;
                }
                if (RegistroHasta != string.Empty)
                {
                    comando.Parameters.Add("@RegistroHasta", SqlDbType.Date);
                    comando.Parameters["@RegistroHasta"].Value = RegistroHasta;
                }
                if (ImporteMayor != 0)
                {
                    comando.Parameters.Add("@ImporteMayor", SqlDbType.Decimal);
                    comando.Parameters["@ImporteMayor"].Value = ImporteMayor;
                }
                if (ImporteMenor != 0)
                {
                    comando.Parameters.Add("@ImporteMenor", SqlDbType.Decimal);
                    comando.Parameters["@ImporteMenor"].Value = ImporteMenor;
                }
                
                foreach (DataRow item in this.Busquedas(comando).Rows)
                {
                    string VchColor = "#007bff";
                    decimal ImporteReal = 0;

                    if (!string.IsNullOrEmpty(item["VchEstatus"].ToString()))
                    {
                        switch (item["VchEstatus"].ToString())
                        {
                            case "approved":
                                VchColor = "#4caf50 ";
                                break;
                            case "denied":
                                VchColor = "#ff9800 ";
                                break;
                            case "error":
                                VchColor = "#f55145 ";
                                break;
                        }
                    }

                    ImporteReal = decimal.Parse(item.IsNull("DcmImporte") ? "0.00" : item["DcmImporte"].ToString());

                    ligasUrlsGridViewModel = new LigasUrlsGridViewModel()
                    {
                        IdReferencia = item["IdReferencia"].ToString(),
                        VchUrl = item["VchUrl"].ToString(),
                        VchIdentificador = item["VchIdentificador"].ToString(),
                        VchConcepto = item["VchConcepto"].ToString(),
                        DcmImporte = ImporteReal,
                        DtVencimiento = DateTime.Parse(item.IsNull("DtVencimiento") ? "2020-02-18 16:57:39.113" : item["DtVencimiento"].ToString()),
                        VchEstatus = item.IsNull("VchEstatus") ? "Pendiente" : item["VchEstatus"].ToString(),
                        VchAsunto = item["VchAsunto"].ToString(),
                        VchColor = VchColor,
                        DtRegistro = DateTime.Parse(item["DtRegistro"].ToString()),
                        DcmImportePromocion = decimal.Parse(item.IsNull("DcmImportePromocion") ? "0.00" : item["DcmImportePromocion"].ToString()),
                        VchPromocion = item.IsNull("VchPromocion") ? "CONTADO" : item["VchPromocion"].ToString(),
                        UidLigaAsociado = item.IsNull("UidLigaAsociado") ? Guid.Empty : Guid.Parse(item["UidLigaAsociado"].ToString()),
                        VchNombre = item["VchNombre"].ToString(),
                        VchApePaterno = item["VchApePaterno"].ToString(),
                        VchApeMaterno = item["VchApeMaterno"].ToString(),

                        VchNombreAlumno = item["NombresAlumno"].ToString(),
                        VchApePaternoAlumno = item["ApePaternoAlumno"].ToString(),
                        VchApeMaternoAlumno = item["ApeMaternoAlumno"].ToString()
                    };

                    lsLigasUrlsGridViewModel.Add(ligasUrlsGridViewModel);
                }

                return lsLigasUrlsGridViewModel.OrderByDescending(x => x.DtRegistro).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion  

        #endregion
    }
}
