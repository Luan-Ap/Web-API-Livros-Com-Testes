using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLivros.Core.DTO;
using WebLivros.Core.Entities;

namespace WebLivros.Core.Interfaces.Services
{
    public interface ILivroService
    {
        Task<LivroDto> AddLivroAsync(NewLivroDto livroDto);

        Task<LivroDto> FindLivroById(int id);

        Task<IEnumerable<LivroDto>> FindLivroByNomeAsync(string nome);

        Task<IEnumerable<LivroDto>> FindLivroByAutorAsync(string autor);

        Task<IEnumerable<LivroDto>> FindAllLivrosAsync();

        Task<LivroDto> UpdateLivroAsync(LivroDto livroDto);
    }
}
