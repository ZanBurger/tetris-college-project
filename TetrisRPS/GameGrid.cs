namespace TetrisRPS{
    public class GameGrid{
        private readonly int[,] _grid;
        public int Rows { get; }
        public int Columns { get; }
    
        // Easy access to the grid values.
        public int this[int row, int column]{
            get => _grid[row, column];
            set => _grid[row, column] = value;
        }
        //So it could be made for differently sized grids.
        public GameGrid(int rows, int columns){
            Rows = rows;
            Columns = columns;
            _grid = new int[rows, columns];
        }
        public bool IsInsideGrid(int row, int column){
            return row >= 0 && row < Rows && column >= 0 && column < Columns;
        }
        // Must be inside array and the value must be 0. -> all empty values are 0.
        public bool IsEmpty(int row, int column){
            return IsInsideGrid(row, column) && _grid[row, column] == 0;
        }
        
        //Goes trough the entire row, if it isn't all zeroes it returns false.
        public bool IsRowFull(int row){
            for(int column = 0; column < Columns; column++){
                if(_grid[row, column] == 0){
                    return false;
                }
            }
            return true;
        }
        //Goes trough the entire row, if it isn't all zeroes it returns false.
        public bool IsRowEmpty(int row){
            for(int column = 0; column < Columns; column++){
                if(_grid[row, column] != 0){
                    return false;
                }
            }
            return true;
        }
        //Set entire row to 0.
        private void ClearRow(int row){
            for(int column = 0; column < Columns; column++){
                _grid[row, column] = 0;
            }
        }
        private void MoveRowDown(int row,int numOfRows){
            for(int column = 0; column < Columns; column++){
               _grid[row + numOfRows, column] = _grid[row, column];
               _grid[row, column] = 0;
            }
        }
        public int ClearFullRows(){
            int clearedRows = 0;
            for(int row = 0; row < Rows; row++){
                if(IsRowFull(row)){
                    ClearRow(row);
                    clearedRows++;
                }
            }
            for(int row = 0; row < Rows; row++){
                if(IsRowEmpty(row)){
                    MoveRowDown(row, clearedRows);
                }
            }
            return clearedRows;
        }
    }
}

