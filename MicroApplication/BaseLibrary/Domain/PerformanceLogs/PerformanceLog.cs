namespace BaseLibrary.Domain.PerformanceLogs
{
    public class PerformanceLog : Entity
    {
        public string Method { get; set; }
        public int Duration { get; set; }
        public DateTime ExecutedOn { get; set; }
        public string Controller { get; set; }
        public string ActionId { get; set; }
        public Guid? ApplicationUserId { get; set; }
        public string Detail { get; set; }
        public PerformanceLog()
        {
            Id = IdentityGenerator.NewSequentialGuid();
            ExecutedOn = DateTime.UtcNow;
            SetCreatedOn();
            SetUpdatedOn();
        }
        public override IEnumerable<ValidationResult> ValidateEntity(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            X.Validator.GuidValidator.CheckForEmpltyOrDefaulValue(Id, "Id", validationResults);
            X.Validator.StringValidator.CheckForNullOrEmpty(Method, "Method", validationResults);
            X.Validator.StringValidator.CheckForMaxLength(Method, "Method", 128, validationResults);
            X.Validator.IntegerValidator.CheckForEmpltyOrDefaulValue(Duration, "Duration", validationResults);
            X.Validator.StringValidator.CheckForNullOrEmpty(Controller, "Controller", validationResults);
            X.Validator.StringValidator.CheckForMaxLength(Controller, "Controller", 128, validationResults);
            X.Validator.StringValidator.CheckForNullOrEmpty(ActionId, "ActionId", validationResults);
            X.Validator.StringValidator.CheckForMaxLength(ActionId, "ActionId", 64, validationResults);

            return validationResults;
        }


        //public PerformanceLogVM ToPerformanceLogVM(IApplicationCacheAppService cache)
        //{
        //    var vm = new PerformanceLogVM
        //    {
        //        Id = Id,
        //        Controller = Controller,
        //        Method = Method,
        //        Duration = Duration,
        //        ExecutedOn = ExecutedOn

        //    };

        //    if (this.ApplicationUserId.HasValue && this.ApplicationUserId != Guid.Empty)
        //    {
        //        var user = cache.GetApplicationUser(this.ApplicationUserId.Value);
        //        if (user != null)
        //            vm.User = user.Name;
        //    }
        //    return vm;
        //}

    }
}
