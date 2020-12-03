namespace Linq.PredicateBuilder
{
    /// <summary>
    /// Промежуточный билдер выражения
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности в выражении</typeparam>
    public interface IAndOrQueryBuilderResult<TEntity> : IAndQueryBuilderResult<TEntity>, IOrQueryBuilderResult<TEntity>
    {
    }
}