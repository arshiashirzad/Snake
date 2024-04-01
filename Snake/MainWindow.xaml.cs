﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<GridValue, ImageSource> GridValueToImage = new()
        {
            {GridValue.Snake , Images.Body },
            {GridValue.Food , Images.Food },
            {GridValue.Empty, Images.Empty },

        };
        private readonly int rows = 15, cols = 15;
        private readonly Image[,] gridImages;
        private GameState gameState;
        private bool gameRunning;
        public MainWindow()
        {
            InitializeComponent();
            gridImages = SetupGrid();
            gameState = new GameState(rows, cols);

        }
        private async Task RunGame()
        {
            Draw();
            await ShowCountDown();
            Overlay.Visibility = Visibility.Hidden;
            await GameLoop();
        }
        private async Task GameLoop()
        {
            while (!gameState.GameOver)
            {
                await Task.Delay(100);
                gameState.Move();
                Draw();
            }
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
                Draw();
                await GameLoop();
        }

        private async Task GameLoop()
            {
                while (!gameState.GameOver)
                {
                    await Task.Delay(100);
                    gameState.Move();
                    Draw();
                }
        }
        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
            {
                if (Overlay.Visibility == Visibility.Visible)
                {
                    e.Handled = true;
                }
                if (!gameRunning)
                {
                    gameRunning = true;
                    await RunGame();
                    gameRunning = false;
                }
          }
          private void Window_KeyDown(object sender, KeyEventArgs e)
            {
                if (gameState.GameOver)
                {
                    return;
                }
                switch (e.Key)
                {
                    case Key.Left:
                        gameState.ChangeDirection(Direction.Left);
                        break;
                    case Key.Right:
                        gameState.ChangeDirection(Direction.Right);
                        break;
                    case Key.Up:
                        gameState.ChangeDirection(Direction.Down);
                        break;
                    case Key.Down:
                        gameState.ChangeDirection(Direction.Up);
                        break;

                }
            }
            private Image[,] SetupGrid()
            {
                Image[,] images = new Image[rows, cols];
                GameGrid.Rows = rows;
                GameGrid.Columns = cols;
                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < cols; c++)
                    {
                        Image image = new Image
                        {
                            Source = Images.Empty
                        };
                        images[r, c] = image;
                        GameGrid.Children.Add(image);

                    }

                }
                return images;
            }
            private void Draw()
            {
                DrawGrid();
                ScoreText.Text = $"SCORE {gameState.Score}";
            }
            private void DrawGrid()
            {
                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < cols; c++)
                    {
                        GridValue gridVal = gameState.Grid[r, c];
                        gridImages[r, c].Source = GridValueToImage[gridVal];
                    }

                }
            }
            private async Task ShowCountDown()
            {
                for (int i = 3; i >= 1; i--)
                {
                    OverlayText.Text = i.ToString();
                    await Task.Delay(500);
                }
            }
    }