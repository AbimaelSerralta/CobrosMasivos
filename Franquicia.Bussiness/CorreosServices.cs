using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class CorreosServices
    {
        public void CorreoLiga(string Nombre, string Asunto, string Concepto, decimal Importe, DateTime Vencimiento, string LigaUrl, string Correo, string strPromociones, bool boolPromociones, string NombreComercial)
        {
            MailMessage mail = new MailMessage("liga@cobrosmasivos.com", Correo);
            mail.Subject = Asunto;
            //Se crea el contenido del correo eletronico

            string Promociones = string.Empty;

            if (boolPromociones)
            {
                Promociones =
                "\t\t\t\t\t\t\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n" +
                "\t\t\t\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td colspan=\"2\" style=\"padding-top: 20px; color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: center;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t<h3>Si lo desea puede pagar con las siguientes promociones:</h3>\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t\t\t\t" + strPromociones + "\r\n" +
                "\t\t\t\t\t\t\t</table>\r\n";
            }

            string ho = "<!DOCTYPE html>\r\n" +
                "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n" +
                "<head>\r\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />\r\n" +
                "<title></title>\r\n" +
                "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"/>\r\n" +
                "</head>\r\n" +
                "<body style=\"margin: 0; padding: 0;\">\r\n" +
                "\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\t\r\n" +
                "\t\t<tr>\r\n" +
                "\t\t\t<td style=\"padding: 10px 0 30px 0;\">\r\n" +
                "\t\t\t\t<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"600\" style=\"border: 1px solid #cccccc; border-collapse: collapse;\">\r\n" +
                "\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t<td align=\"center\" bgcolor=\"#00bcd4\" style=\"padding: 40px 0 30px 0; color: #153643; font-size: 28px; font-weight: bold; font-family: Arial, sans-serif;\">\r\n" +
                "\t\t\t\t\t\t\t<a href=\"http://www.cobroscontarjeta.com/\"><img src=\"http://www.cobroscontarjeta.com/images/logo.png\" alt=\"Cobroscontarjeta\" width=\"500\" height=\"120\" style=\"display: block;\" /></a>\r\n" +
                "\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t<td bgcolor=\"#ffffff\" style=\"padding: 40px 30px 40px 30px;\">\r\n" +
                "\t\t\t\t\t\t\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n" +
                "\t\t\t\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td colspan=\"2\" style=\"text-align: center;color: #153643; font-family: Arial, sans-serif; font-size: 24px;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t<b> Hola, " + Nombre + "</b>\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t\t\t</table>\r\n" +

                "\t\t\t\t\t\t\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n" +
                "\t\t\t\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td colspan=\"2\" style=\"padding-top: 20px; color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: center;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t<h3>" + NombreComercial + " le ha enviado su liga de pago con los siguientes datos:</h3>\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: right;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\tConcepto:\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: left;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t &nbsp;" + Concepto + "\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: right;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\tImporte:\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: left;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t &nbsp;$" + Importe.ToString("N2") + "\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: right;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\tVencimiento:\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: left;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t &nbsp;" + Vencimiento.ToString("dd/MM/yyyy") + "\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t\t\t</table>\r\n" +

                "\r\n\r\n\t\t\t\t\t\t<br/><br/><a style =\"display:block;color:#fff;font-weight:400;text-align:center;width:230px;font-size:20px;text-decoration:none;background:#28a745;margin:0 auto;padding:15px 0\" href=" + LigaUrl + "> Pagar $" + Importe.ToString("N2") + "</a>" +

                "\t\t\t\t\t\t\t\t" + Promociones + "\r\n" +

                "\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t<td bgcolor=\"#df5f16\" style=\"padding: 30px 30px 30px 30px;\">\r\n" +
                "\t\t\t\t\t\t\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n" +
                "\t\t\t\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td style=\"color: #ffffff; font-family: Arial, sans-serif; font-size: 14px;\" width=\"75%\">\r\n\t\t\t\t\t\t\t\t\t\tCobroscontarjeta &reg; Todos los derechos reservados, 2020<br/>\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td align=\"right\" width=\"25%\">\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t\t\t</table>\r\n" +
                "\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t</table>\r\n" +
                "\t\t\t</td>\r\n" +
                "\t\t</tr>\r\n" +
                "\t</table>\r\n" +
                "</body>\r\n" +
                "</html>";

            mail.Body = ho;
            mail.IsBodyHtml = true;
            //Se activa una variable del protocolo SMTP para poder enviar el correo electronico
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "mail.cobrosmasivos.com";
            smtp.EnableSsl = false;

            //Activacion de la cuenta de la que se enviaran los correos electronicos
            NetworkCredential credenciales = new NetworkCredential("liga@cobrosmasivos.com", "L1g4m4s1va.c0m");
            // asignacion de las credenciales al protocolo smtp
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = credenciales;
            //Puerto de salida de correo electronico
            smtp.Port = 2525;
            //envio del correo electronico
            smtp.Send(mail);
        }
        public void CorreoLigaMultiple(string Nombre, string Asunto, string Concepto, decimal Importe, DateTime Vencimiento, string LigaUrl, string Correo)
        {
            MailMessage mail = new MailMessage("liga@cobrosmasivos.com", Correo);
            mail.Subject = Asunto;
            //Se crea el contenido del correo eletronico

            string ho = "<!DOCTYPE html>\r\n" +
                "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n" +
                "<head>\r\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />\r\n" +
                "<title></title>\r\n" +
                "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"/>\r\n" +
                "</head>\r\n" +
                "<body style=\"margin: 0; padding: 0;\">\r\n" +
                "\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\t\r\n" +
                "\t\t<tr>\r\n" +
                "\t\t\t<td style=\"padding: 10px 0 30px 0;\">\r\n" +
                "\t\t\t\t<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"600\" style=\"border: 1px solid #cccccc; border-collapse: collapse;\">\r\n" +
                "\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t<td align=\"center\" bgcolor=\"#00bcd4\" style=\"padding: 40px 0 30px 0; color: #153643; font-size: 28px; font-weight: bold; font-family: Arial, sans-serif;\">\r\n" +
                "\t\t\t\t\t\t\t<a href=\"http://www.cobroscontarjeta.com/\"><img src=\"http://www.cobroscontarjeta.com/images/logo.png\" alt=\"Cobroscontarjeta\" width=\"500\" height=\"120\" style=\"display: block;\" /></a>\r\n" +
                "\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t<td bgcolor=\"#ffffff\" style=\"padding: 40px 30px 40px 30px;\">\r\n" +
                "\t\t\t\t\t\t\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n" +
                "\t\t\t\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td colspan=\"2\" style=\"text-align: center;color: #153643; font-family: Arial, sans-serif; font-size: 24px;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t<b> Hola, " + Nombre + "</b>\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t\t\t</table>\r\n" +

                "\t\t\t\t\t\t\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n" +
                "\t\t\t\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td colspan=\"2\" style=\"padding-top: 20px; color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: center;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t<h3>Le hemos enviado su liga de pago con los siguientes datos:</h3>\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: right;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\tConcepto:\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: left;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t &nbsp;" + Concepto + "\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: right;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\tImporte:\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: left;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t &nbsp;$" + Importe.ToString("N2") + "\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: right;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\tVencimiento:\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: left;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t &nbsp;" + Vencimiento.ToString("dd/MM/yyyy") + "\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t\t\t</table>\r\n" +

                "\r\n\r\n\t\t\t\t\t\t<br/><br/><a style =\"display:block;color:#fff;font-weight:400;text-align:center;width:230px;font-size:20px;text-decoration:none;background:#28a745;margin:0 auto;padding:15px 0\" href=" + LigaUrl + "> Pagar $" + Importe.ToString("N2") + "</a>" +

                "\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t<td bgcolor=\"#df5f16\" style=\"padding: 30px 30px 30px 30px;\">\r\n" +
                "\t\t\t\t\t\t\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n" +
                "\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t<td style=\"color: #ffffff; font-family: Arial, sans-serif; font-size: 14px;\" width=\"75%\">\r\n\t\t\t\t\t\t\t\t\t\tCobroscontarjeta &reg; Todos los derechos reservados, 2020<br/>\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td align=\"right\" width=\"25%\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<td style=\"font-family: Arial, sans-serif; font-size: 12px; font-weight: bold;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t\t\t<a href=\"https://twitter.com/ \" style=\"color: #ffffff;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<img src=\"https://help.twitter.com/content/dam/help-twitter/brand/logo.png\" alt=\"Twitter\" width=\"38\" height=\"38\" style=\"display: block;\" border=\"0\" />\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t\t\t</a>\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t\t<td style=\"font-size: 0; line-height: 0;\" width=\"20\">&nbsp;</td>\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t\t<td style=\"font-family: Arial, sans-serif; font-size: 12px; font-weight: bold;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t\t\t<a href=\"https://www.facebook.com/ \" style=\"color: #ffffff;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t\t\t\t<img src=\"https://images.vexels.com/media/users/3/137253/isolated/preview/90dd9f12fdd1eefb8c8976903944c026-icono-de-facebook-logo-by-vexels.png\" alt=\"Facebook\" width=\"38\" height=\"38\" style=\"display: block;\" border=\"0\" />\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t\t\t</a>\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t\t\t\t\t\t</table>\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t\t\t</table>\r\n" +
                "\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t</table>\r\n" +
                "\t\t\t</td>\r\n" +
                "\t\t</tr>\r\n" +
                "\t</table>\r\n" +
                "</body>\r\n" +
                "</html>";

            mail.Body = ho;
            mail.IsBodyHtml = true;
            //Se activa una variable del protocolo SMTP para poder enviar el correo electronico
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "mail.cobrosmasivos.com";
            smtp.EnableSsl = false;

            //Activacion de la cuenta de la que se enviaran los correos electronicos
            NetworkCredential credenciales = new NetworkCredential("liga@cobrosmasivos.com", "L1g4m4s1va.c0m");
            // asignacion de las credenciales al protocolo smtp
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = credenciales;
            //Puerto de salida de correo electronico
            smtp.Port = 2525;
            //envio del correo electronico
            smtp.Send(mail);
        }

        public void CorreoCadena(string Nombre, string Correo)
        {
            MailMessage mail = new MailMessage("liga@cobrosmasivos.com", Correo);
            mail.Subject = "Cadena";
            //Se crea el contenido del correo eletronico

            string ho = "<!DOCTYPE html>\r\n" +
                "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n" +
                "<head>\r\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />\r\n" +
                "<title></title>\r\n" +
                "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"/>\r\n" +
                "</head>\r\n" +
                "<body style=\"margin: 0; padding: 0;\">\r\n" +
                "\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\t\r\n" +
                "\t\t<tr>\r\n" +
                "\t\t\t<td style=\"padding: 10px 0 30px 0;\">\r\n" +
                "\t\t\t\t<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"600\" style=\"border: 1px solid #cccccc; border-collapse: collapse;\">\r\n" +
                "\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t<td align=\"center\" bgcolor=\"#00bcd4\" style=\"padding: 40px 0 30px 0; color: #153643; font-size: 28px; font-weight: bold; font-family: Arial, sans-serif;\">\r\n" +
                "\t\t\t\t\t\t\t<a href=\"http://www.cobroscontarjeta.com/\"><img src=\"http://www.cobroscontarjeta.com/images/logo.png\" alt=\"Cobroscontarjeta\" width=\"500\" height=\"120\" style=\"display: block;\" /></a>\r\n" +
                "\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t<td bgcolor=\"#ffffff\" style=\"padding: 40px 30px 40px 30px;\">\r\n" +
                "\t\t\t\t\t\t\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n" +
                "\t\t\t\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td colspan=\"2\" style=\"text-align: center;color: #153643; font-family: Arial, sans-serif; font-size: 24px;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t<b> Hola, " + "" + "</b>\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t\t\t</table>\r\n" +

                "\t\t\t\t\t\t\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n" +
                "\t\t\t\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td colspan=\"2\" style=\"padding-top: 20px; color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: center;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t<h3>Le hemos enviado su liga de pago con los siguientes datos:</h3>\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: right;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\tConcepto:\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: left;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t &nbsp;" + Nombre + "\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t\t\t</table>\r\n" +

                "\r\n\r\n\t\t\t\t\t\t<br/><br/><a style =\"display:block;color:#fff;font-weight:400;text-align:center;width:230px;font-size:20px;text-decoration:none;background:#28a745;margin:0 auto;padding:15px 0\" href=" + "" + "> Pagar</a>" +

                "\r\n\r\n\t\t\t\t\t\t<br/><br/><textarea>" + Nombre + "</textarea>" +

                "\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t<td bgcolor=\"#df5f16\" style=\"padding: 30px 30px 30px 30px;\">\r\n" +
                "\t\t\t\t\t\t\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n" +
                "\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t<td style=\"color: #ffffff; font-family: Arial, sans-serif; font-size: 14px;\" width=\"75%\">\r\n\t\t\t\t\t\t\t\t\t\tCobroscontarjeta &reg; Todos los derechos reservados, 2020<br/>\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td align=\"right\" width=\"25%\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<td style=\"font-family: Arial, sans-serif; font-size: 12px; font-weight: bold;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t\t\t<a href=\"https://twitter.com/ \" style=\"color: #ffffff;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<img src=\"https://help.twitter.com/content/dam/help-twitter/brand/logo.png\" alt=\"Twitter\" width=\"38\" height=\"38\" style=\"display: block;\" border=\"0\" />\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t\t\t</a>\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t\t<td style=\"font-size: 0; line-height: 0;\" width=\"20\">&nbsp;</td>\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t\t<td style=\"font-family: Arial, sans-serif; font-size: 12px; font-weight: bold;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t\t\t<a href=\"https://www.facebook.com/ \" style=\"color: #ffffff;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t\t\t\t<img src=\"https://images.vexels.com/media/users/3/137253/isolated/preview/90dd9f12fdd1eefb8c8976903944c026-icono-de-facebook-logo-by-vexels.png\" alt=\"Facebook\" width=\"38\" height=\"38\" style=\"display: block;\" border=\"0\" />\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t\t\t</a>\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t\t\t\t\t\t</table>\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t\t\t</table>\r\n" +
                "\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t</table>\r\n" +
                "\t\t\t</td>\r\n" +
                "\t\t</tr>\r\n" +
                "\t</table>\r\n" +
                "</body>\r\n" +
                "</html>";

            mail.Body = ho;
            mail.IsBodyHtml = true;
            //Se activa una variable del protocolo SMTP para poder enviar el correo electronico
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "mail.cobrosmasivos.com";
            smtp.EnableSsl = false;

            //Activacion de la cuenta de la que se enviaran los correos electronicos
            NetworkCredential credenciales = new NetworkCredential("liga@cobrosmasivos.com", "L1g4m4s1va.c0m");
            // asignacion de las credenciales al protocolo smtp
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = credenciales;
            //Puerto de salida de correo electronico
            smtp.Port = 2525;
            //envio del correo electronico
            smtp.Send(mail);
        }

        public string CorreoEnvioCredenciales(string Nombre, string Asunto, string Usuario, string Contrasenia, string LigaUrl, string Correo, string NombreComercial)
        {
            string mnsj = string.Empty;

            try
            {
                MailMessage mail = new MailMessage("liga@cobrosmasivos.com", Correo);
                mail.Subject = Asunto;
                //Se crea el contenido del correo eletronico

                string ho = "<!DOCTYPE html>\r\n" +
                    "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n" +
                    "<head>\r\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />\r\n" +
                    "<title></title>\r\n" +
                    "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"/>\r\n" +
                    "</head>\r\n" +
                    "<body style=\"margin: 0; padding: 0;\">\r\n" +
                    "\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\t\r\n" +
                    "\t\t<tr>\r\n" +
                    "\t\t\t<td style=\"padding: 10px 0 30px 0;\">\r\n" +
                    "\t\t\t\t<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"600\" style=\"border: 1px solid #cccccc; border-collapse: collapse;\">\r\n" +
                    "\t\t\t\t\t<tr>\r\n" +
                    "\t\t\t\t\t\t<td align=\"center\" bgcolor=\"#00bcd4\" style=\"padding: 40px 0 30px 0; color: #153643; font-size: 28px; font-weight: bold; font-family: Arial, sans-serif;\">\r\n" +
                    "\t\t\t\t\t\t\t<a href=\"http://www.cobroscontarjeta.com/\"><img src=\"http://www.cobroscontarjeta.com/images/logo.png\" alt=\"Cobroscontarjeta\" width=\"500\" height=\"120\" style=\"display: block;\" /></a>\r\n" +
                    "\t\t\t\t\t\t</td>\r\n" +
                    "\t\t\t\t\t</tr>\r\n" +
                    "\t\t\t\t\t<tr>\r\n" +
                    "\t\t\t\t\t\t<td bgcolor=\"#ffffff\" style=\"padding: 40px 30px 40px 30px;\">\r\n" +
                    "\t\t\t\t\t\t\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n" +
                    "\t\t\t\t\t\t\t\t<tr>\r\n" +
                    "\t\t\t\t\t\t\t\t\t<td colspan=\"2\" style=\"text-align: center;color: #153643; font-family: Arial, sans-serif; font-size: 24px;\">\r\n" +
                    "\t\t\t\t\t\t\t\t\t\t<b> Hola, " + Nombre + "</b>\r\n" +
                    "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                    "\t\t\t\t\t\t\t\t</tr>\r\n" +
                    "\t\t\t\t\t\t\t</table>\r\n" +

                    "\t\t\t\t\t\t\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n" +
                    "\t\t\t\t\t\t\t\t<tr>\r\n" +
                    "\t\t\t\t\t\t\t\t\t<td colspan=\"2\" style=\"padding-top: 20px; color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: center;\">\r\n" +
                    "\t\t\t\t\t\t\t\t\t\t<h3>" + NombreComercial + " le ha enviado las credenciales de acceso para el sistema Paga La Escuela:</h3>\r\n" +
                    "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                    "\t\t\t\t\t\t\t\t</tr>\r\n" +
                    "\t\t\t\t\t\t\t\t<tr>\r\n" +
                    "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: right;\">\r\n" +
                    "\t\t\t\t\t\t\t\t\t\tUsuario:\r\n" +
                    "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                    "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: left;\">\r\n" +
                    "\t\t\t\t\t\t\t\t\t\t &nbsp;" + Usuario + "\r\n" +
                    "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                    "\t\t\t\t\t\t\t\t</tr>\r\n" +
                    "\t\t\t\t\t\t\t\t<tr>\r\n" +
                    "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: right;\">\r\n" +
                    "\t\t\t\t\t\t\t\t\t\tContraseña:\r\n" +
                    "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                    "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: left;\">\r\n" +
                    "\t\t\t\t\t\t\t\t\t\t &nbsp;" + Contrasenia + "\r\n" +
                    "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                    "\t\t\t\t\t\t\t\t</tr>\r\n" +
                    "\t\t\t\t\t\t\t\t<tr>\r\n" +
                    "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: right;\">\r\n" +
                    "\t\t\t\t\t\t\t\t\t\tUrl:\r\n" +
                    "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                    "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: left;\">\r\n" +
                    "\t\t\t\t\t\t\t\t\t\t &nbsp;" + LigaUrl + "\r\n" +
                    "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                    "\t\t\t\t\t\t\t\t</tr>\r\n" +
                    "\t\t\t\t\t\t\t</table>\r\n" +

                    "\t\t\t\t\t\t\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n" +
                    "\t\t\t\t\t\t\t\t<tr>\r\n" +
                    "\t\t\t\t\t\t\t\t\t<td colspan=\"2\" style=\"text-align: center;color: #153643; font-family: Arial, sans-serif; font-size: 24px;\">\r\n" +
                    "\r\n\r\n\t\t\t\t\t\t<br/><br/><a style =\"display:block;color:#fff;font-weight:400;width:230px;font-size:20px;text-decoration:none;background:#28a745;margin:0 auto;padding:15px 0\" href=" + LigaUrl + "> Iniciar sesión" + "</a>" +
                    "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                    "\t\t\t\t\t\t\t\t</tr>\r\n" +
                    "\t\t\t\t\t\t\t</table>\r\n" +

                    "\t\t\t\t\t\t</td>\r\n" +
                    "\t\t\t\t\t</tr>\r\n" +
                    "\t\t\t\t\t<tr>\r\n" +
                    "\t\t\t\t\t\t<td bgcolor=\"#df5f16\" style=\"padding: 30px 30px 30px 30px;\">\r\n" +
                    "\t\t\t\t\t\t\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n" +
                    "\t\t\t\t\t\t\t\t<tr>\r\n" +
                    "\t\t\t\t\t\t\t\t\t<td style=\"color: #ffffff; font-family: Arial, sans-serif; font-size: 14px;\" width=\"75%\">\r\n\t\t\t\t\t\t\t\t\t\tCobroscontarjeta &reg; Todos los derechos reservados, 2020<br/>\r\n" +
                    "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                    "\t\t\t\t\t\t\t\t\t<td align=\"right\" width=\"25%\">\r\n" +
                    "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                    "\t\t\t\t\t\t\t\t</tr>\r\n" +
                    "\t\t\t\t\t\t\t</table>\r\n" +
                    "\t\t\t\t\t\t</td>\r\n" +
                    "\t\t\t\t\t</tr>\r\n" +
                    "\t\t\t\t</table>\r\n" +
                    "\t\t\t</td>\r\n" +
                    "\t\t</tr>\r\n" +
                    "\t</table>\r\n" +
                    "</body>\r\n" +
                    "</html>";

                mail.Body = ho;
                mail.IsBodyHtml = true;
                //Se activa una variable del protocolo SMTP para poder enviar el correo electronico
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "mail.cobrosmasivos.com";
                smtp.EnableSsl = false;

                //Activacion de la cuenta de la que se enviaran los correos electronicos
                NetworkCredential credenciales = new NetworkCredential("liga@cobrosmasivos.com", "L1g4m4s1va.c0m");
                // asignacion de las credenciales al protocolo smtp
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = credenciales;
                //Puerto de salida de correo electronico
                smtp.Port = 2525;
                //envio del correo electronico
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                mnsj = ex.Message;
            }

            return mnsj;
        }
    }
}
