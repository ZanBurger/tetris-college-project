using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisRPS
{
    internal class Tetris
    {
        public int[,] array = new int[10, 20];
        // public int holding;
        // public int next;
        public int button;
        public void ButtonPress(int butt) 
        {
            button = butt;
        }
        
        public void Tick()
        {

        }
    }
}
