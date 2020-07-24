using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Shared;
using FeatureDBPortal.Shared.Utilities;
using System;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Builders
{
    public class CombinationIndexerBuilder
    {
        private IReadOnlyList<QueryableEntity> _rows;
        private IReadOnlyList<QueryableEntity> _columns;
        private string _title;

        public CombinationIndexerBuilder Rows(IReadOnlyList<QueryableEntity> rows)
        {
            _rows = rows;
            return this;
        }

        public CombinationIndexerBuilder Columns(IReadOnlyList<QueryableEntity> columns)
        {
            _columns = columns;
            return this;
        }

        public CombinationIndexerBuilder Title(string title)
        {
            _title = title;
            return this;
        }

        public CombinationIndexer Build()
        {
            using var watcher = new Watcher("COMBINATION INDEXER BUILDER --> BUILD");

            CheckConsistency();

            return BuildCombination(_rows, _columns, _title);
        }

        private void CheckConsistency()
        {
            if (_rows == null)
                throw new InvalidOperationException($"{nameof(_rows)} not set");
            if (_columns == null)
                throw new InvalidOperationException($"{nameof(_columns)} not set");
        }

        CombinationIndexer BuildCombination(IReadOnlyList<QueryableEntity> rows, IReadOnlyList<QueryableEntity> columns, string title)
        {
            Dictionary<int?, int> rowIdToIndexMapper = new Dictionary<int?, int>();
            Dictionary<int?, int> columnIdToIndexMapper = new Dictionary<int?, int>();

            var combination = new CombinationDTO { IntersectionTitle = title };
            var newRows = new List<RowDTO>();

            var rowIndex = 0;
            foreach (var row in rows)
            {
                rowIdToIndexMapper.Add(row.Id, rowIndex);

                var newRow = new RowDTO
                {
                    RowId = row.Id,
                    Title = new RowTitleDTO
                    {
                        Id = row.Id,
                        Name = row.Name
                    }
                };

                var newCells = new List<CellDTO>();
                foreach (var column in columns)
                {
                    var newCell = new CellDTO
                    {
                        RowId = row.Id,
                        ColumnId = column.Id,
                    };

                    newCells.Add(newCell);
                }
                newRow.Cells = newCells;

                newRows.Add(newRow);

                rowIndex++;
            }

            int columnIndex = 0;
            var newColumns = new List<ColumnDTO>();
            foreach (var column in columns)
            {
                columnIdToIndexMapper.Add(column.Id, columnIndex);
                var newColumn = new ColumnDTO
                {
                    Id = column.Id,
                    Name = column.Name
                };

                newColumns.Add(newColumn);

                columnIndex++;
            }

            combination.Rows = newRows;
            combination.Columns = newColumns;

            return new CombinationIndexer(combination, rowIdToIndexMapper, columnIdToIndexMapper);
        }
    }
}
