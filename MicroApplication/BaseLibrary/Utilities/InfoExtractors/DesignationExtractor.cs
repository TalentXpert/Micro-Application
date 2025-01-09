using System;
using System.Collections.Generic;

namespace BaseLibrary.Utilities
{
    public class DesignationExtractor : InfoExtractorBase
    {
        static List<string> designations;
        public static string ExtractDesignation(string cvText, List<string> inputDesignations)
        {
            if (inputDesignations == null || inputDesignations.Count == 0) return string.Empty;
            designations = inputDesignations;


            var designation = GetDesignationByIndex(cvText);
            if (!string.IsNullOrWhiteSpace(designation)) return designation;

            return string.Empty;
        }

        private static string GetDesignationByIndex(string cvText)
        {
            string candiateCity = string.Empty;
            int deginationIndex = 100000;
            foreach (var designation in designations)
            {
                var index = cvText.IndexOf(designation);
                if (index > 0)
                {
                    if (index < deginationIndex)
                    {
                        candiateCity = designation;
                        deginationIndex = index;
                    }
                }
            }
            return candiateCity;
        }
       
    }
}
