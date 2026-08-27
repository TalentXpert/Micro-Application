using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Xml;

namespace BaseLibrary.Utilities.Files
{
    public class StudyDocumentTagConst
    {
        public const string PrefixFirstPart = "<<";
        public const string PrefixSecondPart = "@";
        public const string PostfixFirstPart = ">>";

        public static string GetPrefix()
        {
            return PrefixFirstPart + PrefixSecondPart;
        }

        public static string GetPostfix()
        {
            return PostfixFirstPart;
        }
        public static string GetTagPlaceHolder(string tag)
        {
            return $"{GetPrefix()}{tag}{GetPostfix()}";
        }
    }

    public class DocxFileReader
    {
        private static IEnumerable<HeaderPart> GetDocumentHeaders(WordprocessingDocument doc)
        {
            if (doc.MainDocumentPart is not null)
                return doc.MainDocumentPart.HeaderParts;
            return [];
        }
        private static IEnumerable<OpenXmlElement> GetDocumentElements(WordprocessingDocument doc)
        {
            var body = doc?.MainDocumentPart?.Document.Body;
            if (body != null)
                return body.Elements();
            return [];
        }
        private static IEnumerable<FooterPart> GetDocumentFooters(WordprocessingDocument doc)
        {
            if (doc.MainDocumentPart is not null)
                return doc.MainDocumentPart.FooterParts;
            return [];
        }
        public static string ConvertDocxFileInToText(string path)
        {
            if (!path.Contains(".docx") && !path.Contains(".DOCX")) throw new ValidationException("Provided file extension is not .docx so can not convert it.");

            StringBuilder text = new StringBuilder();
            try
            {
                using (var doc = WordprocessingDocument.Open(path, false))
                {
                    foreach (var headerpart in GetDocumentHeaders(doc)) // code for extract cv details from header section
                    {
                        foreach (var header in headerpart.Header)
                        {
                            string paragraph = string.Empty;
                            if (header.HasChildren)
                            {
                                foreach (var t in header.ChildElements)
                                {
                                    if (t.LocalName == "tr")
                                    {
                                        if (string.IsNullOrWhiteSpace(t.InnerText)) continue;
                                        paragraph += t.InnerText;
                                    }
                                    else
                                    {
                                        if (string.IsNullOrWhiteSpace(t.InnerText)) continue;
                                        paragraph += t.InnerText;
                                    }
                                }
                            }
                            text.AppendLine(paragraph);
                        }
                    }

                    foreach (var el in GetDocumentElements(doc))
                    {
                        if (el.LocalName == "sectPr") continue;
                        if (el.HasChildren)
                        {
                            string paragraph = string.Empty;
                            foreach (var run in el.ChildElements)
                            {
                                if (run.HasChildren)
                                {
                                    foreach (var t in run.ChildElements)
                                    {
                                        if (t.LocalName == "t")
                                        {
                                            if (string.IsNullOrWhiteSpace(t.InnerText)) continue;
                                            paragraph += t.InnerText;
                                        }
                                        else
                                        {
                                            if (string.IsNullOrWhiteSpace(t.InnerText)) continue;
                                            paragraph += t.InnerText;
                                        }
                                    }
                                }

                            }

                            text.AppendLine(paragraph);
                        }
                    }

                    foreach (var footerPart in GetDocumentFooters(doc)) // code for extract cv details from footer section
                    {
                        foreach (var footer in footerPart.Footer)
                        {
                            string paragraph = string.Empty;
                            if (footer.HasChildren)
                            {
                                foreach (var t in footer.ChildElements)
                                {
                                    if (t.LocalName == "tr")
                                    {
                                        if (string.IsNullOrWhiteSpace(t.InnerText)) continue;
                                        paragraph += t.InnerText;
                                    }
                                    else
                                    {
                                        if (string.IsNullOrWhiteSpace(t.InnerText)) continue;
                                        paragraph += t.InnerText;
                                    }
                                }
                            }
                            text.AppendLine(paragraph);
                        }
                    }

                }
            }
            catch (Exception exception)
            {
                if (X.Logger is not null)
                    X.Logger.LogError(exception, CodeHelper.CallingMethodInfo(), null, new { Path = path });
            }

            var fileText = text.ToString();
            return fileText;
        }

