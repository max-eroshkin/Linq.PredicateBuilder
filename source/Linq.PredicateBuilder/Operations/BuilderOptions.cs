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
        /// IgnoreCase + IgnoreDefaultInputs
        /// </summary>
        Default = 0,

        /// <summary>
        /// Ignore case in string operations.
        /// </summary>
        CaseSensitive = 1,

        /// <summary>
        /// Ignore default values.
        /// </summary>
        UseDefaultInputs = 2
    }
}