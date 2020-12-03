namespace Linq.PredicateBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using JetBrains.Annotations;

    /// <summary>
    /// Defines methods of logical operations.
    /// </summary>
    /// <typeparam name="TBuilderResult">Type of builder result.</typeparam>
    /// <typeparam name="TEntity">The type of the data in the data source.</typeparam>
    public interface ILogicOperationT<out TBuilderResult, TEntity>
    {
        /// <summary>
        /// Builds a predicate indicating whether a specified substring occurs within the string
        /// defined by a property expression.
        /// </summary>
        /// <param name="propertyExpression">The property selector expression.</param>
        /// <param name="input">The string to seek.</param>
        TBuilderResult Contains(
            [NotNull] Expression<Func<TEntity, string>> propertyExpression,
            [CanBeNull] string input);

        /// <summary>
        /// Проверяет свойство на равенство
        /// </summary>
        /// <param name="propertyExpression">Селектор свойства</param>
        /// <param name="input">Значение для сравнения</param>
        /// <typeparam name="TValue">Тип свойства</typeparam>
        TBuilderResult Equals<TValue>(
            [NotNull] Expression<Func<TEntity, TValue>> propertyExpression,
            [CanBeNull] TValue input);

        /// <summary>
        /// Проверяет на выполнение условия
        /// </summary>
        /// <param name="predicate">Условие</param>
        TBuilderResult Where(
            [NotNull] Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Проверяет свойство на вхождение в множество
        /// </summary>
        /// <param name="propertyExpression">Селектор свойства</param>
        /// <param name="input">Множество для сравнения</param>
        /// <typeparam name="TValue">Тип свойства</typeparam>
        TBuilderResult In<TValue>(
            [NotNull] Expression<Func<TEntity, TValue>> propertyExpression,
            [CanBeNull] IEnumerable<TValue> input);

        /// <summary>
        /// qqq
        /// </summary>
        /// <param name="manyToManySelector">sss</param>
        /// <param name="builder">inner bldr</param>
        /// <typeparam name="TValue">ttt</typeparam>
        TBuilderResult Any<TValue>(
            Expression<Func<TEntity, ICollection<TValue>>> manyToManySelector,
            Func<QueryBuilder<TValue>, IQueryBuilderResult<TValue>> builder);

        /// <summary>
        /// Builds a predicate using inner <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">Inner builder.</param>
        TBuilderResult Brackets(
            Func<QueryBuilder<TEntity>, IQueryBuilderResult<TEntity>> builder);
    }
}