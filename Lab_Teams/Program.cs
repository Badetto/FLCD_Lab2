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
            
            string relativeOutPath = Path.Combine(baseDirectory, "..", "..", "..", "Problems", "g2.out");
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
                    /*int stackSize = 1024 * 1024 * 100;
                    Thread parserThread = new Thread(new ThreadStart(parser.RunFirstAndFollowMethods), stackSize);
                    parserThread.Start();
                    parserThread.Join();*/
                    int stackSize = 1024 * 1024 * 100; 
                    Thread parserThread = new Thread(new ThreadStart(parser.RunInitializeParsingTable), stackSize);
                    parserThread.Start();
                    parserThread.Join();
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
