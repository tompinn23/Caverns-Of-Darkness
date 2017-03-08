using RLNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonRouge.game
{
    public class SidePanel
    {
        public RLConsole console;
        public Player player;
        public int w;
        public int h;
        private Engine engine;

        public SidePanel(RLConsole console, Player player, int w, int h, Engine engine)
        {
            this.console = console;
            this.player = player;
            this.w = w;
            this.h = h;
            this.engine = engine;
        }
        public void Draw()
        {
            for(int y = 0; y <= h; y++)
            {
                console.Set(0, y, RLColor.Gray, RLColor.Black, 178);
            }
            console.Print(1, 1, "Info:", RLColor.White);
            for(int x = 0; x <= w; x++)
            {
                console.Set(x, 2, RLColor.Gray, RLColor.Black, 178);
            }
            console.Print(1, 4, "Health: ", RLColor.White);
            console.Print(8, 4, player.health.ToString(), RLColor.Red);
            Entity target = player.getTarget(engine);
            console.Print(1,8, "Target:", RLColor.White);
            console.Print(1,9, "Health: ", RLColor.White);
            console.Print(8,9, target.health.ToString(), RLColor.Red);
        }
    }
}
