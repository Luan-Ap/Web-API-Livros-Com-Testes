using System;
using System.ComponentModel.DataAnnotations;
using WebLivros.Core.ValidationAttributes;

namespace WebLivros.Core.DTO
{
    public class LivroDto
    {
        [LivroDto_GarantirLivroIdMaiorQueZero]
        public int LivroId { get; set; }

        [Required(ErrorMessage = "O Campo Nome é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O Campo Nome deve ter, no máximo, 100 caracteres.")]
        [MinLength(5, ErrorMessage = "O Campo Nome deve ter, no mínimo, 5 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "O Campo Sinopse é obrigatório.")]
        [MaxLength(450, ErrorMessage = "O Campo Sinopse deve ter, no máximo, 450 caracteres.")]
        [MinLength(10, ErrorMessage = "O Campo Sinopse deve ter, no mínimo, 10 caracteres.")]
        public string Sinopse { get; set; }

        [Required(ErrorMessage = "O Campo Publicação é obrigatório.")]
        [DataType(DataType.Date, ErrorMessage = "Informe um formato de Data Válido. Ex: 1990/08/11, 11/08/1990")]
        public DateTime Publicacao { get; set; }

        [Required(ErrorMessage = "O Campo Autor é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O Campo Autor deve ter, no máximo, 100 caracteres.")]
        [MinLength(5, ErrorMessage = "O Campo Author deve ter, no mínimo, 5 caracteres.")]
        public string Autor { get; set; }

        public bool ValidarLivroId()
        {
            if (LivroId <= 0)
            {
                return false;
            }

            return true;
        }
    }
}
