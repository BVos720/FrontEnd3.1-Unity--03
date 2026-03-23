using JetBrains.Annotations;
using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class InlogPaginaScript : MonoBehaviour
{
    public GameObject InlogSchermAccount;
    public GameObject CreeërOuderScherm;
    public GameObject LevelOverzicht;
    public TMP_InputField EmailInput;
    public TMP_InputField PasswordInput;
    public UserApiClient UserApiClient;
    public User User;
    
    private string email;
    private string password;
    public async void Login()
    {
        email = EmailInput.text;
        password = PasswordInput.text;
        User user = new User(email, password);

        IWebRequestReponse webRequestResponse = await UserApiClient.Login(user);
        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Login succes!");
                Debug.Log("Token: " + dataResponse.Data);
                InlogSchermAccount.SetActive(false);
                CreeërOuderScherm.SetActive(true);
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Login error: " + errorMessage);
                // TODO: Handle error scenario. Show the errormessage to the user.
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    public async void LoginAfterRegister(string Email, string Password)
    {
 
        User user = new User(Email, Password);

        IWebRequestReponse webRequestResponse = await UserApiClient.Login(user);
        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Login succes!");
                Debug.Log("Token: " + dataResponse.Data);
                InlogSchermAccount.SetActive(false);
                LevelOverzicht.SetActive(true);
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Login error: " + errorMessage);
                // TODO: Handle error scenario. Show the errormessage to the user.
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    public async void Register()
    {
        email = EmailInput.text;
        password = PasswordInput.text;
        User user = new User(email, password);

        IWebRequestReponse webRequestResponse = await UserApiClient.Register(user);

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Register succes!");
                LoginAfterRegister(email, password);
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Register error: " + errorMessage);
                // TODO: Handle error scenario. Show the errormessage to the user.
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
}
}
