using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FloppyFinch
{
    public class Game
    {
        private Canvas gameCanvas;
        public Bird Bird { get; private set; }
        private List<Pipe> pipes = new();
        private DispatcherTimer gameTimer;
        private Random rand = new();

        public Game(Canvas canvas)
        {
            gameCanvas = canvas;
            Bird = new Bird(gameCanvas);
            gameTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(20) };
            gameTimer.Tick += GameLoop;
            gameTimer.Start();
        }

        private void GameLoop(object sender, EventArgs e)
        {
            Bird.Update();

            if (rand.Next(100) < 2)
            {
                pipes.Add(new Pipe(gameCanvas));
            }

            for (int i = pipes.Count - 1; i >= 0; i--)
            {
                pipes[i].Update();
                if (pipes[i].IsOutOfBounds)
                {
                    pipes.RemoveAt(i);
                }

                if (pipes[i].CheckCollision(Bird))
                {
                    GameOver();
                }
            }

            if (Bird.IsOutOfBounds(gameCanvas.ActualHeight))
            {
                GameOver();
            }
        }

        private void GameOver()
        {
            gameTimer.Stop();
            MessageBox.Show("Game Over!");
        }
    }
}