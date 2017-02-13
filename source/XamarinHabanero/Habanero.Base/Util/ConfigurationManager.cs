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
        private static readonly NameValueCollection _appSettings = new NameValueCollection();

        public static NameValueCollection AppSettings => _appSettings;

        public static IDictionary GetSection(string configSectionName)
        {
            return _doc?.Descendants(configSectionName)
                      .Where(elm => elm.Name == configSectionName).Descendants()
                      .ToDictionary(a=>a.Attribute("key")?.Value.ToString(), a=>a.Attribute("value")?.Value.ToString());
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

            MakeAppSettings();
        }

        private static void MakeAppSettings()
        {
            var appSettings = _doc?.Descendants("appSettings")
                .Descendants()
                .ToDictionary(a => a.Attribute("key")?.Value.ToString(), a => a.Attribute("value")?.Value.ToString());

            foreach (var appSetting in appSettings)
            {
                _appSettings.Add(appSetting.Key, appSetting.Value);
            }
        }
    }
}