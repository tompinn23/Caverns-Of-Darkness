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
    public class PathNode
    {
        public PathNode parent;
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
        
        public PathNode(Vector2 pos, bool canWalk)
        {
            parent = null;
            this.pos = pos;
            H = -1;
            G = 1;
            this.canWalk = canWalk;
        }

        public class Astar
        {
            Dictionary<Vector2, PathNode> grid;
            int height;
            int width;

            public Astar(Dictionary<Vector2, Tile> map, int width, int height)
            {
                foreach(KeyValuePair<Vector2, Tile> kvp in map)
                {
                    if(kvp.Value.type != TileType.Floor)
                    {
                        grid.Add(kvp.Key, new PathNode(kvp.Key, false));
                    }
                    else
                    {
                        grid.Add(kvp.Key, new PathNode(kvp.Key, true));
                    }
                }
                this.height = height;
                this.width = width;
            }

            public Stack<Vector2> CalcPath(Vector2 Start, Vector2 End)
            {
                var start = new PathNode(Start, true);
                var end = new PathNode(End, true);
                Stack<Vector2> Path = new Stack<Vector2>();
                List<PathNode> open = new List<PathNode>();
                List<PathNode> closed = new List<PathNode>();
                List<PathNode> neighbours;
                var current = start;

                open.Add(start);
                while (open.Count != 0 && !closed.Exists(x => x.pos == end.pos))
                {
                    current = open[0];
                    open.Remove(current);
                    closed.Add(current);
                    neighbours = GetNeighbours(current);

                    foreach (PathNode node in neighbours)
                    {
                        if (!closed.Contains(node) && node.canWalk)
                        {
                            if(!open.Contains(node))
                            { 
                                node.parent = current;
                                node.H = Math.Abs(node.pos.X - end.pos.X) + Math.Abs(node.pos.Y - end.pos.Y);
                                node.G = 1 + node.parent.G;
                                open.Add(node);
                                open = open.OrderBy(nod => nod.f).ToList<PathNode>();
                            }
                        }
                    }
                }
                if (!closed.Exists(x => x.pos== end.pos))
                {
                    return null;
                }
                PathNode path = closed[closed.IndexOf(current)];
                while(path.parent != start && path != null)
                {
                    Path.Push(path.pos);
                    path = path.parent;
                }
                return Path;
            }

            public List<PathNode> GetNeighbours(PathNode n)
            {
                List<PathNode> neighbours = new List<PathNode>();
                if(n.pos.X + 1 < height)
                {
                    neighbours.Add(grid[new Vector2(n.pos.X + 1, n.pos.Y)]);
                }
                if (n.pos.X - 1 >= 0)
                {
                    neighbours.Add(grid[new Vector2(n.pos.X - 1, n.pos.Y)]);
                }
                if (n.pos.Y + 1 < width)
                {
                    neighbours.Add(grid[new Vector2(n.pos.X + 1, n.pos.Y)]);
                }
                if (n.pos.Y - 1 >= 0)
                {
                    neighbours.Add(grid[new Vector2(n.pos.X + 1, n.pos.Y)]);
                }
                return neighbours;
            }
        }

}

}
