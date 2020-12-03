namespace Linq.PredicateBuilder
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Операция логического "И"
    /// </summary>
    /// <typeparam name="TEntity">The type of the data in the data source.</typeparam>
    public abstract class ConditionOperationBase<TEntity> : LogicOperation<TEntity>
    {
        private readonly bool _condition;
        private readonly Func<Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>> _baseOperation;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="baseOperation">Базовая логическая операция</param>
        /// <param name="condition">Условие добавления операции</param>
        /// <param name="strategy">A filtering strategy.</param>
        public ConditionOperationBase(
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