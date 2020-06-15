using System.Collections.Generic;

namespace FeatureDBPortal.Shared
{
    public class CellDTO<T>
    {
        public T Data { get; set; }
    }

    public class MatrixDTO<T>
    {
        public CellDTO<T>[,] Cells { get; set; }
    }

    public class CombinationMatrixDTO<T>
    {
        public IEnumerable<string> RowNames { get; set; }
        public IEnumerable<string> ColumnNames { get; set; }

        public MatrixDTO<T> Matrix { get; set; }
    }
}
