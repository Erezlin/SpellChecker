using SpellChecker.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellChecker.Model
{
    public class WordProcessor : IWordProcessor
    {
        public List<string> CalculateEdits(string source, string target)
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

        public bool IsValidEditSequence(List<string> edits)
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
                    // Invalid if two adjacent edits are of the same type (insert or delete)
                    return false;
                }
            }

            // Valid if the above condition never holds
            return true;
        }
    }
}
