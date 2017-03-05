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
using System.Timers;
using System.Diagnostics;

namespace PythonRouge.game
{
    public class SPEngine : Engine
    {
        private readonly Player player = new Player(new Vector2(0, 0), '@', 100, "Tom");
        public int monsters = 4;
        public List<Monster> monsterList = new List<Monster>();

        public System.Timers.Timer TickTimer = new System.Timers.Timer();


        public bool tickDone = true;
        private bool mapLoadDone = false;
        

        public SPEngine(RLRootConsole rootConsole)
        {
            this.rootConsole = rootConsole;
            MapGenerate();
            do
            {
                rootConsole.Clear();
                MapConsole.Clear();
                MapConsole.Print(0, 0, "Loading Map",RLColor.White);
                RLConsole.Blit(MapConsole, 0, 0, 70, 50, rootConsole, 0, 10);
                rootConsole.Draw();
                Thread.Sleep(200);
                MapConsole.Print(0, 0, "Loading Map.", RLColor.White);
                RLConsole.Blit(MapConsole, 0, 0, 70, 50, rootConsole, 0, 10);
                rootConsole.Draw();
                Thread.Sleep(200);
                MapConsole.Print(0, 0, "Loading Map..", RLColor.White);
                RLConsole.Blit(MapConsole, 0, 0, 70, 50, rootConsole, 0, 10);
                rootConsole.Draw();
                Thread.Sleep(200);
                MapConsole.Print(0, 0, "Loading Map...", RLColor.White);
                RLConsole.Blit(MapConsole, 0, 0, 70, 50, rootConsole, 0, 10);
                rootConsole.Draw();

            } while(mapLoadDone == false);
            inv = new Inventory(InvConsole);
            TickTimer.Elapsed += new ElapsedEventHandler(OnTick);
            TickTimer.Interval = 200;
            TickTimer.Enabled = true;
            AddMonsters();
            player.pos = map.findPPos();
            entityList.Add(player);
            ConstructGrid();
            ShadowCast.ComputeVisibility(map.grid, player.pos, 7.5f, player.name);
        }

        private void OnTick(object sender, ElapsedEventArgs e)
        {
            if(tickDone)
            {
                tickDone = false;
                OnMonsterUpdate(new MonsterUpdateEventArgs { playerPos = player.pos, engine = this });
                tickDone = true;
            }
            
        }

        public void AddMonsters()
        {
            Random rnd = new Random();
            for (int i =0; i <= monsters; i++)
            {
                var spawnPos = map.openTiles[rnd.Next(map.openTiles.Count)];
                var monster = new Monster(spawnPos, 'M', 100, "monster" + i, 1.5f, 1.2f, 500 ,this);
                monsterList.Add(monster);
                entityList.Add(monster);
            }
        }

        public async void MapGenerate()
        {
            await DoMapGenerate();
            mapLoadDone = true;
        } 

        public Task DoMapGenerate()
        {
            return Task.Run(() =>
            {
                map.generate();
            });
            
        }

        public override void PreRender()
        {
            base.PreRender();
            
            player.draw(MapConsole);
            foreach(Monster m in monsterList)
            {
                if (map.grid.Game_map[m.pos].lit) m.draw(MapConsole);   
            }
        }

        public override void PostRender()
        {
            base.PreRender();
            player.clear(MapConsole);
            foreach (Monster m in monsterList)
            {
                m.clear(MapConsole);
            }
        }



        public void HandleKey(RLKeyPress keyPress)
        {
            base.HandleKey(keyPress, this.player);
            OnPlayerMove(new PlayerMoveEventArgs { playerPos = player.pos, engine = this });
        }


        
    }
}