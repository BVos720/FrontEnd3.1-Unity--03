using MySecureBackend.WebApi.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using UnityEngine;

public class OuderController : MonoBehaviour
{
    public OuderApiClient ouderApiClient;

    public async Task<Ouder> Create(string naam)
    {
        Ouder ouder = new Ouder(naam);
        IWebRequestReponse response = await ouderApiClient.Create(ouder);

        switch (response)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Create ouder success");
                return JsonConvert.DeserializeObject<Ouder>(dataResponse.Data);
            case WebRequestError errorResponse:
                Debug.Log("Create ouder error: " + errorResponse.ErrorMessage);
                return null;
            default:
                throw new System.NotImplementedException("No implementation for webRequestResponse of class: " + response.GetType());
        }
    }
}
