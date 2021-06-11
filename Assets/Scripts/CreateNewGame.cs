using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LiteNetLib;
using LiteNetLib.Utils;

public class CreateNewGame : MonoBehaviour
{
    public Text bet;
    public Text players;
    public Text cotbucks;
    public Text hotbucks;
    public Text level;
    public Player player;

    public Slider betValue;
    public Slider playerValue;

    public Image mode1;
    public Image mode2;
    public Slider slider1;

    public Image mode3;
    public Image mode4;
    public Slider slider2;

    public Image mode5;
    public Image mode6;
    public Slider slider3;

    public GameObject deckSelection;
    public Button deck24;

    public GameObject fon;
    public GameObject loadingScreen;
    public Slider progressLoadingGame;

    public GameObject menuNavigation;
    public GameObject setNewGameScreen;
    public GameObject gameRoom;
    string[] callMethod;
    public NetPeer server;

    private int _betCount;
    private int _playerCount;
    private string _abilityToTransferToFightOffAnotherPlayer;
    private string _whoCanThrowCards;
    private string _hiddenFool;

    private void Update()
    {
        BetCount();
        PlayersCount();
        AbilityToTransferToFightOffAnotherPlayer();
        WhoCanThrowCards();
        HiddenFool();
    }

    public void GameModeSet()
    {
        if (_betCount > 0)
        {
            deckSelection.SetActive(true);
        }
    }

    public void ShowPlayerInformation()
    {
        cotbucks.text = player.cotbucks.ToString();
        hotbucks.text = player.hotbucks.ToString();
        level.text = player.level.ToString();
    }

    public void CallSorter(string[] reader) 
    {
        switch (reader[1]) {
            case ("CreateRoom"):
                StartCoroutine(CreateRoom(reader[2]));
                break;
        }
    }

    public void CreateNewRoom(int deck)
    {
        NetDataWriter writer = new NetDataWriter();
        callMethod = new[] { "ControlRooms", "CreateNewRoom", _betCount.ToString(), _playerCount.ToString(), _abilityToTransferToFightOffAnotherPlayer, _whoCanThrowCards, _hiddenFool, deck.ToString()};
        writer.PutArray(callMethod);
        server.Send(writer, DeliveryMethod.ReliableSequenced);
        menuNavigation.SetActive(false);
        setNewGameScreen.SetActive(false);
        fon.SetActive(false);
        loadingScreen.SetActive(true);
        progressLoadingGame.value += 50;
    }

    private IEnumerator CreateRoom(string idRoom)
    {
        progressLoadingGame.value += 50;        
        yield return new WaitForSeconds(1f);
        loadingScreen.SetActive(false);        
        gameRoom.SetActive(true);
        gameRoom.GetComponent<GameRoom>().OpenGameRoom(int.Parse(idRoom), player, server);        
        gameObject.SetActive(false);
    }

