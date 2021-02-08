using Connect4;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Connect4Game.Game_Resources.Graphics_Manager;
using System.Windows;

namespace Connect4Game.Game_Resources
{
    public abstract class Game
    {

        #region VARIABLES

        protected MainWindow gameWindow;

        protected Player Player;

        protected CellState[] CellStates;

        protected int gridSize;

        protected int GameRows;

        protected int GameColumns;

        protected GameState Status;

        protected List<Button> Winners;

        public MainWindow GameWindow { get => gameWindow; set => gameWindow = value; }

        public abstract void StartGame();

        public abstract void CheckNewPlay(int columnPosition);

        protected abstract void UpdateLabels();

        public abstract void EndGame();

        #endregion

        //Posibles estados de juego.
        protected enum GameState
        {
            Playing,
            NotStartedOrFinished
        }

        public Game()
        {
            Player = new Player();
            Player.Name = "asd";
            Player.Turn = true;
        }

        //Se crea el tablero de juego.
        public void CreateGame(GridSize _gridSize)
        {
            gridSize = (int)_gridSize;
            GameRows = gridSize - 2;    //La primera y última fila se reservan para etiquetas.
            GameColumns = gridSize - 2;     //La primera y última columna se reservan para etiquetas.
            CellStates = new CellState[GameRows * GameColumns]; //Se crea un arreglo de estados que guarda el estado de cada celda (Free por defecto).
            GameManager.GenerateGameGrid(gameWindow, gridSize); //Se crean todos los componentes del grid de juego.
        }

        //Se le asigna el nombre al jugador.
        public void SetPlayerName(string name)
        {
            Player.Name = name;
        }

        //Se devuelve la cantidad de columnas de juego.
        public int GetColumns()
        {
            return GameColumns;
        }

        //Se actualiza la información de la celda (color y estado);
        protected int UpdateCell(int cellNumber)
        {
            Button button = GameWindow.Buttons[cellNumber]; //Se le asigna el botón correspondiente a la primera fila de la columna del botón presionado.

            int updatedCellPosition = UpdateCellPositionAndColor(button, cellNumber);   //

            UpdateCellState(updatedCellPosition);   //Se actualiza el estado de la última celda, dependiendo de de quien es el turno.

            return updatedCellPosition;

        }

        //Recorre la columna presionada hasta que no pueda bajar más.
        private int UpdateCellPositionAndColor(Button cell, int firstPosition)
        {
            int cellNumber = firstPosition;
            int lastPosition = firstPosition;

            //Mientras haya posiciones por debajo, y las mismas esten libres (No sean Red o Blue), se baja.
            while (cellNumber <= (GameRows * GameColumns) - GameColumns - 1 && CellStates[cellNumber + GameColumns] == CellState.Free)
            {
                //GraphicsManager.Animations.FlashLabel(cell,Player.Turn);
                cellNumber += GameColumns;
                lastPosition = cellNumber;
                cell = GameWindow.Buttons[cellNumber];
            }
            UpdateCellColor(cell);  //Se cambia el color del boton al del correspondiente jugador.

            return lastPosition;
        }

        private void UpdateCellColor(Button cell)
        {
            if (Player.Turn)
                GraphicsManager.PaintCellRed(cell);
            else
                GraphicsManager.PaintCellBlue(cell);
        }

        private void UpdateCellState(int cellPosition)
        {
            CellStates[cellPosition] = Player.Turn ? CellState.Red : CellState.Blue;
        }

        //Alterna el estado de los botones del tablero.
        protected void EnableDisableButtons()
        {
            foreach (var button in GameWindow.Buttons)
            {
                button.IsEnabled ^= true;
            }
        }

        //Controla si la columna ya esta llena.
        public bool ColumnNotFree(int columnNumber)
        {
            return CellStates[columnNumber] != CellState.Free;
        }

        //Esconde el mensaje de fin de juego.
        public void HideEndGameMessage()
        {
            GameWindow.EndOfGameMessage.Visibility = Visibility.Hidden;
        }

        //Controla si alguno de los jugadores ganó.
        protected void CheckForWinner(int cellPosition)
        {
            int points;
            Button button = GameWindow.Buttons[cellPosition];
            Winners = new List<Button>();   //Lista que guarda las posibles celdas "ganadoras".

            //Control Horizontal.
            Winners.Add(button);
            points = 1 + CheckLeft(cellPosition - 1) + CheckRight(cellPosition + 1);
            if (points >= 4)
            {
                PlayerWon();
                EndGame();
                return;
            }

            //Control Vertical.
            Winners.Clear();
            Winners.Add(button);
            points = 1 + CheckDown(cellPosition + GameColumns);
            if (points >= 4)
            {
                PlayerWon();
                EndGame();
                return;
            }

            //Control Diagonal \.
            Winners.Clear();
            Winners.Add(button);
            points = 1 + CheckUpLeft(cellPosition - (GameColumns + 1)) + CheckDownRight(cellPosition + (GameColumns + 1));
            if (points >= 4)
            {
                PlayerWon();
                EndGame();
                return;
            }

            //Control Diagonal /.
            Winners.Clear();
            Winners.Add(button);
            points = 1 + CheckUpRight(cellPosition - (GameColumns - 1)) + CheckDownLeft(cellPosition + (GameColumns - 1));
            if (points >= 4)
            {
                PlayerWon();
                EndGame();
                return;
            }

            //En caso de no haber ganador, se controla si el panel esta lleno.
            //Si lo esta, se termina el juego.
            if (FullPanel())
                EndGame();

        }

