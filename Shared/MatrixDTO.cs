//using System.Collections.Generic;
//using System.Linq;

//namespace FeatureDBPortal.Shared
//{
//    public class MatrixDTO<T>
//    {
//        private MatrixDTO()
//        {
//        }

//        public MatrixDTO(int x = 1, int y = 1)
//        {
//            Create(x, y);
//        }

//        T[][] _cells { get; set; }
//        public T this[int x, int y]
//        {
//            get { return _cells[x][y]; }
//            set { _cells[x][y] = value; }
//        }

//        private void Create(int x = 1, int y = 1)
//        {
//            _cells = new T[x][];

//            for (var row = 0; row < x; row++)
//            {
//                _cells[row] = new T[y];
//            }
//        }
//    }

//    public class CombinationMatrixDTO<T>
//    {
//        public CombinationMatrixDTO(int x = 1, int y = 1)
//        {
//            Matrix = new MatrixDTO<T>(x, y);
//        }

//        public IEnumerable<string> RowNames { get; set; }
//        public IEnumerable<string> ColumnNames { get; set; }

//        public MatrixDTO<T> Matrix { get; set; }
//    }

//    //public class CellDTO
//    //{
//    //    public bool? Allow { get; set; }

//    //    public int? RowId { get; set; }
//    //    public int? ColumnId { get; set; }
//    //}

//    //public class ColumnDTO
//    //{
//    //    public bool Allow { get; set; }

//    //    public int ColumnId { get; set; }
//    //}

//    //public class RowDTO
//    //{
//    //    public Dictionary<int?, CellDTO> Cs { get; set; } = new Dictionary<int?, CellDTO>();

//    //    public IEnumerable<ColumnDTO> Columns { get; set; }
//    //}

//    //public class MatrixDTO
//    //{
//    //    public Dictionary<int?, RowDTO> Rs { get; set; } = new Dictionary<int?, RowDTO>();
//    //    public IEnumerable<RowDTO> Rows { get; set; }
//    //}

//    //public class Combination
//    //{
//    //    public IEnumerable<IQueryableCombination> RowHeaders { get; set; }
//    //    public IEnumerable<IQueryableCombination> ColumnHeaders { get; set; }

//    //    public MatrixDTO Matrix { get; set; }
//    //}
//}
