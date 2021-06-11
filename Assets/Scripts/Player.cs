using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string login;
    public string password;
    public string gender;
    public int cotbucks;
    public int hotbucks;
    public int level;
    public float levelProgress;
    public string avatar;
    public string frame;

    public void LoadPlayerData()
    {
        login = PlayerPrefs.GetString("login");
        password = PlayerPrefs.GetString("password");
    }

    public void SavePlayerSecretData(string login, string password)
    {
        PlayerPrefs.SetString("login", login);
        PlayerPrefs.SetString("password", password);
    }

    public void SavePlayerData(string gender,int cotbucks, int hotbucks, int level, float levelProgress, string avatar, string frame)
    {
        this.gender = gender;
        this.cotbucks = cotbucks;
        this.hotbucks = hotbucks;
        this.level = level;
        this.levelProgress = levelProgress;
        this.avatar = avatar;
        this.frame = frame;
    }
}
