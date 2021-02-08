using Connect4Game.Game_Resources;
using Connect4Game.Game_Resources.Graphics_Manager;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Connect4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        /// <summary>
        /// Variables que se usan en el programa.
        /// </summary>
        #region VARIABLES

        //Instancia del juego.
        private Game Game;

        //Tamaño del grid del juego seleccionado.
        private GridSize SelectedGridSize;

        //Etiqueta que indica de quien es el turno.
        public Label TurnLabel { get; set; } = new Label();

        //Etiqueta que indica el nombre del jugador.
        public Label MyNameLabel { get; set; } = new Label();

        //Etiqueta que indica el nombre del contrincante.
        public Label AdversaryNameLabel { get; set; } = new Label();

        //Etiqueta con el nombre del Juego.
        public Label GameLabel { get; set; } = new Label();

        //Sirve para acomodar el texto desbordante dentro del botón.
        public TextBlock textBlock { get; set; } = new TextBlock();

        //Botón que aparece al final del juego.
        public Button EndOfGameMessage { get; set; } = new Button();

        //Lista de botones que se generan con el grid de juego.
        public List<Button> Buttons { get; set; }
        #endregion


        //Constructor de la ventana del juego.
        public MainWindow()
        {
            InitializeComponent();

            textBlock.TextWrapping = TextWrapping.Wrap;
            SelectedGridSize = GridSize.Medium; //por defecto el tamaño del grid es de 7x7.
            PlayerNameTextBox.Text = $"Jugador{DateTime.Now.Millisecond}";  //Nombre aleatorio dentro del TextBox del menu principal.
            GraphicsManager.LoadImages(this);   //Se carga el arreglo de imagenes.

            GraphicsManager.InitButtons(this);  //Se asigna una imagen a cada botón del menu principal.
        }

        public void MainMenu()
        {
            GraphicsManager.ShowMenu(this); //Muestra el Menú.
        }


        /// <summary>
        /// Eventos que ocurren al presionar botones
        /// </summary>
        /// <param name="sender"> El boton presionado </param>
        /// <param name="e"> El evento </param>
        #region BUTTON EVENTS

        //Botón que elige el modo un jugador.
        private void SinglePlayer_OnClick(object sender, RoutedEventArgs e)
        {
            StartNewGame(GameType.Singleplayer, SelectedGridSize);
        }

        //Botón que elige modo multijugador.
        private void MultiPlayer_OnClick(object sender, RoutedEventArgs e)
        {
            if (SelectedGridSize != GridSize.Medium) //Solo se puede jugar de a 2 con el tamaño de grid mediano
            {
                MessageBox.Show("Debes seleccionar el tamaño 'Mediano' para acceder a una partida de 2 jugadores",
                                "Tamaño no disponible",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }
            StartNewGame(GameType.Multiplayer, SelectedGridSize);
        }

        //Botón salir. cierra la ventana
        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //Cambia el tamaño del tablero a pequeño. Alterna la opacidad de los botones.
        private void SmallGrid_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedGridSize = GridSize.Small; //5x5
            SmallGrid.Opacity = 1;
            MediumGrid.Opacity = 0.75;
            LargeGrid.Opacity = 0.75;
        }

        //Cambia el tamaño del tablero a mediano. Alterna la opacidad de los botones.
        private void MediumGrid_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedGridSize = GridSize.Medium; //7x7
            SmallGrid.Opacity = 0.75;
            MediumGrid.Opacity = 1;
            LargeGrid.Opacity = 0.75;
        }

        //Cambia el tamaño del tablero a grande. Alterna la opacidad de los botones.
        private void BigGrid_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedGridSize = GridSize.Big; //9x9
            SmallGrid.Opacity = 0.75;
            MediumGrid.Opacity = 0.75;
            LargeGrid.Opacity = 1;
        }

        //Botones del tablero de juego.
        public void Button_Click(object sender, RoutedEventArgs e)
        {
            int columnPosition = Grid.GetColumn((Button)sender) - 1; //Se le asigna a posición el valor de la columna del botón apretado, menos uno para corregir el desfase.
            Game.CheckNewPlay(columnPosition);    //Controla que la jugada sea valida, y la ejecuta.

        }

        //Botón de fin de juego.
        public void EndGame_Click(object sender, RoutedEventArgs e)
        {
            Game.HideEndGameMessage();  //Esconde el botón de fin de Juego.
            MainMenu();
        }

        #endregion

        //Se inicia un nuevo Juego.
        private void StartNewGame(GameType gameType, GridSize selectedSize)
        {
            Game = GameManager.NewGame(gameType);   //Se Crea una nueva instancia de juego.
            Game.GameWindow = this; //Se le asigna la ventana.
            Game.CreateGame(selectedSize);  //Se crea el tablero con el tamaño elegido.
            Game.SetPlayerName(PlayerNameTextBox.Text); //Se asigna al jugador el nombre ingresado en el TextBox.
            GraphicsManager.ShowGame(this); //Se muestra el tablero de juego.
            Game.StartGame();   //¡Empieza el juego!
        }

        //
        private void GameWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                Game.EndGame();
            }
            catch (Exception)
            {
            }
        }
    }
}