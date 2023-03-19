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
        private readonly Image[,] imageControls;

        private GameState gameState = new GameState();
        
        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(gameState.GameGrid);
            GameLoop();
        }

        private Image[,] SetupGameCanvas(GameGrid grid) 
        {
            Image[,] ImageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for (int r = 0; r < grid.Rows; r++) {
                for (int c = 0; c < grid.Columns; c++) {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };
                    Canvas.SetTop(imageControl, (r - 2) * cellSize);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    firstCanvas.Children.Add(imageControl);
                    ImageControls[r, c] = imageControl;
                }
            }
            return ImageControls;
    }

        private void DrawGrid(GameGrid grid) {
            for (int r = 0; r < grid.Rows; r++) {
                for (int c = 0; c < grid.Columns; c++) { 
                    int id = grid[r, c];
                    imageControls[r, c].Source = tileImages[id];
                }
            }
        }

        private void DrawBlock(Block block) {
            foreach (Position p in block.TilePositions()) {
                imageControls[p.Row, p.Column].Source = tileImages[block.Id];
            }
        }

        private void DrawHeldBlock(Block heldBlock) {
            if (heldBlock == null)
            {
                holdImage.Source = blockImages[0];
            }
            else {
                holdImage.Source = blockImages[heldBlock.Id];
            }
        }

        private void Draw(GameState gameState) {
            DrawGrid(gameState.GameGrid);
            DrawBlock(gameState.currentBlock);
            DrawHeldBlock(gameState.HeldBlock);
        }

        // Gameloop that draws the game state.
        private async Task GameLoop() {
            Draw(gameState);
            while (!gameState.IsGameOver) {
                await Task.Delay(500);
                gameState.MoveBlockDown();
                Draw(gameState);
            }
            gameOverScreen.Visibility = Visibility.Visible;
            playerWinText.Text = "Game Over";           
        }
        // Detecting player input
        // Function is called inside the Window
        private void WindowKeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.IsGameOver) {
                return;
            }
            gameState.MoveBlock((int)e.Key);
            Draw(gameState);
            }
            Draw(gameState);
        }

        // Draw the the game grid on the canvas
        // The function is called inside the canvas using the Loaded function
        private async void GameCanvasLoaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();
        }

        // Play again button
        // The button appears in the overlay once the game is over
        // Honestly not sure how this one is going to work with two players having to press it at the same time
        

    }
}
