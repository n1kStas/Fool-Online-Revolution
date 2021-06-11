using LiteNetLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LiteNetLib.Utils;

public class GameRoom : MonoBehaviour
{
    public enum StatePlayer
    {
        waiting,
        attacker,
        defender
    }

    public GameObject table;
    public Player player;
    public NetPeer server;
    public GameObject enemyPrefab;
    public List<GameObject> enemysList;
    public GameObject room;
    public GameObject enemys;

    public int ID;

    public Sprite[] defaultAvatars;
    public Sprite[] defaultFrames;
    public Image playerAvatar;
    public Image playerFrame;
    public Text timerGame;
    public Text numberOfRemainingCard;
    public GameObject buttonConfirmReadiness;

    public GameObject waitingForOtherPlayers;
    public GameObject youDefend;
    public GameObject youGiveIn;

    public GameObject trumpCard;

    public GameObject gameTable;

    public StatePlayer statePlayer;

    public GameObject cardsInHand;
    public GameObject cardPrefab;
    public List<GameObject> playerCardsList = new List<GameObject>();
    public Button discardPile;
    public Button discardPileSkill;
    public Button toAbandonTheDefense;
    private bool _defenderTakesCards;
    public GameObject cardSelected;

    public void OpenGameRoom(int id, Player player, NetPeer server)
    {
        this.player = player;
        this.server = server;
        ID = id;
        table.SetActive(true);
        SetAvatar(player.avatar);
        SetFrame(player.frame);
        waitingForOtherPlayers.SetActive(true);
        youDefend.SetActive(false);
        youGiveIn.SetActive(false);
        timerGame.text = "";
    }

    private void SetAvatar(string avatar)
    {
        switch (avatar)
        {
            case ("defaultMan"):
                playerAvatar.sprite = defaultAvatars[0];
                break;

            case ("defaultWoman"):
                playerAvatar.sprite = defaultAvatars[1];
                break;
        }
    }

    private void SetFrame(string frame)
    {
        switch (frame)
        {
            case ("defaultFrame"):
                playerFrame.sprite = defaultFrames[0];
                break;
        }
    }

    public void CallSorter(string[] reader)
    {
        switch (reader[1])
        {
            case ("ShowNewConnectedPlayer"):
                ShowNewConnectedPlayer(reader);
                break;

            case ("ShowAlreadyConnectedPlayers"):
                ShowAlreadyConnectedPlayers(reader);
                break;

            case ("EnemyLeaveTheRoom"):
                EnemyLeaveTheRoom(reader);
                break;

            case ("ConfirmReadiness"):
                ConfirmReadiness();
                break;

            case ("GetTrump"):
                GetTrump(reader);
                break;

            case ("Attacker"):
                Attacker();
                break;

            case ("Defender"):
                Defender();
                break;

            case ("Waiting"):
                Waiting();
                break;

            case ("AttackCard"):
                AttackCard(reader);
                break;

            case ("DefendCard"):
                DefendCard(reader);
                break;

            case ("ClearGameTable"):
                ClearGameTable();
                break;

            case ("GetMissingCard"):
                GetMissingCard(reader);
                break;

            case ("NumberOfRemainingCard"):
                NumberOfRemainingCard(reader);
                break;

            case ("Win"):
                Win();
                break;

            case ("Lose"):
                Lose();
                break;

            case ("DefenderTakesTheCardsFromTheTable"):
                DefenderTakesTheCardsFromTheTable();
                break;

            case ("DefenderTakesTheCards"):
                DefenderTakesTheCards();
                break;
        }
    }

    private void ShowNewConnectedPlayer(string[] reader) //отобразить подключенного игрока
    {
        GameObject enemy = Instantiate(enemyPrefab, enemys.transform);
        enemysList.Add(enemy);
        enemy.GetComponent<Enemy>().SetLogin(reader[2]);
        enemy.GetComponent<Enemy>().SetAvatar(reader[3]);
        enemy.GetComponent<Enemy>().SetFrame(reader[4]);
    }

    private void ShowAlreadyConnectedPlayers(string[] reader) //отобразить уже подключенных игроков
    {
        GameObject enemy = Instantiate(enemyPrefab, enemys.transform);
        enemysList.Add(enemy);
        enemy.GetComponent<Enemy>().SetLogin(reader[2]);
        enemy.GetComponent<Enemy>().SetAvatar(reader[3]);
        enemy.GetComponent<Enemy>().SetFrame(reader[4]);
    }

