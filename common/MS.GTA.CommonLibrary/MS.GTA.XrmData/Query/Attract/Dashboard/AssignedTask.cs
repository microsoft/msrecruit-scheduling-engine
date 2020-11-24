
namespace MS.GTA.XrmData.Query.Attract.Dashboard
{
    using System;

    public enum AssignedTaskType
    {
        /// <summary>Job application activity</summary>
        JobApplicationActivity = 0,

        /// <summary>Job approval</summary>
        JobApproval = 1,
    }

    public class AssignedTask
    {
        public Guid? RecId { get; set; }

        public AssignedTaskType AssignedTaskType { get; set; }

        public DateTime? Date { get; set; }
    }
}
