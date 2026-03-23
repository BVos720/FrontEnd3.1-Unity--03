using MySecureBackend.WebApi.Models;
using Newtonsoft.Json;
using System;
using TMPro;
using UnityEngine;

public class KindRegistratieScherm : MonoBehaviour
{
    public GameObject kindRegistratieScherm;
    public GameObject SelectBehandelingScherm;
    public KindApiClient kindApiClient;
    public Kind kind;
    public TMP_InputField KindNaamInput;
    public TMP_InputField KindLeeftijdInput;
    

    public async void CreateKind()
        {
        Kind kind = new Kind(KindNaamInput.text, Convert.ToInt32(KindLeeftijdInput.text));

            IWebRequestReponse webRequestResponse = await kindApiClient.Create(kind);

            switch (webRequestResponse)
            {
               case WebRequestData<string> dataResponse:
                    Kind aangemaaktKind = JsonConvert.DeserializeObject<Kind>(dataResponse.Data);
                    PlayerPrefs.SetString("kindID", aangemaaktKind.KindID.ToString());
                    Debug.Log("Create kind success, KindID: " + aangemaaktKind.KindID);
                    SelectBehandelingScherm.SetActive(true);
                    kindRegistratieScherm.SetActive(false);
                break;
                case WebRequestError errorResponse:
                    string errorMessage = errorResponse.ErrorMessage;
                    Debug.Log("Create kind error: " + errorMessage);
                    // TODO: Handle error scenario. Show the errormessage to the user.
                    break;
                default:
                    throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
            }
        }

}
