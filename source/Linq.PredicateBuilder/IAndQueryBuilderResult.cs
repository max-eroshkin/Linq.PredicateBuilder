namespace Linq.PredicateBuilder
{
    /// <summary>
    /// Промежуточный билдер выражения
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности в выражении</typeparam>
    public interface IAndQueryBuilderResult<TEntity> : IQueryBuilderResult<TEntity>
    {
        /// <summary>
        /// Операция логического "И"
        /// </summary>
        IAndLogicOperation<TEntity> And { get; }
    }
}