namespace BaseLibrary.Utilities
{
    public class ListExtension
    {
        public static List<string> RemoveNullString(List<string?> values)
        {
            if(values == null)
                return [];
            List<string> cleanList = values.OfType<string>().ToList();
            return cleanList;
        }
    }
}
