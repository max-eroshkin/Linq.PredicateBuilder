namespace Linq.PredicateBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Операция отрицания
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности в выражении</typeparam>
    public class NotOperation<TEntity> : ILogicOperation<TEntity>
    {
        private readonly Func<Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>> _operation;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="operation">Возвращает делегат применения логической операции к выражению</param>
        /// <param name="strategy">A filtering strategy.</param>
        public NotOperation(
            Func<Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>> operation,
            IOperationStrategy strategy)
        {
            _operation = operation;
            Strategy = strategy;
        }

        /// <summary>
        /// The filtering strategy.
        /// </summary>
        private IOperationStrategy Strategy { get; }

        /// <inheritdoc />
        public QueryBuilderResult<TEntity> Contains(Expression<Func<TEntity, string>> propertyExpression, string input)
        {
            var predicate = Strategy.Contains(propertyExpression, input).Not();
            return new QueryBuilderResult<TEntity>(
                _operation(predicate),
                Strategy);
        }

        /// <inheritdoc />
        public QueryBuilderResult<TEntity> Equals<TValue>(
            Expression<Func<TEntity, TValue>> propertyExpression,
            TValue input)
        {
            var predicate = Strategy.Equals(propertyExpression, input).Not();
            return new QueryBuilderResult<TEntity>(
                _operation(predicate),
                Strategy);
        }

        /// <inheritdoc />
        public QueryBuilderResult<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return new QueryBuilderResult<TEntity>(
                _operation(predicate.Not()),
                Strategy);
        }

        /// <inheritdoc />
        public QueryBuilderResult<TEntity> In<TValue>(
            Expression<Func<TEntity, TValue>> propertyExpression,
            IEnumerable<TValue> input)
        {
            var predicate = Strategy.In(propertyExpression, input).Not();
            return new QueryBuilderResult<TEntity>(
                _operation(predicate),
                Strategy);
        }

        /// <inheritdoc />
        public QueryBuilderResult<TEntity> Any<TValue>(
            Expression<Func<TEntity, ICollection<TValue>>> manyToManySelector,
            Func<QueryBuilder<TValue>, QueryBuilderResult<TValue>> builder)
        {
            throw new NotImplementedException();
        }
    }
}