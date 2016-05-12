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
            return this.Json("Hello");
        }

        [Authorize]
        [Route("two")]
        [HttpGet]
        public IHttpActionResult Two()
        {
            return this.Json("Two");
        }

        [Route("one")]
        [HttpGet]
        public IHttpActionResult One()
        {
            return this.Json("One");
        }
    }
}
