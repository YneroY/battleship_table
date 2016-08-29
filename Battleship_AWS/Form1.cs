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

        /// <summary>
        /// Destroyer ship array. Last index represents ship's orientation. 500: vertical, 501: horizontal.
        /// </summary>
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

        /// <summary>
        /// Map individual pictureBox with their corresponding coordinates in a grid
        /// </summary>
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

        /// <summary>
        /// Populate the grid with the corresponding pictureBox
        /// </summary>
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
        /// General pictureBox MouseEnter event
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event data</param>
        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            if (current_selected_ship != null) //Only need to run this routine if there is a ship selected
            {
                var currentpb = sender as PictureBox;
                int pictureBoxDigit = Int32.Parse(currentpb.Name.Substring(currentpb.Name.Length - 1, 1)); //Get the number on the pictureBox using the last char in the string
                lastClickedGrid = pictureBoxDigit;

                if (current_selected_ship == destroyer)
                {
                    if (destroyer_array[destroyer_array.Length - 1] == 501 && lastClickedGrid + getShipSize(current_selected_ship) <= getRowMax(lastClickedGrid))
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
                    else if (destroyer_array[destroyer_array.Length - 1] == 500 && lastClickedGrid + (getShipSize(current_selected_ship) * GRID_ROW_COLUMN_MAXSIZE) <= getColumnMax(lastClickedGrid))
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
                                destroyer_array[x] = destroyer_array[x - 1] + 3;
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
        /// General pictureBox click event (rotate, or change ship's location)
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Mouse event data</param>
        private void pictureBox_Click(object sender, MouseEventArgs e)
        {
            if(e.Button == System.Windows.Forms.MouseButtons.Left) //Left click, change ship's position
            {
                leftClickEvent(sender);
            }
            else //Right click, rotate ship
            {
                rightClickEvent(sender);
            }
        }

        /// <summary>
        /// Move the ship in the grid, or place the ship at the current position
        /// </summary>
        /// <param name="sender">Sender object</param>
        void leftClickEvent(object sender)
        {
            var currentpb = sender as PictureBox;
            int pictureBoxDigit = Int32.Parse(currentpb.Name.Substring(currentpb.Name.Length - 1, 1)); //Get the number on the pictureBox using the last char in the string

            if (current_selected_ship == null) //Pick-up a ship
            {
                if (currentpb.BackColor == ship_Color) //The selected pictureBox contains a ship
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
            else //Drop a ship
            {
                saveShipStaticPosition();
                    
                //Reset variables containing the selected ship
                lastClickedGrid = 0;
                current_selected_ship = null;
            }
        }

        /// <summary>
        /// Move the ship in the grid, or place the ship at the current position
        /// </summary>
        /// <param name="sender">Sender object</param>
        void rightClickEvent(object sender)
        {
            var currentpb = sender as PictureBox;
            int pictureBoxDigit = Int32.Parse(currentpb.Name.Substring(currentpb.Name.Length - 1, 1)); //Get the number on the pictureBox using the last char in the string

            if (current_selected_ship == null)
            {
                if (currentpb.BackColor == ship_Color) //The double-clicked pictureBox contains a ship
                {
                    shipLocation.TryGetValue(pictureBoxDigit, out current_selected_ship);
                    rotateShip(current_selected_ship);
                    saveShipStaticPosition();

                    //Reset variables containing the selected ship
                    current_selected_ship = null;
                }
            }
        }

        /// <summary>
        /// Rotate a ship by 90 degrees
        /// </summary>
        /// <param name="shipname">Current selected ship</param>
        void rotateShip(string shipname)
        {
            if(shipname == destroyer) 
            {
                if(destroyer_array[destroyer_array.Length - 1] == 501) //Check ship's current orientation
                {
                    #region Horizontal -> vertical
                    if (destroyer_array[0] + (destroyer_array.Length - 2 * GRID_ROW_COLUMN_MAXSIZE) <= getColumnMax(destroyer_array[0]))
                    {
                        //Clear ship from grid
                        for (int x = 0; x < destroyer_array.Length - 1; x++)
                        {
                            if (gridMapping.TryGetValue(destroyer_array[x], out gridMapping_value))
                            {
                                seaGrid[Int32.Parse(gridMapping_value.Substring(0, 1)), Int32.Parse(gridMapping_value.Substring(1, 1))].BackColor = none_ship_Color;
                            }
                        }

                        for (int x = 1; x < destroyer_array.Length - 1; x++)
                        {
                            destroyer_array[x] = destroyer_array[x - 1] + GRID_ROW_COLUMN_MAXSIZE;
                        }

                        //Load ship onto grid
                        for (int x = 0; x < destroyer_array.Length - 1; x++)
                        {
                            if (gridMapping.TryGetValue(destroyer_array[x], out gridMapping_value))
                            {
                                seaGrid[Int32.Parse(gridMapping_value.Substring(0, 1)), Int32.Parse(gridMapping_value.Substring(1, 1))].BackColor = ship_Color;
                            }
                        }

                        destroyer_array[destroyer_array.Length - 1] = 500; //Update the ship's orientation
                    }
                    #endregion
                }
                else
                {
                    #region Vertical -> horizontal
                    if (destroyer_array[0] + (destroyer_array.Length - 2) <= getRowMax(destroyer_array[0]))
                    {
                        //Clear ship from grid
                        for (int x = 0; x < destroyer_array.Length - 1; x++)
                        {
                            if (gridMapping.TryGetValue(destroyer_array[x], out gridMapping_value))
                            {
                                seaGrid[Int32.Parse(gridMapping_value.Substring(0, 1)), Int32.Parse(gridMapping_value.Substring(1, 1))].BackColor = none_ship_Color;
                            }
                        }

                        for (int x = 1; x < destroyer_array.Length - 1; x++)
                        {
                            destroyer_array[x] = destroyer_array[x - 1] + 1;
                        }

                        //Load ship onto grid
                        for (int x = 0; x < destroyer_array.Length - 1; x++)
                        {
                            if (gridMapping.TryGetValue(destroyer_array[x], out gridMapping_value))
                            {
                                seaGrid[Int32.Parse(gridMapping_value.Substring(0, 1)), Int32.Parse(gridMapping_value.Substring(1, 1))].BackColor = ship_Color;
                            }
                        }

                        destroyer_array[destroyer_array.Length - 1] = 501; //Update the ship's orientation
                    }
                    #endregion
                }
            }
        }

        /// <summary>
        /// Save the ship's current position
        /// </summary>
        void saveShipStaticPosition()
        {
            //Remove the ship's previous location from the Dictionary object
            foreach (var item in shipLocation.Where(kvp => kvp.Value == current_selected_ship).ToList())
            {
                shipLocation.Remove(item.Key);
            }

            //Load the ship's current location into the Dictionary object
            if (current_selected_ship == destroyer) //Destroyer
            {
                for (int x = 0; x < destroyer_array.Length - 1; x++)
                {
                    shipLocation.Add(destroyer_array[x], destroyer);
                }
            }
        }

        /// <summary>
        /// Get the last point of a specific row
        /// </summary>
        /// <param name="currentpoint">Current position in the grid</param>
        /// <returns>Last point in the current row (3, 6, or 9)</returns>
        int getRowMax(int currentpoint)
        {
            string gridCoordinate = null;
            gridMapping.TryGetValue(currentpoint, out gridCoordinate);

            int x = (Int32.Parse(gridCoordinate.Substring(0, 1)) + 1) * GRID_ROW_COLUMN_MAXSIZE;
            return x;
        }

        /// <summary>
        /// Get the last point of a specific column
        /// </summary>
        /// <param name="currentpoint">Current position in the grid</param>
        /// <returns>Last point in the current row (7, 8, or 9)</returns>
        int getColumnMax(int currentpoint)
        {
            string gridCoordinate = null;
            gridMapping.TryGetValue(currentpoint, out gridCoordinate);
            int x = ((GRID_ROW_COLUMN_MAXSIZE - 1) * GRID_ROW_COLUMN_MAXSIZE) + (Int32.Parse(gridCoordinate.Substring(1, 1)) + 1);
            return x;
        }

        /// <summary>
        /// Get the difference between the last and the first point of a ship
        /// </summary>
        /// <param name="shipname">Current selected ship</param>
        /// <returns>The difference in integer</returns>
        int getShipSize(string shipname)
        {
            if (shipname == destroyer)
            {
                return destroyer_array.Length - 2;
            }
            else
                return 0;
        }

        /// <summary>
        /// Get the first point of a ship (pivot point)
        /// </summary>
        /// <param name="shipname">Current selected ship</param>
        /// <returns>First point of a ship</returns>
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
