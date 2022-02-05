using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebLivros.Api.Controllers;
using WebLivros.Core.DTO;
using WebLivros.Core.Interfaces.Services;
using Xunit;

namespace WebLivros.Api.UnitTests
{
    public class LivrosControllerTests
    {
        private LivrosController controller;
        private Mock<ILivroService> mockService;

        public LivrosControllerTests()
        {
            mockService = new Mock<ILivroService>();
            controller = new LivrosController(mockService.Object);
        }

        [Fact(DisplayName = "DADO livros já existentes na base de dados QUANDO chamamos o método Get da controller ENTÃO chamar o service e o status code 200 deve ser retornado com a lista de livros.")]
        public async void Get_ChamaFindAllLivrosAsyncDeveSerChamado_StatusCode200RetornadoComLivrosEncontrados()
        {
            //ARRANGE
            var livrosEsperadosDoService = new List<LivroDto>
            {
                new LivroDto { Titulo = "Livro 1", Sinopse = "Sinopse Livro 1", Publicacao = new DateTime(1990, 10, 08), Autor = "Autor Livro 1" },
                new LivroDto { Titulo = "Livro 2", Sinopse = "Sinopse Livro 2", Publicacao = new DateTime(1990, 10, 08), Autor = "Autor Livro 2" }
            };

            mockService.Setup(s => s.FindAllLivrosAsync()).ReturnsAsync(livrosEsperadosDoService);

            //ACT
            var result = await controller.Get();

            //ASSERT
            mockService.Verify(s => s.FindAllLivrosAsync(), Times.Once);

            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);

            var listaRetornadaDoService = Assert.IsType<List<LivroDto>>(objectResult.Value);
            Assert.NotEmpty(listaRetornadaDoService);
            Assert.Equal(livrosEsperadosDoService.Count, listaRetornadaDoService.Count);
        }

        [Fact(DisplayName = "DADO o LivroId de um livro já existente na base de dados QUANDO chamamos o método GetByLivroId ENTÃO chamar o service e o status code 200 deve ser retornado com o livro que possui o LivroId passado.")]
        public async void GetByLivroId_ChamarFindLivroById_StatusCode200RetornadoComLivroEncontrado()
        {
            //ARRANGE
            var livroEsperado = new LivroDto { LivroId = 1, Titulo = "Livro 1", Sinopse = "Sinopse Livre 1", Publicacao = new DateTime(1990, 10, 08), Autor = "Autor Livro 1" };

            mockService.Setup(s => s.FindLivroById(1)).ReturnsAsync(livroEsperado);

            //ACT
            var result = await controller.GetByLivroId(1);

            //ASSERT
            mockService.Verify(s => s.FindLivroById(1), Times.Once);

            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);

