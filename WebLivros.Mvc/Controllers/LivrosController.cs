using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebLivros.Mvc.Models;

namespace WebLivros.Mvc.Controllers
{
    public class LivrosController : Controller
    {
        private readonly IHttpClientFactory _http;

        public LivrosController(IHttpClientFactory http)
        {
            _http = http;
        }

        // GET: LivrosController
        // Esta Action servirá tanto para a listagem geral dos livros quanto para as buscas mais detalhadas
        public async Task<ActionResult> Index(string busca = null)
        {
            //Criamos a instância de um HttpCliet através do IHttpClientFactory passando o nome lógico 'livros' definido na Startup.cs
            var client = _http.CreateClient("livros");

            //É criada a lista de livros que será preenchida pelos livros retornadas da API, caso haja algum é claro. 
            IEnumerable<LivroViewModel> livros = new List<LivroViewModel>();

            //É criada uma variável do tipo HttpResponseMessage para receber as respostas das requisições Http
            HttpResponseMessage response;


            //Primeiro é verificado o valor da string 'busca'. Null é o valor padrão
            //Caso o valor não seja null ou empty, uma busca detalhada será solicitada
            if (!string.IsNullOrEmpty(busca))
            {

                //Aqui é feita uma requisição para retornar livros pelo título 
                response = await client.GetAsync($"Livros/Titulo/{busca}");

                //Aqui é verificado se a requisição foi um sucesso, ou seja, se algum livro foi encontrado
                if (response.IsSuccessStatusCode)
                {
                    //Caso haja algum livro, ele será passado para a lista
                    livros = response.Content.ReadFromJsonAsync<IEnumerable<LivroViewModel>>().Result;
                }
                else
                {
                    //Caso a busca por nome falhe, será feita uma requisição para buscar por autor
                    response = await client.GetAsync($"Livros/Autor/{busca}");
                    if (response.IsSuccessStatusCode)
                    {
                        livros = response.Content.ReadFromJsonAsync<IEnumerable<LivroViewModel>>().Result;
                    }
                }
            }
            else
            {
                //Se a string 'busca' for null ou empty, uma requisição para recuperar todos os livros será feita
                response = await client.GetAsync("Livros");

                if (response.IsSuccessStatusCode)
                {
                    livros = response.Content.ReadFromJsonAsync<IEnumerable<LivroViewModel>>().Result;
                }
            }

            return View(livros);
        }

        // GET: LivrosController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var client = _http.CreateClient("livros");

            var livro = await client.GetFromJsonAsync<LivroViewModel>($"Livros/{id}");

            return View(livro);
        }

        // GET: LivrosController/CreateOrUpdate
        public async Task<ActionResult> CreateOrUpdate(int id = 0)
        {
            //Caso o id seja 0, que é o valor padrão, a view será retornada com os campos vazios, prontos para a inserção de um novo livro
            if(id == 0)
            {
                return View();
            }
            else
            {
                //Caso contrário, um HttpClient é instanciado para solicitar uma requisição à API
                var client = _http.CreateClient("livros");

                //A API deve retornar o livro com o respectivo Id
                var livro = await client.GetFromJsonAsync<LivroViewModel>($"Livros/{id}");

                //A view será retornada com os campos preenchidos, prontos para uma atualização
                return View(livro);
            }
                
        }

        // POST: LivrosController/CreateOrUpdate
        //Esta Action servirá tanto para a Criação quanto para a Atualização de um livro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateOrUpdate(LivroViewModel livro)
        {
            var client = _http.CreateClient("livros");

            //Caso o LivroId seja 0, trata-se de uma inserção
            if (livro.LivroId == 0)
            {
                try
                {
                    var response = await client.PostAsJsonAsync("Livros", livro);
                }
                catch
                {
                    return View();
                }
            }
            //Caso contrário, trata-se de uma atualização
            else
            {
                try
                {
                    var response = await client.PutAsJsonAsync($"Livros/{livro.LivroId}", livro);
                }
                catch
                {
                    return View();
                }
            }

            return RedirectToAction("Index");
        }
    }
}
