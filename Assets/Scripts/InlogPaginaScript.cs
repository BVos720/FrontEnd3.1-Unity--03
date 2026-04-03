using System;
using TMPro;
using UnityEngine;

public class InlogPaginaScript : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject InlogSchermAccount;
    public GameObject CreeërOuderScherm;
    public GameObject LevelOverzicht;
    public TMP_InputField EmailInput;
    public TMP_InputField PasswordInput;
    public LoginController loginController;
    public AudioSource GameTheme;

    [Header("ErrorMessages")]
    public TMP_Text EmailError;
    public TMP_Text PasswordError;



    public void Start()
    {
     EmailError.text = "";
     PasswordError.text = "";
    }


    public async void Login()
    {
        string token = await loginController.Login(EmailInput.text, PasswordInput.text);
        if (token != null)
        {
            InlogSchermAccount.SetActive(false);
            LevelOverzicht.SetActive(true);
        }
        else
        {
            PasswordError.text = "Fout bij inloggen, controleer je email en wachtwoord";
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
        if (email == null)
        {
            Debug.Log("Foutieve email");
            EmailError.text = "Email mag niet leeg zijn";
        }
        else if (password == null || password.Length < 9)
        {
             PasswordError.text = "Wachtwoord moet minimaal 9 characters lang zijn en 1 uppercase, lowercase, cijfer en special character bevatten";
        }
        bool success = await loginController.Register(email, password);
        if (success)
        {
            LoginAfterRegister(email, password);
        }
        else
        {
            Debug.Log("Registreer fout");
            PasswordError.text = "Fout bij registreren";
        }
    }
}
