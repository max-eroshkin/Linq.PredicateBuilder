namespace Linq.PredicateBuilder
{
    /// <summary>
    /// Промежуточный билдер выражения
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности в выражении</typeparam>
    public interface IOrQueryBuilderResult<TEntity> : IQueryBuilderResult<TEntity>
    {
        /// <summary>
        /// Операция логического "И"
        /// </summary>
        IOrLogicOperation<TEntity> Or { get; }
    }
}