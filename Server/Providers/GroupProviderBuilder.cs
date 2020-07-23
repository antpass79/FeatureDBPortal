﻿using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Extensions;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Server.Services;
using FeatureDBPortal.Shared;
using System.Linq;

namespace FeatureDBPortal.Server.Providers
{
    public class GroupProviderBuilder
    {
        private readonly FeaturesContext _context;
        private readonly IAllowModeProvider _allowModeProvider;
        private readonly IFilterCache _filterCache;

        private LayoutTypeDTO _groupByOne = LayoutTypeDTO.None;
        private LayoutTypeDTO _groupByTwo = LayoutTypeDTO.None;
        private LayoutTypeDTO _groupByThree = LayoutTypeDTO.None;

        public GroupProviderBuilder(
            FeaturesContext context,
            IAllowModeProvider allowModeProvider,
            IFilterCache filterCache)
        {
            _context = context;
            _allowModeProvider = allowModeProvider;
            _filterCache = filterCache;
        }

        public GroupProviderBuilder GroupByOne(LayoutTypeDTO groupType)
        {
            _groupByOne = groupType;
            return this;
        }
        public GroupProviderBuilder GroupByTwo(LayoutTypeDTO groupType)
        {
            _groupByTwo = groupType;
            return this;
        }
        public GroupProviderBuilder GroupByThree(LayoutTypeDTO groupType)
        {
            _groupByThree = groupType;
            return this;
        }

        public IGroupProvider Build()
        {
            var rowGroupProperties = BuildGroupProperties(_groupByOne);
            var columnGroupProperties = BuildGroupProperties(_groupByTwo);
            var cellGroupProperties = BuildGroupProperties(_groupByThree);

            IGroupProvider groupProvider = null;
            if (rowGroupProperties == null && columnGroupProperties == null && cellGroupProperties == null)
                groupProvider = new GroupByAnyProvider(_allowModeProvider);
            if (rowGroupProperties != null && columnGroupProperties == null && cellGroupProperties == null)
                groupProvider = new GroupByOneProvider(rowGroupProperties, _allowModeProvider);
            else if (rowGroupProperties != null && columnGroupProperties != null && cellGroupProperties == null)
                groupProvider = new GroupByTwoProvider(rowGroupProperties, columnGroupProperties, _allowModeProvider);
            else if (rowGroupProperties != null && columnGroupProperties != null && cellGroupProperties != null)
                groupProvider = new GroupByThreeProvider(rowGroupProperties, columnGroupProperties, cellGroupProperties, _allowModeProvider);

            return groupProvider;
        }

        private GroupProperties BuildGroupProperties(LayoutTypeDTO groupType)
        {
            if (groupType == LayoutTypeDTO.None)
                return null;

            var items = _filterCache.Get(groupType.ToTableName());

            var groupableItems = items
                .Where(item => !item.IsFake)
                .Select(item => new QueryableEntity { Id = item.Id.Value, Name = item.Name })
                .OrderBy(item => item.Name)
                .ToDictionary(item => item.Id);

            var discardItemIds = items
                .Where(item => item.IsFake)
                .Select(item => item.Id.Value)
                .ToList();

            return new GroupProperties(groupType, groupableItems, discardItemIds);
        }
    }
}
