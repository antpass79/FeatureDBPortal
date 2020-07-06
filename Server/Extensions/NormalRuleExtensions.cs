using FeatureDBPortal.Server.Data.Models.RD;
using System;

namespace FeatureDBPortal.Server.Extensions
{
    public static class NormalRuleExtensions
    {
        static public int? GetPropertyIdByGroupName(this NormalRule normalRule, string groupName)
        {
            var result = groupName switch
            {
                "Application" => normalRule.ApplicationId,
                "Probe" => normalRule.ProbeId,
                "Option" => normalRule.OptionId,
                "Model" => normalRule.LogicalModelId,
                "Kit" => normalRule.KitId,
                "Country" => normalRule.CountryId,
                _ => throw new NotSupportedException()
            };

            return result;
        }
    }
}
