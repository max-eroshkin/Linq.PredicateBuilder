﻿// <auto-generated />

namespace BuilderTests.Data
{
    using System.Collections.Generic;

    public class Item
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public long? ParentId { get; set; }

        public string Description { get; set; }

        public List<Request> Requests { get; set; }
    }

    public class Request
    {
        public long Id { get; set; }

        public long ApproverId { get; set; }

        public long ItemId { get; set; }
    }
}