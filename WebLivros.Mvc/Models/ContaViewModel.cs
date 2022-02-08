using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebLivros.Mvc.Models
{
    public class ContaViewModel
    {
        [Required(ErrorMessage = "O Campo Usuário precisa ser preenchido.")]
        [MinLength(3, ErrorMessage = "O Usuário precisa ter, no mínimo, três caracteres")]
        [Display(Name = "Usuário")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "O Campo Senha precisa ser preenchido.")]
        [MinLength(3, ErrorMessage = "A Senha precisa ter, no mínimo, três caracteres")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Display(Name = "Lembrar-me")]
        public bool RememberMe { get; set; }
    }
}
