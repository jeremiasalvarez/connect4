using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GameService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
    public class GameService : IGameService
    {

        private GameServiceHandler Game = new GameServiceHandler();

        public void LeaveGame(string playerName)
        {
            Task.Factory.StartNew(() =>
            {
                Game.PlayerDisconnected(playerName);
            });
        }

        public void JoinGame(string playerName)
        {
            var connection = OperationContext.Current.GetCallbackChannel<IGameClient>();
            Game.AddPlayer(playerName, connection);
        }

        public void SendSelectedCell(int cellNumber)
        {
            Task.Factory.StartNew(() =>
            {
                //var connection = OperationContext.Current.GetCallbackChannel<IGameClient>();
                Game.UpdateClientGame(cellNumber);
                //Game.UpdateGameTurns();
            });
        }

    }
}
