using System.Linq.Expressions;
using System.Numerics;
using vali_flow.Classes.Options;
using vali_flow.Classes.Results;
using vali_flow.Utils;

namespace vali_flow.Interfaces.Evaluators;

//29 
public interface IDatabaseEvaluator<T>
{
    Task<bool> EvaluateAsync(T entity);

    Task<bool> EvaluateAnyAsync<TProperty>(
        IQueryable<T> query,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default);

    Task<int> EvaluateCountAsync<TProperty>(
        IQueryable<T> query,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default);

    Task<T?> GetFirstFailedAsync<TProperty>(
        IQueryable<T> query,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default);

    Task<T?> GetFirstAsync<TProperty>(
        IQueryable<T> query,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default);

    Task<IQueryable<T>> EvaluateAllFailedAsync<TKey, TProperty>(
        IQueryable<T> query,
        int? page = null,
        int? pageSize = null,
        Expression<Func<T, TKey>>? orderBy = null,
        bool ascending = true,
        IEnumerable<ThenByDataBaseExpression<T, TKey>>? thenBys = null,
        bool asNoTracking = false,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null)
        where TKey : notnull;

    Task<IQueryable<T>> EvaluateAllAsync<TKey, TProperty>(
        IQueryable<T> query,
        Expression<Func<T, TKey>>? orderBy = null,
        bool ascending = true,
        IEnumerable<ThenByDataBaseExpression<T, TKey>>? thenBys = null,
        bool asNoTracking = false,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null)
        where TKey : notnull;

    Task<IQueryable<T>> EvaluatePagedAsync<TKey, TProperty>(
        IQueryable<T> query,
        int page = ConstantsHelper.One,
        int pageSize = ConstantsHelper.Ten,
        Expression<Func<T, TKey>>? orderBy = null,
        bool ascending = true,
        IEnumerable<ThenByDataBaseExpression<T, TKey>>? thenBys = null,
        bool asNoTracking = false,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null)
        where TKey : notnull;

    Task<IQueryable<T>> EvaluateTopAsync<TKey, TProperty>(
        IQueryable<T> query,
        int count,
        Expression<Func<T, TKey>>? orderBy = null,
        bool ascending = true,
        IEnumerable<ThenByDataBaseExpression<T, TKey>>? thenBys = null,
        bool asNoTracking = false,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null)
        where TKey : notnull;

    Task<IQueryable<T>> EvaluateDistinctAsync<TKey, TProperty>(
        IQueryable<T> query,
        Expression<Func<T, TKey>> selector,
        int? page = null,
        int? pageSize = null,
        Expression<Func<T, TKey>>? orderBy = null,
        bool ascending = true,
        IEnumerable<ThenByDataBaseExpression<T, TKey>>? thenBys = null,
        bool asNoTracking = false,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null)
        where TKey : notnull;

    Task<IQueryable<T>> EvaluateDuplicatesAsync<TKey, TProperty>(
        IQueryable<T> query,
        Expression<Func<T, TKey>> selector,
        int? page = null,
        int? pageSize = null,
        Expression<Func<T, TKey>>? orderBy = null,
        bool ascending = true,
        IEnumerable<ThenByDataBaseExpression<T, TKey>>? thenBys = null,
        bool asNoTracking = false,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null)
        where TKey : notnull;

    Task<T?> GetLastFailedAsync<TKey, TProperty>(
        IQueryable<T> query,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default)
        where TKey : notnull;

    Task<T?> GetLastAsync<TProperty>(
        IQueryable<T> query,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default);

    Task<TResult> EvaluateMinAsync<TResult, TProperty>(
        IQueryable<T> query,
        Expression<Func<T, TResult>> selector,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    ) where TResult : INumber<TResult>;

    Task<TResult> EvaluateMaxAsync<TResult, TProperty>(
        IQueryable<T> query,
        Expression<Func<T, TResult>> selector,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    ) where TResult : INumber<TResult>;

    Task<decimal> EvaluateAverageAsync<TResult, TProperty>(
        IQueryable<T> query,
        Expression<Func<T, TResult>> selector,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    ) where TResult : INumber<TResult>;

    Task<int> EvaluateSumAsync<TProperty>(
        IQueryable<T> query,
        Expression<Func<T, int>> selector,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    );

    Task<long> EvaluateSumAsync<TProperty>(
        IQueryable<T> query,
        Expression<Func<T, long>> selector,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    );

    Task<double> EvaluateSumAsync<TProperty>(
        IQueryable<T> query,
        Expression<Func<T, double>> selector,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    );

    Task<decimal> EvaluateSumAsync<TProperty>(
        IQueryable<T> query,
        Expression<Func<T, decimal>> selector,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    );

