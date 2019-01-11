using Eventos.IO.Domain.Eventos;
using Eventos.IO.Domain.Eventos.Repository;
using Eventos.IO.Domain.Interfaces;
using Eventos.IO.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eventos.IO.Infra.Data.Repository
{
    public class EventosRepository : Repository<Evento>, IEventoRepository
    {
        public EventosRepository(EventosContext context) : base (context){}

        public void AdicionarEndereco(Endereco endereco)
        {
            _context.Enderecos.Add(endereco);
        }

        public void AtualizarEndereco(Endereco endereco)
        {
            _context.Enderecos.Update(endereco);
        }

        public Endereco ObterEnderecoPorId(Guid Id)
        {
            return _context.Enderecos.Find(Id);
        }

        public IEnumerable<Evento> ObterEventoPorOrganizador(Guid organizadorId)
        {
            return _context.Eventos.Where(e => e.OrganizadorId == organizadorId).ToList(); 
        }

        public override Evento ObterPorId(Guid id)
        {
            return _context.Eventos.Include(e => e.Endereco).FirstOrDefault(e => e.EnderecoId == id);
        }
    }
}
