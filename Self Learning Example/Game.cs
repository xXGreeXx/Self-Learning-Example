using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Self_Learning_Example
{
    public partial class Game : Form
    {
        float tileSize = 30;
        float[] tiles;
        int widthOfTileArray = 34;
        int heightOfTileArray = 21;

        float player1Index = 0;
        float player1Rotation = 0;
        NeuralNetwork player1Brain = new NeuralNetwork();

        float player2Index = 0;
        float player2Rotation = 0;
        NeuralNetwork player2Brain = new NeuralNetwork();

        List<float> apples = new List<float>();

        public static Random r { get; } = new Random();
        Boolean started = false;

        //initialize
        public Game()
        {
            InitializeComponent();

            //set form not minimizable
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            //generate tile array
            tiles = new float[widthOfTileArray * heightOfTileArray];
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i] = 0.0F;
            }

            //put players in position
            player1Index = 0;
            player2Index = widthOfTileArray * heightOfTileArray - 1;

            //add base apple
            apples.Add(r.Next(0, widthOfTileArray * heightOfTileArray));

            //start main loop
            updateTimer.Start();
            physicsTimerUpdate.Start();
        }

        //refresh canvas
        private void updateTimer_Tick(object sender, EventArgs e)
        {
            canvas.Refresh();
        }

        //rendering engine
        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int width = canvas.Width;
            int height = canvas.Height;

            //draw grid
            float x = 0;
            float y = 0;
            for (int i = 0; i < tiles.Length; i++)
            {
                g.DrawRectangle(Pens.White, x, y, tileSize, tileSize);

                //draw players
                if (i == player1Index)
                {
                    g.FillRectangle(Brushes.Red, x + 1, y + 1, tileSize - 1, tileSize - 1);
                }
                if (i == player2Index)
                {
                    g.FillRectangle(Brushes.Red, x + 1, y + 1, tileSize - 1, tileSize - 1);
                }

                //draw apples
                foreach (float apple in apples)
                {
                    if (i == apple)
                    {
                        g.FillRectangle(Brushes.Green, x + 1, y + 1, tileSize - 1, tileSize - 1);
                    }
                }

                x += tileSize;
                if (x >= widthOfTileArray * tileSize)
                {
                    x = 0;
                    y += tileSize;
                }
            }

            //draw text if not started
            if (!started)
            {
                Font f = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold);
                g.DrawString("Click Anywhere To Begin", f, Brushes.Green, width / 2 - g.MeasureString("Click Anywhere To Begin", f).Width / 2, 150);
            }
        }

        //update physics
        private void physicsTimerUpdate_Tick(object sender, EventArgs e)
        {
            if (started)
            {

            }
        }

        //canvas mouse down
        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            int x = e.X;
            int y = e.Y;

            if (!started)
            {
                started = true;
            }
        }
    }
}
