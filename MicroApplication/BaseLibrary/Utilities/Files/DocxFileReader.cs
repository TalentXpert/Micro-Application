using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;

namespace BaseLibrary.Utilities.Files
{
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
                if(X.Logger is not null)
                    X.Logger.LogError(exception, CodeHelper.CallingMethodInfo(), null, new { Path = path });
            }

            var fileText = text.ToString();
            return fileText;
        }
    }
}
