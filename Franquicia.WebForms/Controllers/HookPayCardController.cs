using System.Web.Mvc;
using Twilio.AspNet.Mvc;
using Twilio.TwiML;

namespace Franquicia.WebForms.Controllers
{
    //[Route("api/{controller}/{action}/")]

    public class HookPayCardController : TwilioController
    {
        [HttpPost]
        public ActionResult ReceiveSms()
        {
            var requestBody = Request.Form["Body"];
            var response = new MessagingResponse();
            if (requestBody == "hello")
            {
                response.Message("Gracias por confirmar!");
            }
            else if (requestBody == "bye")
            {
                response.Message("Goodbye");
            }

            return TwiML(response);
        }
    }
}