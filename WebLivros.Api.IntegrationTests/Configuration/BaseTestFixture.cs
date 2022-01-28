using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;
using WebLivros.Data.Context;

namespace WebLivros.Api.IntegrationTests.Configuration
{
    public class BaseTestFixture : IDisposable
    {
        public readonly TestServer TestServer;
        public readonly HttpClient HttpClient;
        public readonly WebLivrosContext LivrosContext;

        public BaseTestFixture()
        {
            TestServer = new TestServer(
                    WebHost.CreateDefaultBuilder()
                        .UseEnvironment("Testing")
                        .UseStartup<Startup>());

            LivrosContext = TestServer.Host.Services.GetService(typeof(WebLivrosContext)) as WebLivrosContext;

            HttpClient = TestServer.CreateClient();
        }

        public void Dispose()
        {
            TestServer.Dispose();
            LivrosContext.Dispose();
            HttpClient.Dispose();
        }
    }
}
