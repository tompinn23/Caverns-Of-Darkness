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
        public RLConsole InvConsole = new RLConsole(20, 70);
        public Map map = new Map(70, 50);
        public BaseGrid searchgrid;
        public RLConsole MapConsole = new RLConsole(70, 50);

        public RLRootConsole rootConsole;

        public delegate void MonsterUpdateEventHandler(object sender, MonsterUpdateEventArgs e);
        public event MonsterUpdateEventHandler MonsterUpdate;

        public delegate void PlayerMoveEventHandler(object sender, PlayerMoveEventArgs e);
        public event PlayerMoveEventHandler PlayerMove;

       

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
            RLConsole.Blit(MapConsole, 0, 0, 70, 50, rootConsole, 0, 10);
            RLConsole.Blit(InvConsole, 0, 0, 20, 70, rootConsole, 70, 0);
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
                            MapConsole.Set(pos.X, pos.Y, Colours.floor_lit, Colours.floor_lit, tile.symbol);
                        else
                            MapConsole.Set(pos.X, pos.Y, Colours.floor, Colours.floor, tile.symbol);
                        break;
                    case TileType.Wall:
                        if (tile.lit)
                            MapConsole.Set(pos.X, pos.Y, Colours.wall_lit, Colours.wall_lit, tile.symbol);
                        else
                            MapConsole.Set(pos.X, pos.Y, Colours.wall, Colours.wall, tile.symbol);
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
        }

        public virtual void PostRender()
        {

        }

        public virtual void HandleKey(RLKeyPress keyPress, Player player)
        {
            switch (keyPress.Key)
            {
                case RLKey.Up:
                    {
                        if (!map.canMove(player.pos, 0, -1)) return;
                        map.resetLight();
                        player.move(0, -1);
                        ShadowCast.ComputeVisibility(map.grid, player.pos, 7.5f, player.name);
                        OnPlayerMove(new PlayerMoveEventArgs { playerPos = player.pos, engine = this });
                    }
                    break;
                case RLKey.Down:
                    {
                        if (!map.canMove(player.pos, 0, 1)) return;
                        map.resetLight();
                        player.move(0, 1);
                        var pos = new Vector2(player.pos.X, player.pos.Y);
                        ShadowCast.ComputeVisibility(map.grid, pos, 7.5f, player.name);
                        OnPlayerMove(new PlayerMoveEventArgs { playerPos = player.pos, engine = this });
                    }
                    break;
                case RLKey.Left:
                    {
                        if (!map.canMove(player.pos, -1, 0)) return;
                        map.resetLight();
                        player.move(-1, 0);
                        var pos = new Vector2(player.pos.X, player.pos.Y);
                        ShadowCast.ComputeVisibility(map.grid, pos, 7.5f, player.name);
                        OnPlayerMove(new PlayerMoveEventArgs { playerPos = player.pos, engine = this });
                    }
                    break;
                case RLKey.Right:
                    {
                        if (!map.canMove(player.pos, 1, 0)) return;
                        map.resetLight();
                        player.move(1, 0);
                        var pos = new Vector2(player.pos.X, player.pos.Y);
                        ShadowCast.ComputeVisibility(map.grid, pos, 7.5f, player.name);
                        OnPlayerMove(new PlayerMoveEventArgs { playerPos = player.pos, engine = this });
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

