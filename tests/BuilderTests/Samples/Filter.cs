namespace BuilderTests.Samples;

using System;
using System.Collections.Generic;

public class Filter
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Comment { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public Gender? Gender { get; set; }

    public List<int>? Ids { get; set; }

    public bool? HasRelatives { get; set; }
}