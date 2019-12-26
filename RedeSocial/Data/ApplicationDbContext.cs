using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RedeSocial.Domain;

namespace RedeSocial.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext()            
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BemTeVi;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Perfil>().ToTable("Perfil");
            builder.Entity<Mensagem>().ToTable("Mensagem");
            builder.Entity<Convite>().ToTable("Convite");
        }

        public DbSet<Perfil> Perfis { get; set; }
        public DbSet<Mensagem> Mensagens { get; set; }
        public DbSet<Convite> Convites { get; set; }
    }
}
