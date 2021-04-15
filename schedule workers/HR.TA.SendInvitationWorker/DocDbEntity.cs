//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ScheduleWorker
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>The Document DB entity.</summary>
    [DataContract]
    [PartitionKeyPath("/Type")]
    [BackfillDBAndCollection(false)]
    public abstract class DocDbEntity
    {
        /// <summary>Gets or sets the id.</summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>Gets or sets the type.</summary>
        public string Type { get; set; }

        /// <summary>Gets or sets the OID that the document was created by.</summary>
        public string CreatedBy { get; set; }

        /// <summary>Gets or sets the OID that the document was updated by.</summary>
        public string UpdatedBy { get; set; }

        /// <summary>Gets or sets the created at.</summary>
        public long CreatedAt { get; set; }

        /// <summary>Gets or sets the updated at.</summary>
        public long UpdatedAt { get; set; }

        /// <summary>Gets or sets the created at.</summary>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>Gets or sets the updated at.</summary>
        public DateTime UpdatedDateTime { get; set; }
    }

    /// <summary>
    /// Attribute to specify the collection partition key path.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PartitionKeyPathAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="PartitionKeyPathAttribute"/> class.</summary>
        /// <param name="path">The path.</param>
        public PartitionKeyPathAttribute(string path)
        {
            this.Path = path;
        }

        /// <summary>Gets the path.</summary>
        public string Path { get; }
    }

    /// <summary>
    /// Attribute to indicate if the HCM document client will try to backfill the database and collection if it doesn't exist.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class BackfillDBAndCollectionAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="BackfillDBAndCollectionAttribute"/> class.</summary>
        /// <param name="backfill">Indicates if the HCM document client will try to backfill the database and collection if it doesn't exist.</param>
        public BackfillDBAndCollectionAttribute(bool backfill = false)
        {
            this.Backfill = backfill;
        }

        /// <summary>Gets a value indicating whether if the HCM document client will try to backfill the database and collection if it doesn't exist.</summary>
        public bool Backfill { get; }
    }
}
