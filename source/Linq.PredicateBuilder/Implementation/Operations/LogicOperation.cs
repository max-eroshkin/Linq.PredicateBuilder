namespace Linq.PredicateBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Defines methods of logical operations.
    /// </summary>
    /// <typeparam name="TEntity">The type of the data in the data source.</typeparam>
    public abstract class LogicOperation<TEntity>
    {
        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="builderResult">An intermediate builder.</param>
        /// <param name="strategy">A filtering strategy.</param>
        protected LogicOperation(IAndOrQueryBuilderResult<TEntity> builderResult, IOperationStrategy strategy)
            : this(strategy)
        {
            BuilderResult = builderResult;
        }

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="strategy">A filtering strategy.</param>
        protected LogicOperation(IOperationStrategy strategy)
        {
            Strategy = strategy;
        }

        /// <summary>
        /// A delegate that transforms the specified filtering expression.
        /// </summary>
        protected abstract Func<Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>> Operation { get; }

        /// <summary>
        /// An intermediate builder.
        /// </summary>
        protected IAndOrQueryBuilderResult<TEntity> BuilderResult { get; }

        /// <summary>
        /// A filtering strategy.
        /// </summary>
        protected IOperationStrategy Strategy { get; }

        /// <inheritdoc cref="ILogicOperation{TEntity}.Conditional"/>
        protected ConditionOperation<TEntity> ConditionalInternal(bool condition)
        {
            return new ConditionOperation<TEntity>(Operation, condition, Strategy);
        }

        /// <inheritdoc cref="ILogicOperationT{TBuilderResult,TEntity}.Contains"/>
        protected IAndOrQueryBuilderResult<TEntity> ContainsInternal(
            Expression<Func<TEntity, string>> propertyExpression,
            string input)
        {
            return new QueryBuilderResult<TEntity>(
                Operation(Strategy.Contains(propertyExpression, input)),
                Strategy);
        }

        /// <inheritdoc cref="ILogicOperationT{TBuilderResult,TEntity}.Equals{TInput}"/>
        protected IAndOrQueryBuilderResult<TEntity> EqualsInternal<TInput>(
            Expression<Func<TEntity, TInput>> propertyExpression,
            TInput input)
        {
            return new QueryBuilderResult<TEntity>(
                Operation(Strategy.Equals(propertyExpression, input)),
                Strategy);
        }

        /// <inheritdoc cref="ILogicOperationT{TBuilderResult,TEntity}.Where"/>
        protected IAndOrQueryBuilderResult<TEntity> WhereInternal(Expression<Func<TEntity, bool>> predicate)
        {
            return new QueryBuilderResult<TEntity>(
                Operation(predicate),
                Strategy);
        }

        /// <inheritdoc cref="ILogicOperationT{TBuilderResult,TEntity}.Where"/>
        protected IAndOrQueryBuilderResult<TEntity> WhereInternal<TInput>(
            Expression<Func<TEntity, TInput, bool>> predicate,
            TInput input)
        {
            return new QueryBuilderResult<TEntity>(
                Operation(Strategy.Where(predicate, input)),
                Strategy);
        }

        /// <inheritdoc cref="ILogicOperationT{TBuilderResult,TEntity}.In{TInput}"/>
        protected IAndOrQueryBuilderResult<TEntity> InInternal<TInput>(
            Expression<Func<TEntity, TInput>> propertyExpression,
            IEnumerable<TInput> input)
        {
            return new QueryBuilderResult<TEntity>(
                Operation(Strategy.In(propertyExpression, input)),
                Strategy);
        }

        /// <inheritdoc cref="ILogicOperationT{TBuilderResult,TEntity}.Any{TInput}"/>
        protected IAndOrQueryBuilderResult<TEntity> AnyInternal<TInput>(
            Expression<Func<TEntity, ICollection<TInput>>> manyToManySelector,
            Func<ILogicOperation<TInput>, IQueryBuilderResult<TInput>> builder)
        {
            if (builder == null)
                throw new ArgumentException("Builder cannot be null", nameof(builder));
            var init = new QueryBuilder<TInput>(Strategy);
            var expression = builder(init).GetExpression();

            return new QueryBuilderResult<TEntity>(
                Operation(Strategy.Any(manyToManySelector, expression)),
                Strategy);
        }

        /// <inheritdoc cref="ILogicOperationT{TBuilderResult,TEntity}.Brackets"/>
        protected IAndOrQueryBuilderResult<TEntity> BracketsInternal(
            Func<ILogicOperation<TEntity>, IQueryBuilderResult<TEntity>> builder)
        {
            if (builder == null)
                throw new ArgumentException("Builder cannot be null", nameof(builder));
            var init = new QueryBuilder<TEntity>(Strategy);
            return new QueryBuilderResult<TEntity>(
                Operation(builder(init).GetExpression()),
                Strategy);
        }
    }
}