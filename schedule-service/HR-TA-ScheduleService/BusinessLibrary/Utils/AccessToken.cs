//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ScheduleService.BusinessLibrary.Utils
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using HR.TA.CommonDataService.Common.Internal;

    /// <summary>
    /// Class Access Token
    /// </summary>
    public static class AccessToken
    {
        /// <summary>
        /// Get User Email From Token
        /// </summary>
        /// <param name="accessToken">Access Token</param>
        /// <returns>service account email</returns>
        public static string GetEmailAddressFromToken(string accessToken)
        {
            Contract.CheckValue(accessToken, nameof(accessToken));

            var handler = new JwtSecurityTokenHandler();
            var user = handler.ReadToken(accessToken) as JwtSecurityToken;
            return user?.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value;
        }

        /// <summary>
        /// Get expiration date time from token
        /// </summary>
        /// <param name="accessToken">Access Token</param>
        /// <returns>service account email</returns>
        public static DateTime GetExpirationDateTimeFromToken(string accessToken)
        {
            Contract.CheckValue(accessToken, nameof(accessToken));

            var handler = new JwtSecurityTokenHandler();
            var user = handler.ReadToken(accessToken) as JwtSecurityToken;

            return user?.ValidTo ?? DateTime.UtcNow;
        }
    }
}
