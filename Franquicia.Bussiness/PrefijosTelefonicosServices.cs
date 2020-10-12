using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class PrefijosTelefonicosServices
    {
        private PrefijosTelefonicosRepository prefijosTelefonicosRepository = new PrefijosTelefonicosRepository();

        public List<PrefijosTelefonicos> lsPrefijosTelefonicos = new List<PrefijosTelefonicos>();

        public void CargarPrefijosTelefonicos()
        {
            lsPrefijosTelefonicos = new List<PrefijosTelefonicos>();

            lsPrefijosTelefonicos = prefijosTelefonicosRepository.CargarPrefijosTelefonicos();
        }
    }
}
