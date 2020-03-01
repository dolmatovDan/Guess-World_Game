using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace snake
{
    public partial class Form1 : Form
    {
        
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();
        private int maxXPos;
        private int maxYPos;

        public Form1()
        {
            InitializeComponent();

            new Settings();
            

            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();

            maxXPos = pbCanvas.Size.Width / Settings.Width;
            maxYPos = pbCanvas.Size.Height / Settings.Height;

            StartGame();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void StartGame()
        {
            
            lblGameOverfc.Visible = false;
            maxXPos = pbCanvas.Size.Width / Settings.Width;
            maxYPos = pbCanvas.Size.Height / Settings.Height;
            Snake.Clear();
            Circle head = new Circle();
            head.X = 10;
            head.Y = 5;
            
            Snake.Add(head);
            new Settings();
            label1.Text = Settings.Score.ToString();
            GenerateFood();
        }
        private void GenerateFood()
        {
            
            Random random = new Random();
            food = new Circle();
            food.X = random.Next(0, maxXPos);
            food.Y = random.Next(0, maxYPos);
        }

        private void MovePlayer()
        {
            for(int i = Snake.Count - 1; i >= 0; i--)
            {
                if(i == 0) // Если голова, то двигаем ее внужную сторону 
                {
                    switch (Settings.direction)
                    {
                        case Direction.Right:
                            Snake[i].X++;
                            break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                    }

                    if(Snake[i].X < 0 || Snake[i].Y < 0 || Snake[i].X >= maxXPos || Snake[i].Y >= maxYPos)
                    {
                        Die();
                    }
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            Die();
                        }
                    }
                    if (Snake[i].Y == food.Y && Snake[i].X == food.X)
                    {
                        Eat();
                    }
                }
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }

        }
        private void Die()
        {
            Settings.GameOver = true;
        }
        private void Eat()
        {
            Circle food = new Circle();
            food.X = Snake[Snake.Count - 1].X;
            food.Y = Snake[Snake.Count - 1].Y;

            Snake.Add(food);

            Settings.Score += Settings.Points;
            label1.Text = "Score: " + Settings.Score.ToString();

            GenerateFood();
        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            if (!Settings.GameOver) // Если прогирали
            {
                Brush snakeColor;
                for (int i = 0; i < Snake.Count; i++)
                {
                    if(i == 0) 
                    {
                        snakeColor = Brushes.Red;
                    }
                    else
                    {
                        snakeColor = Brushes.Aqua;
                    }

                    canvas.FillEllipse(snakeColor, 
                        new Rectangle(Snake[i].X * Settings.Width,
                            Snake[i].Y * Settings.Height, 
                            Settings.Height,
                            Settings.Width));

                    canvas.FillEllipse(Brushes.LightGreen,
                        new Rectangle(food.X * Settings.Width,
                            food.Y * Settings.Height,
                            Settings.Height,
                            Settings.Width));

                } // end for
            } // end if
            else
            {
                string gameOver = "Game Over \nYouyr final score is: " + Settings.Score + "\nPress Enter to try again";
                lblGameOverfc.Text = gameOver;
                lblGameOverfc.Visible = true;
            }
        }
        private void UpdateScreen(object sender, EventArgs e)
        {
            if (Settings.GameOver)
            {
                if(Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if (Input.KeyPressed(Keys.Up) && Settings.direction != Direction.Down)
                {
                    Settings.direction = Direction.Up;
                }
                else if (Input.KeyPressed(Keys.Right) && Settings.direction != Direction.Left)
                {
                    Settings.direction = Direction.Right;
                }
                else if (Input.KeyPressed(Keys.Left) && Settings.direction != Direction.Right)
                {
                    Settings.direction = Direction.Left;
                }
                else if (Input.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
                {
                    Settings.direction = Direction.Down;
                }

                MovePlayer();
            }
            pbCanvas.Invalidate();
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }
    }
}
