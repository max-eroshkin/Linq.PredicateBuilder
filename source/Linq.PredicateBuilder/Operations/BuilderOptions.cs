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
        None = 0x0,

        /// <summary>
        /// IgnoreCase + IgnoreDefaultInputs + Trim
        /// </summary>
        Default = 0x1 | 0x2 | 0x4,

        /// <summary>
        /// Ignore case in string operations.
        /// </summary>
        IgnoreCase = 0x1,

        /// <summary>
        /// Ignore default input values.
        /// </summary>
        IgnoreDefaultInputs = 0x2,

        /// <summary>
        /// Trim string input values.
        /// </summary>
        Trim = 0x4
    }
}