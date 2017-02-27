// CSharpRouge Copyright (C) 2017 Tom Pinnock
// 
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
// warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more
//  details.
// 
// You should have received a copy of the GNU General Public License along with this program. If not, see
// http://www.gnu.org/licenses/.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonRouge.game
{
    public class Node
    {
        public Node parent;
        public Vector2 pos;
        public float H;
        public float G;
        public float f
        {
            get
            {
                if (H != -1 && G != -1)
                    return H + G;
                else
                    return -1;
            }
        }
        public bool canWalk;

        public Node(Vector2 pos, bool canWalk)
        {
            parent = null;
            this.pos = pos;
            H = -1;
            G = 1;
            this.canWalk = canWalk;
        }
    }

        public class Astar
        {
            List<List<Node>> Grid;
        int GridRows
            {
                get
                {
                    return Grid[0].Count;
                }
            }
            int GridCols
            {
                get
                {
                    return Grid.Count;
                }
            }

            public Astar(List<List<Node>> grid)
            {
                Grid = grid;
            }

        public Astar(Dictionary<Vector2, Tile> game_map, int w, int h)
        {
            Grid = new List<List<Node>>();
            for (int y = 0; y < h; y++)
            {
                Grid.Add(new List<Node>());
                for (int x=0; x < w; x++)
                {
                    var pos = new Vector2(x, y);
                    switch(game_map[pos].type)
                    {
                        case TileType.Empty:
                            Grid[y].Add(new Node(pos, false));
                            break;
                        case TileType.Floor:
                            Grid[y].Add(new Node(pos, true));
                            break;
                        case TileType.Wall:
                            Grid[y].Add(new Node(pos, false));
                            break;
                    }
                }
            }
        }

        public Stack<Node> FindPath(Vector2 Start, Vector2 End)
            {
                Node start = new Node(new Vector2(Start.X, Start.Y), true);
                Node end = new Node(new Vector2(End.X, End.Y), true);

                Stack<Node> Path = new Stack<Node>();
                List<Node> OpenList = new List<Node>();
                List<Node> ClosedList = new List<Node>();
                List<Node> adjacencies;
                Node current = start;

                // add start node to Open List
                OpenList.Add(start);

                while (OpenList.Count != 0 && !ClosedList.Exists(x => x.pos == end.pos))
                {
                    current = OpenList[0];
                    OpenList.Remove(current);
                    ClosedList.Add(current);
                    adjacencies = GetAdjacentNodes(current);


                    foreach (Node n in adjacencies)
                    {
                        if (!ClosedList.Contains(n) && n.canWalk)
                        {
                            if (!OpenList.Contains(n))
                            {
                                n.parent = current;
                                n.H = Math.Abs(n.pos.X - end.pos.X) + Math.Abs(n.pos.Y - end.pos.Y);
                                n.G = 1 + n.parent.G;
                                OpenList.Add(n);
                                OpenList = OpenList.OrderBy(node => node.f).ToList<Node>();
                            }
                        }
                    }
                }

                // construct path, if end was not closed return null
                if (!ClosedList.Exists(x => x.pos == end.pos))
                {
                    return null;
                }

                // if all good, return path
                Node temp = ClosedList[ClosedList.IndexOf(current)];
                while (temp.parent != start && temp != null)
                {
                    Path.Push(temp);
                    temp = temp.parent;
                }
                return Path;
            }

            private List<Node> GetAdjacentNodes(Node n)
            {
                List<Node> temp = new List<Node>();

                int row = (int)n.pos.Y;
                int col = (int)n.pos.X;

                if (row + 1 < GridRows)
                {
                    temp.Add(Grid[col][row + 1]);
                }
                if (row - 1 >= 0)
                {
                    temp.Add(Grid[col][row - 1]);
                }
                if (col - 1 >= 0)
                {
                    temp.Add(Grid[col - 1][row]);
                }
                if (col + 1 < GridCols)
                {
                    temp.Add(Grid[col + 1][row]);
                }

                return temp;
            }
        }

    }

