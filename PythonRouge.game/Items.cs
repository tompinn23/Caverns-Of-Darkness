using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
namespace PythonRouge.game
{
    public class Item
    {
        int cooldown;
        public char symbol;
        Timer cooldownTimer = new Timer();
        bool cooling;
        public Item(char symbol, int cooldown, string name)
        {
            this.symbol = symbol;
            this.cooldown = cooldown;
            cooldownTimer.AutoReset = false;
            cooldownTimer.Interval = cooldown;
            cooldownTimer.Elapsed += new ElapsedEventHandler(cooldownFinished);
        }

        private void cooldownFinished(object sender, ElapsedEventArgs e)
        {
            cooling = false;
        }
        public bool use()
        {
            if (cooling) return false;
            else
            {
                cooldownTimer.Start();
                cooling = true;
                return true;
            }
        }
        public virtual bool use(Entity target)
        {
            return use();
        }
        public bool use(Entity target, int atkDamage)
        {
            var success = use();
            if(!success) return false;
            else
            {
                target.TakeDamage(atkDamage);
                return true;
            }

        }
    }
    class Weapon : Item
    {
        int atkDamage;
        public Weapon(char symbol, int cooldown, int atkDamage, string name = "Bronze Sword") : base(symbol, cooldown, name)
        {
            this.atkDamage = atkDamage;  
        }
        public override bool use(Entity target)
        {
            return base.use(target, atkDamage);
        }
    }

}
