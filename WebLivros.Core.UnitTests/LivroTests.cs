using System;
using System.Linq;
using WebLivros.Core.Entities;
using Xunit;

namespace WebLivros.Core.Testes
{
    public class LivroTests
    {
        const string NOME_VALIDO = "The Shining";
        const string AUTOR_VALIDO = "Stephen King";
        const string NOME_E_AUTOR_MAXLENGTH = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        const string SINOPSE_VALIDA = "A história se passa quase que inteiramente no Overlook, um hotel fictício mal-assombrado isolado nas rochosas do Colorado. A história do hotel é descrita durante o livro por vários personagens e inclui a morte de vários dos seus clientes e do ex-zelador de isolamento matando sua família e a si mesmo.";
        const string PUBLICACAO_VALIDA = "1977/01/28";
        const string SINOPOSE_MAX_LENGTH = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

        [Fact(DisplayName = "DADO um livro indevidamente instanciado QUANDO preenchemos seus campos ENTÃO a validação deve retornar false e com 6 de erros.")]
        public void Livro_IndevidamenteInstanciado_LivroInvalido()
        {
            //ARRANGE
            var livro = new Livro(0, "", "", DateTime.Parse(PUBLICACAO_VALIDA), "");

            //ACT
            var result = livro.Validar();

            //ASSERT
            Assert.False(result);
            Assert.NotNull(livro.ValidationResult.Errors);
            Assert.NotEmpty(livro.ValidationResult.Errors);
            Assert.Equal(6, livro.ValidationResult.Errors.Count);
        }

        [Fact(DisplayName = "DADO um livro devidamente instanciado QUANDO preenchemos seus campos ENTÃO a validação deve retornar true e sem erros.")]
        public void Livro_DevidamenteInstanciado_LivroValido()
        {
            //ARRANGE
            var livro = new Livro(0, NOME_VALIDO, SINOPSE_VALIDA, DateTime.Parse(PUBLICACAO_VALIDA), AUTOR_VALIDO);

            //ACT
            var result = livro.Validar();

            //ASSERT
            Assert.True(result);
            Assert.Empty(livro.ValidationResult.Errors);
        }

        [Fact(DisplayName = "DADO um nome vazio QUANDO preenchemos os campos ENTÃO a validação deve retornar false e com a mensagem: O Campo Nome é obrigatório.")]
        public void NomeLivro_Vazio_NomeInvalido()
        {
            //ARRANGE
            var livro = new Livro(0, "", SINOPSE_VALIDA, DateTime.Parse(PUBLICACAO_VALIDA), AUTOR_VALIDO);

            //ACT
            var result = livro.Validar();

            //ASSERT
            Assert.False(result);
            Assert.NotNull(livro.ValidationResult.Errors);
            Assert.NotEmpty(livro.ValidationResult.Errors);
            Assert.NotNull(livro.ValidationResult.Errors.FirstOrDefault(v => v.ErrorMessage == "O Campo Nome é obrigatório."));
        }

        [Fact(DisplayName = "DADO um nome com caracteres em excesso QUANDO preenchemos os campos ENTÃO a validação deve retornar false e com a mensagem: O Campo Nome deve ter, no máximo, 100 caracteres.")]
        public void NomeLivro_MaxLengthExcedido_NomeInvalido()
        {
            //ARRANGE
            var livro = new Livro(0, NOME_E_AUTOR_MAXLENGTH, SINOPSE_VALIDA, DateTime.Parse(PUBLICACAO_VALIDA), AUTOR_VALIDO);

            //ACT
            var result = livro.Validar();

            //ASSERT
            Assert.False(result);
            Assert.NotNull(livro.ValidationResult.Errors);
            Assert.NotEmpty(livro.ValidationResult.Errors);
            Assert.NotNull(livro.ValidationResult.Errors.FirstOrDefault(v => v.ErrorMessage == "O Campo Nome deve ter, no máximo, 100 caracteres."));
        }

        [Fact(DisplayName = "DADO um nome sem atingir a quantidade de caracteres mínimos QUANDO preenchemos os campos ENTÃO a validação deve retornar false e com a mensagem: O Campo Nome deve ter, no mínimo, 5 caracteres.")]
        public void NomeLivro_MinLengthNaoAtingido_NomeInvalido()
        {
            //ARRANGE
            var livro = new Livro(0, "aaaa", SINOPSE_VALIDA, DateTime.Parse(PUBLICACAO_VALIDA), AUTOR_VALIDO);

            //ACT
            var result = livro.Validar();

            //ASSERT
            Assert.False(result);
            Assert.NotNull(livro.ValidationResult.Errors);
            Assert.NotEmpty(livro.ValidationResult.Errors);
            Assert.NotNull(livro.ValidationResult.Errors.FirstOrDefault(v => v.ErrorMessage == "O Campo Nome deve ter, no mínimo, 5 caracteres."));
        }

