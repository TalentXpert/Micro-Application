
namespace BaseLibrary.Controls.Grid
{
    public class SmartControlAlignment
    {
        public string Name { get; private set; }
        private SmartControlAlignment(string name)
        {
            Name = name;
        }

        public static SmartControlAlignment Left = new SmartControlAlignment("Left");
        public static SmartControlAlignment Right = new SmartControlAlignment("Right");
        public static SmartControlAlignment Center = new SmartControlAlignment("Center");
    }
}
