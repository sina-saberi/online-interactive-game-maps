﻿using game_maps.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace game_maps.Infrastructure.IRepositories
{
    public interface IEfRepository<T> where T : BaseEntitie
    {
        Task<List<T>> ListAsync(CancellationToken cancellationToken = default);
        Task<List<T>> ListAsync(Expression<Func<T, object>> includes, CancellationToken cancellationToken = default);
        Task<List<T>> WhereAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default);
        Task<List<T>> WhereAsync(Expression<Func<T, bool>> condition, Expression<Func<T, object>> includes, CancellationToken cancellationToken = default);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> condition, Expression<Func<T, object>> include, CancellationToken cancellationToken = default);
        Task<T> SingleAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default);
        Task<T> SingleAsync(Expression<Func<T, bool>> condition, Expression<Func<T, object>> include, CancellationToken cancellationToken = default);
        Task<T> CreateAsync(T model, CancellationToken cancellationToken = default);
        Task<T> UpdateAsync(T model, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default);
        IQueryable<T> Queryable(CancellationToken cancellationToken = default);
    }
}
