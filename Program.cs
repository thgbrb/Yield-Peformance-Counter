using ByteSizeLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using static System.Console;

namespace YieldPeformance
{
    class Program
    {
        /// <summary>
        /// Stopwatch object
        /// </summary>
        static Stopwatch c = new Stopwatch();
        /// <summary>
        /// Numbers range
        /// </summary>
        static IEnumerable<int> Range = Enumerable.Range(1, 999999999);

        static void Main(string[] args)
        {
            Init("With Yield");
            Count(EvenNumbersWithYield(Range));
            // EvenNumbersWithYield(range).ToList().ForEach(Print);
            Finish();

            Init("Without Yield");
            Count(EvenNumbersWithoutYield(Range));
            // EvenNumbersWithoutYield(range).ToList().ForEach(Print);
            Finish();

            ReadLine();
        }

        /// <summary>
        /// Filter list with Even numbers yielding return
        /// </summary>
        /// <param name="numbers">Numbers list</param>
        /// <returns>IEnumerable of Even numbers</returns>
        static IEnumerable<int> EvenNumbersWithYield(IEnumerable<int> numbers)
        {
            foreach (var n in numbers)
                if (EvenPredicate(n))
                    yield return n;
        }

        /// <summary>
        /// Filter list with Even numbers using a new collection 
        /// </summary>
        /// <param name="numbers">Numbers list</param>
        /// <returns>IEnumerable of Even numbers</returns>
        static IEnumerable<int> EvenNumbersWithoutYield(IEnumerable<int> numbers)
        {
            var result = new Collection<int>();

            foreach (var n in numbers)
                if (EvenPredicate(n))
                    result.Add(n);

            return result;
        }

        /// <summary>
        /// Even number predicate
        /// </summary>
        static Func<int, bool> EvenPredicate = n => n % 2 == 0;

        /// <summary>
        /// Force GC to clean all objects
        /// </summary>
        static Action CallGC = () =>
        {
            GC.Collect(0, GCCollectionMode.Forced, true);
            GC.Collect(1, GCCollectionMode.Forced, true);
            GC.Collect(2, GCCollectionMode.Forced, true);
        };

        /// <summary>
        /// Print actual calculated number
        /// </summary>
        static Action<int> Print = i =>
        {
            WriteLine("Calculating: {0}", i);
            SetCursorPosition(0, CursorTop - 1);
        };

        /// <summary>
        /// Print total elements count of list
        /// </summary>
        static Action<IEnumerable<int>> Count = i =>
        {
            WriteLine("Total of Elements: {0}", i.Count());
            SetCursorPosition(0, CursorTop - 1);
        };

        /// <summary>
        /// Initial Action: Start stopwatch and print initial memory
        /// </summary>
        static Action<string> Init = s =>
         {
             WriteLine(s);
             WriteLine("---------");

             c.Start();

             WriteLine("Initial Memory:    {0}",
                 ByteSize.FromBytes(
                    Process
                    .GetCurrentProcess()
                    .VirtualMemorySize64)
                    .ToString());
         };

        /// <summary>
        /// Finish Action: Stop stopwatch, Print counters and Call forced GC
        /// </summary>
        static Action Finish = () =>
        {
            c.Stop();
            WriteLine();

            WriteLine("End Memory:        {0}",
                ByteSize.FromBytes(
                   Process
                   .GetCurrentProcess()
                   .VirtualMemorySize64)
                   .ToString());

            WriteLine("Peak Memory:       {0}",
                ByteSize.FromBytes(
                   Process
                   .GetCurrentProcess()
                   .PeakVirtualMemorySize64)
                   .ToString());

            WriteLine("UserProcessorTime: {0}",
                Process
                .GetCurrentProcess()
                .UserProcessorTime);

            WriteLine("Elapsed:           {0}",
                c.Elapsed);

            CallGC();

            WriteLine("---------------");
        };
    }
}