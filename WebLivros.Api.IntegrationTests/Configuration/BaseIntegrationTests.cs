using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebLivros.Data.Context;
using Xunit;

namespace WebLivros.Api.IntegrationTests.Configuration
{
    [Collection("Base Collection")]
    public abstract class BaseIntegrationTests
    {
        protected readonly TestServer TestServer;
        protected readonly HttpClient HttpClient;
        protected readonly WebLivrosContext LivrosContext;

        protected BaseTestFixture BaseFixture;

        public BaseIntegrationTests(BaseTestFixture baseTestFixture)
        {
            BaseFixture = baseTestFixture;

            TestServer = baseTestFixture.TestServer;
            HttpClient = baseTestFixture.HttpClient;
            LivrosContext = baseTestFixture.LivrosContext;

            LimparDb().Wait();
        }

        private async Task LimparDb()
        {
            var commandos = new[]
            {
                "EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'",
                "EXEC sp_MSForEachTable 'SET QUOTED_IDENTIFIER ON; DELETE FROM ?'",
                "EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'"
            };

            await LivrosContext.Database.OpenConnectionAsync();

            foreach(var comando in commandos)
            {
                await LivrosContext.Database.ExecuteSqlRawAsync(comando);
            }

            LivrosContext.Database.CloseConnection();
        }

        protected StringContent GerarConteudoRequisicao(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }
    }
}
