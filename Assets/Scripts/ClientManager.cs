using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using LiteNetLib.Utils;

public class ClientManager : MonoBehaviour, INetEventListener
{
    public enum ServerToConnect
    {
        localhost,
        server
    }

    public ServerToConnect serverToConnect;

    private string _iplocalhost = "localhost";
    public string IpServer;
    public string host;
    public int port;
    public string key;
    private NetManager _netClient;
    public MainManager mainManager;
    public FindGame findGame;
    public NetPeer server;
    public CreateNewGame createNewGame;
    public GameRoom gameRoom;

    public void ConnectionToServer()
    {
        _netClient = new NetManager(this);
        _netClient.UnconnectedMessagesEnabled = true;
        _netClient.UpdateTime = 30;
        switch (serverToConnect)
        {
            case ServerToConnect.localhost:
                host = _iplocalhost;
                break;

            case ServerToConnect.server:
                host = IpServer;
                break;
        }
        _netClient.Start();
        _netClient.Connect(host, port, key);
    }

    private void Update()
    {
        _netClient.PollEvents();
    }

    public void OnConnectionRequest(ConnectionRequest request)
    {
    }

    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {

    }

    public void OnNetworkLatencyUpdate(NetPeer server, int latency)
    {

    }

    public void OnNetworkReceive(NetPeer server, NetPacketReader reader, DeliveryMethod deliveryMethod) //получил информацию от сервера
    {
        string[] mReader = reader.GetStringArray();
        switch (mReader[0])
        {
            case ("MainManager"):
                mainManager.CallSorter(mReader);
                break;

            case ("FindGame"):
                findGame.CallSorter(mReader);
                break;

            case ("CreateNewGame"):
                createNewGame.CallSorter(mReader);
                break;

            case ("GameRoom"):
                gameRoom.CallSorter(mReader);
                break;
    }
}

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {

    }

    public void OnPeerConnected(NetPeer server)
    {
        this.server = server;
        NetDataWriter writer = new NetDataWriter();
        string[] callMethod = new[] { "MainManager", "MakeAConnection" };
        writer.PutArray(callMethod);
        server.Send(writer, DeliveryMethod.ReliableSequenced);
        mainManager.MakeAConnection();
    }

    public void OnPeerDisconnected(NetPeer server, DisconnectInfo disconnectInfo)
    {
        mainManager.ErrorConectionToServer();
    }
}
