namespace Linq.PredicateBuilder;

using System.Linq.Expressions;

public static class ConditionalWhere
{
    private static Result<TEntity> GetResultInternal<TEntity, TInput>(
        IOperator<TEntity> oper,
        Expression<Func<TEntity, TInput, bool>> predicate,
        TInput? input,  
        BuilderOptions? options = null)
    {
        var strategy = options == null 
            ? oper.Strategy 
            : new OperationStrategy(options.Value);
        var expression = strategy.Where(predicate, input);
        return new(oper.GetExpression(expression), oper.Strategy);
    }

    public static IAndResult<TEntity> Where<TEntity, TInput>(
        this IAndOperator<TEntity> oper,
        Expression<Func<TEntity, TInput, bool>> predicate,
        TInput? input,  
        BuilderOptions? options = null)
        => GetResultInternal(oper, predicate, input, options);

    public static IOrResult<TEntity> Where<TEntity, TInput>(
        this IOrOperator<TEntity> oper,
        Expression<Func<TEntity, TInput, bool>> predicate,
        TInput? input,  
        BuilderOptions? options = null)
        => GetResultInternal(oper, predicate, input, options);

    public static IFullResult<TEntity> Where<TEntity, TInput>(
        this IAndOrOperator<TEntity> oper,
        Expression<Func<TEntity, TInput, bool>> predicate,
        TInput? input,  
        BuilderOptions? options = null)
        => GetResultInternal(oper, predicate, input, options);
}