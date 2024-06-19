using System;
using System.Collections.Generic;
using System.Linq;



class SpellChecker
{
    // Function to calculate the Levenshtein distance and track the operations (only insertions and deletions)
    private static List<string> LevenshteinDistanceWithEdits(string source, string target)
    {
        int lengthSource = source.Length;
        int lengthTarget = target.Length;
        var matrix = new int[lengthSource + 1, lengthTarget + 1];
        var operations = new List<string>[lengthSource + 1, lengthTarget + 1];

        // Step 1: Initialize the first row and column
        for (int i = 0; i <= lengthSource; i++)
        {
            matrix[i, 0] = i;
            operations[i, 0] = new List<string>(Enumerable.Repeat("delete", i));
        }
        for (int j = 0; j <= lengthTarget; j++)
        {
            matrix[0, j] = j;
            operations[0, j] = new List<string>(Enumerable.Repeat("insert", j));
        }

        // Step 2: Compute the distances with the restriction on adjacent insertions/deletions
        for (int i = 1; i <= lengthSource; i++)
        {
            for (int j = 1; j <= lengthTarget; j++)
            {
                if (source[i - 1] == target[j - 1])
                {
                    // If characters are the same, no edit is needed
                    matrix[i, j] = matrix[i - 1, j - 1];
                    operations[i, j] = new List<string>(operations[i - 1, j - 1]) { "noedit" };
                }
                else
                {
                    // Calculate costs for insertion and deletion
                    int insertion = matrix[i, j - 1] + 1;
                    int deletion = matrix[i - 1, j] + 1;

                    if (insertion <= deletion)
                    {
                        // If insertion is the cheapest operation
                        matrix[i, j] = insertion;
                        operations[i, j] = new List<string>(operations[i, j - 1]) { "insert" };
                    }
                    else
                    {
                        // If deletion is the cheapest operation
                        matrix[i, j] = deletion;
                        operations[i, j] = new List<string>(operations[i - 1, j]) { "delete" };
                    }
                }
            }
        }

        // Return the list of operations to transform source into target
        return operations[lengthSource, lengthTarget];
    }

    // Function to check if a sequence of edits is valid according to the given restriction
    private static bool IsValidCorrection(List<string> edits)
    {
        for (int i = 1; i < edits.Count; i++)
        {
            // Ignore "noedit" in the validation process
            if (edits[i] == "noedit")
            {
                continue;
            }

            // Check if the current edit is the same as the previous and they are either 'insert' or 'delete'
            if (edits[i] == edits[i - 1] && (edits[i] == "insert" || edits[i] == "delete"))
            {
                return false; // Invalid if two adjacent edits are of the same type (insert or delete)
            }
        }
        return true; // Valid if the above condition never holds
    }

    // Function to check and correct a word based on the dictionary
    static string CheckWord(HashSet<string> dictionary, string word)
    {
        if (dictionary.Contains(word))
        {
            return word; // Return the word as is if it exists in the dictionary
        }

        var corrections = new List<string>();
        foreach (var dictWord in dictionary)
        {
            var edits = LevenshteinDistanceWithEdits(word, dictWord);
            int actualEditCount = edits.Count(e => e != "noedit"); // Count only actual edits
            if (actualEditCount <= 2 && IsValidCorrection(edits))
            {
                corrections.Add(dictWord); // Add valid corrections within 2 edits
            }
        }

        if (corrections.Count == 0)
        {
            return $"{{{word}?}}"; // No valid corrections found
        }

        var oneEditCorrections = corrections.Where(c =>
        {
            var edits = LevenshteinDistanceWithEdits(word, c);
            int actualEditCount = edits.Count(e => e != "noedit");
            return actualEditCount == 1;
        }).ToList();
        if (oneEditCorrections.Count == 1)
        {
            return oneEditCorrections[0]; // Return the single valid one-edit correction
        }

        if (oneEditCorrections.Count > 1)
        {
            return $"{{{string.Join(' ', oneEditCorrections)}}}"; // Return all valid one-edit corrections
        }

        if (corrections.Count == 1)
        {
            return corrections[0]; // Return the single valid correction
        }

        return $"{{{string.Join(' ', corrections)}}}"; // Return all valid corrections
    }

    public static void Main(string[] args)
    {
        // Read the dictionary and text input
        var dictionary = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var textLines = new List<string>();
        bool readingDictionary = true;

        while (true)
        {
            var line = Console.ReadLine();
            if (line == "===")
            {
                if (readingDictionary)
                {
                    readingDictionary = false;
                }
                else
                {
                    break;
                }
            }
            else
            {
                if (readingDictionary)
                {
                    var words = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var word in words)
                    {
                        dictionary.Add(word.Trim());
                    }
                }
                else
                {
                    textLines.Add(line);
                }
            }
        }

        // Process each line of text
        foreach (var line in textLines)
        {
            var words = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = CheckWord(dictionary, words[i]);
            }
            Console.WriteLine(string.Join(' ', words));
        }
    }
}
