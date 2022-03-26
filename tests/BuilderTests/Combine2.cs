namespace BuilderTests;

using System.Linq;
using FluentAssertions;
using Linq.PredicateBuilder;
using Xunit;
using static Data.TestData;

public class Combine2
{
    [Fact]
    public void CombineAnd()
    {
        var result = DataSet.Build(
            _ => _
                .Where(x => x.Id > 1).And
                .Where(x => x.Name.Contains("A")));

        result.Should().BeEquivalentTo(DataSet.Where(x => x.Id > 1 && x.Name.Contains("A")));
    }
    
    [Fact]
    public void CombineAnd1()
    {
        var result = DataSet.Build(
            _ => _
                .Conditional(false).Where(x => x.Id > 1).And
                .Where(x => x.Name.Contains("A")));

        result.Should().BeEquivalentTo(DataSet.Where(x => x.Name.Contains("A")));
    }
    
    [Fact]
    public void CombineAnd2()
    {
        var result = DataSet.Build(
            _ => _
                .Where(x => x.Id > 1).And
                .Conditional(false).Where(x => x.Name.Contains("A")));

        result.Should().BeEquivalentTo(DataSet.Where(x => x.Id > 1));
    }
    
    [Fact]
    public void CombineAnd3()
    {
        var result = DataSet.Build(
            _ => _
                .Conditional(false).Where(x => x.Id > 1).And
                .Conditional(false).Where(x => x.Name.Contains("A")));

        result.Should().BeEquivalentTo(DataSet);
    }
    
    [Fact]
    public void CombineOr()
    {
        var result = DataSet.Build(
            _ => _
                .Where(x => x.Id > 3).Or
                .Where(x => x.Name.Contains("A")));

        result.Should().BeEquivalentTo(DataSet.Where(x => x.Id > 3 || x.Name.Contains("A")));
    }
    
    [Fact]
    public void CombineOr1()
    {
        var result = DataSet.Build(
            _ => _
                .Conditional(false).Where(x => x.Id > 3).Or
                .Where(x => x.Name.Contains("A")));

        result.Should().BeEquivalentTo(DataSet.Where(x => x.Name.Contains("A")));
    }
    
    [Fact]
    public void CombineOr2()
    {
        var result = DataSet.Build(
            _ => _
                .Conditional(true).Where(x => x.Id > 3).Or
                .Conditional(false).Where(x => x.Name.Contains("A")));

        result.Should().BeEquivalentTo(DataSet.Where(x => x.Id > 3));
    }
    
    [Fact]
    public void CombineOr3()
    {
        var result = DataSet.Build(
            _ => _
                .Conditional(false).Where(x => x.Id > 3).Or
                .Conditional(false).Where(x => x.Name.Contains("A")));

        result.Should().BeEquivalentTo(DataSet);
    }

    [Fact]
    public void CombineNot()
    {
        var result = DataSet.Build(
            _ => _
                .Conditional(true).Not.Where(x => x.Id > 3));

        result.Should().BeEquivalentTo(DataSet.Where(x => !(x.Id > 3)));
    }  
    
    [Fact]
    public void CombineNot1()
    {
        var result = DataSet.Build(
            _ => _
                .Not.Conditional(false).Where(x => x.Id > 3));

        result.Should().BeEquivalentTo(DataSet);
    }
}