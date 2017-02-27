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
using System;
using System.Threading;
using System.Threading.Tasks;
using EpPathFinding.cs;
using System.Collections.Generic;

namespace PythonRouge.game
{
    public class SPEngine : Engine
    {
        private readonly RLConsole invConsole = new RLConsole(20, 70);
        private readonly Map map = new Map(70, 50);
        BaseGrid searchgrid;
        JumpPointParam jpParam;
        private readonly RLConsole mapConsole = new RLConsole(70, 50);

        private readonly Player player = new Player(new Vector2(0, 0), '@', 100, "Tom");
        private readonly RLRootConsole rootConsole;

        private bool mapLoadDone = false;

       

        public SPEngine(RLRootConsole rootConsole) : base(rootConsole)
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
            var pos = map.findPPos();
            ConstructGrid();
            jpParam = new JumpPointParam(searchgrid, false, false);
            player.pos = pos;
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
                    case TileType.PathTest:
                        mapConsole.Set(pos.X, pos.Y, RLColor.Yellow, RLColor.Yellow, tile.symbol);
                        break;
                }
            }
        }


     

        public override void update()
        {
            
        }
    }
}