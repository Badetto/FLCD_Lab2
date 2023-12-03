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
                bool containsEpsilon = true;

                foreach (var prodSymbol in production)
                {
                    var firstOfProdSymbol = First(prodSymbol);

                    firstSet.UnionWith(firstOfProdSymbol.Where(x => x != "ε"));

                    if (!firstOfProdSymbol.Contains("ε"))
                    {
                        containsEpsilon = false;
                        break;
                    }
                }

                if (containsEpsilon)
                {
                    firstSet.Add("ε");
                }
            }

            return firstSet;
        }

        public HashSet<string> Follow(string nonTerminal)
        {
            HashSet<string> followSet = new HashSet<string>();

            if (nonTerminal == grammar.startingSymbol)
            {
                followSet.Add("$");
            }

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
                            followSet.UnionWith(firstOfNextSymbol.Where(x => x != "ε"));

                            if (firstOfNextSymbol.Contains("ε"))
                            {
                                followSet.UnionWith(Follow(production.Key[0]));
                            }
                        }
                        else
                        {
                            followSet.UnionWith(Follow(production.Key[0]));
                        }
                    }
                }
            }

            return followSet;
        }
    }
}
