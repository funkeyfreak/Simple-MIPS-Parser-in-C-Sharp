using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AssemblyParser.Utilities.Data;
using AssemblyParser.Utilities.Memory;

namespace AssemblyParser.Parsers
{
    public class Accumulator
    {
        //data members

        private String line;
        private String[] lineFeed;
        private int lineCounter;
        private Mem mainMemory;
        private List<string[]> instructionsRead;
        private static int accum, memoryPointer, referenceCount, instrucitonCount;
        private int currentCounter;
        Dictionary<string, int> tags;

        //methods
        //instructionsRead.Add(lineFeed);

        /// <summary>
        /// Constructor for the Accumulator Class
        /// </summary>
        public Accumulator(Mem container)
        {
            tags = new Dictionary<string, int>();
            instructionsRead = new List<string[]>();
            mainMemory = container;
            lineCounter = 0;
            currentCounter = 0;
            memoryPointer = mainMemory.memory.Count-1;
            referenceCount = 0;
            instrucitonCount = 0;
            accum = 0;
        }

        /// gets and returns the reference count
        public int getReferenceCount(){
            return referenceCount;
        }
        public int getInstructionCount(){
            return instrucitonCount;
        }

        /// <summary>
        /// This public method takes a StreamReader object and breaks the stream into per-line segmens.
        /// Pases this data to Accumulator.
        /// </summary>
        /// <param name="reader">StreamReader containing the contents of a file</param>
        public void parserAccumulator(StreamReader reader)
        {
            while ((line = reader.ReadLine()) != null)
            {
                lineFeed = line.Split(null);
                instructionsRead.Add(lineFeed);
                if (lineFeed[0].Contains(':'))
                {
                    string[] tempLineFeed = lineFeed[0].Split(':');
                    tags.Add(tempLineFeed[0], lineCounter - 1);
                }
                lineCounter++;
            }
            runAccumulator();
        }

        //begins the accumulator simulation
        private void runAccumulator()
        {
            currentCounter = 0;
            while (currentCounter < lineCounter)
            {
                DoAccumulator(instructionsRead[currentCounter]);
                currentCounter++;
            }
        }

        ///Instruction definitions

        ///LOAD sets the accumulator to an item in memory
        private void LOAD(int offset, int address)
        {
            accum = mainMemory.memory[(address + offset)];


        }

        ///LOADI sets the accum to the result of an inner load (which will be taken as the address in the second)
        private void LOADI(int outterOffset, int innerOffset, int address)
        {
            LOAD(innerOffset, address);
            int tempAccum = accum;
            LOAD(outterOffset, tempAccum);


        }

        ///STORE takes the data in the accumulator and stores it to memory
        private void STORE(int offset, int address)
        {
            mainMemory.memory[(address + offset)] = accum;


        }

        ///STOREI takes the data in the accumulator and stores the data to the result of the inner LOAD
        private void STOREI(int outterOffset, int innerOffset, int address)
        {
            int tempAccum = accum;
            LOAD(innerOffset, address);
            int hold = accum;
            accum = tempAccum;
            STORE(outterOffset, hold);


        }

        ///ADD sets the accumulator to the result of the LOAD and the accumulator
        private void ADD(int address, int offset)
        {
            int loadedNumber = accum;
            LOAD(offset, address);
            accum = accum + loadedNumber;

        }

        //ADDI sets the accumulator to the accumulator with the immediate
        private void ADDI(int immediate)
        {
           accum = accum + immediate;
        }

        //AND initiates a bitwise and with accum and a value from memory
        private void AND(int address, int offset) 
        {
            int prevAccum = accum;
            LOAD(offset, address);

            accum = accum & prevAccum;

        }

        ///OR initiates a bitwise or with accum and a value from memory
        private void OR(int address, int offset)
        {
            int prevAccum = accum;
            LOAD(offset, address);

            accum = accum | prevAccum;

        }

        ///NOT inverts the accumulator
        private void NOT()
        {
            accum = ~accum;

        }

        ///XOR preforms a bitwise xor opperation on the accumulator and some item in memory
        private void XOR(int address, int offset)
        {
            int prevAccum = accum;
            LOAD(offset, address);

            accum = accum ^ prevAccum;

        }

