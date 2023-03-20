using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace TetrisRPS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Array containg the tile images
        // Order goes first with the empty tile then corresponds to each blocks ID
        private readonly ImageSource[] tileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("assets/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/TileRed.png", UriKind.Relative))
        };

        // Array contains full picture of the blocks
        // Used in holding and upcoming block 
        // The order matches each blocks ID
        private readonly ImageSource[] blockImages = new ImageSource[]
        {
            new BitmapImage(new Uri("assets/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/Block-Z.png", UriKind.Relative))
        };

        // For each of the game grid cells there is and image control
        private readonly Image[,] firstImageControls;
        private readonly Image[,] secondImageControls;

        private GameState firstGameState = new GameState();
        private GameState secondGameState = new GameState();

        DispatcherTimer timer = new DispatcherTimer();

        GameNetwork gameNetwork = new GameNetwork();
        public const int Tick = 180;

        public MainWindow()
        {
            InitializeComponent();
            firstImageControls = SetupGameCanvas(firstGameState.GameGrid, firstCanvas);
            secondImageControls = SetupGameCanvas(secondGameState.GameGrid, secondCanvas);
            timer.Tick += Game_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(500);

            gameNetwork.OnEventReceived += GameNetwork_OnEventReceived;
        }

        private void GameNetwork_OnEventReceived(int e)
        {
            if(e == Tick) secondGameState.MoveBlockDown();
            secondGameState.MoveBlock(e);
            if (!secondGameState.IsGameOver) gameNetwork.BeginRead();
        }

        private void Game_Tick(object? sender, EventArgs e)
        {
            if (!firstGameState.IsGameOver && !secondGameState.IsGameOver)
            {
                firstGameState.MoveBlockDown();
                Draw(firstGameState, firstImageControls);
                DrawHeldBlock(firstGameState.HeldBlock);
                gameNetwork.SendData(Tick);
            }
            else
            {
                gameOverScreen.Visibility = Visibility.Visible;
                if (firstGameState.IsGameOver)
                    playerWinText.Text = "You Lose";
                else
                    playerWinText.Text = "You Win";


                if (!isSingle)
                {
                    if (isHost)
                        gameNetwork.StopListener();
                    else
                        gameNetwork.StopClient();
                }



                timer.Stop();
            }
            Draw(secondGameState, secondImageControls);
        }

        private Image[,] SetupGameCanvas(GameGrid grid, Canvas canvas)
        {
            Image[,] ImageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };
                    Canvas.SetTop(imageControl, (r - 2) * cellSize);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    canvas.Children.Add(imageControl);
                    ImageControls[r, c] = imageControl;
                }
            }
            return ImageControls;
        }

        private void DrawGrid(GameGrid grid, Image[,] control)
        {
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r, c];
                    control[r, c].Source = tileImages[id];
                }
            }
        }

        private void DrawBlock(Block block, Image[,] control)
        {
            foreach (Position p in block.TilePositions())
            {
                control[p.Row, p.Column].Source = tileImages[block.Id];
            }
        }

        private void DrawHeldBlock(Block heldBlock)
        {
            if (heldBlock == null)
            {
                holdImage.Source = blockImages[0];
            }
            else
            {
                holdImage.Source = blockImages[heldBlock.Id];
            }
        }

        private void Draw(GameState gameState, Image[,] control)
        {
            DrawGrid(gameState.GameGrid, control);
            DrawBlock(gameState.currentBlock, control);
        }

        // Detecting player input
        private void WindowKeyDown(object sender, KeyEventArgs e)
        {
            if (firstGameState.IsGameOver || secondGameState.IsGameOver)
                return;
            firstGameState.MoveBlock((int)e.Key);
            gameNetwork.SendData((int)e.Key);
            Draw(firstGameState, firstImageControls);
            DrawHeldBlock(firstGameState.HeldBlock);
            Draw(secondGameState, secondImageControls);
        }

        private void End_Click(object sender, RoutedEventArgs e)
        {
            gameOverScreen.Visibility = Visibility.Hidden;
            IPInput.IsEnabled = true;
            StartButton.IsEnabled = true;
            IsSinglePlayer.IsEnabled = true;

            firstGameState = new GameState();
            secondGameState = new GameState();

            Draw(firstGameState, firstImageControls);
            DrawHeldBlock(firstGameState.HeldBlock);
            Draw(secondGameState, secondImageControls);
        }

        private bool isSingle = true;
        private bool isHost = false;
        private void Star_Click(object sender, RoutedEventArgs e)
        {
            IPInput.IsEnabled = false;
            StartButton.IsEnabled = false;
            IsSinglePlayer.IsEnabled = false;

            if (IsSinglePlayer.IsChecked is bool a) 
            {
                isSingle = a;
                if (!a) 
                {
                    if (IPInput.Text == "")
                    {
                        gameNetwork.StartListener();
                        isHost = true;
                    }
                    else
                    {
                        gameNetwork.StartClient(IPInput.Text);
                        isHost = false;
                    }
                    gameNetwork.BeginRead();
                }
            }
            
            timer.Start();
        }
    }
}
