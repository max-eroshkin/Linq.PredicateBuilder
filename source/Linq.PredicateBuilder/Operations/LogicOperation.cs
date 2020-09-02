namespace Linq.PredicateBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <inheritdoc />
    public abstract class LogicOperation<TEntity> : ILogicOperation<TEntity>
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
        /// Операция логического "И"
        /// </summary>
        public NotOperation<TEntity> Not => new NotOperation<TEntity>(Operation, Strategy);

        /// <summary>
        /// Промежуточный билдер
        /// </summary>
        protected QueryBuilderResult<TEntity> BuilderResult { get; }

        /// <summary>
        /// A filtering strategy.
        /// </summary>
        protected IOperationStrategy Strategy { get; }

        /// <summary>
        /// Возвращает делегат применения логической операции к выражению
        /// </summary>
        protected abstract Func<Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>> Operation { get; }

        /// <inheritdoc />
        public virtual QueryBuilderResult<TEntity> Contains(
            Expression<Func<TEntity, string>> propertyExpression,
            string input)
        {
            return new QueryBuilderResult<TEntity>(
                Operation(Strategy.Contains(propertyExpression, input)),
                Strategy);
        }

        /// <inheritdoc />
        public virtual QueryBuilderResult<TEntity> Equals<TValue>(
            Expression<Func<TEntity, TValue>> propertyExpression,
            TValue input)
        {
            return new QueryBuilderResult<TEntity>(
                Operation(Strategy.Equals(propertyExpression, input)),
                Strategy);
        }

        /// <inheritdoc />
        public virtual QueryBuilderResult<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return new QueryBuilderResult<TEntity>(
                Operation(predicate),
                Strategy);
        }

        /// <inheritdoc />
        public QueryBuilderResult<TEntity> In<TValue>(
            Expression<Func<TEntity, TValue>> propertyExpression,
            IEnumerable<TValue> input)
        {
            return new QueryBuilderResult<TEntity>(
                Operation(Strategy.In(propertyExpression, input)),
                Strategy);
        }

        /// <inheritdoc />
        public QueryBuilderResult<TEntity> Any<TValue>(
            Expression<Func<TEntity, ICollection<TValue>>> manyToManySelector,
            Func<QueryBuilder<TValue>, QueryBuilderResult<TValue>> builder)
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
        /// Добавляет логическую операцию только при выполнении услович
        /// </summary>
        /// <param name="condition">Условие добавления операции</param>
        public ILogicOperation<TEntity> Conditional(bool condition)
        {
            return new ConditionOperation(Operation, condition, Strategy);
        }

        /// <summary>
        /// Операция логического "И"
        /// </summary>
        private class ConditionOperation : LogicOperation<TEntity>
        {
            private readonly bool _condition;
            private readonly Func<Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>> _baseOperation;

            /// <summary>
            /// ctor
            /// </summary>
            /// <param name="baseOperation">Базовая логическая операция</param>
            /// <param name="condition">Условие добавления операции</param>
            /// <param name="strategy">A filtering strategy.</param>
            public ConditionOperation(
                Func<Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>> baseOperation,
                bool condition,
                IOperationStrategy strategy)
                : base(strategy)
            {
                _condition = condition;
                _baseOperation = baseOperation;
            }

            /// <inheritdoc />
            protected override Func<Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>> Operation =>
                x => _baseOperation(_condition ? x : null);
        }
    }
}