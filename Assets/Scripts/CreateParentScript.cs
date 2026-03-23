using MySecureBackend.WebApi.Models;
using System;
using TMPro;
using UnityEngine;

public class CreateParentScript : MonoBehaviour
{
    public OuderApiClient ouderApiClient;
    public GameObject OuderRegistratieScherm;
    public GameObject KindRegistratieScherm;
    public TMP_InputField OudernaamInput;
    private string OuderNaam;
    public Ouder Ouder;
    public async void CreateOuder()
        {
        OuderNaam = OudernaamInput.text;
        Ouder ouder = new Ouder(OuderNaam);

               IWebRequestReponse webRequestResponse = await ouderApiClient.Create(ouder);

              switch (webRequestResponse)
               {
                   case WebRequestData<string> dataResponse:
                       Debug.Log("Create ouder success");
                       OuderRegistratieScherm.SetActive(false);
                       KindRegistratieScherm.SetActive(true);
                break;
                   case WebRequestError errorResponse:
                       string errorMessage = errorResponse.ErrorMessage;
                        Debug.Log("Create ouder error: " + errorMessage);
                       // TODO: Handle error scenario. Show the errormessage to the user.
                        break;
                  default:
                       throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
               }
    }

}
