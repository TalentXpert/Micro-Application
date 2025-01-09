using BaseLibrary.Domain;
using BaseLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroAppAPI.Domain
{
    /// <summary>
    /// This class will keep information about extra field those are required for a lookup othen then default given. 
    /// </summary>
    public class LookupType : Entity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string ExtraFieldsConfiguration { get; set; }
        public override IEnumerable<ValidationResult> ValidateEntity(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }

    public class Lookup : Entity
    {
        public string Text { get; protected set; }
        public bool IsReviewed { get; protected set; }
        public string LookupCode { get; protected set; }
        public Guid? ParentId { get; protected set; }
        public Guid? ResolvedWithId { get; protected set; }
        public string JSONData { get; protected set; }

        public bool IsValid { get; protected set; }

        public static Lookup Create(LookupType lookupType, string text, Guid? lookupParent, Guid? addedById, string sourceObject)
        {
            var lookup = Create(IdentityGenerator.NewSequentialGuid(), lookupType.Code, text, lookupParent, addedById, sourceObject);
            return lookup;
        }

        public static Lookup Create(Guid id, string lookupCode, string text, Guid? lookupParent, Guid? addedById, string sourceObject)
        {
            var lookup = new Lookup()
            {
                Id = id,
                LookupCode = lookupCode,
                IsReviewed = false,
                ParentId = lookupParent,
                IsValid = true,
            };
            lookup.SetCreatedOn();
            lookup.Update(text);
            return lookup;
        }


        public void ResetToOriginal()
        {
            ResolvedWithId = null;
            IsReviewed = true;
            IsValid = true;
            SetUpdatedOn();
        }

        public void Update(string text, string textInfo1 = null, string textInfo2 = null, int? numInfo1 = 0, int? numInfo2 = 0)
        {
            Text = text.Trim();
            SetUpdatedOn();
        }


        public void Reviewed()
        {
            ResetToOriginal();
        }

        public void MarkAsValid()
        {
            IsValid = true;
            SetUpdatedOn();
        }

        public void MarkAsInValid()
        {
            IsValid = false;
            SetUpdatedOn();
        }



        public override IEnumerable<ValidationResult> ValidateEntity(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            X.Validator.GuidValidator.CheckForEmpltyOrDefaulValue(Id, "Id", validationResults);
            X.Validator.StringValidator.CheckForNullOrEmpty(LookupCode, "LookupCode", validationResults);

            X.Validator.StringValidator.CheckForNullOrEmpty(Text, "Text", validationResults);
            X.Validator.StringValidator.CheckForMaxLength(Text, "Text", 64, validationResults);


            return validationResults;

        }

        public void Resolved(Guid resolvedWithId)
        {
            //if (IsSeeded) throw new ValidationException("You can not resolved seeded lookup");
            ResolvedWithId = resolvedWithId;
            SetUpdatedOn();
        }

        //public LookupVM ToLookupVM(IApplicationCacheAppService cacheStore)
        //{
        //    var vm = new LookupVM
        //    {
        //        Id = this.Id,
        //        LookupTypeId = this.LookupType,
        //        Number1 = this.NumInfo1,
        //        Number2 = this.NumInfo2,
        //        ParentId = this.ParentId,
        //        Text = this.Text,
        //        Text1 = this.TextInfo1,
        //        Text2 = this.TextInfo2,
        //        IsReviewed = this.IsReviewed.ToString(),
        //        IsSeeded = this.IsSeeded.ToString(),
        //        ResolvedId = this.ResolvedWithId,
        //        IsValid = this.IsValid,
        //        AddedById = this.AddedById,
        //        SourceObject = SourceObject
        //    };


        //    return vm;
        //}

        public void ToggleInvalidFlag()
        {
            IsValid = !IsValid;
            SetUpdatedOn();
        }




        //public Lookup Clone()
        //{
        //    return new Lookup
        //    {
        //        Id = this.Id,
        //        LookupType = this.LookupType,
        //        NumInfo1 = this.NumInfo1,
        //        NumInfo2 = this.NumInfo2,
        //        ParentId = this.ParentId,
        //        Text = this.Text,
        //        TextInfo1 = this.TextInfo1,
        //        TextInfo2 = this.TextInfo2,
        //        IsReviewed = this.IsReviewed,
        //        IsSeeded = this.IsSeeded,
        //        ResolvedWithId = this.ResolvedWithId,
        //        IsValid = this.IsValid,
        //        AddedById = this.AddedById,
        //        SourceObject = SourceObject,
        //        CreatedOn = this.CreatedOn,
        //        UpdatedOn = this.UpdatedOn
        //    };
        //}
    }

    public class LookupVM
    {
        public Guid Id { get; set; }
        public int LookupTypeId { get; set; }
        public string Text { get; set; }
        public Guid? ParentId { get; set; }
        public string Parent { get; set; }
        public int? Number1 { get; set; }
        public int? Number2 { get; set; }
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string IsReviewed { get; set; }
        public string IsSeeded { get; set; }
        public Guid? ResolvedId { get; set; }
        public bool IsValid { get; set; }
        public Guid? AddedById { get; set; }
        public string SourceObject { get; set; }
    }

    public class AddLookupsVM
    {
        public int LookupTypeId { get; set; }
        public Guid? ParentId { get; set; }
        public string CSVLookups { get; set; }
    }
}
