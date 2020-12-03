namespace Linq.PredicateBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using JetBrains.Annotations;

    /// <summary>
    /// Defines methods of logical operations.
    /// </summary>
    /// <typeparam name="TEntity">The type of the data in the data source.</typeparam>
    public interface IOrLogicOperation<TEntity> : ILogicOperationT<IOrQueryBuilderResult<TEntity>, TEntity>
    {
        /// <summary>
        /// Logic NOT.
        /// </summary>
        IOrLogicOperation<TEntity> Not { get; }

        /// <summary>
        /// Добавляет логическую операцию только при выполнении услович
        /// </summary>
        /// <param name="condition">Условие добавления операции</param>
        IOrLogicOperation<TEntity> Conditional(bool condition);
    }
}