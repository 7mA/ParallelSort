using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ParallelSort
{
    public class Vector
    {
        public double W;
        public double X;
        public double Y;
        public double Z;
        public double T;
    }

    internal class VectorComparer : IComparer<Vector>
    {
        public int Compare(Vector c1, Vector c2)
        {
            if (c1 == null || c2 == null)
                throw new ArgumentNullException("Both objects must not be null");
            double x = Math.Sqrt(Math.Pow(c1.X, 2)
                                 + Math.Pow(c1.Y, 2)
                                 + Math.Pow(c1.Z, 2)
                                 + Math.Pow(c1.W, 2));
            double y = Math.Sqrt(Math.Pow(c2.X, 2)
                                 + Math.Pow(c2.Y, 2)
                                 + Math.Pow(c2.Z, 2)
                                 + Math.Pow(c2.W, 2));
            if (x > y)
                return 1;
            else if (x < y)
                return -1;
            else
                return 0;
        }
    }

    internal class VectorComparer2 : IComparer<Vector>
    {
        public int Compare(Vector c1, Vector c2)
        {
            if (c1 == null || c2 == null)
                throw new ArgumentNullException("Both objects must not be null");
            if (c1.T > c2.T)
                return 1;
            else if (c1.T < c2.T)
                return -1;
            else
                return 0;
        }
    }

    class Program
    {
        private static void Print(Vector[] vectors)
        {
            //foreach (Vector v in vectors)
            //{
            //    Console.WriteLine(v.T);
            //}
        }

        private static void Main(string[] args)
        {
            Vector[] vectors = GetVectors();

            Console.WriteLine(string.Format("n = {0}", vectors.Length));

            Stopwatch watch1 = new Stopwatch();
            watch1.Start();
            A(vectors);
            watch1.Stop();
            Console.WriteLine("A sort time: " + watch1.Elapsed);
            Print(vectors);

            vectors = GetVectors();
            watch1.Reset();
            watch1.Start();
            B(vectors);
            watch1.Stop();
            Console.WriteLine("B sort time: " + watch1.Elapsed);
            Print(vectors);

            vectors = GetVectors();
            watch1.Reset();
            watch1.Start();
            C(vectors);
            watch1.Stop();
            Console.WriteLine("C sort time: " + watch1.Elapsed);
            Print(vectors);

            vectors = GetVectors();
            watch1.Reset();
            watch1.Start();
            D(vectors);
            watch1.Stop();
            Console.WriteLine("D sort time: " + watch1.Elapsed);
            Print(vectors);

            Console.ReadKey();
        }

        private static Vector[] GetVectors()
        {
            int n = 1 << 21;
            Vector[] vectors = new Vector[n];
            Random random = new Random();

            for (int i = 0; i < n; i++)
            {
                vectors[i] = new Vector();
                vectors[i].X = random.NextDouble();
                vectors[i].Y = random.NextDouble();
                vectors[i].Z = random.NextDouble();
                vectors[i].W = random.NextDouble();
            }
            return vectors; 
        }

        private static void A(Vector[] vectors)
        {
            Array.Sort(vectors, new VectorComparer());
        }

        private static void B(Vector[] vectors)
        {
            int n = vectors.Length;
            for (int i = 0; i < n; i++)
            {
                Vector c1 = vectors[i];
                c1.T = Math.Sqrt(Math.Pow(c1.X, 2)
                                 + Math.Pow(c1.Y, 2)
                                 + Math.Pow(c1.Z, 2)
                                 + Math.Pow(c1.W, 2));
            }
            Array.Sort(vectors, new VectorComparer2());
        }

        private static void C(Vector[] vectors)
        {
            int n = vectors.Length;
            for (int i = 0; i < n; i++)
            {
                Vector c1 = vectors[i];
                c1.T = Math.Sqrt(c1.X * c1.X
                                 + c1.Y * c1.Y
                                 + c1.Z * c1.Z
                                 + c1.W * c1.W);
            }
            Array.Sort(vectors, new VectorComparer2());
        }

        private static void D(Vector[] vectors)
        {
            int n = vectors.Length;
            for (int i = 0; i < n; i++)
            {
                Vector c1 = vectors[i];
                c1.T = Math.Sqrt(c1.X * c1.X
                                 + c1.Y * c1.Y
                                 + c1.Z * c1.Z
                                 + c1.W * c1.W);
            }

            ParallelSort<Vector> parallelSort = new ParallelSort<Vector>();
            parallelSort.Sort(vectors, new VectorComparer2());
        }
    }
}
