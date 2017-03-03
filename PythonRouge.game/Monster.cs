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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EpPathFinding.cs;


namespace PythonRouge.game
{
    public class Monster : Entity
    {
        public bool PlayerSeen = false;

        private float atkMod;
        private float defMod;

        private Queue<Vector2> moves = new Queue<Vector2>();
        List<Vector2> rndMoves = new List<Vector2> { new Vector2(-1,0), new Vector2(1, 0), new Vector2(0, -1), new Vector2(0, 1)};
        Random rnd = new Random();

        public Monster(Vector2 pos, char symbol, int health, string name, float atkMod, float defMod, SPEngine engine) : base(pos, symbol, health, name)
        {
            this.atkMod = atkMod;
            this.defMod = defMod;
            engine.PlayerMove += new Engine.PlayerMoveEventHandler(playerMoved);
            engine.MonsterUpdate += new Engine.MonsterUpdateEventHandler(monsterUpdate);

        }
        
        public Monster(Vector2 pos, char symbol, int health, string name, float atkMod, float defMod, MPEngine engine) : base(pos, symbol, health, name)
        {
            this.atkMod = atkMod;
            this.defMod = defMod;
            engine.PlayerMove += new Engine.PlayerMoveEventHandler(playerMoved);
            engine.MonsterUpdate += new Engine.MonsterUpdateEventHandler(monsterUpdate);
        }

        public void playerMoved(object sender, PlayerMoveEventArgs e)
        {
            PlayerSeen = canSeePlayer(e.engine.map.mGrid, e.playerPos);
            if(PlayerSeen)
            {
                JumpPointParam jParam = new JumpPointParam(e.engine.searchgrid, false, false);
                jParam.Reset(new GridPos(pos.X, pos.Y), new GridPos(e.playerPos.X, e.playerPos.Y));
                List<GridPos> resultPathList = JumpPointFinder.FindPath(jParam);
                resultPathList = JumpPointFinder.GetFullPath(resultPathList);
                if(moves.Count != 0)
                {
                    moves.Clear();
                }
                for(int i =0; i < resultPathList.Count; i++)
                {
                    moves.Enqueue(new Vector2(resultPathList[i].x, resultPathList[i].y));
                }
            }
            else
            {
                moves.Clear();
            }
        }

        private void monsterUpdate(object sender, MonsterUpdateEventArgs e)
        {
            if(moves.Count != 0)
            {
                var point = moves.Dequeue();
                move(point);
            }
            if(!PlayerSeen)
            {
                Vector2 a = rndMoves[rnd.Next(rndMoves.Count)];
                if (e.engine.map.canMove(pos, a.X, a.Y)) move(a.X, a.Y);
            }
        }

        public bool canSeePlayer(GameGrid grid, Vector2 player)
        {
            ShadowCast.ComputeVisibility(grid, pos, 7f, name);
            return grid.Game_map.Any(t => t.Value.lit && t.Value.discoveredby == name);
        }
    }
}
