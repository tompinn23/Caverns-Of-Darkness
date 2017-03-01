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

namespace PythonRouge.game
{
    class Monster : Entity
    {
        public bool seen;

        private float atkMod;
        private float defMod;


        public Monster(Vector2 pos, char symbol, int health, string name, float atkMod, float defMod, SPEngine engine) : base(pos, symbol, health, name)
        {
            this.atkMod = atkMod;
            this.defMod = defMod;
            engine.MonsterUpdate += new EventHandler(monsterUpdate);    
        }
        public Monster(Vector2 pos, char symbol, int health, string name, float atkMod, float defMod, MpEngine engine) : base(pos, symbol, health, name)
        {
            this.atkMod = atkMod;
            this.defMod = defMod;
        }
        public monsterUpdate(object sender, EventArgs e)
        {
            
        }
    }
}
