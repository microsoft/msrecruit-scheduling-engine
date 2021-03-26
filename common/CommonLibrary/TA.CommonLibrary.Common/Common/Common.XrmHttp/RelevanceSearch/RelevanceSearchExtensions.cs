//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.XrmHttp.RelevanceSearch
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    public static class RelevanceSearchExtensions
    {
        public static T CopySearchHighlights<T>(this T xrmODataEntity, List<RelevanceSearchHighlight> searchHighlights, Guid? id = null) where T : XrmODataEntity
        {
            if (xrmODataEntity != null && searchHighlights != null)
            {

                foreach (var highlight in searchHighlights.Where(h => h.EntityType == ODataEntityContractInfo.GetEntitySingularName(typeof(T))))
                {
                    var propInfo = typeof(T)
                                    .GetProperties()
                                    .Where(p => Attribute.IsDefined(p, typeof(DataMemberAttribute)))
                                    .SingleOrDefault(p => ((DataMemberAttribute)Attribute.GetCustomAttribute(
                                         p, typeof(DataMemberAttribute))).Name == highlight.Field);

                    if (propInfo != null && (!id.HasValue || id.Value == highlight.Id))
                    {
                        propInfo.SetValue(xrmODataEntity, highlight.ValueWithHighlight);
                    }
                }

            }
            return xrmODataEntity;
        }

        public static bool HasHighlightsForType<T>(this List<RelevanceSearchHighlight> highlights) where T : XrmODataEntity
        {
            return highlights?.Any(h => h.EntityType == ODataEntityContractInfo.GetEntitySingularName(typeof(T))) == true;
        }
    }
}
