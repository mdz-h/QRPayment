using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;

namespace Oxxo.Cloud.Security.Infrastructure.Extensions
{
    public static class QueryExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        /// 
        public static T FirstWithNoLock<T>(this IQueryable<T> query, Expression<Func<T, bool>> expression = null, CancellationToken cancellationToken = default)
        {
            T result = default;
            using (new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                if (expression != null)
                {
                    query = query.Where(expression);
                }
                result = query.First();
            }
            return result;
        }
        public static T FirstOrDefaultWithNoLock<T>(this IQueryable<T> query, Expression<Func<T, bool>> expression = null, CancellationToken cancellationToken = default)
        {
            T result = default;
            using (new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                if (expression != null)
                {
                    query = query.Where(expression);
                }
                result = query.FirstOrDefault();
            }
            return result;
        }

        public static async Task<T> FirstOrDefaultWithNoLockAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>> expression = null, CancellationToken cancellationToken = default)
        {
            T result = default;
            using (new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                if (expression != null)
                {
                    query = query.Where(expression);
                }
                result = await query.FirstOrDefaultAsync(cancellationToken);
            }
            return result;
        }

        public static async Task<List<T>> ToListWithNoLockAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>> expression = null, CancellationToken cancellationToken = default)
        {
            List<T> result = default;
            using (new TransactionScope(
               TransactionScopeOption.Required,
               new TransactionOptions()
               {
                   IsolationLevel = IsolationLevel.ReadUncommitted
               },
               TransactionScopeAsyncFlowOption.Enabled))
            {
                if (expression != null)
                {
                    query = query.Where(expression);
                }
                result = await query.ToListAsync(cancellationToken);
            }
            return result;
        }

        public static bool AnyWithNoLock<T>(this IQueryable<T> query, Expression<Func<T, bool>> expression = null)
        {
            bool result = false;


            using (new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                if (expression != null)
                {
                    query = query.Where(expression);
                }
                result = query.Any();
            }
            return result;
        }

        public static async Task<bool> AnyWithNoLockAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>> expression = null, CancellationToken cancellationToken = default)
        {
            bool result = false;

            using (new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                if (expression != null)
                {
                    query = query.Where(expression);
                }

                result = await query.AnyAsync(cancellationToken);
            }

            return result;
        }
    }
}
