using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace BaseLibrary.Utilities.Files
{
    public class DocxTagReplacer
    {
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

            var paragraphs = body?.Elements<Paragraph>();

            if (paragraphs is null)
                throw new ValidationException("Document doesn't contain any valid paragraphs.");

            foreach (var paragraph in paragraphs)
            {
                string pText = paragraph.InnerText;
                string allText = string.Empty;
                bool hasPartialTag = false;
                Run? currentRun = null;
               // Text? currentText = null;
                Text? lastText = null;
                var textList = new List<Text>();
                
                foreach (var runElem in paragraph.Elements<Run>())
                {
                    //RunProperties rPr = runElem.RunProperties;
                    //var font = rPr.RunFonts;
                    var hasTag = false;
                    
                    currentRun = runElem;

                    foreach (var textElem in runElem.Elements<Text>())
                    {
                        allText += textElem.Text;
                        textList.Add(textElem);
                        lastText= textElem;
                        //textElem.Remove();
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
                        hasTag = true;
                        if (handleTag is not null)
                            tagHandled = handleTag(body, paragraph, runElem, allText, tag);
                        if(tagHandled)
                            RemoveTexts(textList, null);
                        if (tagHandled == false)
                            allText = allText.Replace(tag, matchedTags[tag].ToString());
                    }
                    //only do this when tag is not handled by external method as external method will take care of it. 
                    if (hasTag && tagHandled == false)
                    {
                        if (lastText is not null)
                        {
                            RemoveTexts(textList, lastText);
                            lastText.Text = allText;
                        }
                        else
                        {
                            var newText = new Text()
                            {
                                Text = allText
                            };
                            runElem.Append(newText);
                        }
                    }
                    allText = string.Empty;
                    textList.Clear();
                }

                //in case there was not a tag and only partial tag was there so just add text without replace tag. 
                //if (currentRun is not null && hasPartialTag && string.IsNullOrWhiteSpace(allText) == false)
                //{
                //    currentText = new Text()
                //    {
                //        Text = allText
                //    };
                //    currentRun.Append(currentText);
                //}
            }
        }

        private static void RemoveTexts(List<Text> textList, Text? lastText)
        {
            foreach(var  text in textList)
            {
                if (lastText is not null && text.Equals(lastText)) continue;
                text.Remove();
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
