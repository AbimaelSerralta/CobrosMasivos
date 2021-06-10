using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class AvatarsServices
    {
        private AvatarsRepository avatarsRepository = new AvatarsRepository();

        public List<Avatars> lsAvatars = new List<Avatars>();

        public void CargarAvatars()
        {
            lsAvatars = new List<Avatars>();

            lsAvatars = avatarsRepository.CargarAvatars();
        }
    }
}
