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
        private Dictionary<List<string>, HashSet<List<string>>> productions;
        
        public Grammar()
        {
            nonTerminals = new List<string>();
            terminals = new List<string>();
            productions = new Dictionary<List<string>, HashSet<List<string>>>();
            startingSymbol = "";
        }

        public void ReadGrammarFromFile(string filePath)
        {
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
                        string[] tokens = line.Split("->");
                        string nonTerminals = tokens[0].Trim();
                        string[] elems = tokens[1].Split("|");
                        HashSet<List<string>> finalProductionValue = new HashSet<List<string>>();
                        foreach(var elem in elems)
                        {
                            List<string> elemsTrimmed = new List<string>();
                            foreach (var chr in elem.Trim())
                            {
                                elemsTrimmed.Add(chr.ToString());
                            }
                            finalProductionValue.Add(elemsTrimmed);
                        }
                        List<string> nonTerminalsList = new List<string>(); 
                        foreach(var elem in nonTerminals)
                        {
                            nonTerminalsList.Add(elem.ToString());
                        }
                        productions.Add(nonTerminalsList, finalProductionValue);
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
                Console.Write(kvp.Key[0] + " -> ");

                var listValuesRepresentations = kvp.Value.Select(list => string.Join("", list));

                Console.WriteLine(string.Join(" | ", listValuesRepresentations));
            }
        }

        public HashSet<List<string>> GetProductionsForNonTerminal(string nonTerminal)
        {
            foreach (var production in productions)
            {
                if (production.Key[0] == nonTerminal)
                {
                    return production.Value;
                }
            }
            return new HashSet<List<string>>();
        }

        public bool IsCFG()
        {
            foreach (var production in productions)
            {
                if (production.Key.Count > 1)
                {
                    return false;
                }
                if (!nonTerminals.Contains(production.Key[0]))
                {
                    return false;
                }
            }

            return true;
        }
    }

}
