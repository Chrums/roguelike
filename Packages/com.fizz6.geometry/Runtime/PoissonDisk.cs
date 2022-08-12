using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Fizz6.Geometry
{
    public static class PoissonDisk
    {
        private const int Limit = 32;
        private const int Unset = -1;
        private const int Range = 2;

        public static float Magnitude(float[] vector)
        {
            var zero = new float[vector.Length];
            return Distance(zero, vector);
        }

        public static float Distance(float[] point0, float[] point1)
        {
            var aggregate = 0.0f;

            var dimensions = Math.Max(point0.Length, point1.Length);
            for (var dimension = 0; dimension < dimensions; ++dimension)
            {
                var value0 = dimension < point0.Length ? point0[dimension] : 0.0f;
                var value1 = dimension < point1.Length ? point1[dimension] : 0.0f;
                var value = value1 - value0;
                aggregate += value * value;
            }

            return Mathf.Sqrt(aggregate);
        }

        public static HashSet<int[]> Bounds(int dimensions, int range)
        {
            var cells = new HashSet<int[]>();

            void Recurse(int[] values = null, int dimension = 0)
            {
                values ??= new int[dimensions];
                for (var offset = -range; offset <= range; ++offset)
                {
                    values[dimension] = offset;
                    var cell = (int[])values.Clone();

                    if (dimension < dimensions - 1)
                        for (var index = dimension + 1; index < dimensions; ++index)
                            Recurse(cell, index);
                    else cells.Add(cell);
                }
            }
            
            Recurse();
            return cells;
        }

        public static HashSet<float[]> Sample(int dimensions = 2, float size = 32.0f, float distance = 1.0f)
        {
            var spacing = distance / Mathf.Sqrt(dimensions);
            var depth = Mathf.CeilToInt(size / spacing);

            var count = (int)Mathf.Pow(depth, dimensions);
            var grid = new int[count];
            Array.Fill(grid, Unset);

            var samples = new List<float[]>();
            var queue = new List<float[]>();

            int CellToIndex(int[] cell)
            {
                var index = 0;
                for (var dimension = 0; dimension < dimensions; ++dimension)
                    index += (int)Mathf.Pow(depth, dimensions - 1 - dimension) * cell[dimension];
                return index;
            }

            int[] PointToCell(float[] point)
            {
                var cell = new int[dimensions];
                for (var dimension = 0; dimension < dimensions; ++dimension)
                    cell[dimension] = Mathf.FloorToInt(point[dimension] / spacing);
                return cell;
            }

            void Queue(float[] point)
            {
                var cell = PointToCell(point);
                var index = CellToIndex(cell);
                grid[index] = samples.Count;
                samples.Add(point);
                queue.Add(point);
            }

            float[] Instantiate(float[] root)
            {
                for (var index = 0; index < Limit; ++index)
                {
                    var delta = new float[dimensions];
                    while (true)
                    {
                        for (var dimension = 0; dimension < dimensions; ++dimension)
                            delta[dimension] = Random.Range(-1.0f, 1.0f) * 2.0f * spacing;
                        var magnitude = Magnitude(delta);
                        if (magnitude >= spacing && magnitude <= spacing * 2.0f) break;
                    }

                    var point = new float[dimensions];
                    for (var dimension = 0; dimension < dimensions; ++dimension)
                        point[dimension] = root[dimension] + delta[dimension];

                    if (point.Any(value => value < 0.0f || value >= size)) continue;

                    return point;
                }

                return null;
            }

            bool Validate(float[] point)
            {
                if (point == null) return false;
                
                var cell = PointToCell(point);
                var bounds = Bounds(dimensions, Range);
                foreach (var bound in bounds)
                {
                    for (var dimension = 0; dimension < dimensions; ++dimension)
                        bound[dimension] += cell[dimension];
                }

                var neighbors = bounds.Where(bound => bound.All(value => value >= 0 && value < depth));
                return neighbors
                    .Select(CellToIndex)
                    .All(index => grid[index] == -1 || !(Distance(point, samples[grid[index]]) < distance));
            }
            
            float[] Generate(float[] point)
            {
                for (var index = 0; index < Limit; ++index)
                {
                    var sample = Instantiate(point);
                    if (Validate(sample)) return sample;
                }

                return null;
            }
            
            var point = new float[dimensions];
            for (var dimension = 0; dimension < dimensions; ++dimension)
                point[dimension] = Random.Range(0.0f, size);

            Queue(point);
            
            while (queue.Count > 0)
            {
                var index = Random.Range(0, queue.Count);
                point = queue[index];
                point = Generate(point);
                if (point != null) Queue(point);
                else queue.RemoveAt(index);
            }

            foreach (var sample in samples)
                for (var dimension = 0; dimension < dimensions; ++dimension)
                    sample[dimension] -= size / 2.0f;
            return new HashSet<float[]>(samples);
        }
    }
}