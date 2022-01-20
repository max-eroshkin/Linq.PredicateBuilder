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
        /// No options
        /// </summary>
        None = 0,

        /// <summary>
        /// IgnoreCase + IgnoreDefaultInputs + Trim
        /// </summary>
        Default = IgnoreCase | IgnoreDefaultInputs | Trim,

        /// <summary>
        /// Ignore case in string operations.
        /// </summary>
        IgnoreCase = 1,

        /// <summary>
        /// Ignore default input values.
        /// </summary>
        IgnoreDefaultInputs = 1 << 1,

        /// <summary>
        /// Trim string input values.
        /// </summary>
        Trim = 1 << 2
    }
}