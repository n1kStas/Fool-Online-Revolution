using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayButtonsInTheMainMenu : MonoBehaviour
{
    public Sprite enabledSprite;
    public Sprite disabledSprite;

    public void Activated()
    {
        GetComponent<Image>().sprite = enabledSprite;
    }

    public void Deactivated()
    {
        GetComponent<Image>().sprite = disabledSprite;
    }
}
