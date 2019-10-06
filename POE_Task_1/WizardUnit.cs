using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POE_Task_1
{
    [Serializable]
    class WizardUnit : Units
    {
        public int PosX
        {
            get { return posX; }
            set { base.posX = value; }
        }

        public int PosY
        {
            get { return posY; }
            set { base.posY = value; }
        }

        public int Health
        {
            get { return health; }
            set { base.health = value; }
        }

        public int MaxHealth
        {
            get { return maxHealth; }

        }

        public int Speed
        {
            get { return speed; }
        }

        public int Attack
        {
            get { return attack; }
        }

        public int AtkRange
        {
            get { return atkRange; }
        }

        public Faction FactionType
        {
            get { return factionType; }
        }

        public string Symbol
        {
            get { return symbol; }
        }


        public bool IsAtk
        {
            get { return isAtk; }
        }

        List<Units> units = new List<Units>();
        List<Building> buildings = new List<Building>();
        private int speedCounter = 1;
        Random r = new Random();
        Units ClosestUnit;

        public WizardUnit(string N, int x, int y, int hp, int spd, int atk, int attRange, Faction fac, string sym,
            bool iatk)
            : base(N, x, y, hp, spd, atk, attRange, fac, sym, iatk)
        {

        }

        public override void
            Move(int type) // all the move functions for the units once they have spawned and the the Buildings themselves
        {
            if (Health > MaxHealth * 0.25)
            {
                if (ClosestUnit is MelleUnit)
                {
                    MelleUnit closestUnitM = (MelleUnit) ClosestUnit;

                    if (closestUnitM.posX > posX && posX < 20)
                    {
                        posX++;
                    }
                    else if (closestUnitM.posX < posX && posX > 0)
                    {
                        posX--;
                    }

                    if (closestUnitM.posY > posY && posY < 20)
                    {
                        PosY++;
                    }
                    else if (closestUnitM.posY < posY && posY > 0)
                    {
                        posY--;
                    }
                }
                else if (ClosestUnit is RangedUnit)
                {
                    RangedUnit closestUnitR = (RangedUnit) ClosestUnit;

                    if (closestUnitR.PosX > posX && PosX < 20)
                    {
                        posX++;
                    }
                    else if (closestUnitR.PosX < posX && posX > 0)
                    {
                        posX--;
                    }

                    if (closestUnitR.PosY > posY && PosY < 20)
                    {
                        posY++;
                    }
                    else if (closestUnitR.PosY < posY && posY > 0)
                    {
                        posY--;
                    }
                }
            }
        }

        public override void
            Combat(int type) // the method to get the units to fight their enemies and shows them who to fight
        {
            if (ClosestUnit is MelleUnit)
            {
                MelleUnit M = (MelleUnit) ClosestUnit;
                M.Health -= Attack;
            }
            else if (ClosestUnit is RangedUnit)
            {
                RangedUnit R = (RangedUnit) ClosestUnit;
                R.Health -= Attack;
            }
        }

        public override void AttRange(List<Units> uni, List<Building> builds) // checking the range of the building and the units they spawn
        {

            units = uni;
            buildings = builds;

            ClosestUnit = Position();

            int enemyType;

            int xDis = 0, yDis = 0;

            int uDistance = 10000, bDistance = 10000;
            int distance;

            if (ClosestUnit is MelleUnit)
            {
                MelleUnit M = (MelleUnit)ClosestUnit;
                xDis = Math.Abs((PosX - M.PosX) * (PosX - M.PosX));
                yDis = Math.Abs((PosY - M.PosY) * (PosY - M.PosY));

                uDistance = (int)Math.Round(Math.Sqrt(xDis + yDis), 0);
            }
            else if (ClosestUnit is RangedUnit)
            {
                RangedUnit R = (RangedUnit)ClosestUnit;
                xDis = Math.Abs((PosX - R.PosX) * (PosX - R.PosX));
                yDis = Math.Abs((PosY - R.PosY) * (PosY - R.PosY));

                uDistance = (int)Math.Round(Math.Sqrt(xDis + yDis), 0);
            }
            if (units[0] != null)
            {
                if (uDistance < bDistance)
                {
                    distance = uDistance;
                    enemyType = 0;
                }
                else
                {
                    distance = bDistance;
                    enemyType = 1;
                }
            }
            else
            {
                distance = bDistance;
                enemyType = 1;
            }

            //Checks to see if they are below 25% health so they move rather than attacking
            if (Health > MaxHealth * 0.25)
            {
                if (distance <= atkRange)
                {
                    isAtk = true;
                    Combat(enemyType);
                }
                else
                {
                    isAtk = false;
                    Move(enemyType);
                }
            }
            else
            {
                Move(enemyType);
            }
        }

        public override bool Death() // Checks if the Units health is at or below 0
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

        public override Units Position() //checks the Position of the Buildings and which positions to spawn units
        {

            int Xdis = 0, Ydis = 0;
            double Distance = 1000;
            double temp = 1000;
            Units Target = null;

            foreach (Units b in units)
            {
                if (b is RangedUnit)
                {
                    RangedUnit Fb = (RangedUnit)b;

                    if (FactionType != b.factionType)
                    {
                        Xdis = Math.Abs(PosX - Fb.PosX) * (PosX - Fb.PosX);
                        Ydis = Math.Abs(PosY - Fb.PosY) * (PosY - Fb.PosY);

                        Distance = Math.Round(Math.Sqrt(Xdis + Ydis), 0);
                    }

                }
                else if(b is MelleUnit)
                {
                    MelleUnit Rb = (MelleUnit)b;
                    if (FactionType != b.factionType)
                    {
                        Xdis = Math.Abs(PosX - Rb.PosX) * (PosX - Rb.PosX);
                        Ydis = Math.Abs(PosY - Rb.PosY) * (PosY - Rb.PosY);

                        Distance = Math.Round(Math.Sqrt(Xdis + Ydis), 0);
                    }
                }

                if (Distance < temp)
                {
                    temp = Distance;
                    Target = b;
                }

            }
            return Target;
        }
        public override string ToString() //Shares the unit and Building information with the player
        {
            return name + " X: " + posX
                   + " Y: " + posY
                   + "\nMaxHealth: " + MaxHealth
                   + "\nHealth: " + Health
                   + "\nSpeed: " + Speed
                   + "\nAttackDamage: " + Attack
                   + "AttackRange: " + AtkRange
                   + "\nFaction: " + FactionType;

        }
    }
}