    private void EnemyLeaveTheRoom(string[] reader) //игрок покинул комнату
    {
        foreach (GameObject desiredEnemy in enemysList)
        {
            if (desiredEnemy.GetComponent<Enemy>().login == reader[2])
            {
                enemysList.Remove(desiredEnemy);
                Destroy(desiredEnemy);
                break;
            }
        }
        buttonConfirmReadiness.SetActive(false);
    }

    private void ConfirmReadiness()
    {
        buttonConfirmReadiness.SetActive(true);
    }

    public void ThePlayerIsReadyToPlay()
    {
        NetDataWriter writer = new NetDataWriter();
        string[] callMethod = { "GameRoom", "ThePlayerIsReadyToPlay", ID.ToString() };
        writer.PutArray(callMethod);
        server.Send(writer, DeliveryMethod.Sequenced);
    }

    private void GetTrump(string[] reader)
    {
        trumpCard.GetComponent<Card>().SetValue(reader[3]);
        trumpCard.GetComponent<Card>().enabled = false;
    }

    private void Attacker()
    {
        _defenderTakesCards = false;
        waitingForOtherPlayers.SetActive(false);
        youDefend.SetActive(false);
        youGiveIn.SetActive(true);
        gameTable.GetComponent<Image>().enabled = true;
        toAbandonTheDefense.interactable = false;
        statePlayer = StatePlayer.attacker;
    }

    private void Defender()
    {
        _defenderTakesCards = false;
        waitingForOtherPlayers.SetActive(false);
        youDefend.SetActive(true);
        youGiveIn.SetActive(false);
        gameTable.GetComponent<Image>().enabled = false;
        statePlayer = StatePlayer.defender;
    }

    private void Waiting()
    {
        _defenderTakesCards = false;
        //gameInformation.text = "Ход противника";
        gameTable.GetComponent<Image>().enabled = false;
        statePlayer = StatePlayer.waiting;
    }

    public void Attack(Card card)
    {
        NetDataWriter writer = new NetDataWriter();
        string[] callMethod = { "GameRoom", "AttackCard", ID.ToString(), card.suit, card.rank };
        writer.PutArray(callMethod);
        server.Send(writer, DeliveryMethod.Sequenced);
        playerCardsList.Remove(card.gameObject);
        if (!_defenderTakesCards)
        {
            discardPile.interactable = false;
            discardPileSkill.interactable = false;
        }
    }

    private void AttackCard(string[] reader)
    {
        GameObject card = Instantiate(cardPrefab, gameTable.transform);
        card.transform.eulerAngles = new Vector3(0, 0, 5);
        card.GetComponent<Card>().SetValue(reader[2]);
        card.GetComponent<Card>().gameRoom = this;
        card.GetComponent<Card>().transformParent = gameTable.transform;
        card.GetComponent<Card>().placeToProtect.SetActive(true);
        gameTable.GetComponent<GameTable>().cardsOnTheTable.Add(card);
        switch (statePlayer)
        {
            case StatePlayer.attacker:
                card.GetComponent<Image>().enabled = false;
                break;

            case StatePlayer.waiting:
                card.GetComponent<Image>().enabled = false;
                break;

            case StatePlayer.defender:
                if (!_defenderTakesCards)
                {
                    toAbandonTheDefense.interactable = true;
                }
                break;
        }
    }

    public void Defend(Card card)
    {
        NetDataWriter writer = new NetDataWriter();
        string[] callMethod = { "GameRoom", "DefendCard", ID.ToString(), card.suit, card.rank,
            card.transformParent.transform.parent.GetComponent<Card>().suit, card.transformParent.transform.parent.GetComponent<Card>().rank};
        writer.PutArray(callMethod);
        server.Send(writer, DeliveryMethod.Sequenced);
        playerCardsList.Remove(card.gameObject);

        card.transform.parent.parent.GetComponent<Card>().defendCard = card.gameObject;
        int countCardOnToCard = gameTable.transform.childCount;
        int countDefenceCard = 0;
        for (int i = 0; i < countCardOnToCard; i++)
        {
            if (gameTable.transform.GetChild(i).GetComponent<Card>().defendCard != null)
            {
                countDefenceCard++;
            }
        }
        if (countCardOnToCard == countDefenceCard)
        {
            toAbandonTheDefense.interactable = false;
        }
        else
        {
            toAbandonTheDefense.interactable = true;
        }
    }

