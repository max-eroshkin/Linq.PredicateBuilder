namespace Research;

using System.Linq.Expressions;

public static class ContainsExtensions
{
    private static Result<TEntity> GetResultInternal<TEntity>(
        IOperator<TEntity> oper,
        Expression<Func<TEntity, string>> propertyExpression,
        string? input, 
        BuilderOptions? options = null)
    {
        var strategy = options == null 
            ? oper.Strategy 
            : new OperationStrategy(options.Value);
        var expression = strategy.Contains(propertyExpression, input);
        
        return new Result<TEntity>(oper.GetExpression(expression), oper.Strategy);
    }

    public static IAndResult<TEntity> Contains<TEntity>(
        this IAndOperator<TEntity> oper,
        Expression<Func<TEntity, string>> propertyExpression,
        string? input,
        BuilderOptions? options = null)
        => GetResultInternal(oper, propertyExpression, input, options);

    public static IOrResult<TEntity> Contains<TEntity>(
        this IOrOperator<TEntity> oper,
        Expression<Func<TEntity, string>> propertyExpression,
        string? input,
        BuilderOptions? options = null)
        => GetResultInternal(oper, propertyExpression, input, options);

    public static IFullResult<TEntity> Contains<TEntity>(
        this IAndOrOperator<TEntity> oper,
        Expression<Func<TEntity, string>> propertyExpression,
        string? input,
        BuilderOptions? options = null)
        => GetResultInternal(oper, propertyExpression, input, options);
}