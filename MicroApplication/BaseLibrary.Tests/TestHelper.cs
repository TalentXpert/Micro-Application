using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BaseLibrary.Tests
{
    public class TestHelper
    {

        public static bool CompareObjectAttributes(object firstObject, object secondObject)
        {
            Type t1 = firstObject.GetType();
            Type t2 = secondObject.GetType();
            /*the two objects must have the same type*/
            if (!t1.Equals(t2)) return false;

            PropertyInfo[] infos1 = t1.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            PropertyInfo[] infos2 = t2.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            for (int i = 0; i < infos1.Length; i++)
            {
                /*this if is needed because if it is a datetime it should compare only the date and the time and not the ticks*/
                if (infos1[i].PropertyType.Equals(typeof(DateTime)))
                {
                    DateTime firstDate = (DateTime)infos1[i].GetValue(firstObject, null);
                    DateTime secondDate = (DateTime)infos2[i].GetValue(secondObject, null);
                    /*if the datatype in the database was date then only compare the date part of the datetime object*/
                    if (firstDate.ToString().Contains("00:00:00") || secondDate.ToString().Contains("00:00:00"))
                    {
                        if (!firstDate.Date.ToString().Equals(secondDate.Date.ToString())) return false;
                    }
                    /*otherwise compare the string representation of the two datetime objects because the ticks may differ*/
                    else
                    {
                        if (!firstDate.ToString().Equals(secondDate.ToString()))
                            return false;
                    }
                }
                else
                {
                    /*if one property value differs return false*/
                    if (!(infos1[i].GetValue(firstObject, null)).Equals(infos2[i].GetValue(secondObject, null)))
                    {
                        var value1 = infos1[i].GetValue(firstObject, null);
                        var value2 = infos2[i].GetValue(secondObject, null);
                        return false;
                    }
                }
            }

            /*when everything went fine the objects have the same values for their properties*/
            return true;
        }

        public static string CheckIfAllPropertiesAreEqual(object firstObject, object secondObject, List<string> skipFields = null)
        {
            var resultList = new List<string>();
            Type t1 = firstObject.GetType();
            Type t2 = secondObject.GetType();
            /*the two objects must have the same type*/
            if (!t1.Equals(t2)) throw new ValidationException("Both objects are of not same type");

            PropertyInfo[] infos1 = t1.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            PropertyInfo[] infos2 = t2.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            int i;
            try
            {
                for (i = 0; i < infos1.Length; i++)
                {
                    /*this if is needed because if it is a datetime it should compare only the date and the time and not the ticks*/
                    if (infos1[i].PropertyType.Equals(typeof(DateTime)))
                    {
                        DateTime firstDate = (DateTime)infos1[i].GetValue(firstObject, null);
                        DateTime secondDate = (DateTime)infos2[i].GetValue(secondObject, null);
                        /*if the datatype in the database was date then only compare the date part of the datetime object*/
                        if (firstDate.ToString().Contains("00:00:00") || secondDate.ToString().Contains("00:00:00"))
                        {
                            if (!firstDate.Date.ToString().Equals(secondDate.Date.ToString())) resultList.Add(infos1[i].Name);
                        }
                        /*otherwise compare the string representation of the two datetime objects because the ticks may differ*/
                        else
                        {
                            if (!firstDate.ToString().Equals(secondDate.ToString()))
                                resultList.Add(infos1[i].Name);
                        }
                    }
                    else
                    {
                        /*if one property value differs return false*/
                        var value1 = infos1[i].GetValue(firstObject, null);
                        var value2 = infos2[i].GetValue(secondObject, null);
                        if (value1 == null)
                        {
                            if (value2 != null)
                            {
                                resultList.Add(infos1[i].Name);
                            }
                        }
                        else if (!(infos1[i].GetValue(firstObject, null)).Equals(infos2[i].GetValue(secondObject, null)))
                        {
                            resultList.Add(infos1[i].Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (skipFields != null && skipFields.Count > 0)
            {
                foreach (var skipField in skipFields)
                    resultList.Remove(skipField);
            }
            return string.Join(",", resultList);
        }

        public static string CheckIfGivenPropertiesAreEqual(object firstObject, object secondObject, List<string> fieldsToCheck)
        {
            var resultList = new List<string>();
            Type t1 = firstObject.GetType();
            Type t2 = secondObject.GetType();
            /*the two objects must have the same type*/
            if (!t1.Equals(t2)) throw new ValidationException("Both objects are of not same type");

            PropertyInfo[] infos1 = t1.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            PropertyInfo[] infos2 = t2.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            int i;
            try
            {
                for (i = 0; i < infos1.Length; i++)
                {
                    if (!fieldsToCheck.Contains(infos1[i].Name)) continue;
                    /*this if is needed because if it is a datetime it should compare only the date and the time and not the ticks*/
                    if (infos1[i].PropertyType.Equals(typeof(DateTime)))
                    {
                        DateTime firstDate = (DateTime)infos1[i].GetValue(firstObject, null);
                        DateTime secondDate = (DateTime)infos2[i].GetValue(secondObject, null);
                        /*if the datatype in the database was date then only compare the date part of the datetime object*/
                        if (firstDate.ToString().Contains("00:00:00") || secondDate.ToString().Contains("00:00:00"))
                        {
                            if (!firstDate.Date.ToString().Equals(secondDate.Date.ToString())) resultList.Add(infos1[i].Name);
                        }
                        /*otherwise compare the string representation of the two datetime objects because the ticks may differ*/
                        else
                        {
                            if (!firstDate.ToString().Equals(secondDate.ToString()))
                                resultList.Add(infos1[i].Name);
                        }
                    }
                    else
                    {
                        /*if one property value differs return false*/
                        var value1 = infos1[i].GetValue(firstObject, null);
                        var value2 = infos2[i].GetValue(secondObject, null);
                        if (value1 == null)
                        {
                            if (value2 != null)
                            {
                                resultList.Add(infos1[i].Name);
                            }
                        }
                        else if (!(infos1[i].GetValue(firstObject, null)).Equals(infos2[i].GetValue(secondObject, null)))
                        {
                            resultList.Add(infos1[i].Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return string.Join(",", resultList);
        }
        public static string CheckIfAllPropertiesAreEqualOfDifferentObjects(object firstObject, object secondObject, List<string> skipFields = null)
        {
            var resultList = new List<string>();
            Type t1 = firstObject.GetType();
            Type t2 = secondObject.GetType();
            /*the two objects must have the same type*/
            //if (!t1.Equals(t2)) throw new ValidationException("Both objects are of not same type");

            PropertyInfo[] infos1 = t1.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            PropertyInfo[] infos2 = t2.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            int i;

            try
            {
                var propertiesCount = infos1.Length;
                var largerObject = t1;
                PropertyInfo[] largerList = infos1;
                if (propertiesCount < infos2.Length)
                {
                    propertiesCount = infos2.Length;
                    largerObject = t2;
                    largerList = infos2;
                }
                string propertyName;
                for (i = 0; i < propertiesCount; i++)
                {
                    /*this if is needed because if it is a datetime it should compare only the date and the time and not the ticks*/
                    propertyName = largerList[i].Name;
                    if (largerList[i].PropertyType.Equals(typeof(DateTime)))
                    {
                        var value1 = GetValueOfTypeDate(t1, firstObject, largerList[i].Name);
                        var value2 = GetValueOfTypeDate(t2, secondObject, largerList[i].Name);

                        //DateTime firstDate =  (DateTime)infos1[i].GetValue(firstObject, null);
                        //DateTime secondDate = (DateTime)infos2[i].GetValue(secondObject, null);

                        DateTime firstDate = (DateTime)value1;
                        DateTime secondDate = (DateTime)value2;
                        /*if the datatype in the database was date then only compare the date part of the datetime object*/
                        if (firstDate.ToString().Contains("00:00:00") || secondDate.ToString().Contains("00:00:00"))
                        {
                            if (!firstDate.Date.ToString().Equals(secondDate.Date.ToString())) resultList.Add(infos1[i].Name);
                        }
                        /*otherwise compare the string representation of the two datetime objects because the ticks may differ*/
                        else
                        {
                            if (!firstDate.ToString().Equals(secondDate.ToString()))
                                resultList.Add(infos1[i].Name);
                        }
                    }
                    else
                    {
                        /*if one property value differs return false*/
                        var value1 = GetValueOf(t1, firstObject, largerList[i].Name);
                        var value2 = GetValueOf(t2, secondObject, largerList[i].Name);

                        //var value1 = infos1[i].GetValue(firstObject, null);
                        //var value2 = infos2[i].GetValue(secondObject, null);
                        if (value1 == null)
                        {
                            if (value2 != null)
                            {
                                resultList.Add(propertyName);
                            }
                        }
                        else if (!value1.Equals(value2))
                        {
                            resultList.Add(propertyName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (skipFields != null && skipFields.Count > 0)
            {
                foreach (var skipField in skipFields)
                    resultList.Remove(skipField);
            }
            return string.Join(",", resultList);
        }

        private static object GetValueOf(Type t, object firstObject, string name)
        {
            var prop = t.GetProperty(name);
            if (prop != null)
                return prop.GetValue(firstObject, null);
            return null;
        }

        private static object GetValueOfTypeDate(Type t, object firstObject, string name)
        {
            var prop = t.GetProperty(name);
            if (prop != null)
                return prop.GetValue(firstObject, null);
            return DateTime.Now.AddDays(-1000);
        }
    }
}
