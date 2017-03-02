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

namespace PythonRouge.game
{
    public class SPEngine : Engine
    {
        private readonly Player player = new Player(new Vector2(0, 0), '@', 100, "Tom");
        public int monsters = 5;
        public List<Monster> monsterList = new List<Monster>();
        public System.Timers.Timer TickTimer = new System.Timers.Timer();
        
        private bool mapLoadDone = false;


        public SPEngine(RLRootConsole rootConsole)
        {
            this.rootConsole = rootConsole;
            MapConsole.SetBackColor(0, 0, 70, 50, RLColor.Blue);
            InvConsole.SetBackColor(0, 0, 20, 70, RLColor.Cyan);
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
            TickTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTick);
            TickTimer.Interval = 200;
            TickTimer.Enabled = true;
            AddMonsters();
            var pos = map.findPPos();
            ConstructGrid();
            player.pos = pos;
            ShadowCast.ComputeVisibility(map.grid, player.pos, 7.5f, player.name);
        }

        private void OnTick(object sender, ElapsedEventArgs e)
        {
            OnMonsterUpdate(new MonsterUpdateEventArgs { playerPos = player.pos, engine = this });
        }

        public void AddMonsters()
        {
            Random rnd = new Random();
            for (int i =0; i <= monsters; i++)
            {
                var spawnPos = map.openTiles[rnd.Next(map.openTiles.Count)];
                var monster = new Monster(spawnPos, 'M', 100, "monster" + i, 1.5f, 1.2f, this);
                monsterList.Add(monster);
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
                m.draw(MapConsole);
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

        public void update()
        {
            //OnMonsterUpdate(new MonsterUpdateEventArgs { playerPos  = player.pos, engine = this});
        }

        public void HandleKey(RLKeyPress keyPress)
        {
            base.HandleKey(keyPress, this.player);
            OnPlayerMove(new PlayerMoveEventArgs { playerPos = player.pos, engine = this });
        }
    }
}