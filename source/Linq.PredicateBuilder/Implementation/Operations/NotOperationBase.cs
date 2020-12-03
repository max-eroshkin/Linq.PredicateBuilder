namespace Linq.PredicateBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Операция отрицания
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности в выражении</typeparam>
    internal abstract class NotOperationBase<TEntity>
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="operation">Возвращает делегат применения логической операции к выражению</param>
        /// <param name="strategy">A filtering strategy.</param>
        public NotOperationBase(
            Func<Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>> operation,
            IOperationStrategy strategy)
        {
            Operation = operation;
            Strategy = strategy;
        }

        /// <summary>
        /// fsadf
        /// </summary>
        protected Func<Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>> Operation { get; }

        /// <summary>
        /// The filtering strategy.
        /// </summary>
        protected IOperationStrategy Strategy { get; }

        /// <summary>
        /// Builds a predicate indicating whether a specified substring occurs within the string
        /// defined by a property expression.
        /// </summary>
        /// <param name="propertyExpression">The property selector expression.</param>
        /// <param name="input">The string to seek.</param>
        protected QueryBuilderResult<TEntity> ContainsInternal(
            Expression<Func<TEntity, string>> propertyExpression,
            string input)
        {
            var predicate = Strategy.Contains(propertyExpression, input).Not();
            return new QueryBuilderResult<TEntity>(
                Operation(predicate),
                Strategy);
        }

        /// <summary>
        /// Проверяет свойство на равенство
        /// </summary>
        /// <param name="propertyExpression">Селектор свойства</param>
        /// <param name="input">Значение для сравнения</param>
        /// <typeparam name="TValue">Тип свойства</typeparam>
        protected QueryBuilderResult<TEntity> EqualsInternal<TValue>(
            Expression<Func<TEntity, TValue>> propertyExpression,
            TValue input)
        {
            var predicate = Strategy.Equals(propertyExpression, input).Not();
            return new QueryBuilderResult<TEntity>(
                Operation(predicate),
                Strategy);
        }

        /// <summary>
        /// Проверяет на выполнение условия
        /// </summary>
        /// <param name="predicate">Условие</param>
        protected QueryBuilderResult<TEntity> WhereInternal(Expression<Func<TEntity, bool>> predicate)
        {
            return new QueryBuilderResult<TEntity>(
                Operation(predicate.Not()),
                Strategy);
        }

        /// <summary>
        /// Проверяет свойство на вхождение в множество
        /// </summary>
        /// <param name="propertyExpression">Селектор свойства</param>
        /// <param name="input">Множество для сравнения</param>
        /// <typeparam name="TValue">Тип свойства</typeparam>
        protected QueryBuilderResult<TEntity> InInternal<TValue>(
            Expression<Func<TEntity, TValue>> propertyExpression,
            IEnumerable<TValue> input)
        {
            var predicate = Strategy.In(propertyExpression, input).Not();
            return new QueryBuilderResult<TEntity>(
                Operation(predicate),
                Strategy);
        }

        /// <summary>
        /// qqq
        /// </summary>
        /// <param name="manyToManySelector">sss</param>
        /// <param name="builder">inner bldr</param>
        /// <typeparam name="TValue">ttt</typeparam>
        protected QueryBuilderResult<TEntity> AnyInternal<TValue>(
            Expression<Func<TEntity, ICollection<TValue>>> manyToManySelector,
            Func<ILogicOperation<TValue>, IQueryBuilderResult<TValue>> builder)
        {
            if (builder == null)
                throw new ArgumentException("Builder cannot be null", nameof(builder));
            var init = new QueryBuilder<TValue>(Strategy);
            var expression = builder(init).GetExpression();
            var predicate = Strategy.Any(manyToManySelector, expression).Not();
            return new QueryBuilderResult<TEntity>(
                Operation(predicate),
                Strategy);
        }

        /// <summary>
        /// Builds a predicate using inner <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">Inner builder.</param>
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