//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.FalconData.ViewModelExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// enum extenstion
    /// </summary>
    internal static class EnumExtensions
    {
        /// <summary>
        /// Gets View Model Enum from Entity Enum
        /// </summary>
        /// <typeparam name="TEnum">Entity Enum Type</typeparam>
        /// <param name="entityEnum">Entity Enum</param>
        /// <returns>Enum</returns>
        public static TEnum ToContractEnum<TEnum>(this Enum entityEnum)
            where TEnum : struct, IConvertible
        {
            try
            {
                if (entityEnum == null)
                {
                    return default(TEnum);
                }

                var sourceType = entityEnum.GetType();
                var targetType = typeof(TEnum);

                var fieldInfo = targetType.GetFields();
                foreach (var member in fieldInfo)
                {
                    if (string.Equals(member.Name, entityEnum.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        return (TEnum)Enum.Parse(targetType, member.Name);
                    }
                }

                return default(TEnum);
            }
            catch (Exception)
            {
                return default(TEnum);
            }
        }
    }
}
