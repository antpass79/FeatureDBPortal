using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Shared;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Extensions
{
    public static class LayoutTypeDTOExtensions
    {
        static Dictionary<LayoutTypeDTO, string> _layoutTypeToTableNameMapping = new Dictionary<LayoutTypeDTO, string>();
        static Dictionary<LayoutTypeDTO, string> _layoutTypeToNormalRulePropertyNameMapping = new Dictionary<LayoutTypeDTO, string>();

        static LayoutTypeDTOExtensions()
        {
            FillLayoutTypeToTableNameMapping();
            FillLayoutTypeToNormalRulePropertyNameMapping();
        }

        public static string ToTableName(this LayoutTypeDTO layoutType)
        {
            return _layoutTypeToTableNameMapping[layoutType];
        }

        public static string ToNormalRulePropertyName(this LayoutTypeDTO layoutType)
        {
            return _layoutTypeToNormalRulePropertyNameMapping[layoutType];
        }

        public static string ToNormalRulePropertyNameId(this LayoutTypeDTO layoutType)
        {
            var extensionId = layoutType != LayoutTypeDTO.Version ? "Id" : string.Empty;
            return $"{_layoutTypeToNormalRulePropertyNameMapping[layoutType]}{extensionId}";
        }

        private static void FillLayoutTypeToTableNameMapping()
        {
            _layoutTypeToTableNameMapping.Add(LayoutTypeDTO.Application, nameof(Application));
            _layoutTypeToTableNameMapping.Add(LayoutTypeDTO.Country, nameof(Country));
            _layoutTypeToTableNameMapping.Add(LayoutTypeDTO.Feature, nameof(Feature));
            _layoutTypeToTableNameMapping.Add(LayoutTypeDTO.Kit, nameof(BiopsyKits));
            _layoutTypeToTableNameMapping.Add(LayoutTypeDTO.License, nameof(License));
            _layoutTypeToTableNameMapping.Add(LayoutTypeDTO.Model, nameof(LogicalModel));
            _layoutTypeToTableNameMapping.Add(LayoutTypeDTO.Option, nameof(Option));
            _layoutTypeToTableNameMapping.Add(LayoutTypeDTO.Probe, nameof(Probe));
            _layoutTypeToTableNameMapping.Add(LayoutTypeDTO.UserLevel, "UserLevel");
            _layoutTypeToTableNameMapping.Add(LayoutTypeDTO.Version, nameof(MinorVersionAssociation));
        }

        private static void FillLayoutTypeToNormalRulePropertyNameMapping()
        {
            _layoutTypeToNormalRulePropertyNameMapping.Add(LayoutTypeDTO.Application, "Application");
            _layoutTypeToNormalRulePropertyNameMapping.Add(LayoutTypeDTO.Country, "Country");
            _layoutTypeToNormalRulePropertyNameMapping.Add(LayoutTypeDTO.Feature, "Feature");
            _layoutTypeToNormalRulePropertyNameMapping.Add(LayoutTypeDTO.Kit, "Kit");
            _layoutTypeToNormalRulePropertyNameMapping.Add(LayoutTypeDTO.License, "License");
            _layoutTypeToNormalRulePropertyNameMapping.Add(LayoutTypeDTO.Model, "LogicalModel");
            _layoutTypeToNormalRulePropertyNameMapping.Add(LayoutTypeDTO.Option, "Option");
            _layoutTypeToNormalRulePropertyNameMapping.Add(LayoutTypeDTO.Probe, "Probe");
            _layoutTypeToNormalRulePropertyNameMapping.Add(LayoutTypeDTO.UserLevel, "UserLevel");
            _layoutTypeToNormalRulePropertyNameMapping.Add(LayoutTypeDTO.Version, "Version");
        }
    }
}