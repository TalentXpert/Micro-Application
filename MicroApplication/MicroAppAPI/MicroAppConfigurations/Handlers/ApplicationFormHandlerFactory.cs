
namespace MicroAppAPI.Configurations
{
    public class ApplicationFormHandlerFactory : FormHandlerFactory
    {
        public ApplicationFormHandlerFactory(IServiceFactory serviceFactory) : base(serviceFactory) 
        {
            ServiceFactory = serviceFactory;
        }

        public IServiceFactory ServiceFactory { get; }

        public override SelectFromListFormHandler GetSelectFromListFormHandler(Guid formId)
        {
            switch(formId.ToString().ToUpper())
            {
                case ApplicationForm.UserRoleFormId:
                    return new UserRoleSelectFromListFormHandler(ServiceFactory);
            }
            throw new ValidationException($"No form handler found for form id -{formId}");
        }
    }
}
