
using BaseLibrary.Controls;

namespace BaseLibrary.Tests
{
    /*
    public class SmartFormTest
    {
        [TestMethod]
        public void AddPropertyForm()
        {
            const string pageIdentifier = "Property";

            var formGenerateRequest = new SmartFormGenerateRequest { PageIdentifier = pageIdentifier, Mode = "Add" };
            
            var propertyForm = new SmartForm(pageIdentifier, "Add Property");
            propertyForm.AddTextBox("Name", "Name", "", ControlDataType.String, true, true);
            propertyForm.AddDropdown("ContactPerson", "Contact Person", "", ControlDataType.String, true, true, new List<SmartControlOption> { new SmartControlOption("Select Contact Person", ""), new SmartControlOption("Pradeep", "F65D5C62-08B7-430E-ADD5-D9B75A647497") });
            propertyForm.AddTextArea("Description", "Description", "", true, false);
            propertyForm.AddTextBox("Address1", "Address Line 1", "", ControlDataType.String, true, true);
            propertyForm.AddTextBox("Address2", "Address Line 2", "", ControlDataType.String, true, true);
            propertyForm.AddTextBox("Address3", "Address Line 3", "", ControlDataType.String, true, true);
            var country = new SmartControl
            {
                ControlIdentifer = "Country",
                DisplayLabel = "Country",
                IsMandatory = true,
                IsEditable = true,
                ControlType = "Dropdown",
                IsParent = true,
                Options = new List<SmartControlOption> { new SmartControlOption("India", "328F20EC-2BF0-42E4-AFBE-097A2423721A"), new SmartControlOption("USA", "EA9B6094-8D4C-4817-BE66-64B552595C88") },
            };
            propertyForm.AddControl(country);
            var state = new SmartControl
            {
                ControlIdentifer = "State",
                DisplayLabel = "State",
                IsMandatory = true,
                IsEditable = true,
                ControlType = "Dropdown",
                IsParent = true,
                ParentControlIdentifier= country.ControlIdentifer,
                Options = new List<SmartControlOption> { new SmartControlOption("Maharashtra", "328F20EC-2BF0-42E4-AFBE-097A2423721A"), new SmartControlOption("Gujrat", "EA9B6094-8D4C-4817-BE66-64B552595C88") },
            };
            propertyForm.AddControl(state);
            var city = new SmartControl
            {
                ControlIdentifer = "City",
                DisplayLabel = "City",
                IsMandatory = true,
                IsEditable = true,
                ControlType = "Dropdown",
                IsParent = false,
                ParentControlIdentifier = state.ControlIdentifer,
                Options = new List<SmartControlOption> { new SmartControlOption("Pune", "328F20EC-2BF0-42E4-AFBE-097A2423721A"), new SmartControlOption("Thane", "EA9B6094-8D4C-4817-BE66-64B552595C88") },
            };
            propertyForm.AddControl(city);
            propertyForm.AddTextBox("Landmark", "Landmark", "", ControlDataType.String, true, false);

            var requestToSave = new SmartFormTemplateRequest { Mode = formGenerateRequest.Mode, PageIdentifier = formGenerateRequest.PageIdentifier };
            requestToSave.RequestControls.Add(
                new SmartFormTemplateRequestControl
                {
                    ControlIdentifier = "Name",
                    ControlValues = new List<SmartControlNameValue>
                    { new SmartControlNameValue { Name = "Value", Value = "SaiApex" }}
                });
            //Send to FormProcessAPI
        }
    }
    */
}