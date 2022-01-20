namespace Research;

public static class ConditionalExtensions
{
    public static IAndOperator<TEntity> Conditional<TEntity>(this IAndOperator<TEntity> oper)
    {
        return default;
    }
    public static IOrOperator<TEntity> Conditional<TEntity>(this IOrOperator<TEntity> oper)
    {
        return default;
    }    
    public static IAndOrOperator<TEntity> Conditional<TEntity>(this IAndOrOperator<TEntity> oper)
    {
        return default;
    }
}