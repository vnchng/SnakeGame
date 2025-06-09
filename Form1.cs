using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;

using System.Drawing.Imaging;

namespace Snake_Game
{
    public partial class SnakeGame : Form
    {
        private List<Circle> Snake = new List<Circle>(); // List with Type as Circle, named Snake
        // List is like an array but allows us to do more things with the items inside
        // Anything new that happens to the Snake and the Snake body, head will be added to this list.

        private Circle food = new Circle();
        // referring to the class, create new insance of Circle, using the constructor thats being called at the end.
        // When a new instance of a class, everything inside the class will be referenced inside the memory.

        int maxWidth;
        int maxHeight;

        int score;
        int highScore;

        Random rand = new Random(); // create a random number between two values

        bool goLeft, goRight, goUp, goDown;

        public SnakeGame()
        {
            InitializeComponent();

            new Settings(); // Because the instance already contains the static variables, it can be called directly
        }


        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && Settings.directions != "right")
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right && Settings.directions != "left")
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Up && Settings.directions != "down")
            {
                goUp = true;
            }
            if (e.KeyCode == Keys.Down && Settings.directions != "up")
            {
                goDown = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }
        }

        private void StartGame(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void TakeSnapshot(object sender, EventArgs e)
        {
            Label caption = new Label();
            caption.Text = "Score: " + score;
            caption.ForeColor = Color.LightBlue;
            caption.AutoSize = false;
            caption.Width = picCanvas.Width;
            caption.Height = 30;
            caption.TextAlign = ContentAlignment.MiddleCenter;
            picCanvas.Controls.Add(caption);
            
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "Snake Game Snapshot";
            dialog.DefaultExt = "jpg";
            dialog.Filter = "JPG Image File | *.jpg";
            dialog.ValidateNames = true;

            if (dialog.ShowDialog() == DialogResult.OK) 
            {
                int width = Convert.ToInt32(picCanvas.Width);
                int height = Convert.ToInt32(picCanvas.Height);

                Bitmap bmp = new Bitmap(width, height);
                picCanvas.DrawToBitmap(bmp, new Rectangle(0,0, width, height));
                bmp.Save(dialog.FileName, ImageFormat.Jpeg);
                picCanvas.Controls.Remove(caption);
            }
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            // setting the directions
            if (goLeft) 
            {
                Settings.directions = "left";
            }
            if (goRight)
            {
                Settings.directions = "right";
            }
            if (goUp)
            {
                Settings.directions = "up";
            }
            if (goDown) 
            {
                Settings.directions = "down";
            }
            // end of directions

            for (int i = Snake.Count - 1; i >= 0; i--) 
            {
                if (i == 0)
                {
                    switch (Settings.directions)
                    {
                        case "left":
                            Snake[i].X--;
                            break;
                        case "right":
                            Snake[i].X++;
                            break;
                        case "up":
                            Snake[i].Y--;
                            break;
                        case "down":
                            Snake[i].Y++;
                            break;
                    }

                    if (Snake[i].X < 0)
                    {
                        Snake[i].X = maxWidth;
                    }
                    if (Snake[i].X > maxWidth)
                    {
                        Snake[i].X = 0;
                    }
                    if (Snake[i].Y < 0)
                    {
                        Snake[i].Y = maxHeight;
                    }
                    if (Snake[i].Y > maxHeight)
                    {
                        Snake[i].Y = 0;
                    }

                    if (Snake[i].X == food.X && Snake[i].Y == food.Y) // Snack head, and food X and Y match
                    {
                        EatFood();
                    }

                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y) 
                        {
                            GameOver();
                        }
                    }


                }
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    // as it increments, the last body part follows the other body part
                    // in the list
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
            picCanvas.Invalidate(); // each tick its going to refresh the canvas
        }

        private void UpdatePictureBocGraphics(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics; // linking that paint event to e.canvas

            Brush snakeColour;

            for (int i = 0; i < Snake.Count; i++) // counts how many body parts the snake has
            {
                if (i == 0) // accessing the first index
                {
                    snakeColour = Brushes.Black;
                }
                else
                {
                    snakeColour = Brushes.DarkGreen;
                }
                // update X, and Y values, and draw it to the canvas
                canvas.FillEllipse(snakeColour, new Rectangle // snakeColour fills in dynamically.
                    (
                    Snake[i].X * Settings.Width, // Circle class, access to X & Y
                    Snake[i].Y * Settings.Height,
                    Settings.Width, Settings.Height // rectangle class accepts x,y width, height
                    
                    ));


            }

            canvas.FillEllipse(Brushes.DarkRed, new Rectangle 
            (
            food.X * Settings.Width, 
            food.Y * Settings.Height,
            Settings.Width, Settings.Height

            ));
        }

        private void txtScore_Click(object sender, EventArgs e)
        {

        }

        private void SnakeGame_Load(object sender, EventArgs e)
        {

        }

        private void RestartGame()
        {
            maxWidth = picCanvas.Width / Settings.Width - 1;
            // padding for the snake, so it doesn't go too close to the edge.
            // when its by the edge, it will appear on the other side of the screen
            maxHeight = picCanvas.Height / Settings.Height - 1;

            Snake.Clear(); // if theres any existing child inside of Snake List. It will clear it.

            playButton.Enabled = false;
            snapshotButton.Enabled = false;

            score = 0;
            txtScore.Text = "Score " + score;

            Circle head = new Circle {X=10, Y=5 }; // place it in the middle of screen
            Snake.Add(head);
            // The snake's head is added to an array, 
            // snake's head is index 0, adding head part to the list

            for (int i = 0; i < 5; i++)
            {
                Circle body = new Circle();
                Snake.Add(body);
            }

            food = new Circle {X = rand.Next(2,maxWidth), Y = rand.Next(2, maxHeight)};

            gameTimer.Start();
        }

        private void EatFood() 
        {
            score += 1;
            txtScore.Text = "Score: " + score;

            Circle body = new Circle // add new instance of the circle class
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y,

            };
            Snake.Add(body);

            food = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight) };
            
        }

        private void GameOver() 
        {
            gameTimer.Stop();
            playButton.Enabled = true;
            snapshotButton.Enabled = true;

            if (score > highScore)
            {
                highScore = score;

                txtHighScore.Text = "High Score: " + Environment.NewLine + highScore;
                txtHighScore.ForeColor = Color.Maroon;
                txtHighScore.TextAlign = ContentAlignment.MiddleCenter;
            }
        }
    }
}
