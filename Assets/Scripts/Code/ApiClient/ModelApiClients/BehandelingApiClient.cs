using System;
using Newtonsoft.Json;
using UnityEngine;
using MySecureBackend.WebApi.Models;

public class BehandelingApiClient : MonoBehaviour
{
    public WebClient webClient;

    public async Awaitable<IWebRequestReponse> GetAll()
    {
        string route = "/behandeling";
        return await webClient.SendGetRequest(route);
    }

    public async Awaitable<IWebRequestReponse> GetById(Guid behandelingID)
    {
        string route = "/behandeling/" + behandelingID;
        return await webClient.SendGetRequest(route);
    }

    public async Awaitable<IWebRequestReponse> Create(Behandeling behandeling)
    {
        string route = "/behandeling";
        string data = JsonConvert.SerializeObject(behandeling, JsonHelper.CamelCaseSettings);
        return await webClient.SendPostRequest(route, data);
    }

    public async Awaitable<IWebRequestReponse> UpdateItem(Guid behandelingID, Behandeling behandeling)
    {
        string route = "/behandeling/" + behandelingID;
        string data = JsonConvert.SerializeObject(behandeling, JsonHelper.CamelCaseSettings);
        return await webClient.SendPutRequest(route, data);
    }

    public async Awaitable<IWebRequestReponse> Delete(Guid behandelingID)
    {
        string route = "/behandeling/" + behandelingID;
        return await webClient.SendDeleteRequest(route);
    }
}
