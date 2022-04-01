﻿namespace BuilderTests.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
            
            Persons = new Person[]
            {
                new(1, "John", "Smith", new DateOnly(1977, 5, 13), Gender.Male),
                new(2, "Barbara", "Johnson", new DateOnly(1985, 1, 1), Gender.Male),
                new(3, "Michael", "Brown", new DateOnly(1997, 11, 20), Gender.Male),
                new(4, "Charles", "Walker", new DateOnly(1981, 8, 18), Gender.Male),
                new(5, "Maria", "Carter", new DateOnly(2000, 12, 7), Gender.Male),
                new(6, "Catherine", "Walker", new DateOnly(1987, 5, 3), Gender.Male),
                new(7, "Diane", "Brown", new DateOnly(2001, 2, 3), Gender.Male),
                new(8, "Catherine", "Campbell", new DateOnly(2003, 4, 3), Gender.Male),
            }.AsQueryable();
        }
    }
}