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

        //string accountSid = "ACcf4d1380ccb0be6d47e78a73036a29ab";
        //string authToken = "30401e7bf2b7b3a2ab24c0a22203acc1";
        //string NumberFrom = "+14582243212";

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