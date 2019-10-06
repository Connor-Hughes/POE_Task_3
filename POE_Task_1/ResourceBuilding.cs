using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POE_Task_1
{
    [Serializable]
    class ResourceBuilding : Building
    {
        public enum ResourceType
        {
            Diamonds,
            Coal
        }

        public int PosX
        {
            get { return  base.posX; }
            set { posX = value; }
        }

        public int PosY
        {
            get { return base.posY; }
            set { posY = value; }
        }

        public int Health
        {
            get { return base.health; }
            set { health = value; }
        }

        public Faction Faction
        {
            get { return base.faction; }
            set { faction = value; }
        }

        public string Symbol
        {
            get { return base.symbol; }
            set { symbol = value; }
        }

        private int ResourceGenerated = 0;
        private int GeneratePerRound = 0;
        private int ResourcePool = 1000;
        private ResourceType Resource;


        public ResourceBuilding(int x, int y, int hp, Faction fac, string sym, int ResPerRound) :
            base(x, y, hp, fac, sym)
        {
            GeneratePerRound = ResPerRound;
        }

        public override bool Destruction() // same method to destroy Buildings when the health reaches 0
        {
            if (Health <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void GenerateResources()
        {
            if (ResourcePool > 0)
            {
                ResourcePool -= GeneratePerRound;
                ResourceGenerated += GeneratePerRound;
            }
        }



        public override string ToString()
        {
            return " Mine: X: " + posX
                   + " Y: " + posY
                   + "\nHealth: " + Health
                   + "\nResource: " + Resource + ": " + ResourceGenerated
                   + "\nResource: " + ResourcePool
                   + "\nFaction: " + Faction;
        }



        



    }

}

