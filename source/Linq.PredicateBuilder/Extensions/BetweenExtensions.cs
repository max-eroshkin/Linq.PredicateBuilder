namespace Linq.PredicateBuilder;

using System.Linq.Expressions;

/// <summary>
/// Contains Between methods.
/// </summary>
public static class BetweenExtensions
{
    /// <summary>
    /// Builds a predicate indicating whether a property value is between two values.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="propertyExpression">The property selector expression.</param>
    /// <param name="input1">The first range boundary.</param>
    /// <param name="input2">The second range boundary.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInput">The property and input value type.</typeparam>
    public static IAndResult<TEntity> Between<TEntity, TInput>(
        this IAndOperator<TEntity> oper,
        Expression<Func<TEntity, TInput>> propertyExpression,
        TInput? input1,
        TInput? input2)
        where TInput : struct, IComparable<TInput>
        => GetResultInternal(oper, propertyExpression, input1, input2);

    /// <summary>
    /// Builds a predicate indicating whether a property value is between two values.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="propertyExpression">The property selector expression.</param>
    /// <param name="input1">The first range boundary.</param>
    /// <param name="input2">The second range boundary.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInput">The property and input value type.</typeparam>
    public static IOrResult<TEntity> Between<TEntity, TInput>(
        this IOrOperator<TEntity> oper,
        Expression<Func<TEntity, TInput>> propertyExpression,
        TInput? input1,
        TInput? input2)
        where TInput : struct, IComparable<TInput>
        => GetResultInternal(oper, propertyExpression, input1, input2);

    /// <summary>
    /// Builds a predicate indicating whether a property value is between two values.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="propertyExpression">The property selector expression.</param>
    /// <param name="input1">The first range boundary.</param>
    /// <param name="input2">The second range boundary.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInput">The property and input value type.</typeparam>
    public static IFullResult<TEntity> Between<TEntity, TInput>(
        this IAndOrOperator<TEntity> oper,
        Expression<Func<TEntity, TInput>> propertyExpression,
        TInput? input1,
        TInput? input2)
        where TInput : struct, IComparable<TInput>
        => GetResultInternal(oper, propertyExpression, input1, input2);

    private static Result<TEntity> GetResultInternal<TEntity, TInput>(
        IOperator<TEntity> oper,
        Expression<Func<TEntity, TInput>> propertyExpression,
        TInput? input1,
        TInput? input2)
        where TInput : struct, IComparable<TInput>
    {
        var strategy = oper.Strategy;
        if (input1 == null! || input2 == null!)
            return new(oper.GetExpression(null), oper.Strategy);
        
        var lower = input1.Value.CompareTo(input2.Value) <= 0 ? input1 : input2;
        var upper = input1.Value.CompareTo(input2.Value) <= 0 ? input2 : input1;

        var selector = strategy.PreprocessSelector<TInput>(propertyExpression.Body);
        var lowerValue = Expression.Constant(strategy.PreprocessInput(lower), typeof(TInput));
        var upperValue = Expression.Constant(strategy.PreprocessInput(upper), typeof(TInput));

        var greaterOrEqual = Expression.GreaterThanOrEqual(
            Expression.Call(selector, nameof(IComparable<TInput>.CompareTo), null, lowerValue),
            Expression.Constant(0));
        
        var lowerOrEqual = Expression.LessThanOrEqual(
            Expression.Call(selector, nameof(IComparable<TInput>.CompareTo), null, upperValue),
            Expression.Constant(0));

        var predicate = Expression.Lambda<Func<TEntity, bool>>(
            Expression.AndAlso(greaterOrEqual, lowerOrEqual),
            propertyExpression.Parameters);

        return new(oper.GetExpression(predicate), oper.Strategy);
    }
}
