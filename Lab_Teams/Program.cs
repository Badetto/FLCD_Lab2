using System;
using System.Globalization;

namespace Lab_Teams
{
    class Program
    {

        static void Main()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string relativePath = Path.Combine(baseDirectory, "..", "..", "..", "Problems", "g2.txt");
            string filePath = Path.GetFullPath(relativePath);
            Console.WriteLine(filePath);
            if (File.Exists(filePath))
            {
                try
                {
                    Grammar grammar = new Grammar();
                    grammar.ReadGrammarFromFile(filePath);
                    grammar.PrintNonTerminals();
                    grammar.PrintTerminals();
                    grammar.PrintStartingSymbol();
                    grammar.PrintProductions();
                    grammar.IsCFG();
                }
                catch (Exception e)
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
