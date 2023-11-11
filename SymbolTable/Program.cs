using System;
using System.Globalization;

namespace SymbolTable
{
    class Program
    {
        static List<string> ReadSequenceOfCharacters()
        {
            Console.Write("Number of characters in the sequence: ");
            int n = int.Parse(Console.ReadLine());
            var listOfCharacters = new List<string>();

            for (int i = 0; i < n; i++)
            {
                Console.Write("> ");
                string chr = Console.ReadLine();
                listOfCharacters.Add(chr);
            }

            return listOfCharacters;
        }

        static void Main()
        {
            /* Symbol Table test - previous commits */
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string filePath = Path.Combine(baseDirectory, "..", "..", "..", "Problems", "fa_identifier.txt");
            if (File.Exists(filePath))
            {
                try
                {
                    /* Scanner test - previous commits */
                    string sourceCode = File.ReadAllText(filePath);
                    FiniteAutomata.FiniteAutomata finiteAutomata = new FiniteAutomata.FiniteAutomata(sourceCode);
                    finiteAutomata.ReadFile();    
                    while (true)
                    {
                        Console.WriteLine("***** Print States: 1");
                        Console.WriteLine("***** Print Alphabet: 2");
                        Console.WriteLine("***** Print Input State: 3");
                        Console.WriteLine("***** Print Final States: 4");
                        Console.WriteLine("***** Print Transitions: 5");
                        Console.WriteLine("***** Check if sequence is accepted: 6");
                        Console.Write("--> ");
                        int command = int.Parse(Console.ReadLine());
                        switch (command)
                        {
                            case 1:
                                finiteAutomata.DisplayStates();
                                break;
                            case 2:
                                finiteAutomata.DisplayAlphabet();
                                break;
                            case 3:
                                finiteAutomata.DisplayInitialState();
                                break;
                            case 4:
                                finiteAutomata.DisplayFinalStates();
                                break;
                            case 5:
                                finiteAutomata.DisplayTransitions();
                                break;
                            case 6:
                                Console.WriteLine(finiteAutomata.CheckSequence(ReadSequenceOfCharacters()));
                                break;
                            default:
                                Console.WriteLine("Invalid choice");
                                break;
                        }
                    }
                }
                catch (ScannerException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine("File Not Found!");
            }
        }
    }
}
