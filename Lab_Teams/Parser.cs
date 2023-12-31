﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab_Teams
{
    public class Parser
    {
        private Grammar grammar;
        private Dictionary<(string, string), List<string>> parsingTable;

        public Parser(Grammar grammar)
        {
            this.grammar = grammar;
            this.parsingTable = new Dictionary<(string, string), List<string>>();
        }

        public HashSet<string> First(string symbol)
        {
            HashSet<string> firstSet = new HashSet<string>();

            if (grammar.IsTerminal(symbol))
            {
                firstSet.Add(symbol);
                return firstSet;
            }

            var productions = grammar.GetProductionsForNonTerminal(symbol);
            foreach (var production in productions)
            {
                if (production.Count == 1 && production.First() == "epsilon")
                {
                    firstSet.Add("epsilon");
                    continue; 
                }
                bool containsEpsilon = true;

                foreach (var prodSymbol in production)
                {
                    var firstOfProdSymbol = First(prodSymbol);

                    firstSet.UnionWith(firstOfProdSymbol.Where(x => x != "epsilon"));

                    if (!firstOfProdSymbol.Contains("epsilon"))
                    {
                        containsEpsilon = false;
                        break;
                    }
                }

                if (containsEpsilon)
                {
                    firstSet.Add("epsilon");
                }
            }

            return firstSet;
        }

        public HashSet<string> Follow(string nonTerminal, HashSet<string> inProgress = null)
        {
            if (inProgress == null)
            {
                inProgress = new HashSet<string>();
            }        
            if (inProgress.Contains(nonTerminal))
            {
                return new HashSet<string>();
            }

            HashSet<string> followSet = new HashSet<string>();

            if (nonTerminal == grammar.startingSymbol)
            {
                followSet.Add("$");
            }

            inProgress.Add(nonTerminal);

            foreach (var production in grammar.productions)
            {
                for (int i = 0; i < production.Value.Count; i++)
                {
                    var symbols = production.Value.ElementAt(i);
                    if (symbols.Contains(nonTerminal))
                    {
                        int position = symbols.IndexOf(nonTerminal);

                        if (position < symbols.Count - 1)
                        {
                            var nextSymbol = symbols[position + 1];
                            var firstOfNextSymbol = First(nextSymbol);
                            followSet.UnionWith(firstOfNextSymbol.Where(x => x != "epsilon"));

                            if (firstOfNextSymbol.Contains("epsilon"))
                            {
                                followSet.UnionWith(Follow(production.Key[0], inProgress));
                            }
                        }   
                        else
                        {
                            followSet.UnionWith(Follow(production.Key[0], inProgress));
                        }
                    }
                }
            }

            inProgress.Remove(nonTerminal);

            return followSet;
        }

        public void InitializeParsingTable()
        {
            foreach (var nonTerminal in grammar.nonTerminals)
            {
                Console.WriteLine(nonTerminal);
                foreach (var terminal in grammar.terminals)
                {
                    parsingTable.Add((nonTerminal, terminal), new List<string>());
                }
                parsingTable.Add((nonTerminal, "$"), new List<string>());
            }

            foreach (var production in grammar.productions)
            {
                var nonTerminal = production.Key[0];
                foreach (var prod in production.Value)
                {
                    string prodAsString = String.Join(" ", prod);
                    bool epsilonFound = false;
                    foreach (var symbol in prod)
                    {
                        var firstSet = First(symbol);
                        if (!firstSet.Contains("epsilon"))
                        {
                            foreach (var firstSymbol in firstSet)
                            {
                                if (!parsingTable[(nonTerminal, firstSymbol)].Contains(prodAsString))
                                {
                                    parsingTable[(nonTerminal, firstSymbol)].Add(prodAsString);
                                }
                            }
                            epsilonFound = false;
                            break;
                        }
                        else
                        {
                            foreach (var firstSymbol in firstSet.Where(x => x != "epsilon"))
                            {
                                if (!parsingTable[(nonTerminal, firstSymbol)].Contains(prodAsString))
                                {
                                    parsingTable[(nonTerminal, firstSymbol)].Add(prodAsString);
                                }
                            }
                            epsilonFound = true;
                        }
                    }

                    if (epsilonFound || prod.Contains("epsilon"))
                    {
                        var followSet = Follow(nonTerminal);
                        foreach (var followSymbol in followSet)
                        {
                            if (!parsingTable[(nonTerminal, followSymbol)].Contains(prodAsString))
                            {
                                parsingTable[(nonTerminal, followSymbol)].Add(prodAsString);
                            }
                        }
                    }
                }
            }

            //Conflicts
            foreach (var entry in parsingTable)
            {
                if (entry.Value.Count > 1)
                {
                    Console.WriteLine($"Conflict at {entry.Key}: {string.Join(", ", entry.Value)}");
                }
            }
        }

        public void PrintParsingTable()
        {
            Console.WriteLine("LL(1) Parsing Table:");
            Console.WriteLine(new String('-', 50));
            foreach (var entry in parsingTable)
            {
                string key = $"({entry.Key.Item1}, {entry.Key.Item2})";
                string value = string.Join(" ", entry.Value);
                Console.WriteLine($"{key}: {value}");
            }
            Console.WriteLine(new String('-', 50)); 
        }

        public void WriteParsingTableToFile(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.WriteLine("LL(1) Parsing Table:");
                sw.WriteLine(new String('-', 50));
                foreach (var entry in parsingTable)
                {
                    string key = $"({entry.Key.Item1}, {entry.Key.Item2})";
                    string value = string.Join(" ", entry.Value);
                    sw.WriteLine($"{key}: {value}");
                }
                sw.WriteLine(new String('-', 50));
            }
        }

        public void RunFirstAndFollowMethods()
        {
            foreach (var nT in grammar.nonTerminals)
            {
                Console.Write("FIRST(" + nT + ") = ");
                var firstSet = First(nT);
                foreach (var set in firstSet)
                {
                    Console.Write(set.ToString() + " ");
                }
                Console.WriteLine();
            }

            foreach (var nT in grammar.nonTerminals)
            {
                Console.Write("FOLLOW(" + nT + ") = ");
                var followSet = Follow(nT);
                foreach (var set in followSet)
                {
                    Console.Write(set.ToString() + " ");
                }
                Console.WriteLine();
            }
        }

        public void RunInitializeParsingTable()
        {
            InitializeParsingTable();
        }
    }
}
