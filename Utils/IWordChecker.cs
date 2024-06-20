using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellChecker.Utils
{
    public interface IWordChecker
    {
        string CheckWord(string word);
    }
}
