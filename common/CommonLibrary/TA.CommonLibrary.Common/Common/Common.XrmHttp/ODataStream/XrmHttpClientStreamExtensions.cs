//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.XrmHttp.ODataStream
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using TA.CommonLibrary.Common.XrmHttp.Model;

    public static class XrmHttpClientStreamExtensions
    {
        public static async Task<Stream> DownloadXrmDocument(this IXrmHttpClient client, Guid annotationPrimaryId)
        {
            // Build the query for the url to only retrieve the documentBody from the annotation entity.
            var annotationQuery = client.Get<Annotation>(
                id: annotationPrimaryId,
                select: s => new
                {
                    s.DocumentBody,
                });

            using (var request = new HttpRequestMessage
            {
                Method = annotationQuery.Method,
                RequestUri = new Uri(client.BaseAddress, annotationQuery.RequestUri),
            })
            {
                foreach (var header in annotationQuery.Headers)
                {
                    request.Headers.Add(header.Item1, header.Item2);
                }

                // Intentionally not disposed so the returned stream can be used.
                var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                if (!response.IsSuccessStatusCode)
                {
                    throw await XrmHttpClientExceptionFactory.CreateExceptionFromResponse(response);
                }

                var httpContentStream = await response.Content.ReadAsStreamAsync();
                var odataResponseStream = new XrmAnnotationDocumentStream(httpContentStream);
                return new CryptoStream(odataResponseStream, new FromBase64Transform(), CryptoStreamMode.Read);
            }
        }

        public static async Task<Annotation> UploadXrmDocument(this IXrmHttpClient client, Annotation annotation, Stream contentStream, long? streamLength = null)
        {
            if (annotation?.RecId == null)
            {
                // make sure not passing any document body while creating the annotation
                annotation.DocumentBody = string.Empty;
                var createdAnnotation = await client.CreateAndReturn(annotation).ExecuteAndGetAsync();
                annotation.RecId = createdAnnotation.RecId;
            }

            var guid = Guid.NewGuid().ToString();
            annotation.DocumentBody = guid;
            var updateAnnotation = client.Update(annotation.RecId.Value, annotation, c => new { c.DocumentBody });
            var base64ContentStream = new CryptoStream(contentStream, new ToBase64Transform(), CryptoStreamMode.Read);

            var indexOfValue = updateAnnotation.Content.IndexOf(guid);

            var preContent = updateAnnotation.Content.Substring(0, indexOfValue);
            var preContentByteArray = Encoding.UTF8.GetBytes(preContent);
            var preContentStream = new MemoryStream(preContentByteArray);
            var postContent = updateAnnotation.Content.Substring(indexOfValue + guid.Length);
            var postContentByteArray = Encoding.UTF8.GetBytes(postContent);
            var postContentStream = new MemoryStream(postContentByteArray);

            var totalContentLength = (long)(preContent.Length + (Math.Ceiling((double)(streamLength ?? contentStream.Length) / 3.0) * 4.0) + postContent.Length);
            
            var annotationUploadStream = new XrmAnnotationUploadStream(new List<Stream>() { preContentStream, base64ContentStream, postContentStream });
            using (var request = new HttpRequestMessage
            {
                Method = updateAnnotation.Method,
                RequestUri = new Uri(client.BaseAddress, updateAnnotation.RequestUri),
                Content = new StreamContent(annotationUploadStream)
                {
                    Headers =
                    {
                        ContentType = new MediaTypeWithQualityHeaderValue("application/json"),
                        ContentLength = totalContentLength,
                    }
                },
                Headers =
                {
                    Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
                },
            })
            {
                foreach (var header in updateAnnotation.Headers)
                {
                    request.Headers.Add(header.Item1, header.Item2);
                }

                using (var response = await client.SendAsync(request))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw await XrmHttpClientExceptionFactory.CreateExceptionFromResponse(response);
                    }
                }

                return annotation;
            }
        }

    }
}
