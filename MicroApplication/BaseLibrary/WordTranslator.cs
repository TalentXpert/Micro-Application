namespace BaseLibrary
{
    public class WordTranslator
    {
        static WordTranslator()
        {
            InitialiseWords();
        }

        public static Dictionary<string, Tuple<string, string, string, string, string>> words = new Dictionary<string, Tuple<string, string, string, string, string>>();
        private static void AddWord(string key, string en, string de, string hu, string fr, string br)
        {
            words[key] = new Tuple<string, string, string, string, string>(en, de, hu, fr, br);
        }

        private static string TranslateWord(string key, string language)
        {
            Tuple<string, string, string, string, string> result;
            if (words.TryGetValue(key, out result))
            {
                if (language.Equals("en", StringComparison.InvariantCultureIgnoreCase))
                    return result.Item1;
                if (language.Equals("de", StringComparison.InvariantCultureIgnoreCase))
                    return result.Item2;
                if (language.Equals("hu", StringComparison.InvariantCultureIgnoreCase))
                    return result.Item3;
                if (language.Equals("fr", StringComparison.InvariantCultureIgnoreCase))
                    return result.Item4;
                if (language.Equals("br", StringComparison.InvariantCultureIgnoreCase))
                    return result.Item5;
            }

            return "";
        }

        public static string Translate(string key, string language)
        {
            if (string.IsNullOrWhiteSpace(key))
                return key;
            key = key.Trim();
            var result = TranslateWord(key, language);
            if (string.IsNullOrWhiteSpace(result) == false)
                return result;
            return key;
        }
        private static void InitialiseWords() { }
    }
}
