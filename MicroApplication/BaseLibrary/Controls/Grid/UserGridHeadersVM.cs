
namespace BaseLibrary.Controls.Grid
{
    /// <summary>
    /// This class save user configured headers into user application setting table
    /// </summary>
    public class UserGridHeader
    {
        public string HeaderIdentifier { get; set; }
        public int Position { get; set; }
    }

    /// <summary>
    /// This class used to show user headers on header setting dialog
    /// </summary>
    public class UserGridHeadersVM
    {
        public Guid PageId { get; set; }
        public List<UserGridHeaderVM> Headers { get; set; }
        public UserGridHeadersVM() { Headers = new List<UserGridHeaderVM>(); }
    }

    public class UserGridHeaderVM
    {
        public string HeaderIdentifier { get; set; }
        public int Position { get; set; }
        public bool IsVisible { get; set; }
        public bool IsFixed { get; set; } // a column which has action so it is fixed and can not be removed.
        public string HeaderText { get; set; }
        public UserGridHeaderVM() { }
        public UserGridHeaderVM(string headerIdentifier, int position, string headerText, bool isVisible, bool isFixed)
        {
            HeaderIdentifier = headerIdentifier;
            Position = position;
            IsVisible = isVisible;
            IsFixed = isFixed;
            HeaderText = headerText;
        }
    }

   
}
