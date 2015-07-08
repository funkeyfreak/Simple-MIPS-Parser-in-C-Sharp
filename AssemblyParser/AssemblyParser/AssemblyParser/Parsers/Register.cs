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
    public class Register
    {
        //data members

        private String line;
        private String[] lineFeed;
        private int lineCounter;
        private Registers allRegisters;
        private Mem mainMemory;
        private int currentCounter;
        private static int referenceCount, instrucitonCount;
        private List<string[]> instructionsRead;
        Dictionary<string, int> tags;

        //methods

        /// <summary>
        /// Constructor for the Register class
        /// </summary>
        public Register(Mem container)
        {
            mainMemory = container;
            instructionsRead = new List<string[]>();
            tags = new Dictionary<string, int>();
            lineCounter = 0;
            allRegisters = new Registers();
            allRegisters.Reg["$sp"] = mainMemory.memory.Count-1;
            referenceCount = 0;
            instrucitonCount = 0;
        }

        /// gets and returns the reference count
        public int getReferenceCount()
        {
            return referenceCount;
        }
        public int getInstructionCount()
        {
            return instrucitonCount;
        }
        

        /// <summary>
        /// This public method takes a StreamReader object and breaks the stream into per-line segmens.
        /// Pases this data to Register.
        /// </summary>
        /// <param name="reader">StreamReader containing the contents of a file</param>
        public void parserRegister(StreamReader reader)
        {
            while ((line = reader.ReadLine()) != null)
            {
                lineFeed = line.Split(' ');
                instructionsRead.Add(lineFeed);
                if (lineFeed[0].Contains(':'))
                {
                    string[] tempLineFeed = lineFeed[0].Split(':');
                    tags.Add(tempLineFeed[0], lineCounter-1);
                }
                lineCounter++;
            }
            runRegister();
        }

        /// Initiates the parser
        private void runRegister()
        {
            currentCounter = 0;
            while (currentCounter < lineCounter)
            {
                DoRegister(instructionsRead[currentCounter]);
                currentCounter++;
            }
        }

        ///Instruction definitions

        ///LOAD sets the incoming register to the address + offset data in mem
        private void LOAD(ref int register, int offset, int address)
        {
            register = mainMemory.memory[(address + offset)];


        }


        ///LOADI takes the initial return of the innner LOAD and uses said data an address from which to load data into into incoming register
        private void LOADI(ref int register, int outterOffset, int innerOffset, int address)
        {
            int intermeadiateRegister = 0;
            LOAD(ref intermeadiateRegister, innerOffset, address);
            LOAD(ref register, outterOffset, intermeadiateRegister);


        }

        ///STORE takes the incoming register and stores it into address + offset location in mem
        private void STORE(int register, int offset, int address)
        {
            mainMemory.memory[(address + offset)] = register;


        }

        ///STOREI takes the initial return of the inner LOAD and using said dt aas an addres in which to store the data withing the incoming register
        private void STOREI(int register, int outterOffset, int innerOffset, int address)
        {
            int intermeadiateRegister = register;
            LOAD(ref intermeadiateRegister, innerOffset, address);
            STORE(register, outterOffset, intermeadiateRegister);


        }

        ///ADD takes the value stored at address + offset, adds it to the incoming register, and stores it into the same
        private void ADD(ref int register, int address, int offset)
        {
            register = mainMemory.memory[(address + offset)] + register;

        }

        ///ADDI takes the incoming constant value, adds it to the incoming register, and stores it into the same
        private void ADDI(ref int register, int constant)
        {
            register = register + constant;

        }

        ///AND preforms a logical and with the data within address + offset and the register and stores the result into said register
        private void AND(ref int register, int address, int offset)
        {
            register = (register & mainMemory.memory[(address + offset)]);

        }

        ///OR preforms a logical or with the data within address + offset and the register and stores the result into said register
        private void OR(ref int register, int address, int offset)
        {
            register = (register | mainMemory.memory[(address + offset)]);

        }

        ///NOT preforms a logical not with the incoming register and stores the result into said register
        private void NOT(ref int register)
        {
            register = ~register;

        }

        ///XOR preforms a logical xor with the data within address + offset and the register and stores the result into said register
        private void XOR(ref int register, int address, int offset)
        {
            register = (register ^ mainMemory.memory[(address + offset)]);

        }

        ///MOVE takes the data within register 2 and places it into register 1
        private void MOVE(ref int register1, ref int register2)
        {
            register1 = register2;

        }

        ///BZERO branches to the location in mem if the incoming register is zero
        private void BZERO(int register, int address, int offset)
        {
            int holderValue = mainMemory.memory[(address + offset)];
            if (register == 0)
                currentCounter += holderValue - 1;


        }

        ///JUMP sets the current counter to the location of the jump
        private void JUMP(string tag)
        {
            currentCounter = tags[tag];

        }

        private void JUMP(int counter)
        {
            if (counter == 24)
            {
    
            }
            currentCounter = counter - 1;

        }

        ///SEQ sets the value of the incoming register to 1 if it is equal to the data stored in mem specified by address + offset
        private void SEQ(ref int register, int address, int offset)
        {
            int holderValue = mainMemory.memory[(address + offset)];
            if (register == holderValue)
                register = 1;
            else
                register = 0;

        }

        ///SNE sets the value of the incoming register to 0 if it is not equal to the data stored in mem specified by address + offset
        private void SNE(ref int register, int address, int offset)
        {
            int holderValue = mainMemory.memory[(address + offset)];
            if (register == holderValue)
                register = 0;
            else
                register = 1;

        }

        ///SGT sets the value of the incoming register to 1 if it is greater than the data stored in mem specified by address + offset
        private void SGT(ref int register, int address, int offset)
        {
            int holderValue = mainMemory.memory[(address + offset)];
            if (register > holderValue)
                register = 1;
            else
                register = 0;

        }

        ///SGT sets the value of the incoming register to 1 if it is less than the data stored in mem specified by address + offset
        private void SLT(ref int register, int address, int offset)
        {
            int holderValue = mainMemory.memory[(address + offset)];
            if (register < holderValue)
                register = 1;
            else
                register = 0;

        }

        ///SLE sets the value of the incoming register to 1 if it is less or equal to the data stored in mem specified by address + offset
        private void SLE(ref int register, int address, int offset)
        {
            int holderValue = mainMemory.memory[(address + offset)];
            if (register <= holderValue)
                register = 1;
            else
                register = 0;

        }

        ///SGE sets the value of the incoming register to 1 if it is greater than or equal to the data stored in mem specified by address + offset
        private void SGE(ref int register, int address, int offset)
        {
            int holderValue = mainMemory.memory[(address + offset)];
            if (register >= holderValue)
                register = 1;
            else
                register = 0;

        }

        ///PUSH adds the contents of the incoming register to the top of mem
        private void PUSH(int register)
        {
            mainMemory.memory.Add(register);
            int tempSP = allRegisters.Reg["$sp"];
            allRegisters.Reg["$sp"] = tempSP + 1;


        }

        ///POP removes the top item in mem and stores it into the incoming register
        private void POP(ref int register)
        {
            register = mainMemory.memory[allRegisters.Reg["sp"]];
            mainMemory.memory.RemoveAt(allRegisters.Reg["$sp"]);
            int tempSP = allRegisters.Reg["$sp"];
            allRegisters.Reg["$sp"] = tempSP - 1;


        }

        private void CALL(int address, int offset)
        {
            mainMemory.memory.Add(currentCounter + 1);
            int tempSP = allRegisters.Reg["$sp"];
            allRegisters.Reg["$sp"] = tempSP + 1;  
            currentCounter = mainMemory.memory[(address + offset)];


        }

        private void RET()
        {
            currentCounter = mainMemory.memory[allRegisters.Reg["$sp"]];
            int tempSP = allRegisters.Reg["$sp"];
            allRegisters.Reg["$sp"] = tempSP - 1;


        }

        ///END: Instruciton definitions


        /// <summary>
        /// Accumulator - Parses through the contents of an instruction.
        /// </summary>
        /// <param name="input">An array of input generated from parserAccumulator</param>
        private void DoRegister(String[] input)
        {
            String[] args;
            int reg1 = 0;
            int reg2 = 0;
            int address = 0;
            
            switch (input[0])
            {
                case "LOAD":
                    args = input[2].Split(new Char[] { '(', ')' });
                    reg1 = allRegisters.Reg[input[1]];
                    LOAD(ref reg1, int.Parse(args[0]), allRegisters.Reg[args[1]]);
                    allRegisters.Reg[input[1]] = reg1;
                    instrucitonCount += 1;
                    referenceCount += 1;
                    break;
                case "LOADI":
                    args = input[2].Split(new Char[] { '(', ')' });
                    reg1 = allRegisters.Reg[input[1]];
                    LOADI(ref reg1, int.Parse(args[0]), int.Parse(args[1]), allRegisters.Reg[args[2]]);
                    allRegisters.Reg[input[1]] = reg1;
                    instrucitonCount += 1;
                    referenceCount += 1;
                    break;
                case "STORE":
                    args = input[2].Split(new Char[] { '(', ')' });
                    reg1 = allRegisters.Reg[input[1]];
                    address = allRegisters.Reg[args[1]];
                    STORE(reg1, int.Parse(args[0]), address);
                    instrucitonCount += 1;
                    referenceCount += 1;
                    break;
                case "STOREI":
                    args = input[2].Split(new Char[] { '(', ')' });
                    reg1 = allRegisters.Reg[input[1]];
                    STOREI(reg1, int.Parse(args[0]), int.Parse(args[1]), allRegisters.Reg[args[2]]);
                    instrucitonCount += 1;
                    referenceCount += 1;
                    break;
                case "ADD":
                    args = input[2].Split(new Char[] { '(', ')' });
                    reg1 = allRegisters.Reg[input[1]];
                    address = allRegisters.Reg[args[1]];
                    ADD(ref reg1, address, int.Parse(args[0]));
                    allRegisters.Reg[input[1]] = reg1;
                    instrucitonCount += 1;
                    break;
                case "ADDI":
                    reg1 = allRegisters.Reg[input[1]];
                    ADDI(ref reg1, int.Parse(input[2]));
                    allRegisters.Reg[input[1]] = reg1;
                    instrucitonCount += 1;
                    break;
                case "AND":
                    args = input[2].Split(new Char[] { '(', ')' });
                    reg1 = allRegisters.Reg[input[1]];
                    AND(ref reg1, int.Parse(args[1]), int.Parse(args[0]));
                    allRegisters.Reg[input[1]] = reg1;
                    instrucitonCount += 1;
                    break;
                case "OR":
                    args = input[2].Split(new Char[] { '(', ')' });
                    reg1 = allRegisters.Reg[input[1]];
                    OR(ref reg1, int.Parse(args[1]), int.Parse(args[0]));
                    allRegisters.Reg[input[1]] = reg1;
                    instrucitonCount += 1;
                    break;
                case "NOT":
                    args = input[2].Split(new Char[] { '(', ')' });
                    reg1 = allRegisters.Reg[input[1]];
                    NOT(ref reg1);
                    allRegisters.Reg[input[1]] = reg1;
                    instrucitonCount += 1;
                    break;
                case "XOR":
                    args = input[2].Split(new Char[] { '(', ')' });
                    reg1 = allRegisters.Reg[input[1]];
                    XOR(ref reg1, int.Parse(args[1]), int.Parse(args[0]));
                    allRegisters.Reg[input[1]] = reg1;
                    instrucitonCount += 1;
                    break;
                case "MOVE":
                    reg1 = allRegisters.Reg[input[1]];
                    reg2 = allRegisters.Reg[input[2]];
                    MOVE(ref reg1, ref reg2);
                    allRegisters.Reg[input[1]] = reg1;
                    instrucitonCount += 1;
                    break;
                case "JUMP":
                    int vsl;
                    if (int.TryParse(input[1], out vsl))
                    {
                        JUMP(vsl);
                    }
                    else
                        JUMP(input[1]);
                    instrucitonCount += 1;
                    break;
                case "BZERO":
                    args = input[2].Split(new Char[] { '(', ')' });
                    reg1 = allRegisters.Reg[input[1]];
                    address = allRegisters.Reg[args[1]];
                    BZERO(reg1, address, int.Parse(args[0]));
                    referenceCount += 1;
                    instrucitonCount += 1;
                    break;
                case "SEQ":
                    args = input[2].Split(new Char[] { '(', ')' });
                    reg1 = allRegisters.Reg[input[1]];
                    address = allRegisters.Reg[args[1]];
                    SEQ(ref reg1, address, int.Parse(args[0]));
                    allRegisters.Reg[input[1]] = reg1;
                    instrucitonCount += 1;
                    break;
                case "SNE":
                    args = input[2].Split(new Char[] { '(', ')' });
                    reg1 = allRegisters.Reg[input[1]];
                    address = allRegisters.Reg[args[1]];
                    SNE(ref reg1, address, int.Parse(args[0]));
                    allRegisters.Reg[input[1]] = reg1;
                    instrucitonCount += 1;
                    break;
                case "SGT":
                    args = input[2].Split(new Char[] { '(', ')' });
                    reg1 = allRegisters.Reg[input[1]];
                    address = allRegisters.Reg[args[1]];
                    SGT(ref reg1, address, int.Parse(args[0]));
                    allRegisters.Reg[input[1]] = reg1;
                    instrucitonCount += 1;
                    break;
                case "SLT":
                    args = input[2].Split(new Char[] { '(', ')' });
                    reg1 = allRegisters.Reg[input[1]];
                    address = allRegisters.Reg[args[1]];
                    SLT(ref reg1, address, int.Parse(args[0]));
                    allRegisters.Reg[input[1]] = reg1;
                    instrucitonCount += 1;
                    break;
                case "SLE":
                    args = input[2].Split(new Char[] { '(', ')' });
                    reg1 = allRegisters.Reg[input[1]];
                    address = allRegisters.Reg[args[1]];
                    SLE(ref reg1, address, int.Parse(args[0]));
                    allRegisters.Reg[input[1]] = reg1;
                    instrucitonCount += 1;
                    break;
                case "SGE":
                    args = input[2].Split(new Char[] { '(', ')' });
                    reg1 = allRegisters.Reg[input[1]];
                    address = allRegisters.Reg[args[1]];
                    SGE(ref reg1, address, int.Parse(args[0]));
                    allRegisters.Reg[input[1]] = reg1;
                    instrucitonCount += 1;
                    break;
                case "PUSH":
                    reg1 = allRegisters.Reg[input[1]];
                    PUSH(reg1);
                    referenceCount += 1;
                    instrucitonCount += 1;
                    break;
                case "POP":
                    reg1 = allRegisters.Reg[input[1]];
                    POP(ref reg1);
                    allRegisters.Reg[input[1]] = reg1;
                    referenceCount += 1;
                    instrucitonCount += 1;
                    break;
                case "CALL":
                    args = input[1].Split(new Char[] { '(', ')' });
                    address = allRegisters.Reg[args[1]];
                    CALL(address, int.Parse(args[0]));
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
                        DoRegister(tempInput.ToArray());
                        break;
                    }
                    Console.WriteLine("An error has been found, cannot compute instruction at line {0}", currentCounter);
                    break;
            }
        }
    }
}
