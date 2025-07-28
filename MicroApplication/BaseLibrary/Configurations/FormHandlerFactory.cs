using BaseLibrary.Configurations.FormHandlers;

namespace BaseLibrary.Configurations
{
    
    public abstract class FormHandlerFactory
    {
        public FormHandlerFactory(IBaseLibraryServiceFactory serviceFactory)
        {
            BaseLibraryServiceFactory = serviceFactory;
        }

        public IBaseLibraryServiceFactory BaseLibraryServiceFactory { get; }

        public virtual SelectFromListFormHandler GetSelectFromListFormHandler(Guid formId)
        {
            switch (formId.ToString().ToUpper())
            {
                case BaseForm.UserRoleFormId:
                    return new UserRoleSelectFromListFormHandler(BaseLibraryServiceFactory);
            }
            throw new ValidationException($"No form handler found for form id -{formId}");
        }

    }
}
