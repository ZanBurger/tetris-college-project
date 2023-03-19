

using System.Collections.Generic;

namespace TetrisRPS;

public abstract class Block
{
    protected abstract Position[][] Tiles { get; }

    public abstract Position StartOffset { get; }

    public abstract int Id { get; }

    private int rotationState;
    private Position offset;

    public Block()
    {
        offset = new Position(StartOffset.Row, StartOffset.Column);
    }

    public IEnumerable<Position> TilePositions()
    {
        foreach (var tile in Tiles[rotationState])
        {
            yield return new Position(tile.Row + offset.Row, tile.Column + offset.Column);
        }
    }

    public void RotateCW()
    {
        rotationState = (rotationState + 1) % Tiles.Length;
    }
    public void RotateCCW()
    {
        if (rotationState == 0)
        {
            rotationState = Tiles.Length - 1;
        }
        else
        {
            rotationState--;
        }
    }
    //Move by x rows and y columns.
    public void Move(int row, int column)
    {
        offset.Row += row;
        offset.Column += column;
    }

    public void Reset()
    {
        rotationState = 0;
        offset.Row = StartOffset.Row;
        offset.Column = StartOffset.Column;
    }
}