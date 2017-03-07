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
using System;
using RLNET;

namespace PythonRouge.game
{
    public class Player : Entity
    {
        Weapon Sword;
        public Player(Vector2 pos, char symbol, int health, string name) : base(pos, symbol, health, name)
        {
            Sword = new Weapon((char)234, 450, 12);
        }

        public void attack()
        {
           Sword.use(getTarget());
        }

    }
}