using MySecureBackend.WebApi.Models;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    public SettingsApiClient settingsApiClient;

    public async Task<SettingsData> GetSettings()
    {
        IWebRequestReponse response = await settingsApiClient.GetSettings();

        switch (response)
        {
            case WebRequestData<string> dataResponse:
                SettingsData loaded = JsonConvert.DeserializeObject<SettingsData>(dataResponse.Data);
                Debug.Log("Settings geladen");
                return loaded;
            case WebRequestError errorResponse:
                Debug.LogWarning("Kon settings niet laden: " + errorResponse.ErrorMessage);
                return null;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + response.GetType());
        }
    }

    public async Task<bool> UpdateItem(Guid id, SettingsData settingsData)
    {
        IWebRequestReponse response = await settingsApiClient.UpdateItem(id, settingsData);

        switch (response)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Update settings success");
                return true;
            case WebRequestError errorResponse:
                Debug.Log("Update settings error: " + errorResponse.ErrorMessage);
                return false;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + response.GetType());
        }
    }

    public async Task<bool> Create(SettingsData settingsData)
    {
        IWebRequestReponse response = await settingsApiClient.Create(settingsData);

        switch (response)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Create settings success");
                return true;
            case WebRequestError errorResponse:
                Debug.Log("Create settings error: " + errorResponse.ErrorMessage);
                return false;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + response.GetType());
        }
    }
}
