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
    public class PerfilesRepository : SqlDataRepository
    {
        private AppWebRepository _appWebRepository = new AppWebRepository();

        public AppWebRepository appWebRepository
        {
            get { return _appWebRepository; }
            set { _appWebRepository = value; }
        }

        private Perfiles _perfiles = new Perfiles();
        public Perfiles perfiles
        {
            get { return _perfiles; }
            set { _perfiles = value; }
        }

        private PerfilesGridViewModel _perfilesGridViewModel = new PerfilesGridViewModel();
        public PerfilesGridViewModel perfilesGridViewModel
        {
            get { return _perfilesGridViewModel; }
            set { _perfilesGridViewModel = value; }
        }
        
        private PerfilesDropDownListModel _perfilesDropDownListModel = new PerfilesDropDownListModel();
        public PerfilesDropDownListModel perfilesDropDownListModel
        {
            get { return _perfilesDropDownListModel; }
            set { _perfilesDropDownListModel = value; }
        }

        public List<PerfilesGridViewModel> CargarPerfilesGridViewModel()
        {
            List<PerfilesGridViewModel> lsPerfilesGridViewModel = new List<PerfilesGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select sp.UidSegPerfil, sp.VchNombre, tp.UidTipoPerfil, sp.UidModuloInicial, tp.VchDescripcion as VchPerfil, es.VchDescripcion as VchEstatus, es.VchIcono from SegPerfiles sp, TiposPerfiles tp, Estatus es where sp.UidSegPerfil != '17BB8F08-9D5F-425C-9B9B-1CA230C07C7F' and sp.UidSegPerfil != '18523B2B-C671-44AE-A3F6-F0255C4D11A8' and sp.UidTipoPerfil = tp.UidTipoPerfil and sp.UidEstatus = es.UidEstatus and tp.UidAppWeb = '514433C7-4439-42F5-ABE4-6BF1C330F0CA'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                perfilesGridViewModel = new PerfilesGridViewModel()
                {
                    UidSegPerfil = new Guid(item["UidSegPerfil"].ToString()),
                    VchNombre = item["VchNombre"].ToString(),
                    UidTipoPerfil = new Guid(item["UidTipoPerfil"].ToString()),
                    VchPerfil = item["VchPerfil"].ToString(),
                    UidModuloInicial = new Guid(item["UidModuloInicial"].ToString()),
                    VchEstatus = item["VchEstatus"].ToString(),
                    VchIcono = item["VchIcono"].ToString()
                };

                lsPerfilesGridViewModel.Add(perfilesGridViewModel);
            }

            return lsPerfilesGridViewModel;
        }

        public List<PerfilesDropDownListModel> CargarPerfilesDropDownListModel(Guid UidSegPerfil)
        {
            List<PerfilesDropDownListModel> lsPerfilesDropDownListModel = new List<PerfilesDropDownListModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select sp.UidSegPerfil, sp.VchNombre, tp.VchDescripcion as VchPerfil, es.VchDescripcion as VchEstatus, es.VchIcono  from SegPerfiles sp, TiposPerfiles tp, Estatus es where sp.UidSegPerfil != '17BB8F08-9D5F-425C-9B9B-1CA230C07C7F' and sp.UidTipoPerfil = tp.UidTipoPerfil and sp.UidEstatus = es.UidEstatus and tp.UidTipoPerfil = '" + UidSegPerfil + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                perfilesDropDownListModel = new PerfilesDropDownListModel()
                {
                    UidSegPerfil = new Guid(item["UidSegPerfil"].ToString()),
                    VchNombre = item["VchNombre"].ToString(),
                    VchPerfil = item["VchPerfil"].ToString(),
                    VchEstatus = item["VchEstatus"].ToString(),
                    VchIcono = item["VchIcono"].ToString()
                };

                lsPerfilesDropDownListModel.Add(perfilesDropDownListModel);
            }

            return lsPerfilesDropDownListModel;
        }

        public void ObtenerPerfil(Guid UidPerfil)
        {
            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from SegPerfiles where UidSegPerfil ='" + UidPerfil.ToString() + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                perfiles = new Perfiles()
                {
                    UidSegPerfil = new Guid(item["UidSegPerfil"].ToString()),
                    UidAppWeb = item.IsNull("UidAppWeb") ? Guid.Empty : new Guid(item["UidAppWeb"].ToString()),
                    UidEstatus = new Guid(item["UIDEstatus"].ToString()),
                    VchNombre = item["VchNombre"].ToString(),
                    UidTipoPerfil = item.IsNull("UidTipoPerfil") ? Guid.Empty : new Guid(item["UidTipoPerfil"].ToString()),
                    UidModuloInicial = item.IsNull("UidModuloInicial") ? Guid.Empty : new Guid(item["UidModuloInicial"].ToString()),
                    UidFranquiciatario = item.IsNull("UidFranquiciatario") ? Guid.Empty : new Guid(item["UidFranquiciatario"].ToString())
                };
            }
        }

        public bool RegistrarPerfiles(Perfiles perfiles)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PerfilesRegistrar";

                comando.Parameters.Add("@UidSegPerfil", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidSegPerfil"].Value = perfiles.UidSegPerfil;

                comando.Parameters.Add("@VchNombre", SqlDbType.VarChar, 50);
                comando.Parameters["@VchNombre"].Value = perfiles.VchNombre;

                comando.Parameters.Add("@UidAppWeb", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidAppWeb"].Value = perfiles.UidAppWeb;

                comando.Parameters.Add("@UidModuloInicial", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidModuloInicial"].Value = perfiles.UidModuloInicial;

                comando.Parameters.Add("@UidTipoPerfil", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoPerfil"].Value = perfiles.UidTipoPerfil;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }

        public bool ActualizarPerfiles(Perfiles perfiles)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PerfilesActualizar";

                comando.Parameters.Add("@UidSegPerfil", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidSegPerfil"].Value = perfiles.UidSegPerfil;

                comando.Parameters.Add("@VchNombre", SqlDbType.VarChar, 50);
                comando.Parameters["@VchNombre"].Value = perfiles.VchNombre;

                comando.Parameters.Add("@UidAppWeb", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidAppWeb"].Value = perfiles.UidAppWeb;

                comando.Parameters.Add("@UidEstatus", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEstatus"].Value = perfiles.UidEstatus;

                comando.Parameters.Add("@UidModuloInicial", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidModuloInicial"].Value = perfiles.UidModuloInicial;

                comando.Parameters.Add("@UidTipoPerfil", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoPerfil"].Value = perfiles.UidTipoPerfil;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }

        #region MetodosFranquicias
        public List<PerfilesGridViewModel> CargarPerfilesFranquiciaGridViewModel(Guid UidFranquiciatario)
        {
            List<PerfilesGridViewModel> lsPerfilesGridViewModel = new List<PerfilesGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select sp.UidSegPerfil, sp.VchNombre, tp.UidTipoPerfil, sp.UidModuloInicial, tp.VchDescripcion as VchPerfil, es.VchDescripcion as VchEstatus, es.VchIcono from SegPerfiles sp, TiposPerfiles tp, Estatus es where sp.UidSegPerfil != 'D2C80D47-C14C-4677-A63D-C46BCB50FE17' and sp.UidTipoPerfil = tp.UidTipoPerfil and sp.UidEstatus = es.UidEstatus and tp.UidAppWeb = '6D70F88D-3CE0-4C8B-87A1-92666039F5B2' and sp.UidFranquiciatario = '" + UidFranquiciatario +"'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                perfilesGridViewModel = new PerfilesGridViewModel()
                {
                    UidSegPerfil = new Guid(item["UidSegPerfil"].ToString()),
                    VchNombre = item["VchNombre"].ToString(),
                    UidTipoPerfil = new Guid(item["UidTipoPerfil"].ToString()),
                    VchPerfil = item["VchPerfil"].ToString(),
                    UidModuloInicial = new Guid(item["UidModuloInicial"].ToString()),
                    VchEstatus = item["VchEstatus"].ToString(),
                    VchIcono = item["VchIcono"].ToString()
                };

                lsPerfilesGridViewModel.Add(perfilesGridViewModel);
            }

            return lsPerfilesGridViewModel;
        }

        public List<PerfilesDropDownListModel> CargarPerfilesFranquiciaDropDownListModel(Guid UidFranquiciatario, Guid UidTipoPerfil)
        {
            List<PerfilesDropDownListModel> lsPerfilesDropDownListModel = new List<PerfilesDropDownListModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            //query.CommandText = "select sp.UidSegPerfil, sp.VchNombre, tpf.VchDescripcion as VchPerfil, es.VchDescripcion as VchEstatus, es.VchIcono, sp.UidFranquiciatario from SegPerfiles sp, TiposPerfilesFranquicias tpf, Estatus es where sp.UidTipoPerfilFranquicia = tpf.UidTipoPerfilFranquicia and sp.UidEstatus = es.UidEstatus and sp.UidFranquiciatario = '" + UidFranquiciatario + "' and tpf.UidTipoPerfilFranquicia = '" + UidTipoPerfilFranquicia + "'";
            query.CommandText = "select sp.UidSegPerfil, sp.VchNombre, tp.VchDescripcion as VchPerfil, es.VchDescripcion as VchEstatus, es.VchIcono, sp.UidFranquiciatario from SegPerfiles sp, TiposPerfiles tp, Estatus es where sp.UidTipoPerfil = tp.UidTipoPerfil and sp.UidEstatus = es.UidEstatus and sp.UidFranquiciatario = '" + UidFranquiciatario + "' and tp.UidTipoPerfil = '" + UidTipoPerfil + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                perfilesDropDownListModel = new PerfilesDropDownListModel()
                {
                    UidSegPerfil = new Guid(item["UidSegPerfil"].ToString()),
                    VchNombre = item["VchNombre"].ToString(),
                    VchPerfil = item["VchPerfil"].ToString(),
                    VchEstatus = item["VchEstatus"].ToString(),
                    VchIcono = item["VchIcono"].ToString()
                };

                lsPerfilesDropDownListModel.Add(perfilesDropDownListModel);
            }

            return lsPerfilesDropDownListModel;
        }

        public void ObtenerPerfilFranquicia(Guid UidPerfil)
        {
            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from SegPerfiles where UidSegPerfil ='" + UidPerfil.ToString() + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                perfiles = new Perfiles()
                {
                    UidSegPerfil = new Guid(item["UidSegPerfil"].ToString()),
                    UidAppWeb = item.IsNull("UidAppWeb") ? Guid.Empty : new Guid(item["UidAppWeb"].ToString()),
                    UidEstatus = new Guid(item["UIDEstatus"].ToString()),
                    VchNombre = item["VchNombre"].ToString(),
                    UidTipoPerfil = item.IsNull("UidTipoPerfil") ? Guid.Empty : new Guid(item["UidTipoPerfil"].ToString()),
                    UidModuloInicial = item.IsNull("UidModuloInicial") ? Guid.Empty : new Guid(item["UidModuloInicial"].ToString()),
                    UidFranquiciatario = item.IsNull("UidFranquiciatario") ? Guid.Empty : new Guid(item["UidFranquiciatario"].ToString())
                };
            }
        }

        public bool RegistrarPerfilesFranquicia(Perfiles perfiles)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PerfilesFranquiciaRegistrar";

                comando.Parameters.Add("@UidSegPerfil", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidSegPerfil"].Value = perfiles.UidSegPerfil;

                comando.Parameters.Add("@VchNombre", SqlDbType.VarChar, 50);
                comando.Parameters["@VchNombre"].Value = perfiles.VchNombre;

                comando.Parameters.Add("@UidAppWeb", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidAppWeb"].Value = perfiles.UidAppWeb;

                comando.Parameters.Add("@UidFranquiciatario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidFranquiciatario"].Value = perfiles.UidFranquiciatario;

                comando.Parameters.Add("@UidModuloInicial", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidModuloInicial"].Value = perfiles.UidModuloInicial;

                comando.Parameters.Add("@UidTipoPerfil", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoPerfil"].Value = perfiles.UidTipoPerfil;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }

        public bool ActualizarPerfilesFranquicia(Perfiles perfiles)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PerfilesFranquiciaActualizar";

                comando.Parameters.Add("@UidSegPerfil", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidSegPerfil"].Value = perfiles.UidSegPerfil;

                comando.Parameters.Add("@VchNombre", SqlDbType.VarChar, 50);
                comando.Parameters["@VchNombre"].Value = perfiles.VchNombre;

                comando.Parameters.Add("@UidAppWeb", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidAppWeb"].Value = perfiles.UidAppWeb;

                comando.Parameters.Add("@UidEstatus", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEstatus"].Value = perfiles.UidEstatus;

                comando.Parameters.Add("@UidModuloInicial", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidModuloInicial"].Value = perfiles.UidModuloInicial;

                comando.Parameters.Add("@UidTipoPerfil", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoPerfil"].Value = perfiles.UidTipoPerfil;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        #endregion

        #region MetodosClientes
        public List<PerfilesGridViewModel> CargarPerfilesClienteGridViewModel(Guid UidCliente)
        {
            List<PerfilesGridViewModel> lsPerfilesGridViewModel = new List<PerfilesGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select sp.UidSegPerfil, sp.VchNombre, tp.UidTipoPerfil, sp.UidModuloInicial, tp.VchDescripcion as VchPerfil, es.VchDescripcion as VchEstatus, es.VchIcono from SegPerfiles sp, TiposPerfiles tp, Estatus es where sp.UidTipoPerfil = tp.UidTipoPerfil and sp.UidEstatus = es.UidEstatus and tp.UidAppWeb = '0D910772-AE62-467A-A7A3-79540F0445CB' and sp.UidCliente = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                perfilesGridViewModel = new PerfilesGridViewModel()
                {
                    UidSegPerfil = new Guid(item["UidSegPerfil"].ToString()),
                    VchNombre = item["VchNombre"].ToString(),
                    UidTipoPerfil = new Guid(item["UidTipoPerfil"].ToString()),
                    VchPerfil = item["VchPerfil"].ToString(),
                    UidModuloInicial = new Guid(item["UidModuloInicial"].ToString()),
                    VchEstatus = item["VchEstatus"].ToString(),
                    VchIcono = item["VchIcono"].ToString()
                };

                lsPerfilesGridViewModel.Add(perfilesGridViewModel);
            }

            return lsPerfilesGridViewModel;
        }

        public List<PerfilesDropDownListModel> CargarPerfilesClienteDropDownListModel(Guid UidCliente, Guid UidTipoPerfil)
        {
            List<PerfilesDropDownListModel> lsPerfilesDropDownListModel = new List<PerfilesDropDownListModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select sp.UidSegPerfil, sp.VchNombre, tp.VchDescripcion as VchPerfil, es.VchDescripcion as VchEstatus, es.VchIcono, sp.UidFranquiciatario from SegPerfiles sp, TiposPerfiles tp, Estatus es where sp.UidTipoPerfil = tp.UidTipoPerfil and sp.UidEstatus = es.UidEstatus and sp.UidCliente = '" + UidCliente + "' and tp.UidTipoPerfil = '" + UidTipoPerfil + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                perfilesDropDownListModel = new PerfilesDropDownListModel()
                {
                    UidSegPerfil = new Guid(item["UidSegPerfil"].ToString()),
                    VchNombre = item["VchNombre"].ToString(),
                    VchPerfil = item["VchPerfil"].ToString(),
                    VchEstatus = item["VchEstatus"].ToString(),
                    VchIcono = item["VchIcono"].ToString()
                };

                lsPerfilesDropDownListModel.Add(perfilesDropDownListModel);
            }

            return lsPerfilesDropDownListModel;
        }

        public void ObtenerPerfilCliente(Guid UidPerfil)
        {
            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from SegPerfiles where UidSegPerfil ='" + UidPerfil.ToString() + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                perfiles = new Perfiles()
                {
                    UidSegPerfil = new Guid(item["UidSegPerfil"].ToString()),
                    UidAppWeb = item.IsNull("UidAppWeb") ? Guid.Empty : new Guid(item["UidAppWeb"].ToString()),
                    UidEstatus = new Guid(item["UIDEstatus"].ToString()),
                    VchNombre = item["VchNombre"].ToString(),
                    UidTipoPerfil = item.IsNull("UidTipoPerfil") ? Guid.Empty : new Guid(item["UidTipoPerfil"].ToString()),
                    UidModuloInicial = item.IsNull("UidModuloInicial") ? Guid.Empty : new Guid(item["UidModuloInicial"].ToString()),
                    UidFranquiciatario = item.IsNull("UidFranquiciatario") ? Guid.Empty : new Guid(item["UidFranquiciatario"].ToString())
                };
            }
        }

        public bool RegistrarPerfilesCliente(Perfiles perfiles)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PerfilesClienteRegistrar";

                comando.Parameters.Add("@UidSegPerfil", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidSegPerfil"].Value = perfiles.UidSegPerfil;

                comando.Parameters.Add("@VchNombre", SqlDbType.VarChar, 50);
                comando.Parameters["@VchNombre"].Value = perfiles.VchNombre;

                comando.Parameters.Add("@UidAppWeb", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidAppWeb"].Value = perfiles.UidAppWeb;

                comando.Parameters.Add("@UidModuloInicial", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidModuloInicial"].Value = perfiles.UidModuloInicial;

                comando.Parameters.Add("@UidTipoPerfil", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoPerfil"].Value = perfiles.UidTipoPerfil;

                comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCliente"].Value = perfiles.UidCliente;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }

        public bool ActualizarPerfilesCliente(Perfiles perfiles)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PerfilesClienteActualizar";

                comando.Parameters.Add("@UidSegPerfil", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidSegPerfil"].Value = perfiles.UidSegPerfil;

                comando.Parameters.Add("@VchNombre", SqlDbType.VarChar, 50);
                comando.Parameters["@VchNombre"].Value = perfiles.VchNombre;

                comando.Parameters.Add("@UidAppWeb", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidAppWeb"].Value = perfiles.UidAppWeb;

                comando.Parameters.Add("@UidEstatus", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEstatus"].Value = perfiles.UidEstatus;

                comando.Parameters.Add("@UidModuloInicial", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidModuloInicial"].Value = perfiles.UidModuloInicial;

                comando.Parameters.Add("@UidTipoPerfil", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoPerfil"].Value = perfiles.UidTipoPerfil;

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
