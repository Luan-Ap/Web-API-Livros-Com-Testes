using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using WebLivros.Api.IntegrationTests.Configuration;
using WebLivros.Core.DTO;
using Xunit;

namespace WebLivros.Api.IntegrationTests.Controller
{
    public class LivrosControllerIntegrationTests : BaseIntegrationTests
    {
        public LivrosControllerIntegrationTests(BaseTestFixture fixture)
            : base(fixture)
        {
        }

        [Fact(DisplayName = "DADO um livro válido QUANDO utilizamos o método PostAsync ENTÃO realizar a inserção na base de dados e chamar o método GetAsync para retornar o livro recem inserido e comparar seus valores.")]
        public async void PostGetByLivroId_LivroValido_VerificaLivroInserido()
        {
            //ARRANGE e ACT
            var novoLivro = new NewLivroDto { Titulo = "Livro Teste Integrado 1", Sinopse = "Livro usado para testar a Integração", Publicacao = new DateTime(2022, 01, 28), Autor = "Autor do Livro" };

            var respostaPost = await TestServer
                .CreateRequest("api/v1/Livros")
                .And(req => req.Content = GerarConteudoRequisicao(novoLivro))
                .PostAsync();

            var livroInserido = JsonConvert.DeserializeObject<LivroDto>(await respostaPost.Content.ReadAsStringAsync());

            var respostaGetByLivroId = await TestServer
                .CreateRequest($"api/v1/Livros/{livroInserido.LivroId}")
                .GetAsync();

            var livroRetornado = JsonConvert.DeserializeObject<LivroDto>(await respostaGetByLivroId.Content.ReadAsStringAsync());

            //ASSERT
            Assert.Equal(201, Convert.ToInt32(respostaPost.StatusCode));
            Assert.Equal(200, Convert.ToInt32(respostaGetByLivroId.StatusCode));

            Assert.NotNull(livroInserido);
            Assert.NotNull(livroRetornado);

            Assert.Equal(livroInserido.LivroId, livroRetornado.LivroId);
            Assert.Equal(livroInserido.Titulo, livroRetornado.Titulo);
            Assert.Equal(livroInserido.Sinopse, livroRetornado.Sinopse);
            Assert.Equal(livroInserido.Publicacao, livroRetornado.Publicacao);
            Assert.Equal(livroInserido.Autor, livroRetornado.Autor);

        }

        [Fact(DisplayName = "DADO uma lista de livros válidos QUANDO utilizamos o método PostAsync ENTÃO realizar a inserção na base de dados e chamar o método GetAsync para retornar todos os livros inseridos.")]
        public async void PostGet_LivroValido_RetornarLivrosInseridos()
        {
            //ARRANGE
            var listaNovosLivros = new List<NewLivroDto>
            {
                new NewLivroDto { Titulo = "Livro 1", Sinopse = "Sinopse Livro 1", Publicacao = new DateTime(2000, 01, 20), Autor = "Autor Livro 1" },
                new NewLivroDto { Titulo = "Livro 2", Sinopse = "Sinopse Livro 2", Publicacao = new DateTime(2001, 04, 10), Autor = "Autor Livro 2" },
                new NewLivroDto { Titulo = "Livro 3", Sinopse = "Sinopse Livro 3", Publicacao = new DateTime(2003, 07, 14), Autor = "Autor Livro 3" },
                new NewLivroDto { Titulo = "Livro 4", Sinopse = "Sinopse Livro 4", Publicacao = new DateTime(2004, 08, 24), Autor = "Autor Livro 4" },
                new NewLivroDto { Titulo = "Livro 5", Sinopse = "Sinopse Livro 5", Publicacao = new DateTime(2005, 02, 11), Autor = "Autor Livro 5" }
            };

            //ACT e ASSERT
            foreach (var livro in listaNovosLivros)
            {
                var respostaPost = await TestServer
                    .CreateRequest("api/v1/Livros")
                    .And(req => req.Content = GerarConteudoRequisicao(livro))
                    .PostAsync();

                Assert.Equal(201, Convert.ToInt32(respostaPost.StatusCode));
            }

            var respostaGetByNome = await TestServer
                .CreateRequest($"api/v1/Livros")
                .GetAsync();

            var listaLivrosRetornados = JsonConvert.DeserializeObject<List<LivroDto>>(await respostaGetByNome.Content.ReadAsStringAsync());

            Assert.Equal(200, Convert.ToInt32(respostaGetByNome.StatusCode));

            Assert.NotEmpty(listaLivrosRetornados);
            Assert.Equal(5, listaLivrosRetornados.Count);
        }

