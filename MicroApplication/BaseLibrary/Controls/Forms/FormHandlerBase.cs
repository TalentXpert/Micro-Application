
namespace BaseLibrary.Controls.Forms
{
    /// <summary>
    /// This class is base class for each class that implement form handler logic
    /// </summary>
    public abstract class FormHandlerBase : CleanCode
    {
        public IBaseLibraryServiceFactory BaseLibraryServiceFactory { get; }
        public ApplicationUser LoggedInUser { get; }
        public AppForm Form { get; }
        /// <summary>
        /// This property should be true for form having it own table and storing data into it instead of default data store.
        /// </summary>
        public bool HasPrivateDataStore { get; }
        /// <summary>
        /// ParentControl should be set to control that hold direct parent data for this form like country for state
        /// </summary>
        public AppControl? ParentControl { get; }
        /// <summary>
        /// This property should be set tp a global control that change form layout like asset subgoup 
        /// </summary>
        public AppControl? LayoutControl { get; }
        public string Language { get; }

        public FormHandlerBase(IBaseLibraryServiceFactory baseLibraryServiceFactory, ApplicationUser loggedInUser, AppForm form, AppControl? parentControl, AppControl? layoutControl, bool hasPrivateDataStore)
        {
            BaseLibraryServiceFactory = baseLibraryServiceFactory;
            LoggedInUser = loggedInUser;
            Form = form;
            ParentControl = parentControl;
            HasPrivateDataStore = hasPrivateDataStore;
            LayoutControl = layoutControl;
            Language = "en";
        }

        #region Save Form
        /// <summary>
        /// This method process form save request and return saved data key. This method should be overriden for form having its private data store.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual string ProcessFormSaveRequest(SmartFormTemplateRequest model)
        {
            return BaseLibraryServiceFactory.PageDataStoreService.SaveFormDatum(model, ValidateAndParentId(model), LoggedInUser);
        }
        public virtual void PostProcessFormSaveRequest(object entitySaved)
        {
        }
        public virtual string ProcessFormSaveRequestAndReturnDataKey(SmartFormTemplateRequest model)
        {
            return BaseLibraryServiceFactory.PageDataStoreService.SaveFormDatum(model, ValidateAndParentId(model), LoggedInUser);
        }

        private Guid? ValidateAndParentId(SmartFormTemplateRequest model)
        {
            if (HasPrivateDataStore)
            {
                throw new ValidationException("This method can not be called for form having its private data store. Please imlement ProcessFormSaveRequest method into form handler class for private data store.");
            }
            return  GetParentId(model.ControlValues);
        }

        public virtual void ProcessFormConsentRequest(SmartFormConsentTemplateRequest model)
        {           
        }

        protected virtual Guid? GetParentId(List<ControlValue> controlValues)
        {
            if (IsNull(ParentControl))
                return null;

            var textId = controlValues.GetControlFirstValue(ParentControl);
            if (IsNullOrEmpty(textId))
                throw new ValidationException($"{ParentControl.DisplayLabel} id is not a valid Guid value.");

            if (Guid.TryParse(textId, out var id))
                return id;

            throw new ValidationException($"{ParentControl.DisplayLabel} id {id} is not a valid guid value.");
        }
        #endregion

