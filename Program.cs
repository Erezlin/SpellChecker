using SpellChecker.Model;
using System;
using System.Collections.Generic;
using System.Linq;



class Program
{
    static void Main(string[] args)
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

        // Create a word processor and word checker
        var wordProcessor = new WordProcessor();
        var wordChecker = new WordChecker(dictionary, wordProcessor);

        // Process each line of text
        foreach (var line in textLines)
        {
            var words = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = wordChecker.CheckWord(words[i]);
            }
            Console.WriteLine(string.Join(' ', words));
        }
    }
}
