using System.Reflection;

namespace BaseLibrary.DatabaseMigrations
{
    public class DbScriptReader
    {
        public string GetScript(string fileName,Assembly assembly)
        {
            var script = X.File.AssemblyEmbededFileReader.GetScript(assembly, fileName);
            return script;
        }
    }

}
