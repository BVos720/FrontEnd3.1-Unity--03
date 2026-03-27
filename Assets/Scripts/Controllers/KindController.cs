using MySecureBackend.WebApi.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class KindController : MonoBehaviour
{
    public KindApiClient kindApiClient;

    public async Task<Kind> Create(string naam, int leeftijd)
    {
        Kind kind = new Kind(naam, leeftijd);
        IWebRequestReponse response = await kindApiClient.Create(kind);

        switch (response)
        {
            case WebRequestData<string> dataResponse:
                Kind aangemaaktKind = JsonConvert.DeserializeObject<Kind>(dataResponse.Data);
                Debug.Log("Create kind success, KindID: " + aangemaaktKind.KindID);
                return aangemaaktKind;
            case WebRequestError errorResponse:
                Debug.Log("Create kind error: " + errorResponse.ErrorMessage);
                return null;
            default:
                throw new System.NotImplementedException("No implementation for webRequestResponse of class: " + response.GetType());
        }
    }

    public async Task<List<Kind>> GetAll()
    {
        IWebRequestReponse response = await kindApiClient.GetAll();

        switch (response)
        {
            case WebRequestData<string> dataResponse:
                List<Kind> kinderen = JsonConvert.DeserializeObject<List<Kind>>(dataResponse.Data);
                Debug.Log("Kinderen opgehaald: " + kinderen.Count);
                return kinderen;
            case WebRequestError errorResponse:
                Debug.LogError("Get all kinderen error: " + errorResponse.ErrorMessage);
                return null;
            default:
                throw new System.NotImplementedException("No implementation for webRequestResponse of class: " + response.GetType());
        }
    }
}
