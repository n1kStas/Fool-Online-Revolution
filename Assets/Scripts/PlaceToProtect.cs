using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaceToProtect : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.parent.GetComponent<Card>().transformParent.CompareTag("Game Table") && transform.childCount == 0)
        {
            Card card = eventData.pointerDrag.GetComponent<Card>();

            if(card.suit == transform.parent.GetComponent<Card>().gameRoom.trumpCard.GetComponent<Card>().suit)
            {
                if (transform.parent.GetComponent<Card>().suit == transform.parent.GetComponent<Card>().gameRoom.trumpCard.GetComponent<Card>().suit)
                {
                    if (card.numberRank > transform.parent.GetComponent<Card>().numberRank)
                    {
                        card.transformParent = transform;
                        card.gameRoom.gameTable.GetComponent<GameTable>().cardsOnTheTable.Add(card.gameObject);
                    }
                }
                else
                {
                    card.transformParent = transform;
                    card.transformParent = transform;
                    card.gameRoom.gameTable.GetComponent<GameTable>().cardsOnTheTable.Add(card.gameObject);
                }
            }
            else
            {
                if(transform.parent.GetComponent<Card>().suit == card.suit)
                {
                    if(transform.parent.GetComponent<Card>().numberRank < card.numberRank)
                    {
                        card.transformParent = transform;
                        card.transformParent = transform;
                        card.gameRoom.gameTable.GetComponent<GameTable>().cardsOnTheTable.Add(card.gameObject);
                    }
                }
            }
        }
    }
}
