using MySecureBackend.WebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameProgressController : MonoBehaviour
{
    public GameProgressApiClient gameProgressApiClient;

    private List<GameProgress> cachedGameProgresses;
    private bool isCaching = false;

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
                ClearGameProgressCache();
                return JsonConvert.DeserializeObject<GameProgress>(dataResponse.Data);
            case WebRequestError errorResponse:
                Debug.LogError("Create gameProgress error: " + errorResponse.ErrorMessage);
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
                ClearGameProgressCache();
                return true;
            case WebRequestError errorResponse:
                Debug.LogError("Update gameProgress error: " + errorResponse.ErrorMessage);
                return false;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + response.GetType());
        }
    }

    public async Task<List<GameProgress>> GetAll()
    {
        // Return cached data if available
        if (cachedGameProgresses != null)
        {
            return cachedGameProgresses;
        }

        // Prevent multiple simultaneous requests
        if (isCaching)
        {
            while (isCaching)
            {
                await System.Threading.Tasks.Task.Delay(10);
            }
            return cachedGameProgresses;
        }

        isCaching = true;
        IWebRequestReponse response = await gameProgressApiClient.GetAll();

        switch (response)
        {
            case WebRequestData<string> dataResponse:
                cachedGameProgresses = JsonConvert.DeserializeObject<List<GameProgress>>(dataResponse.Data);
                isCaching = false;
                return cachedGameProgresses;
            case WebRequestError errorResponse:
                Debug.LogError("Get game progress error: " + errorResponse.ErrorMessage);
                isCaching = false;
                return null;
            default:
                isCaching = false;
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + response.GetType());
        }
    }

    public void ClearGameProgressCache()
    {
        cachedGameProgresses = null;
    }

    public async Task<GameProgress> GetOrCreate(float levelProgress, int points, int levelNumber = 0)
    {
        string behandelingIDStr = PlayerPrefs.GetString("behandelingID", "");
        if (string.IsNullOrEmpty(behandelingIDStr) || !System.Guid.TryParse(behandelingIDStr, out System.Guid behandelingID))
        {
            Debug.LogError("BehandelingID not found in PlayerPrefs");
            return null;
        }

        // Get all game progress records
        List<GameProgress> allProgresses = await GetAll();

        if (allProgresses != null && allProgresses.Count > 0)
        {
            // Try to find existing record for this behandeling and level
            // We use Points as a level indicator (Points stores which level: 1, 2, 3, or 4)
            GameProgress existingProgress = allProgresses.Find(p => 
                p.BehandelingID == behandelingID && p.Points == levelNumber);

            if (existingProgress != null)
            {
                return existingProgress;
            }
        }

        // No existing record found, create a new one
        GameProgress newProgress = await Create(levelProgress, levelNumber);
        return newProgress;
    }
}
