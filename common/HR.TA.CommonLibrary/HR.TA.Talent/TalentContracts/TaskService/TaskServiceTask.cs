//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.Talent.TalentContracts.TaskService
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text;


    /// <summary>The task service task.</summary>
    [DataContract]
    public class TaskServiceTask : SerializeableToJson
    {
        private string name;

        /// <summary>Determines where the service will callback on, either HTTPS or Fabric (prefered) urls (use this or SubscriptionName).</summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string CallbackUrl { get; set; }

        /// <summary>A timestamp of when the task was created.</summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public DateTime? CreatedDate { get; set; }

        /// <summary>A timestamp of when the task should be handled (service bus concept).</summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public DateTime? DeferUntil { get; set; }

        /// <summary>Unused: TBD to limit number of delivery attempts.</summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public int? DeliveryAttempt { get; set; }

        /// <summary>List of remote service error codes that will cause the task to be completed instead of failed.</summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public List<string> ExpectedRemoteServiceErrorCodes { get; set; } = new List<string>();

        /// <summary>The callback url to use if the task fails. Use this to handle any cleanup/reset logic. </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string FailureCallbackUrl { get; set; }

        /// <summary>The UTC datetime when the task was marked as 'completed' or 'failed'.</summary>
        [DataMember(Name = "finishedDate")]
        [Queryable("finishedDate")]
        public DateTime? FinishedDate { get; set; }

        /// <summary>Used in conjunction with CallbackUrl. Sets the headers that will be perisisted on callback to the service.</summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        /// <summary>A timestamp that indicates the last time the task was updated. Can be used for sorting tasks by time.</summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        [Queryable("Timestamp")]
        public DateTime? LastUpdated { get; set; }

        /// <summary>A name for the task for reference or when looking up the task in logs or storage.</summary>
        [DataMember]
        [Queryable("PartitionKey")]
        public string TaskName
        {
            get => string.IsNullOrEmpty(this.name) ? "unnamedTask" : this.name;
            set => this.name = value;
        }

        /// <summary>Id of the a parent task where the parent task will not be marked as "completed" until all available child tasks are completed.</summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        [Queryable("ParentTaskId")]
        public string ParentTaskId { get; set; }

        /// <summary>Key-value pair of properties available for usage, i.e tenantId, environmentId, ect.</summary>
        [DataMember(EmitDefaultValue = false)]
        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();

        /// <summary>The UTC datetime when the task was marked as 'running'.</summary>
        [DataMember(Name = "startedDate")]
        [Queryable("startedDate")]
        public DateTime? StartedDate { get; set; }

        /// <summary>The current status of the job ("completed", "failed", "faulted", "notQueued", "notStarted", "queued", "running", "scheduled")</summary>
        [DataMember(Name = "status")]
        [Queryable("JobStatus")]
        public string Status { get; set; } = JobStatus.NotStarted;

        /// <summary>The subscription to add the task to (use this or CallbackUrl).</summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        [Queryable("SubscriptionName")]
        public string SubscriptionName { get; set; }

        /// <summary>The sub task name. Used for reporting when there are multiple tasks within a larger parent task.</summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        [Queryable("SubTaskName")]
        public string SubTaskName { get; set; }

        /// <summary>Used in conjunction with CallbackUrl to determine which method to use when posting back to the service. Note: On POST the body will be the task service task.</summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string Method { get; set; }

        /// <summary>Used in conjunction with Subscriptions to determine how long to wait before retrying a task.</summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public int? OperationTimeout { get; set; }

        /// <summary>Gets or sets the number attempts to attempt the task.</summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public int? MaxDeliveryAttempts { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating if the task should always run in the same region.
        /// Use this to avoid scenarios where the task is queued in one region but handled in another.
        /// Example: Queued in westcentralus but handled in westus2.</summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public bool? RegionDependent { get; set; }

        /// <summary>Used by the service to indicate to the client the sequence of the task in the service bus.</summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public long SequenceNumber
        {
            get;

            // Set should only be used internally to set the sequence number when receiving from service bus.
            internal set;
        }

        /// <summary>Unique id for a given task.</summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        [Queryable("RowKey")]
        public string TaskId { get; set; }

        /// <summary>Used in conjunction with scheduled tasks to identify which scheduled task the task came from. Setable by the service.</summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        [Queryable("ScheduledTaskId")]
        public string ScheduledTaskId
        {
            get;

            // Set should only be used internally to set the id when receiving from service bus.
            internal set;
        }

        internal bool IsSystemTask { get; set; }

        public override string ToString()
        {
            return $"{this?.TaskName} - {this?.TaskId}";
        }

        /// <summary>The failure reason.</summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string FailureReason { get; set; }
    }
}
