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
                        string[] elements = line.Split(' ');
                        nonTerminals.AddRange(elements);
                    }
                    else if (nrLine == 2) 
                    {
                        string[] elements = line.Split(' ');
                        terminals.AddRange(elements);
                    }
                    else if (nrLine == 3) 
                    {
                        string[] elements = line.Split(' ');
                        startingSymbol = elements[0];
                    }
                    else
                    {
                        string[] tokens = line.Split("-");
                        string nonTerminal = tokens[0].Trim();
                        string[] elems = tokens[1].Split("|");
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
            Console.WriteLine("Non-terminals:");
            foreach (var nonTerminal in nonTerminals)
            {
                Console.Write(nonTerminal + " ");
            }
            Console.WriteLine();
        }

        public void PrintTerminals()
        {
            Console.WriteLine("Terminals:");
            foreach (var terminal in terminals)
            {
                Console.Write(terminal + " ");
            }
            Console.WriteLine();    
        }

        public void PrintStartingSymbol()
        {
            Console.WriteLine("Starting Symbol: ");
            Console.WriteLine(startingSymbol);
        }

        public void PrintProductions()
        {
            Console.WriteLine("Productions:");
            foreach (var kvp in productions)
            {
                Console.Write(kvp.Key + " -> ");
                Console.WriteLine(string.Join(" | ", kvp.Value));
            }
        }

        public List<string> GetProductionsForNonTerminal(string nonTerminal)
        {
            return productions.ContainsKey(nonTerminal) ? productions[nonTerminal] : new List<string>();
        }

        public bool IsCFG()
        {
            foreach (var production in productions)
            {
                if (!nonTerminals.Contains(production.Key))
                {
                    return false;
                }
            }

            return true;
        }
    }

}
