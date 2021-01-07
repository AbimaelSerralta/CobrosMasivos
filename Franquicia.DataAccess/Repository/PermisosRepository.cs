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
    public class PermisosRepository : SqlDataRepository
    {
        private Permisos _permisos = new Permisos();
        public Permisos permisos
        {
            get { return _permisos; }
            set { _permisos = value; }
        }

        private PermisosCheckBoxListModel _permisosCheckBoxListModel = new PermisosCheckBoxListModel();
        public PermisosCheckBoxListModel permisosCheckBoxListModel
        {
            get { return _permisosCheckBoxListModel; }
            set { _permisosCheckBoxListModel = value; }
        }

        public List<PermisosCheckBoxListModel> CargarModulosPermisos(Guid UidSegPerfil)
        {
            List<PermisosCheckBoxListModel> lsmodulosCheckBoxListModel = new List<PermisosCheckBoxListModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            //No tiene validacion de perfil query.CommandText = "select * from Permisos order by VchDescripcion asc";
            query.CommandText = "select pe.* from Permisos pe, SegModulos sm, AccesosPerfiles ap, SegPerfiles sp where ap.UidPermiso = pe.UidPermiso and sp.UidSegPerfil = ap.UidSegPerfil and ap.UidSegModulo = sm.UidSegModulo and pe.UidSegModulo = sm.UidSegModulo and sp.UidSegPerfil = '" + UidSegPerfil + "' order by pe.VchDescripcion asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                permisosCheckBoxListModel = new PermisosCheckBoxListModel()
                {
                    UidPermiso = new Guid(item["UidPermiso"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    UidSegModulo = new Guid(item["UidSegModulo"].ToString())
                };

                lsmodulosCheckBoxListModel.Add(permisosCheckBoxListModel);
            }
            return lsmodulosCheckBoxListModel;
        }

        public List<PermisosCheckBoxListModel> CargarAccesosModulosPermisos(Guid UidSegPerfil)
        {
            List<PermisosCheckBoxListModel> lsmodulosCheckBoxListModel = new List<PermisosCheckBoxListModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select p.* from Permisos p, AccesosPerfiles ap where p.UidPermiso = ap.UidPermiso and ap.UidSegPerfil ='" + UidSegPerfil + "'order by VchDescripcion asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                permisosCheckBoxListModel = new PermisosCheckBoxListModel()
                {
                    UidPermiso = new Guid(item["UidPermiso"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    UidSegModulo = new Guid(item["UidSegModulo"].ToString())
                };

                lsmodulosCheckBoxListModel.Add(permisosCheckBoxListModel);
            }
            return lsmodulosCheckBoxListModel;
        }

        public List<PermisosCheckBoxListModel> ObtenerModulosPermisos(Guid UidModulo)
        {
            List<PermisosCheckBoxListModel> lsmodulosCheckBoxListModel = new List<PermisosCheckBoxListModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select pe.UidPermiso, pe.VchDescripcion from Permisos pe, SegModulos sm where pe.UidSegModulo = sm.UidSegModulo and sm.UidSegModulo ='" + UidModulo.ToString() + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                permisosCheckBoxListModel = new PermisosCheckBoxListModel()
                {
                    UidPermiso = new Guid(item["UidPermiso"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString()
                };

                lsmodulosCheckBoxListModel.Add(permisosCheckBoxListModel);
            }
            return lsmodulosCheckBoxListModel;
        }

        public bool RegistrarModulosPermisos(Guid UidSegPerfil, Guid UidSegModulo, Guid UidPermiso, bool Permiso)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PermisosRegistrar";

                comando.Parameters.Add("@UidSegPerfil", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidSegPerfil"].Value = UidSegPerfil;

                comando.Parameters.Add("@UidSegModulo", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidSegModulo"].Value = UidSegModulo;

                comando.Parameters.Add("@UidPermiso", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPermiso"].Value = UidPermiso;

                comando.Parameters.Add("@Permiso", SqlDbType.Bit);
                comando.Parameters["@Permiso"].Value = Permiso;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }

        public bool EliminarModulosPermisos(Guid UidPermiso, Guid UidSegPerfil)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PermisosEliminar";

                comando.Parameters.Add("@UidPermiso", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPermiso"].Value = UidPermiso;

                comando.Parameters.Add("@UidSegPerfil", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidSegPerfil"].Value = UidSegPerfil;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }

        public bool ValidarModulosPermisosExiste(Guid UidPermiso, Guid UidSegPerfil)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select UidPermiso from AccesosPerfiles where UidPermiso = '" + UidPermiso + "' and UidSegPerfil = '" + UidSegPerfil + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }

        #region MetodosFranquicias
        public List<PermisosCheckBoxListModel> CargarModulosPermisosFranquicias(Guid UidSegPerfil)
        {
            List<PermisosCheckBoxListModel> lsmodulosCheckBoxListModel = new List<PermisosCheckBoxListModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            //query.CommandText = "select * from Permisos p, SegModulos sm where sm.UidAppWeb != '514433C7-4439-42F5-ABE4-6BF1C330F0CA' and p.UidSegModulo = sm.UidSegModulo order by VchDescripcion asc";
            query.CommandText = "select pe.*, sm.* from Permisos pe, SegModulos sm, AccesosPerfiles ap, SegPerfiles sp where ap.UidPermiso = pe.UidPermiso and sp.UidSegPerfil = ap.UidSegPerfil and ap.UidSegModulo = sm.UidSegModulo and pe.UidSegModulo = sm.UidSegModulo and sp.UidSegPerfil = '" + UidSegPerfil + "' order by pe.VchDescripcion asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                permisosCheckBoxListModel = new PermisosCheckBoxListModel()
                {
                    UidPermiso = new Guid(item["UidPermiso"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    UidSegModulo = new Guid(item["UidSegModulo"].ToString())
                };

                lsmodulosCheckBoxListModel.Add(permisosCheckBoxListModel);
            }
            return lsmodulosCheckBoxListModel;
        }
        #endregion
    }
}
