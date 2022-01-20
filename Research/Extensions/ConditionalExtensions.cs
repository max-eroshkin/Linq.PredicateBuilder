namespace Linq.PredicateBuilder;

public static class ConditionalExtensions
{
    private static IgnogreOperator<TEntity> GetOperatorInternal<TEntity>(IOperator<TEntity> oper, bool condition)
    {
        return new IgnogreOperator<TEntity>(oper, condition);
    }
    
    public static IAndOperator<TEntity> Conditional<TEntity>(this IAndOperator<TEntity> oper, bool condition)
    {
        return GetOperatorInternal(oper, condition);
    }
    
    public static IOrOperator<TEntity> Conditional<TEntity>(this IOrOperator<TEntity> oper, bool condition)
    {
        return GetOperatorInternal(oper, condition);
    }    
    public static IAndOrOperator<TEntity> Conditional<TEntity>(this IAndOrOperator<TEntity> oper, bool condition)
    {
        return GetOperatorInternal(oper, condition);
    }
}