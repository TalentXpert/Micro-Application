using BaseLibrary.Repositories;
using BaseLibrary.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;

namespace BaseLibrary.Utilities.Excels
{
    public abstract class Localizable
    {
        protected string Language { get; }
        public Localizable(string language)
        {
            Language = language;
        }
        protected string Translate(string word)
        {
            return word; // WordTranslator.Translate(word, Language);
        }
    }
    public class DynmicFormImporter : ImporterBase
    {
        public DynmicFormImporter(IBaseLibraryServiceFactory serviceFactory, ImportInput input) : base(serviceFactory, input)
        {
        }

        public override byte[] GetTemplate(ImportTemplate model)
        {
            throw new NotImplementedException();
        }

        public override ImportDataResult ImportData(ImportTemplateFile model)
        {
            throw new NotImplementedException();
        }

        public override string Instructions()
        {
            throw new NotImplementedException();
        }
    }

    public class ImportInput
    {
        public string Language { get; set; }
        public ApplicationUser User { get; set; }
    }

    public class ExcelImportRequest
    {
        public Guid FormId { get; set; }

        public IFormFile File { get; set; }

        public List<ControlValue> GlobalControls { get; set; }

    }

    public class ExcelImportTmplateRequest
    {
        public Guid FormId { get; set; } //programing use only
        public List<ControlValue> GlobalControls { get; set; }
        public int Rows { get; set; }
        public ExcelImportTmplateRequest()
        {
            GlobalControls = new List<ControlValue>();
        }
    }

    public class ImportDataResult
    {
        public string Message { get; set; }
        public byte[] File { get; set; }
        public int Imported { get; set; }
        public bool HasError { get; set; } = false;
        public int NotImported { get; set; }
    }
    public class ImportTemplateFile
    {
        public string ImportType { get; set; }
        public int EquipmentGroupId { get; set; }
        public string? WerkId { get; set; }
        public FileHelperModel ImportFile { get; set; }
    }
    public class FileHelperModel
    {
        public string Filename { get; set; }
        public string Filecontent { get; set; }
        public string Filetyp { get; set; }

    }
    public class ImportTemplate
    {
        public string ImportType { get; set; }
        public int EquipmentGroupId { get; set; }
        public string? WerkId { get; set; }
    }

    public abstract class ImporterBase : Localizable
    {
        protected List<Dictionary<string, string>> ErrorRows { get; }
        public ImporterBase(IBaseLibraryServiceFactory serviceFactory, ImportInput input) : base(input.Language)
        {
            SF = serviceFactory;
            User = input.User;
            // Organization = input.Organisation;
            ErrorRows = new List<Dictionary<string, string>>();
            ValidationHandler = new ValidationHandler("en");
        }

        public IBaseLibraryServiceFactory SF { get; private set; }
        public ApplicationUser User { get; }
        ValidationHandler ValidationHandler;

        public byte[] GetWorkbook(ImportTemplateFile model)
        {
            string base64String = model.ImportFile.Filecontent.Contains("base64") ? model.ImportFile.Filecontent.Split("base64,")[1] : model.ImportFile.Filecontent;
            byte[] file = Convert.FromBase64String(base64String);
            return file;
        }

        public abstract byte[] GetTemplate(ImportTemplate model);
        public abstract ImportDataResult ImportData(ImportTemplateFile model);
        public abstract string Instructions();
        

        public virtual bool NeedUniqueId() { return false; }
        private const string KeyName = "Id";
        public void AddUniqueKeyHeader(List<string> headers)
        {
            if (NeedUniqueId())
                headers.Insert(0, KeyName);
        }
        public bool IsUniqueKeyHeader(string header)
        {
            return header.Equals(KeyName, StringComparison.InvariantCultureIgnoreCase);
        }

        protected void HandleError(Exception ex, Dictionary<string, string> exceRow)
        {
            var errormessage = ValidationHandler.GetCustomValidationExceptionMessage(ex);
            exceRow.Add("Error", errormessage);
            ErrorRows.Add(exceRow);
        }
        protected ImportDataResult GetImportResult(int totalRecords, List<ImportExcelHeader> excelheaders, string worksheet, bool showLookup = false, bool addLookupDropdown = true)
        {
            var result = new ImportDataResult();
            result.HasError = ErrorRows.Any();
            result.Imported = totalRecords - ErrorRows.Count;
            result.NotImported = ErrorRows.Count;
            if (result.HasError)
                result.File = new ExcelImportTemplate(WordTranslator.Translate(worksheet, Language), excelheaders, showLookup, addLookupDropdown).GenerateExcel(ErrorRows);
            return result;
        }
        protected static List<string> boolTrueValues = new List<string> { "Yes", "Y", "J", "Ja", "1", "True" };
        protected static List<string> boolFalseValues = new List<string> { "No", "N", "NEIN", "1", "false" };
        protected string GetBoolValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;
            if (boolTrueValues.Any(v => v.Equals(value, StringComparison.InvariantCultureIgnoreCase)))
                return true.ToString();
            return false.ToString();
        }
        protected bool IsTrueBoolValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value) == false && boolTrueValues.Any(v => v.Equals(value, StringComparison.InvariantCultureIgnoreCase)))
                return true;
            return false;
        }
        private void SaveFile(IFormFile file, string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                file.CopyTo(stream);
            }
        }
    }
}
