using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLivros.Core.DTO;

namespace WebLivros.Core.ValidationAttributes
{
    public class LivroDto_GarantirLivroIdMaiorQueZero : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var livroDto = validationContext.ObjectInstance as LivroDto;

            if(!livroDto.ValidarLivroId())
            {
                return new ValidationResult("LivroId precisa ser maior que 0.");
            }

            return ValidationResult.Success;
        }
    }
}
