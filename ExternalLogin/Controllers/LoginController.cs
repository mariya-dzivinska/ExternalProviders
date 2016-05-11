using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace ExternalLogin.Controllers
{
    public class LoginController : ApiController
    {
        [Route("login")]
        [HttpPost]
        public IHttpActionResult Login(string userName, string password)
        {
            //TODO: authenticate through google provider

            return this.Ok();
        }
    }
}
