//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace Common.Base.Calendar
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// The calendar file template.
    /// </summary>
    public class CalendarTemplate
    {
        /// <summary>Gets or sets the event created time.</summary>
        public string Created { get; set; }

        /// <summary>Gets or sets the event description.</summary>
        public string Description { get; set; }

        /// <summary>Gets or sets the event end time.</summary>
        public string DtEnd { get; set; }

        /// <summary>Gets or sets the event set time.</summary>
        public string DtStamp { get; set; }

        /// <summary>Gets or sets the event start time.</summary>
        public string DtStart { get; set; }

        /// <summary>Gets or sets the event last modified time.</summary>
        public string LastModified { get; set; }

        /// <summary>Gets or sets the event location.</summary>
        public string Location { get; set; }

        /// <summary>Gets or sets the event priority.</summary>
        public string Priority { get; set; }

        /// <summary>Gets or sets the summary.</summary>
        public string Summary { get; set; }

        /// <summary>Gets or sets the alternate description.</summary>
        public string AltDesc { get; set; }
    }
}
