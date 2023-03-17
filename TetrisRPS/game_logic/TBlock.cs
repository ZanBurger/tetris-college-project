﻿namespace TetrisRPS;

public class TBlock : Block{
    private readonly Position[][] tiles = new Position[][]
    {
        new Position[] { new(0, 1), new(1, 0), new(1, 1), new(1, 2) },
        new Position[] { new(0, 1), new(1, 1), new(1, 2), new(2, 1) },
        new Position[] { new(1, 0), new(1, 1), new(1, 2), new(2, 1) },
        new Position[] { new(0, 1), new(1, 0), new(1, 1), new(2, 1) }
    };
    public override Position StartOffset => new(0,3);
    public override int Id  => 6;
    protected override Position[][] Tiles => tiles;
}