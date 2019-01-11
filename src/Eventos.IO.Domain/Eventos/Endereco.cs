using System;
using FluentValidation;
using Eventos.IO.Domain.Core;

namespace Eventos.IO.Domain.Eventos
{
    public class Endereco : Entity<Endereco>
    {
        public string Logradouro { get; private set; }
        public string Numero { get; private set; }
        public string Complemento { get; private set; }
        public string Bairro { get; private set; }
        public string CEP { get; private set; }
        public string Cidade { get; private set; }
        public string Estado { get; private set; }
        public Guid? EventoId { get; set; }

        public virtual Evento Evento { get; private set; }

        public Endereco(Guid id, string logradouro,  string numero, string complemento, string bairro, 
            string cep, string cidade, string estado, Guid eventoId)
        {
            Id = id;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            CEP = cep;
            Cidade = cidade;
            Estado = estado;
            EventoId = eventoId;
        }

        protected Endereco() { }


        public override bool IsValid()
        {
            RuleFor(c => c.Logradouro)
                .NotNull().WithMessage("O Logradouro precisa ser preenchido")
                .Length(2, 150).WithMessage("O Logradouro precisa ter entre 2 e 150 caracteres");

            RuleFor(c => c.Bairro)
                .NotNull().WithMessage("O Bairro precisa ser preenchido")
                .Length(2, 150).WithMessage("O Bairro precisa ter entre 2 e 150 caracteres");

            RuleFor(c => c.CEP)
                .NotNull().WithMessage("O CEP precisa ser preenchido")
                .Length(8).WithMessage("O CEP precisa ter 8 caracteres");

            RuleFor(c => c.Cidade)
                .NotNull().WithMessage("O Cidade precisa ser preenchido")
                .Length(2, 150).WithMessage("O Cidade precisa ter entre 2 e 150 caracteres");

            RuleFor(c => c.Estado)
                .NotNull().WithMessage("O Estado precisa ser preenchido")
                .Length(2, 150).WithMessage("O Estado precisa ter entre 2 e 150 caracteres");

            RuleFor(c => c.Numero)
                .NotNull().WithMessage("O Numero precisa ser preenchido")
                .Length(2, 10).WithMessage("O Numero precisa ter entre 2 e 10 caracteres");

            ValidationResult = Validate(this);

            return ValidationResult.IsValid;
        }
    }
}