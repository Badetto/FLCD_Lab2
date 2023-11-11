using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiniteAutomata
{
    public class FiniteAutomata
    {
        string input;

        public List<string> States { get; set; }

        public List<string> Alphabet { get; set; }

        public List<Tuple<string, string, string>> Transitions { get; set; }

        public string InitialState { get; set; }

        public List<string> FinalStates { get; set; }

        public FiniteAutomata(string sourceCode)
        {
            input = sourceCode;
            States = new List<string>();
            Alphabet = new List<string>();
            Transitions = new List<Tuple<string, string, string>>();
            InitialState = "";
            FinalStates = new List<string>();
        }

        public void ReadFile()
        {
            string[] lines = input.Split('\n');
            string currentSection = null;

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();

                if (string.IsNullOrEmpty(trimmedLine))
                {
                    continue;
                }

                if (trimmedLine == "states")
                {
                    currentSection = "states";
                    continue;
                }
                else if (trimmedLine == "in_state")
                {
                    currentSection = "in_state";
                    continue;
                }
                else if (trimmedLine == "out_states")
                {
                    currentSection = "out_states";
                    continue;
                }
                else if (trimmedLine == "alphabet")
                {
                    currentSection = "alphabet";
                    continue;
                }
                else if (trimmedLine == "transitions")
                {
                    currentSection = "transitions";
                    continue;
                }

                switch (currentSection)
                {
                    case "states":
                        States.Add(trimmedLine);
                        break;
                    case "in_state":
                        InitialState = trimmedLine;
                        break;
                    case "out_states":
                        FinalStates.Add(trimmedLine);
                        break;
                    case "alphabet":
                        Alphabet.Add(trimmedLine);
                        break;
                    case "transitions":
                        string[] parts = trimmedLine.Split(',');
                        if (parts.Length == 3)
                        {
                            Transitions.Add(new Tuple<string, string, string>(parts[0].Trim(), parts[1].Trim(), parts[2].Trim()));
                        }
                        break;
                }
            }

            if (States.Count == 0 || InitialState == "" || FinalStates.Count == 0 || Alphabet.Count == 0 || Transitions.Count == 0)
            {
                throw new FormatException("Invalid file format.");
            }
        }

        public bool CheckSequence(List<string> sequence)
        {
            var state = InitialState;
            foreach (var chr in sequence)
            {
                string newState = null;
                foreach (var transition in Transitions)
                {
                    if (transition.Item1 == state && transition.Item2 == chr)
                    {
                        newState = transition.Item3;
                        break;
                    }
                }

                if (newState == null)
                {
                    return false;
                }
                    
                state = newState;
            }
            return FinalStates.Contains(state);
        }

        public void DisplayStates()
        {
            Console.WriteLine("States: " + string.Join(", ", States));
        }

        public void DisplayAlphabet()
        {
            Console.WriteLine("Alphabet: " + string.Join(", ", Alphabet));
        }

        public void DisplayTransitions()
        {
            Console.WriteLine("Transitions:");
            foreach (var transition in Transitions)
            {
                Console.WriteLine($"{transition.Item1} --({transition.Item2})--> {transition.Item3}");
            }
        }

        public void DisplayInitialState()
        {
            Console.WriteLine("Initial State: " + InitialState);
        }

        public void DisplayFinalStates()
        {
            Console.WriteLine("Final States: " + string.Join(", ", FinalStates));
        }
    }
}
