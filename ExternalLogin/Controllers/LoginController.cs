using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace ExternalLogin.Controllers
{
    public class LoginController : ApiController
    {
        [HttpGet]
        [Route("identity")]
        public IHttpActionResult ExternalLogin()
        {
            var user = User as ClaimsPrincipal;
            var claims = from c in user.Claims
                         select new
                         {
                             type = c.Type,
                             value = c.Value
                         };

            return Json(claims);
        }
    }
}
