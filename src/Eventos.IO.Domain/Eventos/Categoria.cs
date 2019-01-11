using Eventos.IO.Domain.Core;
using System;
using System.Collections.Generic;

namespace Eventos.IO.Domain.Eventos
{
    public class Categoria : Entity<Categoria>
    {
        public string Nome{ get; private set; }

        //para o Entity Fw
        protected Categoria(){}

        public Categoria(Guid id)
        {
            Id = id;
        }
        
        //EF propriedade de navegação
        public virtual ICollection<Evento> Eventos { get; private set; }

        public override bool IsValid()
        {
            return true;        
        }
    }
}