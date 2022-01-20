using Xunit;

namespace ResearchTests;

using System;
using System.Linq.Expressions;
using Research;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var _ = new Start<Order>();
        IResult<Order> expression = _
            .Contains(x => x.Title, "a").And
            .Contains(x => x.Title, "C").And
            .Contains(x => x.Title, "f");
        var exp1 = expression.GetExpression();
    }
    
    [Fact]
    public void Test2()
    {
        var _ = new Start<Order>();
        IResult<Order> expression = _
            .Not.Contains(x => x.Title, "a").And
            .Contains(x => x.Title, "C").And
            .Not.Contains(x => x.Title, "f");
        var exp1 = expression.GetExpression();
    }
    
    [Fact]
    public void Test3()
    {
        var _ = new Start<Order>();
        IResult<Order> expression = _
            .Conditional(true).Not.Contains(x => x.Title, "a").And
            .Contains(x => x.Title, "C").And
            .Not.Conditional(true).Contains(x => x.Title, "f");
        var exp1 = expression.GetExpression();
    }
}