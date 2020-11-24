[module: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:FileHeaderFileNameDocumentationMustMatchTypeName", Justification = "Small closely related classes may be combined.")]

//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="XmlNodeExtension.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.Common.Base.Configuration
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Threading.Tasks;
    using System.Xml;

    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// XML node extension class
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Small closely related classes may be combined.")]
    public static class XmlNodeExtension
    {
        /// <summary>
        /// Gets node attribute value
        /// </summary>
        /// <param name="node">XML node</param>
        /// <param name="value">Value of node</param>
        /// <returns>Node attribute value string</returns>
        public static string GetAttribute(this XmlNode node, string value)
        {
            var namedAttribute = node.Attributes?[value];
            if (namedAttribute != null)
            {
                return namedAttribute.Value;
            }

            throw new InvalidOperationException($"FabricXml Node {value} not found");
        }
    }

    /// <summary>
    /// Fabric XML configuration provider class
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
       Justification = "Small closely related classes may be combined.")]
    public class FabricXmlConfigurationProvider : FileConfigurationProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FabricXmlConfigurationProvider" /> class.
        /// </summary>
        /// <param name="fabricXmlConfigurationSource">Instance of Fabric XML configuration</param>
        public FabricXmlConfigurationProvider(FabricXmlConfigurationSource fabricXmlConfigurationSource)
            : base(fabricXmlConfigurationSource)
        {
        }

        /// <summary>
        /// Load setting file as stream
        /// </summary>
        /// <param name="stream">File read as stream</param>
        public override void Load(Stream stream)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(stream);
            foreach (XmlNode section in xmlDocument.GetElementsByTagName("Section"))
            {
                foreach (XmlNode childNode in section.ChildNodes)
                {
                    Data.Add(
                    $"{section.GetAttribute("Name")}:{childNode.GetAttribute("Name")}", childNode.GetAttribute("Value"));
                }
            }
        }
    }
}
