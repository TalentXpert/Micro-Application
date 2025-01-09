namespace BaseLibrary.Configurations.DataSources.LinqDataSources
{
    /// <summary>
    /// This represent data source having collection of related forms that provide list of form controls to be added on a datagrid or listview control.
    /// </summary>
    public class LinqDataSource
    {
        public List<AppForm> Forms { get; private set; }
        public string Name { get; set; }
        public AppForm MasterForm { get; private set; }
        public LinqDataSource(string name, AppForm masterForm)
        {
            Forms = new List<AppForm>();
            Name = name;
            MasterForm = masterForm;
        }
    }

    /// <summary>
    /// This class hold data related to a user 
    /// </summary>
    public class UserDataSource : LinqDataSource
    {
        public UserDataSource() : base("User Data", BaseForm.UserManagement)
        {

        }
    }
}
