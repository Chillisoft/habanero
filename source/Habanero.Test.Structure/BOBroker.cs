using System.IO;

namespace Habanero.Test.Structure
{
    public static class BOBroker
    {
        public static string GetClassDefsXml()
        {
            StreamReader classDefStream = new StreamReader(
                typeof(BOBroker).Assembly.GetManifestResourceStream("Habanero.Test.Structure.ClassDefs.xml"));
            return classDefStream.ReadToEnd();
        }
    }
}