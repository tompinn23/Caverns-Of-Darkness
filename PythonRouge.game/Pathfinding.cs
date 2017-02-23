﻿// CSharpRouge Copyright (C) 2017 Tom Pinnock
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
    static class Pathfinding
    {
        static List<Vector2> offsets = new List<Vector2> { new Vector2(-1, 0), new Vector2(-1, -1), new Vector2(-1, 1), new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, -1), new Vector2(1, 0), new Vector2(1, 1) };
        static List<Vector2> CalcMovement(Vector2 start, Vector2 end, Dictionary<Vector2, Tile> grid)
        {
            List<Vector2> open = new List<Vector2>();
            
            open.Add(start);
            return new List<Vector2>();
        }
    }
}
