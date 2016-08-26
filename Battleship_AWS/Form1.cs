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

        /// <summary>
        /// General pictureBox mouse enter event
        /// </summary>
        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            if (current_selected_ship != null)
            {
                var currentpb = sender as PictureBox;
                if (currentpb != null)
                {
                    Console.WriteLine(currentpb.Name);
                }

                int pictureBoxDigit = Int32.Parse(currentpb.Name.Substring(currentpb.Name.Length - 1, 1)); //Get the number on the pictureBox using the last char in the string

                int transition = pictureBoxDigit - lastClickedGrid;
                lastClickedGrid = pictureBoxDigit;

                if (destroyer_array[destroyer_array.Length - 1] == 501) //Horizontal
                {
                    if (lastClickedGrid + getShipSize(current_selected_ship) <= getRowMax(lastClickedGrid))
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

        /// <summary>
        /// General pictureBox click event
        /// </summary>
        private void pictureBox_Click(object sender, EventArgs e)
        {
            var currentpb = sender as PictureBox;
            if(currentpb != null)
            {
                Console.WriteLine(currentpb.Name);
            }

            int pictureBoxDigit = Int32.Parse(currentpb.Name.Substring(currentpb.Name.Length - 1, 1)); //Get the number on the pictureBox using the last char in the string

            if (current_selected_ship == null)
            {
                if (currentpb.BackColor == ship_Color) //The pictureBox clicked contains a ship
                {
                    shipLocation.TryGetValue(pictureBoxDigit, out current_selected_ship);
                    int shipPivot = getShipFirstPoint(current_selected_ship);
                    gridMapping.TryGetValue(shipPivot, out gridMapping_value);
                    lastClickedGrid = shipPivot;
                    Control currentCont = seaGrid[Int32.Parse(gridMapping_value.Substring(0, 1)), Int32.Parse(gridMapping_value.Substring(1, 1))];
                    Point targetPoint = new Point((currentCont.Left + currentCont.Right) / 2, (currentCont.Top + currentCont.Bottom) / 2); //Get the pictureBox's center point
                    Point screen_coordinates = currentCont.Parent.PointToScreen(targetPoint);
                    Cursor.Position = screen_coordinates;
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

        int getRowMax(int currentpoint)
        {
            if (currentpoint % GRID_ROW_COLUMN_MAXSIZE == 0)
            {
                return (currentpoint / GRID_ROW_COLUMN_MAXSIZE) * GRID_ROW_COLUMN_MAXSIZE;
            }
            else
            {
                return ((currentpoint / GRID_ROW_COLUMN_MAXSIZE) + 1) * GRID_ROW_COLUMN_MAXSIZE;
            }
        }

        int getShipSize(string shipname)
        {
            if (shipname == destroyer)
            {
                return destroyer_array.Length - 2;
            }
            else
                return 0;
        }

        int getShipFirstPoint(string shipname)
        {
            if (shipname == destroyer)
            {
                return destroyer_array[0];
            }
            else
                return 0;
        }
    }
}
