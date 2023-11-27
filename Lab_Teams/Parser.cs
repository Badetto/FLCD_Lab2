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
    }
}
