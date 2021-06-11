using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPlayerManager : MonoBehaviour
{
    public MainManager mainManager;
    public InputField nameEntry;
    public Text information;
    public string gender;
    public GameObject noGenderSelected;
    public Image imageBackgroundButtonMan;
    public Image imageBackgroundButtonWoman;

    public void CreateNewPlayer()
    {
        string login = nameEntry.text.ToLower();
        if (login.Length < 4)
        {
            nameEntry.text = "";
            information.text = "Имя должно содержать от 4 символов";
        }
        else if (login.Length > 12)
        {
            nameEntry.text = "";
            information.text = "Имя должно содержать не больше 12 символов";
        }
        else
        {
            bool registrationPermission = false;
            string allowedSymbols = "1234567890qwertyuiopasdfghjklzxcvbnmйцукенгшщзхъфывапролджячсмитьбюё-";
            for (int i = 0; i < login.Length; i++)
            {
                registrationPermission = false;
                foreach (var symbol in allowedSymbols)
                {
                    if (login[i] == symbol)
                    {
                        registrationPermission = true;
                        break;
                    }
                }
                if (registrationPermission == false)
                {
                    break;
                }
            }
            if (registrationPermission)
            {
                if (gender == "")
                {
                    noGenderSelected.SetActive(true);
                }
                else
                {
                    string avatar;
                    if (gender == "man")
                    {
                        avatar = "defaultMan";
                    }
                    else
                    {
                        avatar = "defaultWoman";
                    }
                    mainManager.RegisterNewPlayer(login, gender, avatar);
                }
            }
            else
            {
                nameEntry.text = "";
                information.text = "Имя содержит не допустимые символы";
            }
        }
    }

    public void RegistrationError()
    {
        nameEntry.text = "";
        information.text = "Имя уже занято, попробуйте другое";
    }

    public void SetGender(string gender)
    {
        this.gender = gender;
        switch (gender)
        {
            case "man":
                imageBackgroundButtonMan.color = Color.blue;
                imageBackgroundButtonWoman.color = Color.white;
                break;

            case "woman":
                imageBackgroundButtonMan.color = Color.white;
                imageBackgroundButtonWoman.color = Color.red;
                break;
        }
    }
}
