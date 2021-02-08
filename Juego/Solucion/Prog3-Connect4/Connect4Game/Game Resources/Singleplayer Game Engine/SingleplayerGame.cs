using System.Windows;
using Connect4Game.Game_Resources.Graphics_Manager;

namespace Connect4Game.Game_Resources.Singleplayer_Game_Engine
{
    public enum PlayerType
    {
        Human,
        CPU
    }

    public class SingleplayerGame : Game
    {
        private CpuPlayer CpuPlayer;
        private PlayerType CurrentPlayer;

        public SingleplayerGame()
        {
            CpuPlayer = new CpuPlayer();
            CpuPlayer.Turn = false;
            CurrentPlayer = PlayerType.Human;
        }

        public override void CheckNewPlay(int columnPosition)
        {

            if (ColumnNotFree(columnPosition))
            {
                MessageBox.Show("Columna llena, elegir otra");
                return;
            }

            EnableDisableButtons();

            ProcessPlay(PlayerType.Human, columnPosition);

            if (!Player.Turn && Status != GameState.NotStartedOrFinished)
            {
                CpuPlayer.Play();
            }

            EnableDisableButtons();
        }

        public override void EndGame()
        {
            EnableDisableButtons();
            //Thread.Sleep(1000);
            GameWindow.EndOfGameMessage.Visibility = Visibility.Visible;

            gameWindow.textBlock.Inlines.Clear();
            //Se muestra un mensaje que depende de si hubo ganador, y quien fue.
            if (!FullPanel())
            {
                if (Player.Turn)
                {
                    GraphicsManager.BackgroundRed(GameWindow.EndOfGameMessage);
                    gameWindow.textBlock.Inlines.Add($"Ganador: {Player.Name}");
                    GameWindow.EndOfGameMessage.Content = gameWindow.textBlock;
                }
                else
                {
                    GraphicsManager.BackgroundBlue(GameWindow.EndOfGameMessage);
                    gameWindow.textBlock.Inlines.Add($"Ganador: {CpuPlayer.Name}");
                    GameWindow.EndOfGameMessage.Content = gameWindow.textBlock;
                }
            }
            else
            {
                GraphicsManager.BackgroundOrange(GameWindow.EndOfGameMessage);
                gameWindow.textBlock.Inlines.Add("Empate");
                GameWindow.EndOfGameMessage.Content = gameWindow.textBlock;
            }
            //se cambia el estado del juego a "terminado"
            Status = GameState.NotStartedOrFinished;

        }

        public override void StartGame()
        {
            CpuPlayer.GameContext_ = this;

            gameWindow.MyNameLabel.Content = Player.Name;
            gameWindow.AdversaryNameLabel.Content = CpuPlayer.Name;
            UpdateLabels();
            Status = GameState.Playing;

        }

        protected void UpdateTurns()
        {
            Player.Turn = !Player.Turn;
            CpuPlayer.Turn = !CpuPlayer.Turn;
            CurrentPlayer = Player.Turn ? PlayerType.Human : PlayerType.CPU;
        }


        protected override void UpdateLabels()
        {
            if (Status != GameState.NotStartedOrFinished)
            {
                GameWindow.TurnLabel.Content = Player.Turn ? $"Turno de {Player.Name}" : "Turno de la CPU";
                return;
            }
            GameWindow.TurnLabel.Content = string.Empty;
        }

        public void ProcessPlay(PlayerType player, int columnPosition)
        {
            //controla que el jugador humano no juegue cuando no es su turno
            if (player == PlayerType.Human && CurrentPlayer == PlayerType.CPU)
            {
                MessageBox.Show("No es tu turno");
                return;
            }

            int lastCellPosition = UpdateCell(columnPosition);

            CheckForWinner(lastCellPosition);

            UpdateTurns();

            UpdateLabels();
        }
    }
}
