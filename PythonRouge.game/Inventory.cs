using RLNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonRouge.game
{
    public class Inventory
    {
        public int selected = 0;
        public List<List<Vector2>> selections = new List<List<Vector2>> { new List<Vector2> { new Vector2(0, 0), new Vector2(1, 0), new Vector2(2, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(2, 1), new Vector2(0, 2), new Vector2(1, 2), new Vector2(2, 2) },
                                                                          new List<Vector2> { new Vector2(2, 0), new Vector2(3, 0), new Vector2(4, 0), new Vector2(2, 1), new Vector2(3, 1), new Vector2(4, 1), new Vector2(2, 2), new Vector2(3, 2), new Vector2(4, 2) },
                                                                          new List<Vector2> { new Vector2(4, 0), new Vector2(5, 0), new Vector2(6, 0), new Vector2(4, 1), new Vector2(5, 1), new Vector2(6, 1), new Vector2(4, 2), new Vector2(5, 2), new Vector2(6, 2) },
                                                                          new List<Vector2> { new Vector2(6, 0), new Vector2(7, 0), new Vector2(8, 0), new Vector2(6, 1), new Vector2(7, 1), new Vector2(8, 1), new Vector2(6, 2), new Vector2(7, 2), new Vector2(8, 2) },
                                                                          new List<Vector2> { new Vector2(8, 0), new Vector2(9, 0), new Vector2(10, 0), new Vector2(8, 1), new Vector2(9, 1), new Vector2(10, 1), new Vector2(8, 2), new Vector2(9, 2), new Vector2(10, 2) },
                                                                          new List<Vector2> { new Vector2(0, 2), new Vector2(1, 2), new Vector2(2, 2), new Vector2(0, 3), new Vector2(1, 3), new Vector2(2, 3), new Vector2(0, 4), new Vector2(1, 4), new Vector2(2, 4) },
                                                                          new List<Vector2> { new Vector2(2, 2), new Vector2(3, 2), new Vector2(4, 2), new Vector2(2, 3), new Vector2(3, 3), new Vector2(4, 3), new Vector2(2, 4), new Vector2(3, 4), new Vector2(4, 4) },
                                                                          new List<Vector2> { new Vector2(4, 2), new Vector2(5, 2), new Vector2(6, 2), new Vector2(4, 3), new Vector2(5, 3), new Vector2(6, 3), new Vector2(4, 4), new Vector2(5, 4), new Vector2(6, 4) },
                                                                          new List<Vector2> { new Vector2(6, 2), new Vector2(7, 2), new Vector2(8, 2), new Vector2(6, 3), new Vector2(7, 3), new Vector2(8, 3), new Vector2(6, 4), new Vector2(7, 4), new Vector2(8, 4) },
                                                                          new List<Vector2> { new Vector2(8, 2), new Vector2(9, 2), new Vector2(10, 2), new Vector2(8, 3), new Vector2(9, 3), new Vector2(10, 3), new Vector2(8, 4), new Vector2(9, 4), new Vector2(10, 4) },};
        private RLConsole console;
        private char[,] charMap = new char[5,11] { { '+', '-', '+', '-', '+', '-', '+', '-', '+', '-', '+' }, 
                                                   { '|', ' ', '|', ' ', '|', ' ', '|', ' ', '|', ' ', '|' },
                                                   { '+', '-', '+', '-', '+', '-', '+', '-', '+', '-', '+' },
                                                   { '|', ' ', '|', ' ', '|', ' ', '|', ' ', '|', ' ', '|' },
                                                   { '+', '-', '+', '-', '+', '-', '+', '-', '+', '-', '+' }};


        public Inventory(RLConsole console)
        {
            this.console = console;
            //selections.Add(new List<Vector2> );
        }
        public void Draw()
        {
            console.Print(0, 0, "Inventory:", Colours.Inventory, RLColor.Black);
            for(int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    console.Set(j, i + 1, Colours.Inventory, RLColor.Black, charMap[i, j]);
                }
            }
            foreach (Vector2 p in selections[selected])
            {
                console.Set(p.X, p.Y + 1, Colours.Inventory_sel, RLColor.Black, charMap[p.Y, p.X]);
            }
        }
        public void Move(int d)
        {
            if (((selected + d) < 0) || ((selected + d) > 9)) return;
            else selected += d;
        }        


    }
    
}
