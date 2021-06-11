using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LiteNetLib;
using LiteNetLib.Utils;

public class ButtonFreeRoom : MonoBehaviour
{
    public string ID;

    public NetPeer server;
    public string listItem;

    public Text bet;
    public Text playerCount;
    public Text maxPlayerCount;
    public Image abilityToTransferToFightOffAnotherPlayer;
    public Image whoCanThrowCards;
    public Image hiddenFool;
    public Text deck;
    public Text creator;
    public Player player;
    public FindGame findGame;

    public List<Sprite> spritesGameMode;



    public void FindOutInformationAboutTheRoom()
    {
        NetDataWriter writer = new NetDataWriter();
        string[] callMethod = new[] { "ControlRooms", "FindOutInformationAboutTheRoom", ID, listItem };
        writer.PutArray(callMethod);
        server.Send(writer, DeliveryMethod.ReliableSequenced);
    }

    public void ShowInformationTheRoom(string[] reader)
    {
        creator.text = reader[3];
        bet.text = reader[4];        
        maxPlayerCount.text = reader[5];
        playerCount.text = reader[6];

        if (reader[7] == "default")
        {
            abilityToTransferToFightOffAnotherPlayer.sprite = spritesGameMode[0];
        }
        else
        {
            abilityToTransferToFightOffAnotherPlayer.sprite = spritesGameMode[1];
        }
        abilityToTransferToFightOffAnotherPlayer.enabled = true;

        if (reader[8] == "aroundTheEdges")
        {
            whoCanThrowCards.sprite = spritesGameMode[2];
        }
        else 
        {
            whoCanThrowCards.sprite = spritesGameMode[3];
        }
        whoCanThrowCards.enabled = true;

        if(reader[9] == "default")
        {
            hiddenFool.sprite = spritesGameMode[4];
        }
        else
        {
            hiddenFool.sprite = spritesGameMode[5];
        }
        hiddenFool.enabled = true;

        deck.text = reader[10];
    }

    public void ConnectToGame()
    {
        if(player.cotbucks >= int.Parse(bet.text))
        {
            NetDataWriter writer = new NetDataWriter();
            string[] callMethod = new[] { "GameRoom", "ThePlayerConnectsToTheGame", ID};
            writer.PutArray(callMethod);
            server.Send(writer, DeliveryMethod.ReliableSequenced);
            findGame.ConnectionToTheRoom();
        }
    }
}
