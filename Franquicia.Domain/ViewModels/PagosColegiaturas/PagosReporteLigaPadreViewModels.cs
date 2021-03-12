using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class PagosReporteLigaPadreViewModels : PagosColegiaturas
    {
        public string VchIdentificador { get; set; }
        
        public Guid UidAlumno { get; set; }
        public string VchMatricula { get; set; }
        public string VchNombres { get; set; }
        public string VchApePaterno { get; set; }
        public string VchApeMaterno { get; set; }
        public string VchAlumno { get { return VchNombres + " " + VchApePaterno + " " + VchApeMaterno; } }

        public Guid UidFechaColegiatura { get; set; }

        public int IntNum { get; set; }
        public string VchNum { get; set; }
        public decimal DcmImporteSaldado { get; set; }
        public decimal DcmImporteCole { get; set; }
        public decimal DcmImportePagado { get; set; }
        public decimal DcmImporteNuevo { get; set; }

        public string UsNombre { get; set; }
        public string UsPaterno { get; set; }
        public string UsMaterno { get; set; }
        public string VchPadre { get { return UsNombre + " " + UsPaterno + " " + UsMaterno; } }

        public Guid UidFormaPago { get; set; }
        public string VchFormaPago { get; set; }

        public bool blAprobarPago { get; set; }
        public bool blRechazarPago { get; set; }

        public bool blConfirmarPago { get; set; }
        public bool blEditarPago { get; set; }

        public string VchEstatus { get; set; }
        public string VchColor { get; set; }
        
        public string VchBanco { get; set; }
        public string VchCuenta { get; set; }
        public string VchFolio { get; set; }

    }
}
