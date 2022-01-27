using AutoMapper;
using WebLivros.Core.DTO;
using WebLivros.Core.Entities;

namespace WebLivros.Services.AutoMapper
{
    public class WebLivrosApiMappingProfile : Profile
    {
        public WebLivrosApiMappingProfile()
        {
            CreateMap<NewLivroDto, Livro>()
                .ConstructUsing(dto => new Livro(0, dto.Titulo, dto.Sinopse, dto.Publicacao, dto.Autor));

            CreateMap<Livro, LivroDto>();

            CreateMap<LivroDto, Livro>();

        }
    }
}
