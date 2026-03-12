using JetBrains.Annotations;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UinityAPIClient;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class Inlog : MonoBehaviour
{
    public GameObject SelectWorld;
    public GameObject inlog;
    public GameObject CreateUser;
    public TMP_InputField GebruikersVeld;
    public TMP_InputField WachtwoordVeld;
    private string IngevoerdeEmail;
    private string ingevoerdeWachtwoord;
    public UserApiClient UserAPI;
    public TMP_Text InlogErrorText;

    //Stuurt je door naar de CreateUser scene   
    public void ToonCreateUser()
    {
        inlog.SetActive(false);
        CreateUser.SetActive(true);
    }


    public async void Login()
    {
        //Invoer velden van de login pagina
        IngevoerdeEmail = GebruikersVeld.text;
        ingevoerdeWachtwoord = WachtwoordVeld.text;

        //API Word aangeroepen voor de Login 
        IWebRequestReponse webRequestResponse = await UserAPI.Login(new User { Email = IngevoerdeEmail, Password = ingevoerdeWachtwoord });

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                //Als login succesvol is ga je door naar de SelectWorld scene
                Debug.Log("Login succes!");
                inlog.SetActive(false);
                SelectWorld.SetActive(true);
                break;
            case WebRequestError errorResponse:
                //Als de login onsucessvol is, wordt de error message getoond in de console en op het scherm
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Login error: " + errorMessage);
                InlogErrorText.gameObject.SetActive(true);

                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());


        }

        
    }
}
