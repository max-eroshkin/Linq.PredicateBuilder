namespace BuilderTests.Data
{
    using System.Collections.Generic;
    using System.Linq;

    public static class TestData
    {
        static TestData()
        {
            dataSet = new List<Family>
            {
                new Family { Description = "1", Requests = new List<Request> { new Request { ApproverId = 1 } } },
                new Family { Description = "2", Requests = new List<Request> { new Request { ApproverId = 2 } } },
                new Family { Description = "3", Requests = new List<Request> { new Request { ApproverId = 3 } } },
                new Family { Description = "4", Requests = new List<Request> { new Request { ApproverId = 4 } } },
            }.AsQueryable();
        }

        private static IQueryable<Family> dataSet;

        public static IQueryable<Family> DataSet => dataSet;
    }
}