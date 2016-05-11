using System.Security.Claims;
using System.Web.Http;

namespace ExternalLogin.Controllers
{
    [Authorize]
    public class HomeController : ApiController
    {
        [HttpGet]
        [Authorize]
        [Route("test")]
        public IHttpActionResult Home()
        {
            var user = User as ClaimsPrincipal;
            return this.Ok(
                new
                {
                    message = "OK computer",
                    client = user.FindFirst("client_id").Value
                });
        }
    }
}
