using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLivros.Core.Entities;
using WebLivros.Data.Mapping;

namespace WebLivros.Data.Context
{
    public class WebLivrosContext : DbContext
    {
        public DbSet<Livro> Livros { get; set; }

        public WebLivrosContext(DbContextOptions<WebLivrosContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LivroMapping());
            base.OnModelCreating(modelBuilder);
        }
    }
}
