using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POE_Task_1
{
    
    enum  Faction //factions for both the Melee and the Ranged unit
    {
        Hero, 
        Villain
    }

    [Serializable]

    abstract class Units
    {
        public string name;

        public int posX;

        public int posY;

        public int health;

        public int maxHealth;

        public int speed;

        public int attack;

        public int atkRange;

        public Faction factionType;

        public string symbol;

        public bool isAtk;


        public abstract void Move(int type);

        public abstract void Combat(int type);

        public abstract void AttRange(List<Units> uni,List<Building> builds);

        public abstract bool Death();

        public abstract Units Position();

        public abstract string ToString();

        public abstract Building BuildingPosition();

        public Units(string N, int x, int y, int hp, int spd, int atk, int attRange, Faction fac, string sym, bool iatk )
        {
            name = N;
            posX = x;
            posY = y;
            health = hp;
            speed = spd;
            attack = atk;
            atkRange = attRange;
            factionType = fac;
            symbol = sym;
            isAtk = iatk;

            maxHealth = hp;
        }


    }
}
