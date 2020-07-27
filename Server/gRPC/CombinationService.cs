using AutoMapper;
using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Services;
using FeatureDBPortal.Shared;
using FeatureDBPortal.Shared.Utilities;
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
        private readonly IAvailabilityCombinationService _availabilityCombinationService;

        public CombinationService(IAvailabilityCombinationService availabilityCombinationService)
        {
            _availabilityCombinationService = availabilityCombinationService;
        }

        async public override Task<CombinationGRPC> GetCombination(CombinationSearchGRPC request, ServerCallContext context)
        {
            var combination = await _availabilityCombinationService.Get(new CombinationSearchDTO
            {
                ModelId = request.ModelId,
                CountryId = request.CountryId,
                UserLevel = (UserLevelDTO)request.UserLevel,
                ApplicationId = request.ApplicationId,
                ProbeId = request.ProbeId,
                KitId = request.KitId,
                OptionId = request.OptionId,
                VersionId = request.VersionId,
                RowLayout = (LayoutTypeDTO)request.RowLayout,
                ColumnLayout = (LayoutTypeDTO)request.ColumnLayout,
                CellLayout = (LayoutTypeDTO)request.CellLayout
            });

            using var watcher = new Watcher("ON SERVER GRPC CONVERSION");

            var conversion = new CombinationGRPC { IntersectionTitle = combination.IntersectionTitle };
            conversion.Columns.AddRange(combination.Columns.Select(column => new ColumnGRPC
            {
                Id = column.Id,
                Name = column.Name
            }));
            conversion.Rows.AddRange(combination.Rows.Select(row =>
            {
                var newRow = new RowGRPC
                {
                    Title = new RowTitleGRPC { Id = row.Title.Id, Name = row.Title.Name },
                    RowId = row.RowId
                };

                newRow.Cells.AddRange(row.Cells.Select(cell =>
                {
                    var newCell = new CellGRPC
                    {
                        RowId = cell.RowId,
                        ColumnId = cell.ColumnId,
                        Visible = cell.Visible,
                        Available = cell.Available,
                        AllowMode = cell.AllowMode.HasValue ? (AllowModeGRPC)cell.AllowMode : AllowModeGRPC.AllowModeDef,
                    };

                    if (cell.Items != null)
                    {
                        newCell.Items.AddRange(cell.Items.Select(item => new CellItemGRPC
                        {
                            RowId = item.RowId,
                            ColumnId = item.ColumnId,
                            ItemId = item.ItemId,
                            Name = item.Name,
                            Allow = item.Allow
                        }));
                    }

                    return newCell;
                }));

                return newRow;
            }));

            return await Task.FromResult(conversion);
        }
    }
}