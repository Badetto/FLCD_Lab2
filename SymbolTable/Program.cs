using System;
using System.Globalization;

namespace SymbolTable
{
    class Program
    {
        static void Main()
        {
            /*Symbol Table
            SymbolTable symbolTable = new(13);

            Console.WriteLine("Add: " + symbolTable.Add("a"));
            Console.WriteLine("Add: " + symbolTable.Add("ab"));
            Console.WriteLine("Add: " + symbolTable.Add("ab"));
            Console.WriteLine("Add: " + symbolTable.Add("abc"));
            Console.WriteLine("Add: " + symbolTable.Add("acb"));            
            Console.WriteLine("Contains: " + symbolTable.KeyPosition("ab"));
            Console.WriteLine("Remove: " + symbolTable.Remove("ab"));
            Console.WriteLine("Contains: " + symbolTable.KeyPosition("ab"));
            Console.WriteLine("Remove: " + symbolTable.Remove("ab"));

            Console.WriteLine(symbolTable.ToString());*/
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string filePath = Path.Combine(baseDirectory, "..", "..", "..", "Problems", "fa.txt");
            if (File.Exists(filePath))
            {
                try
                {
                    /*Scanner
                    string sourceCode = File.ReadAllText(filePath);
                    Scanner scanner = new Scanner();
                    scanner.Tokenize(sourceCode);*/
                    string sourceCode = File.ReadAllText(filePath);
                    FiniteAutomata.FiniteAutomata finiteAutomata = new FiniteAutomata.FiniteAutomata(sourceCode);
                    finiteAutomata.ReadFile();
                    finiteAutomata.DisplayElements();
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
