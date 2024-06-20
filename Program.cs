using SpellChecker.Model;
using SpellChecker.Utils;
using System;
using System.Collections.Generic;
using System.Linq;



class Program
{
    static void Main(string[] args)
    {
        IInputValidator inputValidator = new InputValidator();
        // Display the startup message
        DisplayStartupMessage();

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
                        if (inputValidator.ValidateWord(word))
                        {
                            dictionary.Add(word.Trim());
                        }
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
                if (inputValidator.ValidateWord(words[i]))
                {
                    words[i] = wordChecker.CheckWord(words[i]);
                }
                else
                {
                    // Mark invalid words
                    words[i] = $"{{{words[i]}?}}"; 
                }
            }
            Console.WriteLine(string.Join(' ', words));
        }
    }

    // Function to display the startup message
    public static void DisplayStartupMessage()
    {
        Console.WriteLine("*******************************************");
        Console.WriteLine("*    Welcome to the Spell Checker App!    *");
        Console.WriteLine("*******************************************\n");

        Console.WriteLine("How to use this application:");
        Console.WriteLine("1. Type your dictionary words followed by a space.");
        Console.WriteLine("2. When finished, type '===' on a new line to end the dictionary input.");
        Console.WriteLine("3. Type the text lines you want to check and correct.");
        Console.WriteLine("4. When finished, type '===' on a new line to end the text input.");
        Console.WriteLine("5. The application will display the corrected text lines.\n");

        Console.WriteLine("Examples:");
        Console.WriteLine("Dictionary: rain spain plain plaint pain main mainly");
        Console.WriteLine("End dictionary input with: ===");
        Console.WriteLine("Text: hte rame in pain fells");
        Console.WriteLine("End text input with: ===\n");

        Console.WriteLine("Note:");
        Console.WriteLine("- The input is case-insensitive.");
        Console.WriteLine("- Corrections will be made for words within two edits from the dictionary.");
        Console.WriteLine("- Adjacent insertions or deletions of the same type are not allowed.\n");

        Console.WriteLine("Happy spell checking!");
        Console.WriteLine("*******************************************\n");
    }
}
