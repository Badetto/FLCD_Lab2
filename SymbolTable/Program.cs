using System;

namespace SymbolTable
{
    class Program
    {
        static void Main()
        {
            SymbolTable symbolTable = new(13);

            Console.WriteLine("Add: " + symbolTable.Add("a"));
            Console.WriteLine("Add: " + symbolTable.Add("ab"));
            Console.WriteLine("Add: " + symbolTable.Add("ab"));
            Console.WriteLine("Add: " + symbolTable.Add("abc"));
            Console.WriteLine("Add: " + symbolTable.Add("acb"));            
            Console.WriteLine("Contains: " + symbolTable.Contains("ab"));
            Console.WriteLine("Remove: " + symbolTable.Remove("ab"));
            Console.WriteLine("Contains: " + symbolTable.Contains("ab"));
            Console.WriteLine("Remove: " + symbolTable.Remove("ab"));

            Console.WriteLine(symbolTable.ToString());
        }
    }
}