        // https://stackoverflow.com/questions/77054239/replace-text-in-docx-document
        public static bool ReplaceTags(string templatePath, string destinationPath, Dictionary<string, object> tags)
        {
            string fileName = Path.GetFileName(templatePath);
            using WordprocessingDocument doc = WordprocessingDocument.Open(templatePath, true);
            ReplaceTagsInDocument(doc, tags, null);
            string documentPath = Path.Combine(destinationPath, fileName);
            doc.SaveAs(documentPath);
            return true;
        }
        public static void ReplaceTagsInDocument(WordprocessingDocument doc, Dictionary<string, object> tags, Func<Body, Paragraph, Run, string, string, bool>? handleTag)
        {
            var body = doc.MainDocumentPart?.Document.Body;
            if (body is null) return;
            var paraElems = body?.Elements<Paragraph>();

            if (paraElems is null)
                throw new ValidationException("Document doesn't contain any valid paragraphs.");

            foreach (var paraElem in paraElems)
            {
                string pText = paraElem.InnerText;
                string allText = string.Empty;
                bool hasPartialTag = false;
                Run? currentRun = null;
                Text? currentText = null;
                foreach (var runElem in paraElem.Elements<Run>())
                {
                    currentRun = runElem;
                    foreach (var textElem in runElem.Elements<Text>())
                    {
                        allText += textElem.Text;
                        textElem.Remove();
                    }

                    //if partial tag is detected then lets read next run to get full tag.
                    hasPartialTag = HasPartialTag(allText);
                    if (hasPartialTag)
                        continue;
                    //if we are here then we read full tag so setting hasPartialTag to false. 
                    hasPartialTag = false;
                    var matchedTags = GetTags(allText, tags);
                    var tagHandled = false;
                    foreach (var tag in matchedTags.Keys)
                    {
                        if (handleTag is not null)
                            tagHandled = handleTag(body, paraElem, runElem, allText, tag);
                        if (tagHandled == false)
                            allText = allText.Replace(tag, matchedTags[tag].ToString());
                    }
                    //only do this when tag is not handled by external method as external method will take care of it. 
                    if (tagHandled == false)
                    {
                        var newText = new Text()
                        {
                            Text = allText
                        };
                        runElem.Append(newText);
                    }
                    allText = string.Empty;
                }

                //in case there was not a tag and only partial tag was there so just add text without replace tag. 
                if (currentRun is not null && hasPartialTag && string.IsNullOrWhiteSpace(allText) == false)
                {
                    currentText = new Text()
                    {
                        Text = allText
                    };
                    currentRun.Append(currentText);
                }
            }
        }
        
        private static bool HasPartialTag(string allText)
        {
            if (allText.Contains(StudyDocumentTagConst.PrefixFirstPart))
            {
                if (allText.Contains(StudyDocumentTagConst.PrefixSecondPart) && allText.Contains(StudyDocumentTagConst.PostfixFirstPart))
                    return false;
                return true;
            }
            return false;
        }
        private static Dictionary<string, object> GetTags(string allText, Dictionary<string, object> tags)
        {
            var tagPrefix = StudyDocumentTagConst.GetPrefix();
            var tagPostfix = StudyDocumentTagConst.GetPostfix();

            var result = new Dictionary<string, object>();
            if (allText.Contains(tagPrefix) == false)
                return result;

            foreach (var tag in tags.Keys)
            {
                string placeholder = $"{tagPrefix}{tag}{tagPostfix}";
                if (allText.Contains(placeholder))
                    result.Add(placeholder, tags[tag]);
            }
            return result;
        }
    }
}
