using System.Xml;

namespace Habanero.Base.Util
{
    public static class XmlHelpersCF
    {
        public static string GetAttributeOrDefault(XmlReader reader, string attributeName, string defaultValue)
        {
            var attribute = reader.GetAttribute(attributeName) ?? defaultValue;
            return attribute;
        }
    }
}