    private void BetCount()
    {
        #region enough for bet
        if (betValue.value == 0 && player.cotbucks >= 100)
        {
            bet.text = "ставка - 100";
            _betCount = 100;
        }
        else if (betValue.value == 1 && player.cotbucks >= 250)
        {
            bet.text = "ставка - 250";
            _betCount = 250;
        }
        else if (betValue.value == 2 && player.cotbucks >= 500)
        {
            bet.text = "ставка - 500";
            _betCount = 500;
        }
        else if (betValue.value == 3 && player.cotbucks >= 1000)
        {
            bet.text = "ставка - 1000";
            _betCount = 1000;
        }
        else if (betValue.value == 4 && player.cotbucks >= 2500)
        {
            bet.text = "ставка - 2.5к";
            _betCount = 2500;
        }
        else if (betValue.value == 5 && player.cotbucks >= 5000)
        {
            bet.text = "ставка - 5к";
            _betCount = 5000;
        }
        else if (betValue.value == 6 && player.cotbucks >= 10000)
        {
            bet.text = "ставка - 10к";
            _betCount = 10000;
        }
        else if (betValue.value == 7 && player.cotbucks >= 25000)
        {
            bet.text = "ставка - 25к";
            _betCount = 25000;
        }
        else if (betValue.value == 8 && player.cotbucks >= 50000)
        {
            bet.text = "ставка - 50к";
            _betCount = 50000;
        }
        else if (betValue.value == 9 && player.cotbucks >= 100000)
        {
            bet.text = "ставка - 100к";
            _betCount = 100000;
        }
        else if (betValue.value == 10 && player.cotbucks >= 250000)
        {
            bet.text = "ставка - 250к";
            _betCount = 250000;
        }
        else if (betValue.value == 11 && player.cotbucks >= 500000)
        {
            bet.text = "ставка - 500к";
            _betCount = 500000;
        }
        else if (betValue.value == 12 && player.cotbucks >= 1000000)
        {
            bet.text = "ставка - 100кк";
            _betCount = 1000000;
        }
        #endregion
        #region enough for bet
        else if (betValue.value == 0 && player.cotbucks < 100 || betValue.value == 1 && player.cotbucks < 250 || betValue.value == 2 && player.cotbucks < 500 ||
            betValue.value == 3 && player.cotbucks < 1000 || betValue.value == 4 && player.cotbucks < 2500 || betValue.value == 5 && player.cotbucks < 5000 ||
            betValue.value == 6 && player.cotbucks < 10000 || betValue.value == 7 && player.cotbucks < 25000 || betValue.value == 8 && player.cotbucks < 50000 ||
            betValue.value == 9 && player.cotbucks < 100000 || betValue.value == 10 && player.cotbucks < 250000 || betValue.value == 10 && player.cotbucks < 500000 ||
            betValue.value == 10 && player.cotbucks < 1000000)
        {
            bet.text = "не хватает cotbucks для ставки";
            _betCount = 0;
        }
        #endregion
    }

    private void PlayersCount()
    {
        if(playerValue.value == 0)
        {
            players.text = "Игроков - 2";
            _playerCount = 2;
        }
        else if (playerValue.value == 1)
        {
            players.text = "Игроков - 3";
            _playerCount = 3;
        }
        else if (playerValue.value == 2)
        {
            players.text = "Игроков - 4";
            _playerCount = 4;
        }
        else if (playerValue.value == 3)
        {
            players.text = "Игроков - 5";
            _playerCount = 5;
        }
        else if (playerValue.value == 4)
        {
            players.text = "Игроков - 6";
            _playerCount = 6;
        }
        if (_playerCount < 5)
        {
            deck24.interactable = true;
        }
        else
        {
            deck24.interactable = false;
        }
    }

    private void AbilityToTransferToFightOffAnotherPlayer()
    {
        if (slider1.value == 1)
        {
            mode1.color = new Color32(255, 255, 255, 100);
            mode2.color = new Color32(255, 255, 255, 255);
            _abilityToTransferToFightOffAnotherPlayer = "transferable";
        }
        else
        {
            mode1.color = new Color32(255, 255, 255, 255);
            mode2.color = new Color32(255, 255, 255, 100);
            _abilityToTransferToFightOffAnotherPlayer = "default";            
        }
    }
    private void WhoCanThrowCards()
    {
        if (slider2.value == 1)
        {
            mode3.color = new Color32(255, 255, 255, 100);
            mode4.color = new Color32(255, 255, 255, 255);
            _whoCanThrowCards = "all";
        }
        else
        {
            mode3.color = new Color32(255, 255, 255, 255);
            mode4.color = new Color32(255, 255, 255, 100);
            _whoCanThrowCards = "aroundTheEdges";            
        }
    }

    private void HiddenFool()
    {
        if (slider3.value == 1)
        {
            mode5.color = new Color32(255, 255, 255, 100);
            mode6.color = new Color32(255, 255, 255, 255);
            _hiddenFool = "hidenFool";            
        }
        else
        {
            mode5.color = new Color32(255, 255, 255, 255);
            mode6.color = new Color32(255, 255, 255, 100);
            _hiddenFool = "default";        }
    }

}
