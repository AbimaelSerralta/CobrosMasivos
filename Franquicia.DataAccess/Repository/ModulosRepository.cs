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
    public class ModulosRepository : SqlDataRepository
    {
        Modulos _modulos = new Modulos();
        public Modulos modulos
        {
            get { return _modulos; }
            set { _modulos = value; }
        }

        PermisosMenuModel _permisosMenuModel = new PermisosMenuModel();
        public PermisosMenuModel permisosMenuModel
        {
            get { return _permisosMenuModel; }
            set { _permisosMenuModel = value; }
        }

        public List<Modulos> CargarModulosNivel()
        {
            List<Modulos> lsModulos = new List<Modulos>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "Select * from SegModulos";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsModulos.Add(new Modulos()
                {
                    VchNombre = item["VchNombre"].ToString(),
                    VchUrl = item["VchUrl"].ToString(),
                    VchIcono = item["VchIcono"].ToString()
                });
            }

            return lsModulos;
        }
        public List<Modulos> CargarModulosNivelPrincipal(Guid UidSegPerfil)
        {
            List<Modulos> lsModulos = new List<Modulos>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            //query.CommandText = "select * from SegModulos where UidAppWeb = '514433c7-4439-42f5-abe4-6bf1c330f0ca'";
            query.CommandText = "select distinct sm.* from SegModulos sm, AccesosPerfiles ap, SegPerfiles sp where sm.UidSegModulo = ap.UidSegModulo and sp.UidSegPerfil = ap.UidSegPerfil and sm.UidAppWeb = '514433c7-4439-42f5-abe4-6bf1c330f0ca' and sp.UidSegPerfil = '"+ UidSegPerfil + "' order by sm.IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsModulos.Add(new Modulos()
                {
                    UidSegModulo = new Guid(item["UidSegModulo"].ToString()),
                    VchNombre = item["VchNombre"].ToString(),
                    VchUrl = item["VchUrl"].ToString(),
                    VchIcono = item["VchIcono"].ToString()
                });
            }

            return lsModulos;
        }
        public List<Modulos> CargarModulosNivelFranquicias(Guid UidSegPerfil)
        {
            List<Modulos> lsModulos = new List<Modulos>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            //query.CommandText = "select * from SegModulos where UidAppWeb = '6d70f88d-3ce0-4c8b-87a1-92666039f5b2'";
            query.CommandText = "select distinct sm.* from SegModulos sm, AccesosPerfiles ap, SegPerfiles sp where sm.UidSegModulo = ap.UidSegModulo and sp.UidSegPerfil = ap.UidSegPerfil and sm.UidAppWeb = '6d70f88d-3ce0-4c8b-87a1-92666039f5b2' and sp.UidSegPerfil = '" + UidSegPerfil + "' order by sm.IntGerarquia asc";
            

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsModulos.Add(new Modulos()
                {
                    UidSegModulo = new Guid(item["UidSegModulo"].ToString()),
                    VchNombre = item["VchNombre"].ToString(),
                    VchUrl = item["VchUrl"].ToString(),
                    VchIcono = item["VchIcono"].ToString()
                });
            }

            return lsModulos;
        }
        public List<Modulos> CargarModulosNivelClientes(Guid UidSegPerfil)
        {
            List<Modulos> lsModulos = new List<Modulos>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            //query.CommandText = "select * from SegModulos where UidAppWeb = '0d910772-ae62-467a-a7a3-79540f0445cb'";
            query.CommandText = "select distinct sm.* from SegModulos sm, AccesosPerfiles ap, SegPerfiles sp where sm.UidSegModulo = ap.UidSegModulo and sp.UidSegPerfil = ap.UidSegPerfil and sm.UidAppWeb = '0d910772-ae62-467a-a7a3-79540f0445cb' and sp.UidSegPerfil = '" + UidSegPerfil + "' order by sm.IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsModulos.Add(new Modulos()
                {
                    UidSegModulo = new Guid(item["UidSegModulo"].ToString()),
                    VchNombre = item["VchNombre"].ToString(),
                    VchUrl = item["VchUrl"].ToString(),
                    VchIcono = item["VchIcono"].ToString()
                });
            }

            return lsModulos;
        }
        public List<Modulos> CargarModulosNivelUsuarios(Guid UidSegPerfil)
        {
            List<Modulos> lsModulos = new List<Modulos>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            //query.CommandText = "select * from SegModulos where UidAppWeb = '9C8AD059-A37B-42EE-BF37-FEB7ACA84088'";
            query.CommandText = "select distinct sm.* from SegModulos sm, AccesosPerfiles ap, SegPerfiles sp where sm.UidSegModulo = ap.UidSegModulo and sp.UidSegPerfil = ap.UidSegPerfil and sm.UidAppWeb = '9C8AD059-A37B-42EE-BF37-FEB7ACA84088' and sp.UidSegPerfil = '" + UidSegPerfil + "' order by sm.IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsModulos.Add(new Modulos()
                {
                    UidSegModulo = new Guid(item["UidSegModulo"].ToString()),
                    VchNombre = item["VchNombre"].ToString(),
                    VchUrl = item["VchUrl"].ToString(),
                    VchIcono = item["VchIcono"].ToString()
                });
            }

            return lsModulos;
        }

        public List<PermisosMenuModel> CargarMenu(Guid UidAppWeb)
        {
            List<PermisosMenuModel> lsModulos = new List<PermisosMenuModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from SegModulos where UidAppWeb = '"+ UidAppWeb +"'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                bool q = false;
                bool w = false;
                bool e = false;

                query.CommandText = "select VchDescripcion from Permisos where UidSegModulo = '" + item["UidSegModulo"].ToString() + "'";
                DataTable dtPermiDesc = this.Busquedas(query);

                query.CommandText = "Select sm.VchNombre, sm.VchUrl, sm.VchIcono, p.VchDescripcion as Permiso from SegModulos sm, SegPerfiles sp, Permisos p, AccesosPerfiles ap where ap.UidPermiso = p.UidPermiso and ap.UidSegPerfil = sp.UidSegPerfil and ap.UidSegModulo = sm.UidSegModulo and sm.UidSegModulo = '" + item["UidSegModulo"].ToString() + "'";
                DataTable dtPermso = this.Busquedas(query);

                foreach (DataRow i in dtPermiDesc.Rows)
                {
                    if (i["VchDescripcion"].ToString().Contains("Lectura"))
                    {
                        foreach (DataRow it in dtPermso.Rows)
                        {
                            if (it["Permiso"].ToString().Contains("Lectura"))
                            {

                                if (i["VchDescripcion"].ToString() == it["Permiso"].ToString())
                                {
                                    q = true;
                                }
                            }

                        }
                    }
                }                

                lsModulos.Add(new PermisosMenuModel()
                {
                    UidSegModulo = new Guid(item["UidSegModulo"].ToString()),
                    VchNombre = item["VchNombre"].ToString(),
                    VchUrl = item["VchUrl"].ToString(),
                    VchIcono = item["VchIcono"].ToString(),
                    Lectura = q,
                    Agregar = w,
                    Actualizar = e
                });
            }

            return lsModulos;
        }

        public List<PermisosMenuModel> CargarAccesosPermitidos(Guid UidSegPerfil)
        {
            List<PermisosMenuModel> lsModulos = new List<PermisosMenuModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from SegModulos";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                bool Lectura = false;
                bool Agregar = false;
                bool Actualizar = false;

                query.CommandText = "select VchDescripcion from Permisos where UidSegModulo = '" + item["UidSegModulo"].ToString() + "'";
                DataTable dtPermiDesc = this.Busquedas(query);

                query.CommandText = "Select sm.VchNombre, sm.VchUrl, sm.VchIcono, p.VchDescripcion as Permiso from SegModulos sm, SegPerfiles sp, Permisos p, AccesosPerfiles ap where ap.UidPermiso = p.UidPermiso and ap.UidSegPerfil = sp.UidSegPerfil and ap.UidSegModulo = sm.UidSegModulo and sp.UidSegPerfil = '" + UidSegPerfil + "'";
                //query.CommandText = "Select sm.VchNombre, sm.VchUrl, sm.VchIcono, p.VchDescripcion as Permiso from SegModulos sm, SegPerfiles sp, Permisos p, AccesosPerfiles ap where ap.UidPermiso = p.UidPermiso and ap.UidSegPerfil = sp.UidSegPerfil and ap.UidSegModulo = sm.UidSegModulo and sm.UidSegModulo = '" + item["UidSegModulo"].ToString() + "' " ;
                DataTable dtPermso = this.Busquedas(query);

                foreach (DataRow i in dtPermiDesc.Rows)
                {
                    if (i["VchDescripcion"].ToString().Contains("Lectura"))
                    {
                        foreach (DataRow it in dtPermso.Rows)
                        {
                            if (it["Permiso"].ToString().Contains("Lectura"))
                            {
                                if (i["VchDescripcion"].ToString() == it["Permiso"].ToString())
                                {
                                    Lectura = true;
                                }
                            }
                            if (it["Permiso"].ToString().Contains("Agregar"))
                            {
                                if (i["VchDescripcion"].ToString() == it["Permiso"].ToString())
                                {
                                    Agregar = true;
                                }
                            }

                            if (it["Permiso"].ToString().Contains("Actualizar"))
                            {
                                if (i["VchDescripcion"].ToString() == it["Permiso"].ToString())
                                {
                                    Actualizar = true;
                                }
                            }

                        }
                    }
                    if (i["VchDescripcion"].ToString().Contains("Agregar"))
                    {
                        foreach (DataRow it in dtPermso.Rows)
                        {
                            if (it["Permiso"].ToString().Contains("Agregar"))
                            {
                                if (i["VchDescripcion"].ToString() == it["Permiso"].ToString())
                                {
                                    Agregar = true;
                                }
                            }
                        }
                    }
                    if (i["VchDescripcion"].ToString().Contains("Actualizar"))
                    {
                        foreach (DataRow it in dtPermso.Rows)
                        {
                            if (it["Permiso"].ToString().Contains("Actualizar"))
                            {
                                if (i["VchDescripcion"].ToString() == it["Permiso"].ToString())
                                {
                                    Actualizar = true;
                                }
                            }

                        }
                    }
                }

                lsModulos.Add(new PermisosMenuModel()
                {
                    UidSegModulo = new Guid(item["UidSegModulo"].ToString()),
                    VchNombre = item["VchNombre"].ToString(),
                    VchUrl = item["VchUrl"].ToString(),
                    VchIcono = item["VchIcono"].ToString(),
                    Lectura = Lectura,
                    Agregar = Agregar,
                    Actualizar = Actualizar,
                    UidAppWeb = new Guid(item["UidAppWeb"].ToString()),
                    IntGerarquia = int.Parse(item["IntGerarquia"].ToString())
                });
            }

            return lsModulos;
        }

        public List<PermisosMenuModel> CargarTodosAccesosPermitidos(Guid UidSegPerfil)
        {
            List<PermisosMenuModel> lsModulos = new List<PermisosMenuModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from SegModulos";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                bool q = false;
                bool w = false;
                bool e = false;

                query.CommandText = "select VchDescripcion from Permisos where UidSegModulo = '" + item["UidSegModulo"].ToString() + "'";
                DataTable dtPermiDesc = this.Busquedas(query);

                query.CommandText = "Select sm.VchNombre, sm.VchUrl, sm.VchIcono, p.VchDescripcion as Permiso from SegModulos sm, SegPerfiles sp, Permisos p, AccesosPerfiles ap where ap.UidPermiso = p.UidPermiso and ap.UidSegPerfil = sp.UidSegPerfil and ap.UidSegModulo = sm.UidSegModulo and sp.UidSegPerfil = '" + UidSegPerfil + "'";
                //query.CommandText = "Select sm.VchNombre, sm.VchUrl, sm.VchIcono, p.VchDescripcion as Permiso from SegModulos sm, SegPerfiles sp, Permisos p, AccesosPerfiles ap where ap.UidPermiso = p.UidPermiso and ap.UidSegPerfil = sp.UidSegPerfil and ap.UidSegModulo = sm.UidSegModulo and sm.UidSegModulo = '" + item["UidSegModulo"].ToString() + "' " ;
                DataTable dtPermso = this.Busquedas(query);

                foreach (DataRow i in dtPermiDesc.Rows)
                {
                    if (i["VchDescripcion"].ToString().Contains("Lectura"))
                    {
                        foreach (DataRow it in dtPermso.Rows)
                        {
                            if (it["Permiso"].ToString().Contains("Lectura"))
                            {

                                if (i["VchDescripcion"].ToString() == it["Permiso"].ToString())
                                {
                                    q = true;
                                }
                            }

                        }
                    }
                }

                lsModulos.Add(new PermisosMenuModel()
                {
                    UidSegModulo = new Guid(item["UidSegModulo"].ToString()),
                    VchNombre = item["VchNombre"].ToString(),
                    VchUrl = item["VchUrl"].ToString(),
                    VchIcono = item["VchIcono"].ToString(),
                    Lectura = q,
                    Agregar = w,
                    Actualizar = e,
                    UidAppWeb = new Guid(item["UidAppWeb"].ToString())
                });
            }

            return lsModulos;
        }

        public void ObtenerModulo(Guid UidModulo)
        {
            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from SegModulos where UidSegModulo ='" + UidModulo.ToString() + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                modulos = new Modulos()
                {
                    UidSegModulo = new Guid(item["UidSegModulo"].ToString()),
                    UidAppWeb = new Guid(item["UidAppWeb"].ToString()),
                    VchNombre = item["VchNombre"].ToString(),
                    VchUrl = item["VchUrl"].ToString()
                };
            }
        }

    }
}
