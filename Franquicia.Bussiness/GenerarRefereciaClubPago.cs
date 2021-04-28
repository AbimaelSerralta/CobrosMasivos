using Franquicia.DataAccess.Repository;
using Franquicia.Domain;
using Franquicia.Domain.Models.ClubPago;
using Franquicia.Domain.Models.Praga;
using Franquicia.Domain.ViewModels.ClubPago;
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

        #region Metodos Integraciones
        public List<AuthClub> ApiObtenerCredenciales(Guid UidTipoPagoIntegracion, int IdIntegracion)
        {
            if (UidTipoPagoIntegracion == Guid.Parse("3F792D20-B3B6-41D3-AF88-1BCB20D99BBE")) //SANDBOX
            {
                parametrosClubPagoRepository.ObtenerParametrosPragaSandbox(IdIntegracion);
            }
            else if (UidTipoPagoIntegracion == Guid.Parse("D87454C9-12EF-4459-9CED-36E8401E4033")) //PRODUCCION
            {
                parametrosClubPagoRepository.ObtenerParametrosPragaProduccion(IdIntegracion);
            }

            VchUrlAuth = parametrosClubPagoRepository.parametrosClubPago.VchUrlAuth;
            VchUrlGenerarRef = parametrosClubPagoRepository.parametrosClubPago.VchUrlGenerarRef;
            User = parametrosClubPagoRepository.parametrosClubPago.VchUser;
            Pswd = parametrosClubPagoRepository.parametrosClubPago.VchPass;

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
        public ObtenerRefereciaPago ApiGenerarReferencia(GenerarRefereciaPagoIntegraciones generarRefereciaPagoIntegraciones)
        {
            ObtenerRefereciaPago obtenerRefereciaPago = new ObtenerRefereciaPago();

            GenerarRefereciaPago generarRefereciaPago = new GenerarRefereciaPago();

            string token = string.Empty;
            foreach (var item in ApiObtenerCredenciales(Guid.Parse(generarRefereciaPagoIntegraciones.UidTipoPagoIntegracion), int.Parse(generarRefereciaPagoIntegraciones.IntegrationID)))
            {
                token = item.Token;
            }

            //Asignacion de parametros necesarios para generar la referencia
            generarRefereciaPago.Description = generarRefereciaPagoIntegraciones.Description;
            generarRefereciaPago.Amount = generarRefereciaPagoIntegraciones.Amount;
            generarRefereciaPago.Account = generarRefereciaPagoIntegraciones.Account;
            generarRefereciaPago.CustomerEmail = generarRefereciaPagoIntegraciones.CustomerEmail;
            generarRefereciaPago.CustomerName = generarRefereciaPagoIntegraciones.CustomerName;
            generarRefereciaPago.ExpirationDate = generarRefereciaPagoIntegraciones.ExpirationDate;

            try
            {
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
                    obtenerRefereciaPago = JsonConvert.DeserializeObject<ObtenerRefereciaPago>(content.ToString());
                    obtenerRefereciaPago.ReferenceEmisor = generarRefereciaPagoIntegraciones.ReferenceEmisor;
                }

                return obtenerRefereciaPago;
            }
            catch (Exception ex)
            {
                string mnsj = ex.Message;

                return obtenerRefereciaPago;
            }
        }

        public void EnviarRespuesta(ObtenerRefereciaPago obtenerRefereciaPago, string UrlEntrega)
        {
            try
            {
                var client = new RestClient(UrlEntrega);
                var request = new RestRequest(Method.POST);
                string json = JsonConvert.SerializeObject(obtenerRefereciaPago);
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", json, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var content = response.Content;

            }
            catch (Exception ex)
            {
                string mnsj = ex.Message;
            }
        }
        #endregion
    }
}
