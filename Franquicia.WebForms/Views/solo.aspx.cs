using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Franquicia.WebForms.Views
{
    public partial class solo : System.Web.UI.Page
    {
        protected string accountSid = "ACc7561cb09df3180ee1368e40055eedf5";
        protected string authToken = "0f47ce2d28c9211ac6a9ae42f630d1d6";

        protected void Page_Load(object sender, EventArgs e)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            if (!IsPostBack)
            {

            }
            else
            {
                
            }
        }

        protected void btnSendMessage_Click(object sender, EventArgs e)
        {
            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: taMensaje.Text,
                from: new Twilio.Types.PhoneNumber("+14158739087"),
                to: new Twilio.Types.PhoneNumber("+529841651607")
            );

            lblResponse.Text = message.Sid;
        }

        protected void btnSendWhatsApp_Click(object sender, EventArgs e)
        {
            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: taMensaje.Text,
                from: new Twilio.Types.PhoneNumber("whatsapp:+14155238886"),
                to: new Twilio.Types.PhoneNumber("whatsapp:+5219841651607")
            );
            
            //lblResponse.Text = message.Status.ToString();
            lblResponse.Text = message.Sid;
        }
    }
}