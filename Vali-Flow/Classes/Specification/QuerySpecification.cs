    using System.Linq.Expressions;
    using Vali_Flow.Classes.Options;
    using Vali_Flow.Core.Builder;
    using Vali_Flow.Core.Utils;
    using Vali_Flow.Interfaces.Options;
    using Vali_Flow.Interfaces.Specification;
    using Vali_Flow.Utils;

    namespace Vali_Flow.Classes.Specification;

    /// <summary>
    /// Represents a query specification for defining reusable query criteria and options for entities of type <typeparamref name="T"/>.
    /// </summary>
    /// <remarks>
    /// The <see cref="QuerySpecification{T}"/> class extends <see cref="BasicSpecification{T}"/> and implements <see cref="IQuerySpecification{T}"/>
    /// to provide a flexible and reusable way to define query criteria, including filtering, ordering, pagination, and Entity Framework Core-specific options
    /// such as change tracking and split queries. It uses a <see cref="ValiFlow{T}"/> filter to define filtering criteria and supports primary ordering
    /// (<see cref="OrderBy"/>) and secondary ordering (<see cref="ThenBys"/>) through expressions. Pagination is configured via <see cref="Page"/> and
    /// <see cref="PageSize"/>, and a maximum number of items can be specified with <see cref="Top"/>. This class is designed to work with
    /// Entity Framework Core queries, enabling complex query composition in a maintainable and testable manner. Use this class to encapsulate query logic
    /// for consistent application across data access operations.
    /// </remarks>
    /// <typeparam name="T">The type of the entity to which the specification applies. Must be a class.</typeparam>
    public class QuerySpecification<T> : BasicSpecification<T>, IQuerySpecification<T> where T : class
    {
        private IEfOrderBy<T>? _orderBy;
        private readonly List<IEfOrderThenBy<T>> _thenBys = new();
        private int? _page;
        private int? _pageSize;
        private int? _top;
        
        /// <summary>
        /// Gets the primary ordering expression, if any.
        /// </summary>
        public IEfOrderBy<T>? OrderBy => _orderBy;

        /// <summary>
        /// Gets a collection of secondary ordering expressions (ThenBy), if any.
        /// </summary>
        public IEnumerable<IEfOrderThenBy<T>>? ThenBys => _thenBys.Count > Constants.ZeroInt ? _thenBys : null;

        /// <summary>
        /// Gets the number of items to skip for pagination, if specified.
        /// </summary>
        public int? Page => _page;

        /// <summary>
        /// Gets the number of items to take for pagination, if specified.
        /// </summary>
        public int? PageSize => _pageSize;

        /// <summary>
        /// Gets the maximum number of items to take (top), if specified.
        /// </summary>
        public int? Top => _top;
        
        
        public QuerySpecification()
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="QuerySpecification{T}"/> class with a validation filter.
        /// </summary>
        /// <param name="filter">The validation flow that defines the filtering criteria. Cannot be null.</param>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="filter"/> parameter is null.</exception>
        public QuerySpecification(ValiFlow<T> filter) : base(filter)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="QuerySpecification{T}"/> class with a validation filter and optional configurations.
        /// </summary>
        /// <param name="filter">The validation flow that defines the filtering criteria. Cannot be null.</param>
        /// <param name="asNoTracking">Indicates whether the query should be executed without change tracking. Default is true.</param>
        /// <param name="asSplitQuery">Indicates whether the query should be executed as a split query. Default is false.</param>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="filter"/> parameter is null.</exception>
        public QuerySpecification(
            ValiFlow<T> filter,
            bool asNoTracking = true,
            bool asSplitQuery = false
        ) : base(filter, asNoTracking, asSplitQuery)
        {
        }
        
        /// <summary>
        /// Sets the primary ordering expression for the specification.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property used for ordering, which must be non-nullable.</typeparam>
        /// <param name="expression">The expression that defines the property to order by. Cannot be null.</param>
        /// <param name="ascending">A value indicating whether to order in ascending order. Default is true.</param>
        /// <returns>The current specification instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="expression"/> parameter is null.</exception>
        public QuerySpecification<T> WithOrderBy<TProperty>(Expression<Func<T, TProperty>> expression, bool ascending = true)
            where TProperty : notnull
        {
            _orderBy = new EfOrderBy<T, TProperty>(expression, ascending);
            return this;
        }
        
        /// <summary>
        /// Adds a single secondary ordering expression (ThenBy) to the specification.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property used for ordering, which must be non-nullable.</typeparam>
        /// <param name="expression">The expression that defines the property to order by. Cannot be null.</param>
        /// <param name="ascending">A value indicating whether to order in ascending order. Default is true.</param>
        /// <returns>The current specification instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="expression"/> parameter is null.</exception>
        public QuerySpecification<T> AddThenBy<TProperty>(Expression<Func<T, TProperty>> expression, bool ascending = true)
            where TProperty : notnull
        {
            _thenBys.Add(new EfOrderThenBy<T, TProperty>(expression, ascending));
            return this;
        }
        
        /// <summary>
        /// Adds multiple secondary ordering expressions (ThenBy) to the specification.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property used for ordering, which must be non-nullable.</typeparam>
        /// <param name="expressions">The collection of expressions that define the properties to order by. Cannot be null.</param>
        /// <param name="ascending">A value indicating whether to order in ascending order. Default is true.</param>
        /// <returns>The current specification instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="expressions"/> parameter is null.</exception>
        public QuerySpecification<T> AddThenBys<TProperty>(IEnumerable<Expression<Func<T, TProperty>>> expressions,
            bool ascending = true)
            where TProperty : notnull
        {
            foreach (var expression in expressions)
            {
                _thenBys.Add(new EfOrderThenBy<T, TProperty>(expression, ascending));
            }

            return this;
        }
        
        /// <summary>
        /// Configures pagination for the specification using a page number and page size.
        /// </summary>
        /// <param name="page">The page number to retrieve (must be greater than or equal to 1).</param>
        /// <param name="pageSize">The number of items per page (must be greater than or equal to 1).</param>
        /// <returns>The current specification instance for method chaining.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="page"/> is less than 1 or <paramref name="pageSize"/> is less than 1.</exception>
        public QuerySpecification<T> WithPagination(int page, int pageSize)
        {
            if (page < 1) throw new ArgumentException("Page must be greater than or equal to 1.", nameof(page));
            if (pageSize < 1) throw new ArgumentException("PageSize must be greater than or equal to 1.", nameof(pageSize));

            _page = page;
            _pageSize = pageSize;
            return this;
        }
        
        /// <summary>
        /// Sets the page number for pagination.
        /// </summary>
        /// <param name="page">The page number to retrieve (must be greater than or equal to 1).</param>
        /// <returns>The current specification instance for method chaining.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="page"/> is less than 1.</exception>
        public QuerySpecification<T> WithPage(int page)
        {
            if (page < 1) throw new ArgumentException("Page must be greater than or equal to 1.", nameof(page));
            _page = page;
            return this;
        }
        
        /// <summary>
        /// Sets the number of items per page for pagination.
        /// </summary>
        /// <param name="pageSize">The number of items per page (must be greater than or equal to 1).</param>
        /// <returns>The current specification instance for method chaining.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="pageSize"/> is less than 1.</exception>
        public QuerySpecification<T> WithPageSize(int pageSize)
        {
            if (pageSize < 1) throw new ArgumentException("PageSize must be greater than or equal to 1.", nameof(pageSize));
            _pageSize = pageSize;
            return this;
        }
        
        /// <summary>
        /// Sets the maximum number of items to take (top) for the specification.
        /// </summary>
        /// <param name="top">The maximum number of items to take (must be greater than or equal to 1).</param>
        /// <returns>The current specification instance for method chaining.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="top"/> is less than 1.</exception>
        public QuerySpecification<T> WithTop(int top)
        {
            if (top < 1) throw new ArgumentException("Top must be greater than or equal to 1.", nameof(top));
            _top = top;
            return this;
        }
    }