//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace CommonLibrary.Common.Base.Calendar
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// The serializer class for calendar format.
    /// </summary>
    public class CalendarSerializer : ISerializer
    {
        /// <summary>
        /// Serialize template to calendar file format.
        /// </summary>
        /// <typeparam name="T">Generic type T</typeparam>
        /// <param name="t">Calendar file template.</param>
        /// <returns>String representation of calendar file format.</returns>
        public string Serialize<T>(T t) where T : CalendarTemplate
        {
            var sb = new StringBuilder();
            sb.AppendLine("BEGIN:VCALENDAR")
                .AppendLine("PRODID:-//Microsoft Corporation//EN")
                .AppendLine("VERSION:2.0")
                .AppendLine("METHOD:PUBLISH")
                .AppendLine("X-MS-OLK-FORCEINSPECTOROPEN:TRUE")
                .AppendLine("BEGIN:VEVENT")
                .AppendLine("CLASS:PUBLIC");

            var properties = t.GetType().GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(t).ToString();
                if (!string.IsNullOrEmpty(value))
                {
                    sb.AppendLine(property.Name.ToUpper() + ":" + value);
                }
            }

            sb.AppendLine("TRANSP:OPAQUE")
              .AppendLine("UID:" + Guid.NewGuid())
              .AppendLine("X-MICROSOFT-CDO-BUSYSTATUS:BUSY")
              .AppendLine("X-MICROSOFT-CDO-IMPORTANCE:1")
              .AppendLine("BEGIN:VALARM")
              .AppendLine("TRIGGER:-PT5M")
              .AppendLine("ACTION:DISPLAY")
              .AppendLine("DESCRIPTION:Reminder")
              .AppendLine("END:VALARM")
              .AppendLine("END:VEVENT")
              .AppendLine("END:VCALENDAR");
            return sb.ToString();
        }
    }
}
