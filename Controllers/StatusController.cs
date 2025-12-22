using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Net;
using System.Net.Http;
using System.Web.Http;
using RoutePrefixAttribute = System.Web.Http.RoutePrefixAttribute;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace EmpireOneRestAPIFHS.Controllers
{
    [RoutePrefix("api/status")]
    public class StatusController : ApiController
    {
        [HttpGet, Route("")]
        public IHttpActionResult GetStatus()
        {
            return Ok(new { status = "ok", timeUtc = DateTime.UtcNow });
        }

        [HttpGet, Route("ping")]
        public HttpResponseMessage Ping()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "pong");
        }


    }
}
