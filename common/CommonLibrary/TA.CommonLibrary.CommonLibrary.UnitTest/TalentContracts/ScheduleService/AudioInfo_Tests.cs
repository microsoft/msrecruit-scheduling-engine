//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.UnitTest.TalentContracts.ScheduleService
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Graph;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TA.CommonLibrary.Talent.TalentContracts.ScheduleService.Conferencing;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class AudioInfo_Tests
    {
        [TestMethod]
        public void OperatiorWithNullAudioConferencing()
        {
            AudioConferencing audioConferencing = null;
            AudioInfo audioInfo = audioConferencing;
            Assert.IsNull(audioInfo);
        }

        [TestMethod]
        public void OperatiorWithValidAudioConferencing()
        {
            AudioConferencing audioConferencing = new AudioConferencing
            {
                TollFreeNumber = "12345",
                TollNumber = "09876",
                ConferenceId = "ABCD",
                DialinUrl = "https://someUrl"
            };

            AudioInfo audioInfo = audioConferencing;

            Assert.IsTrue(audioInfo.DialInUrl.Equals(audioConferencing.DialinUrl, StringComparison.Ordinal));
            Assert.IsTrue(audioInfo.TollFreeNumber.Equals(audioConferencing.TollFreeNumber, StringComparison.Ordinal));
            Assert.IsTrue(audioInfo.TollNumber.Equals(audioConferencing.TollNumber, StringComparison.Ordinal));
            Assert.IsTrue(audioInfo.ConferenceId.Equals(audioConferencing.ConferenceId, StringComparison.Ordinal));
        }
    }
}
