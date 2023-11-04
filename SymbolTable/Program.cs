using System;
using System.Globalization;

namespace SymbolTable
{
    class Program
    {
        static void Main()
        {
            /*SymbolTable symbolTable = new(13);

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

            string filePath = Path.Combine(baseDirectory, "..", "..", "..", "Problems", "p1.txt");
            if (File.Exists(filePath))
            {
                try
                {
                    string sourceCode = File.ReadAllText(filePath);
                    Scanner scanner = new Scanner();
                    scanner.Tokenize(sourceCode);
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
