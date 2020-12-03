namespace Linq.PredicateBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <inheritdoc />
    public abstract class LogicOperation<TEntity>
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="builderResult">Промежуточный билдер выражения</param>
        /// <param name="strategy">A filtering strategy.</param>
        protected LogicOperation(QueryBuilderResult<TEntity> builderResult, IOperationStrategy strategy)
            : this(strategy)
        {
            BuilderResult = builderResult;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="strategy">A filtering strategy.</param>
        protected LogicOperation(IOperationStrategy strategy)
        {
            Strategy = strategy;
        }

        /// <summary>
        /// Возвращает делегат применения логической операции к выражению
        /// </summary>
        protected abstract Func<Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>> Operation { get; }

        /// <summary>
        /// Промежуточный билдер
        /// </summary>
        protected QueryBuilderResult<TEntity> BuilderResult { get; }

        /// <summary>
        /// A filtering strategy.
        /// </summary>
        protected IOperationStrategy Strategy { get; }

        /// <summary>
        /// Добавляет логическую операцию только при выполнении услович
        /// </summary>
        /// <param name="condition">Условие добавления операции</param>
        protected ConditionOperation<TEntity> ConditionalInternal(bool condition)
        {
            return new ConditionOperation<TEntity>(Operation, condition, Strategy);
        }

        /// <summary>
        /// Builds a predicate indicating whether a specified substring occurs within the string
        /// defined by a property expression.
        /// </summary>
        /// <param name="propertyExpression">The property selector expression.</param>
        /// <param name="input">The string to seek.</param>
        protected IAndOrQueryBuilderResult<TEntity> ContainsInternal(
            Expression<Func<TEntity, string>> propertyExpression,
            string input)
        {
            return new QueryBuilderResult<TEntity>(
                Operation(Strategy.Contains(propertyExpression, input)),
                Strategy);
        }

        /// <summary>
        /// Проверяет свойство на равенство
        /// </summary>
        /// <param name="propertyExpression">Селектор свойства</param>
        /// <param name="input">Значение для сравнения</param>
        /// <typeparam name="TValue">Тип свойства</typeparam>
        protected IAndOrQueryBuilderResult<TEntity> EqualsInternal<TValue>(
            Expression<Func<TEntity, TValue>> propertyExpression,
            TValue input)
        {
            return new QueryBuilderResult<TEntity>(
                Operation(Strategy.Equals(propertyExpression, input)),
                Strategy);
        }

        /// <summary>
        /// Проверяет на выполнение условия
        /// </summary>
        /// <param name="predicate">Условие</param>
        protected IAndOrQueryBuilderResult<TEntity> WhereInternal(Expression<Func<TEntity, bool>> predicate)
        {
            return new QueryBuilderResult<TEntity>(
                Operation(predicate),
                Strategy);
        }

        /// <summary>
        /// Проверяет свойство на вхождение в множество
        /// </summary>
        /// <param name="propertyExpression">Селектор свойства</param>
        /// <param name="input">Множество для сравнения</param>
        /// <typeparam name="TValue">Тип свойства</typeparam>
        protected IAndOrQueryBuilderResult<TEntity> InInternal<TValue>(
            Expression<Func<TEntity, TValue>> propertyExpression,
            IEnumerable<TValue> input)
        {
            return new QueryBuilderResult<TEntity>(
                Operation(Strategy.In(propertyExpression, input)),
                Strategy);
        }

        /// <summary>
        /// qqq
        /// </summary>
        /// <param name="manyToManySelector">sss</param>
        /// <param name="builder">inner bldr</param>
        /// <typeparam name="TValue">ttt</typeparam>
        protected IAndOrQueryBuilderResult<TEntity> AnyInternal<TValue>(
            Expression<Func<TEntity, ICollection<TValue>>> manyToManySelector,
            Func<QueryBuilder<TValue>, IQueryBuilderResult<TValue>> builder)
        {
            if (builder == null)
                throw new ArgumentException("Builder cannot be null", nameof(builder));
            var init = new QueryBuilder<TValue>(Strategy);
            var expression = builder(init).GetExpression();

            return new QueryBuilderResult<TEntity>(
                Operation(Strategy.Any(manyToManySelector, expression)),
                Strategy);
        }

        /// <summary>
        /// Builds a predicate using inner <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">Inner builder.</param>
        protected IAndOrQueryBuilderResult<TEntity> BracketsInternal(
            Func<QueryBuilder<TEntity>, IQueryBuilderResult<TEntity>> builder)
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