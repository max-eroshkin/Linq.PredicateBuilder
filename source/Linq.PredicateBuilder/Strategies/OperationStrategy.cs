namespace Linq.PredicateBuilder
{
    using System.Collections;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <inheritdoc />
    public class OperationStrategy : IOperationStrategy
    {
        private static readonly MethodInfo ToLowerMethod = typeof(string).GetMethods()
            .First(x => x.Name == nameof(string.ToLower) && x.ToString() == "System.String ToLower()");

        private static readonly MethodInfo ContainsMethod =
            typeof(string)
                .GetMethods()
                .First(x => x.Name == nameof(string.Contains) && x.ToString() == "Boolean Contains(System.String)");

        private static readonly MethodInfo CollectionContainsMethod =
            typeof(Enumerable)
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .First(x => x.Name == "Contains" && x.GetParameters().Length == 2);

        private static readonly MethodInfo AnyMethod =
            typeof(Enumerable)
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .First(x => x.Name == "Any" && x.GetParameters().Length == 2);

        private readonly BuilderOptions _options;

        /// <summary>
        /// Constructs a strategy.
        /// </summary>
        /// <param name="options">A filtering options.</param>
        public OperationStrategy(BuilderOptions options = BuilderOptions.Default)
        {
            _options = options;
        }

        private bool IgnoreDefaults => (_options & BuilderOptions.IgnoreDefaultInputs) != 0;

        private bool IgnoreCase => (_options & BuilderOptions.IgnoreCase) != 0;

        private bool TrimStrings => (_options & BuilderOptions.Trim) != 0;

        /// <inheritdoc />
        public Expression<Func<TEntity, bool>>? Contains<TEntity>(
            Expression<Func<TEntity, string>> propertyExpression,
            string? input)
        {
            if (SegmentIgnored(input))
                return null;

            _ = input ?? throw new ArgumentNullException(nameof(input), "Input cannot be null.");

            var predicate = Expression.Lambda<Func<TEntity, bool>>(
                Expression.Call(
                    PreprocessSelector<string>(propertyExpression.Body),
                    ContainsMethod,
                    Expression.Constant(PreprocessInput(input), typeof(string))),
                propertyExpression.Parameters);

            return predicate;
        }

        /// <inheritdoc />
        public Expression<Func<TEntity, bool>>? Equals<TEntity, TInput>(
            Expression<Func<TEntity, TInput>> propertyExpression,
            TInput? input)
        {
            if (SegmentIgnored(input))
                return null;

            var predicate = Expression.Lambda<Func<TEntity, bool>>(
                Expression.Equal(
                    PreprocessSelector<TInput>(propertyExpression.Body),
                    Expression.Constant(PreprocessInput(input), typeof(TInput))),
                propertyExpression.Parameters);

            return predicate;
        }

        /// <inheritdoc />
        public Expression<Func<TEntity, bool>>? In<TEntity, TInput>(
            Expression<Func<TEntity, TInput>> propertyExpression,
            IEnumerable<TInput>? input)
        {
            _ = propertyExpression ??
                throw new ArgumentNullException(nameof(propertyExpression), "Expression cannot be null.");

            if (SegmentIgnored(input))
                return null;

            _ = input ?? throw new ArgumentNullException(nameof(input), "Input cannot be null.");

            var inputParameter = Expression.Constant(input.Select(PreprocessInput));

            var predicate = Expression.Lambda<Func<TEntity, bool>>(
                Expression.Call(
                    null,
                    CollectionContainsMethod.MakeGenericMethod(typeof(TInput)),
                    inputParameter,
                    PreprocessSelector<TInput>(propertyExpression.Body)),
                propertyExpression.Parameters);

            return predicate;
        }

        /// <inheritdoc />
        public Expression<Func<TEntity, bool>>? Any<TEntity, TInput>(
            Expression<Func<TEntity, IEnumerable<TInput>>> collectionSelector,
            Expression<Func<TInput, bool>>? predicate)
        {
            _ = collectionSelector ??
                throw new ArgumentNullException(nameof(collectionSelector), "Expression cannot be null");

            if (predicate == null)
                return null;

            var anyMethod = AnyMethod.MakeGenericMethod(typeof(TInput));

            var result = Expression.Lambda<Func<TEntity, bool>>(
                Expression.Call(
                    null,
                    anyMethod,
                    collectionSelector.Body,
                    predicate),
                collectionSelector.Parameters);

            return result;
        }

        /// <inheritdoc />
        public Expression<Func<TEntity, bool>>? Where<TEntity, TInput>(
            Expression<Func<TEntity, TInput, bool>> predicate,
            TInput? input)
        {
            if (SegmentIgnored(input))
                return null;

            var replacement = new Dictionary<ParameterExpression, Expression>
            {
                { predicate.Parameters.Last(), Expression.Constant(PreprocessInput(input), typeof(TInput)) }
            };

            var body = ParameterRebinder.ReplaceParameters(
                replacement,
                predicate.Body);

            var result = Expression.Lambda<Func<TEntity, bool>>(
                body,
                predicate.Parameters.First());

            return result;
        }

        /// <inheritdoc />
        public bool SegmentIgnored<TInput>(TInput input)
        {
            if (!IgnoreDefaults)
                return false;
     
            return input switch
            {
                null => true,
                string val => string.IsNullOrWhiteSpace(val),
                IEnumerable val => !EnumerableAny(val),
                _ => input.Equals(default(TInput))
            };

            static bool EnumerableAny(IEnumerable enumerable)
            {
                var enumerator = enumerable.GetEnumerator();
                var disposable = enumerator as IDisposable;
                try
                {
                    return enumerator.MoveNext();
                }
                finally
                {
                    disposable?.Dispose();
                }
            }
        }

        /// <inheritdoc />
        public TInput PreprocessInput<TInput>(TInput input)
        {
            return input switch
            {
                string val => (TInput)(object)PreprocessString(val),
                _ => input
            };
        }

        /// <inheritdoc />
        public Expression PreprocessSelector<TInput>(Expression propertyExpression)
        {
            return IgnoreCase && typeof(TInput) == typeof(string)
                ? Expression.Call(propertyExpression, ToLowerMethod)
                : propertyExpression;
        }

        private string PreprocessString(string input)
        {
            if (TrimStrings)
                input = input.Trim();

            return IgnoreCase
                ? input.ToLower()
                : input;
        }
    }
}