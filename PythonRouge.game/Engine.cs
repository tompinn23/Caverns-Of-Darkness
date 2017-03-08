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
using EpPathFinding.cs;
using RLNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PythonRouge.game
{
    public class Engine
    {
        public RLConsole InvConsole = new RLConsole(11, 6);
        public Map map = new Map(70, 50);
        public BaseGrid searchgrid;
        public RLConsole MapConsole = new RLConsole(70, 50);
        public RLConsole SideConsole = new RLConsole(20, 56);
        public SidePanel side;
        public Inventory inv;
        public RLRootConsole rootConsole;
        public List<Entity> entityList = new List<Entity>();
        public List<Monster> monsterList = new List<Monster>();
        public Player player;

        public delegate void MonsterUpdateEventHandler(object sender, MonsterUpdateEventArgs e);
        public event MonsterUpdateEventHandler MonsterUpdate;

        public delegate void PlayerMoveEventHandler(object sender, PlayerMoveEventArgs e);
        public event PlayerMoveEventHandler PlayerMove;
        public Random mRnd = new Random();

        public bool checkEntity(Vector2 start, Vector2 end, int dx, int dy)
        {

            foreach (Entity e in entityList)
            {
                if ((start.X + dx) == end.X && (start.Y + dy) == end.Y)
                {
                    return true;
                }
            }
            return false;
        }


        public void ConstructGrid()
        {
            NodePool nodePool = new NodePool();
            searchgrid = new DynamicGridWPool(nodePool);
            foreach (Vector2 p in map.grid.Game_map.Keys)
            {
                if (map.grid.Game_map[p].type == TileType.Floor) searchgrid.SetWalkableAt(new GridPos(p.X, p.Y), true);
                else searchgrid.SetWalkableAt(new GridPos(p.X, p.Y), false);
            }
        }

        public void render()
        {
            PreRender();
            RLConsole.Blit(MapConsole, 0, 0, 70, 50, rootConsole, 0, 0);
            RLConsole.Blit(InvConsole, 0, 0, 11, 6, rootConsole, 0, 50);
            RLConsole.Blit(SideConsole, 0, 0, 21, 96, rootConsole, 71, 0);
            PostRender();
        }

        public void renderMap()
        {
            var game_map = map.grid.Game_map;
            foreach (var kvp in game_map)
            {
                var pos = kvp.Key;
                var tile = kvp.Value;
                switch (tile.type)
                {
                    case TileType.Floor:
                        if (tile.lit)
                            MapConsole.Set(pos.X, pos.Y, Colours.floor_lit, RLColor.Black, tile.symbol);
                        else
                            MapConsole.Set(pos.X, pos.Y, Colours.floor, RLColor.Black, tile.symbol);
                        break;
                    case TileType.Wall:
                        if (tile.lit)
                            MapConsole.Set(pos.X, pos.Y, Colours.wall_lit, RLColor.Black, tile.symbol);
                        else
                            MapConsole.Set(pos.X, pos.Y, Colours.wall, RLColor.Black, tile.symbol);
                        break;
                    case TileType.Empty:
                        MapConsole.Set(pos.X, pos.Y, RLColor.Black, RLColor.Black, tile.symbol);
                        break;
                    case TileType.PathTest:
                        MapConsole.Set(pos.X, pos.Y, RLColor.Yellow, RLColor.Yellow, tile.symbol);
                        break;
                }
            }
        }
        public virtual void PreRender()
        {
            renderMap();
            inv?.Draw();
            side?.Draw();
        }

        public virtual void PostRender()
        {

        }

        public virtual void HandleKey(RLKeyPress keyPress, Player player)
        {
            switch (keyPress.Key)
            {
                case RLKey.W:
                    {
                        if (!map.canMove(player.pos, 0, -1)) return;
                        map.resetLight();
                        player.move(0, -1);
                        ShadowCast.ComputeVisibility(map.grid, player.pos, 7.5f, player.name);
                        OnPlayerMove(new PlayerMoveEventArgs { playerPos = player.pos, engine = this });
                    }
                    break;
                case RLKey.S:
                    {
                        if (!map.canMove(player.pos, 0, 1)) return;
                        map.resetLight();
                        player.move(0, 1);
                        var pos = new Vector2(player.pos.X, player.pos.Y);
                        ShadowCast.ComputeVisibility(map.grid, pos, 7.5f, player.name);
                        OnPlayerMove(new PlayerMoveEventArgs { playerPos = player.pos, engine = this });
                    }
                    break;
                case RLKey.A:
                    {
                        if (!map.canMove(player.pos, -1, 0)) return;
                        map.resetLight();
                        player.move(-1, 0);
                        var pos = new Vector2(player.pos.X, player.pos.Y);
                        ShadowCast.ComputeVisibility(map.grid, pos, 7.5f, player.name);
                        OnPlayerMove(new PlayerMoveEventArgs { playerPos = player.pos, engine = this });
                    }
                    break;
                case RLKey.D:
                    {
                        if (!map.canMove(player.pos, 1, 0)) return;
                        map.resetLight();
                        player.move(1, 0);
                        var pos = new Vector2(player.pos.X, player.pos.Y);
                        ShadowCast.ComputeVisibility(map.grid, pos, 7.5f, player.name);
                        OnPlayerMove(new PlayerMoveEventArgs { playerPos = player.pos, engine = this });
                    }
                    break;
                case RLKey.Space:
                    {
                        player.attack();
                    }
                    break;
                case RLKey.Comma:
                    {
                        inv?.Move(-1);
                    }
                    break;
                case RLKey.Period:
                    {
                        inv?.Move(1);
                    }
                    break;
                default:
                    break;
            }
        }
        protected virtual void OnPlayerMove(PlayerMoveEventArgs e)
        {
            PlayerMove?.Invoke(this, e);
        }

        protected virtual void OnMonsterUpdate(MonsterUpdateEventArgs e)
        {
            MonsterUpdate?.Invoke(this, e);
        }

    }
}

