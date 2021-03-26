//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Talent.TalentContracts.TaskService
{
    /// <summary>The job status.</summary>
    public static class JobStatus
    {
        /// <summary>The completed.</summary>
        public const string Completed = "completed";

        /// <summary>The abandon.</summary>
        public const string Abandon = "abandon";

        /// <summary>The failed.</summary>
        public const string Failed = "failed";

        /// <summary>The faulted.</summary>
        public const string Faulted = "faulted";

        /// <summary>The not queued.</summary>
        public const string NotQueued = "notQueued";

        /// <summary>The not started.</summary>
        public const string NotStarted = "notStarted";

        /// <summary>The queued.</summary>
        public const string Queued = "queued";

        /// <summary>The running.</summary>
        public const string Running = "running";

        /// <summary>The scheduled.</summary>
        public const string Scheduled = "scheduled";
    }
}
