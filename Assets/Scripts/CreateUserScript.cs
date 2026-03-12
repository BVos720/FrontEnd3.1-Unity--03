using JetBrains.Annotations;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using static UinityAPIClient;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class CreateUserScript : MonoBehaviour
{
    public GameObject LoginScreen;
    public GameObject CreateUserScene;
    public GameObject CreateUserButon;
    public GameObject WWErrorShort;
    public GameObject UserError;
    public GameObject BackButton;
    public GameObject UserCreatedText;
    public TMP_InputField UserNameInput;
    public TMP_InputField PasswordInput;
    public UserApiClient UserAPI;

    public async void createUser()
    {
        //Input text van input fields in de front end
        string UserName = UserNameInput.text;
        string Password = PasswordInput.text;
        
        //Api word hier aangeroepen voor het registreren van een gebruiker
        IWebRequestReponse webRequestResponse = await UserAPI.Register(new User { Email = UserName, Password = Password });

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Register succes!");
                UserCreatedText.SetActive(true);
                UserError.SetActive(false);
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Register error: " + errorMessage);
                UserError.SetActive(true);
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());

        }
    }
    //Terug knop om terug naar de login te gaan 
        public void back()
        {
            CreateUserScene.SetActive(false);
            LoginScreen.SetActive(true);
         }

    }

