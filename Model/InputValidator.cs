using SpellChecker.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellChecker.Model
{
    public class InputValidator : IInputValidator
    {
        private const int MaxWordLength = 50;

        public bool ValidateWord(string word)
        {
            // Checks if word length exceeds maximum word length
            if (word.Length > MaxWordLength)
            {
                Console.WriteLine($"Error: Word '{word}' exceeds maximum allowed length of {MaxWordLength} characters.");
                return false;
            }
            return true;
        }
    }
}
