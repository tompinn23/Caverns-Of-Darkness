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
using RLNET;
using System.Threading;
using System.Threading.Tasks;

namespace PythonRouge.game
{
    public class SPEngine
    {
        private readonly RLConsole invConsole = new RLConsole(20, 70);
        private readonly Map map = new Map(70, 50);

        private readonly RLConsole mapConsole = new RLConsole(70, 50);

        private readonly Player player = new Player(new Vector2(0, 0), '@', 100, "Tom");
        private readonly RLRootConsole rootConsole;

        private bool mapLoadDone = false;

        public SPEngine(RLRootConsole rootConsole)
        {
            this.rootConsole = rootConsole;
            mapConsole.SetBackColor(0, 0, 70, 50, RLColor.Blue);
            invConsole.SetBackColor(0, 0, 20, 70, RLColor.Cyan);
            mapGenerate();
            do
            {
                rootConsole.Clear();
                mapConsole.Clear();
                mapConsole.Print(0, 0, "Loading Map",RLColor.White);
                RLConsole.Blit(mapConsole, 0, 0, 70, 50, rootConsole, 0, 10);
                rootConsole.Draw();
                Thread.Sleep(200);
                mapConsole.Print(0, 0, "Loading Map.", RLColor.White);
                RLConsole.Blit(mapConsole, 0, 0, 70, 50, rootConsole, 0, 10);
                rootConsole.Draw();
                Thread.Sleep(200);
                mapConsole.Print(0, 0, "Loading Map..", RLColor.White);
                RLConsole.Blit(mapConsole, 0, 0, 70, 50, rootConsole, 0, 10);
                rootConsole.Draw();
                Thread.Sleep(200);
                mapConsole.Print(0, 0, "Loading Map...", RLColor.White);
                RLConsole.Blit(mapConsole, 0, 0, 70, 50, rootConsole, 0, 10);
                rootConsole.Draw();

            } while(mapLoadDone == false);
            Pathfinding.CalcMovement(new Vector2(8, 4), new Vector2(14, 30), map.grid.Game_map);
            var pos = map.findPPos();
            player.pos = pos;
        }

        public void render()
        {
            PreRender();
            RLConsole.Blit(mapConsole, 0, 0, 70, 50, rootConsole, 0, 10);
            RLConsole.Blit(invConsole, 0, 0, 20, 70, rootConsole, 70, 0);
            PostRender();
        }

        public async void mapGenerate()
        {
            await doMapGenerate();
            mapLoadDone = true;
        } 

        public Task doMapGenerate()
        {
            return Task.Run(() =>
            {
                map.generate();

            });
            
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
                            mapConsole.Set(pos.X, pos.Y, Colours.floor_lit, Colours.floor_lit, tile.symbol);
                        else
                            mapConsole.Set(pos.X, pos.Y, Colours.floor, Colours.floor, tile.symbol);
                        break;
                    case TileType.Wall:
                        if (tile.lit)
                            mapConsole.Set(pos.X, pos.Y, Colours.wall_lit, Colours.wall_lit, tile.symbol);
                        else
                            mapConsole.Set(pos.X, pos.Y, Colours.wall, Colours.wall, tile.symbol);
                        break;
                    case TileType.Empty:
                        mapConsole.Set(pos.X, pos.Y, RLColor.Black, RLColor.Black, tile.symbol);
                        break;
                }
            }
        }


        public void PreRender()
        {
            renderMap();
            player.draw(mapConsole);
        }

        public void PostRender()
        {
            player.clear(mapConsole);
        }


        public void handleKey(RLKeyPress keyPress)
        {
            if (keyPress.Key == RLKey.Up)
            {
                if (map.canMove(player.pos, 0, -1))
                {
                    map.resetLight();
                    player.move(0, -1);
                    var pos = new Vector2(player.pos.X, player.pos.Y);
                    ShadowCast.ComputeVisibility(map.grid, pos, 7.5f);
                }
            }
            else if (keyPress.Key == RLKey.Down)
            {
                if (map.canMove(player.pos, 0, 1))
                {
                    map.resetLight();
                    player.move(0, 1);
                    var pos = new Vector2(player.pos.X, player.pos.Y);
                    ShadowCast.ComputeVisibility(map.grid, pos, 7.5f);
                }
            }
            else if (keyPress.Key == RLKey.Left)
            {
                if (map.canMove(player.pos, -1, 0))
                {
                    map.resetLight();
                    player.move(-1, 0);
                    var pos = new Vector2(player.pos.X, player.pos.Y);
                    ShadowCast.ComputeVisibility(map.grid, pos, 7.5f);
                }
            }
            else if (keyPress.Key == RLKey.Right)
            {
                if (map.canMove(player.pos, 1, 0))
                {
                    map.resetLight();
                    player.move(1, 0);
                    var pos = new Vector2(player.pos.X, player.pos.Y);
                    ShadowCast.ComputeVisibility(map.grid, pos, 7.5f);
                }
            }
        }
    }
}