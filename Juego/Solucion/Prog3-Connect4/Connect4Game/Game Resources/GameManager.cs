using Connect4Game.Game_Resources.Multiplayer_Game_Engine;
using Connect4Game.Game_Resources.Singleplayer_Game_Engine;
using Connect4Game.Game_Resources.Graphics_Manager;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using Connect4;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace Connect4Game.Game_Resources
{
    //Tipos de Juego.
    public enum GameType
    {
        Multiplayer,
        Singleplayer
    }

    //Tamaños posibles del tablero.
    public enum GridSize
    {
        Small = 7,
        Medium = 9,
        Big = 11
    }

    public static class GameManager
    {
        private static int GridSize;

        //Se crea y retorna una instancia del tipo de juego correspondiente.
        public static Game NewGame(GameType type)
        {
            if (type == GameType.Multiplayer)
            {
                return new MultiplayerGame();
            }
            return new SingleplayerGame();
        }

        //Se genera cada elemento del tablero de juego.
        public static void GenerateGameGrid(MainWindow gameWindow, int gridSize)
        {
            GridSize = gridSize;

            RemovePreviousGrid(gameWindow);

            CreateGrid(gameWindow);

            CreateColums(gameWindow);

            CreateRows(gameWindow);

            CreateButtons(gameWindow);

            GenerateLabels(gameWindow);

            GenerateEndGameMessage(gameWindow);

            gameWindow.MainGrid.Children.Add(gameWindow.GameGrid);

        }

        //Si existiesen, se remueven los elementos del grid de juego, y se remueve el propio grid de juego del grid principal.
        private static void RemovePreviousGrid(MainWindow gameWindow)
        {
            gameWindow.GameGrid.Children.Remove(gameWindow.TurnLabel);
            gameWindow.GameGrid.Children.Remove(gameWindow.GameLabel);
            gameWindow.GameGrid.Children.Remove(gameWindow.MyNameLabel);
            gameWindow.GameGrid.Children.Remove(gameWindow.AdversaryNameLabel);
            gameWindow.GameGrid.Children.Remove(gameWindow.EndOfGameMessage);

            gameWindow.MainGrid.Children.Remove(gameWindow.GameGrid);
        }

        //Se genera un nuevo grid de juego.
        private static void CreateGrid(MainWindow gameWindow)
        {
            gameWindow.GameGrid = new Grid();
            //gameWindow.GameGrid.Visibility = Visibility.Hidden;
            //gameWindow.GameGrid.Margin = new Thickness(10);
        }

        //Se crean y agregan la cantidad de columnas correspondientes al tamaño del grid.
        private static void CreateColums(MainWindow gameWindow)
        {
            for (int i = 0; i < GridSize; i++)
            {
                ColumnDefinition coldef = new ColumnDefinition();
                gameWindow.GameGrid.ColumnDefinitions.Add(coldef);
            }
        }

        //Se crean y agregan la cantidad de filas correspondientes al tamaño del grid.
        private static void CreateRows(MainWindow gameWindow)
        {
            for (int i = 0; i < GridSize; i++)
            {
                RowDefinition rowdef = new RowDefinition();
                gameWindow.GameGrid.RowDefinitions.Add(rowdef);
            }
        }

        //Se crean los botones correspondientes
        private static void CreateButtons(MainWindow gameWindow)
        {
            gameWindow.Buttons = new List<Button>();    //Se reinicia la lista de botones, volviendose a crear con la cantidad indicada.

            for (int i = 1; i < GridSize - 1; i++)  //Primer y última fila reservadas para etiquetas
            {
                for (int j = 1; j < GridSize - 1; j++)  //Primer y última columna reservadas para etiquetas.
                {
                    //Por cada celda en el tablero, se crea un botón, se le dan propiedas y se agrega a la lista de botones. 
                    Button gridButton = new Button();
                    gridButton.Cursor = Cursors.Hand;
                    gridButton.Click += gameWindow.Button_Click;
                    //gridButton.Content = (i - 1) * (GridSize - 2) + (j - 1);
                    Grid.SetRow(gridButton, i);
                    Grid.SetColumn(gridButton, j);
                    gameWindow.GameGrid.Children.Add(gridButton);
                    GraphicsManager.Paint(gridButton);  //Pinta el botón con el fondo por defecto (centro transparente).
                    gameWindow.Buttons.Add(gridButton);
                }
            }
        }

        /// <summary>
        /// Se generan las etiquetas a los margenes del tablero.
        /// </summary>
        /// <param name="gameWindow"></param>
        #region LABELS

        private static void GenerateLabels(MainWindow gameWindow)
        {
            TopLabel(gameWindow);
            DownLabel(gameWindow);
            LeftLabel(gameWindow);
            RightLabel(gameWindow);
        }

        //Se agregan propiedades a la etiqueta del juego y se la asigna a la primer fila.
        private static void TopLabel(MainWindow gameWindow)
        {
            gameWindow.GameLabel.Content = "¡Conecta 4!";
            gameWindow.GameLabel.FontSize = 20;
            gameWindow.GameLabel.VerticalContentAlignment = VerticalAlignment.Top;
            gameWindow.GameLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
            gameWindow.GameLabel.Foreground = Brushes.Azure;
            Grid.SetRow(gameWindow.GameLabel, 0);
            Grid.SetColumnSpan(gameWindow.GameLabel, GridSize);
            gameWindow.GameGrid.Children.Add(gameWindow.GameLabel);

        }

        //Se agregan propiedades a la etiqueta del turno y se la asigna a la última fila.
        private static void DownLabel(MainWindow gameWindow)
        {
            gameWindow.TurnLabel.Content = string.Empty;
            gameWindow.TurnLabel.FontSize = 15;
            gameWindow.TurnLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
            gameWindow.TurnLabel.Foreground = Brushes.Azure;
            Grid.SetRow(gameWindow.TurnLabel, GridSize);
            Grid.SetColumnSpan(gameWindow.TurnLabel, GridSize);
            gameWindow.GameGrid.Children.Add(gameWindow.TurnLabel);
        }

        //Se crean etiquetas que se asignan a la primer columna.
        private static void LeftLabel(MainWindow gameWindow)
        {
            Label myIcon = new Label();
            myIcon.Width = 50;
            myIcon.Height = 50;
            myIcon.HorizontalAlignment = HorizontalAlignment.Stretch;
            myIcon.VerticalAlignment = VerticalAlignment.Stretch;
            myIcon.Background = GraphicsManager.myBrushes[7];
            myIcon.Content = "Tu";
            myIcon.Foreground = Brushes.Azure;
            myIcon.FontSize = 9;
            myIcon.HorizontalContentAlignment = HorizontalAlignment.Center;
            myIcon.VerticalContentAlignment = VerticalAlignment.Center;
            Grid.SetColumn(myIcon, 0);
            Grid.SetRow(myIcon, 1);
            Grid.SetRowSpan(myIcon, 2);
            gameWindow.GameGrid.Children.Add(myIcon);

            gameWindow.MyNameLabel.Content = string.Empty;
            gameWindow.MyNameLabel.Foreground = Brushes.Azure;
            gameWindow.MyNameLabel.FontSize = 10;
            gameWindow.MyNameLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
            gameWindow.MyNameLabel.HorizontalAlignment = HorizontalAlignment.Stretch;
            gameWindow.MyNameLabel.VerticalAlignment = VerticalAlignment.Bottom;
            Grid.SetColumn(gameWindow.MyNameLabel, 0);
            Grid.SetRow(gameWindow.MyNameLabel, 2);
            gameWindow.GameGrid.Children.Add(gameWindow.MyNameLabel);

        }

        //Se crean etiquetas que se asignan a la última columna.
        private static void RightLabel(MainWindow gameWindow)
        {
            Label adversaryIcon = new Label();
            adversaryIcon.Width = 50;
            adversaryIcon.Height = 50;
            adversaryIcon.HorizontalAlignment = HorizontalAlignment.Stretch;
            adversaryIcon.VerticalAlignment = VerticalAlignment.Stretch;
            adversaryIcon.Background = GraphicsManager.myBrushes[8];
            adversaryIcon.Content = "Oponente";
            adversaryIcon.Foreground = Brushes.Azure;
            adversaryIcon.FontSize = 9;
            adversaryIcon.HorizontalContentAlignment = HorizontalAlignment.Center;
            adversaryIcon.VerticalContentAlignment = VerticalAlignment.Center;
            Grid.SetColumn(adversaryIcon, GridSize);
            Grid.SetRow(adversaryIcon, 1);
            Grid.SetRowSpan(adversaryIcon, 2);
            gameWindow.GameGrid.Children.Add(adversaryIcon);

            gameWindow.AdversaryNameLabel.Content = string.Empty;
            gameWindow.AdversaryNameLabel.Foreground = Brushes.Azure;
            gameWindow.AdversaryNameLabel.FontSize = 11;
            gameWindow.AdversaryNameLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
            gameWindow.AdversaryNameLabel.HorizontalAlignment = HorizontalAlignment.Stretch;
            gameWindow.AdversaryNameLabel.VerticalAlignment = VerticalAlignment.Bottom;
            Grid.SetColumn(gameWindow.AdversaryNameLabel, GridSize);
            Grid.SetRow(gameWindow.AdversaryNameLabel, 2);
            Grid.SetColumnSpan(gameWindow.AdversaryNameLabel, 2);
            gameWindow.GameGrid.Children.Add(gameWindow.AdversaryNameLabel);

        }

        #endregion

        //Se agregan valores al mensaje de fin de juego y se asigna en el medio-ish del grid.
        private static void GenerateEndGameMessage(MainWindow gameWindow)
        {
            gameWindow.EndOfGameMessage.Foreground = Brushes.Azure;
            gameWindow.EndOfGameMessage.Visibility = Visibility.Hidden;
            gameWindow.EndOfGameMessage.Click += gameWindow.EndGame_Click;
            gameWindow.EndOfGameMessage.Cursor = Cursors.Hand;
            Grid.SetColumn(gameWindow.EndOfGameMessage, (int)Math.Floor((double)(GridSize / 2)) - 1);
            Grid.SetColumnSpan(gameWindow.EndOfGameMessage, 3);
            Grid.SetRow(gameWindow.EndOfGameMessage, (int)Math.Floor((double)(GridSize / 2)) - 1);
            Grid.SetRowSpan(gameWindow.EndOfGameMessage, 2);
            Grid.SetZIndex(gameWindow.EndOfGameMessage, 1);
            gameWindow.GameGrid.Children.Add(gameWindow.EndOfGameMessage);
        }

    }
}
