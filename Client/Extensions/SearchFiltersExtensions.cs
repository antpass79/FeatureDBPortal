using FeatureDBPortal.Client.Models;
using FeatureDBPortal.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Extensions
{
    public static class SearchFiltersExtensions
    {
        public static bool IsOutputLayoutTypeSelected(this SearchFilters searchFilters, LayoutTypeDTO layoutType)
        {
            return searchFilters.RowLayout == layoutType || searchFilters.ColumnLayout == layoutType || searchFilters.CellLayout == layoutType;
        }
    }
}
