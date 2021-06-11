using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public string login;
    public TextMeshProUGUI loginShow;
    public Image avatar;
    public Image frame;

    public Sprite[] defaultAvatars;
    public Sprite[] defaultFrames;

    public GameObject miniChat;
    public TextMeshProUGUI message;

    public void SetLogin(string login)
    {
        this.login = login;
        loginShow.text = login;
    }

    public void SetAvatar(string nameAvatar)
    {
        switch (nameAvatar)
        {
            case ("defaultMan"):
                avatar.sprite = defaultAvatars[0];
                break;

            case ("defaultWoman"):
                avatar.sprite = defaultAvatars[1];
                break;
        }
    }

    public void EnabledMiniChat(string message)
    {
        this.message.text = message;
        miniChat.SetActive(true);        
    }

    public void DisabledMiniChat()
    {
        message.text = "";
        miniChat.SetActive(false);        
    }

    public void SetFrame(string nameFrame)
    {
        switch (nameFrame)
        {
            case ("defaultFrame"):
                frame.sprite = defaultFrames[0];
                break;
        }
    }
}
