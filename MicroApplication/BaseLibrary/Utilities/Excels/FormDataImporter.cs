namespace BaseLibrary.Utilities.Excels
{
    //public class FormDataImporter : ImporterBase
    //{
    //    public FormDataImporter(IServiceFactory serviceFactory, ImportInput input) : base(serviceFactory, input)
    //    { }
    //    public override byte[] GetTemplate(ImportTemplate model)
    //    {
    //        var headers = GetHeaders();
    //        return new ExcelImportTemplate(Translate("Building"), headers).CreateImportTemplate(false);
    //    }

    //    private List<ImportExcelHeader> GetHeaders()
    //    {
    //        var headers = new List<ImportExcelHeader>
    //        {
    //            new ImportExcelHeader(Translate("BuildingName"), true),
    //        };
    //        return headers;
    //    }

    //    public override ImportDataResult ImportData(ImportTemplateFile model)
    //    {
    //        var headers = GetHeaders();
    //        var excelRows = new ExcelImportData(GetWorkbook(model), headers).GetExcelData(true);

    //        foreach (var exceRow in excelRows)
    //        {
    //            try
    //            {
    //                CreateNewServiceFactory();
    //                var bauModel = GetModel(exceRow, model.WerkId);
    //                SF.BuildingService.SaveUpdateBuilding(Organization, bauModel);
    //                SF.RepositoryFactory.BuildingRepository.UnitOfWork.Commit();
    //            }
    //            catch (Exception ex)
    //            {
    //                HandleError(ex, exceRow);
    //                SF.RepositoryFactory.BuildingRepository.UnitOfWork.RollbackChanges();
    //            }
    //        }

    //        var result = GetImportResult(excelRows.Count, headers, Translate("Building"));
    //        return result;
    //    }
    //    private BauVorlageVM GetModel(Dictionary<string, string> r, string werkId)
    //    {
    //        var model = new BauVorlageVM();
    //        model.WerkID = werkId;
    //        foreach (var k in r.Keys)
    //        {
    //            var value = r[k];
    //            if (Translate("BuildingName") == k)
    //                model.Bau = value;
    //        }
    //        return model;
    //    }

    //    public override string Instructions()
    //    {
    //        return "";
    //    }
    //}
}

