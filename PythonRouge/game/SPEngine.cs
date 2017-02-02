﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp;
using RogueSharp.MapCreation;

namespace PythonRouge.game
{
    class SPEngine
    {
        private RLRootConsole rootConsole;

        private RLConsole mapConsole = new RLConsole(70, 50);
        private RLConsole invConsole = new RLConsole(20, 70);

        private Player player = new Player(0,0,100,"Tom", '@');


        public SPEngine(RLRootConsole rootConsole)
        {
            this.rootConsole = rootConsole;
            mapConsole.SetBackColor(0, 0, 70, 50, RLColor.Blue);
            invConsole.SetBackColor(0, 0, 20, 70, RLColor.Cyan);
        }

        public void render()
        {
            startUpdate();
            RLConsole.Blit(mapConsole, 0, 0, 70, 50, this.rootConsole, 0, 10);
            RLConsole.Blit(invConsole, 0, 0, 20, 70, this.rootConsole, 70, 0);
            endUpdate();
        }

        public void mapGenerate()
        {
        }


        public void startUpdate()
        {
            player.draw(mapConsole);                     
        }
        public void endUpdate()
        {
            player.clear(mapConsole);
        }
        

        public void handleKey(RLKeyPress keyPress)
        {
            if (keyPress.Key == RLKey.Up)
            {
                player.move(0, -1);
            }
            else if (keyPress.Key == RLKey.Down)
            {
                player.move(0, 1);
            }
            else if (keyPress.Key == RLKey.Left)
            {
                player.move(-1, 0);
            }
            else if (keyPress.Key == RLKey.Right)
            {
                player.move(1, 0);
            }
        }

    }
}