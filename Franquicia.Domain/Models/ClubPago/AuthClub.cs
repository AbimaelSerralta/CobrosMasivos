using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models.ClubPago
{
    public class AuthClub
    {
        public string Message { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
