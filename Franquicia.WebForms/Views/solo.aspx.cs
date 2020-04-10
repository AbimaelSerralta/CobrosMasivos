using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Franquicia.WebForms.Views
{
    public partial class solo : System.Web.UI.Page
    {
        protected string accountId = "AC6900a8ff8d6ab5876e0edbcd65954fed";
        protected string authToken = "1cb16023c0529703d757f1f9ca9ed1dc";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
            else
            {
                
            }
        }

        protected void btnSendMessage_Click(object sender, EventArgs e)
        {
            //TwilioClient.Init(this.accountId, this.authToken);

            //var message = MessageResource.Create(
            //    body: this.txtMessage.Text,
            //    from: new Twilio.Types.PhoneNumber("+12052939637"),
            //    to: new Twilio.Types.PhoneNumber("+529842129836"));

            //lblResponse.Text = message.Sid;
        }

        protected void btnSendWhatsApp_Click(object sender, EventArgs e)
        {
            //TwilioClient.Init(this.accountId, this.authToken);

            //var messageOptions = new CreateMessageOptions(new Twilio.Types.PhoneNumber("whatsapp:+5219842129836"));

            //messageOptions.From = new Twilio.Types.PhoneNumber("whatsapp:+14155238886");
            //messageOptions.Body = "Web Page: " + this.txtMessage.Text;

            ////var message = MessageResource.Create(
            ////    body: this.txtMessage.Text,
            ////    from: new Twilio.Types.PhoneNumber("whatsapp:+14155238886"),
            ////    to: new Twilio.Types.PhoneNumber("whatsapp:+529842129836"));

            //var message = MessageResource.Create(messageOptions);

            //lblResponse.Text = message.Status.ToString();
        }
    }
}