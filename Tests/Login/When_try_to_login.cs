using NUnit.Framework;
using Tests.Configuration;
using TestStack.BDDfy;

namespace Tests.Login
{
    public class When_try_to_login : BaseConfiguration
    {
        [Test]
        public void Run()
        {
            this.Given(x => this.GivenNothing())
                .When(x => this.WhenGet("login"))
                .Then(x => this.ThenAuthorized())
                .BDDfy();

        }

        private void GivenNothing()
        {
        }
    }
}
