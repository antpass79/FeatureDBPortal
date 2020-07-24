using FeatureDBPortal.Shared;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Builders
{
    public class CombinationIndexer
    {
        private readonly Dictionary<int?, int> _rowIdToIndexMapper = new Dictionary<int?, int>();
        private readonly Dictionary<int?, int> _columnIdToIndexMapper = new Dictionary<int?, int>();

        public CombinationIndexer(CombinationDTO combination, Dictionary<int?, int> rowIdToIndexMapper, Dictionary<int?, int> columnIdToIndexMapper)
        {
            Combination = combination;

            _rowIdToIndexMapper = rowIdToIndexMapper;
            _columnIdToIndexMapper = columnIdToIndexMapper;
        }

        public CombinationDTO Combination { get; }

        public RowDTO this[int? rowId]
        {
            get { return Combination
                    .Rows[_rowIdToIndexMapper[rowId]]; }
        }
        public CellDTO this[int? rowId, int? columnId]
        {
            get { return Combination
                    .Rows[_rowIdToIndexMapper[rowId]]
                    .Cells[_columnIdToIndexMapper[columnId]]; }
        }
    }
}
