using System.Xml.Linq;

namespace MS.GTA.ServicePlatform.Exceptions
{
    /// <summary>
    /// Utilities for handling xml.
    /// </summary>
    internal static class XmlUtil
    {
        /// <summary>
        /// Ugly replacement for SecureElement.Escape, since that is not portable until NETStandard 2.0.
        /// </summary>
        /// <param name="xml">The xml node content to be escaped.</param>
        /// <returns>The escaped xml node content.</returns>
        public static string EscapeXmlNodeContent(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
            {
                return string.Empty;
            }

            return new XElement("r", xml).FirstNode.ToString();
        }
    }
}