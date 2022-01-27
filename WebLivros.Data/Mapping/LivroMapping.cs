using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLivros.Core.Entities;

namespace WebLivros.Data.Mapping
{
    public class LivroMapping : IEntityTypeConfiguration<Livro>
    {
        public void Configure(EntityTypeBuilder<Livro> builder)
        {
            builder.HasKey(l => l.LivroId);

            builder.Property(l => l.Titulo)
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.Property(l => l.Sinopse)
                .HasColumnType("varchar(450)")
                .IsRequired();

            builder.Property(l => l.Autor)
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.Property(l => l.Publicacao)
                .HasColumnType("date")
                .IsRequired();

            builder.Ignore(l => l.ValidationResult);

            builder.ToTable("Livros");
        }
    }
}
