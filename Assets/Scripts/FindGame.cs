using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;
using UnityEngine.UI;

public class FindGame : MonoBehaviour
{
    public Text cotbucks;
    public Text hotbucks;
    public Text level;
    public Player player;

    public NetPeer server;
    public GameObject buttonEnterTheRoomPrefabs;
    public GameObject content;
    public GameObject fon;
    public GameObject loadingScreen;
    public Slider progressLoadingGame;
    public GameObject gameRoom;
    public GameObject findGameScreen;
    public GameObject menuNavigation;

    public List<GameObject> buttonsFreeRooms = new List<GameObject>();
    private IEnumerator _findFreeRooms;

    public void ShowPlayerInformation()
    {
        cotbucks.text = player.cotbucks.ToString();
        hotbucks.text = player.hotbucks.ToString();
        level.text = player.level.ToString();
    }

    public void FindFreeRooms()
    {
        _findFreeRooms = FindFreeRoomsIEnumerator();
        StartCoroutine(_findFreeRooms);
    }

    private IEnumerator FindFreeRoomsIEnumerator()
    {
        ClearListFreeRoom();
        while (true)
        {
            NetDataWriter writer = new NetDataWriter();
            string[] callMethod = new[] { "ControlRooms", "FindFreeRoom" };
            writer.PutArray(callMethod);
            server.Send(writer, DeliveryMethod.ReliableSequenced);
            yield return new WaitForSeconds(5);
        }
    }

    public void CallSorter(string[] reader)
    {
        switch (reader[1])
        {
            case ("ShowFreeRoom"):
                ShowFreeRoom(reader[2]);
                break;

            case ("ShowRoomInformation"):
                ShowInformationTheRoom(reader);
                break;

            case ("ConnectionComplited"):
                StartCoroutine(ConnectionComplited(reader));
                break;
        }
    }

    private void ShowFreeRoom(string reader)
    {
        ClearListFreeRoom();
        List<string> IDFreeRoom = ToExtractIDFreeRoom(reader);
        for(int i = 0; i < IDFreeRoom.Count; i++)
        {
            GameObject buttonEnterTheRoom = Instantiate(buttonEnterTheRoomPrefabs, content.transform);
            buttonsFreeRooms.Add(buttonEnterTheRoom);
            buttonEnterTheRoom.GetComponent<ButtonFreeRoom>().player = player;
            buttonEnterTheRoom.GetComponent<ButtonFreeRoom>().server = server;
            buttonEnterTheRoom.GetComponent<ButtonFreeRoom>().findGame = this;
            buttonEnterTheRoom.GetComponent<ButtonFreeRoom>().ID = IDFreeRoom[i];
            buttonEnterTheRoom.GetComponent<ButtonFreeRoom>().listItem = i.ToString();
            buttonEnterTheRoom.GetComponent<ButtonFreeRoom>().FindOutInformationAboutTheRoom();
        }
        
    }

    private List<string> ToExtractIDFreeRoom(string idRooms)
    {
        List<string> IDFreeRoom = new List<string>();
        string id = "";
        foreach(var symbol in idRooms)
        {
            if(symbol.ToString() != "\n")
            {
                id += symbol.ToString();
            }
            else
            {
                IDFreeRoom.Add(id);
                id = "";
            }
        }
        return IDFreeRoom;
    }

    public void ClearListFreeRoom()
    {
        foreach (GameObject freeRoom in buttonsFreeRooms)
        {
            Destroy(freeRoom);
        }
        buttonsFreeRooms.Clear();
    }

    private void ShowInformationTheRoom(string[] reader)
    {
        foreach (GameObject buttonFreeRoom in buttonsFreeRooms)
        {
            if(buttonFreeRoom.GetComponent<ButtonFreeRoom>().listItem == reader[2])
            {
                buttonFreeRoom.GetComponent<ButtonFreeRoom>().ShowInformationTheRoom(reader);
                break;
            }
        }
    }

    public void ConnectionToTheRoom()
    {
        StopFindFreeRoom();
        ClearListFreeRoom();
        menuNavigation.SetActive(false);
        findGameScreen.SetActive(false);
        fon.SetActive(false);
        loadingScreen.SetActive(true);
        progressLoadingGame.value += 50;
    }

    public void StopFindFreeRoom()
    {
        if (_findFreeRooms != null)
        {
            StopCoroutine(_findFreeRooms);
        }
    }

    public IEnumerator ConnectionComplited(string[] reader)
    {        
        progressLoadingGame.value += 50;
        yield return new WaitForSeconds(1f);
        loadingScreen.SetActive(false);
        gameRoom.SetActive(true);
        gameRoom.GetComponent<GameRoom>().OpenGameRoom(int.Parse(reader[2]), player, server);
        gameObject.SetActive(false);
    }
}
