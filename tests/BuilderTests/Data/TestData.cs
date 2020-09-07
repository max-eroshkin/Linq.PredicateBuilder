namespace BuilderTests.Data
{
    using System.Collections.Generic;
    using System.Linq;

    public static class TestData
    {
        public static readonly IQueryable<Item> DataSet;

        static TestData()
        {
            DataSet = new List<Item>
            {
                new Item { Id = 1, Name = "aaAa1", ParentId = null, Description = "1", Requests = new List<Request> { new Request { ApproverId = 1 } } },
                new Item { Id = 2, Name = "aAaa2", ParentId = null, Description = "2", Requests = new List<Request> { new Request { ApproverId = 2 } } },
                new Item { Id = 3, Name = "baBa", ParentId = 1, Description = "3", Requests = new List<Request> { new Request { ApproverId = 3 } } },
                new Item { Id = 4, Name = "Baba", ParentId = 2, Description = "4", Requests = new List<Request> { new Request { ApproverId = 4 } } },
            }.AsQueryable();
        }

        ////public static IQueryable<Item> DataSet => _dataSet;
    }
}