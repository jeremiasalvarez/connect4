using System;

namespace Connect4Game.Game_Resources.Singleplayer_Game_Engine
{
    public class CpuPlayer : Player
    {
        private SingleplayerGame GameContext;

        public SingleplayerGame GameContext_ { set => GameContext = value; }


        public CpuPlayer()
        {
            this.Name = "CPU";
        }

        public void Play()
        {
            //GameContext.EnableDisableButtons();
            //Thread.Sleep(300);
            //Se genera un numero aleatorio no mayor que el numero de columnas - 1.

            int cellPosition = FindFreeCell();

            //Se simula el click del boton situado en la posicion aleatoria generada.
            GameContext.ProcessPlay(PlayerType.CPU, cellPosition);
            //GameContext.GameWindow.Buttons[cellPosition].RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            //GameContext.EnableDisableButtons();
    
        }

        private int FindFreeCell()
        {
            int cellPosition;

            do
            {
                cellPosition = new Random().Next(0, GameContext.GetColumns());
                
            } while (GameContext.ColumnNotFree(cellPosition));

            return cellPosition;
        }
    }
}
