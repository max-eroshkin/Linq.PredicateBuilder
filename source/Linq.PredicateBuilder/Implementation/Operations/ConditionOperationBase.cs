namespace Linq.PredicateBuilder
{
    using System;
    using System.Linq.Expressions;

    /// <inheritdoc cref="LogicOperation{TEntity}"/>
    public abstract class ConditionOperationBase<TEntity> : LogicOperation<TEntity>
    {
        private readonly bool _condition;
        private readonly Func<Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>> _baseOperation;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="baseOperation">A filter transforming operation.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="strategy">The filtering strategy.</param>
        protected ConditionOperationBase(
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