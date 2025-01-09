using System.Collections.Generic;

namespace BaseLibrary.Utilities
{

    public class CityExtractor : InfoExtractorBase
    {
        static List<string> _cities;
        static string _cvText;
        public static string ExtractCity(string cvText,string fileName, List<string> inputCities)
        {
            if(inputCities == null || inputCities.Count==0 ) return string.Empty;
            if(string.IsNullOrWhiteSpace(cvText)) return string.Empty;
            _cities = inputCities;
            _cvText = cvText;

            var city = GetCityFromFileName(fileName);
            if (!string.IsNullOrWhiteSpace(city)) return city;

            city = GetCityByAddress();
            if (!string.IsNullOrWhiteSpace(city)) return city;

            city = GetCityByIndexAndCount();
            if (!string.IsNullOrWhiteSpace(city)) return city;

            return string.Empty;
        }

        private static string GetCityFromFileName(string fileName)
        {
            foreach (var city in _cities)
            {
                if (ContainWord(fileName,city)) return city;
            }
            return null;
        }

        private static string GetCityByIndexAndCount()
        {
            var indexCity = GetCityByIndex();
            var countCity = GetCityByCount();
            if (indexCity == countCity) return countCity;
            if(!string.IsNullOrWhiteSpace(countCity)) return countCity;
            return indexCity;
        }

        private static string GetCityByIndex()
        {
            string candiateCity=string.Empty;
            int cityIndex=100000;
            foreach(var city in _cities)
            {
                var index = _cvText.IndexOf(city,StringComparisonRule);
                if(index>0)
                {
                    if (index < cityIndex)
                    {
                        candiateCity = city;
                        cityIndex = index;
                    }
                }
            }
            return candiateCity;
        }

        private static string GetCityByCount()
        {
            int count = 0;
            string candiateCity = string.Empty;
            foreach (var city in _cities)
            {
                var cityCount = CountOfccurrences(_cvText, city);
                if (cityCount > count)
                {
                    count = cityCount;
                    candiateCity = city;
                }
            }
            return candiateCity;
        }

        private static string GetCityByAddress()
        {
            var keywords = new List<string> { "address" };
            foreach (var keyword in keywords)
            {
                var experienceLines = GetLinesHavingWord(_cvText, keyword);
                foreach(var line in experienceLines)
                { 
                    foreach(var city in _cities)
                    {
                        if (ContainWord(line,city))
                            return city;
                    }
                }
            }
            return string.Empty;
        }
       
    }
}
