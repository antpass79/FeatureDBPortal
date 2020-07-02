using AutoMapper;
using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Services;
using FeatureDBPortal.Shared;
using Grpc.Core;
using GrpcCombination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.gRPC
{
    public class CombinationService : Combiner.CombinerBase
    {
        private readonly FeaturesContext _context;
        private readonly IMapper _mapper;

        private readonly IGRPCCombinationGroupService _combinationGroupByAnyService;
        private readonly IGRPCCombinationGroupService _combinationGroupByOneService;
        private readonly IGRPCCombinationGroupService _combinationGroupByTwoService;
        private readonly IGRPCCombinationGroupService _combinationGroupByThreeService;

        public CombinationService(IMapper mapper, DbContext context)
        {
            _mapper = mapper;
            _context = context as FeaturesContext;

            _combinationGroupByAnyService = new GRPCCombinationGroupByAnyService(context);
            _combinationGroupByOneService = new GRPCCombinationGroupByOneService(context);
            _combinationGroupByTwoService = new GRPCCombinationGroupByTwoService(context);
            _combinationGroupByThreeService = new GRPCCombinationGroupByThreeService(context);
        }

        async public override Task<CombinationGRPC> GetCombination(CombinationSearchGRPC request, ServerCallContext context)
        {
            var start = DateTime.Now;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                IEnumerable<LayoutType> groupBy = GetGroups(request);
                CombinationGRPC combination = groupBy.Count() switch
                {
                    0 => await _combinationGroupByAnyService.Combine(request, groupBy),
                    1 => await _combinationGroupByOneService.Combine(request, groupBy),
                    2 => await _combinationGroupByTwoService.Combine(request, groupBy),
                    3 => await _combinationGroupByThreeService.Combine(request, groupBy),
                    _ => throw new NotImplementedException()
                };

                return combination;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                return await Task.FromResult<CombinationGRPC>(null);
            }
            finally
            {
                Trace.WriteLine(string.Empty);
                Trace.WriteLine($"Process starts at {start} and stops at {DateTime.Now} with duration of {stopwatch.Elapsed}");
            }
        }

        #region Private Functions

        private IEnumerable<LayoutType> GetGroups(CombinationSearchGRPC search)
        {
            List<LayoutType> groupBy = new List<LayoutType>();
            if (search.RowLayout != LayoutTypeGRPC.None && search.RowLayout != LayoutTypeGRPC.Unused)
            {
                groupBy.Add(_mapper.Map<LayoutType>(search.RowLayout));
            }
            if (search.ColumnLayout != LayoutTypeGRPC.None && search.RowLayout != LayoutTypeGRPC.Unused)
            {
                groupBy.Add(_mapper.Map<LayoutType>(search.ColumnLayout));
            }
            if (search.CellLayout != LayoutTypeGRPC.None && search.RowLayout != LayoutTypeGRPC.Unused)
            {
                groupBy.Add(_mapper.Map<LayoutType>(search.CellLayout));
            }

            return groupBy;
        }

        #endregion
    }
}