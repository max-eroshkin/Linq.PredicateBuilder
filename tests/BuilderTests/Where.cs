namespace BuilderTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Linq.PredicateBuilder;
    using Xunit;
    using static Data.TestData;

    public class Where
    {
        [Fact]
        void NumberGt()
        {
            var result = DataSet.FromBuilder(
                _ => _.Where(x => x.Id > 1));

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id > 1));
        }
    }
}