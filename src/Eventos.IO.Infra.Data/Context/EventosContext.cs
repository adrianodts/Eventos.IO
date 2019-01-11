using Eventos.IO.Domain.Eventos;
using Eventos.IO.Domain.Organizadores;
using Eventos.IO.Infra.Data.Extensions;
using Eventos.IO.Infra.Data.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Eventos.IO.Infra.Data.Context
{
    public class EventosContext : DbContext
    {
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Organizador> Organizadores { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }

        //public EventosContext(DbContextOptions dbContextOptions) : base (dbContextOptions)
        //{
            
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Fluent API

            modelBuilder.AddConfiguration(new EventoMapping());
            modelBuilder.AddConfiguration(new CategoriaMapping());
            modelBuilder.AddConfiguration(new EnderecoMapping());
            modelBuilder.AddConfiguration(new OrganizadorMapping());

            //CriarEventos(modelBuilder);
            //CriarEnderecos(modelBuilder);
            //CriarOrganizadores(modelBuilder);
            //CriarCategorias(modelBuilder);

            #endregion

            base.OnModelCreating(modelBuilder);
        }

        private static void CriarCategorias(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>()
                 .Ignore(e => e.ValidationResult);

            modelBuilder.Entity<Categoria>()
                .Ignore(e => e.CascadeMode);

            modelBuilder.Entity<Categoria>()
                .ToTable("Categorias");
        }

        private static void CriarOrganizadores(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Organizador>()
                .Ignore(e => e.ValidationResult);

            modelBuilder.Entity<Organizador>()
                .Ignore(e => e.CascadeMode);

            modelBuilder.Entity<Organizador>()
                .ToTable("Organizadores");
        }

        private static void CriarEnderecos(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Endereco>()
                .HasOne(e => e.Evento)
                .WithOne(e => e.Endereco)
                .HasForeignKey<Endereco>(e => e.EventoId)
                .IsRequired(false);

            modelBuilder.Entity<Endereco>()
                .Ignore(e => e.ValidationResult);

            modelBuilder.Entity<Endereco>()
                .Ignore(e => e.CascadeMode);

            modelBuilder.Entity<Endereco>()
                .ToTable("Enderecos");
        }

        private static void CriarEventos(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Evento>()
                .Property(e => e.Nome).HasColumnType("varchar(150)").IsRequired();

            modelBuilder.Entity<Evento>()
                .Property(e => e.DescricaoCurta).HasColumnType("varchar(150)");

            modelBuilder.Entity<Evento>()
                .Property(e => e.DescricaoLonga).HasColumnType("varchar(max)");

            modelBuilder.Entity<Evento>()
                .Property(e => e.NomeEmpresa).HasColumnType("varchar(150)").IsRequired();

            modelBuilder.Entity<Evento>()
                .Ignore(e => e.ValidationResult);

            modelBuilder.Entity<Evento>()
                .Ignore(e => e.Tags);

            modelBuilder.Entity<Evento>()
                .Ignore(e => e.CascadeMode);

            modelBuilder.Entity<Evento>()
                .ToTable("Eventos");


            modelBuilder.Entity<Evento>()
                .HasOne(e => e.Organizador)
                .WithMany(e => e.Eventos)
                .HasForeignKey(e => e.OrganizadorId);

            modelBuilder.Entity<Evento>()
                .HasOne(e => e.Categoria)
                .WithMany(e => e.Eventos)
                .HasForeignKey(e => e.CategoriaId)
                .IsRequired(false);
        }
    }
}
