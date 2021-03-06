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
using RLNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EpPathFinding.cs;
using System.Timers;


namespace PythonRouge.game
{
    public class Monster : Entity
    {
        public bool PlayerSeen = false;

        private float atkMod;
        private float defMod;
        private int damage = 2;
        private Timer cooldownTimer = new Timer();
        private bool ready = true;

        private Queue<Vector2> moves = new Queue<Vector2>();
        List<Vector2> rndMoves = new List<Vector2> { new Vector2(-1,0), new Vector2(1, 0), new Vector2(0, -1), new Vector2(0, 1)};
        Random rnd;

        public Monster(Vector2 pos, char symbol, int health, string name, float atkMod, float defMod, int cooldown, SPEngine engine) : base(pos, symbol, health, name)
        {
            this.atkMod = atkMod;
            this.defMod = defMod;
            engine.MonsterUpdate += new Engine.MonsterUpdateEventHandler(monsterUpdate);
            rnd = engine.mRnd;
            cooldownTimer.AutoReset = false;
            cooldownTimer.Interval = cooldown;
            cooldownTimer.Elapsed += new ElapsedEventHandler(resetReady);
        }

        private void resetReady(object sender, ElapsedEventArgs e)
        {
            ready = true;
        }

        public Monster(Vector2 pos, char symbol, int health, string name, float atkMod, float defMod, int cooldown, MPEngine engine) : base(pos, symbol, health, name)
        {
            this.atkMod = atkMod;
            this.defMod = defMod;
            engine.MonsterUpdate += new Engine.MonsterUpdateEventHandler(monsterUpdate);
            rnd = engine.mRnd;
            cooldownTimer.AutoReset = false;
            cooldownTimer.Interval = cooldown;
            cooldownTimer.Elapsed += new ElapsedEventHandler(resetReady);
        }

        public override void TakeDamage(float atkDamage)
        {
            base.TakeDamage(atkDamage / defMod);
        }
         
        public void attack(Player target)
        {
            if(ready)
            {
                ready = false;
                target.TakeDamage(-(damage * atkMod));
                cooldownTimer.Start();
            }
        }

        private void monsterUpdate(object sender, MonsterUpdateEventArgs e)
        {
            PlayerSeen = canSeePlayer(e.engine.map.mGrid,e.playerPos);
            if(moves.Count == 0)
            {
            if (PlayerSeen)
            {
                JumpPointParam jParam = new JumpPointParam(e.engine.searchgrid, false, false);
                jParam.Reset(new GridPos(pos.X, pos.Y), new GridPos(e.playerPos.X, e.playerPos.Y));
                List<GridPos> resultPathList = JumpPointFinder.FindPath(jParam);
                resultPathList = JumpPointFinder.GetFullPath(resultPathList);
                if (moves.Count != 0)
                {
                    moves.Clear();
                }
                for (int i = 0; i < resultPathList.Count; i++)
                {
                    moves.Enqueue(new Vector2(resultPathList[i].x, resultPathList[i].y));
                }
            }
            }
            if (moves.Count != 0)
            {
                var dxdy = e.engine.map.getDxDy(pos, moves.Dequeue());
                if (!e.engine.checkEntity(pos, e.playerPos, dxdy.X, dxdy.Y)) move(dxdy.X, dxdy.Y);
            }
            if(PlayerSeen == false)
            {
                Vector2 a = rndMoves[rnd.Next(rndMoves.Count)];
                if (e.engine.map.canMove(pos, a.X, a.Y)) move(a.X, a.Y);
            }
            if(isPlayerNear(e.playerPos))
            {
                attack(e.engine.player);
            }
        }

        public bool isPlayerNear(Vector2 PlayerPos)
        {
            foreach(Vector2 p in rndMoves)
            {
                if((pos.X + p.X) == PlayerPos.X && (pos.Y + p.Y) == PlayerPos.Y)
                {
                    return true;
                }
            }
            return false;
        }

        public bool canSeePlayer(GameGrid grid, Vector2 player)
        {
            grid.Clear();
            ShadowCast.ComputeVisibility(grid, pos, 5f, name);
            foreach(Tile t in grid.Game_map.Values)
            {
                if (t.lit && t.discoveredby == name && t.pos == player)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
