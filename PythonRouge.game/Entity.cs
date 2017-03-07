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

namespace PythonRouge.game
{
    public class Entity
    {
        public Vector2 pos;
        public int health;
        public string name;
        public char symbol;
        public FacingDirection facing;


        public Entity(Vector2 pos, char symbol, int health, string name)
        {
            this.pos = pos;
            this.symbol = symbol;
            this.health = health;
            this.name = name;
        }

        //Move method updates player pos in change of y and x
        public void move(int dx, int dy)
        {
            pos.X += dx;
            pos.Y += dy;
            if(dx > 0)
            {
                facing = FacingDirection.East;
            }
            if(dx < 0)
            {
                facing = FacingDirection.West;
            }
            if(dy > 0)
            {
                facing = FacingDirection.North;
            }
            if(dy < 0)
            {
                facing = FacingDirection.South;
            }
        }

        public virtual void TakeDamage(float atkDamage)
        {
            this.health += (int)Math.Round(atkDamage);
        }
        public void draw(RLConsole console)
        {
            console.Set(pos.X, pos.Y, RLColor.White, null, symbol);
        }

        public void clear(RLConsole console)
        {
            console.Set(pos.X, pos.Y, null, null, ' ');
        }
    }

    public Entity getTarget(Engine engine)
    {
        foreach(Entity en in engine.entityList)
        {
            if()
        }
    }
    public enum FacingDirection
    {
        North,
        East,
        South,
        West
    }

}
