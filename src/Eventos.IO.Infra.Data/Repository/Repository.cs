using Eventos.IO.Domain.Core;
using Eventos.IO.Domain.Eventos.Repository;
using Eventos.IO.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Eventos.IO.Infra.Data.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity<TEntity>
    {
        public EventosContext _context { get; set; }
        public DbSet<TEntity> _dbSet { get; set; }

        protected Repository(EventosContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual void Adicionar(TEntity obj)
        {
            _dbSet.Add(obj);
        }

        public virtual void Atualizar(TEntity obj)
        {
            _dbSet.Update(obj);
        }

        public virtual IEnumerable<TEntity> Buscar(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.AsNoTracking().Where(predicate);
        }

        
        public virtual TEntity ObterPorId(Guid id)
        {
            return _dbSet.AsNoTracking().FirstOrDefault(t => t.Id == id);
        }

        public virtual IEnumerable<TEntity> ObterTodos()
        {
            return _dbSet.ToList();
        }

        public virtual void Remover(Guid id)
        {
            _dbSet.Remove(_dbSet.Find(id));
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public virtual void Dispose()
        {
            _context.Dispose();
        }
    }
}
