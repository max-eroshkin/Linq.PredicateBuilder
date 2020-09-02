namespace BuilderTests.Data
{
    using System.Collections.Generic;
    using System.Linq;

    public static class TestData
    {
        static TestData()
        {
            dataSet = new List<Item>
            {
                new Item { Id = 1, Name = "aaAa1", Description = "1", Requests = new List<Request> { new Request { ApproverId = 1 } } },
                new Item { Id = 2, Name = "aAaa2", Description = "2", Requests = new List<Request> { new Request { ApproverId = 2 } } },
                new Item { Id = 3, Name = "baBa", Description = "3", Requests = new List<Request> { new Request { ApproverId = 3 } } },
                new Item { Id = 4, Name = "Baba", Description = "4", Requests = new List<Request> { new Request { ApproverId = 4 } } },
            }.AsQueryable();
        }

        private static IQueryable<Item> dataSet;

        public static IQueryable<Item> DataSet => dataSet;
    }
}