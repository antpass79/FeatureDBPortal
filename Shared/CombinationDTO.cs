using System.Collections;
using System.Collections.Generic;

namespace FeatureDBPortal.Shared
{
    //public class ColumnDTO
    //{
    //    public int? Id { get; set; }
    //    public string Name { get; set; }
    //}

    //public class CombinationDTO
    //{
    //    public string Description { get; set; }
    //    public bool Allow { get; set; }
    //    public GroupByDTO GroupBy { get; set; }
    //    public IEnumerable<ColumnDTO> Columns { get; set; }
    //    public IEnumerable<CombinationDTO> Combinations { get; set; }
    //}

    public class CellDTO
    {
        public string Name { get; set; }
        public bool? Allow { get; set; }

        public int? RowId { get; set; }
        public int? ColumnId { get; set; }
    }

    public class MatrixRowDTO
    {
        public string Name { get; set; }
        public Dictionary<int?, CellDTO> Columns { get; set; } = new Dictionary<int?, CellDTO>();
    }

    public class RowDTO
    {
        public CellDTO TitleCell { get; set; }
        public IList<CellDTO> Cells { get; set; }
    }

    public class MatrixDTO
    {
        public Dictionary<int?, MatrixRowDTO> Rows { get; set; } = new Dictionary<int?, MatrixRowDTO>();
    }

    public class TitleDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; }
    }

    public class CombinationDTO
    {
        public IEnumerable<TitleDTO> Headers { get; set; }

        public IList<RowDTO> Rows { get; set; }

        public GroupByDTO GroupBy { get; set; }
        public MatrixDTO Matrix { get; set; }
    }
}
