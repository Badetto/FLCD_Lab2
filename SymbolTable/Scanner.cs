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
            operators = new HashSet<string> { "%", "+", "-", "*", "/", "==", "<", "<=", "=", ">=" };
            separators = new HashSet<string> { "{", "}", "(", ")", ",", ":", "<>", " ", "\n" };
        }

        static List<string> ReadSequenceOfCharacters(string token)
        {
            var listOfCharacters = new List<string>();

            foreach (var chr in token)
            {
                listOfCharacters.Add(chr.ToString());
            }

            return listOfCharacters;
        }

        private bool IsIdentifier(string token)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string filePath = Path.Combine(baseDirectory, "..", "..", "..", "Problems", "fa_identifier.txt");

            string sourceCode = File.ReadAllText(filePath);
            FiniteAutomata.FiniteAutomata finiteAutomata = new FiniteAutomata.FiniteAutomata(sourceCode);
            finiteAutomata.ReadFile(); 
            return finiteAutomata.CheckSequence(ReadSequenceOfCharacters(token));


        }

        private bool IsNumberConstant(string token)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string filePath = Path.Combine(baseDirectory, "..", "..", "..", "Problems", "fa_int_constant.txt");

            string sourceCode = File.ReadAllText(filePath);
            FiniteAutomata.FiniteAutomata finiteAutomata = new FiniteAutomata.FiniteAutomata(sourceCode);
            finiteAutomata.ReadFile();
            return finiteAutomata.CheckSequence(ReadSequenceOfCharacters(token));

              
        }

        private string ProcessStringIdentifier(string combinedToken)
        {
            return combinedToken.Substring(0, combinedToken.Length - 2) + "\"";
        }

        private void ProcessStringToken(string combinedToken)
        {
            var processedToken = ProcessStringIdentifier(combinedToken);
            bool contains = symbolTable.Contains(processedToken);
            if (!contains)
            {
                (int, int) position = symbolTable.Add(processedToken);
                Console.WriteLine($"PIF: (constant, {position})");
            }
            else
            {
                (int, int) position = symbolTable.KeyPosition(processedToken);
                Console.WriteLine($"PIF: (constant, {position})");
            }
        }

        private void ProcessIdentifierToken(string token)
        {
            bool contains = symbolTable.Contains(token);
            if (!contains)
            {
                (int, int) position = symbolTable.Add(token);
                Console.WriteLine($"PIF: (identifier, {position})");
            }
            else
            {
                (int, int) position = symbolTable.KeyPosition(token);
                Console.WriteLine($"PIF: (identifier, {position})");
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
                (int, int) position = symbolTable.Add(numberToken);
                Console.WriteLine($"PIF: (constant, {position})");
            }
            else
            {
                (int, int) position = symbolTable.KeyPosition(numberToken);
                Console.WriteLine($"PIF: (constant, {position})");
            }
        }

        private void TokenizeLine(string line)
        {
            var tokens = Regex.Matches(line, @"==|<=|>=|[-+*/=<>,:(){}""/%]|\b\w+\b|""[^""]*""|//.*");

            string combinedToken = null;
            bool isSign = false;
            string sign = "";
            string lastToken = "";
            string currentToken = "";
            int numberOfQuotes = 0;

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
                        numberOfQuotes++;
                        ProcessStringToken(combinedToken);
                        combinedToken = null;
                    }
                    continue;
                }
                else if (token.StartsWith("\""))
                {
                    numberOfQuotes++;
                    combinedToken = token;
                    continue;
                }
                else if (token.StartsWith("-") || token.StartsWith("+"))
                {
                    if (!operators.Contains(lastToken))
                    {
                        Console.WriteLine($"PIF: ({token}, -1)");
                    }

                    if (operators.Contains(lastToken))
                    {
                       
                        isSign = true;
                        sign = token;
                    }
                    else
                    {
                        isSign = false;
                        sign = token;
                    }
                    continue;
                }
                else if (operators.Contains(token) || separators.Contains(token) || reservedWords.ContainsKey(token))
                {
                    if (token != "/")
                    {
                        Console.WriteLine($"PIF: ({token}, -3)");
                    }
                    continue;
                }
                else if (IsIdentifier(token))
                {
                    ProcessIdentifierToken(token);
                }
                else if (IsNumberConstant(token))
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
                    if (!IsNumberConstant(numberToken))
                    {
                        string errorMessage = "Lexical Error at line " + lineNumber + " - Token: " + numberToken + " is invalid";
                        throw new ScannerException(errorMessage);
                    }
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
            if (numberOfQuotes % 2 == 1)
            {
                string errorMessage = "Lexical Error at line " + lineNumber + " - Quotes not closed correctly";
                throw new ScannerException(errorMessage);
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
