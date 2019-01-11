using System;
using System.Collections.Generic;
using System.Linq;
using Eventos.IO.Domain.Core;
using Eventos.IO.Domain.Organizadores;
using FluentValidation;

namespace Eventos.IO.Domain.Eventos
{
    public class Evento : Entity<Evento>
    {
        private Dictionary<string, string> ErrosValidacao;
        private Evento() { }

        public Evento(string nome, DateTime dataInicio, DateTime dataFim, 
            bool gratuito, decimal valor,  bool online, string nomeEmpresa)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            DataInicio = dataInicio;
            DataFim = dataFim;
            Gratuito = gratuito;
            Valor = valor;
            Online = online;
            NomeEmpresa = nomeEmpresa;



            ErrosValidacao = new Dictionary<string, string>();

            //Implentar FluentValidation ao invés do abaixo 

            //if (nome.Length < 3)
            //    ErrosValidacao.Add("Nome", "O nome não pode possuir menos de 3 caracteres");


            //if (valor > 0 && gratuito)
            //    ErrosValidacao.Add("Valor", "Não pode ser gratuito");

        }

        public string Nome { get; private set; }
        public string DescricaoCurta { get; private set; }
        public string DescricaoLonga { get; private set; }
        public DateTime DataInicio { get; private set; }
        public DateTime DataFim { get; private set; }
        public bool Gratuito { get; private set; }
        public decimal Valor { get; private set; }
        public bool Online { get; private set; }
        public string NomeEmpresa { get; private set; }
        public bool Excluido { get; private set; }  
        public ICollection<Tags> Tags { get; private set; }

        //Propriedades de navegação 
        public virtual Categoria Categoria { get; private set; }
        public virtual Endereco Endereco { get; private set; }
        public virtual Organizador Organizador { get; private set; }
        
        public Guid? CategoriaId { get; private set; }
        public Guid? EnderecoId { get; private set; }
        public Guid OrganizadorId { get; private set; }

        public void AtribuirEndereco(Endereco endereco)
        {
            if (!endereco.IsValid()) return;

            Endereco = endereco;
        }

        public void AtribuirCategoria(Categoria categoria)
        {
            if (!categoria.IsValid()) return;

            Categoria = categoria;
        }
        
        public void ExcluirEvento(Guid Id)
        {
            // TODO: Deve validar alguma regra?
            Excluido = true;
        }

        public override bool IsValid()
        {
            Validar();
            return ValidationResult.IsValid;
        }

        private void Validar()
        {
            ValidarNome();
            ValidarValor();
            ValidarData();
            ValidarLocal();
            ValidarNomeEmpresa();
            ValidationResult = Validate(this);

            //Validações adicionais

            ValidarEndereco();
        }

        private void ValidarNome()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("O nome não pode ser vazio")
                .Length(2, 150).WithMessage("O nome precisa ter entre 2 e 150 caracteres");
        }

        private void ValidarValor()
        {
            if(!Gratuito)
                RuleFor(c => c.Valor)
                    .ExclusiveBetween(1, 50000).WithMessage("O valor precisa ter entre 1 e 50.000");

            if (Gratuito)
                RuleFor(c => c.Valor)
                    .ExclusiveBetween(0, 0).When(e => e.Gratuito)
                    .WithMessage("O valor não deve ser diferente de 0 para um evento gratuito");
        }

        private void ValidarData()
        {
            RuleFor(c => c.DataInicio)
                .GreaterThan(e => e.DataFim)
                .WithMessage("A data de inicio deve ser maior que a data final do evento");

            RuleFor(c => c.DataInicio)
                 .LessThan(e => DateTime.Now)
                 .WithMessage("A data de inicio não deve ser maior que a data atual");

        }

        private void ValidarLocal()
        {
            if(Online)
                RuleFor(c => c.Endereco)
                    .Null().When(e => e.Online)
                    .WithMessage("O evento não pode possuir um endereço se for online");

            if (!Online)
                RuleFor(c => c.Endereco)
                    .NotNull().When(e => e.Online)
                    .WithMessage("O evento deve possuir um endereço");

        }

        private void ValidarNomeEmpresa()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("O nome do organizador precisa ser fornecido")
                .Length(2, 150).WithMessage("O nome do organizador precisa 2 e 150 caracteres");
        }

        private void ValidarEndereco()
        {
            if (Online) return;
            if (Endereco.IsValid()) return;

            foreach (var error in Endereco.ValidationResult.Errors)
            {
                ValidationResult.Errors.Add(error);
            }
        }

        public static class EventoFactory
        {
            public static Evento NovoEventoCompleto(Guid id, string nome, string descricaoCurta, string descricaoLonga, 
                DateTime dataInicio, DateTime dataFim, bool gratuito, decimal valor, bool online, string nomeEmpresa, 
                Guid? organizadorId, Endereco endereco, Guid categoriaId)
            {
                var evento = new Evento()
                {
                    Id = id,
                    Nome = nome,
                    DescricaoCurta = descricaoCurta,
                    DescricaoLonga = descricaoLonga,
                    DataInicio = dataInicio,
                    DataFim = dataFim,
                    Gratuito = gratuito,
                    Valor = valor,
                    Online = online,
                    NomeEmpresa = nomeEmpresa,
                    Endereco = endereco,
                    CategoriaId = categoriaId
                };

                if ( organizadorId != null || organizadorId.HasValue)
                    evento.OrganizadorId = organizadorId.Value;

                if (evento.Online)
                    evento.Endereco = null;


                return evento;
            }
        }
    }

    public class Test
    {
        public Test()
        {
            var evento = new Evento("Teste", DateTime.Now, DateTime.Now, 
                false, 150.00M, false, "Adriano Dantas");
            
        }
    }
}
