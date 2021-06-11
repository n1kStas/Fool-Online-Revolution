using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text login;
    public Image avatar;
    public Text cotbucks;
    public Text hotbucks;
    public Text level;
    public Slider levelProgress;
    public Player player;
    public Image frame;

    public Sprite[] defaultAvatars;
    public Sprite[] defaultFrames;

    public void ShowPlayerInformation()
    {
        login.text = player.login;
        SetAvatar(player.avatar);
        SetFrame(player.frame);
        cotbucks.text = player.cotbucks.ToString();
        hotbucks.text = player.hotbucks.ToString();
        level.text = player.level.ToString();
        levelProgress.value = player.levelProgress;
    }

    private void SetAvatar(string avatar)
    {
        switch (avatar)
        {
            case ("defaultMan"):
                this.avatar.sprite = defaultAvatars[0];
                break;

            case ("defaultWoman"):
                this.avatar.sprite = defaultAvatars[1];
                break;
        }
    }

    private void SetFrame(string frame)
    {
        switch (frame)
        {
            case ("defaultFrame"):
                this.frame.sprite = defaultFrames[0];
                break;
        }
    }

    public void FonControl(GameObject fon)
    {
        fon.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        fon.transform.position = new Vector2(1, 1);
    }
}
