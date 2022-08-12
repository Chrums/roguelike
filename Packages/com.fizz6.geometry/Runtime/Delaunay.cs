using System.Collections.Generic;
using System.Linq;
using DelaunatorSharp;
using UnityEngine;

public static class Delaunay
{
    public static Delaunator Triangulate(HashSet<Vector2> points)
    {
        var values = points
            .Select(point => new Point(point.x, point.y))
            .Cast<IPoint>()
            .ToArray();
        return new Delaunator(values);
    }
}