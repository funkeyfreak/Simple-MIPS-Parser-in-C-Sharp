/// Assembly Parser 
/// Author : Dalin Williams
///  Class - Advanced Architecture

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AssemblyParser.Parsers;
using AssemblyParser.Utilities.Data;
using AssemblyParser.Utilities.Memory;
using System.Diagnostics;

namespace AssemblyParser
{
    class Program
    {
        static void Main(string[] args)
        {

            bool exit = false;
            Mem mainMemory = new Mem();
            Stopwatch watch = new Stopwatch();
            int tempCounter, tempInstructions;
            //check and see if data has loaded properly

            StreamReader coreReader;
            System.Console.WriteLine("Welcome to assemParser! To begin, please enter one of the following: \n accum for Accumulator Architecture \n reg for Single-Cycle Architecture \n exit for Exiting the program \n");
                while (!exit)
                {

                    string response = System.Console.ReadLine();

                    switch (response)
                    {
                        case "accum":
                            coreReader = new StreamReader(@"./Utilities/Code/Single-Cycle Accumulator-Architecture.txt");
                            Console.WriteLine("\n Please enter the array number you would like to test \n");
                            mainMemory.createMemory(Int32.Parse(Console.ReadLine()));
                            watch.Start();
                            Accumulator a = new Accumulator(mainMemory);
                            a.parserAccumulator(coreReader);
                            watch.Stop();
                            TimeSpan elapsed = watch.Elapsed;
                            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                            elapsed.Hours, elapsed.Minutes, elapsed.Seconds,
                            elapsed.Milliseconds / 10);
                            tempCounter = a.getReferenceCount();
                            tempInstructions = a.getInstructionCount();
                            Console.WriteLine("The total references to memory is {0} with {1} instruction counts", tempCounter, tempInstructions);
                            Console.WriteLine("\n The total time was {0}", elapsedTime);
                            Console.WriteLine("\n accum, reg, exit \n");
                            watch.Reset();
                            break;
                        case "reg":
                            coreReader = new StreamReader(@"./Utilities/Code/Single-Cycle Register-Archictrure.txt");
                            Console.WriteLine("\n Please enter the array number you would like to test \n");
                            mainMemory.createMemory(Int32.Parse(Console.ReadLine()));
                            watch.Start();
                            Register r = new Register(mainMemory);
                            r.parserRegister(coreReader);
                            watch.Stop();
                            TimeSpan elapsed2 = watch.Elapsed;
                            string elapsedTime2 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                            elapsed2.Hours, elapsed2.Minutes, elapsed2.Seconds,
                            elapsed2.Milliseconds / 10);
                            tempCounter = r.getReferenceCount();
                            tempInstructions = r.getInstructionCount();
                            Console.WriteLine("The total references to memory is {0} with {1} instruction counts", tempCounter, tempInstructions);
                            Console.WriteLine("\n The total time was {0}", elapsedTime2);
                            Console.WriteLine("\n accum, reg, exit \n");
                            watch.Reset();
                            break;
                        case "exit":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Please Enter a Valid Parameter");
                            break;

                    }
                }
            
        }
    }
}

