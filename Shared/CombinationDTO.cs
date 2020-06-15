using System.Collections;
using System.Collections.Generic;

namespace FeatureDBPortal.Shared
{
    public class ColumnDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; }
    }

    public class CombinationDTO
    {
        public string Description { get; set; }
        public bool Allow { get; set; }
        public GroupByDTO GroupBy { get; set; }
        public IEnumerable<ColumnDTO> Columns { get; set; }
        public IEnumerable<CombinationDTO> Combinations { get; set; }
    }
}
