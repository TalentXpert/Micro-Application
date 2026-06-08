namespace BaseLibrary.Utilities
{

    public class NextListItem<T>
    {
        readonly List<T> ItemList;
        int index = 0;
        public NextListItem(List<T> list)
        {
            ItemList = list;
        }
        public T? GetNext()
        {
            if (index < ItemList.Count)
            {
                var item = ItemList[index];
                index += 1;
                return item;
            }
            return default(T);
        }
    }
}
