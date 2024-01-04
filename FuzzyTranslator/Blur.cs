using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzyTranslator
{
    internal static class Blur
    {
        private const string DIRECTORY = "Data";
        private const string FILENAME = "generatedWords.dat";

        public static void Load()
        {
            //Load generated words
            try
            {
                GeneratedWords = Serialization.DeserializeFromFile<Dictionary<Language, Dictionary<string, string>>>(DIRECTORY, FILENAME);
            }

            //Create words if it doesn't exist and serialize to file
            catch (Exception)
            {
                Generate();
                Serialization.SerializeToFile(GeneratedWords, DIRECTORY, FILENAME);
            }
        }

        internal static Dictionary<Language, Dictionary<string, string>> GeneratedWords = new Dictionary<Language, Dictionary<string, string>>();
        public static string LanguageBased(Language language, string word)
        {
            return GeneratedWords[language][word];
        }

        #region Language Generation

        public static event EventHandler<string> ProcessingWord;

        private static readonly Dictionary<Language, string[]> Consonants = new Dictionary<Language, string[]>
        {
            { Language.Blin,     new [] {"Ꮣ", "Ꮥ", "ᛕ", "Ꮨ", "Ᏽ", "ዘ", "ጠ", "የ", "ነ", "ፕ", "ሀ", "ሠ", "ሃ", "Ꮑ", "Ꮒ", "Ꮷ", "Ᏺ", "Ꮯ", "Ꮧ"}},
            { Language.Deku,     new [] {"𐑐", "𐑓", "𐑕", "𐑖", "𐑚", "𐑝", "𐑟", "𐑠", "𐑢", "𐑤", "𐑥", "𐑦", "𐑧", "𐑨", "𐑩", "𐑪", "𐑮", "𐑯"}},
            { Language.Fae,      new [] {"Ꭳ", "Ꭷ", "Ꮂ", "Ꮡ", "Ꮱ", "Ꮺ", "Ꮼ", "Ꮽ", "Ꮾ", "Ꮿ", "Ꮘ", "Ꮚ", "Ꮗ", "Ꮉ", "Ꮝ", "Ꮫ"}},
            { Language.Gerudo,   new [] {"ꡑ", "ꡇ", "꡵", "ꡈ", "ꡥ", "ꡘ", "ꡱ", "ꡡ", "ꡊ", "ꡪ", "ꡫ", "ꡢ", "ꡒ", "ꡌ", "ꡯ", "ꡠ", "ꡨ", "ꡆ", "ꡤ", "ꡖ", "ꡛ", "Ⱑ"}},
            { Language.Goron,    new [] {"߁‎", "߂", "߃", "߄", "߅", "߆", "߇", "ߊ", "ߌ", "ߍ", "ߎ", "ߏ", "ߑ", "ߓ", "ߔ", "ߖ", "ߗ", "ߙ", "ߞ", "ߠ", "ߣ", "ߤ", "߷"}},
            { Language.Hylian,   new [] {"ꡑ", "ꡔ", "꡵", "ꡩ", "ꡚ", "ꡘ", "ꡱ", "ꡡ", "꡷", "ꡉ", "ꡃ", "ꡢ", "ꡒ", "ꡌ", "ꡙ", "ꡧ", "ꡨ", "ꡆ", "ꡤ", "ꡖ", "ꡛ", "Ⱑ"}},
            { Language.Lizalfos, new [] {"Ⰰ", "Ⰱ", "Ⰵ", "Ⰽ", "Ⱀ", "Ⱁ", "Ⱂ", "Ⱃ", "Ⱇ", "Ⱔ", "Ⱚ", "Ⱜ", "Ⱝ"}},
            { Language.Lynel,    new [] {"Б", "Г", "И", "Л", "Н", "П", "Р", "С", "Т", "Х", "Ѕ", "Ѵ", "П"}},
            { Language.Octorok,  new [] {"ꡁ", "ꡂ", "ꡄ", "ꡅ", "ꡣ", "ꡓ", "ꡢ"}},
            { Language.Wizkin,   new [] {"Բ", "Խ", "Թ", "Ձ", "Ջ"}},
            { Language.Zora,     new [] {"Ⰰ", "Ⰱ", "Ⰵ", "Ⰷ", "Ⰸ", "Ⰻ", "Ⰼ", "Ⰽ", "Ⱀ", "Ⱁ", "Ⱂ", "Ⱃ", "Ⱄ", "Ⱅ", "Ⱆ", "Ⱇ", "Ⱈ", "Ⱉ", "Ⱊ", "Ⱋ", "Ⱌ", "Ⱍ", "Ⱎ", "Ⱏ", "Ⱐ", "Ⱒ", "Ⱓ", "Ⱔ", "Ⱕ", "Ⱖ", "Ⱗ", "Ⱘ", "Ⱙ", "Ⱚ", "Ⱛ", "Ⱜ", "Ⱝ", "Ⱞ", "Ꮬ"}},
        };

        private static readonly Dictionary<Language, string[]> Vowels = new Dictionary<Language, string[]>
        {
            { Language.Blin,     new [] {"Ꭿ", "ዕ", "ዐ", "Ꮜ", "Ꮖ", "ሁ"}},
            { Language.Deku,     new [] {"𐑔", "𐑙", "𐑞", "𐑴", "𐑼"}},
            { Language.Fae,      new [] {"Ꮎ", "Ꮸ", "Ꭴ", "Ꮻ", "Ꮕ", "Ꮊ"}},
            { Language.Gerudo,   new [] {"ꡍ", "ꡀ", "ꡲ", "ꡟ", "ꡦ"}},
            { Language.Goron,    new [] {"ߐ", "ߥ", "ߦ", "ߧ", "߉", "‎ߒ", "ߕ", "ߝ", "ߘ", "ߟ", "ߢ", "߶", "߈", "ߚ", "ߜ", "‎ߡ"}},
            { Language.Hylian,   new [] {"꡴", "ꡗ", "꡶", "ꡟ", "ꡜ"}},
            { Language.Lizalfos, new [] {"Ⰲ", "Ⰳ", "Ⰴ", "Ⰶ", "Ⰹ"}},
            { Language.Lynel,    new [] {"Ч", "Є", "З", "І", "О", "Ц"}},
            { Language.Octorok,  new [] {"ꡐ", "ꡏ", "ꡋ", "ꡞ", "ꡟ", "ꡬ"}},
            { Language.Wizkin,   new [] {"Ա", "Գ", "Ժ", "Ծ", "Կ", "Ճ", "Մ", "Յ", "Ս", "Վ", "Օ"}},
            { Language.Zora,     new [] {"Ⰲ", "Ⰳ", "Ⰴ", "Ⰶ", "Ⰹ", "Ⰺ", "Ⰾ", "Ⰿ"}},
        };

        private static readonly Dictionary<Language, Tuple<string, double>[]> NormalPhonemeRules = new Dictionary<Language, Tuple<string, double>[]>
        {
            { Language.Blin, new[]
            {
                Tuple.Create("cv",   0.30),
                Tuple.Create("ccv",  0.15),
                Tuple.Create("cckv", 0.05),
                Tuple.Create("vcv",  0.10),
                Tuple.Create("v",    0.25),
                Tuple.Create("vv",   0.15),
            }},
            { Language.Deku, new[]
            {
                Tuple.Create("cv",    0.10),
                Tuple.Create("cc~",   0.25),
                Tuple.Create("cvc~",  0.35),
                Tuple.Create("cc~k~", 0.03),
                Tuple.Create("vcv",   0.12),
                Tuple.Create("vc~",   0.15),
            }},
            { Language.Fae, new[]
            {
                Tuple.Create("cv",     0.45),
                Tuple.Create("cvk'",   0.45),
                Tuple.Create("cvv",    0.30),
                Tuple.Create("cvvk'",  0.30),
                Tuple.Create("cvvv",   0.20),
                Tuple.Create("cvvvk'",  0.05),
            }},
            { Language.Gerudo, new[]
            {
                Tuple.Create("cv",    0.35),
                Tuple.Create("ccv",   0.13),
                Tuple.Create("cc'kv", 0.02),
                Tuple.Create("vcv",   0.15),
                Tuple.Create("v",     0.25),
                Tuple.Create("vv",    0.10),
            }},
            { Language.Goron, new[]
            {
                Tuple.Create("cv",     0.10),
                Tuple.Create("cvv",    0.10),
                Tuple.Create("cvkvc'", 0.10),
                Tuple.Create("vcv",    0.10),
                Tuple.Create("vc'",    0.10),
                Tuple.Create("vvc'",   0.10),
            }},
            { Language.Hylian, new[]
            {
                Tuple.Create("cv",    0.30),
                Tuple.Create("ccv",   0.12),
                Tuple.Create("cc'kv", 0.03),
                Tuple.Create("vcv",   0.20),
                Tuple.Create("v",     0.20),
                Tuple.Create("vv",    0.15),
            }},
            { Language.Lizalfos, new[]
            {
                Tuple.Create("v",     1.0),
                Tuple.Create("vv",    1.0),
                Tuple.Create("cv",    1.0),
                Tuple.Create("cvv",   1.0),
                Tuple.Create("c'cv",  1.0),
                Tuple.Create("c'cvv", 1.0),
            }},
            { Language.Lynel, new[]
            {
                Tuple.Create("cv",    0.10),
                Tuple.Create("ccv",   0.25),
                Tuple.Create("cc'kv", 0.15),
                Tuple.Create("vcv",   0.10),
                Tuple.Create("v",     0.25),
                Tuple.Create("vv",    0.15),
            }},
            { Language.Octorok, new[]
            {
                Tuple.Create("cv", 0.5),
                Tuple.Create("vc", 0.5),
            }},
            { Language.Wizkin, new[]
            {
                Tuple.Create("cv",     0.45),
                Tuple.Create("cvv",    0.30),
                Tuple.Create("cvvv",   0.20),
                Tuple.Create("cvvvv",  0.05),
            }},
            { Language.Zora, new[]
            {
                Tuple.Create("v",     1.0),
                Tuple.Create("vv",    1.0),
                Tuple.Create("cv",    1.0),
                Tuple.Create("cvv",   1.0),
                Tuple.Create("ccv",   1.0),
                Tuple.Create("ccvv",  1.0),
                Tuple.Create("cvkv",  1.0),
                Tuple.Create("c'kv",  1.0),
                Tuple.Create("cv'kv", 1.0),
                Tuple.Create("c'vk",  1.0),
                Tuple.Create("vc'vk", 1.0),
                Tuple.Create("k'k'",  1.0),
            }},
        };

        private static readonly Dictionary<Language, Tuple<string, double>[]> EndingPhonemeRules = new Dictionary<Language, Tuple<string, double>[]>
        {
            { Language.Blin, new[]
            {
                Tuple.Create("c",   0.40),
                Tuple.Create("cc",  0.10),
                Tuple.Create("cck", 0.05),
                Tuple.Create("v",   0.30),
                Tuple.Create("vv",  0.15),
            }},
            { Language.Deku, new[]
            {
                Tuple.Create("cc",  0.25),
                Tuple.Create("cvc", 0.25),
                Tuple.Create("vcv",  0.25),
                Tuple.Create("vv~",  0.25),
            }},
            { Language.Fae, new[]
            {
                Tuple.Create("v",    0.05),
                Tuple.Create("vv",   0.15),
                Tuple.Create("vvv",  0.25),
                Tuple.Create("vc",   0.05),
                Tuple.Create("vvc",  0.15),
                Tuple.Create("vvvc", 0.25),
                Tuple.Create("~cv",   0.10),
            }},
            { Language.Gerudo, new[]
            {
                Tuple.Create("c",   0.25),
                Tuple.Create("cc",  0.18),
                Tuple.Create("cck", 0.02),
                Tuple.Create("v",   0.30),
                Tuple.Create("vv",  0.25),
            }},
            { Language.Goron, new[]
            {
                Tuple.Create("-cvkv", 1.00),
            }},
            { Language.Hylian, new[]
            {
                Tuple.Create("c",   0.25),
                Tuple.Create("cc",  0.13),
                Tuple.Create("cck", 0.02),
                Tuple.Create("v",   0.30),
                Tuple.Create("vv",  0.30),
            }},
            { Language.Lizalfos, new[]
            {
                Tuple.Create("v", 1.0)
            }},
            { Language.Lynel, new[]
            {
                Tuple.Create("c",    0.25),
                Tuple.Create("cc",   0.15),
                Tuple.Create("cc'k", 0.15),
                Tuple.Create("c'ck", 0.10),
                Tuple.Create("v",    0.20),
                Tuple.Create("vv",   0.15),
            }},
            { Language.Octorok, new[]
            {
                Tuple.Create("~cv", 0.5),
                Tuple.Create("~vc", 0.5),
            }},
            { Language.Wizkin, new[]
            {
                Tuple.Create("v",      0.025),
                Tuple.Create("vv",     0.10),
                Tuple.Create("vvv",    0.15),
                Tuple.Create("vvvv",   0.20),
                Tuple.Create("vc",     0.025),
                Tuple.Create("vvc",    0.10),
                Tuple.Create("vvvc",   0.15),
                Tuple.Create("vvvvc",  0.20),
            }},
            { Language.Zora, new[]
            {
                Tuple.Create("v", 1.0)
            }},
        };

        private static readonly Dictionary<Language, double> AddEndingPhonemeProbabilty = new Dictionary<Language, double>
        {
            { Language.Blin,     0.90},
            { Language.Deku,     0.50},
            { Language.Fae,      0.75},
            { Language.Gerudo,   0.85},
            { Language.Goron,    0.25},
            { Language.Hylian,   0.85},
            { Language.Lizalfos, 0.15},
            { Language.Lynel,    0.75},
            { Language.Octorok,  0.80},
            { Language.Wizkin,   1.00},
            { Language.Zora,     0.20},
        };

        private const double ALPHABET_LENGTH = 26.0;
        private static Dictionary<Language, Dictionary<string, string>> Generate()
        {
            try
            {
                var existingWords = new Dictionary<Language, HashSet<string>>();
                foreach (var word in TranslatorViewModel.CommonWords.Keys)
                {
                    ProcessingWord?.Invoke(null, word);

                    foreach (var language in Enum.GetValues(typeof(Language)).Cast<Language>())
                    {
                        if (!GeneratedWords.Keys.Contains(language))
                        {
                            GeneratedWords.Add(language, new Dictionary<string, string>());
                            existingWords.Add(language, new HashSet<string>());
                        }
                            

                        if (language == Language.Hylian && Rand.NextDouble() < 0.885)
                        {
                            var sharedWord = GeneratedWords[Language.Gerudo][word];
                            var hylianWord = "";
                            foreach (var letter in sharedWord.Select(s => s.ToString()))
                            {
                                if (Consonants[Language.Gerudo].Contains(letter))
                                {
                                    var consonantIndex = Consonants[Language.Gerudo].ToList().IndexOf(letter);
                                    hylianWord += Consonants[Language.Hylian][consonantIndex];
                                }
                                else if (Vowels[Language.Gerudo].Contains(letter))
                                {
                                    var vowelIndex = Vowels[Language.Gerudo].ToList().IndexOf(letter);
                                    hylianWord += Vowels[Language.Hylian][vowelIndex];
                                }
                                else
                                {
                                    hylianWord += letter;
                                }
                            }
                            GeneratedWords[Language.Hylian].Add(word, hylianWord);
                            existingWords[Language.Hylian].Add(hylianWord);
                            continue;
                        }

                        var targetAlphabetLength = Consonants[language].Length + Vowels[language].Length * 1.0;
                        var transformedLength = word.Length * ALPHABET_LENGTH / targetAlphabetLength;
                        var targetWordLength = transformedLength < 4 ? transformedLength : (int)Math.Floor(Math.Max(1, transformedLength + Rand.Next(-3, 1)));

                        var stemmedWords = TranslatorViewModel.StemmedWords;
                        var similar = stemmedWords[Stemming.Stem(word)].Where(w => w != word)
                            .Where(w => GeneratedWords[language].ContainsKey(w))
                            .ToList();

                        var generated = "";
                        do
                        {
                            generated = GenerateWord(language, targetWordLength, similar);
                            targetWordLength += 1;
                        } while (existingWords[language].Contains(generated));

                        GeneratedWords[language].Add(word, generated);
                        existingWords[language].Add(generated);
                    }
                }
                return GeneratedWords;
            }
            catch (Exception e)
            {
                Console.Write(e);
                throw;
            }
        }

        private static string GenerateWord(Language lang, double targetWordLength, List<string> similar)
        {
            try
            {
                var generatedWord = "";
                if (similar.Any())
                {
                    var selected = similar.OrderBy(x => x.Length).First();
                    var root = GeneratedWords[lang][selected];
                    var stem = lang != Language.Deku
                        ? Rand.Next(4, root.Length + 1)
                        : root.Length;
                    generatedWord += string.Concat(root.Take(stem));
                    targetWordLength -= stem;
                    if (targetWordLength < 1) targetWordLength = 2;
                    generatedWord += GenerateWord(lang, targetWordLength);
                }
                else
                {
                    generatedWord += GenerateWord(lang, targetWordLength);
                }
                return generatedWord;
            }
            catch (Exception e)
            {
                Console.Write(e);
                throw;
            }
        }

        private static string GenerateWord(Language lang, double targetWordLength)
        {
            try
            {
                var expectedEndLength = EndingPhonemeRules[lang].Sum(x => x.Item1.Length * x.Item2);
                var generatedWord = "";
                do
                {
                    var phonemePattern = Rand.SelectWeighted(NormalPhonemeRules[lang]);
                    var generatedPhoneme = ParsePattern(phonemePattern, Consonants[lang], Vowels[lang]);
                    generatedWord += generatedPhoneme;
                } while (generatedWord.Length < targetWordLength - expectedEndLength);
                if (generatedWord.Length < targetWordLength + 3 && Rand.NextDouble() < AddEndingPhonemeProbabilty[lang])
                {
                    var phonemePattern = Rand.SelectWeighted(EndingPhonemeRules[lang]);
                    var generatedPhoneme = ParsePattern(phonemePattern, Consonants[lang], Vowels[lang]);
                    generatedWord += generatedPhoneme;
                }
                return generatedWord;
            }
            catch (Exception e)
            {
                Console.Write(e);
                throw;
            }
        }

        private static string ParsePattern(string pattern, string[] consonants, string[] vowels)
        {
            try
            {
                var toGo = pattern.ToList();
                var phoneme = "";
                do
                {
                    var current = toGo[0];
                    toGo.RemoveAt(0);

                    if (current == 'c')
                    {
                        phoneme += Rand.Select(consonants);
                    }
                    else if (current == 'k')
                    {
                        phoneme += Rand.Select(consonants.Where(c => !phoneme.Contains(c)).ToArray());
                    }
                    else if (current == 'v')
                    {
                        phoneme += Rand.Select(vowels);
                    }
                    else
                    {
                        phoneme += current;
                    }
                } while (toGo.Any());
                return phoneme;
            }
            catch (Exception e)
            {
                Console.Write(e);
                throw;
            }
        }

        #endregion
    }
}
