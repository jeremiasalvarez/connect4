using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameService
{
    public class GameServiceHandler
    {
        private bool[] Turns;
        //private static List<IGameClient> Players = new List<IGameClient>(2);
        private static Dictionary<string, IGameClient> Players = new Dictionary<string, IGameClient>(2);

        public GameServiceHandler()
        {
            Turns = new bool[2];
            Turns[0] = true;
            Turns[1] = false;

        } 
        public void PlayerDisconnected(string playerName)
        {
            Players.Remove(playerName);
            UpdateGameStatus(false);
        }

        public bool AddPlayer(string playerName, IGameClient Player)
        {
            if (Players.Count == 2)
            {
                return false;
            }

            Players.Add(playerName, Player);

            if (Players.Count == 2)
            {
                UpdateGameStatus(true);
                SendOpponentNames();
                UpdateGameTurns();

            }
            return true;
        }

        private void SendOpponentNames()
        {
            var firstPlayer = Players.ElementAt(0);
            var secondPlayer = Players.ElementAt(1);

            firstPlayer.Value.GetOpponentName(secondPlayer.Key);
            secondPlayer.Value.GetOpponentName(firstPlayer.Key);
        }

        private void SwitchTurns()
        {
            Turns[0] = !Turns[0];
            Turns[1] = !Turns[1];
        }

        public void UpdateGameTurns()
        {
            int index = 0;

            foreach (var player in Players.Values)
            {
                player.UpdateTurn(Turns[index]);
                index++;
            }
            SwitchTurns();
        }

        public void UpdateClientGame(int cellNumber)
        {
            foreach (var player in Players.Values)
            {
                player.UpdateClient(cellNumber);

            }
            UpdateGameTurns();
        }

        private void UpdateGameStatus(bool status)
        {
            foreach (var player in Players.Values)
            {
                player.UpdateGameStatus(status);
            }
            //Players.Values.ForEach(player => player.UpdateGameStatus(status));
        }

    }
}

