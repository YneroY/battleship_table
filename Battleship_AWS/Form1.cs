using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Battleship_AWS
{
    public partial class Form1 : Form
    {
        const int MAX_GRID_SIZE = 9;
        const int GRID_ROW_COLUMN_MAXSIZE = 3;

        //Ship name
        const string destroyer = "Destroyer";

        Control[,] seaGrid = new Control[3, 3];

        //500 = vertical, 501 = horizontal
        int[] destroyer_array = new int[3] { 1, 2, 501};

        Dictionary<int, string> gridMapping = new Dictionary<int, string>();
        Dictionary<int, string> shipLocation = new Dictionary<int, string>();
        string gridMapping_value = null;
        string current_selected_ship = null;
        int lastClickedGrid = 0;

        Color ship_Color = Color.Thistle;
        Color none_ship_Color = Color.LightSkyBlue;

        public Form1()
        {
            InitializeComponent();
            mapGrid();
            placePictureBoxGrid();

            //Load destroyer into grid
            for (int x = 0; x < destroyer_array.Length - 1; x++)
            {
                if (gridMapping.TryGetValue(destroyer_array[x], out gridMapping_value))
                {
                    seaGrid[Int32.Parse(gridMapping_value.Substring(0, 1)), Int32.Parse(gridMapping_value.Substring(1,1))].BackColor = ship_Color;
                }

                shipLocation.Add(destroyer_array[x], destroyer);
            }
        }

        //Map individual pictureBox with their corresponding coordinates in a grid
        void mapGrid()
        {
            int row_seaGrid = 0;
            int column_seaGrid = 0;

            //Grid mapping in the Dictionary object
            for(int x = 1; x < MAX_GRID_SIZE + 1; x++)
            {
                if (column_seaGrid < GRID_ROW_COLUMN_MAXSIZE)
                {
                    gridMapping.Add(x, row_seaGrid.ToString() + column_seaGrid.ToString());
                    column_seaGrid += 1;
                }
                else
                {
                    column_seaGrid = 0;
                    row_seaGrid += 1;

                    gridMapping.Add(x, row_seaGrid.ToString() + column_seaGrid.ToString());
                    column_seaGrid += 1;
                }
            }
        }

        //Populate the grid with the corresponding pictureBox
        void placePictureBoxGrid()
        {
            int pictureBoxNumbering = 1;

            for(int x = 0; x < GRID_ROW_COLUMN_MAXSIZE; x++)
            {
                for(int y = 0; y < GRID_ROW_COLUMN_MAXSIZE; y++)
                {
                    seaGrid[x, y] = this.Controls.Find("pictureBox" + pictureBoxNumbering.ToString(), true)[0];
                    pictureBoxNumbering += 1;
                }

            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (current_selected_ship == null)
            {
                if (pictureBox1.BackColor == ship_Color)
                {
                    shipLocation.TryGetValue(1, out current_selected_ship);
                    lastClickedGrid = 1;
                }
            }
            else
            {
                //Remove the previous ship location in dictionary object
                foreach(var item in shipLocation.Where(kvp => kvp.Value == current_selected_ship).ToList())
                {
                    shipLocation.Remove(item.Key);
                }


                //Load ship onto grid
                for (int x = 0; x < destroyer_array.Length - 1; x++)
                {
                    shipLocation.Add(destroyer_array[x], destroyer);
                }


                lastClickedGrid = 0;
                current_selected_ship = null;
            }
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            if (current_selected_ship != null)
            {
                int transition = 1 - lastClickedGrid;
                lastClickedGrid = 1;

                if (destroyer_array[destroyer_array.Length - 1] == 501) //Horizontal
                {
                    if (destroyer_array[destroyer_array.Length - 2] + transition <= ((lastClickedGrid/3) * 3) + 3)
                    {
                        //Clear ship from grid
                        for (int x = 0; x < destroyer_array.Length - 1; x++)
                        {
                            if (gridMapping.TryGetValue(destroyer_array[x], out gridMapping_value))
                            {
                                seaGrid[Int32.Parse(gridMapping_value.Substring(0, 1)), Int32.Parse(gridMapping_value.Substring(1, 1))].BackColor = none_ship_Color;
                            }
                        }

                        for (int x = 0; x < destroyer_array.Length - 1; x++)
                        {
                            if (x == 0)
                            {
                                destroyer_array[x] = lastClickedGrid;
                            }
                            else
                            {
                                destroyer_array[x] = destroyer_array[x - 1] + 1;
                            }
                        }

                        //Load ship onto grid
                        for (int x = 0; x < destroyer_array.Length - 1; x++)
                        {
                            if (gridMapping.TryGetValue(destroyer_array[x], out gridMapping_value))
                            {
                                seaGrid[Int32.Parse(gridMapping_value.Substring(0, 1)), Int32.Parse(gridMapping_value.Substring(1, 1))].BackColor = ship_Color;
                            }
                        }

                    }
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (current_selected_ship == null)
            {
                if (pictureBox2.BackColor == ship_Color)
                {
                    shipLocation.TryGetValue(2, out current_selected_ship);
                    lastClickedGrid = 2;
                }
            }
            else
            {
                //Remove the previous ship location in dictionary object
                foreach (var item in shipLocation.Where(kvp => kvp.Value == current_selected_ship).ToList())
                {
                    shipLocation.Remove(item.Key);
                }


                //Load ship onto grid
                for (int x = 0; x < destroyer_array.Length - 1; x++)
                {
                    shipLocation.Add(destroyer_array[x], "ship1");
                }


                lastClickedGrid = 0;
                current_selected_ship = null;
            }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            var currentpb = sender as PictureBox;
            if(currentpb != null)
            {
                Console.WriteLine(currentpb.Name);
            }

            if (current_selected_ship == null)
            {
                if (pictureBox1.BackColor == ship_Color)
                {
                    shipLocation.TryGetValue(1, out current_selected_ship);
                    lastClickedGrid = 1;
                }
            }
            else
            {
                //Remove the previous ship location in dictionary object
                foreach (var item in shipLocation.Where(kvp => kvp.Value == current_selected_ship).ToList())
                {
                    shipLocation.Remove(item.Key);
                }


                //Load ship onto grid
                for (int x = 0; x < destroyer_array.Length - 1; x++)
                {
                    shipLocation.Add(destroyer_array[x], destroyer);
                }


                lastClickedGrid = 0;
                current_selected_ship = null;
            }
        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            if(current_selected_ship != null)
            {
                int transition = 2 - lastClickedGrid;
                lastClickedGrid = 2;

                if (destroyer_array[destroyer_array.Length - 1] == 501) //Horizontal
                {
                    if (destroyer_array[destroyer_array.Length - 2] + transition <= ((lastClickedGrid / 3) * 3) + 3)
                    {
                        //Clear ship from grid
                        for (int x = 0; x < destroyer_array.Length - 1; x++)
                        {
                            if (gridMapping.TryGetValue(destroyer_array[x], out gridMapping_value))
                            {
                                seaGrid[Int32.Parse(gridMapping_value.Substring(0, 1)), Int32.Parse(gridMapping_value.Substring(1, 1))].BackColor = none_ship_Color;
                            }
                        }

                        for (int x = 0; x < destroyer_array.Length - 1; x++)
                        {
                            if(x == 0)
                            {
                                destroyer_array[x] = lastClickedGrid;
                            }
                            else
                            {
                                destroyer_array[x] = destroyer_array[x - 1] + 1;
                            }
                        }

                        //Load ship onto grid
                        for (int x = 0; x < destroyer_array.Length - 1; x++)
                        {
                            if (gridMapping.TryGetValue(destroyer_array[x], out gridMapping_value))
                            {
                                seaGrid[Int32.Parse(gridMapping_value.Substring(0, 1)), Int32.Parse(gridMapping_value.Substring(1, 1))].BackColor = ship_Color;
                            }
                        }

                    }
                }
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (current_selected_ship == null)
            {
                if (pictureBox3.BackColor == ship_Color)
                {
                    shipLocation.TryGetValue(3, out current_selected_ship);
                    lastClickedGrid = 3;
                }
            }
            else
            {
                //Remove the previous ship location in dictionary object
                foreach (var item in shipLocation.Where(kvp => kvp.Value == current_selected_ship).ToList())
                {
                    shipLocation.Remove(item.Key);
                }


                //Load ship onto grid
                for (int x = 0; x < destroyer_array.Length - 1; x++)
                {
                    shipLocation.Add(destroyer_array[x], "ship1");
                }


                lastClickedGrid = 0;
                current_selected_ship = null;
            }
        }

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {

        }
    }
}
