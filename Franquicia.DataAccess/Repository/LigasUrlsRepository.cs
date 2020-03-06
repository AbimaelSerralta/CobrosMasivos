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

        public List<LigasUrlsGridViewModel> ConsultarEstatusLiga(Guid UidCliente)
        {
            List<LigasUrlsGridViewModel> lsLigasUrlsGridViewModel = new List<LigasUrlsGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;
            //query.CommandText = "select lu.*, us.UidUsuario, pt.VchEstatus from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente where pt.VchEstatus != 'error' and pt.VchEstatus != 'denied' and cl.UidCliente = '" + UidCliente + "' UNION select lu.*, us.UidUsuario, pt.VchEstatus from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente where pt.VchEstatus = 'error' or pt.VchEstatus = 'denied' or pt.VchEstatus IS NULL and cl.UidCliente = '" + UidCliente + "'";
            //query.CommandText = "select lu.*, us.UidUsuario, pt.VchEstatus from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente where pt.VchEstatus != 'error' and pt.VchEstatus != 'denied' and cl.UidCliente = '" + UidCliente + "' UNION select lu.*, us.UidUsuario, pt.VchEstatus from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente where pt.VchEstatus IS NULL and cl.UidCliente = '" + UidCliente + "'";
            query.CommandText = "select lu.*, us.UidUsuario, pt.VchEstatus from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente where pt.VchEstatus IS NULL and cl.UidCliente = '" + UidCliente + "' UNION select lu.*, us.UidUsuario, pt.VchEstatus from LigasUrls lu left join PagosTarjeta pt  on pt.IdReferencia = lu.IdReferencia left join Usuarios us on us.UidUsuario = lu.UidUsuario left join ClientesUsuarios cu on cu.UidUsuario = us.UidUsuario left join Clientes cl on cl.UidCliente = cu.UidCliente where pt.DtmFechaDeRegistro = (select MAX(DtmFechaDeRegistro) from PagosTarjeta where IdReferencia = lu.IdReferencia) and cl.UidCliente = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                string VchColor = "#007bff";

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

                ligasUrlsGridViewModel = new LigasUrlsGridViewModel()
                {
                    IdReferencia = item["IdReferencia"].ToString(),
                    VchUrl = item["VchUrl"].ToString(),
                    VchIdentificador = item["VchIdentificador"].ToString(),
                    VchConcepto = item["VchConcepto"].ToString(),
                    DcmImporte = decimal.Parse(item.IsNull("DcmImporte") ? "0.00" : item["DcmImporte"].ToString()),
                    DtVencimiento = DateTime.Parse(item.IsNull("DtVencimiento") ? "2020-02-18 16:57:39.113" : item["DtVencimiento"].ToString()),
                    VchEstatus = item.IsNull("VchEstatus") ? "Pendiente" : item["VchEstatus"].ToString(),
                    VchAsunto = item["VchAsunto"].ToString(),
                    VchColor = VchColor,
                    DtRegistro = DateTime.Parse(item["DtRegistro"].ToString())
                };

                lsLigasUrlsGridViewModel.Add(ligasUrlsGridViewModel);
            }

            return lsLigasUrlsGridViewModel.OrderByDescending(x => x.DtRegistro).ToList();
        }

        public List<LigasUrlsGridViewModel> BuscarLigas(Guid UidCliente, string Identificador, string Usuario, string Asunto, string Concepto, decimal ImporteMayor, decimal ImporteMenor, string VencimientoDesde, string VencimientoHasta)
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
                if (Usuario != string.Empty)
                {
                    comando.Parameters.Add("@Usuario", SqlDbType.VarChar, 50);
                    comando.Parameters["@Usuario"].Value = Usuario;
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
                //if (Estatus != Guid.Empty)
                //{
                //    comando.Parameters.Add("@UidEstatus", SqlDbType.UniqueIdentifier);
                //    comando.Parameters["@UidEstatus"].Value = Estatus;
                //}

                foreach (DataRow item in this.Busquedas(comando).Rows)
                {
                    string VchColor = "#007bff";

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

                    ligasUrlsGridViewModel = new LigasUrlsGridViewModel()
                    {
                        IdReferencia = item["IdReferencia"].ToString(),
                        VchUrl = item["VchUrl"].ToString(),
                        VchIdentificador = item["VchIdentificador"].ToString(),
                        VchConcepto = item["VchConcepto"].ToString(),
                        DcmImporte = decimal.Parse(item.IsNull("DcmImporte") ? "0.00" : item["DcmImporte"].ToString()),
                        DtVencimiento = DateTime.Parse(item.IsNull("DtVencimiento") ? "2020-02-18 16:57:39.113" : item["DtVencimiento"].ToString()),
                        VchEstatus = item.IsNull("VchEstatus") ? "Pendiente" : item["VchEstatus"].ToString(),
                        VchAsunto = item["VchAsunto"].ToString(),
                        VchColor = VchColor,
                        DtRegistro = DateTime.Parse(item["DtRegistro"].ToString())
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

    }
}
