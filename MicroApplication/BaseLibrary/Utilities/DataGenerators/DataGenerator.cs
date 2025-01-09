

namespace BaseLibrary.Utilities.DataGenerators
{

    public class DataGenerator
    {
        DigilCVUrlGenerator? _digilCVUrlGenerator;
        public DigilCVUrlGenerator DigilCVUrlGenerator { get { return _digilCVUrlGenerator ?? (_digilCVUrlGenerator = new DigilCVUrlGenerator()); } }
    }

    public class DigilCVUrlGenerator
    {
        public string RandomCVUrlGenerator(string name)
        {
            int PasswordLength = 5;
            string _allowedChars = "123456789ABCDEFGHJKLMNPQRSTUVWXYZ";
            Random randNum = new Random();
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;
            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }
    }
}
