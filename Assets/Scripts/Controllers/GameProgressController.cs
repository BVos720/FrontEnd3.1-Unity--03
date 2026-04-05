using MySecureBackend.WebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GameProgressController : MonoBehaviour
{
    public GameProgressApiClient gameProgressApiClient;

    private List<GameProgress> cachedGameProgresses;
    private bool isCaching = false;
    private static bool isCreating = false;
    private static System.Collections.Generic.Dictionary<int, GameProgress> levelProgressCache = new();

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
        if (cachedGameProgresses != null)
            return cachedGameProgresses;

        
        if (isCaching)
        {
            while (isCaching)
                await System.Threading.Tasks.Task.Delay(10);

            return cachedGameProgresses;
        }

        isCaching = true;
        try
        {
            IWebRequestReponse response = await gameProgressApiClient.GetAll();

            switch (response)
            {
                case WebRequestData<string> dataResponse:
                    cachedGameProgresses = JsonConvert.DeserializeObject<List<GameProgress>>(dataResponse.Data);
                    return cachedGameProgresses;
                case WebRequestError errorResponse:
                    Debug.LogError("Get game progress error: " + errorResponse.ErrorMessage);
                    return null;
                default:
                    throw new NotImplementedException("No implementation for webRequestResponse of class: " + response.GetType());
            }
        }
        finally
        {
      
            isCaching = false;
        }
    }

    public void ClearGameProgressCache()
    {
        cachedGameProgresses = null;
    }

    public async Task<GameProgress> GetOrCreate(float levelProgress, int points, int levelNumber = 0)
    {
     
        if (levelProgressCache.TryGetValue(levelNumber, out GameProgress cached))
            return cached;

        string behandelingIDStr = PlayerPrefs.GetString("behandelingID", "");
        if (string.IsNullOrEmpty(behandelingIDStr) || !System.Guid.TryParse(behandelingIDStr, out System.Guid behandelingID))
        {
            Debug.LogError("BehandelingID not found in PlayerPrefs");
            return null;
        }

       
        while (isCreating)
            await System.Threading.Tasks.Task.Delay(50);

        
        if (levelProgressCache.TryGetValue(levelNumber, out cached))
            return cached;

        List<GameProgress> allProgresses = await GetAll();

        if (allProgresses != null)
        {
            GameProgress existingProgress = allProgresses
                .Where(p => p.BehandelingID == behandelingID)
                .OrderByDescending(p => p.LevelProgress)
                .FirstOrDefault();

            if (existingProgress != null)
            {
                levelProgressCache[levelNumber] = existingProgress;
                return existingProgress;
            }
        }

        
        isCreating = true;
        GameProgress newProgress = await Create(levelProgress, levelNumber);
        isCreating = false;

        if (newProgress != null)
            levelProgressCache[levelNumber] = newProgress;

        return newProgress;
    }

    public void ClearLevelProgressCache()
    {
        levelProgressCache.Clear();
    }
}