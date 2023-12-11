using System;
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
                    bool epsilonFound = false;
                    foreach (var symbol in prod)
                    {
                        var firstSet = First(symbol);
                        if (!firstSet.Contains("epsilon"))
                        {
                            foreach (var firstSymbol in firstSet)
                            {
                                parsingTable[(nonTerminal, firstSymbol)].AddRange(prod);
                            }
                            epsilonFound = false;
                            break;
                        }
                        else
                        {
                            foreach (var firstSymbol in firstSet.Where(x => x != "epsilon"))
                            {
                                parsingTable[(nonTerminal, firstSymbol)].AddRange(prod);
                            }
                            epsilonFound = true;
                        }
                    }

                    if (epsilonFound || prod.Contains("epsilon"))
                    {
                        var followSet = Follow(nonTerminal);
                        foreach (var followSymbol in followSet)
                        {
                            parsingTable[(nonTerminal, followSymbol)].AddRange(prod);
                        }
                    }
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
    }
}
