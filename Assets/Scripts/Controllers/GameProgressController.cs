using MySecureBackend.WebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameProgressController : MonoBehaviour
{
    public GameProgressApiClient gameProgressApiClient;

    public async Task<GameProgress> Create(float levelProgress, int points)
    {
        GameProgress gameProgress = new GameProgress(levelProgress, points);

        string behandelingIDStr = PlayerPrefs.GetString("behandelingID", "");
        if (!string.IsNullOrEmpty(behandelingIDStr) && System.Guid.TryParse(behandelingIDStr, out System.Guid behandelingID))
            gameProgress.BehandelingID = behandelingID;

        IWebRequestReponse response = await gameProgressApiClient.Create(gameProgress);

        switch (response)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Create gameProgress success");
                return JsonConvert.DeserializeObject<GameProgress>(dataResponse.Data);
            case WebRequestError errorResponse:
                Debug.Log("Create gameProgress error: " + errorResponse.ErrorMessage);
                return null;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + response.GetType());
        }
    }

    public async Task<bool> UpdateItem(Guid gameProgressID, GameProgress gameProgress)
    {
        IWebRequestReponse response = await gameProgressApiClient.UpdateItem(gameProgressID, gameProgress);

        switch (response)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Update gameProgress success");
                return true;
            case WebRequestError errorResponse:
                Debug.Log("Update gameProgress error: " + errorResponse.ErrorMessage);
                return false;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + response.GetType());
        }
    }

    public async Task<List<GameProgress>> GetAll()
    {
        IWebRequestReponse response = await gameProgressApiClient.GetAll();

        switch (response)
        {
            case WebRequestData<string> dataResponse:
                List<GameProgress> progresses = JsonConvert.DeserializeObject<List<GameProgress>>(dataResponse.Data);
                Debug.Log("GameProgress opgehaald: " + progresses.Count);
                return progresses;
            case WebRequestError errorResponse:
                Debug.LogError("Get game progress error: " + errorResponse.ErrorMessage);
                return null;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + response.GetType());
        }
    }
}
