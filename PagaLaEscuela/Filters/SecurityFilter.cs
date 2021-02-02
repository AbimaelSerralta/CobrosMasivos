using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PagaLaEscuela.Filters
{
    public class SecurityFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            var h = context.HttpContext.Request.Headers;

            if (h["X-Origin"].ToString() != "563296814ASWRS")
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = new JsonResult();
                return;
            }

            if (h["User-Agent"].ToString() != "CPAPI_AGNT_V1")
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                context.Result = new JsonResult();
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}