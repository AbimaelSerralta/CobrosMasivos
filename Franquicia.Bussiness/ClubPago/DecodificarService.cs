﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness.ClubPago
{
    public class DecodificarService
    {
        public string DecodeBase64ToString(string valor)
        {
            string myStr = "";

            try
            {
                byte[] myBase64ret = Convert.FromBase64String(valor);
                myStr = System.Text.Encoding.UTF8.GetString(myBase64ret);
            }
            catch (Exception)
            {


            }

            return myStr;
        }

        public string Base64Encode(string valor)
        {
            byte[] myByte = System.Text.Encoding.UTF8.GetBytes(valor);
            string myBase64 = Convert.ToBase64String(myByte);
            return myBase64;
        }
    }
}
