using FeatureDBPortal.Client.Models;
using FeatureDBPortal.Shared;

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
