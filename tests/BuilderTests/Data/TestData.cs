namespace BuilderTests.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Samples;

    public static class TestData
    {
        public static readonly IQueryable<Item> DataSet;
        public static readonly IQueryable<Person> Persons;

        static TestData()
        {
            DataSet = new List<Item>
            {
                new Item
                {
                    Id = 1, Name = "aaAa1", ParentId = null, Description = "1",
                    Requests = new List<Request> { new Request { ApproverId = 1 } }
                },
                new Item
                {
                    Id = 2, Name = "aAaa2", ParentId = null, Description = "2",
                    Requests = new List<Request> { new Request { ApproverId = 2 } }
                },
                new Item
                {
                    Id = 3, Name = "baBa", ParentId = 1, Description = "3",
                    Requests = new List<Request> { new Request { ApproverId = 3 } }
                },
                new Item
                {
                    Id = 4, Name = "Baba", ParentId = 2, Description = "4",
                    Requests = new List<Request> { new Request { ApproverId = 4 } }
                },
            }.AsQueryable();
            
            var persons = new Person[]
            {
                new() { Id = 1, FirstName = "John", LastName = "Smith", DateOfBirth = new DateOnly(1977, 5, 13), Gender = Gender.Male },
                new() { Id = 2, FirstName = "Barbara", LastName = "Johnson", DateOfBirth = new DateOnly(1985, 1, 1), Gender = Gender.Female },
                new() { Id = 3, FirstName = "Michael", LastName = "Brown", DateOfBirth = new DateOnly(1997, 11, 20), Gender = Gender.Male },
                new() { Id = 4, FirstName = "Charles", LastName = "Walker", DateOfBirth = new DateOnly(1981, 8, 18), Gender = Gender.Male },
                new() { Id = 5, FirstName = "Maria", LastName = "Carter", DateOfBirth = new DateOnly(2000, 12, 7), Gender = Gender.Female, Comment = "This is comment" },
                new() { Id = 6, FirstName = "Catherine", LastName = "Walker", DateOfBirth = new DateOnly(1987, 5, 3), Gender = Gender.Female },
                new() { Id = 7, FirstName = "Diane", LastName = "Brown", DateOfBirth = new DateOnly(2001, 2, 3), Gender = Gender.Female },
                new() { Id = 8, FirstName = "Catherine", LastName = "Campbell", DateOfBirth = new DateOnly(2003, 4, 3), Gender = Gender.Female },
            };
            persons.Single(x => x.Id == 3).Relatives = new[] { persons.Single(x => x.Id == 7) };
            persons.Single(x => x.Id == 7).Relatives = new[] { persons.Single(x => x.Id == 3) };
            persons.Single(x => x.Id == 6).Relatives = new[] { persons.Single(x => x.Id == 8) };
            persons.Single(x => x.Id == 8).Relatives = new[] { persons.Single(x => x.Id == 6) };
            Persons = persons.AsQueryable();
        }
    }
}