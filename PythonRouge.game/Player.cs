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
using System;
using RLNET;

namespace PythonRouge.game
{

    public class Player : Entity
    {
        Engine engine;
        public Player(Vector2 pos, char symbol, int health, string name, Engine Engine) : base(pos, symbol, health, name)
        {
            this.engine = engine;
        }

        public void attack()
        {
           engine?.inv?.getItemInCurrentSlot().use(getTarget(engine));
        }

    }
}