using BaseLibrary.Controls.Pages;
using BaseLibrary.UI.Controls;

namespace BaseLibrary.Configurations.PageHandlers
{
    public abstract class PageHandlerBase : CleanCode
    {
        public IBaseLibraryServiceFactory BaseLibraryServiceFactory { get; }
        public ApplicationUser LoggedInUser { get; }
        public AppPage AppPage { get; }
        public bool HasServerFilter { get; }
        public int PageSize { get; set; }
        public string DefaultSortDirection { get; set; }
        public string DefaultSortHeader { get; set; }
        public SmartPageState SmartPageState { get; set; }
        public GridModel Grid { get; private set; }
        public UIControlBaseFactory ApplicationControlFactory => BaseLibraryServiceFactory.ApplicationControlBaseFactory;
        public AppForm AppForm { get; }
        public PageHandlerBase(IBaseLibraryServiceFactory baseLibraryServiceFactory, ApplicationUser loggedInUser, AppPage appPage, AppForm appForm, bool hasServerFilter = false, string defaultSortDirection = "asc", string defaultSortHeader = "id")
        {
            BaseLibraryServiceFactory = baseLibraryServiceFactory;
            LoggedInUser = loggedInUser;
            HasServerFilter = hasServerFilter;
            AppPage = appPage;
            AppForm = appForm;
            SmartPageState = BaseLibraryServiceFactory.UserSettingService.GetSmartPageState(AppPage.Id, LoggedInUser);
            PageSize = SmartPageState.PageSize;
            DefaultSortDirection = defaultSortDirection;
            DefaultSortHeader = defaultSortHeader;
            Grid = new GridModel();
        }

        #region SmartPage - This section prepare smart page object with page actions, global control, currentgridfilterid and page size values
        public SmartPage GetSmartPage()
        {
            var pageActions = GetPageActions();
            var globalControls = GetGlobalControls();
            var smartPage = new SmartPage(AppPage, pageActions, globalControls, SmartPageState.CurrentGridFilterId, SmartPageState.PageSize);
            return smartPage;
        }
        /// <summary>
        /// This method should return page actions displayed on page main menu after - data/column configuration menu item.
        /// </summary>
        /// <returns></returns>
        public abstract List<SmartAction> GetPageActions();
        public abstract List<string> GetIgnoreExcelHeaders();
        /// <summary>
        /// This method should return global controls those should be displayed on top of grid or card control.
        /// </summary>
        /// <returns></returns>
        public List<UIControl> GetGlobalControls()
        {
           

            List<UIControl> controls = BaseLibraryServiceFactory.AppFormControlService.GetGlobalControls(LoggedInUser.OrganizationId, AppForm, SmartPageState.GlobalControls, ApplicationControlFactory);

            

            return controls;

        }

        private List<AppFormControl> ReorderParentChild(List<AppFormControl> globalControls)
        {
            List<AppFormControl> result = new List<AppFormControl>();
            if (HasNoChild(globalControls))
                return result;

            AppFormControl? parent = globalControls.FirstOrDefault(c => string.IsNullOrWhiteSpace(c.AppControl.ParentControlIdentifier));
            if (IsNull(parent))
                return result;

            result.Add(parent);
            var child = globalControls.FirstOrDefault(c => c.AppControl.ParentControlIdentifier == parent.AppControl.ControlIdentifier);
            while (IsNotNull(child))
            {
                result.Add(child);
                parent = child;
                child = globalControls.FirstOrDefault(c => c.AppControl.ParentControlIdentifier == parent.AppControl.ControlIdentifier);
            }

            return result;
        }

        #endregion

        #region Smart Grid - This section prepare Smart Grid object with filter controls and user saved filters for that page or global control value.
        public virtual SmartGrid GetSmartGridConfiguration(SmartGridConfigurationVM vm)
        {
            var filters = GetFilters(vm);
            var savedFilters = BaseLibraryServiceFactory.UserSettingService.GetFilters(LoggedInUser.Id, vm.PageId);

            if (vm.GlobalControlValues != null && vm.GlobalControlValues.Any())
            {
                foreach (var g in vm.GlobalControlValues)
                {
                    savedFilters = savedFilters.Where(f => f.Filters.Any(fa => fa.ControlIdentifier == g.ControlIdentifier && fa.Value == g.GetFirstValue())).ToList();
                }
            }
            var smartGrid = new SmartGrid(vm.PageId, savedFilters, filters);
            return smartGrid;
        }

