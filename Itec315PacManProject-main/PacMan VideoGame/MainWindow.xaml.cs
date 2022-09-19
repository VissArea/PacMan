using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PacMan_VideoGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTime = new DispatcherTimer();

        bool moveLeft, moveRight, moveDown, moveUp, notLeft, notRight, notUp, notDown;

        Rect pacHitBox, greenAAHitBox, greenABHitBox;

        int pacSpeed = 7;
        int ghostSpeed = 8;
        int ghostMoves = 580;
        int ghostMove = 396;
        int currentGhost;
        int score = 0;
        public MainWindow()
        {
            InitializeComponent();
            GameStart();
        }

        private void CanvasKeyDown(object sender, KeyEventArgs e)
        {
            //~~~ PacMan Movements ~~~
            //Move Left
            if(e.Key == Key.Left && notLeft == false)
            {
                notRight = notUp = notDown = false;
                moveRight = moveUp = moveDown = false;
                moveLeft = true;

                missPac.RenderTransform = new RotateTransform(-180, missPac.Width / 2 , missPac.Height / 2);
            }
            //Move Right
            if (e.Key == Key.Right && notRight == false)
            {
                notLeft = notUp = notDown = false;
                moveLeft = moveUp = moveDown = false;
                moveRight = true;

                missPac.RenderTransform = new RotateTransform(0, missPac.Width / 2, missPac.Height / 2);
            }
            //Move Down
            if (e.Key == Key.Down && notDown == false)
            {
                notRight = notUp = notLeft = false;
                moveRight = moveUp = moveLeft = false;
                moveDown = true;

                missPac.RenderTransform = new RotateTransform(90, missPac.Width / 2, missPac.Height / 2);
            }
            //Move Up
            if (e.Key == Key.Up && notUp == false)
            {
                notRight = notLeft = notDown = false;
                moveRight = moveLeft = moveDown = false;
                moveUp = true;

                missPac.RenderTransform = new RotateTransform(-90, missPac.Width / 2, missPac.Height / 2);
            }
        }
        private void GameStart()
        {
            pacCanvas.Focus();

            gameTime.Tick += gameLoop;
            gameTime.Interval = TimeSpan.FromMilliseconds(20);
            gameTime.Start();
            currentGhost = ghostMove;

            ImageBrush pinkGhostPic = new ImageBrush();
            pinkGhostPic.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/pinkGhost.png"));
            pinkGhost.Fill = pinkGhostPic;
            

            ImageBrush redGhostPic = new ImageBrush();
            redGhostPic.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/redGhost.png"));
            redGhost.Fill = redGhostPic;

            ImageBrush orangeGhostPic = new ImageBrush();
            orangeGhostPic.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/orangeGhost.png"));
            orangeGhost.Fill = orangeGhostPic;

            ImageBrush blueGhostPic = new ImageBrush();
            blueGhostPic.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/tealGhost.png"));
            tealGhost.Fill = blueGhostPic;

            ImageBrush alienGhostPic = new ImageBrush();
            alienGhostPic.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/alienGhost.png"));
            greenAlienA.Fill = alienGhostPic;
            greenAlienB.Fill = alienGhostPic;

            ImageBrush missPacPic = new ImageBrush();
            missPacPic.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/bluePackMan.png"));
            missPac.Fill = missPacPic;

            ImageBrush arrowPic = new ImageBrush();
            arrowPic.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/arrows.jpg"));
            arrows.Fill = arrowPic;


            // DID NOT use this because I would have had to name each coin
            ImageBrush coinPic = new ImageBrush();
            coinPic.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/coinImage.png"));
            //coin.Fill = coinPic;
           


        }

        private void gameLoop(object sender, EventArgs e)
        {
            txtScore.Content = "Score:  " + score;

            if (moveLeft)
            {
                Canvas.SetLeft(missPac, Canvas.GetLeft(missPac) - pacSpeed);
            }
            if (moveRight)
            {
                Canvas.SetLeft(missPac, Canvas.GetLeft(missPac) + pacSpeed);
            }
            if (moveUp)
            {
                Canvas.SetTop(missPac, Canvas.GetTop(missPac) - pacSpeed);
            }
            if (moveDown)
            {
                Canvas.SetTop(missPac, Canvas.GetTop(missPac) + pacSpeed);
            }


            if(moveDown && Canvas.GetTop(missPac) + 90 > Application.Current.MainWindow.Height)
            {
                moveDown = false;
                notDown = true;
            }
            if (moveUp && Canvas.GetTop(missPac) < 1)
            {
                moveUp = false;
                notUp = true;
            }
            if (moveLeft && Canvas.GetLeft(missPac) < 1)
            {
                moveLeft = false;
                notLeft = true;
            }
            if (moveRight && Canvas.GetLeft(missPac) + 60 > Application.Current.MainWindow.Width)
            {
                moveRight = false;
                notRight = true;
            }

                pacHitBox = new Rect(Canvas.GetLeft(missPac), Canvas.GetTop(missPac), missPac.Width, missPac.Height);

                foreach (var x in pacCanvas.Children.OfType<Rectangle>())
                {
                Rect hitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                if ((string)x.Tag == "wall")
                {
                    if (moveLeft == true && pacHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(missPac, Canvas.GetLeft(missPac) + 15);
                        notLeft = true;
                        moveLeft = true;
                    }
                    if (moveRight == true && pacHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(missPac, Canvas.GetLeft(missPac) - 10);
                        notRight = true;
                        moveRight = true;
                    }
                    if (moveUp == true && pacHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(missPac, Canvas.GetTop(missPac) + 15);
                        notUp = true;
                        moveUp = true;
                    }
                    if (moveDown == true && pacHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(missPac, Canvas.GetTop(missPac) - 10);
                        notDown = true;
                        moveDown = true;
                    }
                }

                if ((string)x.Tag == "coin")
                    {
                        if( pacHitBox.IntersectsWith(hitBox) && x.Visibility == Visibility.Visible)
                        {
                            x.Visibility = Visibility.Hidden;
                            score += 5;
                        }
                    }
                if((string)x.Tag == "sideGhost")
                {
                    if (pacHitBox.IntersectsWith(hitBox))
                    {
                        GameEnd("You ran into a Ghost, click Ok to play again!");
                    }

                    if(x.Name.ToString() == "pinkGhost")
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) - ghostSpeed );
                    }
                    else
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) + ghostSpeed);
                    }
                    currentGhost--;
                    if(currentGhost < 1)
                    {
                        currentGhost = ghostMoves;
                        ghostSpeed = -ghostSpeed;
                    }
                }
                if ((string)x.Tag == "upGhost")
                {
                    if (pacHitBox.IntersectsWith(hitBox))
                    {
                        GameEnd("You ran into a Ghost, click Ok to play again!");
                    }

                    if (x.Name.ToString() == "greenAlienA" || x.Name.ToString() == "tealGhost")
                    {
                        Canvas.SetTop(x, Canvas.GetTop(x) - ghostSpeed);
                    }
                    else
                    {
                        Canvas.SetTop(x, Canvas.GetTop(x) + ghostSpeed);
                    }
                    currentGhost--;
                    if (currentGhost < 1)
                    {
                        currentGhost = ghostMove;
                        ghostSpeed = -ghostSpeed;
                    }
                }
               if(score == 1100)
                {
                    GameEnd("You have won the game! Click okay to play again.");
                }

            }
        }


        private void GameEnd(string message)
        {
            gameTime.Stop();
            MessageBox.Show(message, "Visser's Blue-PacMan");
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}
