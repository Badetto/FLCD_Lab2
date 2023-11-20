using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab_Teams
{
    public class Grammar
    {
        private List<string> nonTerminals;
        private List<string> terminals;
        private Dictionary<string, List<string>> productions;
        private string startingSymbol;

        public Grammar()
        {
            nonTerminals = new List<string>();
            terminals = new List<string>();
            productions = new Dictionary<string, List<string>>();
            startingSymbol = "";
        }

        public void ReadGrammarFromFile(string filePath)
        {
            // Read the file
        }

        public void PrintNonTerminals()
        {
            Console.WriteLine("Non-terminals");
            Console.WriteLine(nonTerminals);
        }

        public void PrintTerminals()
        {
            Console.WriteLine("Terminals");
            Console.WriteLine(terminals);
        }

        public void PrintProductions()
        {
            Console.WriteLine("Productions");
            Console.WriteLine(productions);
        }

        public List<string> GetProductionsForNonTerminal(string nonTerminal)
        {
            return productions.ContainsKey(nonTerminal) ? productions[nonTerminal] : new List<string>();
        }

        public bool IsCFG()
        {
            // Check and return if the grammar is a CFG
            return true;
        }
    }

}
