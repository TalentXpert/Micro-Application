using BaseLibrary.Configurations.FormHandlers;

namespace BaseLibrary.Configurations
{
    
    public abstract class FormHandlerFactory
    {
        public FormHandlerFactory() { }

        public abstract SelectFromListFormHandler GetSelectFromListFormHandler(Guid formId);
    }
}
