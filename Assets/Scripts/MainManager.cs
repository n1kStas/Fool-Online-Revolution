using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;
using UnityEditor;

public class MainManager : MonoBehaviour
{
    public ClientManager clientManager;

    public GameObject connectScreen;
    public GameObject createNewPlayerScreen;
    public GameObject mainMenuScreen;
    public GameObject fon;
    public GameObject findGameScreen;
    public GameObject createNewGame;
    public GameObject menuNavigation;

    public Player player;

    string[] callMethod;
    NetPeer server;

    private void Start()
    {
        ConectionToServer();
    }

    public void CallSorter(string[] reader)
    {
        switch (reader[1])
        {
            case ("ConnectionComplited"):
                ConfirmConnection();
                break;

            case ("RegistrationComplited"):
                RegistrationComplited(login: reader[2], password: reader[3], gender: reader[4], cotbucks: int.Parse(reader[5]), hotbucks: int.Parse(reader[6]), level: int.Parse(reader[7]), levelProgress: float.Parse(reader[8]), avatar: reader[9], frame: reader[10]);
                break;

            case ("RegistrationError"):
                RegistrationError();
                break;

            case ("AuthenticationError"):
                AuthenticationError();
                break;

            case ("AuthenticationSuccessful"):
                StartCoroutine(AuthenticationPlayerComplited(gender: reader[2], cotbucks: int.Parse(reader[3]), hotbucks: int.Parse(reader[4]), level: int.Parse(reader[5]), levelProgress: float.Parse(reader[6]), avatar: reader[7], frame: reader[8]));
                break;
        }
    }

    private void ConectionToServer() //начать соединение с сервером при запуске игры
    {
        connectScreen.SetActive(true);
        clientManager.ConnectionToServer();
    }

    public void MakeAConnection()  //Соединение с сервером выполнено 
    {
        connectScreen.GetComponent<ConnectScreen>().ShowProgressIEnumerator(33);        
    }

    private void ConfirmConnection()
    {
        server = clientManager.server;
        ShowEveryoneWhoTheServer();
        connectScreen.GetComponent<ConnectScreen>().ShowProgressIEnumerator(33);
        StartCoroutine(LoadPlayerData());
    }

    public void ErrorConectionToServer() //Соединение с сервером не удалось установить при запуске игры
    {
        connectScreen.GetComponent<ConnectScreen>().ErrorConectionToServer();
    }

    private IEnumerator LoadPlayerData()
    {
        if (PlayerPrefs.HasKey("login") && PlayerPrefs.HasKey("password"))
        {
            player.LoadPlayerData();
            NetDataWriter writer = new NetDataWriter();
            callMethod = new[] { "MainManager", "AuthenticationPlayer", player.login, player.password };
            writer.PutArray(callMethod);
            server.Send(writer, DeliveryMethod.ReliableSequenced);
            connectScreen.GetComponent<ConnectScreen>().ShowProgressIEnumerator(12);
        }
        else
        {
            connectScreen.GetComponent<ConnectScreen>().ShowProgressIEnumerator(33);
            yield return connectScreen.GetComponent<ConnectScreen>().ConnectionComplited();
            connectScreen.SetActive(false);
            createNewPlayerScreen.SetActive(true);
        }
    }

    public void CloseCreateNewPlayerScreen()
    {
        createNewPlayerScreen.SetActive(false);
    }

    private void OpenMainMenuScreen()
    {
        menuNavigation.SetActive(true);
        mainMenuScreen.SetActive(true);
        mainMenuScreen.GetComponent<MainMenu>().FonControl(fon);
        mainMenuScreen.GetComponent<MainMenu>().ShowPlayerInformation();
    }

    private void RegistrationComplited(string login, string password, string gender, int cotbucks, int hotbucks, int level, float levelProgress, string avatar, string frame)
    {
        player.SavePlayerSecretData(login, password);
        player.SavePlayerData(gender, cotbucks, hotbucks, level, levelProgress, avatar, frame);
        CloseCreateNewPlayerScreen();
        player.LoadPlayerData();
        ShowEveryoneWhoThePlayer();
        OpenMainMenuScreen();        
    }

    private void RegistrationError()
    {
        createNewPlayerScreen.GetComponent<NewPlayerManager>().RegistrationError();
    }

    private IEnumerator AuthenticationPlayerComplited(string gender, int cotbucks, int hotbucks, int level, float levelProgress, string avatar, string frame)
    {
        player.SavePlayerData(gender, cotbucks, hotbucks, level, levelProgress, avatar, frame);
        connectScreen.GetComponent<ConnectScreen>().ShowProgressIEnumerator(12);
        yield return connectScreen.GetComponent<ConnectScreen>().ConnectionComplited();
        connectScreen.SetActive(false);
        ShowEveryoneWhoThePlayer();
        OpenMainMenuScreen();        
    }

    private void AuthenticationError()
    {
        server.Disconnect();
        clientManager.server = null;
        server = null;
    }

    public void RegisterNewPlayer(string login, string gender, string avatar) // регистрация нового игрока
    {
        NetDataWriter writer = new NetDataWriter();
        callMethod = new[] { "MainManager", "RegisterNewPlayer", login, gender, avatar };
        writer.PutArray(callMethod);
        server.Send(writer, DeliveryMethod.ReliableSequenced);
    }

    private void ShowEveryoneWhoThePlayer()
    {
        mainMenuScreen.GetComponent<MainMenu>().player = player;
        findGameScreen.GetComponent<FindGame>().player = player;
        createNewGame.GetComponent<CreateNewGame>().player = player;
    }

    private void ShowEveryoneWhoTheServer()
    {
        createNewGame.GetComponent<CreateNewGame>().server = server;
        findGameScreen.GetComponent<FindGame>().server = server;
    }
}
