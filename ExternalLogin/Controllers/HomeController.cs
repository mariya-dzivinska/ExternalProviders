using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace ExternalLogin.Controllers
{
    public class HomeController : ApiController
    {
        [Authorize]
        [Route("home")]
        [HttpGet]
        public IHttpActionResult Index()
        {
            var user = (ClaimsPrincipal)User;
            return this.Json(string.Join(",", user.Claims.Select(x => x.Type + " " + x.Value)));
        }

        [Route("one")]
        [HttpGet]
        public IHttpActionResult One()
        {
            return this.Json("One");
        }
    }
}
