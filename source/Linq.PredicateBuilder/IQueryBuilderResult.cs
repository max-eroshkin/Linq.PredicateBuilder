namespace Linq.PredicateBuilder
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Промежуточный билдер выражения
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности в выражении</typeparam>
    public interface IQueryBuilderResult<TEntity>
    {
        /// <summary>
        /// Возвращает выражение предиката, построенное билдером
        /// </summary>
        Expression<Func<TEntity, bool>> GetExpression();
    }
}