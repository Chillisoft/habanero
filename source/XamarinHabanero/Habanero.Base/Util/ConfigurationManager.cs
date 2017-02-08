using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Habanero.Base.Util
{
    public static class ConfigurationManager
    {
        private static XDocument _doc;

        public static NameValueCollection AppSettings => new NameValueCollection();

        public static IDictionary GetSection(string configSectionName)
        {
            return _doc?.Element(configSectionName)?.Elements().ToDictionary(x => x.Name.LocalName, x => x.Value);
        }

        public static void Initialise(Assembly asm)
        {
            var path = asm.GetManifestResourceNames().FirstOrDefault(x => x.Contains("App.config"));

            using (var stream = asm.GetManifestResourceStream(path))
            {
                using (var reader = new StreamReader(stream))
                {
                    _doc = XDocument.Parse(reader.ReadToEnd());
                }
            }
        }
    }
}