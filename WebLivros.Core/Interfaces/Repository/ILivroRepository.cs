using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebLivros.Core.Entities;

namespace WebLivros.Core.Interfaces.Repository
{
    public interface ILivroRepository
    {
        Task<Livro> AddAsync(Livro livro);

        Task<IEnumerable<Livro>> FindAsync(Expression<Func<Livro, bool>> expression);

        Task<Livro> FindIdAsync(int id);

        Task<Livro> UpdateAsync(Livro livro);
    }
}
