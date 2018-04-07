using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ParallelSort
{
    class ParallelSort<T>
    {
        enum Status
        {
            Idle = 0,
            Running = 1,
            Finish = 2,
        }

        class ParallelEntity
        {
            public Status Status;
            public T[] Array;
            public IComparer<T> Comparer;

            public ParallelEntity(Status status, T[] array, IComparer<T> comparer)
            {
                Status = status;
                Array = array;
                Comparer = comparer;
            }
        }

        private void ThreadProc(Object stateInfo)
        {
            ParallelEntity pe = stateInfo as ParallelEntity;

            lock (pe)
            {
                pe.Status = ParallelSort<T>.Status.Running;

                Array.Sort(pe.Array, pe.Comparer);

                pe.Status = ParallelSort<T>.Status.Finish;
            }
        }

        public void Sort(T[] array, IComparer<T> comparer)
        {
            //Calculate process count 
            int processorCount = Environment.ProcessorCount;

            //If array.Length too short, do not use Parallel sort
            if (processorCount == 1 || array.Length < processorCount)
            {
                Array.Sort(array, comparer);
                return;
            }

            //Split array 
            ParallelEntity[] partArray = new ParallelEntity[processorCount];

            int remain = array.Length;
            int partLen = array.Length / processorCount;

            //Copy data to splited array
            for (int i = 0; i < processorCount; i++)
            {
                if (i == processorCount - 1)
                {
                    partArray[i] = new ParallelEntity(Status.Idle, new T[remain], comparer);
                }
                else
                {
                    partArray[i] = new ParallelEntity(Status.Idle, new T[partLen], comparer);

                    remain -= partLen;
                }

                Array.Copy(array, i * partLen, partArray[i].Array, 0, partArray[i].Array.Length);
            }

            //Parallel sort
            for (int i = 0; i < processorCount - 1; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadProc), partArray[i]);
            }

            ThreadProc(partArray[processorCount - 1]);

            //Wait all threads finish
            for (int i = 0; i < processorCount; i++)
            {
                while (true)
                {
                    lock (partArray[i])
                    {
                        if (partArray[i].Status == ParallelSort<T>.Status.Finish)
                        {
                            break;
                        }
                    }

                    Thread.Sleep(0);
                }
            }

            //Merge sort
            MergeSort<T> mergeSort = new MergeSort<T>();

            List<T[]> source = new List<T[]>(processorCount);

            foreach (ParallelEntity pe in partArray)
            {
                source.Add(pe.Array);
            }

            mergeSort.Sort(array, source, comparer);
        }
    }
}