    Task<float> EvaluateSumAsync<TProperty>(
        IQueryable<T> query,
        Expression<Func<T, float>> selector,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    );

    Task<TResult> EvaluateAggregateAsync<TResult, TProperty>(
        IQueryable<T> query,
        Expression<Func<T, TResult>> selector,
        Func<TResult, TResult, TResult> aggregator,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    ) where TResult : INumber<TResult>;

    // Evalúa y agrupa los elementos que cumplen la condición
    Task<Dictionary<TKey, List<T>>> EvaluateGroupedAsync<TKey, TProperty>(
        IQueryable<T> query,
        Expression<Func<T, TKey>> keySelector,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    ) where TKey : notnull;

    // Evalúa y agrupa los elementos con agregación de conteo
    Task<Dictionary<TKey, int>> EvaluateCountByGroupAsync<TKey, TProperty>(
        IQueryable<T> query,
        Func<T, TKey> keySelector,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    ) where TKey : notnull;

    // Evalúa y agrupa los elementos con suma de un campo seleccionado
    Task<Dictionary<TKey, TResult>> EvaluateSumByGroupAsync<TKey, TResult, TProperty>(
        IQueryable<T> query,
        Func<T, TKey> keySelector,
        Func<T, TResult> selector,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    ) where TResult : INumber<TResult> where TKey : notnull;

    // Evalúa y agrupa los elementos con el valor mínimo por grupo
    Task<Dictionary<TKey, TResult>> EvaluateMinByGroupAsync<TKey, TResult, TProperty>(
        IQueryable<T> query,
        Func<T, TKey> keySelector,
        Func<T, TResult> selector,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    ) where TResult : INumber<TResult> where TKey : notnull;

    // Evalúa y agrupa los elementos con el valor máximo por grupo
    Task<Dictionary<TKey, TResult>> EvaluateMaxByGroupAsync<TKey, TResult, TProperty>(
        IQueryable<T> query,
        Func<T, TKey> keySelector,
        Func<T, TResult> selector,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    ) where TResult : INumber<TResult> where TKey : notnull;

    // Evalúa y agrupa los elementos con el promedio de un campo por grupo
    Task<Dictionary<TKey, decimal>> EvaluateAverageByGroupAsync<TKey, TResult, TProperty>(
        IQueryable<T> query,
        Func<T, TKey> keySelector,
        Func<T, TResult> selector,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    ) where TResult : INumber<TResult> where TKey : notnull;

    // Evalúa y devuelve los grupos que tienen más de un elemento (duplicados)
    Task<Dictionary<TKey, List<T>>> EvaluateDuplicatesByGroupAsync<TKey, TProperty>(
        IQueryable<T> query,
        Func<T, TKey> keySelector,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    ) where TKey : notnull;

    // Evalúa y devuelve los grupos que contienen solo un elemento (únicos)
    Task<Dictionary<TKey, T>> EvaluateUniquesByGroupAsync<TKey, TProperty>(
        IQueryable<T> query,
        Func<T, TKey> keySelector,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    ) where TKey : notnull;

    // Evalúa y devuelve los N primeros elementos de cada grupo ordenados
    Task<Dictionary<TKey, List<T>>> EvaluateTopByGroupAsync<TKey, TKey2, TProperty>(
        IQueryable<T> query,
        Func<T, TKey> keySelector,
        int count,
        Expression<Func<T, TKey2>>? orderBy = null,
        bool ascending = true,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    ) where TKey : notnull where TKey2 : notnull;

    Task<IQueryable<T>> EvaluateQuery(IQueryable<T> query, bool asNoTracking = false);

    Task<PaginatedBlockResult<T>> GetPaginatedBlockAsync<TKey, TProperty>(
        IQueryable<T> query,
        int blockSize = ConstantsHelper.Thousand,
        int page = ConstantsHelper.One,
        int pageSize = ConstantsHelper.OneHundred,
        Expression<Func<T, TKey>>? orderBy = null,
        bool ascending = true,
        IEnumerable<ThenByDataBaseExpression<T, TKey>>? thenBys = null,
        bool asNoTracking = false,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null,
        CancellationToken cancellationToken = default
    ) where TKey : notnull;

     Task<IQueryable<T>> GetPaginatedBlockQueryAsync<TKey, TProperty>(
        IQueryable<T> query,
        int blockSize = ConstantsHelper.Thousand, 
        int page = ConstantsHelper.One,
        int pageSize = ConstantsHelper.OneHundred,
        Expression<Func<T, TKey>>? orderBy = null,
        bool ascending = true,
        IEnumerable<ThenByDataBaseExpression<T, TKey>>? thenBys = null,
        bool asNoTracking = false,
        IEnumerable<Expression<Func<T, TProperty>>>? includes = null
        ) where TKey : notnull;
}