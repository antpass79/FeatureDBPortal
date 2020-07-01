using FeatureDBPortal.Server.Tests.Models;
using FeatureDBPortal.Shared;
using System;
using System.Linq;
using Xunit;

namespace FeatureDBPortal.Server.Tests.Utils
{
    public static class CombinationAssert
    {
        public static void Null(CombinationDTO result)
        {
            Assert.Null(result.Columns);
            Assert.Null(result.Rows);
        }
        public static void Empty(CombinationDTO result)
        {
            Assert.Empty(result.Columns);
            Assert.Empty(result.Rows);
        }

        public static void AllEqual(ExpectedResult expectedResult, CombinationDTO result)
        {
            HeaderEqual(expectedResult, result);
            RowEqual(expectedResult, result);
            CellEqual(expectedResult, result);
            ItemEqual(expectedResult, result);
        }

        public static void HeaderEqual(ExpectedResult expectedResult, CombinationDTO result)
        {
            Assert.Equal(expectedResult.ExpectedHeaderCount, result.Columns.Count());

            if (expectedResult.ExpectedHeaders != null)
            {
                expectedResult.ExpectedHeaders.ToList().ForEach(expectedHeader =>
                {
                    var header = result.Columns.Single(item => expectedResult.FilterBy == FindBy.Id ? item.Id == expectedHeader.ForId : item.Name == expectedHeader.ExpectedName);
                    Assert.Equal(expectedHeader.ExpectedName, header.Name);
                });
            }
        }

        public static void RowEqual(ExpectedResult expectedResult, CombinationDTO result)
        {
            Assert.Equal(expectedResult.ExpectedRowCount, result.Rows.Count());

            if (expectedResult.ExpectedRows != null)
            {
                expectedResult.ExpectedRows.ToList().ForEach(expectedRow =>
                {
                    var row = result.Rows.Single(item => expectedResult.FilterBy == FindBy.Id ? item.RowId == expectedRow.ForRowId : item.Title.Name == expectedRow.ExpectedName);
                    Assert.Equal(expectedRow.ExpectedName, row.Title.Name);
                });
            }
        }

        public static void CellEqual(ExpectedResult expectedResult, CombinationDTO result)
        {
            if (expectedResult.ExpectedCells != null)
            {
                expectedResult.ExpectedCells.ToList().ForEach(expectedCell =>
                {
                    var row = result.Rows.Single(item => expectedResult.FilterBy == FindBy.Id ? item.RowId == expectedCell.ForRowId : item.Title.Name == expectedCell.ForRowName);
                    var cell = row.Cells.Single(item => expectedResult.FilterBy == FindBy.Id ? item.ColumnId == expectedCell.ForColumnId : item.Name == expectedCell.ForColumnName);
                    Assert.Equal(expectedCell.ExpectedAllowMode, cell.AllowMode);
                    Assert.Equal(expectedCell.ExpectedVisibility, cell.Visible);
                    Assert.Equal(expectedCell.ExpectedAvailability, cell.Available);
                });
            }
        }

        public static void ItemEqual(ExpectedResult expectedResult, CombinationDTO result)
        {
            if (expectedResult.ExpectedItems != null)
            {
                expectedResult.ExpectedItems.ToList().ForEach(expectedItem =>
                {
                    var row = result.Rows.Single(item => expectedResult.FilterBy == FindBy.Id ? item.RowId == expectedItem.ForRowId : item.Title.Name == expectedItem.ForRowName);
                    var cell = row.Cells.Single(item => expectedResult.FilterBy == FindBy.Id ? item.ColumnId == expectedItem.ForColumnId : item.Name == expectedItem.ForColumnName);
                    var item = cell.Items.Single(item => item.ItemId == expectedItem.ForItemId);

                    Assert.Equal(expectedItem.ExpectedName, item.Name);
                });
            }
        }
    }
}
