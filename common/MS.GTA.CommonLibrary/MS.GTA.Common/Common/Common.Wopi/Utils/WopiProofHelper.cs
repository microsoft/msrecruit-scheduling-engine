//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.Wopi.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using CommonDataService.Common.Internal;
    using Configuration;
    using MS.GTA.ServicePlatform.Configuration;
    using MS.GTA.ServicePlatform.Tracing;
    using Discovery;
    using Interfaces;

    /// <summary>
    /// Verifies the proof that comes from the WOPI client server
    /// </summary>
    public class WopiProofHelper : IProofHelper
    {
        /// <summary>
        /// Trace source
        /// </summary>
        private readonly ITraceSource trace;

        /// <summary>
        /// Time period for request header validity
        /// </summary>
        private int headerValidityTimePeriod = 20; // in minutes

        /// <summary>
        /// Initializes a new instance of the <see cref="WopiProofHelper"/> class.
        /// </summary>
        /// <param name="configurationManager">Configuration settings</param>
        /// <param name="trace">Trace source instance</param>
        public WopiProofHelper(IConfigurationManager configurationManager, ITraceSource trace)
        {
            Contract.CheckValue(configurationManager, nameof(configurationManager), "configurationManager must be provided");
            Contract.CheckValue(trace, nameof(trace), "trace should be provided");

            this.trace = trace;
            this.headerValidityTimePeriod = configurationManager.Get<WopiSetting>().WopiProofValidityWindowInMinutes;            
        }

        /// <summary>
        /// Verifies that the token has not expired
        /// </summary>
        /// <param name="ticks">A 64-bit integer that represents the number of 100-nanosecond intervals that have elapsed between 12:00:00 midnight, January 1, 0001, UTC and the UTC time of the request</param>
        /// <returns>True if the token has expired</returns>
        public virtual bool IsTokenExpired(long ticks)
        {
            Contract.Check(ticks > 0, "The number of ticks should be a positive non-zero value");

            var tickDifference = Math.Abs(DateTime.UtcNow.Ticks - ticks);
            var ticksInMinutes = TimeSpan.FromTicks(tickDifference).TotalMinutes;
            var tokenHasExpired = ticksInMinutes > this.headerValidityTimePeriod;
            this.trace.TraceInformation($"WopiProofHelper: Token expiry status: {tokenHasExpired}; ticksInMinutes: {ticksInMinutes} and headerValidityPeriod: {this.headerValidityTimePeriod}");

            return tokenHasExpired;
        }

        /// <summary>
        /// Validates information coming in from a WOPI client
        /// </summary>
        /// <param name="validationPack">Contains information about the request to validate</param>
        /// <returns>True if the requests originates from the expected WOPI client</returns>
        public bool Validate(WopiProofValidationPack validationPack)
        {
            Contract.CheckValue(validationPack, "The validationPack instance should be provided");
            Contract.Check(validationPack.TimeStamp > 0, "The timestamp should be valid");
            Contract.CheckNonEmpty(validationPack.Url, nameof(validationPack.Url), "The URL should be provided");
            Contract.CheckNonEmpty(validationPack.AccessToken, nameof(validationPack.AccessToken), "The AccessToken should be provided");
            Contract.CheckNonEmpty(validationPack.CurrentHeaderProof, nameof(validationPack.CurrentHeaderProof), "The CurrentProof should be provided");
            Contract.CheckNonEmpty(validationPack.OldHeaderProof, nameof(validationPack.OldHeaderProof), "The OldProof should be provided");

            if (this.IsTokenExpired(validationPack.TimeStamp))
            {
                this.trace.TraceInformation("WopiProofHelper: Expired timestamp; validation pack is invalid");
                return false;
            }

            // Encode values from headers into byte[]
            var accessTokenBytes = Encoding.UTF8.GetBytes(validationPack.AccessToken);
            var hostUrlBytes = Encoding.UTF8.GetBytes(validationPack.Url.ToUpperInvariant());
            var timeStampBytes = EncodeInt64Number(validationPack.TimeStamp);

            // prepare a list that will be used to combine all those arrays together
            var expectedProof = new List<byte>(
                4 + accessTokenBytes.Length +
                4 + hostUrlBytes.Length +
                4 + timeStampBytes.Length);

            expectedProof.AddRange(EncodeInt32Number(accessTokenBytes.Length));
            expectedProof.AddRange(accessTokenBytes);
            expectedProof.AddRange(EncodeInt32Number(hostUrlBytes.Length));
            expectedProof.AddRange(hostUrlBytes);
            expectedProof.AddRange(EncodeInt32Number(timeStampBytes.Length));
            expectedProof.AddRange(timeStampBytes);

            // create another byte[] from that list
            byte[] expectedProofArray = expectedProof.ToArray();

            this.trace.TraceInformation($"WopiProofHelper: Attempting to validate headers from caller");

            // validate it against current and old keys in proper combinations
            bool validationResult =
                TryVerification(expectedProofArray, validationPack.CurrentHeaderProof, validationPack.CurrentDiscoveryKey) ||
                TryVerification(expectedProofArray, validationPack.OldHeaderProof, validationPack.CurrentDiscoveryKey) ||
                TryVerification(expectedProofArray, validationPack.CurrentHeaderProof, validationPack.OldDiscoveryKey);

            this.trace.TraceInformation($"WopiProofHelper: Proof verification status: {validationResult}");

            return validationResult;
        }

        /// <summary>
        /// Gets the bytes for a 32-bit number; reverses to avoid little ENDIAN nature of intel cores
        /// </summary>
        /// <param name="value">The number to get the bytes for</param>
        /// <returns>Byte array</returns>
        private static byte[] EncodeInt32Number(int value)
        {
            Contract.Check(value >= 0, "The value should be >= 0");

            return BitConverter.GetBytes(value).Reverse().ToArray();
        }

        /// <summary>
        /// Gets the bytes for a 64-bit number; reverses to avoid little ENDIAN nature of intel cores
        /// </summary>
        /// <param name="value">The number to get the bytes for</param>
        /// <returns>Byte array</returns>
        private static byte[] EncodeInt64Number(long value)
        {
            Contract.Check(value >= 0, "The value should be >= 0");

            return BitConverter.GetBytes(value).Reverse().ToArray();
        }

        /// <summary>
        /// Verifies a proof against a CSP blob.
        /// </summary>
        /// <param name="expectedProof">The proof constructed based off of <see cref="WopiProofValidationPack"/> values.</param>
        /// <param name="signedProof">The proof coming from the request header</param>
        /// <param name="publicKeyCspBlob">The public key coming from the client's hosted discovery</param>
        /// <returns>True if valid</returns>
        private static bool TryVerification(
            byte[] expectedProof,
            string signedProof,
            string publicKeyCspBlob)
        {
            Contract.CheckValue(expectedProof, "The expectedProof should be provided");
            Contract.CheckNonEmpty(signedProof, nameof(signedProof), "The signedProof should be provided");
            Contract.CheckNonEmpty(publicKeyCspBlob, nameof(publicKeyCspBlob), "The publicKeyCspBlob should be provided");

            using (RSACryptoServiceProvider rsaAlg = new RSACryptoServiceProvider())
            {
                byte[] publicKey = Convert.FromBase64String(publicKeyCspBlob);
                byte[] signedProofBytes = Convert.FromBase64String(signedProof);
                try
                {
                    rsaAlg.ImportCspBlob(publicKey);
                    return rsaAlg.VerifyData(expectedProof, "SHA256", signedProofBytes);
                }
                catch (FormatException)
                {
                    return false;
                }
                catch (CryptographicException)
                {
                    return false;
                }
            }
        }
    }
}