        [Fact(DisplayName = "DADO uma sinopse vazio QUANDO preenchemos os campos ENTÃO a validação deve retornar false e com a mensagem: O Campo Sinopse é obrigatório.")]
        public void SinopseLivro_Vazio_SinopseInvalida()
        {
            //ARRANGE
            var livro = new Livro(0, NOME_VALIDO, "", DateTime.Parse(PUBLICACAO_VALIDA), AUTOR_VALIDO);

            //ACT
            var result = livro.Validar();

            //ASSERT
            Assert.False(result);
            Assert.NotNull(livro.ValidationResult.Errors);
            Assert.NotEmpty(livro.ValidationResult.Errors);
            Assert.NotNull(livro.ValidationResult.Errors.FirstOrDefault(v => v.ErrorMessage == "O Campo Sinopse é obrigatório."));
        }

        [Fact(DisplayName = "DADO uma sinopse com caracteres em excesso QUANDO preenchemos os campos ENTÃO a validação deve retornar false e com a mensagem: O Campo Sinopse deve ter, no máximo, 450 caracteres.")]
        public void SinopseLivro_MaxLengthExcedido_SinopseInvalida()
        {
            //ARRANGE
            var livro = new Livro(0, NOME_VALIDO, SINOPOSE_MAX_LENGTH,
                    DateTime.Parse(PUBLICACAO_VALIDA), AUTOR_VALIDO);

            //ACT
            var result = livro.Validar();

            //ASSERT
            Assert.False(result);
            Assert.NotNull(livro.ValidationResult.Errors);
            Assert.NotEmpty(livro.ValidationResult.Errors);
            Assert.NotNull(livro.ValidationResult.Errors.FirstOrDefault(v => v.ErrorMessage == "O Campo Sinopse deve ter, no máximo, 450 caracteres."));
        }

        [Theory(DisplayName = "DADO uma sinopse sem atingir a quantidade mínima de caracteres QUANDO preenchemos os campos ENTÃO a validação deve retornar false e com a mensagem: O Campo Sinopse deve ter, no mínimo, 10 caracteres.")]
        [InlineData("aaa")]
        [InlineData("aaaaa")]
        [InlineData("aaaa")]
        public void SinopseLivro_MinLengthNaoAtingido_SinopseInvalida(string sinopse)
        {
            //ARRANGE
            var livro = new Livro(0, NOME_VALIDO, sinopse, DateTime.Parse(PUBLICACAO_VALIDA), AUTOR_VALIDO);

            //ACT
            var result = livro.Validar();

            //ASSERT
            Assert.False(result);
            Assert.NotNull(livro.ValidationResult.Errors);
            Assert.NotEmpty(livro.ValidationResult.Errors);
            Assert.NotNull(livro.ValidationResult.Errors.FirstOrDefault(v => v.ErrorMessage == "O Campo Sinopse deve ter, no mínimo, 10 caracteres."));
        }

        [Fact(DisplayName = "DADO um autor vazio QUANDO preenchemos os campos ENTÃO a validação deve retornar false e com a mensagem: O Campo Autor é obrigatório.")]
        public void AutorLivro_Vazio_AutorInvalido()
        {
            //ARRANGE
            var livro = new Livro(0, NOME_VALIDO, SINOPSE_VALIDA, DateTime.Parse(PUBLICACAO_VALIDA), "");

            //ACT
            var result = livro.Validar();

            //ASSERT
            Assert.False(result);
            Assert.NotNull(livro.ValidationResult.Errors);
            Assert.NotEmpty(livro.ValidationResult.Errors);
            Assert.NotNull(livro.ValidationResult.Errors.FirstOrDefault(v => v.ErrorMessage == "O Campo Autor deve ter, no mínimo, 5 caracteres."));
        }

        [Fact(DisplayName = "DADO um autor com caracteres em excesso QUANDO preenchemos os campos ENTÃO a validação deve retornar false e com a mensagem: O Campo Autor deve ter, no máximo, 100 caracteres.")]
        public void AutorLivro_MaxLengthExcedido_AutorInvalido()
        {
            //ARRANGE
            var livro = new Livro(0, NOME_VALIDO, SINOPSE_VALIDA, DateTime.Parse(PUBLICACAO_VALIDA), NOME_E_AUTOR_MAXLENGTH);

            //ACT
            var result = livro.Validar();

            //ASSERT
            Assert.False(result);
            Assert.NotNull(livro.ValidationResult.Errors);
            Assert.NotEmpty(livro.ValidationResult.Errors);
            Assert.NotNull(livro.ValidationResult.Errors.FirstOrDefault(v => v.ErrorMessage == "O Campo Autor deve ter, no máximo, 100 caracteres."));
        }

        [Fact(DisplayName = "DADO um autor sem atingir a quantidade mínima de caracteres QUANDO preenchemos os campos ENTÃO a validação deve retornar false e com a mensagem: O Campo Autoe deve ter, no mínimo, 5 caracteres.")]
        public void AutorLivro_MinLengthNaoAtingido_AutorInvalido()
        {
            //ARRANGE
            var livro = new Livro(0, NOME_VALIDO, SINOPSE_VALIDA, DateTime.Parse(PUBLICACAO_VALIDA), "aaaa");

            //ACT
            var result = livro.Validar();

            //ASSERT
            Assert.False(result);
            Assert.NotNull(livro.ValidationResult.Errors);
            Assert.NotEmpty(livro.ValidationResult.Errors);
            Assert.NotNull(livro.ValidationResult.Errors.FirstOrDefault(v => v.ErrorMessage == "O Campo Autor deve ter, no mínimo, 5 caracteres."));
        }
    }
}