    private void DefendCard(string[] reader)
    {
        GameObject card = Instantiate(cardPrefab);
        card.transform.eulerAngles = new Vector3(0, 0, -10);
        card.GetComponent<Card>().SetValue(reader[2]);
        foreach (Card cardOnTheTable in gameTable.transform.GetComponentsInChildren<Card>())
        {
            if (cardOnTheTable.suit == reader[3] && cardOnTheTable.rank == reader[4])
            {
                card.transform.SetParent(cardOnTheTable.placeToProtect.transform, false);
                cardOnTheTable.placeToProtect.SetActive(true);
                card.transform.localScale = Vector3.one;
                cardOnTheTable.defendCard = card;
            }
        }
        card.GetComponent<Card>().gameRoom = this;
        card.GetComponent<Card>().placeToProtect.SetActive(false);
        gameTable.GetComponent<GameTable>().cardsOnTheTable.Add(card);

        int countCardOnToCard = gameTable.transform.childCount;
        int countDefenceCard = 0;
        for (int i = 0; i < countCardOnToCard; i++)
        {
            if (gameTable.transform.GetChild(i).GetComponent<Card>().defendCard != null)
            {
                countDefenceCard++;
            }
        }
        if (countCardOnToCard == countDefenceCard)
        {
            discardPile.interactable = true;
            discardPileSkill.interactable = true;
        }
        else
        {
            discardPile.interactable = false;
            discardPileSkill.interactable = false;
        }
    }

    public void DiscardPile()
    {
        NetDataWriter writer = new NetDataWriter();
        string[] callMethod = { "GameRoom", "DiscardPile", ID.ToString() };
        writer.PutArray(callMethod);
        server.Send(writer, DeliveryMethod.Sequenced);
        discardPile.interactable = false;
        discardPileSkill.interactable = false;

        enemysList[0].GetComponent<Enemy>().DisabledMiniChat();
    }

    private void ClearGameTable()
    {
        gameTable.GetComponent<GameTable>().ClearGameTable();
    }

    private void GetMissingCard(string[] reader)
    {
        GameObject card = Instantiate(cardPrefab, cardsInHand.transform);
        playerCardsList.Add(card);
        card.GetComponent<Card>().SetValue(reader[2]);
        card.GetComponent<Card>().gameRoom = this;
        card.GetComponent<Card>().transformParent = cardsInHand.transform;
        card.GetComponent<Card>().placeToProtect.SetActive(false);
    }

    private void NumberOfRemainingCard(string[] reader)
    {
        numberOfRemainingCard.text = reader[2];
    }

    public void ToAbandonTheDefense()
    {
        NetDataWriter writer = new NetDataWriter();
        string[] callMethod = { "GameRoom", "ToAbandonTheDefense", ID.ToString() };
        writer.PutArray(callMethod);
        server.Send(writer, DeliveryMethod.Sequenced);
        _defenderTakesCards = true;
        toAbandonTheDefense.interactable = false;
    }

    private void DefenderTakesTheCardsFromTheTable()
    {
        _defenderTakesCards = false;
        List<GameObject> cardsOnTheTable = gameTable.GetComponent<GameTable>().GiveCardsToThePlayer();
        foreach (GameObject card in cardsOnTheTable)
        {
            card.GetComponent<Card>().transformParent = cardsInHand.transform;
            playerCardsList.Add(card);
            card.transform.SetParent(card.GetComponent<Card>().transformParent, false);
            card.GetComponent<Card>().TheCardIsBackInHand();
            card.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 636);
            card.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 900);
            card.transform.eulerAngles = Vector3.zero;
            card.GetComponent<Card>().placeToProtect.SetActive(false);
        }
    }

    private void DefenderTakesTheCards()
    {
        _defenderTakesCards = true;
        discardPile.interactable = true;
        discardPileSkill.interactable = true;
        enemysList[0].GetComponent<Enemy>().EnabledMiniChat("Беру");
    }

    private void Win()
    {
        //gameInformation.text = "Вы выиграли";
        gameTable.GetComponent<Image>().enabled = false;
        statePlayer = StatePlayer.waiting;
    }

    private void Lose()
    {
        //gameInformation.text = "Вы проиграли";
        gameTable.GetComponent<Image>().enabled = false;
        statePlayer = StatePlayer.waiting;
    }

    public void ChooseACard(GameObject cardSelected)
    {
        this.cardSelected = cardSelected;
    }
}
