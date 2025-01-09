namespace BaseLibrary.Configurations.DataSources.LinqDataSources
{
    /// <summary>
    /// This class and its children represent single entry of a form and its related data on other form. FromControl value resolver will use this to return a form control value. A Linq query will fill this object with related and relevant data
    /// </summary>
    public class BaseLinqFormDataSource
    {
        public FormDataStore? City { get; set; }
        public FormDataStore? State { get; set; }
        public FormDataStore? Country { get; set; }
        public AppControl? Control { get; set; }
        public ApplicationUser? User { get; set; }
        public FormDataStore? DataStore { get; set; }
        public UserRole? UserRole { get; set; }
        public AppPage? AppPage { get; set; }
        public ApplicationUserSetting? ApplicationUserSetting { get; set; }
        public ApplicationSetting? ApplicationSetting { get; set; }
        public ApplicationRolePermission? ApplicationRolePermission { get; set; }
        public ApplicationRole? ApplicationRole { get; set; }
    }
}


/*
 * 
 * public List<FormControlValueDataSource> GetCityData()
        {
            var query = from ct in unitOfWork.PageDataStore
                        join s in unitOfWork.PageDataStore on ct.ParentId equals s.Id
                        join c in unitOfWork.PageDataStore on s.ParentId equals c.Id
                        select new FormControlValueDataSource { City = ct, State = s, Country = c };
            return query.ToList();
        }
 * 
 */