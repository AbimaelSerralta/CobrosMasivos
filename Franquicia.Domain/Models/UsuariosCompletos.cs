using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class UsuariosCompletos
    {
        #region Propiedades
        #region SegUsuarios
        public Guid UidSegUsuario { get; set; }
        public string VchUsuario { get; set; }
        public string VchContrasenia { get; set; }
        public Guid UidModuloInicial { get; set; }
        public Guid UidUltimoModulo { get; set; }
        public DateTime DtUltimaActividad { get; set; }
        public Guid UidSegPerfil { get; set; }
        #endregion

        #region SegModulos
        public Guid UidSegModulo { get; set; }
        public string VchNombre { get; set; }
        public Guid UidAppWeb { get; set; }
        public string VchUrl { get; set; }
        #endregion

        #region Usuarios
        public Guid UidUsuario { get; set; }
        public string StrNombre { get; set; }
        public string StrApePaterno { get; set; }
        public string StrApeMaterno { get; set; }
        public string StrCorreo { get; set; }
        public Guid UidEstatus { get; set; }
        public string NombreCompleto { get { return StrNombre + " " + StrApePaterno + " " + StrApeMaterno; } }
        public int IdUsuario { get; set; }

        #endregion

        #region SegPerfiles
        public string VchNombrePerfil { get; set; }
        #endregion

        #region Estatus
        public string VchDescripcion { get; set; }
        public string VchIcono { get; set; }
        #endregion

        #region Fanquicias
        public Guid UidEstatusEmpresa { get; set; }
        public string VchNombreComercial { get; set; }
        #endregion

        #region Otros Modelos

        #endregion
        #endregion

    }
}