        #region Form Template 
        public UIForm ProcessFormGenerateRequest(SmartFormGenerateRequest model, UIControlBaseFactory applicationControlFactory)
        {
            var form = BaseLibraryServiceFactory.AppFormService.GetForm(Form.Id);
            if (IsNull(form))
                throw new ValidationException($"No form found for given form id {Form.Id}");

            Guid? layoutControlValue = null;
            if (IsNotNull(LayoutControl))
            {
                layoutControlValue = model.GlobalControls.GetControlFirstValueAsGuid(LayoutControl);
                if (IsNull(layoutControlValue))
                    throw new ValidationException($"Layout control have can not be empty as this form need a layout control value. ");
            }

            var formControls = BaseLibraryServiceFactory.AppFormControlService.GetFormControlsWithControl(Form.Id, layoutControlValue, LoggedInUser?.OrganizationId);
            var controlValues = GetFormControlValues(model);

            if (HasNoChild(controlValues) && IsNullOrEmpty(model.DataKey)) //this is add data request so put global control value.
                controlValues = model.GlobalControls;

            var smartForm = new UIForm(form);
            smartForm.DataKey = model.DataKey;
            foreach (var formControl in formControls)
            {
                AppControl appcontrol = formControl.AppControl;
                var parentId = model.GlobalControls.GetControlFirstValueAsGuid(appcontrol.ParentControlIdentifier);
                if (appcontrol.IsComplexControl())
                {
                    smartForm.AddControl(applicationControlFactory.GetComplexUIControl(appcontrol, formControl, model.DataKey.Value));
                }
                else
                {
                    
                    smartForm.AddControl(applicationControlFactory.GetUIControl(LoggedInUser, appcontrol, formControl, formControl.GetFormControlValue(controlValues), parentId, true));
                }
            }
            return smartForm;
        }
        #endregion

        public virtual byte[] GetImportTemplateRequest(ExcelImportTmplateRequest model, UIControlBaseFactory applicationControlFactory)
        {
            var form = BaseLibraryServiceFactory.AppFormService.GetForm(Form.Id);
            if (IsNull(form))
                throw new ValidationException($"No form found for given form id {Form.Id}");

            Guid? layoutControlId = model.GlobalControls.GetControlFirstValueAsGuid(LayoutControl?.ControlIdentifier);
            var formControls = BaseLibraryServiceFactory.AppFormControlService.GetFormControlsWithControl(Form.Id, layoutControlId, LoggedInUser.OrganizationId);

            var headers = GetExcelHeaders(formControls, model.GlobalControls, applicationControlFactory);
            return new ExcelImportTemplate(WordTranslator.Translate(form.Name, Language), headers).CreateImportTemplate(model.Rows);
        }

        protected List<ImportExcelHeader> GetExcelHeaders(List<AppFormControl> formControls, List<ControlValue> globalControls, UIControlBaseFactory applicationControlFactory)
        {
            var headers = new List<ImportExcelHeader>();

            ImportExcelHeader header = null;

            foreach (var formControl in formControls)
            {
                var appcontrol = formControl.AppControl;
                var controlValue = globalControls.GetControlFirstValueAsGuid(appcontrol.ControlIdentifier);
                var parentId = globalControls.GetControlFirstValueAsGuid(appcontrol.ParentControlIdentifier);
                var control = applicationControlFactory.GetUIControl(LoggedInUser, appcontrol, formControl, null, parentId, false);
                header = new ImportExcelHeader(formControl.GetDisplayLabel(), formControl.IsMandatory);
                if (ControlTypes.IsOptionNeeded(control) && HasChildren(control.Options))
                {
                    if (header != null)
                        header.Lookups.AddRange(control.Options.Select(d => d.Label).ToList());
                }
                if (appcontrol.IsBoolean())
                {
                    if (header != null)
                        header.Lookups.AddRange(new List<string> { "", WordTranslator.Translate("Yes", Language), WordTranslator.Translate("No", Language) });
                }
                if (controlValue != null)
                {
                    var option = control.Options.FirstOrDefault(c => c.Value == controlValue?.ToString());
                    if (option != null)
                        header.AddDefaultData(option.Label);
                }

                headers.Add(header);
            }
            return headers;
        }


        /// <summary>
        /// This method return form values save in database table.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected virtual List<ControlValue> GetFormControlValues(SmartFormGenerateRequest model)
        {
            List<ControlValue> controlValues = new List<ControlValue>();
            var id = model.DataKey;
            if (id.IsNullOrEmpty())
                return controlValues;

            Guid? layoutControlId = model.GlobalControls.GetControlFirstValueAsGuid(LayoutControl);
            var formControls = BaseLibraryServiceFactory.AppFormControlService.GetFormControlsWithControl(Form.Id, layoutControlId, LoggedInUser?.OrganizationId).OrderBy(c => c.Position);

            var pageData = GetStoreObject(id.Value);
            if (IsNotNull(pageData))
            {
                foreach (var formControl in formControls)
                {
                    var appControl = formControl.AppControl;
                    controlValues.Add(new ControlValue(appControl.Id, appControl.ControlIdentifier, pageData.GetValue(formControl.AppControl.ControlIdentifier)));
                }
            }
            return controlValues;
        }

