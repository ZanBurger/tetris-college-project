using System.ComponentModel.Design;
using System.Windows.Ink;

namespace TetrisRPS;

public class GameState {
    public Block currentBlock { get; set; }

    public Block CurrentBlock {
        get => currentBlock;
        private set {
            currentBlock = value;
            currentBlock.Reset();
        }
    }
    public GameGrid GameGrid { get; }
    public BlockQueue BlockQueue { get; }
    
    public bool IsGameOver { get; private set; }
    public Block HeldBlock { get; private set; }
    public bool CanHold { get; private set; }
    
    public GameState() {
        GameGrid = new GameGrid(22,10);
        BlockQueue = new BlockQueue();
        CurrentBlock = BlockQueue.GetAndReplaceNextBlock();
        CanHold = true;
    }
    // Holding the block
    public void HoldBlock() {
        if (!CanHold) {
            return;
        }
        if (HeldBlock == null)
        {
            HeldBlock = CurrentBlock;
            CurrentBlock = BlockQueue.GetAndReplaceNextBlock();
        }
        else {
            Block tmp = CurrentBlock;
            CurrentBlock = HeldBlock;
            HeldBlock = tmp;
        }
        CanHold = false;
    }


     // If any block is out of bounds, it will return true.
    private bool IsBlockOutOfBounds() {
        foreach (Position position in CurrentBlock.TilePositions()) {
            if(!GameGrid.IsEmpty(position.Row, position.Column)) {
                return true;
            }
        }
        return false;
    }
    // If the block is out of bounds, it will rotate it back.
    public void RotateBlockCW() {
        CurrentBlock.RotateCW();
        if (IsBlockOutOfBounds()) {
            CurrentBlock.RotateCCW();
        }
    }
    public void RotateBlockCCW() {
        CurrentBlock.RotateCCW();
        if (IsBlockOutOfBounds()) {
            CurrentBlock.RotateCW();
        }
    }
    // If the block is out of bounds, it will move it back.
    public void MoveBlockLeft() {
        CurrentBlock.Move(0, -1);
        if (IsBlockOutOfBounds()) {
            CurrentBlock.Move(0, 1);
        }
    }
    public void MoveBlockRight() {
        CurrentBlock.Move(0, 1);
        if (IsBlockOutOfBounds()) {
            CurrentBlock.Move(0, -1);
        }
    }
    private bool IsGameOverCheck() {
        return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
    }
    
    private void PlaceBlock() {
        foreach (Position position in CurrentBlock.TilePositions()) {
            GameGrid[position.Row, position.Column] = CurrentBlock.Id; 
        }
        GameGrid.ClearFullRows();
        if (IsGameOverCheck()) {
            IsGameOver = true;
        }
        else {
            currentBlock = BlockQueue.GetAndReplaceNextBlock();
            CanHold = true;
        }
    }
    public void MoveBlockDown() {
        CurrentBlock.Move(1, 0);
        if (IsBlockOutOfBounds()) {
            CurrentBlock.Move(-1, 0);
            PlaceBlock();
        }
    }

    private int TileDropDistance(Position p) {
        int drop = 0;

        while (GameGrid.IsEmpty(p.Row + drop + 1, p.Column)) {
            drop++;
        }
        return drop;
    }

    public int BlockDropDistance() {
        int drop = GameGrid.Rows;
        foreach (Position p in CurrentBlock.TilePositions())
        {
            drop = System.Math.Min(drop, TileDropDistance(p));
        }
        return drop;
    }
    public void DropBlock() {
        CurrentBlock.Move(BlockDropDistance(), 0);
        PlaceBlock();
    }
}