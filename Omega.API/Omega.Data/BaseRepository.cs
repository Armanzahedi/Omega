﻿using Microsoft.EntityFrameworkCore;
using Omega.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Omega.Infrastructure
{
    public interface IBaseRepository<T> where T : class, IBaseEntity
    {
        Task<List<T>> GetAll();
        Task<T> Get(int id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(int id);
        bool EntityExists(int id);
    }
    public abstract class BaseRepository<TEntity, TContext> : IBaseRepository<TEntity>
         where TEntity : class, IBaseEntity
         where TContext : DbContext
    {
        private readonly TContext context;
        public BaseRepository(TContext context)
        {
            this.context = context;
        }
        public async Task<TEntity> Add(TEntity entity)
        {
            entity.AddedDate = DateTime.Now;
            context.Set<TEntity>().Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> Delete(int id)
        {
            var entity = await context.Set<TEntity>().FindAsync(id);
            entity.Deleted = true;
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> Get(int id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            entity.ModifiedDate = DateTime.Now;
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return entity;
        }
        public bool EntityExists(int id)
        {
            var entity = context.Set<TEntity>().FindAsync(id);
            if (entity != null) return true;
            else return false;
        }
    }
}
