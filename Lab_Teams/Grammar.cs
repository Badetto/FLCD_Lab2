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
        private string startingSymbol;
        private Dictionary<string, List<string>> productions;
        
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
            using(StreamReader sr = new StreamReader(filePath)) 
            {
                string line;
                int nrLine = 1;
                while ((line = sr.ReadLine()) != null) 
                {
                    if (nrLine == 1)
                    {
                        String[] elements = line.Split(' ');
                        nonTerminals.AddRange(elements);
                    }
                    else if (nrLine == 2) 
                    {
                        String[] elements = line.Split(' ');
                        terminals.AddRange(elements);
                    }
                    else if (nrLine == 3) 
                    {
                        String[] elements = line.Split(' ');
                        startingSymbol = elements[0];
                    }
                    else
                    {
                        String[] tokens = line.Split("-");
                        String nonTerminal = tokens[0].Trim();
                        String[] elems = tokens[1].Split("|");
                        List<string> elemsTrimmed = new List<string>();
                        foreach(var elem in elems)
                        {
                            elemsTrimmed.Add(elem.Trim());
                        }
                        productions.Add(nonTerminal, elemsTrimmed);
                    }

                    nrLine++;
                }
            }

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
