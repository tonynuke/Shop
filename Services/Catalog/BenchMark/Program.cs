using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace BenchMark
{
    public sealed class Point3D
    {
        public Point3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; }
        public int Y { get; }
        public int Z { get; }
    }

    public readonly struct Point3DStruct
    {
        public Point3DStruct(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; }
        public int Y { get; }
        public int Z { get; }
    }

    [SimpleJob(RuntimeMoniker.Net50)]
    [MemoryDiagnoser]
    [RPlotExporter]
    public class Tests
    {
        public int N = 100_000;

        private static void Process(IEnumerable<Point3D> points)
        {
            var r = points.GroupBy(x => x.X).ToList();
        }

        private static void Process(IEnumerable<Point3DStruct> points)
        {
            var r = points.GroupBy(x => x.X).ToList();
        }

        [Benchmark]
        public void Test()
        {
            var x = Enumerable.Range(0, N).Select(i => new Point3D(i, i + 1, i + 2));
            Process(x);
        }

        [Benchmark]
        public void TestStruct()
        {
            var x = Enumerable.Range(0, N).Select(i => new Point3DStruct(i, i + 1, i + 2));
            Process(x);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}