        ///JUMP goes and finds the instruciton at posistion referenced by the tag
        private void JUMP(string tag)
        {
            currentCounter = tags[tag];

        }

        private void JUMP(int counter)
        {
            if (counter == 34)
            {
    
            }
            currentCounter = counter - 1;

        }
        ///BZERO if accum is zero, return, otherwise jump to a tag
        private void BZERO(int address, int offset)
        {
            int valueHolder = mainMemory.memory[(address + offset)];

            if (accum == 0)
                currentCounter += valueHolder - 1;


        }

        ///SEQ sets the accumulator to one if it is equal to some item in memory, otherwise accum is zero
        private void SEQ(int address, int offset)
        {
            if (accum == mainMemory.memory[(address + offset)])
                accum = 1;
            else
                accum = 0;

        }

        ///SNE sets the accumulator to one if it is not equal to some item in memory, otherwise accum is zero
        private void SNE(int address, int offset)
        {
            if (accum == mainMemory.memory[(address + offset)])
                accum = 0;
            else
                accum = 1;

        }

        ///SGT sets the accumulator to one if it is greater than to some item in memory, otherwise accum is zero
        private void SGT(int address, int offset)
        {
            if (accum > mainMemory.memory[(address + offset)])
                accum = 1;
            else
                accum = 0;

        }

        ///SEQ sets the accumulator to one if it is equal to some item in memory, otherwise accum is zero
        private void SLT(int address, int offset)
        {
            if (accum < mainMemory.memory[(address + offset)])
                accum = 1;
            else
                accum = 0;

        }

        ///SGT sets the accumulator to one if it is greater than to some item in memory, otherwise accum is zero
        private void SGE(int address, int offset)
        {
            if (accum >= mainMemory.memory[(address + offset)])
                accum = 1;
            else
                accum = 0;

        }

        ///SEQ sets the accumulator to one if it is equal to some item in memory, otherwise accum is zero
        private void SLE(int address, int offset)
        {
            if (accum <= mainMemory.memory[(address + offset)])
                accum = 1;
            else
                accum = 0;

        }

        ///PUSH places the value in accum on the 'back end' of memory and updates the stack pointer
        private void PUSH()
        {
            mainMemory.memory.Add(accum);
            memoryPointer++;


        }

        ///PUSH places an item on the 'back end' of memory and updates the stack pointer -- overload
        private void PUSH(int address, int offset)
        {
            int loaded = mainMemory.memory[(address + offset)];
            mainMemory.memory.Add(loaded);


        }

        //POP removes the item from the 'back end' of memory and updates the stack pointer
        private void POP()
        {
            accum = mainMemory.memory[memoryPointer];
            mainMemory.memory.RemoveAt(mainMemory.memory.Count - 1);
            memoryPointer--;


        }

        private void CALL(int address, int offset)
        {
            mainMemory.memory.Add(currentCounter + 1);
            memoryPointer++;
            currentCounter = mainMemory.memory[(address + offset)];


        }

        private void RET()
        {
            currentCounter = mainMemory.memory[memoryPointer];
            memoryPointer--;


        }

        ///END: Instruction definitions
        //

