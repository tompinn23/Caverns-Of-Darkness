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
using Karcero.Engine;
using Karcero.Engine.Models;
using System;
using System.Collections.Generic;

namespace PythonRouge.game
{
    public class Map
    {
        public GameGrid grid;
        public GameGrid mGrid;
        public List<Vector2> openTiles = new List<Vector2>();
        public List<Vector2> litTiles = new List<Vector2>();

        public Map(int width, int height, GameGrid grid = null)
        {
            mapHeight = height;
            mapWidth = width;
            if (grid == null)
            {
                this.grid = new GameGrid(width, height);
                this.mGrid = new GameGrid(width, height);
            }
            else
                this.grid = grid;
            fillMap();
        }

        public int mapWidth { get; set; }
        public int mapHeight { get; set; }

        public void fillMap()
        {
            for (var x = 0; x < mapWidth; x++)
            {
                for (var y = 0; y < mapHeight; y++)
                {
                    grid.Game_map[new Vector2(x, y)] = new Tile(x, y, TileType.Wall);
                    mGrid.Game_map[new Vector2(x, y)] = new Tile(x, y, TileType.Wall);
                }
            }
        }

        public Vector2 getDxDy(Vector2 start, Vector2 end)
        {
            int dx;
            int dy;
            int d1 = Math.Max(start.X, end.X) - Math.Min(start.X, end.X);
            int d2 = Math.Max(start.Y, end.Y) - Math.Min(start.Y, end.Y);
            if(start.X < end.X)
            {
                dx = d1;
            }
            else
            {
                dx = -d1;
            }
            if(start.Y < end.Y)
            {
                dy = d2;
            }
            else
            {
                dy = -d2;
            }
            return new Vector2(dx, dy);
        }


        public void resetLight()
        {
            foreach (var kvp in grid.Game_map)
                kvp.Value.lit = false;
        }

        public bool canMove(Vector2 pos, int dx, int dy)
        {
            try
            {
                if (grid.Game_map[new Vector2(pos.X + dx, pos.Y + dy)].type == TileType.Wall)
                {
                    return false;

                }
                return true;
            }
#pragma warning disable 0168
            catch (KeyNotFoundException e)
#pragma warning restore 0168
            {
                return false;
            }
        }

        public Vector2 findPPos()
        {
            foreach (var kvp in grid.Game_map)
                if (kvp.Value.type == TileType.Floor)
                    return new Vector2(kvp.Key.X, kvp.Key.Y);
            return new Vector2(0, 0);
        }

        public bool NeighboursIsNotFloor(Vector2 pos)
        {
            var offsets = new List<Vector2>
            {
                new Vector2(-1, -1),
                new Vector2(-1, 0),
                new Vector2(-1, 1),
                new Vector2(0, -1),
                new Vector2(0, 1),
                new Vector2(1, -1),
                new Vector2(1, 0),
                new Vector2(1, 1)
            };
            foreach (var offset in offsets)
                try
                {
                    if (grid.Game_map[new Vector2(pos.X + offset.X, pos.Y + offset.Y)].type ==
                        TileType.Floor)
                        return false;
                }
#pragma warning disable 0168
                catch (KeyNotFoundException e)
#pragma warning restore 0168
                {
                }
            return true;
        }

        public void setEmpty()
        {
            foreach (var kvp in grid.Game_map)
                if (NeighboursIsNotFloor(new Vector2(kvp.Key.X, kvp.Key.Y)))
                    kvp.Value.type = TileType.Empty;
            foreach (var kvp in mGrid.Game_map)
                if (NeighboursIsNotFloor(new Vector2(kvp.Key.X, kvp.Key.Y)))
                    kvp.Value.type = TileType.Empty;
        }

        public void generate()
        {
            var generator = new DungeonGenerator<Cell>();
            var map = generator.GenerateA()
                .DungeonOfSize(69, 49)
                .ABitRandom()
                .VerySparse()
                .WithBigChanceToRemoveDeadEnds()
                .RemoveAllDeadEnds()
                .WithRoomSize(3, 10, 3, 10)
                .WithRoomCount(35)
                .Now();
            foreach (var cell in map.AllCells)
            {
                var pos = new Vector2(cell.Column, cell.Row);
                switch (cell.Terrain)
                {
                    case TerrainType.Door:
                        grid.Game_map[pos].blocked = false;
                        grid.Game_map[pos].block_sight = false;
                        grid.Game_map[pos].type = TileType.Floor;
                        mGrid.Game_map[pos].blocked = false;
                        mGrid.Game_map[pos].block_sight = false;
                        mGrid.Game_map[pos].type = TileType.Floor;
                        break;
                    case TerrainType.Floor:
                        grid.Game_map[pos].blocked = false;
                        grid.Game_map[pos].block_sight = false;
                        grid.Game_map[pos].type = TileType.Floor;
                        mGrid.Game_map[pos].blocked = false;
                        mGrid.Game_map[pos].block_sight = false;
                        mGrid.Game_map[pos].type = TileType.Floor;
                        break;
                    case TerrainType.Rock:
                        grid.Game_map[pos].blocked = true;
                        grid.Game_map[pos].block_sight = true;
                        grid.Game_map[pos].type = TileType.Wall;
                        mGrid.Game_map[pos].blocked = false;
                        mGrid.Game_map[pos].block_sight = false;
                        mGrid.Game_map[pos].type = TileType.Floor;
                        break;
                }
            }
            setEmpty();
            Console.WriteLine(DateTime.Now);
            foreach(Vector2 p in grid.Game_map.Keys)
            {
                if (grid.Game_map[p].type == TileType.Floor) openTiles.Add(p);
            }
            Console.WriteLine(DateTime.Now);
        }
    
