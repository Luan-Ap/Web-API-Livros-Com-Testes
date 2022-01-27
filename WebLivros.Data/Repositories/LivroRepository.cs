using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebLivros.Core.Entities;
using WebLivros.Core.Interfaces.Repository;
using WebLivros.Data.Context;

namespace WebLivros.Data.Repositories
{
    public class LivroRepository : ILivroRepository
    {
        private WebLivrosContext _context;
        public LivroRepository(WebLivrosContext context)
        {
            _context = context;
        }

        public async Task<Livro> AddAsync(Livro livro)
        {
            await _context.Livros.AddAsync(livro);
            await _context.SaveChangesAsync();

            return livro;
        }

        public async Task<IEnumerable<Livro>> FindAsync(Expression<Func<Livro, bool>> expression)
        {
            return await _context.Livros.AsNoTracking().Where(expression).ToListAsync();
        }

        public async Task<Livro> FindIdAsync(int id)
        {
            return await _context.Livros.AsNoTracking().Where(l => l.LivroId == id).FirstOrDefaultAsync();
        }

        public async Task<Livro> UpdateAsync(Livro livro)
        {
            _context.Entry(livro).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return livro;
        }
    }
}
