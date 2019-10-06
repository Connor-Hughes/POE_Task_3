using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POE_Task_1
{
    [Serializable]

    abstract class Building
    {
        protected int posX;

        protected int posY;

        protected int health;

        public Faction faction;

        protected string symbol;


        public abstract bool Destruction();

        public abstract string ToString();

        public Building(int x, int y, int hp, Faction fac, string sym)
        {
            posX = x;
            posY = y;
            health = hp;
            faction = fac;
            symbol = sym;

        }


    }
}
