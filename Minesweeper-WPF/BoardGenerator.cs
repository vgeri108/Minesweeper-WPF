using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper_WPF
{
    class BoardGenerator
    {
        private string[,] board = Data.board;
        private string[,] visible = Data.visible;
        private int meretM = Data.meretM;
        private int meretSZ = Data.meretSZ;
        private int aknakszama = Data.aknakszama;
        void Generate()
        {
            Random random = new Random();
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    board[i, j] = semmi;
                    visible[i, j] = "false";
                }
            }
            for (int i = 0; i < aknakszama; i++)
            {
                int x, y;
                do
                {
                    x = random.Next(0, meretM);
                    y = random.Next(0, meretSZ);
                } while ((board[x, y] != semmi) || (x == cursor_y && y == cursor_x));
                board[x, y] = minemark;
            }
        }
    }
}
