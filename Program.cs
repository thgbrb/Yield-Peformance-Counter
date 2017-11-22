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
        static Stopwatch c { get; set; } = new Stopwatch();

        static void Main(string[] args)
        {
            var range = Enumerable.Range(1, 999999999);

            Init("With Yield");

            Count(EvenNumbersWithYield(range));
                // EvenNumbersWithYield(range)
                // .ToList()
                // .ForEach(Print);

            Finish();

            Init("Without Yield");

            Count(EvenNumbersWithoutYield(range));
                // EvenNumbersWithoutYield(range);
                // .ToList()
                // .ForEach(Print);

            Finish();

            ReadLine();
        }

        static IEnumerable<int> EvenNumbersWithYield(IEnumerable<int> numbers)
        {
            foreach (var n in numbers)
                if (EvenPredicate(n))
                    yield return n;
        }

        static IEnumerable<int> EvenNumbersWithoutYield(IEnumerable<int> numbers)
        {
            var result = new Collection<int>();

            foreach (var n in numbers)
                if (EvenPredicate(n))
                    result.Add(n);

            return result;
        }

        static Func<int, bool> EvenPredicate = n => n % 2 == 0;

        static Action CallGC = () =>
        {
            GC.Collect(0, GCCollectionMode.Forced, true);
            GC.Collect(1, GCCollectionMode.Forced, true);
            GC.Collect(2, GCCollectionMode.Forced, true);
        };

        static Action<int> Print = i =>
        {
            WriteLine("Calculating: {0}", i);
            SetCursorPosition(0, CursorTop - 1);
        };

        static Action<IEnumerable<int>> Count = i =>
        {
            WriteLine("Total of Elements: {0}", i.Count());
            SetCursorPosition(0, CursorTop - 1);
        };

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