using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace POE_Task_1
{
    [Serializable]

    public partial class Form1 : Form
    {
        
        public Button[,] buttons = new Button[20,20]; //button array that will be used for the grid
        
        static int UnitNum = 8;
        public int Round = 1;

        Map m = new Map(UnitNum);
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            m.GenerateBattleField();
            PlaceButtons();
        }

        public void PlaceButtons() // Method to fill the Map with buttons to create the battlefield 
        {
            GbBoxMap.Controls.Clear();

            Size btnSize = new Size(30, 30);

            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    Button btn = new Button();

                    btn.Size = btnSize;
                    btn.Location = new Point(i * 30, j * 30);

                    //buttons[i, j] = btn;
                    if (m.map[i, j] == "R")
                    {
                        btn.Text = "->";
                        btn.Name = m.uniMap[i, j].ToString();
                        btn.Click += MyButtonCLick;

                        btn.BackColor = m.uniMap[i, j].factionType == Faction.Hero ? Color.Chartreuse : Color.Crimson;
                    }
                    else if (m.map[i, j] == "M")
                    {
                        Debug.Print("Found meleeunit");
                        btn.Text = "#";
                        btn.Name = m.uniMap[i, j].ToString();
                        btn.Click += MyButtonCLick;

                        btn.BackColor = m.uniMap[i, j].factionType == Faction.Hero ? Color.Chartreuse : Color.Crimson;
                    }
                    else if (m.map[i, j] == "FB")
                    {
                        FactoryBuilding FB = (FactoryBuilding) m.buildingMap[i, j];
                        btn.Text = FB.Symbol;
                        btn.BackColor = FB.Faction == Faction.Hero ? Color.Chartreuse : Color.Crimson;

                        btn.Name = m.buildingMap[i, j].ToString();
                        btn.Click += MyButtonCLick;
                    }
                    else if (m.map[i, j] == "RB")
                    {
                        ResourceBuilding RB = (ResourceBuilding)m.buildingMap[i, j];
                        btn.Text = RB.Symbol;
                        btn.BackColor = RB.Faction == Faction.Hero ? Color.Chartreuse : Color.Crimson;

                        btn.Name = m.buildingMap[i, j].ToString();
                        btn.Click += MyButtonCLick;
                    }

                    buttons[i, j] = btn;
                }
            }

            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    GbBoxMap.Controls.Add(buttons[i, j]);
                }
            }
        }

        public void MyButtonCLick(object sender, EventArgs e)
        {
            Button btn = ((Button) sender);

            foreach (Units u in m.units)
            {
                if (btn.Name == u.ToString())
                {
                    txtOutput.Text = u.ToString();
                }
            }

            foreach (Building b in m.buildings)
            {
                if (btn.Name == b.ToString())
                {
                    txtOutput.Text = b.ToString();
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e) // enabling the ticker to start game 
        {
            Ticker.Enabled = true;
        }

        private void btnPause_Click(object sender, EventArgs e) // disabling ticker when pause button is pressed
        {
            Ticker.Enabled = false;
        }

        private void Ticker_Tick(object sender, EventArgs e)
        {
            GameEngine();
            lblRound.Text = "Round: " + Round;
        }

        public void GameEngine() //game engine method instead of a separate class
        {
            int hero = 0;
            int villian = 0;

            foreach (ResourceBuilding u in m.diamondMines)
            {
                if (u.Faction == Faction.Hero)
                {
                    hero++;
                }
                else
                {
                    villian++;
                }
            }

            foreach (FactoryBuilding u in m.barracks)
            {
                if (u.Faction == Faction.Hero)
                {
                    hero++;
                }
                else
                {
                    villian++;
                }
            }

            foreach (Units u in m.units)
            {
                if (u.factionType == Faction.Hero)
                {
                    hero++;
                }
                else
                {
                    villian++;
                }
            }


            if (hero > 0 && villian > 0)
            {
                foreach (ResourceBuilding Rb in m.diamondMines)
                {
                    Rb.GenerateResources();
                }

                foreach (FactoryBuilding Fb in m.barracks)
                {
                    if (Round % Fb.ProductionSPeed == 0)
                    {
                        m.SpawnUnits(Fb.SpawnPointX, Fb.SpawnPointY, Fb.Faction, Fb.UnitType);
                    }
                }

                foreach (Units u in m.units)
                {
                    u.AttRange(m.units, m.buildings);
                }

                m.Populate();
                m.PlaceBuildings();
                Round++;
                PlaceButtons();

            }
            else
            {
                m.Populate();
                m.PlaceBuildings();
                PlaceButtons();
                Ticker.Enabled = false;

                if (hero > villian)
                {
                    MessageBox.Show("Hero Wins on Round: " + Round);
                }
                else
                {
                    MessageBox.Show("Villain Wins on Round: " + Round);
                }
            }

            for (int i = 0; i < m.rangedUnit.Count; i++) // for loop to remove Ranged units once health has reached 0
            {
                if (m.rangedUnit[i].Death())
                {
                    m.map[m.rangedUnit[i].posX, m.rangedUnit[i].posX] = "";
                    m.rangedUnit.RemoveAt(i);
                }
            }

            for (int i = 0; i < m.melleUnit.Count; i++) // for loop to remove Melee units once health has reached 0
            {
                if (m.melleUnit[i].Death())
                {
                    m.map[m.melleUnit[i].posX, m.melleUnit[i].posX] = "";
                    m.melleUnit.RemoveAt(i);
                }
            }

            for (int i = 0; i < m.units.Count; i++) // for loop to remove units once health has reached 0
            {
                if (m.units[i].Death())
                {
                    m.map[m.units[i].posX, m.units[i].posX] = "";
                    m.units.RemoveAt(i);
                }
            }

            for (int i = 0; i < m.diamondMines.Count; i++) // for loop to remove The diamond mine buildings once health has reached 0
            {
                if (m.diamondMines[i].Destruction())
                {
                    m.map[m.diamondMines[i].PosX, m.diamondMines[i].PosX] = "";
                    m.diamondMines.RemoveAt(i);
                }
            }

            for (int i = 0; i < m.barracks.Count; i++) // for loop to remove The Barrack buildings once health has reached 0
            {
                if (m.barracks[i].Destruction())
                {
                    m.map[m.barracks[i].PosX, m.barracks[i].PosX] = "";
                    m.barracks.RemoveAt(i);
                }
            }

            for (int i = 0; i < m.buildings.Count; i++) // for loop to remove The buildings once health has reached 0
            {
                if (m.buildings[i].Destruction())
                {
                    if (m.buildings[i] is FactoryBuilding)
                    {
                        FactoryBuilding FB = (FactoryBuilding) m.buildings[i];
                        m.map[FB.PosX, FB.PosY] = "";
                    }
                    else if (m.buildings[i] is ResourceBuilding)
                    {
                        ResourceBuilding RB = (ResourceBuilding) m.buildings[i];
                        m.map[RB.PosX, RB.PosY] = "";
                    }
                    
                    m.buildings.RemoveAt(i);
                }
            }

        }

        private void btnRead_Click(object sender, EventArgs e) // Read button to implement the saved files from the Save button onto the map
        {

            try
            {
                FileStream fs = new FileStream("SaveFile.dat", FileMode.Open, FileAccess.Read, FileShare.None);
                BinaryFormatter bf = new BinaryFormatter();

                using (fs)
                {
                    m = (Map)bf.Deserialize(fs);
                    PlaceButtons();
                    MessageBox.Show("File Loaded ");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnSave_Click(object sender, EventArgs e) // Save button to save the map, unit and building Information
        {
            
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream("SaveFile.dat", FileMode.Create, FileAccess.Write, FileShare.None);
                using (fs)
                {
                    bf.Serialize(fs, m);
                    MessageBox.Show("File Saved");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
