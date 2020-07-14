using FeatureDBPortal.Server.Data.Models.RD;
using System;

namespace FeatureDBPortal.Server.Extensions
{
    public static class NormalRuleExtensions
    {
        static public int? GetPropertyIdByGroupNameId(this NormalRule normalRule, string groupName)
        {
            var result = groupName switch
            {
                "ApplicationId" => normalRule.ApplicationId,
                "ProbeId" => normalRule.ProbeId,
                "OptionId" => normalRule.OptionId,
                "LogicalModelId" => normalRule.LogicalModelId,
                "KitId" => normalRule.KitId,
                "CountryId" => normalRule.CountryId,
                "Version" => normalRule.Version,
                "UserLevel" => normalRule.UserLevel,
                _ => throw new NotSupportedException()
            };

            return result;
        }
    }
}
