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

        // TODO - GameState object
        

        public MainWindow()
        {
            InitializeComponent();
            //this is comment
        }

        // TODO
        // Method for setting up the image controls on the canvas
        // The image controls array should contain 22 rows and 10 collumns (10x22)
        
        // TOOD
        // Drawing the game grid

        // TODO
        // Draw the current block

        // TODO 
        // Combine the drawing of the game grid with drawing the current block so it is done at the same time

        // Detecting player input
        // Function is called inside the Window
        private void WindowKeyDown(object sender, KeyEventArgs e)
        {

        }

        // Draw the the game grid on the canvas
        // The function is called inside the canvas using the Loaded function
        private void GameCanvasLoaded(object sender, RoutedEventArgs e)
        {

        }

        // Play again button
        // The button appears in the overlay once the game is over
        // Honestly not sure how this one is going to work with two players having to press it at the same time
        

    }
}