        /// <summary>
        /// This method should return filter controls based on page id and global control id.
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public abstract List<UIControl> GetFilters(SmartGridConfigurationVM vm);
        #endregion

        public GridModel ProcessPageDataFilterRequest(Guid filterId, int pageSize)
        {
            var filter = BaseLibraryServiceFactory.UserSettingService.GetFilter(filterId);
            var request = new GridRequestVM
            {
                PageId = filter.PageId,
                PageNumber = 1,
                PageSize = pageSize,
                Filters = filter.Filters,
                SortDirection = DefaultSortDirection,
                SortHeaderText = DefaultSortHeader,
            };
            var gridData = ProcessPageDataRequest(request);
            return gridData;
        }

        public GridModel ProcessPageDataRequest(GridRequestVM model)
        {
            if (model.FilterId.IsNullOrEmpty() == false)
            {
                var filter = BaseLibraryServiceFactory.UserSettingService.GetFilter(model.FilterId.Value);
                model = new GridRequestVM
                {
                    PageId = filter.PageId,
                    PageNumber = 1,
                    PageSize = model.PageSize,
                    Filters = filter.Filters,
                    SortDirection = DefaultSortDirection,
                    SortHeaderText = DefaultSortHeader,
                };
            }
            Grid.Headers = GetGridHeaders(model.GetSmartGridConfigurationVM(), false);
            Grid.Rows = GetRows(model, out int totalRows);
            var totalPages = totalRows / model.PageSize + 1;
            Grid.PagingInfo = new GridPagingInfo { PageNumber = model.PageNumber, TotalPages = totalPages, PageSize = model.PageSize, TotalRows = Grid.Rows.Count };
            return Grid;
        }

        public abstract List<List<GridCell>> GetRows(GridRequestVM model, out int totalPages);

        public GridCell PrepareDynamicCell(Dictionary<string, string> data, string identifier)
        {
            if (data.ContainsKey(identifier))
            {
                var cell = new GridCell { T = data[identifier] };
                return cell;
            }
            return new GridCell();
        }

        protected bool CheckIfFiltered(GridRequestVM model, FormStoreBase data)
        {
            var filters = model.Filters;
            if (filters.Any())
            {
                foreach (var filter in filters)
                {
                    var filterValue = filter.Value;
                    if (string.IsNullOrWhiteSpace(filterValue)) continue;
                    var controlValue = data.GetValue(filter.ControlIdentifier)?.ToString();
                    if (string.IsNullOrWhiteSpace(controlValue)) return true;
                    return ShouldFilter(filterValue, filter.Operator, controlValue);
                }
            }
            return false;
        }

        protected bool ShouldFilter(string filterValue, string op, string value)
        {
            if (string.IsNullOrWhiteSpace(filterValue)) return false;
            if (AreEqualsIgnoreCase(filterValue, "missing") && string.IsNullOrWhiteSpace(value)) return false;
            switch (op)
            {
                case "=":
                    if (filterValue != value)
                        return true;
                    break;
                case "*":
                    if (value.StartsWith(filterValue) == false)
                        return true;
                    break;
                case "%":
                    if (value.Contains(filterValue) == false)
                        return true;
                    break;
                case "$":
                    if (value.EndsWith(filterValue) == false)
                        return true;
                    break;
            }
            var t = decimal.TryParse(filterValue, out var filterDecimalValue);
            var c = decimal.TryParse(value, out var controlDecimalValue);
            if (t && c)
            {
                switch (op)
                {
                    case ">":
                        if (controlDecimalValue <= filterDecimalValue)
                            return true;
                        break;
                    case "<":
                        if (controlDecimalValue >= filterDecimalValue)
                            return true;
                        break;
                    case ">=":
                        if (controlDecimalValue < filterDecimalValue)
                            return true;
                        break;
                    case "<=":
                        if (controlDecimalValue > filterDecimalValue)
                            return true;
                        break;
                }
            }
            return false;
        }
        #region GridHeader
        public abstract List<GridHeader> GetGridHeaders(SmartGridConfigurationVM vm, bool includeExcludedHeaders);

