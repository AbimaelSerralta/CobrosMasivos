using Franquicia.DataAccess.Repository;
using Franquicia.Domain;
using Franquicia.Domain.Models.ClubPago;
using Franquicia.Domain.Models.Praga;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class GenerarRefereciaClubPago
    {
        private ParametrosClubPagoRepository _parametrosClubPagoRepository = new ParametrosClubPagoRepository();
        public ParametrosClubPagoRepository parametrosClubPagoRepository
        {
            get { return _parametrosClubPagoRepository; }
            set { _parametrosClubPagoRepository = value; }
        }

        string VchUrlAuth = "";
        string VchUrlGenerarRef = "";
        string User = "";
        string Pswd = "";

        public GenerarRefereciaClubPago()
        {
            parametrosClubPagoRepository.ObtenerParametrosClubPago();

            VchUrlAuth = parametrosClubPagoRepository.parametrosClubPago.VchUrlAuth;
            VchUrlGenerarRef = parametrosClubPagoRepository.parametrosClubPago.VchUrlGenerarRef;
            User = parametrosClubPagoRepository.parametrosClubPago.VchUser;
            Pswd = parametrosClubPagoRepository.parametrosClubPago.VchPass;
        }

        public List<AuthClub> AuthClub()
        {
            List<AuthClub> lsAuthClub = new List<AuthClub>();

            var client = new RestClient(VchUrlAuth);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\n\t\"user\":\"" + User + "\",\n\t\"pswd\":\"" + Pswd + "\"\n}", ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            var content = response.Content;

            if (content != string.Empty)
            {
                AuthClub authClub = JsonConvert.DeserializeObject<AuthClub>(content.ToString());

                lsAuthClub.Add(authClub);
            }

            return lsAuthClub;
        }

        public List<ObtenerRefereciaPago> GenerarReferencia(string Description, string Amount, string Account, string CustomerEmail, string CustomerName, string ExpirationDate)
        {
            string token = string.Empty;
            foreach (var item in AuthClub())
            {
                token = item.Token;
            }

            GenerarRefereciaPago generarRefereciaPago = new GenerarRefereciaPago();
            List<ObtenerRefereciaPago> lsObtenerRefereciaPago = new List<ObtenerRefereciaPago>();

            try
            {
                generarRefereciaPago.Description = Description;
                generarRefereciaPago.Amount = Amount;
                generarRefereciaPago.Account = Account;
                generarRefereciaPago.CustomerEmail = CustomerEmail;
                generarRefereciaPago.CustomerName = CustomerName;
                generarRefereciaPago.ExpirationDate = ExpirationDate;

                var client = new RestClient(VchUrlGenerarRef);
                var request = new RestRequest(Method.POST);
                string json = JsonConvert.SerializeObject(generarRefereciaPago);
                request.AddHeader("content-type", "application/json");
                request.AddHeader("authorization", "Bearer " + token);
                request.AddParameter("application/json", json, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var content = response.Content;

                if (content != string.Empty)
                {
                    ObtenerRefereciaPago obtenerRefereciaPago = JsonConvert.DeserializeObject<ObtenerRefereciaPago>(content.ToString());

                    lsObtenerRefereciaPago.Add(obtenerRefereciaPago);
                }
                
                return lsObtenerRefereciaPago;
            }
            catch (Exception ex)
            {
                string mnsj = ex.Message;

                return lsObtenerRefereciaPago;
            }
        }
    }
}