        public virtual FormStoreBase? GetStoreObject(Guid id)
        {
            if (HasPrivateDataStore)
            {
                throw new ValidationException("This method can not be called for form having its private data store. Please imlement GetStoreObject method into form handler class for private data store.");
            }
            FormDataStore? pageData = BaseLibraryServiceFactory.PageDataStoreService.GetFormDatum(id);
            return pageData;
        }
        public virtual string? GetTextFromId(string controlIdentifier, Guid id)
        {
            var data = GetStoreObject(id);
            if (data == null)
                throw new ValidationException($"{controlIdentifier} is not stored in base data store. It has its private store so add this option in a class drived from UIControlBaseFactory in your project.");
            return data.GetValue(controlIdentifier);
        }
        public virtual void DeleteData(Guid id)
        {
            if (HasPrivateDataStore)
            {
                throw new ValidationException("This method can not be called for form having its private data store. Please imlement DeleteData method into form handler class for private data store.");
            }
            BaseLibraryServiceFactory.PageDataStoreService.DeleteData(id, LoggedInUser);
        }
        public virtual string PageAction(Guid formId, Guid id, string param, string actionIdentifier)
        {
            return "This action is not implemented for this page. please implement it.";
        }
        public virtual List<SmartControlOption> GetControlOptions(Guid? organizationId, Guid? parentId, string searchTerm)
        {
            if (HasPrivateDataStore)
            {
                throw new ValidationException("This method can not be called for form having its private data store. Please imlement GetControlOptions method into form handler class for private data store.");
            }
            var options = BaseLibraryServiceFactory.PageDataStoreService.GetSearchResult(organizationId, Form.Id, parentId, searchTerm);
            return options;
        }
        public virtual PageContentView GetPageContentView(SmartFormGenerateRequest model)
        {
            var factory = BaseLibraryServiceFactory.ApplicationControlBaseFactory;
            //if (HasPrivateDataStore)
            //{
            //    throw new ValidationException("This method can not be called for form having its private data store. Please imlement GetPageContentView method into form handler class for private data store.");
            //}

            if (model.DataKey.IsNullOrEmpty())
                throw new ValidationException("Form DataKey can not be null or empty.");

            //PageDataStore pageData = BaseLibraryServiceFactory.PageDataStoreService.GetPageDatum(model.DataKey.Value);
            var view = new PageContentView();
            PagePanelContentView? columnView = null;

            //if (IsNotNull(ParentControl) && IsNotNullOrEmpty(pageData.ParentId))
            //{
            //    var parentValue = BaseLibraryServiceFactory.PageDataStoreService.GetPageDatum(pageData.ParentId.Value)?.Name;
            //    columnView.AddPageContentItem(new PageContentItemView(ParentControl.DisplayLabel, parentValue));
            //}
            var controlValues = GetFormControlValues(model);

            foreach (var controlValue in controlValues)
            {
                if (controlValue == null)
                    continue;
                var control = BaseLibraryServiceFactory.AppControlService.GetAppControl(controlValue.ControlId);
                string? controlText = string.Empty;
                if (control.DataType == "Guid")
                {
                    var id = Convertor.ToGuid(controlValue.GetFirstValue());
                    if (id.HasValue)
                        controlText = factory.GetTextFromId(controlValue.ControlIdentifier, id.Value);
                }
                else
                    controlText = controlValue.GetFirstValue();

                if (columnView == null)
                    columnView = new PagePanelContentView(controlText);
                else
                    columnView.AddPageContentItem(new PageContentItemView(control.DisplayLabel, controlText));
            }
            
            if (columnView != null)
                view.Contents.Add(columnView);

            return view;
        }

        //public object ImportDataFromExcel(ExcelImportRequest model, ApplicationControlBaseFactory factory, ApplicationUser loggedInUser)
        //{

        //}
    }
}
