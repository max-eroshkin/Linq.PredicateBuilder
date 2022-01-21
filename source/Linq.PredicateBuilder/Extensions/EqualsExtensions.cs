namespace Linq.PredicateBuilder;

using System.Linq.Expressions;

public static class EqualsExtensions
{
    private static Result<TEntity> GetResultInternal<TEntity, TValue>(
        IOperator<TEntity> oper,
        Expression<Func<TEntity, TValue>> propertyExpression,
        TValue input, 
        BuilderOptions? options = null)
    {
        var strategy = options == null 
            ? oper.Strategy 
            : new OperationStrategy(options.Value);
        var expression = strategy.Equals(propertyExpression, input);
        
        return new Result<TEntity>(oper.GetExpression(expression), oper.Strategy);
    }
    
    public static IAndResult<TEntity> Equals<TEntity, TValue>(
        this IAndOperator<TEntity> oper,
        Expression<Func<TEntity, TValue>> propertyExpression,
        TValue input, 
        BuilderOptions? options = null)
        => GetResultInternal(oper, propertyExpression, input, options);
    
    public static IOrResult<TEntity> Equals<TEntity, TValue>(
        this IOrOperator<TEntity> oper,
        Expression<Func<TEntity, TValue>> propertyExpression,
        TValue input, 
        BuilderOptions? options = null)
        => GetResultInternal(oper, propertyExpression, input, options);
    
    public static IFullResult<TEntity> Equals<TEntity, TValue>(
        this IAndOrOperator<TEntity> oper,
        Expression<Func<TEntity, TValue>> propertyExpression,
        TValue input, 
        BuilderOptions? options = null)
        => GetResultInternal(oper, propertyExpression, input, options);
}