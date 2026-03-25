using System;
using Newtonsoft.Json;
using UnityEngine;
using MySecureBackend.WebApi.Models;

public class SettingsApiClient : MonoBehaviour
{
    public WebClient webClient;

    public async Awaitable<IWebRequestReponse> GetSettings()
    {
        return await webClient.SendGetRequest("/settings");
    }

    public async Awaitable<IWebRequestReponse> GetById(Guid settingsID)
    {
        return await webClient.SendGetRequest("/settings/" + settingsID);
    }

    public async Awaitable<IWebRequestReponse> Create(SettingsData settingsData)
    {
        string data = JsonConvert.SerializeObject(settingsData, JsonHelper.CamelCaseSettings);
        return await webClient.SendPostRequest("/settings", data);
    }

    public async Awaitable<IWebRequestReponse> UpdateItem(Guid settingsID, SettingsData settingsData)
    {
        string data = JsonConvert.SerializeObject(settingsData, JsonHelper.CamelCaseSettings);
        return await webClient.SendPutRequest("/settings/" + settingsID, data);
    }

    public async Awaitable<IWebRequestReponse> Delete(Guid settingsID)
    {
        return await webClient.SendDeleteRequest("/settings/" + settingsID);
    }
}
