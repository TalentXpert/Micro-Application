using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Utilities.Files
{
    public class AssemblyEmbededFileReader
    {
        public string GetScript(Assembly assembly, string filename)
        {
            var script = X.File.AssemblyTextFileReader.ReadFile(assembly, filename);
            return script;
        }
    }

    public class AssemblyTextFileReader
    {
        public string ReadFile(Assembly assembly, string fileName)
        {
            var resourceName = GetManifestResourceName(assembly, fileName);

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        private static string GetManifestResourceName(Assembly assembly, string fileName)
        {
            fileName = "." + fileName;
            string name = assembly.GetManifestResourceNames().SingleOrDefault(n => n.EndsWith(fileName, StringComparison.InvariantCultureIgnoreCase));

            if (string.IsNullOrEmpty(name))
            {
                throw new FileNotFoundException($"Embedded file '{fileName}' could not be found in assembly '{assembly.FullName}'.", fileName);
            }

            return name;
        }
    }
}