            var livroRetornado = Assert.IsType<LivroDto>(objectResult.Value);
            Assert.Same(livroEsperado, livroRetornado);
            Assert.Equal(livroEsperado.LivroId, livroRetornado.LivroId);
        }

        [Fact(DisplayName = "DADO o LivroId de um livro não existente na base de dados QUANDO chamamos o método GetByLivroId ENTÃO chamar o service e o status code 404 deve ser retornado com uma mensagem de erro.")]
        public async void GetByLivroId_ChamarFindLivroById_StatusCode404RetornadoComUmaMensagemDeErro()
        {
            //ARRANGE
            mockService.Setup(s => s.FindLivroById(1)).ReturnsAsync(value: null);

            //ACT
            var result = await controller.GetByLivroId(1);

            //ASSERT
            mockService.Verify(s => s.FindLivroById(1), Times.Once);

            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, objectResult.StatusCode);

            var mensagemErro = Assert.IsType<string>(objectResult.Value);
            Assert.Equal("Não há nenhum livro com este id na base de dados.", mensagemErro);
        }

        [Fact(DisplayName = "DADO o Nome de livros já existentes na base de dados QUANDO chamamos o método GetByNome da controller ENTÃO chamar o service e o status code 200 deve ser retornado com a lista de livros que combinam com o Nome dado.")]
        public async void GetByNome_ChamaFindLivroByNomeAsync_StatusCode200RetornadoComLivrosEncontrados()
        {
            //ARRANGE
            var livroEsperadoDoService = new List<LivroDto> 
            {
                new LivroDto { Titulo = "Livro 1", Sinopse = "Sinopse Livro 1", Publicacao = new DateTime(1990, 10, 08), Autor = "Autor Livro 1-2" },
                new LivroDto { Titulo = "Livro 2", Sinopse = "Sinopse Livro 2", Publicacao = new DateTime(1990, 10, 08), Autor = "Autor Livro 1-2" }
            };

            mockService.Setup(s => s.FindLivroByNomeAsync("Livro")).ReturnsAsync(livroEsperadoDoService);

            //ACT
            var result = await controller.GetByNome("Livro");

            //ASSERT
            mockService.Verify(s => s.FindLivroByNomeAsync("Livro"), Times.Once);

            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);

            var livrosRetornadosDoService = Assert.IsType<List<LivroDto>>(objectResult.Value);
            Assert.NotEmpty(livrosRetornadosDoService);
            Assert.Equal(livroEsperadoDoService.Count, livrosRetornadosDoService.Count);
            Assert.Contains("Livro", livrosRetornadosDoService[0].Titulo);
            Assert.Contains("Livro", livrosRetornadosDoService[1].Titulo);
        }

        [Fact(DisplayName = "DADO o Nome de livros não existentes na base de dados QUANDO chamamos o método GetByNome da controller ENTÃO chamar o service e o status code 404 deve ser retornado com uma lista vazia.")]
        public async void GetByNome_ChamaFindLivroByNomeAsync_StatusCode404RetornadoComUmaListaVazia()
        {
            //ARRANGE
            var livroEsperadoDoService = new List<LivroDto> { };

            mockService.Setup(s => s.FindLivroByNomeAsync("Livro")).ReturnsAsync(livroEsperadoDoService);

            //ACT
            var result = await controller.GetByNome("Livro");

            //ASSERT
            mockService.Verify(s => s.FindLivroByNomeAsync("Livro"), Times.Once);

            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, objectResult.StatusCode);

            var listaRetornada = Assert.IsType<List<LivroDto>>(objectResult.Value);
            Assert.Empty(listaRetornada);
        }

        [Fact(DisplayName = "DADO o Autor de livros já existentes na base de dados QUANDO chamamos o método GetByAutor da controller ENTÃO chamar o service e o status code 200 deve ser retornado com a lista de livros que combinam com o Autor dado.")]
        public async void GetByAutor_ChamarFindLivroByAutorAsync_StatusCode200RetornadoComLivrosEncontrados()
        {
            //ARRANGE
            var livroEsperadoDoService = new List<LivroDto>
            {
                new LivroDto { Titulo = "Livro 1", Sinopse = "Sinopse Livro 1", Publicacao = new DateTime(1990, 10, 08), Autor = "Autor Livro 1-2" },
                new LivroDto { Titulo = "Livro 2", Sinopse = "Sinopse Livro 2", Publicacao = new DateTime(1990, 10, 08), Autor = "Autor Livro 1-2" }
            };

            mockService.Setup(s => s.FindLivroByAutorAsync("Autor Livro 1-2")).ReturnsAsync(livroEsperadoDoService);

            //ACT
            var result = await controller.GetByAutor("Autor Livro 1-2");

            //ASSERT
            mockService.Verify(s => s.FindLivroByAutorAsync("Autor Livro 1-2"), Times.Once);

            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);

            var livrosRetornadosDoService = Assert.IsType<List<LivroDto>>(objectResult.Value);
            Assert.NotEmpty(livrosRetornadosDoService);
            Assert.Equal(livroEsperadoDoService.Count, livrosRetornadosDoService.Count);
            Assert.Equal("Autor Livro 1-2", livrosRetornadosDoService[0].Autor);
            Assert.Equal("Autor Livro 1-2", livrosRetornadosDoService[1].Autor);
        }

        [Fact(DisplayName = "DADO o Autor de livros não existentes na base de dados QUANDO chamamos o método GetByAutor da controller ENTÃO chamar o service e o status code 404 deve ser retornado com uma lista vazia.")]
        public async void GetByAutor_ChamarFindLivroByAutorAsync_StatusCode404RetornadoComUmaListaVazia()
        {
            //ARRANGE
            var livroEsperadoDoService = new List<LivroDto> { };

            mockService.Setup(s => s.FindLivroByAutorAsync("Autor Livro 1-2")).ReturnsAsync(livroEsperadoDoService);

            //ACT
            var result = await controller.GetByAutor("Autor Livro 1-2");

            //ASSERT
            mockService.Verify(s => s.FindLivroByAutorAsync("Autor Livro 1-2"), Times.Once);

            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, objectResult.StatusCode);

            var listaRetornada = Assert.IsType<List<LivroDto>>(objectResult.Value);
            Assert.Empty(listaRetornada);
        }

        [Fact(DisplayName = "DADO um livro válido QUANDO chamamos o método Post da controller ENTÃO chamar o service para realizar a inserção e o staus code 201 deve ser retornado com o livro recem inserido.")]
        public async void Post_LivroValidoChamarAddLivroAsync_StatusCode201RetornadoComLivroInserido()
        {
            //ARRANGE
            var livroValido = new NewLivroDto { Titulo = "Livro ABC", Sinopse = "Sinopse Livro ABC", Publicacao = new DateTime(1990, 10, 08), Autor = "Autor Livro ABC" };
            var livroEsperado = new LivroDto { LivroId = 1, Titulo = livroValido.Titulo, Sinopse = livroValido.Sinopse, Publicacao = livroValido.Publicacao, Autor = livroValido.Autor };
            
            mockService.Setup(s => s.AddLivroAsync(livroValido)).ReturnsAsync(livroEsperado);

            //ACT
            var result = await controller.Post(livroValido);

            //ASSERT
            mockService.Verify(s => s.AddLivroAsync(livroValido), Times.Once);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, objectResult.StatusCode);

            var livroRetornado = Assert.IsType<LivroDto>(objectResult.Value);
            Assert.NotNull(livroRetornado);
            Assert.Equal(livroEsperado.LivroId, livroRetornado.LivroId);
        }

        [Fact(DisplayName = "DADO um livro inválido QUANDO chamamos o método Post da controller ENTÃO chamar o service e o staus code 400 deve ser retornado.")]
        public async void Post_LivroInvalidoChamarAddLivroAsync_StatusCode400Retornado()
        {
            //ARRANGE
            var livroInvalido = new NewLivroDto { Titulo = "Liv", Sinopse = "Sinopse", Publicacao = new DateTime(1990, 10, 08), Autor = "Au" };

            mockService.Setup(s => s.AddLivroAsync(livroInvalido)).ReturnsAsync(value: null);

            //ACT
            var result = await controller.Post(livroInvalido);

            //ASSERT
            mockService.Verify(s => s.AddLivroAsync(livroInvalido), Times.Once);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, objectResult.StatusCode);
        }

        [Fact(DisplayName = "DADO um livro válido mas com Id e LivroId divergentes QUANDO chamamos o método Put da controller ENTÃO não chamar service para realizar a alteração e o staus code 400 deve ser retornado com uma mensagem de erro.")]
        public async void Put_LivroValidoChamarUpdateLivroAsyncComLivroIdDiferenteDeId_StatusCode400RetornadoComMensagemDeErro()
        {
            //ARRANGE
            var livroValido = new LivroDto { LivroId = 1, Titulo = "Livro ABCD", Sinopse = "Sinopse Livro ABCD", Publicacao = new DateTime(1990, 10, 08), Autor = "Autor Livro ABCD" };

            //ACT
            var result = await controller.Put(2, livroValido);

            //Assert
            mockService.Verify(s => s.FindLivroById(It.IsAny<int>()), Times.Never);
            mockService.Verify(s => s.UpdateLivroAsync(It.IsAny<LivroDto>()), Times.Never);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, objectResult.StatusCode);

            var mensagemErro = Assert.IsType<string>(objectResult.Value);
            Assert.Equal("LivroId do livro é diferente do id passado.", mensagemErro);
        }

        [Fact(DisplayName = "DADO um livro inválido mas com Id e LivroId divergentes QUANDO chamamos o método Put da controller ENTÃO não chamar service para realizar a alteração e o staus code 400 deve ser retornado com uma mensagem de erro.")]
        public async void Put_LivroInvalidoChamarUpdateLivroAsyncComLivroIdDiferenteDeId_StatusCode400RetornadoComMensagemDeErro()
        {
            //ARRANGE
            var livroValido = new LivroDto { LivroId = 1, Titulo = "", Sinopse = "", Publicacao = new DateTime(1990, 10, 08), Autor = "" };

            //ACT
            var result = await controller.Put(2, livroValido);

            //Assert
            mockService.Verify(s => s.FindLivroById(It.IsAny<int>()), Times.Never);
            mockService.Verify(s => s.UpdateLivroAsync(It.IsAny<LivroDto>()), Times.Never);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, objectResult.StatusCode);

            var mensagemErro = Assert.IsType<string>(objectResult.Value);
            Assert.Equal("LivroId do livro é diferente do id passado.", mensagemErro);
        }

        [Fact(DisplayName = "DADO um livro válido com LivroId e Id iguais QUANDO chamamos o método Put da controller ENTÃO chamar o service para realizar a alteração e o staus code 204 deve ser retornado.")]
        public async void Put_LivroValidoChamarUpdateLivroAsyncComLivroIdIgualAoId_StatusCode204Retornado()
        {
            //ARRANGE
            var livroValido = new LivroDto { LivroId = 1, Titulo = "Livro ABCD", Sinopse = "Sinopse Livro ABCD", Publicacao = new DateTime(1990, 10, 08), Autor = "Autor Livro ABCD" };

            mockService.Setup(s => s.FindLivroById(1)).ReturnsAsync(livroValido);
            mockService.Setup(s => s.UpdateLivroAsync(livroValido)).ReturnsAsync(livroValido);

            //ACT
            var result = await controller.Put(livroValido.LivroId, livroValido);

            //Assert
            mockService.Verify(s => s.FindLivroById(1), Times.Once);
            mockService.Verify(s => s.UpdateLivroAsync(livroValido), Times.Once);

            var objectResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, objectResult.StatusCode);
        }

        [Fact(DisplayName = "DADO um livro inválido com LivroId e Id iguais QUANDO chamamos o método Put da controller ENTÃO chamar o service que deve retornar null e o staus code 400 deve ser retornado pela controller.")]
        public async void Put_LivroInvalidoChamarUpdateLivroAsyncComLivroIdIgualId_StatusCode404Retornado()
        {
            //ARRANGE
            var livroInvalido = new LivroDto { LivroId = 1, Titulo = "", Sinopse = "", Publicacao = new DateTime(1990, 10, 08), Autor = "" };

            mockService.Setup(s => s.FindLivroById(1)).ReturnsAsync(livroInvalido);
            mockService.Setup(s => s.UpdateLivroAsync(livroInvalido)).ReturnsAsync(value: null);

            //ACT
            var result = await controller.Put(livroInvalido.LivroId, livroInvalido);

            //Assert
            mockService.Verify(s => s.FindLivroById(1), Times.Once);
            mockService.Verify(s => s.UpdateLivroAsync(livroInvalido), Times.Once);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, objectResult.StatusCode);
        }

        [Fact(DisplayName = "DADO um livro válido com LivroId e Id iguais mas sem existir na base de dados QUANDO chamamos o método Put da controller ENTÃO chamar apenas o service FindLivroById e o staus code 400 deve ser retornado com uma mensagem de erro.")]
        public async void Put_LivroValidoChamarUpdateLivroAsyncComLivroIdIgualAoIdMasNaoExistente_StatusCode400RetornadoComMensagemDeErro()
        {
            //ARRANGE
            var livroValido = new LivroDto { LivroId = 1, Titulo = "Livro ABCD", Sinopse = "Sinopse Livro ABCD", Publicacao = new DateTime(1990, 10, 08), Autor = "Autor Livro ABCD" };

            mockService.Setup(s => s.FindLivroById(1)).ReturnsAsync(value: null);

            //ACT
            var result = await controller.Put(livroValido.LivroId, livroValido);

            //Assert
            mockService.Verify(s => s.FindLivroById(1), Times.Once);
            mockService.Verify(s => s.UpdateLivroAsync(livroValido), Times.Never);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, objectResult.StatusCode);

            var mensagemErro = Assert.IsType<string>(objectResult.Value);
            Assert.Equal("Não há nenhum livro com este id na base de dados.", mensagemErro);
        }

        [Fact(DisplayName = "DADO um livro inválido com LivroId e Id iguais mas sem existir na base de dados QUANDO chamamos o método Put da controller ENTÃO chamar apenas o service FindLivroById e o staus code 400 deve ser retornado com uma mensagem de erro.")]
        public async void Put_LivroInvalidoChamarUpdateLivroAsyncComLivroIdIgualAoIdMasNaoExistente_StatusCode400RetornadoComMensagemDeErro()
        {
            //ARRANGE
            var livroInvalido = new LivroDto { LivroId = 1, Titulo = "", Sinopse = "", Publicacao = new DateTime(1990, 10, 08), Autor = "" };

            mockService.Setup(s => s.FindLivroById(1)).ReturnsAsync(value: null);

            //ACT
            var result = await controller.Put(livroInvalido.LivroId, livroInvalido);

            //Assert
            mockService.Verify(s => s.FindLivroById(1), Times.Once);
            mockService.Verify(s => s.UpdateLivroAsync(livroInvalido), Times.Never);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, objectResult.StatusCode);

            var mensagemErro = Assert.IsType<string>(objectResult.Value);
            Assert.Equal("Não há nenhum livro com este id na base de dados.", mensagemErro);
        }
    }
}
