using System;
using TMPro;
using UnityEngine;

public class InlogPaginaScript : MonoBehaviour
{
    public GameObject InlogSchermAccount;
    public GameObject CreeërOuderScherm;
    public GameObject LevelOverzicht;
    public TMP_InputField EmailInput;
    public TMP_InputField PasswordInput;
    public LoginController loginController;
    public AudioSource GameTheme;


    public void Start()
    {
        
    }


    public async void Login()
    {
        string token = await loginController.Login(EmailInput.text, PasswordInput.text);
        if (token != null)
        {
            InlogSchermAccount.SetActive(false);
            LevelOverzicht.SetActive(true);
        }
    }

    public async void LoginAfterRegister(string email, string password)
    {
        string token = await loginController.Login(email, password);
        if (token != null)
        {
            InlogSchermAccount.SetActive(false);
            CreeërOuderScherm.SetActive(true);
        }
    }

    public async void Register()
    {
        string email = EmailInput.text;
        string password = PasswordInput.text;
        bool success = await loginController.Register(email, password);
        if (success)
            LoginAfterRegister(email, password);
    }
}
