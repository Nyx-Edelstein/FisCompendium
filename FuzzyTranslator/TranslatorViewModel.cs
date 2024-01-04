using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FuzzyTranslator
{
    public class TranslatorViewModel : INotifyPropertyChanged
    {
        private const string LANGUAGE_LEVELS_DIRECTORY = "Data";
        internal static readonly Dictionary<string,int> CommonWords = new Dictionary<string, int>();
        internal static readonly Dictionary<string, List<string>> StemmedWords = new Dictionary<string, List<string>>();
        private Dictionary<Language, int> LanguageLevels = new Dictionary<Language, int>();

        public TranslatorViewModel()
        {
            //Load words
            var textSplit = Words.WORDS.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            var i = 1;
            foreach (var word in textSplit)
            {
                CommonWords.Add(word.ToUpper(), i);
                var stem = Stemming.Stem(word);
                if (!StemmedWords.ContainsKey(stem)) StemmedWords.Add(stem, new List<string>());
                StemmedWords[stem].Add(word);
                i++;
            }

            foreach (var lang in PossibleLanguages)
            {
                LanguageLevels.Add(lang, 0);
            }

            Task.Run(() =>
            {
                try
                {
                    Blur.Load();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            });
        }
        
        public IEnumerable<Language> PossibleLanguages => Enum.GetValues(typeof(Language)).Cast<Language>().ToList();

        private Language _selectedLanguage;
        public Language SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                _selectedLanguage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Limit));
                OnPropertyChanged(nameof(FuzzyText));
            }
        }

        public int Limit
        {
            get { return LanguageLevels[SelectedLanguage]; }
            set
            {
                if (value > CommonWords.Count)
                    LanguageLevels[SelectedLanguage] = CommonWords.Count;
                else if (value < 0)
                    LanguageLevels[SelectedLanguage] = 0;
                else LanguageLevels[SelectedLanguage] = value;

                OnPropertyChanged(nameof(Limit));
                OnPropertyChanged(nameof(FuzzyText));
            }
        }

        private double _cost;
        public double Cost
        {
            get { return _cost; }
            private set
            {
                _cost = value;
                OnPropertyChanged();
            }
        }

        private string _inputText = "";
        public string InputText
        {
            get { return _inputText; }
            set
            {
                _inputText = value;
                OnPropertyChanged(nameof(InputText));
                OnPropertyChanged(nameof(FuzzyText));
            }
        }

        public string FuzzyText => Fuzzy(InputText);

        private string Fuzzy(string inputText)
        {
            var output = "";
            var punctuation = inputText.Where(c => c != '~' && Char.IsPunctuation(c)).Distinct().ToArray();
            var words = inputText.Split().Select(x => x.Trim(punctuation));
            Cost = 0;
            foreach (var word in words)
            {
                if (word.StartsWith("~"))
                {
                    output += string.Concat(word.Skip(1)) + " ";
                    Cost += 10;
                    continue;
                }

                var contains = CommonWords.ContainsKey(word.ToUpper()) && CommonWords[word.ToUpper()] <= Limit;
                if (contains)
                {
                    output += word + " ";
                    Cost += Math.Ceiling(Math.Log(CommonWords[word.ToUpper()]) + 1);
                    continue;
                }

                var stemmed = Stemming.Stem(word);
                var containsStemmed = CommonWords.ContainsKey(stemmed) && CommonWords[stemmed] <= Limit;
                if (containsStemmed)
                {
                    output += word + " ";
                    Cost += Math.Ceiling(Math.Pow(Math.Log(CommonWords[stemmed]), 2) + 1);
                    continue;
                }

                var wordAllowed = CommonWords.ContainsKey(word.ToUpper());
                if (wordAllowed)
                {
                    var languageBlurred = Blur.LanguageBased(SelectedLanguage, word.ToUpper());
                    output += languageBlurred + " ";
                    continue;
                }

                var blurred = new string('*', word.Length);
                output += blurred + " ";
            }
            Cost = Math.Round(Cost);
            return output;
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
