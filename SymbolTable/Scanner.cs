using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SymbolTable
{
    internal class Scanner
    {
        private SymbolTable symbolTable;
        private Dictionary<string, string> reservedWords;
        private HashSet<string> operators;
        private HashSet<string> separators;
        private int lineNumber = 1;

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
            operators = new HashSet<string> { "+", "-", "*", "/", "==", "<", "<=", "=", ">=" };
            separators = new HashSet<string> { "{", "}", "(", ")", ",", ":", "<>", " ", "\n" };
        }

        private bool IsIdentifier(string token)
        {
            return Regex.IsMatch(token, @"^[a-zA-Z_][a-zA-Z0-9_]*$");
        }

        private bool IsNumberConstant(string token)
        {
            return Regex.IsMatch(token, @"^[+-]?\d+(\.\d+)?$");
        }

        private string ProcessStringIdentifier(string combinedToken)
        {
            return combinedToken.Substring(0, combinedToken.Length - 2) + "\"";
        }

        private void ProcessStringToken(string combinedToken)
        {
            Console.WriteLine($"PIF: (string, -1)");
            var processedToken = ProcessStringIdentifier(combinedToken);
            bool contains = symbolTable.Contains(processedToken);
            if (!contains)
            {
                symbolTable.Add(processedToken);
            }
        }

        private void ProcessIdentifierToken(string token)
        {
            bool contains = symbolTable.Contains(token);
            if (!contains)
            {
                (int, int) position = symbolTable.Add(token);
                Console.WriteLine($"PIF: ({token}, {position})");
            }
            else
            {
                (int, int) position = symbolTable.KeyPosition(token);
                Console.WriteLine($"PIF: ({token}, {position})");
            }
        }

        private void ProcessNumberToken(bool isSign, string sign, string token)
        {
            string numberToken;
            if (isSign)
            {
                numberToken = sign + token;
            }
            else
            {
                numberToken = token;
            }
            bool contains = symbolTable.Contains(numberToken);
            if (!contains)
            {
                symbolTable.Add(numberToken);
            }
            Console.WriteLine($"PIF: (number, -1)");
        }

        private void TokenizeLine(string line)
        {
            var tokens = Regex.Matches(line, @"==|<=|>=|[-+*/=<>,:(){}""]|\b\w+\b|""[^""]*""|//.*");

            string combinedToken = null;
            bool isSign = false;
            string sign = "";
            string lastToken = "";
            string currentToken = "";

            foreach (Match match in tokens)
            {
                string token = match.Value;
                if (string.IsNullOrWhiteSpace(token))
                {
                    continue;
                }
                if (currentToken == "")
                {
                    currentToken = token;
                }
                else
                {
                    lastToken = currentToken;
                    currentToken = token;
                }
                if ((lastToken + currentToken) == "//")
                {
                    break;
                }
                if (combinedToken != null)
                {
                    if (token == "\"")
                    {
                        combinedToken += token;
                    }
                    else
                    {
                        combinedToken += token + " ";
                    }
                    if (token.EndsWith("\""))
                    {
                        ProcessStringToken(combinedToken);
                        combinedToken = null;
                    }
                    continue;
                }
                else if (token.StartsWith("\""))
                {
                    combinedToken = token;
                    continue;
                }
                else if (token.StartsWith("-") || token.StartsWith("+"))
                {
                    isSign = true;
                    sign = token;
                    continue;
                }
                else if (operators.Contains(token) || separators.Contains(token) || reservedWords.ContainsKey(token))
                {
                    continue;
                }
                else if (IsIdentifier(token))
                {
                    ProcessIdentifierToken(token);
                }
                else if (IsNumberConstant(token))
                { 
                    ProcessNumberToken(isSign, sign, token);
                    isSign = false;
                    sign = "";
                }
                else
                {
                    string errorMessage = "Lexical Error at line " + lineNumber + " - Token: " + token + " is invalid";
                    throw new ScannerException(errorMessage);
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
                    lineNumber++;
                }
                Console.WriteLine("Lexically correct");
                Console.WriteLine("Symbol Table:");
                Console.WriteLine(symbolTable.ToString());
            }
            catch (ScannerException e)
            {
                Console.WriteLine(e.Message);
            }       
        }
    }
}