        public List<GridHeader> GetGridHeaders(Guid? organizationId, Guid formId, Guid? layoutControlId, bool includeExcludedHeaders, List<AppControl> skipControls)
        {
            if (IsNull(skipControls)) skipControls = new List<AppControl>();
            var form = BaseLibraryServiceFactory.AppFormService.GetForm(formId);
            var formControls = BaseLibraryServiceFactory.AppFormControlService.GetFormControlsWithControl(form.Id, layoutControlId, LoggedInUser.OrganizationId);
            var smartForm = new UIForm(form);
            formControls = formControls.OrderBy(control => control.Position).ToList();
            foreach (var formControl in formControls)
            {
                if (skipControls.Any(c => c.Id == formControl.AppControl.Id)) continue;
                if (formControl.GetIsGlobalControl()) continue;
                smartForm.AddControl(ApplicationControlFactory.GetUIControl(organizationId, formControl.AppControl, formControl, null, null, false));
            }

            var headers = new List<GridHeader>();
            int i = 1;
            headers.Add(new GridHeader(i++, "Id", "Id", ControlDataType.Guid, SmartControlAlignment.Left, false).Hide());

            foreach (var ctrl in smartForm.UIControls)
            {
                headers.Add(new GridHeader(i++, ctrl.ControlIdentifier, ctrl.DisplayLabel, ControlDataType.GetDataType(ctrl.DataType), ControlDataType.GetAlignment(ctrl.DataType), ctrl.IsSingleLine));
            }

            var actions = PrepareColumnActions(form);

            if (headers.Count > 1)
                headers[1].AddActions(actions);
            if (IsFalse(includeExcludedHeaders))
                headers = ConfigureUserHeaders(headers);

            return headers;
        }

        protected virtual List<SmartAction> PrepareColumnActions(AppForm form)
        {
            var actions = new List<SmartAction>
            {
                new SmartAction(form,FormTypes.DynamicForm, "View","View","View", SmartActionFormMode.View),
                new SmartAction(form,FormTypes.DynamicForm,"Edit","Edit","Edit", SmartActionFormMode.Edit),
                new SmartAction(form,FormTypes.DynamicForm,"Delete","Delete","Delete", SmartActionFormMode.Delete),
                //new SmartAction(BaseForm.UserRoleForm,FormTypes.SelectFromListForm, "Manage Roles","ManageRoles","ManageRoles",SmartActionFormMode.Add),
            };
            return actions;
        }

        /// <summary>
        /// This method will configure header based on user configured headers.
        /// </summary>
        private List<GridHeader> ConfigureUserHeaders(List<GridHeader> headers)
        {
            List<GridHeader> userConfiguredHeaders = new List<GridHeader>();
            var userHeaders = BaseLibraryServiceFactory.UserSettingService.GetUserConfiguredGridHeaders(LoggedInUser.Id, AppPage.Id);

            if (HasNoChild(userHeaders))
                return headers;

            userHeaders.Insert(0, new UserGridHeader { HeaderIdentifier = "Id", Position = 1 });
            foreach (var userHeader in userHeaders)
            {
                var header = headers.FirstOrDefault(h => h.HeaderIdentifier == userHeader.HeaderIdentifier);
                if (IsNotNull(header))
                {
                    header.SetPosition(userHeader.Position);
                    userConfiguredHeaders.Add(header);
                }
            }
            return userConfiguredHeaders;
        }

        public GridModel ProcessRowDataRequest(Guid datakey)
        {            
            Grid.Headers = GetGridHeaders(null, false);
            Grid.Rows = GetRow(datakey);           
            return Grid;
        }
        public abstract List<List<GridCell>> GetRow(Guid datakey);
        #endregion
    }
}
