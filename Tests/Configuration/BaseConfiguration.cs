using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using ExternalLogin;
using FluentAssertions;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace Tests.Configuration
{
    [TestFixture]
    public class BaseConfiguration
    {
        private TestServer Server { get; set; }
        protected HttpResponseMessage Response { get; set; }
        protected string Content { get; set; }

        protected const string BaseUrl = "http://tests/";

        [SetUp]
        public void Setup()
        {
            Server = TestServer.Create<Startup>();
        }

        protected void WhenPost<T>(string relativeUrl, T obj)
        {
            HttpRequestMessage request = this.CreatePostRequest<T>(relativeUrl, obj);
            Response = this.Server.HttpClient.SendAsync(request).Result;
            Content = Response.Content.ReadAsStringAsync().Result;
        }

        protected void WhenGet(string relativeUrl)
        {
            var request = this.CreateGetRequest(relativeUrl);
            Response = Server.HttpClient.SendAsync(request).Result;
            Content = Response.Content.ReadAsStringAsync().Result;
        }

        private HttpRequestMessage CreatePostRequest<T>(string relativeUrl, T obj)
        {
            var wholeUrl = new Uri(string.Concat(BaseUrl, relativeUrl));
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = wholeUrl,
                Content = new ObjectContent(typeof(T), obj, new JsonMediaTypeFormatter()),
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return request;
        }

        private HttpRequestMessage CreateGetRequest(string relativeUrl)
        {
            var url = string.Concat(BaseUrl, relativeUrl);
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get
            };
            return request;
        }

        protected void Then200()
        {
            Response.Should().NotBeNull();
            Response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
        }

        protected void Then302()
        {
            Response.Should().NotBeNull();
            Response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.Found);
        }

        protected void ThenAuthorized()
        {
            var context = Response.RequestMessage.GetOwinContext();
        }
    }
}
