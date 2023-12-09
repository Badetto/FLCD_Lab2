using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab_Teams
{
    public class Parser
    {
        private Grammar grammar;

        public Parser(Grammar grammar)
        {
            this.grammar = grammar;
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
    }
}
