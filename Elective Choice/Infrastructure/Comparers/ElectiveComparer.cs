using System;
using System.Collections.Generic;
using Elective_Choice.Models;

namespace Elective_Choice.Infrastructure.Comparers;

public class ElectiveComparer : IEqualityComparer<Elective>

{
    public bool Equals(Elective? x, Elective? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.Name == y.Name;
    }

    public int GetHashCode(Elective obj)
    {
        return HashCode.Combine(obj.Name, obj.Capacity, obj.Annotation, obj.Link, obj.Counts, obj.Problem);
    }
}