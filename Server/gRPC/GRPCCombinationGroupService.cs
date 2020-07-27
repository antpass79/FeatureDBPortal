//using FeatureDBPortal.Server.Data.Models.RD;
//using FeatureDBPortal.Server.Extensions;
//using GrpcCombination;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace FeatureDBPortal.Server.gRPC
//{
//    public abstract class GRPCCombinationGroupService : IGRPCCombinationGroupService
//    {
//        protected FeaturesContext Context { get; }

//        protected GRPCCombinationGroupService(FeaturesContext context)
//        {
//            Context = context;
//        }

//        public abstract Task<CombinationGRPC> Combine(CombinationSearchGRPC search, IEnumerable<LayoutType> groupBy);

//        protected IEnumerable<NormalRule> FilterNormalRules(CombinationSearchGRPC search)
//        {
//            return Context
//                .NormalRule
//                    .WhereIf(item => item.LogicalModelId == search.Model.Id || !item.LogicalModelId.HasValue, search.Model != null)
//                    .WhereIf(item => item.CountryId == search.Country.Id || !item.CountryId.HasValue, search.Country != null)
//                    .WhereIf(item => item.UserLevel == (short)search.UserLevel || !item.UserLevel.HasValue, search.UserLevel != UserLevelGRPC.None)
//                    .WhereIf(item => item.ProbeId == search.Probe.Id || !item.ProbeId.HasValue, search.Probe != null)
//                    .WhereIf(item => item.KitId == search.Kit.Id || !item.KitId.HasValue, search.Kit != null)
//                    .WhereIf(item => item.OptionId == search.Option.Id || !item.OptionId.HasValue, search.Option != null)
//                    .WhereIf(item => item.ApplicationId == search.Application.Id || !item.ApplicationId.HasValue, search.Application != null);
//        }
//    }
//}
