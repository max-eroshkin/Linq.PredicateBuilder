namespace Research;

using System.Linq.Expressions;

public class Test
{
    public Test()
    {
        var _ = new Start<Order>();
        IResult<Order> expression = _
            .Contains(x => x.Title, "a").And
            .Contains(x => x.Title, "c").And
            .Contains(x => x.Title, "f");
        var exp1 = expression.GetExpression();
        
        IResult<Order> expression2 = _
            .Contains(x => x.Title, "a").Or
            .Contains(x => x.Title, "a").Or
            .Contains(x => x.Title, "a");
        // IResult<TEntity> expression3 = _
        //     .Conditional().Contains()
        //     .Or.Conditional().Conditional().Contains()
        //     .Or.Contains();
        // IResult expression4 = _
        //     .Conditional().Contains()
        //     .And.Conditional().Conditional().Contains()
        //     .And.Contains();
        // IResult expression5 = _.Not.Contains().Or.Not.Contains().Or.Contains();
        // IResult expression6 = _.Not.Contains().And.Conditional().Not.Contains().And.Contains();
        // IResult exp7 = _
        //     .Equals()
        //     .And.Not.Contains()
        //     .And.Not.Equals();
    }
}