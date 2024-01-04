using System.Linq;
using NHunspell;

namespace FuzzyTranslator
{
    public static class Stemming
    {
        private static readonly Hunspell Stemmer = new Hunspell("en-US.aff", "en-US.dic");
        public static string Stem(string input)
        {
            return Stemmer.Stem(input.ToUpper()).OrderBy(x => x.Length).FirstOrDefault()?.ToUpper() ?? input.ToUpper();
        }
    }
}