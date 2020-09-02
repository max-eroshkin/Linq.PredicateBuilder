namespace Linq.PredicateBuilder
{
    using System;

    /// <summary>
    /// Contains predicate builder options.
    /// </summary>
    [Flags]
    public enum BuilderOptions
    {
        /// <summary>
        /// Ignore case in string operations.
        /// </summary>
        IgnoreCase = 1,

        /// <summary>
        /// Ignore default values.
        /// </summary>
        IgnoreDefault = 2
    }
}