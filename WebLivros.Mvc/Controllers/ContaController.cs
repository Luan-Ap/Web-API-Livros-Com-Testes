using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebLivros.Mvc.Models;

namespace WebLivros.Mvc.Controllers
{
    public class ContaController : Controller
    {
        public IActionResult AcessoNegado()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        //Esta Action é a responsável pela realização do Login
        //Eu defini dois tipo de usuário. Um administrador e um visitante.
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(ContaViewModel conta)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            //Primeiro é verificado se o usuário é um admin
            if (conta.UserName == "admin.livros@api.com" && conta.Password == "souAdmin123")
            {
                //Caso seja, então são definidas as Claims(reivindicações) do usuário em questão
                var claims = new List<Claim>
                {
                    //Definimos:

                    //Nome
                    new Claim(ClaimTypes.Name, "admin"),
                    
                    //Email
                    new Claim(ClaimTypes.Email, "admin.livros@api.com"),
                    
                    //Role(função)
                    new Claim(ClaimTypes.Role, "Admin"),
                };

                //É criando um novo ClaimIdentity utilizando a lista de Claims e o AthenticationType definido na startup 
                var identity = new ClaimsIdentity(claims, "CookieLivros");

                //É inicializado um novo ClaimsPrincipal com a ClaimsIdentity criada
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                //É definida as propriedades de autenticação
                var authProperties = new AuthenticationProperties
                {
                    //Caso o usuário marque a opção Lembrar-Me, ao voltar ao site, mesmo que tenha fechado a janela, ele continuará logado, desde de que não faça o Logout
                    //Caso não marque a opção, ele precisará fazer Login toda vez que voltar ao site
                    IsPersistent = conta.RememberMe
                };


                //Por fim, utilizamos o HttpContext para realizar o login
                await HttpContext.SignInAsync("CookieLivros", claimsPrincipal, authProperties);

                return RedirectToAction("Index", "Home");
            }


            //Para o usuário visitante o processo é o mesmo, mudando apenas sua Claims
            if(conta.UserName == "visitante.livros@api.com" && conta.Password == "souVisitante123")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "visitante"),
                    new Claim(ClaimTypes.Email, "visitante.livros@api.com"),
                    new Claim(ClaimTypes.Role, "Visitante")
                };

                var identity = new ClaimsIdentity(claims, "CookieLivros");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = conta.RememberMe
                };

                await HttpContext.SignInAsync("CookieLivros", claimsPrincipal, authProperties);

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        //Esta Action é responsável pelo Logout
        public async Task<IActionResult> Logout()
        {
            //O HttpContext é chamado para realizar o logout do esquema de autenticação em queatão
            await HttpContext.SignOutAsync("CookieLivros");

            return RedirectToAction("Index", "Home");
        }
    }
}
