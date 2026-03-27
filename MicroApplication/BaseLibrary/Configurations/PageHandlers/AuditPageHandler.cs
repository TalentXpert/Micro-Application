using BaseLibrary.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Configurations.PageHandlers
{
    public class AuditPageHandler : DefaultPageHandler
    {
        public AuditPageHandler(IBaseLibraryServiceFactory serviceFactory, ApplicationUser loggedInUser) : base(serviceFactory, loggedInUser, BasePage.AuditPage, BaseForm.AuditForm, null)
        {

        }
        public override List<GridHeader> GetGridHeaders(SmartGridConfigurationVM vm, bool includeExcludedHeaders)
        {
            var headers = new List<GridHeader>();
            int i = 1;
            headers.Add(new GridHeader(i++, "Id", "Id", ControlDataType.Guid, SmartControlAlignment.Left, false).Hide());
            headers.Add(new GridHeader(i++, "Event", "Event", ControlDataType.String, SmartControlAlignment.Left, true));
            headers.Add(new GridHeader(i++, "Description", "Description", ControlDataType.String, SmartControlAlignment.Left, true));
            headers.Add(new GridHeader(i++, "CreatedOn", "CreatedOn", ControlDataType.Date, SmartControlAlignment.Left, true));

            var actions = PrepareColumnActions(AppForm);

            if (headers.Count > 1)
                headers[1].AddActions(actions);

            return headers;
        }

        public override List<UIControl> GetFilters(SmartGridConfigurationVM vm)
        {
            var events = BSF.MicroAppContract.GetApplicatinAuditEvents().GetAuditEvents();
            var eventFilters = AppControl.CreateSystemControl("F93F34C9-53FC-CB09-1958-08D9065B2081", "Events", "Events", ControlDataType.String, ControlTypes.Dropdown);
            var eventFilter = AppControl.CreateSystemControl("F93F34C9-53FC-CB09-1958-08D9065B2081", "Event", "Event", ControlDataType.String, ControlTypes.TextBox);
            var eventStartDate = AppControl.CreateSystemControl("B9CAFA4F-F8B1-C91F-CE83-08DDC87CD15E", "From", "From", ControlDataType.Date, ControlTypes.DatePicker);
            var eventEndDate = AppControl.CreateSystemControl("13BAD6B3-49B5-C20F-6288-08DDC9C2F00F", "To", "To", ControlDataType.Date, ControlTypes.DatePicker);
            var eventUIControl = new UIControl(eventFilters, null);
            eventUIControl.AddOptions(events.Select(e => new SmartControlOption { Value = e.Id.ToString(), Label = e.Name }).ToList());
            return new List<UIControl>()
            {
               eventUIControl,
                 new UIControl(eventStartDate,null),
                  new UIControl(eventEndDate,null)
            };
        }

        public override List<SmartAction> GetPageActions()
        {
            return new List<SmartAction>();
            //{
            //    new SmartAction(BaseForm.UserManagement,FormTypes.DynamicForm, "Add Country","","AddCountry", SmartActionFormMode.Add)
            //};
        }

        public override List<List<GridCell>> GetRows(GridRequestVM model, out int totalRows)
        {
            var result = BSF.RF.AuditLogRepository.GetRows(model.Filters, GetGridHeaders(model.GetSmartGridConfigurationVM(), false), LoggedInUser);
            totalRows = result.Count;
            return result;
        }

        protected override List<SmartAction> PrepareColumnActions(AppForm form)
        {
            var actions = new List<SmartAction>
            {
                new SmartAction(form,FormTypes.DynamicForm, "View","View","View", SmartActionFormMode.View),
                //new SmartAction(form,FormTypes.DynamicForm,"Edit","Edit","Edit", SmartActionFormMode.Edit),
                //new SmartAction(BaseForm.UserRoleForm,FormTypes.SelectFromListForm, "Manage Roles","ManageRoles","ManageRoles",SmartActionFormMode.Add),
            };
            return actions;
        }
    }
}

/*
 

D5F61116-FBA2-CC3A-81EC-08DDC9C399DD
2A08563C-4E51-C86F-1B2C-08DDC9C39B5A
60BF9AF2-8B97-C3C7-9FE2-08DDC9CF1B56
3EA5ADAF-F0AE-C974-D25B-08DDC9CF3565
2B921C85-5CC2-C97D-019B-08DDC9D4A0EF
A64E68FF-0F06-C66B-F488-08DDC9D4A498
BF0D2448-E509-CC76-67AE-08DDC9EC702E
CF32C010-73A5-C5B4-17F0-08DDC9F12516
 */