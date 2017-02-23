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
    class MapGenerator
    {
        static Random rnd = new Random();
        int width;
        int height;
        int maxRoomTries;
        int minRoomSize = 5;
        int maxRoomSize = 100;
        List<Room> rooms = new List<Room>();

        public MapGenerator(int x, int y, int maxRoomTries)
        {
            width = x;
            height = y;
            this.maxRoomTries = maxRoomTries;
        }

        public Dictionary<Vector2, Tile> generateMap()
        {
            var tiles = new Dictionary<Vector2, Tile>();
            for(int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tiles[new Vector2(x, y)] = new Tile(x, y, TileType.Empty);
                }
            }
            placeRooms();
            foreach(Vector2 pos in tiles.Keys)
            {
                foreach(Room room in rooms)
                {
                    if (room.isInside(pos)) tiles[pos].type = TileType.Floor;                    
                }
            }
            return tiles;
        }

        public void placeRooms()
        {
            for (int i = 0; i <= maxRoomTries; i++)
            {
                var roomIntersects = false;
                var tempRoom = new Room(rnd.Next(width), rnd.Next(height), rnd.Next(minRoomSize, maxRoomSize), rnd.Next(minRoomSize, maxRoomSize));
                if (isValidPlacement(tempRoom))
                {
                    foreach (Room r in rooms)
                    {
                        if (tempRoom.intersects(r))
                        {
                            roomIntersects = true;
                            break;
                        }
                    }
                }
                else roomIntersects = true;
                if (!roomIntersects)
                {
                    rooms.Add(tempRoom);
                }
            }
        }

        public bool isValidPlacement(Room room)
        {
            if (room.x1 < 0 || room.y1 < 0 || room.x2 > width || room.y2 > height) return false;
            return true; 
        }
    }
    struct Room
    {
        public int x1;
        public int y1;
        public int w;
        public int h;
        public int x2;
        public int y2;
        public Room(int x, int y, int w, int h)
        {
            x1 = x;
            y1 = y;
            this.w = w;
            this.h = h;
            x2 = x1 + w;
            y2 = y1 + h;
        }
        public bool intersects(Room other)
        {
            if (other.x1 < this.x2 && other.x1 > this.x1 && other.y1 < this.y2 && other.y1 > this.y1) return true;
            else if (other.x2 > this.x1 && other.x2 < this.x2 && other.y2 > this.y1 && other.y2 < this.y2) return true;
            else if (other.x1 < this.x2 && other.x1 > this.x1 && other.y2 < this.y2 && other.y2 > this.y1) return true;
            else if (other.x2 < this.x2 && other.x1 > this.x1 && other.y1 < this.y2 && other.y1 > this.y1) return true;
            else return false;
        }
        public bool isInside(Vector2 pos)
        {
            if (pos.x > this.x1 && pos.x < this.x2 && pos.y > this.y1 && pos.y < this.y2) return true;
            else return false;
        }
    }
}
