namespace Linq.PredicateBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <inheritdoc cref="ILogicOperation{TEntity}" />
    internal abstract class NotOperationBase<TEntity>
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="operation">A filter transforming operation.</param>
        /// <param name="strategy">A filtering strategy.</param>
        protected NotOperationBase(
            Func<Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>> operation,
            IOperationStrategy strategy)
        {
            Operation = operation;
            Strategy = strategy;
        }

        /// <summary>
        /// A delegate that transforms the specified filtering expression.
        /// </summary>
        protected Func<Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>> Operation { get; }

        /// <summary>
        /// The filtering strategy.
        /// </summary>
        protected IOperationStrategy Strategy { get; }

        /// <inheritdoc cref="ILogicOperationT{TBuilderResult,TEntity}.Contains"/>
        protected QueryBuilderResult<TEntity> ContainsInternal(
            Expression<Func<TEntity, string>> propertyExpression,
            string input)
        {
            var predicate = Strategy.Contains(propertyExpression, input).Not();
            return new QueryBuilderResult<TEntity>(
                Operation(predicate),
                Strategy);
        }

        /// <inheritdoc cref="ILogicOperationT{TBuilderResult,TEntity}.Equals{TInput}"/>
        protected QueryBuilderResult<TEntity> EqualsInternal<TInput>(
            Expression<Func<TEntity, TInput>> propertyExpression,
            TInput input)
        {
            var predicate = Strategy.Equals(propertyExpression, input).Not();
            return new QueryBuilderResult<TEntity>(
                Operation(predicate),
                Strategy);
        }

        /// <inheritdoc cref="ILogicOperationT{TBuilderResult,TEntity}.Where"/>
        protected QueryBuilderResult<TEntity> WhereInternal(Expression<Func<TEntity, bool>> predicate)
        {
            return new QueryBuilderResult<TEntity>(
                Operation(predicate.Not()),
                Strategy);
        }

        /// <inheritdoc cref="ILogicOperationT{TBuilderResult,TEntity}.Where"/>
        protected IAndOrQueryBuilderResult<TEntity> WhereInternal<TInput>(
            Expression<Func<TEntity, TInput, bool>> predicate,
            TInput input)
        {
            var negatedPredicate = Strategy.Where(predicate, input).Not();
            return new QueryBuilderResult<TEntity>(
                Operation(negatedPredicate),
                Strategy);
        }

        /// <inheritdoc cref="ILogicOperationT{TBuilderResult,TEntity}.In{TInput}"/>
        protected QueryBuilderResult<TEntity> InInternal<TInput>(
            Expression<Func<TEntity, TInput>> propertyExpression,
            IEnumerable<TInput> input)
        {
            var predicate = Strategy.In(propertyExpression, input).Not();
            return new QueryBuilderResult<TEntity>(
                Operation(predicate),
                Strategy);
        }

        /// <inheritdoc cref="ILogicOperationT{TBuilderResult,TEntity}.Any{TInput}"/>
        protected QueryBuilderResult<TEntity> AnyInternal<TInput>(
            Expression<Func<TEntity, ICollection<TInput>>> manyToManySelector,
            Func<ILogicOperation<TInput>, IQueryBuilderResult<TInput>> builder)
        {
            if (builder == null)
                throw new ArgumentException("Builder cannot be null", nameof(builder));
            var init = new QueryBuilder<TInput>(Strategy);
            var expression = builder(init).GetExpression();
            var predicate = Strategy.Any(manyToManySelector, expression).Not();
            return new QueryBuilderResult<TEntity>(
                Operation(predicate),
                Strategy);
        }

        /// <inheritdoc cref="ILogicOperationT{TBuilderResult,TEntity}.Brackets"/>
        protected IAndOrQueryBuilderResult<TEntity> BracketsInternal(
            Func<ILogicOperation<TEntity>, IQueryBuilderResult<TEntity>> builder)
        {
            if (builder == null)
                throw new ArgumentException("Builder cannot be null", nameof(builder));
            var init = new QueryBuilder<TEntity>(Strategy);
            var expression = builder(init).GetExpression();
            var predicate = expression.Not();
            return new QueryBuilderResult<TEntity>(
                Operation(predicate),
                Strategy);
        }
    }
}