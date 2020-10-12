using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain
{
    public class EmailConfiguration
    {
        public string Host { get; set; }
        public string EmailFrom { get; set; }
        public string Password { get; set; }
        public bool IsBodyHtml { get; set; }
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public int Port { get; set; }

        //Build Email
        public string BodyHtml { get; set; }
        public string EmailTo { get; set; }
        public string Subject { get; set; }

    }
}