        /// <summary>
        /// Accumulator - Parses through the contents of an instruction.
        /// </summary>
        /// <param name="input">An array of input generated from parserAccumulator</param>
        private void DoAccumulator(String[] input)
        {
            String[] args;
            //instrucitonCount += 1;
            switch (input[0])
            {
                case "LOAD":
                    args = input[1].Split(new Char[] { '(', ')' });
                    LOAD(int.Parse(args[0]), memoryPointer);
                    referenceCount += 1;
                    instrucitonCount += 1;
                    break;
                case "LOADI":
                    args = input[1].Split(new Char[] { '(', ')' });
                    LOADI(int.Parse(args[0]), int.Parse(args[1]), memoryPointer);
                    referenceCount += 1;
                    instrucitonCount += 1;
                    break;
                case "STORE":
                     args = input[1].Split(new Char[] { '(', ')' });
                    STORE(int.Parse(args[0]), memoryPointer);
                    referenceCount += 1;
                    instrucitonCount += 1;
                    break;
                case "STOREI":
                    args = input[1].Split(new Char[] { '(', ')' });
                    STOREI(int.Parse(args[0]), int.Parse(args[1]), memoryPointer);
                    referenceCount += 1;
                    instrucitonCount += 1;
                    break;
                case "ADD":
                    args = input[1].Split(new Char[] { '(', ')' });
                    ADD(memoryPointer, int.Parse(args[0]));
                    instrucitonCount += 1;
                    break;
                case "ADDI":
                    args = input[1].Split(new Char[] { '(', ')' });
                    ADDI(int.Parse(args[0]));
                    instrucitonCount += 1;
                    break;
                case "AND":
                    args = input[1].Split(new Char[] { '(', ')' });
                    AND(memoryPointer, int.Parse(args[0]));
                    instrucitonCount += 1;
                    break;
                case "OR":
                    args = input[1].Split(new Char[] { '(', ')' });
                    OR(memoryPointer, int.Parse(args[0]));
                    instrucitonCount += 1;
                    break;
                case "NOT":
                    NOT();
                    instrucitonCount += 1;
                    break;
                case "XOR":
                    args = input[1].Split(new Char[] { '(', ')' });
                    XOR(memoryPointer, int.Parse(args[0]));
                    instrucitonCount += 1;
                    break;
                case "JUMP":
                    int vsl;
                    if (int.TryParse(input[1],out vsl))
                    {
                        JUMP(vsl);
                    }
                    else
                        JUMP(input[1]);
                    instrucitonCount += 1;
                    break;
                case "BZERO":
                    args = input[1].Split(new Char[] { '(', ')' });
                    BZERO(memoryPointer, int.Parse(args[0]));
                    referenceCount += 1;
                    instrucitonCount += 1;
                    break;
                case "SEQ":
                    args = input[1].Split(new Char[] { '(', ')' });
                    SEQ(memoryPointer, int.Parse(args[0]));
                    instrucitonCount += 1;
                    break;
                case "SNE":
                    args = input[1].Split(new Char[] { '(', ')' });
                    SNE(memoryPointer, int.Parse(args[0]));
                    instrucitonCount += 1;
                    break;
                case "SGT":
                    args = input[1].Split(new Char[] { '(', ')' });
                    SGT(memoryPointer, int.Parse(args[0]));
                    instrucitonCount += 1;
                    break;
                case "SLT":
                    args = input[1].Split(new Char[] { '(', ')' });
                    SLT(memoryPointer, int.Parse(args[0]));
                    instrucitonCount += 1;
                    break;
                case "SGE":
                    args = input[1].Split(new Char[] { '(', ')' });
                    SGE(memoryPointer, int.Parse(args[0]));
                    instrucitonCount += 1;
                    break;
                case "SLE":
                    args = input[1].Split(new Char[] { '(', ')' });
                    SLE(memoryPointer, int.Parse(args[0]));
                    instrucitonCount += 1;
                    break;
                case "PUSH":
                    if (input.Length == 2)
                    {
                        args = input[1].Split(new Char[] { '(', ')' });
                        PUSH(memoryPointer, int.Parse(args[0]));
                    }
                    else
                        PUSH();
                    referenceCount += 1;
                    instrucitonCount += 1;
                    break;
                case "POP":
                    POP();
                    referenceCount += 1;
                    instrucitonCount += 1;
                    break;
                case "CALL":
                    args = input[1].Split(new Char[] { '(', ')' });
                    CALL(memoryPointer, int.Parse(args[0]));
                    referenceCount += 1;
                    instrucitonCount += 1;
                    break;
                case "RET":
                    RET();
                    referenceCount += 1;
                    instrucitonCount += 1;
                    break;
                default:
                    //Add the tag to if that is what it represents
                    if (input[0].Contains(":"))
                    {
                        List<string> tempInput = input.ToList();
                        tempInput.RemoveAt(0);
                        DoAccumulator(tempInput.ToArray());
                        break;
                    }
                    Console.WriteLine("An error has been found, cannot compute instruction at line {0}", currentCounter);
                    break;
            }
        }
    }
}
