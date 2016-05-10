using System.Security.Claims;
using System.Web.Http;

namespace ExternalLogin.Controllers
{
    [Authorize]
    public class HomeController : ApiController
    {
        [Route("home")]
        [Authorize]
        public IHttpActionResult Get()
        {
            var user = (User as ClaimsPrincipal).Claims;
            return this.Ok(user);
        }
    }
}
