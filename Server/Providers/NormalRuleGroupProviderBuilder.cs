using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Extensions;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Shared;
using System.Linq;

namespace FeatureDBPortal.Server.Providers
{
    public class NormalRuleGroupProviderBuilder
    {
        private readonly FeaturesContext _context;

        private LayoutTypeDTO _groupByOne = LayoutTypeDTO.None;
        private LayoutTypeDTO _groupByTwo = LayoutTypeDTO.None;
        private LayoutTypeDTO _groupByThree = LayoutTypeDTO.None;

        public NormalRuleGroupProviderBuilder(FeaturesContext context)
        {
            _context = context;
        }

        public NormalRuleGroupProviderBuilder GroupByOne(LayoutTypeDTO groupType)
        {
            _groupByOne = groupType;
            return this;
        }
        public NormalRuleGroupProviderBuilder GroupByTwo(LayoutTypeDTO groupType)
        {
            _groupByTwo = groupType;
            return this;
        }
        public NormalRuleGroupProviderBuilder GroupByThree(LayoutTypeDTO groupType)
        {
            _groupByThree = groupType;
            return this;
        }

        public INormalRuleGroupProvider Build()
        {
            var rowGroupProperties = BuildProperties(_groupByOne);
            var columnGroupProperties = BuildProperties(_groupByTwo);
            var cellGroupProperties = BuildProperties(_groupByThree);

            INormalRuleGroupProvider normalRuleProvider = null;
            if (rowGroupProperties != null && columnGroupProperties == null && cellGroupProperties == null)
                normalRuleProvider = new NormalRuleGroupByOneProvider(rowGroupProperties);
            else if (rowGroupProperties != null && columnGroupProperties != null && cellGroupProperties == null)
                normalRuleProvider = new NormalRuleGroupByTwoProvider(rowGroupProperties, columnGroupProperties);
            else if (rowGroupProperties != null && columnGroupProperties != null && cellGroupProperties != null)
                normalRuleProvider = new NormalRuleGroupByThreeProvider(rowGroupProperties, columnGroupProperties, cellGroupProperties);

            return normalRuleProvider;
        }

        private NormalRuleGroupProperties BuildProperties(LayoutTypeDTO groupType)
        {
            if (groupType == LayoutTypeDTO.None)
                return null;

            var items = _context.GetPropertyValue<IQueryable<IQueryableCombination>>(groupType.ToTableName())
                .AsEnumerable();

            var groupableItems = items
                .Where(item => !item.IsFake)
                .Select(item => new QueryableCombination { Id = item.Id, Name = item.Name })
                .OrderBy(item => item.Name)
                .ToList();

            var discardItemIds = items
                .Where(item => item.IsFake)
                .Select(item => item.Id)
                .ToList();

            return new NormalRuleGroupProperties(groupType, groupableItems, discardItemIds);
        }
    }
}
