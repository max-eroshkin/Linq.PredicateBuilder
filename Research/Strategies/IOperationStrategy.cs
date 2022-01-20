﻿namespace Linq.PredicateBuilder;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

/// <summary>
/// Contains filtering methods.
/// </summary>
public interface IOperationStrategy
{
    /// <summary>
    /// Contains predicate
    /// </summary>
    /// <param name="propertyExpression">Property selector</param>
    /// <param name="input">string to find</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <returns></returns>
    Expression<Func<TEntity, bool>> Contains<TEntity>(
        Expression<Func<TEntity, string>> propertyExpression,
        string? input);

    /// <summary>
    /// Contains predicate
    /// </summary>
    /// <param name="propertyExpression">Property selector</param>
    /// <param name="input">string to find</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <returns></returns>
    Expression<Func<TEntity, bool>>? StringEquals<TEntity>(
        Expression<Func<TEntity, string>> propertyExpression,
        string? input);

    /// <summary>
    /// Contains predicate
    /// </summary>
    /// <param name="propertyExpression">Property selector</param>
    /// <param name="input">string to find</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TValue">Тип свойства</typeparam>
    Expression<Func<TEntity, bool>>? Equals<TEntity, TValue>(
        Expression<Func<TEntity, TValue>> propertyExpression,
        TValue input);

    /// <summary>
    /// Contains predicate
    /// </summary>
    /// <param name="propertyExpression">Property selector</param>
    /// <param name="input">string to find</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TValue">Тип свойства</typeparam>
    Expression<Func<TEntity, bool>>? In<TEntity, TValue>(
        Expression<Func<TEntity, TValue>> propertyExpression,
        IEnumerable<TValue>? input);

    /// <summary>
    /// Contains predicate
    /// </summary>
    /// <param name="collectionSelector">Many-to-many Property selector</param>
    /// <param name="predicate">Property selector</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TValue">Тип свойства коллекции many-to-many</typeparam>
    Expression<Func<TEntity, bool>>? Any<TEntity, TValue>(
        Expression<Func<TEntity, ICollection<TValue>>> collectionSelector,
        Expression<Func<TValue, bool>>? predicate);
}