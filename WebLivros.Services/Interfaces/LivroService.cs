using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLivros.Core.DTO;
using WebLivros.Core.Entities;
using WebLivros.Core.Interfaces.Repository;
using WebLivros.Core.Interfaces.Services;

namespace WebLivros.Services.Interfaces
{
    public class LivroService : ILivroService
    {
        private IMapper _mapper;
        private ILivroRepository _repository;

        public LivroService(IMapper mapper, ILivroRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<LivroDto> AddLivroAsync(NewLivroDto livroDto)
        {
            var livro = _mapper.Map<Livro>(livroDto);
            if (!livro.Validar())
            {
                return null;
            }

            return _mapper.Map<LivroDto>(await _repository.AddAsync(livro));
        }

        public async Task<IEnumerable<LivroDto>> FindAllLivrosAsync()
        {
            var livrosDto = _mapper.Map<IEnumerable<LivroDto>>(await _repository.FindAsync(l => true));

            return livrosDto;
        }

        public async Task<LivroDto> FindLivroById(int id)
        {
            var livroDto = _mapper.Map<LivroDto>(await _repository.FindIdAsync(id));

            return livroDto;
        }

        public async Task<IEnumerable<LivroDto>> FindLivroByAutorAsync(string autor)
        {
            var livroDto = _mapper.Map<IEnumerable<LivroDto>>(await _repository.FindAsync(l => l.Autor.Equals(autor)));

            return livroDto;
        }

        public async Task<IEnumerable<LivroDto>> FindLivroByNomeAsync(string nome)
        {
            var livroDto = _mapper.Map<IEnumerable<LivroDto>>(await _repository.FindAsync(l => l.Titulo.Contains(nome)));

            return livroDto;
        }

        public async Task<LivroDto> UpdateLivroAsync(LivroDto livroDto)
        {
            var livro = _mapper.Map<Livro>(livroDto);
            if (!livro.Validar())
            {
                return null;
            };

            return _mapper.Map<LivroDto>(await _repository.UpdateAsync(livro));
        }
    }
}