        //Pinta las celdas ganadoras de color dorado
        private void PlayerWon()
        {
            Winners.ForEach(WinnerCell =>
            {
                GraphicsManager.PaintCellGold(WinnerCell);
            });
        }

        protected bool FullPanel()
        {
            if (!CellStates.Any(result => result == CellState.Free))
            {
                //Pinta los botones de naranja para indicar la perdida de ambos jugadores.
                foreach (var button in GameWindow.Buttons)
                {
                    GraphicsManager.PaintCellGold(button);
                }
                return true;
            }
            return false;
        }

        #region RECURSIVE METHODS
        private int CheckLeft(int position)
        {

            //Control que no este en la columna mas a la izquierda | Que no sea una posicion negativa | Que el boton tenga un tipo distinto de free | Que tenga el mismo tipo que el boton anterior
            if (!(InBounds(position) && ValidPosition(position, GameColumns - 1) && CellIsFree(position) && CellEqualPreviousCell(position, position + 1)))
            {
                return 0;
            }
            Button button = GameWindow.Buttons[position];
            Winners.Add(button);
            return CheckLeft(position - 1) + 1;
        }

        //Controla los elementos iguales a la derecha.
        private int CheckRight(int position)
        {
            if (!(InBounds(position) && ValidPosition(position, 0) && CellIsFree(position) && CellEqualPreviousCell(position, position - 1)))
            {
                return 0;
            }
            Button button = GameWindow.Buttons[position];
            Winners.Add(button);
            return CheckRight(position + 1) + 1;
        }

        //Controla los elementos iguales hacia abajo.
        private int CheckDown(int position)
        {
            //
            if (!(InBounds(position) && CellIsFree(position) && CellEqualPreviousCell(position, position - GameColumns)))
            {
                return 0;
            }
            Button button = GameWindow.Buttons[position];
            Winners.Add(button);
            return CheckDown(position + GameColumns) + 1;
        }

        //Controla los elementos iguales en diagonal arriba/izquierda.
        private int CheckUpLeft(int position)
        {
            if (!(InBounds(position) && ValidPosition(position + (GameColumns + 1), 0) && CellIsFree(position) && CellEqualPreviousCell(position, position + (GameColumns + 1))))
            {
                return 0;
            }
            Button button = GameWindow.Buttons[position];
            Winners.Add(button);
            return CheckUpLeft(position - (GameColumns + 1)) + 1;
        }

        //Controla los elementos iguales en diagonal abajo/derecha.
        private int CheckDownRight(int position)
        {
            if (!(InBounds(position) && ValidPosition(position - (GameColumns + 1), GameColumns - 1) && CellIsFree(position) && CellEqualPreviousCell(position, position - (GameColumns + 1))))
            {
                return 0;
            }
            Button button = GameWindow.Buttons[position];
            Winners.Add(button);
            return CheckDownRight(position + (GameColumns + 1)) + 1;
        }

        //Controla los elementos iguales en diagonal arriba\derecha.
        private int CheckUpRight(int position)
        {
            if (!(InBounds(position) && ValidPosition(position + (GameColumns - 1), GameColumns - 1) && CellIsFree(position) && CellEqualPreviousCell(position, position + (GameColumns - 1))))
            {
                return 0;
            }
            Button button = GameWindow.Buttons[position];
            Winners.Add(button);
            return CheckUpRight(position - (GameColumns - 1)) + 1;
        }

        //Controla los elementos iguales en diagonal abajo\izquierda.
        private int CheckDownLeft(int position)
        {
            if (!(InBounds(position) && ValidPosition(position - (GameColumns - 1), 0) && CellIsFree(position) && CellEqualPreviousCell(position, position - (GameColumns - 1))))
            {
                return 0;
            }
            Button button = GameWindow.Buttons[position];
            Winners.Add(button);
            return CheckDownLeft(position + (GameColumns - 1)) + 1;
        }

        #endregion

        //Controla si la celda esta ocupada.
        private bool CellIsFree(int position)
        {
            if (CellStates[position] != CellState.Free)
            {
                return true;
            }
            return false;
        }

        //Controla si la celda tiene el mismo "color" (estado) que la anterior.
        private bool CellEqualPreviousCell(int position, int previousPosition)
        {
            if (CellStates[position] == CellStates[previousPosition])
            {
                return true;
            }
            return false;
        }

        //Controla que la posición este dentro de los limites establecidos por el tamaño del tablero.
        private bool InBounds(int position)
        {
            if (position >= 0 && position <= (GameColumns * GameRows - 1))
            {
                return true;
            }
            return false;
        }

        //Control que evita que botones en lados opuestos del grid cuenten como una victoria.
        private bool ValidPosition(int dividend, int remainder)
        {
            if (dividend % GameColumns != remainder)
            {
                return true;
            }
            return false;
        }

        




    }
}
