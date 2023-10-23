using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace SymbolTable
{
    internal class Scanner
    {
        private SymbolTable symbolTable;
        private Dictionary<string, string> reservedWords;
        private Dictionary<string, string> operators;
        private HashSet<string> separators;
        private int line = 1;

        public Scanner()
        {
            symbolTable = new SymbolTable(13);
            reservedWords = new Dictionary<string, string>
            {
                {"create", "create"},
                {"create_multiple", "create_multiple"},
                {"if", "if"},
                {"else", "else"},
                {"while", "while"},
                {"leaving", "leaving"},
                {"print", "print"},
                {"readNumber", "readNumber"},
                {"readText", "readText"},
            };
            operators = new Dictionary<string, string>
            {
                {"+", "operator"},
                {"-", "operator"},
                {"*", "operator"},
                {"/", "operator"},
                {"==", "operator"},
                {"<", "operator"},
                {"<=", "operator"},
                {"=", "operator"},
                {">=", "operator"},
            };
            separators = new HashSet<string> { "{", "}", "(", ")", ",", ":", "<", ">", " ", "\n" };
        }

        private bool IsIdentifier(string token)
        {
            return Regex.IsMatch(token, @"^[a-zA-Z][a-zA-Z0-9_]*$");
        }

        public bool IsConstant(string token)
        {
            return Regex.IsMatch(token, @"^[+-]?\d+(\.\d+)?$");
        }

        private void TokenizeLine(string line)
        {
            string[] tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var token in tokens)
            {
                if (string.IsNullOrEmpty(token))
                {
                    continue;
                }

                if (reservedWords.ContainsKey(token))
                {
                    continue;
                }
                else if (operators.ContainsKey(token))
                {
                    continue;
                }
                else if (separators.Contains(token))
                {
                    continue;
                }
                else
                {
                    if (IsIdentifier(token))
                    {
                        bool contains = symbolTable.Contains(token);
                        if (!contains)
                        {
                            (int, int) position = symbolTable.Add(token);
                            Console.WriteLine($"PIF: ({token}, identifier; {position}, position)");
                        }
                        else
                        {
                            (int, int) position = symbolTable.KeyPosition(token);
                            Console.WriteLine($"PIF: ({token}, identifier; {position}, position)");
                        }
                    }
                    else if (IsConstant(token))
                    {
                        Console.WriteLine($"PIF: ({token}, -1)");
                    }
                    else
                    {
                        throw new Exception("Lexical Error at line " + this.line);
                    }
                }
            }
        }

        public void Tokenize(string sourceCode)
        {
            string[] lines = sourceCode.Split('\n');

            try
            {
                foreach (var line in lines)
                {
                    TokenizeLine(line);
                    this.line++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("Lexically correct");
            Console.WriteLine("Symbol Table:");
            Console.WriteLine(symbolTable.ToString());
        }
    }
}
