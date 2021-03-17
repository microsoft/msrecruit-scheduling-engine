//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.XrmHttp
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;

    public class XrmHttpClientUriSanitizer
    {
        private static readonly Regex SanitizeUriPathRegex = new Regex(@"[0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12}|'[^']*(''[^']*)*'");
        private static readonly string[] SanitizeUriQueryKeyWhitelist = new[] { "$select", "$expand", "$top", "$orderBy", "$count" };

        // http://docs.oasis-open.org/odata/odata/v4.0/errata03/os/complete/part3-csdl/odata-v4.0-errata03-os-part3-csdl-complete.html#_Toc453752675
        private static readonly Regex ODataIndentifierRegex = new Regex(@"^[\p{L}\p{Nl}_][\p{L}\p{Nl}\p{Nd}\p{Mn}\p{Mc}\p{Pc}\p{Cf}]*$");

        public static string SanitizeUri(Uri uri)
        {
            var uriBuilder = new UriBuilder(uri);
            uriBuilder.Path = SanitizeUriPathRegex.Replace(uriBuilder.Path, "...");
            var parsedQuery = HttpUtility.ParseQueryString(uriBuilder.Query);
            foreach (var key in parsedQuery.AllKeys)
            {
                if (!SanitizeUriQueryKeyWhitelist.Contains(key))
                {
                    parsedQuery.Set(key, "...");
                }
            }
            uriBuilder.Query = string.Join("&", parsedQuery.AllKeys.Select(k => $"{k}={parsedQuery[k]}"));
            return uriBuilder.Uri.ToString();
        }

        public static bool IsValidODataIdentifier(string value)
        {
            return ODataIndentifierRegex.IsMatch(value);
        }
    }
}
