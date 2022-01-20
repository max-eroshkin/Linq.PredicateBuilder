namespace Research;

public static class EqualsExtensions
{
    public static IAndResult<TEntity> Equals<TEntity>(this IAndOperator<TEntity> oper)
    {
        return default;
    }
    public static IOrResult<TEntity> Equals<TEntity>(this IOrOperator<TEntity> oper)
    {
        return default;
    }    
    public static IFullResult<TEntity> Equals<TEntity>(this IAndOrOperator<TEntity> oper)
    {
        return default;
    }
}