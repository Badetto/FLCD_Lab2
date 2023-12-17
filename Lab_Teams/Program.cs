using System;
using System.Globalization;

namespace Lab_Teams
{
    class Program
    {

        static void Main()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string relativePath = Path.Combine(baseDirectory, "..", "..", "..", "Problems", "g3.txt");
            string filePath = Path.GetFullPath(relativePath);
            Console.WriteLine(filePath);
            
            string relativeOutPath = Path.Combine(baseDirectory, "..", "..", "..", "Problems", "g3.out");
            string filePathOut = Path.GetFullPath(relativeOutPath);

            if (File.Exists(filePath))
            {
                try
                {
                    Grammar grammar = new Grammar();
                    Parser parser = new Parser(grammar);
                    grammar.ReadGrammarFromFile(filePath);
                    grammar.PrintNonTerminals();
                    grammar.PrintTerminals();
                    grammar.GetProductionsForNonTerminal("A");
                    grammar.PrintStartingSymbol();
                    grammar.PrintProductions();
                    Console.WriteLine(grammar.IsCFG());
                    foreach (var nT in grammar.nonTerminals)
                    {
                        Console.Write("FRIST(" + nT + ") = ");
                        var firstSet = parser.First(nT);
                        foreach (var set in firstSet)
                        {
                            Console.Write(set.ToString() + " ");
                        }
                        Console.WriteLine();
                    }
                    foreach (var nT in grammar.nonTerminals)
                    {
                        Console.Write("FOLLOW(" + nT + ") = ");
                        var followSet = parser.Follow(nT);
                        foreach (var set in followSet)
                        {
                            Console.Write(set.ToString() + " ");
                        }
                        Console.WriteLine();
                    }
                    parser.InitializeParsingTable();
                    parser.PrintParsingTable();

                    
                    parser.WriteParsingTableToFile(filePathOut);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    using (StreamWriter sw = new StreamWriter(filePathOut, true))
                    {
                       sw.WriteLine("\n" + e.Message + "\n");
                    }
                }           
            }
            else
            {
                Console.WriteLine("File Not Found!");
            }
        }
    }
}