        [Fact(DisplayName = "DADO uma lista de livros válidos QUANDO utilizamos o método PostAsync ENTÃO realizar a inserção na base de dados e chamar o método GetAsync para retornar todos os livros com Títulos contendo a string passada.")]
        public async void PostGetByNome_LivroValido_RetornarLivrosPeloTitulo()
        {
            //ARRANGE
            var listaNovosLivros = new List<NewLivroDto>
            {
                new NewLivroDto { Titulo = "As Aventuras de L: Vol. 1", Sinopse = "Vol. 1 da série de livros As Aventuras de L", Publicacao = new DateTime(2000, 01, 20), Autor = "Autor L" },
                new NewLivroDto { Titulo = "As Aventuras de L: Vol. 2", Sinopse = "Vol. 2 da série de livros As Aventuras de L", Publicacao = new DateTime(2001, 04, 10), Autor = "Autor L" },
                new NewLivroDto { Titulo = "As Aventuras de L: Vol. 3", Sinopse = "Vol. 3 da série de livros As Aventuras de L", Publicacao = new DateTime(2002, 03, 14), Autor = "Autor L" },
                new NewLivroDto { Titulo = "As Aventuras de R: Vol. 1", Sinopse = "Vol. 1 da série de livros As Aventuras de R", Publicacao = new DateTime(2002, 03, 14), Autor = "Autor R" },
                new NewLivroDto { Titulo = "As Aventuras de R: Vol. 2", Sinopse = "Vol. 2 da série de livros As Aventuras de R", Publicacao = new DateTime(2003, 05, 11), Autor = "Autor R" }
            };

            var titulo = "As Aventuras de L";

            //ACT e ASSERT
            foreach (var livro in listaNovosLivros)
            {
                var respostaPost = await TestServer
                    .CreateRequest("api/v1/Livros")
                    .And(req => req.Content = GerarConteudoRequisicao(livro))
                    .PostAsync();

                Assert.Equal(201, Convert.ToInt32(respostaPost.StatusCode));
            }

            var respostaGetByNome = await TestServer
                .CreateRequest($"api/v1/Livros/Titulo/{titulo}")
                .GetAsync();

            var listaLivrosRetornados = JsonConvert.DeserializeObject<List<LivroDto>>(await respostaGetByNome.Content.ReadAsStringAsync());

            Assert.Equal(200, Convert.ToInt32(respostaGetByNome.StatusCode));

            Assert.NotEmpty(listaLivrosRetornados);
            Assert.Equal(3, listaLivrosRetornados.Count);

            foreach(var livro in listaLivrosRetornados)
            {
                Assert.Contains(titulo, livro.Titulo);
            }
        }

