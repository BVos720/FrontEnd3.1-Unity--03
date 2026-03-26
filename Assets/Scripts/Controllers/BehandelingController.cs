using Assets.Scripts;
using MySecureBackend.WebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BehandelingController : MonoBehaviour
{
    public BehandelingApiClient behandelingApiClient;

    public async Task<Behandeling> Create(string type, DateTime datum, string artsNaam)
    {
        Behandeling behandeling = new Behandeling(type, datum, artsNaam);
        IWebRequestReponse response = await behandelingApiClient.Create(behandeling);

        switch (response)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Create behandeling success");
                return JsonConvert.DeserializeObject<Behandeling>(dataResponse.Data);
            case WebRequestError errorResponse:
                Debug.Log("Create behandeling error: " + errorResponse.ErrorMessage);
                return null;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + response.GetType());
        }
    }

    public async Task<List<Behandeling>> GetAll()
    {
        IWebRequestReponse response = await behandelingApiClient.GetAll();

        switch (response)
        {
            case WebRequestData<string> dataResponse:
                List<Behandeling> behandelingen = JsonConvert.DeserializeObject<List<Behandeling>>(dataResponse.Data);
                Debug.Log("Behandelingen opgehaald: " + behandelingen.Count);
                return behandelingen;
            case WebRequestError errorResponse:
                Debug.LogError("Get behandelingen error: " + errorResponse.ErrorMessage);
                return null;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + response.GetType());
        }
    }
}
