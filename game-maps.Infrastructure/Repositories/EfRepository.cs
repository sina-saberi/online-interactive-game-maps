using game_maps.Core.Base;
using game_maps.Infrastructure.Contexts;
using game_maps.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace game_maps.Infrastructure.Repositories
{
    public class EfRepository<T> : IEfRepository<T> where T : BaseEntitie
    {
        private readonly GameMapsContext context;

        public EfRepository(GameMapsContext context)
        {
            this.context = context;
        }
        public async Task<List<T>> ListAsync(Expression<Func<T, object>> includes, CancellationToken cancellationToken = default)
        {
            return await context.Set<T>().Include(includes).ToListAsync(cancellationToken);
        }
        public async Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
        {
            var query = context.Set<T>();
            return await query.ToListAsync(cancellationToken);
        }
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default)
        {
            var res = await context.Set<T>().FirstOrDefaultAsync(condition, cancellationToken);
            return res!;
        }
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> condition, Expression<Func<T, object>> include, CancellationToken cancellationToken = default)
        {
            var res = await context.Set<T>().Include(include).FirstOrDefaultAsync(condition, cancellationToken);
            return res!;
        }
        public async Task<List<T>> WhereAsync(Expression<Func<T, bool>> condition, Expression<Func<T, object>> includes, CancellationToken cancellationToken = default)
        {
            var res = await context.Set<T>().Where(condition).Include(includes).ToListAsync();
            return res;
        }
        public async Task<List<T>> WhereAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default)
        {
            var res = await context.Set<T>().Where(condition).ToListAsync();
            return res;
        }
        public async Task<T> SingleAsync(Expression<Func<T, bool>> condition, Expression<Func<T, object>> include, CancellationToken cancellationToken = default)
        {
            var res = await context.Set<T>().Include(include).SingleAsync(condition, cancellationToken);
            return res!;
        }
        public async Task<T> SingleAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default)
        {
            var res = await context.Set<T>().SingleAsync(condition, cancellationToken);
            return res;
        }
        public async Task<T> CreateAsync(T model, CancellationToken cancellationToken = default)
        {
            var res = await context.Set<T>().AddAsync(model);
            await context.SaveChangesAsync();
            return res.Entity;
        }
        public async Task<T> UpdateAsync(T model, CancellationToken cancellationToken = default)
        {
            var res = context.Set<T>().Update(model);
            await context.SaveChangesAsync();
            return res.Entity;
        }
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default)
        {
            var res = await context.Set<T>().AnyAsync(condition);
            return res;
        }
        public IQueryable<T> Queryable(CancellationToken cancellationToken = default)
        {
            return context.Set<T>().AsQueryable();
        }
    }
}
