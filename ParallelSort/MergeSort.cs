using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelSort
{
    class MergeSort<T>
    {
        public void Sort(T[] destArray, List<T[]> source, IComparer<T> comparer)
        {
            //Merge Sort  
            int[] mergePoint = new int[source.Count];

            for (int i = 0; i < source.Count; i++)
            {
                mergePoint[i] = 0;
            }

            int index = 0;

            while (index < destArray.Length)
            {
                int min = -1;

                for (int i = 0; i < source.Count; i++)
                {
                    if (mergePoint[i] >= source[i].Length)
                    {
                        continue;
                    }

                    if (min < 0)
                    {
                        min = i;
                    }
                    else
                    {
                        if (comparer.Compare(source[i][mergePoint[i]], source[min][mergePoint[min]]) < 0)
                        {
                            min = i;
                        }
                    }
                }

                if (min < 0)
                {
                    continue;
                }

                destArray[index++] = source[min][mergePoint[min]];
                mergePoint[min]++;
            }
        }
    }
}
