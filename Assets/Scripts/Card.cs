using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string suit;
    public string rank;
    public int numberRank;

    public Sprite[] heartSprites;
    public Sprite[] clubsSprites;
    public Sprite[] diamondsSprites;
    public Sprite[] spadesSprites;

    private Camera _mainCamera;
    public Transform transformParent;
    public GameRoom gameRoom;

    public GameObject placeToProtect;
    private bool _drag;

    public GameObject defendCard;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (transformParent.CompareTag("Hand") && gameRoom.cardSelected == null)
        {
            gameRoom.cardSelected = gameObject;
            _drag = true;
            transformParent = transform.parent;
            transform.SetParent(transformParent.parent);
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (transformParent.CompareTag("Hand") && gameRoom.cardSelected == gameObject)
        {
            Vector2 newPosition = _mainCamera.ScreenToWorldPoint(eventData.position);
            transform.position = newPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_drag && gameRoom.cardSelected == gameObject)
        {
            gameRoom.cardSelected = null;
            _drag = false;
            transform.SetParent(transformParent);
            if (transformParent.CompareTag("Hand"))
            {
                TheCardIsBackInHand();
            }
            else if (transformParent.CompareTag("Game Table"))
            {
                GetComponent<CanvasGroup>().blocksRaycasts = false;
                transform.eulerAngles = new Vector3(0, 0, 5);
                gameRoom.Attack(this);
            }
            else if (transformParent.CompareTag("Place to protect"))
            {
                GetComponent<CanvasGroup>().blocksRaycasts = false;
                transform.eulerAngles = new Vector3(0, 0, -10);
                gameRoom.Defend(this);
            }
        }
    }

    public void TheCardIsBackInHand()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void SetValue(string cardValue)
    {
        bool knownSuit = false;
        for (int i = 0; i < cardValue.Length; i++)
        {
            if (knownSuit)
            {
                rank += cardValue[i];
            }
            else
            {
                if(cardValue[i].ToString() == " ")
                {
                    knownSuit = true;
                }
                else
                {
                    suit += cardValue[i];
                }
            }
        }


        #region rank card
        if (rank == "2")
        {
            numberRank = 0;
        }
        else if (rank == "3")
        {
            numberRank = 1;
        }
        else if (rank == "4")
        {
            numberRank = 2;
        }
        else if (rank == "5")
        {
            numberRank = 3;
        }
        else if (rank == "6")
        {
            numberRank = 4;
        }
        else if (rank == "7")
        {
            numberRank = 5;
        }
        else if (rank == "8")
        {
            numberRank = 6;
        }
        else if (rank == "9")
        {
            numberRank = 7;
        }
        else if (rank == "10")
        {
            numberRank = 8;
        }
        else if (rank == "Jack")
        {
            numberRank = 9;
        }
        else if (rank == "Queen")
        {
            numberRank = 10;
        }
        else if (rank == "King")
        {
            numberRank = 11;
        }
        else if (rank == "Ace")
        {
            numberRank = 12;
        }
        #endregion

        ShowCard();
    }

    private void ShowCard()
    {        
        switch (suit)
        {
            case ("Hearts"):
                GetComponent<Image>().sprite = heartSprites[numberRank];
                break;

            case ("Clubs"):
                GetComponent<Image>().sprite = clubsSprites[numberRank];
                break;

            case ("Diamonds"):
                GetComponent<Image>().sprite = diamondsSprites[numberRank];
                break;

            case ("Spades"):
                GetComponent<Image>().sprite = spadesSprites[numberRank];
                break;
        }

    }
}