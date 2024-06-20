using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellChecker.Utils
{
    public interface IWordProcessor
    {
        List<string> CalculateEdits(string source, string target);
        bool IsValidEditSequence(List<string> edits);
    }
}
