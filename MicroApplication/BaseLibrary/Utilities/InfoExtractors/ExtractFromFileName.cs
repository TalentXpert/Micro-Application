using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BaseLibrary.Utilities
{
    public abstract class ExtractNameBase
    {
        private static List<string> invalidWordInLine = new List<string> { "Resume", "Curriculum", "Vitae", "Summary", "Technical", "Skills", "Professional", "Work", "Experience", "Qualification", "Email :", "OBJECTIVES", "PHP developer", "AREAS OF EXPERTISE", "PERSONAL DETAILS", "Reference", "PROFILE ABOUT ME", "City", "Temporary" };
        private static List<string> unwantedWordsInName = new List<string> { "Mr.", "Name", ":", "Contact", "Details", "Permanent address", "My", "Mobile No" };
        private static List<string> lineStartWithLabels = new List<string> { "name:", "name :" };

        protected bool IsLineStartWithLabel(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return false;
            foreach (var label in lineStartWithLabels)
                if (line.StartsWith(label, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        protected bool ContainInvalidWord(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return false;
            foreach (var invalidWord in invalidWordInLine)
            {
                if (line.Contains(invalidWord, StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }

        protected string RemoveInvalidWordsFromLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return line;
            foreach (var invalidWord in invalidWordInLine)
                line = line.Replace(invalidWord, "", StringComparison.OrdinalIgnoreCase);
            return line;
        }

        protected string RemoveUnwantedWordsInName(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return line;
            foreach (var invalidWord in unwantedWordsInName)
                line = line.Replace(invalidWord, "", StringComparison.OrdinalIgnoreCase);
            return line;
        }

        protected string BuildName(List<string> words)
        {
            string name = string.Empty;
            int i = 0;
            foreach (var word in words)
            {
                i += 1;
                if (i <= 3)
                    name = name + " " + word;
                else
                    break;
            }
            return name.Trim();
        }

        //private string ExtractNameFromWord(string word)
        //{
        //    string name = word[0].ToString();
        //    foreach (var character in word.Substring(1))
        //    {
        //        if (!char.IsLower(character))
        //            break;
        //        name += character;
        //    }
        //    return name;
        //}

        protected List<string> ExtractNameParts(string line)
        {
            var result = new List<string>();
            if (string.IsNullOrWhiteSpace(line)) return result;
            string name = string.Empty;
            foreach (var character in line)
            {
                //lower is part of name so continue adding to existing name part
                if (char.IsLower(character))
                {
                    name += character.ToString();
                }

                //There is possibility that this is next part of name but its can not be if name is written in capital letter like AMIT SINGH
                if (char.IsUpper(character) || !char.IsLetter(character))
                {
                    if (!string.IsNullOrWhiteSpace(name))
                        result.Add(name);

                    if (char.IsLetter(character))
                        name = character.ToString();
                    else
                        name = string.Empty;
                }
            }

            //now lets combine capital letter name which is divided into pieces
            name = string.Empty;
            var newResult = new List<string>();
            foreach (var res in result)
            {
                if (res.Length == 1)
                    name += res;
                else
                {
                    newResult.Add(name);
                    name = res;
                }
            }

            return newResult;
        }
    }


    public class ExtractUsingLabel : ExtractNameBase
    {
        public string ExtractName(string cvText)
        {
            using (StringReader sr = new StringReader(cvText))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    if (IsLineStartWithLabel(line))
                    {
                        line = RemoveInvalidWordsFromLine(line);
                        line = RemoveUnwantedWordsInName(line);
                        var nameParts = ExtractNameParts(line);
                        return BuildName(nameParts);
                    }
                }
            }
            return string.Empty;
        }
    }

    public class ExractFromLinesNearEmail : ExtractNameBase
    {
        public string ExtractName(string cvText, List<string> emails)
        {
            string name = null;
            bool hasEmail = emails.Count > 0;
            if (hasEmail)
            {
                var lines = new List<string>();
                using (StringReader sr = new StringReader(cvText))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        line = GetStringBeforeWord(line); //removed mobile: ,E-mail: this word from line like - Harishankar SharmaE-mail: 20harishankar@gmail.comMobile: +91-851800493355
                        if (IsLineLengthValid(line))
                            lines.Add(SantiseLine(line));
                        if (line.Contains(emails[0])) break;
                    }
                    lines = RemoveUnwantedLines(lines);
                    name = ExtractNameFromLines(lines);
                }
                name = RemoveInvalidWordsFromLine(name);
                name = RemoveUnwantedWordsInName(name);
            }

            return name;
        }
        
        private string SantiseLine(string line)
        {
            line = line.Trim();
            line = Regex.Replace(line, @"\s+", " ");
            return line;
        }

        private List<string> RemoveUnwantedLines(List<string> lines)
        {
            var result = new List<string>();
            foreach (var line in lines)
            {
                if (IsValidLine(line))
                    result.Add(line);
            }
            return result;
        }

        private bool IsValidLine(string line)
        {
            var temp = line.Replace("\u200B", "");
            foreach (var character in temp.ToCharArray())
            {
                if (character == ' ' || character == '.' || character == ':') continue;
                if (char.IsLetter(character)) continue;
                return false;
            }

            if (ContainInvalidWord(line))
                return false;

            return true;
        }

        private bool IsLineLengthValid(string line)
        {
            var words = line.Split(" ");
            var result = words.Where(w => !string.IsNullOrWhiteSpace(w)).ToList();
            if (result.Count() >= 1 && result.Count() <= 6) return true;
            return false;
        }

        private string ExtractNameFromLines(List<string> lines)
        {
            if (lines.Count == 1) return GetNameFromLine(lines[0]);
            foreach (var line in lines)
            {
                if (IsEachWordStartWithCapital(line))
                {
                    if (line.Length > 5) return line;
                }
            }
            return string.Empty;
        }

        private static string GetNameFromLine(string line)
        {
            return line.Trim();
        }

        private static bool IsEachWordStartWithCapital(string line)
        {
            var words = line.Split(" ");
            foreach (var word in words)
            {
                if (InvalidNameCharacters(word)) continue;
                if (string.IsNullOrWhiteSpace(word)) return false;
                if (!Char.IsUpper(word[0])) return false;
            }
            return true;
        }

        private static bool InvalidNameCharacters(string word)
        {
            string invalid = "!@$%^&*_|{}:;<>,?/";
            return invalid.Contains(word);
        }

        public string GetStringBeforeWord(string line)
        {
            var words = new List<string> { "Mobile:", "E-mail:", "Email", " ", "Contact", "Ph", "Address", "Experience:", "Java", "Mobile No" };
            foreach (var word in words)
            {
                if (line.Contains(word, StringComparison.OrdinalIgnoreCase))
                {
                    int charLocation = line.IndexOf(word, StringComparison.Ordinal);
                    if (charLocation > 0)
                    {
                        line= line.Substring(0, charLocation);
                    }
                }
            }
            return line;
        }

    }

    public class ExtractFromFileName : ExtractNameBase
    {
        public string ExtractName(string filename, string cvText)
        {
            var result = new List<string>();
            filename = RemoveExtension(filename);
            var nameParts = ExtractNameParts(filename);
            foreach (var namePart in nameParts)
            {
                if (cvText.Contains(namePart, StringComparison.OrdinalIgnoreCase))
                    result.Add(namePart);
            }

            var name = BuildName(result);
            name = name.Trim();
            name = RemoveInvalidWordsFromLine(name);
            name = RemoveUnwantedWordsInName(name);
            return name;
        }

        private string RemoveExtension(string filename)
        {
            var file = Path.GetFileName(filename);
            var extension = Path.GetExtension(file);
            file = file.Replace(extension, "");
            return file.Replace(".", "");
        }

        //private string BuildName(List<string> words)
        //{
        //    string name = string.Empty;
        //    int i = 1;
        //    foreach (var word in words)
        //    {
        //        i += 1;
        //        if (i <= 3)
        //            name = name + " " + ExtractNameFromWord(word);
        //        else
        //            break;
        //    }
        //    return name;
        //}

        //private string ExtractNameFromWord(string word)
        //{
        //    string name = word[0].ToString();
        //    foreach (var character in word.Substring(1))
        //    {
        //        if (!char.IsLower(character))
        //            break;
        //        name += character;
        //    }
        //    return name;
        //}
    }
}
