namespace Linq.PredicateBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Операция отрицания
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности в выражении</typeparam>
    internal class NotOrOperation<TEntity> : NotOperationBase<TEntity>, IOrLogicOperation<TEntity>
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="operation">Возвращает делегат применения логической операции к выражению</param>
        /// <param name="strategy">A filtering strategy.</param>
        public NotOrOperation(
            Func<Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>> operation,
            IOperationStrategy strategy)
            : base(operation, strategy)
        {
        }

        /// <inheritdoc />
        public IOrLogicOperation<TEntity> Not => new NotOrOperation<TEntity>(Operation, Strategy);

        /// <inheritdoc/>
        public IOrQueryBuilderResult<TEntity> Equals<TValue>(
            Expression<Func<TEntity, TValue>> propertyExpression,
            TValue input)
            => EqualsInternal(propertyExpression, input);

        /// <inheritdoc/>
        public IOrQueryBuilderResult<TEntity> Where(
            Expression<Func<TEntity, bool>> predicate)
            => WhereInternal(predicate);

        /// <inheritdoc/>
        public IOrQueryBuilderResult<TEntity> In<TValue>(
            Expression<Func<TEntity, TValue>> propertyExpression,
            IEnumerable<TValue> input)
            => InInternal(propertyExpression, input);

        /// <inheritdoc/>
        public IOrQueryBuilderResult<TEntity> Any<TValue>(
            Expression<Func<TEntity, ICollection<TValue>>> manyToManySelector,
            Func<QueryBuilder<TValue>, IQueryBuilderResult<TValue>> builder)
            => AnyInternal(manyToManySelector, builder);

        /// <inheritdoc/>
        public IOrQueryBuilderResult<TEntity> Brackets(
            Func<QueryBuilder<TEntity>, IQueryBuilderResult<TEntity>> builder)
            => BracketsInternal(builder);

        /// <inheritdoc/>
        public IOrLogicOperation<TEntity> Conditional(bool condition)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IOrQueryBuilderResult<TEntity> Contains(
            Expression<Func<TEntity, string>> propertyExpression,
            string input)
            => ContainsInternal(propertyExpression, input);
    }
}