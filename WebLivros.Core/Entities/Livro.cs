using FluentValidation;
using FluentValidation.Results;
using System;

namespace WebLivros.Core.Entities
{
    public class Livro
    {
        public int LivroId { get; set; }
        public string Titulo { get; set; }
        public string Sinopse { get; set; }
        public DateTime Publicacao { get; set; }
        public string Autor { get; set; }
        public ValidationResult ValidationResult { get; set; }

        public Livro() {}

        public Livro(int id, string nome, string sinopse, DateTime publicacao, string autor)
        {
            LivroId = id;
            Titulo = nome;
            Sinopse = sinopse;
            Publicacao = publicacao;
            Autor = autor;
            ValidationResult = new ValidationResult();
        }

        public bool Validar()
        {
            ValidationResult = new LivroValido().Validate(this);
            return ValidationResult.IsValid;
        }

    }

    public class LivroValido : AbstractValidator<Livro>
    {
        private const int MAX_LENGTH_NOME = 100;
        private const int MIN_LENGTH_NOME = 5;
        private const int MAX_LENGTH_SINOPSE = 450;
        private const int MIN_LENGTH_SINOPSE = 10;
        public LivroValido()
        {
            RuleFor(l => l.Titulo)
                .NotEmpty().WithMessage("O Campo Nome é obrigatório.")
                .MinimumLength(MIN_LENGTH_NOME).WithMessage("O Campo Nome deve ter, no mínimo, 5 caracteres.")
                .MaximumLength(MAX_LENGTH_NOME).WithMessage("O Campo Nome deve ter, no máximo, 100 caracteres.");

            RuleFor(l => l.Sinopse)
                .NotEmpty().WithMessage("O Campo Sinopse é obrigatório.")
                .MinimumLength(MIN_LENGTH_SINOPSE).WithMessage("O Campo Sinopse deve ter, no mínimo, 10 caracteres.")
                .MaximumLength(MAX_LENGTH_SINOPSE).WithMessage("O Campo Sinopse deve ter, no máximo, 450 caracteres.");

            RuleFor(l => l.Autor)
                .NotEmpty().WithMessage("O Campo Autor é obrigatório.")
                .MinimumLength(MIN_LENGTH_NOME).WithMessage("O Campo Autor deve ter, no mínimo, 5 caracteres.")
                .MaximumLength(MAX_LENGTH_NOME).WithMessage("O Campo Autor deve ter, no máximo, 100 caracteres.");

            RuleFor(l => l.Publicacao)
                .NotEmpty().WithMessage("O Campo Publicação é obrigatório.");
        }
    }
}