        [Fact(DisplayName = "DADO uma lista de livros válidos QUANDO utilizamos o método PostAsync ENTÃO realizar a inserção na base de dados e chamar o método GetAsync para retornar todos os livros com o mesmo Autor.")]
        public async void PostGetByAutor_LivroValido_RetornarLivrosPeloAutor()
        {
            //ARRANGE
            var listaNovosLivros = new List<NewLivroDto>
            {
                new NewLivroDto { Titulo = "As Aventuras de L: Vol. 1", Sinopse = "Vol. 1 da série de livros As Aventuras de L", Publicacao = new DateTime(2000, 01, 20), Autor = "Autor L" },
                new NewLivroDto { Titulo = "As Aventuras de L: Vol. 2", Sinopse = "Vol. 2 da série de livros As Aventuras de L", Publicacao = new DateTime(2001, 04, 10), Autor = "Autor L" },
                new NewLivroDto { Titulo = "As Aventuras de L: Vol. 3", Sinopse = "Vol. 3 da série de livros As Aventuras de L", Publicacao = new DateTime(2002, 03, 14), Autor = "Autor L" },
                new NewLivroDto { Titulo = "As Aventuras de R: Vol. 1", Sinopse = "Vol. 1 da série de livros As Aventuras de R", Publicacao = new DateTime(2002, 03, 14), Autor = "Autor R" },
                new NewLivroDto { Titulo = "As Aventuras de R: Vol. 2", Sinopse = "Vol. 2 da série de livros As Aventuras de R", Publicacao = new DateTime(2003, 05, 11), Autor = "Autor R" }
            };

            var autor = "Autor R";

            //ACT e ASSERT
            foreach (var livro in listaNovosLivros)
            {
                var respostaPost = await TestServer
                    .CreateRequest("api/v1/Livros")
                    .And(req => req.Content = GerarConteudoRequisicao(livro))
                    .PostAsync();

                Assert.Equal(201, Convert.ToInt32(respostaPost.StatusCode));
            }

            var respostaGetByNome = await TestServer
                .CreateRequest($"api/v1/Livros/Autor/{autor}")
                .GetAsync();

            var listaLivrosRetornados = JsonConvert.DeserializeObject<List<LivroDto>>(await respostaGetByNome.Content.ReadAsStringAsync());

            Assert.Equal(200, Convert.ToInt32(respostaGetByNome.StatusCode));

            Assert.NotEmpty(listaLivrosRetornados);
            Assert.Equal(2, listaLivrosRetornados.Count);

            foreach (var livro in listaLivrosRetornados)
            {
                Assert.Equal(autor, livro.Autor);
            }
        }

        [Fact(DisplayName = "DADO um livro válido QUANDO utilizamos o método PostAsync ENTÃO realizar a inserção na base de dados, depois atualizá-lo e chamar o método GetAsync para retornar o livro e comparar seus valores.")]
        public async void PutGetByLivroId_LivroValido_VerificaLivroAtualizado()
        {
            //ARRANGE e ACT
            var novoLivro = new NewLivroDto { Titulo = "Livro Teste Integrado 1", Sinopse = "Livro usado para testar a Integração", Publicacao = new DateTime(2022, 01, 28), Autor = "Autor do Livro" };

            var respostaPost = await TestServer
                .CreateRequest("api/v1/Livros")
                .And(req => req.Content = GerarConteudoRequisicao(novoLivro))
                .PostAsync();

            var livroInserido = JsonConvert.DeserializeObject<LivroDto>(await respostaPost.Content.ReadAsStringAsync());

            var livroAtualizado = new LivroDto { LivroId = livroInserido.LivroId, Titulo = "Livro Teste Integrado", Sinopse = "Livro usado para testar", Publicacao = new DateTime(2021, 03, 17), Autor = "Autor Integração" };

            var respostaPut = await TestServer
                .CreateRequest($"api/v1/Livros/{livroInserido.LivroId}")
                .And(req => req.Content = GerarConteudoRequisicao(livroAtualizado))
                .SendAsync("PUT");

            var respostaGetByLivroId = await TestServer
                .CreateRequest($"api/v1/Livros/{livroInserido.LivroId}")
                .GetAsync();

            var livroRetornado = JsonConvert.DeserializeObject<LivroDto>(await respostaGetByLivroId.Content.ReadAsStringAsync());

            //ASSERT
            Assert.Equal(201, Convert.ToInt32(respostaPost.StatusCode));
            Assert.Equal(204, Convert.ToInt32(respostaPut.StatusCode));
            Assert.Equal(200, Convert.ToInt32(respostaGetByLivroId.StatusCode));

            Assert.NotNull(livroInserido);
            Assert.NotNull(livroRetornado);

            Assert.Equal(livroAtualizado.LivroId, livroRetornado.LivroId);
            Assert.Equal(livroAtualizado.Titulo, livroRetornado.Titulo);
            Assert.Equal(livroAtualizado.Sinopse, livroRetornado.Sinopse);
            Assert.Equal(livroAtualizado.Publicacao, livroRetornado.Publicacao);
            Assert.Equal(livroAtualizado.Autor, livroRetornado.Autor);

            Assert.NotEqual(novoLivro.Titulo, livroRetornado.Titulo);
            Assert.NotEqual(novoLivro.Sinopse, livroRetornado.Sinopse);
            Assert.NotEqual(novoLivro.Publicacao, livroRetornado.Publicacao);
            Assert.NotEqual(novoLivro.Autor, livroRetornado.Autor);

        }
    }
}
