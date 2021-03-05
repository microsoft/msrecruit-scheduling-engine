//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.Email.SendGridContracts
{
    using System;
    using System.Drawing;
    using System.IO;
    using CommonDataService.Common.Internal;
    using Newtonsoft.Json;

    /// <summary>
    /// Attachment class
    /// </summary>
    public class Attachment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Attachment" /> class.
        /// </summary>
        public Attachment()
        {
            this.ContentId = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Gets or sets the Content of attachment in base64 block.
        /// </summary>
        [JsonProperty("content")]
        public string Base64Content { get; set; }

        /// <summary>
        /// Gets the contentId.
        /// </summary>
        [JsonProperty("content_id")]
        public string ContentId { get; private set; }

        /// <summary>
        /// Gets the Disposition.
        /// </summary>
        [JsonProperty("disposition")]
        public string Disposition
        {
            get
            {
                return "inline";
            }
        }

        /// <summary>
        /// Gets the Filename.
        /// </summary>
        [JsonProperty("filename")]
        public string Filename
        {
            get
            {
                return $"{Name}.{Type}";
            }
        }

        /// <summary>
        /// Gets or sets the file name. It is the name portion of the file name without extension.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type. It is an extension of the attached file.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets inline Cid parameter name. It allows the attachment to show up embedded in the message.
        /// </summary>
        public string InlineCidParameterName { private get; set; }

        /// <summary>
        /// It will create the attachment from the image resource stored in a resource file.
        /// </summary>
        /// <param name="bitmap">Bitmap the image resource.</param>
        /// <param name="inlineCidParameterName">Optional cid Parameter name.</param>
        /// <param name="fileName">file name of the attachment.</param>
        /// <param name="fileExtension">Optional extension parameter. If it does not exist, it will be created for us.</param>
        /// <returns>The attachment object.</returns>
        public static Attachment CreateAttachmentFromImageResource(Bitmap bitmap, string inlineCidParameterName, string fileName, string fileExtension)
        {
            Contract.CheckValue(bitmap, nameof(bitmap));
            Contract.CheckNonEmpty(fileName, nameof(fileName), "fileName should not be null or empty");

            byte[] bannerImageBytes = null;
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                bannerImageBytes = stream.ToArray();
            }

            Contract.CheckValue(bannerImageBytes, nameof(bannerImageBytes));

            var imageBase64String = Convert.ToBase64String(bannerImageBytes);

            return new Attachment()
            {
                Base64Content = imageBase64String,
                Name = fileName,
                Type = fileExtension,
                InlineCidParameterName = inlineCidParameterName
            };
        }

        /// <summary>
        /// Method to get Cid Parameter for inline rendering of the attachment.
        /// </summary>
        /// <returns>Returns the Cid Parameter.</returns>
        public string GetInlineCidParameterName()
        {
            return this.InlineCidParameterName;
        }
    }
}
