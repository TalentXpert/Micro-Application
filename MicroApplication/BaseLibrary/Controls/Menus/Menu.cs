

namespace BaseLibrary.Controls.Menus
{
    public class Menu
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public bool IsFixed { get; set; }
        public bool IsLanding { get; set; }
        public List<Menu> Children { get; set; } = new List<Menu>();
        public Menu(Guid id, string name, string link, bool isFixed = true,bool isLanding = false)
        {
            Id = id;
            Name = name;
            Link = link;
            IsFixed = isFixed;
            IsLanding = isLanding;  
        }
    }
}
