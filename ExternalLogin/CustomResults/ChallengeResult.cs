using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace ExternalLogin.CustomResults
{
    public class ChallengeResult : IHttpActionResult
    {
        public ChallengeResult(string provider, HttpRequestMessage request)
        {
            this.AuthenticationProvider = provider;
            this.MessageRequest = request;
        }

        public HttpRequestMessage MessageRequest { get; set; }

        public string AuthenticationProvider { get; set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            this.MessageRequest.GetOwinContext().Authentication.Challenge(this.AuthenticationProvider);
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.RequestMessage = this.MessageRequest;
            return Task.FromResult(response);
        }
    }
}