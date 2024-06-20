using SpellChecker.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellChecker.Model
{
    public class WordChecker : IWordChecker
    {
        private readonly HashSet<string> dictionary;
        private readonly IWordProcessor wordProcessor;

        public WordChecker(HashSet<string> dictionary, IWordProcessor wordProcessor)
        {
            this.dictionary = dictionary;
            this.wordProcessor = wordProcessor;
        }

        public string CheckWord(string word)
        {
            // Return the word as is if it exists in the dictionary
            if (dictionary.Contains(word))
            {
                return word;
            }

            // Counts possible correction for the word from dictionary
            var corrections = new List<string>();
            foreach (var dictWord in dictionary)
            {
                // Checks if words length differs for more than 2 letters
                if (Math.Abs(word.Length - dictWord.Length) <= 2 || Math.Abs(dictWord.Length - word.Length) <= 2)
                {
                    var edits = wordProcessor.CalculateEdits(word, dictWord);

                    // Count only actual edits
                    int actualEditCount = edits.Count(e => e != "noedit");

                    // Add valid corrections within 2 edits
                    if (actualEditCount <= 2 && wordProcessor.IsValidEditSequence(edits))
                    {
                        corrections.Add(dictWord);
                    }
                }
            }

            // No valid corrections found
            if (corrections.Count == 0)
            {
                return $"{{{word}?}}";
            }

            // Return the single valid correction
            if (corrections.Count == 1)
            {
                return corrections[0];
            }

            // Checks for oneEdit corrections
            var oneEditCorrections = corrections.Where(c =>
            {
                var edits = wordProcessor.CalculateEdits(word, c);
                int actualEditCount = edits.Count(e => e != "noedit");
                return actualEditCount == 1;
            }).ToList();

            // Return the single valid one-edit correction
            if (oneEditCorrections.Count == 1)
            {
                return oneEditCorrections[0];
            }

            // Return all valid one-edit corrections
            if (oneEditCorrections.Count > 1)
            {
                return $"{{{string.Join(' ', oneEditCorrections)}}}";
            }

            // Return all valid corrections
            return $"{{{string.Join(' ', corrections)}}}";
        }
    }
}
