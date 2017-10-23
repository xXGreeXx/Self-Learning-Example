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

        int player1Index = 0;
        int player1Rotation = 0;
        NeuralNetwork player1Brain = new NeuralNetwork();

        int player2Index = 0;
        int player2Rotation = 0;
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
            Brush shadowGray = new SolidBrush(Color.FromArgb(120, 130, 130, 130));
            Font f = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);

            #region Grid
            //draw grid
            float x = 0;
            float y = 0;
            for (int i = 0; i < tiles.Length; i++)
            {
                //g.DrawRectangle(Pens.DarkGray, x, y, tileSize, tileSize);

                //draw players
                if (i == player1Index)
                {
                    g.FillRectangle(Brushes.Red, x + 1, y + 1, tileSize - 1, tileSize - 1);

                    //draw field of view
                    if (player1Rotation == 0)
                    {
                        g.FillRectangle(shadowGray, (x - 1 * tileSize) + 1, (y - 1 * tileSize) + 1, tileSize - 1, tileSize - 1);
                        g.FillRectangle(shadowGray, (x - 0 * tileSize) + 1, (y - 1 * tileSize) + 1, tileSize - 1, tileSize - 1);
                        g.FillRectangle(shadowGray, (x + 1 * tileSize) + 1, (y - 1 * tileSize) + 1, tileSize - 1, tileSize - 1);
                    }
                    if (player1Rotation == 90)
                    {
                        g.FillRectangle(shadowGray, (x + 1 * tileSize) + 1, (y - 1 * tileSize) + 1, tileSize - 1, tileSize - 1);
                        g.FillRectangle(shadowGray, (x + 1 * tileSize) + 1, (y - 0 * tileSize) + 1, tileSize - 1, tileSize - 1);
                        g.FillRectangle(shadowGray, (x + 1 * tileSize) + 1, (y + 1 * tileSize) + 1, tileSize - 1, tileSize - 1);
                    }
                    if (player1Rotation == 180)
                    {
                        g.FillRectangle(shadowGray, (x - 1 * tileSize) + 1, (y + 1 * tileSize) + 1, tileSize - 1, tileSize - 1);
                        g.FillRectangle(shadowGray, (x - 0 * tileSize) + 1, (y + 1 * tileSize) + 1, tileSize - 1, tileSize - 1);
                        g.FillRectangle(shadowGray, (x + 1 * tileSize) + 1, (y + 1 * tileSize) + 1, tileSize - 1, tileSize - 1);
                    }
                    if (player1Rotation == 270)
                    {
                        g.FillRectangle(shadowGray, (x - 1 * tileSize) + 1, (y - 1 * tileSize) + 1, tileSize - 1, tileSize - 1);
                        g.FillRectangle(shadowGray, (x - 1 * tileSize) + 1, (y - 0 * tileSize) + 1, tileSize - 1, tileSize - 1);
                        g.FillRectangle(shadowGray, (x - 1 * tileSize) + 1, (y + 1 * tileSize) + 1, tileSize - 1, tileSize - 1);
                    }
                }
                if (i == player2Index)
                {
                    g.FillRectangle(Brushes.Red, x + 1, y + 1, tileSize - 1, tileSize - 1);

                    //draw field of view
                    if (player1Rotation == 0)
                    {
                        g.FillRectangle(shadowGray, (x - 1 * tileSize) + 1, (y - 1 * tileSize) + 1, tileSize - 1, tileSize - 1);
                        g.FillRectangle(shadowGray, (x - 0 * tileSize) + 1, (y - 1 * tileSize) + 1, tileSize - 1, tileSize - 1);
                        g.FillRectangle(shadowGray, (x + 1 * tileSize) + 1, (y - 1 * tileSize) + 1, tileSize - 1, tileSize - 1);
                    }
                    if (player1Rotation == 90)
                    {
                        g.FillRectangle(shadowGray, (x + 1 * tileSize) + 1, (y - 1 * tileSize) + 1, tileSize - 1, tileSize - 1);
                        g.FillRectangle(shadowGray, (x + 1 * tileSize) + 1, (y - 0 * tileSize) + 1, tileSize - 1, tileSize - 1);
                        g.FillRectangle(shadowGray, (x + 1 * tileSize) + 1, (y + 1 * tileSize) + 1, tileSize - 1, tileSize - 1);
                    }
                    if (player1Rotation == 180)
                    {
                        g.FillRectangle(shadowGray, (x - 1 * tileSize) + 1, (y + 1 * tileSize) + 1, tileSize - 1, tileSize - 1);
                        g.FillRectangle(shadowGray, (x - 0 * tileSize) + 1, (y + 1 * tileSize) + 1, tileSize - 1, tileSize - 1);
                        g.FillRectangle(shadowGray, (x + 1 * tileSize) + 1, (y + 1 * tileSize) + 1, tileSize - 1, tileSize - 1);
                    }
                    if (player1Rotation == 270)
                    {
                        g.FillRectangle(shadowGray, (x - 1 * tileSize) + 1, (y - 1 * tileSize) + 1, tileSize - 1, tileSize - 1);
                        g.FillRectangle(shadowGray, (x - 1 * tileSize) + 1, (y - 0 * tileSize) + 1, tileSize - 1, tileSize - 1);
                        g.FillRectangle(shadowGray, (x - 1 * tileSize) + 1, (y + 1 * tileSize) + 1, tileSize - 1, tileSize - 1);
                    }
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
            #endregion

            #region NNPreview
            //draw neural network preview
            g.FillRectangle(Brushes.Gray, 0, y, width, height - y);

            //1
            int xOfPreview1 = 0;
            int yOfPreview1 = 0;

            int layerIndex = 0;
            foreach (List<Perceptron> layer in player1Brain.perceptrons)
            {
                yOfPreview1 = 0;
                foreach (Perceptron neuron in layer)
                {
                    g.FillEllipse(Brushes.Black, xOfPreview1, y + yOfPreview1 + 125 - (layer.Count * 55) / 2, 40, 40);

                    int weightIndex = 0;
                    foreach (float weight in neuron.weights)
                    {
                        float conWeight = weight < 25 ? weight : 25;

                        float targetY = y + 125 - (player1Brain.perceptrons[layerIndex - 1].Count * 55) / 2 + 20 + (weightIndex * 55);
                        if (weight < 0) g.DrawLine(new Pen(Brushes.Black, conWeight), xOfPreview1, y + yOfPreview1 + 125 - (layer.Count * 55) / 2 + 20, xOfPreview1 - 125 + 40, targetY);
                        else g.DrawLine(new Pen(Brushes.White, conWeight), xOfPreview1, y + yOfPreview1 + 125 - (layer.Count * 55) / 2 + 20, xOfPreview1 - 125 + 40, targetY);
                        g.DrawString(neuron.lastOut.ToString(), f, Brushes.White, xOfPreview1, y + yOfPreview1 + 125 - (layer.Count * 55) / 2);

                        weightIndex++;
                    }

                    yOfPreview1 += 55;
                }

                xOfPreview1 += 125;
                layerIndex++;
            }

            //2
            xOfPreview1 = 732;
            yOfPreview1 = 0;

            layerIndex = 0;
            foreach (List<Perceptron> layer in player2Brain.perceptrons)
            {
                yOfPreview1 = 0;
                foreach (Perceptron neuron in layer)
                {
                    g.FillEllipse(Brushes.Black, xOfPreview1, y + yOfPreview1 + 125 - (layer.Count * 55) / 2, 40, 40);

                    int weightIndex = 0;
                    foreach (float weight in neuron.weights)
                    {
                        float conWeight = weight < 25 ? weight : 25;

                        float targetY = y + 125 - (player1Brain.perceptrons[layerIndex - 1].Count * 55) / 2 + 20 + (weightIndex * 55);
                        if (weight < 0) g.DrawLine(new Pen(Brushes.Black, conWeight), xOfPreview1, y + yOfPreview1 + 125 - (layer.Count * 55) / 2 + 20, xOfPreview1 - 125 + 40, targetY);
                        else g.DrawLine(new Pen(Brushes.White, conWeight), xOfPreview1, y + yOfPreview1 + 125 - (layer.Count * 55) / 2 + 20, xOfPreview1 - 125 + 40, targetY);
                        g.DrawString(neuron.lastOut.ToString(), f, Brushes.White, xOfPreview1, y + yOfPreview1 + 125 - (layer.Count * 55) / 2);

                        weightIndex++;
                    }

                    yOfPreview1 += 55;
                }

                xOfPreview1 += 125;
                layerIndex++;
            }
            #endregion

            //draw text if not started
            if (!started)
            {
                Font f2 = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold);
                g.DrawString("Click Anywhere To Begin", f2, Brushes.Green, width / 2 - g.MeasureString("Click Anywhere To Begin", f2).Width / 2, 150);
            }
        }

        //update physics
        private void physicsTimerUpdate_Tick(object sender, EventArgs e)
        {
            if (started)
            {
                player1Index = simulateNeuralNetwork(player1Brain, player1Index, player2Index, out player1Rotation);
                player2Index = simulateNeuralNetwork(player2Brain, player2Index, player1Index, out player2Rotation);

                int addIndex = r.Next(0, widthOfTileArray * heightOfTileArray);
                if (!apples.Contains(addIndex))
                {
                    apples.Add(addIndex);
                }
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

        //simulate nn
        private int simulateNeuralNetwork(NeuralNetwork brain, int positionIndex, int enemyPositionIndex, out int playerRotation)
        {
            //get field of view
            float[] inputs = new float[3];
            float[] fieldOfView = new float[3];

            if (player1Rotation == 0)
            {
                fieldOfView = new float[3];
                fieldOfView[0] = (player1Index + -1 * widthOfTileArray) - 1;
                fieldOfView[1] = (player1Index + -1 * widthOfTileArray) - 0;
                fieldOfView[2] = (player1Index + -1 * widthOfTileArray) + 1;
            }
            if (player1Rotation == 90)
            {
                fieldOfView = new float[3];
                fieldOfView[0] = (player1Index + 1 * widthOfTileArray) + 1;
                fieldOfView[1] = (player1Index + 0 * widthOfTileArray) + 1;
                fieldOfView[2] = (player1Index + -1 * widthOfTileArray) + 1;
            }
            if (player1Rotation == 180)
            {
                fieldOfView = new float[3];
                fieldOfView[0] = (player1Index + 1 * widthOfTileArray) - 1;
                fieldOfView[1] = (player1Index + 1 * widthOfTileArray) - 0;
                fieldOfView[2] = (player1Index + 1 * widthOfTileArray) + 1;
            }
            if (player1Rotation == 270)
            {
                fieldOfView = new float[3];
                fieldOfView[0] = (player1Index + 1 * widthOfTileArray) - 1;
                fieldOfView[1] = (player1Index + 0 * widthOfTileArray) - 1;
                fieldOfView[2] = (player1Index + -1 * widthOfTileArray) - 1;
            }

            inputs[0] = 0.0F;
            inputs[1] = 0.0F;
            inputs[2] = 0.0F;

            foreach (float apple in apples)
            {
                if (apple == fieldOfView[0])
                {
                    inputs[0] = 0.5F;
                }
                if (apple == fieldOfView[1])
                {
                    inputs[1] = 0.5F;
                }
                if (apple == fieldOfView[2])
                {
                    inputs[2] = 0.5F;
                }
            }

            if (fieldOfView[0] == enemyPositionIndex)
            {
                inputs[0] = 1.0F;
            }
            if (fieldOfView[1] == enemyPositionIndex)
            {
                inputs[1] = 1.0F;
            }
            if (fieldOfView[2] == enemyPositionIndex)
            {
                inputs[2] = 1.0F;
            }

            //feed to nn
            float[] output = brain.ForwardPropagate(inputs);

            //move with output
            //Console.WriteLine(output[0] + " " + output[1]);
            positionIndex += output[0] < 1 ? -1 : 1;
            positionIndex += output[1] < 1 ? -1 * widthOfTileArray : 1 * widthOfTileArray;

            playerRotation = output[0] < 1 ? 270 : 90;
            playerRotation = output[1] < 1 ? 0 : 180;

            //check if it went off the edge
            int posX = positionIndex / heightOfTileArray;
            int posY = positionIndex / widthOfTileArray;
            if (posX < 0) posX = widthOfTileArray;
            if (posX > widthOfTileArray) posX = 0;
            if (posY < 0) posY = heightOfTileArray;
            if (posY > heightOfTileArray) posY = 0;
            positionIndex = posX + posY * widthOfTileArray;

            //handle collisions
            for (int appleIndex = 0; appleIndex < apples.Count; appleIndex++)
            {
                if (apples[appleIndex] == positionIndex)
                {
                    apples.RemoveAt(appleIndex);
                }
            }


            //train nn
            //TODO\\
            float targetX = output[0] + r.Next(-1, 1);
            float targetY = output[1] + r.Next(-1, 1);

            brain.BackPropagate(inputs, new float[] { targetX, targetY });
            

            //return new position of player
            return positionIndex;
        }
    }
}
