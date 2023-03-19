using System;
using Microsoft.VisualBasic;

namespace TetrisRPS;

public class BlockQueue
{
    private readonly Block[] blocks = new Block[]
    {
        new IBlock(),
        new JBlock(),
        new LBlock(),
        new OBlock(),
        new SBlock(),
        new TBlock(),
        new ZBlock()
    };
    private readonly Random random = new();

    public Block NextBlock { get; private set; }
    int curBlockID = 0;
    public BlockQueue()
    {
        NextBlock = RandomBlock();
    }
    private Block RandomBlock()
    {
        curBlockID++;
        curBlockID %= blocks.Length;
        return blocks[curBlockID].GetType().GetConstructor(Type.EmptyTypes).Invoke(null) as Block;
    }

    public Block GetAndReplaceNextBlock()
    {
        Block block = NextBlock;
        do
        {
            NextBlock = RandomBlock();
        } while (block.Id == NextBlock.Id);
        return block;
    }

}