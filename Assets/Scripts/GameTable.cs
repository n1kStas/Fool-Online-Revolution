using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameTable : MonoBehaviour, IDropHandler
{
    public List<GameObject> cardsOnTheTable = new List<GameObject>();

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount < 6)
        {
            Card card = eventData.pointerDrag.GetComponent<Card>();
            if (transform.childCount == 0)
            {
                card.transformParent = transform;
                card.GetComponent<CanvasGroup>().blocksRaycasts = false;
                cardsOnTheTable.Add(card.gameObject);
            }
            else
            {
                foreach(Card cardOnTheRable in transform.GetComponentsInChildren<Card>())
                {
                    if(cardOnTheRable.rank == card.rank)
                    {
                        card.transformParent = transform;
                        card.GetComponent<CanvasGroup>().blocksRaycasts = false;
                        cardsOnTheTable.Add(card.gameObject);
                    }
                    else
                    {
                        try
                        {
                            if (cardOnTheRable.placeToProtect.GetComponentInChildren<Card>().rank == card.rank)
                            {
                                card.transformParent = transform;
                                card.GetComponent<CanvasGroup>().blocksRaycasts = false;
                                cardsOnTheTable.Add(card.gameObject);
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }
    }

    public void ClearGameTable()
    {
        foreach (GameObject card in cardsOnTheTable)
        {
            if (!card.transform.parent.CompareTag("Hand"))
            {
                Destroy(card);
            }
        }

        cardsOnTheTable.Clear();
    }
    
    public List<GameObject> GiveCardsToThePlayer()
    {
        return cardsOnTheTable;
    }
}
