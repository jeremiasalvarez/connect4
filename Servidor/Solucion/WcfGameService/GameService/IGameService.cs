using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GameService
{
    [ServiceContract(CallbackContract = typeof(IGameClient))]
    public interface IGameService
    {
        [OperationContract(IsOneWay = true)]
        void JoinGame(string playerName);

        [OperationContract(IsOneWay = true)]
        void LeaveGame(string playerName);

        [OperationContract(IsOneWay = true)]
        void SendSelectedCell(int cellNumber);


    }

    [ServiceContract]
    public interface IGameClient
    {
        [OperationContract(IsOneWay = true)]
        void UpdateClient(int cellNumber);

        [OperationContract(IsOneWay = true)]
        void UpdateTurn(bool turn);

        [OperationContract(IsOneWay = true)]
        void UpdateGameStatus(bool status);

        [OperationContract(IsOneWay = true)]
        void GetOpponentName(string oppName);
    }

}