    public void getLitTiles()
    {
        
        foreach(Tile t in grid.Game_map.Values)
        {
            if(t.lit)
            {
                litTiles.Add(t.pos);
            }
        }
    }

    }

    [Serializable]
    public class GameGrid
    {
        private Dictionary<Vector2, Tile> game_map = new Dictionary<Vector2, Tile>();
        public int xDim;
        public int yDim;

        public GameGrid(int w, int h)
        {
            xDim = w;
            yDim = h;
        }

        public Dictionary<Vector2, Tile> Game_map { get { return game_map; } set { game_map  = value; } }

        public bool IsWall(int x, int y)
        {
            return Game_map[new Vector2(x, y)].type == TileType.Wall;
        }

        public void SetLight(int x, int y, float disSqrd, string discoveredby)
        {
            Game_map[new Vector2(x, y)].lit = true;
            Game_map[new Vector2(x, y)].discoveredby = discoveredby;
        }

        public string mapTostring()
        {
            string[] temp = new string[game_map.Count];
            int counter = 0;
            foreach (KeyValuePair<Vector2, Tile> kvp in Game_map)
            {
                string xy = kvp.Key.X.ToString() + "/" + kvp.Key.Y.ToString();
                string type = null;
                switch (kvp.Value.type)
                {
                        case TileType.Empty:
                            type = "Empty";
                            break;
                        case TileType.Floor:
                            type = "Floor";
                            break;
                        case TileType.Wall:
                            type = "Wall";
                            break;
                }
                string part = xy + ":" + type;
                temp[counter] = part;
                counter++;
            }
            return string.Join(",", temp);
        }

        public void mapFromString(string map)
        {
            string[] firstsep = map.Split(new char[] {','});
            foreach (string value in firstsep)
            {
                string[] secSep = value.Split(new char[] {':'});
                string[] key = secSep[0].Split(new char[] {'/'});
                var accKey = new Vector2(int.Parse(key[0]), int.Parse(key[1]));
                switch (secSep[1])
                {
                    case "Empty":
                        game_map[accKey].type = TileType.Empty;
                        game_map[accKey].block_sight = false;
                        game_map[accKey].blocked = false;
                        break;
                    case "Floor":
                        game_map[accKey].type = TileType.Floor;
                        game_map[accKey].block_sight = false;
                        game_map[accKey].blocked = false;
                        break;
                    case "Wall":
                        game_map[accKey].type = TileType.Wall;
                        game_map[accKey].block_sight = true;
                        game_map[accKey].blocked = true;
                        break;
                }
            }
        }

        public void Clear()
        {
            foreach(Tile t in Game_map.Values)
            {
                t.lit = false;
            }
        }
    }

    [Serializable]
    public struct Vector2
    {
        private int x;
        private int y;

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public static bool operator ==(Vector2 v1, Vector2 v2)
        {
            return v1.Equals(v2);
        }
        public static bool operator !=(Vector2 v1, Vector2 v2)
        {
            return !v1.Equals(v2);
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }    

    [Serializable]
    public class Tile
    {
        public char symbol
        {
            get
            {
                switch(type)
                {
                    case TileType.Empty:
                        return ' ';
                    case TileType.Floor:
                        return '.';
                    case TileType.Wall:
                        return '#';
                }
                return ' ';
            }
        }
        public Vector2 pos;
        public string discoveredby;

        public Tile(int x, int y, TileType type)
        {
            this.x = x;
            this.y = y;
            pos = new Vector2(x, y);
            switch (type)
            {
                case TileType.Floor:
                    blocked = false;
                    block_sight = false;
                    break;
                case TileType.Empty:
                    blocked = true;
                    block_sight = true;
                    break;
                case TileType.Wall:
                    blocked = true;
                    block_sight = true;
                    break;
            }
            lit = false;
            this.type = type;
        }

        public int x { get; set; }
        public int y { get; set; }
        public bool blocked { get; set; }
        public bool block_sight { get; set; }
        public bool lit { get; set; }
        public TileType type { get; set; }

        public void setType(TileType type)
        {
            switch (type)
            {
                case TileType.Floor:
                    blocked = false;
                    block_sight = false;
                    break;
                case TileType.Empty:
                    blocked = true;
                    block_sight = true;
                    break;
                case TileType.Wall:
                    blocked = true;
                    block_sight = true;
                    break;
            }
            this.type = type;
        }
        

    }

    public enum TileType
    {
        Floor,
        Wall,
        Empty,
        PathTest
    }
}   