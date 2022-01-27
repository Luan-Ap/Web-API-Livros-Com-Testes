using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WebLivros.Core.DTO;
using WebLivros.Core.Entities;
using WebLivros.Core.Interfaces.Repository;
using WebLivros.Services.AutoMapper;
using WebLivros.Services.Interfaces;
using Xunit;

namespace WebLivros.Services.UnitTests
{
    public class LivroServiceTests
    {
        private LivroService service;
        private Mock<ILivroRepository> mockRepository;
        private IMapper mapper;

        public LivroServiceTests()
        {
            mockRepository = new Mock<ILivroRepository>();

            var config = new MapperConfiguration(c => c.AddProfile<WebLivrosApiMappingProfile>());
            mapper = config.CreateMapper();

            service = new LivroService(mapper, mockRepository.Object);
        }

        [Fact(DisplayName = "DADO um livro válido QUANDO inserimos ENTÃO chamar o repositório para persistir e retorna o livro com deu LivroId.")]
        public async void AddAsync_LivroValido_LivroRepositoryDeveSerChamado()
        {
            //ARRANGE
            var dto = new NewLivroDto { Titulo = "Livro 1", Sinopse = "Sinopse Livro 1", Publicacao = new DateTime(1990, 10, 08), Autor = "Autor Livro 1" };

            mockRepository.Setup(r => r.AddAsync(It.IsAny<Livro>())).ReturnsAsync(new Livro(1, dto.Titulo, dto.Sinopse, dto.Publicacao, dto.Autor));
            //ACT
            var livro = await service.AddLivroAsync(dto);

            //ASSERT
            mockRepository.Verify(r => r.AddAsync(It.IsAny<Livro>()), Times.Once);
            Assert.NotNull(livro);
            Assert.Equal(1, livro.LivroId);
        }

        [Fact(DisplayName = "DADO um livro inválido QUANDO inserimos ENTÃO não chamar o repositório para persistir e retornar null.")]
        public async void AddAsync_LivroInvalido_LivroRepositoryNaoDeveSerChamado()
        {
            //ARRANGE
            var dto = new NewLivroDto { Titulo = "", Sinopse = "", Publicacao = new DateTime(1990, 10, 08), Autor = "" };

            //ACT
            var livro = await service.AddLivroAsync(dto);

            //ASSERT
            mockRepository.Verify(r => r.AddAsync(It.IsAny<Livro>()), Times.Never);
            Assert.Null(livro);
        }

        [Fact(DisplayName = "DADOS livros já inseridos QUANDO solicitamos uma busca ENTÃO chamar o repositório e retornar todos os livros.")]
        public async void FindAllLivrosAsync_LivroRepositoryDeveSerChamado_RetornarTodosOsLivros()
        {
            //ARRANGE
            var livrosRepository = new List<Livro>
            {
                new Livro (0, "Livro 1", "Sinopse Livro 1", new DateTime(1990, 10, 08), "Autor Livro 1"),
                new Livro (0, "Livro 2", "Sinopse Livro 2", new DateTime(2000, 10, 08), "Autor Livro 2")
            };

            mockRepository.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Livro, bool>>>())).ReturnsAsync(livrosRepository);

            //ACT
            var livrosService = await service.FindAllLivrosAsync();

