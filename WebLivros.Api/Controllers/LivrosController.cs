using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebLivros.Core.DTO;
using WebLivros.Core.Interfaces.Services;

namespace WebLivros.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LivrosController : ControllerBase
    {
        private readonly ILivroService _service;

        public LivrosController(ILivroService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var livrosDto = await _service.FindAllLivrosAsync();

            return Ok(livrosDto);
        }

        [HttpGet]
        [Route("/api/v1/[controller]/LivroId/{id}")]
        public async Task<IActionResult> GetByLivroId(int id)
        {
            var livrosDto = await _service.FindLivroById(id);

            if (livrosDto is null)
            {
                return NotFound("Não há nenhum livro com este id na base de dados.");
            }

            return Ok(livrosDto);
        }

        [HttpGet("{nome}")]
        public async Task<IActionResult> GetByNome(string nome)
        {
            var livrosDto = await _service.FindLivroByNomeAsync(nome);

            if (!livrosDto.Any())
            {
                return NotFound("Ainda não há nenhum livro com este nome na base de dados.");
            }

            return Ok(livrosDto);
        }

        [HttpGet]
        [Route("/api/v1/[controller]/Autor/{autor}")]
        public async Task<IActionResult> GetByAutor(string autor)
        {
            var livrosDto = await _service.FindLivroByAutorAsync(autor);

            if (!livrosDto.Any())
            {
                return NotFound("Ainda não há nenhum livro escrito por este autor na base de dados.");
            }

            return Ok(livrosDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewLivroDto livroDto)
        {
            var retorno = await _service.AddLivroAsync(livroDto);

            if (retorno is null)
            {
                return BadRequest(livroDto);
            }

            return StatusCode(201, retorno);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] LivroDto livroDto)
        {
            if(id != livroDto.LivroId)
            {
                return BadRequest("LivroId do livro é diferente do id passado.");
            }

            if(await _service.FindLivroById(livroDto.LivroId) is null)
            {
                return BadRequest("Não há nenhum livro com este id na base de dados.");
            }

            var livro = await _service.UpdateLivroAsync(livroDto);

            if(livro is null)
            {
                return BadRequest(livroDto);
            }

            return NoContent();
        }
    }
}