            //ASSERT
            mockRepository.Verify(r => r.FindAsync(It.IsAny<Expression<Func<Livro, bool>>>()), Times.Once);
            Assert.NotSame(livrosRepository, livrosService);
            Assert.Equal(livrosRepository[0].Titulo, livrosService.First().Titulo);
            Assert.Equal(livrosRepository[1].Titulo, livrosService.Last().Titulo);
        }

        [Fact(DisplayName = "DADO o LivroId de um livro já existente na base de dados QUANDO solicitamos uma busca por LivroId ENTÃO chamar o repositório e retornar o livro que possui o LivroId passado")]
        public async void FindLivroById_LivroRepositoryDeveSerChamado_RetornarLivroEncontrado()
        {
            //ARRANGE
            var livroEsperado = new Livro(1, "Livro 1", "Sinopse Livro 1", new DateTime(1990, 10, 08), "Autor Livro 1");

            mockRepository.Setup(r => r.FindIdAsync(1)).ReturnsAsync(livroEsperado);

            //ACT
            var livroRetornado = await service.FindLivroById(1);

            //ASSERT
            mockRepository.Verify(r => r.FindIdAsync(1), Times.Once);
            Assert.NotNull(livroRetornado);
            Assert.Equal(livroEsperado.LivroId, livroRetornado.LivroId);
            Assert.Equal(livroEsperado.Titulo, livroRetornado.Titulo);
        }

        [Fact(DisplayName = "DADO o LivroId de um livro não existente na base de dados QUANDO solicitamos uma busca por LivroId ENTÃO chamar o repositório e retornar null")]
        public async void FindLivroById_LivroRepositoryDeveSerChamado_RetornarNull()
        {
            //ARRANGE
            mockRepository.Setup(r => r.FindIdAsync(1)).ReturnsAsync(value: null);

            //ACT
            var livroRetornado = await service.FindLivroById(1);

            //ASSERT
            mockRepository.Verify(r => r.FindIdAsync(1), Times.Once);
            Assert.Null(livroRetornado);
        }

        [Fact(DisplayName = "DADOS o Autor de livros já inseridos QUANDO solicitamos uma busca por Autor ENTÃO chamar o repositório e retornar todos os livros do mesmo Autor.")]
        public async void FindLivroByAutorAsync_LivroRepositoryDeveSerChamado_RetornarLivrosComMesmoAutor()
        {
            //ARRANGE
            var livrosRepository = new List<Livro>
            {
                new Livro (0, "Livro A", "Sinopse Livro A", new DateTime(1990, 10, 08), "Autor Livro AB"),
                new Livro (0, "Livro B", "Sinopse Livro B", new DateTime(2000, 10, 08), "Autor Livro AB")
            };

            mockRepository.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Livro, bool>>>())).ReturnsAsync(livrosRepository);

            //ACT
            var livrosService = await service.FindLivroByAutorAsync("Autor Livro AB");

            //ASSERT
            mockRepository.Verify(r => r.FindAsync(It.IsAny<Expression<Func<Livro, bool>>>()), Times.Once);
            Assert.NotSame(livrosRepository, livrosService);
            Assert.Equal(livrosRepository[0].Autor, livrosService.First().Autor);
            Assert.Equal(livrosRepository[1].Autor, livrosService.First().Autor);
        }

        [Fact(DisplayName = "DADOS o Autor de livros não inseridos QUANDO solicitamos uma busca por Autor ENTÃO chamar o repositório e retornar uma lista vazia.")]
        public async void FindLivroByAutorAsync_LivroRepositoryDeveSerChamado_RetornarListaVazia()
        {
            //ARRANGE
            var livrosRepository = new List<Livro> { };

            mockRepository.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Livro, bool>>>())).ReturnsAsync(livrosRepository);

            //ACT
            var livrosService = await service.FindLivroByAutorAsync("Autor Livro AB");

            //ASSERT
            mockRepository.Verify(r => r.FindAsync(It.IsAny<Expression<Func<Livro, bool>>>()), Times.Once);
            Assert.NotSame(livrosRepository, livrosService);
            Assert.Empty(livrosService);
        }

        [Fact(DisplayName = "DADOS o Nome de livros já inseridos QUANDO solicitamos uma busca por Nome ENTÃO chamar o repositório e retornar todos os livros com mesmo Nome ou similares.")]
        public async void FindLivroByNomeAsync_LivroRepositoryDeveSerChamado_RetornarLivrosComMesmoNome()
        {
            //ARRANGE
            var livrosRepository = new List<Livro>
            {
                new Livro (0, "Livro A", "Sinopse Livro A", new DateTime(1990, 10, 08), "Autor Livro AB"),
                new Livro (0, "Livro B", "Sinopse Livro B", new DateTime(2000, 10, 08), "Autor Livro AB")
            };

            mockRepository.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Livro, bool>>>())).ReturnsAsync(livrosRepository);

            //ACT
            var livrosService = await service.FindLivroByNomeAsync("Livro");

            //ASSERT
            mockRepository.Verify(r => r.FindAsync(It.IsAny<Expression<Func<Livro, bool>>>()), Times.Once);
            Assert.NotSame(livrosRepository, livrosService);
            Assert.Contains("Livro", livrosRepository[0].Titulo);
            Assert.Contains("Livro", livrosService.First().Titulo);
        }

        [Fact(DisplayName = "DADOS o Nome de livros não inseridos QUANDO solicitamos uma busca por Autor ENTÃO chamar o repositório e retornar uma lista vazia.")]
        public async void FindLivroByNomeAsync_LivroRepositoryDeveSerChamado_RetornarListaVazia()
        {
            //ARRANGE
            var livrosRepository = new List<Livro> { };

            mockRepository.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Livro, bool>>>())).ReturnsAsync(livrosRepository);

            //ACT
            var livrosService = await service.FindLivroByNomeAsync("Autor Livro AB");

            //ASSERT
            mockRepository.Verify(r => r.FindAsync(It.IsAny<Expression<Func<Livro, bool>>>()), Times.Once);
            Assert.NotSame(livrosRepository, livrosService);
            Assert.Empty(livrosService);
        }

        [Fact(DisplayName = "DADO um livro válido QUANDO atualizamos ENTÃO chamar o repositório para atualizar a base de dados e retorna o livro atualizado.")]
        public async void UpdateLivroAsync_LivroValido_LivroRepositoryDeveSerChamado()
        {
            //ARRANGE
            var dto = new LivroDto {LivroId = 1, Titulo = "Livro 2", Sinopse = "Sinopse Livro 2", Publicacao = new DateTime(1990, 10, 08), Autor = "Autor Livro 2" };

            mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Livro>())).ReturnsAsync(new Livro(1, dto.Titulo, dto.Sinopse, dto.Publicacao, dto.Autor));
            //ACT
            var livro = await service.UpdateLivroAsync(dto);

            //ASSERT
            mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Livro>()), Times.Once);
            Assert.NotNull(livro);
            Assert.Equal(1, livro.LivroId);
        }

        [Fact(DisplayName = "DADO um livro inválido QUANDO atualizamos ENTÃO não chamar o repositório para atualizar a base de dados e retornar null.")]
        public async void UpdateLivroAsync_LivroValido_LivroRepositoryNaoDeveSerChamado()
        {
            //ARRANGE
            var dto = new LivroDto {LivroId = 1, Titulo = "", Sinopse = "", Publicacao = new DateTime(1990, 10, 08), Autor = "" };

            //ACT
            var livro = await service.UpdateLivroAsync(dto);

            //ASSERT
            mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Livro>()), Times.Never);
            Assert.Null(livro);
        }
    }
}
